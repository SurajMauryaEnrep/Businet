using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.DeliveryNote;
using System.Text;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.DeliveryNote;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SubContracting.DeliveryNote
{
    public class DeliveryNoteSCController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105108110", title;
        List<DNSCList> _DNSCList;
        DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        DeliveryNoteDetailSC_IServices _DeliveryNoteDetailSC_IServices;
        CommonController cmn = new CommonController();
        public DeliveryNoteSCController(Common_IServices _Common_IServices, DeliveryNoteDetailSC_IServices _DeliveryNoteDetailSC_IServices)
        {
            this._Common_IServices = _Common_IServices;
            this._DeliveryNoteDetailSC_IServices = _DeliveryNoteDetailSC_IServices;
        }
        // GET: ApplicationLayer/DeliveryNoteSC
        public ActionResult DeliveryNoteSC(string WF_Status)
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
                DNListModel _DNListModel = new DNListModel();
                if (WF_Status != null && WF_Status != "")
                {
                    _DNListModel.WF_Status = WF_Status;

                }
                // DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                GetAutoCompleteSearchSuppList(_DNListModel);
                CommonPageDetails();
                _DNListModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _DNListModel.StatusList = statusLists;
                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        _DNListModel.SuppID = a[0].Trim();
                        _DNListModel.FromDate = a[1].Trim();
                        _DNListModel.ToDate = a[2].Trim();
                        _DNListModel.Status = a[3].Trim();
                        if (_DNListModel.Status == "0")
                        {
                            _DNListModel.Status = null;
                        }
                        _DNListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                }
                else
                {
                    _DNListModel.FromDate = startDate;
                    _DNListModel.ToDate = CurrentDate;
                }
                    _DNListModel.DeliveryNoteSCList = getDNSCList(_DNListModel);
               

                _DNListModel.Title = title;
                //Session["DNSCSearch"] = "0";
                _DNListModel.DNSCSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/SubContracting/DeliveryNote/DeliveryNoteSCList.cshtml", _DNListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
           
           
        }
        public ActionResult AddDeliveryNoteSCDetail()
        {
            DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();
            _DeliveryNoteDetailSC_Model.Message = "New";
            _DeliveryNoteDetailSC_Model.Command = "Add";
            _DeliveryNoteDetailSC_Model.AppStatus = "D";
            ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model.AppStatus;
            _DeliveryNoteDetailSC_Model.TransType = "Save";
            _DeliveryNoteDetailSC_Model.BtnName = "BtnAddNew";
            TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
            CommonPageDetails();
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
                return RedirectToAction("DeliveryNoteSC");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("DeliveryNoteSCDetail", "DeliveryNoteSC");
        }
        public ActionResult DeliveryNoteSCDetail(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model1, string DNCodeURL, string DNDate, string TransType, string BtnName, string command,string WF_Status1)
        {
            try
            {
                ViewBag.DocID = DocumentMenuId;
                CommonPageDetails();
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, DNDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _DeliveryNoteDetailSC_Model = TempData["ModelData"] as DeliveryNoteDetailSC_Model;
                if (_DeliveryNoteDetailSC_Model != null)
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
                    _DeliveryNoteDetailSC_Model.Title = title;
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));

                    _DeliveryNoteDetailSC_Model.Title = title;
                    _DeliveryNoteDetailSC_Model.ValDigit = ValDigit;
                    _DeliveryNoteDetailSC_Model.QtyDigit = QtyDigit;
                    _DeliveryNoteDetailSC_Model.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _DeliveryNoteDetailSC_Model.SupplierNameList = suppLists;
                    
                    List<JobOrderNo> jobOrd_NoLists = new List<JobOrderNo>();
                    jobOrd_NoLists.Add(new JobOrderNo { JobOrdnoId = "0", JobOrdnoVal = "---Select---" });
                    _DeliveryNoteDetailSC_Model.JobOrderNoList = jobOrd_NoLists;

                    List<MaterialDispatchNo> md_NoLists = new List<MaterialDispatchNo>();
                    md_NoLists.Add(new MaterialDispatchNo { MdNoId = "0", MdNoVal = "---Select---" });
                    _DeliveryNoteDetailSC_Model.MaterialDispatchNoList = md_NoLists;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DeliveryNoteDetailSC_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }


                    if (_DeliveryNoteDetailSC_Model.TransType == "Update" || _DeliveryNoteDetailSC_Model.Command == "Edit")

                    {
                        string DNSC_NO = _DeliveryNoteDetailSC_Model.DN_No;
                        string DNSC_Date = _DeliveryNoteDetailSC_Model.DN_Dt;
                        DataSet ds = _DeliveryNoteDetailSC_IServices.GetDNDetailEditUpdate(CompID, BrchID, DNSC_NO, DNSC_Date, UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            _DeliveryNoteDetailSC_Model.JobOrdTyp = ds.Tables[11].Rows[0]["JobOrdTyp"].ToString();
                            _DeliveryNoteDetailSC_Model.DN_No = ds.Tables[0].Rows[0]["dn_no"].ToString();
                            _DeliveryNoteDetailSC_Model.DocNoAttach = ds.Tables[0].Rows[0]["dn_no"].ToString();
                            _DeliveryNoteDetailSC_Model.DN_Dt = ds.Tables[0].Rows[0]["dn_date"].ToString();
                            _DeliveryNoteDetailSC_Model.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _DeliveryNoteDetailSC_Model.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _DeliveryNoteDetailSC_Model.SuppID, supp_name = _DeliveryNoteDetailSC_Model.SuppName });
                            _DeliveryNoteDetailSC_Model.SupplierNameList = suppLists;
                            _DeliveryNoteDetailSC_Model.Bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _DeliveryNoteDetailSC_Model.Bill_date = ds.Tables[0].Rows[0]["bill_date"].ToString();
                            _DeliveryNoteDetailSC_Model.Veh_no = ds.Tables[0].Rows[0]["veh_no"].ToString(); 
                            _DeliveryNoteDetailSC_Model.Veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"]).ToString(QtyDigit);
                            _DeliveryNoteDetailSC_Model.Remarks = ds.Tables[0].Rows[0]["dn_rem"].ToString();
                            //_DeliveryNoteDetailSC_Model.Remarks = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _DeliveryNoteDetailSC_Model.JobOrdNum = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _DeliveryNoteDetailSC_Model.JobOrd_Num = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                           
                            jobOrd_NoLists.Add(new JobOrderNo { JobOrdnoId = _DeliveryNoteDetailSC_Model.JobOrdNum, JobOrdnoVal = _DeliveryNoteDetailSC_Model.JobOrdNum });
                            _DeliveryNoteDetailSC_Model.JobOrderNoList = jobOrd_NoLists;
                            if (ds.Tables[0].Rows[0]["scjob_dt"] != null && ds.Tables[0].Rows[0]["scjob_dt"].ToString() != "")
                            {
                                _DeliveryNoteDetailSC_Model.JobOrdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["scjob_dt"]).ToString("yyyy-MM-dd");
                            }
                            _DeliveryNoteDetailSC_Model.FinishProduct = ds.Tables[0].Rows[0]["FItem"].ToString();
                            _DeliveryNoteDetailSC_Model.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _DeliveryNoteDetailSC_Model.FinishUom = ds.Tables[0].Rows[0]["FUOM"].ToString();
                            _DeliveryNoteDetailSC_Model.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();
                            //_DeliveryNoteDetailSC_Model.MDNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            //_DeliveryNoteDetailSC_Model.MD_Num = ds.Tables[0].Rows[0]["src_doc_no"].ToString();

                            //md_NoLists.Add(new MaterialDispatchNo { MdNoId = _DeliveryNoteDetailSC_Model.MDNo, MdNoVal = _DeliveryNoteDetailSC_Model.MDNo });
                            //_DeliveryNoteDetailSC_Model.MaterialDispatchNoList = md_NoLists;
                            //if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            //{
                            //    _DeliveryNoteDetailSC_Model.MDDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            //}


                            _DeliveryNoteDetailSC_Model.CreatedBy = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _DeliveryNoteDetailSC_Model.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _DeliveryNoteDetailSC_Model.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _DeliveryNoteDetailSC_Model.AmendedBy = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _DeliveryNoteDetailSC_Model.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _DeliveryNoteDetailSC_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _DeliveryNoteDetailSC_Model.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _DeliveryNoteDetailSC_Model.StatusName = ds.Tables[0].Rows[0]["DnStatus"].ToString();
                            _DeliveryNoteDetailSC_Model.DNStatus = ds.Tables[0].Rows[0]["dn_status"].ToString().Trim();

                            _DeliveryNoteDetailSC_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                            _DeliveryNoteDetailSC_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);//Cancelled

                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ReturnItemDetails= ds.Tables[7];
                            ViewBag.ItemDispatchQtyDetail = ds.Tables[8];
                            ViewBag.AttechmentDetails = ds.Tables[6];
                            ViewBag.SubItemDetails = ds.Tables[9];
                            ViewBag.SubItemByProductScrapDetails = ds.Tables[10];
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["dn_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _DeliveryNoteDetailSC_Model.Cancelled = true;
                        }
                          else
                        {
                            _DeliveryNoteDetailSC_Model.Cancelled = false;
                        }
                      
                        _DeliveryNoteDetailSC_Model.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        if (ViewBag.AppLevel != null && _DeliveryNoteDetailSC_Model.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _DeliveryNoteDetailSC_Model.BtnName = "Refresh";
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
                                        _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
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
                                        _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";

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
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
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
                                        _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
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
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
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
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }

                    if (_DeliveryNoteDetailSC_Model.BtnName != null)
                    {
                        _DeliveryNoteDetailSC_Model.BtnName = _DeliveryNoteDetailSC_Model.BtnName;
                    }
                    _DeliveryNoteDetailSC_Model.TransType = _DeliveryNoteDetailSC_Model.TransType;
                    ViewBag.TransType = _DeliveryNoteDetailSC_Model.TransType;


                    if (_DeliveryNoteDetailSC_Model.DocumentStatus == null)
                    {
                        _DeliveryNoteDetailSC_Model.DocumentStatus = "D";
                        ViewBag.DocumentCode = "D";
                    }
                    else
                    {
                        _DeliveryNoteDetailSC_Model.DocumentStatus = _DeliveryNoteDetailSC_Model.DocumentStatus;
                        ViewBag.DocumentCode = _DeliveryNoteDetailSC_Model.DocumentStatus;
                        ViewBag.Command = _DeliveryNoteDetailSC_Model.Command;
                    }
                    ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model.DocumentStatus;
                    ViewBag.DocumentCode = _DeliveryNoteDetailSC_Model.DocumentStatus;

                    return View("~/Areas/ApplicationLayer/Views/SubContracting/DeliveryNote/DeliveryNoteDetail.cshtml", _DeliveryNoteDetailSC_Model);

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

                    string ValDigit1 = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit1 = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit1 = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _DeliveryNoteDetailSC_Model1.ValDigit = ValDigit1;
                    _DeliveryNoteDetailSC_Model1.QtyDigit = QtyDigit1;
                    _DeliveryNoteDetailSC_Model1.RateDigit = RateDigit1;
                    ViewBag.ValDigit = ValDigit1;
                    ViewBag.QtyDigit = QtyDigit1;
                    ViewBag.RateDigit = RateDigit1;
                    ViewBag.DocumentStatus = "D";
                    if (WF_Status1 != null && WF_Status1 != "")
                    {
                        _DeliveryNoteDetailSC_Model1.WF_Status1 = WF_Status1;
                    }
                    List<SupplierName> suppLists1 = new List<SupplierName>();
                    suppLists1.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _DeliveryNoteDetailSC_Model1.SupplierNameList = suppLists1;

                    List<JobOrderNo> jobOrd_NoLists1 = new List<JobOrderNo>();
                    jobOrd_NoLists1.Add(new JobOrderNo { JobOrdnoId = "0", JobOrdnoVal = "---Select---" });
                    _DeliveryNoteDetailSC_Model1.JobOrderNoList = jobOrd_NoLists1;

                    List<MaterialDispatchNo> md_NoLists1 = new List<MaterialDispatchNo>();
                    md_NoLists1.Add(new MaterialDispatchNo { MdNoId = "0", MdNoVal = "---Select---" });
                    _DeliveryNoteDetailSC_Model1.MaterialDispatchNoList = md_NoLists1;

                    if (_DeliveryNoteDetailSC_Model1.TransType == "Update" || _DeliveryNoteDetailSC_Model1.Command == "Edit")

                    {
                        string DNSC_NO = DNCodeURL;
                        string DNSC_Date = DNDate;
                        DataSet ds = _DeliveryNoteDetailSC_IServices.GetDNDetailEditUpdate(CompID, BrchID, DNSC_NO, DNSC_Date, UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            _DeliveryNoteDetailSC_Model1.JobOrdTyp = ds.Tables[11].Rows[0]["JobOrdTyp"].ToString();
                            _DeliveryNoteDetailSC_Model1.DN_No = ds.Tables[0].Rows[0]["dn_no"].ToString();
                            _DeliveryNoteDetailSC_Model1.DocNoAttach = ds.Tables[0].Rows[0]["dn_no"].ToString();
                            _DeliveryNoteDetailSC_Model1.DN_Dt = ds.Tables[0].Rows[0]["dn_date"].ToString();
                            _DeliveryNoteDetailSC_Model1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _DeliveryNoteDetailSC_Model1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists1.Add(new SupplierName { supp_id = _DeliveryNoteDetailSC_Model1.SuppID, supp_name = _DeliveryNoteDetailSC_Model1.SuppName });
                            _DeliveryNoteDetailSC_Model1.SupplierNameList = suppLists1;
                            _DeliveryNoteDetailSC_Model1.Bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _DeliveryNoteDetailSC_Model1.Bill_date = ds.Tables[0].Rows[0]["bill_date"].ToString();
                            _DeliveryNoteDetailSC_Model1.Veh_no = ds.Tables[0].Rows[0]["veh_no"].ToString();
                            _DeliveryNoteDetailSC_Model1.Veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"].ToString()).ToString(QtyDigit1);
                            _DeliveryNoteDetailSC_Model1.Remarks = ds.Tables[0].Rows[0]["dn_rem"].ToString();
                            //_DeliveryNoteDetailSC_Model.Remarks = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_upto"]).ToString("yyyy-MM-dd");
                            _DeliveryNoteDetailSC_Model1.JobOrdNum = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _DeliveryNoteDetailSC_Model1.JobOrd_Num = ds.Tables[0].Rows[0]["scjob_no"].ToString();

                            jobOrd_NoLists1.Add(new JobOrderNo { JobOrdnoId = _DeliveryNoteDetailSC_Model1.JobOrdNum, JobOrdnoVal = _DeliveryNoteDetailSC_Model1.JobOrdNum });
                            _DeliveryNoteDetailSC_Model1.JobOrderNoList = jobOrd_NoLists1;
                            if (ds.Tables[0].Rows[0]["scjob_dt"] != null && ds.Tables[0].Rows[0]["scjob_dt"].ToString() != "")
                            {
                                _DeliveryNoteDetailSC_Model1.JobOrdDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["scjob_dt"]).ToString("yyyy-MM-dd");
                            }
                            _DeliveryNoteDetailSC_Model1.FinishProduct = ds.Tables[0].Rows[0]["FItem"].ToString();
                            _DeliveryNoteDetailSC_Model1.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _DeliveryNoteDetailSC_Model1.FinishUom = ds.Tables[0].Rows[0]["FUOM"].ToString();
                            _DeliveryNoteDetailSC_Model1.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();
                            _DeliveryNoteDetailSC_Model1.MDNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            _DeliveryNoteDetailSC_Model1.MD_Num = ds.Tables[0].Rows[0]["src_doc_no"].ToString();

                            md_NoLists1.Add(new MaterialDispatchNo { MdNoId = _DeliveryNoteDetailSC_Model1.MDNo, MdNoVal = _DeliveryNoteDetailSC_Model1.MDNo });
                            _DeliveryNoteDetailSC_Model1.MaterialDispatchNoList = md_NoLists1;
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _DeliveryNoteDetailSC_Model1.MDDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            _DeliveryNoteDetailSC_Model1.CreatedBy = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _DeliveryNoteDetailSC_Model1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _DeliveryNoteDetailSC_Model1.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _DeliveryNoteDetailSC_Model1.AmendedBy = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _DeliveryNoteDetailSC_Model1.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _DeliveryNoteDetailSC_Model1.ApprovedBy = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _DeliveryNoteDetailSC_Model1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _DeliveryNoteDetailSC_Model1.StatusName = ds.Tables[0].Rows[0]["DnStatus"].ToString();
                            _DeliveryNoteDetailSC_Model1.DNStatus = ds.Tables[0].Rows[0]["dn_status"].ToString().Trim();

                            _DeliveryNoteDetailSC_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                            _DeliveryNoteDetailSC_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);//Cancelled

                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ReturnItemDetails = ds.Tables[7];
                            ViewBag.ItemDispatchQtyDetail = ds.Tables[8];
                            ViewBag.AttechmentDetails = ds.Tables[6];
                            ViewBag.SubItemDetails = ds.Tables[9];
                            ViewBag.SubItemByProductScrapDetails = ds.Tables[10];
                            ViewBag.QtyDigit = QtyDigit1;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["dn_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _DeliveryNoteDetailSC_Model1.Cancelled = true;
                        }
                        else
                        {
                            _DeliveryNoteDetailSC_Model1.Cancelled = false;
                        }

                        _DeliveryNoteDetailSC_Model1.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        if (ViewBag.AppLevel != null && _DeliveryNoteDetailSC_Model1.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _DeliveryNoteDetailSC_Model1.BtnName = "Refresh";
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
                                        _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";
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
                                        _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";

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
                                    _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";
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
                                        _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";
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
                                    _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";
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
                                    _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _DeliveryNoteDetailSC_Model1.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }


                    var DNCode = "";
                    if (DNCodeURL != null)
                    {
                        DNCode = DNCodeURL;
                        _DeliveryNoteDetailSC_Model1.DN_No = DNCodeURL;
                    }
                    else
                    {
                        DNCode = _DeliveryNoteDetailSC_Model1.DN_No;
                    }
                    if (TransType != null)
                    {
                        _DeliveryNoteDetailSC_Model1.TransType = TransType;
                        ViewBag.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _DeliveryNoteDetailSC_Model1.Command = command;
                        ViewBag.Command = command;
                    }
                    if ((_DeliveryNoteDetailSC_Model1.DNStatus=="D"|| _DeliveryNoteDetailSC_Model1.DNStatus == "A"|| _DeliveryNoteDetailSC_Model1.DNStatus == "QP") && _DeliveryNoteDetailSC_Model1.Command== "Edit")
                    {
                        _DeliveryNoteDetailSC_Model1.Command = command;
                        _DeliveryNoteDetailSC_Model1.BtnName = "BtnEdit";
                        _DeliveryNoteDetailSC_Model1.TransType = "Update";

                    }
                    if (_DeliveryNoteDetailSC_Model1.BtnName == null && _DeliveryNoteDetailSC_Model1.Command == null)
                    {
                        _DeliveryNoteDetailSC_Model1.BtnName = "AddNew";
                        _DeliveryNoteDetailSC_Model1.Command = "Add";
                        _DeliveryNoteDetailSC_Model1.AppStatus = "D";
                        ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model1.AppStatus;
                        _DeliveryNoteDetailSC_Model1.TransType = "Save";
                        _DeliveryNoteDetailSC_Model1.BtnName = "BtnAddNew";

                    }
                    if(_DeliveryNoteDetailSC_Model1.DNStatus=="QC"|| _DeliveryNoteDetailSC_Model1.DNStatus=="GR"|| _DeliveryNoteDetailSC_Model1.DNStatus == "QP")
                    {
                        _DeliveryNoteDetailSC_Model1.BtnName = "BtnToDetailPage";
                        _DeliveryNoteDetailSC_Model1.TransType = "Update";
                        _DeliveryNoteDetailSC_Model1.Command = "Update";

                    }
                    if (_DeliveryNoteDetailSC_Model1.BtnName != null)
                    {
                        _DeliveryNoteDetailSC_Model1.BtnName = _DeliveryNoteDetailSC_Model1.BtnName;
                    }
                    
                    _DeliveryNoteDetailSC_Model1.TransType = _DeliveryNoteDetailSC_Model1.TransType;
                    ViewBag.TransType = _DeliveryNoteDetailSC_Model1.TransType;
                    if (_DeliveryNoteDetailSC_Model1.DocumentStatus != null)
                    {
                        _DeliveryNoteDetailSC_Model1.DocumentStatus = _DeliveryNoteDetailSC_Model1.DocumentStatus;
                        ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model1.DocumentStatus;
                        ViewBag.DocumentCode = _DeliveryNoteDetailSC_Model1.DocumentStatus;
                    }

                    _DeliveryNoteDetailSC_Model1.Title = title;
                    return View("~/Areas/ApplicationLayer/Views/SubContracting/DeliveryNote/DeliveryNoteDetail.cshtml", _DeliveryNoteDetailSC_Model1);

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
        public ActionResult DeliveryNoteSCBtnCommand(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model, string command)
        {
            try
            {/*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/

                if (_DeliveryNoteDetailSC_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //_DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();
                        //_DeliveryNoteDetailSC_Model.Message = "New";
                        //_DeliveryNoteDetailSC_Model.Command = "Add";
                        //_DeliveryNoteDetailSC_Model.AppStatus = "D";
                        //_DeliveryNoteDetailSC_Model.DocumentStatus = "D";
                        //ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model.DocumentStatus;
                        //_DeliveryNoteDetailSC_Model.TransType = "Save";
                        //_DeliveryNoteDetailSC_Model.BtnName = "BtnAddNew";
                        //TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                        DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_ModelAdd = new DeliveryNoteDetailSC_Model();
                        _DeliveryNoteDetailSC_ModelAdd.Message = "New";
                        _DeliveryNoteDetailSC_ModelAdd.Command = "Add";
                        _DeliveryNoteDetailSC_ModelAdd.AppStatus = "D";
                        _DeliveryNoteDetailSC_ModelAdd.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model.DocumentStatus;
                        _DeliveryNoteDetailSC_ModelAdd.TransType = "Save";
                        _DeliveryNoteDetailSC_ModelAdd.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _DeliveryNoteDetailSC_ModelAdd;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_DeliveryNoteDetailSC_Model.DN_No))
                                return RedirectToAction("DoubleClickFromList", new { DocNo = _DeliveryNoteDetailSC_Model.DN_No, DocDate = _DeliveryNoteDetailSC_Model.DN_Dt, ListFilterData = _DeliveryNoteDetailSC_Model.ListFilterData1, WF_status = _DeliveryNoteDetailSC_Model.WFStatus });
                            else
                                _DeliveryNoteDetailSC_ModelAdd.Command = "Refresh";
                            _DeliveryNoteDetailSC_ModelAdd.TransType = "Refresh";
                            _DeliveryNoteDetailSC_ModelAdd.BtnName = "Refresh";
                            _DeliveryNoteDetailSC_ModelAdd.DocumentStatus = null;
                            TempData["ModelData"] = _DeliveryNoteDetailSC_ModelAdd;
                            return RedirectToAction("DeliveryNoteSCDetail", "DeliveryNoteSC");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("DeliveryNoteSCDetail", "DeliveryNoteSC");

                    case "Edit":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _DeliveryNoteDetailSC_Model.DN_No, DocDate = _DeliveryNoteDetailSC_Model.DN_Dt, ListFilterData = _DeliveryNoteDetailSC_Model.ListFilterData1, WF_status = _DeliveryNoteDetailSC_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string dnDt = _DeliveryNoteDetailSC_Model.DN_Dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, dnDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _DeliveryNoteDetailSC_Model.DN_No, DocDate = _DeliveryNoteDetailSC_Model.DN_Dt, ListFilterData = _DeliveryNoteDetailSC_Model.ListFilterData1, WF_status = _DeliveryNoteDetailSC_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        var TransType = "";
                        var BtnName = "";
                        if (_DeliveryNoteDetailSC_Model.DNStatus == "A"|| _DeliveryNoteDetailSC_Model.DNStatus == "QP")
                        { if (_DeliveryNoteDetailSC_Model.DNStatus == "QP")
                            {
                                if (CheckQCAgainstDNSC(_DeliveryNoteDetailSC_Model) == "Used")
                                {
                                    _DeliveryNoteDetailSC_Model.Message = "Used";
                                    _DeliveryNoteDetailSC_Model.TransType = "Update";
                                    _DeliveryNoteDetailSC_Model.Command = "Add";
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
                                    TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                                }

                                else
                                {
                                    _DeliveryNoteDetailSC_Model.TransType = "Update";
                                    _DeliveryNoteDetailSC_Model.Command = command;
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnEdit";
                                    _DeliveryNoteDetailSC_Model.Message = "New";
                                    _DeliveryNoteDetailSC_Model.AppStatus = "D";
                                    _DeliveryNoteDetailSC_Model.DocumentStatus = "D";
                                    ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model.AppStatus;
                                    TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                                }
                            }
                            else
                            {
                                if (CheckGRNSCAgainstDeliveryNoteSC(_DeliveryNoteDetailSC_Model) == "Used1")
                                {
                                    _DeliveryNoteDetailSC_Model.Message = "Used1";
                                    _DeliveryNoteDetailSC_Model.TransType = "Update";
                                    _DeliveryNoteDetailSC_Model.Command = "Add";
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
                                    TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                                }

                                else
                                {
                                    _DeliveryNoteDetailSC_Model.TransType = "Update";
                                    _DeliveryNoteDetailSC_Model.Command = command;
                                    _DeliveryNoteDetailSC_Model.BtnName = "BtnEdit";
                                    _DeliveryNoteDetailSC_Model.Message = "New";
                                    _DeliveryNoteDetailSC_Model.AppStatus = "D";
                                    _DeliveryNoteDetailSC_Model.DocumentStatus = "D";
                                    ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model.AppStatus;
                                    TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                                }
                            }
                        }
                        else if (_DeliveryNoteDetailSC_Model.DNStatus == "PDL")
                        {
                            _DeliveryNoteDetailSC_Model.Message = "New";
                            _DeliveryNoteDetailSC_Model.TransType = "Update";
                            _DeliveryNoteDetailSC_Model.Command = "Edit";
                            _DeliveryNoteDetailSC_Model.BtnName = "BtnEdit";
                            TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                        }
                        else
                        {
                            _DeliveryNoteDetailSC_Model.TransType = "Update";
                            _DeliveryNoteDetailSC_Model.Command = command;
                            _DeliveryNoteDetailSC_Model.BtnName = "BtnEdit";
                            _DeliveryNoteDetailSC_Model.Message = "New";
                            _DeliveryNoteDetailSC_Model.AppStatus = "D";
                            _DeliveryNoteDetailSC_Model.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _DeliveryNoteDetailSC_Model.AppStatus;
                            TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                        }
                        var DNCodeURL = _DeliveryNoteDetailSC_Model.DN_No;
                        var DNDate = _DeliveryNoteDetailSC_Model.DN_Dt;
                        command = _DeliveryNoteDetailSC_Model.Command;
                        TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                        TempData["ListFilterData"] = _DeliveryNoteDetailSC_Model.ListFilterData1;
                        return (RedirectToAction("DeliveryNoteSCDetail", new { DNCodeURL = DNCodeURL, DNDate, TransType, BtnName, command }));

                    case "Delete":
                        _DeliveryNoteDetailSC_Model.Command = command;
                        _DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                        DeleteDNDetails(_DeliveryNoteDetailSC_Model, command);
                        TempData["ListFilterData"] = _DeliveryNoteDetailSC_Model.ListFilterData1;
                        return RedirectToAction("DeliveryNoteSCDetail");

                    case "Save":
                        _DeliveryNoteDetailSC_Model.Command = command;
                        SaveDNSCDetail(_DeliveryNoteDetailSC_Model);
                        if (_DeliveryNoteDetailSC_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_DeliveryNoteDetailSC_Model.Message == "DocModify")
                        {
                            //_DeliveryNoteDetailSC_Model.DocumentMenuId = _DeliveryNoteDetailSC_Model.DocumentMenuId;
                            DocumentMenuId = _DeliveryNoteDetailSC_Model.DocumentMenuId;
                            CommonPageDetails();
                            //ViewBag.DocID = _DeliveryNoteDetailSC_Model.DocumentMenuId;
                            ViewBag.DocumentMenuId = _DeliveryNoteDetailSC_Model.DocumentMenuId;
                            ViewBag.DocumentStatus = "D";


                            List<SupplierName> suppLists1 = new List<SupplierName>();
                            suppLists1.Add(new SupplierName { supp_id = _DeliveryNoteDetailSC_Model.SuppID, supp_name = _DeliveryNoteDetailSC_Model.SuppName });
                            _DeliveryNoteDetailSC_Model.SupplierNameList = suppLists1;

                            List<JobOrderNo> jobOrd_NoLists = new List<JobOrderNo>();
                            jobOrd_NoLists.Add(new JobOrderNo { JobOrdnoId = _DeliveryNoteDetailSC_Model.JobOrd_Num, JobOrdnoVal = _DeliveryNoteDetailSC_Model.JobOrd_Num });
                            _DeliveryNoteDetailSC_Model.JobOrderNoList = jobOrd_NoLists;

                            List<MaterialDispatchNo> md_NoLists = new List<MaterialDispatchNo>();
                            md_NoLists.Add(new MaterialDispatchNo { MdNoId = "0", MdNoVal = "---Select---" });
                            _DeliveryNoteDetailSC_Model.MaterialDispatchNoList = md_NoLists;

                            _DeliveryNoteDetailSC_Model.JobOrdNum = _DeliveryNoteDetailSC_Model.JobOrd_Num;
                            _DeliveryNoteDetailSC_Model.JobOrdDate = _DeliveryNoteDetailSC_Model.JobOrdDate;
                            _DeliveryNoteDetailSC_Model.Bill_no = _DeliveryNoteDetailSC_Model.Bill_no;
                            _DeliveryNoteDetailSC_Model.Bill_date = _DeliveryNoteDetailSC_Model.Bill_date;

                            ViewBag.ItemDetailsList = ViewData["ItemDetails"];
                            ViewBag.ReturnItemDetails = ViewData["ReturnItemDetails"];
                            ViewBag.ItemDispatchQtyDetail = ViewData["MDOrderQtyItemDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            ViewBag.SubItemByProductScrapDetails = ViewData["ScrapSubItemDetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                            _DeliveryNoteDetailSC_Model.Command = "Refresh";
                            _DeliveryNoteDetailSC_Model.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            _DeliveryNoteDetailSC_Model.ValDigit = ValDigit;
                            _DeliveryNoteDetailSC_Model.QtyDigit = QtyDigit;
                            _DeliveryNoteDetailSC_Model.RateDigit = RateDigit;
                            return View("~/Areas/ApplicationLayer/Views/SubContracting/DeliveryNote/DeliveryNoteDetail.cshtml", _DeliveryNoteDetailSC_Model);
                        }
                        else
                        {
                            DNCodeURL = _DeliveryNoteDetailSC_Model.DN_No;
                            DNDate = _DeliveryNoteDetailSC_Model.DN_Dt;
                            TransType = _DeliveryNoteDetailSC_Model.TransType;
                            BtnName = _DeliveryNoteDetailSC_Model.BtnName;
                            TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                            TempData["ListFilterData"] = _DeliveryNoteDetailSC_Model.ListFilterData1;
                            return (RedirectToAction("DeliveryNoteSCDetail", new { DNCodeURL = DNCodeURL, DNDate, TransType, BtnName, command }));
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
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _DeliveryNoteDetailSC_Model.DN_No, DocDate = _DeliveryNoteDetailSC_Model.DN_Dt, ListFilterData = _DeliveryNoteDetailSC_Model.ListFilterData1, WF_status = _DeliveryNoteDetailSC_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string dnDt1 = _DeliveryNoteDetailSC_Model.DN_Dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, dnDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _DeliveryNoteDetailSC_Model.DN_No, DocDate = _DeliveryNoteDetailSC_Model.DN_Dt, ListFilterData = _DeliveryNoteDetailSC_Model.ListFilterData1, WF_status = _DeliveryNoteDetailSC_Model.WFStatus });
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
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _DeliveryNoteDetailSC_Model.DN_No, DocDate = _DeliveryNoteDetailSC_Model.DN_Dt, ListFilterData = _DeliveryNoteDetailSC_Model.ListFilterData1, WF_status = _DeliveryNoteDetailSC_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string dnDt2 = _DeliveryNoteDetailSC_Model.DN_Dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, dnDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _DeliveryNoteDetailSC_Model.DN_No, DocDate = _DeliveryNoteDetailSC_Model.DN_Dt, ListFilterData = _DeliveryNoteDetailSC_Model.ListFilterData1, WF_status = _DeliveryNoteDetailSC_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _DeliveryNoteDetailSC_Model.Command = command;
                        DNApprove(_DeliveryNoteDetailSC_Model,"");

                        DNCodeURL = _DeliveryNoteDetailSC_Model.DN_No;
                        DNDate = _DeliveryNoteDetailSC_Model.DN_Dt;
                        TransType = _DeliveryNoteDetailSC_Model.TransType;
                        BtnName = _DeliveryNoteDetailSC_Model.BtnName;
                        TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                        TempData["ListFilterData"] = _DeliveryNoteDetailSC_Model.ListFilterData1;
                        return (RedirectToAction("DeliveryNoteSCDetail", new { DNCodeURL = DNCodeURL, DNDate, TransType, BtnName, command }));



                    case "Refresh":
                        DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_ModelRefresh = new DeliveryNoteDetailSC_Model();
                        //_DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();
                        _DeliveryNoteDetailSC_Model.Message = null;
                        //_DeliveryNoteDetailSC_Model.Command = command;
                        //_DeliveryNoteDetailSC_Model.TransType = "Refresh";
                        //_DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                        //_DeliveryNoteDetailSC_Model.DocumentStatus = null;
                        _DeliveryNoteDetailSC_ModelRefresh.Command = command;
                        _DeliveryNoteDetailSC_ModelRefresh.TransType = "Refresh";
                        _DeliveryNoteDetailSC_ModelRefresh.BtnName = "Refresh";
                        _DeliveryNoteDetailSC_ModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _DeliveryNoteDetailSC_ModelRefresh;
                        TempData["ListFilterData"] = _DeliveryNoteDetailSC_Model.ListFilterData1;
                        return RedirectToAction("DeliveryNoteSCDetail");

                    case "Print":
                    return GenratePdfFile(_DeliveryNoteDetailSC_Model);
                    case "BacktoList":
                        var WF_Status = _DeliveryNoteDetailSC_Model.WF_Status1;
                        TempData["ListFilterData"] = _DeliveryNoteDetailSC_Model.ListFilterData1;
                        return RedirectToAction("DeliveryNoteSC", "DeliveryNoteSC",new { WF_Status });

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
        public ActionResult SaveDNSCDetail(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model)
        {
            string SaveMessage = "";
            /*getDocumentName();*/ /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_DeliveryNoteDetailSC_Model.Cancelled == false)
                {
                    _DeliveryNoteDetailSC_Model.DocumentMenuId = _DeliveryNoteDetailSC_Model.DocumentMenuId;


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
                    DataTable DispatchQtyItemDetails = new DataTable();
                    DataTable DtblAttchDetail = new DataTable();
                    DataTable DtblItemReturnDetail = new DataTable();
                    DataTable dtheader = new DataTable();

                    DtblHDetail = ToDtblHDetail(_DeliveryNoteDetailSC_Model);
                    DtblItemDetail = ToDtblItemDetail(_DeliveryNoteDetailSC_Model.DnItemdetails);
                    // DispatchQtyItemDetails = ToDtblDispatchQtyItemDetail(_DeliveryNoteDetailSC_Model.ItemDispatchQtyDetail);
                    DtblItemReturnDetail = ToDtblItemReturnDetail(_DeliveryNoteDetailSC_Model.DnItemReturndetails);

                    DataTable dtDispQtyItem = new DataTable();

                    dtDispQtyItem.Columns.Add("ItemID", typeof(string));
                    dtDispQtyItem.Columns.Add("UomID", typeof(string));
                    dtDispQtyItem.Columns.Add("Disp_No", typeof(string));
                    dtDispQtyItem.Columns.Add("Disp_Dt", typeof(string));

                    dtDispQtyItem.Columns.Add("DispQty", typeof(string));
                    dtDispQtyItem.Columns.Add("PendQty", typeof(string));
                    dtDispQtyItem.Columns.Add("BillQty", typeof(string));
                    dtDispQtyItem.Columns.Add("ReceivQty", typeof(string));
                    //dtDispQtyItem.Columns.Add("bal_qty", typeof(string));
                    //dtDispQtyItem.Columns.Add("pack_qty", typeof(string));

                    if (_DeliveryNoteDetailSC_Model.ItemDispatchQtyDetail != null)
                    {
                        JArray jObjectOrderQty = JArray.Parse(_DeliveryNoteDetailSC_Model.ItemDispatchQtyDetail);
                        for (int i = 0; i < jObjectOrderQty.Count; i++)
                        {
                            DataRow dtrowOrderDetails = dtDispQtyItem.NewRow();
                            dtrowOrderDetails["ItemID"] = jObjectOrderQty[i]["ItemID"].ToString();
                            dtrowOrderDetails["UomID"] = jObjectOrderQty[i]["UomID"].ToString();
                            dtrowOrderDetails["Disp_No"] = jObjectOrderQty[i]["DocNo"].ToString();
                            dtrowOrderDetails["Disp_Dt"] = jObjectOrderQty[i]["DocDate"].ToString();

                            dtrowOrderDetails["DispQty"] = jObjectOrderQty[i]["DispatchQty"].ToString();
                            dtrowOrderDetails["PendQty"] = jObjectOrderQty[i]["PendingQty"].ToString();
                            dtrowOrderDetails["BillQty"] = jObjectOrderQty[i]["BilledQty"].ToString();
                            dtrowOrderDetails["ReceivQty"] = jObjectOrderQty[i]["ReceiveQty"].ToString();
                            dtDispQtyItem.Rows.Add(dtrowOrderDetails);
                        }
                        ViewData["MDOrderQtyItemDetails"] = dtMDOrderdetail(jObjectOrderQty);
                    }
                    DispatchQtyItemDetails = dtDispQtyItem;

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("Disp_qty", typeof(string));
                    dtSubItem.Columns.Add("Pend_qty", typeof(string));
                    dtSubItem.Columns.Add("Bill_qty", typeof(string));
                    dtSubItem.Columns.Add("Rec_qty", typeof(string));
                    dtSubItem.Columns.Add("Disp_No", typeof(string));
                    dtSubItem.Columns.Add("Disp_Dt", typeof(string));
                    if (_DeliveryNoteDetailSC_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_DeliveryNoteDetailSC_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["Disp_qty"] = jObject2[i]["DispatchQty"].ToString();
                            dtrowItemdetails["Pend_qty"] = jObject2[i]["PendingQuantity"].ToString();
                            dtrowItemdetails["Bill_qty"] = jObject2[i]["BilledQty"].ToString();
                            dtrowItemdetails["Rec_qty"] = jObject2[i]["ReceivedQuantity"].ToString();
                            dtrowItemdetails["Disp_No"] = jObject2[i]["DispSrcDocNo"].ToString();
                            dtrowItemdetails["Disp_Dt"] = jObject2[i]["DispSrcDocDt"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                    }

                    /*------------------Sub Item end----------------------*/
                    /*----------------------Sub Item ----------------------*/
                    DataTable dtByPrdctScrapSubItem = new DataTable();
                    dtByPrdctScrapSubItem.Columns.Add("item_id", typeof(string));
                    dtByPrdctScrapSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtByPrdctScrapSubItem.Columns.Add("Qty", typeof(string));
                    
                    if (_DeliveryNoteDetailSC_Model.SubItemByProductScrapDetailsDt != null)
                    {
                        JArray jObject3 = JArray.Parse(_DeliveryNoteDetailSC_Model.SubItemByProductScrapDetailsDt);
                        for (int i = 0; i < jObject3.Count; i++)
                        {
                            DataRow dtrowPrdtscrapItemdetails = dtByPrdctScrapSubItem.NewRow();
                            dtrowPrdtscrapItemdetails["item_id"] = jObject3[i]["item_id"].ToString();
                            dtrowPrdtscrapItemdetails["sub_item_id"] = jObject3[i]["sub_item_id"].ToString();
                            dtrowPrdtscrapItemdetails["Qty"] = jObject3[i]["Qty"].ToString();

                            dtByPrdctScrapSubItem.Rows.Add(dtrowPrdtscrapItemdetails);
                        }
                        ViewData["ScrapSubItemDetail"] = dtscrapsubitemdetail(jObject3);
                    }

                    /*------------------Sub Item end----------------------*/


                    var _JobOrderDetailsattch = TempData["ModelDataattch"] as DNDetailsattch;
                    TempData["ModelDataattch"] = null;
                    DataTable dtAttachment = new DataTable();
                    if (_DeliveryNoteDetailSC_Model.attatchmentdetail != null)
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
                            if (_DeliveryNoteDetailSC_Model.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _DeliveryNoteDetailSC_Model.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_DeliveryNoteDetailSC_Model.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_DeliveryNoteDetailSC_Model.DN_No))
                                {
                                    dtrowAttachment1["id"] = _DeliveryNoteDetailSC_Model.DN_No;
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

                        if (_DeliveryNoteDetailSC_Model.TransType == "Update")
                        {

                            string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_DeliveryNoteDetailSC_Model.DN_No))
                                {
                                    ItmCode = _DeliveryNoteDetailSC_Model.DN_No;
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

                    SaveMessage = _DeliveryNoteDetailSC_IServices.InsertDN_Details(DtblHDetail, DtblItemDetail, DtblItemReturnDetail, DispatchQtyItemDetails, DtblAttchDetail, dtSubItem, dtByPrdctScrapSubItem);
                    if (SaveMessage == "DocModify")
                    {
                        _DeliveryNoteDetailSC_Model.Message = "DocModify";
                        _DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                        _DeliveryNoteDetailSC_Model.Command = "Refresh";
                        _DeliveryNoteDetailSC_Model.DocumentMenuId = DocumentMenuId;
                        TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                        return RedirectToAction("DeliveryNoteSCDetail");
                    }
                    else
                    {
                        string[] Data = SaveMessage.Split(',');

                        string DNNo = Data[1];
                        string DN_No = DNNo.Replace("/", "");
                        string Message = Data[0];
                        if (Message == "Data_Not_Found")
                        {
                            var a = DN_No.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _DeliveryNoteDetailSC_Model.Message = Message.Replace("_", "");
                            return RedirectToAction("DeliveryNoteSCDetail");
                        }
                        string DNDate = Data[2];
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
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, DN_No, _DeliveryNoteDetailSC_Model.TransType, DtblAttchDetail);

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
                            //                string DN_No1 = DN_No.Replace("/", "");
                            //                string img_nm = CompID + BrchID + DN_No1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                        //if (Message1 == "Cancelled")
                        //{
                        //    _DeliveryNoteDetailSC_Model.Message = "Cancelled";
                        //    _DeliveryNoteDetailSC_Model.Command = "Update";
                        //    _DeliveryNoteDetailSC_Model.TransType = "Update";
                        //    _DeliveryNoteDetailSC_Model.AppStatus = "D";
                        //    _DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                        //    _DeliveryNoteDetailSC_Model.JO_No = JONo;
                        //    _DeliveryNoteDetailSC_Model.JO_Date = JODate;
                        //    TempData["ModelData"] = _DeliveryNoteDetailSC_Model;

                        //    return RedirectToAction("JobOrderSCDetail");
                        //}


                        if (Message == "Update" || Message == "Save")
                        {
                            _DeliveryNoteDetailSC_Model.Message = "Save";

                            _DeliveryNoteDetailSC_Model.DN_No = DNNo;
                            _DeliveryNoteDetailSC_Model.DN_Dt = DNDate;
                            _DeliveryNoteDetailSC_Model.TransType = "Update";
                            _DeliveryNoteDetailSC_Model.Command = "Update";
                            _DeliveryNoteDetailSC_Model.AppStatus = "D";
                            _DeliveryNoteDetailSC_Model.DocumentStatus = "D";

                            _DeliveryNoteDetailSC_Model.BtnName = "BtnSave";
                            TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                            return RedirectToAction("DeliveryNoteSCDetail");

                        }

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
                    _DeliveryNoteDetailSC_Model.CreatedBy = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage1 = _DeliveryNoteDetailSC_IServices.DNCancel(_DeliveryNoteDetailSC_Model, CompID, br_id, mac_id);

                    _DeliveryNoteDetailSC_Model.Message = "Cancelled";
                    _DeliveryNoteDetailSC_Model.Command = "Update";
                    _DeliveryNoteDetailSC_Model.DN_No = _DeliveryNoteDetailSC_Model.DN_No;
                    _DeliveryNoteDetailSC_Model.DN_Dt = _DeliveryNoteDetailSC_Model.DN_Dt;
                    _DeliveryNoteDetailSC_Model.TransType = "Update";
                    _DeliveryNoteDetailSC_Model.AppStatus = "D";
                    _DeliveryNoteDetailSC_Model.BtnName = "Refresh";
                    TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                    return RedirectToAction("DeliveryNoteSCDetail");
                }
                //TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                return RedirectToAction("DeliveryNoteSCDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_DeliveryNoteDetailSC_Model.TransType == "Save")
                    {
                        string Guid = "";
                        if (_DeliveryNoteDetailSC_Model.Guid != null)
                        {
                            Guid = _DeliveryNoteDetailSC_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable ToDtblHDetail(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model)
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
                dtheader.Columns.Add("DN_No", typeof(string));
                dtheader.Columns.Add("DN_Date", typeof(string));
                dtheader.Columns.Add("SuppID", typeof(int));
                dtheader.Columns.Add("BillNo", typeof(string));
                dtheader.Columns.Add("BillDate", typeof(string));
                dtheader.Columns.Add("VechNo", typeof(string));
                dtheader.Columns.Add("VechLoad", typeof(float));
                dtheader.Columns.Add("JobOrdDocNo", typeof(string));
                dtheader.Columns.Add("JobOrdDocDate", typeof(string));
                dtheader.Columns.Add("FItemID", typeof(string));
                dtheader.Columns.Add("FUomId", typeof(int));
                dtheader.Columns.Add("MDDocNo", typeof(string));
                dtheader.Columns.Add("MDDocDate", typeof(string));
                dtheader.Columns.Add("Remarks", typeof(string));
               
              
                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("DNStatus", typeof(string));
                dtheader.Columns.Add("SystemDetail", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _DeliveryNoteDetailSC_Model.TransType;
                //dtrowHeader["Cancelled"] = ConvertBoolToStrint(_DeliveryNoteDetailSC_Model.Cancelled);
                dtrowHeader["DN_No"] = _DeliveryNoteDetailSC_Model.DN_No;
                dtrowHeader["DN_Date"] = _DeliveryNoteDetailSC_Model.DN_Dt;
                dtrowHeader["SuppID"] = _DeliveryNoteDetailSC_Model.SuppID;
                dtrowHeader["BillNo"] = _DeliveryNoteDetailSC_Model.Bill_no;
                dtrowHeader["BillDate"] = _DeliveryNoteDetailSC_Model.Bill_date;
                dtrowHeader["VechNo"] = _DeliveryNoteDetailSC_Model.Veh_no;
                if (!string.IsNullOrEmpty(_DeliveryNoteDetailSC_Model.Veh_load))
                {
                    dtrowHeader["VechLoad"] = _DeliveryNoteDetailSC_Model.Veh_load;
                }
                else
                {
                    dtrowHeader["VechLoad"] = "0";
                }
                //dtrowHeader["VechLoad"] = _DeliveryNoteDetailSC_Model.Veh_load;
                dtrowHeader["JobOrdDocNo"] = _DeliveryNoteDetailSC_Model.JobOrd_Num;
                dtrowHeader["JobOrdDocDate"] = _DeliveryNoteDetailSC_Model.JobOrdDate;
                dtrowHeader["FItemID"] = _DeliveryNoteDetailSC_Model.FinishProductId;
                dtrowHeader["FUomId"] = _DeliveryNoteDetailSC_Model.FinishUomId;
                dtrowHeader["MDDocNo"] = _DeliveryNoteDetailSC_Model.MD_Num;
                dtrowHeader["MDDocDate"] = _DeliveryNoteDetailSC_Model.MDDate;
                dtrowHeader["Remarks"] = _DeliveryNoteDetailSC_Model.Remarks;
                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["DNStatus"] = IsNull(_DeliveryNoteDetailSC_Model.DNStatus, "D");
               
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
        private DataTable ToDtblItemDetail(string dnItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                //dtItem.Columns.Add("MDSrcDocNo", typeof(string));
                //dtItem.Columns.Add("MDSrcDocDate", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(string));
                dtItem.Columns.Add("DispatchQty", typeof(float));
                dtItem.Columns.Add("PendingQty", typeof(float));
                dtItem.Columns.Add("BillQty", typeof(float));
                dtItem.Columns.Add("RecdQty", typeof(float));
                dtItem.Columns.Add("QCCheck", typeof(string));
                dtItem.Columns.Add("AcceptQty", typeof(float));
                dtItem.Columns.Add("RejectQty", typeof(float));
                dtItem.Columns.Add("ReworkQty", typeof(float));
                dtItem.Columns.Add("Remarks", typeof(string));
                

                if (dnItemDetail != null)
                {
                    JArray jObject = JArray.Parse(dnItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal Disp_qty, Pend_qty, Bill_qty, Recd_qty, Accept_qty, Reject_qty, Rework_qty;
                        if (jObject[i]["DispQty"].ToString() == "")
                            Disp_qty = 0;
                        else
                            Disp_qty = Convert.ToDecimal(jObject[i]["DispQty"].ToString());

                        if (jObject[i]["PendQty"].ToString() == "")
                            Pend_qty = 0;
                        else
                            Pend_qty = Convert.ToDecimal(jObject[i]["PendQty"].ToString());

                        if (jObject[i]["BillQty"].ToString() == "") 
                             Bill_qty = 0;
                        else
                            Bill_qty = Convert.ToDecimal(jObject[i]["BillQty"].ToString());

                        if (jObject[i]["RecievQty"].ToString() == "")
                            Recd_qty = 0;
                        else
                            Recd_qty = Convert.ToDecimal(jObject[i]["RecievQty"].ToString());

                        if (jObject[i]["AccptQty"].ToString() == "")
                            Accept_qty = 0;
                        else
                            Accept_qty = Convert.ToDecimal(jObject[i]["AccptQty"].ToString());

                        if (jObject[i]["RejQty"].ToString() == "")
                            Reject_qty = 0;
                        else
                            Reject_qty = Convert.ToDecimal(jObject[i]["RejQty"].ToString());

                        if (jObject[i]["RewrkQty"].ToString() == "")
                            Rework_qty = 0;
                        else
                            Rework_qty = Convert.ToDecimal(jObject[i]["RewrkQty"].ToString());

                        DataRow dtrowLines = dtItem.NewRow();

                        //dtrowLines["MDSrcDocNo"] = jObject[i]["SRCDocNo"].ToString();
                        //dtrowLines["MDSrcDocDate"] = jObject[i]["SRCDocDate"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        //dtrowLines["DispatchQty"] = jObject[i]["DispQty"].ToString();
                        //dtrowLines["BillQty"] = jObject[i]["BillQty"].ToString();
                        //dtrowLines["RecdQty"] = jObject[i]["RecievQty"].ToString();
                        //dtrowLines["QCCheck"] = jObject[i]["QCRequired"].ToString();
                        //dtrowLines["AcceptQty"] = jObject[i]["AccptQty"].ToString();
                        //dtrowLines["RejectQty"] = jObject[i]["RejQty"].ToString();
                        //dtrowLines["ReworkQty"] = jObject[i]["RewrkQty"].ToString();
                        dtrowLines["DispatchQty"] = Disp_qty;
                        dtrowLines["PendingQty"] = Pend_qty;
                        dtrowLines["BillQty"] = Bill_qty;
                        dtrowLines["RecdQty"] = Recd_qty;
                        dtrowLines["QCCheck"] = jObject[i]["QCRequired"].ToString();
                        dtrowLines["AcceptQty"] = Accept_qty;
                        dtrowLines["RejectQty"] = Reject_qty;
                        dtrowLines["ReworkQty"] = Rework_qty;
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
        private DataTable ToDtblItemReturnDetail(string dnItemReturnDetail)
        {
            try
            {
                DataTable DtblItemReturnDetail = new DataTable();
                DataTable dtItemreturn = new DataTable();

                dtItemreturn.Columns.Add("ItemID", typeof(string));
                dtItemreturn.Columns.Add("UOMID", typeof(string));
                dtItemreturn.Columns.Add("ItemTypeID", typeof(string));
                //dtItemreturn.Columns.Add("IssueQty", typeof(float));
                dtItemreturn.Columns.Add("ReceivedQty", typeof(float));


                if (dnItemReturnDetail != null)
                {
                    JArray jObject = JArray.Parse(dnItemReturnDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {


                        DataRow dtrowLines = dtItemreturn.NewRow();

                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();

                        dtrowLines["ItemTypeID"] = jObject[i]["ItmTypId"].ToString();
                        //dtrowLines["IssueQty"] = jObject[i]["IssuedQty"].ToString();
                        dtrowLines["ReceivedQty"] = jObject[i]["ReceiveQty"].ToString();


                        dtItemreturn.Rows.Add(dtrowLines);
                    }
                    ViewData["ReturnItemDetails"] = dtReturnItemdetail(jObject);
                }

                DtblItemReturnDetail = dtItemreturn;
                return DtblItemReturnDetail;
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
            dtItem.Columns.Add("disp_qty", typeof(float));
            dtItem.Columns.Add("pend_qty", typeof(float));
            dtItem.Columns.Add("bill_qty", typeof(float));
            dtItem.Columns.Add("recd_qty", typeof(float));
            dtItem.Columns.Add("qc_check", typeof(string));
            dtItem.Columns.Add("i_qc", typeof(string));
            dtItem.Columns.Add("accept_qty", typeof(float));
            dtItem.Columns.Add("reject_qty", typeof(float));
            dtItem.Columns.Add("rework_qty", typeof(float)); 
            dtItem.Columns.Add("it_remarks", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                decimal Disp_qty, Pend_qty, Bill_qty, Recd_qty, Accept_qty, Reject_qty, Rework_qty;
                if (jObject[i]["DispQty"].ToString() == "")
                    Disp_qty = 0;
                else
                    Disp_qty = Convert.ToDecimal(jObject[i]["DispQty"].ToString());

                if (jObject[i]["PendQty"].ToString() == "")
                    Pend_qty = 0;
                else
                    Pend_qty = Convert.ToDecimal(jObject[i]["PendQty"].ToString());

                if (jObject[i]["BillQty"].ToString() == "")
                    Bill_qty = 0;
                else
                    Bill_qty = Convert.ToDecimal(jObject[i]["BillQty"].ToString());

                if (jObject[i]["RecievQty"].ToString() == "")
                    Recd_qty = 0;
                else
                    Recd_qty = Convert.ToDecimal(jObject[i]["RecievQty"].ToString());

                if (jObject[i]["AccptQty"].ToString() == "")
                    Accept_qty = 0;
                else
                    Accept_qty = Convert.ToDecimal(jObject[i]["AccptQty"].ToString());

                if (jObject[i]["RejQty"].ToString() == "")
                    Reject_qty = 0;
                else
                    Reject_qty = Convert.ToDecimal(jObject[i]["RejQty"].ToString());

                if (jObject[i]["RewrkQty"].ToString() == "")
                    Rework_qty = 0;
                else
                    Rework_qty = Convert.ToDecimal(jObject[i]["RewrkQty"].ToString());

                DataRow dtrowLines = dtItem.NewRow();


                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["UomName"] = jObject[i]["UOMName"].ToString();
                dtrowLines["disp_qty"] = Disp_qty;
                dtrowLines["pend_qty"] = Pend_qty;
                dtrowLines["bill_qty"] = Bill_qty;
                dtrowLines["recd_qty"] = Recd_qty;
                dtrowLines["qc_check"] = jObject[i]["QCRequired"].ToString();
                dtrowLines["i_qc"] = jObject[i]["QCRequired"].ToString();
                dtrowLines["accept_qty"] = Accept_qty;
                dtrowLines["reject_qty"] = Reject_qty;
                dtrowLines["rework_qty"] = Rework_qty;
                dtrowLines["it_remarks"] = jObject[i]["Remarks"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtReturnItemdetail(JArray jObject)
        {
            DataTable dtItemreturn = new DataTable();

            dtItemreturn.Columns.Add("item_id", typeof(string));
            dtItemreturn.Columns.Add("item_name", typeof(string));
            dtItemreturn.Columns.Add("sub_item", typeof(string));
            dtItemreturn.Columns.Add("uom_id", typeof(string));
            dtItemreturn.Columns.Add("UomName", typeof(string)); 
            dtItemreturn.Columns.Add("ItemTypeId", typeof(string));
            dtItemreturn.Columns.Add("ItemType", typeof(string));
            dtItemreturn.Columns.Add("ReceiveQty", typeof(float));

        for (int i = 0; i < jObject.Count; i++)
            {


                DataRow dtrowLines = dtItemreturn.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["UomName"] = jObject[i]["UOMName"].ToString();
                dtrowLines["ItemTypeId"] = jObject[i]["ItmTypId"].ToString();
                dtrowLines["ItemType"] = jObject[i]["ItmTypName"].ToString();
                dtrowLines["ReceiveQty"] = jObject[i]["ReceiveQty"].ToString();


                dtItemreturn.Rows.Add(dtrowLines);
            }
            return dtItemreturn;
        }
        public DataTable dtMDOrderdetail(JArray jObjectOrderQty)
        {
            DataTable dtDispQtyItem = new DataTable();

            dtDispQtyItem.Columns.Add("item_id", typeof(string));
            dtDispQtyItem.Columns.Add("sub_item", typeof(string));
            dtDispQtyItem.Columns.Add("uom_id", typeof(string));
            dtDispQtyItem.Columns.Add("disp_no", typeof(string));
            dtDispQtyItem.Columns.Add("disp_dt", typeof(string));

            dtDispQtyItem.Columns.Add("disp_qty", typeof(string));
            dtDispQtyItem.Columns.Add("pend_qty", typeof(string));
            dtDispQtyItem.Columns.Add("bill_qty", typeof(string));
            dtDispQtyItem.Columns.Add("rec_qty", typeof(string));

        for (int i = 0; i < jObjectOrderQty.Count; i++)
            {
                DataRow dtrowOrderDetails = dtDispQtyItem.NewRow();
                dtrowOrderDetails["item_id"] = jObjectOrderQty[i]["ItemID"].ToString();
                dtrowOrderDetails["sub_item"] = jObjectOrderQty[i]["sub_item"].ToString();
                dtrowOrderDetails["uom_id"] = jObjectOrderQty[i]["UomID"].ToString();
                dtrowOrderDetails["disp_no"] = jObjectOrderQty[i]["DocNo"].ToString();
                dtrowOrderDetails["disp_dt"] = jObjectOrderQty[i]["DocDate"].ToString();

                dtrowOrderDetails["disp_qty"] = jObjectOrderQty[i]["DispatchQty"].ToString();
                dtrowOrderDetails["pend_qty"] = jObjectOrderQty[i]["PendingQty"].ToString();
                dtrowOrderDetails["bill_qty"] = jObjectOrderQty[i]["BilledQty"].ToString();
                dtrowOrderDetails["rec_qty"] = jObjectOrderQty[i]["ReceiveQty"].ToString();
                dtDispQtyItem.Rows.Add(dtrowOrderDetails);
            }
            return dtDispQtyItem;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("Disp_qty", typeof(string));
            dtSubItem.Columns.Add("Pending_Qty", typeof(string));
            dtSubItem.Columns.Add("BilledQty", typeof(string));
            dtSubItem.Columns.Add("RecievedQty", typeof(string));
            dtSubItem.Columns.Add("Accept_Qty", typeof(string));
            dtSubItem.Columns.Add("Reject_Qty", typeof(string));
            dtSubItem.Columns.Add("Rework_Qty", typeof(string));
            dtSubItem.Columns.Add("src_doc_number", typeof(string));
            dtSubItem.Columns.Add("src_doc_date", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["Disp_Qty"] = jObject2[i]["DispatchQty"].ToString();
                dtrowItemdetails["Pending_Qty"] = jObject2[i]["PendingQuantity"].ToString();
                dtrowItemdetails["BilledQty"] = jObject2[i]["BilledQty"].ToString();
                dtrowItemdetails["RecievedQty"] = jObject2[i]["ReceivedQuantity"].ToString();
                dtrowItemdetails["Accept_Qty"] = jObject2[i]["AcceptedQty"].ToString();
                dtrowItemdetails["Reject_Qty"] = jObject2[i]["RejectedQty"].ToString();
                dtrowItemdetails["Rework_Qty"] = jObject2[i]["ReworkableQty"].ToString();
                dtrowItemdetails["src_doc_number"] = jObject2[i]["DispSrcDocNo"].ToString();
                dtrowItemdetails["src_doc_date"] = jObject2[i]["DispSrcDocDt"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public DataTable dtscrapsubitemdetail(JArray jObject3)
        {
            DataTable dtByPrdctScrapSubItem = new DataTable();
            dtByPrdctScrapSubItem.Columns.Add("item_id", typeof(string));
            dtByPrdctScrapSubItem.Columns.Add("sub_item_id", typeof(string));
            dtByPrdctScrapSubItem.Columns.Add("Qty", typeof(string));

            for (int i = 0; i < jObject3.Count; i++)
            {
                DataRow dtrowPrdtscrapItemdetails = dtByPrdctScrapSubItem.NewRow();
                dtrowPrdtscrapItemdetails["item_id"] = jObject3[i]["item_id"].ToString();
                dtrowPrdtscrapItemdetails["sub_item_id"] = jObject3[i]["sub_item_id"].ToString();
                dtrowPrdtscrapItemdetails["Qty"] = jObject3[i]["Qty"].ToString();

                dtByPrdctScrapSubItem.Rows.Add(dtrowPrdtscrapItemdetails);
            }
            return dtByPrdctScrapSubItem;
        }
        //private DataTable ToDtblDispatchQtyItemDetail(string ItemDispatchQtyDetail)
        //{
        //    try
        //    {
        //        DataTable DispatchQtyItemDetails = new DataTable();
        //        DataTable dtDispQtyItem = new DataTable();

        //        dtDispQtyItem.Columns.Add("ItemID", typeof(string));
        //        dtDispQtyItem.Columns.Add("Disp_No", typeof(string));
        //        dtDispQtyItem.Columns.Add("Disp_Dt", typeof(string));
        //        dtDispQtyItem.Columns.Add("UomID", typeof(string));
        //        dtDispQtyItem.Columns.Add("DispQty", typeof(string));
        //        //dtDispQtyItem.Columns.Add("bal_qty", typeof(string));
        //        //dtDispQtyItem.Columns.Add("pack_qty", typeof(string));

        //        if (_DeliveryNoteDetailSC_Model.ItemDispatchQtyDetail != null)
        //        {
        //            JArray jObjectOrderQty = JArray.Parse(_DeliveryNoteDetailSC_Model.ItemDispatchQtyDetail);
        //            for (int i = 0; i < jObjectOrderQty.Count; i++)
        //            {
        //                DataRow dtrowOrderDetails = dtDispQtyItem.NewRow();
        //                dtrowOrderDetails["item_id"] = jObjectOrderQty[i]["itemid"].ToString();
        //                dtrowOrderDetails["so_no"] = jObjectOrderQty[i]["docno"].ToString();
        //                dtrowOrderDetails["so_dt"] = jObjectOrderQty[i]["docdate"].ToString();
        //                dtrowOrderDetails["uom_id"] = jObjectOrderQty[i]["uomid"].ToString();
        //                dtrowOrderDetails["ord_qty"] = jObjectOrderQty[i]["orderqty"].ToString();
        //                dtrowOrderDetails["bal_qty"] = jObjectOrderQty[i]["pendingqty"].ToString();
        //                dtrowOrderDetails["pack_qty"] = jObjectOrderQty[i]["packedqty"].ToString();
        //                dtDispQtyItem.Rows.Add(dtrowOrderDetails);
        //            }
        //        }
        //        DispatchQtyItemDetails = dtDispQtyItem;
        //        return DispatchQtyItemDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        throw ex;
        //    }


        //}


        public ActionResult DoubleClickFromList(string DocNo, string DocDate, DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model,string ListFilterData,string WF_Status)
        {
            try
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
                _DeliveryNoteDetailSC_Model.Message = "New";
                _DeliveryNoteDetailSC_Model.Command = "Update";
                _DeliveryNoteDetailSC_Model.TransType = "Update";
                _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
                _DeliveryNoteDetailSC_Model.DN_No = DocNo;
                _DeliveryNoteDetailSC_Model.DN_Dt = DocDate;
                if (WF_Status != null && WF_Status != "")
                {
                    _DeliveryNoteDetailSC_Model.WF_Status1 = WF_Status;
                }
                var WF_Status1 = _DeliveryNoteDetailSC_Model.WF_Status1;
                var DNCodeURL = DocNo;
                var DNDate = DocDate;
                var TransType = "Update";
                var BtnName = "BtnToDetailPage";
                var command = "Add";

                TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                TempData["ListFilterData"] = ListFilterData;

                return (RedirectToAction("DeliveryNoteSCDetail", "DeliveryNoteSC", new { DNCodeURL = DNCodeURL, DNDate, TransType, BtnName, command, WF_Status1 }));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }



        }
        public ActionResult DeleteDNDetails(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model, string command)
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
                string DNNo = _DeliveryNoteDetailSC_Model.DN_No;
                string DNDelete = _DeliveryNoteDetailSC_IServices.DN_DeleteDetail(_DeliveryNoteDetailSC_Model, CompID, BrchID);

                if (!string.IsNullOrEmpty(DNNo))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string DNNo1 = DNNo.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, DNNo1, Server);
                }
                _DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();
                _DeliveryNoteDetailSC_Model.Message = "Deleted";
                _DeliveryNoteDetailSC_Model.Command = "Refresh";
                _DeliveryNoteDetailSC_Model.TransType = "Refresh";
                _DeliveryNoteDetailSC_Model.BtnName = "BtnDelete";
                TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                return RedirectToAction("DeliveryNoteSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private List<DNSCList> getDNSCList(DNListModel _DNListModel)
        {
            _DNSCList = new List<DNSCList>();
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
                if(_DNListModel.WF_Status != null)
                {
                    wfstatus = _DNListModel.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }

                DataSet DSet = _DeliveryNoteDetailSC_IServices.GetDNSCListandSrchDetail(CompID, BrchID, _DNListModel, UserID, wfstatus, DocumentMenuId);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        DNSCList _DNList = new DNSCList();
                        _DNList.DNNumber = dr["DNNo"].ToString();
                        _DNList.DNDate = dr["DNDate"].ToString();
                        _DNList.DN_Dt = dr["DNDT"].ToString();
                        _DNList.SuppName = dr["supp_name"].ToString();
                        _DNList.BillNo = dr["bill_no"].ToString();
                        _DNList.BillDate = dr["BillDt"].ToString();
                        _DNList.JobOrdNo = dr["scjob_no"].ToString();
                        _DNList.JobOrdDt = dr["scjob_dt"].ToString();
                        _DNList.DN_Status = dr["DNStatus"].ToString();
                        _DNList.CreatedON = dr["CreateDate"].ToString();
                        _DNList.ApprovedOn = dr["ApproveDate"].ToString();
                        _DNList.FinStDt = DSet.Tables[2].Rows[0]["findate"].ToString();

                        _DNSCList.Add(_DNList);
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
            return _DNSCList;
        }

        [HttpPost]
        public ActionResult SearchDNSCListDetail(string SuppId, string Fromdate, string Todate, string Status)
        {
            DNListModel _DNListModel = new DNListModel();
            try
            {
                //Session.Remove("WF_Docid");
                //Session.Remove("WF_status");
                _DNListModel.WF_Status = null;
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
                _DNSCList = new List<DNSCList>();
                _DNListModel.SuppID = SuppId;

                _DNListModel.FromDate = Fromdate;
                _DNListModel.ToDate = Todate;
                _DNListModel.Status = Status;
                DataSet DSet = _DeliveryNoteDetailSC_IServices.GetDNSCListandSrchDetail(CompID, BrchID, _DNListModel, "", "", "");
                //Session["DNSCSearch"] = "DNSC_Search";
                _DNListModel.DNSCSearch = "DNSC_Search";
                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    DNSCList _DNList = new DNSCList();
                    _DNList.DNNumber = dr["DNNo"].ToString();
                    _DNList.DNDate = dr["DNDate"].ToString();
                    _DNList.DN_Dt = dr["DNDT"].ToString();
                    _DNList.SuppName = dr["supp_name"].ToString();
                    _DNList.BillNo = dr["bill_no"].ToString();
                    _DNList.BillDate = dr["BillDt"].ToString();
                    _DNList.JobOrdNo = dr["scjob_no"].ToString();
                    _DNList.JobOrdDt = dr["scjob_dt"].ToString();
                    _DNList.DN_Status = dr["DNStatus"].ToString();
                    _DNList.CreatedON = dr["CreateDate"].ToString();
                    _DNList.ApprovedOn = dr["ApproveDate"].ToString();
                    _DNSCList.Add(_DNList);
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                _DNListModel.DeliveryNoteSCList = _DNSCList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDeliveryNoteSCList.cshtml", _DNListModel);
        }
        public ActionResult DNApprove(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model, string ListFilterData1)
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
                string DN_No = _DeliveryNoteDetailSC_Model.DN_No;
                string DN_Date = _DeliveryNoteDetailSC_Model.DN_Dt;
                string A_Status = _DeliveryNoteDetailSC_Model.A_Status;
                string A_Level = _DeliveryNoteDetailSC_Model.A_Level;
                string A_Remarks = _DeliveryNoteDetailSC_Model.A_Remarks;

                string Message = _DeliveryNoteDetailSC_IServices.DNApproveDetails(CompID, BrchID, DN_No, DN_Date, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks);
                string ApMessage = Message.Split(',')[2].Trim();
                string MDSC_No = Message.Split(',')[0].Trim();

                if (ApMessage == "A" )
                {
                    _DeliveryNoteDetailSC_Model.Message = "Approved";
                }
                if (ApMessage == "QP")
                {
                    _DeliveryNoteDetailSC_Model.Message = "Approved";
                }
                _DeliveryNoteDetailSC_Model.TransType = "Update";
                _DeliveryNoteDetailSC_Model.Command = "Approve";
                _DeliveryNoteDetailSC_Model.AppStatus = "D";
                _DeliveryNoteDetailSC_Model.BtnName = "BtnEdit";

                var DNCodeURL = MDSC_No;
                var DNDate = _DeliveryNoteDetailSC_Model.DN_Dt;
                var TransType = _DeliveryNoteDetailSC_Model.TransType;
                var BtnName = _DeliveryNoteDetailSC_Model.BtnName;
                var command = _DeliveryNoteDetailSC_Model.Command;
                TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("DeliveryNoteSCDetail", new { DNCodeURL = DNCodeURL, DNDate, TransType, BtnName, command }));


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList,string ListFilterData1,string WF_Status)
        {
            try
            {
                DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();

                if (AppDtList != null)
                {
                    JArray jObjectBatch = JArray.Parse(AppDtList);
                    for (int i = 0; i < jObjectBatch.Count; i++)
                    {
                        _DeliveryNoteDetailSC_Model.DN_No = jObjectBatch[i]["DNNo"].ToString();
                        _DeliveryNoteDetailSC_Model.DN_Dt = jObjectBatch[i]["DNDate"].ToString();

                        _DeliveryNoteDetailSC_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                        _DeliveryNoteDetailSC_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                        _DeliveryNoteDetailSC_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                    }
                }
                if (_DeliveryNoteDetailSC_Model.A_Status != "Approve")
                {
                    _DeliveryNoteDetailSC_Model.A_Status = "Approve";
                }
                DNApprove(_DeliveryNoteDetailSC_Model, ListFilterData1);
                if (WF_Status != null && WF_Status != "")
                {
                    _DeliveryNoteDetailSC_Model.WF_Status1 = WF_Status;
                }
                var WF_Status1 = _DeliveryNoteDetailSC_Model.WF_Status1;
                var DNCodeURL = _DeliveryNoteDetailSC_Model.DN_No;
                var DNDate = _DeliveryNoteDetailSC_Model.DN_Dt;
                var TransType = _DeliveryNoteDetailSC_Model.TransType;
                var BtnName = _DeliveryNoteDetailSC_Model.BtnName;
                var command = _DeliveryNoteDetailSC_Model.Command;
                return (RedirectToAction("DeliveryNoteSCDetail", new { DNCodeURL = DNCodeURL, DNDate, TransType, BtnName, command, WF_Status1 }));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
           
        }
        public ActionResult ToRefreshByJS(string FrwdDtList,string ListFilterData1, string WF_Status)
        {
            DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();
           
            if (FrwdDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(FrwdDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _DeliveryNoteDetailSC_Model.DN_No = jObjectBatch[i]["DNNo"].ToString();
                    _DeliveryNoteDetailSC_Model.DN_Dt = jObjectBatch[i]["DNDate"].ToString();
                    _DeliveryNoteDetailSC_Model.TransType = "Update";
                    _DeliveryNoteDetailSC_Model.BtnName = "BtnToDetailPage";
                    if(WF_Status != null && WF_Status != "")
                    {
                        _DeliveryNoteDetailSC_Model.WF_Status1 = WF_Status;                   
                    }
                    TempData["ModelData"] = _DeliveryNoteDetailSC_Model;
                }
            }
            var WF_Status1 = _DeliveryNoteDetailSC_Model.WF_Status1;
            var DNCodeURL = _DeliveryNoteDetailSC_Model.DN_No;
            var DNDate = _DeliveryNoteDetailSC_Model.DN_Dt;
            var TransType = _DeliveryNoteDetailSC_Model.TransType;
            var BtnName = _DeliveryNoteDetailSC_Model.BtnName;
            var command = "Refresh";
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("DeliveryNoteSCDetail", new { DNCodeURL = DNCodeURL, DNDate, TransType, BtnName, command, WF_Status1 }));

        }
        public ActionResult GetDNDashbordList(string docid, string status)
        {

            // Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("DeliveryNoteSC",new { WF_Status });
        }
        private string CheckGRNSCAgainstDeliveryNoteSC(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model)
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

                str = _DeliveryNoteDetailSC_IServices.ChkGRNSCDagainstDNSC(CompID, BrchID, _DeliveryNoteDetailSC_Model.DN_No, _DeliveryNoteDetailSC_Model.DN_Dt).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);              
                throw ex;
            }

        }

        public string CheckQCAgainstDNSC(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model)
        {
            string str = "";
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
                string DocNo = _DeliveryNoteDetailSC_Model.DN_No;
                string DocDate= _DeliveryNoteDetailSC_Model.DN_Dt;
                DataSet Deatils = _DeliveryNoteDetailSC_IServices.CheckQCAgainstDNSC(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return str;
        }

        public ActionResult getDetailBySourceDocumentMDNo(string SourDocumentNo, string SourDocumentDate,string DNNo)
        {
            try
            {
                JsonResult DataRows = null;
                _DeliveryNoteDetailSC_Model = new DeliveryNoteDetailSC_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null) 
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _DeliveryNoteDetailSC_IServices.getDetailBySourceDocumentMDNo(CompID, BrchID, SourDocumentNo, SourDocumentDate, DNNo);

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
        public ActionResult GetAutoCompleteSearchSuppList(DNListModel _DNListModel)
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
                if (string.IsNullOrEmpty(_DNListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _DNListModel.SuppName;
                }
                CustList = _DeliveryNoteDetailSC_IServices.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _DNListModel.SupplierNameList = _SuppList;
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
        public JsonResult GetJobORDDocNOList(string Supp_id)
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
                DataSet Deatils = _DeliveryNoteDetailSC_IServices.GetJobORDDocNOList(Supp_id,Comp_ID, Br_ID);

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
        public JsonResult GetMDDocNOList(string JONO)
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
                DataSet Deatils = _DeliveryNoteDetailSC_IServices.GetMDDocNOList(Comp_ID, Br_ID, JONO);

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

        public ActionResult GetReturnItemDDLList(BindRtrnItemList bindItem, string PageName, string JONO)
        {

            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                DataSet RetrnItmList = _DeliveryNoteDetailSC_IServices.GetReturnItemDDLList(Comp_ID, Br_ID, bindItem.SearchName, PageName, JONO);
                if (RetrnItmList.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    for (int i = 0; i < RetrnItmList.Tables[0].Rows.Count; i++)
                    {
                        string itemId = RetrnItmList.Tables[0].Rows[i]["Item_id"].ToString();
                        string itemName = RetrnItmList.Tables[0].Rows[i]["Item_name"].ToString();
                        string UomId = RetrnItmList.Tables[0].Rows[i]["uom_id"].ToString();
                        string Uom = RetrnItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        string ItmTyp = RetrnItmList.Tables[0].Rows[i]["ItemType"].ToString();
                        string ItmTypNm = RetrnItmList.Tables[0].Rows[i]["ItemTypeName"].ToString();
                        //string IssueQty = RetrnItmList.Tables[0].Rows[i]["issueQty"].ToString();
                        //ItemList.Add(itemId+ ","  + Uom + "," + UomId + "," + ItmTypId+","+ ItmTyp+","+ IssueQty, itemName);
                        ItemList.Add(itemId + "," + Uom + "," + UomId + "," + ItmTyp + "," + ItmTypNm, itemName);
                    }
                }
                //DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindByProdctScrapItm_AgainstDircetJO(BindRtrnItemList bindItem, string PageName/*, string JONO*/)
        {

            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                DataSet RetrnItmList = _DeliveryNoteDetailSC_IServices.BindByProdctScrapItm_AgainstDircetJO(Comp_ID, Br_ID, bindItem.SearchName, PageName/*, JONO*/);
                if (RetrnItmList.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    for (int i = 0; i < RetrnItmList.Tables[0].Rows.Count; i++)
                    {
                        string itemId = RetrnItmList.Tables[0].Rows[i]["Item_id"].ToString();
                        string itemName = RetrnItmList.Tables[0].Rows[i]["Item_name"].ToString();
                        string Uom = RetrnItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        ItemList.Add(itemId + "_" + Uom, itemName);
                    }
                }
                //DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getItemDispatchQuantityDetail(string DnNo,string DN_Command, string ItemID, string Status, string SelectedItemdetail,string HdnMessage)
        {
            try
            {
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.QtyDigit = QtyDigit;
                //_DeliveryNoteDetailSC_Model.QtyDigit = QtyDigit;
                // _DeliveryNoteDetailSC_Model.TransType = _DeliveryNoteDetailSC_Model.TransType;
                // _DeliveryNoteDetailSC_Model.Command = _DeliveryNoteDetailSC_Model.Command;
                // _DeliveryNoteDetailSC_Model.DocumentStatus = _DeliveryNoteDetailSC_Model.DocumentStatus;
                if (DnNo == "")
                {
                   ViewBag.TransType = "Save";
                    ViewBag.Command = "Add";
                    ViewBag.DocumentStatus = "D";
                }
                else
                {  if(Status=="D"&& DN_Command=="Edit")
                    {
                        ViewBag.TransType = "Update";
                        ViewBag.Command = "Edit";
                        ViewBag.DocumentStatus = "D";
                    }
                    else
                    {
                        ViewBag.TransType = "";
                        ViewBag.Command = "";
                        ViewBag.DocumentStatus = "";
                    }
                   
                }

                DataTable DTableOrderQty = new DataTable();
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    DataTable dtorderqty = new DataTable();
                    dtorderqty.Columns.Add("disp_no", typeof(string));
                    dtorderqty.Columns.Add("disp_dt", typeof(string));
                    dtorderqty.Columns.Add("item_id", typeof(string));
                    dtorderqty.Columns.Add("sub_item", typeof(string));
                    dtorderqty.Columns.Add("uom_id", typeof(string));
                    dtorderqty.Columns.Add("disp_qty", typeof(string));
                    dtorderqty.Columns.Add("pend_qty", typeof(string));
                    dtorderqty.Columns.Add("bill_qty", typeof(string));
                    dtorderqty.Columns.Add("rec_qty", typeof(string));

                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);

                    foreach (JObject item in jObjectBatch.Children())
                    {
                        if (item.GetValue("ItemID").ToString() == ItemID.ToString())
                        {
                            DataRow dtorderqtyrow = dtorderqty.NewRow();
                            dtorderqtyrow["disp_no"] = item.GetValue("DocNo").ToString();
                            dtorderqtyrow["disp_dt"] = item.GetValue("DocDate").ToString();
                            dtorderqtyrow["item_id"] = item.GetValue("ItemID").ToString();
                            dtorderqtyrow["sub_item"] = item.GetValue("sub_item").ToString();
                            dtorderqtyrow["uom_id"] = item.GetValue("UomID").ToString();
                            dtorderqtyrow["disp_qty"] = item.GetValue("DispatchQty").ToString();
                            dtorderqtyrow["pend_qty"] = item.GetValue("PendingQty").ToString();
                            dtorderqtyrow["bill_qty"] = item.GetValue("BilledQty").ToString();
                            dtorderqtyrow["rec_qty"] = item.GetValue("ReceiveQty").ToString();

                            dtorderqty.Rows.Add(dtorderqtyrow);
                        }
                    }
                    DTableOrderQty = dtorderqty;
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.Message = HdnMessage;
                if (DTableOrderQty.Rows.Count > 0)
                    ViewBag.ItemDispatchQtyDetail = DTableOrderQty;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_DispatchDetail.cshtml");
            
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /*-------------Start Sub Item Detail---------------------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
  , string Flag, string Status, string Doc_no, string Doc_dt, string JobOrdNo, string MDNo, string MDDate,string JobOrdTyp)
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

                if (Status == "D" || Status == "F" || Status == "" || Status == "0")
                {
                    if(JobOrdTyp=="D" && Flag == "BYPSRecvQty")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    }
                    else
                    {
                        dt = _DeliveryNoteDetailSC_IServices.GetSubItemDetailsFromMD(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, JobOrdNo, MDNo, MDDate).Tables[0];

                    }
                    if (Flag == "BYPSRecvQty")
                    {
                        ViewBag.DocId = "105108110";
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        DataTable NewDt = new DataTable();

                        int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("Item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("Qty").ToString(), DecDgt);
                                }
                            }

                        }
                        
                    }
                    
                    else
                    {
                        ViewBag.DocId = "105108110";
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        DataTable NewDt = new DataTable();

                        int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("Item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString()
                                    && item.GetValue("DispSrcDocNo").ToString() == dt.Rows[i]["dispatch_no"].ToString())
                                {   
                                    if(item.GetValue("BilledQty").ToString()=="")
                                    {
                                        dt.Rows[i]["BilledQty"] = item.GetValue("BilledQty").ToString();
                                    }
                                    else
                                    {
                                        dt.Rows[i]["BilledQty"] = cmn.ConvertToDecimal(item.GetValue("BilledQty").ToString(), DecDgt);
                                    }
                                    if (item.GetValue("ReceivedQuantity").ToString() == "")
                                    {
                                        dt.Rows[i]["RecievedQty"] = item.GetValue("ReceivedQuantity").ToString();
                                    }
                                    else
                                    {
                                        dt.Rows[i]["RecievedQty"] = cmn.ConvertToDecimal(item.GetValue("ReceivedQuantity").ToString(), DecDgt);
                                    }
                                    dt.Rows[i]["Disp_Qty"] = cmn.ConvertToDecimal(item.GetValue("DispatchQty").ToString(), DecDgt);
                                    dt.Rows[i]["Pending_Qty"] = cmn.ConvertToDecimal(item.GetValue("PendingQuantity").ToString(), DecDgt);
                                    dt.Rows[i]["Accept_Qty"] = cmn.ConvertToDecimal(item.GetValue("AcceptedQty").ToString(), DecDgt);
                                    dt.Rows[i]["Reject_Qty"] = cmn.ConvertToDecimal(item.GetValue("RejectedQty").ToString(), DecDgt);
                                    dt.Rows[i]["Rework_Qty"] = cmn.ConvertToDecimal(item.GetValue("ReworkableQty").ToString(), DecDgt);
                                }
                            }

                        }

                    }
                    

                }

                else
                { if (Flag == "DNSCAcceptQty" || Flag == "DNSCRejctQty" || Flag == "DNSCRewrkQty")
                    {
                        dt = _DeliveryNoteDetailSC_IServices.GetSubItemDetailsFromMD(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, JobOrdNo, MDNo, MDDate).Tables[0];
                        Flag = "DNSC_QCAcptRejRwkQty";
                    }
                    else
                    {
                        dt = _DeliveryNoteDetailSC_IServices.DNSC_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, MDNo, Doc_no, Doc_dt, Flag).Tables[0];
                    }

                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,


                    dt_SubItemDetails = dt,
                    _subitemPageName = "DN",
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

        /*--------------------------End Sub Item Detail-------------------------*/

        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                DNDetailsattch _DNDetail = new DNDetailsattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Guid gid = new Guid();
                gid = Guid.NewGuid();


                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _DNDetail.Guid = DocNo;
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
                    _DNDetail.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _DNDetail.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _DNDetail;
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
        /*--------------------------For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(DeliveryNoteDetailSC_Model _model)
        {
            return File(GetPdfData(_model.DN_No, _model.DN_Dt), "application/pdf", "DeliveryNote.pdf");
        }
        public byte[] GetPdfData(string dnNo, string dnDate)
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
                DataSet Details = _DeliveryNoteDetailSC_IServices.GetDeliveryNoteSCDeatilsForPrint(CompID, BrchID, dnNo, Convert.ToDateTime(dnDate).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.Title = "Delivery Note";/*comment and change name of title by Hina shrama on 16-10-2024*/
                ViewBag.Title = "Gate Entry";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["dn_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 07-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SubContracting/DeliveryNote/DeliveryNoteSCPrint.cshtml"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
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

        /*--------------------------For PDF Print End--------------------------*/
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
            try
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
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