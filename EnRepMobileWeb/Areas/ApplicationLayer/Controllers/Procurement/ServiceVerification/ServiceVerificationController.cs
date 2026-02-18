using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServiceVerification;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServiceVerification;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.ServiceVerification
{
    public class ServiceVerificationController : Controller
    {
        List<ServiceVerificationList> _ServiceVerificationList;
        string CompID, language, title, UserID, create_id, BrchID = string.Empty;
        string DocumentMenuId = "105101137";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ServiceVerification_ISERVICE _SrVer_ISERVICE;
        public ServiceVerificationController(Common_IServices _Common_IServices, ServiceVerification_ISERVICE _SrVer_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._SrVer_ISERVICE = _SrVer_ISERVICE;
        }
        // GET: ApplicationLayer/ServiceVerification
        public ActionResult ServiceVerification(ServiceVerificationListModel _SrVerListModel)
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
                //ServiceVerificationListModel _SrVerListModel = new ServiceVerificationListModel();              
                //if (Session["WF_Docid"] != null)
                //{
                //    _SrVerListModel.wfdocid = Session["WF_Docid"].ToString();
                //}
                //else
                //{
                //    _SrVerListModel.wfdocid = "0";
                //}
                //if (Session["WF_status"] != null)
                //{
                //    _SrVerListModel.wfstatus = Session["WF_status"].ToString();
                //}
                //else
                //{
                //    _SrVerListModel.wfstatus = "";
                // } 
               //string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _SrVerListModel.WF_status = TempData["WF_status"].ToString();                   
                    if (_SrVerListModel.WF_status != null)
                    {
                        _SrVerListModel.wfstatus = _SrVerListModel.WF_status;
                    }
                    else
                    {
                        _SrVerListModel.wfstatus = "";
                    }
                }
                else
                {
             
                    if (_SrVerListModel.WF_status != null)
                    {
                        _SrVerListModel.wfstatus = _SrVerListModel.WF_status;
                    }
                    else
                    {
                        _SrVerListModel.wfstatus = "";
                    }
                }
                if( DocumentMenuId !=null)
                {
                    _SrVerListModel.wfdocid = DocumentMenuId;
                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string todate = range.ToDate;

                //GetAutoCompleteSearchSuppList(_SrVerListModel);


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
                _SrVerListModel.StatusList = statusLists;
                //_SrVerModel.SuppID, _SrVerModel.Ver_FromDate, _SrVerModel.Ver_ToDate, _SrVerModel.Status,
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != null && PRData != "")
                    {
                        var a = PRData.Split(',');
                        _SrVerListModel.SuppID = a[0].Trim();
                        _SrVerListModel.Ver_FromDate = a[1].Trim();
                        _SrVerListModel.Ver_ToDate = a[2].Trim();
                        _SrVerListModel.Status = a[3].Trim();
                        if (_SrVerListModel.Status == "0")
                        {
                            _SrVerListModel.Status = null;

                        }
                        _SrVerListModel.ListFilterData = TempData["ListFilterData"].ToString();
                       // _SrVerListModel.VerificationList = GetVerificationList(_SrVerListModel);
                        _SrVerListModel.FromDate = _SrVerListModel.Ver_FromDate;
                    }
                }
                else
                {
                    //_SrVerListModel.VerificationList = GetVerificationList(_SrVerListModel);
                    _SrVerListModel.FromDate = startDate;
                    _SrVerListModel.Ver_FromDate = startDate;
                    _SrVerListModel.Ver_ToDate = todate;
                }
                GetAllData(_SrVerListModel);
                _SrVerListModel.Title = title;
                _SrVerListModel.VerSearch = "0";
              
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/Procurement/ServiceVerification/ServiceVerificationList.cshtml", _SrVerListModel);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");

            }

        }
        private void GetAllData(ServiceVerificationListModel _SrVerListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SuppType = string.Empty;
            string User_ID = string.Empty;
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
                    User_ID = Session["UserId"].ToString();
                }
                if (string.IsNullOrEmpty(_SrVerListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _SrVerListModel.SuppName;
                }
                DataSet SuppList1 = _SrVer_ISERVICE.GetAlldata(Comp_ID, SupplierName, Br_ID, 
                    User_ID, _SrVerListModel.SuppID, _SrVerListModel.Ver_FromDate, _SrVerListModel.Ver_ToDate, _SrVerListModel.Status, _SrVerListModel.wfdocid, _SrVerListModel.wfstatus);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (DataRow data in SuppList1.Tables[0].Rows)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data["supp_id"].ToString();
                    _SuppDetail.supp_name = data["supp_name"].ToString();
                    _SuppList.Add(_SuppDetail);
                }
                _SuppList.Insert(0, new SupplierName() { supp_id = "0", supp_name = "All" });
                _SrVerListModel.SupplierNameList = _SuppList;
                SetAlldataInListTabelData(SuppList1, _SrVerListModel);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetAlldataInListTabelData(DataSet DSet, ServiceVerificationListModel _SrVerListModel)
        {
            List<ServiceVerificationList> _ServiceVerificationList = new List<ServiceVerificationList>();
            if (DSet.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in DSet.Tables[1].Rows)
                {
                    ServiceVerificationList _VerficationList = new ServiceVerificationList();
                    _VerficationList.VerNo = dr["sr_ver_no"].ToString();
                    _VerficationList.VerDate = dr["sr_ver_dt"].ToString();
                    _VerficationList.VerDt = dr["VerDt"].ToString();
                    _VerficationList.SourceDocNo = dr["src_doc_number"].ToString();
                    _VerficationList.SourceDocDt = dr["src_doc_date"].ToString();
                    _VerficationList.SuppName = dr["supp_name"].ToString();
                    _VerficationList.VerStauts = dr["VerStauts"].ToString();
                    _VerficationList.CreateDate = dr["CreateDate"].ToString();
                    _VerficationList.ApproveDate = dr["ApproveDate"].ToString();
                    _VerficationList.ModifyDate = dr["ModifyDate"].ToString();
                    _VerficationList.create_by = dr["create_by"].ToString();
                    _VerficationList.app_by = dr["app_by"].ToString();
                    _VerficationList.mod_by = dr["mod_by"].ToString();
                    _ServiceVerificationList.Add(_VerficationList);
                }
            }
            _SrVerListModel.VerificationList = _ServiceVerificationList;
            _SrVerListModel.FinStDt = Convert.ToDateTime(DSet.Tables[3].Rows[0]["findate"]);
        }
        public ActionResult DoubleClickOnList(string DocNo, string DocDate,string ListFilterData, string WF_status)
        {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message1"] = "Financial Year not Exist";
            }
            /*End to chk Financial year exist or not*/
            ServiceVerificationModel _SrVerModel = new ServiceVerificationModel();
            _SrVerModel.Message = "New";
            _SrVerModel.Command = "Update";
            _SrVerModel.TransType = "Update";
            _SrVerModel.BtnName = "BtnToDetailPage";
            _SrVerModel.SrVerNo = DocNo;
            _SrVerModel.SrVerDate = DocDate;
            _SrVerModel.WF_status1 = WF_status;
            var varNO = DocNo;
            var verDate = DocDate;
            TempData["ModelData"] = _SrVerModel;
            var transType = "Update";
            var btnName = "BtnToDetailPage";
            var command = "Add";
            TempData["ListFilterData"] = ListFilterData;

            return (RedirectToAction("ServiceVerificationDetail",new { varNO= varNO , verDate, transType, btnName , command }));
        }
        public ActionResult AddServiceVerificationDetail()
        {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                ServiceVerificationListModel pqModel = new ServiceVerificationListModel();
                //pqModel.Message = "Financial Year not Exist";
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ServiceVerification", pqModel);
            }
            /*End to chk Financial year exist or not*/
            ServiceVerificationModel _SrVerModel = new ServiceVerificationModel();
            _SrVerModel.Message = "New";
            _SrVerModel.Command = "Add";
            _SrVerModel.AppStatus = "D";
            _SrVerModel.TransType = "Save";
            _SrVerModel.BtnName = "BtnAddNew";
            ViewBag.MenuPageName = getDocumentName();
            TempData["ListFilterData"] = null;
            return RedirectToAction("ServiceVerificationDetail");
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType)
        {
            ServiceVerificationModel _SrVerModel = new ServiceVerificationModel();
            _SrVerModel.Message = "";
            var a = TrancType.Split(',');
            _SrVerModel.SrVerNo = a[0].Trim();
            _SrVerModel.TransType = "Update";
            _SrVerModel.BtnName = "BtnToDetailPage";
            
            var WF_status1 = a[2].Trim();
            _SrVerModel.WF_status1 = WF_status1;
            _SrVerModel.SrVerDate = a[1].Trim();
            TempData["WF_status1"] = WF_status1;
            TempData["ModelData"] = _SrVerModel;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ServiceVerificationDetail");
        }
        public ActionResult ServiceVerificationDetail(ServiceVerificationModel _SrVerModel,string varNO,string verDate,string transType,string btnName,string command)
        {
            try
            {
                var Ser_verModel = TempData["ModelData"] as ServiceVerificationModel;
                if (Ser_verModel != null)
                {
                    //ServiceVerificationModel Ser_verModel = new ServiceVerificationModel();
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    //Session["ILSearch"] = null;
                    Ser_verModel.ILSearch = null;

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


                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));


                    Ser_verModel.QtyDigit = QtyDigit;
                    ViewBag.QtyDigit = QtyDigit;

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    Ser_verModel.SupplierNameList = suppLists;

                    List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                    SourceDoc _DocumentNumber = new SourceDoc();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    Ser_verModel.SourceDocList = _DocumentNumberList;
                    //if(Ser_verModel.Message=="Approved")
                    //{
                    //    varNO = Ser_verModel.SrVerNo;
                    //    verDate = Ser_verModel.SrVerDate;
                    //    //Ser_verModel.BtnName=Ser_verModel.
                    //}
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        Ser_verModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        Ser_verModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    varNO = Ser_verModel.SrVerNo;
                    verDate = Ser_verModel.SrVerDate;
                    if (varNO != null && verDate != null)
                    {
                        string Doc_no = varNO;
                        string Doc_date = verDate;

                        DataSet ds = GetServiceVerificationDetailEdit(Doc_no, Doc_date);


                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Ser_verModel.SrVerNo = ds.Tables[0].Rows[0]["sr_ver_no"].ToString();
                            Ser_verModel.SrVerDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["sr_ver_dt"].ToString()).ToString("yyyy-MM-dd");
                            Ser_verModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            Ser_verModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = Ser_verModel.SuppID, supp_name = Ser_verModel.SuppName });
                            Ser_verModel.SupplierNameList = suppLists;
                            Ser_verModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            Ser_verModel.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            Ser_verModel.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            Ser_verModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            Ser_verModel.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            Ser_verModel.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            Ser_verModel.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            Ser_verModel.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            Ser_verModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            Ser_verModel.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            Ser_verModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            Ser_verModel.StatusName = ds.Tables[0].Rows[0]["VerificationStauts"].ToString();
                            Ser_verModel.SrVerStatus = ds.Tables[0].Rows[0]["ver_status"].ToString().Trim();
                            Ser_verModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            Ser_verModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);//Cancelled
                            Ser_verModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _DocumentNumberList.Add(new SourceDoc { doc_no = Ser_verModel.SrcDocNo, doc_dt = Ser_verModel.SrcDocNo });
                            Ser_verModel.SourceDocList = _DocumentNumberList;
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                Ser_verModel.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }

                            create_id = ds.Tables[0].Rows[0]["createid"].ToString();   //

                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.AttechmentDetails = ds.Tables[2];
                            // ViewBag.QtyDigit = QtyDigit;
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["ver_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            Ser_verModel.Cancelled = true;
                            Ser_verModel.BtnName = "Refresh";
                        }
                        else
                        {
                            Ser_verModel.Cancelled = false;
                        }
                        Ser_verModel.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }
                        if (ViewBag.AppLevel != null && Ser_verModel.Command != "Edit")
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
                                    Ser_verModel.BtnName = "BtnRefresh";
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
                                        Ser_verModel.BtnName = "BtnToDetailPage";
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
                                        Ser_verModel.BtnName = "BtnToDetailPage";
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
                                    Ser_verModel.BtnName = "BtnToDetailPage";
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
                                        Ser_verModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
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
                                    Ser_verModel.BtnName = "BtnToDetailPage";
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
                                    Ser_verModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    Ser_verModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    Ser_verModel.BtnName = "BtnRefresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }
                    Ser_verModel.Title = title;
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/Procurement/ServiceVerification/ServiceVerificationDetail.cshtml", Ser_verModel);
                }
                else
                {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message1"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    //Session["ILSearch"] = null;
                    _SrVerModel.ILSearch = null;
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


                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));


                    _SrVerModel.QtyDigit = QtyDigit;
                    ViewBag.QtyDigit = QtyDigit;

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _SrVerModel.SupplierNameList = suppLists;

                    List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                    SourceDoc _DocumentNumber = new SourceDoc();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _SrVerModel.SourceDocList = _DocumentNumberList;

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SrVerModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        Ser_verModel.WF_status1 = TempData["WF_status1"].ToString();
                    }              
                    if (varNO != null && verDate != null)
                    {
                        string Doc_no = varNO;
                        string Doc_date = verDate;

                        DataSet ds = GetServiceVerificationDetailEdit(Doc_no, Doc_date);


                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            _SrVerModel.SrVerNo = ds.Tables[0].Rows[0]["sr_ver_no"].ToString();
                            _SrVerModel.SrVerDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["sr_ver_dt"].ToString()).ToString("yyyy-MM-dd");
                            _SrVerModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _SrVerModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _SrVerModel.SuppID, supp_name = _SrVerModel.SuppName });
                            _SrVerModel.SupplierNameList = suppLists;
                            _SrVerModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _SrVerModel.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            _SrVerModel.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _SrVerModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _SrVerModel.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            _SrVerModel.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _SrVerModel.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _SrVerModel.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            _SrVerModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _SrVerModel.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            _SrVerModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _SrVerModel.StatusName = ds.Tables[0].Rows[0]["VerificationStauts"].ToString();
                            _SrVerModel.SrVerStatus = ds.Tables[0].Rows[0]["ver_status"].ToString().Trim();
                            _SrVerModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            _SrVerModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);//Cancelled
                            _SrVerModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _DocumentNumberList.Add(new SourceDoc { doc_no = _SrVerModel.SrcDocNo, doc_dt = _SrVerModel.SrcDocNo });
                            _SrVerModel.SourceDocList = _DocumentNumberList;
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _SrVerModel.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }

                            create_id = ds.Tables[0].Rows[0]["createid"].ToString();   //

                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.AttechmentDetails = ds.Tables[2];
                            // ViewBag.QtyDigit = QtyDigit;
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["ver_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _SrVerModel.Cancelled = true;
                            _SrVerModel.BtnName = "Refresh";
                        }
                        else
                        {
                            _SrVerModel.Cancelled = false;
                        }
                        _SrVerModel.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }
                        if (ViewBag.AppLevel != null && _SrVerModel.Command != "Edit")
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
                                    _SrVerModel.BtnName = "BtnRefresh";
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
                                        _SrVerModel.BtnName = "BtnToDetailPage";
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
                                        _SrVerModel.BtnName = "BtnToDetailPage";
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
                                    _SrVerModel.BtnName = "BtnToDetailPage";
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
                                        _SrVerModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
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
                                    _SrVerModel.BtnName = "BtnToDetailPage";
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
                                    _SrVerModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _SrVerModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _SrVerModel.BtnName = "BtnRefresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }
                    else
                    {
                        if (_SrVerModel.BtnName == "Refresh")
                        {

                        }
                        else
                        {
                            _SrVerModel.Message = "New";
                            _SrVerModel.Command = "Add";
                            _SrVerModel.AppStatus = "D";
                            _SrVerModel.TransType = "Save";
                            _SrVerModel.BtnName = "BtnAddNew";
                        }

                    }
                    _SrVerModel.Title = title;
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/Procurement/ServiceVerification/ServiceVerificationDetail.cshtml", _SrVerModel);
                }        
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
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
        public ActionResult GetAutoCompleteSearchSuppList(ServiceVerificationListModel _SrVerModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SuppType = string.Empty;
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
                if (string.IsNullOrEmpty(_SrVerModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _SrVerModel.SuppName;
                }

                SuppList = _SrVer_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in SuppList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _SrVerModel.SupplierNameList = _SuppList;
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

        public ActionResult GetVerificList(string docid, string status)
        {
            ServiceVerificationListModel _SrVerModel = new ServiceVerificationListModel();
            _SrVerModel.WF_status= status;
            //_SrVerModel.WF_Docid = docid;           
            //Session["WF_status"] = status;
            //Session["WF_Docid"] = docid;
            return RedirectToAction("ServiceVerification", _SrVerModel);
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
        public DataSet GetServiceVerificationDetailEdit(string Ver_No, string Ver_Date)
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
                DataSet result = _SrVer_ISERVICE.GetServiceVerificationDetail(Comp_ID, Br_ID, Ver_No, Ver_Date, User_ID, DocumentMenuId);
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
        private List<ServiceVerificationList> GetVerificationList(ServiceVerificationListModel _SrVerModel)
        {
            _ServiceVerificationList = new List<ServiceVerificationList>();
            try
            {
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
                DataSet DSet = _SrVer_ISERVICE.GetVerificationList(CompID, BrchID, User_ID, _SrVerModel.SuppID, _SrVerModel.Ver_FromDate, _SrVerModel.Ver_ToDate, _SrVerModel.Status, _SrVerModel.wfdocid, _SrVerModel.wfstatus);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ServiceVerificationList _VerficationList = new ServiceVerificationList();
                        _VerficationList.VerNo = dr["sr_ver_no"].ToString();
                        _VerficationList.VerDate = dr["sr_ver_dt"].ToString();
                        _VerficationList.VerDt = dr["VerDt"].ToString();                      
                        _VerficationList.SourceDocNo = dr["src_doc_number"].ToString();
                        _VerficationList.SourceDocDt = dr["src_doc_date"].ToString();
                        _VerficationList.SuppName = dr["supp_name"].ToString();                       
                        _VerficationList.VerStauts = dr["VerStauts"].ToString();
                        _VerficationList.CreateDate = dr["CreateDate"].ToString();
                        _VerficationList.ApproveDate = dr["ApproveDate"].ToString();
                        _VerficationList.ModifyDate = dr["ModifyDate"].ToString();
                        _VerficationList.create_by = dr["create_by"].ToString();
                        _VerficationList.app_by = dr["app_by"].ToString();
                        _VerficationList.mod_by = dr["mod_by"].ToString();
                        _ServiceVerificationList.Add(_VerficationList);
                    }
                }

                _SrVerModel.FinStDt = Convert.ToDateTime(DSet.Tables[2].Rows[0]["findate"]);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _ServiceVerificationList;
        }
       
        public ActionResult SearchVerificationList(string SuppId, string Fromdate, string Todate, string Status)
        {
            ServiceVerificationListModel _ServiceVerificationListModel = new ServiceVerificationListModel();
            try
            {
                //Session.Remove("WF_Docid");
                //Session.Remove("WF_status");
                //_ServiceVerificationListModel.wfdocid = null;
                //_ServiceVerificationListModel.wfstatus = null;
                //_ServiceVerificationListModel.WF_Docid = null;
                //_ServiceVerificationListModel.WF_status = null;
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

                _ServiceVerificationList = new List<ServiceVerificationList>();
                DataSet DSet = _SrVer_ISERVICE.GetVerificationList(CompID, BrchID, User_ID, SuppId, Fromdate, Todate, Status, "0", "");
                _ServiceVerificationListModel.VerSearch = "Ver_Search";

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ServiceVerificationList _VerficationList = new ServiceVerificationList();
                        _VerficationList.VerNo = dr["sr_ver_no"].ToString();
                        _VerficationList.VerDate = dr["sr_ver_dt"].ToString();
                        _VerficationList.VerDt = dr["VerDt"].ToString();
                        _VerficationList.SourceDocNo = dr["src_doc_number"].ToString();
                        _VerficationList.SourceDocDt = dr["src_doc_date"].ToString();
                        _VerficationList.SuppName = dr["supp_name"].ToString();
                        _VerficationList.VerStauts = dr["VerStauts"].ToString();
                        _VerficationList.CreateDate = dr["CreateDate"].ToString();
                        _VerficationList.ApproveDate = dr["ApproveDate"].ToString();
                        _VerficationList.ModifyDate = dr["ModifyDate"].ToString();
                        _VerficationList.create_by = dr["create_by"].ToString();
                        _VerficationList.app_by = dr["app_by"].ToString();
                        _VerficationList.mod_by = dr["mod_by"].ToString();
                        _ServiceVerificationList.Add(_VerficationList);
                    }
                }
                _ServiceVerificationListModel.FinStDt =Convert.ToDateTime(DSet.Tables[2].Rows[0]["findate"]);
                _ServiceVerificationListModel.VerificationList = _ServiceVerificationList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialServiceVerificationList.cshtml", _ServiceVerificationListModel);
        }

        [HttpPost]
        public JsonResult GetServicePOList(string Supp_id)
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
                DataSet result = _SrVer_ISERVICE.GetServicePOList(Supp_id, Comp_ID, Br_ID);

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
        public JsonResult GetSPODetails(string SPONo, string SPODate)
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
                DataSet result = _SrVer_ISERVICE.GetServicePODetail(SPONo, SPODate, Comp_ID, Br_ID);

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ServiceVerificationSave(ServiceVerificationModel _SrVerModel, string SrVerNo, string command)
        {
            try
            {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/

                if (_SrVerModel.Delete == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {

                    case "AddNew":
                        _SrVerModel.Message = "New";
                        _SrVerModel.Command = "Add";
                        _SrVerModel.DocumentStatus = "D";
                        _SrVerModel.TransType = "Save";
                        _SrVerModel.BtnName = "BtnAddNew";
                        //_SrVerModel.SrVerNo = null;
                        //_SrVerModel.SrVerDate = null;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                             if (!string.IsNullOrEmpty(SrVerNo))
                                return RedirectToAction("DoubleClickOnList", new { DocNo = SrVerNo, DocDate = _SrVerModel.SrVerDate, ListFilterData = _SrVerModel.ListFilterData1, WF_status = _SrVerModel.WFStatus });
                            else
                                _SrVerModel.Command = "Refresh";
                            _SrVerModel.TransType = "Refresh";
                            _SrVerModel.BtnName = "Refresh";
                            _SrVerModel.DocumentStatus = null;
                            TempData["ModelData"] = _SrVerModel;
                            return RedirectToAction("ServiceVerificationDetail", "ServiceVerification", _SrVerModel);
                        }
                        /*End to chk Financial year exist or not*/
                        _SrVerModel.SrVerNo = null;
                        _SrVerModel.SrVerDate = null;
                        
                        return RedirectToAction("ServiceVerificationDetail", "ServiceVerification");

                    case "Edit":
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _SrVerModel.SrVerNo, DocDate = _SrVerModel.SrVerDate, ListFilterData = _SrVerModel.ListFilterData1, WF_status = _SrVerModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //if (CheckAdvancePayment(_SrVerModel.SrVerNo, _SrVerModel.SrVerDate) == "Used")
                        //{
                        //    Session["Message"] = "Used";
                        //}
                        //else
                        //{
                        _SrVerModel.TransType = "Update";
                        _SrVerModel.Command = command;
                        _SrVerModel.BtnName = "BtnEdit";
                        _SrVerModel.Message = "New";
                        _SrVerModel.AppStatus = "D";
                        var varNO = _SrVerModel.SrVerNo;
                        var verDate = _SrVerModel.SrVerDate.ToString();
                        var transType = "Update";
                        var btnName = "BtnEdit";
                        TempData["ModelData"]= _SrVerModel;
                        TempData["ListFilterData"] = _SrVerModel.ListFilterData1;
                        return RedirectToAction("ServiceVerificationDetail", new { varNO = varNO, verDate, transType, btnName, command });

                    case "Delete":
                        ServiceVerificationModel _SVerDeleteModel = new ServiceVerificationModel();
                        _SVerDeleteModel.Command = command;
                        _SVerDeleteModel.BtnName = "Refresh";
                        SrVerNo = _SrVerModel.SrVerNo;
                        ServiceVerificationDelete(_SrVerModel, command);
                        _SVerDeleteModel.Message = "Deleted";
                        _SVerDeleteModel.Command = "Refresh";
                        _SrVerModel.SrVerNo = null;
                        _SrVerModel.SrVerDate = null;
                        //_SrVerModel = null;
                        _SVerDeleteModel.TransType = "Refresh";
                        _SVerDeleteModel.AppStatus = "DL";
                        _SVerDeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = _SVerDeleteModel;
                        TempData["ListFilterData"] = _SrVerModel.ListFilterData1;
                        return RedirectToAction("ServiceVerificationDetail");
                    case "Save":
                        _SrVerModel.Command = command;
                        SaveServiceVerification(_SrVerModel);
                        if(_SrVerModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_SrVerModel.Message == "DocModify")
                        {
                            CommonPageDetails();
                            ViewBag.DocID = DocumentMenuId;
                            ViewBag.DocumentMenuId = DocumentMenuId;

                            DataTable dt = new DataTable();
                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _SrVerModel.SuppID, supp_name = _SrVerModel.SuppName });
                            _SrVerModel.SupplierNameList = suppLists;

                            List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                            SourceDoc _DocumentNumber = new SourceDoc();
                            _DocumentNumber.doc_no = _SrVerModel.SrcDocNo;
                            _DocumentNumber.doc_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);
                            _SrVerModel.SourceDocList = _DocumentNumberList;

                            _SrVerModel.SuppName = _SrVerModel.SuppName;
                            _SrVerModel.Address = _SrVerModel.Address;
                           
                            _SrVerModel.SrcDocNo = _SrVerModel.SrcDocNo;
                            _SrVerModel.SrcDocDate = _SrVerModel.SrcDocDate;
                            ViewBag.ItemDetailsList = ViewData["ItemDetails"];
                           
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _SrVerModel.BtnName = "Refresh";
                            _SrVerModel.Command = "Refresh";
                            _SrVerModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            return View("~/Areas/ApplicationLayer/Views/Procurement/ServiceVerification/ServiceVerificationDetail.cshtml", _SrVerModel);
                        }
                        else
                        {
                            //Session["SrVerNo"] = Session["SrVerNo"].ToString();
                            //Session["SrVerDate"] = Session["SrVerDate"].ToString();
                            TempData["ModelData"] = _SrVerModel;
                            varNO = _SrVerModel.SrVerNo;
                            verDate = _SrVerModel.SrVerDate;
                            transType = "Update";
                            //_SrVerModel.Message = "Save";             
                            command = "Update";
                            btnName = "BtnToDetailPage";
                            //TempData["ModelData"] = _SrVerModel;
                            TempData["ListFilterData"] = _SrVerModel.ListFilterData1;
                            return RedirectToAction("ServiceVerificationDetail", new { varNO = varNO, verDate, transType, btnName, command });
                        }
                    case "Forward":
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _SrVerModel.SrVerNo, DocDate = _SrVerModel.SrVerDate, ListFilterData = _SrVerModel.ListFilterData1, WF_status = _SrVerModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        _SrVerModel.Command = command;
                        SrVerNo = _SrVerModel.SrVerNo;
                        _SrVerModel.SrVerNo = SrVerNo;
                        _SrVerModel.SrVerDate = _SrVerModel.SrVerDate;                    
                        ServiceVerificationApprove(_SrVerModel,_SrVerModel.SrVerNo, _SrVerModel.SrVerDate, "", "", "","","");               
                        _SrVerModel.SrVerDate = _SrVerModel.SrVerDate;
                        varNO = _SrVerModel.SrVerNo;
                        verDate = _SrVerModel.SrVerDate;
                        transType = _SrVerModel.TransType;
                        btnName = _SrVerModel.BtnName;
                        TempData["ModelData"] = _SrVerModel;
                        TempData["ListFilterData"] = _SrVerModel.ListFilterData1;
                        return RedirectToAction("ServiceVerificationDetail", new { varNO = varNO, verDate, transType, btnName, command });

                    case "Refresh":
                        ServiceVerificationModel _SVerRefreshModel = new ServiceVerificationModel();
                        _SVerRefreshModel.BtnName = "BtnRefresh";
                        _SVerRefreshModel.Command = command;
                        _SVerRefreshModel.TransType = "Save";
                        _SVerRefreshModel.Message = "Refresh";
                        _SVerRefreshModel.DocumentStatus = null;
                        TempData["ModelData"] = _SVerRefreshModel;
                        ViewBag.DocumentStatus = null;
                        TempData["ListFilterData"] = _SrVerModel.ListFilterData1;
                        return RedirectToAction("ServiceVerificationDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        //Session["SrVerNo"] = null;
                        //Session["SrVerDate"] = null;
                        _SrVerModel.SrVerNo = null;
                        _SrVerModel.SrVerDate = null;
                        TempData["ModelData"] = null;                      
                        TempData["WF_status"] = _SrVerModel.WF_status1;
                        TempData["ListFilterData"] = _SrVerModel.ListFilterData1;
                        return RedirectToAction("ServiceVerification", "ServiceVerification");

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

        public ActionResult SaveServiceVerification(ServiceVerificationModel _SrVerModel)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_SrVerModel.Cancelled == false)
                {

                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    BrchID = Session["BranchId"].ToString();
                    DataTable SrVerificationHeader = new DataTable();
                    DataTable SrVerificationItemDetails = new DataTable();

                    DataTable dtheader = new DataTable();

                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("CompID", typeof(string));
                    dtheader.Columns.Add("BranchID", typeof(string));
                    dtheader.Columns.Add("SrVerNo", typeof(string));
                    dtheader.Columns.Add("SrVerDate", typeof(string));
                    dtheader.Columns.Add("SuppName", typeof(int));
                    dtheader.Columns.Add("bill_add_id", typeof(int));
                    dtheader.Columns.Add("SrcDocNo", typeof(string));
                    dtheader.Columns.Add("SrcDocDate", typeof(string));
                    dtheader.Columns.Add("UserID", typeof(int));
                    dtheader.Columns.Add("Status", typeof(string));
                    dtheader.Columns.Add("SystemDetail", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["TransType"] = _SrVerModel.TransType;
                    dtrowHeader["CompID"] = CompID;
                    dtrowHeader["BranchID"] = BrchID;
                    dtrowHeader["SrVerNo"] = _SrVerModel.SrVerNo;
                    dtrowHeader["SrVerDate"] = _SrVerModel.SrVerDate;
                    dtrowHeader["SuppName"] = _SrVerModel.SuppID;
                    dtrowHeader["bill_add_id"] = _SrVerModel.bill_add_id;
                    if (_SrVerModel.SrcDocNo == "---Select---" || _SrVerModel.SrcDocNo == "0")
                    {
                        dtrowHeader["SrcDocNo"] = "";
                    }
                    else
                    {
                        dtrowHeader["SrcDocNo"] = _SrVerModel.SrcDocNo;
                    }

                    dtrowHeader["SrcDocDate"] = _SrVerModel.SrcDocDate;

                    dtrowHeader["UserID"] = UserID;
                    dtrowHeader["Status"] = IsNull(_SrVerModel.SrVerStatus, "D");
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["SystemDetail"] = mac_id;
                    dtheader.Rows.Add(dtrowHeader);
                    SrVerificationHeader = dtheader;

                    DataTable dtItem = new DataTable();

                    dtItem.Columns.Add("ItemID", typeof(string));
                    dtItem.Columns.Add("HsnNo", typeof(string));
                    dtItem.Columns.Add("OrderQty", typeof(string));
                    dtItem.Columns.Add("PendQty", typeof(string));
                    dtItem.Columns.Add("VerificQty", typeof(string));
                    dtItem.Columns.Add("VeriBy", typeof(string));
                    dtItem.Columns.Add("VeriOn", typeof(DateTime));
                    dtItem.Columns.Add("Remarks", typeof(string));
                    JArray jObject = JArray.Parse(_SrVerModel.Itemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["ItemID"] = jObject[i]["ItemId"].ToString();
                        dtrowLines["HsnNo"] = jObject[i]["HsnNo"].ToString();
                        dtrowLines["OrderQty"] = jObject[i]["ord_qty"].ToString();
                        dtrowLines["PendQty"] = jObject[i]["pend_qty"].ToString();
                        if (jObject[i]["Verific_qty"].ToString() == "")
                        {
                            dtrowLines["VerificQty"] = 0;
                        }
                        else
                        {
                            dtrowLines["VerificQty"] = jObject[i]["Verific_qty"].ToString();
                        }
                        dtrowLines["VeriBy"] = jObject[i]["VerifiedBy"].ToString();
                        dtrowLines["VeriOn"] = jObject[i]["VerifiedOn"].ToString();
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    SrVerificationItemDetails = dtItem;
                    ViewData["ItemDetails"] = dtitemdetail(jObject);


                    /*-----------------Attachment Section Start------------------------*/
                    DataTable SrVerAttachments = new DataTable();
                    DataTable BPdtAttachment = new DataTable();
                    var _RequestForServiceattch = TempData["ModelDataattch"] as RequestForServiceattch;
                    TempData["ModelDataattch"] = null;
                    if (_SrVerModel.attatchmentdetail != null)
                    {
                        if (_RequestForServiceattch != null)
                        {
                            if (_RequestForServiceattch.AttachMentDetailItmStp != null)
                            {
                                BPdtAttachment = _RequestForServiceattch.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                BPdtAttachment.Columns.Add("id", typeof(string));
                                BPdtAttachment.Columns.Add("file_name", typeof(string));
                                BPdtAttachment.Columns.Add("file_path", typeof(string));
                                BPdtAttachment.Columns.Add("file_def", typeof(char));
                                BPdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_SrVerModel.AttachMentDetailItmStp != null)
                            {
                                BPdtAttachment = _SrVerModel.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                BPdtAttachment.Columns.Add("id", typeof(string));
                                BPdtAttachment.Columns.Add("file_name", typeof(string));
                                BPdtAttachment.Columns.Add("file_path", typeof(string));
                                BPdtAttachment.Columns.Add("file_def", typeof(char));
                                BPdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_SrVerModel.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in BPdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = BPdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_SrVerModel.SrVerNo).ToString()))
                                {
                                    dtrowAttachment1["id"] = _SrVerModel.SrVerNo;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                BPdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        if (_SrVerModel.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string BP_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_SrVerModel.SrVerNo).ToString()))
                                {
                                    BP_CODE = (_SrVerModel.SrVerNo).ToString();

                                }
                                else
                                {
                                    BP_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + BP_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in BPdtAttachment.Rows)
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
                        SrVerAttachments = BPdtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/



                    SaveMessage = _SrVer_ISERVICE.InsertSrVerificationDetail(SrVerificationHeader, SrVerificationItemDetails, SrVerAttachments);
                    if (SaveMessage == "DocModify")
                    {
                        _SrVerModel.Message = "DocModify";
                        _SrVerModel.BtnName = "BtnRefresh";
                        _SrVerModel.Command = "Refresh";
                        TempData["ModelData"] = _SrVerModel;
                        return RedirectToAction("ServiceVerificationDetail");
                    }
                    else
                    {
                        string SrVerNo = SaveMessage.Split(',')[1].Trim();
                        string BP_Number = SrVerNo.Replace("/", "");
                        string Message = SaveMessage.Split(',')[0].Trim();
                        string SrVerDate = SaveMessage.Split(',')[2].Trim();
                        if (Message == "Data_Not_Found")
                        {
                            var a = SrVerNo.Split(',');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim()+" in "+PageName;
                            //var msg = "Data Not Found" +" "+ a[0].Trim();
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _SrVerModel.Message = Message.Replace("_", "");
                            return RedirectToAction("ServiceVerificationDetail");
                        }
                        /*-----------------Attachment Section Start------------------------*/
                        if (Message == "Save")
                        {
                            string Guid = "";
                            if (_RequestForServiceattch != null)
                            {
                                if (_RequestForServiceattch.Guid != null)
                                {
                                    Guid = _RequestForServiceattch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, BP_Number, _SrVerModel.TransType, SrVerAttachments);

                            //string Guid = Session["Guid"].ToString();
                            //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //if (Directory.Exists(sourcePath))
                            //{
                            //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                            //    foreach (string file in filePaths)
                            //    {
                            //        string[] items = file.Split('\\');
                            //        string ItemName = items[items.Length - 1];
                            //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            //        foreach (DataRow dr in SrVerAttachments.Rows)
                            //        {
                            //            string DrItmNm = dr["file_name"].ToString();
                            //            if (ItemName == DrItmNm)
                            //            {
                            //                string img_nm = CompID + BrchID + BP_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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

                        if (Message == "Update" || Message == "Save")
                            _SrVerModel.Message = "Save";
                        _SrVerModel.Command = "Update";
                        _SrVerModel.SrVerNo = SrVerNo;
                        _SrVerModel.SrVerDate = SrVerDate;
                        _SrVerModel.TransType = "Update";
                        _SrVerModel.AppStatus = "D";
                        _SrVerModel.BtnName = "BtnToDetailPage";

                        return RedirectToAction("ServiceVerificationDetail");
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
                    _SrVerModel.Create_by = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage1 = _SrVer_ISERVICE.ServiceVerificationCancel(_SrVerModel, CompID, br_id, mac_id);

                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _SrVerModel.SrVerNo, "C", UserID, "");
                    _SrVerModel.Message = "Cancelled";
                    _SrVerModel.Command = "Update";
                    _SrVerModel.SrVerNo = _SrVerModel.SrVerNo;
                    _SrVerModel.SrVerDate = _SrVerModel.SrVerDate;
                    _SrVerModel.TransType = "Update";
                    _SrVerModel.AppStatus = "D";
                    _SrVerModel.BtnName = "Refresh";
                    return RedirectToAction("ServiceVerificationDetail");
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_SrVerModel.TransType == "Save")
                    {

                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_SrVerModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _SrVerModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
              //  return View("~/Views/Shared/Error.cshtml");
            }

        }
        public DataTable dtitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("hsn_no", typeof(string));
            dtItem.Columns.Add("ord_qty", typeof(string));
            dtItem.Columns.Add("pend_qty", typeof(string));
            dtItem.Columns.Add("conf_qty", typeof(string));
            dtItem.Columns.Add("ver_by", typeof(string));
            dtItem.Columns.Add("ver_on", typeof(DateTime));
            dtItem.Columns.Add("it_remarks", typeof(string));


            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["hsn_no"] = jObject[i]["HsnNo"].ToString();
                dtrowLines["ord_qty"] = jObject[i]["ord_qty"].ToString();
                dtrowLines["pend_qty"] = jObject[i]["pend_qty"].ToString();
                if (jObject[i]["Verific_qty"].ToString() == "")
                {
                    dtrowLines["conf_qty"] = 0;
                }
                else
                {
                    dtrowLines["conf_qty"] = jObject[i]["Verific_qty"].ToString();
                }
                dtrowLines["ver_by"] = jObject[i]["VerifiedBy"].ToString();
                dtrowLines["ver_on"] = jObject[i]["VerifiedOn"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["Remarks"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList,string ListFilterData1, string WF_status1)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            ServiceVerificationModel _SrVerModel = new ServiceVerificationModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _SrVerModel.SrVerNo = jObjectBatch[i]["DocNo"].ToString();
                    _SrVerModel.SrVerDate = jObjectBatch[i]["DocDate"].ToString();
                    _SrVerModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _SrVerModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _SrVerModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                  
                }
            }
            if (_SrVerModel.A_Status != "Approve")
            {
                _SrVerModel.A_Status = "Approve";
            }
            //_SrVerModel.WF_status1 = WF_status1;
            _SrVerModel.ListFilterData1 = ListFilterData1;
            ServiceVerificationApprove(_SrVerModel, _SrVerModel.SrVerNo, _SrVerModel.SrVerDate, _SrVerModel.A_Status, _SrVerModel.A_Level, _SrVerModel.A_Remarks, _SrVerModel.ListFilterData1, WF_status1);
            var varNO = _SrVerModel.SrVerNo;
            var verDate = _SrVerModel.SrVerDate;
            var transType = _SrVerModel.TransType;
            var btnName = _SrVerModel.BtnName;
            var command = _SrVerModel.Command;
            TempData["ModelData"] = _SrVerModel;
            return RedirectToAction("ServiceVerificationDetail", new { varNO = varNO, verDate, transType, btnName, command });
        }

        public JsonResult ServiceVerificationApprove(ServiceVerificationModel _SrVerModel,string DocNo, string DocDate, string A_Status, string A_Level, string A_Remarks,string ListFilterData1, string WF_status1)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                //ServiceVerificationModel _SrVerModel =new ServiceVerificationModel();
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
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string POId = _SrVer_ISERVICE.InsertServiceVerificationApproveDetail(DocNo, DocDate, Br_ID, DocumentMenuId, Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks);

                //Send email alert
                _Common_IServices.SendAlertEmail(Comp_ID, Br_ID, DocumentMenuId, DocNo, "AP", UserID, "");
                _SrVerModel.SrVerNo = POId.Split(',')[0];
                _SrVerModel.Message = "Approved";                
                _SrVerModel.BtnName = "BtnEdit";
                _SrVerModel.TransType = "Update";
                _SrVerModel.SrVerDate = DocDate;
                TempData["WF_status1"] = WF_status1;
                TempData["ListFilterData"] = ListFilterData1;  
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
        private ActionResult ServiceVerificationDelete(ServiceVerificationModel _SrVerModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();

                string BPNo = _SrVerModel.SrVerNo;
                string BankPayNumber = BPNo.Replace("/", "");

                string Message = _SrVer_ISERVICE.ServiceVerificationDelete(_SrVerModel, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(BankPayNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, BankPayNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                _SrVerModel.Message = "Deleted";
                _SrVerModel.Command = "Refresh";
                _SrVerModel.SrVerNo = null;
                _SrVerModel.SrVerDate = null;
                //_SrVerModel = null;
                _SrVerModel.TransType = "Refresh";
                _SrVerModel.AppStatus = "DL";
                _SrVerModel.BtnName= "BtnDelete";
                TempData["ModelData"] = _SrVerModel;
                return RedirectToAction("ServiceVerificationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string Title, string DocNo, string TransType)
        {

            try
            {
                RequestForServiceattch _RequestForServiceattch = new RequestForServiceattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "Save";
                //string PONo = "";
                string branchID = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (TransType == "Save")
                //{
                //    PONo = gid.ToString();
                //}
                //PONo = PONo.Replace("/", "");
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _RequestForServiceattch.Guid = DocNo;
                //_RequestForServiceattch.Guid = PONo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
              //  getDocumentName(); /* To set Title*/
                string PageName = Title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _RequestForServiceattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _RequestForServiceattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _RequestForServiceattch;
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




