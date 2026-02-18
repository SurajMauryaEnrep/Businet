using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Globalization;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialIssue
{
    public class MaterialIssueController : Controller
    {

        //string DocumentMenuId = "105102130101";
        string DocumentMenuId = string.Empty;
        string FromDate, title, TransType;
        // GET: ApplicationLayer/MaterialIssue
        MaterialIssue_Model _MaterialIssue_Model;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        MaterialIssue_IServices _MaterialIssue_IServices;
        Common_IServices _Common_IServices;
        CommonController cmn = new CommonController();
        string CompID, BrchID, ship_no, userid, language = String.Empty;
        public MaterialIssueController(MaterialIssue_IServices _MaterialIssue_IServices, Common_IServices _Common_IServices)
        {
            this._MaterialIssue_IServices = _MaterialIssue_IServices;
            this._Common_IServices = _Common_IServices;
        }
        /*Commented by Hina on 19-03-2024 to add CommonPageDetails()*/
        //private string getDocumentName() 
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["Language"] != null)
        //        {
        //            language = Session["Language"].ToString();
        //        }
        //        string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //        string[] Docpart = DocumentName.Split('>');
        //        int len = Docpart.Length;
        //        if (len > 1)
        //        {
        //            title = Docpart[len - 1].Trim();
        //        }
        //        return DocumentName;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
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
                userid = Session["UserId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, userid, DocumentMenuId, language);
            ViewBag.AppLevel = ds.Tables[0];
            ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
            string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
            ViewBag.VBRoleList = ds.Tables[3];
            ViewBag.StatusList = ds.Tables[4];
            //ViewBag.PackSerialization = ds.Tables[6].Rows.Count > 0 ? ds.Tables[6].Rows[0]["param_stat"].ToString() : "";
            string[] Docpart = DocumentName.Split('>');
            int len = Docpart.Length;
            if (len > 1)
            {
                title = Docpart[len - 1].Trim();
            }
            ViewBag.MenuPageName = DocumentName;
        }
        public ActionResult MaterialIssueList(string reqType, MaterialIssueList _MaterialIssueList)
        {
            try
            {
                //string reqType = "";
                if (reqType == "I")
                {
                    DocumentMenuId = "105102130101";
                    ViewBag.DocumentMenuId = "105102130101";
                }
                if (reqType == "E")
                {
                    DocumentMenuId = "105102130105";
                    ViewBag.DocumentMenuId = "105102130105";
                }
                if (reqType == "S")
                {
                    DocumentMenuId = "105102130108";
                    ViewBag.DocumentMenuId = "105102130108";
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                // MaterialIssueList _MaterialIssueList = new MaterialIssueList();
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                _MaterialIssue_Model = new MaterialIssue_Model();
                _MaterialIssueList.DocumentMenuId = DocumentMenuId;
                CommonPageDetails();
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_code = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _MaterialIssueList.StatusList = statusLists;
                //List<Status> statusLists = new List<Status>();
                //var other = new CommonController(_Common_IServices);
                //var statusListsC = other.GetStatusList1(DocumentMenuId);
                //var listOfStatus = statusListsC.ConvertAll(x => new Status { status_code = x.status_id, status_name = x.status_name });
                //_MaterialIssueList.StatusList = listOfStatus;
                /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                //_MaterialIssueList.MaterialIssueDetailList = GetMaterialIssueAllDetail(_MaterialIssueList, reqType);
                //DataSet ds = _MaterialIssue_IServices.GetRequirmentreaList(CompID, BrchID, reqType);
                //List<RequiredArea> RequiredAreaList = new List<RequiredArea>();
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{

                //    RequiredArea _RequiredAreaList = new RequiredArea();
                //    _RequiredAreaList.ReqArea_ID = Convert.ToInt32(dr["setup_id"]);
                //    _RequiredAreaList.ReqArea_Name = dr["setup_val"].ToString();
                //    RequiredAreaList.Add(_RequiredAreaList);
                //}

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                DataSet ds = _MaterialIssue_IServices.GetAllDDLandListPageData(CompID, BrchID, reqType, startDate, CurrentDate);
                List<RequiredArea> RequiredAreaList = new List<RequiredArea>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    RequiredArea _RequiredAreaList = new RequiredArea();
                    _RequiredAreaList.ReqArea_ID = Convert.ToInt32(dr["setup_id"]);
                    _RequiredAreaList.ReqArea_Name = dr["setup_val"].ToString();
                    RequiredAreaList.Add(_RequiredAreaList);
                }
                RequiredArea _RequiredAreaList2 = new RequiredArea();
                _RequiredAreaList2.ReqArea_ID = 0;
                _RequiredAreaList2.ReqArea_Name = "---Select---";
                RequiredAreaList.Insert(0, _RequiredAreaList2);
                _MaterialIssueList.RequiredAreaList = RequiredAreaList;

                List<EntityType> _EntityTypeList = new List<EntityType>();
                EntityType _EntityType2 = new EntityType();
                _EntityType2.Entity_ID = "0";
                _EntityType2.Entity_Name = "---Select---";
                _EntityType2.Entity_Type = "0";
                _EntityTypeList.Insert(0, _EntityType2);
                _MaterialIssueList.EntityTypelist = _EntityTypeList;
                //ViewBag.MenuPageName = getDocumentName();

                _MaterialIssueList.Title = title;
                //ds = GetMaterialIssueToList();
                List<MaterialIssueTo> _MaterialIssueToList = new List<MaterialIssueTo>();
                //foreach (DataRow dr in ds.Tables[0].Rows)
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    MaterialIssueTo _MaterialIssueTo = new MaterialIssueTo();
                    _MaterialIssueTo.issue_to_id = dr["issue_to_id"].ToString(); ;
                    _MaterialIssueTo.issue_to_name = dr["issue_to_name"].ToString();
                    _MaterialIssueToList.Add(_MaterialIssueTo);
                }
                MaterialIssueTo _MaterialIssueTo2 = new MaterialIssueTo();
                _MaterialIssueTo2.issue_to_id = "0";
                _MaterialIssueTo2.issue_to_name = "---Select---";
                _MaterialIssueToList.Insert(0, _MaterialIssueTo2);
                _MaterialIssueList.MaterialIssueToList = _MaterialIssueToList;

                //DateTime dtnow = DateTime.Now;
                //FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                // FromDate = DetailDatable.Tables[1].Rows[0]["finstrdate"].ToString();


                List<MaterialIssueDetail> _MaterialIssueDetailList = new List<MaterialIssueDetail>();
                //Session["MISearch"] = "0";
                //RequisitionTyp, RequiredArea, MaterialIssueTo, Fromdate, Todate, Status);

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var RequisitionTyp = a[0].Trim();
                    var RequiredArea = a[1].Trim();
                    var MaterialIssueTo = a[2].Trim();
                    if (MaterialIssueTo != null && MaterialIssueTo != "" && MaterialIssueTo != "0")
                    {
                        for (int i = 0; i <= MaterialIssueTo.Length; i++)
                        {
                            if ((MaterialIssueTo[i] >= 'A') && (MaterialIssueTo[i] <= 'Z'))
                            {
                                MaterialIssueTo = a[2].Trim(MaterialIssueTo[i]);
                            }
                        }
                    }



                    var Fromdate = a[3].Trim();
                    var Todate = a[4].Trim();
                    var Status = a[5].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    var flag = "ListPage";
                    //List<MaterialIssueDetail> _MaterialIssueDetailList = new List<MaterialIssueDetail>();
                    DataTable DetailDatable = _MaterialIssue_IServices.GetMaterialIssueDetailByFilter(CompID, BrchID, RequisitionTyp, RequiredArea, MaterialIssueTo, Fromdate, Todate, Status, flag);
                    _MaterialIssueList.MRS_type = RequisitionTyp;
                    _MaterialIssueList.RequiredArea = RequiredArea;
                    _MaterialIssueList.MaterialIssueTo = MaterialIssueTo;
                    //MaterialIssueTo _MaterialIssueTo = new MaterialIssueTo();
                    //_MaterialIssueTo.issue_to_id = _MaterialIssueList.MaterialIssueTo;
                    //_MaterialIssueTo.issue_to_name = _MaterialIssueList.hdnisueto;
                    //_MaterialIssueToList.Add(_MaterialIssueTo);

                    // _MaterialIssueList.MaterialIssueToList = _MaterialIssueToList;

                    _MaterialIssueList.FromDate = Fromdate;
                    _MaterialIssueList.ToDate = Todate;
                    _MaterialIssueList.StatusCode = Status;
                    _MaterialIssueList.ListFilterData = TempData["ListFilterData"].ToString();
                    if (DetailDatable.Rows.Count > 0)
                    {

                        foreach (DataRow dr in DetailDatable.Rows)
                        {
                            MaterialIssueDetail _MaterialIssueDetail = new MaterialIssueDetail();
                            _MaterialIssueDetail.issue_type = dr["issue_type"].ToString();
                            _MaterialIssueDetail.issuetype = dr["IssueType"].ToString().Trim();
                            _MaterialIssueDetail.issue_no = dr["issue_no"].ToString();
                            _MaterialIssueDetail.issue_dt = dr["issue_dt"].ToString();
                            _MaterialIssueDetail.issue_date = dr["issue_date"].ToString().Trim();
                            _MaterialIssueDetail.issue_to = dr["issue_to"].ToString();
                            _MaterialIssueDetail.issue_by = dr["issue_by"].ToString();
                            _MaterialIssueDetail.requisition_no = dr["mrs_no"].ToString();
                            _MaterialIssueDetail.requisition_date = dr["mrs_dt"].ToString();
                            _MaterialIssueDetail.entity_type = dr["ReqArea"].ToString();
                            _MaterialIssueDetail.app_dt = dr["app_dt"].ToString();
                            _MaterialIssueDetail.create_dt = dr["create_dt"].ToString();
                            _MaterialIssueDetail.mod_dt = dr["mod_dt"].ToString();
                            _MaterialIssueDetail.issue_status = dr["issue_status"].ToString();
                            _MaterialIssueDetail.create_by = dr["create_by"].ToString();
                            _MaterialIssueDetail.mod_by = dr["mod_by"].ToString();
                            _MaterialIssueDetailList.Add(_MaterialIssueDetail);
                        }
                    }
                    _MaterialIssueList.MaterialIssueDetailList = _MaterialIssueDetailList;
                    _MaterialIssueList.ListFilterData = TempData["ListFilterData"].ToString();
                }
                else
                {
                    //List<MaterialIssueDetail> _MaterialIssueDetailList = new List<MaterialIssueDetail>();
                    if (ds.Tables[2].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            MaterialIssueDetail _MaterialIssueDetail = new MaterialIssueDetail();
                            _MaterialIssueDetail.issue_type = dr["issue_type"].ToString();
                            _MaterialIssueDetail.issuetype = dr["IssueType"].ToString().Trim();
                            _MaterialIssueDetail.issue_no = dr["issue_no"].ToString();
                            _MaterialIssueDetail.issue_dt = dr["issue_dt"].ToString();
                            _MaterialIssueDetail.issue_date = dr["issue_date"].ToString().Trim();
                            _MaterialIssueDetail.issue_to = dr["issue_to"].ToString();
                            _MaterialIssueDetail.issue_by = dr["issue_by"].ToString();
                            _MaterialIssueDetail.requisition_no = dr["mrs_no"].ToString();
                            _MaterialIssueDetail.requisition_date = dr["mrs_dt"].ToString();
                            _MaterialIssueDetail.entity_type = dr["ReqArea"].ToString();
                            _MaterialIssueDetail.app_dt = dr["app_dt"].ToString();
                            _MaterialIssueDetail.create_dt = dr["create_dt"].ToString();
                            _MaterialIssueDetail.mod_dt = dr["mod_dt"].ToString();
                            _MaterialIssueDetail.issue_status = dr["issue_status"].ToString();
                            _MaterialIssueDetail.create_by = dr["create_by"].ToString();
                            _MaterialIssueDetail.mod_by = dr["mod_by"].ToString();
                            _MaterialIssueDetailList.Add(_MaterialIssueDetail);
                        }
                    }
                    _MaterialIssueList.MaterialIssueDetailList = _MaterialIssueDetailList;
                    _MaterialIssueList.MRS_type = reqType;
                    _MaterialIssueList.FromDate = startDate;
                }


                _MaterialIssueList.DocumentMenuId = DocumentMenuId;
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssueList.cshtml", _MaterialIssueList);
                //return RedirectToAction("MaterialIssueL", "MaterialIssue", new { reqType = reqType });
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        //public ActionResult MaterialIssueDetail(MaterialIssue_Model _model,DataSet Mdtset)
        public ActionResult MaterialIssueDetail(GetModelDetails _model, URLModelDetails _url_Model)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _url_Model.Doc_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _MaterialIssue_Model1 = TempData["ModelData"] as GetModelDetails;
                if (_MaterialIssue_Model1 != null)
                {

                    MaterialIssue_Model _MaterialIssue_Model = new MaterialIssue_Model();

                    //;// _MaterialIssue_Model = _model;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MaterialIssue_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    GetModelDetails _GetModelDetails = new GetModelDetails();
                    _GetModelDetails = _model;
                    if (_GetModelDetails != null && _MaterialIssue_Model1 == null)
                    {
                        _MaterialIssue_Model.BtnName = _GetModelDetails.BtnName;
                        _MaterialIssue_Model.TransType = _GetModelDetails.TransType;
                        _MaterialIssue_Model.MaterialIssueDate = _GetModelDetails.MaterialIssueDate;
                        _MaterialIssue_Model.MaterialIssueNo = _GetModelDetails.MaterialIssueNo;
                        _MaterialIssue_Model.MRS_type = _GetModelDetails.MRS_type;
                        _MaterialIssue_Model._mdlCommand = _GetModelDetails._mdlCommand;
                        _MaterialIssue_Model.Message = _GetModelDetails.Message;
                        //_MaterialIssue_Model.CancelFlag = _GetModelDetails.CancelFlag;
                        //_MaterialIssue_Model.DocumentMenuId = _GetModelDetails.DocumentMenuId;
                    }
                    else
                    {
                        _MaterialIssue_Model.BtnName = _MaterialIssue_Model1.BtnName;
                        _MaterialIssue_Model.TransType = _MaterialIssue_Model1.TransType;
                        _MaterialIssue_Model.MaterialIssueDate = _MaterialIssue_Model1.MaterialIssueDate;
                        _MaterialIssue_Model.MaterialIssueNo = _MaterialIssue_Model1.MaterialIssueNo;
                        _MaterialIssue_Model.MRS_type = _MaterialIssue_Model1.MRS_type;
                        _MaterialIssue_Model._mdlCommand = _MaterialIssue_Model1._mdlCommand;
                        _MaterialIssue_Model.Message = _MaterialIssue_Model1.Message;
                    }
                    //if (TempData["MaterialIssue_Model"] != null)
                    //{
                    //    _MaterialIssue_Model = TempData["MaterialIssue_Model"] as MaterialIssue_Model;
                    //}
                    _MaterialIssue_Model.BtnName = _MaterialIssue_Model.BtnName == null ? "BtnAddNew" : _MaterialIssue_Model.BtnName;
                    //   DocumentMenuId = _MaterialIssue_Model.DocumentMenuId != null? _MaterialIssue_Model.DocumentMenuId : Session["MenuDocumentId"]!=null ?Session["MenuDocumentId"].ToString():"";
                    TransType = _MaterialIssue_Model.TransType;
                    var reqType = _MaterialIssue_Model.MRS_type;
                    if (reqType != null)
                    {
                        if (reqType == "I")
                        {
                            DocumentMenuId = "105102130101";
                            ViewBag.DocumentMenuId = "105102130101";
                        }
                        if (reqType == "E")
                        {
                            DocumentMenuId = "105102130105";
                            ViewBag.DocumentMenuId = "105102130105";
                        }
                        if (reqType == "S")
                        {
                            DocumentMenuId = "105102130108";
                            ViewBag.DocumentMenuId = "105102130108";
                        }
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (DocumentMenuId == "105102130108")
                    {
                        DataSet ds1 = _MaterialIssue_IServices.GetIssuedByData(CompID, BrchID);
                        List<Issuedby> _IssuedbyList = new List<Issuedby>();
                        foreach (DataRow dr in ds1.Tables[0].Rows)
                        {
                            Issuedby _Issuedby = new Issuedby();
                            _Issuedby.Issuedby_id = dr["emp_id"].ToString();
                            _Issuedby.Issuedby_Name = dr["emp_name"].ToString();
                            _IssuedbyList.Add(_Issuedby);

                        }
                        Issuedby _Issuedby2 = new Issuedby();
                        _Issuedby2.Issuedby_id = "0";
                        _Issuedby2.Issuedby_Name = "---Select---";
                        _IssuedbyList.Insert(0, _Issuedby2);
                        _MaterialIssue_Model.IssuedbyList = _IssuedbyList;
                        //_MaterialIssue_Model.IssuedbyList = _IssuedbyList;
                    }
                    else
                    {

                        List<Issuedby> _IssuedbyList = new List<Issuedby>();

                        Issuedby _Issuedby = new Issuedby();
                        _Issuedby.Issuedby_id = "0";
                        _Issuedby.Issuedby_Name = "---Select---";
                        _IssuedbyList.Add(_Issuedby);
                        _MaterialIssue_Model.IssuedbyList = _IssuedbyList;
                    }
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    CommonPageDetails();
                    DataSet ds = _MaterialIssue_IServices.GetRequirmentreaList(CompID, BrchID, reqType);
                    List<RequiredArea> RequiredAreaList = new List<RequiredArea>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RequiredArea _RequiredAreaList = new RequiredArea();
                        _RequiredAreaList.ReqArea_ID = Convert.ToInt32(dr["setup_id"]);
                        _RequiredAreaList.ReqArea_Name = dr["setup_val"].ToString();
                        RequiredAreaList.Add(_RequiredAreaList);
                    }
                    RequiredArea _RequiredAreaList2 = new RequiredArea();
                    _RequiredAreaList2.ReqArea_ID = 0;
                    _RequiredAreaList2.ReqArea_Name = "---Select---";
                    RequiredAreaList.Insert(0, _RequiredAreaList2);
                    _MaterialIssue_Model.RequiredAreaList = RequiredAreaList;

                    List<MRS_NO> _MRS_NOList = new List<MRS_NO>();
                    MRS_NO _MRS_NO = new MRS_NO();
                    _MRS_NO.MaterialIssueDate = "0";
                    _MRS_NO.MaterialIssueNo = "---Select---";
                    _MRS_NOList.Add(_MRS_NO);

                    List<EntityType> _EntityTypeList = new List<EntityType>();
                    EntityType _EntityType2 = new EntityType();
                    _EntityType2.Entity_ID = "0";
                    _EntityType2.Entity_Name = "---Select---";
                    _EntityType2.Entity_Type = "0";
                    _EntityTypeList.Insert(0, _EntityType2);
                    _MaterialIssue_Model.EntityTypelist = _EntityTypeList;
                    _MaterialIssue_Model.CompId = CompID;
                    _MaterialIssue_Model.BrchID = BrchID;
                    _MaterialIssue_Model.MRS_NO_List = _MRS_NOList;
                    //ViewBag.MenuPageName = getDocumentName();
                    _MaterialIssue_Model.MaterialIssueDate = _MaterialIssue_Model.MaterialIssueDate == null ? DateTime.Now.ToString("yyyy-MM-dd") : _MaterialIssue_Model.MaterialIssueDate;
                    _MaterialIssue_Model.Title = title;
                    _MaterialIssue_Model.DocumentMenuId = DocumentMenuId;
                    //if (Session["TransType"].ToString() == "Update")
                    if (TransType == "Update")
                    {
                        string mi_type = _MaterialIssue_Model.MRS_type;
                        string mi_no = _MaterialIssue_Model.MaterialIssueNo;
                        string mi_date = _MaterialIssue_Model.MaterialIssueDate;//.ToString("dd-MMM-yyyy");
                        DataSet dset = new DataSet();
                        dset = _MaterialIssue_IServices.GetMatrialIssueDetailByNo(CompID, BrchID, mi_type, mi_no, mi_date);
                        List<MRS_NO> MRS_NoList = new List<MRS_NO>();
                        MRS_NO MRS_No = new MRS_NO();
                        MRS_No.MaterialIssueDate = dset.Tables[0].Rows[0]["mrs_dt"].ToString();
                        MRS_No.MaterialIssueNo = dset.Tables[0].Rows[0]["mrs_no"].ToString();
                        MRS_NoList.Add(MRS_No);

                        _MaterialIssue_Model.MRS_NO_List = MRS_NoList;
                        _MaterialIssue_Model.MRS_type = dset.Tables[0].Rows[0]["issue_type"].ToString();
                        _MaterialIssue_Model.MaterialIssueNo = dset.Tables[0].Rows[0]["issue_no"].ToString();
                        _MaterialIssue_Model.MaterialIssueDate = dset.Tables[0].Rows[0]["issue_dt"].ToString();
                        _MaterialIssue_Model.RequiredArea = dset.Tables[0].Rows[0]["Req_area"].ToString();
                        _MaterialIssue_Model.MaterialIssueRemarks = dset.Tables[0].Rows[0]["issue_rem"].ToString();
                        _MaterialIssue_Model.EntityType = dset.Tables[0].Rows[0]["entity_type"].ToString();
                        _MaterialIssue_Model.IssueToCode = dset.Tables[0].Rows[0]["issue_to"].ToString();
                        _MaterialIssue_Model.IssueToName = dset.Tables[0].Rows[0]["issueToName"].ToString();
                        _MaterialIssue_Model.MRS_No = dset.Tables[0].Rows[0]["mrs_no"].ToString();
                        //_MaterialIssue_Model.MRS_Dt = dset.Tables[0].Rows[0]["mrs_dt"].ToString();
                        //     _MaterialIssue_Model.MRS_Date = Convert.ToDateTime(dset.Tables[0].Rows[0]["mrs_date"].ToString());
                        _MaterialIssue_Model.MRS_Date = dset.Tables[0].Rows[0]["mrs_date"].ToString();
                        _MaterialIssue_Model.CreatedBy = dset.Tables[0].Rows[0]["create_id"].ToString();
                        _MaterialIssue_Model.CreatedOn = dset.Tables[0].Rows[0]["create_dt"].ToString();
                        _MaterialIssue_Model.ApprovedBy = dset.Tables[0].Rows[0]["app_id"].ToString();
                        _MaterialIssue_Model.ApprovedOn = dset.Tables[0].Rows[0]["app_dt"].ToString();
                        _MaterialIssue_Model.mod_id = dset.Tables[0].Rows[0]["mod_id"].ToString();
                        _MaterialIssue_Model.mod_dt = dset.Tables[0].Rows[0]["mod_dt"].ToString();
                        _MaterialIssue_Model.Status = dset.Tables[0].Rows[0]["app_status"].ToString().Trim();
                        _MaterialIssue_Model.StatusCode = dset.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _MaterialIssue_Model.Issuedby = dset.Tables[0].Rows[0]["issued_by"].ToString();
                        _MaterialIssue_Model.bill_add_id = dset.Tables[0].Rows[0]["entity_add_id"].ToString();
                        _MaterialIssue_Model.Address = dset.Tables[0].Rows[0]["entity_address"].ToString();
                        _MaterialIssue_Model.Dispatchthrough = dset.Tables[0].Rows[0]["disp_through"].ToString();
                        _MaterialIssue_Model.VehicleNo = dset.Tables[0].Rows[0]["VehicleNo"].ToString();
                      
                            _MaterialIssue_Model.CheckDependcySampleIssue = dset.Tables[4].Rows[0]["CheckDependcySampleIssue"].ToString();
                        if (dset.Tables[1].Rows[0]["src_doc_id"].ToString() != null)
                        {
                            _MaterialIssue_Model.SrcType = dset.Tables[1].Rows[0]["src_doc_id"].ToString();
                            //_MaterialIssue_Model.ItmRWKJOFlag = dset.Tables[1].Rows[0]["FlagRwkJO"].ToString();
                        }
                        string MI_Status = string.Empty;
                     
                        MI_Status = dset.Tables[0].Rows[0]["status_code"].ToString().Trim();
                     
                        if (MI_Status == "C")
                        {
                            _MaterialIssue_Model.CancelledRemarks = dset.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _MaterialIssue_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _MaterialIssue_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _MaterialIssue_Model.CancelFlag = false;
                        }
                        string return_able = string.Empty;
                        return_able = dset.Tables[0].Rows[0]["return_able"].ToString().Trim();
                        if (return_able == "Y")
                        {                           
                            _MaterialIssue_Model.ReturnableFlag = true;
                           
                        }
                        else
                        {
                            _MaterialIssue_Model.ReturnableFlag = false;
                        }
                        getWarehouse(_MaterialIssue_Model);
                        //Session["DocumentStatus"] = dset.Tables[0].Rows[0]["app_status"].ToString().Trim();

                        ViewBag.ItemDetails = dset.Tables[1];
                        ViewBag.ItemStockBatchWise = dset.Tables[2];
                        ViewBag.ItemStockSerialWise = dset.Tables[3];
                        // ViewBag.Issuedtotabledata = dset.Tables[4];
                        ViewBag.DocumentCode = dset.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssueDetail.cshtml", _MaterialIssue_Model);
                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                        //Session["DocumentStatus"] = "New";
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssueDetail.cshtml", _MaterialIssue_Model);
                    }
                }
                else
                {
                    MaterialIssue_Model _MaterialIssue_Model = new MaterialIssue_Model();
                    //;// _MaterialIssue_Model = _model;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MaterialIssue_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    GetModelDetails _GetModelDetails = new GetModelDetails();
                    _GetModelDetails = _model;
                    if (_GetModelDetails != null && _url_Model == null)
                    {
                        _MaterialIssue_Model.BtnName = _GetModelDetails.BtnName;
                        _MaterialIssue_Model.TransType = _GetModelDetails.TransType;
                        _MaterialIssue_Model.MaterialIssueDate = _GetModelDetails.MaterialIssueDate;
                        _MaterialIssue_Model.MaterialIssueNo = _GetModelDetails.MaterialIssueNo;
                        _MaterialIssue_Model.MRS_type = _GetModelDetails.MRS_type;
                        _MaterialIssue_Model._mdlCommand = _GetModelDetails._mdlCommand;
                        _MaterialIssue_Model.Message = _GetModelDetails.Message;
                        //_MaterialIssue_Model.CancelFlag = _GetModelDetails.CancelFlag;
                        //_MaterialIssue_Model.DocumentMenuId = _GetModelDetails.DocumentMenuId;
                    }
                    else
                    {
                        if (_url_Model != null)
                        {
                            _MaterialIssue_Model.BtnName = _url_Model.BtnName;
                            _MaterialIssue_Model.TransType = _url_Model.Tnstyp;
                            _MaterialIssue_Model.MaterialIssueDate = _url_Model.Doc_dt;
                            _MaterialIssue_Model.MaterialIssueNo = _url_Model.Docid;
                            _MaterialIssue_Model.MRS_type = _url_Model.MRS_type;
                            _MaterialIssue_Model._mdlCommand = _url_Model.CMD;

                            //_MaterialIssue_Model.CancelFlag = _GetModelDetails.CancelFlag;
                            //_MaterialIssue_Model.DocumentMenuId = _GetModelDetails.DocumentMenuId;
                        }
                    }
                    //if (TempData["MaterialIssue_Model"] != null)
                    //{
                    //    _MaterialIssue_Model = TempData["MaterialIssue_Model"] as MaterialIssue_Model;
                    //}
                    _MaterialIssue_Model.BtnName = _MaterialIssue_Model.BtnName == null ? "BtnAddNew" : _MaterialIssue_Model.BtnName;
                    //   DocumentMenuId = _MaterialIssue_Model.DocumentMenuId != null? _MaterialIssue_Model.DocumentMenuId : Session["MenuDocumentId"]!=null ?Session["MenuDocumentId"].ToString():"";
                    TransType = _MaterialIssue_Model.TransType;
                    var reqType = _MaterialIssue_Model.MRS_type;
                    if (reqType != null)
                    {
                        if (reqType == "I")
                        {
                            DocumentMenuId = "105102130101";
                            ViewBag.DocumentMenuId = "105102130101";
                        }
                        if (reqType == "E")
                        {
                            DocumentMenuId = "105102130105";
                            ViewBag.DocumentMenuId = "105102130105";
                        }
                        if (reqType == "S")
                        {
                            DocumentMenuId = "105102130108";
                            ViewBag.DocumentMenuId = "105102130108";
                        }
                    }
                    if (DocumentMenuId == "105102130108")
                    {
                        DataSet ds1 = _MaterialIssue_IServices.GetIssuedByData(CompID, BrchID);
                        List<Issuedby> _IssuedbyList = new List<Issuedby>();
                        foreach (DataRow dr in ds1.Tables[0].Rows)
                        {
                            Issuedby _Issuedby = new Issuedby();
                            _Issuedby.Issuedby_id = dr["emp_id"].ToString();
                            _Issuedby.Issuedby_Name = dr["emp_name"].ToString();
                            _IssuedbyList.Add(_Issuedby);

                        }
                        Issuedby _Issuedby2 = new Issuedby();
                        _Issuedby2.Issuedby_id = "0";
                        _Issuedby2.Issuedby_Name = "---Select---";
                        _IssuedbyList.Insert(0, _Issuedby2);
                        _MaterialIssue_Model.IssuedbyList = _IssuedbyList;
                        // _MaterialIssue_Model.IssuedbyList = _IssuedbyList;
                    }
                    else
                    {

                        List<Issuedby> _IssuedbyList = new List<Issuedby>();

                        Issuedby _Issuedby = new Issuedby();
                        _Issuedby.Issuedby_id = "0";
                        _Issuedby.Issuedby_Name = "---Select---";
                        _IssuedbyList.Add(_Issuedby);
                        _MaterialIssue_Model.IssuedbyList = _IssuedbyList;
                    }
                    //***Comment by Shubham Maurya on 16-01-2023 All Data Passing In URL Change ***//
                    //string reqType = "";
                    //if (DocumentMenuId == "105102130101")
                    //{
                    //    reqType = "I";
                    //}
                    //if (DocumentMenuId == "105102130105")
                    //{
                    //    reqType = "E";
                    //}
                    //if (DocumentMenuId == "105102130108")
                    //{
                    //    reqType = "S";
                    //}
                    //if (Session["MenuDocumentId"] != null)
                    //{
                    //    if (Session["MenuDocumentId"].ToString() == "105102130101")
                    //    {
                    //        Session["internalissue"] = "105102130101";
                    //        DocumentMenuId = "105102130101";
                    //        Session["Document_Menu_Id"] = "105102130101";
                    //        reqType = "I";
                    //    }
                    //    if (Session["MenuDocumentId"].ToString() == "105102130105")
                    //    {
                    //        Session["externalissue"] = "105102130105";
                    //        DocumentMenuId = "105102130105";
                    //        Session["Document_Menu_Id"] = "105102130105";
                    //        reqType = "E";
                    //    }
                    //    if (Session["MenuDocumentId"].ToString() == "105102130108")
                    //    {
                    //        Session["externalissue"] = "105102130108";
                    //        DocumentMenuId = "105102130108";
                    //        Session["Document_Menu_Id"] = "105102130108";
                    //        reqType = "S";
                    //    }
                    //}

                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                    //_MaterialIssue_Model = new MaterialIssue_Model();
                    //_MaterialIssue_Model = TempData["MaterialIssue_Model"] as MaterialIssue_Model;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    CommonPageDetails();
                    DataSet ds = _MaterialIssue_IServices.GetRequirmentreaList(CompID, BrchID, reqType);
                    List<RequiredArea> RequiredAreaList = new List<RequiredArea>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RequiredArea _RequiredAreaList = new RequiredArea();
                        _RequiredAreaList.ReqArea_ID = Convert.ToInt32(dr["setup_id"]);
                        _RequiredAreaList.ReqArea_Name = dr["setup_val"].ToString();
                        RequiredAreaList.Add(_RequiredAreaList);
                    }
                    RequiredArea _RequiredAreaList2 = new RequiredArea();
                    _RequiredAreaList2.ReqArea_ID = 0;
                    _RequiredAreaList2.ReqArea_Name = "---Select---";
                    RequiredAreaList.Insert(0, _RequiredAreaList2);
                    _MaterialIssue_Model.RequiredAreaList = RequiredAreaList;

                    List<MRS_NO> _MRS_NOList = new List<MRS_NO>();
                    MRS_NO _MRS_NO = new MRS_NO();
                    _MRS_NO.MaterialIssueDate = "0";
                    _MRS_NO.MaterialIssueNo = "---Select---";
                    _MRS_NOList.Add(_MRS_NO);

                    List<EntityType> _EntityTypeList = new List<EntityType>();
                    EntityType _EntityType2 = new EntityType();
                    _EntityType2.Entity_ID = "0";
                    _EntityType2.Entity_Name = "---Select---";
                    _EntityType2.Entity_Type = "0";
                    _EntityTypeList.Insert(0, _EntityType2);
                    _MaterialIssue_Model.EntityTypelist = _EntityTypeList;
                    _MaterialIssue_Model.CompId = CompID;
                    _MaterialIssue_Model.BrchID = BrchID;
                    _MaterialIssue_Model.MRS_NO_List = _MRS_NOList;
                    //ViewBag.MenuPageName = getDocumentName();
                    _MaterialIssue_Model.MaterialIssueDate = _MaterialIssue_Model.MaterialIssueDate == null ? DateTime.Now.ToString("yyyy-MM-dd") : _MaterialIssue_Model.MaterialIssueDate;
                    _MaterialIssue_Model.Title = title;
                    _MaterialIssue_Model.DocumentMenuId = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    //if (Session["TransType"].ToString() == "Update")
                    if (TransType == "Update")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        //BrchID = Session["BranchId"].ToString();
                        //string mi_type = Session["MI_Type"].ToString();
                        //string mi_no = Session["MI_Number"].ToString();
                        //string mi_date = Session["MI_Date"].ToString();
                        string mi_type = _MaterialIssue_Model.MRS_type;
                        string mi_no = _MaterialIssue_Model.MaterialIssueNo;
                        string mi_date = _MaterialIssue_Model.MaterialIssueDate;//.ToString("dd-MMM-yyyy");
                        DataSet dset = new DataSet();
                        //***Comment by Shubham Maurya on 16-01-2023 All Data Passing In URL Change ***//
                        //if (TempData["MIDtSet"] == null)
                        //{
                        dset = _MaterialIssue_IServices.GetMatrialIssueDetailByNo(CompID, BrchID, mi_type, mi_no, mi_date);
                        //}
                        //else
                        //{
                        //    dset = TempData["MIDtSet"] as DataSet;
                        //}
                        List<MRS_NO> MRS_NoList = new List<MRS_NO>();
                        MRS_NO MRS_No = new MRS_NO();
                        MRS_No.MaterialIssueDate = dset.Tables[0].Rows[0]["mrs_dt"].ToString();
                        MRS_No.MaterialIssueNo = dset.Tables[0].Rows[0]["mrs_no"].ToString();
                        MRS_NoList.Add(MRS_No);

                        _MaterialIssue_Model.MRS_NO_List = MRS_NoList;
                        _MaterialIssue_Model.MRS_type = dset.Tables[0].Rows[0]["issue_type"].ToString();
                        _MaterialIssue_Model.MaterialIssueNo = dset.Tables[0].Rows[0]["issue_no"].ToString();
                        _MaterialIssue_Model.MaterialIssueDate = dset.Tables[0].Rows[0]["issue_dt"].ToString();
                        _MaterialIssue_Model.RequiredArea = dset.Tables[0].Rows[0]["Req_area"].ToString();
                        _MaterialIssue_Model.MaterialIssueRemarks = dset.Tables[0].Rows[0]["issue_rem"].ToString();
                        _MaterialIssue_Model.EntityType = dset.Tables[0].Rows[0]["entity_type"].ToString();
                        _MaterialIssue_Model.IssueToCode = dset.Tables[0].Rows[0]["issue_to"].ToString();
                        _MaterialIssue_Model.IssueToName = dset.Tables[0].Rows[0]["issueToName"].ToString();
                        //_MaterialIssue_Model.MRS_No = dset.Tables[0].Rows[0]["mrs_no"].ToString();
                        _MaterialIssue_Model.MRS_No = dset.Tables[0].Rows[0]["mrs_no"].ToString();
                        _MaterialIssue_Model.MRS_Date = dset.Tables[0].Rows[0]["mrs_dt"].ToString();
                        _MaterialIssue_Model.CreatedBy = dset.Tables[0].Rows[0]["create_id"].ToString();
                        _MaterialIssue_Model.CreatedOn = dset.Tables[0].Rows[0]["create_dt"].ToString();
                        _MaterialIssue_Model.ApprovedBy = dset.Tables[0].Rows[0]["app_id"].ToString();
                        _MaterialIssue_Model.ApprovedOn = dset.Tables[0].Rows[0]["app_dt"].ToString();
                        _MaterialIssue_Model.mod_id = dset.Tables[0].Rows[0]["mod_id"].ToString();
                        _MaterialIssue_Model.mod_dt = dset.Tables[0].Rows[0]["mod_dt"].ToString();
                        _MaterialIssue_Model.Status = dset.Tables[0].Rows[0]["app_status"].ToString().Trim();
                        _MaterialIssue_Model.StatusCode = dset.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _MaterialIssue_Model.Issuedby = dset.Tables[0].Rows[0]["issued_by"].ToString();
                        _MaterialIssue_Model.bill_add_id = dset.Tables[0].Rows[0]["entity_add_id"].ToString();
                        _MaterialIssue_Model.Address = dset.Tables[0].Rows[0]["entity_address"].ToString();
                        _MaterialIssue_Model.Dispatchthrough = dset.Tables[0].Rows[0]["disp_through"].ToString();
                        _MaterialIssue_Model.VehicleNo = dset.Tables[0].Rows[0]["VehicleNo"].ToString();
                      
                        _MaterialIssue_Model.CheckDependcySampleIssue = dset.Tables[4].Rows[0]["CheckDependcySampleIssue"].ToString();
                        if (dset.Tables[1].Rows[0]["src_doc_id"].ToString() != null)
                        {
                            _MaterialIssue_Model.SrcType = dset.Tables[1].Rows[0]["src_doc_id"].ToString();
                            //_MaterialIssue_Model.ItmRWKJOFlag = dset.Tables[1].Rows[0]["FlagRwkJO"].ToString();
                        }
                        string MI_Status = string.Empty;
                        MI_Status = dset.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (MI_Status == "C")
                        {
                            _MaterialIssue_Model.CancelledRemarks = dset.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _MaterialIssue_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _MaterialIssue_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _MaterialIssue_Model.CancelFlag = false;
                        }
                        string return_able = string.Empty;
                        return_able = dset.Tables[0].Rows[0]["return_able"].ToString().Trim();
                        if (return_able == "Y")
                        {
                            _MaterialIssue_Model.ReturnableFlag = true;

                        }
                        else
                        {
                            _MaterialIssue_Model.ReturnableFlag = false;
                        }
                        getWarehouse(_MaterialIssue_Model);
                        //Session["DocumentStatus"] = dset.Tables[0].Rows[0]["app_status"].ToString().Trim();

                        ViewBag.ItemDetails = dset.Tables[1];
                        ViewBag.ItemStockBatchWise = dset.Tables[2];
                        ViewBag.ItemStockSerialWise = dset.Tables[3];
                        //ViewBag.Issuedtotabledata= dset.Tables[4];
                        ViewBag.DocumentCode = dset.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        // ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssueDetail.cshtml", _MaterialIssue_Model);
                    }
                    else
                    {

                        ViewBag.DocumentCode = "0";
                        //Session["DocumentStatus"] = "New";
                        // ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssueDetail.cshtml", _MaterialIssue_Model);
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AddNewMaterialIssue(MaterialIssue_Model _MaterialIssue_Model)
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //***Comment by Shubham Maurya on 16-01-2023 All Data Passing In URL Change ***//
            //_MaterialIssue_Model.BtnName= "BtnAddNew"; 
            //if (_MaterialIssue_Model.DocumentMenuId != null)
            //{
            //DocumentMenuId = _MaterialIssue_Model.DocumentMenuId;
            //}
            GetModelDetails _GetModelDetails = new GetModelDetails();
            _GetModelDetails.MRS_type = _MaterialIssue_Model.MRS_type;
            _GetModelDetails.MaterialIssueNo = _MaterialIssue_Model.MaterialIssueNo;
            _GetModelDetails.MaterialIssueDate = _MaterialIssue_Model.MaterialIssueDate;
            _GetModelDetails.TransType = "Save";
            _GetModelDetails.BtnName = "BtnAddNew";
            _GetModelDetails._mdlCommand = _MaterialIssue_Model._mdlCommand;
            TempData["ModelData"] = _GetModelDetails;
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
                return RedirectToAction("MaterialIssueList", new { reqType = _MaterialIssue_Model.MRS_type });
            }
            /*End to chk Financial year exist or not*/
            URLModelDetails addnewModel = new URLModelDetails();
            addnewModel.MRS_type = _MaterialIssue_Model.MRS_type;
            addnewModel.Tnstyp = _GetModelDetails.TransType;
            addnewModel.BtnName = _GetModelDetails.BtnName;
            addnewModel.CMD = _GetModelDetails._mdlCommand;
            //TempData["MaterialIssue_Model"] = _MaterialIssue_Model;

            return RedirectToAction("MaterialIssueDetail", "MaterialIssue", addnewModel);
        }
        [NonAction]
        private void getWarehouse(MaterialIssue_Model _MaterialIssue_Model)
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
                DataSet result = _MaterialIssue_IServices.GetWarehouseList(Comp_ID, Br_ID);
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
                _MaterialIssue_Model.WarehouseList = _WarehouseList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        [HttpPost]
        public JsonResult GetWarehouseList1(string doc_id)
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
                DataSet result = _MaterialIssue_IServices.GetWarehouseList1(Comp_ID, Br_ID, doc_id);
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
        public ActionResult MaterialIssueSave(MaterialIssue_Model _MaterialIssue_Model, string command)
        {
            try
            {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                Session["MenuDocumentId"] = _MaterialIssue_Model.DocumentMenuId;
                if (_MaterialIssue_Model.DeleteCommand == "Delete")
                    if (true)
                    {
                        command = "Delete";
                    }
                switch (command)
                {
                    case "AddNew":
                        GetModelDetails _GetModelDetails = new GetModelDetails();
                        _GetModelDetails.MRS_type = _MaterialIssue_Model.MRS_type;
                        _GetModelDetails.MaterialIssueNo = null;
                        _GetModelDetails.MaterialIssueDate = null;
                        _GetModelDetails.TransType = "Save";
                        _GetModelDetails.BtnName = "BtnAddNew";
                        _GetModelDetails._mdlCommand = command;
                        TempData["ModelData"] = _GetModelDetails;
                        URLModelDetails addnewModel = new URLModelDetails();
                        addnewModel.MRS_type = _GetModelDetails.MRS_type;
                        addnewModel.Tnstyp = _GetModelDetails.TransType;
                        addnewModel.BtnName = _GetModelDetails.BtnName;
                        addnewModel.CMD = _GetModelDetails._mdlCommand;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_MaterialIssue_Model.MaterialIssueNo))
                                return RedirectToAction("EditMaterialIssue", new { IssueType = _MaterialIssue_Model.MRS_type, IssueNumber = _MaterialIssue_Model.MaterialIssueNo, IssueDate = _MaterialIssue_Model.MaterialIssueDate, DMenuId = _MaterialIssue_Model.DocumentMenuId, ListFilterData = _MaterialIssue_Model.ListFilterData1 });
                            else
                                _GetModelDetails._mdlCommand = "Refresh";
                            _GetModelDetails.TransType = "Refresh";
                            _GetModelDetails.BtnName = "Refresh";
                            TempData["ModelData"] = _GetModelDetails;
                            return RedirectToAction("MaterialIssueDetail");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("MaterialIssueDetail", addnewModel);

                    case "Edit":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditMaterialIssue", new { IssueType = _MaterialIssue_Model.MRS_type, IssueNumber = _MaterialIssue_Model.MaterialIssueNo, IssueDate = _MaterialIssue_Model.MaterialIssueDate, DMenuId = _MaterialIssue_Model.DocumentMenuId, ListFilterData = _MaterialIssue_Model.ListFilterData1 });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string issueDt = _MaterialIssue_Model.MaterialIssueDate;

                        if (_MaterialIssue_Model.DocumentMenuId != "105102130108")
                        {
                            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, issueDt) == "TransNotAllow")
                            {
                                TempData["Message1"] = "TransNotAllow";
                                return RedirectToAction("EditMaterialIssue", new { IssueType = _MaterialIssue_Model.MRS_type, IssueNumber = _MaterialIssue_Model.MaterialIssueNo, IssueDate = _MaterialIssue_Model.MaterialIssueDate, DMenuId = _MaterialIssue_Model.DocumentMenuId, ListFilterData = _MaterialIssue_Model.ListFilterData1 });
                            }
                        }
                        GetModelDetails _GetModelDetails2 = new GetModelDetails();
                        URLModelDetails URLModel = new URLModelDetails();
                        if (_MaterialIssue_Model.MRS_type=="E")
                        {
                            checkDependency(_MaterialIssue_Model);
                        }
                        else
                        {
                            _MaterialIssue_Model.Message = "";
                        }
                       
                        if (_MaterialIssue_Model.Message == "Used")
                        {
                         
                        
                            _GetModelDetails2.TransType = "Update";
                            _GetModelDetails2.BtnName = "BtnRefresh";
                            _GetModelDetails2._mdlCommand = "Refresh";
                            URLModel.Tnstyp = "Update";
                            URLModel.CMD = "Refresh";
                            URLModel.BtnName = "BtnRefresh";
                            _GetModelDetails2.Message = "Used";

                        }
                        else
                        {
                          //  GetModelDetails _GetModelDetails2 = new GetModelDetails();
                            _GetModelDetails2.TransType = "Update";
                            _GetModelDetails2.BtnName = "BtnEdit";
                            _GetModelDetails2._mdlCommand = command;
                            _GetModelDetails2.Message = null;
                            URLModel.Tnstyp = "Update";
                            URLModel.CMD = command;
                            URLModel.BtnName = "BtnEdit";
                        }
                        /*End to chk Financial year exist or not*/
                       // GetModelDetails _GetModelDetails2 = new GetModelDetails();
                        _GetModelDetails2.MRS_type = _MaterialIssue_Model.MRS_type;
                        _GetModelDetails2.MaterialIssueNo = _MaterialIssue_Model.MaterialIssueNo;
                        _GetModelDetails2.MaterialIssueDate = _MaterialIssue_Model.MaterialIssueDate;
                      
                        TempData["ModelData"] = _GetModelDetails2;
                      
                        URLModel.MRS_type = _GetModelDetails2.MRS_type;
                        URLModel.Docid = _GetModelDetails2.MaterialIssueNo;
                        URLModel.Doc_dt = _GetModelDetails2.MaterialIssueDate;
                      
                        TempData["ListFilterData"] = _MaterialIssue_Model.ListFilterData1;
                        return RedirectToAction("MaterialIssueDetail", URLModel);
                    case "Save":

                        //Session["Command"] = command;
                        _MaterialIssue_Model._mdlCommand = command;
                        SaveUpdateMaterialIssue(_MaterialIssue_Model);
                        if (_MaterialIssue_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_MaterialIssue_Model.Message == "DocModify"||_MaterialIssue_Model.Message== "StockNotAvail_S")
                        {


                            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["Language"].Value);
                            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["Language"].Value);
                            if (Session["CompId"] != null)
                            {
                                CompID = Session["CompId"].ToString();
                            }
                            if (Session["BranchId"] != null)
                            {
                                BrchID = Session["BranchId"].ToString();
                            }

                            var reqType = _MaterialIssue_Model.MRS_type;
                            if (reqType != null)
                            {
                                if (reqType == "I")
                                {
                                    DocumentMenuId = "105102130101";
                                    ViewBag.DocumentMenuId = "105102130101";
                                }
                                if (reqType == "E")
                                {
                                    DocumentMenuId = "105102130105";
                                    ViewBag.DocumentMenuId = "105102130105";
                                }
                                if (reqType == "S")
                                {
                                    DocumentMenuId = "105102130108";
                                    ViewBag.DocumentMenuId = "105102130108";
                                }
                            }
                            CommonPageDetails();
                            DataSet ds = _MaterialIssue_IServices.GetRequirmentreaList(CompID, BrchID, reqType);
                            List<RequiredArea> RequiredAreaList = new List<RequiredArea>();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                RequiredArea _RequiredAreaList = new RequiredArea();
                                _RequiredAreaList.ReqArea_ID = Convert.ToInt32(dr["setup_id"]);
                                _RequiredAreaList.ReqArea_Name = dr["setup_val"].ToString();
                                RequiredAreaList.Add(_RequiredAreaList);
                            }
                            var ReqArea = _MaterialIssue_Model.hidenRequiredArea;
                            RequiredArea _RequiredAreaList2 = new RequiredArea();
                            _RequiredAreaList2.ReqArea_ID = 0;
                            _RequiredAreaList2.ReqArea_Name = ReqArea;
                            RequiredAreaList.Insert(0, _RequiredAreaList2);
                            _MaterialIssue_Model.RequiredAreaList = RequiredAreaList;

                            var MtIssuDT = _MaterialIssue_Model.hiddenMRS_Date;
                            var MtIssuNo = _MaterialIssue_Model.hiddenMRS_No;
                            List<MRS_NO> _MRS_NOList = new List<MRS_NO>();
                            MRS_NO _MRS_NO = new MRS_NO();
                            _MRS_NO.MaterialIssueDate = MtIssuDT;
                            _MRS_NO.MaterialIssueNo = MtIssuNo;
                            _MRS_NOList.Add(_MRS_NO);

                            List<EntityType> _EntityTypeList = new List<EntityType>();
                            EntityType _EntityType2 = new EntityType();
                            _EntityType2.Entity_ID = "0";
                            _EntityType2.Entity_Name = "---Select---";
                            _EntityType2.Entity_Type = "0";
                            _EntityTypeList.Insert(0, _EntityType2);
                            _MaterialIssue_Model.EntityTypelist = _EntityTypeList;
                            _MaterialIssue_Model.CompId = CompID;
                            _MaterialIssue_Model.BrchID = BrchID;
                            _MaterialIssue_Model.MRS_NO_List = _MRS_NOList;
                            //ViewBag.MenuPageName = getDocumentName();
                            _MaterialIssue_Model.MaterialIssueDate = _MaterialIssue_Model.MaterialIssueDate == null ? DateTime.Now.ToString("yyyy-MM-dd") : _MaterialIssue_Model.MaterialIssueDate;
                            _MaterialIssue_Model.Title = title;
                            _MaterialIssue_Model.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            _MaterialIssue_Model.MRS_Date = _MaterialIssue_Model.hiddenMRS_Date;
                            getWarehouse(_MaterialIssue_Model);
                            ViewBag.ItemDetails = ViewData["MaterialIssueItemDetails"];
                            ViewBag.ItemStockBatchWise = ViewData["BatchDetails"];
                            ViewBag.ItemStockSerialWise = ViewData["SerialDetail"];
                            ViewBag.SubItemDetails = ViewData["SubItem"];

                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssueDetail.cshtml", _MaterialIssue_Model);
                        }
                        GetModelDetails _GetModelDetails1 = new GetModelDetails();
                        _GetModelDetails1.MRS_type = _MaterialIssue_Model.MRS_type;
                        _GetModelDetails1.MaterialIssueNo = _MaterialIssue_Model.MaterialIssueNo;
                        _GetModelDetails1.MaterialIssueDate = _MaterialIssue_Model.MaterialIssueDate;
                        _GetModelDetails1.TransType = _MaterialIssue_Model.TransType;
                        _GetModelDetails1.BtnName = _MaterialIssue_Model.BtnName;
                        _GetModelDetails1._mdlCommand = _MaterialIssue_Model._mdlCommand;
                        _GetModelDetails1.Message = _MaterialIssue_Model.Message;
                        TempData["ModelData"] = _GetModelDetails1;
                        URLModelDetails Save_model = new URLModelDetails();
                        Save_model.MRS_type = _GetModelDetails1.MRS_type;
                        Save_model.Docid = _GetModelDetails1.MaterialIssueNo;
                        Save_model.Doc_dt = _GetModelDetails1.MaterialIssueDate;
                        Save_model.Tnstyp = _GetModelDetails1.TransType;
                        Save_model.CMD = _GetModelDetails1._mdlCommand;
                        Save_model.BtnName = _GetModelDetails1.BtnName;
                        TempData["ListFilterData"] = _MaterialIssue_Model.ListFilterData1;
                        return RedirectToAction("MaterialIssueDetail", Save_model);
                    case "Refresh":
                        GetModelDetails _GetModelDetails3 = new GetModelDetails();
                        _GetModelDetails3.MRS_type = _MaterialIssue_Model.MRS_type;
                        _GetModelDetails3.MaterialIssueNo = null;
                        _GetModelDetails3.MaterialIssueDate = null;
                        _GetModelDetails3.TransType = "Save";
                        _GetModelDetails3.BtnName = "BtnRefresh";
                        _GetModelDetails3._mdlCommand = command;
                        TempData["ModelData"] = _GetModelDetails3;
                        URLModelDetails Refresh_model = new URLModelDetails();
                        Refresh_model.MRS_type = _GetModelDetails3.MRS_type;
                        Refresh_model.Tnstyp = _GetModelDetails3.TransType;
                        Refresh_model.CMD = _GetModelDetails3._mdlCommand;
                        Refresh_model.BtnName = _GetModelDetails3.BtnName;
                        TempData["ListFilterData"] = _MaterialIssue_Model.ListFilterData1;
                        return RedirectToAction("MaterialIssueDetail", Refresh_model);

                    case "Print":
                        //return new EmptyResult();
                        return GenratePdfFile(_MaterialIssue_Model);
                    case "BacktoList":

                        TempData["ListFilterData"] = _MaterialIssue_Model.ListFilterData1;
                        return RedirectToAction("MaterialIssueList", new { reqType = _MaterialIssue_Model.MRS_type });

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
        public ActionResult GetMaterialIssueList(MaterialIssue_Model _MaterialIssue_Model)
        {
            try
            {
                string MRSNo, Area, RequisitionType = string.Empty;
                DataSet PackListNumberDs = new DataSet();

                List<MRS_NO> _MRS_NOList = new List<MRS_NO>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                MRSNo = _MaterialIssue_Model.FilterMRSNo;
                Area = _MaterialIssue_Model.FilterArea;
                RequisitionType = _MaterialIssue_Model.FilterRequisitionType;
                string BrchID = Session["BranchId"].ToString();
                PackListNumberDs = _MaterialIssue_IServices.getMRSNOList(CompID, BrchID, MRSNo, Area, RequisitionType);
                if (PackListNumberDs.Tables[0].Rows.Count > 0)
                {
                    DataRow Drow = PackListNumberDs.Tables[0].NewRow();
                    Drow[0] = "---Select---";
                    Drow[1] = "0";

                    PackListNumberDs.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in PackListNumberDs.Tables[0].Rows)
                    {
                        MRS_NO _MRS_NO = new MRS_NO(); ;
                        _MRS_NO.MaterialIssueDate = dr["mrs_dt"].ToString();
                        _MRS_NO.MaterialIssueNo = dr["mrs_no"].ToString();
                        _MRS_NOList.Add(_MRS_NO);
                    }

                }

                _MaterialIssue_Model.MRS_NO_List = _MRS_NOList;
                return Json(_MRS_NOList.Select(c => new { Name = c.MaterialIssueNo, ID = c.MaterialIssueDate }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult EditMaterialIssue(string IssueType, string IssueNumber, string IssueDate, string DMenuId, string ListFilterData)
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
            GetModelDetails Dblclick = new GetModelDetails();
            Dblclick.MRS_type = IssueType.Trim();
            Dblclick.MaterialIssueNo = IssueNumber.Trim();
            Dblclick.MaterialIssueDate = Convert.ToDateTime(IssueDate.Trim()).ToString("yyyy-MM-dd");
            Dblclick.TransType = "Update";
            Dblclick.BtnName = "BtnEdit";
            TempData["ModelData"] = Dblclick;
            URLModelDetails Dbl_ClickModel = new URLModelDetails();
            Dbl_ClickModel.MRS_type = Dblclick.MRS_type;
            Dbl_ClickModel.Docid = Dblclick.MaterialIssueNo;
            Dbl_ClickModel.Doc_dt = Dblclick.MaterialIssueDate;
            Dbl_ClickModel.Tnstyp = Dblclick.TransType;
            Dbl_ClickModel.CMD = Dblclick._mdlCommand;
            Dbl_ClickModel.BtnName = Dblclick.BtnName;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("MaterialIssueDetail", "MaterialIssue", Dbl_ClickModel);
        }
        public ActionResult GetMaterialRequisitionIssueTo(string MRSDate, string MRSNo, string mrs_type)
        {
            try
            {
                MaterialIssue_Model _MaterialIssue_Model = new MaterialIssue_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet MateriaRequisitionIssueTo = _MaterialIssue_IServices.GetMaterialRequisitionIssueTo(CompID, BrchID, MRSDate, MRSNo, mrs_type);
                //_MaterialIssue_Model.EntityType = MateriaRequisitionIssueTo;
                _MaterialIssue_Model.IssueToName = MateriaRequisitionIssueTo.Tables[0].Rows[0]["IssueToName"].ToString();
                _MaterialIssue_Model.IssueToCode = MateriaRequisitionIssueTo.Tables[0].Rows[0]["IssueToCode"].ToString();
                _MaterialIssue_Model.EntityType = MateriaRequisitionIssueTo.Tables[0].Rows[0]["entity_type"].ToString();
                _MaterialIssue_Model.HDN_Issuedby = MateriaRequisitionIssueTo.Tables[1].Rows[0]["mrs_rem"].ToString();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialIssue_IssueTo.cshtml", _MaterialIssue_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        [HttpPost]
        public JsonResult GetSuppAddrDetail(string MRSDate, string MRSNo, string mrs_type)
        {
            try
            {
                DataSet result = new DataSet();
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
                result = _MaterialIssue_IServices.GetSuppAddrDetailDAL(Comp_ID, BrchID, MRSDate, MRSNo, mrs_type);

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
        public ActionResult getMaterialRequisitionDetailByNumber(string MRSType, string MRSNo, string MRSDate)
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
                DataSet ds = _MaterialIssue_IServices.GetMaterialRequisitionItemDetailByNO(CompID, BrchID, MRSDate, MRSNo, MRSType);
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
        public ActionResult SaveUpdateMaterialIssue(MaterialIssue_Model _MaterialIssue_Model)
        {
            try
            {

                if (_MaterialIssue_Model.CancelFlag == false)
                {
                    var commonContr = new CommonController();
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }
                    //if(Session["Document_Menu_Id"]!=null)
                    //{
                    //    DocumentMenuId = Session["Document_Menu_Id"].ToString();
                    //}
                    DocumentMenuId = _MaterialIssue_Model.DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    DataTable MaterialIssuetHeader = new DataTable();
                    DataTable MaterialIssueItemDetails = new DataTable();
                    //DataTable MaterialIssueAttachments = new DataTable();
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("issue_type", typeof(string));
                    dtheader.Columns.Add("issue_no", typeof(string));
                    dtheader.Columns.Add("issue_dt", typeof(string));
                    dtheader.Columns.Add("Req_area", typeof(int));
                    dtheader.Columns.Add("mrs_no", typeof(string));
                    dtheader.Columns.Add("mrs_dt", typeof(string));
                    dtheader.Columns.Add("issue_to", typeof(string));
                    dtheader.Columns.Add("entity_type", typeof(string));
                    dtheader.Columns.Add("issue_rem", typeof(string));
                    dtheader.Columns.Add("create_id", typeof(string));
                    dtheader.Columns.Add("mrs_status", typeof(string));
                    dtheader.Columns.Add("UserMacaddress", typeof(string));
                    dtheader.Columns.Add("UserSystemName", typeof(string));
                    dtheader.Columns.Add("UserIP", typeof(string));
                    dtheader.Columns.Add("issuedby", typeof(string));
                    dtheader.Columns.Add("entity_add_id", typeof(int));
                    dtheader.Columns.Add("Dispatchthrough", typeof(string));
                    dtheader.Columns.Add("VehicleNo", typeof(string));
                    dtheader.Columns.Add("return_able", typeof(char));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    dtrowHeader["TransType"] = "Save";// _MaterialIssue_Model.TransType;// Session["TransType"].ToString();
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    dtrowHeader["br_id"] = Session["BranchId"].ToString();
                    dtrowHeader["issue_type"] = _MaterialIssue_Model.MRS_type;
                    dtrowHeader["issue_no"] = _MaterialIssue_Model.MaterialIssueNo;
                    dtrowHeader["issue_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                    dtrowHeader["Req_area"] = _MaterialIssue_Model.RequiredArea;
                    dtrowHeader["mrs_no"] = _MaterialIssue_Model.MRS_No;
                    // dtrowHeader["mrs_dt"] = Convert.ToDateTime(_MaterialIssue_Model.MRS_Date);
                    dtrowHeader["mrs_dt"] = _MaterialIssue_Model.MRS_Date;
                    dtrowHeader["issue_to"] = _MaterialIssue_Model.IssueToCode;
                    dtrowHeader["entity_type"] = _MaterialIssue_Model.EntityType;
                    dtrowHeader["issue_rem"] = _MaterialIssue_Model.MaterialIssueRemarks;
                    dtrowHeader["create_id"] = Session["UserId"].ToString();
                    dtrowHeader["mrs_status"] = "I";// Session["AppStatus"].ToString();
                    dtrowHeader["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrowHeader["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrowHeader["UserIP"] = Session["UserIP"].ToString();
                    dtrowHeader["issuedby"] = _MaterialIssue_Model.Issuedby;

                    if (_MaterialIssue_Model.MRS_type == "E" && _MaterialIssue_Model.EntityType != "E")
                    {
                        dtrowHeader["entity_add_id"] = _MaterialIssue_Model.bill_add_id;
                    }
                    else
                    {
                        dtrowHeader["entity_add_id"] = 0;
                    }
                    dtrowHeader["Dispatchthrough"] = _MaterialIssue_Model.Dispatchthrough;
                    dtrowHeader["VehicleNo"] = _MaterialIssue_Model.VehicleNo;
                    if(DocumentMenuId== "105102130105")
                    {
                        if (_MaterialIssue_Model.ReturnableFlag == true)
                        {
                            dtrowHeader["return_able"] = "Y";
                        }
                        else
                        {
                            dtrowHeader["return_able"] = "N";
                        }
                    }
                    else
                    {
                        dtrowHeader["return_able"] = "N";
                    }
                   
                   
                    dtheader.Rows.Add(dtrowHeader);
                    MaterialIssuetHeader = dtheader;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(int));
                    dtItem.Columns.Add("br_id", typeof(int));
                    dtItem.Columns.Add("issue_type", typeof(string));
                    dtItem.Columns.Add("RewrkFlag", typeof(string));

                    //dtItem.Columns.Add("issue_no", typeof(string));
                    //dtItem.Columns.Add("issue_dt", typeof(DateTime));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("mrs_qty", typeof(string));
                    dtItem.Columns.Add("pend_qty", typeof(string));
                    dtItem.Columns.Add("wh_id", typeof(int));
                    dtItem.Columns.Add("avl_stock", typeof(string));
                    dtItem.Columns.Add("issue_qty", typeof(string));
                    dtItem.Columns.Add("cost_pr", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));

                    dtItem.Columns.Add("sr_type", typeof(string));
                    dtItem.Columns.Add("other_dtl", typeof(string));
                    dtItem.Columns.Add("issue_date", typeof(string));
                    dtItem.Columns.Add("bin_loc", typeof(string));


                    if (_MaterialIssue_Model.MaterialIssueItemDetails != null)
                    {
                        JArray jObject = JArray.Parse(_MaterialIssue_Model.MaterialIssueItemDetails);
                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["comp_id"] = Session["CompId"].ToString();
                            dtrowLines["br_id"] = Session["BranchId"].ToString();
                            dtrowLines["issue_type"] = _MaterialIssue_Model.MRS_type;
                            dtrowLines["RewrkFlag"] = jObject[i]["FlagRwkJO"].ToString();
                            //dtrowLines["issue_no"] = _MaterialIssue_Model.MaterialIssueNo;
                            //dtrowLines["issue_dt"] = DateTime.Now;
                            dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                            dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                            dtrowLines["mrs_qty"] = jObject[i]["mrs_qty"].ToString();
                            dtrowLines["pend_qty"] = jObject[i]["pend_qty"].ToString();
                            dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                            dtrowLines["avl_stock"] = jObject[i]["avl_stock"].ToString();
                            dtrowLines["issue_qty"] = jObject[i]["issue_qty"].ToString();
                            dtrowLines["cost_pr"] = jObject[i]["CostPrice"].ToString();
                            dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                            if (_MaterialIssue_Model.DocumentMenuId == "105102130108")
                            {
                                dtrowLines["sr_type"] = jObject[i]["sr_type"].ToString();
                                dtrowLines["other_dtl"] = jObject[i]["other_dtl"].ToString();
                                dtrowLines["issue_date"] = jObject[i]["issue_date"].ToString();
                                dtrowLines["bin_loc"] = jObject[i]["bin_loc"].ToString();
                            }
                            else
                            {
                                dtrowLines["sr_type"] = null;
                                dtrowLines["other_dtl"] = null;
                                dtrowLines["issue_date"] = null;
                                dtrowLines["bin_loc"] = null;
                            }
                            dtItem.Rows.Add(dtrowLines);
                        }
                        ViewData["MaterialIssueItemDetails"] = dtitemdetail(jObject, _MaterialIssue_Model);
                    }
                    MaterialIssueItemDetails = dtItem;

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("comp_id", typeof(int));
                    Batch_detail.Columns.Add("br_id", typeof(int));
                    Batch_detail.Columns.Add("issue_type", typeof(string));
                    //Batch_detail.Columns.Add("issue_no", typeof(string));
                    //Batch_detail.Columns.Add("issue_dt", typeof(DateTime));
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("uom_id", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                    Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
                    Batch_detail.Columns.Add("issue_qty", typeof(string));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    Batch_detail.Columns.Add("mfg_name", typeof(string));
                    Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                    Batch_detail.Columns.Add("mfg_date", typeof(string));
                    if (_MaterialIssue_Model.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_MaterialIssue_Model.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                            dtrowBatchDetailsLines["issue_type"] = _MaterialIssue_Model.MRS_type;
                            //dtrowBatchDetailsLines["issue_no"] = _MaterialIssue_Model.MaterialIssueNo;
                            //dtrowBatchDetailsLines["issue_dt"] = DateTime.Now;
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
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }
                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();


                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectBatch[i]["mfg_name"].ToString(),null);
                            dtrowBatchDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(),null);
                            dtrowBatchDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectBatch[i]["mfg_date"].ToString(),null);
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                        ViewData["BatchDetails"] = dtBatchDetails(jObjectBatch, _MaterialIssue_Model);
                    }
                    ItemBatchDetails = Batch_detail;

                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("comp_id", typeof(int));
                    Serial_detail.Columns.Add("br_id", typeof(int));
                    Serial_detail.Columns.Add("issue_type", typeof(string));
                    //Serial_detail.Columns.Add("issue_no", typeof(string));
                    //Serial_detail.Columns.Add("issue_dt", typeof(DateTime));
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    Serial_detail.Columns.Add("uom_id", typeof(int));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("serial_qty", typeof(string));
                    Serial_detail.Columns.Add("issue_qty", typeof(string));
                    Serial_detail.Columns.Add("lot_no", typeof(string));
                    Serial_detail.Columns.Add("mfg_name", typeof(string));
                    Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                    Serial_detail.Columns.Add("mfg_date", typeof(string));

                    if (_MaterialIssue_Model.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_MaterialIssue_Model.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();

                            dtrowSerialDetailsLines["issue_type"] = _MaterialIssue_Model.MRS_type;
                            //dtrowSerialDetailsLines["issue_no"] = _MaterialIssue_Model.MaterialIssueNo;
                            //dtrowSerialDetailsLines["issue_dt"] = DateTime.Now;
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            dtrowSerialDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectSerial[i]["mfg_name"].ToString(),null);
                            dtrowSerialDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectSerial[i]["mfg_mrp"].ToString(),null);
                            dtrowSerialDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectSerial[i]["mfg_date"].ToString(),null);
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["SerialDetail"] = dtSerialDetails(jObjectSerial, _MaterialIssue_Model);
                    }
                    ItemSerialDetails = Serial_detail;

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    dtSubItem.Columns.Add("mrs_qty", typeof(string));
                    dtSubItem.Columns.Add("pend_qty", typeof(string));
                    dtSubItem.Columns.Add("avl_stock", typeof(string));
                    if (_MaterialIssue_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_MaterialIssue_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["mrs_qty"] = jObject2[i]["req_qty"].ToString();
                            dtrowItemdetails["pend_qty"] = jObject2[i]["pend_qty"].ToString();
                            dtrowItemdetails["avl_stock"] = jObject2[i]["avl_qty"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItem"] = dtSubItemDetails(jObject2);
                    }

                    /*------------------Sub Item end----------------------*/

                    string SaveMessage = _MaterialIssue_IServices.InsertUpdateMaterialIssue(MaterialIssuetHeader, MaterialIssueItemDetails, ItemBatchDetails, ItemSerialDetails, dtSubItem);
                    if (SaveMessage == "DocModify")
                    {
                        _MaterialIssue_Model.Message = "DocModify";
                        _MaterialIssue_Model.BtnName = "BtnRefresh";
                        _MaterialIssue_Model._mdlCommand = "Refresh";
                        TempData["ModelData"] = _MaterialIssue_Model;
                        return RedirectToAction("MaterialIssueDetail");
                    }
                    else if (SaveMessage == "Used")
                    {
                        _MaterialIssue_Model.Message = "Used";
                        _MaterialIssue_Model.BtnName = "BtnRefresh";
                        _MaterialIssue_Model._mdlCommand = "Refresh";
                        return RedirectToAction("MaterialIssueDetail");
                    }
                    else
                    {
                        string[] FDate = SaveMessage.Split(',');

                        string Message = FDate[0].ToString();
                        string MI_Number = FDate[1].ToString();
                        string MI_Date = FDate[2].ToString();
                        string MI_Type = FDate[3].ToString();

                        if (Message == "Data_Not_Found")
                        {
                            var msg = Message.Replace("_", " ") + " " + MI_Number + " in " + MI_Date;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _MaterialIssue_Model.Message = Message.Replace("_", "");
                            return RedirectToAction("MaterialIssueDetail");
                        }
                        if (Message == "StockNotAvail")
                        {
                            //Session["Message"] = "StockNotAvail_S";
                            _MaterialIssue_Model.StockItemWiseMessage = string.Join(",", FDate.Skip(4));
                            _MaterialIssue_Model.Message = "StockNotAvail_S";
                            //Session["BtnName"] = "BtnRefresh";
                            _MaterialIssue_Model.BtnName = "BtnRefresh";
                            _MaterialIssue_Model._mdlCommand = "Refresh";
                            //Session["Command"] = "Refresh";
                            //Session["TransType"] = "Save";
                            //Session["DocumentStatus"] = null;
                            //Session["MI_Date"] = MI_Date;
                            return RedirectToAction("MaterialIssueDetail");
                        }
                        if (Message == "Update" || Message == "Save")
                        {
                            try
                            {
                                string fileName = "MaterialIssue_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                var filePath = SavePdfDocToSendOnEmailAlert(MI_Number, MI_Date, MI_Type, fileName, DocumentMenuId, "AP");//_Model Added by Nidhi 05-05-25
                                _Common_IServices.SendAlertEmail(Session["CompId"].ToString(), Session["BranchId"].ToString(), DocumentMenuId, MI_Number, "AP", userid, "0", filePath);

                            }
                            catch (Exception exMail)
                            {
                                _MaterialIssue_Model.Message = "ErrorInMail";
                                string path = Server.MapPath("~");
                                Errorlog.LogError(path, exMail);
                            }
                           
                            //Session["MI_Type"] = MI_Type;
                            _MaterialIssue_Model.EntityType = MI_Type;
                            //Session["MI_Number"] = MI_Number;
                            _MaterialIssue_Model.MaterialIssueNo = MI_Number;
                            //Session["MI_Date"] = MI_Date;
                            _MaterialIssue_Model.MaterialIssueDate = MI_Date;// Convert.ToDateTime(MI_Date);

                            //Session["Message"] = "Save";
                            _MaterialIssue_Model.Message = _MaterialIssue_Model.Message == "ErrorInMail" ? "ErrorInMail" : "Save";
                            //_MaterialIssue_Model.Message = "Save";
                            //Session["Command"] = "Update";
                            _MaterialIssue_Model._mdlCommand = "Update";
                            _MaterialIssue_Model.TransType = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnEdit";
                            _MaterialIssue_Model.BtnName = "BtnEdit";
                            return RedirectToAction("MaterialIssueDetail");
                        }
                    }

                    return RedirectToAction("MaterialIssueDetail");
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
                    //if (Session["Document_Menu_Id"] != null)
                    //{
                    //    DocumentMenuId = Session["Document_Menu_Id"].ToString();
                    //}
                    DocumentMenuId = _MaterialIssue_Model.DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    string br_id = Session["BranchId"].ToString();
                    _MaterialIssue_Model.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                    string message = _MaterialIssue_IServices.MaterialIssueCancel(_MaterialIssue_Model, DocumentMenuId, CompID, br_id, mac_id);

                    if (message == "Cancelled")
                    {
                        try
                        {
                            string fileName = "MaterialIssue_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_MaterialIssue_Model.MaterialIssueNo, _MaterialIssue_Model.MaterialIssueDate, _MaterialIssue_Model.MRS_type, fileName, DocumentMenuId, "C");//_Model Added by Nidhi 05-05-25
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MaterialIssue_Model.MaterialIssueNo, "C", userid, "0", filePath);
                            //Session["Message"] = "Cancelled";
                            //_MaterialIssue_Model.Message = "Cancelled";
                        }

                        catch (Exception exMail)
                        {
                            _MaterialIssue_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _MaterialIssue_Model.Message = _MaterialIssue_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    }
                    if (message == "StockNotAvail")
                    {
                        //Session["Message"] = "StockNotAvail_C";
                        _MaterialIssue_Model.Message = "StockNotAvail_C";
                        _MaterialIssue_Model.CancelFlag = false;
                    }

                    //Session["Command"] = "Update";
                    _MaterialIssue_Model._mdlCommand = "Update";
                    //Session["TransType"] = "Update";
                    _MaterialIssue_Model.TransType = "Update";
                    //Session["BtnName"] = "Refresh";
                    _MaterialIssue_Model.BtnName = "Refresh";
                    return RedirectToAction("MaterialIssueDetail", _MaterialIssue_Model);
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

        public ActionResult checkDependency(MaterialIssue_Model _MaterialIssue_Model)
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
                var Issue_no = _MaterialIssue_Model.MaterialIssueNo;
                 var Issue_dt =   _MaterialIssue_Model.MaterialIssueDate;

                DataSet ds = _MaterialIssue_IServices.checkDependency(CompID, BrchID, Issue_no, Issue_dt);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if(ds.Tables[0].Rows[0]["UsedDocumant"].ToString()== "Used")
                    {
                        _MaterialIssue_Model.Message = "Used";
                    }
                    else
                    {
                        _MaterialIssue_Model.Message = null;
                    }
                    
                }
                return RedirectToAction("MaterialIssueDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public DataTable dtitemdetail(JArray jObject, MaterialIssue_Model _MaterialIssue_Model)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("comp_id", typeof(int));
            dtItem.Columns.Add("br_id", typeof(int));
            dtItem.Columns.Add("issue_type", typeof(string));
            dtItem.Columns.Add("RewrkFlag", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("base_uom_id", typeof(int));
            dtItem.Columns.Add("mrs_qty", typeof(string));
            dtItem.Columns.Add("pend_qty", typeof(string));
            dtItem.Columns.Add("wh_id", typeof(int));
            dtItem.Columns.Add("avl_stock", typeof(string));
            dtItem.Columns.Add("issue_qty", typeof(string));
            dtItem.Columns.Add("cost_pr", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("sr_type", typeof(string));
            dtItem.Columns.Add("other_dtl", typeof(string));
            dtItem.Columns.Add("issue_date", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("FlagRwkJO", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["comp_id"] = Session["CompId"].ToString();
                dtrowLines["br_id"] = Session["BranchId"].ToString();
                dtrowLines["issue_type"] = _MaterialIssue_Model.MRS_type;
                dtrowLines["RewrkFlag"] = jObject[i]["FlagRwkJO"].ToString();
                dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowLines["base_uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowLines["mrs_qty"] = jObject[i]["mrs_qty"].ToString();
                dtrowLines["pend_qty"] = jObject[i]["pend_qty"].ToString();
                dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                dtrowLines["avl_stock"] = jObject[i]["avl_stock"].ToString();
                dtrowLines["issue_qty"] = jObject[i]["issue_qty"].ToString();
                dtrowLines["cost_pr"] = jObject[i]["CostPrice"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                if (_MaterialIssue_Model.DocumentMenuId == "105102130108")
                {
                    dtrowLines["sr_type"] = jObject[i]["sr_type"].ToString();
                    dtrowLines["other_dtl"] = jObject[i]["other_dtl"].ToString();
                    dtrowLines["issue_date"] = jObject[i]["issue_date"].ToString();
                }
                else
                {
                    dtrowLines["sr_type"] = null;
                    dtrowLines["other_dtl"] = null;
                    dtrowLines["issue_date"] = null;
                }
                if (_MaterialIssue_Model.MRS_type == "S")
                {
                    dtrowLines["sub_item"] = "";
                }
                else
                {
                    dtrowLines["sub_item"] = jObject[i]["sub_item"].ToString();
                }
                dtrowLines["FlagRwkJO"] = jObject[i]["FlagRwkJO"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["uom_name"] = jObject[i]["UOM"].ToString();
                if (_MaterialIssue_Model.MRS_type == "S")
                {
                    dtrowLines["i_batch"] = "N";
                    dtrowLines["i_serial"] = "N";
                }
                else
                {
                    dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                    dtrowLines["i_serial"] = jObject[i]["i_serial"].ToString();
                }
                dtItem.Rows.Add(dtrowLines);
            }
            return dtItem;
        }
        public DataTable dtBatchDetails(JArray jObjectBatch, MaterialIssue_Model _MaterialIssue_Model)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("comp_id", typeof(int));
            Batch_detail.Columns.Add("br_id", typeof(int));
            Batch_detail.Columns.Add("issue_type", typeof(string));
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("uom_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
            Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
            Batch_detail.Columns.Add("Issue_Qty", typeof(string));
            Batch_detail.Columns.Add("lot_id", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));

            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowBatchDetailsLines["issue_type"] = _MaterialIssue_Model.MRS_type;
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
                    dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                }
                dtrowBatchDetailsLines["Issue_Qty"] = jObjectBatch[i]["IssueQty"].ToString();
                dtrowBatchDetailsLines["lot_id"] = jObjectBatch[i]["LotNo"].ToString();
                dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["ExpiryDate"].ToString();
                Batch_detail.Rows.Add(dtrowBatchDetailsLines);
            }
            return Batch_detail;
        }
        public DataTable dtSerialDetails(JArray jObjectSerial, MaterialIssue_Model _MaterialIssue_Model)
        {
            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("comp_id", typeof(int));
            Serial_detail.Columns.Add("br_id", typeof(int));
            Serial_detail.Columns.Add("issue_type", typeof(string));
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("uom_id", typeof(int));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("serial_qty", typeof(string));
            Serial_detail.Columns.Add("issue_qty", typeof(string));
            Serial_detail.Columns.Add("lot_no", typeof(string));

            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowSerialDetailsLines["issue_type"] = _MaterialIssue_Model.MRS_type;
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                Serial_detail.Rows.Add(dtrowSerialDetailsLines);
            }
            return Serial_detail;
        }
        public DataTable dtSubItemDetails(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("Qty", typeof(string));
            dtSubItem.Columns.Add("req_qty", typeof(string));
            dtSubItem.Columns.Add("pend_qty", typeof(string));
            dtSubItem.Columns.Add("avl_stk", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["Qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["req_qty"] = jObject2[i]["req_qty"].ToString();
                dtrowItemdetails["pend_qty"] = jObject2[i]["pend_qty"].ToString();
                dtrowItemdetails["avl_stk"] = jObject2[i]["avl_qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        private string getNextDocumentNumber()
        {
            try
            {
                string MenuDocumentId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                string CompId = Session["CompId"].ToString();
                string BranchId = Session["BranchId"].ToString();
                string Prefix = "MI";
                string NextDocumentNumber = _MaterialIssue_IServices.getNextDocumentNumber(CompID, BranchId, MenuDocumentId, Prefix);
                return NextDocumentNumber;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        public List<MaterialIssueDetail> GetMaterialIssueAllDetail(MaterialIssueList _MaterialIssueList, string RequisitionTyp)
        {
            try
            {
                List<MaterialIssueDetail> _MaterialIssueDetailList = new List<MaterialIssueDetail>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string br_id = Session["BranchId"].ToString();
                //DataSet DetailDatable = _MaterialIssue_IServices.GettMaterialIssueListAll(CompID, br_id);

                DataTable DetailDatable = _MaterialIssue_IServices.GetMaterialIssueDetailByFilter(CompID, BrchID, RequisitionTyp, "", "", "", "", "", "");
                //FromDate = Convert.ToDateTime(DetailDatable.Tables[1].Rows[0]["finstrdate"].ToString());
                DateTime dtnow = DateTime.Now;
                FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                // FromDate = DetailDatable.Tables[1].Rows[0]["finstrdate"].ToString();
                if (DetailDatable.Rows.Count > 0)
                {

                    foreach (DataRow dr in DetailDatable.Rows)
                    {
                        MaterialIssueDetail _MaterialIssueDetail = new MaterialIssueDetail();
                        _MaterialIssueDetail.issue_type = dr["issue_type"].ToString();
                        _MaterialIssueDetail.issuetype = dr["IssueType"].ToString().Trim();
                        _MaterialIssueDetail.issue_no = dr["issue_no"].ToString();
                        _MaterialIssueDetail.issue_dt = dr["issue_dt"].ToString();
                        _MaterialIssueDetail.issue_date = dr["issue_date"].ToString().Trim();
                        _MaterialIssueDetail.issue_to = dr["issue_to"].ToString();
                        _MaterialIssueDetail.issue_by = dr["issue_by"].ToString();
                        _MaterialIssueDetail.requisition_no = dr["mrs_no"].ToString();
                        _MaterialIssueDetail.requisition_date = dr["mrs_dt"].ToString();
                        _MaterialIssueDetail.entity_type = dr["ReqArea"].ToString();
                        _MaterialIssueDetail.app_dt = dr["app_dt"].ToString();
                        _MaterialIssueDetail.create_dt = dr["create_dt"].ToString();
                        _MaterialIssueDetail.mod_dt = dr["mod_dt"].ToString();
                        _MaterialIssueDetail.issue_status = dr["issue_status"].ToString();
                        _MaterialIssueDetail.create_by = dr["create_by"].ToString();
                        _MaterialIssueDetail.mod_by = dr["mod_by"].ToString();
                        _MaterialIssueDetailList.Add(_MaterialIssueDetail);
                    }
                }
                return _MaterialIssueDetailList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //public DataSet GetMaterialIssueToList()
        //{
        //    try
        //    {
        //        string CompId = Session["CompId"].ToString();
        //        string BranchId = Session["BranchId"].ToString();
        //        DataSet ds = _MaterialIssue_IServices.GetMaterialIssueToList(CompID, BranchId);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }

        //}
        public ActionResult GetAutoCompleteSearchIssueTo(MaterialIssue_Model _MaterialIssue_Model)
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
                if (string.IsNullOrEmpty(_MaterialIssue_Model.SearchIssueToName))
                {
                    Entity = "0";
                }
                else
                {
                    Entity = _MaterialIssue_Model.SearchIssueToName;
                }
                if (string.IsNullOrEmpty(_MaterialIssue_Model.sr_type))
                {
                    sr_type = "0";
                }
                else
                {
                    sr_type = _MaterialIssue_Model.sr_type;
                }
                EntityList = _MaterialIssue_IServices.IssueToList(Comp_ID, Entity, BrchID, sr_type);


                List<EntityType> _EntityTypeList = new List<EntityType>();
                foreach (var data in EntityList)
                {
                    EntityType _EntityType = new EntityType();
                    _EntityType.Entity_ID = data.Key;
                    _EntityType.Entity_Name = data.Value;
                    _EntityTypeList.Add(_EntityType);
                }
                _MaterialIssue_Model.EntityTypelist = _EntityTypeList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(EntityList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId
            , string SelectedItemSerial, string DMenuId, string Command, string TransType, string MRSNo, string HdnitmRJOFlag)
        {
            try
            {
                DataSet ds = new DataSet();
                if (HdnitmRJOFlag == "RWK")
                {
                    if (ItemId != "")
                    {
                        ds = _MaterialIssue_IServices.getItemStockSerialWisefromRwkJO(ItemId, WarehouseId, CompId, BranchId, MRSNo);
                    }
                }
                else
                {
                    if (ItemId != "")
                        ds = _MaterialIssue_IServices.getItemstockSerialWise(ItemId, WarehouseId, CompId, BranchId);
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

                //if (Session["MenuDocumentId"] != null)  //Commented by Suraj on 15-12-2022 for removing session dependency
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105102130101")
                //    {
                //        DocumentMenuId = "105102130101";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130105")
                //    {
                //        DocumentMenuId = "105102130105";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130108")
                //    {
                //        DocumentMenuId = "105102130108";
                //    }
                //}
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                //return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialIssueItemStockSerialWise.cshtml");
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string IssueType, string IssueNo, string IssueDate, string ItemID
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
                ds = _MaterialIssue_IServices.getItemstockSerialWiseAfterStockUpdate(CompID, br_id, IssueType, IssueNo, IssueDate, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105102130101")
                //    {
                //        DocumentMenuId = "105102130101";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130105")
                //    {
                //        DocumentMenuId = "105102130105";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130108")
                //    {
                //        DocumentMenuId = "105102130108";
                //    }
                //}
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                //return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialIssueItemStockSerialWise.cshtml");
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId
            , string SelectedItemdetail, string DMenuId, string Command, string TransType, string MRSNo, string HdnitmRJOFlag, string UomId = null)
        {
            try
            {
                DataSet ds = new DataSet();
                if (HdnitmRJOFlag == "RWK")
                {
                    if (ItemId != "")
                    {
                        ds = _MaterialIssue_IServices.getItemStockBatchWisefromRwkJO(ItemId, UomId, WarehouseId, CompId, BranchId, MRSNo);
                    }
                }
                else
                {
                    if (ItemId != "")
                    {
                        ds = _MaterialIssue_IServices.getItemStockBatchWise(ItemId, UomId, WarehouseId, CompId, BranchId);
                    }
                }
                //if (ItemId != "")
                //{
                //    ds = _MaterialIssue_IServices.getItemStockBatchWise(ItemId, WarehouseId, CompId, BranchId);
                //}

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

                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105102130101")
                //    {
                //        DocumentMenuId = "105102130101";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130105")
                //    {
                //        DocumentMenuId = "105102130105";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130108")
                //    {
                //        DocumentMenuId = "105102130108";
                //    }
                //}
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                //Session["TransType"] = TransType==null?"": TransType;
                //Session["Command"] = Command==null?"": Command;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemStockBatchWiseAfterStockUpadte(string IssueType, string IssueNo, string IssueDate, string ItemID
            , string DMenuId, string Command, string TransType, string UomId = null)
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
                ds = _MaterialIssue_IServices.getItemStockBatchWiseAfterStockUpdate(CompID, br_id, IssueType, IssueNo, IssueDate, ItemID, UomId);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105102130101")
                //    {
                //        DocumentMenuId = "105102130101";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130105")
                //    {
                //        DocumentMenuId = "105102130105";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105102130108")
                //    {
                //        DocumentMenuId = "105102130108";
                //    }
                //}
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                //Session["TransType"] = TransType == null ? "" : TransType;
                //Session["Command"] = Command == null ? "" : Command;
                //return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialIssueItemStockBatchWise.cshtml");//
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult MaterialIssuetListSearch(string RequisitionTyp, string RequiredArea, string MaterialIssueTo, string Fromdate, string Todate, string Status, string flag, string Docid)
        {
            try
            {
                MaterialIssueList _MaterialIssueList = new MaterialIssueList();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                _MaterialIssueList.DocumentMenuId = Docid;
                List<MaterialIssueDetail> _MaterialIssueDetailList = new List<MaterialIssueDetail>();
                DataTable DetailDatable = _MaterialIssue_IServices.GetMaterialIssueDetailByFilter(CompID, BrchID, RequisitionTyp, RequiredArea, MaterialIssueTo, Fromdate, Todate, Status, flag);
                if (flag == "DetailPage")
                {
                    MaterialIssue_Model _MaterialIssue_Model = new MaterialIssue_Model();
                    if (DetailDatable.Rows.Count > 0)
                    {
                        ViewBag.Issuedtotabledata = DetailDatable;
                    }
                    _MaterialIssue_Model.MISearch = "Issue_data";
                    //Session["MISearch"] = "MI_Search";
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialIssueDetail.cshtml", _MaterialIssue_Model);
                }
                else
                {
                    if (DetailDatable.Rows.Count > 0)
                    {

                        foreach (DataRow dr in DetailDatable.Rows)
                        {
                            MaterialIssueDetail _MaterialIssueDetail = new MaterialIssueDetail();
                            _MaterialIssueDetail.issue_type = dr["issue_type"].ToString();
                            _MaterialIssueDetail.issuetype = dr["IssueType"].ToString().Trim();
                            _MaterialIssueDetail.issue_no = dr["issue_no"].ToString();
                            _MaterialIssueDetail.issue_dt = dr["issue_dt"].ToString();
                            _MaterialIssueDetail.issue_date = dr["issue_date"].ToString().Trim();
                            _MaterialIssueDetail.issue_to = dr["issue_to"].ToString();
                            _MaterialIssueDetail.issue_by = dr["issue_by"].ToString();
                            _MaterialIssueDetail.requisition_no = dr["mrs_no"].ToString();
                            _MaterialIssueDetail.requisition_date = dr["mrs_dt"].ToString();
                            _MaterialIssueDetail.entity_type = dr["ReqArea"].ToString();
                            _MaterialIssueDetail.app_dt = dr["app_dt"].ToString();
                            _MaterialIssueDetail.create_dt = dr["create_dt"].ToString();
                            _MaterialIssueDetail.mod_dt = dr["mod_dt"].ToString();
                            _MaterialIssueDetail.issue_status = dr["issue_status"].ToString();
                            _MaterialIssueDetail.create_by = dr["create_by"].ToString();
                            _MaterialIssueDetail.mod_by = dr["mod_by"].ToString();
                            _MaterialIssueDetailList.Add(_MaterialIssueDetail);
                        }
                    }
                    _MaterialIssueList.MaterialIssueDetailList = _MaterialIssueDetailList;
                    //Session["MISearch"] = "MI_Search";
                    _MaterialIssueList.MISearch = "MI_Search";
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialIssueList.cshtml", _MaterialIssueList);
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }



        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
   , string Flag, string Status, string Doc_no, string Doc_dt, string MI_Type, string wh_id, string RwkJobOrdFlag, string UomId)
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
                    if (RwkJobOrdFlag == "RWK")
                    {
                        dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, UomId, "Rwkwh").Tables[0];
                    }
                    else
                    {
                        dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, UomId, "wh").Tables[0];
                    }
                    //dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    //dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID,BrchID,wh_id, Item_id,"wh").Tables[0];
                    dt.Columns.Add("Qty", typeof(string));
                    dt.Columns.Add("req_qty", typeof(string));
                    dt.Columns.Add("pend_qty", typeof(string));
                    //dt.Columns.Add("avl_stk", typeof(string));
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
                    string flag = "N";
                    int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        flag = "N";
                        foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                                dt.Rows[i]["req_qty"] = cmn.ConvertToDecimal(item.GetValue("req_qty").ToString(), DecDgt);
                                dt.Rows[i]["pend_qty"] = cmn.ConvertToDecimal(item.GetValue("pend_qty").ToString(), DecDgt);
                                //dt.Rows[i]["avl_stk"] = item.GetValue("avl_qty");
                                flag = "Y";
                            }
                        }

                        if (flag == "N")
                        {
                            dt.Rows[i].Delete();
                            //dt.Rows[i].AcceptChanges();
                        }



                    }
                    for (var i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        dt.Rows[i].AcceptChanges();
                    }
                }

                else
                {
                    dt = _MaterialIssue_IServices.MI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag == "IssueQty" ? Flag : "MTI",
                    dt_SubItemDetails = dt,
                    _subitemPageName = MI_Type == "S" ? "SampleIssue" : "MTI",
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
        /*--------------------------For PDF Print Start--------------------------*/
        public FileResult GenratePdfFile(MaterialIssue_Model _MaterialIssue_Model)
        {
            var IssueType = "";
            if (_MaterialIssue_Model.DocumentMenuId == "105102130101")
            {
                IssueType = "I";
            }
            if (_MaterialIssue_Model.DocumentMenuId == "105102130105")
            {
                IssueType = "E";
            }
            if (_MaterialIssue_Model.DocumentMenuId == "105102130108")
            {
                IssueType = "S";
            }
            ViewBag.DocumentMenuId = _MaterialIssue_Model.DocumentMenuId;
            return File(GetPdfData(_MaterialIssue_Model.MaterialIssueNo, _MaterialIssue_Model.MaterialIssueDate, _MaterialIssue_Model.StatusCode, IssueType), "application/pdf", "MaterialIssue.pdf");
        }
        public byte[] GetPdfData(string Doc_No, string Doc_dt, string StatusCode, string IssueType)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            string CompID = string.Empty;
            string BrchID = string.Empty;
            string htmlcontent = "";

            try
            {
                MaterialIssue_Model _MaterialIssue_Model = new MaterialIssue_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataSet Details = _MaterialIssue_IServices.GetMRSDeatilsForPrint(CompID, BrchID, Doc_No, Convert.ToDateTime(Doc_dt).ToString("yyyy-MM-dd"), IssueType);
                ViewBag.Details = Details;
                ViewBag.DocStatus = StatusCode;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                if (IssueType == "E")
                {
                    _MaterialIssue_Model.DocumentMenuId = "105102130105";
                    ViewBag.Title = "External Issue";
                    ViewBag.DocumentMenuId = _MaterialIssue_Model.DocumentMenuId;
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/ExternalIssuePrint.cshtml"));

                }
                else if (IssueType == "I")
                {
                    _MaterialIssue_Model.DocumentMenuId = "105102130101";
                    ViewBag.Title = "Internal Issue";
                    ViewBag.DocumentMenuId = _MaterialIssue_Model.DocumentMenuId;
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssuePrint.cshtml"));

                }
                else
                {
                    _MaterialIssue_Model.DocumentMenuId = "105102130108";
                    ViewBag.Title = "Sample Issue Slip";
                    ViewBag.DocumentMenuId = _MaterialIssue_Model.DocumentMenuId;
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssuePrint.cshtml"));

                }
                //string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssuePrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 20f, 100f);
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 80, 0);
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
            return null;
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
        public ActionResult GetPendingDocument(string Docid, string ItemID, string flag)
        {
            try
            {
                MaterialIssueList _MaterialIssueList = new MaterialIssueList();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                _MaterialIssueList.DocumentMenuId = Docid;
                List<MaterialIssueDetail> _MaterialIssueDetailList = new List<MaterialIssueDetail>();
                DataTable DetailDatatable = new DataTable();
                if (Docid == "105102130101")
                {
                    DetailDatatable = _MaterialIssue_IServices.GetPendingDocumentData(CompID, BrchID,
                        Docid, language, ItemID, flag).Tables[0];
                    ViewBag.PendingDocument = DetailDatatable;
                }
                else
                {
                    ViewBag.PendingDocument = null;
                }

                _MaterialIssueList.MISearch = "PendingData";


                return PartialView("~/Areas/ApplicationLayer/Views/Shared/MIPartialPendingSourceDocument.cshtml", _MaterialIssueList);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }



        public ActionResult AddNewDocumentMRSPending(string mrs_no, string mrs_dt, string req_id, string mrs_type)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            MaterialIssue_Model _MaterialIssue_Model = new MaterialIssue_Model();

            _MaterialIssue_Model.MRS_No = mrs_no.Trim();
            _MaterialIssue_Model.MRS_Date = mrs_dt.Trim().ToString();
            _MaterialIssue_Model.RequiredArea = req_id.Trim();
            _MaterialIssue_Model.MRS_type = mrs_type.Trim();

            var reqType = _MaterialIssue_Model.MRS_type;
            if (reqType != null)
            {
                if (reqType == "I")
                {
                    DocumentMenuId = "105102130101";
                    ViewBag.DocumentMenuId = "105102130101";
                }
            }
            _MaterialIssue_Model.MRS_type = _MaterialIssue_Model.MRS_type;
            _MaterialIssue_Model.MaterialIssueNo = null;
            _MaterialIssue_Model.MaterialIssueDate = null;
            _MaterialIssue_Model.TransType = "Save";
            _MaterialIssue_Model.BtnName = "BtnAddNew";
            _MaterialIssue_Model._mdlCommand = "AddNew";
            CommonPageDetails();
            DataSet ds = _MaterialIssue_IServices.GetRequirmentreaList(CompID, BrchID, reqType);
            List<RequiredArea> RequiredAreaList = new List<RequiredArea>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RequiredArea _RequiredAreaList = new RequiredArea();
                _RequiredAreaList.ReqArea_ID = Convert.ToInt32(dr["setup_id"]);
                _RequiredAreaList.ReqArea_Name = dr["setup_val"].ToString();
                RequiredAreaList.Add(_RequiredAreaList);
            }
            RequiredArea _RequiredAreaList2 = new RequiredArea();
            _RequiredAreaList2.ReqArea_ID = 0;
            _RequiredAreaList2.ReqArea_Name = "---Select---";
            RequiredAreaList.Insert(0, _RequiredAreaList2);
            _MaterialIssue_Model.RequiredAreaList = RequiredAreaList;




            List<MRS_NO> _MRS_NOList = new List<MRS_NO>();
            MRS_NO _MRS_NO = new MRS_NO();
            _MRS_NO.MaterialIssueDate = _MaterialIssue_Model.MRS_Date;
            _MRS_NO.MaterialIssueNo = _MaterialIssue_Model.MRS_No;
            _MRS_NOList.Add(_MRS_NO);

            List<EntityType> _EntityTypeList = new List<EntityType>();
            EntityType _EntityType2 = new EntityType();
            _EntityType2.Entity_ID = "0";
            _EntityType2.Entity_Name = "---Select---";
            _EntityType2.Entity_Type = "0";
            _EntityTypeList.Insert(0, _EntityType2);
            _MaterialIssue_Model.EntityTypelist = _EntityTypeList;
            _MaterialIssue_Model.CompId = CompID;
            _MaterialIssue_Model.BrchID = BrchID;
            _MaterialIssue_Model.MRS_NO_List = _MRS_NOList;
            getWarehouse(_MaterialIssue_Model);
            //ViewBag.MenuPageName = getDocumentName();
            _MaterialIssue_Model.MaterialIssueDate = _MaterialIssue_Model.MaterialIssueDate == null ? DateTime.Now.ToString("yyyy-MM-dd") : _MaterialIssue_Model.MaterialIssueDate;
            _MaterialIssue_Model.Title = title;
            _MaterialIssue_Model.DocumentMenuId = DocumentMenuId;
            DataSet mrsData = _MaterialIssue_IServices.GetMaterialRequisitionItemDetailByNO(CompID, BrchID, _MaterialIssue_Model.MRS_Date, _MaterialIssue_Model.MRS_No, _MaterialIssue_Model.MRS_type);
            _MaterialIssue_Model.ListPendingCreateDocument = "CreateDocument";
            ViewBag.ItemDetails = mrsData.Tables[0];
            ViewBag.SubItemDetails = mrsData.Tables[1];
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/InteralIssue/MaterialIssueDetail.cshtml", _MaterialIssue_Model);
        }
        public ActionResult GetPendingDocumentitemDetail(string doc_no, string doc_dt)
        {
            try
            {
                MaterialIssueList _MaterialIssueList = new MaterialIssueList();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                List<MaterialIssueDetail> _MaterialIssueDetailList = new List<MaterialIssueDetail>();
                DataTable itemDetail = new DataTable();

                itemDetail = _MaterialIssue_IServices.GetPendingDocumentDataitemdetail(CompID, BrchID, doc_no, doc_dt).Tables[0];
                ViewBag.PendingItemDetail = itemDetail;
                return PartialView("~/Areas/Common/Views/Cmn_ItemInformationPSDList.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt,string MI_Type, string fileName, string docid, string docstatus)
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
            var commonCont = new CommonController(_Common_IServices);
            string mailattch = commonCont.CheckMailAttch(Comp_ID, Br_ID, docid, docstatus);
            if (!string.IsNullOrEmpty(mailattch))
            {
                if (mailattch.Trim() == "Yes")
                {
                    var data = GetPdfData(Doc_no, Doc_dt, docstatus, MI_Type);
                    return commonCont.SaveAlertDocument(data, fileName);
                }
            }
            return null;
        }
        
    }
}
