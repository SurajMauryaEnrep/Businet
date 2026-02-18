using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialRequisitionSlip;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MRS;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialRequisitionSlip
{
    public class MaterialRequisitionSlipController : Controller
    {
        string title;
        List<MRSList> _MRSList;
        string DocumentMenuId = "105102125";
        MaterialRequisitionSlip_ISERVICE _materialRequisitionSlip_ISERVICE;
        MRSList_ISERVICES _MRSList_ISERVICES;
        Common_IServices _Common_IServices;
        //MRSModel _MRSModel;
        string CompID, BrchID, userid, UserID, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        DataTable ds;
        // string TransType;
        public MaterialRequisitionSlipController(MaterialRequisitionSlip_ISERVICE _materialRequisitionSlip_ISERVICE, Common_IServices _Common_IServices, MRSList_ISERVICES _MRSList_ISERVICES)
        {
            this._materialRequisitionSlip_ISERVICE = _materialRequisitionSlip_ISERVICE;
            this._Common_IServices = _Common_IServices;
            this._MRSList_ISERVICES = _MRSList_ISERVICES;
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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //private DataTable GetRoleList()
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            userid = Session["userid"].ToString();
        //        }
        //        DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);

        //        return RoleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
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
        public ActionResult MaterialRequisitionSlipList(MRSList_Model _MRSListDash_Model)
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
                MRSList_Model _MRSList_Model = new MRSList_Model();
                if (_MRSListDash_Model.WF_status != null)
                {
                    _MRSList_Model.WF_status = _MRSListDash_Model.WF_status;
                }
                else
                {
                    _MRSList_Model.WF_status = null;
                }
                CommonPageDetails();
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                //ViewBag.DocumentMenuId = DocumentMenuId;
                //GetStatusList(_MRSList_Model);
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _MRSList_Model.StatusList = statusLists;

                List<RequirementAreaList> _RequirementAreaList = new List<RequirementAreaList>();

                /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/
                //dt = GetRequirmentreaList("I");
                //foreach (DataRow dr in dt.Rows)
                //{
                //    RequirementAreaList _RequirementArea = new RequirementAreaList();
                //    _RequirementArea.req_id = Convert.ToInt32(dr["setup_id"]);
                //    _RequirementArea.req_val = dr["setup_val"].ToString();
                //    _RequirementAreaList.Add(_RequirementArea);
                //}
                //_RequirementAreaList.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "All" });
                //_MRSList_Model.RequirementAreaList = _RequirementAreaList;



                //string Entity = string.Empty;
                //Dictionary<string, string> EntityList = new Dictionary<string, string>();
                //if (string.IsNullOrEmpty(_MRSList_Model.ddlissueto))
                //{
                //    Entity = "0";
                //}
                //else
                //{
                //    Entity = _MRSList_Model.ddlissueto;
                //}
                //EntityList = _materialRequisitionSlip_ISERVICE.EntityList(CompID, Entity, BrchID);


                //List<IssueIDList> _IssueIDList = new List<IssueIDList>();
                //foreach (var data in EntityList)
                //{
                //    IssueIDList _IssueID = new IssueIDList();
                //    _IssueID.issue_id = data.Key;
                //    _IssueID.issue_val = data.Value;
                //    _IssueIDList.Add(_IssueID);
                //}
                //_MRSList_Model.IssueList = _IssueIDList;

                string Entity = string.Empty;
                if (string.IsNullOrEmpty(_MRSListDash_Model.ddlissueto))
                {
                    Entity = "0";
                }
                else
                {
                    Entity = _MRSListDash_Model.ddlissueto;
                }
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

               

              
                //var flag = "ListPage";
                DataSet ds = _materialRequisitionSlip_ISERVICE.MRS_GetAllDDLListAndListPageData(CompID, BrchID, Entity,
                    _MRSList_Model.WF_status, UserID, DocumentMenuId, startDate, CurrentDate);
                /******************Bind RequirementArea Dropdown*************************/
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    RequirementAreaList _RequirementArea = new RequirementAreaList();
                    _RequirementArea.req_id = Convert.ToInt32(dr["setup_id"]);
                    _RequirementArea.req_val = dr["setup_val"].ToString();
                    _RequirementAreaList.Add(_RequirementArea);
                }
                _RequirementAreaList.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "All" });
                _MRSList_Model.RequirementAreaList = _RequirementAreaList;

                /**********************End*************************/
                /******************Bind EnityList Dropdown*************************/
                List<IssueIDList> _IssueIDList = new List<IssueIDList>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    IssueIDList _IssueID = new IssueIDList();
                    _IssueID.issue_id = dr["id"].ToString();
                    _IssueID.issue_val = dr["val"].ToString();
                    _IssueIDList.Add(_IssueID);
                }
                _IssueIDList.Insert(0, new IssueIDList() { issue_id = "0", issue_val = "All" });
                _MRSList_Model.IssueList = _IssueIDList;

                /**********************End*************************/
                //  _IssueIDList.Insert(0, new IssueIDList() { issue_id = "0", issue_val = "---Select---" });
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");



                //string endDate = dtnow.ToString("yyyy-MM-dd");
                string itemID = "";
                string ItemName = "";
                //GetAutoCompleteSearchEntityList(_MRSList_Model);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    _MRSList_Model.req_area = Convert.ToInt32(a[0].Trim());
                    GetAutoCompleteSearchEntityList(_MRSList_Model);
                    var b = _MRSList_Model.IssueList;
                    _MRSList_Model.IssueList = _MRSList_Model.IssueList;
                    _MRSList_Model.issue_to = a[1].Trim().ToString();
                    _MRSList_Model.entity_type = a[2].Trim().ToString();

                    _MRSList_Model.MRS_Type = a[3].Trim();
                    _MRSList_Model.SRC_Type = a[4].Trim();
                    _MRSList_Model.MRS_FromDate = a[5].Trim();
                    _MRSList_Model.MRS_ToDate = a[6].Trim();
                    _MRSList_Model.MRSStatus = a[7].Trim();

                    itemID = a[8].Trim();
                    ItemName = a[9].Trim();
                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = itemID;
                    _ItemList.Item_Name = ItemName;
                    _ItemList1.Add(_ItemList);
                    _MRSList_Model.ItemNameList = _ItemList1;
                    if (_MRSList_Model.MRSStatus == "0")
                    {
                        _MRSList_Model.MRSStatus = null;
                    }
                    _MRSList_Model.FromDate = _MRSList_Model.MRS_FromDate;
                    _MRSList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    _MRSList_Model.BindMRSList = GetMRSDetailList(_MRSList_Model, itemID);
                    _MRSList_Model.issue_to = (a[1] + a[2]).Trim().ToString();
                    _MRSList_Model.MRS_Type = a[3].Trim();

                }
                else
                {

                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = "0";
                    _ItemList.Item_Name = "---Select---";
                    _ItemList1.Add(_ItemList);
                    _MRSList_Model.ItemNameList = _ItemList1;

                    //GetAutoCompleteSearchEntityList(_MRSList_Model);
                    /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/
                    //_MRSList_Model.BindMRSList = GetMRSDetailList(_MRSList_Model);
                    _MRSList = new List<MRSList>();
                    if (ds.Tables[2].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            MRSList _TempMRSList = new MRSList();
                            _TempMRSList.MRSNo = dr["mrs_no"].ToString();
                            _TempMRSList.MRSDate = dr["MrsDate"].ToString();
                            _TempMRSList.MRS_Dt = dr["Mrs_Date"].ToString();
                            _TempMRSList.MRSType = dr["MRSType"].ToString();
                            _TempMRSList.ReqArea = dr["reqArea"].ToString();
                            if (dr["src_type"].ToString() == "Direct" || dr["src_type"].ToString() == "Production Order")
                            {
                                _TempMRSList.SrcType = dr["src_type"].ToString();
                            }
                            else
                            {
                                _TempMRSList.SrcType = dr["SrcDocName"].ToString();
                            }
                            _TempMRSList.SrcDocNo = dr["src_doc_no"].ToString();
                            _TempMRSList.SrcDocDate = dr["SrcDocDate"].ToString();
                            _TempMRSList.SrcDocDt = dr["SrcDocDt"].ToString();
                            _TempMRSList.IssueTo = dr["Entity"].ToString();
                            _TempMRSList.MRSList_Stauts = dr["Status"].ToString();
                            _TempMRSList.CreateDate = dr["CreateDate"].ToString();
                            _TempMRSList.ApproveDate = dr["ApproveDate"].ToString();
                            _TempMRSList.ModifyDate = dr["ModifyDate"].ToString();
                            _TempMRSList.create_by = dr["create_by"].ToString();
                            _TempMRSList.app_by = dr["app_by"].ToString();
                            _TempMRSList.mod_by = dr["mod_by"].ToString();
                            _MRSList.Add(_TempMRSList);
                        }
                    }
                    _MRSList_Model.BindMRSList = _MRSList;
                    _MRSList_Model.FromDate = startDate;

                }
                //ViewBag.MenuPageName = getDocumentName();
                _MRSList_Model.Title = title;
                //Session["MRSSearch"] = "0";
                _MRSList_Model.MRSSearch = "0";
                _MRSList_Model.DocumentMenuId = DocumentMenuId;
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialRequisitionSlip/MaterialRequisitionSlipList.cshtml", _MRSList_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult MaterialRequisitionSlipDetail(UrlModel _UrlModel)
        {
            try
            {
                ViewBag.DocumentMenuId = DocumentMenuId;
                MRSModel _MRSModel = new MRSModel();
                var other = new CommonController(_Common_IServices);

                _MRSModel.mrs_dt = DateTime.Now;
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
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _UrlModel.MRSDate.ToString("yyyy-MM-dd")) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var MRSData = TempData["ModelData"] as MRSModel;
                if (MRSData != null)
                {
                    CommonPageDetails();
                    MRSData.mrs_dt = DateTime.Now;
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    if (MRSData.WF_status1 != null)
                    {
                        MRSData.WF_status1 = MRSData.WF_status1;
                    }
                    else
                    {
                        MRSData.WF_status1 = null;
                    }
                    List<RequirementArea> _RequirementAreaList = new List<RequirementArea>();
                    //dt = GetRequirmentreaList("I");
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    RequirementArea _RequirementArea = new RequirementArea();
                    //    _RequirementArea.req_id = Convert.ToInt32(dr["setup_id"]);
                    //    _RequirementArea.req_val = dr["setup_val"].ToString();
                    //    _RequirementAreaList.Add(_RequirementArea);
                    //}
                    _RequirementAreaList.Insert(0, new RequirementArea() { req_id = 0, req_val = "---Select---" });
                    MRSData.RequirementAreaList = _RequirementAreaList;

                    GetAutoCompleteSearchEntity(MRSData);
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        MRSData.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    MRSData.docNoLists = srcDocNoLists;
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (MRSData.TransType == "Update" || MRSData.Command == "Edit")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        //BrchID = Session["BranchId"].ToString();
                        //string mrs_no = Session["MRSNo"].ToString();
                        string mrs_no = MRSData.MRSNo;
                        DataSet ds = _materialRequisitionSlip_ISERVICE.GetMRSDetail(CompID, mrs_no, BrchID, UserID, DocumentMenuId, language);
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        MRSData.mrs_no = ds.Tables[0].Rows[0]["mrs_no"].ToString();
                        MRSData.mrs_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["mrs_dt"].ToString());
                        MRSData.mrs_type = ds.Tables[0].Rows[0]["mrs_type"].ToString();
                        MRSData.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        MRSData.issue_to = ds.Tables[0].Rows[0]["issue_to"].ToString();
                        MRSData.sr_type = ds.Tables[0].Rows[0]["mrs_type"].ToString();
                        GetAutoCompleteSearchEntity(MRSData);
                        //MRSData.SrcType = ds.Tables[0].Rows[0]["SrcDocName"].ToString();
                        //if(MRSData.SrcType!=null && MRSData.SrcType!="")
                        if (ds.Tables[0].Rows[0]["SrcDocName"].ToString() != null && ds.Tables[0].Rows[0]["SrcDocName"].ToString() != "")
                        {
                            MRSData.SrcType = ds.Tables[0].Rows[0]["SrcDocName"].ToString();
                            MRSData.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            MRSData.SrcDocDt = ds.Tables[0].Rows[0]["SrcDocDate"].ToString();
                        }
                        MRSData.mrs_rem = ds.Tables[0].Rows[0]["mrs_rem"].ToString();
                        MRSData.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        MRSData.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        MRSData.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        MRSData.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        MRSData.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        MRSData.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        MRSData.mrs_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        MRSData.Createid = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        MRSData.StatusCode = doc_status;
                        dt = GetRequirmentreaList(MRSData.mrs_type);
                        List<RequirementArea> _RequirementAreaList1 = new List<RequirementArea>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            RequirementArea _RequirementArea = new RequirementArea();
                            _RequirementArea.req_id = Convert.ToInt32(dr["setup_id"]);
                            _RequirementArea.req_val = dr["setup_val"].ToString();
                            _RequirementAreaList1.Add(_RequirementArea);
                        }
                        _RequirementAreaList1.Insert(0, new RequirementArea() { req_id = 0, req_val = "---Select---" });
                        MRSData.RequirementAreaList = _RequirementAreaList1;
                        MRSData.Pro_order_Num = ds.Tables[0].Rows[0]["src_docno"].ToString();
                        List<SrcDocNoList> _srcDocNoLists = new List<SrcDocNoList>();
                        srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = MRSData.Pro_order_Num, SrcDocnoVal = MRSData.Pro_order_Num });
                        MRSData.docNoLists = srcDocNoLists;
                        MRSData.Pro_order_dt = ds.Tables[0].Rows[0]["src_dt"].ToString();
                        MRSData.Pro_order_dt = ds.Tables[0].Rows[0]["src_dt"].ToString();
                        MRSData.Out_PutItm = ds.Tables[1].Rows[0]["outputitm"].ToString();
                        MRSData.src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        if (MRSData.StatusCode == "C")
                        {
                            MRSData.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            MRSData.BtnName = "Refresh";
                        }
                        else
                        {
                            MRSData.CancelFlag = false;
                        }
                        if (MRSData.StatusCode == "FC")
                        {
                            MRSData.ForceClose = true;
                            //Session["BtnName"] = "Refresh";
                            MRSData.BtnName = "Refresh";
                        }
                        else
                        {
                            MRSData.ForceClose = false;
                        }

                        MRSData.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        MRSData.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && MRSData.Command != "Edit")
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
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    MRSData.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
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
                                        MRSData.BtnName = "BtnToDetailPage";
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
                                        MRSData.BtnName = "BtnToDetailPage";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    MRSData.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        MRSData.BtnName = "BtnToDetailPage";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    MRSData.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
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
                                    MRSData.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    MRSData.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";

                                }
                                else
                                {
                                    MRSData.BtnName = "Refresh";
                                    //Session["BtnName"] = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                        MRSData.DocumentStatus = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        //ViewBag.MenuPageName = getDocumentName();
                        MRSData.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialRequisitionSlip/MaterialRequisitionSlipDetail.cshtml", MRSData);
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        MRSData.Title = title;
                        //Session["DocumentStatus"] = "D";
                        MRSData.DocumentStatus = "D";
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialRequisitionSlip/MaterialRequisitionSlipDetail.cshtml", MRSData);
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
                    if (_UrlModel.MRSNo != null && _UrlModel.MRSNo != "")
                    {
                        _MRSModel.MRSNo = _UrlModel.MRSNo;
                    }
                    if (_UrlModel.TransType != null)
                    {
                        _MRSModel.TransType = _UrlModel.TransType;
                    }
                    else
                    {
                        _MRSModel.TransType = "New";
                    }
                    if (_UrlModel.BtnName != null)
                    {
                        _MRSModel.BtnName = _UrlModel.BtnName;
                    }
                    else
                    {
                        _MRSModel.BtnName = "BtnRefresh";
                    }
                    if (_UrlModel.Command != null)
                    {
                        _MRSModel.Command = _UrlModel.Command;
                    }
                    else
                    {
                        _MRSModel.Command = "Refresh";
                    }
                    if (_UrlModel.WF_status1 != null)
                    {
                        _MRSModel.WF_status1 = _UrlModel.WF_status1;
                    }
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    CommonPageDetails();
                    List<RequirementArea> _RequirementAreaList = new List<RequirementArea>();
                    //dt = GetRequirmentreaList("I");
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    RequirementArea _RequirementArea = new RequirementArea();
                    //    _RequirementArea.req_id = Convert.ToInt32(dr["setup_id"]);
                    //    _RequirementArea.req_val = dr["setup_val"].ToString();
                    //    _RequirementAreaList.Add(_RequirementArea);
                    //}
                    _RequirementAreaList.Insert(0, new RequirementArea() { req_id = 0, req_val = "---Select---" });
                    _MRSModel.RequirementAreaList = _RequirementAreaList;

                    List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
                    srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    _MRSModel.docNoLists = srcDocNoLists;

                    GetAutoCompleteSearchEntity(_MRSModel);
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MRSModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_MRSModel.TransType == "Update" || _MRSModel.TransType == "Edit")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        //BrchID = Session["BranchId"].ToString();
                        //string mrs_no = Session["MRSNo"].ToString();
                        string mrs_no = _MRSModel.MRSNo;
                        DataSet ds = _materialRequisitionSlip_ISERVICE.GetMRSDetail(CompID, mrs_no, BrchID, UserID, DocumentMenuId, language);
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        _MRSModel.mrs_no = ds.Tables[0].Rows[0]["mrs_no"].ToString();
                        _MRSModel.mrs_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["mrs_dt"].ToString());
                        _MRSModel.mrs_type = ds.Tables[0].Rows[0]["mrs_type"].ToString();
                        _MRSModel.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        _MRSModel.issue_to = ds.Tables[0].Rows[0]["issue_to"].ToString();
                        _MRSModel.sr_type = ds.Tables[0].Rows[0]["mrs_type"].ToString();
                        GetAutoCompleteSearchEntity(_MRSModel);
                        if (ds.Tables[0].Rows[0]["SrcDocName"].ToString() != null && ds.Tables[0].Rows[0]["SrcDocName"].ToString() != "")
                        {
                            _MRSModel.SrcType = ds.Tables[0].Rows[0]["SrcDocName"].ToString();
                            _MRSModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            _MRSModel.SrcDocDt = ds.Tables[0].Rows[0]["SrcDocDate"].ToString();
                        }
                        _MRSModel.mrs_rem = ds.Tables[0].Rows[0]["mrs_rem"].ToString();
                        _MRSModel.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _MRSModel.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _MRSModel.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _MRSModel.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _MRSModel.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _MRSModel.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _MRSModel.mrs_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _MRSModel.Createid = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _MRSModel.StatusCode = doc_status;
                        dt = GetRequirmentreaList(_MRSModel.mrs_type);
                        List<RequirementArea> _RequirementAreaList1 = new List<RequirementArea>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            RequirementArea _RequirementArea = new RequirementArea();
                            _RequirementArea.req_id = Convert.ToInt32(dr["setup_id"]);
                            _RequirementArea.req_val = dr["setup_val"].ToString();
                            _RequirementAreaList1.Add(_RequirementArea);
                        }
                        _RequirementAreaList1.Insert(0, new RequirementArea() { req_id = 0, req_val = "---Select---" });
                        _MRSModel.RequirementAreaList = _RequirementAreaList1;
                        _MRSModel.Pro_order_Num = ds.Tables[0].Rows[0]["src_docno"].ToString();
                        List<SrcDocNoList> _srcDocNoLists = new List<SrcDocNoList>();
                        srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _MRSModel.Pro_order_Num, SrcDocnoVal = _MRSModel.Pro_order_Num });
                        _MRSModel.docNoLists = srcDocNoLists;
                        _MRSModel.Pro_order_dt = ds.Tables[0].Rows[0]["src_dt"].ToString();
                        _MRSModel.Out_PutItm = ds.Tables[1].Rows[0]["outputitm"].ToString();
                        _MRSModel.src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        if (_MRSModel.StatusCode == "C")
                        {
                            _MRSModel.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _MRSModel.BtnName = "Refresh";
                        }
                        else
                        {
                            _MRSModel.CancelFlag = false;
                        }
                        if (_MRSModel.StatusCode == "FC")
                        {
                            _MRSModel.ForceClose = true;
                            //Session["BtnName"] = "Refresh";
                            _MRSModel.BtnName = "Refresh";
                        }
                        else
                        {
                            _MRSModel.ForceClose = false;
                        }

                        _MRSModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _MRSModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _MRSModel.Command != "Edit")
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
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _MRSModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
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
                                        _MRSModel.BtnName = "BtnToDetailPage";
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
                                        _MRSModel.BtnName = "BtnToDetailPage";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MRSModel.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _MRSModel.BtnName = "BtnToDetailPage";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MRSModel.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
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
                                    _MRSModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _MRSModel.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";

                                }
                                else
                                {
                                    _MRSModel.BtnName = "Refresh";
                                    //Session["BtnName"] = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                        _MRSModel.DocumentStatus = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        //ViewBag.MenuPageName = getDocumentName();
                        _MRSModel.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialRequisitionSlip/MaterialRequisitionSlipDetail.cshtml", _MRSModel);
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        _MRSModel.Title = title;
                        //Session["DocumentStatus"] = "D";
                        _MRSModel.DocumentStatus = "D";
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialRequisitionSlip/MaterialRequisitionSlipDetail.cshtml", _MRSModel);
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
        public ActionResult AddNewMaterialRequisitionSlip()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            MRSModel _MRSModel = new MRSModel();
            _MRSModel.Message = "New";
            _MRSModel.Command = "Add";
            _MRSModel.AppStatus = "D";
            _MRSModel.TransType = "Save";
            _MRSModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _MRSModel;
            ViewBag.MenuPageName = getDocumentName();
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
                return RedirectToAction("MaterialRequisitionSlipList");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("MaterialRequisitionSlipDetail", "MaterialRequisitionSlip");
        }
        public ActionResult EditMRS(string MRSId, string MRSDt, string ListFilterData, string WF_status)
        { /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
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
            //Session["Command"] = "Add";
            //Session["MRSNo"] = MRSId;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            MRSModel _MRSModel = new MRSModel();
            _MRSModel.Message = "New";
            _MRSModel.Command = "Add";
            _MRSModel.AppStatus = "D";
            _MRSModel.TransType = "Update";
            _MRSModel.BtnName = "BtnToDetailPage";
            _MRSModel.MRSNo = MRSId;
            _MRSModel.MRSDate = Convert.ToDateTime(MRSDt);
            _MRSModel.WF_status1 = WF_status;
            TempData["ModelData"] = _MRSModel;
            UrlModel _urlModel = new UrlModel();
            //_urlModel.Message = "New";
            // _urlModel.Command = "Add";
            //_urlModel.AppStatus = "D";
            _urlModel.TransType = "Update";
            _urlModel.BtnName = "BtnToDetailPage";
            _urlModel.MRSNo = MRSId;
            _urlModel.MRSDate = Convert.ToDateTime(MRSDt);
            _urlModel.WF_status1 = WF_status;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("MaterialRequisitionSlipDetail", "MaterialRequisitionSlip", _urlModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MaterialRequisitionSlipSave(MRSModel _MRSModel, string command, HttpPostedFileBase[] MRS_Files)
        {
            try
            {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                string mrs_no = string.Empty;
                if (_MRSModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        MRSModel _MRSModeladdnew = new MRSModel();
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        _MRSModeladdnew.Command = "Add";
                        _MRSModeladdnew.TransType = "Save";
                        _MRSModeladdnew.AppStatus = "D";
                        _MRSModeladdnew.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _MRSModeladdnew;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_MRSModel.mrs_no))
                                return RedirectToAction("EditMRS", new { MRSId = _MRSModel.mrs_no, MRSDt = _MRSModel.mrs_dt, ListFilterData = _MRSModel.ListFilterData1, WF_status = _MRSModel.WFStatus });
                            else
                                _MRSModeladdnew.Command = "Refresh";
                            _MRSModeladdnew.TransType = "Refresh";
                            _MRSModeladdnew.BtnName = "Refresh";
                            _MRSModeladdnew.DocumentStatus = null;
                            TempData["ModelData"] = _MRSModeladdnew;
                            return RedirectToAction("MaterialRequisitionSlipDetail", "MaterialRequisitionSlip");
                        }
                        /*End to chk Financial year exist or not*/

                        return RedirectToAction("MaterialRequisitionSlipDetail", "MaterialRequisitionSlip");

                    case "Edit":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditMRS", new { MRSId = _MRSModel.mrs_no,MRSDt=_MRSModel.mrs_dt, ListFilterData = _MRSModel.ListFilterData1, WF_status = _MRSModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string MrsDt = _MRSModel.mrs_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MrsDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditMRS", new { MRSId = _MRSModel.mrs_no, MRSDt = _MRSModel.mrs_dt, ListFilterData = _MRSModel.ListFilterData1, WF_status = _MRSModel.WFStatus });
                        }
                        string checkstatus = CheckInternalIssue(_MRSModel.mrs_no, _MRSModel.mrs_dt.ToString("yyyy-MM-dd"));/*Add by hina on 24-09-2024 to use on internal issue*/
                        if (checkstatus == "Used")
                        {
                            _MRSModel.TransType = "Update";
                            _MRSModel.Command = "Add";
                            _MRSModel.BtnName = "BtnToDetailPage";
                            _MRSModel.Message = checkstatus;
                            _MRSModel.MRSNo = _MRSModel.mrs_no;
                            _MRSModel.MRSDate = _MRSModel.mrs_dt;/*.ToString("yyyy-MM-dd");*/
                            TempData["ModelData"] = _MRSModel;
                            return RedirectToAction("MaterialRequisitionSlipDetail");
                        }
                        else
                        {
                            /*End to chk Financial year exist or not*/
                            //Session["TransType"] = "Update";
                            //Session["Command"] = command;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["Message"] = null;
                            //Session["MRSNo"] = _MRSModel.mrs_no;
                            _MRSModel.Command = command;
                            _MRSModel.TransType = "Update";
                            _MRSModel.AppStatus = "D";
                            _MRSModel.BtnName = "BtnEdit";
                            _MRSModel.MRSNo = _MRSModel.mrs_no;
                            TempData["ModelData"] = _MRSModel;
                            UrlModel _urlModel = new UrlModel();
                            _urlModel.Command = command;
                            _urlModel.TransType = "Update";
                            _urlModel.AppStatus = "D";
                            _urlModel.BtnName = "BtnEdit";
                            _urlModel.MRSNo = _MRSModel.mrs_no;
                            _urlModel.MRSDate = _MRSModel.mrs_dt;
                            TempData["ListFilterData"] = _MRSModel.ListFilterData1;
                            return RedirectToAction("MaterialRequisitionSlipDetail", _urlModel);
                        }


                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        _MRSModel.Command = command;
                        _MRSModel.BtnName = "Refresh";
                        mrs_no = _MRSModel.mrs_no;
                        MRSDelete(_MRSModel, command);
                        MRSModel _MRSDeleteModel = new MRSModel();
                        _MRSDeleteModel.Message = "Deleted";
                        _MRSDeleteModel.Command = "Refresh";
                        _MRSDeleteModel.TransType = "Refresh";
                        _MRSDeleteModel.AppStatus = "DL";
                        _MRSDeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = _MRSDeleteModel;
                        TempData["ListFilterData"] = _MRSModel.ListFilterData1;
                        return RedirectToAction("MaterialRequisitionSlipDetail");

                    case "Save":
                        //Session["Command"] = command;
                        _MRSModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            SaveMRSDetail(_MRSModel, MRS_Files);
                            if (_MRSModel.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            // Session["MRSNo"] = Session["MRSNo"].ToString();
                            TempData["ModelData"] = _MRSModel;
                            UrlModel URLModelSave = new UrlModel();
                            URLModelSave.BtnName = _MRSModel.BtnName;
                            URLModelSave.TransType = _MRSModel.TransType;
                            URLModelSave.Command = _MRSModel.Command;
                            URLModelSave.MRSNo = _MRSModel.MRSNo;
                            URLModelSave.MRSDate = _MRSModel.MRSDate;
                            TempData["ListFilterData"] = _MRSModel.ListFilterData1;
                            return RedirectToAction("MaterialRequisitionSlipDetail", URLModelSave);

                        }
                        else
                        {
                            TempData["ListFilterData"] = _MRSModel.ListFilterData1;
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialRequisitionSlip/MaterialRequisitionSlipDetail.cshtml", _MRSModel);
                        }

                    case "Forward":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditMRS", new { MRSId = _MRSModel.mrs_no,MRSDt=_MRSModel.mrs_dt, ListFilterData = _MRSModel.ListFilterData1, WF_status = _MRSModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string MrsDt1 = _MRSModel.mrs_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MrsDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditMRS", new { MRSId = _MRSModel.mrs_no, MRSDt = _MRSModel.mrs_dt, ListFilterData = _MRSModel.ListFilterData1, WF_status = _MRSModel.WFStatus });
                        }
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
                        //    return RedirectToAction("EditMRS", new { MRSId = _MRSModel.mrs_no,MRSDt=_MRSModel.mrs_dt, ListFilterData = _MRSModel.ListFilterData1, WF_status = _MRSModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string MrsDt2 = _MRSModel.mrs_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MrsDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditMRS", new { MRSId = _MRSModel.mrs_no, MRSDt = _MRSModel.mrs_dt, ListFilterData = _MRSModel.ListFilterData1, WF_status = _MRSModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _MRSModel.Command = command;
                        mrs_no = _MRSModel.mrs_no;
                        //Session["MRSNo"] = mrs_no;
                        _MRSModel.MRSNo = mrs_no; ;
                        MRSApprove(_MRSModel.mrs_no, _MRSModel.mrs_dt, _MRSModel.mrs_type, "", "", "", "", "", "");
                        _MRSModel.MRSNo = _MRSModel.mrs_no;
                        _MRSModel.MRSDate = _MRSModel.mrs_dt;
                        _MRSModel.Message = "Approved";
                        _MRSModel.Command = "Approve";
                        _MRSModel.TransType = "Update";
                        _MRSModel.AppStatus = "D";
                        _MRSModel.BtnName = "BtnEdit";
                        // _MRSModel.WF_status1 = WF_Status1;
                        // TempData["WF_Status1"] = WF_Status1;
                        TempData["ModelData"] = _MRSModel;
                        UrlModel _approve_Model = new UrlModel();
                        _approve_Model.MRSNo = _MRSModel.MRSNo;
                        _approve_Model.WF_status1 = _MRSModel.WF_status1;
                        _approve_Model.MRSDate = _MRSModel.MRSDate;
                        _approve_Model.TransType = "Update";
                        _approve_Model.BtnName = "BtnToDetailPage";
                        TempData["ListFilterData"] = _MRSModel.ListFilterData1;
                        return RedirectToAction("MaterialRequisitionSlipDetail", _approve_Model);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        MRSModel _MRSModelRefresh = new MRSModel();
                        _MRSModelRefresh.BtnName = "BtnRefresh";
                        _MRSModelRefresh.Command = command;
                        _MRSModelRefresh.TransType = "Save";
                        _MRSModelRefresh.Message = null;
                        _MRSModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _MRSModelRefresh;
                        TempData["ListFilterData"] = _MRSModel.ListFilterData1;
                        return RedirectToAction("MaterialRequisitionSlipDetail");

                    case "Print":
                        return GenratePdfFile(_MRSModel);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        MRSList_Model MRSDashBord = new MRSList_Model();
                        MRSDashBord.WF_status = _MRSModel.WF_status1;
                        TempData["ListFilterData"] = _MRSModel.ListFilterData1;
                        return RedirectToAction("MaterialRequisitionSlipList", "MaterialRequisitionSlip", MRSDashBord);

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
        public JsonResult GetAvlbStockForItem(string Itm_ID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _materialRequisitionSlip_ISERVICE.GetAvlbStockForItem(Itm_ID, Comp_ID, BrchID);
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
        public ActionResult getListOfItems(MRSModel _MRSModel)
        {
            JsonResult DataRows = null;
            string ItmName = string.Empty;
            //Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BranchId = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BranchId = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_MRSModel.Mrs_ItemName))
                {
                    ItmName = "0";
                }
                else
                {
                    ItmName = _MRSModel.Mrs_ItemName;
                }
                DataSet SOItmList = _materialRequisitionSlip_ISERVICE.GetItemList(Comp_ID, BranchId, ItmName);
                DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        [NonAction]
        public ActionResult SaveMRSDetail(MRSModel _MRSModel, HttpPostedFileBase[] MRSFiles)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            if (Session["compid"] != null)
            {
                CompID = Session["compid"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {
                if (_MRSModel.ForceClose != false)
                {

                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }

                    string br_id = Session["BranchId"].ToString();
                    _MRSModel.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    SaveMessage = _materialRequisitionSlip_ISERVICE.MRSForceClose(_MRSModel, CompID, br_id, mac_id);
                    string MRSNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                    //string fileName = "MRS_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "MaterialRequisitionSlip_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_MRSModel.mrs_no, _MRSModel.mrs_dt.ToString(), fileName, DocumentMenuId,"FC");
                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _MRSModel.mrs_no, "FC", userid, "", filePath);
                    //Session["Message"] = "Save";
                    //Session["Command"] = "Update";
                    //TempData["MRSNo"] = _MRSModel.mrs_no;
                    //TempData["MRSDate"] = _MRSModel.mrs_dt;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    _MRSModel.Command = "Update";
                    _MRSModel.TransType = "Update";
                    _MRSModel.AppStatus = "D";
                    _MRSModel.BtnName = "BtnEdit";
                    _MRSModel.Message = "Save";
                    _MRSModel.MRSNo = _MRSModel.mrs_no;
                    _MRSModel.MRSDate = _MRSModel.mrs_dt;
                    TempData["ModelData"] = _MRSModel;
                    UrlModel _urlModel = new UrlModel();
                    _urlModel.Command = "Update";
                    _urlModel.TransType = "Update";
                    _urlModel.AppStatus = "D";
                    _urlModel.BtnName = "BtnEdit";
                    _urlModel.MRSNo = _MRSModel.mrs_no;
                    _urlModel.MRSDate = _MRSModel.mrs_dt;
                    return RedirectToAction("MaterialRequisitionSlipDetail", _urlModel);
                }
                else
                {
                    if (_MRSModel.CancelFlag == false)
                    {
                        if (Session["compid"] != null)
                        {
                            CompID = Session["compid"].ToString();
                        }
                        if (Session["userid"] != null)
                        {
                            userid = Session["userid"].ToString();
                        }

                        DataTable MRSHeader = new DataTable();
                        DataTable MRSItemDetails = new DataTable();
                        DataTable MRSAttachments = new DataTable();


                        DataTable dtheader = new DataTable();
                        dtheader.Columns.Add("MenuDocumentId", typeof(string));
                        dtheader.Columns.Add("TransType", typeof(string));
                        dtheader.Columns.Add("comp_id", typeof(int));
                        dtheader.Columns.Add("br_id", typeof(int));
                        dtheader.Columns.Add("mrs_no", typeof(string));
                        dtheader.Columns.Add("mrs_dt", typeof(DateTime));
                        dtheader.Columns.Add("mrs_type", typeof(string));
                        dtheader.Columns.Add("req_area", typeof(int));
                        dtheader.Columns.Add("issue_to", typeof(int));
                        dtheader.Columns.Add("mrs_rem", typeof(string));
                        dtheader.Columns.Add("create_id", typeof(int));
                        dtheader.Columns.Add("mod_id", typeof(int));
                        dtheader.Columns.Add("mrs_status", typeof(string));
                        dtheader.Columns.Add("UserMacaddress", typeof(string));
                        dtheader.Columns.Add("UserSystemName", typeof(string));
                        dtheader.Columns.Add("UserIP", typeof(string));
                        dtheader.Columns.Add("entity_type", typeof(string));
                        dtheader.Columns.Add("src_doc_no", typeof(string));
                        dtheader.Columns.Add("src_doc_dt", typeof(string));
                        dtheader.Columns.Add("src_doc_id", typeof(string));
                        dtheader.Columns.Add("src_type", typeof(string));

                        DataRow dtrowHeader = dtheader.NewRow();
                        dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                        //dtrowHeader["TransType"] = Session["TransType"].ToString();
                        if (_MRSModel.mrs_no != null)
                        {
                            dtrowHeader["TransType"] = "Update";
                        }
                        else
                        {
                            dtrowHeader["TransType"] = "Save";
                        }

                        dtrowHeader["comp_id"] = Session["CompId"].ToString();
                        dtrowHeader["br_id"] = Session["BranchId"].ToString();
                        dtrowHeader["mrs_no"] = _MRSModel.mrs_no;
                        dtrowHeader["mrs_dt"] = _MRSModel.mrs_dt;
                        dtrowHeader["mrs_type"] = _MRSModel.mrs_type;
                        dtrowHeader["req_area"] = _MRSModel.req_area;
                        if (_MRSModel.mrs_type == "I" && _MRSModel.src_type == "O")
                        {

                            dtrowHeader["src_doc_no"] = _MRSModel.Pro_order_Num;
                            dtrowHeader["src_doc_dt"] = _MRSModel.Pro_order_dt;
                            dtrowHeader["src_doc_id"] = "105105125";
                            dtrowHeader["src_type"] = _MRSModel.src_type;
                        }
                        else
                        {
                            if (_MRSModel.mrs_type == "I" && _MRSModel.src_type == "D")
                            {
                                dtrowHeader["src_doc_no"] = null;
                                dtrowHeader["src_doc_dt"] = null;
                                dtrowHeader["src_doc_id"] = null;
                                dtrowHeader["src_type"] = _MRSModel.src_type;
                            }
                            else
                            {
                                dtrowHeader["src_doc_no"] = null;
                                dtrowHeader["src_doc_dt"] = null;
                                dtrowHeader["src_doc_id"] = null;
                                dtrowHeader["src_type"] = null;
                            }

                        }
                        if (_MRSModel.issue_to != null && _MRSModel.issue_to != "" && _MRSModel.mrs_type != "O")
                        {
                            dtrowHeader["issue_to"] = Convert.ToInt32(_MRSModel.issue_to.ToString().Replace("C", "").Replace("S", "").Replace("E", "").Trim());
                        }
                        else
                        {
                            dtrowHeader["issue_to"] = 0;
                        }
                        dtrowHeader["mrs_rem"] = _MRSModel.mrs_rem;
                        dtrowHeader["create_id"] = Session["UserId"].ToString();
                        dtrowHeader["mod_id"] = Session["UserId"].ToString();
                        //dtrowHeader["mrs_status"] = Session["AppStatus"].ToString();
                        dtrowHeader["mrs_status"] = IsNull(_MRSModel.AppStatus, "D");
                        dtrowHeader["UserMacaddress"] = Session["UserMacaddress"].ToString();
                        dtrowHeader["UserSystemName"] = Session["UserSystemName"].ToString();
                        dtrowHeader["UserIP"] = Session["UserIP"].ToString();
                        dtrowHeader["entity_type"] = _MRSModel.entity_type;

                        dtheader.Rows.Add(dtrowHeader);
                        MRSHeader = dtheader;


                        DataTable dtItem = new DataTable();
                        dtItem.Columns.Add("comp_id", typeof(int));
                        dtItem.Columns.Add("br_id", typeof(int));
                        dtItem.Columns.Add("mrs_no", typeof(string));
                        dtItem.Columns.Add("mrs_dt", typeof(DateTime));
                        dtItem.Columns.Add("mrs_type", typeof(string));
                        dtItem.Columns.Add("item_id", typeof(string));
                        dtItem.Columns.Add("uom_id", typeof(int));
                        dtItem.Columns.Add("mrs_qty", typeof(string));
                        //dtItem.Columns.Add("issue_qty", typeof(float));                   
                        dtItem.Columns.Add("it_remarks", typeof(string));

                        JArray jObject = JArray.Parse(_MRSModel.Itemdetails);

                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["comp_id"] = Session["CompId"].ToString();
                            dtrowLines["br_id"] = Session["BranchId"].ToString();
                            dtrowLines["mrs_no"] = _MRSModel.mrs_no;
                            //dtrowLines["mrs_dt"] = DateTime.Now;
                            dtrowLines["mrs_dt"] = _MRSModel.mrs_dt;
                            dtrowLines["mrs_type"] = _MRSModel.mrs_type;
                            dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                            dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                            dtrowLines["mrs_qty"] = jObject[i]["RequQty"].ToString();
                            //dtrowLines["issue_qty"] = jObject[i]["IssueQty"].ToString();                       
                            dtrowLines["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                            dtItem.Rows.Add(dtrowLines);
                        }
                        MRSItemDetails = dtItem;

                        //DataTable dtAttachment = new DataTable();
                        //dtAttachment.Columns.Add("comp_id", typeof(int));
                        //dtAttachment.Columns.Add("br_id", typeof(int));
                        //dtAttachment.Columns.Add("mrs_no", typeof(string));
                        //dtAttachment.Columns.Add("mrs_dt", typeof(DateTime));
                        //dtAttachment.Columns.Add("mrs_type", typeof(string));
                        //dtAttachment.Columns.Add("doc_path", typeof(string));

                        //if (MRSFiles.Length > 0)
                        //{
                        //    string[] filePaths;
                        //    String NextDocumentNumber = getNextDocumentNumber();

                        //    string AttachmentFilePath = Server.MapPath("~/Attachment/MRS/");
                        //    if (Directory.Exists(AttachmentFilePath))
                        //    {
                        //        if (_MRSModel.mrs_no != null)
                        //        {
                        //            filePaths = Directory.GetFiles(AttachmentFilePath, CompID + _MRSModel.mrs_no.Replace("/", "") + "-*");
                        //        }
                        //        else
                        //        {
                        //            filePaths = Directory.GetFiles(AttachmentFilePath, CompID + NextDocumentNumber.Replace("/", "") + "-*");
                        //        }
                        //        foreach (var fielpath in filePaths)
                        //        {
                        //            System.IO.File.Delete(fielpath);
                        //        }
                        //    }

                        //    foreach (HttpPostedFileBase file in MRSFiles)
                        //    {
                        //        if (file != null)
                        //        {
                        //            string str;
                        //            if (_MRSModel.mrs_no != null)
                        //            {
                        //                str = _MRSModel.mrs_no.Replace("/", "");
                        //            }
                        //            else
                        //            {
                        //                str = NextDocumentNumber.Replace("/", "");
                        //            }
                        //            string img_nm = CompID + str + "-" + Path.GetFileName(file.FileName).ToString();
                        //            string doc_path = Path.Combine(Server.MapPath("~/Attachment/MRS/"), img_nm);
                        //            string DocumentPath = Server.MapPath("~/Attachment/MRS/");
                        //            string br_id = Session["BranchId"].ToString();
                        //            string mrs_type = _MRSModel.mrs_type;
                        //            string mrs_no = _MRSModel.mrs_no;
                        //            DateTime mrs_dt = _MRSModel.mrs_dt
                        //                ;
                        //            if (!Directory.Exists(DocumentPath))
                        //            {
                        //                DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                        //            }
                        //            file.SaveAs(doc_path);

                        //            DataRow dtAttachmentrow = dtAttachment.NewRow();
                        //            dtAttachmentrow["comp_id"] = CompID;
                        //            dtAttachmentrow["br_id"] = br_id;
                        //            dtAttachmentrow["mrs_no"] = _MRSModel.mrs_no;
                        //            dtAttachmentrow["mrs_dt"] = DateTime.Now;
                        //            dtAttachmentrow["mrs_type"] = _MRSModel.mrs_type;
                        //            dtAttachmentrow["doc_path"] = doc_path;
                        //            dtAttachment.Rows.Add(dtAttachmentrow);
                        //        }


                        //    }
                        //    MRSAttachments = dtAttachment;
                        //}
                        DataTable dtAttachment = new DataTable();
                        var AttchDataIMG = TempData["AttchData"] as MTSModelAttch;
                        TempData["AttchData"] = null;
                        if (_MRSModel.attatchmentdetail != null)
                        {
                            if (AttchDataIMG != null)
                            {
                                //if (Session["AttachMentDetailItmStp"] != null)
                                if (AttchDataIMG.AttachMentDetailItmStp != null)
                                {
                                    //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                    dtAttachment = AttchDataIMG.AttachMentDetailItmStp as DataTable;
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
                                if (_MRSModel.AttachMentDetailItmStp != null)
                                {
                                    //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                    dtAttachment = _MRSModel.AttachMentDetailItmStp as DataTable;
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
                            JArray jObject1 = JArray.Parse(_MRSModel.attatchmentdetail);
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
                                    if (!string.IsNullOrEmpty(_MRSModel.mrs_no))
                                    {
                                        dtrowAttachment1["id"] = _MRSModel.mrs_no;
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
                            if (_MRSModel.TransType == "Update")
                            {

                                string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                                if (Directory.Exists(AttachmentFilePath))
                                {
                                    string ItmCode = string.Empty;
                                    if (!string.IsNullOrEmpty(_MRSModel.mrs_no))
                                    {
                                        ItmCode = _MRSModel.mrs_no;
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
                            MRSAttachments = dtAttachment;
                        }
                        /*----------------------Sub Item ----------------------*/
                        DataTable dtSubItem = new DataTable();
                        dtSubItem.Columns.Add("item_id", typeof(string));
                        dtSubItem.Columns.Add("sub_item_id", typeof(string));
                        dtSubItem.Columns.Add("qty", typeof(string));
                        if (_MRSModel.SubItemDetailsDt != null)
                        {
                            JArray jObject2 = JArray.Parse(_MRSModel.SubItemDetailsDt);
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

                        SaveMessage = _materialRequisitionSlip_ISERVICE.InsertUpdateMRS(MRSHeader, MRSItemDetails, MRSAttachments, dtSubItem);
                        string MRSNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                        if (Message == "Data_Not_Found")
                        {
                            var a = MRSNo.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _MRSModel.Message = Message.Replace("_", "");
                            return RedirectToAction("MaterialRequisitionSlipDetail");
                        }
                        if (Message == "Save")
                        {
                            string Guid = "";
                            //if (Session["Guid"] != null)
                            //{
                            //    Guid = Session["Guid"].ToString();
                            //}
                            if (AttchDataIMG != null)
                            {
                                if (AttchDataIMG.Guid != null)
                                {
                                    Guid = AttchDataIMG.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, MRSNo, _MRSModel.TransType, MRSAttachments);
                            //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //if (Directory.Exists(sourcePath))
                            //{
                            //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                            //    foreach (string file in filePaths)
                            //    {
                            //        string[] items = file.Split('\\');
                            //        string ItemName = items[items.Length - 1];
                            //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            //        foreach (DataRow dr in MRSAttachments.Rows)
                            //        {
                            //            string DrItmNm = dr["file_name"].ToString();
                            //            if (ItemName == DrItmNm)
                            //            {
                            //                string MRSNo1 = MRSNo.Replace("/", "");
                            //                string img_nm = CompID + BrchID + MRSNo1 + "_" + Path.GetFileName(DrItmNm).ToString();
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

                        if (Message == "Update" || Message == "Save")
                        {
                            //Session["Message"] = "Save";
                            //Session["Command"] = "Update";
                            //Session["MRSNo"] = MRSNo;
                            //Session["TransType"] = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnSave";
                            //Session["AttachMentDetailItmStp"] = null;
                            //Session["Guid"] = null;
                            _MRSModel.Message = "Save";
                            _MRSModel.Command = "Update";
                            _MRSModel.MRSNo = MRSNo;
                            _MRSModel.TransType = "Update";
                            _MRSModel.AppStatus = "D";
                            _MRSModel.BtnName = "BtnSave";
                            _MRSModel.AttachMentDetailItmStp = null;
                            _MRSModel.Guid = null;
                            TempData["ModelData"] = _MRSModel;
                        }
                        return RedirectToAction("MaterialRequisitionSlipDetail");

                    }
                    else
                    {
                        if (Session["compid"] != null)
                        {
                            CompID = Session["compid"].ToString();
                        }
                        if (Session["userid"] != null)
                        {
                            userid = Session["userid"].ToString();
                        }

                        string br_id = Session["BranchId"].ToString();
                        _MRSModel.CreatedBy = userid;
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        SaveMessage = _materialRequisitionSlip_ISERVICE.MRSCancel(_MRSModel, CompID, br_id, mac_id);
                        string MRSNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        try
                        {
                            string fileName = "MaterialRequisitionSlip_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_MRSModel.mrs_no, _MRSModel.mrs_dt.ToString(), fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MRSModel.mrs_no, "C", userid, "", filePath);
                        }

                        catch (Exception exMail)
                        {
                            _MRSModel.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        //Session["Message"] = "Cancelled";
                        //Session["Command"] = "Update";
                        //TempData["MRSNo"] = _MRSModel.mrs_no;
                        //TempData["MRSDate"] = _MRSModel.mrs_dt;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";
                        _MRSModel.MRSNo = _MRSModel.mrs_no;
                        _MRSModel.MRSDate = _MRSModel.mrs_dt;
                        // _MRSModel.Message = "Cancelled";
                        _MRSModel.Message = _MRSModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        _MRSModel.Command = "Update";
                        _MRSModel.TransType = "Update";
                        _MRSModel.AppStatus = "D";
                        _MRSModel.BtnName = "Refresh";
                        TempData["ModelData"] = _MRSModel;
                        return RedirectToAction("MaterialRequisitionSlipDetail");


                    }
                }
                //return RedirectToAction("MaterialRequisitionSlipDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    //{
                    //    string Guid = "";
                    //    if (Session["Guid"] != null)
                    //    {
                    //        Guid = Session["Guid"].ToString();
                    //    }
                    if (_MRSModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_MRSModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _MRSModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                MTSModelAttch _MTSModelAttch = new MTSModelAttch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                // string TransType = "";
                //string mrs_no = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["MRSNo"] != null)
                //{
                //    mrs_no = Session["MRSNo"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = mrs_no;
                _MTSModelAttch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _MTSModelAttch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _MTSModelAttch.AttachMentDetailItmStp = null;
                }
                TempData["AttchData"] = _MTSModelAttch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }

        private ActionResult MRSDelete(MRSModel _MRSModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string MRS = _MRSModel.mrs_no;
                DataSet Message = _materialRequisitionSlip_ISERVICE.MRSDelete(_MRSModel, CompID, br_id, MRS);
                if (!string.IsNullOrEmpty(MRS))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string MRS1 = MRS.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, MRS1, Server);
                }

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["MRSNo"] = "";
                //_MRSModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                _MRSModel = null;
                MRSModel _MRSDeleteModel = new MRSModel();
                _MRSDeleteModel.Message = "Deleted";
                _MRSDeleteModel.Command = "Refresh";
                _MRSDeleteModel.TransType = "Refresh";
                _MRSDeleteModel.AppStatus = "DL";
                _MRSDeleteModel.BtnName = "BtnDelete";
                return RedirectToAction("MaterialRequisitionSlipDetail", "MaterialRequisitionSlip");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        //[NonAction]
        public ActionResult MRSApprove(string MRS_no, DateTime MRS_date, string ReqType, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1, string docid)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string br_id = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    DocumentMenuId = Session["MenuDocumentId"].ToString();
                //}
                if (!string.IsNullOrEmpty(docid))
                {
                    DocumentMenuId = docid;
                }
                MRSModel _MRSModel = new MRSModel();
                _MRSModel.CreatedBy = Session["UserId"].ToString();
                _MRSModel.mrs_no = MRS_no;
                _MRSModel.mrs_dt = MRS_date;
                _MRSModel.mrs_type = ReqType;
                //string app_id = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _materialRequisitionSlip_ISERVICE.MRSApprove(_MRSModel, CompID, br_id, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                string ApMessage = Message.Split(',')[0].Trim();
                if (ApMessage == "A")
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string folderPath = path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\");
                    if (!Directory.Exists(folderPath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(folderPath);
                    }
                    try
                    {
                        //string fileName = "MRS_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "MaterialRequisitionSlip_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_MRSModel.mrs_no, MRS_date.ToString(), fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, MRS_no, "AP", UserID, "", filePath);
                    }

                    catch (Exception exMail)
                    {
                        _MRSModel.Message = "ErrorInMail";
                        string _path = Server.MapPath("~");
                        Errorlog.LogError(_path, exMail);
                    }
                    //Session["Message"] = "Approved";
                    _MRSModel.Message = "Approved";
                    _MRSModel.Message = _MRSModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approved";
                //Session["PriceListNo"] = _MRSModel.mrs_no;
                ////Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                _MRSModel.MRSNo = MRS_no;
                _MRSModel.MRSDate = MRS_date;
                _MRSModel.Message = "Approved";
                _MRSModel.Command = "Approve";
                _MRSModel.TransType = "Update";
                _MRSModel.AppStatus = "D";
                _MRSModel.BtnName = "BtnEdit";
                _MRSModel.WF_status1 = WF_Status1;
                // TempData["WF_Status1"] = WF_Status1;
                TempData["ModelData"] = _MRSModel;
                UrlModel _approve_Model = new UrlModel();
                _approve_Model.MRSNo = _MRSModel.MRSNo;
                _approve_Model.WF_status1 = _MRSModel.WF_status1;
                _approve_Model.MRSDate = _MRSModel.MRSDate;
                _approve_Model.TransType = "Update";
                _approve_Model.BtnName = "BtnToDetailPage";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("MaterialRequisitionSlipDetail", "MaterialRequisitionSlip", _approve_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");\
                throw ex;
            }
        }
        private string CheckInternalIssue(string MRS_no, string MRS_date)/*Add By Hina on 24-09-2024 to use on internal issue*/
        {
            try
            {
                //JsonResult DataRows = null;
                // DataTable ds = new DataTable();
                string CompID = string.Empty;
                string br_id = string.Empty;
                string result = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                dt = _materialRequisitionSlip_ISERVICE.CheckInternalIssue(CompID, br_id, MRS_no, MRS_date).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    result = "Used";
                }
                else
                {
                    result = null;
                }
                // DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetAutoCompleteSearchEntityList(MRSList_Model _MRSList_Model)
        {
            string Entity = string.Empty;
            Dictionary<string, string> EntityList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_MRSList_Model.ddlissueto))
                {
                    Entity = "0";
                }
                else
                {
                    Entity = _MRSList_Model.ddlissueto;
                }
                EntityList = _materialRequisitionSlip_ISERVICE.EntityList(Comp_ID, Entity, BrchID, "");


                List<IssueIDList> _IssueIDList = new List<IssueIDList>();
                foreach (var data in EntityList)
                {
                    IssueIDList _IssueID = new IssueIDList();
                    _IssueID.issue_id = data.Key;
                    _IssueID.issue_val = data.Value;
                    _IssueIDList.Add(_IssueID);
                }
                _MRSList_Model.IssueList = _IssueIDList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return RedirectToAction("MaterialRequisitionSlipList", "MaterialRequisitionSlip");

            //return Json(EntityList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteSearchEntity(MRSModel _MRSModel)
        {
            string Entity = string.Empty;
            string sr_type = string.Empty;
            Dictionary<string, string> EntityList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_MRSModel.ddlissueto))
                {
                    Entity = "0";
                }
                else
                {
                    Entity = _MRSModel.ddlissueto;
                }
                if (string.IsNullOrEmpty(_MRSModel.sr_type))
                {
                    sr_type = "0";
                }
                else
                {
                    sr_type = _MRSModel.sr_type;
                }
                EntityList = _materialRequisitionSlip_ISERVICE.EntityList(Comp_ID, Entity, BrchID, sr_type);


                List<IssueID> _IssueIDList = new List<IssueID>();
                foreach (var data in EntityList)
                {
                    IssueID _IssueID = new IssueID();
                    _IssueID.issue_id = data.Key;
                    _IssueID.issue_val = data.Value;

                    _IssueIDList.Add(_IssueID);
                }
                _MRSModel.IssueList = _IssueIDList;
                return Json(EntityList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }
        private string getNextDocumentNumber()
        {
            try
            {
                string MenuDocumentId = "105102108";
                string CompId = Session["CompId"].ToString();
                string BranchId = Session["BranchId"].ToString();
                string Prefix = "MRS";
                string NextDocumentNumber = _materialRequisitionSlip_ISERVICE.getNextDocumentNumber(CompID, BranchId, MenuDocumentId, Prefix);
                return NextDocumentNumber;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        [NonAction]
        private DataTable GetRequirmentreaList(string flag)
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataTable dt = _materialRequisitionSlip_ISERVICE.GetRequirmentreaList(CompID, BrchID, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public string getReqAreaByRequisitionType(string flag)
        {
            try
            {
                DataTable dtreq = GetRequirmentreaList(flag);
                return JsonConvert.SerializeObject(dtreq);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        //public void GetStatusList(MRSList_Model _MRSList_Model)
        //{
        //    try
        //    {
        //        List<Status> statusLists = new List<Status>();
        //        var other = new CommonController(_Common_IServices);
        //        var statusListsC = other.GetStatusList1(DocumentMenuId);
        //        var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
        //        _MRSList_Model.StatusList = listOfStatus;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //    }
        //}
        private List<MRSList> GetMRSDetailList(MRSList_Model _MRSList_Model, string itemID)
        {
            try
            {
                _MRSList = new List<MRSList>();

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
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                string wfstatus = "";
                if (_MRSList_Model.WF_status != null)
                {
                    wfstatus = _MRSList_Model.WF_status;
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
                DataSet dt = new DataSet();
                dt = _MRSList_ISERVICES.GetMRSDetailList(CompID, BrchID, _MRSList_Model.req_area, _MRSList_Model.issue_to, _MRSList_Model.entity_type,
                    _MRSList_Model.MRS_FromDate, _MRSList_Model.MRS_ToDate, _MRSList_Model.MRS_Type, _MRSList_Model.SRC_Type, _MRSList_Model.MRSStatus, UserID, wfstatus, DocumentMenuId, language, itemID);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        MRSList _TempMRSList = new MRSList();
                        _TempMRSList.MRSNo = dr["mrs_no"].ToString();
                        _TempMRSList.MRSDate = dr["MrsDate"].ToString();
                        _TempMRSList.MRS_Dt = dr["Mrs_Date"].ToString();
                        _TempMRSList.MRSType = dr["MRSType"].ToString();
                        _TempMRSList.ReqArea = dr["reqArea"].ToString();
                        if (dr["src_type"].ToString() == "Direct" || dr["src_type"].ToString() == "Production Order")
                        {
                            _TempMRSList.SrcType = dr["src_type"].ToString();
                        }
                        else
                        {
                            _TempMRSList.SrcType = dr["SrcDocName"].ToString();
                        }
                        //_TempMRSList.SrcType = dr["SrcDocName"].ToString();
                        _TempMRSList.SrcDocNo = dr["src_doc_no"].ToString();
                        _TempMRSList.SrcDocDate = dr["SrcDocDate"].ToString();
                        _TempMRSList.SrcDocDt = dr["SrcDocDt"].ToString();
                        _TempMRSList.IssueTo = dr["Entity"].ToString();
                        _TempMRSList.MRSList_Stauts = dr["Status"].ToString();
                        _TempMRSList.CreateDate = dr["CreateDate"].ToString();
                        _TempMRSList.ApproveDate = dr["ApproveDate"].ToString();
                        _TempMRSList.ModifyDate = dr["ModifyDate"].ToString();
                        _TempMRSList.create_by = dr["create_by"].ToString();
                        _TempMRSList.app_by = dr["app_by"].ToString();
                        _TempMRSList.mod_by = dr["mod_by"].ToString();
                        _MRSList.Add(_TempMRSList);
                    }
                }

                return _MRSList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult SearchMRSDetail(int req_area, string issue_to, string EntityType, string Fromdate, string Todate,
            string MRS_Type, string SRC_Type, string Status, string ItemID)
        {
            try
            {

                _MRSList = new List<MRSList>();
                MRSList_Model _MRSList_Model = new MRSList_Model();
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
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                // Session["WF_status"] = null;
                _MRSList_Model.WF_status = null;
                DataSet dt = new DataSet();
                dt = _MRSList_ISERVICES.GetMRSDetailList(CompID, BrchID, req_area, issue_to, EntityType, Fromdate, Todate, MRS_Type, SRC_Type, Status, UserID, "", DocumentMenuId, language, ItemID);
                //Session["MRSSearch"] = "MRS_Search";
                _MRSList_Model.MRSSearch = "MRS_Search";
                if (dt.Tables[0].Rows.Count > 0)
                {


                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        MRSList _TempMRSList = new MRSList();
                        _TempMRSList.MRSNo = dr["mrs_no"].ToString();
                        _TempMRSList.MRSDate = dr["MrsDate"].ToString();
                        _TempMRSList.MRS_Dt = dr["Mrs_Date"].ToString();
                        _TempMRSList.MRSType = dr["MRSType"].ToString();
                        _TempMRSList.ReqArea = dr["reqArea"].ToString();
                        if (dr["src_type"].ToString() == "Direct" || dr["src_type"].ToString() == "Production Order")
                        {
                            _TempMRSList.SrcType = dr["src_type"].ToString();
                        }
                        else
                        {
                            _TempMRSList.SrcType = dr["SrcDocName"].ToString();
                        }
                        // _TempMRSList.SrcType = dr["SrcDocName"].ToString();
                        _TempMRSList.SrcDocNo = dr["src_doc_no"].ToString();
                        _TempMRSList.SrcDocDate = dr["SrcDocDate"].ToString();
                        _TempMRSList.SrcDocDt = dr["SrcDocDt"].ToString();
                        _TempMRSList.IssueTo = dr["Entity"].ToString();
                        _TempMRSList.MRSList_Stauts = dr["Status"].ToString();
                        _TempMRSList.CreateDate = dr["CreateDate"].ToString();
                        _TempMRSList.ApproveDate = dr["ApproveDate"].ToString();
                        _TempMRSList.ModifyDate = dr["ModifyDate"].ToString();
                        _TempMRSList.create_by = dr["create_by"].ToString();
                        _TempMRSList.app_by = dr["app_by"].ToString();
                        _TempMRSList.mod_by = dr["mod_by"].ToString();
                        _MRSList.Add(_TempMRSList);
                    }
                }
                _MRSList_Model.BindMRSList = _MRSList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMRSList.cshtml", _MRSList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult ToRefreshByJS(string ListFilterData1, string dashbordData,string MailError)
        {
            //Session["Message"] = "";
            MRSModel _mrsmodel = new MRSModel();
            _mrsmodel.Message = null;
            var a = dashbordData.Split(',');
            _mrsmodel.docid = a[0].Trim();
            _mrsmodel.MRSNo = a[1].Trim();
            _mrsmodel.MRSDate = Convert.ToDateTime(a[2].Trim());
            _mrsmodel.WF_status1 = a[3].Trim();
            _mrsmodel.TransType = "Update";
            _mrsmodel.BtnName = "BtnToDetailPage";
            _mrsmodel.Message = MailError;
            _mrsmodel.Message = null;
            TempData["ModelData"] = _mrsmodel;
            UrlModel _UrlModel = new UrlModel();
            _UrlModel.MRSNo = _mrsmodel.MRSNo;
            //_UrlModel.docid =  _mrsmodel.docid;
            _UrlModel.WF_status1 = _mrsmodel.WF_status1;
            _UrlModel.MRSDate = _mrsmodel.MRSDate;
            _UrlModel.TransType = "Update";
            _UrlModel.BtnName = "BtnToDetailPage";
            //_UrlModel.Message = null;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("MaterialRequisitionSlipDetail", _UrlModel);
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

        public ActionResult GetMTSDetailDashbrd(string docid, string status)
        {
            MRSList_Model _MRSListDash_Model = new MRSList_Model();
            // Session["WF_status"] = status;
            _MRSListDash_Model.WF_status = status;

            return RedirectToAction("MaterialRequisitionSlipList", _MRSListDash_Model);
        }

        /*--------------------------For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(MRSModel _model)
        {
            return File(GetPdfData(_model.mrs_no, _model.mrs_dt), "application/pdf", "MaterialRequisitionSlip.pdf");
        }
        public byte[] GetPdfData(string mrsNo, DateTime mrsDate)
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

                DataSet Details = _materialRequisitionSlip_ISERVICE.GetMRSDeatilsForPrint(CompID, BrchID, mrsNo, Convert.ToDateTime(mrsDate).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Title = "Material Requisition Slip";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["mrs_status"].ToString().Trim();
                //DataSet Details = _LSODetail_ISERVICE.GetSalesOrderDeatilsForPrint(CompID, Br_ID, _model.SO_no, _model.SO_dt);
                //ViewBag.PageName = "SO";
                //ViewBag.Title = "Sales Order";
                //ViewBag.Details = Details;
                //ViewBag.InvoiceTo = "";
                //ViewBag.ApprovedBy = "Arvind Gupta";
                //ViewBag.DocStatus = Details.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialRequisitionSlip/MaterialRequisitionSlipPrint.cshtml"));
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

        /*--------------------------For PDF Print End--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
            , string Flag, string Status, string Doc_no, string Doc_dt, string SrcTyp, string srcdocno, string srcdocdt, string RequiredArea)
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
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        if (SrcTyp == "O")
                        {
                            if (Status == "")
                            {
                                dt = _materialRequisitionSlip_ISERVICE.GetSubItemDetailsFromPrductnOrd(CompID, BrchID, srcdocno, srcdocdt, RequiredArea, Item_id).Tables[0];
                            }
                            else
                            {
                                dt = _materialRequisitionSlip_ISERVICE.MRS_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                            }

                        }
                        else
                        {
                            dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        }

                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                                else
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(dt.Rows[i]["Qty"].ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _materialRequisitionSlip_ISERVICE.MRS_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                }
                else if (Flag == "Issue")
                {
                    dt = _materialRequisitionSlip_ISERVICE.MRS_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag == "Quantity" ? Flag : "MRS",
                    _subitemPageName = "MRS",
                    dt_SubItemDetails = dt,
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

        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
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
            try
            {
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Convert.ToDateTime(Doc_dt));
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
        public JsonResult GetSourceDocList(string RequiredArea, string Req_type)
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
                DataSet Deatils = _materialRequisitionSlip_ISERVICE.GetSourceDocList(Comp_ID, Br_ID, RequiredArea, Req_type);

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
        public ActionResult getMaterialIssuedata(string srcdocno, string srcdocdt, string RequiredArea)
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
                DataSet ds = _materialRequisitionSlip_ISERVICE.getProductionOrderdata(CompID, BrchID, srcdocno, srcdocdt, RequiredArea);
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
        public JsonResult Getoutputitm(string docno, string docdt)
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
                DataSet Deatils = _materialRequisitionSlip_ISERVICE.GetoutputList(Comp_ID, Br_ID, docno, docdt);

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

        public ActionResult GetMRSTrackingDetail(string MRS_No, string MRS_Date)
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
                DataSet result = _materialRequisitionSlip_ISERVICE.GetSOTrackingDetail(Comp_ID, BranchID, MRS_No, MRS_Date);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.SOTrackingList = result.Tables[0];

                return View("~/Areas/ApplicationLayer/Views/Shared/PartialMRSTracking.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult BindReplicateWithlist(MRSModel MRSModel)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    if (MRSModel.Search == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = MRSModel.Search;
                    }
                    DataSet ProductList = _materialRequisitionSlip_ISERVICE.getReplicateWith(CompID, BrchID, MRSModel.mrs_type, MRSModel.req_area.ToString(), SarchValue);
                    if (ProductList.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[0].Rows.Count; i++)
                        {
                            string mrs_no = ProductList.Tables[0].Rows[i]["mrs_no"].ToString();
                            string mrs_dt = ProductList.Tables[0].Rows[i]["mrs_dt"].ToString();
                            string req_area = ProductList.Tables[0].Rows[i]["req_area"].ToString();
                            string setup_val = ProductList.Tables[0].Rows[i]["setup_val"].ToString();
                            ItemList.Add(mrs_no + "," + mrs_dt+ "," + req_area, setup_val);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReplicateWithMRSNumber(string mrs_no, string mrs_dt)
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
                DataSet result = _materialRequisitionSlip_ISERVICE.GetReplicateWithItemdata(CompID, BrchID, mrs_no, mrs_dt);
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