using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobOrder;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.JobOrder;
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
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SubContracting.JobOrder
{
    public class JobOrderSCController : Controller
    {
        string CompID, BrchID, UserID, language, create_id = String.Empty;
        string DocumentMenuId = "105108101", title;
        List<JobOrderList> _JobOrderList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        JobOrder_ISERVICES _JobOrder_ISERVICES;
        DataSet dtSet;
        DataTable dtSubitm;
        CommonController cmn = new CommonController();
        public JobOrderSCController(Common_IServices _Common_IServices, JobOrder_ISERVICES _JobOrder_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._JobOrder_ISERVICES = _JobOrder_ISERVICES;
        }
        // GET: ApplicationLayer/JobOrderSC
        public ActionResult JobOrderSC(string WF_Status)
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
                JOListModel _JOListModel = new JOListModel();
                _JOListModel.WF_Status = WF_Status;
                //if (Session["WF_Docid"] != null)
                //{
                //    _JOListModel.wfdocid = Session["WF_Docid"].ToString();
                //}
                //else
                //{
                //    _JOListModel.wfdocid = "0";
                //}
                //if (Session["WF_status"] != null)
                //{
                //    _JOListModel.wfstatus = Session["WF_status"].ToString();
                //}
                //else
                //{
                //    _JOListModel.wfstatus = "";
                //}

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                CommonPageDetails();
                _JOListModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;
                _JOListModel.FromDate = startDate;
                _JOListModel.ToDate = CurrentDate;
                dtSet = JOGetAllData(_JOListModel);
                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        _JOListModel.SuppID = a[0].Trim();
                        _JOListModel.Product_id = a[1].Trim();
                        _JOListModel.FromDate = a[2].Trim();
                        _JOListModel.ToDate = a[3].Trim();
                        _JOListModel.Status = a[4].Trim();
                        _JOListModel.FinishProdct_Id = a[5].Trim();
                        if (_JOListModel.Status == "0")
                        {
                            _JOListModel.Status = null;
                        }
                        _JOListModel.JobOrdList = getJOList(_JOListModel);
                        _JOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                }
                else
                {
                    _JOListModel.FromDate = startDate;
                    _JobOrderList = new List<JobOrderList>();
                    if (dtSet.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtSet.Tables[1].Rows)
                        {
                            JobOrderList _JOList = new JobOrderList();
                            _JOList.SourceType = dr["src_type"].ToString();
                            _JOList.OrderNo = dr["OrderNo"].ToString();
                            _JOList.OrderDate = dr["JODate"].ToString();
                            _JOList.OrderDt = dr["JODT"].ToString();
                            _JOList.SuppName = dr["supp_name"].ToString();
                            _JOList.Valid_Upto = dr["ValidUpto"].ToString();
                            _JOList.FgItemName = dr["FgItemName"].ToString();
                            _JOList.FgUomName = dr["FgUomName"].ToString();
                            _JOList.ProductName = dr["item_name"].ToString();
                            _JOList.UOM = dr["UOM"].ToString();
                            _JOList.Quantity = dr["Quantity"].ToString();
                            _JOList.ProductionOrdNo = dr["SrcDocNo"].ToString();
                            _JOList.ProductionOrdDate = dr["SrcDocDate"].ToString();
                            _JOList.OprationName = dr["op_name"].ToString();

                            _JOList.JO_Status = dr["JOStatus"].ToString();
                            _JOList.CreateDate = dr["CreateDate"].ToString();
                            _JOList.ApproveDate = dr["ApproveDate"].ToString();
                            _JOList.ModifyDate = dr["ModifyDate"].ToString();
                            _JobOrderList.Add(_JOList);
                        }
                    }
                    _JOListModel.JobOrdList = _JobOrderList;
                }
                /*commented by Hina on 05-04-2024 to combine all list Procedure  in single Procedure*/
                //_JOListModel.JobOrdList = getJOList(_JOListModel);
                //GetAutoCompleteSearchSuppList(_JOListModel);
                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (DataRow dr in dtSet.Tables[0].Rows)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = dr["supp_id"].ToString();
                    _SuppDetail.supp_name = dr["supp_name"].ToString();
                    _SuppList.Add(_SuppDetail);
                }
                _JOListModel.SupplierNameList = _SuppList;
                _SuppList.Insert(0, new SupplierName() { supp_id = "0", supp_name = "All" });
                _JOListModel.SupplierNameList = _SuppList;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _JOListModel.StatusList = statusLists;

                _JOListModel.Title = title;
                //Session["JOSearch"] = "0";
                _JOListModel.JOSearch = null;
                return View("~/Areas/ApplicationLayer/Views/SubContracting/JobOrder/JobOrderList.cshtml", _JOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult AddJobOrderSCDetail()
        {
            JobOrderModel _JobOrderModel = new JobOrderModel();
            _JobOrderModel.Message = "New";
            _JobOrderModel.Command = "Add";
            _JobOrderModel.AppStatus = "D";
            ViewBag.DocumentStatus = _JobOrderModel.AppStatus;
            _JobOrderModel.TransType = "Save";
            _JobOrderModel.BtnName = "BtnAddNew";
            
            TempData["ModelData"] = _JobOrderModel;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("JobOrderSC");
            }
            /*End to chk Financial year exist or not*/
            CommonPageDetails();
            return RedirectToAction("JobOrderSCDetail", "JobOrderSC");
        }
        public ActionResult JobOrderSCDetail(JobOrderModel _JobOrderModel1, string JOCodeURL, string JoDate, string TransType, string BtnName, string command, string WF_Status1)
        {
            try
            {  
                ViewBag.DocID = DocumentMenuId;
                CommonPageDetails();
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                //var commCont = new CommonController(_Common_IServices);
                //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, JoDate) == "TransNotAllow")
                //{
                //    //TempData["Message2"] = "TransNotAllow";
                //    ViewBag.Message = "TransNotAllow";
                //}
                var _JobOrderModel = TempData["ModelData"] as JobOrderModel;
                if (_JobOrderModel != null)
                {
                    /*----------Attachment Section Start----------*/

                    _JobOrderModel.AttachMentDetailItmStp = null;
                    _JobOrderModel.Guid = null;

                    /*----------Attachment Section End----------*/
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
                    string ValDigit1 = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit1 = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit1 = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));

                    //CommonPageDetails();
                    _JobOrderModel.Title = title;
                    _JobOrderModel.ValDigit = ValDigit1;
                    _JobOrderModel.QtyDigit = QtyDigit1;
                    _JobOrderModel.RateDigit = RateDigit1;
                    ViewBag.ValDigit = ValDigit1;
                    ViewBag.QtyDigit = QtyDigit1;
                    ViewBag.RateDigit = RateDigit1;
                    List<SupplierName> suppLists1 = new List<SupplierName>();
                    suppLists1.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _JobOrderModel.SupplierNameList = suppLists1;

                    List<ProdOrdNoList> srcDocNoLists1 = new List<ProdOrdNoList>();
                    srcDocNoLists1.Add(new ProdOrdNoList { ProdOrdnoId = "0", ProdOrdnoVal = "---Select---" });
                    _JobOrderModel.prdordNoLists = srcDocNoLists1;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _JobOrderModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_JobOrderModel.TransType == "Update" || _JobOrderModel.Command == "Edit")

                    {
                        string JOSC_NO = _JobOrderModel.JO_No;
                        string JOSC_Date = _JobOrderModel.JO_Date;
                        DataSet ds = _JobOrder_ISERVICES.GetJODetailEditUpdate(CompID, BrchID, JOSC_NO, JOSC_Date, UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            _JobOrderModel.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                            if(_JobOrderModel.SourceType=="D")
                            {
                                _JobOrderModel.SourceType = "Direct";
                            }
                            else
                            {
                                _JobOrderModel.SourceType = "PrdOrd";
                            }
                            _JobOrderModel.JO_No = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _JobOrderModel.DocNoAttach = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _JobOrderModel.JO_Date = ds.Tables[0].Rows[0]["scjob_date"].ToString();
                            _JobOrderModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _JobOrderModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists1.Add(new SupplierName { supp_id = _JobOrderModel.SuppID, supp_name = _JobOrderModel.SuppName });
                            _JobOrderModel.SupplierNameList = suppLists1;
                            _JobOrderModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _JobOrderModel.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["add_id"].ToString());
                            _JobOrderModel.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _JobOrderModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _JobOrderModel.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _JobOrderModel.ProducOrdNum = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _JobOrderModel.ProducOrd_Num = ds.Tables[0].Rows[0]["src_doc_number"].ToString();

                            srcDocNoLists1.Add(new ProdOrdNoList { ProdOrdnoId = _JobOrderModel.ProducOrdNum, ProdOrdnoVal = _JobOrderModel.ProducOrdNum });
                            _JobOrderModel.prdordNoLists = srcDocNoLists1;
                            _JobOrderModel.ForAmmendendBtn = ds.Tables[15].Rows[0]["flag"].ToString();
                            _JobOrderModel.Amendment = ds.Tables[16].Rows[0]["Amendment"].ToString();
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _JobOrderModel.ProducOrdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }

                            _JobOrderModel.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _JobOrderModel.FinishProduct = ds.Tables[0].Rows[0]["FgItemName"].ToString();
                            _JobOrderModel.Fproduct_id = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _JobOrderModel.Fproduct_name = ds.Tables[0].Rows[0]["FgItemName"].ToString();
                            if(_JobOrderModel.SourceType=="Direct")
                            {
                                _JobOrderModel.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                            }
                           
                            _JobOrderModel.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();
                            _JobOrderModel.FinishUom = ds.Tables[0].Rows[0]["FgUomName"].ToString();
                            _JobOrderModel.JOOrderQty = ds.Tables[0].Rows[0]["qty"].ToString();
                            _JobOrderModel.ItmWeight = ds.Tables[0].Rows[0]["item_wgt"].ToString();
                            _JobOrderModel.Operation_Name = ds.Tables[0].Rows[0]["OpName"].ToString();
                            _JobOrderModel.OpId = ds.Tables[0].Rows[0]["op_id"].ToString();
                            _JobOrderModel.Remarks = ds.Tables[0].Rows[0]["scjob_rem"].ToString();
                            _JobOrderModel.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit1);
                            _JobOrderModel.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit1);
                            _JobOrderModel.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit1);
                            _JobOrderModel.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit1);
                            _JobOrderModel.Created_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _JobOrderModel.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _JobOrderModel.Created_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _JobOrderModel.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _JobOrderModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _JobOrderModel.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _JobOrderModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _JobOrderModel.StatusName = ds.Tables[0].Rows[0]["JoStatus"].ToString();
                            _JobOrderModel.JobOrdStatus = ds.Tables[0].Rows[0]["scjob_status"].ToString().Trim();
                            if (_JobOrderModel.JobOrdStatus == "PD")
                            {
                                _JobOrderModel.DispatchQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["disqty"]);
                            }
                            if (_JobOrderModel.SourceType == "Direct")
                            {
                                _JobOrderModel.OrderQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["qty"].ToString().Trim());
                            }
                            else
                            {
                                _JobOrderModel.OrderQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["jobqty"].ToString().Trim());
                            }
                            _JobOrderModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _JobOrderModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled

                            ViewBag.ItemDelSchdetails = ds.Tables[4];
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.AttechmentDetails = ds.Tables[11];
                            ViewBag.InputItemDetailsList = ds.Tables[12];
                            ViewBag.OutPutItemDetailsList = ds.Tables[13];
                            ViewBag.SubItemDetails = ds.Tables[14];
                            ViewBag.QtyDigit = QtyDigit1;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["scjob_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _JobOrderModel.Cancelled = true;
                        }
                        else if (Statuscode == "FC")
                        {
                            _JobOrderModel.FClosed = true;
                        }
                        else
                        {
                            _JobOrderModel.Cancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        //if (ForceClose == "Y")
                        //{
                        //    _JobOrderModel.FClosed = true;
                        //}
                        //else
                        //{
                        //    _JobOrderModel.FClosed = false;
                        //}
                        _JobOrderModel.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        if (ViewBag.AppLevel != null && _JobOrderModel.Command != "Edit")
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
                                    _JobOrderModel.BtnName = "Refresh";
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
                                        _JobOrderModel.BtnName = "BtnToDetailPage";
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
                                        _JobOrderModel.BtnName = "BtnToDetailPage";

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
                                    _JobOrderModel.BtnName = "BtnToDetailPage";
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
                                        _JobOrderModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
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
                                    _JobOrderModel.BtnName = "BtnToDetailPage";
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
                                    _JobOrderModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _JobOrderModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _JobOrderModel.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }
                    if (_JobOrderModel.BtnName != null)
                    {
                        _JobOrderModel.BtnName = _JobOrderModel.BtnName;
                    }
                    _JobOrderModel.TransType = _JobOrderModel.TransType;
                    _JobOrderModel.TranstypAttach = _JobOrderModel.TransType;
                    if (_JobOrderModel.DocumentStatus == null)
                    {
                        _JobOrderModel.DocumentStatus = "D";
                    }
                    else
                    {
                        _JobOrderModel.DocumentStatus = _JobOrderModel.DocumentStatus;
                    }
                    ViewBag.DocumentStatus = _JobOrderModel.DocumentStatus;
                    // ViewBag.FinstDt = Session["FinStDt"].ToString();
                    if (_JobOrderModel.Amend == "Amend")
                    {
                        _JobOrderModel.JobOrdStatus = "D";
                        _JobOrderModel.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                    }
                    _JobOrderModel.UserID = UserID;
                    if (_JobOrderModel.Amendment != "Amendment" && _JobOrderModel.Amendment != "" && _JobOrderModel.Amendment != null)
                    {
                        _JobOrderModel.BtnName = "Refresh";
                        _JobOrderModel.wfDisableAmnd = "wfDisableAmnd";
                    }

                    return View("~/Areas/ApplicationLayer/Views/SubContracting/JobOrder/JobOrderDetail.cshtml", _JobOrderModel);
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
                    /*----------Attachment Section Start----------*/

                    _JobOrderModel1.AttachMentDetailItmStp = null;
                    _JobOrderModel1.Guid = null;

                    /*----------Attachment Section End----------*/
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _JobOrderModel1.ValDigit = ValDigit;
                    _JobOrderModel1.QtyDigit = QtyDigit;
                    _JobOrderModel1.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.DocumentStatus = "D";
                    _JobOrderModel1.WF_Status1 = WF_Status1;
                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _JobOrderModel1.SupplierNameList = suppLists;

                    List<ProdOrdNoList> srcDocNoLists = new List<ProdOrdNoList>();
                    srcDocNoLists.Add(new ProdOrdNoList { ProdOrdnoId = "0", ProdOrdnoVal = "---Select---" });
                    _JobOrderModel1.prdordNoLists = srcDocNoLists;
                    if (TempData["ListFilterData"] != null)
                    {
                        _JobOrderModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    if (_JobOrderModel1.TransType == "Update" || _JobOrderModel1.Command == "Edit")

                    {
                        string JOSC_NO = JOCodeURL;
                        string JOSC_Date = JoDate;
                        DataSet ds = _JobOrder_ISERVICES.GetJODetailEditUpdate(CompID, BrchID, JOSC_NO, JOSC_Date, UserID, DocumentMenuId);


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            _JobOrderModel1.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                            if (_JobOrderModel1.SourceType == "D")
                            {
                                _JobOrderModel1.SourceType = "Direct";
                            }
                            else
                            {
                                _JobOrderModel1.SourceType = "PrdOrd";
                            }
                            _JobOrderModel1.JO_No = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _JobOrderModel1.DocNoAttach = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _JobOrderModel1.JO_Date = ds.Tables[0].Rows[0]["scjob_date"].ToString();
                            _JobOrderModel1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _JobOrderModel1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _JobOrderModel1.SuppID, supp_name = _JobOrderModel1.SuppName });
                            _JobOrderModel1.SupplierNameList = suppLists;
                            _JobOrderModel1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _JobOrderModel1.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["add_id"].ToString());
                            _JobOrderModel1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _JobOrderModel1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _JobOrderModel1.ValidUpto = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _JobOrderModel1.ProducOrdNum = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _JobOrderModel1.ProducOrd_Num = ds.Tables[0].Rows[0]["src_doc_number"].ToString();

                            srcDocNoLists.Add(new ProdOrdNoList { ProdOrdnoId = _JobOrderModel1.ProducOrdNum, ProdOrdnoVal = _JobOrderModel1.ProducOrdNum });
                            _JobOrderModel1.prdordNoLists = srcDocNoLists;
                            _JobOrderModel1.ForAmmendendBtn = ds.Tables[15].Rows[0]["flag"].ToString();
                            _JobOrderModel1.Amendment = ds.Tables[16].Rows[0]["Amendment"].ToString();

                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _JobOrderModel1.ProducOrdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            _JobOrderModel1.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _JobOrderModel1.FinishProduct = ds.Tables[0].Rows[0]["FgItemName"].ToString();
                            _JobOrderModel1.Fproduct_id = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _JobOrderModel1.Fproduct_name = ds.Tables[0].Rows[0]["FgItemName"].ToString();
                            if (_JobOrderModel1.SourceType == "Direct")
                            {
                                _JobOrderModel1.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                            }
                            _JobOrderModel1.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();
                            _JobOrderModel1.FinishUom = ds.Tables[0].Rows[0]["FgUomName"].ToString();
                            _JobOrderModel1.JOOrderQty = ds.Tables[0].Rows[0]["qty"].ToString();
                            _JobOrderModel1.ItmWeight = ds.Tables[0].Rows[0]["item_wgt"].ToString();

                            _JobOrderModel1.Operation_Name = ds.Tables[0].Rows[0]["OpName"].ToString();
                            _JobOrderModel1.OpId = ds.Tables[0].Rows[0]["op_id"].ToString();

                            _JobOrderModel1.Remarks = ds.Tables[0].Rows[0]["scjob_rem"].ToString();
                            _JobOrderModel1.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _JobOrderModel1.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _JobOrderModel1.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _JobOrderModel1.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _JobOrderModel1.Created_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _JobOrderModel1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _JobOrderModel1.Created_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _JobOrderModel1.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _JobOrderModel1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _JobOrderModel1.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _JobOrderModel1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _JobOrderModel1.StatusName = ds.Tables[0].Rows[0]["JoStatus"].ToString();
                            _JobOrderModel1.JobOrdStatus = ds.Tables[0].Rows[0]["scjob_status"].ToString().Trim();
                            if (_JobOrderModel1.JobOrdStatus == "PD")
                            {

                                //_JobOrderModel1.OrderQty = Convert.ToInt32(ds.Tables[0].Rows[0]["jobqty"].ToString().Trim());
                                _JobOrderModel1.DispatchQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["disqty"]);

                            }
                            if (_JobOrderModel1.SourceType == "Direct")
                            {
                                _JobOrderModel1.OrderQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["qty"].ToString().Trim());
                            }
                            else
                            {
                                _JobOrderModel1.OrderQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["jobqty"]);
                            }
                            _JobOrderModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _JobOrderModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled

                            ViewBag.ItemDelSchdetails = ds.Tables[4];
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.AttechmentDetails = ds.Tables[11];
                            ViewBag.InputItemDetailsList = ds.Tables[12];
                            ViewBag.OutPutItemDetailsList = ds.Tables[13];
                            ViewBag.SubItemDetails = ds.Tables[14];
                            //  _JobOrderModel1.ServcOrdQty = ds.Tables[0].Rows[0]["item_type"].ToString();
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["scjob_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _JobOrderModel1.Cancelled = true;
                        }
                        else
                        {
                            _JobOrderModel1.Cancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        if (ForceClose == "Y")
                        {
                            _JobOrderModel1.FClosed = true;
                        }
                        else
                        {
                            _JobOrderModel1.FClosed = false;
                        }
                        _JobOrderModel1.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        if (ViewBag.AppLevel != null && _JobOrderModel1.Command != "Edit")
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
                                    _JobOrderModel1.BtnName = "Refresh";
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
                                        _JobOrderModel1.BtnName = "BtnToDetailPage";
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
                                        _JobOrderModel1.BtnName = "BtnToDetailPage";
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
                                    _JobOrderModel1.BtnName = "BtnToDetailPage";
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
                                        _JobOrderModel1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
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
                                    _JobOrderModel1.BtnName = "BtnToDetailPage";
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
                                    _JobOrderModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _JobOrderModel1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _JobOrderModel1.BtnName = "Refresh";
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
                    var JOCode = "";
                    if (JOCodeURL != null)
                    {
                        JOCode = JOCodeURL;
                        _JobOrderModel1.JO_No = JOCodeURL;
                    }
                    else
                    {
                        JOCode = _JobOrderModel1.JO_No;
                    }
                    if (TransType != null)
                    {
                        _JobOrderModel1.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _JobOrderModel1.Command = command;
                    }

                    if (_JobOrderModel1.BtnName == null && _JobOrderModel1.Command == null)
                    {
                        _JobOrderModel1.BtnName = "AddNew";
                       _JobOrderModel1.Command = "Add";
                        
                        _JobOrderModel1.AppStatus = "D";
                        ViewBag.DocumentStatus = _JobOrderModel1.AppStatus;
                        _JobOrderModel1.TransType = "Save";
                        _JobOrderModel1.BtnName = "BtnAddNew";

                    }
                    if (_JobOrderModel1.SourceType == "Direct")
                    {
                        _JobOrderModel1.Command = null;
                    }

                    if (_JobOrderModel1.BtnName != null)
                    {
                        _JobOrderModel1.BtnName = _JobOrderModel1.BtnName;
                    }
                    if (_JobOrderModel1.TransType == null || _JobOrderModel1.BtnName == null)
                    {
                        _JobOrderModel1.TransType = "Update";
                        _JobOrderModel1.BtnName = "BtnEdit";
                        if (_JobOrderModel1.SourceType == "Direct")
                        {
                            _JobOrderModel1.Command = "Edit";
                        }

                    }
                    if (_JobOrderModel1.JobOrdStatus == "PD" || _JobOrderModel1.JobOrdStatus == "DP" || _JobOrderModel1.JobOrdStatus == "PDL" || _JobOrderModel1.JobOrdStatus == "DL" || _JobOrderModel1.JobOrdStatus == "IN" || _JobOrderModel1.JobOrdStatus == "PN")
                    {

                        _JobOrderModel1.BtnName = "Refresh";
                    }
                    _JobOrderModel1.TransType = _JobOrderModel1.TransType;
                    _JobOrderModel1.TranstypAttach = _JobOrderModel1.TransType;
                    if (_JobOrderModel1.DocumentStatus != null)
                    {
                        _JobOrderModel1.DocumentStatus = _JobOrderModel1.DocumentStatus;
                        ViewBag.DocumentStatus = _JobOrderModel1.DocumentStatus;
                    }

                    _JobOrderModel1.Title = title;
                    // ViewBag.FinstDt = Session["FinStDt"].ToString();
                    if (_JobOrderModel1.Amend == "Amend")
                    {
                        _JobOrderModel1.JobOrdStatus = "D";
                        _JobOrderModel1.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                    }
                    _JobOrderModel1.UserID = UserID;
                    if (_JobOrderModel1.Amendment != "Amendment" && _JobOrderModel1.Amendment != "" && _JobOrderModel1.Amendment != null)
                    {
                        _JobOrderModel1.BtnName = "Refresh";
                        _JobOrderModel1.wfDisableAmnd = "wfDisableAmnd";
                    }

                    return View("~/Areas/ApplicationLayer/Views/SubContracting/JobOrder/JobOrderDetail.cshtml", _JobOrderModel1);
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
        public ActionResult JobOrderBtnCommand(JobOrderModel _JobOrderModel, string command)
        {
            try
            {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/

                if (_JobOrderModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        JobOrderModel _JobOrderModelAdd = new JobOrderModel();
                        _JobOrderModelAdd.Message = "New";
                        _JobOrderModelAdd.Command = "Add";
                        _JobOrderModelAdd.AppStatus = "D";
                        _JobOrderModelAdd.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _JobOrderModel.DocumentStatus;
                        _JobOrderModelAdd.TransType = "Save";
                        _JobOrderModelAdd.BtnName = "BtnAddNew";
                        // _JobOrderModel = new JobOrderModel();
                        //_JobOrderModel.Message = "New";
                        //_JobOrderModel.Command = "Add";
                        //_JobOrderModel.AppStatus = "D";
                        //_JobOrderModel.DocumentStatus = "D";
                        //ViewBag.DocumentStatus = _JobOrderModel.DocumentStatus;
                        //_JobOrderModel.TransType = "Save";
                        //_JobOrderModel.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _JobOrderModelAdd;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_JobOrderModel.JO_No))
                                return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                            else
                             _JobOrderModelAdd.Command = "Refresh";
                            _JobOrderModelAdd.TransType = "Refresh";
                            _JobOrderModelAdd.BtnName = "Refresh";
                            _JobOrderModelAdd.DocumentStatus = null;
                            TempData["ModelData"] = _JobOrderModelAdd;
                            return RedirectToAction("JobOrderSCDetail", "JobOrderSC");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("JobOrderSCDetail", "JobOrderSC");

                    case "Edit":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        //string joDt = _JobOrderModel.JO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, joDt) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        var TransType = "";
                        var BtnName = "";
                        if (_JobOrderModel.JobOrdStatus == "A")
                        {
                            if (CheckMaterialDispatchAgainstJobOrdr(_JobOrderModel) == "Used")
                            {
                                _JobOrderModel.Message = "Used";
                                _JobOrderModel.TransType = "Update";
                                _JobOrderModel.Command = "Add";
                                _JobOrderModel.BtnName = "BtnToDetailPage";
                                TempData["ModelData"] = _JobOrderModel;
                            }

                            else
                            {
                                _JobOrderModel.TransType = "Update";
                                _JobOrderModel.Command = command;
                                _JobOrderModel.BtnName = "BtnEdit";
                                _JobOrderModel.Message = "New";
                                _JobOrderModel.AppStatus = "D";
                                _JobOrderModel.DocumentStatus = "D";
                                ViewBag.DocumentStatus = _JobOrderModel.AppStatus;
                                TempData["ModelData"] = _JobOrderModel;
                            }
                        }
                        else if (_JobOrderModel.JobOrdStatus == "PD")
                        {
                            _JobOrderModel.Message = "New";
                            _JobOrderModel.TransType = "Update";
                            _JobOrderModel.Command = "Edit";
                            _JobOrderModel.BtnName = "BtnEdit";
                            TempData["ModelData"] = _JobOrderModel;
                        }
                        else
                        {
                            _JobOrderModel.TransType = "Update";
                            _JobOrderModel.Command = command;
                            _JobOrderModel.BtnName = "BtnEdit";
                            _JobOrderModel.Message = "New";
                            _JobOrderModel.AppStatus = "D";
                            _JobOrderModel.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _JobOrderModel.AppStatus;
                            TempData["ModelData"] = _JobOrderModel;
                        }
                        var JOCodeURL = _JobOrderModel.JO_No;
                        var JoDate = _JobOrderModel.JO_Date;
                        command = _JobOrderModel.Command;
                        TempData["ModelData"] = _JobOrderModel;
                        TempData["ListFilterData"] = _JobOrderModel.ListFilterData1;
                        return (RedirectToAction("JobOrderSCDetail", new { JOCodeURL = JOCodeURL, JoDate, TransType, BtnName, command }));

                    case "Delete":
                        _JobOrderModel.Command = command;
                        _JobOrderModel.BtnName = "Refresh";
                        DeleteJODetails(_JobOrderModel, command);
                        TempData["ListFilterData"] = _JobOrderModel.ListFilterData1;
                        return RedirectToAction("JobOrderSCDetail");

                    case "Save":
                        _JobOrderModel.Command = command;
                        SaveJODetail(_JobOrderModel);
                        //TempData["ModelData"] = _JobOrderModel;
                        if (_JobOrderModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_JobOrderModel.Message == "DocModify")
                        {
                            //_JobOrderModel.DocumentMenuId = _JobOrderModel.DocumentMenuId;
                            DocumentMenuId = _JobOrderModel.DocumentMenuId;
                            CommonPageDetails();
                            //ViewBag.DocID = _JobOrderModel.DocumentMenuId;
                            ViewBag.DocumentMenuId = _JobOrderModel.DocumentMenuId;
                            ViewBag.DocumentStatus = "D";


                            List<SupplierName> suppLists1 = new List<SupplierName>();
                            suppLists1.Add(new SupplierName { supp_id = _JobOrderModel.SuppID, supp_name = _JobOrderModel.SuppName });
                            _JobOrderModel.SupplierNameList = suppLists1;

                            List<ProdOrdNoList> srcDocNoLists1 = new List<ProdOrdNoList>();
                            srcDocNoLists1.Add(new ProdOrdNoList { ProdOrdnoId = _JobOrderModel.ProducOrd_Num, ProdOrdnoVal = _JobOrderModel.ProducOrd_Num });
                            _JobOrderModel.prdordNoLists = srcDocNoLists1;

                            _JobOrderModel.SuppName = _JobOrderModel.SuppName;
                            _JobOrderModel.ValidUpto = _JobOrderModel.ValidUpto;
                            _JobOrderModel.Address = _JobOrderModel.Address;
                            _JobOrderModel.GrVal = _JobOrderModel.GrVal;
                            _JobOrderModel.TaxAmt = _JobOrderModel.TaxAmt;
                            _JobOrderModel.OcAmt = _JobOrderModel.OcAmt;
                            _JobOrderModel.NetValSpec = _JobOrderModel.NetValSpec;
                            _JobOrderModel.ProducOrdNum = _JobOrderModel.ProducOrd_Num;
                            _JobOrderModel.ProducOrd_Num = _JobOrderModel.ProducOrd_Num;
                            _JobOrderModel.ProducOrdDate = _JobOrderModel.ProducOrdDate;
                            ViewBag.ItemDetailsList = ViewData["ItemDetails"];
                            ViewBag.OutPutItemDetailsList = ViewData["OutItemDetails"];
                            ViewBag.InputItemDetailsList = ViewData["InItemDetails"];
                            ViewBag.ItemDelSchdetails = ViewData["DelvScheDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["TaxDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTermsdetails = ViewData["TrmAndConDetails"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _JobOrderModel.BtnName = "Refresh";
                            _JobOrderModel.Command = "Refresh";
                            _JobOrderModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            _JobOrderModel.ValDigit = ValDigit;
                            _JobOrderModel.QtyDigit = QtyDigit;
                            _JobOrderModel.RateDigit = RateDigit;
                            return View("~/Areas/ApplicationLayer/Views/SubContracting/JobOrder/JobOrderDetail.cshtml", _JobOrderModel);
                        }
                        else
                        {

                            JOCodeURL = _JobOrderModel.JO_No;
                            JoDate = _JobOrderModel.JO_Date;
                            TransType = _JobOrderModel.TransType;
                            BtnName = _JobOrderModel.BtnName;
                            TempData["ModelData"] = _JobOrderModel;
                            TempData["ListFilterData"] = _JobOrderModel.ListFilterData1;
                            return (RedirectToAction("JobOrderSCDetail", new { JOCodeURL = JOCodeURL, JoDate, TransType, BtnName, command }));
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
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        //string joDt1 = _JobOrderModel.JO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, joDt1) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Update":
                        _JobOrderModel.TransType = command;
                        if (_JobOrderModel.Amend != null && _JobOrderModel.Amend != "" && _JobOrderModel.Amendment == null)
                        {
                            _JobOrderModel.TransType = "Amendment";
                        }
                        SaveJODetail(_JobOrderModel);
                        JobOrderModel URLModelUpdate = new JobOrderModel();
                        TempData["ListFilterData"] = _JobOrderModel.ListFilterData1;
                        //Session["TransType"] = "Update";
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnSave";
                        _JobOrderModel.TransType = "Update";
                        _JobOrderModel.Command = command;
                        _JobOrderModel.BtnName = "BtnSave";
                        if (_JobOrderModel.JobOrdStatus == "PD" || _JobOrderModel.JobOrdStatus == "PDL" || _JobOrderModel.JobOrdStatus == "PR" || _JobOrderModel.JobOrdStatus == "PN")
                        {
                            //Session["BtnName"] = "BtnToDetailPage";
                            _JobOrderModel.BtnName = "BtnToDetailPage";
                        }
                        URLModelUpdate.DocumentMenuId = _JobOrderModel.DocumentMenuId;
                        URLModelUpdate.TransType = "Update";
                        URLModelUpdate.BtnName = _JobOrderModel.BtnName;
                        URLModelUpdate.Command = command;
                        URLModelUpdate.JO_Date = _JobOrderModel.JO_Date;
                        URLModelUpdate.JO_No = _JobOrderModel.JO_No;
                        TempData["ModelData"] = _JobOrderModel;
                        return RedirectToAction("JobOrderSCDetail", URLModelUpdate);
                    case "Approve":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        //string joDt2 = _JobOrderModel.JO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, joDt2) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        _JobOrderModel.Command = command;
                        JOApprove(_JobOrderModel, "");

                        JOCodeURL = _JobOrderModel.JO_No;
                        JoDate = _JobOrderModel.JO_Date;
                        TransType = _JobOrderModel.TransType;
                        BtnName = _JobOrderModel.BtnName;
                        TempData["ModelData"] = _JobOrderModel;
                        TempData["ListFilterData"] = _JobOrderModel.ListFilterData1;
                        return (RedirectToAction("JobOrderSCDetail", new { JOCodeURL = JOCodeURL, JoDate, TransType, BtnName, command }));

                    case "Refresh":
                        JobOrderModel _JobOrderModelRefresh = new JobOrderModel();
                        _JobOrderModelRefresh.Message = null;
                        _JobOrderModelRefresh.Command = command;
                        _JobOrderModelRefresh.TransType = "Refresh";
                        _JobOrderModelRefresh.BtnName = "Refresh";
                        _JobOrderModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _JobOrderModelRefresh;
                        TempData["ListFilterData"] = _JobOrderModel.ListFilterData1;
                        return RedirectToAction("JobOrderSCDetail");
                    case "Amendment":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        //string joDt3 = _JobOrderModel.JO_Date;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, joDt3) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _JobOrderModel.JO_No, DocDate = _JobOrderModel.JO_Date, ListFilterData = _JobOrderModel.ListFilterData1, WF_status = _JobOrderModel.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        JobOrderModel URLModelAmendment = new JobOrderModel();
                        _JobOrderModel.Command = "Edit";
                        _JobOrderModel.TransType = "Update";
                        _JobOrderModel.BtnName = "BtnEdit";
                        _JobOrderModel.Amend = "Amend";
                        TempData["ModelData"] = _JobOrderModel;
                        URLModelAmendment.Command = "Edit";
                        URLModelAmendment.TransType = "Update";
                        URLModelAmendment.BtnName = "BtnEdit";
                        URLModelAmendment.DocumentMenuId = _JobOrderModel.DocumentMenuId;
                        URLModelAmendment.JO_Date = _JobOrderModel.JO_Date;
                        URLModelAmendment.JO_No = _JobOrderModel.JO_No;
                        URLModelAmendment.bill_add_id = _JobOrderModel.bill_add_id;
                        //URLModelAmendment.SrcDocNo= _JobOrderModel.SrcDocNo;
                        //URLModelAmendment.HdnProducOrdNum = _JobOrderModel.ProducOrdNum;

                        URLModelAmendment.Amend = "Amend";
                        return RedirectToAction("JobOrderSCDetail", URLModelAmendment);
                    case "Print":

                        return GenratePdfFile(_JobOrderModel);
                    case "BacktoList":
                        var WF_Status = _JobOrderModel.WF_Status1;
                        TempData["ListFilterData"] = _JobOrderModel.ListFilterData1;
                        return RedirectToAction("JobOrderSC", "JobOrderSC", new { WF_Status });

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
        public ActionResult SaveJODetail(JobOrderModel _JobOrderModel)
        {
            string SaveMessage = "";
            /*getDocumentName();*/ /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                _JobOrderModel.DocumentMenuId = _JobOrderModel.DocumentMenuId;


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
                DataTable DtblOutputItemDetail = new DataTable();
                DataTable DtblInputItemDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                //DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblOCDetail = new DataTable();
                DataTable DtblDeliSchDetail = new DataTable();
                DataTable DtblTermsDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable dtheader = new DataTable();

                DtblHDetail = ToDtblHDetail(_JobOrderModel);
                DtblItemDetail = ToDtblItemDetail(_JobOrderModel.Itemdetails);
                DtblOutputItemDetail = ToDtblOutputItemDetail(_JobOrderModel.OutputItemdetails);
                 DtblInputItemDetail = ToDtblInputItemDetail(_JobOrderModel.InputItemdetails);

                DtblTaxDetail = ToDtblTaxDetail(_JobOrderModel.ItemTaxdetails);
                //DtblOCTaxDetail = ToDtblTaxDetail(_JobOrderModel.ItemOCTaxdetails);
                DtblOCDetail = ToDtblOCDetail(_JobOrderModel.ItemOCdetails);
                DtblDeliSchDetail = ToDtblDelSchDetail(_JobOrderModel.ItemDelSchdetails);
                DtblTermsDetail = ToDtblTermsDetail(_JobOrderModel.ItemTermsdetails);

                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                dtSubItem.Columns.Add("Type", typeof(string));
                if (_JobOrderModel.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(_JobOrderModel.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                        if(_JobOrderModel.SourceType=="D")
                        {
                            dtrowItemdetails["Type"] = jObject2[i]["subItemTyp"].ToString();
                        }
                        else
                        {
                            dtrowItemdetails["Type"] = "";
                        }
                        
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                    //ViewData["SubItem"] = dtSubItemDetails(jObject2);
                }

                /*------------------Sub Item end----------------------*/




                var _JobOrderDetailsattch = TempData["ModelDataattch"] as JobOrderDetailsattch;
                TempData["ModelDataattch"] = null;
                DataTable dtAttachment = new DataTable();
                if (_JobOrderModel.attatchmentdetail != null)
                {
                    if (_JobOrderDetailsattch != null)
                    {
                        if (_JobOrderDetailsattch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _JobOrderDetailsattch.AttachMentDetailItmStp as DataTable;
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
                        if (_JobOrderModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _JobOrderModel.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_JobOrderModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_JobOrderModel.JO_No))
                            {
                                dtrowAttachment1["id"] = _JobOrderModel.JO_No;
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

                    if (_JobOrderModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_JobOrderModel.JO_No))
                            {
                                ItmCode = _JobOrderModel.JO_No;
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
               
                 SaveMessage = _JobOrder_ISERVICES.InsertJO_Details(DtblHDetail, DtblItemDetail, DtblOutputItemDetail, DtblInputItemDetail, DtblTaxDetail, DtblOCDetail, DtblDeliSchDetail, DtblTermsDetail, dtSubItem, DtblAttchDetail/*, DtblOCTaxDetail, */);
                
                if (SaveMessage == "DocModify")
                {
                    _JobOrderModel.Message = "DocModify";
                    _JobOrderModel.BtnName = "Refresh";
                    _JobOrderModel.Command = "Refresh";
                    _JobOrderModel.DocumentMenuId = DocumentMenuId;
                    TempData["ModelData"] = _JobOrderModel;
                    return RedirectToAction("JobOrderSCDetail");
                }
                else
                {
                    string[] Data = SaveMessage.Split(',');
                    string JONo = Data[1];
                    string JO_No = JONo.Replace("/", "");
                    string Message = Data[0];
                    if (Message == "Data_Not_Found")
                    {
                        var a = JONo.Split('-');
                        var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _JobOrderModel.Message = Message.Replace("_", "");
                        return RedirectToAction("JobOrderSCDetail");
                    }
                    string JODate = Data[2];
                    string Message1 = Data[4];
                    string StatusCode = Data[3];
                    /*-----------------Attachment Section Start------------------------*/
                    if (Message == "Save")
                    {

                        string Guid = "";
                        if (_JobOrderDetailsattch != null)
                        {
                            if (_JobOrderDetailsattch.Guid != null)
                            {
                                Guid = _JobOrderDetailsattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, JO_No, _JobOrderModel.TransType, DtblAttchDetail);

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
                        //                string JO_No1 = JO_No.Replace("/", "");
                        //                string img_nm = CompID + BrchID + JO_No1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                    /*-----------------Attachment Section End------------------------*/
                    if (Message1 == "Cancelled")
                    {
                        _JobOrderModel.Message = "Cancelled";
                        _JobOrderModel.Command = "Update";
                        _JobOrderModel.TransType = "Update";
                        _JobOrderModel.AppStatus = "D";
                        _JobOrderModel.BtnName = "Refresh";
                        _JobOrderModel.JO_No = JONo;
                        _JobOrderModel.JO_Date = JODate;
                        TempData["ModelData"] = _JobOrderModel;

                        return RedirectToAction("JobOrderSCDetail");
                    }


                    if (Message == "Update" || Message == "Save")
                    {
                        _JobOrderModel.Message = "Save";

                        _JobOrderModel.JO_No = JONo;
                        _JobOrderModel.JO_Date = JODate;
                        _JobOrderModel.TransType = "Update";
                        if (Message1 == "Force Closed")
                        {
                            _JobOrderModel.Command = "Edit";
                            _JobOrderModel.DocumentStatus = "PD";
                        }
                        else
                        {
                            _JobOrderModel.Command = "Update";
                            _JobOrderModel.AppStatus = "D";
                            _JobOrderModel.DocumentStatus = "D";
                        }
                        _JobOrderModel.BtnName = "BtnSave";
                        TempData["ModelData"] = _JobOrderModel;


                    }
                    return RedirectToAction("JobOrderSCDetail");
                }
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_JobOrderModel.TransType == "Save")
                    {
                        string Guid = "";
                        if (_JobOrderModel.Guid != null)
                        {
                            Guid = _JobOrderModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable ToDtblHDetail(JobOrderModel _JobOrderModel)
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
                dtheader.Columns.Add("SourceType", typeof(string));
                dtheader.Columns.Add("ItmWeight", typeof(string));
                
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("FClosed", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("JO_No", typeof(string));
                dtheader.Columns.Add("JO_Date", typeof(string));
                 dtheader.Columns.Add("ProdDocNo", typeof(string));
                  dtheader.Columns.Add("ProdDocDate", typeof(string));
                
                dtheader.Columns.Add("SuppName", typeof(int));
                dtheader.Columns.Add("ValidUpto", typeof(string));
                dtheader.Columns.Add("FItemID", typeof(string));
                dtheader.Columns.Add("FUomId", typeof(int));
                dtheader.Columns.Add("OrdQty", typeof(string));
                dtheader.Columns.Add("OpId", typeof(int));
               
                dtheader.Columns.Add("Remarks", typeof(string));
                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("GrVal", typeof(string));
                dtheader.Columns.Add("TaxAmt", typeof(string));
                dtheader.Columns.Add("OcAmt", typeof(string));
                dtheader.Columns.Add("NetValBs", typeof(string));
                //dtheader.Columns.Add("NetValSpec", typeof(string));//
                dtheader.Columns.Add("JOStatus", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(int));
                dtheader.Columns.Add("SystemDetail", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                if (_JobOrderModel.SourceType=="Direct")
                {
                    _JobOrderModel.SourceType = "D";
                }
                else
                {
                    _JobOrderModel.SourceType = "P";
                }
                dtrowHeader["SourceType"] = _JobOrderModel.SourceType;
                
                    dtrowHeader["ItmWeight"] = _JobOrderModel.ItmWeight;
                
                    dtrowHeader["TransType"] = _JobOrderModel.TransType;
                dtrowHeader["FClosed"] = ConvertBoolToStrint(_JobOrderModel.FClosed);
                dtrowHeader["Cancelled"] = ConvertBoolToStrint(_JobOrderModel.Cancelled);
                dtrowHeader["JO_No"] = _JobOrderModel.JO_No;
                dtrowHeader["JO_Date"] = _JobOrderModel.JO_Date;
               
                    dtrowHeader["ProdDocNo"] = _JobOrderModel.ProducOrd_Num;
                    dtrowHeader["ProdDocDate"] = _JobOrderModel.ProducOrdDate;
               
                dtrowHeader["SuppName"] = _JobOrderModel.SuppID;

                dtrowHeader["ValidUpto"] = _JobOrderModel.ValidUpto;
                if (_JobOrderModel.SourceType == "D")
                {
                    dtrowHeader["FItemID"] = _JobOrderModel.Fproduct_id;
                }
                else
                {
                    dtrowHeader["FItemID"] = _JobOrderModel.FinishProductId;
                }
                    
                dtrowHeader["FUomId"] = _JobOrderModel.FinishUomId;
                dtrowHeader["OrdQty"] = _JobOrderModel.JOOrderQty;
                if (_JobOrderModel.SourceType == "D")
                {
                    dtrowHeader["OpId"] = 0;
                }
                else
                {
                    dtrowHeader["OpId"] = _JobOrderModel.OpId;
                }
                    
                
                dtrowHeader["Remarks"] = _JobOrderModel.Remarks;
                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["GrVal"] = IsNull(_JobOrderModel.GrVal, "0");
                dtrowHeader["TaxAmt"] = IsNull(_JobOrderModel.TaxAmt, "0");
                dtrowHeader["OcAmt"] = IsNull(_JobOrderModel.OcAmt, "0");

                dtrowHeader["NetValBs"] = IsNull(_JobOrderModel.NetValBs, "0");
                //dtrowHeader["NetValSpec"] = IsNull(_JobOrderModel.NetValSpec, "0");
                dtrowHeader["JOStatus"] = IsNull(_JobOrderModel.JobOrdStatus, "D");
                dtrowHeader["bill_add_id"] = _JobOrderModel.bill_add_id;
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
        private DataTable ToDtblItemDetail(string jQItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(string));
                dtItem.Columns.Add("OrderQty", typeof(string));

                dtItem.Columns.Add("ItmRate", typeof(string));

                dtItem.Columns.Add("GrossVal", typeof(string));
                //dtItem.Columns.Add("AssVal", typeof(string));
                dtItem.Columns.Add("TaxAmt", typeof(string));
                dtItem.Columns.Add("OCAmt", typeof(string));
                dtItem.Columns.Add("NetVal", typeof(string));
                dtItem.Columns.Add("SimpleIssue", typeof(string));
                dtItem.Columns.Add("MRSNo", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));
                dtItem.Columns.Add("ItemType", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));

                if (jQItemDetail != null)
                {
                    JArray jObject = JArray.Parse(jQItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["OrderQty"] = jObject[i]["OrderQty"].ToString();
                        dtrowLines["ItmRate"] = jObject[i]["ItmRate"].ToString();
                        dtrowLines["GrossVal"] = jObject[i]["GrossVal"].ToString();
                        dtrowLines["TaxAmt"] = jObject[i]["TaxAmt"].ToString();
                        dtrowLines["OCAmt"] = jObject[i]["OCAmt"].ToString();
                        dtrowLines["NetVal"] = jObject[i]["NetVal"].ToString();
                        dtrowLines["SimpleIssue"] = jObject[i]["SimpleIssue"].ToString();
                        dtrowLines["MRSNo"] = jObject[i]["MRSNo"].ToString();
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();
                        dtrowLines["ItemType"] = jObject[i]["ItemType"].ToString();
                        dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
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
        private DataTable ToDtblOutputItemDetail(string jQOutputItemDetail)
        {
            try
            {
                DataTable DtblOutputItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(int));
                dtItem.Columns.Add("OrderQty", typeof(string));

                dtItem.Columns.Add("ItmRate", typeof(string));
                dtItem.Columns.Add("GrossVal", typeof(string));
                dtItem.Columns.Add("TaxAmt", typeof(string));
                dtItem.Columns.Add("OCAmt", typeof(string));
                dtItem.Columns.Add("NetVal", typeof(string));
                dtItem.Columns.Add("SimpleIssue", typeof(string));
                dtItem.Columns.Add("MRSNo", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));
                dtItem.Columns.Add("ItemType", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));



                if (jQOutputItemDetail != null)
                {
                    JArray jObject = JArray.Parse(jQOutputItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["OrderQty"] = jObject[i]["OrdQty"].ToString();

                        dtrowLines["ItmRate"] = jObject[i]["ItmRate"].ToString();
                        dtrowLines["GrossVal"] = jObject[i]["GrossVal"].ToString();
                        dtrowLines["TaxAmt"] = jObject[i]["TaxAmt"].ToString();
                        dtrowLines["OCAmt"] = jObject[i]["OCAmt"].ToString();
                        dtrowLines["NetVal"] = jObject[i]["NetVal"].ToString();
                        dtrowLines["SimpleIssue"] = jObject[i]["SimpleIssue"].ToString();
                        dtrowLines["MRSNo"] = jObject[i]["MRSNo"].ToString();
                        dtrowLines["ItemType"] = jObject[i]["ItemType"].ToString();
                        dtrowLines["tax_expted"] = "";
                        dtrowLines["manual_gst"] = "";
                        dtrowLines["hsn_code"] = "";

                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["OutItemDetails"] = dtOutitemdetail(jObject);
                }

                DtblOutputItemDetail = dtItem;
                return DtblOutputItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblInputItemDetail(string jQInputItemDetail)
        {
            try
            {
                DataTable DtblInputItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(int));

                dtItem.Columns.Add("ItemTypeID", typeof(string));
                dtItem.Columns.Add("RequiredQty", typeof(string));



                if (jQInputItemDetail != null)
                {
                    JArray jObject = JArray.Parse(jQInputItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["ItemTypeID"] = jObject[i]["ItmType"].ToString();
                        dtrowLines["RequiredQty"] = jObject[i]["ReqQty"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["InItemDetails"] = dtINitemdetail(jObject);
                }

                DtblInputItemDetail = dtItem;
                return DtblInputItemDetail;
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
                    ViewData["OCDetails"] = dtOCdetail(jObject);
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

        public DataTable dtitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("ord_qty_base", typeof(string));
            dtItem.Columns.Add("item_type", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            //dtItem.Columns.Add("item_disc_perc", typeof(string));
            //dtItem.Columns.Add("item_disc_amt", typeof(string));
            //dtItem.Columns.Add("item_disc_val", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(string));
            //dtItem.Columns.Add("item_ass_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));

            dtItem.Columns.Add("mrs_no", typeof(string));
            //dtItem.Columns.Add("force_close", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["ord_qty_base"] = jObject[i]["OrderQty"].ToString();
                dtrowLines["item_type"] = jObject[i]["ItemType"].ToString();
                dtrowLines["item_rate"] = jObject[i]["ItmRate"].ToString();
                //dtrowLines["item_disc_perc"] = jObject[i]["ItmDisPer"].ToString();
                //dtrowLines["item_disc_amt"] = jObject[i]["ItmDisAmt"].ToString();
                //dtrowLines["item_disc_val"] = jObject[i]["DisVal"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["GrossVal"].ToString();
                //dtrowLines["item_ass_val"] = jObject[i]["AssVal"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["TaxAmt"].ToString();
                dtrowLines["item_oc_amt"] = jObject[i]["OCAmt"].ToString();
                dtrowLines["item_net_val_bs"] = jObject[i]["NetVal"].ToString();
                dtrowLines["mrs_no"] = jObject[i]["MRSNo"].ToString();
                //dtrowLines["force_close"] = jObject[i]["FClosed"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["Remarks"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();

                dtItem.Rows.Add(dtrowLines);

            }

            return dtItem;
        }
        public DataTable dtOutitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("UomName", typeof(string));
            dtItem.Columns.Add("ord_qty_base", typeof(string));
            dtItem.Columns.Add("item_type", typeof(string));



            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["UomName"] = jObject[i]["UOMName"].ToString();
                dtrowLines["ord_qty_base"] = jObject[i]["OrdQty"].ToString();
                dtrowLines["item_type"] = jObject[i]["ItemType"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }
            return dtItem;
        }
        public DataTable dtINitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("UomName", typeof(string));
            dtItem.Columns.Add("Req_qty", typeof(string));
            dtItem.Columns.Add("ItemTypName", typeof(string));
            dtItem.Columns.Add("ItemTypID", typeof(string));



            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["UomName"] = jObject[i]["UOMName"].ToString();
                dtrowLines["Req_qty"] = jObject[i]["ReqQty"].ToString();
                dtrowLines["ItemTypName"] = jObject[i]["ItmTypeName"].ToString();
                dtrowLines["ItemTypID"] = jObject[i]["ItmType"].ToString();

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
            dtterm.Columns.Add("sno", typeof(int));
            int sno = 1;

            for (int i = 0; i < jObject.Count; i++)
            {
                
                DataRow dtrowLines = dtterm.NewRow();

                dtrowLines["term_desc"] = jObject[i]["TermsDesc"].ToString();
                dtrowLines["sno"] = sno;

                dtterm.Rows.Add(dtrowLines);
                sno += 1;
            }
            return dtterm;
        }
        public ActionResult DoubleClickOnList(string DocNo, string DocDate, JobOrderModel _JobOrderModel, string ListFilterData, string WF_Status,string SrcTyp)
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
            var WF_Status1 = "";
            _JobOrderModel.Message = "New";
            _JobOrderModel.Command = "Update";
            _JobOrderModel.TransType = "Update";
            _JobOrderModel.BtnName = "BtnToDetailPage";
            _JobOrderModel.SourceType = SrcTyp;
            _JobOrderModel.JO_No = DocNo;
            _JobOrderModel.JO_Date = DocDate;
            if (WF_Status != null && WF_Status != "")
            {
                _JobOrderModel.WF_Status1 = WF_Status;
                WF_Status1 = WF_Status;
            }
            var JOCodeURL = DocNo;
            var JoDate = DocDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var command = "";
           if( _JobOrderModel.SourceType== "Production Order")
            {
                command = null;
            }
            else
            {
                 command = "Add";
            }
            
            TempData["ModelData"] = _JobOrderModel;
            TempData["ListFilterData"] = ListFilterData;
            return (RedirectToAction("JobOrderSCDetail", "JobOrderSC", new { JOCodeURL = JOCodeURL, JoDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult GetAutoCompleteSearchSuppList(JOListModel _JOListModel)
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
                if (string.IsNullOrEmpty(_JOListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _JOListModel.SuppName;
                }
                CustList = _JobOrder_ISERVICES.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _JOListModel.SupplierNameList = _SuppList;
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
                DataSet result = _JobOrder_ISERVICES.GetSuppAddrDetailDAL(Supp_id, Comp_ID);
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
        public JsonResult GetProducORDDocList()
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
                DataSet Deatils = _JobOrder_ISERVICES.GetProducORDDocList(Comp_ID, Br_ID);

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
        [HttpPost]
        public JsonResult GetJOHedrItemUOM(string Itm_ID)
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
                DataTable result = _JobOrder_ISERVICES.GetItemUOM(Comp_ID, Itm_ID);
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
        public JsonResult GetDetailsAgainstProducOrdNo(string ProductionOrd_no, string ProductionOrd_date)
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
                DataSet Deatils = _JobOrder_ISERVICES.GetDetailsAgainstProducOrdNo(Comp_ID, Br_ID, ProductionOrd_no, ProductionOrd_date);

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

        [HttpPost]
        public ActionResult GetProductNameInDDLListPage(JobOrderModel _JobOrderModel)
        {
            JsonResult DataRows = null;
            string JOItmName = string.Empty;
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
                    if (string.IsNullOrEmpty(_JobOrderModel.JO_ItemName))
                    {
                        JOItmName = "0";
                    }
                    else
                    {
                        JOItmName = _JobOrderModel.JO_ItemName;
                    }

                    DataSet ProductList = _JobOrder_ISERVICES.BindProductNameInDDL(Comp_ID, Br_ID, JOItmName);
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

        private List<JobOrderList> getJOList(JOListModel _JOListModel)
        {
            _JobOrderList = new List<JobOrderList>();
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
                if (_JOListModel.WF_Status != null)
                {
                    wfstatus = _JOListModel.WF_Status;
                }
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                else
                {
                    wfstatus = "";
                }

                DataSet DSet = _JobOrder_ISERVICES.GetJOListandSrchDetail(CompID, BrchID, _JOListModel, UserID, wfstatus, DocumentMenuId);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        JobOrderList _JOList = new JobOrderList();
                        _JOList.SourceType = dr["src_type"].ToString();
                        _JOList.OrderNo = dr["OrderNo"].ToString();
                        _JOList.OrderDate = dr["JODate"].ToString();
                        _JOList.OrderDt = dr["JODT"].ToString();
                        _JOList.SuppName = dr["supp_name"].ToString();
                        _JOList.Valid_Upto = dr["ValidUpto"].ToString();
                        _JOList.FgItemName = dr["FgItemName"].ToString();
                        _JOList.FgUomName = dr["FgUomName"].ToString();
                        _JOList.ProductName = dr["item_name"].ToString();
                        _JOList.UOM = dr["UOM"].ToString();
                        _JOList.Quantity = dr["Quantity"].ToString();
                        _JOList.ProductionOrdNo = dr["SrcDocNo"].ToString();
                        _JOList.ProductionOrdDate = dr["SrcDocDate"].ToString();
                        _JOList.OprationName = dr["op_name"].ToString();

                        _JOList.JO_Status = dr["JOStatus"].ToString();
                        _JOList.CreateDate = dr["CreateDate"].ToString();
                        _JOList.ApproveDate = dr["ApproveDate"].ToString();
                        _JOList.ModifyDate = dr["ModifyDate"].ToString();
                        _JobOrderList.Add(_JOList);
                    }
                }

                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                _JOListModel.FinStDt = DSet.Tables[2].Rows[0]["findate"].ToString();
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _JobOrderList;
        }

        [HttpPost]
        public ActionResult SearchJODetail(string SuppId, string OutOPProdctID, string FinishProdctID, string Fromdate, string Todate, string Status)
        {
            JOListModel _JOListModel = new JOListModel();
            try
            {
                //Session.Remove("WF_Docid");
                // Session.Remove("WF_status");
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
                _JobOrderList = new List<JobOrderList>();
                _JOListModel.SuppID = SuppId;
                _JOListModel.Product_id = OutOPProdctID;
                _JOListModel.FinishProdct_Id = FinishProdctID;
                _JOListModel.FromDate = Fromdate;
                _JOListModel.ToDate = Todate;
                _JOListModel.Status = Status;
                DataSet DSet = _JobOrder_ISERVICES.GetJOListandSrchDetail(CompID, BrchID, _JOListModel, "", "", "");
                //Session["JOSearch"] = "JO_Search";
                _JOListModel.JOSearch = "JO_Search";

                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    JobOrderList _JOList = new JobOrderList();
                    _JOList.SourceType = dr["src_type"].ToString();
                    _JOList.OrderNo = dr["OrderNo"].ToString();
                    _JOList.OrderDate = dr["JODate"].ToString();
                    _JOList.OrderDt = dr["JODt"].ToString();
                    _JOList.SuppName = dr["supp_name"].ToString();
                    _JOList.Valid_Upto = dr["ValidUpto"].ToString();
                    _JOList.FgItemName = dr["FgItemName"].ToString();
                    _JOList.FgUomName = dr["FgUomName"].ToString();
                    _JOList.ProductName = dr["item_name"].ToString();
                    _JOList.UOM = dr["UOM"].ToString();
                    _JOList.Quantity = dr["Quantity"].ToString();
                    _JOList.ProductionOrdNo = dr["SrcDocNo"].ToString();
                    _JOList.ProductionOrdDate = dr["SrcDocDate"].ToString();
                    _JOList.OprationName = dr["op_name"].ToString();

                    _JOList.JO_Status = dr["JOStatus"].ToString();
                    _JOList.CreateDate = dr["CreateDate"].ToString();
                    _JOList.ApproveDate = dr["ApproveDate"].ToString();
                    _JOList.ModifyDate = dr["ModifyDate"].ToString();
                    _JobOrderList.Add(_JOList);
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                _JOListModel.JobOrdList = _JobOrderList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialJobOrderSCList.cshtml", _JOListModel);
        }
        public ActionResult DeleteJODetails(JobOrderModel _JobOrderModel, string command)
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
                string JONo = _JobOrderModel.JO_No;
                string JODelete = _JobOrder_ISERVICES.JO_DeleteDetail(_JobOrderModel, CompID, BrchID);

                if (!string.IsNullOrEmpty(JONo))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string JONo1 = JONo.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, JONo1, Server);
                }
                _JobOrderModel = new JobOrderModel();
                _JobOrderModel.Message = "Deleted";
                _JobOrderModel.Command = "Refresh";
                _JobOrderModel.TransType = "Refresh";
                _JobOrderModel.BtnName = "BtnDelete";
                TempData["ModelData"] = _JobOrderModel;
                return RedirectToAction("JobOrderSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }

        public ActionResult JOApprove(JobOrderModel _JobOrderModel, string ListFilterData1)
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
                string JONo = _JobOrderModel.JO_No;
                string JODate = _JobOrderModel.JO_Date;
                string A_Status = _JobOrderModel.A_Status;
                string A_Level = _JobOrderModel.A_Level;
                string A_Remarks = _JobOrderModel.A_Remarks;


                string Message = _JobOrder_ISERVICES.JOApproveDetails(CompID, BrchID, JONo, JODate, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks);
                string ApMessage = Message.Split(',')[2].Trim();
                string JOSC_No = Message.Split(',')[0].Trim();
                //string VouNo = Message.Split(',')[3].Trim();
                string JOSC_Date = Message.Split(',')[1].Trim();
                //string VouType = Message.Split(',')[5].Trim();
                if (ApMessage == "A")
                {
                    _JobOrderModel.Message = "Approved";
                    if(_JobOrderModel.Amend == "Amend") /* Added by Suraj Maurya on 30-05-2025, Case : approve btn get enabled even document is approved */ 
                    {
                        _JobOrderModel.Amend = "";
                    }
                }
                _JobOrderModel.TransType = "Update";
                _JobOrderModel.Command = "Approve";
                _JobOrderModel.AppStatus = "D";
                _JobOrderModel.BtnName = "BtnEdit";

                var JOCodeURL = JOSC_No;
                var JoDate = _JobOrderModel.JO_Date;
                var TransType = _JobOrderModel.TransType;
                var BtnName = _JobOrderModel.BtnName;
                var command = _JobOrderModel.Command;
                TempData["ModelData"] = _JobOrderModel;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("JobOrderSCDetail", new { JOCodeURL = JOCodeURL, JoDate, TransType, BtnName, command }));

                //return RedirectToAction("JobOrderSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status)
        {
            JobOrderModel _JobOrderModel = new JobOrderModel();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _JobOrderModel.JO_No = jObjectBatch[i]["JONo"].ToString();
                    _JobOrderModel.JO_Date = jObjectBatch[i]["JODate"].ToString();

                    _JobOrderModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _JobOrderModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _JobOrderModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                }
            }
            if (_JobOrderModel.A_Status != "Approve")
            {
                _JobOrderModel.A_Status = "Approve";
            }
            JOApprove(_JobOrderModel, ListFilterData1);
            var WF_Status1 = "";
            var JOCodeURL = _JobOrderModel.JO_No;
            var JoDate = _JobOrderModel.JO_Date;
            if (WF_Status != null && WF_Status != "")
            {
                WF_Status1 = WF_Status;
                _JobOrderModel.WF_Status1 = WF_Status;
            }
            var TransType = _JobOrderModel.TransType;
            var BtnName = _JobOrderModel.BtnName;
            var command = _JobOrderModel.Command;
            TempData["ModelData"] = _JobOrderModel;
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("JobOrderSCDetail", new { JOCodeURL = JOCodeURL, JoDate, TransType, BtnName, command, WF_Status1 }));

            //return RedirectToAction("JobOrderSCDetail");
        }
        private string CheckMaterialDispatchAgainstJobOrdr(JobOrderModel _JobOrderModel)
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

                str = _JobOrder_ISERVICES.CheckMtrialDisptchDagainstJobOrdr(CompID, BrchID, _JobOrderModel.JO_No, _JobOrderModel.JO_Date).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }


        public ActionResult ToRefreshByJS(string FrwdDtList, string ListFilterData1, string WF_Status)
        {
            JobOrderModel _JobOrderModel = new JobOrderModel();
            var WF_Status1 = "";
            if (FrwdDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(FrwdDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _JobOrderModel.JO_No = jObjectBatch[i]["JONo"].ToString();
                    _JobOrderModel.JO_Date = jObjectBatch[i]["JODate"].ToString();
                    _JobOrderModel.TransType = "Update";
                    _JobOrderModel.BtnName = "BtnToDetailPage";
                    ////_JobOrderModel.Command = "Approve";
                    //_JobOrderModel.AppStatus = "D";
                    //_JobOrderModel.BtnName = "BtnEdit";
                    if (WF_Status != null && WF_Status != "")
                    {
                        _JobOrderModel.WF_Status1 = WF_Status;
                        WF_Status1 = WF_Status;
                    }
                    TempData["ModelData"] = _JobOrderModel;
                }
            }

            var JOCodeURL = _JobOrderModel.JO_No;
            var JoDate = _JobOrderModel.JO_Date;
            var TransType = _JobOrderModel.TransType;
            var BtnName = _JobOrderModel.BtnName;
            var command = "Refresh";
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("JobOrderSCDetail", new { JOCodeURL = JOCodeURL, JoDate, TransType, BtnName, command, WF_Status1 }));
            //return RedirectToAction("JobOrderSCDetail");
        }
        public ActionResult GetJODashbordList(string docid, string status)
        {

            //Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("JobOrderSC", new { WF_Status });
        }

        //-----------------------------For Print Start--------------------------------
        public FileResult GenratePdfFile(JobOrderModel _JobOrderModel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            DataRow dtr = dt.NewRow();
            dtr["PrintFormat"] = _JobOrderModel.PrintFormat;
            dtr["ShowProdDesc"] = _JobOrderModel.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _JobOrderModel.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _JobOrderModel.ShowProdTechDesc;
            dtr["ShowSubItem"] = _JobOrderModel.ShowSubItem;
            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            return File(GetPdfData(_JobOrderModel.JO_No, _JobOrderModel.JO_Date), "application/pdf", "JobOrders.pdf");
        }
        public byte[] GetPdfData(string joNo, string joDate)
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
                DataSet Details = _JobOrder_ISERVICES.GetJobOrderDeatils(CompID, BrchID, joNo, joDate);
                
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();//.Replace("/", "\\'");
                 ViewBag.PageName = "JO";
                //ViewBag.Title = "Job Order";/*Change by Hina on 17-10-2024*/
                ViewBag.Title = "Job Work Order";
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 07-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SubContracting/JobOrder/JobOrderPrint.cshtml"));
                //string DelSchedule = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_PrintReportDeliverySchedule.cshtml")); //Commented By Nitesh 24-11-2023 For Delivery Schedule Print in one page with item print 
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    //  reader = new StringReader(DelSchedule);
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
                //reader.Dispose();
                //pdfDoc.Dispose();
                //writer.Dispose();
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

        /*---------------------------------For Print End--------------------------------*/

        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                JobOrderDetailsattch _JobOrdDetail = new JobOrderDetailsattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Guid gid = new Guid();
                gid = Guid.NewGuid();


                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _JobOrdDetail.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //CommonPageDetails();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _JobOrdDetail.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _JobOrdDetail.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _JobOrdDetail;
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

        /*--------------SubItem Start-------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, 
            string Flag, string Status, string jc_no, string jc_dt, string JobOrdNo, string JobOrdDt,
            string srctyp,string decimalAllowed,string HedrItemId)
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
                if (Status == "D" || Status == "F" || Status == "")
                {
                    if (srctyp=="Direct" )
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        decimalAllowed = "Y";
                    }
                    else
                    {
                        dt = _JobOrder_ISERVICES.JO_GetSubItemDetails(CompID, BrchID, Item_id, jc_no, jc_dt, Flag, Status, JobOrdNo, JobOrdDt).Tables[0];
                        Flag = "Quantity";
                    }
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                       
                        foreach (JObject item in arr.Children())//
                        {
                            if (srctyp == "Direct" /*&& Flag == "JO_HdrOrdQty"*/)
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString() /*&& item.GetValue("type").ToString() == "SubHedr"*/)
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                                if ((Flag == "JOReqQty") && (dt.Rows[0]["item_id"].ToString() == HedrItemId))
                                {
                                    IsDisabled = "Y";
                                    //if (dt.Rows[0]["item_id"].ToString() == HedrItemId)
                                    //{

                                    //}
                                }
                            }
                            else
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = item.GetValue("qty");
                                }
                            }
                            
                        }
                    }
                }
                else
                {
                    if (srctyp == "Direct")
                    {
                        dt = _JobOrder_ISERVICES.JO_GetSubItemDetails(CompID, BrchID, Item_id, jc_no, jc_dt, Flag, Status, JobOrdNo, JobOrdDt).Tables[0];

                        decimalAllowed = "Y";
                    }
                    else
                    {
                        dt = _JobOrder_ISERVICES.JO_GetSubItemDetails(CompID, BrchID, Item_id, jc_no, jc_dt, Flag, Status, JobOrdNo, JobOrdDt).Tables[0];
                        Flag = "Quantity";
                    }
                    
                }
                if (srctyp == "Direct" && Flag == "JOReqQty")
                {
                    
                    if (dt.Rows[0]["item_id"].ToString()== HedrItemId)
                    {
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "qty<>''";
                        if (dv.Count > 0)
                        {
                            dt = dv.ToTable();
                        }
                    }
                }
                
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    decimalAllowed= decimalAllowed == "Y" ? decimalAllowed: "" ,
                    
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    _subitemPageName = "JO"
                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag;
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

        /*-----------------SubItem End-----------------------*/

        /*--------------------------For PDF Print Start--------------------------*/
        //public FileResult GenratePdfFile(JobOrderModel _JobOrderModel)

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
        //        DataSet Details = _JobOrder_ISERVICES.GetJobOrderDeatils(CompID, BrchID, _JobOrderModel.JO_No, _JobOrderModel.JO_Date);
        //        ViewBag.PageName = "JO";
        //        ViewBag.Title = "Job Order";
        //        ViewBag.Details = Details;
        //        ViewBag.InvoiceTo = "";
        //        ViewBag.ApprovedBy = "Arvind Gupta";
        //        ViewBag.DocStatus = Details.Tables[0].Rows[0]["scjob_status"].ToString().Trim();
        //        string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SubContracting/JobOrder/JobOrderPrint.cshtml"));

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
        //            return File(bytes.ToArray(), "application/pdf", "JobOrder.pdf");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return File("ErrorPage", "application/pdf");
        //    }
        //    finally
        //    {
        //        //reader.Dispose();
        //        //pdfDoc.Dispose();
        //        //writer.Dispose();
        //    }
        //}
        //protected string ConvertPartialViewToString(PartialViewResult partialView)
        //{
        //    using (var sw = new StringWriter())
        //    {
        //        partialView.View = ViewEngines.Engines
        //          .FindPartialView(ControllerContext, partialView.ViewName).View;

        //        var vc = new ViewContext(
        //          ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
        //        partialView.View.Render(vc, sw);

        //        var partialViewString = sw.GetStringBuilder().ToString();

        //        return partialViewString;
        //    }
        //}

        /*--------------------------For PDF Print End--------------------------*/

        /*-----------------Job order tracking On List page Start-------------------------------*/
        public ActionResult GetJOTrackingDetail(string JONo, string JODate, string SuppName, string LstFgItemName, string LstFgUomName,
            string LstOprationName, string LstOpOutProductName, string LstOpOutProductUOM, string LstQuantity)
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
                DataSet result = _JobOrder_ISERVICES.GetJOTrackingDetail(Comp_ID, BranchID, JONo, JODate);


                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.JOTrackingList = result.Tables[0];
                ViewBag.JobOrdNo = JONo;
                ViewBag.JO_date = JODate;
                ViewBag.SuppName = SuppName;
                ViewBag.FnshdPrdtName = LstFgItemName;
                ViewBag.FnshdPrdctUom = LstFgUomName;
                ViewBag.OperationName = LstOprationName;
                ViewBag.OpOutPrdtName = LstOpOutProductName;
                ViewBag.OpOutPrdtUom = LstOpOutProductUOM;
                ViewBag.JOQuantity = LstQuantity;

                return View("~/Areas/ApplicationLayer/Views/Shared/_JobOrderTracking .cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        /*-----------------Job order tracking On List page End-------------------------------*/

        private DataSet JOGetAllData(JOListModel _JOListModel)
        {
            string CompID = string.Empty;
            string SuppName = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            string BranchId = Session["BranchId"].ToString();
            if (string.IsNullOrEmpty(_JOListModel.SuppName))
            {
                SuppName = "0";
            }
            else
            {
                SuppName = _JOListModel.SuppName;
            }
            if (Session["UserId"] != null)
            {
                UserID = Session["UserId"].ToString();
            }
            DataSet ds = _JobOrder_ISERVICES.GetAllData(CompID, BranchId, SuppName, _JOListModel,UserID,DocumentMenuId);
            return ds;
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