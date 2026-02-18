using EnRepMobileWeb.Areas.Common.Controllers.Common;
//using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.SampleReceiptOl;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.SampleReceipt;
using EnRepMobileWeb.MODELS.Common;
//using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.SampleReceiptNew;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.SampleReceipt;
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
using System.Linq.Dynamic;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialReceipt.SampleReceipt
{
    public class SampleReceiptController : Controller
    {
        DateTime FromDate;
        string CompID, BrchID, title, Userid = String.Empty;
        string DocumentMenuId = "105102115125";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SampleReceipt_ISERVICE _SampleReceipt_ISERVICE;
        public SampleReceiptController(SampleReceipt_ISERVICE _SampleReceipt_ISERVICE, Common_IServices _Common_IServices)
        {
            this._SampleReceipt_ISERVICE = _SampleReceipt_ISERVICE;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/SampleReceiptNew
        public ActionResult SampleReceiptList(SampleReceipt_ListModel _SampleReceiptListModel)
        {
            //SampleReceipt_ListModel _SampleReceiptListModel = new SampleReceipt_ListModel();
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
                _SampleReceiptListModel.wfdocid = DocumentMenuId;
            }
            else
            {
                _SampleReceiptListModel.wfdocid = "0";
            }
            if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
            {
                _SampleReceiptListModel.WF_status = TempData["WF_status"].ToString();
                if (_SampleReceiptListModel.WF_status != null)
                {
                    _SampleReceiptListModel.wfstatus = _SampleReceiptListModel.WF_status;
                }
                else
                {
                    _SampleReceiptListModel.wfstatus = "";
                }
            }
            else
            {
                if (_SampleReceiptListModel.WF_status != null)
                {
                    _SampleReceiptListModel.wfstatus = _SampleReceiptListModel.WF_status;
                }
                else
                {
                    _SampleReceiptListModel.wfstatus = "";
                }
            }
            var other = new CommonController(_Common_IServices);
            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
            ViewBag.DocumentMenuId = DocumentMenuId;

            List<EntityNameList> _EntityName = new List<EntityNameList>();
            EntityNameList _EntityNameList = new EntityNameList();
            _EntityNameList.entity_name = "---Select---";
            _EntityNameList.entity_id = "0";
            _EntityName.Add(_EntityNameList);
            _SampleReceiptListModel.EntityNameList = _EntityName;
            GetStatusList(_SampleReceiptListModel);

            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;

            SampleReceiptModel sr_model = new SampleReceiptModel();
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var PRData = TempData["ListFilterData"].ToString();
                var a = PRData.Split(',');
                sr_model.entity_type = a[0].Trim();
                _SampleReceiptListModel.EntityType = a[0].Trim();
                GetSupp_CustList(sr_model);
                _SampleReceiptListModel.EntityNameList = sr_model.EntityNameList;
                _SampleReceiptListModel.EntityName = a[1].Trim();
                _SampleReceiptListModel.SR_FromDate = a[2].Trim();
                _SampleReceiptListModel.SR_ToDate = a[3].Trim();
                _SampleReceiptListModel.Status = a[4].Trim();
                if (_SampleReceiptListModel.Status == "0")
                {
                    _SampleReceiptListModel.Status = null;
                }
                _SampleReceiptListModel.FromDate = _SampleReceiptListModel.SR_FromDate;
                GetSupp_CustList(sr_model);
                _SampleReceiptListModel.EntityName = a[1].Trim();
                _SampleReceiptListModel.ListFilterData = TempData["ListFilterData"].ToString();
            }
            else
            {
                _SampleReceiptListModel.FromDate = startDate;
                _SampleReceiptListModel.SR_FromDate = startDate;
                _SampleReceiptListModel.SR_ToDate = CurrentDate;
            }
            _SampleReceiptListModel.SR_List = GetSR_DetailList(_SampleReceiptListModel);
            //string endDate = dtnow.ToString("yyyy-MM-dd");
            /* _SampleReceiptListModel.FromDate = startDate;*/// FromDate.ToString("yyyy-MM-dd");
            ViewBag.MenuPageName = getDocumentName();
            _SampleReceiptListModel.Title = title;
            //Session["SRSearch"] = "0";
            _SampleReceiptListModel.SRSearch = "0";
            ViewBag.VBRoleList = GetRoleList();
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/SampleReceipt/SampleReceiptList.cshtml", _SampleReceiptListModel);
        }
        public ActionResult AddNew_SampleReceipt()
        {
            SampleReceiptModel _SampleReceiptModel = new SampleReceiptModel();
            _SampleReceiptModel.TransType = "Save";
            _SampleReceiptModel.Command = "New";
            _SampleReceiptModel.Message = "New";
            _SampleReceiptModel.BtnName = "BtnAddNew";
            _SampleReceiptModel.DocumentStatus = "D";
            ViewBag.DocumentStatus = "D";
            TempData["ModelData"] = _SampleReceiptModel;
            //Session["TransType"] = "Save";
            //Session["Command"] = "New";
            //Session["Message"] = "New";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("SampleReceiptList");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("SampleReceiptDetail");
        }
        public JsonResult GetSourceDocList(string Itm_ID, string SuppID, string entity_type, string sr_number)
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
                DataSet Deatils = _SampleReceipt_ISERVICE.GetSourceDocList(Comp_ID, Br_ID, Itm_ID, SuppID, entity_type, sr_number);

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

        public ActionResult GetST_ItemList()//, string EntityName, string entity_type) ----** Modified By Nitesh 10-10-2023 16:50 for Sample_Name Dropdown List  
        {
            JsonResult DataRows = null;
            string item_name = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string Entity_Name = string.Empty;
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
                DataSet ItmList = _SampleReceipt_ISERVICE.GetItemList(Comp_ID, Br_ID);//, EntityName, entity_type);
                DataRows = Json(JsonConvert.SerializeObject(ItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        public ActionResult SampleReceiptDetail(URLModelDetails URLModel)
        {
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
            if (Session["UserId"] != null)
            {
                Userid = Session["UserId"].ToString();
            }
            /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.srcpt_dt) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                var _SampleReceiptModel = TempData["ModelData"] as SampleReceiptModel;
                if (_SampleReceiptModel != null)
                {
                    List<EntityNameList> _EntityName = new List<EntityNameList>();
                    EntityNameList _EntityNameList = new EntityNameList();
                    _EntityNameList.entity_name = "---Select---";
                    _EntityNameList.entity_id = "0";
                    _EntityName.Add(_EntityNameList);
                    _SampleReceiptModel.EntityNameList = _EntityName;

                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _SampleReceiptModel.docNoLists = srcDocNoLists;

                    //List<listitem> _selectitm = new List<listitem>();
                    //_selectitm.Add(new listitem { item_id = "0", item_name = "---Select---" });
                    //_SampleReceiptModel.SelectList = _selectitm;
                    //List<uomitemlist> _uomitem = new List<uomitemlist>();
                    //_uomitem.Add(new uomitemlist { uomid = "0", uom_name = "0" });
                    //_SampleReceiptModel.uomlist = _uomitem;
                    List<SampleName> _entitName = new List<SampleName>();

                    SampleName _entitytypeList = new SampleName();
                    _entitytypeList.sample_name = "---Select---";
                    _entitytypeList.sample_id = "0";
                    _entitName.Add(_entitytypeList);
                    _SampleReceiptModel.SampleNamesList = _entitName;

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SampleReceiptModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _SampleReceiptModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_SampleReceiptModel.TransType == "Update" || _SampleReceiptModel.TransType == "Edit")
                    {
                        //string SRNo = Session["SR_No"].ToString();
                        //string SRDate = Session["SR_Date"].ToString();
                        string SRNo = _SampleReceiptModel.srcpt_no;
                        string SRDate = _SampleReceiptModel.srcpt_dt;
                        DataSet ds = _SampleReceipt_ISERVICE.DblClickgetdetailsSR(CompID, BrchID, SRNo, SRDate, Userid, DocumentMenuId);
                        _SampleReceiptModel.srcpt_no = ds.Tables[0].Rows[0]["sr_no"].ToString();
                        _SampleReceiptModel.srcpt_dt = ds.Tables[0].Rows[0]["srDate"].ToString();
                        _SampleReceiptModel.entity_type = ds.Tables[0].Rows[0]["entity_type"].ToString();
                        _SampleReceiptModel.source_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        _SampleReceiptModel.srcpt_rem = ds.Tables[0].Rows[0]["remarks"].ToString();
                        _SampleReceiptModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _SampleReceiptModel.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _SampleReceiptModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _SampleReceiptModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _SampleReceiptModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _SampleReceiptModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _SampleReceiptModel.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _SampleReceiptModel.srcpt_status = ds.Tables[0].Rows[0]["app_status"].ToString();

                        // show  Sample_name and src_doc_no when save and edit by Nitesh 10-10-2023 16:46 ---Start--

                        _SampleReceiptModel.ST_Item = ds.Tables[1].Rows[0]["item_name"].ToString();
                        _SampleReceiptModel.Sample_id = ds.Tables[1].Rows[0]["item_id"].ToString();

                        List<SelectListItem> _SelectListItem = new List<SelectListItem>();
                        _SelectListItem.Add(new SelectListItem { Value = ds.Tables[1].Rows[0]["item_id"].ToString(), Text = ds.Tables[1].Rows[0]["item_name"].ToString() });


                        _SampleReceiptModel.SrcDocNo = ds.Tables[1].Rows[0]["src_docno"].ToString();
                        List<SrcDocNoList> _srcDocNoLists = new List<SrcDocNoList>();
                        srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _SampleReceiptModel.SrcDocNo, SrcDocnoVal = _SampleReceiptModel.SrcDocNo });
                        _SampleReceiptModel.docNoLists = srcDocNoLists;
                        _SampleReceiptModel.SrcDocDate = ds.Tables[1].Rows[0]["src_docDate"].ToString();
                        _SampleReceiptModel.uom = ds.Tables[1].Rows[0]["uom_name"].ToString();
                        // -----------------------------------End---------------------------------------------
                        List<EntityNameList> _EntityName1 = new List<EntityNameList>();
                        EntityNameList _EntityNameList1 = new EntityNameList();
                        _EntityNameList1.entity_name = ds.Tables[0].Rows[0]["EntityName"].ToString();
                        _EntityNameList1.entity_id = ds.Tables[0].Rows[0]["entity_id"].ToString();
                        _EntityName1.Add(_EntityNameList1);
                        _SampleReceiptModel.EntityNameList = _EntityName1;

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _SampleReceiptModel.StatusCode = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        _SampleReceiptModel.DocumentStatus = doc_status;


                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            _SampleReceiptModel.CancelledRemarks  = ds.Tables[0].Rows[0]["cancel_remarks"].ToString(); 
                            _SampleReceiptModel.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _SampleReceiptModel.BtnName = "Refresh";
                        }
                        else
                        {
                            _SampleReceiptModel.CancelFlag = false;
                        }


                        _SampleReceiptModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _SampleReceiptModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SampleReceiptModel.Command != "Edit")
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

                            if (doc_status == "D")
                            {
                                if (create_id != Userid)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SampleReceiptModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == Userid)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SampleReceiptModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SampleReceiptModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Userid == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == Userid)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SampleReceiptModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (Userid == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == Userid)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == Userid || approval_id == Userid)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SampleReceiptModel.BtnName = "Refresh";
                                }
                            }
                            if (doc_status == "QP" || doc_status == "R")
                            {
                                _SampleReceiptModel.BtnName = "Refresh";
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        _SampleReceiptModel.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/SampleReceipt/SampleReceiptDetail.cshtml", _SampleReceiptModel);
                    }
                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        //Session["DocumentStatus"] ='D';
                        _SampleReceiptModel.DocumentStatus = "D";
                        _SampleReceiptModel.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/SampleReceipt/SampleReceiptDetail.cshtml", _SampleReceiptModel);
                    }
                }
                else
                {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
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
                    SampleReceiptModel _SampleReceiptModel1 = new SampleReceiptModel();
                    if (URLModel.srcpt_no != null || URLModel.srcpt_dt != null)
                    {
                        _SampleReceiptModel1.srcpt_no = URLModel.srcpt_no;
                        _SampleReceiptModel1.srcpt_dt = URLModel.srcpt_dt;
                    }
                    if (URLModel.TransType != null)
                    {
                        _SampleReceiptModel1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _SampleReceiptModel1.TransType = "Save";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _SampleReceiptModel1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _SampleReceiptModel1.BtnName = "Refresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _SampleReceiptModel1.Command = URLModel.Command;
                    }
                    else
                    {
                        _SampleReceiptModel1.Command = "Refresh";
                    }
                    List<EntityNameList> _EntityName = new List<EntityNameList>();
                    EntityNameList _EntityNameList = new EntityNameList();
                    _EntityNameList.entity_name = "---Select---";
                    _EntityNameList.entity_id = "0";
                    _EntityName.Add(_EntityNameList);
                    _SampleReceiptModel1.EntityNameList = _EntityName;
                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _SampleReceiptModel1.docNoLists = srcDocNoLists;
                    //List<uomitemlist> _uomitem = new List<uomitemlist>();
                    //_uomitem.Add(new uomitemlist { uomid = "0", uom_name = "0" });
                    //_SampleReceiptModel1.uomlist = _uomitem;
                    //List<listitem> _selectitm = new List<listitem>();
                    //_selectitm.Add(new listitem { item_id = "0", item_name = "---Select---" });
                    //_SampleReceiptModel1.SelectList = _selectitm;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SampleReceiptModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _SampleReceiptModel1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_SampleReceiptModel1.TransType == "Update" || _SampleReceiptModel1.TransType == "Edit")
                    {
                        //string SRNo = Session["SR_No"].ToString();
                        //string SRDate = Session["SR_Date"].ToString();
                        string SRNo = _SampleReceiptModel1.srcpt_no;
                        string SRDate = _SampleReceiptModel1.srcpt_dt;
                        DataSet ds = _SampleReceipt_ISERVICE.DblClickgetdetailsSR(CompID, BrchID, SRNo, SRDate, Userid, DocumentMenuId);

                        _SampleReceiptModel1.srcpt_no = ds.Tables[0].Rows[0]["sr_no"].ToString();
                        _SampleReceiptModel1.srcpt_dt = ds.Tables[0].Rows[0]["srDate"].ToString();
                        _SampleReceiptModel1.entity_type = ds.Tables[0].Rows[0]["entity_type"].ToString();
                        _SampleReceiptModel1.srcpt_rem = ds.Tables[0].Rows[0]["remarks"].ToString();
                        _SampleReceiptModel1.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _SampleReceiptModel1.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _SampleReceiptModel1.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _SampleReceiptModel1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _SampleReceiptModel1.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _SampleReceiptModel1.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _SampleReceiptModel1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _SampleReceiptModel1.srcpt_status = ds.Tables[0].Rows[0]["app_status"].ToString();

                        List<EntityNameList> _EntityName1 = new List<EntityNameList>();
                        EntityNameList _EntityNameList1 = new EntityNameList();
                        _EntityNameList1.entity_name = ds.Tables[0].Rows[0]["EntityName"].ToString();
                        _EntityNameList1.entity_id = ds.Tables[0].Rows[0]["entity_id"].ToString();
                        _EntityName1.Add(_EntityNameList1);
                        _SampleReceiptModel1.EntityNameList = _EntityName1;
                        _SampleReceiptModel1.source_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                                                      
                        _SampleReceiptModel1.ST_Item = ds.Tables[1].Rows[0]["item_name"].ToString();     // show  Sample_name and src_doc_no when save and edit by Nitesh 10-10-2023 16:46 ---Start--
                        _SampleReceiptModel1.Sample_id = ds.Tables[1].Rows[0]["item_id"].ToString();
                        List<SelectListItem> _SelectListItem = new List<SelectListItem>();
                        _SelectListItem.Add(new SelectListItem { Value = ds.Tables[1].Rows[0]["item_id"].ToString(), Text = ds.Tables[1].Rows[0]["item_name"].ToString() });
                        //_SampleReceiptModel1.SampleNamesList = _SelectListItem;
                        _SampleReceiptModel1.SrcDocNo = ds.Tables[1].Rows[0]["src_docno"].ToString();
                        List<SrcDocNoList> _srcDocNoLists = new List<SrcDocNoList>();
                        srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _SampleReceiptModel1.SrcDocNo, SrcDocnoVal = _SampleReceiptModel1.SrcDocNo });
                        _SampleReceiptModel1.docNoLists = srcDocNoLists;
                        _SampleReceiptModel1.SrcDocDate = ds.Tables[1].Rows[0]["src_docDate"].ToString();
                        _SampleReceiptModel1.uom = ds.Tables[1].Rows[0]["uom_name"].ToString();
                        // -----------End-----------------------
                        _SampleReceiptModel1.StatusCode = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        _SampleReceiptModel1.DocumentStatus = doc_status;


                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            _SampleReceiptModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _SampleReceiptModel1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _SampleReceiptModel1.BtnName = "Refresh";
                        }
                        else
                        {
                            _SampleReceiptModel1.CancelFlag = false;
                        }


                        _SampleReceiptModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _SampleReceiptModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SampleReceiptModel1.Command != "Edit")
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

                            if (doc_status == "D")
                            {
                                if (create_id != Userid)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SampleReceiptModel1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == Userid)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SampleReceiptModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SampleReceiptModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Userid == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == Userid)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _SampleReceiptModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (Userid == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == Userid)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == Userid || approval_id == Userid)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _SampleReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _SampleReceiptModel1.BtnName = "Refresh";
                                }
                            }
                            if (doc_status == "QP" || doc_status == "R")
                            {
                                _SampleReceiptModel1.BtnName = "Refresh";
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        _SampleReceiptModel1.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/SampleReceipt/SampleReceiptDetail.cshtml", _SampleReceiptModel1);
                    }
                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        //Session["DocumentStatus"] ='D';
                        _SampleReceiptModel1.DocumentStatus = "D";
                        _SampleReceiptModel1.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/SampleReceipt/SampleReceiptDetail.cshtml", _SampleReceiptModel1);
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
        public ActionResult EditSR(string SRId, string SRDate, string ListFilterData, string WF_status)
        {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
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
            URLModelDetails URLModel = new URLModelDetails();
            SampleReceiptModel _SampleReceiptModel = new SampleReceiptModel();
            _SampleReceiptModel.Message = "New";
            _SampleReceiptModel.Command = "Add";
            _SampleReceiptModel.srcpt_no = SRId;
            _SampleReceiptModel.srcpt_dt = SRDate;
            _SampleReceiptModel.TransType = "Update";
            _SampleReceiptModel.AppStatus = "D";
            _SampleReceiptModel.BtnName = "BtnToDetailPage";
            _SampleReceiptModel.WF_status1 = WF_status;
            TempData["ModelData"] = _SampleReceiptModel;
            URLModel.srcpt_no = SRId;
            URLModel.srcpt_dt = SRDate;
            URLModel.TransType = "Update";
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["SR_No"] = SRId;
            //Session["SR_Date"] = SRDate;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("SampleReceiptDetail", URLModel);
        }

        public string CheckRFQAgainstPR(string DocNo, string DocDate)
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
                DataSet Deatils = _SampleReceipt_ISERVICE.CheckRFQAgainstPR(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                return str;
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
        public ActionResult SampleReceiptBtnCommand(SampleReceiptModel _SampleReceiptModel, string SR_No, string command)
        {
            try
            {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/

                if (_SampleReceiptModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        SampleReceiptModel _SampleReceiptModelAddNew = new SampleReceiptModel();
                        _SampleReceiptModelAddNew.Message = "New";
                        _SampleReceiptModelAddNew.DocumentStatus = "D";
                        _SampleReceiptModelAddNew.BtnName = "BtnAddNew";
                        _SampleReceiptModelAddNew.TransType = "Save";
                        _SampleReceiptModelAddNew.Command = "New";
                        TempData["ModelData"] = _SampleReceiptModelAddNew;
                        //Session["Message"] = "New";
                        //Session["DocumentStatus"] = "D";
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                             if (!string.IsNullOrEmpty(_SampleReceiptModel.srcpt_no))
                                return RedirectToAction("EditSR", new { SRId = _SampleReceiptModel.srcpt_no, SRDate = _SampleReceiptModel.srcpt_dt, ListFilterData = _SampleReceiptModel.ListFilterData1, WF_status = _SampleReceiptModel.WFStatus });
                            else
                                _SampleReceiptModelAddNew.Command = "Refresh";
                            _SampleReceiptModelAddNew.TransType = "Refresh";
                            _SampleReceiptModelAddNew.BtnName = "Refresh";
                            _SampleReceiptModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = _SampleReceiptModelAddNew;
                            return RedirectToAction("SampleReceiptDetail", "SampleReceipt", _SampleReceiptModelAddNew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("SampleReceiptDetail", "SampleReceipt");

                    case "Edit":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSR", new { SRId = _SampleReceiptModel.srcpt_no, SRDate = _SampleReceiptModel.srcpt_dt, ListFilterData = _SampleReceiptModel.ListFilterData1, WF_status = _SampleReceiptModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt = _SampleReceiptModel.srcpt_dt;

                        /*Commented by NItesh 17122025 For last Year data can be Cancel*/
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, rcptDt) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("EditSR", new { SRId = _SampleReceiptModel.srcpt_no, SRDate = _SampleReceiptModel.srcpt_dt, ListFilterData = _SampleReceiptModel.ListFilterData1, WF_status = _SampleReceiptModel.WFStatus });
                        //}


                        /*End to chk Financial year exist or not*/
                        //if (_SampleReceiptModel.DocumentStatus == "QP")
                        //{
                        if (CheckRFQAgainstPR(_SampleReceiptModel.srcpt_no, _SampleReceiptModel.srcpt_dt) == "Used")
                        {
                            //Session["Message"] = "Used";
                            _SampleReceiptModel.Message = "UsedQP";
                            _SampleReceiptModel.srcpt_no = _SampleReceiptModel.srcpt_no;
                            _SampleReceiptModel.srcpt_dt = _SampleReceiptModel.srcpt_dt;
                            _SampleReceiptModel.TransType = "Update";
                            _SampleReceiptModel.Command = "Refresh";
                            _SampleReceiptModel.BtnName = "BtnToDetailPage";

                            TempData["ModelData"] = _SampleReceiptModel;
                            URLModelDetails _URLModel = new URLModelDetails();
                            _URLModel.srcpt_no = _SampleReceiptModel.srcpt_no;
                            _URLModel.srcpt_dt = _SampleReceiptModel.srcpt_dt;
                            _URLModel.TransType = "Update";
                            _URLModel.Command = "Refresh";
                            _URLModel.BtnName = "BtnToDetailPage";
                            TempData["ListFilterData"] = _SampleReceiptModel.ListFilterData1;
                            return RedirectToAction("SampleReceiptDetail", _URLModel);
                        }
                        else
                        {
                            _SampleReceiptModel.TransType = "Update";
                            _SampleReceiptModel.Command = command;
                            _SampleReceiptModel.BtnName = "BtnEdit";
                            _SampleReceiptModel.Message = "New";
                            _SampleReceiptModel.AppStatus = "D";
                            TempData["ModelData"] = _SampleReceiptModel;
                            URLModelDetails URLModel = new URLModelDetails();
                            URLModel.srcpt_no = _SampleReceiptModel.srcpt_no;
                            URLModel.srcpt_dt = _SampleReceiptModel.srcpt_dt;
                            URLModel.TransType = "Update";
                            URLModel.Command = command;
                            URLModel.BtnName = "BtnEdit";

                            //Session["TransType"] = "Update";
                            //Session["Command"] = command;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["Message"] = "New";
                            //Session["AppStatus"] = 'D';
                            TempData["ListFilterData"] = _SampleReceiptModel.ListFilterData1;
                            return RedirectToAction("SampleReceiptDetail", URLModel);
                        }
                    case "Delete":
                        SampleReceiptModel _SampleReceiptModelDelete = new SampleReceiptModel();
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        SRDelete(_SampleReceiptModel, command, _SampleReceiptModel.Title);
                        _SampleReceiptModelDelete.Message = "Deleted";
                        _SampleReceiptModelDelete.Command = "Refresh";
                        _SampleReceiptModelDelete.TransType = "Refresh";
                        _SampleReceiptModelDelete.AppStatus = "DL";
                        _SampleReceiptModelDelete.BtnName = "BtnDelete";
                        TempData["ModelData"] = _SampleReceiptModelDelete;
                        TempData["ListFilterData"] = _SampleReceiptModel.ListFilterData1;
                        return RedirectToAction("SampleReceiptDetail");

                    case "Save":
                        URLModelDetails URLModelSave = new URLModelDetails();
                        //Session["Command"] = command;
                        _SampleReceiptModel.Command = command;
                        SaveSRDetail(_SampleReceiptModel, _SampleReceiptModel.Title);
                        if (_SampleReceiptModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = _SampleReceiptModel;
                        URLModelSave.BtnName = _SampleReceiptModel.BtnName;
                        URLModelSave.TransType = _SampleReceiptModel.TransType;
                        URLModelSave.Command = _SampleReceiptModel.Command;
                        URLModelSave.srcpt_no = _SampleReceiptModel.srcpt_no;
                        URLModelSave.srcpt_dt = _SampleReceiptModel.srcpt_dt;
                        //Session["SR_No"] = Session["SR_No"].ToString();
                        //Session["SR_Date"] = Session["SR_Date"].ToString();
                        TempData["ListFilterData"] = _SampleReceiptModel.ListFilterData1;
                        return RedirectToAction("SampleReceiptDetail", URLModelSave);

                    case "Forward":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSR", new { SRId = _SampleReceiptModel.srcpt_no, SRDate = _SampleReceiptModel.srcpt_dt, ListFilterData = _SampleReceiptModel.ListFilterData1, WF_status = _SampleReceiptModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt1 = _SampleReceiptModel.srcpt_dt;

                        /*Commented by NItesh 17122025 For last Year data can be Forward*/
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, rcptDt1) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("EditSR", new { SRId = _SampleReceiptModel.srcpt_no, SRDate = _SampleReceiptModel.srcpt_dt, ListFilterData = _SampleReceiptModel.ListFilterData1, WF_status = _SampleReceiptModel.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSR", new { SRId = _SampleReceiptModel.srcpt_no, SRDate = _SampleReceiptModel.srcpt_dt, ListFilterData = _SampleReceiptModel.ListFilterData1, WF_status = _SampleReceiptModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt2 = _SampleReceiptModel.srcpt_dt;

                        /*Commented by NItesh 17122025 For last Year data can be Approve*/
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, rcptDt2) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("EditSR", new { SRId = _SampleReceiptModel.srcpt_no, SRDate = _SampleReceiptModel.srcpt_dt, ListFilterData = _SampleReceiptModel.ListFilterData1, WF_status = _SampleReceiptModel.WFStatus });
                        //}
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        SR_No = _SampleReceiptModel.srcpt_no;
                        //Session["SR_No"] = SR_No;
                        SRListApprove(SR_No, _SampleReceiptModel.srcpt_dt, "", "", "", "", "");
                        _SampleReceiptModel.Message = "Approved";
                        _SampleReceiptModel.TransType = "Update";
                        _SampleReceiptModel.Command = "Approve";
                        _SampleReceiptModel.AppStatus = "D";
                        _SampleReceiptModel.BtnName = "BtnEdit";
                        URLModelDetails URLModelApprove = new URLModelDetails();
                        URLModelApprove.BtnName = _SampleReceiptModel.BtnName;
                        URLModelApprove.TransType = _SampleReceiptModel.TransType;
                        URLModelApprove.Command = _SampleReceiptModel.Command;
                        URLModelApprove.srcpt_no = _SampleReceiptModel.srcpt_no;
                        URLModelApprove.srcpt_dt = _SampleReceiptModel.srcpt_dt;
                        TempData["ModelData"] = _SampleReceiptModel;
                        TempData["ListFilterData"] = _SampleReceiptModel.ListFilterData1;
                        return RedirectToAction("SampleReceiptDetail", URLModelApprove);

                    case "Refresh":
                        SampleReceiptModel _SampleReceiptModelRefresh = new SampleReceiptModel();
                        _SampleReceiptModelRefresh.BtnName = "Refresh";
                        _SampleReceiptModelRefresh.Command = command;
                        _SampleReceiptModelRefresh.TransType = "Save";
                        TempData["ModelData"] = _SampleReceiptModelRefresh;
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        TempData["ListFilterData"] = _SampleReceiptModel.ListFilterData1;
                        return RedirectToAction("SampleReceiptDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        TempData["WF_status"] = _SampleReceiptModel.WF_status1;
                        TempData["ListFilterData"] = _SampleReceiptModel.ListFilterData1;
                        return RedirectToAction("SampleReceiptList", "SampleReceipt");

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
        public ActionResult SaveSRDetail(SampleReceiptModel _SampleReceiptModel, string title)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
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
                Userid = Session["Userid"].ToString();
            }

            try
            {
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable dtheader = new DataTable();

                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuDocumentId", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("sr_no", typeof(string));
                dtheader.Columns.Add("sr_dt", typeof(DateTime));
                dtheader.Columns.Add("entity_type", typeof(string));
                dtheader.Columns.Add("entity_id", typeof(int));
                dtheader.Columns.Add("user_id", typeof(int));
                dtheader.Columns.Add("sr_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("sr_val", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("src_type", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));



                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _SampleReceiptModel.TransType;
                dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                string cancelflag = _SampleReceiptModel.CancelFlag.ToString();
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
                dtrowHeader["sr_no"] = _SampleReceiptModel.srcpt_no;
                dtrowHeader["sr_dt"] = _SampleReceiptModel.srcpt_dt;
                dtrowHeader["entity_type"] = _SampleReceiptModel.entity_type.Trim();
                dtrowHeader["entity_type"] = _SampleReceiptModel.entity_type;
                dtrowHeader["entity_id"] = _SampleReceiptModel.entity_id;
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                dtrowHeader["sr_status"] = "D";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["sr_val"] = '0';
                dtrowHeader["remarks"] = _SampleReceiptModel.srcpt_rem;
                dtrowHeader["src_type"] = _SampleReceiptModel.src_type;
                dtrowHeader["cancel_remarks"] = _SampleReceiptModel.CancelledRemarks;

                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;


                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("rec_qty", typeof(string));
                dtItem.Columns.Add("lot_id", typeof(string));
                dtItem.Columns.Add("wh_id", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("it_remarks", typeof(string));
                dtItem.Columns.Add("bin_loc", typeof(string));
                dtItem.Columns.Add("sr_type", typeof(string));
                dtItem.Columns.Add("other_dtl", typeof(string));
                dtItem.Columns.Add("receive_date", typeof(string));
                dtItem.Columns.Add("src_doc_number", typeof(string));
                dtItem.Columns.Add("src_doc_date", typeof(string));
                dtItem.Columns.Add("issued_date", typeof(string));
                dtItem.Columns.Add("issued_qty", typeof(string));

                JArray jObject = JArray.Parse(_SampleReceiptModel.SR_ItemDetail);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["uom_id"].ToString();
                    dtrowLines["rec_qty"] = jObject[i]["rec_qty"].ToString();
                    dtrowLines["lot_id"] = jObject[i]["lot_id"].ToString();
                    dtrowLines["wh_id"] = jObject[i]["wh_id"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["it_remarks"] = jObject[i]["it_remarks"].ToString();
                    dtrowLines["bin_loc"] = jObject[i]["bin_loaction"].ToString();
                    dtrowLines["sr_type"] = jObject[i]["sr_type"].ToString();
                    dtrowLines["other_dtl"] = jObject[i]["other_dtl"].ToString();
                    dtrowLines["receive_date"] = jObject[i]["receive_date"].ToString();
                    if (_SampleReceiptModel.src_type == "A")
                    {
                        dtrowLines["src_doc_number"] = jObject[i]["src_doc"].ToString();
                        dtrowLines["src_doc_date"] = jObject[i]["src_docdate"].ToString();
                        dtrowLines["issued_date"] = jObject[i]["iss_date"].ToString();
                        dtrowLines["issued_qty"] = jObject[i]["issu_qty"].ToString();
                    }
                    else
                    {
                        dtrowLines["src_doc_number"] = "";
                        dtrowLines["src_doc_date"] = "";
                        dtrowLines["issued_date"] = "";
                        dtrowLines["issued_qty"] = "";
                    }

                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;

                /*Commented By Nitesh 04-12-2023  */

                ///*----------------------Sub Item ----------------------*/
                //DataTable dtSubItem = new DataTable();
                //dtSubItem.Columns.Add("item_id", typeof(string));
                //dtSubItem.Columns.Add("sub_item_id", typeof(string));
                //dtSubItem.Columns.Add("qty", typeof(string));

                //dtSubItem.Columns.Add("issued_qty", typeof(string));



                //if (_SampleReceiptModel.SubItemDetailsDt != null)
                //{
                //    JArray jObject2 = JArray.Parse(_SampleReceiptModel.SubItemDetailsDt);
                //    for (int i = 0; i < jObject2.Count; i++)
                //    {
                   //  DataRow dtrowItemdetails = dtSubItem.NewRow();
                //        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                //        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                //        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                //        if (_SampleReceiptModel.src_type == "A")
                //        {
                //            dtrowItemdetails["issued_qty"] = jObject2[i]["issued_qty"].ToString();


                //        }
                //        else
                //        {
                //            dtrowItemdetails["issued_qty"] = 0;


                //        }
                     //  dtSubItem.Rows.Add(dtrowItemdetails);
                //  }
                //}

                ///*------------------Sub Item end----------------------*/

                DataTable dtAttachment = new DataTable();
                var _SampleReceiptModelAttch = TempData["ModelDataattch"] as SampleReceiptModelAttch;
                TempData["ModelDataattch"] = null;
                if (_SampleReceiptModel.attatchmentdetail != null)
                {
                    if (_SampleReceiptModelAttch != null)
                    {
                        if (_SampleReceiptModelAttch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _SampleReceiptModelAttch.AttachMentDetailItmStp as DataTable;
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
                        if (_SampleReceiptModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _SampleReceiptModel.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_SampleReceiptModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_SampleReceiptModel.srcpt_no))
                            {
                                dtrowAttachment1["id"] = _SampleReceiptModel.srcpt_no;
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
                    //if (Session["TransType"].ToString() == "Update")
                    if (_SampleReceiptModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_SampleReceiptModel.srcpt_no))
                            {
                                ItmCode = _SampleReceiptModel.srcpt_no;
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

                SaveMessage = _SampleReceipt_ISERVICE.InsertUpdateSR_Details(DtblHDetail, DtblItemDetail, /*dtSubItem,*/ DtblAttchDetail);

                string[] Data = SaveMessage.Split(',');

                string SRNo = Data[0];
                string SR_No = SRNo.Replace("/", "");
                string Message = Data[2];
                string SRDate = Data[1];
                string Message1 = Data[4];
                string StatusCode = Data[3];

                if (Message == "DataNotFound")
                {
                    var msg = "Data Not found" + " " + StatusCode + " in " + Message1;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _SampleReceiptModel.Message = Message;
                    return RedirectToAction("SampleReceiptDetail");
                }

                /*-----------------Attachment Section Start------------------------*/
                if (Message == "Save")
                {
                    string Guid = "";
                    if (_SampleReceiptModelAttch != null)
                    {
                        //if (Session["Guid"] != null)
                        if (_SampleReceiptModelAttch.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _SampleReceiptModelAttch.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, BrchID, guid, PageName, SR_No, _SampleReceiptModel.TransType, DtblAttchDetail);

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
                    //                string SR_No1 = SR_No.Replace("/", "");
                    //                string img_nm = CompID + BrchID + SR_No1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _SampleReceiptModel.srcpt_no, "C", Userid, "");
                    _SampleReceiptModel.Message = "Cancelled";
                    _SampleReceiptModel.Command = "Update";
                    //TempData["SR_No"] = _SampleReceiptModel.srcpt_no;
                    //TempData["SR_Date"] = _SampleReceiptModel.srcpt_dt;
                    _SampleReceiptModel.TransType = "Update";
                    _SampleReceiptModel.AppStatus = "D";
                    _SampleReceiptModel.BtnName = "Refresh";
                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //TempData["SR_No"] = _SampleReceiptModel.srcpt_no;
                    //TempData["SR_Date"] = _SampleReceiptModel.srcpt_dt;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    return RedirectToAction("SampleReceiptDetail");
                }

                if (Message == "Update" || Message == "Save")
                {
                    _SampleReceiptModel.Message = "Save";
                    _SampleReceiptModel.Command = "Update";
                    _SampleReceiptModel.srcpt_no = SRNo;
                    _SampleReceiptModel.srcpt_dt = SRDate;
                    _SampleReceiptModel.TransType = "Update";
                    _SampleReceiptModel.AppStatus = "D";
                    _SampleReceiptModel.BtnName = "BtnSave";
                    //Session["Message"] = "Save";
                    //Session["Command"] = "Update";
                    //Session["SR_No"] = SRNo;
                    //Session["SR_Date"] = SRDate;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "BtnSave";
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                }
                return RedirectToAction("SampleReceiptDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_SampleReceiptModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_SampleReceiptModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _SampleReceiptModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }

        private ActionResult SRDelete(SampleReceiptModel _SRModel, string command, string title)
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
                string doc_no = _SRModel.srcpt_no;
                string Message = _SampleReceipt_ISERVICE.Delete_SR_Detail(_SRModel, CompID, BrchID);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, doc_no1, Server);
                }
                _SRModel.Message = "Deleted";
                _SRModel.Command = "Refresh";
                _SRModel.TransType = "Refresh";
                _SRModel.AppStatus = "DL";
                _SRModel.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["PR_No"] = "";
                //_SRModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("SampleReceiptDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }


        private List<SampleReceiptList> GetSR_DetailList(SampleReceipt_ListModel _SampleReceiptListModel)
        {
            try
            {
                List<SampleReceiptList> _SampleReceiptList = new List<SampleReceiptList>();
                DataSet dt = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //Session["DocumentStatus"] = "D";
                _SampleReceiptListModel.DocumentStatus = "D";
                if (Session["UserId"] != null)
                {
                    Userid = Session["UserId"].ToString();
                }
                dt = _SampleReceipt_ISERVICE.GetSRDetailList(CompID, BrchID, Userid, _SampleReceiptListModel.EntityType, _SampleReceiptListModel.EntityName, _SampleReceiptListModel.SR_FromDate, _SampleReceiptListModel.SR_ToDate, _SampleReceiptListModel.Status, _SampleReceiptListModel.wfdocid, _SampleReceiptListModel.wfstatus);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = Convert.ToDateTime(dt.Tables[1].Rows[0]["finstrdate"]);
                }
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        SampleReceiptList _SRList = new SampleReceiptList();
                        _SRList.SRNo = dr["SRNo"].ToString();
                        _SRList.SRDate = dr["SRDate"].ToString();
                        _SRList.SR_DT = dr["SR_DT"].ToString();
                        _SRList.EntityType = dr["entity_type"].ToString();
                        _SRList.EntityName = dr["EntityName"].ToString();
                        _SRList.Stauts = dr["Status"].ToString();
                        _SRList.CreateDate = dr["CreateDate"].ToString();
                        _SRList.ApproveDate = dr["ApproveDate"].ToString();
                        _SRList.ModifyDate = dr["ModifyDate"].ToString();
                        _SRList.create_by = dr["create_by"].ToString();
                        _SRList.app_by = dr["app_by"].ToString();
                        _SRList.mod_by = dr["mod_by"].ToString();
                        _SRList.src_no = dr["src_no"].ToString();
                        _SRList.src_dt = dr["src_dt"].ToString();
                        _SampleReceiptList.Add(_SRList);
                    }
                }
                return _SampleReceiptList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult GetSupp_CustList(SampleReceiptModel _SampleReceiptModel)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet SuppCustList = new DataSet();
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                string CompID = string.Empty;
                string BrchID = string.Empty;
                string source_type = string.Empty;

                List<EntityNameList> _EntityName = new List<EntityNameList>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();

                }
                if (_SampleReceiptModel.source_type != null)  // Modified By Nitesh 10-10-2023 1648 foe Source type accoding to supp data and customer data
                {
                    source_type = _SampleReceiptModel.source_type;
                }
                if (string.IsNullOrEmpty(_SampleReceiptModel.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = _SampleReceiptModel.EntityName.ToString();
                }

                if (string.IsNullOrEmpty(_SampleReceiptModel.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = _SampleReceiptModel.entity_type.ToString();
                }
                if (EntityType == "0")
                {
                    SuppCustList = _SampleReceipt_ISERVICE.getSuppCustList(CompID, BrchID, EntityName, EntityType, source_type);


                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList _EntityNameList = new EntityNameList();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                else
                {
                    SuppCustList = _SampleReceipt_ISERVICE.getSuppCustList(CompID, BrchID, EntityName, EntityType,source_type);

                    DataRow Drow = SuppCustList.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList _EntityNameList = new EntityNameList();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }

                _SampleReceiptModel.EntityNameList = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }


        }
        [HttpPost]
        public JsonResult GetItemUOM(string Itm_ID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _SampleReceipt_ISERVICE.GetItemUOMDAL(Itm_ID, Comp_ID);
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
        [HttpPost]
        public ActionResult SearchSampleReceiptDetail(string EntityType, string EntityName, string Fromdate, string Todate, string Status)
        {
            try
            {
                //Session.Remove("WF_Docid");
                //Session.Remove("WF_status");
                List<SampleReceiptList> _SampleReceiptList = new List<SampleReceiptList>();
                SampleReceipt_ListModel _SampleReceiptListModel = new SampleReceipt_ListModel();
                DataSet dt = new DataSet();
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
                    Userid = Session["UserId"].ToString();
                }
                dt = _SampleReceipt_ISERVICE.GetSRDetailList(CompID, BrchID, Userid, EntityType, EntityName, Fromdate, Todate, Status, "0", "");
                //Session["SRSearch"] = "SR_Search";
                _SampleReceiptListModel.SRSearch = "SR_Search";
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        SampleReceiptList _SRList = new SampleReceiptList();
                        _SRList.SRNo = dr["SRNo"].ToString();
                        _SRList.SRDate = dr["SRDate"].ToString();
                        _SRList.SR_DT = dr["SR_DT"].ToString();
                        _SRList.EntityType = dr["entity_type"].ToString();
                        _SRList.EntityName = dr["EntityName"].ToString();
                        _SRList.Stauts = dr["Status"].ToString();
                        _SRList.CreateDate = dr["CreateDate"].ToString();
                        _SRList.ApproveDate = dr["ApproveDate"].ToString();
                        _SRList.ModifyDate = dr["ModifyDate"].ToString();
                        _SRList.create_by = dr["create_by"].ToString();
                        _SRList.app_by = dr["app_by"].ToString();
                        _SRList.mod_by = dr["mod_by"].ToString();
                        _SRList.src_no = dr["src_no"].ToString();
                        _SRList.src_dt = dr["src_dt"].ToString();
                        _SampleReceiptList.Add(_SRList);
                    }
                }
                _SampleReceiptListModel.SR_List = _SampleReceiptList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSampleReceiptList.cshtml", _SampleReceiptListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public ActionResult GetSR_ItemList(SampleReceiptModel _SampleReceiptModel)
        {
            JsonResult DataRows = null;
            string ItmName = string.Empty;
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
                if (string.IsNullOrEmpty(_SampleReceiptModel.SR_Item))
                {
                    ItmName = "0";
                }
                else
                {
                    ItmName = _SampleReceiptModel.SR_Item;
                }
                DataSet ItmList = _SampleReceipt_ISERVICE.GetSampleRcptItmList(Comp_ID, Br_ID, ItmName);
                DataRow Drow = ItmList.Tables[0].NewRow();
                Drow[0] = "0";
                Drow[1] = "---Select---";
                Drow[2] = "0";
                ItmList.Tables[0].Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(ItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
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
            //Session["Message"] = "";
            SampleReceiptModel _SampleReceiptModel = new SampleReceiptModel();
            var a = TrancType.Split(',');
            _SampleReceiptModel.srcpt_no = a[0].Trim();
            _SampleReceiptModel.srcpt_dt = a[1].Trim();
            _SampleReceiptModel.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _SampleReceiptModel.BtnName = "BtnToDetailPage";
            URLModelDetails URLModel = new URLModelDetails();
            URLModel.srcpt_no = a[0].Trim();
            URLModel.srcpt_dt = a[1].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _SampleReceiptModel;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("SampleReceiptDetail", URLModel);
        }
        public ActionResult GetSRList(string docid, string status)
        {
            SampleReceipt_ListModel _SampleReceiptListModel = new SampleReceipt_ListModel();
            //Session["WF_Docid"] = docid;
            //Session["WF_status"] = status;
            _SampleReceiptListModel.WF_status = status;
            return RedirectToAction("SampleReceiptList", _SampleReceiptListModel);
        }
        public ActionResult SRListApprove(string SR_No, string SR_Date, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_status1)
        {
            try

            {
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string BranchID = string.Empty;
                string MenuDocId = string.Empty;
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
                    BranchID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //MenuDocId = DocumentMenuId;
                //}
                SampleReceiptModel _SRModel = new SampleReceiptModel();
                _SRModel.CreatedBy = Session["UserId"].ToString();
                _SRModel.srcpt_no = SR_No;
                //Session["SR_Date"] = SR_Date;
                _SRModel.srcpt_dt = SR_Date;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _SampleReceipt_ISERVICE.Approve_SampleReceipt(_SRModel, CompID, BranchID, SR_Date, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                string ApMessage = Message.Split(',')[1].Trim();
                string SRNo = Message.Split(',')[0].Trim();
                if (ApMessage == "A" || ApMessage == "QP" || ApMessage == "R")
                {
                    string stats = "";
                    if (ApMessage == "R" || ApMessage == "A")
                        stats = "AP";
                    else
                        stats = ApMessage;
                    _Common_IServices.SendAlertEmail(CompID, BranchID, DocumentMenuId, SR_No, stats, UserID, "");
                    //Session["Message"] = "Approved";
                    _SRModel.Message = "Approved";
                }
                _SRModel.TransType = "Update";
                _SRModel.Command = "Approve";
                _SRModel.AppStatus = "D";
                //_SRModel.BtnName = "BtnEdit";
                _SRModel.BtnName = "BtnToDetailPage";
                _SRModel.WF_status1 = WF_status1;
                TempData["ModelData"] = _SRModel;
                URLModelDetails URLModel = new URLModelDetails();
                URLModel.srcpt_no = _SRModel.srcpt_no;
                URLModel.srcpt_dt = _SRModel.srcpt_dt;
                URLModel.TransType = "Update";
               // URLModel.BtnName = "BtnEdit";
                URLModel.BtnName = "BtnToDetailPage";
                URLModel.Command = "Approve";
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["SR_No"] = _SRModel.srcpt_no;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                TempData["WF_status1"] = WF_status1;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("SampleReceiptDetail", URLModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
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
                if (Session["Userid"] != null)
                {
                    Userid = Session["Userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, Userid, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public void GetStatusList(SampleReceipt_ListModel _SampleReceiptListModel)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _SampleReceiptListModel.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private string getDocumentName()
        {
            string CompID = string.Empty;
            string language = string.Empty;
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
        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                SampleReceiptModelAttch _SampleReceiptModelAttch = new SampleReceiptModelAttch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string SR_No = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["SR_No"] != null)
                //{
                //    SR_No = Session["SR_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _SampleReceiptModelAttch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
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
                    _SampleReceiptModelAttch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SampleReceiptModelAttch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _SampleReceiptModelAttch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        /*-----------------Attachment Section End------------------------*/
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
   , string Flag, string Status, string Doc_no, string Doc_dt, string src_doc_no, string src_docdate)
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
                                    dt.Rows[i]["Qty"] = item.GetValue("qty");
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _SampleReceipt_ISERVICE.SR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, src_doc_no, src_docdate, Status).Tables[0];
                    }
                }
                else if (Flag == "SRAgnst_Issue")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {

                        dt = _SampleReceipt_ISERVICE.SR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, src_doc_no, src_docdate, Status).Tables[0];
                        dt.Columns.Add("issued_qty", typeof(string));
                        dt.Columns.Add("pend_qty", typeof(string));
                        dt.Columns.Add("Qty", typeof(string));
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["issued_qty"] = item.GetValue("iss_qty");
                                    dt.Rows[i]["pend_qty"] = item.GetValue("pend_qty");
                                    dt.Rows[i]["Qty"] = item.GetValue("qty");

                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _SampleReceipt_ISERVICE.SR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, src_doc_no, src_docdate, Status).Tables[0];
                        dt.Columns.Add("issued_qty", typeof(string));
                        dt.Columns.Add("pend_qty", typeof(string));
                        dt.Columns.Add("Qty", typeof(string));
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["issued_qty"] = item.GetValue("iss_qty");
                                    dt.Rows[i]["pend_qty"] = item.GetValue("pend_qty");
                                    dt.Rows[i]["Qty"] = item.GetValue("qty");

                                }
                            }
                        }
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

        public ActionResult getMaterialIssuedata(string sample_name, string entity_type, string srcdocno, string srcdocdt, string entityname, string SR_Number)
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
                DataSet ds = _SampleReceipt_ISERVICE.getMIdata(CompID, BrchID, sample_name, srcdocno, srcdocdt, entity_type, entityname, SR_Number);
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


        private List<SampleReceiptList> GetDataSR(string EntityType, string EntityName, string Fromdate, string Todate, string Status)
        {
          
            try
            {
                DataSet dt = new DataSet();
                string User_ID = string.Empty;
                string CompID = string.Empty;
                List<SampleReceiptList> _SampleReceiptList = new List<SampleReceiptList>();
                SampleReceipt_ListModel _SampleReceiptListModel = new SampleReceipt_ListModel();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //  DataTable DSet = new DataTable();
                DataSet DSet = new DataSet();
                                    
                dt = _SampleReceipt_ISERVICE.GetSRDetailList(CompID, BrchID, User_ID, EntityType, EntityName, Fromdate, Todate, Status, "0", "");

                if (dt.Tables[0].Rows.Count > 0)
                {
                    int rowno1 = 0;
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        rowno1++;   // Increment the counter

                        SampleReceiptList _SRList = new SampleReceiptList();
                        _SRList.sr_no = rowno1;
                        _SRList.SRNo = dr["SRNo"].ToString();
                        _SRList.SRDate = dr["SRDate"].ToString();
                        _SRList.SR_DT = dr["SR_DT"].ToString();
                        _SRList.EntityType = dr["entity_type"].ToString();
                        _SRList.EntityName = dr["EntityName"].ToString();
                        _SRList.Stauts = dr["Status"].ToString();
                        _SRList.CreateDate = dr["CreateDate"].ToString();
                        _SRList.ApproveDate = dr["ApproveDate"].ToString();
                        _SRList.ModifyDate = dr["ModifyDate"].ToString();
                        _SRList.create_by = dr["create_by"].ToString();
                        _SRList.app_by = dr["app_by"].ToString();
                        _SRList.mod_by = dr["mod_by"].ToString();
                        _SRList.src_no = dr["src_no"].ToString();
                        _SRList.src_dt = dr["src_dt"].ToString();

                        _SampleReceiptList.Add(_SRList);
                    }
                }

                //  _SampleReceiptListModel.SR_List = _SampleReceiptList;


                return _SampleReceiptList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        [HttpPost]
        public ActionResult PageLoadData(string EntityType, string EntityName, string Fromdate, string Todate, string Status)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn1 = Request.Form.GetValues("columns[" + Request.Form.GetValues
                    ("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<SampleReceiptList> _ItemListModel = new List<SampleReceiptList>();

                _ItemListModel = GetDataSR(EntityType, EntityName, Fromdate, Todate, Status);
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.SRNo.ToUpper().Contains(searchValue)
                    || m.SRDate.ToUpper().Contains(searchValue) || m.SR_DT.ToUpper().Contains(searchValue)
                    || m.EntityType.ToUpper().Contains(searchValue)
                    || m.EntityName.ToUpper().Contains(searchValue) || m.Stauts.ToUpper().Contains(searchValue)
                    || m.CreateDate.ToUpper().Contains(searchValue) || m.ApproveDate.ToUpper().Contains(searchValue)
                    || m.ModifyDate.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue)
                    || m.app_by.ToUpper().Contains(searchValue) || m.mod_by.ToUpper().Contains(searchValue)
                    || m.src_no.ToUpper().Contains(searchValue) || m.src_dt.ToUpper().Contains(searchValue)
                    );
                }

                recordsTotal = ItemListData.Count();

                var data = ItemListData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
               
        }

        public FileResult ExtcelDownload(string searchValue, string EntityType, string EntityName, string Fromdate, string Todate, string Status)
        {
            try
            {
                string ExcelName = string.Empty;
                string User_ID = string.Empty;
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                var sortColumn = "sr_no";// Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = "asc";// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                DataTable dt = new DataTable();


                List<SampleReceiptList> _ItemListModel = new List<SampleReceiptList>();

                _ItemListModel = GetDataSR(EntityType, EntityName, Fromdate, Todate, Status);
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.SRNo.ToUpper().Contains(searchValue)
                    || m.SRDate.ToUpper().Contains(searchValue) || m.SR_DT.ToUpper().Contains(searchValue)
                    || m.EntityType.ToUpper().Contains(searchValue)
                    || m.EntityName.ToUpper().Contains(searchValue) || m.Stauts.ToUpper().Contains(searchValue)
                    || m.CreateDate.ToUpper().Contains(searchValue) || m.ApproveDate.ToUpper().Contains(searchValue)
                    || m.ModifyDate.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue)
                    || m.app_by.ToUpper().Contains(searchValue) || m.mod_by.ToUpper().Contains(searchValue)
                    || m.src_no.ToUpper().Contains(searchValue) || m.src_dt.ToUpper().Contains(searchValue)
                    );
                }

                var data = ItemListData.ToList();


                dt = SamplercptExcel(data);
                ExcelName = "SampleReceipt"; 
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel(ExcelName, dt);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable SamplercptExcel(List<SampleReceiptList> _ItemListModel)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Sr No.", typeof(int));
                dataTable.Columns.Add("Sample Receipt Number", typeof(string));
                dataTable.Columns.Add("Sample Receipt Date", typeof(string));
              
                dataTable.Columns.Add("Entity Type", typeof(string));
                dataTable.Columns.Add("Entity Name", typeof(string));
             
                dataTable.Columns.Add("Source Document Number", typeof(string));
                dataTable.Columns.Add("Source Document Date", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("Created By", typeof(string));
                dataTable.Columns.Add("Created On", typeof(string));
                dataTable.Columns.Add("Approved By", typeof(string));
                dataTable.Columns.Add("Approved On", typeof(string));
                dataTable.Columns.Add("Amended By", typeof(string));
                dataTable.Columns.Add("Amended On", typeof(string));


                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr No."] = item.sr_no;
                    rows["Sample Receipt Number"] = item.SRNo;
                    rows["Sample Receipt Date"] = item.SRDate;              
                    rows["Entity Type"] = item.EntityType;
                    rows["Entity Name"] = item.EntityName;                
                    rows["Source Document Number"] = item.src_no;
                    rows["Source Document Date"] = item.src_dt;
                    rows["Status"] = item.Stauts;
                    rows["Created By"] = item.create_by;
                    rows["Created On"] = item.CreateDate;
                    rows["Approved By"] = item.app_by;
                    rows["Approved On"] = item.ApproveDate;
                    rows["Amended By"] = item.mod_by;
                    rows["Amended On"] = item.ModifyDate;
                    dataTable.Rows.Add(rows);
                }

                return dataTable;
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
