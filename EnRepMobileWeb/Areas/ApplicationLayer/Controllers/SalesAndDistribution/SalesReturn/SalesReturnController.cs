using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesReturn;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesReturn;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.ComponentModel;
using EnRepMobileWeb.MODELS.Common;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using ZXing;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.SalesReturn
{
    public class SalesReturnController : Controller
    {

        string CompID, language, BrchID, title, UserID, FromDate = String.Empty;
        string DocumentMenuId = "105103142";
        List<SRList> _SalesReturnList;
        DataTable dt;

        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SalesReturn_Model _SalesReturn_Model;
        CommonController cmn = new CommonController();

        SalesReturn_ISERVICES _SalesReturn_ISERVICES;
        public SalesReturnController(Common_IServices _Common_IServices, SalesReturn_ISERVICES _SalesReturn_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._SalesReturn_ISERVICES = _SalesReturn_ISERVICES;
        }
        // GET: ApplicationLayer/SalesReturn
        public ActionResult SalesReturn(SalesReturnList_Model _SalesReturnList_Model)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                CommonPageDetails();

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");
                //SalesReturnList_Model _SalesReturnList_Model = new SalesReturnList_Model();
                GetAutoCompleteCustomerNameList(_SalesReturnList_Model);
                GetStatusList(_SalesReturnList_Model);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _SalesReturnList_Model.cust_id = a[0].Trim();
                    _SalesReturnList_Model.SRFromDate = a[1].Trim();
                    _SalesReturnList_Model.SRToDate = a[2].Trim();
                    _SalesReturnList_Model.Status = a[3].Trim();
                    if (_SalesReturnList_Model.Status == "0")
                    {
                        _SalesReturnList_Model.Status = null;
                    }
                    _SalesReturnList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                }
                // _SalesReturnList_Model.SalesReturnList = GetSalesReturnListAll(_SalesReturnList_Model);
                if (_SalesReturnList_Model.SRFromDate != null)
                {
                    _SalesReturnList_Model.FromDate = _SalesReturnList_Model.SRFromDate;
                }
                else
                {
                    _SalesReturnList_Model.FromDate = startDate;
                    _SalesReturnList_Model.SRFromDate = startDate;
                    _SalesReturnList_Model.SRToDate = CurrentDate;
                }
                //_SalesReturnList_Model.FromDate = startDate;
                GetAllData(_SalesReturnList_Model);
                // ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                ViewBag.DocumentMenuId = DocumentMenuId;
                _SalesReturnList_Model.SRSearch = "0";
                // ViewBag.VBRoleList = GetRoleList();               
                //  ViewBag.MenuPageName = getDocumentName();
                _SalesReturnList_Model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesReturn/SalesReturnList.cshtml", _SalesReturnList_Model);

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
        private void GetAllData(SalesReturnList_Model _SalesReturnList_Model)
        {
            string wfstatus = string.Empty;
            string CustName = string.Empty;
            string CustType = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_SalesReturnList_Model.Customer_Name))
                {
                    CustName = "0";
                }
                else
                {
                    CustName = _SalesReturnList_Model.Customer_Name;
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                CustType = null;

                if (_SalesReturnList_Model.WF_status != null)
                {
                    wfstatus = _SalesReturnList_Model.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet GetAllData = _SalesReturn_ISERVICES.GetAllData(Comp_ID, CustName, BrchID, CustType
             , _SalesReturnList_Model.cust_id, _SalesReturnList_Model.SRFromDate, _SalesReturnList_Model.SRToDate, _SalesReturnList_Model.Status,
             wfstatus, UserID, DocumentMenuId);


                List<CustomerNameList> _CustomerNameList1 = new List<CustomerNameList>();
                foreach (DataRow dr in GetAllData.Tables[0].Rows)
                {
                    CustomerNameList _CustomerName = new CustomerNameList();
                    _CustomerName.cust_id = dr["cust_id"].ToString();
                    _CustomerName.cust_name = dr["cust_name"].ToString();
                    _CustomerNameList1.Add(_CustomerName);
                }
                _CustomerNameList1.Insert(0, new CustomerNameList() { cust_id = "0", cust_name = "All" });
                _SalesReturnList_Model.CustomerNameList = _CustomerNameList1;

                SetAllData(_SalesReturnList_Model, GetAllData);
                _SalesReturnList_Model.SalesReturnList = GetSalesReturnListAll(_SalesReturnList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetAllData(SalesReturnList_Model _SalesReturnList_Model, DataSet dt)
        {
            List<SRList> _SalesReturnList = new List<SRList>();
            if (dt.Tables[2].Rows.Count > 0)
            {
                //FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
            }
            if (dt.Tables[1].Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    SRList _SRList = new SRList();
                    _SRList.SRTNumber = dr["srt_no"].ToString();
                    _SRList.SRTDate = dr["srt_dt"].ToString();
                    _SRList.hdSRTDate = dr["srt_date"].ToString();
                    _SRList.list_src_type = dr["src_type"].ToString();
                    _SRList.inv_no = dr["inv_no"].ToString();
                    _SRList.CustomerName = dr["cust_name"].ToString();
                    _SRList.SRT_value = dr["srt_value"].ToString();
                    _SRList.SRTStatus = dr["srt_status"].ToString();
                    _SRList.CreatedON = dr["created_on"].ToString();
                    _SRList.ApprovedOn = dr["app_dt"].ToString();
                    _SRList.ModifiedOn = dr["mod_dt"].ToString();
                    _SRList.create_by = dr["create_by"].ToString();
                    _SRList.app_by = dr["app_by"].ToString();
                    _SRList.mod_by = dr["mod_by"].ToString();

                    _SalesReturnList.Add(_SRList);
                }
            }
            _SalesReturnList_Model.SalesReturnList = _SalesReturnList;
        }
        public ActionResult GetSalesReturnList(string docid, string status)
        {

            //Session["WF_status"] = status;
            SalesReturnList_Model _SalesReturnList_Model = new SalesReturnList_Model();
            _SalesReturnList_Model.WF_status = status;
            return RedirectToAction("SalesReturn", _SalesReturnList_Model);
        }
        public ActionResult AddSalesReturnDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            SalesReturn_Model _SalesReturn_Model = new SalesReturn_Model();
            _SalesReturn_Model.Message = "New";
            _SalesReturn_Model.Command = "Add";
            _SalesReturn_Model.AppStatus = "D";
            _SalesReturn_Model.TransType = "Save";
            _SalesReturn_Model.BtnName = "BtnAddNew";
            TempData["ModelData"] = _SalesReturn_Model;
            UrlModel _urlModel = new UrlModel();
            _urlModel.cmd = "Add";
            _urlModel.Trp = "Save";
            _urlModel.BtnName = "BtnAddNew";
            ViewBag.MenuPageName = getDocumentName();
            /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("SalesReturn");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("SalesReturnDetail", "SalesReturn", _urlModel);

        }
        public ActionResult SalesReturnDetail(UrlModel _urlModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                /*Add by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.SRD) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                CommonPageDetails();

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;
                var _SalesReturn_Models = TempData["ModelData"] as SalesReturn_Model;
                if (_SalesReturn_Models != null)
                {
                    //ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    //SalesReturn_Model _SalesReturn_Models = new SalesReturn_Model();

                    _SalesReturn_Models.srt_dt = DateTime.Now;
                    //ViewBag.MenuPageName = getDocumentName();

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.inv_no = "---Select---";
                    _DocumentNumber.inv_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _SalesReturn_Models.DocumentNumberList = _DocumentNumberList;

                    /*------Start Transport Detail Add by Hina sharma on 29-05-2025--------*/
                    _SalesReturn_Models.TransList = GetTransporterList(CompID);
                    //ViewBag.MenuPageName = getDocumentName();
                    ViewBag.TransDetails = GetTransDetails(CompID, "0", "0", "0");
                    /*------End Transport Detail Add by Hina sharma on 29-05-2025--------*/

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SalesReturn_Models.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_SalesReturn_Models.TransType == "Update" || _SalesReturn_Models.TransType == "Edit")
                    {

                        //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        BrchID = Session["BranchId"].ToString();
                        //string Srt_no = Session["SalesReturnNo"].ToString();
                        string Srt_no = _SalesReturn_Models.SalesReturnNo;
                        string Srt_dt = _SalesReturn_Models.SalesReturnDate;
                        //string Srt_dt = Session["SalesReturnDate"].ToString();
                        DataSet ds = _SalesReturn_ISERVICES.GetSalesReturnDetail(Srt_no, Srt_dt, CompID, BrchID, UserID, DocumentMenuId);
                        _SalesReturn_Models.srt_no = ds.Tables[0].Rows[0]["srt_no"].ToString();
                        _SalesReturn_Models.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                        var src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        _SalesReturn_Models.srt_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["srt_dt"].ToString());
                        if (src_type != "A")
                        {
                           //_SalesReturn_Models.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["inv_dt"].ToString());
                            _SalesReturn_Models.src_doc_date = ds.Tables[0].Rows[0]["billinv_dt"].ToString();
                        }
                        _SalesReturn_Models.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _SalesReturn_Models.inv_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["inv_value"]).ToString(ValDigit);
                        _SalesReturn_Models.srt_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["srt_value"]).ToString(ValDigit);
                        _SalesReturn_Models.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _SalesReturn_Models.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _SalesReturn_Models.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _SalesReturn_Models.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _SalesReturn_Models.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _SalesReturn_Models.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _SalesReturn_Models.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _SalesReturn_Models.srt_status = ds.Tables[0].Rows[0]["app_status"].ToString();

                        _SalesReturn_Models.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                        _SalesReturn_Models.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _SalesReturn_Models.curr = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _SalesReturn_Models.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _SalesReturn_Models.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _SalesReturn_Models.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        _SalesReturn_Models.Customer_Reference = ds.Tables[0].Rows[0]["cust_ref"].ToString();
                        _SalesReturn_Models.Payment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                        _SalesReturn_Models.Delivery_term = ds.Tables[0].Rows[0]["deliv_term"].ToString();
                        _SalesReturn_Models.PlaceOfSupply = ds.Tables[0].Rows[0]["place_of_supp"].ToString();
                        _SalesReturn_Models.SlsRemarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        _SalesReturn_Models.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();

                        /*----------start Transport Details add by Hina sharma on 29-05-2025-------------*/
                        _SalesReturn_Models.GR_No = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        _SalesReturn_Models.GR_Dt = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        _SalesReturn_Models.HdnGRDate = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        _SalesReturn_Models.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                        _SalesReturn_Models.Veh_Number = ds.Tables[0].Rows[0]["veh_number"].ToString();
                        _SalesReturn_Models.Driver_Name = ds.Tables[0].Rows[0]["driver_name"].ToString();
                        _SalesReturn_Models.Mob_No = ds.Tables[0].Rows[0]["mob_no"].ToString();
                        _SalesReturn_Models.Tot_Tonnage = ds.Tables[0].Rows[0]["tot_tonnage"].ToString();
                        _SalesReturn_Models.No_Of_Packages = ds.Tables[0].Rows[0]["no_of_pkgs"].ToString();
                        _SalesReturn_Models.OcAmt = ds.Tables[0].Rows[0]["oc_amt"].ToString();

                        _SalesReturn_Models.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        //_SalesReturn_Models.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                        //_SalesReturn_Models.HdnTrnasportName = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                        GetAutoCompleteCustomerName(_SalesReturn_Models, _SalesReturn_Models.Src_Type);
                        //List<TransListModel> transporterList = new List<TransListModel>();
                        //transporterList.Add(new TransListModel { TransId = _SalesReturn_Models.Transpt_NameID, TransName = _SalesReturn_Models.HdnTrnasportName });
                        //_SalesReturn_Models.TransList = transporterList;
                        ///*----------End Transport Details add by Hina sharma on 29-05-2025-------------*/
                        ViewBag.Approve_id = approval_id;
                        if (roundoff_status == "Y")
                        {
                            _SalesReturn_Models.RoundOffFlag = true;
                        }
                        else
                        {
                            _SalesReturn_Models.RoundOffFlag = false;
                        }

                        _SalesReturn_Models.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _SalesReturn_Models.DocumentStatus = Statuscode;
                        _SalesReturn_Models.VouType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                        _SalesReturn_Models.VouNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                        _SalesReturn_Models.VouDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();
                        if (src_type == "C")
                        {
                            if (Statuscode == "A" || Statuscode == "C")
                            {
                                _SalesReturn_Models.GLVoucherType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                            }
                            _SalesReturn_Models.GLVoucherNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                            _SalesReturn_Models.GLVoucherDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();
                            _SalesReturn_Models.CustomVouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            ViewBag.GLGroup = ds.Tables[6];
                        }
                        else
                        {
                            ViewBag.GLAccount = ds.Tables[6];
                        }
                        ViewBag.ItemTaxDetails = ds.Tables[11];
                        ViewBag.ItemTaxDetailsList = ds.Tables[12];
                        ViewBag.AttechmentDetails = ds.Tables[13];
                        ViewBag.OtherChargeDetails = ds.Tables[14];
                        ViewBag.OCTaxDetails = ds.Tables[15];

                        if (_SalesReturn_Models.Status == "C")
                        {
                            _SalesReturn_Models.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _SalesReturn_Models.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _SalesReturn_Models.BtnName = "Refresh";
                        }
                        else
                        {
                            _SalesReturn_Models.CancelFlag = false;
                        }

                        _SalesReturn_Models.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _SalesReturn_Models.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SalesReturn_Models.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SalesReturn_Models.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SalesReturn_Models.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SalesReturn_Models.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Models.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SalesReturn_Models.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Models.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Models.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Models.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SalesReturn_Models.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _SalesReturn_Models.DocumentNumberList = GetSalesReturnSourceDocument(_SalesReturn_Models);
                        _SalesReturn_Models.src_doc_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        DocumentNumber objDocumentNumber = new DocumentNumber();
                        objDocumentNumber.inv_no = _SalesReturn_Models.src_doc_no;
                        objDocumentNumber.inv_dt = _SalesReturn_Models.src_doc_no;
                        if (!_SalesReturn_Models.DocumentNumberList.Contains(objDocumentNumber))
                        {
                            _SalesReturn_Models.DocumentNumberList.Add(objDocumentNumber);
                        }
                        if (src_type == "A")
                        {
                            _SalesReturn_Models.InvBillNumber = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _SalesReturn_Models.InvBillDate = ds.Tables[0].Rows[0]["billinv_dt"].ToString();
                        }
                        getWarehouse(_SalesReturn_Models);
                        //ViewBag.MenuPageName = getDocumentName();
                        _SalesReturn_Models.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        //ViewBag.ItemLotBatchSerialDetails = ds.Tables[2];

                        ViewBag.VoucherDetails = ds.Tables[6];

                        ViewBag.hdGLDetails = ds.Tables[7];
                        ViewBag.VoucherTotal = ds.Tables[8];
                        ViewBag.SubItemDetails = ds.Tables[9];
                        ViewBag.CostCenterData = ds.Tables[10];
                        ViewBag.SRItemBatchSerialDetail = ds.Tables[2];
                        _SalesReturn_Models.SRItemBatchSerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesReturn/SalesReturn.cshtml", _SalesReturn_Models);
                    }
                    else
                    {
                        GetAutoCompleteCustomerName(_SalesReturn_Models, _SalesReturn_Models.Src_Type);
                        _SalesReturn_Models.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _SalesReturn_Models.DocumentStatus = "New";
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesReturn/SalesReturn.cshtml", _SalesReturn_Models);
                    }
                }
                else
                {/*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
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
                    //ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    SalesReturn_Model _SalesReturn_Model = new SalesReturn_Model();
                    _SalesReturn_Model.UserId = UserID;
                    if (_urlModel != null)
                    {
                        _SalesReturn_Model.Command = _urlModel.cmd;
                        _SalesReturn_Model.SalesReturnNo = _urlModel.SRN;
                        _SalesReturn_Model.SalesReturnDate = _urlModel.SRD;
                        _SalesReturn_Model.TransType = _urlModel.Trp;
                        _SalesReturn_Model.Command = _urlModel.cmd;
                        _SalesReturn_Model.BtnName = _urlModel.BtnName;
                        _SalesReturn_Model.WF_status1 = _urlModel.WFS1;
                        _SalesReturn_Model.DocumentStatus = _urlModel.DMS;
                    }

                    _SalesReturn_Model.srt_dt = DateTime.Now;
                    //ViewBag.MenuPageName = getDocumentName();

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.inv_no = "---Select---";
                    _DocumentNumber.inv_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _SalesReturn_Model.DocumentNumberList = _DocumentNumberList;

                    /*------Start Transport Detail Add by Hina sharma on 29-05-2025--------*/
                    _SalesReturn_Model.TransList = GetTransporterList(CompID);
                    //ViewBag.MenuPageName = getDocumentName();
                    ViewBag.TransDetails = GetTransDetails(CompID, "0", "0", "0");
                    /*------End Transport Detail Add by Hina sharma on 29-05-2025--------*/


                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SalesReturn_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_SalesReturn_Model.TransType == "Update" || _SalesReturn_Model.TransType == "Edit")
                    {

                        //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        BrchID = Session["BranchId"].ToString();
                        //string Srt_no = Session["SalesReturnNo"].ToString();
                        string Srt_no = _SalesReturn_Model.SalesReturnNo;
                        string Srt_dt = _SalesReturn_Model.SalesReturnDate;
                        //string Srt_dt = Session["SalesReturnDate"].ToString();
                        DataSet ds = _SalesReturn_ISERVICES.GetSalesReturnDetail(Srt_no, Srt_dt, CompID, BrchID, UserID, DocumentMenuId);
                        _SalesReturn_Model.srt_no = ds.Tables[0].Rows[0]["srt_no"].ToString();
                        _SalesReturn_Model.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                        _SalesReturn_Model.srt_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["srt_dt"].ToString());
                        var src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        if (src_type != "A")
                        {
                            //_SalesReturn_Model.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["inv_dt"].ToString());
                            _SalesReturn_Model.src_doc_date = ds.Tables[0].Rows[0]["billinv_dt"].ToString();
                        }
                        _SalesReturn_Model.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _SalesReturn_Model.inv_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["inv_value"]).ToString(ValDigit);
                        _SalesReturn_Model.srt_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["srt_value"]).ToString(ValDigit);
                        _SalesReturn_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _SalesReturn_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _SalesReturn_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _SalesReturn_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _SalesReturn_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _SalesReturn_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _SalesReturn_Model.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _SalesReturn_Model.srt_status = ds.Tables[0].Rows[0]["app_status"].ToString();

                        _SalesReturn_Model.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                        _SalesReturn_Model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _SalesReturn_Model.curr = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _SalesReturn_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _SalesReturn_Model.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _SalesReturn_Model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        _SalesReturn_Model.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _SalesReturn_Model.DocumentStatus = Statuscode;

                        GetAutoCompleteCustomerName(_SalesReturn_Model, _SalesReturn_Model.Src_Type);

                        _SalesReturn_Model.VouType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                        _SalesReturn_Model.VouNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                        _SalesReturn_Model.VouDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();
                        if (src_type == "C")
                        {
                            if (Statuscode == "A" || Statuscode == "C")
                            {
                                _SalesReturn_Model.GLVoucherType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                            }
                            _SalesReturn_Model.GLVoucherNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                            _SalesReturn_Model.GLVoucherDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();
                            _SalesReturn_Model.CustomVouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            ViewBag.GLGroup = ds.Tables[6];
                        }
                        else
                        {
                            ViewBag.GLAccount = ds.Tables[6];
                        }

                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _SalesReturn_Model.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();

                        _SalesReturn_Model.Customer_Reference = ds.Tables[0].Rows[0]["cust_ref"].ToString();
                        _SalesReturn_Model.Payment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                        _SalesReturn_Model.Delivery_term = ds.Tables[0].Rows[0]["deliv_term"].ToString();
                        _SalesReturn_Model.PlaceOfSupply = ds.Tables[0].Rows[0]["place_of_supp"].ToString();
                        _SalesReturn_Model.SlsRemarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        _SalesReturn_Model.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();

                        /*----------start Transport Details add by Hina sharma on 29-05-2025-------------*/
                        _SalesReturn_Model.GR_No = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        _SalesReturn_Model.GR_Dt = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        _SalesReturn_Model.HdnGRDate = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        _SalesReturn_Model.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                        _SalesReturn_Model.Veh_Number = ds.Tables[0].Rows[0]["veh_number"].ToString();
                        _SalesReturn_Model.Driver_Name = ds.Tables[0].Rows[0]["driver_name"].ToString();
                        _SalesReturn_Model.Mob_No = ds.Tables[0].Rows[0]["mob_no"].ToString();
                        _SalesReturn_Model.Tot_Tonnage = ds.Tables[0].Rows[0]["tot_tonnage"].ToString();
                        _SalesReturn_Model.No_Of_Packages = ds.Tables[0].Rows[0]["no_of_pkgs"].ToString();
                        _SalesReturn_Model.OcAmt = ds.Tables[0].Rows[0]["oc_amt"].ToString();
                        //_SalesReturn_Model.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                        //_SalesReturn_Model.HdnTrnasportName = ds.Tables[0].Rows[0]["trpt_name"].ToString();

                        //List<TransListModel> transporterList = new List<TransListModel>();
                        //transporterList.Add(new TransListModel { TransId = _SalesReturn_Model.Transpt_NameID, TransName = _SalesReturn_Model.HdnTrnasportName });
                        //_SalesReturn_Model.TransList = transporterList;
                        /////*----------End Transport Details add by Hina sharma on 29-05-2025-------------*/


                        if (roundoff_status == "Y")
                        {
                            _SalesReturn_Model.RoundOffFlag = true;
                        }
                        else
                        {
                            _SalesReturn_Model.RoundOffFlag = false;
                        }
                        ViewBag.ItemTaxDetails = ds.Tables[11];
                        ViewBag.ItemTaxDetailsList = ds.Tables[12];
                        ViewBag.AttechmentDetails = ds.Tables[13];
                        ViewBag.OtherChargeDetails = ds.Tables[14];
                        ViewBag.OCTaxDetails = ds.Tables[15];
                        if (_SalesReturn_Model.Status == "C")
                        {
                            _SalesReturn_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _SalesReturn_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _SalesReturn_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _SalesReturn_Model.CancelFlag = false;
                        }

                        _SalesReturn_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _SalesReturn_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SalesReturn_Model.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SalesReturn_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SalesReturn_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SalesReturn_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SalesReturn_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SalesReturn_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SalesReturn_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _SalesReturn_Model.DocumentNumberList = GetSalesReturnSourceDocument(_SalesReturn_Model);
                        _SalesReturn_Model.src_doc_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        if (src_type == "A")
                        {
                            _SalesReturn_Model.InvBillNumber = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _SalesReturn_Model.InvBillDate = ds.Tables[0].Rows[0]["billinv_dt"].ToString();
                        }

                        DocumentNumber objDocumentNumber = new DocumentNumber();
                        objDocumentNumber.inv_no = _SalesReturn_Model.src_doc_no;
                        objDocumentNumber.inv_dt = _SalesReturn_Model.src_doc_no;
                        if (!_SalesReturn_Model.DocumentNumberList.Contains(objDocumentNumber))
                        {
                            _SalesReturn_Model.DocumentNumberList.Add(objDocumentNumber);
                        }

                        getWarehouse(_SalesReturn_Model);
                        //ViewBag.MenuPageName = getDocumentName();
                        _SalesReturn_Model.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        //ViewBag.ItemLotBatchSerialDetails = ds.Tables[2];

                        ViewBag.VoucherDetails = ds.Tables[6];
                        //ViewBag.GLAccount = ds.Tables[6];
                        ViewBag.hdGLDetails = ds.Tables[7];
                        ViewBag.VoucherTotal = ds.Tables[8];
                        ViewBag.SubItemDetails = ds.Tables[9];
                        ViewBag.CostCenterData = ds.Tables[10];
                        ViewBag.SRItemBatchSerialDetail = ds.Tables[2];
                        _SalesReturn_Model.SRItemBatchSerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesReturn/SalesReturn.cshtml", _SalesReturn_Model);
                    }
                    else
                    {
                        GetAutoCompleteCustomerName(_SalesReturn_Model, _SalesReturn_Model.Src_Type);
                        _SalesReturn_Model.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _SalesReturn_Model.DocumentStatus = "New";
                        //_SalesReturn_Model.BtnName = "BtnAddNew";
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesReturn/SalesReturn.cshtml", _SalesReturn_Model);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [NonAction]
        private void getWarehouse(SalesReturn_Model _SalesReturn_Model)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                List<Warehouse> _WarehouseList = new List<Warehouse>();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet result = _Common_IServices.GetWarehouseList(Comp_ID, Br_ID);
                foreach (DataRow dr in result.Tables[0].Rows)
                {
                    Warehouse _Warehouse = new Warehouse();
                    _Warehouse.wh_id = dr["wh_id"].ToString();
                    _Warehouse.wh_name = dr["wh_name"].ToString();
                    _WarehouseList.Add(_Warehouse);
                }
                Warehouse _oWarehouse = new Warehouse();
                _oWarehouse.wh_id = "0";
                _oWarehouse.wh_name = "---Select---";
                _WarehouseList.Insert(0, _oWarehouse);
                _SalesReturn_Model.WarehouseList = _WarehouseList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }
        }
        [HttpPost]
        public JsonResult GetWarehouseList1()
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
                DataSet result = _Common_IServices.GetWarehouseList(Comp_ID, Br_ID);
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
        public ActionResult EditSalesReturn(string SRTNo, string Srt_Date, string ListFilterData, string WF_status)
        {/*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["SalesReturnNo"] = SRTNo;
            //Session["SalesReturnDate"] = Srt_Date;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            SalesReturn_Model dblclick = new SalesReturn_Model();
            dblclick.BtnName = "BtnToDetailPage";
            dblclick.TransType = "Update";
            dblclick.Command = "Update";
            dblclick.Message = "New"; ;
            dblclick.SalesReturnNo = SRTNo;
            dblclick.SalesReturnDate = Srt_Date;
            dblclick.AppStatus = "D";
            dblclick.UserId = UserID;
            UrlModel _urlModel = new UrlModel();
            _urlModel.cmd = "Update";
            _urlModel.Trp = "Update";
            _urlModel.SRN = dblclick.SalesReturnNo;
            _urlModel.SRD = dblclick.SalesReturnDate;
            if (WF_status != null && WF_status != "")
            {
                dblclick.WF_status1 = WF_status;
                _urlModel.WFS1 = WF_status;
            }
            _urlModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = dblclick;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("SalesReturnDetail", "SalesReturn", _urlModel);
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
        private DataTable GetRoleList()
        {
            try
            {
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
        public ActionResult GetAutoCompleteCustomerName(SalesReturn_Model _SalesReturn_Model, string src_type)
        {
            string CustName = string.Empty;
            string CustType = string.Empty;

            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_SalesReturn_Model.CustomerName))
                {
                    CustName = "0";
                }
                else
                {
                    CustName = _SalesReturn_Model.CustomerName;
                }
                if (Session["BranchId"] == null)
                {
                    RedirectToAction("Index", "Home");
                }
                else
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (src_type != "C")
                    CustType = "D";
                else
                    CustType = "E";
                //CustType = "D";
                //CustType = null;
                CustList = _SalesReturn_ISERVICES.GetCustomerList(Comp_ID, CustName, BrchID, CustType, src_type);
                //CustList = _SalesReturn_ISERVICES.GetCustomerList(Comp_ID, CustName, BrchID);

                List<CustomerName> _CustomerNameList = new List<CustomerName>();
                foreach (var dr in CustList)
                {
                    CustomerName _CustomerName = new CustomerName();
                    _CustomerName.cust_id = dr.Key;
                    _CustomerName.cust_name = dr.Value;
                    _CustomerNameList.Add(_CustomerName);
                }
                _SalesReturn_Model.CustomerNameList = _CustomerNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalesReturnSourceDocumentNoList(SalesReturn_Model _SalesReturn_Model)
        {
            try
            {
                JsonResult DataRows = null;
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Cust_ID = string.Empty;
                string Src_Type = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                Cust_ID = _SalesReturn_Model.CustomerID;
                Src_Type = _SalesReturn_Model.Src_Type;
                DocumentNumber = _SalesReturn_Model.DocumentNo;
                string BrchID = Session["BranchId"].ToString();
                DocumentNumberList = _SalesReturn_ISERVICES.GetSalesInvoiceNo(CompID, BrchID, Cust_ID, DocumentNumber, Src_Type);
                //foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                //{
                //    DocumentNumber _DocumentNumber = new DocumentNumber();
                //    _DocumentNumber.inv_no = dr["inv_no"].ToString();
                //    _DocumentNumber.inv_dt = dr["inv_dt"].ToString();
                //    _DocumentNumberList.Add(_DocumentNumber);
                //}
                //DataRows = Json(JsonConvert.SerializeObject(DocumentNumberList), JsonRequestBehavior.AllowGet);
                DataRows = Json(JsonConvert.SerializeObject(DocumentNumberList));
                return DataRows;
                //return DataRows;
                // return Json(_DocumentNumberList.Select(c => new { Name = c.inv_no, ID = c.inv_dt }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;// View("~/Views/Shared/Error.cshtml");
            }


        }
        public List<DocumentNumber> GetSalesReturnSourceDocument(SalesReturn_Model _SalesReturn_Model)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Cust_ID = string.Empty;
                string Src_Type = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                Cust_ID = _SalesReturn_Model.CustomerID;
                Cust_ID = _SalesReturn_Model.cust_id;
                Src_Type = _SalesReturn_Model.Src_Type;
                DocumentNumber = _SalesReturn_Model.DocumentNo;
                string BrchID = Session["BranchId"].ToString();
                DocumentNumberList = _SalesReturn_ISERVICES.GetSalesInvoiceNo(CompID, BrchID, Cust_ID, DocumentNumber, Src_Type);
                foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                {
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.inv_no = dr["inv_no"].ToString();
                    _DocumentNumber.inv_dt = dr["inv_no"].ToString();
                    _DocumentNumberList.Add(_DocumentNumber);
                }


                return _DocumentNumberList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public ActionResult GetSIItemDetail(string SourDocumentNo, string src_type)
        {
            try
            {
                JsonResult DataRows = null;
                _SalesReturn_Model = new SalesReturn_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _SalesReturn_ISERVICES.GetSIItemDetail(CompID, BrchID, SourDocumentNo, src_type);
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
        public ActionResult GetShipmentItemDetail(string ItemID, string ShipNumber, string SrcDocNumber, string RT_Status, string src_type)
        {
            try
            {
                JsonResult DataRows = null;
                _SalesReturn_Model = new SalesReturn_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _SalesReturn_ISERVICES.GetShipmentItemDetail(CompID, BrchID, ItemID, ShipNumber, SrcDocNumber, RT_Status, src_type);
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
        public ActionResult GetSalesInvoiceItemDetail(string ItemID, string InvoiceNo, string ShipNumber, string src_type)
        {
            try
            {
                JsonResult DataRows = null;
                _SalesReturn_Model = new SalesReturn_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _SalesReturn_ISERVICES.GetSalesInvoiceItemDetail(CompID, BrchID, ItemID, InvoiceNo, ShipNumber, src_type);

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
        public ActionResult GetTaxAmountDetail(string ItmCode, string InvoiceNo, string ShipNumber, string ReturnQuantity, string src_type)/*Add by Hina sharma on 16-12-2024*/
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();

                DataSet dt = _SalesReturn_ISERVICES.GetTaxAmountDetail(CompID, BrchID, ItmCode, InvoiceNo, ShipNumber, ReturnQuantity, src_type);
                ViewBag.TaxAmountDetail = dt.Tables[0];
                ViewBag.TaxAmountDetailTotal = dt.Tables[1];

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSRTaxAmountDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SalesReturnSave(SalesReturn_Model _SalesReturn_Model, string srt_no, string command)
        {
            try
            {
                /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_SalesReturn_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        SalesReturn_Model _SalesReturn_AddnewModel = new SalesReturn_Model();
                        _SalesReturn_AddnewModel.Message = "New";
                        _SalesReturn_AddnewModel.Command = "Add";
                        _SalesReturn_AddnewModel.AppStatus = "D";
                        _SalesReturn_AddnewModel.TransType = "Save";
                        _SalesReturn_AddnewModel.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _SalesReturn_AddnewModel;
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.cmd = "Add";
                        _urlModel.Trp = "Save";
                        _urlModel.BtnName = "BtnAddNew";
                        //Session["Message"] = "New";
                        //Session["Command"] = "Add";
                        //Session["AppStatus"] = 'D';
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_SalesReturn_Model.srt_no))
                                return RedirectToAction("EditSalesReturn", new { SRTNo = _SalesReturn_Model.srt_no, Srt_Date = _SalesReturn_Model.srt_dt, ListFilterData = _SalesReturn_Model.ListFilterData1, WF_status = _SalesReturn_Model.WFStatus });
                            else
                                _SalesReturn_AddnewModel.Command = "Refresh";
                            _SalesReturn_AddnewModel.TransType = "Refresh";
                            _SalesReturn_AddnewModel.BtnName = "Refresh";
                            _SalesReturn_AddnewModel.DocumentStatus = null;
                            TempData["ModelData"] = _SalesReturn_AddnewModel;
                            return RedirectToAction("SalesReturnDetail", "SalesReturn", _SalesReturn_AddnewModel);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("SalesReturnDetail", "SalesReturn", _urlModel);

                    case "Edit":
                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSalesReturn", new { SRTNo = _SalesReturn_Model.srt_no, Srt_Date = _SalesReturn_Model.srt_dt, ListFilterData = _SalesReturn_Model.ListFilterData1, WF_status = _SalesReturn_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SrtDate = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SrtDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSalesReturn", new { SRTNo = _SalesReturn_Model.srt_no, Srt_Date = _SalesReturn_Model.srt_dt, ListFilterData = _SalesReturn_Model.ListFilterData1, WF_status = _SalesReturn_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["Message"] = null;
                        //Session["BtnName"] = "BtnEdit";
                        //Session["SalesReturnNo"] = _SalesReturn_Model.srt_no;
                        //Session["SalesReturnDate"] = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        _SalesReturn_Model.Message = "New";
                        _SalesReturn_Model.Command = command;
                        _SalesReturn_Model.AppStatus = "D";
                        _SalesReturn_Model.TransType = "Update";
                        _SalesReturn_Model.BtnName = "BtnEdit";
                        _SalesReturn_Model.SalesReturnNo = _SalesReturn_Model.srt_no;
                        _SalesReturn_Model.SalesReturnDate = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        TempData["ModelData"] = _SalesReturn_Model;
                        UrlModel _urlEditModel = new UrlModel();
                        _urlEditModel.cmd = command;
                        _urlEditModel.Trp = "Update";
                        _urlEditModel.BtnName = "BtnEdit";
                        _urlEditModel.SRN = _SalesReturn_Model.srt_no;
                        _urlEditModel.SRD = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        TempData["ListFilterData"] = _SalesReturn_Model.ListFilterData1;
                        return RedirectToAction("SalesReturnDetail", _urlEditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        _SalesReturn_Model.Command = command;
                        srt_no = _SalesReturn_Model.srt_no;
                        SalesReturnDelete(_SalesReturn_Model, command);
                        SalesReturn_Model _SalesReturn_DeleteModel = new SalesReturn_Model();
                        _SalesReturn_DeleteModel.Message = "Deleted";
                        _SalesReturn_DeleteModel.Command = "Refresh";
                        _SalesReturn_DeleteModel.AppStatus = "D";
                        _SalesReturn_DeleteModel.TransType = "Refresh";
                        _SalesReturn_DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = _SalesReturn_DeleteModel;
                        UrlModel _urlDeleteModel = new UrlModel();
                        _urlDeleteModel.cmd = "Refresh";
                        _urlDeleteModel.Trp = "Refresh";
                        _urlDeleteModel.BtnName = "BtnDelete";
                        TempData["ListFilterData"] = _SalesReturn_Model.ListFilterData1;
                        return RedirectToAction("SalesReturnDetail", _urlDeleteModel);

                    case "Save":
                        //Session["Command"] = command;
                        _SalesReturn_Model.Command = command;
                        SaveSalesReturn(_SalesReturn_Model);
                        if (_SalesReturn_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_SalesReturn_Model.Message == "DocModify")
                        {
                            //_SalesReturn_Model.DocumentId=DocumentMenuId;
                            CommonPageDetails();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";

                            //var other = new CommonController(_Common_IServices);
                            //ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            //ViewBag.MenuPageName = getDocumentName();
                            //SalesReturn_Model _SalesReturn_Models = new SalesReturn_Model();

                            //List<CustomerNameList> _CustomerNameList = new List<CustomerNameList>();
                            //foreach (var dr in CustList)
                            //{
                            //    CustomerNameList _CustomerName = new CustomerNameList();
                            //    _CustomerName.cust_id = dr.Key;
                            //    _CustomerName.cust_name = dr.Value;
                            //    _CustomerNameList.Add(_CustomerName);
                            //}
                            //_SalesReturn_Model.CustomerNameList = _CustomerNameList;

                            List<CustomerName> _CustomerNameList = new List<CustomerName>();
                            CustomerName _CustomerName = new CustomerName();
                            _CustomerName.cust_name = _SalesReturn_Model.CustomerName;
                            _CustomerName.cust_id = _SalesReturn_Model.CustomerID;
                            _CustomerNameList.Add(_CustomerName);
                            _SalesReturn_Model.CustomerNameList = _CustomerNameList;



                            List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.inv_no = _SalesReturn_Model.src_doc_no;
                            _DocumentNumber.inv_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);
                            _SalesReturn_Model.DocumentNumberList = _DocumentNumberList;

                            _SalesReturn_Model.srt_dt = DateTime.Now;
                            _SalesReturn_Model.src_doc_no = _SalesReturn_Model.src_doc_no;
                            _SalesReturn_Model.src_doc_date = _SalesReturn_Model.src_doc_date;
                            _SalesReturn_Model.CustomerName = _SalesReturn_Model.CustomerName;


                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.VoucherDetails = ViewData["VouDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemdetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _SalesReturn_Model.BtnName = "Refresh";
                            _SalesReturn_Model.Command = "Refresh";
                            _SalesReturn_Model.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesReturn/SalesReturn.cshtml", _SalesReturn_Model);
                        }
                        else
                        {
                            TempData["ModelData"] = _SalesReturn_Model;
                            UrlModel _urlSaveModel = new UrlModel();
                            _urlSaveModel.cmd = _SalesReturn_Model.Command;
                            _urlSaveModel.Trp = _SalesReturn_Model.TransType;
                            _urlSaveModel.BtnName = _SalesReturn_Model.BtnName;
                            _urlSaveModel.SRN = _SalesReturn_Model.SalesReturnNo;
                            _urlSaveModel.SRD = _SalesReturn_Model.SalesReturnDate;
                            //Session["SalesReturnNo"] = Session["SalesReturnNo"].ToString();
                            //Session["SalesReturnDate"] = Session["SalesReturnDate"].ToString();
                            TempData["ListFilterData"] = _SalesReturn_Model.ListFilterData1;
                            return RedirectToAction("SalesReturnDetail", _urlSaveModel);
                        }
                    case "Forward":
                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSalesReturn", new { SRTNo = _SalesReturn_Model.srt_no, Srt_Date = _SalesReturn_Model.srt_dt, ListFilterData = _SalesReturn_Model.ListFilterData1, WF_status = _SalesReturn_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SrtDate1 = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SrtDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSalesReturn", new { SRTNo = _SalesReturn_Model.srt_no, Srt_Date = _SalesReturn_Model.srt_dt, ListFilterData = _SalesReturn_Model.ListFilterData1, WF_status = _SalesReturn_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSalesReturn", new { SRTNo = _SalesReturn_Model.srt_no, Srt_Date = _SalesReturn_Model.srt_dt, ListFilterData = _SalesReturn_Model.ListFilterData1, WF_status = _SalesReturn_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SrtDate2 = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SrtDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSalesReturn", new { SRTNo = _SalesReturn_Model.srt_no, Srt_Date = _SalesReturn_Model.srt_dt, ListFilterData = _SalesReturn_Model.ListFilterData1, WF_status = _SalesReturn_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _SalesReturn_Model.Command = command;
                        srt_no = _SalesReturn_Model.srt_no;
                        //Session["SalesReturnNo"] = srt_no;
                        //Session["SalesReturnDate"] = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        SalesReturnApprove(_SalesReturn_Model, srt_no, _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd"), "Approve", "", "", _SalesReturn_Model.CnNarr, "", "", "", "", _SalesReturn_Model.JVNurr);
                        //_SalesReturn_Model.SalesReturnNo = _SalesReturn_Model.SalesReturnNo;
                        //_SalesReturn_Model.SalesReturnDate = _SalesReturn_Model.SalesReturnDate;
                        //_SalesReturn_Model.Message = "Approved";
                        //_SalesReturn_Model.AppStatus = "D";
                        //_SalesReturn_Model.BtnName = "BtnEdit";
                        TempData["ModelData"] = _SalesReturn_Model;
                        UrlModel _urlapprove = new UrlModel();
                        _urlapprove.SRN = _SalesReturn_Model.SalesReturnNo;
                        _urlapprove.SRD = _SalesReturn_Model.SalesReturnDate;
                        _urlapprove.BtnName = "BtnEdit";
                        _urlapprove.Trp = _SalesReturn_Model.TransType;
                        TempData["ListFilterData"] = _SalesReturn_Model.ListFilterData1;
                        return RedirectToAction("SalesReturnDetail", _urlapprove);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = null;
                        SalesReturn_Model _SalesReturnRefresh_Model = new SalesReturn_Model();
                        _SalesReturnRefresh_Model.BtnName = "Refresh";
                        _SalesReturnRefresh_Model.Command = command;
                        _SalesReturnRefresh_Model.TransType = "Save";
                        UrlModel _urlREfresh = new UrlModel();
                        _urlREfresh.cmd = _SalesReturn_Model.Command;
                        _urlREfresh.Trp = _SalesReturn_Model.TransType;
                        _urlREfresh.BtnName = "Refresh";
                        TempData["ListFilterData"] = _SalesReturn_Model.ListFilterData1;
                        return RedirectToAction("SalesReturnDetail", _urlREfresh);

                    case "Print":
                        //  return new EmptyResult();
                        return GenratePdfFile(_SalesReturn_Model);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        SalesReturnList_Model _SalesReturnList_Model = new SalesReturnList_Model();
                        _SalesReturnList_Model.WF_status = _SalesReturn_Model.WF_status1;
                        TempData["ListFilterData"] = _SalesReturn_Model.ListFilterData1;
                        return RedirectToAction("SalesReturn", "SalesReturn", _SalesReturnList_Model);

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
        [HttpPost]
        public FileResult GenratePdfFile(SalesReturn_Model _SalesReturn_Model)
        {
            DataTable dt = new DataTable();
            dt = PrintFormatDataTable(_SalesReturn_Model);
            ViewBag.PrintOption = dt;
            //return File(GetPdfData(_model.DocumentMenuId, _model.inv_no, _model.inv_dt, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
            //if (_model.GstApplicable == "Y")
            //    return File(GetPdfDataOfGstInv(dt, _model.DocumentMenuId, _model.Sinv_no, _model.Sinv_dt, _model.NumberofCopy), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
            //else
            //    return File(GetPdfData(_model.DocumentMenuId, _model.Sinv_no, _model.Sinv_dt, _model.NumberofCopy), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");

            //return File(GetPdfData(_SalesReturn_Model.srt_no, _SalesReturn_Model.srt_dt, _SalesReturn_Model.Src_Type), "application/pdf", "SalesReturn.pdf");
            return File(GetPdfData(_SalesReturn_Model.srt_no, _SalesReturn_Model.srt_dt, _SalesReturn_Model.Src_Type), "application/pdf", ViewBag.Title.Replace(" ", "") + _SalesReturn_Model.PrintFormat + ".pdf");


        }

        public byte[] GetPdfData(string srt_no, DateTime srdt, string Src_Type)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string htmlcontent = "";
                string DelSchedule = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                Byte[] bytes = null;

                DataSet Details = _SalesReturn_ISERVICES.GetSalesOrderDeatilsForPrint(CompID, BrchID, srt_no, srdt.ToString("yyyy-MM-dd"), Src_Type);
                string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());//Added by Suraj Maurya on 19-07-2025
                ViewBag.PageName = "SR";
                ViewBag.Title = "Sales Return/Credit Note";
                ViewBag.Details = Details;
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add  by hina sharma on 04-04-2025*/
                ViewBag.Src_Type = Src_Type;
                ViewBag.InvoiceTo = "";
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.ApprovedBy = "Arvind Gupta";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["srt_status"].ToString().Trim();

                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesReturn/SalesReturnPrint.cshtml"));



                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

                    reader = new StringReader(DelSchedule);
                    pdfDoc.NewPage();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

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

                                var qrCode = Image.GetInstance(QR);
                                qrCode.SetAbsolutePosition(475, 710);
                                qrCode.ScaleAbsolute(100f, 95f);

                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (i == 1)
                                    {
                                        if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                                        {
                                            content.AddImage(qrCode);
                                        }
                                    }
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
        private DataTable PrintFormatDataTable(SalesReturn_Model _Model)
        {
            DataTable dt = new DataTable();
            //var commonCont = new CommonController(_Common_IServices);

            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            dt.Columns.Add("showInvHeading", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));


            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _Model.PrintFormat;
            dtr["ShowProdDesc"] = _Model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
            dtr["ShowSubItem"] = _Model.ShowSubItem;
            dtr["CustAliasName"] = _Model.CustomerAliasName;
            dtr["ShowTotalQty"] = _Model.ShowTotalQty;
            dtr["ShowWithoutSybbol"] = _Model.ShowWithoutSybbol;
            dtr["showInvHeading"] = _Model.showInvHeading;
            dtr["PrintRemarks"] = _Model.PrintRemarks;

            dt.Rows.Add(dtr);
            //Commented by Suraj on 27-05-2025 
            //Cmn_PrintOptions cmn_PrintOptions = new Cmn_PrintOptions//Added by Suraj on 08-10-2024
            //{
            //    PrintFormat = _Model.PrintFormat,
            //    ShowProdDesc = _Model.ShowProdDesc,
            //    ShowCustSpecProdDesc = _Model.ShowCustSpecProdDesc,
            //    ShowProdTechDesc = _Model.ShowProdTechDesc,
            //    ShowSubItem = _Model.ShowSubItem,
            //    CustomerAliasName = _Model.CustomerAliasName,
            //    ShowTotalQty = _Model.ShowTotalQty,
            //};
            //dt = commonCont.PrintOptionsDt(cmn_PrintOptions);
            return dt;
        }
        public ActionResult SaveSalesReturn(SalesReturn_Model _SalesReturn_Model)
        {
            try
            {
                if (_SalesReturn_Model.CancelFlag == false)
                {
                    var src_type = _SalesReturn_Model.Src_Type;
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }

                    DataTable SalesReturnHeader = new DataTable();
                    DataTable SalesReturnItemDetails = new DataTable();
                    DataTable SalesReturnLotBatchSerial = new DataTable();
                    DataTable SalesReturnVoudetail = new DataTable();
                    DataTable CRCostCenterDetails = new DataTable();
                    DataTable DtblVouGLDetail = new DataTable();
                    DataTable DtblOCDetail = new DataTable();
                    DataTable DtblOCTaxDetail = new DataTable();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MenuDocumentId", typeof(string));
                    dt.Columns.Add("comp_id", typeof(int));
                    dt.Columns.Add("br_id", typeof(int));
                    dt.Columns.Add("srt_no", typeof(string));
                    dt.Columns.Add("srt_dt", typeof(string));
                    dt.Columns.Add("inv_no", typeof(string));
                    dt.Columns.Add("inv_dt", typeof(string));
                    dt.Columns.Add("cust_id", typeof(int));
                    dt.Columns.Add("inv_value", typeof(string));
                    dt.Columns.Add("srt_value", typeof(string));
                    dt.Columns.Add("CreateBy", typeof(string));
                    dt.Columns.Add("srt_status", typeof(string));
                    dt.Columns.Add("UserMacaddress", typeof(string));
                    dt.Columns.Add("UserSystemName", typeof(string));
                    dt.Columns.Add("UserIP", typeof(string));
                    dt.Columns.Add("TransType", typeof(string));
                    dt.Columns.Add("roundoff", typeof(string));
                    dt.Columns.Add("pm_flag", typeof(string));
                    /*Add by Hina Sharma on 15-05-2025 other detail section*/
                    dt.Columns.Add("cust_ref", typeof(string));
                    //dt.Columns.Add("pay_term", typeof(string));
                    //dt.Columns.Add("deli_term", typeof(string));
                    dt.Columns.Add("placeofsupp", typeof(string));
                    dt.Columns.Add("remarks", typeof(string));
                    dt.Columns.Add("inv_heading", typeof(string));
                    /*Add by Hina sharma on 29-05-2025 transport detail*/
                    dt.Columns.Add("GR_Number", typeof(string));
                    dt.Columns.Add("GR_Dt", typeof(string));
                    dt.Columns.Add("No_Of_Pkgs", typeof(string));
                    dt.Columns.Add("Trans_Name", typeof(string));
                    dt.Columns.Add("Veh_Number", typeof(string));
                    dt.Columns.Add("Driver_Name", typeof(string));
                    dt.Columns.Add("Mob_No", typeof(string));
                    dt.Columns.Add("Tot_Tonnage", typeof(string));

                    DataRow dtrow = dt.NewRow();

                    dtrow["MenuDocumentId"] = DocumentMenuId;
                    dtrow["comp_id"] = Session["CompId"].ToString();
                    dtrow["br_id"] = Session["BranchId"].ToString();
                    dtrow["srt_no"] = _SalesReturn_Model.srt_no;
                    dtrow["srt_dt"] = _SalesReturn_Model.srt_dt;// DateTime.Now.ToString("yyyy-MM-dd");
                    if (src_type == "A")
                    {
                        dtrow["inv_no"] = _SalesReturn_Model.InvBillNumber;
                        dtrow["inv_dt"] = _SalesReturn_Model.InvBillDate;
                    }
                    else
                    {
                        dtrow["inv_no"] = _SalesReturn_Model.src_doc_no;
                        dtrow["inv_dt"] = _SalesReturn_Model.src_doc_date;
                    }
                    if (_SalesReturn_Model.CustomerID == null)
                    {
                        dtrow["cust_id"] = DBNull.Value;
                    }
                    else
                    {
                        dtrow["cust_id"] = _SalesReturn_Model.CustomerID;
                    }
                    dtrow["inv_value"] = _SalesReturn_Model.inv_value;
                    dtrow["srt_value"] = _SalesReturn_Model.srt_value;
                    dtrow["CreateBy"] = Session["UserId"].ToString();
                    //dtrow["srt_status"] = Session["AppStatus"].ToString();
                    dtrow["srt_status"] = "D";
                    dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrow["UserIP"] = Session["UserIP"].ToString();
                    //dtrow["TransType"] = Session["TransType"].ToString();
                    if (_SalesReturn_Model.srt_no != null)
                    {
                        dtrow["TransType"] = "Update";
                    }
                    else
                    {
                        dtrow["TransType"] = "Save";
                    }
                    string roundoffflag = _SalesReturn_Model.RoundOffFlag.ToString();
                    if (roundoffflag == "False")
                    {
                        dtrow["roundoff"] = "N";
                    }
                    else
                    {
                        dtrow["roundoff"] = "Y";
                    }
                    dtrow["pm_flag"] = _SalesReturn_Model.pmflagval;
                    /*Add by Hina sharma on 15-05-2025,29-05-2025 all other detail section with transport detail*/
                    dtrow["cust_ref"] = _SalesReturn_Model.Customer_Reference;
                    //dtrow["pay_term"] = _SalesReturn_Model.Payment_term.Trim();
                    //dtrow["deli_term"] = _SalesReturn_Model.Delivery_term.Trim();
                    dtrow["placeofsupp"] = _SalesReturn_Model.PlaceOfSupply;
                    dtrow["remarks"] = _SalesReturn_Model.SlsRemarks;
                    dtrow["inv_heading"] = _SalesReturn_Model.Invoice_Heading;
                    dtrow["GR_Number"] = _SalesReturn_Model.GR_No;
                    dtrow["GR_Dt"] = _SalesReturn_Model.HdnGRDate;
                    dtrow["No_Of_Pkgs"] = _SalesReturn_Model.No_Of_Packages;
                    dtrow["Trans_Name"] = _SalesReturn_Model.Transpt_NameID;
                    dtrow["Veh_Number"] = _SalesReturn_Model.Veh_Number;
                    dtrow["Driver_Name"] = _SalesReturn_Model.Driver_Name;
                    dtrow["Mob_No"] = _SalesReturn_Model.Mob_No;
                    dtrow["Tot_Tonnage"] = _SalesReturn_Model.Tot_Tonnage;

                    dt.Rows.Add(dtrow);

                    SalesReturnHeader = dt;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(Int32));
                    dtItem.Columns.Add("br_id", typeof(Int32));
                    dtItem.Columns.Add("ship_no", typeof(string));
                    dtItem.Columns.Add("ship_date", typeof(string));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("ship_qty", typeof(string));
                    dtItem.Columns.Add("return_qty", typeof(string));
                    dtItem.Columns.Add("inv_value", typeof(string));
                    dtItem.Columns.Add("srt_value", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("item_acc_id", typeof(string));
                    dtItem.Columns.Add("wh_id", typeof(string));
                    dtItem.Columns.Add("Price", typeof(string));
                    dtItem.Columns.Add("tax_amt", typeof(string));
                    dtItem.Columns.Add("srt_avl", typeof(string));

                    if (_SalesReturn_Model.SRItemdetails != null)
                    {
                        JArray jObject = JArray.Parse(_SalesReturn_Model.SRItemdetails);
                        for (int i = 0; i < jObject.Count; i++)
                        {
                            decimal ship_qty, return_qty;
                            if (jObject[i]["ShipQuantity"].ToString() == "")
                                ship_qty = 0;
                            else
                                ship_qty = Convert.ToDecimal(jObject[i]["ShipQuantity"].ToString());

                            if (jObject[i]["ReturnQuantity"].ToString() == "")
                                return_qty = 0;
                            else
                                return_qty = Convert.ToDecimal(jObject[i]["ReturnQuantity"].ToString());

                            DataRow dtrowItemdetails = dtItem.NewRow();
                            dtrowItemdetails["comp_id"] = Session["CompId"].ToString();
                            dtrowItemdetails["br_id"] = Session["BranchId"].ToString();
                            dtrowItemdetails["ship_no"] = jObject[i]["ShipNo"].ToString();
                            dtrowItemdetails["ship_date"] = jObject[i]["ShipDate"].ToString();
                            dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                            string str = Convert.ToInt32(jObject[i]["UOMId"]).ToString();
                            dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMId"]);
                            dtrowItemdetails["ship_qty"] = ship_qty;
                            dtrowItemdetails["return_qty"] = return_qty;
                            dtrowItemdetails["inv_value"] = jObject[i]["InvoiceValue"];
                            dtrowItemdetails["srt_value"] = jObject[i]["ReturnValue"];
                            dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                            dtrowItemdetails["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                            dtrowItemdetails["wh_id"] = jObject[i]["wh_id"].ToString();
                            dtrowItemdetails["Price"] = jObject[i]["Price"].ToString();
                            dtrowItemdetails["tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                            dtrowItemdetails["srt_avl"] = jObject[i]["pending_qty"].ToString();

                            dtItem.Rows.Add(dtrowItemdetails);
                        }
                        SalesReturnItemDetails = dtItem;
                        ViewData["ItemDetails"] = dtitemdetail(jObject);
                    }
                    DataTable dtItemBatchSerial = new DataTable();
                    dtItemBatchSerial.Columns.Add("comp_id", typeof(Int32));
                    dtItemBatchSerial.Columns.Add("br_id", typeof(Int32));
                    dtItemBatchSerial.Columns.Add("item_id", typeof(string));
                    dtItemBatchSerial.Columns.Add("uom_id", typeof(int));
                    dtItemBatchSerial.Columns.Add("lot_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("serial_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("batch_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("srt_qty", typeof(string));
                    dtItemBatchSerial.Columns.Add("wh_id", typeof(int));
                    dtItemBatchSerial.Columns.Add("Batchable", typeof(string));
                    dtItemBatchSerial.Columns.Add("Serialable", typeof(string));
                    dtItemBatchSerial.Columns.Add("ship_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("ship_date", typeof(string));
                    dtItemBatchSerial.Columns.Add("exp_dt", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_name", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_mrp", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_date", typeof(string));

                    if (src_type == "A")
                    {
                        if (_SalesReturn_Model.BatchItemDeatilData != null)
                        {
                            JArray jObject1 = JArray.Parse(_SalesReturn_Model.BatchItemDeatilData);
                            for (int i = 0; i < jObject1.Count; i++)
                            {
                                decimal srt_qty;
                                if (jObject1[i]["BatchQty"].ToString() == "")
                                    srt_qty = 0;
                                else
                                    srt_qty = Convert.ToDecimal(jObject1[i]["BatchQty"].ToString());

                                DataRow dtrowItemBatchSerialdetails = dtItemBatchSerial.NewRow();
                                dtrowItemBatchSerialdetails["comp_id"] = Session["CompId"].ToString();
                                dtrowItemBatchSerialdetails["br_id"] = Session["BranchId"].ToString();
                                dtrowItemBatchSerialdetails["item_id"] = jObject1[i]["itemid"].ToString();
                                dtrowItemBatchSerialdetails["uom_id"] = 0;
                                dtrowItemBatchSerialdetails["lot_no"] = "";
                                dtrowItemBatchSerialdetails["serial_no"] = "";
                                dtrowItemBatchSerialdetails["batch_no"] = jObject1[i]["BatchNo"].ToString();
                                dtrowItemBatchSerialdetails["srt_qty"] = srt_qty;
                                dtrowItemBatchSerialdetails["wh_id"] = 0;
                                dtrowItemBatchSerialdetails["Batchable"] = jObject1[i]["batchable"].ToString();
                                dtrowItemBatchSerialdetails["Serialable"] = jObject1[i]["Seriable"].ToString();
                                dtrowItemBatchSerialdetails["ship_no"] = "";
                                dtrowItemBatchSerialdetails["ship_date"] = "";
                                dtrowItemBatchSerialdetails["exp_dt"] = jObject1[i]["BatchExDate"].ToString();
                                dtrowItemBatchSerialdetails["mfg_name"] = IsBlank(jObject1[i]["mfg_name"].ToString(),null);
                                dtrowItemBatchSerialdetails["mfg_mrp"] = IsBlank(jObject1[i]["mfg_mrp"].ToString(),null);
                                dtrowItemBatchSerialdetails["mfg_date"] = IsBlank(jObject1[i]["mfg_date"].ToString(),null);

                                dtItemBatchSerial.Rows.Add(dtrowItemBatchSerialdetails);
                            }
                            SalesReturnLotBatchSerial = dtItemBatchSerial;
                            //ViewData["BatchSerialDetails"] = dtbatchserialdetail(jObject1, src_type);
                        }
                        if (_SalesReturn_Model.SerialItemDeatilData != null)
                        {
                            JArray jObject1 = JArray.Parse(_SalesReturn_Model.SerialItemDeatilData);
                            for (int i = 0; i < jObject1.Count; i++)
                            {
                                DataRow dtrowItemBatchSerialdetails = dtItemBatchSerial.NewRow();
                                dtrowItemBatchSerialdetails["comp_id"] = Session["CompId"].ToString();
                                dtrowItemBatchSerialdetails["br_id"] = Session["BranchId"].ToString();
                                dtrowItemBatchSerialdetails["item_id"] = jObject1[i]["itemid"].ToString();
                                dtrowItemBatchSerialdetails["uom_id"] = 0;
                                dtrowItemBatchSerialdetails["lot_no"] = "";
                                dtrowItemBatchSerialdetails["serial_no"] = jObject1[i]["SerialNo"].ToString();
                                dtrowItemBatchSerialdetails["batch_no"] = "";
                                dtrowItemBatchSerialdetails["srt_qty"] = "1";
                                dtrowItemBatchSerialdetails["wh_id"] = 0;
                                dtrowItemBatchSerialdetails["Batchable"] = jObject1[i]["batchable"].ToString();
                                dtrowItemBatchSerialdetails["Serialable"] = jObject1[i]["Seriable"].ToString();
                                dtrowItemBatchSerialdetails["ship_no"] = "";
                                dtrowItemBatchSerialdetails["ship_date"] = "";
                                dtrowItemBatchSerialdetails["exp_dt"] = "";
                                dtrowItemBatchSerialdetails["mfg_name"] = IsBlank(jObject1[i]["mfg_name"].ToString(),null);
                                dtrowItemBatchSerialdetails["mfg_mrp"] = IsBlank(jObject1[i]["mfg_mrp"].ToString(),null);
                                dtrowItemBatchSerialdetails["mfg_date"] = IsBlank(jObject1[i]["mfg_date"].ToString(),null);

                                dtItemBatchSerial.Rows.Add(dtrowItemBatchSerialdetails);
                            }
                            SalesReturnLotBatchSerial = dtItemBatchSerial;
                            //ViewData["BatchSerialDetails"] = dtbatchserialdetail(jObject1, src_type);
                        }
                    }
                    else
                    {
                        if (src_type != "R")
                        {
                            if (_SalesReturn_Model.SRItemBatchSerialDetail != null)
                            {
                                JArray jObject1 = JArray.Parse(_SalesReturn_Model.SRItemBatchSerialDetail);
                                for (int i = 0; i < jObject1.Count; i++)
                                {
                                    decimal srt_qty;
                                    if (jObject1[i]["RetQty"].ToString() == "")
                                        srt_qty = 0;
                                    else
                                        srt_qty = Convert.ToDecimal(jObject1[i]["RetQty"].ToString());

                                    DataRow dtrowItemBatchSerialdetails = dtItemBatchSerial.NewRow();
                                    dtrowItemBatchSerialdetails["comp_id"] = Session["CompId"].ToString();
                                    dtrowItemBatchSerialdetails["br_id"] = Session["BranchId"].ToString();
                                    dtrowItemBatchSerialdetails["item_id"] = jObject1[i]["ItmCode"].ToString();
                                    string str = Convert.ToInt32(jObject1[i]["ItmUomId"]).ToString();
                                    dtrowItemBatchSerialdetails["uom_id"] = Convert.ToInt32(jObject1[i]["ItmUomId"]);
                                    dtrowItemBatchSerialdetails["lot_no"] = jObject1[i]["Lot"].ToString();
                                    dtrowItemBatchSerialdetails["serial_no"] = jObject1[i]["Serial"].ToString();
                                    dtrowItemBatchSerialdetails["batch_no"] = jObject1[i]["Batch"].ToString();
                                    dtrowItemBatchSerialdetails["srt_qty"] = srt_qty;
                                    dtrowItemBatchSerialdetails["wh_id"] = jObject1[i]["wh_id"].ToString();
                                    dtrowItemBatchSerialdetails["Batchable"] = jObject1[i]["Batchable"].ToString();
                                    dtrowItemBatchSerialdetails["Serialable"] = jObject1[i]["Serialable"].ToString();
                                    dtrowItemBatchSerialdetails["ship_no"] = jObject1[i]["IconShipNumber"].ToString();
                                    dtrowItemBatchSerialdetails["ship_date"] = jObject1[i]["IconShipDate"].ToString();
                                    dtrowItemBatchSerialdetails["exp_dt"] = "";
                                    dtrowItemBatchSerialdetails["mfg_name"] = IsBlank(jObject1[i]["mfg_name"].ToString(),null);
                                    dtrowItemBatchSerialdetails["mfg_mrp"] = IsBlank(jObject1[i]["mfg_mrp"].ToString(),null);
                                    dtrowItemBatchSerialdetails["mfg_date"] = IsBlank(jObject1[i]["mfg_date"].ToString(),null);

                                    dtItemBatchSerial.Rows.Add(dtrowItemBatchSerialdetails);
                                }
                                SalesReturnLotBatchSerial = dtItemBatchSerial;
                                ViewData["BatchSerialDetails"] = dtbatchserialdetail(jObject1, src_type);
                            }
                        }

                        else
                        {
                            SalesReturnLotBatchSerial = dtItemBatchSerial;
                        }
                    }

                    DataTable Voudt = new DataTable();
                    Voudt.Columns.Add("comp_id", typeof(string));
                    Voudt.Columns.Add("vou_sr_no", typeof(string));
                    Voudt.Columns.Add("gl_sr_no", typeof(string));
                    Voudt.Columns.Add("id", typeof(string));
                    Voudt.Columns.Add("type", typeof(string));
                    Voudt.Columns.Add("doctype", typeof(string));
                    Voudt.Columns.Add("Value", typeof(string));
                    Voudt.Columns.Add("ValueInBase", typeof(string));
                    Voudt.Columns.Add("DrAmt", typeof(string));
                    Voudt.Columns.Add("CrAmt", typeof(string));
                    Voudt.Columns.Add("TransType", typeof(string));
                    Voudt.Columns.Add("Gltype", typeof(string));
                    Voudt.Columns.Add("parent", typeof(string));
                    Voudt.Columns.Add("DrAmtInBase", typeof(string));
                    Voudt.Columns.Add("CrAmtInBase", typeof(string));
                    Voudt.Columns.Add("curr_id", typeof(string));
                    Voudt.Columns.Add("conv_rate", typeof(string));
                    Voudt.Columns.Add("vou_type", typeof(string));
                    Voudt.Columns.Add("bill_no", typeof(string));
                    Voudt.Columns.Add("bill_date", typeof(string));
                    Voudt.Columns.Add("gl_narr", typeof(string));
                    //Voudt.Columns.Add("comp_id", typeof(int));
                    //Voudt.Columns.Add("accid", typeof(string));
                    //Voudt.Columns.Add("type", typeof(string));
                    //Voudt.Columns.Add("doctype", typeof(string));
                    //Voudt.Columns.Add("value", typeof(string));
                    //Voudt.Columns.Add("dramt", typeof(string));
                    //Voudt.Columns.Add("cramt", typeof(string));
                    //Voudt.Columns.Add("Transtype", typeof(string));
                    //Voudt.Columns.Add("GLType", typeof(string));

                    if (_SalesReturn_Model.Voudetails != null)
                    {
                        JArray AObject = JArray.Parse(_SalesReturn_Model.Voudetails);
                        for (int i = 0; i < AObject.Count; i++)
                        {

                            DataRow dtrowVoudetails = Voudt.NewRow();
                            dtrowVoudetails["comp_id"] = AObject[i]["comp_id"].ToString();
                            dtrowVoudetails["vou_sr_no"] = AObject[i]["VouSrNo"].ToString();
                            dtrowVoudetails["gl_sr_no"] = AObject[i]["GlSrNo"].ToString();
                            dtrowVoudetails["id"] = AObject[i]["id"].ToString();
                            dtrowVoudetails["type"] = AObject[i]["type"].ToString();
                            dtrowVoudetails["doctype"] = AObject[i]["doctype"].ToString();
                            dtrowVoudetails["Value"] = AObject[i]["Value"].ToString();
                            dtrowVoudetails["ValueInBase"] = AObject[i]["ValueInBase"].ToString();
                            dtrowVoudetails["DrAmt"] = AObject[i]["DrAmt"].ToString();
                            dtrowVoudetails["CrAmt"] = AObject[i]["CrAmt"].ToString();
                            dtrowVoudetails["TransType"] = AObject[i]["TransType"].ToString();
                            dtrowVoudetails["Gltype"] = AObject[i]["Gltype"].ToString();
                            dtrowVoudetails["parent"] = "0";// AObject[i]["Gltype"].ToString();
                            dtrowVoudetails["DrAmtInBase"] = AObject[i]["DrAmtInBase"].ToString();
                            dtrowVoudetails["CrAmtInBase"] = AObject[i]["CrAmtInBase"].ToString();
                            dtrowVoudetails["curr_id"] = AObject[i]["curr_id"].ToString();
                            dtrowVoudetails["conv_rate"] = AObject[i]["conv_rate"].ToString();
                            dtrowVoudetails["vou_type"] = AObject[i]["vou_type"].ToString();
                            dtrowVoudetails["bill_no"] = AObject[i]["bill_no"].ToString();
                            dtrowVoudetails["bill_date"] = AObject[i]["bill_date"].ToString();
                            dtrowVoudetails["gl_narr"] = AObject[i]["gl_narr"].ToString();
                            //dtrowVoudetails["comp_id"] = Session["CompId"].ToString();
                            //dtrowVoudetails["accid"] = AObject[i]["accid"].ToString();
                            //dtrowVoudetails["type"] = AObject[i]["type"].ToString();
                            //dtrowVoudetails["doctype"] = AObject[i]["doctype"].ToString();
                            //dtrowVoudetails["value"] = AObject[i]["Value"].ToString();
                            //dtrowVoudetails["dramt"] = AObject[i]["DrAmt"].ToString();
                            //dtrowVoudetails["cramt"] = AObject[i]["CrAmt"].ToString();
                            //dtrowVoudetails["TransType"] = AObject[i]["TransType"].ToString();
                            //dtrowVoudetails["GLType"] = AObject[i]["gl_type"].ToString();
                            Voudt.Rows.Add(dtrowVoudetails);
                        }
                        SalesReturnVoudetail = Voudt;
                        ViewData["VouDetails"] = dtVoudetail(AObject);
                    }
                    /*----------------------Sub Item ----------------------*/
                    //DataTable dtSubItem = new DataTable();
                    //dtSubItem.Columns.Add("item_id", typeof(string));
                    //dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    //dtSubItem.Columns.Add("qty", typeof(string));

                    //if (_SalesReturn_Model.SubItemDetailsDt != null)
                    //{
                    //    JArray jObject2 = JArray.Parse(_SalesReturn_Model.SubItemDetailsDt);
                    //    for (int i = 0; i < jObject2.Count; i++)
                    //    {
                    //        DataRow dtrowItemdetails = dtSubItem.NewRow();
                    //        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                    //        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                    //        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                    //        dtSubItem.Rows.Add(dtrowItemdetails);
                    //    }
                    //    ViewData["SubItemdetail"] = dtSubitemdetail(jObject2);
                    //}

                    /*------------------Sub Item end----------------------*/

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();

                    if (_SalesReturn_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_SalesReturn_Model.SubItemDetailsDt);
                        dtSubItem = dtSubitemdetail(jObject2);
                        ViewData["SubItemdetail"] = dtSubItem;
                    }
                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("vou_sr_no", typeof(string));
                    CC_Details.Columns.Add("gl_sr_no", typeof(string));
                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));
                    //CC_Details.Columns.Add("cc_name", typeof(string));
                    //CC_Details.Columns.Add("cc_val_name", typeof(string));
                    if (_SalesReturn_Model.CC_DetailList != null)
                    {
                        JArray JAObj = JArray.Parse(_SalesReturn_Model.CC_DetailList);
                        for (int i = 0; i < JAObj.Count; i++)
                        {
                            DataRow dtrowLines = CC_Details.NewRow();

                            dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                            dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
                            dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                            dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                            dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                            dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                            //dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                            //dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();

                            CC_Details.Rows.Add(dtrowLines);
                        }
                        CRCostCenterDetails = CC_Details;
                        ViewData["CCdetail"] = dtCCdetail(JAObj);
                    }
                    DataTable DtblTaxDetail = new DataTable();
                    DtblTaxDetail = ToDtblTaxDetail(_SalesReturn_Model.TaxDetail, "Y");
                    DtblVouGLDetail = ToDtblVouGlDetail(_SalesReturn_Model.CustomVouGlDetails);
                    /*------------------Sub Item end----------------------*/
                    DtblOCDetail = ToDtblOCDetail(_SalesReturn_Model.ItemOCdetails);
                    DtblOCTaxDetail = ToDtblTaxDetail1(_SalesReturn_Model.ItemOCTaxdetails);

                    String SaveMessage = _SalesReturn_ISERVICES.InsertSalesReturnDetail(SalesReturnHeader, SalesReturnItemDetails
                        , SalesReturnLotBatchSerial, SalesReturnVoudetail, dtSubItem, CRCostCenterDetails, DtblTaxDetail
                        , _SalesReturn_Model.Src_Type, _SalesReturn_Model.InvBillNumber, _SalesReturn_Model.InvBillDate
                        , _SalesReturn_Model.Payment_term, _SalesReturn_Model.Delivery_term, DtblVouGLDetail, DtblOCDetail
                        , DtblOCTaxDetail, IsNull(_SalesReturn_Model.OcAmt,"0"));
                    if (SaveMessage == "DocModify")
                    {
                        _SalesReturn_Model.Message = "DocModify";
                        _SalesReturn_Model.BtnName = "Refresh";
                        _SalesReturn_Model.Command = "Refresh";
                        TempData["ModelData"] = _SalesReturn_Model;
                        return RedirectToAction("SalesReturnDetail");
                    }
                    else
                    {
                        //string SalesReturnNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                        //string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                        string SalesReturnNo = SaveMessage.Split(',')[1].Trim();
                        string Message = SaveMessage.Split(',')[0].Trim();
                        string SalesReturnDate = SaveMessage.Split(',')[2].Trim();
                        if (Message == "Data_Not_Found")
                        {
                            ViewBag.MenuPageName = getDocumentName();
                            _SalesReturn_Model.Title = title;
                            var msg = Message.Replace("_", " ") + " " + SalesReturnNo + " in" + _SalesReturn_Model.Title;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _SalesReturn_Model.Message = Message.Split(',')[0].Replace("_", "");
                            return RedirectToAction("SalesReturnDetail");
                        }
                        if (Message == "Update" || Message == "Save")
                        {
                            //    Session["Message"] = "Save";
                            //Session["Command"] = "Update";
                            //Session["SalesReturnNo"] = SalesReturnNo;
                            //Session["SalesReturnDate"] = SalesReturnDate;
                            //Session["TransType"] = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnToDetailPage";
                            _SalesReturn_Model.Message = "Save";
                            _SalesReturn_Model.Command = "Update";
                            _SalesReturn_Model.SalesReturnNo = SalesReturnNo;
                            _SalesReturn_Model.SalesReturnDate = SalesReturnDate;
                            _SalesReturn_Model.TransType = "Update";
                            _SalesReturn_Model.BtnName = "BtnToDetailPage";
                            _SalesReturn_Model.AppStatus = "D";
                            return RedirectToAction("SalesReturnDetail");
                        }
                        return RedirectToAction("SalesReturnDetail");
                    }
                }
                else
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    _SalesReturn_Model.CreatedBy = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage = _SalesReturn_ISERVICES.SalesReturnCancel(_SalesReturn_Model, CompID, br_id, mac_id, _SalesReturn_Model.Src_Type);
                    //string SalesReturnNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["SalesReturnNo"] = _SalesReturn_Model.srt_no;
                    //Session["SalesReturnDate"] = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";


                    //try
                    //{
                    //    DateTime parsedSalesReturnDate = DateTime.Parse(_SalesReturn_Model.SalesReturnDate);
                    //    string fileName = "SR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    //    var filePath = SavePdfDocToSendOnEmailAlert(_SalesReturn_Model, _SalesReturn_Model.SalesReturnNo, parsedSalesReturnDate, _SalesReturn_Model.Src_Type, fileName);
                    //    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _SalesReturn_Model.srt_no, "C", Session["UserId"].ToString(), "", filePath);
                    //}
                    //catch (Exception exMail)
                    //{
                    //    _SalesReturn_Model.Message = "ErrorInMail";
                    //    string path = Server.MapPath("~");
                    //    Errorlog.LogError(path, exMail);
                    //}
                    var messg = SaveMessage.Tables[0].Rows[0]["result"].ToString();
                    if (messg == "StkNotAvl")
                    {
                        //_Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _SalesReturn_Model.srt_no, "C", Session["UserId"].ToString(), "");
                        _SalesReturn_Model.Message = "StkNotAvl";
                        _SalesReturn_Model.Command = "Update";
                        _SalesReturn_Model.SalesReturnNo = _SalesReturn_Model.srt_no;
                        _SalesReturn_Model.SalesReturnDate = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        _SalesReturn_Model.TransType = "Update";
                        _SalesReturn_Model.AppStatus = "D";
                        _SalesReturn_Model.BtnName = "BtnToDetailPage";
                    }
                    else
                    {
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _SalesReturn_Model.srt_no, "C", Session["UserId"].ToString(), "");
                        _SalesReturn_Model.Message = "Cancelled";
                        _SalesReturn_Model.Command = "Update";
                        _SalesReturn_Model.SalesReturnNo = _SalesReturn_Model.srt_no;
                        _SalesReturn_Model.SalesReturnDate = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                        _SalesReturn_Model.TransType = "Update";
                        _SalesReturn_Model.AppStatus = "D";
                        _SalesReturn_Model.BtnName = "BtnToDetailPage";
                    }
                    //_Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _SalesReturn_Model.srt_no, "C", Session["UserId"].ToString(), "");
                    //_SalesReturn_Model.Message = "Cancelled";
                    //_SalesReturn_Model.Command = "Update";
                    //_SalesReturn_Model.SalesReturnNo = _SalesReturn_Model.srt_no;
                    //_SalesReturn_Model.SalesReturnDate = _SalesReturn_Model.srt_dt.ToString("yyyy-MM-dd");
                    //_SalesReturn_Model.TransType = "Update";
                    //_SalesReturn_Model.AppStatus = "D";
                    //_SalesReturn_Model.BtnName = "BtnToDetailPage";
                    return RedirectToAction("SalesReturnDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
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
                    //ViewData["OCDetails"] = dtOCdetail(jObject);
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
        private DataTable ToDtblVouGlDetail(string VouGlDetails)
        {
            try
            {
                DataTable DtblVouGlDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("id", typeof(double));
                dtItem.Columns.Add("type", typeof(int));
                dtItem.Columns.Add("doctype", typeof(string));
                dtItem.Columns.Add("Value", typeof(int));
                dtItem.Columns.Add("DrAmt", typeof(string));
                dtItem.Columns.Add("CrAmt", typeof(string));
                dtItem.Columns.Add("TransType", typeof(string));
                dtItem.Columns.Add("Gltype", typeof(string));

                JArray jObject = JArray.Parse(VouGlDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
                    dtrowLines["id"] = jObject[i]["id"].ToString();
                    dtrowLines["type"] = 'I';
                    dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
                    dtrowLines["Value"] = jObject[i]["Value"].ToString();
                    dtrowLines["DrAmt"] = jObject[i]["DrAmt"].ToString();
                    dtrowLines["CrAmt"] = jObject[i]["CrAmt"].ToString();
                    dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
                    dtrowLines["Gltype"] = jObject[i]["Gltype"].ToString();

                    dtItem.Rows.Add(dtrowLines);
                }
                DtblVouGlDetail = dtItem;
                //ViewData["VouDetails"] = dtVoudetail(jObject);
                return DtblVouGlDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblTaxDetail(string TaxDetails, string recov)
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
                Tax_detail.Columns.Add("tax_recov", typeof(string));
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
                        dtrowTaxDetailsLines["tax_recov"] = recov == "Y" ? jObjectTax[i]["tax_recov"].ToString() : "";
                        Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                    }
                    ViewData["TaxDetails"] = dtTaxdetail(jObjectTax);
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
        private DataTable ToDtblTaxDetail1(string TaxDetails)
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
                    //ViewData["TaxDetails"] = dtTaxdetail(jObject);
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
        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("ship_no", typeof(string));
            dtItem.Columns.Add("ship_dt", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("ship_qty", typeof(string));
            dtItem.Columns.Add("return_qty", typeof(string));
            dtItem.Columns.Add("inv_val", typeof(string));
            dtItem.Columns.Add("srt_val", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("pending_qty", typeof(string));
            dtItem.Columns.Add("srt_qty", typeof(string));



            for (int i = 0; i < jObject.Count; i++)
            {
                decimal ship_qty, return_qty;
                if (jObject[i]["ShipQuantity"].ToString() == "")
                    ship_qty = 0;
                else
                    ship_qty = Convert.ToDecimal(jObject[i]["ShipQuantity"].ToString());

                if (jObject[i]["ReturnQuantity"].ToString() == "")
                    return_qty = 0;
                else
                    return_qty = Convert.ToDecimal(jObject[i]["ReturnQuantity"].ToString());

                DataRow dtrowItemdetails = dtItem.NewRow();

                dtrowItemdetails["ship_no"] = jObject[i]["ShipNo"].ToString();
                dtrowItemdetails["ship_dt"] = jObject[i]["ShipDate"].ToString();
                dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowItemdetails["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMId"]);
                dtrowItemdetails["uom_name"] = jObject[i]["uom_name"].ToString();
                dtrowItemdetails["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowItemdetails["ship_qty"] = ship_qty;
                dtrowItemdetails["return_qty"] = return_qty;
                dtrowItemdetails["inv_val"] = jObject[i]["InvoiceValue"];
                dtrowItemdetails["srt_val"] = jObject[i]["ReturnValue"];
                dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                dtrowItemdetails["pending_qty"] = jObject[i]["pending_qty"].ToString();
                dtrowItemdetails["srt_qty"] = return_qty;

                dtItem.Rows.Add(dtrowItemdetails);
            }

            return dtItem;
        }
        public DataTable dtbatchserialdetail(JArray jObject1, string src_type)
        {
            DataTable dtItemBatchSerial = new DataTable();

            dtItemBatchSerial.Columns.Add("item_id", typeof(string));
            dtItemBatchSerial.Columns.Add("uom_id", typeof(int));
            dtItemBatchSerial.Columns.Add("lot_no", typeof(string));
            dtItemBatchSerial.Columns.Add("serial_no", typeof(string));
            dtItemBatchSerial.Columns.Add("batch_no", typeof(string));
            dtItemBatchSerial.Columns.Add("srt_qty", typeof(string));
            dtItemBatchSerial.Columns.Add("wh_id", typeof(int));
            dtItemBatchSerial.Columns.Add("Batchable", typeof(string));
            dtItemBatchSerial.Columns.Add("Serialable", typeof(string));
            dtItemBatchSerial.Columns.Add("ship_no", typeof(string));
            dtItemBatchSerial.Columns.Add("ship_date", typeof(string));
            dtItemBatchSerial.Columns.Add("exp_dt", typeof(string));

            for (int i = 0; i < jObject1.Count; i++)
            {
                decimal srt_qty;
                if (jObject1[i]["RetQty"].ToString() == "")
                    srt_qty = 0;
                else
                    srt_qty = Convert.ToDecimal(jObject1[i]["RetQty"].ToString());

                DataRow dtrowItemBatchSerialdetails = dtItemBatchSerial.NewRow();
                dtrowItemBatchSerialdetails["item_id"] = jObject1[i]["ItmCode"].ToString();
                dtrowItemBatchSerialdetails["uom_id"] = Convert.ToInt32(jObject1[i]["ItmUomId"]);
                dtrowItemBatchSerialdetails["lot_no"] = jObject1[i]["Lot"].ToString();
                dtrowItemBatchSerialdetails["serial_no"] = jObject1[i]["Serial"].ToString();
                dtrowItemBatchSerialdetails["batch_no"] = jObject1[i]["Batch"].ToString();
                dtrowItemBatchSerialdetails["srt_qty"] = srt_qty;
                dtrowItemBatchSerialdetails["wh_id"] = jObject1[i]["wh_id"].ToString();
                dtrowItemBatchSerialdetails["Batchable"] = jObject1[i]["Batchable"].ToString();
                dtrowItemBatchSerialdetails["Serialable"] = jObject1[i]["Serialable"].ToString();
                dtrowItemBatchSerialdetails["ship_no"] = jObject1[i]["IconShipNumber"].ToString();
                dtrowItemBatchSerialdetails["ship_date"] = jObject1[i]["IconShipDate"].ToString();
                dtrowItemBatchSerialdetails["exp_dt"] = "";

                dtItemBatchSerial.Rows.Add(dtrowItemBatchSerialdetails);
            }
            return dtItemBatchSerial;
        }
        public DataTable dtVoudetail(JArray AObject)
        {
            DataTable Voudt = new DataTable();
            Voudt.Columns.Add("comp_id", typeof(string));
            Voudt.Columns.Add("vou_sr_no", typeof(string));
            Voudt.Columns.Add("gl_sr_no", typeof(string));
            Voudt.Columns.Add("id", typeof(string));
            Voudt.Columns.Add("type", typeof(string));
            Voudt.Columns.Add("doctype", typeof(string));
            Voudt.Columns.Add("Value", typeof(string));
            Voudt.Columns.Add("ValueInBase", typeof(string));
            Voudt.Columns.Add("DrAmt", typeof(string));
            Voudt.Columns.Add("CrAmt", typeof(string));
            Voudt.Columns.Add("TransType", typeof(string));
            Voudt.Columns.Add("Gltype", typeof(string));
            Voudt.Columns.Add("parent", typeof(string));
            Voudt.Columns.Add("DrAmtInBase", typeof(string));
            Voudt.Columns.Add("CrAmtInBase", typeof(string));
            Voudt.Columns.Add("curr_id", typeof(string));
            Voudt.Columns.Add("conv_rate", typeof(string));
            Voudt.Columns.Add("vou_type", typeof(string));
            Voudt.Columns.Add("bill_no", typeof(string));
            Voudt.Columns.Add("bill_date", typeof(string));
            //Voudt.Columns.Add("comp_id", typeof(string));
            //Voudt.Columns.Add("acc_id", typeof(string));
            //Voudt.Columns.Add("acc_name", typeof(string));
            //Voudt.Columns.Add("type", typeof(string));
            //Voudt.Columns.Add("doctype", typeof(string));
            //Voudt.Columns.Add("Value", typeof(string));
            //Voudt.Columns.Add("dr_amt_sp", typeof(string));
            //Voudt.Columns.Add("cr_amt_sp", typeof(string));
            //Voudt.Columns.Add("TransType", typeof(string));
            //Voudt.Columns.Add("vou_type", typeof(string));
            for (int i = 0; i < AObject.Count; i++)
            {
                DataRow dtrowVoudetails = Voudt.NewRow();
                dtrowVoudetails["comp_id"] = AObject[i]["comp_id"].ToString();
                dtrowVoudetails["vou_sr_no"] = AObject[i]["VouSrNo"].ToString();
                dtrowVoudetails["gl_sr_no"] = AObject[i]["GlSrNo"].ToString();
                dtrowVoudetails["id"] = AObject[i]["id"].ToString();
                dtrowVoudetails["type"] = AObject[i]["type"].ToString();
                dtrowVoudetails["doctype"] = AObject[i]["doctype"].ToString();
                dtrowVoudetails["Value"] = AObject[i]["Value"].ToString();
                dtrowVoudetails["ValueInBase"] = AObject[i]["ValueInBase"].ToString();
                dtrowVoudetails["DrAmt"] = AObject[i]["DrAmt"].ToString();
                dtrowVoudetails["CrAmt"] = AObject[i]["CrAmt"].ToString();
                dtrowVoudetails["TransType"] = AObject[i]["TransType"].ToString();
                dtrowVoudetails["Gltype"] = AObject[i]["Gltype"].ToString();
                dtrowVoudetails["parent"] = "0";// AObject[i]["Gltype"].ToString();
                dtrowVoudetails["DrAmtInBase"] = AObject[i]["DrAmtInBase"].ToString();
                dtrowVoudetails["CrAmtInBase"] = AObject[i]["CrAmtInBase"].ToString();
                dtrowVoudetails["curr_id"] = AObject[i]["curr_id"].ToString();
                dtrowVoudetails["conv_rate"] = AObject[i]["conv_rate"].ToString();
                dtrowVoudetails["vou_type"] = AObject[i]["vou_type"].ToString();
                dtrowVoudetails["bill_no"] = AObject[i]["bill_no"].ToString();
                dtrowVoudetails["bill_date"] = AObject[i]["bill_date"].ToString();
                //dtrowVoudetails["comp_id"] = AObject[i]["comp_id"].ToString();
                //dtrowVoudetails["acc_id"] = AObject[i]["accid"].ToString();
                //dtrowVoudetails["acc_name"] = AObject[i]["acc_name"].ToString();
                //dtrowVoudetails["type"] = AObject[i]["type"].ToString();
                //dtrowVoudetails["doctype"] = AObject[i]["doctype"].ToString();
                //dtrowVoudetails["Value"] = AObject[i]["Value"].ToString();
                //dtrowVoudetails["dr_amt_sp"] = AObject[i]["DrAmt"].ToString();
                //dtrowVoudetails["cr_amt_sp"] = AObject[i]["CrAmt"].ToString();
                //dtrowVoudetails["TransType"] = AObject[i]["TransType"].ToString();
                //dtrowVoudetails["vou_type"] = AObject[i]["gl_type"].ToString();
                Voudt.Rows.Add(dtrowVoudetails);
            }
            return Voudt;
        }
        public DataTable dtSubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));

            dtSubItem.Columns.Add("src_doc_no", typeof(string));
            dtSubItem.Columns.Add("src_doc_dt", typeof(string));
            dtSubItem.Columns.Add("wh_id", typeof(string));
            dtSubItem.Columns.Add("srt_avl", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();

                dtrowItemdetails["src_doc_no"] = jObject2[i]["ShipNo"].ToString();
                dtrowItemdetails["src_doc_dt"] = jObject2[i]["ShipDate"].ToString();
                dtrowItemdetails["wh_id"] = jObject2[i]["WhId"].ToString();
                dtrowItemdetails["srt_avl"] = IsNull(jObject2[i]["avl_stock"].ToString(), "0");
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public ActionResult GetAutoCompleteCustomerNameList(SalesReturnList_Model _SalesReturnList_Model)
        {

            string CustName = string.Empty;
            string CustType = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_SalesReturnList_Model.Customer_Name))
                {
                    CustName = "0";
                }
                else
                {
                    CustName = _SalesReturnList_Model.Customer_Name;
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                CustType = null;
                CustList = _SalesReturn_ISERVICES.GetCustomerList(Comp_ID, CustName, BrchID, CustType, null);
                //SuppList = _SalesReturn_ISERVICES.GetCustomerList(Comp_ID, CustName, BrchID);

                List<CustomerNameList> _CustomerNameList1 = new List<CustomerNameList>();
                foreach (var dr in CustList)
                {
                    CustomerNameList _CustomerName = new CustomerNameList();
                    _CustomerName.cust_id = dr.Key;
                    _CustomerName.cust_name = dr.Value;
                    _CustomerNameList1.Add(_CustomerName);
                }

                _SalesReturnList_Model.CustomerNameList = _CustomerNameList1;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public void GetStatusList(SalesReturnList_Model _SalesReturnList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                if (ViewBag.StatusList.Rows.Count > 0)
                {
                    foreach (DataRow data in ViewBag.StatusList.Rows)
                    {
                        Status _Statuslist = new Status();
                        _Statuslist.status_id = data["status_code"].ToString();
                        _Statuslist.status_name = data["status_name"].ToString();
                        statusLists.Add(_Statuslist);
                    }
                }

                _SalesReturnList_Model.StatusList = statusLists;

                #region Commented By Nitesh 06-04-2024 
                #endregion
                //List<Status> statusLists = new List<Status>();
                //var other = new CommonController(_Common_IServices);
                //var statusListsC = other.GetStatusList1(DocumentMenuId);
                //var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                //_SalesReturnList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<SRList> GetSalesReturnListAll(SalesReturnList_Model _SalesReturnList_Model)
        {
            try
            {
                _SalesReturnList = new List<SRList>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if (_SalesReturnList_Model.WF_status != null)
                {
                    wfstatus = _SalesReturnList_Model.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet ds = _SalesReturn_ISERVICES.GetSalesReturnListAll(_SalesReturnList_Model.cust_id, _SalesReturnList_Model.SRFromDate, _SalesReturnList_Model.SRToDate, _SalesReturnList_Model.Status, CompID, BrchID, wfstatus, UserID, DocumentMenuId);
                dt = ds.Tables[0];
                if (ds.Tables[1].Rows.Count > 0)
                {
                    //FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        SRList _SRList = new SRList();
                        _SRList.SRTNumber = dr["srt_no"].ToString();
                        _SRList.SRTDate = dr["srt_dt"].ToString();
                        _SRList.hdSRTDate = dr["srt_date"].ToString();
                        _SRList.list_src_type = dr["src_type"].ToString();
                        _SRList.inv_no = dr["inv_no"].ToString();
                        _SRList.CustomerName = dr["cust_name"].ToString();
                        _SRList.SRT_value = dr["srt_value"].ToString();
                        _SRList.SRTStatus = dr["srt_status"].ToString();
                        _SRList.CreatedON = dr["created_on"].ToString();
                        _SRList.ApprovedOn = dr["app_dt"].ToString();
                        _SRList.ModifiedOn = dr["mod_dt"].ToString();
                        _SRList.create_by = dr["create_by"].ToString();
                        _SRList.app_by = dr["app_by"].ToString();
                        _SRList.mod_by = dr["mod_by"].ToString();

                        _SalesReturnList.Add(_SRList);
                    }
                }
                return _SalesReturnList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchSalesReturnDetail(string CustId, string Fromdate, string Todate, string Status)
        {
            _SalesReturnList = new List<SRList>();
            SalesReturnList_Model _SalesReturnList_Model = new SalesReturnList_Model();
            //Session.Remove("WF_status");
            _SalesReturnList_Model.WF_status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            //DataSet dt = new DataSet();
            DataSet ds = _SalesReturn_ISERVICES.GetSalesReturnListAll(CustId, Fromdate, Todate, Status, CompID, BrchID, "", "", "");
            //Session["SRSearch"] = "SR_Search";
            _SalesReturnList_Model.SRSearch = "SR_Search";
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                //FromDate = dt.Rows[0]["finstrdate"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    SRList _TempSRList = new SRList();
                    _TempSRList.SRTNumber = dr["srt_no"].ToString();
                    _TempSRList.SRTDate = dr["srt_dt"].ToString();
                    _TempSRList.hdSRTDate = dr["srt_date"].ToString();
                    _TempSRList.list_src_type = dr["src_type"].ToString();
                    _TempSRList.inv_no = dr["inv_no"].ToString();
                    _TempSRList.CustomerName = dr["cust_name"].ToString();
                    _TempSRList.SRT_value = dr["srt_value"].ToString();
                    _TempSRList.SRTStatus = dr["srt_status"].ToString();
                    _TempSRList.CreatedON = dr["created_on"].ToString();
                    _TempSRList.ApprovedOn = dr["app_dt"].ToString();
                    _TempSRList.ModifiedOn = dr["mod_dt"].ToString();
                    _TempSRList.create_by = dr["create_by"].ToString();
                    _TempSRList.app_by = dr["app_by"].ToString();
                    _TempSRList.mod_by = dr["mod_by"].ToString();
                    _SalesReturnList.Add(_TempSRList);
                }
            }
            _SalesReturnList_Model.SalesReturnList = _SalesReturnList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesReturnList.cshtml", _SalesReturnList_Model);
        }
        private ActionResult SalesReturnDelete(SalesReturn_Model _SalesReturn_Model, string command)
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
                DataSet Message = _SalesReturn_ISERVICES.SalesReturnDelete(_SalesReturn_Model, CompID, BrchID);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["TRFNo"] = "";
                //_SalesReturn_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";

                return RedirectToAction("SalesReturnDetail", "SalesReturn");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult SalesReturnApprove(SalesReturn_Model _SalesApprove_Models, string SRTNo, string SRTDate, string A_Status, string A_Level, string A_Remarks, string CnNarr, string ListFilterData1, string WF_status1, string docid, string Src_Type, string JVNurr)
        {
            try
            {
                //SalesReturn_Model _SalesApprove_Models = new SalesReturn_Model();
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
                if (docid != null)
                {
                    MenuDocId = docid;
                }

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _SalesReturn_ISERVICES.SalesReturnApprove(SRTNo, SRTDate, UserID, A_Status, A_Level, A_Remarks, CnNarr, Comp_ID, BranchID, mac_id, DocumentMenuId, JVNurr);
                //Session["TransType"] = "Update";
                _SalesApprove_Models.TransType = "Update";
                //Session["Command"] = command;
                string SalesReturnNo = Message.Split(',')[0].Trim();
                string SalesReturnDate = Message.Split(',')[1].Trim();
                //Session["SalesReturnNo"] = SalesReturnNo;
                //Session["SalesReturnDate"] = SalesReturnDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                try
                {
                    DateTime parsedSalesReturnDate = DateTime.Parse(SalesReturnDate);
                    //string fileName = "SR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "SalesReturn_CreditNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_SalesApprove_Models, SalesReturnNo, parsedSalesReturnDate, Src_Type, fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, SRTNo, "AP", Session["UserId"].ToString(), "", filePath);
                }
                catch (Exception exMail)
                {
                    _SalesApprove_Models.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }

                _SalesApprove_Models.SalesReturnNo = SalesReturnNo;
                _SalesApprove_Models.SalesReturnDate = SalesReturnDate;
                // _SalesApprove_Models.Message = "Approved";
                _SalesApprove_Models.Message = _SalesApprove_Models.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                _SalesApprove_Models.AppStatus = "D";
                _SalesApprove_Models.BtnName = "BtnEdit";
                UrlModel _urlapprove = new UrlModel();
                _urlapprove.SRN = SalesReturnNo;
                _urlapprove.SRD = SalesReturnDate;
                if (WF_status1 != null && WF_status1 != "")
                {
                    _SalesApprove_Models.WF_status1 = WF_status1;
                    _urlapprove.WFS1 = WF_status1;
                }
                _urlapprove.Trp = "Update";
                _urlapprove.APS = "D";
                _urlapprove.BtnName = "BtnEdit";
                TempData["ModelData"] = _SalesApprove_Models;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("SalesReturnDetail", "SalesReturn", _urlapprove);
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
            SalesReturn_Model _SalesReturn_Model = new SalesReturn_Model();
            var a = TrancType.Split(',');
            _SalesReturn_Model.SalesReturnNo = a[0].Trim();
            _SalesReturn_Model.SalesReturnDate = a[1].Trim();
            _SalesReturn_Model.DocumentMenuId = a[2].Trim();
            _SalesReturn_Model.TransType = "Update";
            var wf_status = a[3].Trim();

            _SalesReturn_Model.BtnName = "BtnToDetailPage";
            _SalesReturn_Model.Message = Mailerror;
            UrlModel URLModel = new UrlModel();
            URLModel.SRN = _SalesReturn_Model.SalesReturnNo;
            URLModel.SRD = _SalesReturn_Model.SalesReturnDate;
            URLModel.Trp = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.Docid = _SalesReturn_Model.DocumentMenuId;
            if (wf_status != null && wf_status != "")
            {
                _SalesReturn_Model.WF_status1 = a[3].Trim();
                URLModel.WFS1 = _SalesReturn_Model.WF_status1;
            }

            TempData["ModelData"] = _SalesReturn_Model;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("SalesReturnDetail", URLModel);
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
                DataSet GlDt = _SalesReturn_ISERVICES.GetAllGLDetails(DtblGLDetail);
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
            return Validate;
        }

        [HttpPost]
        public JsonResult GetRoundOffGLDetails()
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {

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
                DataSet GlDt = _SalesReturn_ISERVICES.GetRoundOffGLDetails(Comp_ID, BranchID);
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
            return Validate;
        }
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
, string Flag, string Status, string ShNo, string ShDt, string Doc_no, string Doc_dt, string wh_id, string WhType, string SinvNo, string src_type)
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
                //if (Flag == "IssueQty")
                //{
                string Flag2 = "";
                if (Flag == "ReturnAvlQty")
                {
                    Flag2 = "AvlQty";
                    Flag = "SlsReturn";
                }
                if (Status == "D" || Status == "F" || Status == "")
                {
                    var flag = "";
                    if (WhType == "RJ")
                    {
                        flag = "whrej";
                    }
                    else if (WhType == "RW")
                    {
                        flag = "whrew";
                    }
                    else
                    {
                        flag = "wh";
                    }
                    if (Flag == "SRTReturnQty" || Flag == "AdSRTReturnQty")
                    {
                        //dt = _SalesReturn_ISERVICES.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, flag, ShNo, ShDt).Tables[0];

                        //dt.Columns.Add("Qty", typeof(string));

                        dt = _SalesReturn_ISERVICES.SR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "BeforeApprove", SinvNo, src_type).Tables[0];
                        //SubItemPopupDt subitmModel1 = new SubItemPopupDt
                        //{ ShowStock = "Y"
                        //        };
                    }
                    else if (Flag == "Shipped")
                    {
                        if (Status == "" || Status == "D" || Status == "F" || Status == "A" || Status == "C")
                        {
                            dt = _SalesReturn_ISERVICES.Shipment_GetSubItemDetails(CompID, BrchID, ShNo, Item_id, Doc_no, Doc_dt, Flag, src_type).Tables[0];
                        }
                    }
                    else
                    {
                        if (Flag == "SlsReturn")
                            dt = _SalesReturn_ISERVICES.SR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "BeforeApprove", SinvNo, src_type).Tables[0];
                        else
                            dt = _SalesReturn_ISERVICES.GetSubItemDetailsFromSinv(CompID, BrchID, SinvNo, ShNo, ShDt, Item_id, src_type).Tables[0];
                    }
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
                    //string flag = "N";
                    int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        //flag = "N";
                        foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                            }
                        }
                    }
                }
                else
                {
                    var flag = "";
                    if (WhType == "RJ")
                    {
                        flag = "whrej";
                    }
                    else if (WhType == "RW")
                    {
                        flag = "whrew";
                    }
                    else
                    {
                        flag = "wh";
                    }
                    if (Flag == "SRTReturnQty" || Flag == "AdSRTReturnQty")
                    {
                        dt = _SalesReturn_ISERVICES.SRT_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, flag, ShNo, ShDt, wh_id).Tables[0];
                    }
                    else if (Flag == "Shipped")
                    {
                        if (Status == "" || Status == "D" || Status == "F" || Status == "A" || Status == "C")
                        {
                            dt = _SalesReturn_ISERVICES.Shipment_GetSubItemDetails(CompID, BrchID, ShNo, Item_id, Doc_no, Doc_dt, Flag, src_type).Tables[0];
                        }

                    }
                    else
                    {
                        if (Flag == "SlsReturn")
                            dt = _SalesReturn_ISERVICES.SR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "AfterApprove", SinvNo, src_type).Tables[0];
                        else
                            dt = _SalesReturn_ISERVICES.GetSubItemDetailsFromSinv(CompID, BrchID, SinvNo, ShNo, ShDt, Item_id, src_type).Tables[0];
                    }
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,

                    //ShowStock = "Y",
                    ShowStock = Flag == "SRTReturnQty" ? "Y" : "N",
                    _subitemPageName = Flag2,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y",
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
        //public ActionResult GetSubItemDetails(string Item_id,string ShipNo, string SubItemListwithPageData, string IsDisabled
        // , string Flag, string Status, string Doc_no, string Doc_dt,string src_doc_no)
        // {
        //     try
        //     {
        //         if (Session["CompId"] != null)
        //         {
        //             CompID = Session["CompId"].ToString();
        //         }
        //         if (Session["BranchId"] != null)
        //         {
        //             BrchID = Session["BranchId"].ToString();
        //         }
        //         string Flag2 = "";
        //         DataTable dt = new DataTable();
        //         int QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
        //         if (Flag == "Quantity"||Flag == "ReturnAvlQty")
        //         {
        //             if(Flag== "ReturnAvlQty")
        //             {
        //                 Flag2 = "AvlQty";
        //             }

        //             Flag = "SlsReturn";
        //             if (Status == "D" || Status == "F" || Status == "")
        //             {
        //                 //dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
        //                 dt = _SalesReturn_ISERVICES.SR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "BeforeApprove", src_doc_no).Tables[0];
        //                 JArray arr = JArray.Parse(SubItemListwithPageData);
        //                 for (var i = 0; i < dt.Rows.Count; i++)
        //                 {
        //                     foreach (JObject item in arr.Children())//
        //                     {
        //                         if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
        //                         {
        //                             //dt.Rows[i]["Qty"] = item.GetValue("qty");
        //                             dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));

        //                         }
        //                     }
        //                 }
        //             }
        //             else
        //             {
        //                 dt = _SalesReturn_ISERVICES.SR_GetSubItemDetails(CompID, BrchID, Item_id,Doc_no, Doc_dt, "AfterApprove", src_doc_no).Tables[0];
        //             }

        //         }
        //         if (Flag == "Shipped")
        //         {
        //             if (Status == ""|| Status == "D" || Status == "F" || Status == "A" || Status == "C")
        //             {
        //                 dt = _SalesReturn_ISERVICES.Shipment_GetSubItemDetails(CompID, BrchID, ShipNo, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
        //             }
        //             //else
        //             //{
        //             //    dt = _SalesReturn_ISERVICES.ShipmntSaleRtrn_GetSubItemDetails(CompID, BrchID, ShipNo, Item_id, Doc_no, Doc_dt).Tables[0];
        //             //}
        //         }
        //         SubItemPopupDt subitmModel = new SubItemPopupDt
        //         {
        //             Flag = Flag,
        //             _subitemPageName = Flag2,
        //             dt_SubItemDetails = dt,
        //             IsDisabled = IsDisabled,
        //             decimalAllowed = "Y"

        //         };


        //         return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
        //     }
        //     catch (Exception Ex)
        //     {
        //         string path = Server.MapPath("~");
        //         Errorlog.LogError(path, Ex);
        //         return View("~/Views/Shared/Error.cshtml");
        //     }
        // }


        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }

        public List<TransListModel> GetTransporterList(string compId)
        {

            List<TransListModel> transporterList = new List<TransListModel>();
            transporterList.Add(new TransListModel { TransId = "0", TransName = "---Select---" });
            DataTable dt = GetTransDetails(compId, "0", "0", "0");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    transporterList.Add(new TransListModel { TransId = dr["trans_id"].ToString(), TransName = dr["trans_name"].ToString() });
                }
            }
            return transporterList;
        }
        public DataTable GetTransDetails(string compId, string transId, string transType, string transMode)
        {
            return _SalesReturn_ISERVICES.GetTransportDetails(compId, transId, transType, transMode);
        }
        /*------------- Added by Nidhi 03-07-2025 11:44 ----------*/
        public string SavePdfDocToSendOnEmailAlert(SalesReturn_Model _SalesReturn_Model, string SRTNo, DateTime SRTDate, string SrcType, string fileName,string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {
                DataTable dt = new DataTable();
                dt = PrintFormatDataTable(_SalesReturn_Model);
                ViewBag.PrintOption = dt;
                var commonCont = new CommonController(_Common_IServices);
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(SRTNo, SRTDate, SrcType);
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
        /*-------------- END  03-07-2025 11:44 ---------------*/
        public string SavePdfDocToSendOnEmailAlert_Ext(string SRTNo, DateTime SRTDate, string Src_Type,string fileName, string PrintFormat)
        {
            var printOptionsList = JsonConvert.DeserializeObject<List<SalesReturn_Model>>(PrintFormat);
            SalesReturn_Model model = new SalesReturn_Model();

            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            dt.Columns.Add("showInvHeading", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));

            DataRow dtr = dt.NewRow();
            if(PrintFormat == "")
            {
                dtr["PrintFormat"] = model.PrintFormat;
                dtr["ShowProdDesc"] = model.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = model.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = model.ShowProdTechDesc;
                dtr["ShowSubItem"] = model.ShowSubItem;
                dtr["CustAliasName"] = model.CustomerAliasName;
                dtr["ShowTotalQty"] = model.ShowTotalQty;
                dtr["ShowWithoutSybbol"] = model.ShowWithoutSybbol;
                dtr["showInvHeading"] = model.showInvHeading;
                dtr["PrintRemarks"] = model.PrintRemarks;
            }
            else
            {
                dtr["PrintFormat"] = printOptionsList[0].PrintFormat;
                dtr["ShowProdDesc"] = printOptionsList[0].ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = printOptionsList[0].ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = printOptionsList[0].ShowProdTechDesc;
                dtr["ShowSubItem"] = printOptionsList[0].ShowSubItem;
                dtr["CustAliasName"] = printOptionsList[0].CustomerAliasName;
                dtr["ShowTotalQty"] = printOptionsList[0].ShowTotalQty;
                dtr["ShowWithoutSybbol"] = printOptionsList[0].ShowWithoutSybbol;
                dtr["showInvHeading"] = printOptionsList[0].showInvHeading;
                dtr["PrintRemarks"] = printOptionsList[0].PrintRemarks;
            }
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            var commonCont = new CommonController(_Common_IServices);
            var data = GetPdfData(SRTNo, SRTDate, Src_Type);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        public ActionResult SendEmailAlert(SalesReturn_Model _model, string mail_id, string status, string docid,string Doc_no, DateTime Doc_dt, string statusAM, string filepath, string srcType)
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
                            string fileName = "SR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            filepath = SavePdfDocToSendOnEmailAlert_Ext(Doc_no, Doc_dt, srcType, fileName, "");
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
        private object IsBlank(string input, object output)//Added by Suraj Maurya on 27-11-2025
        {
            return input == "" ? output : input;
        }
    }

}