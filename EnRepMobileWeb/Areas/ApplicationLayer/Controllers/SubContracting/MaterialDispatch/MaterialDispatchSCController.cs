using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MaterialDispatch;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MaterialDispatch;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SubContracting.MaterialDispatch
{
    public class MaterialDispatchSCController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105108105", title;
        List<MaterialDisList> _MaterialDisList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MaterialDispatch_ISERVICES _MaterialDispatch_ISERVICES;
        CommonController cmn = new CommonController();
        public MaterialDispatchSCController(Common_IServices _Common_IServices, MaterialDispatch_ISERVICES _MaterialDispatch_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._MaterialDispatch_ISERVICES = _MaterialDispatch_ISERVICES;

        }
        // GET: ApplicationLayer/MaterialDispatchSC
        public ActionResult MaterialDispatchSC(string WF_Status)
        {
            try
            {
                ViewBag.DocID = DocumentMenuId;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                MDListModel _MDListModel = new MDListModel();
                _MDListModel.WF_Status = WF_Status;
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        _MDListModel.SuppID = a[0].Trim();
                        _MDListModel.FinishProdct_Id = a[1].Trim();
                        _MDListModel.Product_id = a[2].Trim();
                        _MDListModel.FromDate = a[3].Trim();
                        _MDListModel.ToDate = a[4].Trim();
                        _MDListModel.Status = a[5].Trim();
                        if (_MDListModel.Status == "0")
                        {
                            _MDListModel.Status = null;
                        }
                        _MDListModel.ListFilterData = TempData["ListFilterData"].ToString();

                    }
                }
                else
                {
                    _MDListModel.FromDate = startDate;
                    _MDListModel.ToDate = CurrentDate;
                }
                _MDListModel.MaterialDispatchList = getMDList(_MDListModel);
                //_MDListModel.FromDate = startDate;
                
                GetAutoCompleteSearchSuppList(_MDListModel);
                CommonPageDetails();
                _MDListModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _MDListModel.StatusList = statusLists;

                _MDListModel.Title = title;
                //Session["MDSearch"] = "0";
                _MDListModel.MDSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/SubContracting/MaterialDispatch/MaterialDispatchList.cshtml", _MDListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            
        }

        public ActionResult AddMaterialDispatchSCDetail()
        {
            MaterialDispatchModel _MaterialDispatchModel = new MaterialDispatchModel();
            _MaterialDispatchModel.Message = "New";
            _MaterialDispatchModel.Command = "Add";
            _MaterialDispatchModel.AppStatus = "D";
            ViewBag.DocumentStatus = _MaterialDispatchModel.AppStatus;
            _MaterialDispatchModel.TransType = "Save";
            _MaterialDispatchModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _MaterialDispatchModel;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("MaterialDispatchSC");
            }
            /*End to chk Financial year exist or not*/
            CommonPageDetails();

            return RedirectToAction("MaterialDispatchSCDetail", "MaterialDispatchSC");
        }
        public ActionResult MaterialDispatchSCDetail(MaterialDispatchModel _MaterialDispatchModel1, string MDCodeURL, string MDDate, string TransType, string BtnName, string command,string WF_Status1)
        {
            try
            {

                ViewBag.DocID = DocumentMenuId;
                CommonPageDetails();
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MDDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _MaterialDispatchModel = TempData["ModelData"] as MaterialDispatchModel;
                if (_MaterialDispatchModel != null)
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
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _MaterialDispatchModel.Title = title;
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));

                   _MaterialDispatchModel.Title = title;
                    _MaterialDispatchModel.ValDigit = ValDigit;
                    _MaterialDispatchModel.QtyDigit = QtyDigit;
                    _MaterialDispatchModel.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;

                    List<SupplierName> suppLists1 = new List<SupplierName>();
                    suppLists1.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _MaterialDispatchModel.SupplierNameList = suppLists1;

                    List<JobOrdNoList> jobOrd_NoLists = new List<JobOrdNoList>();
                    jobOrd_NoLists.Add(new JobOrdNoList { JobOrdnoId = "0", JobOrdnoVal = "---Select---" });
                    _MaterialDispatchModel.jobordNoLists = jobOrd_NoLists;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MaterialDispatchModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_MaterialDispatchModel.TransType == "Update" || _MaterialDispatchModel.Command == "Edit")
                       
                    {
                        string MDSC_NO = _MaterialDispatchModel.MDIssue_No;
                        string MDSC_Date = _MaterialDispatchModel.MDIssue_Date;
                        DataSet ds = _MaterialDispatch_ISERVICES.GetMDDetailEditUpdate(CompID, BrchID, MDSC_NO, MDSC_Date, UserID, DocumentMenuId);
                        
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            
                            _MaterialDispatchModel.MDIssue_No = ds.Tables[0].Rows[0]["dispatch_no"].ToString();
                            _MaterialDispatchModel.MDIssue_Date = ds.Tables[0].Rows[0]["dispatch_date"].ToString();
                            _MaterialDispatchModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _MaterialDispatchModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists1.Add(new SupplierName { supp_id = _MaterialDispatchModel.SuppID, supp_name = _MaterialDispatchModel.SuppName });
                            _MaterialDispatchModel.SupplierNameList = suppLists1;
                            _MaterialDispatchModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _MaterialDispatchModel.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["add_id"].ToString());
                            _MaterialDispatchModel.JobOrdNum = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            _MaterialDispatchModel.JobOrd_Num=ds.Tables[0].Rows[0]["src_doc_no"].ToString();

                            jobOrd_NoLists.Add(new JobOrdNoList { JobOrdnoId = _MaterialDispatchModel.JobOrdNum, JobOrdnoVal = _MaterialDispatchModel.JobOrdNum });
                            _MaterialDispatchModel.jobordNoLists = jobOrd_NoLists;
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _MaterialDispatchModel.JobOrdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            _MaterialDispatchModel.FinishProduct = ds.Tables[0].Rows[0]["FItem"].ToString();
                            _MaterialDispatchModel.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _MaterialDispatchModel.FinishUom = ds.Tables[0].Rows[0]["FUom"].ToString();
                            _MaterialDispatchModel.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();
                            _MaterialDispatchModel.Operation_Name = ds.Tables[0].Rows[0]["OpName"].ToString();
                            _MaterialDispatchModel.OpId = ds.Tables[0].Rows[0]["op_id"].ToString();

                            _MaterialDispatchModel.Product_Name = ds.Tables[0].Rows[0]["ItemName"].ToString();
                            _MaterialDispatchModel.ProductId = ds.Tables[0].Rows[0]["product_id"].ToString();
                            _MaterialDispatchModel.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                            _MaterialDispatchModel.UOM_Name = ds.Tables[0].Rows[0]["UOMName"].ToString();
                            _MaterialDispatchModel.UomId = ds.Tables[0].Rows[0]["uom_id"].ToString();
                            _MaterialDispatchModel.Order_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["ord_qty"]).ToString(QtyDigit);
                            _MaterialDispatchModel.Pending_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["pend_qty"]).ToString(QtyDigit);
                            _MaterialDispatchModel.Dispatch_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["disp_qty"]).ToString(QtyDigit);
                            _MaterialDispatchModel.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            _MaterialDispatchModel.CreatedBy = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _MaterialDispatchModel.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _MaterialDispatchModel.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _MaterialDispatchModel.AmendedBy = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _MaterialDispatchModel.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _MaterialDispatchModel.ApprovedBy = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _MaterialDispatchModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _MaterialDispatchModel.StatusName = ds.Tables[0].Rows[0]["MdStatus"].ToString();
                            _MaterialDispatchModel.MDStatus = ds.Tables[0].Rows[0]["issue_status"].ToString().Trim();

                            _MaterialDispatchModel.GRNumber = ds.Tables[0].Rows[0]["gr_no"].ToString();
                            if (ds.Tables[0].Rows[0]["gr_date"].ToString() == "")
                            {
                                
                            }
                            else
                            {
                                _MaterialDispatchModel.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                            }
                            if (ds.Tables[0].Rows[0]["freight_amt"].ToString() == "")
                            {
                                _MaterialDispatchModel.FreightAmount = Convert.ToDecimal(0).ToString(RateDigit);
                            }
                            else
                            {
                                _MaterialDispatchModel.FreightAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["freight_amt"]).ToString(RateDigit);
                            }
                            
                            _MaterialDispatchModel.TransporterName = ds.Tables[0].Rows[0]["trans_name"].ToString();
                            _MaterialDispatchModel.VehicleNumber = ds.Tables[0].Rows[0]["veh_no"].ToString();
                            if (ds.Tables[0].Rows[0]["veh_load"].ToString() == "")
                            {
                                _MaterialDispatchModel.veh_load = Convert.ToDecimal(0).ToString(RateDigit);
                            }
                            else
                            {
                                _MaterialDispatchModel.veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"]).ToString(RateDigit);
                            }

                            _MaterialDispatchModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _MaterialDispatchModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);//Cancelled
                            getWarehouse(_MaterialDispatchModel);
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.ItemStockBatchWise = ds.Tables[2];
                            ViewBag.ItemStockSerialWise = ds.Tables[3];
                            ViewBag.SubItemDetails = ds.Tables[8];
                            _MaterialDispatchModel.JobOrderType = ds.Tables[9].Rows[0]["JobOrdTyp"].ToString();
                            if(_MaterialDispatchModel.JobOrderType=="D")
                            {
                                _MaterialDispatchModel.JobOrderType = "Direct";
                            }
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();  
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["issue_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _MaterialDispatchModel.Cancelled = true;
                        }
                        else
                        {
                            _MaterialDispatchModel.Cancelled = false;
                        }
                       
                        _MaterialDispatchModel.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        if (ViewBag.AppLevel != null && _MaterialDispatchModel.Command != "Edit")
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

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _MaterialDispatchModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                                        
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _MaterialDispatchModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _MaterialDispatchModel.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }

                    if (_MaterialDispatchModel.BtnName != null)
                    {
                        _MaterialDispatchModel.BtnName = _MaterialDispatchModel.BtnName;
                    }
                    _MaterialDispatchModel.TransType = _MaterialDispatchModel.TransType;
                    ViewBag.TransType = _MaterialDispatchModel.TransType;


                    if (_MaterialDispatchModel.DocumentStatus == null)
                    {
                        _MaterialDispatchModel.DocumentStatus = "D";
                        ViewBag.DocumentCode = "D";
                    }
                    else
                    {
                        _MaterialDispatchModel.DocumentStatus = _MaterialDispatchModel.DocumentStatus;
                        ViewBag.DocumentCode= _MaterialDispatchModel.DocumentStatus;
                        ViewBag.Command = _MaterialDispatchModel.Command;
                    }
                    ViewBag.DocumentStatus = _MaterialDispatchModel.DocumentStatus;
                    ViewBag.DocumentCode = _MaterialDispatchModel.DocumentStatus;
                    //ViewBag.FinstDt = Session["FinStDt"].ToString();


                    return View("~/Areas/ApplicationLayer/Views/SubContracting/MaterialDispatch/MaterialDispatchDetail.cshtml", _MaterialDispatchModel);

                }
                else
                {/*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
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
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _MaterialDispatchModel1.ValDigit = ValDigit;
                    _MaterialDispatchModel1.QtyDigit = QtyDigit;
                    _MaterialDispatchModel1.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.DocumentStatus = "D";
                    _MaterialDispatchModel1.WF_Status1 = WF_Status1;
                    List<SupplierName> suppLists1 = new List<SupplierName>();
                    suppLists1.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _MaterialDispatchModel1.SupplierNameList = suppLists1;

                    List<JobOrdNoList> jobOrd_NoLists1 = new List<JobOrdNoList>();
                    jobOrd_NoLists1.Add(new JobOrdNoList { JobOrdnoId = "0", JobOrdnoVal = "---Select---" });
                    _MaterialDispatchModel1.jobordNoLists = jobOrd_NoLists1;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MaterialDispatchModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_MaterialDispatchModel1.TransType == "Update" || _MaterialDispatchModel1.Command == "Edit")

                    {
                        string MDSC_NO = MDCodeURL;
                        string MDSC_Date = MDDate;
                        DataSet ds = _MaterialDispatch_ISERVICES.GetMDDetailEditUpdate(CompID, BrchID, MDSC_NO, MDSC_Date, UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _MaterialDispatchModel1.MDIssue_No = ds.Tables[0].Rows[0]["dispatch_no"].ToString();
                            _MaterialDispatchModel1.MDIssue_Date = ds.Tables[0].Rows[0]["dispatch_date"].ToString();
                            _MaterialDispatchModel1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _MaterialDispatchModel1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists1.Add(new SupplierName { supp_id = _MaterialDispatchModel1.SuppID, supp_name = _MaterialDispatchModel1.SuppName });
                            _MaterialDispatchModel1.SupplierNameList = suppLists1;
                            _MaterialDispatchModel1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _MaterialDispatchModel1.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["add_id"].ToString());
                            
                                _MaterialDispatchModel1.JobOrdNum = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                                _MaterialDispatchModel1.JobOrd_Num = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            
                            jobOrd_NoLists1.Add(new JobOrdNoList { JobOrdnoId = _MaterialDispatchModel1.JobOrdNum, JobOrdnoVal = _MaterialDispatchModel1.JobOrdNum });
                            _MaterialDispatchModel1.jobordNoLists = jobOrd_NoLists1;
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _MaterialDispatchModel1.JobOrdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            _MaterialDispatchModel1.FinishProduct = ds.Tables[0].Rows[0]["FItem"].ToString();
                            _MaterialDispatchModel1.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _MaterialDispatchModel1.FinishUom = ds.Tables[0].Rows[0]["FUom"].ToString();
                            _MaterialDispatchModel1.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();

                            _MaterialDispatchModel1.Operation_Name = ds.Tables[0].Rows[0]["OpName"].ToString();
                            _MaterialDispatchModel1.OpId = ds.Tables[0].Rows[0]["op_id"].ToString();

                            _MaterialDispatchModel1.Product_Name = ds.Tables[0].Rows[0]["ItemName"].ToString();
                            _MaterialDispatchModel1.ProductId = ds.Tables[0].Rows[0]["product_id"].ToString();
                            _MaterialDispatchModel1.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                            _MaterialDispatchModel1.UOM_Name = ds.Tables[0].Rows[0]["UOMName"].ToString();
                            _MaterialDispatchModel1.UomId = ds.Tables[0].Rows[0]["uom_id"].ToString();
                            _MaterialDispatchModel1.Order_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["ord_qty"]).ToString(QtyDigit);
                               _MaterialDispatchModel1.Dispatch_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["disp_qty"]).ToString(QtyDigit); _MaterialDispatchModel1.CreatedBy = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _MaterialDispatchModel1.Pending_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["pend_qty"]).ToString(QtyDigit);
                            _MaterialDispatchModel1.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();

                            _MaterialDispatchModel1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _MaterialDispatchModel1.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _MaterialDispatchModel1.AmendedBy = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _MaterialDispatchModel1.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _MaterialDispatchModel1.ApprovedBy = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _MaterialDispatchModel1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _MaterialDispatchModel1.StatusName = ds.Tables[0].Rows[0]["MdStatus"].ToString();
                            _MaterialDispatchModel1.MDStatus = ds.Tables[0].Rows[0]["issue_status"].ToString().Trim();

                            _MaterialDispatchModel1.GRNumber = ds.Tables[0].Rows[0]["gr_no"].ToString();
                            if (ds.Tables[0].Rows[0]["gr_date"].ToString() == "")
                            {

                            }
                            else
                            {
                                _MaterialDispatchModel1.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                            }
                            if (ds.Tables[0].Rows[0]["freight_amt"].ToString() == "")
                            {
                                _MaterialDispatchModel1.FreightAmount = Convert.ToDecimal(0).ToString(RateDigit);
                            }
                            else
                            {
                                _MaterialDispatchModel1.FreightAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["freight_amt"]).ToString(RateDigit);
                            }

                            _MaterialDispatchModel1.TransporterName = ds.Tables[0].Rows[0]["trans_name"].ToString();
                            _MaterialDispatchModel1.VehicleNumber = ds.Tables[0].Rows[0]["veh_no"].ToString();
                            if (ds.Tables[0].Rows[0]["veh_load"].ToString() == "")
                            {
                                _MaterialDispatchModel1.veh_load = Convert.ToDecimal(0).ToString(RateDigit);
                            }
                            else
                            {
                                _MaterialDispatchModel1.veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"]).ToString(RateDigit);
                            }

                            _MaterialDispatchModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _MaterialDispatchModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);//Cancelled

                            getWarehouse(_MaterialDispatchModel1);
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.ItemStockBatchWise = ds.Tables[2];
                            ViewBag.ItemStockSerialWise = ds.Tables[3];
                            ViewBag.SubItemDetails = ds.Tables[8];
                            _MaterialDispatchModel1.JobOrderType = ds.Tables[9].Rows[0]["JobOrdTyp"].ToString();
                            if (_MaterialDispatchModel1.JobOrderType == "D")
                            {
                                _MaterialDispatchModel1.JobOrderType = "Direct";
                            }
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["issue_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _MaterialDispatchModel1.Cancelled = true;
                        }
                        else
                        {
                            _MaterialDispatchModel1.Cancelled = false;
                        }
                        
                        _MaterialDispatchModel1.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        if (ViewBag.AppLevel != null && _MaterialDispatchModel1.Command != "Edit")
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

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _MaterialDispatchModel1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _MaterialDispatchModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _MaterialDispatchModel1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MaterialDispatchModel1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _MaterialDispatchModel1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MaterialDispatchModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _MaterialDispatchModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _MaterialDispatchModel1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _MaterialDispatchModel1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel != null)
                        {
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                        }

                    }

                    var MDCode = "";
                    if (MDCodeURL != null)
                    {
                        MDCode = MDCodeURL;
                        _MaterialDispatchModel1.MDIssue_No = MDCodeURL;
                    }
                    else
                    {
                        MDCode = _MaterialDispatchModel1.MDIssue_No;
                    }
                    if(_MaterialDispatchModel1.MDStatus=="D"&& _MaterialDispatchModel1.Command=="Edit")
                    {
                        _MaterialDispatchModel1.TransType = "Update";
                        _MaterialDispatchModel1.BtnName = "BtnEdit";
                    }
                    if (_MaterialDispatchModel1.MDStatus == "DL"|| _MaterialDispatchModel1.MDStatus == "PDL")
                    {
                        //_MaterialDispatchModel1.TransType = "Update";
                        _MaterialDispatchModel1.BtnName = "Refresh";
                    }
                    if (TransType != null)
                    {
                        _MaterialDispatchModel1.TransType = TransType;
                        ViewBag.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _MaterialDispatchModel1.Command = command;
                        ViewBag.Command = command;
                    }

                    if (_MaterialDispatchModel1.BtnName == null && _MaterialDispatchModel1.Command == null)
                    {
                        _MaterialDispatchModel1.BtnName = "AddNew";
                        _MaterialDispatchModel1.Command = "Add";
                        _MaterialDispatchModel1.AppStatus = "D";
                        ViewBag.DocumentStatus = _MaterialDispatchModel1.AppStatus;
                        _MaterialDispatchModel1.TransType = "Save";
                        _MaterialDispatchModel1.BtnName = "BtnAddNew";

                    }

                    if (_MaterialDispatchModel1.BtnName != null)
                    {
                        _MaterialDispatchModel1.BtnName = _MaterialDispatchModel1.BtnName;
                    }
                    _MaterialDispatchModel1.TransType = _MaterialDispatchModel1.TransType;
                    ViewBag.TransType = _MaterialDispatchModel1.TransType;
                    if (_MaterialDispatchModel1.DocumentStatus != null)
                    {
                        _MaterialDispatchModel1.DocumentStatus = _MaterialDispatchModel1.DocumentStatus;
                        ViewBag.DocumentStatus = _MaterialDispatchModel1.DocumentStatus;
                        ViewBag.DocumentCode = _MaterialDispatchModel1.DocumentStatus;
                    }

                    _MaterialDispatchModel1.Title = title;
                    //ViewBag.FinstDt = Session["FinStDt"].ToString();


                    return View("~/Areas/ApplicationLayer/Views/SubContracting/MaterialDispatch/MaterialDispatchDetail.cshtml", _MaterialDispatchModel1);
                }
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
        public ActionResult MaterialDispatchBtnCommand(MaterialDispatchModel _MaterialDispatchModel, string command)
        {
            try
            {
                /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_MaterialDispatchModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //_MaterialDispatchModel = new MaterialDispatchModel();
                        //_MaterialDispatchModel.Message = "New";
                        //_MaterialDispatchModel.Command = "Add";
                        //_MaterialDispatchModel.AppStatus = "D";
                        //_MaterialDispatchModel.DocumentStatus = "D";
                        //ViewBag.DocumentStatus = _MaterialDispatchModel.DocumentStatus;
                        //_MaterialDispatchModel.TransType = "Save";
                        //_MaterialDispatchModel.BtnName = "BtnAddNew";
                        //TempData["ModelData"] = _MaterialDispatchModel;
                        //TempData["ListFilterData"] = null;
                        MaterialDispatchModel _MaterialDispatchModelAdd = new MaterialDispatchModel();
                        _MaterialDispatchModelAdd.Message = "New";
                        _MaterialDispatchModelAdd.Command = "Add";
                        _MaterialDispatchModelAdd.AppStatus = "D";
                        _MaterialDispatchModelAdd.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _MaterialDispatchModel.DocumentStatus;
                        _MaterialDispatchModelAdd.TransType = "Save";
                        _MaterialDispatchModelAdd.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _MaterialDispatchModelAdd;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_MaterialDispatchModel.MDIssue_No))
                                return RedirectToAction("DoubleClickOnList", new { DocNo = _MaterialDispatchModel.MDIssue_No, DocDate = _MaterialDispatchModel.MDIssue_Date, ListFilterData = _MaterialDispatchModel.ListFilterData1, WF_status = _MaterialDispatchModel.WFStatus });
                            else
                                _MaterialDispatchModelAdd.Command = "Refresh";
                            _MaterialDispatchModelAdd.TransType = "Refresh";
                            _MaterialDispatchModelAdd.BtnName = "Refresh";
                            _MaterialDispatchModelAdd.DocumentStatus = null;
                            TempData["ModelData"] = _MaterialDispatchModelAdd;
                            return RedirectToAction("MaterialDispatchSCDetail", "MaterialDispatchSC");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("MaterialDispatchSCDetail", "MaterialDispatchSC");

                    case "Edit":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _MaterialDispatchModel.MDIssue_No, DocDate = _MaterialDispatchModel.MDIssue_Date, ListFilterData = _MaterialDispatchModel.ListFilterData1, WF_status = _MaterialDispatchModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string joDt = _MaterialDispatchModel.MDIssue_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, joDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _MaterialDispatchModel.MDIssue_No, DocDate = _MaterialDispatchModel.MDIssue_Date, ListFilterData = _MaterialDispatchModel.ListFilterData1, WF_status = _MaterialDispatchModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        var TransType = "";
                        var BtnName = "";
                        if (_MaterialDispatchModel.MDStatus == "A")
                        {
                            if (CheckDeliveryNoteSCAgainstMaterialDispatch(_MaterialDispatchModel) == "Used")
                            {
                                _MaterialDispatchModel.Message = "Used";
                                _MaterialDispatchModel.TransType = "Update";
                                _MaterialDispatchModel.Command = "Add";
                                _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                                TempData["ModelData"] = _MaterialDispatchModel;
                            }

                            else
                            {
                                _MaterialDispatchModel.TransType = "Update";
                                _MaterialDispatchModel.Command = command;
                                _MaterialDispatchModel.BtnName = "BtnEdit";
                                _MaterialDispatchModel.Message = "New";
                                _MaterialDispatchModel.AppStatus = "D";
                                _MaterialDispatchModel.DocumentStatus = "D";
                                ViewBag.DocumentStatus = _MaterialDispatchModel.AppStatus;
                                TempData["ModelData"] = _MaterialDispatchModel;
                            }
                        }
                        else if (_MaterialDispatchModel.MDStatus == "PDL")
                        {
                            _MaterialDispatchModel.Message = "New";
                            _MaterialDispatchModel.TransType = "Update";
                            _MaterialDispatchModel.Command = "Edit";
                            _MaterialDispatchModel.BtnName = "BtnEdit";
                            TempData["ModelData"] = _MaterialDispatchModel;
                        }
                        else
                        {
                            _MaterialDispatchModel.TransType = "Update";
                            _MaterialDispatchModel.Command = command;
                            _MaterialDispatchModel.BtnName = "BtnEdit";
                            _MaterialDispatchModel.Message = "New";
                            _MaterialDispatchModel.AppStatus = "D";
                            _MaterialDispatchModel.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _MaterialDispatchModel.AppStatus;
                            TempData["ModelData"] = _MaterialDispatchModel;
                        }
                        var MDCodeURL = _MaterialDispatchModel.MDIssue_No;
                        var MDDate = _MaterialDispatchModel.MDIssue_Date;
                        command = _MaterialDispatchModel.Command;
                        TempData["ModelData"] = _MaterialDispatchModel;
                        TempData["ListFilterData"] = _MaterialDispatchModel.ListFilterData1;
                        return (RedirectToAction("MaterialDispatchSCDetail", new { MDCodeURL = MDCodeURL, MDDate, TransType, BtnName, command }));
                        
                    case "Delete":
                        _MaterialDispatchModel.Command = command;
                        _MaterialDispatchModel.BtnName = "Refresh";
                        DeleteMDDetails(_MaterialDispatchModel, command);
                        TempData["ListFilterData"] = _MaterialDispatchModel.ListFilterData1;
                        return RedirectToAction("MaterialDispatchSCDetail");

                    case "Save":
                        _MaterialDispatchModel.Command = command;
                        SaveMDDetail(_MaterialDispatchModel);
                        if (_MaterialDispatchModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_MaterialDispatchModel.Message == "DocModify")
                        {
                            //_MaterialDispatchModel.DocumentMenuId = _MaterialDispatchModel.DocumentMenuId;
                            DocumentMenuId = _MaterialDispatchModel.DocumentMenuId;
                            CommonPageDetails();
                            //ViewBag.DocID = _MaterialDispatchModel.DocumentMenuId;
                            ViewBag.DocumentMenuId = _MaterialDispatchModel.DocumentMenuId;
                            ViewBag.DocumentStatus = "D";


                            List<SupplierName> suppLists1 = new List<SupplierName>();
                            suppLists1.Add(new SupplierName { supp_id = _MaterialDispatchModel.SuppID, supp_name = _MaterialDispatchModel.SuppName });
                            _MaterialDispatchModel.SupplierNameList = suppLists1;

                            List<JobOrdNoList> jobOrd_NoLists = new List<JobOrdNoList>();
                            jobOrd_NoLists.Add(new JobOrdNoList { JobOrdnoId = _MaterialDispatchModel.JobOrd_Num, JobOrdnoVal = _MaterialDispatchModel.JobOrd_Num });
                            _MaterialDispatchModel.jobordNoLists = jobOrd_NoLists;

                            getWarehouse(_MaterialDispatchModel);
                            _MaterialDispatchModel.SuppName = _MaterialDispatchModel.SuppName;
                            _MaterialDispatchModel.Address = _MaterialDispatchModel.Address;
                            _MaterialDispatchModel.JobOrdNum = _MaterialDispatchModel.JobOrd_Num;
                            
                            _MaterialDispatchModel.JobOrdDate = _MaterialDispatchModel.JobOrdDate;
                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.ItemStockBatchWise = ViewData["BatchDetails"];
                            ViewBag.ItemStockSerialWise = ViewData["SerialDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _MaterialDispatchModel.BtnName = "Refresh";
                            _MaterialDispatchModel.Command = "Refresh";
                            _MaterialDispatchModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            _MaterialDispatchModel.ValDigit = ValDigit;
                            _MaterialDispatchModel.QtyDigit = QtyDigit;
                            _MaterialDispatchModel.RateDigit = RateDigit;
                            return View("~/Areas/ApplicationLayer/Views/SubContracting/MaterialDispatch/MaterialDispatchDetail.cshtml", _MaterialDispatchModel);
                        }
                        else
                        {
                            MDCodeURL = _MaterialDispatchModel.MDIssue_No;
                            MDDate = _MaterialDispatchModel.MDIssue_Date;
                            TransType = _MaterialDispatchModel.TransType;
                            BtnName = _MaterialDispatchModel.BtnName;
                            TempData["ModelData"] = _MaterialDispatchModel;
                            TempData["ListFilterData"] = _MaterialDispatchModel.ListFilterData1;
                            return (RedirectToAction("MaterialDispatchSCDetail", new { MDCodeURL = MDCodeURL, MDDate, TransType, BtnName, command }));
                        }
                    case "Forward":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _MaterialDispatchModel.MDIssue_No, DocDate = _MaterialDispatchModel.MDIssue_Date, ListFilterData = _MaterialDispatchModel.ListFilterData1, WF_status = _MaterialDispatchModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string mdDt = _MaterialDispatchModel.MDIssue_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, mdDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _MaterialDispatchModel.MDIssue_No, DocDate = _MaterialDispatchModel.MDIssue_Date, ListFilterData = _MaterialDispatchModel.ListFilterData1, WF_status = _MaterialDispatchModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _MaterialDispatchModel.MDIssue_No, DocDate = _MaterialDispatchModel.MDIssue_Date, ListFilterData = _MaterialDispatchModel.ListFilterData1, WF_status = _MaterialDispatchModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string mdDt1 = _MaterialDispatchModel.MDIssue_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, mdDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _MaterialDispatchModel.MDIssue_No, DocDate = _MaterialDispatchModel.MDIssue_Date, ListFilterData = _MaterialDispatchModel.ListFilterData1, WF_status = _MaterialDispatchModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _MaterialDispatchModel.Command = command;
                        MDApprove(_MaterialDispatchModel,"");
                        MDCodeURL = _MaterialDispatchModel.MDIssue_No;
                        MDDate = _MaterialDispatchModel.MDIssue_Date;
                        TransType = _MaterialDispatchModel.TransType;
                        BtnName = _MaterialDispatchModel.BtnName;
                        TempData["ModelData"] = _MaterialDispatchModel;
                        TempData["ListFilterData"] = _MaterialDispatchModel.ListFilterData1;
                        return (RedirectToAction("MaterialDispatchSCDetail", new { MDCodeURL = MDCodeURL, MDDate, TransType, BtnName, command }));
                    case "Refresh":
                        MaterialDispatchModel _MaterialDispatchModelRefresh = new MaterialDispatchModel();
                        _MaterialDispatchModelRefresh.Message = null;
                        _MaterialDispatchModelRefresh.Command = command;
                        _MaterialDispatchModelRefresh.TransType = "Refresh";
                        _MaterialDispatchModelRefresh.BtnName = "Refresh";
                        _MaterialDispatchModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _MaterialDispatchModelRefresh;
                        TempData["ListFilterData"] = _MaterialDispatchModel.ListFilterData1;
                        return RedirectToAction("MaterialDispatchSCDetail");
                    case "Print":
                        return GenratePdfFile(_MaterialDispatchModel);
                    case "BacktoList":
                        var WF_Status = _MaterialDispatchModel.WF_Status1;
                        TempData["ListFilterData"] = _MaterialDispatchModel.ListFilterData1;
                        return RedirectToAction("MaterialDispatchSC", "MaterialDispatchSC",new { WF_Status });
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
        public ActionResult SaveMDDetail(MaterialDispatchModel _MaterialDispatchModel)
        {
            string SaveMessage = "";
            /*getDocumentName();*/ /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_MaterialDispatchModel.Cancelled == false)
                {
                    _MaterialDispatchModel.DocumentMenuId = _MaterialDispatchModel.DocumentMenuId;


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
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();


                    DataTable dtheader = new DataTable();

                    DtblHDetail = ToDtblHDetail(_MaterialDispatchModel);
                    DtblItemDetail = ToDtblItemDetail(_MaterialDispatchModel.Itemdetails);

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("comp_id", typeof(int));
                    Batch_detail.Columns.Add("br_id", typeof(int));
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("uom_id", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("avl_batch_qty", typeof(float));
                    Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
                    Batch_detail.Columns.Add("issue_qty", typeof(float));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    if (_MaterialDispatchModel.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_MaterialDispatchModel.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                            dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                            dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                            dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                            dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["avl_batch_qty"].ToString();
                            if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null || jObjectBatch[i]["ExpiryDate"].ToString() == "undefined")
                            {
                                dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                //dtrowBatchDetailsLines["expiry_date"] = Convert.ToDateTime(jObjectBatch[i]["ExpiryDate"].ToString()).ToString("yyyy-mm-dd");
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }
                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();


                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                        ViewData["BatchDetails"] = dtbatchdetail(jObjectBatch);

                    }
                    ItemBatchDetails = Batch_detail;
                    

                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("comp_id", typeof(int));
                    Serial_detail.Columns.Add("br_id", typeof(int));
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    Serial_detail.Columns.Add("uom_id", typeof(int));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    // Serial_detail.Columns.Add("serial_qty", typeof(string));
                    Serial_detail.Columns.Add("issue_qty", typeof(string));
                    Serial_detail.Columns.Add("lot_no", typeof(string));

                    if (_MaterialDispatchModel.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_MaterialDispatchModel.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            //dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["SerialDetails"] = dtserialdetail(jObjectSerial);

                    }
                    ItemSerialDetails = Serial_detail;

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    dtSubItem.Columns.Add("Avl_stk", typeof(string));
                    dtSubItem.Columns.Add("SubItmTyp", typeof(string));
                    dtSubItem.Columns.Add("Ord_Qty", typeof(string));
                    dtSubItem.Columns.Add("Pend_Qty", typeof(string));
                    //dtSubItem.Columns.Add("wh_id", typeof(string));

                    if (_MaterialDispatchModel.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_MaterialDispatchModel.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                           var SubitmTyp= jObject2[i]["SubItmType"].ToString();
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["Avl_stk"] = jObject2[i]["avl_stk"].ToString();
                            dtrowItemdetails["SubItmTyp"] = jObject2[i]["SubItmType"].ToString();
                            //if(SubitmTyp=="SubHedr")
                            //{
                                dtrowItemdetails["Ord_Qty"] = jObject2[i]["OrderQty"].ToString();
                                dtrowItemdetails["Pend_Qty"] = jObject2[i]["PendQty"].ToString();
                            //}
                            //dtrowItemdetails["wh_id"] = jObject2[i]["WhId"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                    }

                    /*------------------Sub Item end----------------------*/


                    SaveMessage = _MaterialDispatch_ISERVICES.InsertMD_Details(DtblHDetail, DtblItemDetail, ItemBatchDetails, ItemSerialDetails, dtSubItem);
                    if (SaveMessage == "DocModify")
                    {
                        _MaterialDispatchModel.Message = "DocModify";
                        _MaterialDispatchModel.BtnName = "Refresh";
                        _MaterialDispatchModel.Command = "Refresh";
                        _MaterialDispatchModel.DocumentMenuId = DocumentMenuId;
                        TempData["ModelData"] = _MaterialDispatchModel;
                        return RedirectToAction("MaterialDispatchSCDetail");
                    }
                    else
                    {
                        string[] Data = SaveMessage.Split(',');

                        string MDNo = Data[1];
                        string MD_No = MDNo.Replace("/", "");
                        string Message = Data[0];
                        if (Message == "Data_Not_Found")
                        {
                            var a = MD_No.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _MaterialDispatchModel.Message = Message.Replace("_", "");
                            return RedirectToAction("MaterialDispatchSCDetail");
                        }
                        string MDDate = Data[2];
                        string Message1 = Data[4];
                        string StatusCode = Data[3];

                        //if (Message1 == "Cancelled")
                        //{
                        //    _MaterialDispatchModel.Message = "Cancelled";
                        //    _MaterialDispatchModel.Command = "Update";
                        //    _MaterialDispatchModel.TransType = "Update";
                        //    _MaterialDispatchModel.AppStatus = "D";
                        //    _MaterialDispatchModel.BtnName = "Refresh";
                        //    _MaterialDispatchModel.MDIssue_No = MDNo;
                        //    _MaterialDispatchModel.MDIssue_Date = MDDate;
                        //    TempData["ModelData"] = _MaterialDispatchModel;

                        //    return RedirectToAction("JobOrderSCDetail");
                        //}

                        if (Message == "Update" || Message == "Save")
                        {
                            _MaterialDispatchModel.Message = "Save";
                            _MaterialDispatchModel.Command = "Update";
                            _MaterialDispatchModel.MDIssue_No = MDNo;
                            _MaterialDispatchModel.MDIssue_Date = MDDate;
                            _MaterialDispatchModel.TransType = "Update";
                            _MaterialDispatchModel.AppStatus = "D";
                            _MaterialDispatchModel.DocumentStatus = "D";
                            _MaterialDispatchModel.BtnName = "BtnSave";
                            TempData["ModelData"] = _MaterialDispatchModel;

                            return RedirectToAction("MaterialDispatchSCDetail");
                        }
                        //return RedirectToAction("JobOrderSCDetail");
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
                    _MaterialDispatchModel.CreatedBy = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    SaveMessage = _MaterialDispatch_ISERVICES.MDCancel(_MaterialDispatchModel, CompID, br_id, mac_id);
                    if(SaveMessage== "StkNotAvl")
                    {
                        _MaterialDispatchModel.Message = "StockNotAvailable";
                        _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                        _MaterialDispatchModel.Command = "Approve";
                        _MaterialDispatchModel.TransType = "Update";
                        _MaterialDispatchModel.AppStatus = "D";
                    }
                    else
                    {
                        _MaterialDispatchModel.Message = "Cancelled";
                        _MaterialDispatchModel.Command = "Update";
                        _MaterialDispatchModel.MDIssue_No = _MaterialDispatchModel.MDIssue_No;
                        _MaterialDispatchModel.MDIssue_Date = _MaterialDispatchModel.MDIssue_Date;
                        _MaterialDispatchModel.TransType = "Update";
                        _MaterialDispatchModel.AppStatus = "D";
                        _MaterialDispatchModel.BtnName = "Refresh";
                    }
                    return RedirectToAction("MaterialDispatchSCDetail");
                }
                return RedirectToAction("MaterialDispatchSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_MaterialDispatchModel.TransType == "Save")
                    {
                        string Guid = "";
                        if (_MaterialDispatchModel.Guid != null)
                        {
                            Guid = _MaterialDispatchModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable ToDtblHDetail(MaterialDispatchModel _MaterialDispatchModel)
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
                //dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("MD_No", typeof(string));
                dtheader.Columns.Add("MD_Date", typeof(string));
                dtheader.Columns.Add("JobDocNo", typeof(string));
                dtheader.Columns.Add("JobDocDate", typeof(string));
                dtheader.Columns.Add("SuppName", typeof(int));
                dtheader.Columns.Add("bill_add_id", typeof(int));
                dtheader.Columns.Add("FItemID", typeof(string));
                dtheader.Columns.Add("FUomId", typeof(int));
                dtheader.Columns.Add("OpId", typeof(int));
                dtheader.Columns.Add("ItemId", typeof(string));
                dtheader.Columns.Add("UomId", typeof(int));
                dtheader.Columns.Add("OrdQty", typeof(string));
                dtheader.Columns.Add("PendingQty", typeof(string));
                dtheader.Columns.Add("DispatchQty", typeof(string));
                dtheader.Columns.Add("Remarks", typeof(string));

                dtheader.Columns.Add("gr_no", typeof(string));
                if (_MaterialDispatchModel.GRDate != null)
                {
                    dtheader.Columns.Add("gr_date", typeof(DateTime));
                }
                else
                {
                    dtheader.Columns.Add("gr_date", typeof(string));
                }
                dtheader.Columns.Add("freight_amt", typeof(string));
                dtheader.Columns.Add("trans_name", typeof(string));
                dtheader.Columns.Add("veh_no", typeof(string));
                dtheader.Columns.Add("veh_load", typeof(string));
                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("MDStatus", typeof(string));
                dtheader.Columns.Add("SystemDetail", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _MaterialDispatchModel.TransType;
                
                //dtrowHeader["Cancelled"] = ConvertBoolToStrint(_MaterialDispatchModel.Cancelled);
                dtrowHeader["MD_No"] = _MaterialDispatchModel.MDIssue_No;
                dtrowHeader["MD_Date"] = _MaterialDispatchModel.MDIssue_Date;
                dtrowHeader["JobDocNo"] = _MaterialDispatchModel.JobOrd_Num;
                dtrowHeader["JobDocDate"] = _MaterialDispatchModel.JobOrdDate;
                dtrowHeader["SuppName"] = _MaterialDispatchModel.SuppID;
                dtrowHeader["bill_add_id"] = _MaterialDispatchModel.bill_add_id;
                dtrowHeader["FItemID"] = _MaterialDispatchModel.FinishProductId;
                dtrowHeader["FUomId"] = _MaterialDispatchModel.FinishUomId;
                if(_MaterialDispatchModel.JobOrderType=="Direct")
                {
                    dtrowHeader["OpId"] = 0;
                }
                else
                {
                    dtrowHeader["OpId"] = _MaterialDispatchModel.OpId;
                }
                
                dtrowHeader["ItemId"] = _MaterialDispatchModel.ProductId;
                dtrowHeader["UomId"] = _MaterialDispatchModel.UomId;
                dtrowHeader["OrdQty"] = _MaterialDispatchModel.Order_Qty;
                dtrowHeader["PendingQty"] = _MaterialDispatchModel.Pending_Qty;
                dtrowHeader["DispatchQty"] = _MaterialDispatchModel.Dispatch_Qty;
                dtrowHeader["Remarks"] = _MaterialDispatchModel.Remarks;
                dtrowHeader["gr_no"] = _MaterialDispatchModel.GRNumber;
                dtrowHeader["gr_date"] = _MaterialDispatchModel.GRDate;
                dtrowHeader["freight_amt"] = _MaterialDispatchModel.FreightAmount;
                dtrowHeader["trans_name"] = _MaterialDispatchModel.TransporterName;
                dtrowHeader["veh_no"] = _MaterialDispatchModel.VehicleNumber;
                if (!string.IsNullOrEmpty(_MaterialDispatchModel.veh_load))
                {
                    dtrowHeader["veh_load"] = _MaterialDispatchModel.veh_load;
                }
                else
                {
                    dtrowHeader["veh_load"] = "0";
                }
                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["MDStatus"] = IsNull(_MaterialDispatchModel.MDStatus, "D");
                
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["SystemDetail"] = mac_id;
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
        private DataTable ToDtblItemDetail(string MDItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(string));
                dtItem.Columns.Add("RequiredQty", typeof(string));
                dtItem.Columns.Add("WhID", typeof(int));
                dtItem.Columns.Add("AvlStock", typeof(string));
                dtItem.Columns.Add("IssuedQty", typeof(string));
                dtItem.Columns.Add("Value", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));


                if (MDItemDetail != null)
                {
                    JArray jObject = JArray.Parse(MDItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["RequiredQty"] = jObject[i]["RequiredQty"].ToString();
                        dtrowLines["WhID"] = jObject[i]["WhID"].ToString();
                        dtrowLines["AvlStock"] = jObject[i]["AvlStock"].ToString();
                        dtrowLines["IssuedQty"] = jObject[i]["IssuedQty"].ToString();
                        dtrowLines["Value"] = jObject[i]["Value"].ToString();
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();

                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["ItemDetails"] = dtitemdetail(jObject);
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
        public DataTable dtitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(string));
            dtItem.Columns.Add("UOMName", typeof(string));
            dtItem.Columns.Add("Req_Qty", typeof(string));
            dtItem.Columns.Add("wh_id", typeof(int));
            dtItem.Columns.Add("avl_stock", typeof(string));
            dtItem.Columns.Add("issue_qty", typeof(string));
            dtItem.Columns.Add("value", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("remarks", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["UOMName"] = jObject[i]["UOMName"].ToString();
                dtrowLines["Req_Qty"] = jObject[i]["RequiredQty"].ToString();
                dtrowLines["wh_id"] = jObject[i]["WhID"].ToString();
                dtrowLines["avl_stock"] = jObject[i]["AvlStock"].ToString();
                dtrowLines["issue_qty"] = jObject[i]["IssuedQty"].ToString();
                dtrowLines["value"] = jObject[i]["Value"].ToString();
                dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowLines["i_serial"] = jObject[i]["i_serial"].ToString();
                dtrowLines["remarks"] = jObject[i]["Remarks"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtbatchdetail(JArray jObjectBatch)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("comp_id", typeof(int));
            Batch_detail.Columns.Add("br_id", typeof(int));
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("uom_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("avl_batch_qty", typeof(float));
            Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
            Batch_detail.Columns.Add("issue_qty", typeof(float));
            Batch_detail.Columns.Add("lot_id", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));



            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["avl_batch_qty"].ToString();
                if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null || jObjectBatch[i]["ExpiryDate"].ToString() == "undefined")
                {
                    dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                }
                else
                {
                    //dtrowBatchDetailsLines["expiry_date"] = Convert.ToDateTime(jObjectBatch[i]["ExpiryDate"].ToString()).ToString("yyyy-mm-dd");
                    dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                }
                dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["ExpiryDate"].ToString();
                dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                dtrowBatchDetailsLines["lot_id"] = jObjectBatch[i]["LotNo"].ToString();


                dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                Batch_detail.Rows.Add(dtrowBatchDetailsLines);
            }
            return Batch_detail;
        }
        public DataTable dtserialdetail(JArray jObjectSerial)
        {
            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("comp_id", typeof(int));
            Serial_detail.Columns.Add("br_id", typeof(int));
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("uom_id", typeof(int));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("issue_qty", typeof(string));
            Serial_detail.Columns.Add("lot_id", typeof(string));



            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                dtrowSerialDetailsLines["lot_id"] = jObjectSerial[i]["LOTId"].ToString();
                Serial_detail.Rows.Add(dtrowSerialDetailsLines);
            }
            return Serial_detail;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
            dtSubItem.Columns.Add("avl_stk", typeof(string));
            dtSubItem.Columns.Add("subitmType", typeof(string));
            dtSubItem.Columns.Add("OrdQty", typeof(string));
            dtSubItem.Columns.Add("PendingQty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                var SubitmTyp = jObject2[i]["SubItmType"].ToString();
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["avl_stk"] = jObject2[i]["avl_stk"].ToString();
                dtrowItemdetails["subitmType"] = jObject2[i]["SubItmType"].ToString();
                dtrowItemdetails["OrdQty"] = jObject2[i]["OrderQty"].ToString();
                dtrowItemdetails["PendingQty"] = jObject2[i]["PendQty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public FileResult GenratePdfFile(MaterialDispatchModel _Model)
        {/*----------------------------For Print Popup Start-------------------------*/
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _Model.PrintFormat;
            dtr["ShowProdDesc"] = _Model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
            dtr["ShowSubItem"] = _Model.ShowSubItem;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            /*----------------------------For Print Popup End-------------------------*/
            return File(GetPdfData(_Model.MDIssue_No, _Model.MDIssue_Date), "application/pdf", "MaterialDispatch.pdf");
        }
        public byte[] GetPdfData(string Issue_no, string IssueDate)
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
                DataSet Details = _MaterialDispatch_ISERVICES.GetMaterialDispatchDeatils(CompID, BrchID, Issue_no, IssueDate);
                //ViewBag.PageName = "PO";
                //ViewBag.Title = "Material Dispatch";
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();//.Replace("/", "\\'");

                ViewBag.Title = "Job Work Delivery Challan";
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                //ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["issue_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 07-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SubContracting/MaterialDispatch/materialdispatchprint.cshtml"));
                //string DelSchedule = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_PrintReportDeliverySchedule.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    //reader = new StringReader(DelSchedule);
                    //pdfDoc.NewPage();
                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
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
        private List<MaterialDisList> getMDList(MDListModel _MDListModel)
        {
            List<MaterialDisList> _MaterialDisList = new List<MaterialDisList>();
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
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if (_MDListModel.WF_Status != null)
                {
                    wfstatus = _MDListModel.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }

                DataSet DSet = _MaterialDispatch_ISERVICES.GetMDListandSrchDetail(CompID, BrchID, _MDListModel, UserID, wfstatus, DocumentMenuId);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        MaterialDisList _MDList = new MaterialDisList();
                        _MDList.MDIssueNo = dr["MDNO"].ToString();
                        _MDList.MDIssueDate = dr["MDDate"].ToString();
                        _MDList.MDIssueDt = dr["MDDT"].ToString();
                        _MDList.SuppName = dr["supp_name"].ToString();
                        _MDList.JobOrdNo = dr["SrcDocNo"].ToString();
                        _MDList.JobOrdDate = dr["SrcDocDate"].ToString();
                        _MDList.FItemName = dr["FItemName"].ToString();
                        _MDList.FUOM = dr["FUOM"].ToString();
                        _MDList.OprationName = dr["op_name"].ToString();
                        _MDList.ProductName = dr["item_name"].ToString();
                        _MDList.UOM = dr["UOM"].ToString();
                        _MDList.MD_Status = dr["MDStatus"].ToString();
                        _MDList.CreateDate = dr["CreateDate"].ToString();
                        _MDList.ApproveDate = dr["ApproveDate"].ToString();
                        //_MDList.ModifyDate = dr["ModifyDate"].ToString();
                        _MDList.FinStDt = DSet.Tables[2].Rows[0]["findate"].ToString();
                        _MaterialDisList.Add(_MDList);
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
            return _MaterialDisList;
        }

        [HttpPost]
        public ActionResult SearchMDDetail(string SuppId, string OPOutProdctID, string FProdctID, string Fromdate, string Todate, string Status)
        {
            MDListModel _MDListModel = new MDListModel();
            try
            {
                //Session.Remove("WF_Docid");
              //  Session.Remove("WF_status");
                string User_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                var docid = DocumentMenuId;
                _MaterialDisList = new List<MaterialDisList>();
                _MDListModel.SuppID = SuppId;
                _MDListModel.Product_id = OPOutProdctID;
                _MDListModel.FinishProdct_Id = FProdctID;
                _MDListModel.FromDate = Fromdate;
                _MDListModel.ToDate = Todate;
                _MDListModel.Status = Status;
                DataSet DSet = _MaterialDispatch_ISERVICES.GetMDListandSrchDetail(CompID, BrchID, _MDListModel, "", "", "");
                //Session["MDSearch"] = "MD_Search";
                _MDListModel.MDSearch = "MD_Search";

                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    MaterialDisList _MDList = new MaterialDisList();
                    _MDList.MDIssueNo = dr["MDNO"].ToString();
                    _MDList.MDIssueDate = dr["MDDate"].ToString();
                    _MDList.MDIssueDt = dr["MDDT"].ToString();
                    _MDList.SuppName = dr["supp_name"].ToString();
                    _MDList.JobOrdNo = dr["SrcDocNo"].ToString();
                    _MDList.JobOrdDate = dr["SrcDocDate"].ToString();
                    _MDList.FItemName = dr["FItemName"].ToString();
                    _MDList.FUOM = dr["FUOM"].ToString();
                    _MDList.OprationName = dr["op_name"].ToString();
                    _MDList.ProductName = dr["item_name"].ToString();
                    _MDList.UOM = dr["UOM"].ToString();
                    
                    _MDList.MD_Status = dr["MDStatus"].ToString();
                    _MDList.CreateDate = dr["CreateDate"].ToString();
                    _MDList.ApproveDate = dr["ApproveDate"].ToString();
                    //_MDList.ModifyDate = dr["ModifyDate"].ToString();
                    _MaterialDisList.Add(_MDList);
                   
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                _MDListModel.MaterialDispatchList = _MaterialDisList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialDispatchSCList.cshtml", _MDListModel);
        }
        [HttpPost]
        public ActionResult GetMDProductNameInDDLListPage(MaterialDispatchModel _MaterialDispatchModel)
        {
            JsonResult DataRows = null;
            string MDItmName = string.Empty;
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
                    if (string.IsNullOrEmpty(_MaterialDispatchModel.MD_ItemName))
                    {
                        MDItmName = "0";
                    }
                    else
                    {
                        MDItmName = _MaterialDispatchModel.MD_ItemName;
                    }

                    DataSet ProductList = _MaterialDispatch_ISERVICES.BindProductNameInDDL(Comp_ID, Br_ID, MDItmName);
                    DataRow DRow = ProductList.Tables[0].NewRow();

                    DRow[0] = "0";
                    DRow[1] = "All";
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

        public ActionResult DoubleClickOnList(string DocNo, string DocDate, MaterialDispatchModel _MaterialDispatchModel,string ListFilterData,string WF_Status)
        {/*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
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
            var WF_Status1 = "";
            _MaterialDispatchModel.Message = "New";
            _MaterialDispatchModel.Command = "Update";
            _MaterialDispatchModel.TransType = "Update";
            _MaterialDispatchModel.BtnName = "BtnToDetailPage";
            _MaterialDispatchModel.MDIssue_No = DocNo;
            _MaterialDispatchModel.MDIssue_Date = DocDate;
            if (WF_Status != null && WF_Status != "")
            {
                _MaterialDispatchModel.WF_Status1 = WF_Status;
                WF_Status1 = WF_Status;
            }
            var MDCodeURL = DocNo;
            var MdDate = DocDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var command = "Add";
            TempData["ModelData"] = _MaterialDispatchModel;
            TempData["ListFilterData"] = ListFilterData;
            return (RedirectToAction("MaterialDispatchSCDetail", "MaterialDispatchSC", new { MDCodeURL = MDCodeURL, MdDate, TransType, BtnName, command ,WF_Status1 }));

            
        }

        public ActionResult DeleteMDDetails(MaterialDispatchModel _MaterialDispatchModel, string command)
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
                string br_id = Session["BranchId"].ToString();
                string MDNo = _MaterialDispatchModel.MDIssue_No;
                string MDDelete = _MaterialDispatch_ISERVICES.MD_DeleteDetail(_MaterialDispatchModel, CompID, BrchID);

                if (!string.IsNullOrEmpty(MDNo))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string MDNo1 = MDNo.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, MDNo1, Server);
                }
                _MaterialDispatchModel = new MaterialDispatchModel();
                _MaterialDispatchModel.Message = "Deleted";
                _MaterialDispatchModel.Command = "Refresh";
                _MaterialDispatchModel.TransType = "Refresh";
                _MaterialDispatchModel.BtnName = "BtnDelete";
                TempData["ModelData"] = _MaterialDispatchModel;
                return RedirectToAction("MaterialDispatchSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public ActionResult MDApprove(MaterialDispatchModel _MaterialDispatchModel, string ListFilterData1)
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
                string MenuID = DocumentMenuId;

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string MDNo = _MaterialDispatchModel.MDIssue_No;
                string MDDate = _MaterialDispatchModel.MDIssue_Date;
                string A_Status = _MaterialDispatchModel.A_Status;
                string A_Level = _MaterialDispatchModel.A_Level;
                string A_Remarks = _MaterialDispatchModel.A_Remarks;


                string Message = _MaterialDispatch_ISERVICES.MDApproveDetails(CompID, BrchID, MDNo, MDDate, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks);
                string[] FDetail = Message.Split(',');
                string MDSC_No = FDetail[0].ToString();
                if(MDSC_No== "StockNotAvail")
                {
                    _MaterialDispatchModel.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                    _MaterialDispatchModel.Message = "StockNotAvailable";
                }
                else
                {
                    string MDSC_Date = FDetail[1].ToString();
                    string ApMessage = FDetail[2].ToString();
                    if (ApMessage == "A")
                    {
                        _MaterialDispatchModel.Message = "Approved";
                    }
                }
                

                //string MDSC_No = Message.Split(',')[0].Trim();
                //string ApMessage = Message.Split(',')[2].Trim();
                //string MDSC_Date = Message.Split(',')[1].Trim();

                //if (ApMessage == "A")
                //{
                //    _MaterialDispatchModel.Message = "Approved";
                //}
                //if (Message == "StockNotAvail")
                //{
                //    _MaterialDispatchModel.Message = "StockNotAvail";
                //}
                _MaterialDispatchModel.TransType = "Update";
                _MaterialDispatchModel.Command = "Approve";
                _MaterialDispatchModel.AppStatus = "D";
                _MaterialDispatchModel.BtnName = "BtnEdit";

                //var MDCodeURL = MDSC_No;
                var MDCodeURL = MDNo;
                var MdDate = _MaterialDispatchModel.MDIssue_Date;
                var TransType = _MaterialDispatchModel.TransType;
                var BtnName = _MaterialDispatchModel.BtnName;
                var command = _MaterialDispatchModel.Command;
                TempData["ModelData"] = _MaterialDispatchModel;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("MaterialDispatchSCDetail", new { MDCodeURL = MDCodeURL, MdDate, TransType, BtnName, command }));


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1,string WF_Status)
        {
            MaterialDispatchModel _MaterialDispatchModel = new MaterialDispatchModel();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _MaterialDispatchModel.MDIssue_No = jObjectBatch[i]["MDNo"].ToString();
                    _MaterialDispatchModel.MDIssue_Date = jObjectBatch[i]["MDDate"].ToString();

                    _MaterialDispatchModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _MaterialDispatchModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _MaterialDispatchModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                }
            }
            if(_MaterialDispatchModel.A_Status != "Approve")
            {
                _MaterialDispatchModel.A_Status = "Approve";
            }
            MDApprove(_MaterialDispatchModel, ListFilterData1);
            var WF_Status1 = "";
            if (WF_Status != null && WF_Status != "")
            {
                _MaterialDispatchModel.WF_Status1 = WF_Status;
                WF_Status1 = WF_Status;
            }
            var MDCodeURL = _MaterialDispatchModel.MDIssue_No;
            var MdDate = _MaterialDispatchModel.MDIssue_Date;
            var TransType = _MaterialDispatchModel.TransType;
            var BtnName = _MaterialDispatchModel.BtnName;
            var command = _MaterialDispatchModel.Command;
            TempData["ModelData"] = _MaterialDispatchModel;
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("MaterialDispatchSCDetail", new { MDCodeURL = MDCodeURL, MdDate, TransType, BtnName, command, WF_Status1 }));

            
        }
        
        public ActionResult ToRefreshByJS(string FrwdDtList, string ListFilterData1, string WF_Status)
        {
            MaterialDispatchModel _MaterialDispatchModel = new MaterialDispatchModel();
            var WF_Status1 = "";
            if (FrwdDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(FrwdDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _MaterialDispatchModel.MDIssue_No = jObjectBatch[i]["MDNo"].ToString();
                    _MaterialDispatchModel.MDIssue_Date = jObjectBatch[i]["MDDate"].ToString();
                    _MaterialDispatchModel.TransType = "Update";
                    _MaterialDispatchModel.BtnName = "BtnToDetailPage";
                   if(WF_Status != null && WF_Status != "")
                    {
                        _MaterialDispatchModel.WF_Status1 = WF_Status;
                        WF_Status1 = WF_Status;
                    }
                    TempData["ModelData"] = _MaterialDispatchModel;
                }
            }
            var MDCodeURL = _MaterialDispatchModel.MDIssue_No;
            var MdDate = _MaterialDispatchModel.MDIssue_Date;
            var TransType = _MaterialDispatchModel.TransType;
            var BtnName = _MaterialDispatchModel.BtnName;
            var command = "Refresh";
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("MaterialDispatchSCDetail", new { MDCodeURL = MDCodeURL, MdDate, TransType, BtnName, command, WF_Status1 }));
            
        }
        public ActionResult GetMDDashbordList(string docid, string status)
        {

            //Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("MaterialDispatchSC",new { WF_Status });
        }

        private string CheckDeliveryNoteSCAgainstMaterialDispatch(MaterialDispatchModel _MaterialDispatchModel)
        {
            try
            {
                string str = string.Empty;
                if (Session["BranchId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }


                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                str = _MaterialDispatch_ISERVICES.ChkDNSCDagainstMDSC(CompID, BrchID, _MaterialDispatchModel.MDIssue_No, _MaterialDispatchModel.MDIssue_Date).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        public ActionResult GetAutoCompleteSearchSuppList(MDListModel _MDListModel)
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
                if (string.IsNullOrEmpty(_MDListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _MDListModel.SuppName;
                }
                CustList = _MaterialDispatch_ISERVICES.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _MDListModel.SupplierNameList = _SuppList;
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
                DataSet result = _MaterialDispatch_ISERVICES.GetSuppAddrDetailDAL(Supp_id, Comp_ID);
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
        public JsonResult GetJobORDDocList(string Supp_IdNm)
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
                DataSet Deatils = _MaterialDispatch_ISERVICES.GetJobORDDocList(Supp_IdNm, Comp_ID, Br_ID);

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
        public ActionResult getMaterialInputItemDetailByJONumber(string JONo, string JODate, string OrdQty, string DispatchQty)
        {
            string BrchID = string.Empty;
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
                DataSet ds = _MaterialDispatch_ISERVICES.getMaterialInputItemDetailByJONumber(CompID, BrchID, JODate, JONo, OrdQty, DispatchQty);
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
        public ActionResult getWarehouseWiseItemStock(string ItemId, string WarehouseId,string JONo,string JODate,string OrdQty, string DispatchQty, string UomId = null)
        {
            try
            {
                JsonResult DataRows = null;
                string Wh_ID, ItemID, LotID, BatchNo = string.Empty;
                //CompID = CompId;
                //BrID = BranchId;
                string CompID = "", BrID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                Wh_ID = WarehouseId;
                ItemID = ItemId;
                LotID = null;
                BatchNo = null;
                DataSet Stock = _MaterialDispatch_ISERVICES.getWarehouseWiseItemStock(CompID, BrID, Wh_ID, ItemID, JONo, JODate, OrdQty, DispatchQty, UomId ,LotID, BatchNo);
                //return Json(Stock, JsonRequestBehavior.AllowGet);
                DataRows = Json(JsonConvert.SerializeObject(Stock));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult GetDetailofJobOrdNoList(string JobordNo, string JobOrddate,string Disp_No)
        {
            string BrchID = string.Empty;
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
                DataSet ds = _MaterialDispatch_ISERVICES.GetDetailofJobOrdNoList(CompID, BrchID, JobordNo, JobOrddate, Disp_No);
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
        [HttpPost]
        public JsonResult GetWarehouseList()
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
                DataSet result = _MaterialDispatch_ISERVICES.GetWarehouseList(Comp_ID, Br_ID);
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
        [NonAction]
        private void getWarehouse(MaterialDispatchModel _MaterialDispatchModel)
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
                DataSet result = _MaterialDispatch_ISERVICES.GetWarehouseList(Comp_ID, Br_ID);
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
                _MaterialDispatchModel.WarehouseList = _WarehouseList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }

        public ActionResult getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId
            , string SelectedItemSerial, string DMenuId, string Command, string TransType, string Mrd_Status)
        {
            try
            {
                DataSet ds = new DataSet();
                if (ItemId != "")
                {
                    if (Session["CompId"] != null)
                    {
                        CompId = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BranchId = Session["BranchId"].ToString();
                    }

                    ds = _MaterialDispatch_ISERVICES.getItemstockSerialWise(ItemId, WarehouseId, CompId, BranchId);
                }
                if (SelectedItemSerial != null && SelectedItemSerial != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemSerial);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString() == ds.Tables[0].Rows[i]["item_id"].ToString() && item.GetValue("LOTId").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("SerialNO").ToString() == ds.Tables[0].Rows[i]["serial_no"].ToString())
                            {
                                ds.Tables[0].Rows[i]["SerailSelected"] = "Y";
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockSerialWise = ds.Tables[0];

                
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = Mrd_Status;

                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string IssueNo, string IssueDate, string ItemID
            , string DMenuId, string Command, string TransType)
        {
            try
            {
                DataSet ds = new DataSet();
                string CompID = string.Empty;
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                ds = _MaterialDispatch_ISERVICES.getItemstockSerialWiseAfterStockUpdate(CompID, br_id, IssueNo, IssueDate, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
              
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId
            , string SelectedItemdetail, string DMenuId, string Command, string TransType, string MD_Status,string HdnMessage)
        {
            try
            {
                DataSet ds = new DataSet();
                if (ItemId != "")
                {
                    if (Session["CompId"] != null)
                    {
                        CompId = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BranchId = Session["BranchId"].ToString();
                    }
                    ds = _MaterialDispatch_ISERVICES.getItemStockBatchWise(ItemId, WarehouseId, CompId, BranchId);
                }

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())//
                        {
                            if (item.GetValue("ItemId").ToString() == ds.Tables[0].Rows[i]["item_id"].ToString() && item.GetValue("LotNo").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("BatchNo").ToString() == ds.Tables[0].Rows[i]["batch_no"].ToString())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty");
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockBatchWise = ds.Tables[0];
                }

               
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = MD_Status;
                ViewBag.Message = HdnMessage;

                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemStockBatchWiseAfterStockUpadte(string IssueNo, string IssueDate, string ItemID
            , string DMenuId, string Command, string TransType)
        {
            try
            {
                DataSet ds = new DataSet();
                string CompID = string.Empty;
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                ds = _MaterialDispatch_ISERVICES.getItemStockBatchWiseAfterStockUpdate(CompID, br_id, IssueNo, IssueDate, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        
        /*---------------START SUB ITEM SECTION------------------*/
        
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string Flag, string Status, 
            string JobOrdNo, string JobOrdDt, string OrdQty, string DispatchQty,string Wh_id,string DocNo,string DocDt,string JOTyp)
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
                int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                if (Status == "D" || Status == "F" || Status == "")
                {
                    //if(Flag== "MDOrdQty" || Flag == "MDPendQty"|| Flag == "MDReqQty"|| Flag == "MDIssueQty"|| Flag == "MDDispQty")
                    //{
                        var flagwh = "wh";
                        dt = _MaterialDispatch_ISERVICES.MD_GetSubItemDetails(CompID, BrchID, Item_id, JobOrdNo, JobOrdDt, Flag, OrdQty, DispatchQty, Wh_id, flagwh,DocNo, Status).Tables[0];
                        if (Flag == "MDDispQty")
                        {
                            dt.Columns.Add("avl_stk", typeof(string));
                            //dt.Columns.Add("Ord_Qty", typeof(string));
                            //dt.Columns.Add("Pend_Qty", typeof(string));
                        }
                    //}
                    if (Flag == "MDIssueQty")
                    {
                        Flag = "MDIssueQty";
                    }
                    else if(Flag == "MDDispQty")
                    {
                        Flag = "MDDispQty";
                    }
                    else
                    {
                        Flag = "Quantity";
                    }
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                                //if (Flag == "MDDispQty")
                                //{
                                //    dt.Rows[i]["Ord_Qty"] = cmn.ConvertToDecimal(item.GetValue("ord_qty").ToString(), DecDgt);
                                //    dt.Rows[i]["Pend_Qty"] = cmn.ConvertToDecimal(item.GetValue("pend_qty").ToString(), DecDgt);
                                //}
                                //if (Status == "" && JOTyp == "Direct")
                                //{

                                //}
                                //else
                                //{
                                    if (Status == "")
                                    {
                                        dt.Rows[i]["avl_stk"] = cmn.ConvertToDecimal(item.GetValue("avl_stk").ToString(), DecDgt);
                                    }
                                //}
                            }
                        }
                    }
                }
                else
                {
                    var flagwh = "wh";
                    dt = _MaterialDispatch_ISERVICES.MD_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, DocNo, DocDt, JobOrdNo, JobOrdDt, Flag, OrdQty, DispatchQty, Wh_id, flagwh, JOTyp).Tables[0];
                    if (Flag == "MDIssueQty")
                    {
                        Flag = "MDIssueQty";
                    }
                    else if (Flag == "MDDispQty")
                    {
                        Flag = "MDDispQty";
                    }
                    else
                    {
                        Flag = "Quantity";
                    }
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    ShowStock = Flag == "MDIssueQty" ? "Y" : "N",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    _subitemPageName = "CNF",
                    decimalAllowed = "Y"
                };
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        /*---------------END SUB ITEM SECTION------------------*/

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

                //ViewBag.GstApplicable = "N";
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
    }

}