using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.PurchaseReturn;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.PurchaseReturn;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.ComponentModel;
using EnRepMobileWeb.MODELS.Common;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.PurchaseReturn
{
    public class PurchaseReturnController : Controller
    {
        string CompID, language, BrchID, title, UserID, FromDate = String.Empty;
        string DocumentMenuId = "105102150";
        List<PRList> _PurchaseReturnList;
        DataTable dt;        
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        PurchaseReturn_Model _PurchaseReturn_Model;        
        PurchaseReturn_ISERVICES _PurchaseReturn_ISERVICES;
        CommonController cmn = new CommonController();
        public PurchaseReturnController(Common_IServices _Common_IServices, PurchaseReturn_ISERVICES _PurchaseReturn_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._PurchaseReturn_ISERVICES = _PurchaseReturn_ISERVICES;
        }
        // GET: ApplicationLayer/PurchaseReturn
        public ActionResult PurchaseReturn(PurchaseReturnList_Model _PurchaseReturnList_Model)
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
                //PurchaseReturnList_Model _PurchaseReturnList_Model = new PurchaseReturnList_Model();
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                GetStatusList(_PurchaseReturnList_Model);
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    _PurchaseReturnList_Model.supp_id = a[0].Trim();
                    _PurchaseReturnList_Model.PRFromDate = a[1].Trim();
                    _PurchaseReturnList_Model.PRToDate = a[2].Trim();
                    _PurchaseReturnList_Model.Status = a[3].Trim();
                    if (_PurchaseReturnList_Model.Status == "0")
                    {
                        _PurchaseReturnList_Model.Status = null;
                    }

                    _PurchaseReturnList_Model.FromDate = _PurchaseReturnList_Model.PRFromDate;
                    _PurchaseReturnList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                }
                else
                {
                    _PurchaseReturnList_Model.FromDate = startDate;
                    _PurchaseReturnList_Model.PRFromDate = startDate;
                    _PurchaseReturnList_Model.PRToDate = CurrentDate;
                }
                GetAllData(_PurchaseReturnList_Model);
                //string endDate = dtnow.ToString("yyyy-MM-dd");
                //_PurchaseReturnList_Model.FromDate = startDate;
                ViewBag.MenuPageName = getDocumentName();
                ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                ViewBag.DocumentMenuId = DocumentMenuId;               
                _PurchaseReturnList_Model.Title = title;
                //Session["DNSearch"] = "0";
                _PurchaseReturnList_Model.DNSearch = "0";
                ViewBag.VBRoleList = GetRoleList();
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/PurchaseReturn/PurchaseReturnList.cshtml", _PurchaseReturnList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(PurchaseReturnList_Model _PurchaseReturnList_Model)
        {
            string wfstatus = string.Empty;
            string Spp_Name = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_PurchaseReturnList_Model.Spp_Name))
                {
                    Spp_Name = "0";
                }
                else
                {
                    Spp_Name = _PurchaseReturnList_Model.Spp_Name;
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (_PurchaseReturnList_Model.WF_status != null)
                {
                    wfstatus = _PurchaseReturnList_Model.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
               
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }




                //   SuppList = _PurchaseReturn_ISERVICES.AutoGetSupplierListALl(Comp_ID, Spp_Name, BrchID);
                DataSet  Data = _PurchaseReturn_ISERVICES.GetAllData(Comp_ID, Spp_Name, BrchID,
                  _PurchaseReturnList_Model.supp_id, _PurchaseReturnList_Model.PRFromDate, _PurchaseReturnList_Model.PRToDate, _PurchaseReturnList_Model.Status, wfstatus, UserID, DocumentMenuId);

                List<SupplierNameList> _SupplierNameList1 = new List<SupplierNameList>();
                foreach (DataRow dr in Data.Tables[0].Rows)
                {
                    SupplierNameList _SupplierName = new SupplierNameList();
                    _SupplierName.supp_id = dr["supp_id"].ToString();
                    _SupplierName.supp_name = dr["supp_name"].ToString();
                    _SupplierNameList1.Add(_SupplierName);
                }
                _SupplierNameList1.Insert(0,new SupplierNameList() { supp_id="0",supp_name="All"});
                _PurchaseReturnList_Model.SupplierNameList = _SupplierNameList1;

                SetAllData(Data, _PurchaseReturnList_Model);
               // _PurchaseReturnList_Model.PurchaseReturnList = GetPurchaseReturnListAll(_PurchaseReturnList_Model);
                // GetAutoCompleteSupplierNameList(_PurchaseReturnList_Model);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void SetAllData(DataSet ds, PurchaseReturnList_Model _PurchaseReturnList_Model)
        {
            List<PRList> _PurchaseReturnList = new List<PRList>();
            dt = ds.Tables[1];
            if (ds.Tables[2].Rows.Count > 0)
            {
                //FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
            }
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    PRList _PRList = new PRList();
                    _PRList.PRTNumber = dr["prt_no"].ToString();
                    _PRList.PRTDate = dr["prt_dt"].ToString();
                    _PRList.hdPRTDate = dr["prt_date"].ToString();
                    _PRList.SrcType = dr["src_type"].ToString();
                    _PRList.inv_no = dr["inv_no"].ToString();
                    _PRList.inv_date = dr["inv_dt"].ToString();
                    _PRList.SupplierName = dr["supp_name"].ToString();
                    _PRList.PRT_value = dr["prt_value"].ToString();
                    _PRList.PRTStatus = dr["prt_status"].ToString();
                    _PRList.CreatedON = dr["created_on"].ToString();
                    _PRList.ApprovedOn = dr["app_dt"].ToString();
                    _PRList.ModifiedOn = dr["mod_dt"].ToString();
                    _PRList.create_by = dr["create_by"].ToString();
                    _PRList.app_by = dr["app_by"].ToString();
                    _PRList.mod_by = dr["mod_by"].ToString();
                    _PurchaseReturnList.Add(_PRList);
                }
            }
            _PurchaseReturnList_Model.PurchaseReturnList = _PurchaseReturnList;
        }
        public ActionResult GetPurchaseReturnList(string docid, string status)
        {
            PurchaseReturnList_Model dashbord =new  PurchaseReturnList_Model();
            //Session["WF_status"] = status;
            dashbord.WF_status = status;
            return RedirectToAction("PurchaseReturn", dashbord);
        }
        public ActionResult AddPurchaseReturnDetail()
        {
            PurchaseReturn_Model _PurchaseReturn_Model = new PurchaseReturn_Model();
            _PurchaseReturn_Model.Message = "New";
            _PurchaseReturn_Model.Command = "Add";
            _PurchaseReturn_Model.AppStatus = "D";
            _PurchaseReturn_Model.TransType = "Save";
            _PurchaseReturn_Model.BtnName = "BtnAddNew";
            TempData["ModelData"] = _PurchaseReturn_Model;
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            ViewBag.MenuPageName = getDocumentName();
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("PurchaseReturn");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("PurchaseReturnDetail", "PurchaseReturn");
        }
        public ActionResult PurchaseReturnDetail(UrlModel _urlModel)
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
            /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.PurchaseReturnDate) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                CommonPageDetails();
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;
                var PR_ModelData = TempData["ModelData"] as PurchaseReturn_Model;
                if (PR_ModelData != null)
                {
                    //ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                    ViewBag.DocumentMenuId = DocumentMenuId;
                   
                    //PurchaseReturn_Model PR_ModelData = new PurchaseReturn_Model();
                    GetAutoCompleteSupplierName(PR_ModelData);
                    PR_ModelData.prt_dt = DateTime.Now;
                    //ViewBag.MenuPageName = getDocumentName();

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.inv_no = "---Select---";
                    _DocumentNumber.inv_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    PR_ModelData.DocumentNumberList = _DocumentNumberList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        PR_ModelData.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if(PR_ModelData.WF_status1 != null)
                    {

                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (PR_ModelData.TransType == "Update" || PR_ModelData.TransType == "Edit")
                    {

                        //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        BrchID = Session["BranchId"].ToString();
                        //string Prt_no = Session["PurchaseReturnNo"].ToString();
                        string Prt_no = PR_ModelData.PurchaseReturnNo;
                        //string Prt_dt = Session["PurchaseReturnDate"].ToString();
                        string Prt_dt = PR_ModelData.PurchaseReturnDate;
                        DataSet ds = _PurchaseReturn_ISERVICES.GetPurchaseReturnDetail(Prt_no, Prt_dt, CompID, BrchID, UserID, DocumentMenuId);
                        PR_ModelData.prt_no = ds.Tables[0].Rows[0]["prt_no"].ToString();
                        PR_ModelData.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        PR_ModelData.bill_dt = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        PR_ModelData.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        PR_ModelData.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        PR_ModelData.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                        PR_ModelData.prt_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["prt_dt"].ToString());
                        PR_ModelData.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        //PR_ModelData.src_doc_no = ds.Tables[0].Rows[0]["inv_no"].ToString();

                        PR_ModelData.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        PR_ModelData.SupplierID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        PR_ModelData.inv_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["inv_value"]).ToString(ValDigit);
                        PR_ModelData.prt_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["prt_value"]).ToString(ValDigit);
                        PR_ModelData.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        PR_ModelData.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        PR_ModelData.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        PR_ModelData.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        PR_ModelData.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        PR_ModelData.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        PR_ModelData.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        PR_ModelData.prt_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        PR_ModelData.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        PR_ModelData.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        PR_ModelData.OcAmt = ds.Tables[0].Rows[0]["oc_amt"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            PR_ModelData.RoundOffFlag = true;
                        }
                        else
                        {
                            PR_ModelData.RoundOffFlag = false;
                        }

                        //PR_ModelData.VouType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                        //PR_ModelData.VouNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                        //PR_ModelData.VouDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();

                        ViewBag.ItemTaxDetails = ds.Tables[11];
                        ViewBag.ItemTaxDetailsList = ds.Tables[12];
                        ViewBag.ItemStockBatchWise= ds.Tables[13];
                        ViewBag.OtherChargeDetails = ds.Tables[14];
                        ViewBag.OCTaxDetails = ds.Tables[15];

                        //if (PR_ModelData.Src_Type == "H")
                        //{
                        PR_ModelData.AdHocBill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            PR_ModelData.AdHocBill_dt = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        //}
                        //else
                        //{
                            PR_ModelData.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["inv_dt"].ToString());
                        //}
                        getWarehouse(PR_ModelData);

                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        PR_ModelData.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        PR_ModelData.DocumentStatus = Statuscode;
                        if (PR_ModelData.Status == "C")
                        {
                            PR_ModelData.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString().Trim();
                            PR_ModelData.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            PR_ModelData.BtnName = "Refresh";
                        }
                        else
                        {
                            PR_ModelData.CancelFlag = false;
                        }
                        PR_ModelData.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        PR_ModelData.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && PR_ModelData.Command != "Edit")
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
                                    PR_ModelData.BtnName = "Refresh";
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
                                        PR_ModelData.BtnName = "BtnToDetailPage";
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
                                        PR_ModelData.BtnName = "BtnToDetailPage";
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
                                    PR_ModelData.BtnName = "BtnToDetailPage";
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
                                        PR_ModelData.BtnName = "BtnToDetailPage";
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
                                    PR_ModelData.BtnName = "BtnToDetailPage";
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
                                    PR_ModelData.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    PR_ModelData.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    PR_ModelData.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        PR_ModelData.DocumentNumberList = GetPurchaseReturnSourceDocument(PR_ModelData);
                        PR_ModelData.src_doc_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        DocumentNumber objDocumentNumber = new DocumentNumber();
                        objDocumentNumber.inv_no = PR_ModelData.src_doc_no;
                        objDocumentNumber.inv_dt = PR_ModelData.src_doc_no;
                        if (!PR_ModelData.DocumentNumberList.Contains(objDocumentNumber))
                        {
                            PR_ModelData.DocumentNumberList.Add(objDocumentNumber);
                        }
                        //ViewBag.MenuPageName = getDocumentName();
                        PR_ModelData.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemLotBatchSerialDetails = ds.Tables[2];
                        ViewBag.CostCenterData = ds.Tables[10];
                        //ViewBag.VoucherDetails = ds.Tables[6];
                        ViewBag.GLAccount = ds.Tables[6];
                        ViewBag.hdGLDetails = ds.Tables[7];
                        ViewBag.VoucherTotal = ds.Tables[8];
                        PR_ModelData.PRItemBatchSerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        ViewBag.SubItemDetails = ds.Tables[9];
                        
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/PurchaseReturn/PurchaseReturn.cshtml", PR_ModelData);
                    }
                    else
                    {

                        PR_ModelData.Title = title;
                        //Session["DocumentStatus"] = "New";
                        PR_ModelData.DocumentStatus = "New";
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/PurchaseReturn/PurchaseReturn.cshtml", PR_ModelData);
                    }
                }
                else
                {/*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
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

                    PurchaseReturn_Model _PurchaseReturn_Model = new PurchaseReturn_Model();
                    if (_urlModel.PurchaseReturnNo != null && _urlModel.PurchaseReturnNo != "")
                    {
                        _PurchaseReturn_Model.PurchaseReturnNo = _urlModel.PurchaseReturnNo;
                        _PurchaseReturn_Model.PurchaseReturnDate = _urlModel.PurchaseReturnDate;
                    }
                    if (_urlModel.PurchaseReturnDate != null && _urlModel.PurchaseReturnDate != "")
                    {                      
                        _PurchaseReturn_Model.PurchaseReturnDate = _urlModel.PurchaseReturnDate;
                    }
                    if (_urlModel.TransType != null)
                    {
                        _PurchaseReturn_Model.TransType = _urlModel.TransType;
                    }
                    else
                    {
                        _PurchaseReturn_Model.TransType = "New";
                    }
                    if (_urlModel.BtnName != null)
                    {
                        _PurchaseReturn_Model.BtnName = _urlModel.BtnName;
                    }
                    else
                    {
                        _PurchaseReturn_Model.BtnName = "Refresh";
                    }
                    if (_urlModel.Command != null)
                    {
                        _PurchaseReturn_Model.Command = _urlModel.Command;
                    }
                    else
                    {
                        _PurchaseReturn_Model.Command = "Refresh";
                    }
                    if (_urlModel.WF_status1 != null)
                    {
                        _PurchaseReturn_Model.WF_status1 = _urlModel.WF_status1;
                    }
                    GetAutoCompleteSupplierName(_PurchaseReturn_Model);
                    _PurchaseReturn_Model.prt_dt = DateTime.Now;
                    //ViewBag.MenuPageName = getDocumentName();

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.inv_no = "---Select---";
                    _DocumentNumber.inv_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _PurchaseReturn_Model.DocumentNumberList = _DocumentNumberList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _PurchaseReturn_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_PurchaseReturn_Model.TransType == "Update" || _PurchaseReturn_Model.TransType == "Edit")
                    {

                        //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        BrchID = Session["BranchId"].ToString();
                        //string Prt_no = Session["PurchaseReturnNo"].ToString();
                        string Prt_no = _PurchaseReturn_Model.PurchaseReturnNo;
                        //string Prt_dt = Session["PurchaseReturnDate"].ToString();
                        string Prt_dt = _PurchaseReturn_Model.PurchaseReturnDate;
                        DataSet ds = _PurchaseReturn_ISERVICES.GetPurchaseReturnDetail(Prt_no, Prt_dt, CompID, BrchID, UserID, DocumentMenuId);
                        _PurchaseReturn_Model.prt_no = ds.Tables[0].Rows[0]["prt_no"].ToString();
                        _PurchaseReturn_Model.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _PurchaseReturn_Model.bill_dt = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        _PurchaseReturn_Model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _PurchaseReturn_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _PurchaseReturn_Model.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                        _PurchaseReturn_Model.prt_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["prt_dt"].ToString());
                        _PurchaseReturn_Model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        //_PurchaseReturn_Model.src_doc_no = ds.Tables[0].Rows[0]["inv_no"].ToString();

                        _PurchaseReturn_Model.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _PurchaseReturn_Model.inv_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["inv_value"]).ToString(ValDigit);
                        _PurchaseReturn_Model.prt_value = Convert.ToDecimal(ds.Tables[0].Rows[0]["prt_value"]).ToString(ValDigit);
                        _PurchaseReturn_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _PurchaseReturn_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _PurchaseReturn_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _PurchaseReturn_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _PurchaseReturn_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _PurchaseReturn_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _PurchaseReturn_Model.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _PurchaseReturn_Model.prt_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _PurchaseReturn_Model.Src_Type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _PurchaseReturn_Model.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        _PurchaseReturn_Model.OcAmt = ds.Tables[0].Rows[0]["oc_amt"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            _PurchaseReturn_Model.RoundOffFlag = true;
                        }
                        else
                        {
                            _PurchaseReturn_Model.RoundOffFlag = false;
                        }

                        //_PurchaseReturn_Model.VouType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                        //_PurchaseReturn_Model.VouNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                        //_PurchaseReturn_Model.VouDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();

                        ViewBag.ItemTaxDetails = ds.Tables[11];
                        ViewBag.ItemTaxDetailsList = ds.Tables[12];
                        ViewBag.ItemStockBatchWise = ds.Tables[13];
                        ViewBag.OtherChargeDetails = ds.Tables[14];
                        ViewBag.OCTaxDetails = ds.Tables[15];
                        //if (_PurchaseReturn_Model.Src_Type == "H")
                        //{
                        _PurchaseReturn_Model.AdHocBill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _PurchaseReturn_Model.AdHocBill_dt = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        //}
                        //else
                        //{
                            _PurchaseReturn_Model.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["inv_dt"].ToString());
                        //}
                        getWarehouse(_PurchaseReturn_Model);

                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        _PurchaseReturn_Model.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _PurchaseReturn_Model.DocumentStatus = Statuscode;
                        if (_PurchaseReturn_Model.Status == "C")
                        {
                            _PurchaseReturn_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString().Trim();
                            _PurchaseReturn_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _PurchaseReturn_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _PurchaseReturn_Model.CancelFlag = false;
                        }
                        _PurchaseReturn_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _PurchaseReturn_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _PurchaseReturn_Model.Command != "Edit")
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
                                    _PurchaseReturn_Model.BtnName = "Refresh";
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
                                        _PurchaseReturn_Model.BtnName = "BtnToDetailPage";
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
                                        _PurchaseReturn_Model.BtnName = "BtnToDetailPage";
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
                                    _PurchaseReturn_Model.BtnName = "BtnToDetailPage";
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
                                        _PurchaseReturn_Model.BtnName = "BtnToDetailPage";
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
                                    _PurchaseReturn_Model.BtnName = "BtnToDetailPage";
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
                                    _PurchaseReturn_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseReturn_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _PurchaseReturn_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _PurchaseReturn_Model.DocumentNumberList = GetPurchaseReturnSourceDocument(_PurchaseReturn_Model);
                        _PurchaseReturn_Model.src_doc_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        DocumentNumber objDocumentNumber = new DocumentNumber();
                        objDocumentNumber.inv_no = _PurchaseReturn_Model.src_doc_no;
                        objDocumentNumber.inv_dt = _PurchaseReturn_Model.src_doc_no;
                        if (!_PurchaseReturn_Model.DocumentNumberList.Contains(objDocumentNumber))
                        {
                            _PurchaseReturn_Model.DocumentNumberList.Add(objDocumentNumber);
                        }
                        //ViewBag.MenuPageName = getDocumentName();
                        _PurchaseReturn_Model.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemLotBatchSerialDetails = ds.Tables[2];
                        ViewBag.CostCenterData = ds.Tables[10];
                        //ViewBag.VoucherDetails = ds.Tables[6];
                        ViewBag.GLAccount = ds.Tables[6];
                        ViewBag.hdGLDetails = ds.Tables[7];
                        ViewBag.VoucherTotal = ds.Tables[8];
                        _PurchaseReturn_Model.PRItemBatchSerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        //ViewBag.VBRoleList = GetRoleList();
                        ViewBag.SubItemDetails = ds.Tables[9];
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/PurchaseReturn/PurchaseReturn.cshtml", _PurchaseReturn_Model);
                    }
                    else
                    {
                        _PurchaseReturn_Model.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _PurchaseReturn_Model.DocumentStatus = "New";
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/PurchaseReturn/PurchaseReturn.cshtml", _PurchaseReturn_Model);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [NonAction]
        private void getWarehouse(PurchaseReturn_Model _PurchaseReturn_Model)
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
                _PurchaseReturn_Model.WarehouseList = _WarehouseList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

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
        public ActionResult EditPurchaseReturn(string PRTNo, string Prt_Date, string ListFilterData,string WF_status)
        {
            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
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
            
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["PurchaseReturnNo"] = PRTNo;
            //Session["PurchaseReturnDate"] = Prt_Date;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            PurchaseReturn_Model _DoubleClick = new PurchaseReturn_Model();
            _DoubleClick.Message = "New";
            _DoubleClick.Command = "Add";
            _DoubleClick.AppStatus = "D";
            _DoubleClick.TransType = "Update";
            _DoubleClick.BtnName = "BtnToDetailPage";
            _DoubleClick.PurchaseReturnNo = PRTNo;
            _DoubleClick.PurchaseReturnDate = Prt_Date;
            _DoubleClick.WF_status1 = WF_status;
            TempData["ModelData"] = _DoubleClick;
            UrlModel _urlModel = new UrlModel();
            _urlModel.TransType = "Update";
            _urlModel.BtnName = "BtnToDetailPage";
            _urlModel.PurchaseReturnNo = PRTNo;
            _urlModel.PurchaseReturnDate = Prt_Date;
            _urlModel.WF_status1 = WF_status;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("PurchaseReturnDetail", "PurchaseReturn", _urlModel);
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
        public ActionResult GetAutoCompleteSupplierName(PurchaseReturn_Model _PurchaseReturn_Model)
        {

            string Spp_Name = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_PurchaseReturn_Model.SupplierName))
                {
                    Spp_Name = "0";
                }
                else
                {
                    Spp_Name = _PurchaseReturn_Model.SupplierName;
                }
                BrchID = Session["BranchId"].ToString();
                SuppList = _PurchaseReturn_ISERVICES.AutoGetSupplierListALl(Comp_ID, Spp_Name, BrchID);

                List<SupplierName> _SupplierNameList = new List<SupplierName>();
                foreach (var dr in SuppList)
                {
                    SupplierName _SupplierName = new SupplierName();
                    _SupplierName.supp_id = dr.Key;
                    _SupplierName.supp_name = dr.Value;
                    _SupplierNameList.Add(_SupplierName);
                }
                _PurchaseReturn_Model.SupplierNameList = _SupplierNameList;
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
        public JsonResult GetPurchaseReturnSourceDocumentNoList(PurchaseReturn_Model _PurchaseReturn_Model)
        {
            try
            {
                JsonResult DataRows = null;
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Spp_ID = string.Empty;
                string Src_Type = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                Spp_ID = _PurchaseReturn_Model.SupplierID;
                Src_Type = _PurchaseReturn_Model.Src_Type;
                DocumentNumber = _PurchaseReturn_Model.DocumentNo;
                string BrchID = Session["BranchId"].ToString();
                DocumentNumberList = _PurchaseReturn_ISERVICES.GetPurchaseInvoiceNo(CompID, BrchID, Spp_ID, DocumentNumber, Src_Type);
                foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                {
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.inv_no = dr["inv_no"].ToString();
                    _DocumentNumber.inv_dt = dr["inv_dt"].ToString();
                    _DocumentNumberList.Add(_DocumentNumber);
                }
                DataRows = Json(JsonConvert.SerializeObject(DocumentNumberList), JsonRequestBehavior.AllowGet);

                return DataRows;
                // return Json(_DocumentNumberList.Select(c => new { Name = c.inv_no, ID = c.inv_dt }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;// View("~/Views/Shared/Error.cshtml");
            }


        }
        public List<DocumentNumber> GetPurchaseReturnSourceDocument(PurchaseReturn_Model _PurchaseReturn_Model)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Spp_ID = string.Empty;
                string Src_Type = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                Spp_ID = _PurchaseReturn_Model.SupplierID;
                Spp_ID = _PurchaseReturn_Model.supp_id;
                Src_Type = _PurchaseReturn_Model.Src_Type;
                DocumentNumber = _PurchaseReturn_Model.DocumentNo;
                string BrchID = Session["BranchId"].ToString();
                DocumentNumberList = _PurchaseReturn_ISERVICES.GetPurchaseInvoiceNo(CompID, BrchID, Spp_ID, DocumentNumber, Src_Type);
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
        public ActionResult GetPIItemDetail(string SourDocumentNo,string src_type)
        {
            try
            {
                JsonResult DataRows = null;
                _PurchaseReturn_Model = new PurchaseReturn_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _PurchaseReturn_ISERVICES.GetPIItemDetail(CompID, BrchID, SourDocumentNo, src_type);

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
        public ActionResult GetGRNItemDetail(string ItemID, string GRNNumber, string SrcDocNumber, string RT_Status,string src_type)
        {
            try
            {


                JsonResult DataRows = null;
                _PurchaseReturn_Model = new PurchaseReturn_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _PurchaseReturn_ISERVICES.GetGRNItemDetail(CompID, BrchID, ItemID, GRNNumber, SrcDocNumber, RT_Status, src_type);

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
        public ActionResult GetInvoiceItemDetail(string ItemID, string InvoiceNo,string GRNNumber,string src_type)
        {
            try
            {
                JsonResult DataRows = null;
                _PurchaseReturn_Model = new PurchaseReturn_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _PurchaseReturn_ISERVICES.GetInvoiceItemDetail(CompID, BrchID, ItemID, InvoiceNo, GRNNumber,src_type);

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PurchaseReturnSave(PurchaseReturn_Model _PurchaseReturn_Model, string prt_no, string command)
        {
            try
            {/*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/

                if (_PurchaseReturn_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        PurchaseReturn_Model _PRaddnew = new PurchaseReturn_Model();
                        _PRaddnew.Command = "Add";
                        _PRaddnew.TransType = "Save";
                        _PRaddnew.AppStatus = "D";
                        _PRaddnew.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _PRaddnew;
                        UrlModel _urlModeladd = new UrlModel();
                        _urlModeladd.TransType = "Save";
                        _urlModeladd.BtnName = "BtnAddNew";
                        _urlModeladd.Command = "Add";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                              if (!string.IsNullOrEmpty(_PurchaseReturn_Model.prt_no))
                                return RedirectToAction("EditPurchaseReturn", new { PRTNo = _PurchaseReturn_Model.prt_no, Prt_Date = _PurchaseReturn_Model.prt_dt, ListFilterData = _PurchaseReturn_Model.ListFilterData1, WF_status = _PurchaseReturn_Model.WFStatus });
                            else
                                _PRaddnew.Command = "Refresh";
                            _PRaddnew.TransType = "Refresh";
                            _PRaddnew.BtnName = "Refresh";
                            _PRaddnew.DocumentStatus = null;
                            TempData["ModelData"] = _PRaddnew;
                            return RedirectToAction("PurchaseReturnDetail", "PurchaseReturn", _PRaddnew);
                        }
                        /*End to chk Financial year exist or not*/

                        return RedirectToAction("PurchaseReturnDetail", "PurchaseReturn", _urlModeladd);

                    case "Edit":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPurchaseReturn", new { PRTNo = _PurchaseReturn_Model.prt_no, Prt_Date = _PurchaseReturn_Model.prt_dt, ListFilterData = _PurchaseReturn_Model.ListFilterData1, WF_status = _PurchaseReturn_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string prtdt = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, prtdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPurchaseReturn", new { PRTNo = _PurchaseReturn_Model.prt_no, Prt_Date = _PurchaseReturn_Model.prt_dt, ListFilterData = _PurchaseReturn_Model.ListFilterData1, WF_status = _PurchaseReturn_Model.WFStatus });
                        }
                        _PurchaseReturn_Model.Command = command;
                        _PurchaseReturn_Model.TransType = "Update";
                        _PurchaseReturn_Model.AppStatus = "D";
                        _PurchaseReturn_Model.BtnName = "BtnEdit";
                        _PurchaseReturn_Model.PurchaseReturnNo = _PurchaseReturn_Model.prt_no;
                        _PurchaseReturn_Model.PurchaseReturnDate = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        TempData["ModelData"] = _PurchaseReturn_Model;
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.Command = command;
                        _urlModel.TransType = "Update";
                        _urlModel.AppStatus = "D";
                        _urlModel.BtnName = "BtnEdit";
                        _urlModel.PurchaseReturnNo = _PurchaseReturn_Model.prt_no;
                        _urlModel.PurchaseReturnDate = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        TempData["ListFilterData"] = _PurchaseReturn_Model.ListFilterData1;
                        return RedirectToAction("PurchaseReturnDetail", _urlModel);

                    case "Delete":
                        _PurchaseReturn_Model.Command = command;
                        prt_no = _PurchaseReturn_Model.prt_no;
                        PurchaseReturnDelete(_PurchaseReturn_Model, command);
                        PurchaseReturn_Model _PRDelete = new PurchaseReturn_Model();
                        _PRDelete.Message = "Deleted";
                        _PRDelete.Command = "Refresh";
                        _PRDelete.TransType = "Refresh";
                        _PRDelete.AppStatus = "DL";
                        _PRDelete.BtnName = "BtnDelete";
                        TempData["ModelData"] = _PRDelete;
                        TempData["ListFilterData"] = _PurchaseReturn_Model.ListFilterData1;
                        return RedirectToAction("PurchaseReturnDetail");


                    case "Save":
                        //Session["Command"] = command;
                        _PurchaseReturn_Model.Command = command;
                        SavePurchaseReturn(_PurchaseReturn_Model);
                        if (_PurchaseReturn_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_PurchaseReturn_Model.Message == "DocModify")
                        {
                            _PurchaseReturn_Model.Message = "DocModify";

                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";

                            //var other = new CommonController(_Common_IServices);
                            //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                            CommonPageDetails();
                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _PurchaseReturn_Model.SupplierID, supp_name = _PurchaseReturn_Model.SupplierName });
                            _PurchaseReturn_Model.SupplierNameList = suppLists;

                            List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.inv_no = _PurchaseReturn_Model.src_doc_no;
                            _DocumentNumber.inv_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);
                            _PurchaseReturn_Model.DocumentNumberList = _DocumentNumberList;
                            _PurchaseReturn_Model.prt_dt = DateTime.Now;
                            _PurchaseReturn_Model.src_doc_no = _PurchaseReturn_Model.src_doc_no;
                            _PurchaseReturn_Model.src_doc_date = _PurchaseReturn_Model.src_doc_date;
                            _PurchaseReturn_Model.SupplierName = _PurchaseReturn_Model.SupplierName;
                           
                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            //ViewBag.hdGLDetails = ViewData["BatchSerialDetails"];
                            ViewBag.VoucherDetails = ViewData["VouDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _PurchaseReturn_Model.BtnName = "Refresh";
                            _PurchaseReturn_Model.Command = "Refresh";
                            _PurchaseReturn_Model.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/PurchaseReturn/PurchaseReturn.cshtml", _PurchaseReturn_Model);

                        }
                        else
                        {
                            TempData["ModelData"] = _PurchaseReturn_Model;
                            UrlModel _urlModelSave = new UrlModel();
                            _urlModelSave.Command = _PurchaseReturn_Model.Command;
                            _urlModelSave.TransType = _PurchaseReturn_Model.TransType;
                            _urlModelSave.AppStatus = "D";
                            _urlModelSave.BtnName = _PurchaseReturn_Model.BtnName;
                            _urlModelSave.PurchaseReturnNo = _PurchaseReturn_Model.PurchaseReturnNo;
                            _urlModelSave.PurchaseReturnDate = _PurchaseReturn_Model.PurchaseReturnDate;
                            TempData["ListFilterData"] = _PurchaseReturn_Model.ListFilterData1;
                            return RedirectToAction("PurchaseReturnDetail", _urlModelSave);
                        }

                    case "Forward":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPurchaseReturn", new { PRTNo = _PurchaseReturn_Model.prt_no, Prt_Date = _PurchaseReturn_Model.prt_dt, ListFilterData = _PurchaseReturn_Model.ListFilterData1, WF_status = _PurchaseReturn_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string prtdt1 = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, prtdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPurchaseReturn", new { PRTNo = _PurchaseReturn_Model.prt_no, Prt_Date = _PurchaseReturn_Model.prt_dt, ListFilterData = _PurchaseReturn_Model.ListFilterData1, WF_status = _PurchaseReturn_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPurchaseReturn", new { PRTNo = _PurchaseReturn_Model.prt_no, Prt_Date = _PurchaseReturn_Model.prt_dt, ListFilterData = _PurchaseReturn_Model.ListFilterData1, WF_status = _PurchaseReturn_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string prtdt2 = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, prtdt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPurchaseReturn", new { PRTNo = _PurchaseReturn_Model.prt_no, Prt_Date = _PurchaseReturn_Model.prt_dt, ListFilterData = _PurchaseReturn_Model.ListFilterData1, WF_status = _PurchaseReturn_Model.WFStatus });
                        }
                        _PurchaseReturn_Model.Command = command;
                        prt_no = _PurchaseReturn_Model.prt_no;
                        _PurchaseReturn_Model.PurchaseReturnNo = prt_no;
                        _PurchaseReturn_Model.PurchaseReturnDate = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        string Prt_dt = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        PurchaseReturnApprove( _PurchaseReturn_Model,prt_no, Prt_dt,"Approve","","", _PurchaseReturn_Model.DnNarr,"","","", _PurchaseReturn_Model.Src_Type);
                        _PurchaseReturn_Model.Command = command;
                        _PurchaseReturn_Model.PurchaseReturnNo = _PurchaseReturn_Model.prt_no;
                        _PurchaseReturn_Model.PurchaseReturnDate = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                        _PurchaseReturn_Model.TransType = "Update";
                        _PurchaseReturn_Model.AppStatus = "A";
                        _PurchaseReturn_Model.WF_status1 = _PurchaseReturn_Model.WF_status1;
                        _PurchaseReturn_Model.BtnName = "BtnApprove";
                        TempData["ModelData"] = _PurchaseReturn_Model;
                        UrlModel _urlModelApprove = new UrlModel();
                        _urlModelApprove.Command = _PurchaseReturn_Model.Command;
                        _urlModelApprove.TransType = _PurchaseReturn_Model.TransType;                       
                        _urlModelApprove.BtnName = _PurchaseReturn_Model.BtnName;
                        _urlModelApprove.PurchaseReturnNo = _PurchaseReturn_Model.PurchaseReturnNo;
                        _urlModelApprove.PurchaseReturnDate = _PurchaseReturn_Model.PurchaseReturnDate;
                        //_urlModelApprove.WF_status1 = _PurchaseReturn_Model.WF_status1;
                        TempData["ListFilterData"] = _PurchaseReturn_Model.ListFilterData1;
                        return RedirectToAction("PurchaseReturnDetail", _urlModelApprove);
                    case "Refresh":
                        PurchaseReturn_Model _PR_RefreshModel = new PurchaseReturn_Model();
                        _PR_RefreshModel.Message = null;
                        _PR_RefreshModel.Command = command;
                        _PR_RefreshModel.TransType = "Save";
                        _PR_RefreshModel.BtnName = "Refresh";
                        TempData["ModelData"] = _PR_RefreshModel;
                        TempData["ListFilterData"] = _PurchaseReturn_Model.ListFilterData1;
                        return RedirectToAction("PurchaseReturnDetail");
                    case "Print":
                        return GenratePdfFile(_PurchaseReturn_Model);
                    case "BacktoList":
                        PurchaseReturnList_Model _PurchaseReturnList_Model = new PurchaseReturnList_Model();
                        _PurchaseReturnList_Model.WF_status = _PurchaseReturn_Model.WF_status1;
                        TempData["ListFilterData"] = _PurchaseReturn_Model.ListFilterData1;
                        return RedirectToAction("PurchaseReturn", "PurchaseReturn", _PurchaseReturnList_Model);
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
        public ActionResult SavePurchaseReturn(PurchaseReturn_Model _PurchaseReturn_Model)
        {
            try
            {
                if (_PurchaseReturn_Model.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    var commonCont = new CommonController(_Common_IServices);
                    DataTable PurchaseReturnHeader = new DataTable();
                    DataTable PurchaseReturnItemDetails = new DataTable();
                    DataTable PurchaseReturnLotBatchSerial = new DataTable();
                    DataTable PurchaseReturnVoudetail = new DataTable();
                    DataTable PRCostCenterDetails = new DataTable();
                    DataTable DtblTaxDetail = new DataTable();
                    DataTable DtblOCDetail = new DataTable();
                    DataTable DtblOCTaxDetail = new DataTable();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MenuDocumentId", typeof(string));
                    dt.Columns.Add("comp_id", typeof(int));
                    dt.Columns.Add("br_id", typeof(int));
                    dt.Columns.Add("prt_no", typeof(string));
                    dt.Columns.Add("prt_dt", typeof(string));
                    dt.Columns.Add("inv_no", typeof(string));
                    dt.Columns.Add("inv_dt", typeof(string));
                    dt.Columns.Add("supp_id", typeof(int));
                    dt.Columns.Add("curr_id", typeof(int));
                    dt.Columns.Add("conv_rate", typeof(string));
                    dt.Columns.Add("inv_value", typeof(string));
                    dt.Columns.Add("prt_value", typeof(string));
                    dt.Columns.Add("CreateBy", typeof(string));
                    dt.Columns.Add("prt_status", typeof(string));
                    dt.Columns.Add("UserMacaddress", typeof(string));
                    dt.Columns.Add("UserSystemName", typeof(string));
                    dt.Columns.Add("UserIP", typeof(string));
                    dt.Columns.Add("TransType", typeof(string));
                    dt.Columns.Add("roundoff", typeof(string));
                    dt.Columns.Add("pm_fla", typeof(string));
                   


                    DataRow dtrow = dt.NewRow();

                    dtrow["MenuDocumentId"] = DocumentMenuId;
                    dtrow["comp_id"] = Session["CompId"].ToString();
                    dtrow["br_id"] = Session["BranchId"].ToString();
                    dtrow["prt_no"] = _PurchaseReturn_Model.prt_no;
                    //dtrow["prt_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                    dtrow["prt_dt"] = _PurchaseReturn_Model.prt_dt;
                    if (_PurchaseReturn_Model.Src_Type == "H")
                    {
                        dtrow["inv_no"] = _PurchaseReturn_Model.AdHocBill_no;
                        dtrow["inv_dt"] = _PurchaseReturn_Model.AdHocBill_dt;
                    }
                    else
                    {
                        dtrow["inv_no"] = _PurchaseReturn_Model.src_doc_no;
                        dtrow["inv_dt"] = _PurchaseReturn_Model.src_doc_date;
                    }
                    if (_PurchaseReturn_Model.SupplierID == null)
                    {
                        dtrow["supp_id"] = DBNull.Value;
                    }
                    else
                    {
                        dtrow["supp_id"] = _PurchaseReturn_Model.SupplierID;
                    }
                    dtrow["curr_id"] = 1;
                    dtrow["conv_rate"] = 1;
                    dtrow["inv_value"] = _PurchaseReturn_Model.inv_value;
                    dtrow["prt_value"] = _PurchaseReturn_Model.prt_value;
                    dtrow["CreateBy"] = Session["UserId"].ToString();
                    //dtrow["prt_status"] = Session["AppStatus"].ToString();
                    dtrow["prt_status"] = IsNull(_PurchaseReturn_Model.AppStatus, "D");
                    dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrow["UserIP"] = Session["UserIP"].ToString();
                    //dtrow["TransType"] = Session["TransType"].ToString();
                    if(_PurchaseReturn_Model.prt_no != null)
                    {
                        dtrow["TransType"] = "Update";

                    }
                    else
                    {
                        dtrow["TransType"] = "Save";
                    }
                    string roundoffflag = _PurchaseReturn_Model.RoundOffFlag.ToString();
                    if (roundoffflag == "False")
                    {
                        dtrow["roundoff"] = "N";
                    }
                    else
                    {
                        dtrow["roundoff"] = "Y";
                    }
                    dtrow["pm_fla"] = _PurchaseReturn_Model.pmflagval;
                  
                    dt.Rows.Add(dtrow);

                    PurchaseReturnHeader = dt;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(Int32));
                    dtItem.Columns.Add("br_id", typeof(Int32));
                    dtItem.Columns.Add("mr_no", typeof(string));
                    dtItem.Columns.Add("mr_date", typeof(string));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("grn_qty", typeof(string));
                    dtItem.Columns.Add("return_qty", typeof(string));
                    dtItem.Columns.Add("inv_value", typeof(string));
                    dtItem.Columns.Add("prt_value", typeof(string));
                    dtItem.Columns.Add("reason", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("item_acc_id", typeof(string));
                    dtItem.Columns.Add("prt_avl", typeof(string));
                    dtItem.Columns.Add("item_rate", typeof(string));
                    dtItem.Columns.Add("item_tax_amt", typeof(string));
                    dtItem.Columns.Add("wh_id", typeof(string));


                    JArray jObject = JArray.Parse(_PurchaseReturn_Model.PRItemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal grn_qty, return_qty;
                        if (jObject[i]["GRNQuantity"].ToString() == "")
                            grn_qty = 0;
                        else
                            grn_qty = Convert.ToDecimal(jObject[i]["GRNQuantity"].ToString());

                        if (jObject[i]["ReturnQuantity"].ToString() == "")
                            return_qty = 0;
                        else
                            return_qty = Convert.ToDecimal(jObject[i]["ReturnQuantity"].ToString());

                        DataRow dtrowItemdetails = dtItem.NewRow();
                        dtrowItemdetails["comp_id"] = Session["CompId"].ToString();
                        dtrowItemdetails["br_id"] = Session["BranchId"].ToString();
                        dtrowItemdetails["mr_no"] = jObject[i]["GRNNo"].ToString();
                        dtrowItemdetails["mr_date"] = jObject[i]["GRNDate"].ToString();
                        dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                        string str = Convert.ToInt32(jObject[i]["UOMId"]).ToString();
                        dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMId"]);
                        dtrowItemdetails["grn_qty"] = grn_qty;
                        dtrowItemdetails["return_qty"] = return_qty;
                        dtrowItemdetails["inv_value"] = jObject[i]["InvoiceValue"];
                        dtrowItemdetails["prt_value"] = jObject[i]["ReturnValue"];
                        dtrowItemdetails["reason"] = jObject[i]["ReasonForReturn"].ToString();
                        dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                        dtrowItemdetails["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                        dtrowItemdetails["prt_avl"] = jObject[i]["prt_avl"].ToString();
                        dtrowItemdetails["item_rate"] = jObject[i]["item_rate"].ToString();
                        dtrowItemdetails["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                        dtrowItemdetails["wh_id"] = jObject[i]["wh_id"].ToString();

                        dtItem.Rows.Add(dtrowItemdetails);
                    }
                    PurchaseReturnItemDetails = dtItem;
                    ViewData["ItemDetails"] = dtitemdetail(jObject);

                    DataTable dtItemBatchSerial = new DataTable();
                    dtItemBatchSerial.Columns.Add("comp_id", typeof(Int32));
                    dtItemBatchSerial.Columns.Add("br_id", typeof(Int32));
                    dtItemBatchSerial.Columns.Add("item_id", typeof(string));
                    dtItemBatchSerial.Columns.Add("uom_id", typeof(int));
                    dtItemBatchSerial.Columns.Add("lot_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("serial_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("batch_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("prt_qty", typeof(string));
                    dtItemBatchSerial.Columns.Add("wh_id", typeof(int));
                    dtItemBatchSerial.Columns.Add("Batchable", typeof(string));
                    dtItemBatchSerial.Columns.Add("Serialable", typeof(string));
                    dtItemBatchSerial.Columns.Add("mr_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("mr_date", typeof(string));
                    dtItemBatchSerial.Columns.Add("BatchAvlStock", typeof(string));
                    dtItemBatchSerial.Columns.Add("expiry_date", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_name", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_mrp", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_date", typeof(string));

                    DtblTaxDetail = ToDtblTaxDetail(_PurchaseReturn_Model.TaxDetail, "Y");

                    if (_PurchaseReturn_Model.Src_Type == "H")
                    {
                        //DataTable Batch_detail = new DataTable();
                        //Batch_detail.Columns.Add("item_id", typeof(string));
                        //Batch_detail.Columns.Add("lot_no", typeof(string));
                        //Batch_detail.Columns.Add("batch_no", typeof(string));
                        //Batch_detail.Columns.Add("uom_id", typeof(int));
                        //Batch_detail.Columns.Add("ship_qty", typeof(float));
                        //Batch_detail.Columns.Add("avl_batch_qty", typeof(float));
                        //Batch_detail.Columns.Add("expiry_date", typeof(string));
                        //Batch_detail.Columns.Add("issue_qty", typeof(string));
                        //Batch_detail.Columns.Add("Batchable", typeof(string));
                        //Batch_detail.Columns.Add("Serialable", typeof(string));
                        if (_PurchaseReturn_Model.ItemBatchWiseDetail != null)
                        {
                            JArray jObjectBatch = JArray.Parse(_PurchaseReturn_Model.ItemBatchWiseDetail);
                            for (int i = 0; i < jObjectBatch.Count; i++)
                            {
                                DataRow dtrowBatchDetailsLines = dtItemBatchSerial.NewRow();
                                dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                                dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                                dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                                dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                                dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                                dtrowBatchDetailsLines["serial_no"] = jObjectBatch[i]["BatchNo"].ToString();
                                dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                                dtrowBatchDetailsLines["prt_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                                dtrowBatchDetailsLines["wh_id"] = jObjectBatch[i]["wh_id"].ToString();
                                dtrowBatchDetailsLines["Batchable"] = jObjectBatch[i]["batchable"].ToString();
                                dtrowBatchDetailsLines["Serialable"] = jObjectBatch[i]["Seriable"].ToString();
                                dtrowBatchDetailsLines["mr_no"] = "";
                                dtrowBatchDetailsLines["mr_date"] = "";
                                dtrowBatchDetailsLines["BatchAvlStock"] = jObjectBatch[i]["BatchAvlStock"].ToString();
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                                dtrowBatchDetailsLines["mfg_name"] = commonCont.IsBlank(jObjectBatch[i]["mfg_name"].ToString(),null);
                                dtrowBatchDetailsLines["mfg_mrp"] = commonCont.IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(),null);
                                dtrowBatchDetailsLines["mfg_date"] = commonCont.IsBlank(jObjectBatch[i]["mfg_date"].ToString(),null);
                                dtItemBatchSerial.Rows.Add(dtrowBatchDetailsLines);
                            }
                            //ViewData["BatchDetails"] = dtbatchdetail(jObjectBatch);
                        }
                        PurchaseReturnLotBatchSerial = dtItemBatchSerial;
                       
                        //DataTable Serial_detail = new DataTable();
                        //Serial_detail.Columns.Add("item_id", typeof(string));
                        //Serial_detail.Columns.Add("uom_id", typeof(int));
                        //Serial_detail.Columns.Add("ship_qty", typeof(string));
                        //Serial_detail.Columns.Add("lot_no", typeof(string));
                        //Serial_detail.Columns.Add("serial_no", typeof(string));
                        //Serial_detail.Columns.Add("issue_qty", typeof(string));
                        //Serial_detail.Columns.Add("Batchable", typeof(string));
                        //Serial_detail.Columns.Add("Serialable", typeof(string));

                        //if (_PurchaseReturn_Model.ItemSerialWiseDetail != null)
                        //{
                        //    JArray jObjectSerial = JArray.Parse(_PurchaseReturn_Model.ItemSerialWiseDetail);
                        //    for (int i = 0; i < jObjectSerial.Count; i++)
                        //    {
                        //        DataRow dtrowSerialDetailsLines = dtItemBatchSerial.NewRow();
                        //        dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                        //        dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();
                        //        dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                        //        dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                        //        dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                        //        dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                        //        dtrowSerialDetailsLines["batch_no"] = "";
                        //        dtrowSerialDetailsLines["prt_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                        //        dtrowSerialDetailsLines["wh_id"] = jObjectSerial[i]["wh_id"].ToString();
                        //        dtrowSerialDetailsLines["Batchable"] = jObjectSerial[i]["batchable"].ToString();
                        //        dtrowSerialDetailsLines["Serialable"] = jObjectSerial[i]["Seriable"].ToString();
                        //        dtrowSerialDetailsLines["mr_no"] = "";
                        //        dtrowSerialDetailsLines["mr_date"] = "";
                        //        dtrowSerialDetailsLines["BatchAvlStock"] = jObjectSerial[i]["BatchAvlStock"].ToString();
                        //        dtrowSerialDetailsLines["expiry_date"] = jObjectSerial[i]["ExpiryDate"].ToString();
                        //        dtItemBatchSerial.Rows.Add(dtrowSerialDetailsLines);
                        //    }
                        //    //ViewData["SerialDetails"] = dtSerialdetail(jObjectSerial);
                        //    PurchaseReturnLotBatchSerial = dtItemBatchSerial;
                        //}
                    }
                    else
                    {
                        if (_PurchaseReturn_Model.PRItemBatchSerialDetail != null)
                        {
                            JArray jObject1 = JArray.Parse(_PurchaseReturn_Model.PRItemBatchSerialDetail);
                            for (int i = 0; i < jObject1.Count; i++)
                            {
                                decimal prt_qty;
                                if (jObject1[i]["RetQty"].ToString() == "")
                                    prt_qty = 0;
                                else
                                    prt_qty = Convert.ToDecimal(jObject1[i]["RetQty"].ToString());

                                DataRow dtrowItemBatchSerialdetails = dtItemBatchSerial.NewRow();
                                dtrowItemBatchSerialdetails["comp_id"] = Session["CompId"].ToString();
                                dtrowItemBatchSerialdetails["br_id"] = Session["BranchId"].ToString();
                                dtrowItemBatchSerialdetails["item_id"] = jObject1[i]["ItmCode"].ToString();
                                string str = Convert.ToInt32(jObject1[i]["ItmUomId"]).ToString();
                                dtrowItemBatchSerialdetails["uom_id"] = Convert.ToInt32(jObject1[i]["ItmUomId"]);
                                dtrowItemBatchSerialdetails["lot_no"] = jObject1[i]["Lot"].ToString();
                                dtrowItemBatchSerialdetails["serial_no"] = jObject1[i]["Serial"].ToString();
                                dtrowItemBatchSerialdetails["batch_no"] = jObject1[i]["Batch"].ToString();
                                dtrowItemBatchSerialdetails["prt_qty"] = prt_qty;
                                dtrowItemBatchSerialdetails["wh_id"] = jObject1[i]["wh_id"].ToString();
                                dtrowItemBatchSerialdetails["Batchable"] = jObject1[i]["Batchable"].ToString();
                                dtrowItemBatchSerialdetails["Serialable"] = jObject1[i]["Serialable"].ToString();
                                dtrowItemBatchSerialdetails["mr_no"] = jObject1[i]["IconGRNNumber"].ToString();
                                dtrowItemBatchSerialdetails["mr_date"] = jObject1[i]["IconGRNDate"].ToString();
                                dtrowItemBatchSerialdetails["BatchAvlStock"] = "";
                                dtrowItemBatchSerialdetails["expiry_date"] = "";
                                dtrowItemBatchSerialdetails["mfg_name"] = commonCont.IsBlank(jObject1[i]["mfg_name"].ToString(), null);
                                dtrowItemBatchSerialdetails["mfg_mrp"] = commonCont.IsBlank(jObject1[i]["mfg_mrp"].ToString(), null);
                                dtrowItemBatchSerialdetails["mfg_date"] = commonCont.IsBlank(jObject1[i]["mfg_date"].ToString(), null);

                                dtItemBatchSerial.Rows.Add(dtrowItemBatchSerialdetails);
                            }
                            ViewData["BatchSerialDetails"] = dtbatchserialdetail(jObject1);
                        }
                        PurchaseReturnLotBatchSerial = dtItemBatchSerial;
                    }
                    

                    //DataTable Voudt = new DataTable();
                    //Voudt.Columns.Add("comp_id", typeof(int));
                    //Voudt.Columns.Add("accid", typeof(string));
                    //Voudt.Columns.Add("type", typeof(string));
                    //Voudt.Columns.Add("doctype", typeof(string));
                    //Voudt.Columns.Add("value", typeof(string));
                    //Voudt.Columns.Add("dramt", typeof(string));
                    //Voudt.Columns.Add("cramt", typeof(string));
                    //Voudt.Columns.Add("Transtype", typeof(string));
                    //Voudt.Columns.Add("GlType", typeof(string));


                    //JArray AObject = JArray.Parse(_PurchaseReturn_Model.PRVoudetails);
                    //for (int i = 0; i < AObject.Count; i++)
                    //{

                    //    DataRow dtrowVoudetails = Voudt.NewRow();
                    //    dtrowVoudetails["comp_id"] = Session["CompId"].ToString();
                    //    dtrowVoudetails["accid"] = AObject[i]["accid"].ToString();
                    //    dtrowVoudetails["type"] = AObject[i]["type"].ToString();
                    //    dtrowVoudetails["doctype"] = AObject[i]["doctype"].ToString();
                    //    dtrowVoudetails["value"] = AObject[i]["Value"].ToString();
                    //    dtrowVoudetails["dramt"] = AObject[i]["DrAmt"].ToString();
                    //    dtrowVoudetails["cramt"] = AObject[i]["CrAmt"].ToString();
                    //    dtrowVoudetails["TransType"] = AObject[i]["TransType"].ToString();
                    //    dtrowVoudetails["GlType"] = AObject[i]["gl_type"].ToString();

                    //    Voudt.Rows.Add(dtrowVoudetails);
                    //}
                    PurchaseReturnVoudetail = ToDtblVouDetail(_PurchaseReturn_Model.PRVoudetails);

                    /**----------------Cost Center Section--------------------*/
                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("vou_sr_no", typeof(string));
                    CC_Details.Columns.Add("gl_sr_no", typeof(string));
                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));

                    if (_PurchaseReturn_Model.CC_DetailList != null)
                    {
                        JArray JAObj = JArray.Parse(_PurchaseReturn_Model.CC_DetailList);
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
                        ViewData["CCdetail"] = dtCCdetail(JAObj);
                    }

                    PRCostCenterDetails = CC_Details;
                    /**----------------Cost Center Section End--------------------*/
                    //ViewData["VouDetails"] = dtVoudetail(AObject);

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));                  
                    dtSubItem.Columns.Add("src_doc_no", typeof(string));
                    dtSubItem.Columns.Add("src_doc_dt", typeof(string));
                    dtSubItem.Columns.Add("wh_id", typeof(string));
                    dtSubItem.Columns.Add("prt_avl", typeof(string));

                    if (_PurchaseReturn_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_PurchaseReturn_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();                            
                            dtrowItemdetails["src_doc_no"] = jObject2[i]["GRNNo"].ToString();
                            dtrowItemdetails["src_doc_dt"] = jObject2[i]["GRNDate"].ToString();
                            dtrowItemdetails["wh_id"] = jObject2[i]["WhId"].ToString();
                            dtrowItemdetails["prt_avl"] = jObject2[i]["avl_stock"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                    }
                    DtblOCDetail = ToDtblOCDetail(_PurchaseReturn_Model.ItemOCdetails);
                    DtblOCTaxDetail = ToDtblTaxDetail(_PurchaseReturn_Model.ItemOCTaxdetails);
                    /*------------------Sub Item end----------------------*/

                    String SaveMessage = _PurchaseReturn_ISERVICES.InsertPurchaseReturnDetail(PurchaseReturnHeader
                        , PurchaseReturnItemDetails, PurchaseReturnLotBatchSerial, PurchaseReturnVoudetail, dtSubItem
                        , PRCostCenterDetails, DtblTaxDetail, DtblOCDetail, DtblOCTaxDetail, _PurchaseReturn_Model.Src_Type,_PurchaseReturn_Model.AdHocBill_no,_PurchaseReturn_Model.AdHocBill_dt, IsNull(_PurchaseReturn_Model.OcAmt,"0"));
                    if (SaveMessage == "DocModify")
                    {
                        _PurchaseReturn_Model.Message = "DocModify";
                        _PurchaseReturn_Model.BtnName = "Refresh";
                        _PurchaseReturn_Model.Command = "Refresh";
                        TempData["ModelData"] = _PurchaseReturn_Model;
                        return RedirectToAction("PurchaseReturnDetail");
                    }
                    else
                    {
                        string PurchaseReturnNo = SaveMessage.Split(',')[1].Trim();
                        string Message = SaveMessage.Split(',')[0].Trim();
                        string PurchaseReturnDate = SaveMessage.Split(',')[2].Trim();

                        if (Message == "Data_Not_Found")
                        {
                            ViewBag.MenuPageName = getDocumentName();
                            _PurchaseReturn_Model.Title = title;
                            var msg = Message.Replace("_", " ") + " " + PurchaseReturnNo+" in "+ _PurchaseReturn_Model.Title;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _PurchaseReturn_Model.Message = Message.Replace("_", "");
                            return RedirectToAction("PurchaseReturnDetail");
                        }

                        if (Message == "Update" || Message == "Save")
                        {
                            _PurchaseReturn_Model.Message = "Save";
                            _PurchaseReturn_Model.Command = "Update";
                            _PurchaseReturn_Model.PurchaseReturnNo = PurchaseReturnNo;
                            _PurchaseReturn_Model.PurchaseReturnDate = PurchaseReturnDate;
                            _PurchaseReturn_Model.TransType = "Update";
                            _PurchaseReturn_Model.AppStatus = "D";
                            _PurchaseReturn_Model.BtnName = "BtnSave";
                            TempData["ModelData"] = _PurchaseReturn_Model;
                            return RedirectToAction("PurchaseReturnDetail");
                        }
                        return RedirectToAction("PurchaseReturnDetail");
                        
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
                    _PurchaseReturn_Model.CreatedBy = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage = _PurchaseReturn_ISERVICES.PurchaseReturnCancel(_PurchaseReturn_Model, CompID, br_id, mac_id);
                    try
                    {
                        string fileName = "PR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_PurchaseReturn_Model.prt_no, _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd"), fileName);
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _PurchaseReturn_Model.prt_no, "C", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _PurchaseReturn_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    //_PurchaseReturn_Model.Message = "Cancelled";
                    _PurchaseReturn_Model.Message = _PurchaseReturn_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    _PurchaseReturn_Model.Command = "Update";
                    _PurchaseReturn_Model.PurchaseReturnNo = _PurchaseReturn_Model.prt_no;
                    _PurchaseReturn_Model.PurchaseReturnDate = _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd");
                    _PurchaseReturn_Model.TransType = "Update";
                    _PurchaseReturn_Model.AppStatus = "D";
                    _PurchaseReturn_Model.BtnName = "Refresh";
                    TempData["ModelData"] = _PurchaseReturn_Model;
                    return RedirectToAction("PurchaseReturnDetail");
                }
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
                    ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
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
        public DataTable dtTaxdetail(JArray jObjectTax)
        {
            DataTable Tax_detail = new DataTable();
            //Tax_detail.Columns.Add("item_id", typeof(string));
            //Tax_detail.Columns.Add("tax_id", typeof(int));
            //Tax_detail.Columns.Add("tax_name", typeof(string));
            //Tax_detail.Columns.Add("tax_rate", typeof(string));
            //Tax_detail.Columns.Add("tax_val", typeof(string));
            //Tax_detail.Columns.Add("tax_level", typeof(int));
            //Tax_detail.Columns.Add("tax_apply_on", typeof(string));
            //Tax_detail.Columns.Add("tax_apply_Name", typeof(string));
            //Tax_detail.Columns.Add("item_tax_amt", typeof(string));

            //for (int i = 0; i < jObjectTax.Count; i++)
            //{
            //    DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();
            //    dtrowTaxDetailsLines["item_id"] = jObjectTax[i]["item_id"].ToString();
            //    dtrowTaxDetailsLines["tax_id"] = jObjectTax[i]["tax_id"].ToString();
            //    dtrowTaxDetailsLines["tax_name"] = jObjectTax[i]["TaxName"].ToString();
            //    string tax_rate = jObjectTax[i]["tax_rate"].ToString();
            //    tax_rate = tax_rate.Replace("%", "");
            //    dtrowTaxDetailsLines["tax_rate"] = tax_rate;
            //    dtrowTaxDetailsLines["tax_val"] = jObjectTax[i]["tax_val"].ToString();
            //    dtrowTaxDetailsLines["tax_level"] = jObjectTax[i]["tax_level"].ToString();
            //    dtrowTaxDetailsLines["tax_apply_on"] = jObjectTax[i]["tax_apply_on"].ToString();
            //    dtrowTaxDetailsLines["tax_apply_Name"] = jObjectTax[i]["tax_apply_onName"].ToString();
            //    dtrowTaxDetailsLines["item_tax_amt"] = jObjectTax[i]["totaltax_amt"].ToString();
            //    Tax_detail.Rows.Add(dtrowTaxDetailsLines);
            //}

            return Tax_detail;
        }
        public DataTable dtCCdetail(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));
            CC_Details.Columns.Add("cc_name", typeof(string));
            CC_Details.Columns.Add("cc_val_name", typeof(string));
            CC_Details.Columns.Add("vou_sr_no", typeof(string));
            CC_Details.Columns.Add("gl_sr_no", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();
                dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();

                CC_Details.Rows.Add(dtrowLines);
            }
            return CC_Details;
        }

        public DataTable dtitemdetail(JArray jObject)
        {
            try
            {
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("mr_no", typeof(string));
                dtItem.Columns.Add("mr_dt", typeof(string));
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("item_name", typeof(string));
                dtItem.Columns.Add("sub_item", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("uom_name", typeof(string));
                dtItem.Columns.Add("mr_qty", typeof(string));
                dtItem.Columns.Add("prt_qty", typeof(string));
                dtItem.Columns.Add("inv_val", typeof(string));
                dtItem.Columns.Add("prt_val", typeof(string));
                dtItem.Columns.Add("prt_reason", typeof(string));
                dtItem.Columns.Add("it_remarks", typeof(string));
                dtItem.Columns.Add("prt_avl", typeof(string));

                for (int i = 0; i < jObject.Count; i++)
                {
                    decimal grn_qty, return_qty;
                    if (jObject[i]["GRNQuantity"].ToString() == "")
                        grn_qty = 0;
                    else
                        grn_qty = Convert.ToDecimal(jObject[i]["GRNQuantity"].ToString());

                    if (jObject[i]["ReturnQuantity"].ToString() == "")
                        return_qty = 0;
                    else
                        return_qty = Convert.ToDecimal(jObject[i]["ReturnQuantity"].ToString());

                    DataRow dtrowItemdetails = dtItem.NewRow();

                    dtrowItemdetails["mr_no"] = jObject[i]["GRNNo"].ToString();
                    dtrowItemdetails["mr_dt"] = jObject[i]["GRNDate"].ToString();
                    dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                    dtrowItemdetails["item_name"] = jObject[i]["ItemName"].ToString();
                    dtrowItemdetails["sub_item"] = jObject[i]["subitem"].ToString();
                    dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMId"]);
                    dtrowItemdetails["uom_name"] = jObject[i]["uom_name"].ToString();
                    dtrowItemdetails["mr_qty"] = grn_qty;
                    dtrowItemdetails["prt_qty"] = return_qty;
                    dtrowItemdetails["inv_val"] = jObject[i]["InvoiceValue"];
                    dtrowItemdetails["prt_val"] = jObject[i]["ReturnValue"];
                    dtrowItemdetails["prt_reason"] = jObject[i]["ReasonForReturn"].ToString();
                    dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                    dtrowItemdetails["prt_avl"] = jObject[i]["prt_avl"].ToString();

                    dtItem.Rows.Add(dtrowItemdetails);
                }
                return dtItem;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }        
        }
        public DataTable dtbatchserialdetail(JArray jObject1)
        {
            try
            {
                DataTable dtItemBatchSerial = new DataTable();
                dtItemBatchSerial.Columns.Add("comp_id", typeof(Int32));
                dtItemBatchSerial.Columns.Add("br_id", typeof(Int32));
                dtItemBatchSerial.Columns.Add("item_id", typeof(string));
                dtItemBatchSerial.Columns.Add("uom_id", typeof(int));
                dtItemBatchSerial.Columns.Add("lot_no", typeof(string));
                dtItemBatchSerial.Columns.Add("serial_no", typeof(string));
                dtItemBatchSerial.Columns.Add("batch_no", typeof(string));
                dtItemBatchSerial.Columns.Add("prt_qty", typeof(string));
                dtItemBatchSerial.Columns.Add("wh_id", typeof(int));
                dtItemBatchSerial.Columns.Add("Batchable", typeof(string));
                dtItemBatchSerial.Columns.Add("Serialable", typeof(string));
                dtItemBatchSerial.Columns.Add("mr_no", typeof(string));
                dtItemBatchSerial.Columns.Add("mr_date", typeof(string));
                for (int i = 0; i < jObject1.Count; i++)
                {
                    decimal prt_qty;
                    if (jObject1[i]["RetQty"].ToString() == "")
                        prt_qty = 0;
                    else
                        prt_qty = Convert.ToDecimal(jObject1[i]["RetQty"].ToString());

                    DataRow dtrowItemBatchSerialdetails = dtItemBatchSerial.NewRow();
                    dtrowItemBatchSerialdetails["comp_id"] = Session["CompId"].ToString();
                    dtrowItemBatchSerialdetails["br_id"] = Session["BranchId"].ToString();
                    dtrowItemBatchSerialdetails["item_id"] = jObject1[i]["ItmCode"].ToString();
                    string str = Convert.ToInt32(jObject1[i]["ItmUomId"]).ToString();
                    dtrowItemBatchSerialdetails["uom_id"] = Convert.ToInt32(jObject1[i]["ItmUomId"]);
                    dtrowItemBatchSerialdetails["lot_no"] = jObject1[i]["Lot"].ToString();
                    dtrowItemBatchSerialdetails["serial_no"] = jObject1[i]["Serial"].ToString();
                    dtrowItemBatchSerialdetails["batch_no"] = jObject1[i]["Batch"].ToString();
                    dtrowItemBatchSerialdetails["prt_qty"] = prt_qty;
                    dtrowItemBatchSerialdetails["wh_id"] = jObject1[i]["wh_id"].ToString();
                    dtrowItemBatchSerialdetails["Batchable"] = jObject1[i]["Batchable"].ToString();
                    dtrowItemBatchSerialdetails["Serialable"] = jObject1[i]["Serialable"].ToString();
                    dtrowItemBatchSerialdetails["mr_no"] = jObject1[i]["IconGRNNumber"].ToString();
                    dtrowItemBatchSerialdetails["mr_date"] = jObject1[i]["IconGRNDate"].ToString();
                    dtItemBatchSerial.Rows.Add(dtrowItemBatchSerialdetails);
                }
                return dtItemBatchSerial;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }      
        }
        public DataTable dtVoudetail(JArray AObject)
        {
            try
            {
                DataTable Voudt = new DataTable();
                Voudt.Columns.Add("comp_id", typeof(string));
                Voudt.Columns.Add("acc_id", typeof(string));
                Voudt.Columns.Add("acc_name", typeof(string));
                Voudt.Columns.Add("type", typeof(string));
                Voudt.Columns.Add("doctype", typeof(string));
                Voudt.Columns.Add("Value", typeof(string));
                Voudt.Columns.Add("dr_amt_sp", typeof(string));
                Voudt.Columns.Add("cr_amt_sp", typeof(string));
                Voudt.Columns.Add("TransType", typeof(string));
                Voudt.Columns.Add("gl_type", typeof(string));
                for (int i = 0; i < AObject.Count; i++)
                {
                    DataRow dtrowVouDetailsLines = Voudt.NewRow();
                    dtrowVouDetailsLines["comp_id"] = AObject[i]["comp_id"].ToString();
                    dtrowVouDetailsLines["acc_id"] = AObject[i]["id"].ToString();
                    dtrowVouDetailsLines["acc_name"] = AObject[i]["acc_name"].ToString();
                    dtrowVouDetailsLines["type"] = AObject[i]["type"].ToString();
                    dtrowVouDetailsLines["doctype"] = AObject[i]["doctype"].ToString();
                    dtrowVouDetailsLines["Value"] = AObject[i]["Value"].ToString();
                    dtrowVouDetailsLines["dr_amt_sp"] = AObject[i]["DrAmt"].ToString();
                    dtrowVouDetailsLines["cr_amt_sp"] = AObject[i]["CrAmt"].ToString();
                    dtrowVouDetailsLines["TransType"] = AObject[i]["TransType"].ToString();
                    dtrowVouDetailsLines["gl_type"] = AObject[i]["Gltype"].ToString();
                    Voudt.Rows.Add(dtrowVouDetailsLines);
                }
                return Voudt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }          
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
            dtSubItem.Columns.Add("avl_stk", typeof(string));
            dtSubItem.Columns.Add("src_doc_number", typeof(string));
            dtSubItem.Columns.Add("src_doc_date", typeof(string));
            dtSubItem.Columns.Add("wh_id", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["avl_stk"] = jObject2[i]["avl_stock"].ToString();
                dtrowItemdetails["src_doc_number"] = jObject2[i]["GRNNo"].ToString();
                dtrowItemdetails["src_doc_date"] = jObject2[i]["GRNDate"].ToString();
                dtrowItemdetails["wh_id"] = jObject2[i]["WhId"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
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
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        public FileResult GenratePdfFile(PurchaseReturn_Model _Model)
        {
            return File(GetPdfData(_Model.prt_no, _Model.prt_dt.ToString("yyyy-MM-dd"),_Model.Src_Type), "application/pdf", "PurchaseReturn.pdf");
        }
        public byte[] GetPdfData(string prt_no, string prt_dt,string src_type)
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
                DataSet Deatils = _PurchaseReturn_ISERVICES.GetPurchaseReturnDeatils(CompID, BrchID, prt_no, prt_dt, src_type);
                ViewBag.PageName = "PR";
                //ViewBag.Title = "Purchase Return";/*commented and change by Hina on 27-09-2024 to change name*/
                ViewBag.Title = "Debit Note";
                ViewBag.Details = Deatils;
                ViewBag.src_type = src_type;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                
                //ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["prt_status"].ToString().Trim();
                ViewBag.Website = Deatils.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 07-04-2025*/
                string Digi_sign = path1 + Deatils.Tables[0].Rows[0]["digi_sign"].ToString().Replace("/", "\\'");
              
                ViewBag.DigiSign= Digi_sign.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/PurchaseReturn/PurchaseReturnPrint.cshtml"));
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
        public ActionResult GetAutoCompleteSupplierNameList(PurchaseReturnList_Model _PurchaseReturnList_Model)
        {

            string Spp_Name = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_PurchaseReturnList_Model.Spp_Name))
                {
                    Spp_Name = "0";
                }
                else
                {
                    Spp_Name = _PurchaseReturnList_Model.Spp_Name;
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                SuppList = _PurchaseReturn_ISERVICES.AutoGetSupplierListALl(Comp_ID, Spp_Name, BrchID);

                List<SupplierNameList> _SupplierNameList1 = new List<SupplierNameList>();
                foreach (var dr in SuppList)
                {
                    SupplierNameList _SupplierName = new SupplierNameList();
                    _SupplierName.supp_id = dr.Key;
                    _SupplierName.supp_name = dr.Value;
                    _SupplierNameList1.Add(_SupplierName);
                }
                _PurchaseReturnList_Model.SupplierNameList = _SupplierNameList1;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public void GetStatusList(PurchaseReturnList_Model _PurchaseReturnList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _PurchaseReturnList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<PRList> GetPurchaseReturnListAll(PurchaseReturnList_Model _PurchaseReturnList_Model)
        {
            try
            {
                _PurchaseReturnList = new List<PRList>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                string wfstatus = "";
                if(_PurchaseReturnList_Model.WF_status != null)
                {
                    wfstatus = _PurchaseReturnList_Model.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                //else
                //{
                //    wfstatus = "";
                //}
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet ds = _PurchaseReturn_ISERVICES.GetPurchaseReturnListAll(_PurchaseReturnList_Model.supp_id, _PurchaseReturnList_Model.PRFromDate, _PurchaseReturnList_Model.PRToDate, _PurchaseReturnList_Model.Status, CompID, BrchID, wfstatus, UserID,DocumentMenuId);
                dt = ds.Tables[0];
                if (ds.Tables[1].Rows.Count > 0)
                {
                    //FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        PRList _PRList = new PRList();
                        _PRList.PRTNumber = dr["prt_no"].ToString();
                        _PRList.PRTDate = dr["prt_dt"].ToString();
                        _PRList.hdPRTDate = dr["prt_date"].ToString();
                        _PRList.inv_no = dr["inv_no"].ToString();
                        _PRList.inv_date = dr["inv_dt"].ToString();
                        _PRList.SupplierName = dr["supp_name"].ToString();
                        _PRList.PRT_value = dr["prt_value"].ToString();
                        _PRList.PRTStatus = dr["prt_status"].ToString();
                        _PRList.CreatedON = dr["created_on"].ToString();
                        _PRList.ApprovedOn = dr["app_dt"].ToString();
                        _PRList.ModifiedOn = dr["mod_dt"].ToString();
                        _PRList.create_by = dr["create_by"].ToString();
                        _PRList.app_by = dr["app_by"].ToString();
                        _PRList.mod_by = dr["mod_by"].ToString();
                        _PurchaseReturnList.Add(_PRList);
                    }
                }
                return _PurchaseReturnList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchPurchaseReturnDetail(string SuppId, string Fromdate, string Todate, string Status)
        {
            _PurchaseReturnList = new List<PRList>();
            PurchaseReturnList_Model _PurchaseReturnList_Model = new PurchaseReturnList_Model();
            //Session.Remove("WF_status");
            _PurchaseReturnList_Model.WF_status = null;

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            //DataSet dt = new DataSet();
            dt = _PurchaseReturn_ISERVICES.GetPurchaseReturnListAll(SuppId, Fromdate, Todate, Status, CompID, BrchID,"","","").Tables[0];
            if (dt.Rows.Count > 0)
            {
                //Session["MTISearch"] = "MTI_Search";
                _PurchaseReturnList_Model.MTISearch = "MTI_Search";
                //FromDate = dt.Rows[0]["finstrdate"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    PRList _TempPRList = new PRList();
                    _TempPRList.PRTNumber = dr["prt_no"].ToString();
                    _TempPRList.PRTDate = dr["prt_dt"].ToString();
                    _TempPRList.hdPRTDate = dr["prt_date"].ToString();
                    _TempPRList.SrcType = dr["src_type"].ToString();
                    _TempPRList.inv_no = dr["inv_no"].ToString();
                    _TempPRList.inv_date = dr["inv_dt"].ToString();
                    _TempPRList.SupplierName = dr["supp_name"].ToString();
                    _TempPRList.PRT_value = dr["prt_value"].ToString();
                    _TempPRList.PRTStatus = dr["prt_status"].ToString();
                    _TempPRList.CreatedON = dr["created_on"].ToString();
                    _TempPRList.ApprovedOn = dr["app_dt"].ToString();
                    _TempPRList.ModifiedOn = dr["mod_dt"].ToString();
                    _TempPRList.create_by = dr["create_by"].ToString();
                    _TempPRList.app_by = dr["app_by"].ToString();
                    _TempPRList.mod_by = dr["mod_by"].ToString();
                    _PurchaseReturnList.Add(_TempPRList);
                }
            }
            _PurchaseReturnList_Model.PurchaseReturnList = _PurchaseReturnList;
            //Session["LSISearch"] = "LSI_Search";
            _PurchaseReturnList_Model.LSISearch = "LSI_Search";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPurchaseReturnList.cshtml", _PurchaseReturnList_Model);
        }
        private ActionResult PurchaseReturnDelete(PurchaseReturn_Model _PurchaseReturn_Model, string command)
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
                DataSet Message = _PurchaseReturn_ISERVICES.PurchaseReturnDelete(_PurchaseReturn_Model, CompID, BrchID);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["TRFNo"] = "";
                //_PurchaseReturn_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
               
                return RedirectToAction("PurchaseReturnDetail", "PurchaseReturn");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult PurchaseReturnApprove(PurchaseReturn_Model _PurchaseReturn_Model,string PRTNo, string PRTDate, string A_Status, string A_Level, string A_Remarks ,string DnNarr, string ListFilterData1, string docid, string WF_Status1,string src_type)
        {
            try
            {
               // PurchaseReturn_Model _PurchaseReturn_Model = new PurchaseReturn_Model();
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
                //if (Session["MenuDocumentId"] != null)
                if (docid != null)
                {
                    MenuDocId = docid;
                }

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Message = _PurchaseReturn_ISERVICES.PurchaseReturnApprove(PRTNo, PRTDate, UserID, A_Status, A_Level, A_Remarks, DnNarr, Comp_ID, BranchID, mac_id,  DocumentMenuId,src_type);
                //Session["TransType"] = "Update";
                _PurchaseReturn_Model.TransType = "Update";
                //string PurchaseReturnNo = Message.Split(',')[0].Trim();
                string[] FDetail = Message.Split(',');
                string PurchaseReturnNo = FDetail[0].ToString();
                UrlModel _urlModelApprove = new UrlModel();
                if (PurchaseReturnNo == "StkNotAvl")
                {
                    _PurchaseReturn_Model.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                    _PurchaseReturn_Model.Message = "StkNotAvl";
                }
                else
                {
                    string PurchaseReturnDate = Message.Split(',')[1].Trim();
                    string Msg = Message.Split(',')[2].Trim();
                    //Session["PurchaseReturnNo"] = PurchaseReturnNo;
                    //Session["PurchaseReturnDate"] = PurchaseReturnDate;
                    //Session["Message"] = "Approved";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "BtnEdit";
                    try
                    {
                        string fileName = "PR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_PurchaseReturn_Model.prt_no, _PurchaseReturn_Model.prt_dt.ToString("yyyy-MM-dd"), fileName);
                        _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, PRTNo, "AP", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _PurchaseReturn_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    if (Msg == "StkNotAvl")
                    {
                        _PurchaseReturn_Model.Message = "StkNotAvl";
                    }
                    else
                    {
                        //_PurchaseReturn_Model.Message = "Approved";
                        _PurchaseReturn_Model.Message = _PurchaseReturn_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    }

                    _PurchaseReturn_Model.PurchaseReturnNo = PurchaseReturnNo;
                    _PurchaseReturn_Model.PurchaseReturnDate = PurchaseReturnDate;
                    _PurchaseReturn_Model.TransType = "Update";
                    _PurchaseReturn_Model.AppStatus = "A";
                    _PurchaseReturn_Model.WF_status1 = WF_Status1;
                    _PurchaseReturn_Model.BtnName = "BtnApprove";
                    TempData["ModelData"] = _PurchaseReturn_Model;
                    
                    _urlModelApprove.Command = _PurchaseReturn_Model.Command;
                    _urlModelApprove.TransType = _PurchaseReturn_Model.TransType;
                    _urlModelApprove.BtnName = _PurchaseReturn_Model.BtnName;
                    _urlModelApprove.PurchaseReturnNo = PurchaseReturnNo;
                    _urlModelApprove.PurchaseReturnDate = PurchaseReturnDate;
                    _urlModelApprove.WF_status1 = _PurchaseReturn_Model.WF_status1;
                    TempData["ListFilterData"] = ListFilterData1;
                } 
                return RedirectToAction("PurchaseReturnDetail", "PurchaseReturn", _urlModelApprove);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1,string DashBord)
        {
            //Session["Message"] = "";
            PurchaseReturn_Model _PurchaseReturn_Model = new PurchaseReturn_Model();
            _PurchaseReturn_Model.Message = "";
            var a = DashBord.Split(',');
            _PurchaseReturn_Model.docid = a[0].Trim();
            _PurchaseReturn_Model.PurchaseReturnNo = a[1].Trim();
            _PurchaseReturn_Model.PurchaseReturnDate = a[2].Trim();
            _PurchaseReturn_Model.WF_status1 = a[3].Trim();
            _PurchaseReturn_Model.TransType = "Update";
            _PurchaseReturn_Model.BtnName = "BtnToDetailPage";
            _PurchaseReturn_Model.Message = null;
            TempData["ModelData"] = _PurchaseReturn_Model;
            UrlModel _UrlModel = new UrlModel();
            _UrlModel.PurchaseReturnNo = _PurchaseReturn_Model.PurchaseReturnNo;         
            _UrlModel.WF_status1 = _PurchaseReturn_Model.WF_status1;
            _UrlModel.PurchaseReturnDate = _PurchaseReturn_Model.PurchaseReturnDate;
            _UrlModel.TransType = "Update";
            _UrlModel.BtnName = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("PurchaseReturnDetail", _UrlModel);
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
                DataSet GlDt = _PurchaseReturn_ISERVICES.GetAllGLDetails(DtblGLDetail);
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
                DataSet GlDt = _PurchaseReturn_ISERVICES.GetRoundOffGLDetails(Comp_ID, BranchID);
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
        //public ActionResult GetTaxAmountDetail(string ItmCode, string InvoiceNo, string SrcNumber, string SrcDate, string ReturnQuantity, string src_type)/*Add by Hina sharma on 16-12-2024*/
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //            CompID = Session["CompId"].ToString();
        //        if (Session["BranchId"] != null)
        //            BrchID = Session["BranchId"].ToString();

        //        DataSet dt = _SalesReturn_ISERVICES.GetTaxAmountDetail(CompID, BrchID, ItmCode, InvoiceNo, SrcNumber, SrcDate, ReturnQuantity, src_type);
        //        ViewBag.TaxAmountDetail = dt.Tables[0];
        //        ViewBag.TaxAmountDetailTotal = dt.Tables[1];

        //        return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSRTaxAmountDetail.cshtml");
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //}
        /*----------------- Start Sub Item Detail Section---------------------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
 , string Flag, string Status, string GRNNo, string GRNDt, string Doc_no, string Doc_dt, string wh_id, string WhType,string PinvNo,string src_type,int UOMID,string avlstock)
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
                if (Status == "D" || Status == "F" || Status == "")
                { 
                    var flag = "";
                    if(WhType=="RJ")
                    {
                        flag = "whrej";
                    }
                    else if(WhType == "RW")
                    {
                        flag = "whrew";
                    }
                    else
                    {
                        flag = "wh";
                    }
                    if (Flag == "PRTReturnQty"|| Flag == "AdPRTReturnQty")
                    {
                        dt = _PurchaseReturn_ISERVICES.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, flag, GRNNo, GRNDt, src_type, UOMID).Tables[0];

                        dt.Columns.Add("Qty", typeof(string));
                        //SubItemPopupDt subitmModel1 = new SubItemPopupDt
                        //{ ShowStock = "Y"
                        //        };
                    }
                    else
                    {
                        dt = _PurchaseReturn_ISERVICES.GetSubItemDetailsFromPinv(CompID, BrchID, PinvNo, GRNNo, GRNDt, Item_id, src_type).Tables[0];
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
                    if (Flag == "PRTReturnQty"|| Flag == "AdPRTReturnQty")
                    {
                        dt = _PurchaseReturn_ISERVICES.PRT_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, flag, GRNNo, GRNDt, wh_id,src_type).Tables[0];
                        if (avlstock == "avlstock")
                        {
                            ViewBag.SubitemAvlStockDetail = dt;
                            ViewBag.Flag = "WH";
                            return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                        }
                    }
                    else
                    {
                        dt = _PurchaseReturn_ISERVICES.GetSubItemDetailsFromPinv(CompID, BrchID, PinvNo, GRNNo, GRNDt, Item_id,src_type).Tables[0];
                    }
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    
                    //ShowStock = "Y",
                    ShowStock = Flag == "PRTReturnQty"  ? "Y" : "N",
                    _subitemPageName = "PRT",
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
        public ActionResult getItemStockBatchWise(string ItemId, string Status, string WarehouseId, string SelectedItemdetail, string docid, string TransType, string Command)
        {
            try
            {
                string Docnolist = string.Empty;
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                //if (SelectedDocdetail != null && SelectedDocdetail != "")
                //{
                //    JArray jObjectDoc = JArray.Parse(SelectedDocdetail);

                //    foreach (JObject item in jObjectDoc.Children())
                //    {
                //        if (string.IsNullOrEmpty(Docnolist))
                //        {
                //            Docnolist = item.GetValue("docno").ToString().Trim();
                //        }
                //        else
                //        {
                //            Docnolist = Docnolist + "," + item.GetValue("docno").ToString().Trim();
                //        }
                //    }
                //}
                var Doclist = "";
                ds = _PurchaseReturn_ISERVICES.getItemStockBatchWise(ItemId, WarehouseId, CompID, BrchID, Doclist);

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("BatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty").ToString().Trim();
                                //ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty");
                            }
                        }
                    }
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PurchaseRetrunListItemStockBatchWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getItemStockBatchWiseAfterInsert(string prt_no, string prt_dt, string Status, string ItemId, string TransType, string Command)
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
                DataSet ds = _PurchaseReturn_ISERVICES.getItemStockBatchWiseAfterInsert(Comp_ID, Br_ID, prt_no, prt_dt, ItemId);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = Status;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PurchaseRetrunListItemStockBatchWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /*----------------- End Sub Item Detail Section---------------------------------------*/
        // Added by Nidhi on 20-06-2025
        public string SavePdfDocToSendOnEmailAlert(string PRTNo, string PRTDate, string fileName)
        {
            var data = GetPdfData(PRTNo, PRTDate, "");
            var commonCont = new CommonController(_Common_IServices);
            return commonCont.SaveAlertDocument(data, fileName);
        }
        public ActionResult GetTaxAmountDetail(string ItmCode, string InvoiceNo, string ShipNumber, string ReturnQuantity, string src_type,string HdGlTaxDetailInsight)/*Add by Hina sharma on 16-12-2024*/
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataTable GlTaxDetailInsight = ToDtblGlTaxDetailInsight(HdGlTaxDetailInsight);
                DataSet dt = _PurchaseReturn_ISERVICES.GetTaxAmountDetail(CompID, BrchID, ItmCode, InvoiceNo, ShipNumber, ReturnQuantity, src_type, GlTaxDetailInsight);
                ViewBag.TaxAmountDetail = dt.Tables[0];
                ViewBag.TaxAmountDetailTotal = dt.Tables[1];
                ViewBag.GlTaxDetailInsight = GlTaxDetailInsight;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSRTaxAmountDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private DataTable ToDtblGlTaxDetailInsight(string hdIGlTaxDetailInsight)
        {
            try
            {
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("itemid", typeof(string));
                dtItem.Columns.Add("type", typeof(string));
                dtItem.Columns.Add("Entity_id", typeof(string));
                dtItem.Columns.Add("tax_rate", typeof(string));
                dtItem.Columns.Add("amt", typeof(string));
                if (hdIGlTaxDetailInsight != null)
                {
                    JArray jObject = JArray.Parse(hdIGlTaxDetailInsight);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["itemid"] = jObject[i]["itemid"].ToString();
                        dtrowLines["type"] = jObject[i]["type"].ToString();
                        dtrowLines["Entity_id"] = jObject[i]["Entity_id"].ToString();
                        dtrowLines["tax_rate"] = jObject[i]["tax_rate"].ToString();
                        dtrowLines["amt"] = jObject[i]["amt"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                }
                return dtItem;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
    }
}