using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.CustomerPriceList;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomerPriceList;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.CustomerPriceList
{
    public class CustomerPriceListController : Controller
    {
      
        DateTime valid_fr;
        DateTime valid_to;
        string comp_id, userid, language /*CustId*/,title = String.Empty;
        string CompID, BrchID = string.Empty;
        List<ListPageList> _ListPageList;
        PriceListDetailModel _PriceListModel;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        string DocumentMenuId = "105103101";
        Common_IServices _Common_IServices;
        CustomerPriceList_ISERVICES _CustomerPriceList_ISERVICES;
        // GET: ApplicationLayer/CustomerPriceList

        public CustomerPriceListController(Common_IServices _Common_IServices,CustomerPriceList_ISERVICES _CustomerPriceList_ISERVICES)
        {
            this._CustomerPriceList_ISERVICES = _CustomerPriceList_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }

        public ActionResult PriceList(PriceListDetailModel _dashbord)
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
                string wfstatus = "";
                PriceListDetailModel _PriceListModel = new PriceListDetailModel();
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if (_dashbord.WF_status != null)
                {
                    wfstatus = _dashbord.WF_status;
                    _PriceListModel.WF_status = _dashbord.WF_status;
                }
                else
                {
                    wfstatus = "0";
                }      
                
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _PriceListModel.CustomerPriceList = GetPriceList(_PriceListModel);
                ViewBag.MenuPageName = getDocumentName();
              
                _PriceListModel.Title = title;
                ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomerPriceList/PriceList.cshtml", _PriceListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ToRefreshByJS(string DashBord)
        {
            //Session["Message"] = "";
            PriceListDetailModel _PriceforwardModel = new PriceListDetailModel();
            UrlModel _UrlModel = new UrlModel();
            _PriceforwardModel.Message = "";
            //Session["Message"] = "";        
            var a = DashBord.Split(',');
            _PriceforwardModel.MenuDocumentId = a[0].Trim();
            _PriceforwardModel.PriceListNo = a[1].Trim(); 
            var WF_status1 = a[2].Trim();
            if (WF_status1 != null && WF_status1 != "")
            {
                _PriceforwardModel.WF_status1 = WF_status1;
                _UrlModel.WF_status1 = _PriceforwardModel.WF_status1;
            }
            _PriceforwardModel.TransType = "Update";
            _PriceforwardModel.BtnName = "BtnToDetailPage";
            _PriceforwardModel.Message = null;
            TempData["ModelData"] = _PriceforwardModel;           
            _UrlModel.PriceListNo = _PriceforwardModel.PriceListNo;           
            _UrlModel.TransType = "Update";
            _UrlModel.BtnName = "BtnToDetailPage";
            return RedirectToAction("PriceDetail", _UrlModel);
        }
        public ActionResult GetCustomerPriceList(string docid, string status)
        {
            PriceListDetailModel _dashbord = new PriceListDetailModel();
            //Session["WF_status"] = status;
            _dashbord.WF_status = status;
            //Session["MenuDocumentId"] = docid;
            _dashbord.MenuDocumentId = docid;
            return RedirectToAction("PriceList", _dashbord);
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
                    userid = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //public ViewDataDictionary ViewData { get; set; }
        public ActionResult PriceDetail(UrlModel _urlModel)
        {
            try {
                ViewBag.DocID = DocumentMenuId;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                var ModelData = TempData["ModelData"] as PriceListDetailModel;
                if(ModelData != null)
                {
                 
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    ViewBag.MenuPageName = getDocumentName();
                    //ModelData = new PriceListDetailModel();
                    ModelData.Title = title;
                    string Comp_ID = string.Empty;
                    ModelData.list_no = 0;



                    dt = GetCustPriceGrp();
                    List<PriceGroup> _PriceGroupList = new List<PriceGroup>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        PriceGroup _PriceGroup = new PriceGroup();
                        _PriceGroup.setup_id = dr["setup_id"].ToString();
                        _PriceGroup.setup_val = dr["setup_val"].ToString();
                        _PriceGroupList.Add(_PriceGroup);
                    }
                    _PriceGroupList.Insert(0, new PriceGroup() { setup_id = "0", setup_val = "---Select---" });
                    ModelData.PriceGroupList = _PriceGroupList;
                    DataTable dts = GetPriceListName();
                    List<PriceListName> _PriceListName = new List<PriceListName>();
                    foreach (DataRow dr in dts.Rows)
                    {
                        PriceListName _PriceName = new PriceListName();
                        _PriceName.list_no = dr["list_no"].ToString();
                        _PriceName.list_name = dr["list_name"].ToString();
                        _PriceListName.Add(_PriceName);
                    }
                    _PriceListName.Insert(0, new PriceListName() { list_no = "0", list_name = "---Select---" });
                    ModelData.PriceListName = _PriceListName;

                    string Statuscode = "New";
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    //if (Session["TransType"] != null)
                    if (ModelData.TransType != null)
                    {
                        //if (Session["TransType"].ToString() == "Update")
                        if (ModelData.TransType == "Update")
                        {

                            if (Session["userid"] != null)
                            {
                                userid = Session["userid"].ToString();
                            }
                            //string ListNo = Session["PriceListNo"].ToString();
                            string ListNo = ModelData.PriceListNo;

                            if (Session["CompId"] != null)
                            {
                                Comp_ID = Session["CompId"].ToString();
                            }
                            BrchID = Session["BranchId"].ToString();

                            Boolean act_stats = true;
                            //Session["MenuDocumentId"] = "105103101";
                            ModelData.MenuDocumentId = "105103101";
                            DocumentMenuId = "105103101";

                            DataSet ds = _CustomerPriceList_ISERVICES.GetviewPriceListdetail(ListNo, BrchID, Comp_ID, userid, DocumentMenuId);

                            if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                                act_stats = true;
                            else
                                act_stats = false;

                            //create_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["create_dt"]);
                            valid_fr = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_fr"]);
                            valid_to = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_to"]);
                            ModelData.list_no = int.Parse(ds.Tables[0].Rows[0]["list_no"].ToString());
                            ModelData.list_name = ds.Tables[0].Rows[0]["list_name"].ToString();
                            //ModelData.valid_fr = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_fr"].ToString());
                            //ModelData.valid_to = DateTime.Parse(ds.Tables[0].Rows[0]["valid_to"].ToString());
                            ModelData.List_remarks = ds.Tables[0].Rows[0]["List_remarks"].ToString();
                            ModelData.act_status = act_stats;
                            ModelData.list_status = ds.Tables[0].Rows[0]["status_code"].ToString();
                            ModelData.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                            ModelData.create_id = ds.Tables[0].Rows[0]["creater_id"].ToString();
                            ModelData.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            ModelData.create_dt = ds.Tables[0].Rows[0]["create_dt1"].ToString();

                            ModelData.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                            ModelData.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                            ModelData.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            ModelData.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            ModelData.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                            //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                            ModelData.DocumentStatus = ds.Tables[0].Rows[0]["status_code"].ToString();
                            ModelData.CPL_status = ds.Tables[0].Rows[0]["status_code"].ToString();
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.PriceGroupDetail = ds.Tables[2];
                            ModelData.valid_fr = valid_fr.ToString("yyyy-MM-dd");
                            ModelData.valid_to = valid_to.ToString("yyyy-MM-dd");
                            //ModelData.create_dt = create_dt.ToString("yyyy-MM-dd");

                            //Session["DocumentStatus"] = ModelData.CPL_status;
                            ModelData.DocumentStatus = ModelData.CPL_status;
                            ViewBag.DocumentCode = ModelData.CPL_status;
                            Statuscode = /*ds.Tables[0].Rows[0]["status_code"].ToString().Trim();*/ ModelData.CPL_status;
                            string create_id = ds.Tables[0].Rows[0]["creater_id"].ToString();
                            string UserID = Session["UserID"].ToString();
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string Doc_Status = ds.Tables[0].Rows[0]["status_code"].ToString();

                            ModelData.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            ModelData.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                            if (Doc_Status != "D" && Doc_Status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[5];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && ModelData.Command != "Edit")
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
                                        //Session["BtnName"] = "BtnRefresh";
                                        ModelData.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            ModelData.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            ModelData.BtnName = "BtnToDetailPage";
                                        }

                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        ModelData.BtnName = "BtnToDetailPage";
                                    }


                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            ModelData.BtnName = "BtnToDetailPage";
                                        }


                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        ModelData.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        ModelData.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        ModelData.BtnName = "BtnToDetailPage";

                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        ModelData.BtnName = "BtnRefresh";
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
                            ModelData.act_status = true;
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        // ModelData.TransType = Session["TransType"].ToString();
                        ViewBag.VBRoleList = GetRoleList();
                        //Session["DocumentStatus"] = Statuscode;
                        ModelData.DocumentStatus = Statuscode;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomerPriceList/PriceDetail.cshtml",ModelData);

                    }
                    else
                    {
                        ModelData.act_status = true;
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    //ModelData.TransType = Session["TransType"].ToString();
                    ViewBag.VBRoleList = GetRoleList();
                    //Session["DocumentStatus"] = Statuscode;
                    ModelData.DocumentStatus = Statuscode;
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomerPriceList/PriceDetail.cshtml",ModelData);
                }
                else
                {
                    PriceListDetailModel _PriceListModel = new PriceListDetailModel();
                    if (_urlModel.PriceListNo != null && _urlModel.PriceListNo != "")
                    {
                        _PriceListModel.PriceListNo = _urlModel.PriceListNo;
                    }
                    if (_urlModel.TransType != null)
                    {
                        _PriceListModel.TransType = _urlModel.TransType;
                    }
                    else
                    {
                        _PriceListModel.TransType = "New";
                    }
                    if (_urlModel.BtnName != null)
                    {
                        _PriceListModel.BtnName = _urlModel.BtnName;
                    }
                    else
                    {
                        _PriceListModel.BtnName = "BtnRefresh";
                    }
                    if (_urlModel.Command != null)
                    {
                        _PriceListModel.Command = _urlModel.Command;
                    }
                    else
                    {
                        _PriceListModel.Command = "Refresh";
                    }
                    if (_urlModel.WF_status1 != null)
                    {
                        _PriceListModel.WF_status1 = _urlModel.WF_status1;
                    }
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    ViewBag.MenuPageName = getDocumentName();
                    //_PriceListModel = new PriceListDetailModel();
                    _PriceListModel.Title = title;
                    string Comp_ID = string.Empty;
                    _PriceListModel.list_no = 0;



                    dt = GetCustPriceGrp();
                    List<PriceGroup> _PriceGroupList = new List<PriceGroup>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        PriceGroup _PriceGroup = new PriceGroup();
                        _PriceGroup.setup_id = dr["setup_id"].ToString();
                        _PriceGroup.setup_val = dr["setup_val"].ToString();
                        _PriceGroupList.Add(_PriceGroup);
                    }
                    _PriceGroupList.Insert(0, new PriceGroup() { setup_id = "0", setup_val = "---Select---" });
                    _PriceListModel.PriceGroupList = _PriceGroupList;

                    DataTable dts = GetPriceListName();
                    List<PriceListName> _PriceListName = new List<PriceListName>();
                    foreach (DataRow dr in dts.Rows)
                    {
                        PriceListName _PriceName = new PriceListName();
                        _PriceName.list_no = dr["list_no"].ToString();
                        _PriceName.list_name = dr["list_name"].ToString();
                        _PriceListName.Add(_PriceName);
                    }
                    _PriceListName.Insert(0, new PriceListName() { list_no = "0", list_name = "---Select---" });
                    _PriceListModel.PriceListName = _PriceListName;
                    string Statuscode = "New";
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }

                    //if (Session["TransType"] != null)
                    if (_PriceListModel.TransType != null)
                    {
                        //if (Session["TransType"].ToString() == "Update")
                        if (_PriceListModel.TransType == "Update")
                        {

                            if (Session["userid"] != null)
                            {
                                userid = Session["userid"].ToString();
                            }
                            //string ListNo = Session["PriceListNo"].ToString();
                            string ListNo = _PriceListModel.PriceListNo;

                            if (Session["CompId"] != null)
                            {
                                Comp_ID = Session["CompId"].ToString();
                            }
                            BrchID = Session["BranchId"].ToString();

                            Boolean act_stats = true;
                            //Session["MenuDocumentId"] = "105103101";
                            _PriceListModel.MenuDocumentId = "105103101";
                            DocumentMenuId = "105103101";

                            DataSet ds = _CustomerPriceList_ISERVICES.GetviewPriceListdetail(ListNo, BrchID, Comp_ID, userid, DocumentMenuId);

                            if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                                act_stats = true;
                            else
                                act_stats = false;

                            //create_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["create_dt"]);
                            valid_fr = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_fr"]);
                            valid_to = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_to"]);
                            _PriceListModel.list_no = int.Parse(ds.Tables[0].Rows[0]["list_no"].ToString());
                            _PriceListModel.list_name = ds.Tables[0].Rows[0]["list_name"].ToString();
                            //_PriceListModel.valid_fr = Convert.ToDateTime(ds.Tables[0].Rows[0]["valid_fr"].ToString());
                            //_PriceListModel.valid_to = DateTime.Parse(ds.Tables[0].Rows[0]["valid_to"].ToString());
                            _PriceListModel.List_remarks = ds.Tables[0].Rows[0]["List_remarks"].ToString();
                            _PriceListModel.act_status = act_stats;
                            _PriceListModel.list_status = ds.Tables[0].Rows[0]["status_code"].ToString();
                            _PriceListModel.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _PriceListModel.create_id = ds.Tables[0].Rows[0]["creater_id"].ToString();
                            _PriceListModel.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            _PriceListModel.create_dt = ds.Tables[0].Rows[0]["create_dt1"].ToString();

                            _PriceListModel.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                            _PriceListModel.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                            _PriceListModel.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            _PriceListModel.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            _PriceListModel.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                            //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                            _PriceListModel.DocumentStatus = ds.Tables[0].Rows[0]["status_code"].ToString();
                            _PriceListModel.CPL_status = ds.Tables[0].Rows[0]["status_code"].ToString();
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.PriceGroupDetail = ds.Tables[2];
                            _PriceListModel.valid_fr = valid_fr.ToString("yyyy-MM-dd");
                            _PriceListModel.valid_to = valid_to.ToString("yyyy-MM-dd");
                            //_PriceListModel.create_dt = create_dt.ToString("yyyy-MM-dd");

                            //Session["DocumentStatus"] = _PriceListModel.CPL_status;
                            _PriceListModel.DocumentStatus = _PriceListModel.CPL_status;
                            ViewBag.DocumentCode = _PriceListModel.CPL_status;
                            Statuscode = /*ds.Tables[0].Rows[0]["status_code"].ToString().Trim();*/ _PriceListModel.CPL_status;
                            string create_id = ds.Tables[0].Rows[0]["creater_id"].ToString();
                            string UserID = Session["UserID"].ToString();
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string Doc_Status = ds.Tables[0].Rows[0]["status_code"].ToString();

                            _PriceListModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            _PriceListModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                            if (Doc_Status != "D" && Doc_Status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[5];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _PriceListModel.Command != "Edit")
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
                                        //Session["BtnName"] = "BtnRefresh";
                                        _PriceListModel.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _PriceListModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _PriceListModel.BtnName = "BtnToDetailPage";
                                        }

                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _PriceListModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _PriceListModel.BtnName = "BtnToDetailPage";
                                        }


                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _PriceListModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _PriceListModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _PriceListModel.BtnName = "BtnToDetailPage";

                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _PriceListModel.BtnName = "BtnRefresh";
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
                            _PriceListModel.act_status = true;
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        // _PriceListModel.TransType = Session["TransType"].ToString();
                        ViewBag.VBRoleList = GetRoleList();
                        //Session["DocumentStatus"] = Statuscode;
                        _PriceListModel.DocumentStatus = Statuscode;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomerPriceList/PriceDetail.cshtml", _PriceListModel);

                    }
                    else
                    {
                        _PriceListModel.act_status = true;
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    //_PriceListModel.TransType = Session["TransType"].ToString();
                    ViewBag.VBRoleList = GetRoleList();
                    //Session["DocumentStatus"] = Statuscode;
                    _PriceListModel.DocumentStatus = Statuscode;
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomerPriceList/PriceDetail.cshtml", _PriceListModel);
                }
               
             }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult AddNewCustomerPrice()
        {
            //@Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            PriceListDetailModel _PriceaddnewModel = new PriceListDetailModel();
            _PriceaddnewModel.Message = "New";
            _PriceaddnewModel.Command = "Add";
            _PriceaddnewModel.AppStatus = "D";
            _PriceaddnewModel.TransType = "Save";
            _PriceaddnewModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _PriceaddnewModel;
            return RedirectToAction("PriceDetail", "CustomerPriceList");
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
        [NonAction]
        private DataTable GetCustPriceGrp()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _CustomerPriceList_ISERVICES.GetCustPriceGrpDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetPriceListName()
        {
            try
            {
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                DataTable dt = _CustomerPriceList_ISERVICES.GetPriceListName(Comp_ID, BrchID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult EditPriceList(string list_no,string WF_status)
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["PriceListNo"] = list_no;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            PriceListDetailModel _Doubleclick = new PriceListDetailModel();
            UrlModel _urlModel = new UrlModel();
            _Doubleclick.Message = "New";
            _Doubleclick.Command = "Add";
            _Doubleclick.AppStatus = "D";
            _Doubleclick.TransType = "Update";
            _Doubleclick.BtnName = "BtnToDetailPage";
            _Doubleclick.PriceListNo = list_no;
            if(WF_status != null && WF_status != "")
            {
                _Doubleclick.WF_status1 = WF_status;
                _urlModel.WF_status1 = WF_status;
            }          
            TempData["ModelData"] = _Doubleclick;          
            _urlModel.TransType = "Update";
            _urlModel.BtnName = "BtnToDetailPage";
            _urlModel.PriceListNo = list_no;        
            return RedirectToAction("PriceDetail", "CustomerPriceList", _urlModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SavePriceList(PriceListDetailModel _PriceListModel, int list_no, string command)
        {
            try
            {
                if (_PriceListModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        PriceListDetailModel priceaddnewModel = new PriceListDetailModel();
                        priceaddnewModel.Message = "New";
                        priceaddnewModel.Command = "Add";
                        priceaddnewModel.AppStatus = "D";
                        priceaddnewModel.TransType = "Save";
                        priceaddnewModel.BtnName = "BtnAddNew";
                        TempData["ModelData"] = priceaddnewModel;
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.TransType = "Save";
                        _urlModel.BtnName = "BtnAddNew";
                        _urlModel.Command = "Add";
                        return RedirectToAction("PriceDetail", "CustomerPriceList", _urlModel);

                    case "Edit":
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["BtnName"] = "BtnEdit";
                        //Session["Message"] = null;
                        //Session["PriceListNo"] = _PriceListModel.list_no;
                        _PriceListModel.Command = command;
                        _PriceListModel.TransType = "Update";
                        _PriceListModel.AppStatus = "D";
                        _PriceListModel.BtnName = "BtnEdit";
                        _PriceListModel.PriceListNo = _PriceListModel.list_no.ToString();
                        TempData["ModelData"] = _PriceListModel;
                        UrlModel _urleditModel = new UrlModel();
                        _urleditModel.Command = command;
                        _urleditModel.TransType = "Update";
                        _urleditModel.AppStatus = "D";
                        _urleditModel.BtnName = "BtnEdit";
                        _urleditModel.PriceListNo = _PriceListModel.list_no.ToString();
                        return RedirectToAction("PriceDetail", _urleditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        _PriceListModel.Command = command;
                        //Session["BtnName"] = "Refresh";
                        list_no = _PriceListModel.list_no;
                        PriceListDelete(_PriceListModel, command);
                        PriceListDetailModel pricedeleteModel = new PriceListDetailModel();
                        pricedeleteModel.Message = "Deleted";
                        pricedeleteModel.Command = "Refresh";
                        pricedeleteModel.TransType = "Refresh";
                        pricedeleteModel.AppStatus = "DL";
                        pricedeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = pricedeleteModel;                    
                        return RedirectToAction("PriceDetail");

                    case "Save":
                        //Session["Command"] = command;
                        _PriceListModel.Command = command;
                       
                        if (ModelState.IsValid)
                        {
                            SavePriceListDetail(_PriceListModel);

                            if (_PriceListModel.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            //Session["PriceListNo"] = Session["PriceListNo"].ToString();

                            //if (Session["Message"].ToString() == "Duplicate")
                            if (_PriceListModel.Message == "Duplicate")
                            {
                                dt = GetCustPriceGrp(); 
                                List<PriceGroup> _PriceGroupList = new List<PriceGroup>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    PriceGroup _PriceGroup = new PriceGroup();
                                    _PriceGroup.setup_id = dr["setup_id"].ToString();
                                    _PriceGroup.setup_val = dr["setup_val"].ToString();
                                    _PriceGroupList.Add(_PriceGroup);
                                }
                                _PriceGroupList.Insert(0, new PriceGroup() { setup_id = "0", setup_val = "---Select---" });
                                _PriceListModel.PriceGroupList = _PriceGroupList;

                                DataTable dts = GetPriceListName();
                                List<PriceListName> _PriceListName = new List<PriceListName>();
                                foreach (DataRow dr in dts.Rows)
                                {
                                    PriceListName _PriceName = new PriceListName();
                                    _PriceName.list_no = dr["list_no"].ToString();
                                    _PriceName.list_name = dr["list_name"].ToString();
                                    _PriceListName.Add(_PriceName);
                                }
                                _PriceListName.Insert(0, new PriceListName() { list_no = "0", list_name = "---Select---" });
                                _PriceListModel.PriceListName = _PriceListName;

                                DataTable PriceListItemDetail = new DataTable();
                                DataTable dtItem = new DataTable();
                                dtItem.Columns.Add("comp_id", typeof(Int32));
                                dtItem.Columns.Add("br_id", typeof(Int32));
                                dtItem.Columns.Add("list_no", typeof(int));
                                dtItem.Columns.Add("item_id", typeof(string));
                                dtItem.Columns.Add("item_name", typeof(string));
                                dtItem.Columns.Add("uom_id", typeof(int));
                                dtItem.Columns.Add("uom_name", typeof(string));
                                dtItem.Columns.Add("sale_price", typeof(float));
                                dtItem.Columns.Add("disc_perc", typeof(float));
                                dtItem.Columns.Add("disc_mrp", typeof(float));
                                dtItem.Columns.Add("effect_price", typeof(float));
                                dtItem.Columns.Add("it_remarks", typeof(string));
                                JArray jObject = JArray.Parse(_PriceListModel.Itemdetails);
                                for (int i = 0; i < jObject.Count; i++)
                                {
                                    DataRow dtrowItemdetails = dtItem.NewRow();
                                    decimal sale_price, disc_perc, mrp_desc, effect_price;

                                    if (jObject[i]["SalePrice"].ToString() == "")
                                        sale_price = 0;
                                    else
                                        sale_price = Convert.ToDecimal(jObject[i]["SalePrice"].ToString());

                                    if (jObject[i]["DiscPerc"].ToString() == "")
                                        disc_perc = 0;
                                    else
                                        disc_perc = Convert.ToDecimal(jObject[i]["DiscPerc"].ToString());

                                    if (jObject[i]["DiscMRP"].ToString() == "")
                                        mrp_desc = 0;
                                    else
                                        mrp_desc = Convert.ToDecimal(jObject[i]["DiscMRP"].ToString());

                                    if (jObject[i]["EffectPrice"].ToString() == "")
                                        effect_price = 0;
                                    else
                                        effect_price = Convert.ToDecimal(jObject[i]["EffectPrice"].ToString());


                                    dtrowItemdetails["comp_id"] = Session["CompId"].ToString();
                                    dtrowItemdetails["br_id"] = Session["BranchId"].ToString();
                                    dtrowItemdetails["list_no"] = _PriceListModel.list_no;
                                    dtrowItemdetails["item_id"] = jObject[i]["ItemID"].ToString();
                                    var itemname = jObject[i]["ItemName"].ToString();                                
                                    dtrowItemdetails["item_name"] = jObject[i]["ItemName"].ToString();
                                    dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMID"]);
                                    dtrowItemdetails["uom_name"] = (jObject[i]["uom_name"]);
                                    dtrowItemdetails["sale_price"] = sale_price;
                                    dtrowItemdetails["disc_perc"] = disc_perc;
                                    dtrowItemdetails["disc_mrp"] = mrp_desc;
                                    dtrowItemdetails["effect_price"] = effect_price;
                                    dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                                    dtItem.Rows.Add(dtrowItemdetails);
                                }
                                PriceListItemDetail = dtItem;
                                ViewBag.ItemDetails = PriceListItemDetail;
                                //ViewBag.DocumentCode = Status;

                                DataTable PriceListPriceGroup = new DataTable();
                                DataTable dtparam = new DataTable();

                                dtparam.Columns.Add("comp_id", typeof(int));
                                dtparam.Columns.Add("br_id", typeof(int));
                                dtparam.Columns.Add("list_no", typeof(int));
                                dtparam.Columns.Add("setup_val", typeof(string));
                                dtparam.Columns.Add("cust_pr_grp", typeof(int));

                                JArray JPObject = JArray.Parse(_PriceListModel.PriceGroupDetail);
                                for (int i = 0; i < JPObject.Count; i++)
                                {
                                    DataRow dtparamrow = dtparam.NewRow();

                                    dtparamrow["comp_id"] = Session["CompId"].ToString();
                                    dtparamrow["br_id"] = Session["BranchId"].ToString();
                                    dtparamrow["list_no"] = _PriceListModel.list_no;
                                    dtparamrow["setup_val"] = JPObject[i]["PriceGroupName"].ToString();
                                    dtparamrow["cust_pr_grp"] = JPObject[i]["PriceGroupId"].ToString();

                                    dtparam.Rows.Add(dtparamrow);
                                }
                                PriceListPriceGroup = dtparam;
                                ViewBag.PriceGroupDetail = PriceListPriceGroup;

                                ViewBag.DocID = DocumentMenuId;
                                //Session["PriceListNo"] = "";
                                //Session["AppStatus"] = "D";
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                // ViewBag.Message = Session["Message"].ToString();
                               ViewBag.Message = _PriceListModel.Message;
                                CommonPageDetails();
                                //ViewBag.VBRoleList = GetRoleList();
                                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomerPriceList/PriceDetail.cshtml", _PriceListModel);
                            }
                            else
                            {
                                TempData["ModelData"] = _PriceListModel;
                                UrlModel _urlModelSave = new UrlModel();
                                _urlModelSave.Command = _PriceListModel.Command;
                                _urlModelSave.TransType = _PriceListModel.TransType;
                                _urlModelSave.AppStatus = "D";
                                _urlModelSave.BtnName = _PriceListModel.BtnName;
                                _urlModelSave.PriceListNo = _PriceListModel.PriceListNo;
                                return RedirectToAction("PriceDetail", _urlModelSave);
                            }
                        }
                        else
                        {
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomerPriceList/PriceDetail.cshtml", _PriceListModel);
                        }

                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        //Session["Command"] = command;
                        _PriceListModel.Command = command;
                        list_no = _PriceListModel.list_no;
                        //Session["PriceListNo"] = list_no;
                        PriceListApprove(_PriceListModel, command,"","");
                        TempData["ModelData"] = _PriceListModel;
                        UrlModel _urlModelApprove = new UrlModel();
                        _urlModelApprove.Command = _PriceListModel.Command;
                        _urlModelApprove.TransType = _PriceListModel.TransType;
                        _urlModelApprove.AppStatus = "D";
                        _urlModelApprove.BtnName = _PriceListModel.BtnName;
                        _urlModelApprove.PriceListNo = _PriceListModel.PriceListNo;
                        if (_PriceListModel.WF_status1 != null)
                        {
                            _urlModelApprove.WF_status1 = _PriceListModel.WF_status1;
                        }
                        return RedirectToAction("PriceDetail", _urlModelApprove);
                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        PriceListDetailModel PriceRefreshModel = new PriceListDetailModel();
                        PriceRefreshModel.Message = null;
                        PriceRefreshModel.Command = command;
                        PriceRefreshModel.TransType = "Save";
                        PriceRefreshModel.BtnName = "Refresh";
                        TempData["ModelData"] = PriceRefreshModel;
                        return RedirectToAction("PriceDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        PriceListDetailModel _dashbord = new PriceListDetailModel();
                        _dashbord.WF_status = _PriceListModel.WF_status1;
                        return RedirectToAction("PriceList", "CustomerPriceList", _dashbord);

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
        public ActionResult SavePriceListDetail(PriceListDetailModel _PriceListModel)
        {
            try
            {
                
                    if (Session["compid"] != null)
                    {
                        comp_id = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }

                    DataTable PriceListHeader = new DataTable();
                    DataTable PriceListItemDetail = new DataTable();
                    DataTable PriceListPriceGroup = new DataTable();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MenuDocumentId", typeof(string));
                    dt.Columns.Add("TransType", typeof(string));
                    dt.Columns.Add("comp_id", typeof(int));
                    dt.Columns.Add("br_id", typeof(int));
                    dt.Columns.Add("list_no", typeof(int));
                    dt.Columns.Add("list_name", typeof(string));
                    dt.Columns.Add("valid_fr", typeof(DateTime));
                    dt.Columns.Add("valid_to", typeof(DateTime));
                    dt.Columns.Add("act_status", typeof(string));
                    dt.Columns.Add("List_remarks", typeof(string));
                    dt.Columns.Add("create_id", typeof(int));
                    dt.Columns.Add("mod_id", typeof(int));
                    dt.Columns.Add("list_status", typeof(string));
                    dt.Columns.Add("UserName", typeof(string));
                    dt.Columns.Add("UserMacaddress", typeof(string));
                    dt.Columns.Add("UserSystemName", typeof(string));
                    dt.Columns.Add("UserIP", typeof(string));


                    DataRow dtrow = dt.NewRow();

                    dtrow["MenuDocumentId"] = DocumentMenuId;
                //dtrow["TransType"] = Session["TransType"].ToString();
                /*Commented by Hina on 07-03-2024 to modify get status A for save data in history tbl*/
                //if (_PriceListModel.list_no != 0)
                //{
                //    dtrow["TransType"] = "Update";
                //}
                //else
                //{
                //    dtrow["TransType"] = "Save";
                //}
                //dtrow["list_status"] = "D";
                if (_PriceListModel.list_no != 0 && _PriceListModel.CPL_status == "D")
                {
                    dtrow["list_status"] = "D";
                    dtrow["TransType"] = "Update";
                }
                else if (_PriceListModel.list_no != 0 && _PriceListModel.CPL_status == "A")
                {
                    dtrow["list_status"] = "A";
                    dtrow["TransType"] = "Update";
                }
                else
                {
                    dtrow["TransType"] = "Save";
                    dtrow["list_status"] = "D";
                }
                dtrow["comp_id"] = Session["CompId"].ToString();
                    dtrow["br_id"] = Session["BranchId"].ToString();
                    dtrow["list_no"] = _PriceListModel.list_no;
                    dtrow["list_name"] = _PriceListModel.list_name;
                    dtrow["valid_fr"] = _PriceListModel.valid_fr;
                    dtrow["valid_to"] = _PriceListModel.valid_to;
                if (_PriceListModel.act_status)
                {
                    dtrow["act_status"] = "Y";
                }
                else
                {
                    dtrow["act_status"] = "N";
                }
                dtrow["List_remarks"] = _PriceListModel.List_remarks;
                    dtrow["create_id"] = Session["UserId"].ToString();
                    dtrow["mod_id"] = Session["UserId"].ToString();
                    //dtrow["list_status"] = Session["AppStatus"].ToString();
                    
                    dtrow["UserName"] = Session["UserName"].ToString();
                    dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrow["UserIP"] = Session["UserIP"].ToString();

                    dt.Rows.Add(dtrow);

                    PriceListHeader = dt;


                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(Int32));
                    dtItem.Columns.Add("br_id", typeof(Int32));
                    dtItem.Columns.Add("list_no", typeof(int));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("sale_price", typeof(float));
                    dtItem.Columns.Add("disc_perc", typeof(float));
                    dtItem.Columns.Add("disc_mrp", typeof(float));
                    dtItem.Columns.Add("effect_price", typeof(float));
                   dtItem.Columns.Add("it_remarks", typeof(string));
                   dtItem.Columns.Add("sale_price_inc_tax", typeof(string));

                    //DataRow dtrowItemdetails = dtItem.NewRow();

                    JArray jObject = JArray.Parse(_PriceListModel.Itemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtItem.NewRow();
                        decimal sale_price, disc_perc, mrp_desc, effect_price, sale_price_inc_tax;

                        if (jObject[i]["SalePrice"].ToString() == "")
                        sale_price = 0;
                        else
                        sale_price = Convert.ToDecimal(jObject[i]["SalePrice"].ToString());

                        if (jObject[i]["sale_price_inc_tax"].ToString() == "")
                        sale_price_inc_tax = 0;
                        else
                        sale_price_inc_tax = Convert.ToDecimal(jObject[i]["sale_price_inc_tax"].ToString());

                        if (jObject[i]["DiscPerc"].ToString() == "")
                        disc_perc = 0;
                        else
                        disc_perc = Convert.ToDecimal(jObject[i]["DiscPerc"].ToString());

                        if (jObject[i]["DiscMRP"].ToString() == "")
                        mrp_desc = 0;
                        else
                        mrp_desc = Convert.ToDecimal(jObject[i]["DiscMRP"].ToString());

                        if (jObject[i]["EffectPrice"].ToString() == "")
                        effect_price = 0;
                        else
                        effect_price = Convert.ToDecimal(jObject[i]["EffectPrice"].ToString());


                        dtrowItemdetails["comp_id"] = Session["CompId"].ToString();
                        dtrowItemdetails["br_id"] = Session["BranchId"].ToString();
                        dtrowItemdetails["list_no"] = _PriceListModel.list_no;
                        dtrowItemdetails["item_id"] = jObject[i]["ItemID"].ToString();
                        dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMID"]);
                        dtrowItemdetails["sale_price"] = sale_price;
                        
                        dtrowItemdetails["disc_perc"] = disc_perc;
                        dtrowItemdetails["disc_mrp"] = mrp_desc;
                        dtrowItemdetails["effect_price"] = effect_price;
                        dtrowItemdetails["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                        dtrowItemdetails["sale_price_inc_tax"] = sale_price_inc_tax;
                    dtItem.Rows.Add(dtrowItemdetails);
                    }
                    PriceListItemDetail = dtItem;
                DataTable dtparam = new DataTable();

                dtparam.Columns.Add("comp_id", typeof(int));
                dtparam.Columns.Add("br_id", typeof(int));
                dtparam.Columns.Add("list_no", typeof(int));
                dtparam.Columns.Add("cust_pr_grp", typeof(int));

                JArray JPObject = JArray.Parse(_PriceListModel.PriceGroupDetail);
                for (int i = 0; i < JPObject.Count; i++)
                {
                    DataRow dtparamrow = dtparam.NewRow();

                    dtparamrow["comp_id"] = Session["CompId"].ToString();
                    dtparamrow["br_id"] = Session["BranchId"].ToString();
                    dtparamrow["list_no"] = _PriceListModel.list_no;
                    dtparamrow["cust_pr_grp"] = JPObject[i]["PriceGroupId"].ToString();

                    dtparam.Rows.Add(dtparamrow);
                }
                PriceListPriceGroup = dtparam;                
                 
                    String SaveMessage = _CustomerPriceList_ISERVICES.InsertPriceListDetail(PriceListHeader, PriceListItemDetail, PriceListPriceGroup);
                    string PriceListNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Data_Not_Found")
                {
                    ViewBag.MenuPageName = getDocumentName();

                    _PriceListModel.Title = title;
                    var a = PriceListNo.Split('-');
                    var msg = Message.Replace("_", " ") + " " + a[0].Trim()+" in "+ _PriceListModel.Title;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _PriceListModel.Message = Message.Replace("_", "");
                    return RedirectToAction("PriceDetail");
                }
                if (Message == "Update" || Message == "Save")
                {
                    //Session["Message"] = "Save";
                    //Session["Command"] = "Update";
                    //Session["PriceListNo"] = PriceListNo;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    _PriceListModel.Message = "Save";
                    _PriceListModel.Command = "Update";
                    _PriceListModel.PriceListNo = PriceListNo;
                    _PriceListModel.TransType = "Update";
                    _PriceListModel.AppStatus = "D";
                    _PriceListModel.BtnName = "BtnSave";
                    TempData["ModelData"] = _PriceListModel;
                }

                if (Message == "Duplicate")
                {
                    //Session["TransType"] = "Duplicate";
                    //Session["Message"] = "Duplicate";
                    //Session["PriceListNo"] = PriceListNo;
                    _PriceListModel.Message = "Duplicate";
                    _PriceListModel.TransType = "Duplicate";
                    _PriceListModel.PriceListNo = PriceListNo;
                    _PriceListModel.BtnName = "BtnAddNew";
                    _PriceListModel.Itemdetailsdata = PriceListItemDetail;
                }
               // Session["BtnName"] = "BtnSave";
                return RedirectToAction("PriceDetail");            
              
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private List<ListPageList> GetPriceList(PriceListDetailModel _PriceListModel)
        {
            try
            {
                _ListPageList = new List<ListPageList>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                 string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                
                if(_PriceListModel.WF_status != null)
                {
                    wfstatus = _PriceListModel.WF_status;
                }
                else
                {
                    wfstatus = "0";
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }

                dt = _CustomerPriceList_ISERVICES.GetPriceList(CompID, BrchID, userid, wfstatus, DocumentMenuId, _PriceListModel.ActStatus);
                if (dt.Rows.Count > 0)
                {
                    // FromDate = dt.Rows[0]["finstrdate"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListPageList _PriceList = new ListPageList();
                        _PriceList.list_no = dr["list_no"].ToString();
                        _PriceList.list_name = dr["list_name"].ToString();
                        _PriceList.valid_fr = dr["valid_fr"].ToString();
                        _PriceList.valid_to = dr["valid_to"].ToString();
                        _PriceList.Stauts = dr["Status"].ToString();
                        _PriceList.CreateDate = dr["CreateDate"].ToString();
                        _PriceList.create_dt2 = dr["create_dt2"].ToString();
                        _PriceList.ApproveDate = dr["ApproveDate"].ToString();
                        _PriceList.ModifyDate = dr["ModifyDate"].ToString();
                        _PriceList.ModifyDate = dr["ModifyDate"].ToString();
                        _PriceList.List_remarks = dr["List_remarks"].ToString();
                        _PriceList.create_by = dr["create_by"].ToString();
                        _PriceList.app_by = dr["app_by"].ToString();
                        _PriceList.mod_by = dr["mod_by"].ToString();
                        _PriceList.active = dr["active"].ToString();
                        _ListPageList.Add(_PriceList);
                    }
                }
                return _ListPageList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private ActionResult PriceListDelete(PriceListDetailModel _PriceListModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();

                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                int list_no = _PriceListModel.list_no;
                DataSet Message = _CustomerPriceList_ISERVICES.PriceListDetailDelete(_PriceListModel, comp_id, BrchID, DocumentMenuId);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["PriceListNo"] = "";
                //_PriceListModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
       
                return RedirectToAction("PriceDetail", "PriceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private ActionResult PriceListApprove(PriceListDetailModel _PriceListModel, string command,string WF_status1 ,string docid)
        {
            try
            {
                //DocumentMenuId = _PriceListModel.MenuDocumentId;
                //Session["MenuDocumentId"] = DocumentMenuId;
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //if(docid != null && docid != "")
                //{
                //    DocumentMenuId = docid;
                //}
                //else
                //{
                //    DocumentMenuId = _PriceListModel.MenuDocumentId;
                //}
                _PriceListModel.CreatedBy = Session["UserId"].ToString();
                //int list_no = _PriceListModel.list_no;
                string app_id = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                DataSet Message = _CustomerPriceList_ISERVICES.PriceListApprove(_PriceListModel, comp_id, BrchID,mac_id, DocumentMenuId,app_id);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                //Session["PriceListNo"] = _PriceListModel.list_no;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'A';
                //Session["BtnName"] = "BtnApprove";
                UrlModel _urlModelApprove = new UrlModel();
                _PriceListModel.Message = "Approved";
                _PriceListModel.Command = command;
                _PriceListModel.PriceListNo = _PriceListModel.list_no.ToString();
                _PriceListModel.TransType = "Update";
                _PriceListModel.AppStatus = "A";
               if(WF_status1 != null && WF_status1 != "")
                {
                    _PriceListModel.WF_status1 = WF_status1;
                    _urlModelApprove.WF_status1 = _PriceListModel.WF_status1;
                }              
                _PriceListModel.BtnName = "BtnApprove";
                TempData["ModelData"] = _PriceListModel;               
                _urlModelApprove.Command = _PriceListModel.Command;
                _urlModelApprove.TransType = _PriceListModel.TransType;
                // _urlModelApprove.AppStatus = "A";
                _urlModelApprove.BtnName = _PriceListModel.BtnName;
                _urlModelApprove.PriceListNo = _PriceListModel.list_no.ToString();               
                return RedirectToAction("PriceDetail", "CustomerPriceList", _urlModelApprove);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList,string WF_status1,string docid)
        {
            PriceListDetailModel _PriceListModel = new PriceListDetailModel();
          
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _PriceListModel.list_no = (int)jObjectBatch[i]["list_no"];
                    _PriceListModel.create_dt = jObjectBatch[i]["create_dt"].ToString();
                    _PriceListModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _PriceListModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _PriceListModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if(_PriceListModel.A_Status != "Approve")
            {
                _PriceListModel.A_Status = "Approve";
            }
            string command = "";

            PriceListApprove(_PriceListModel, command, WF_status1, docid);
            UrlModel _urlModelApprove = new UrlModel();
            if (WF_status1 != null && WF_status1 != "")
            {
                _PriceListModel.WF_status = WF_status1;
                _urlModelApprove.WF_status1 = _PriceListModel.WF_status1;
            }
            TempData["ModelData"] = _PriceListModel;          
            _urlModelApprove.Command = _PriceListModel.Command;
            _urlModelApprove.TransType = _PriceListModel.TransType;
            // _urlModelApprove.AppStatus = "A";
            _urlModelApprove.BtnName = _PriceListModel.BtnName;
            _urlModelApprove.PriceListNo = _PriceListModel.list_no.ToString();         
            return RedirectToAction("PriceDetail", _urlModelApprove);
        }
        public ActionResult GetPriceHistoryDetails(string Doc_no, string Doc_dt, string Item_id/*,string hd_Status*/)
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
                DataTable dt = new DataTable();
                dt = _CustomerPriceList_ISERVICES.GetCustPriceHistryDtl(Comp_ID, Br_ID, Doc_no, Doc_dt, Item_id/*, hd_Status*/);
                
                if (dt.Rows.Count > 0)
                {
                   ViewBag.PrcHistryItemDetails = dt;
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialCustomerPriceHistoryDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult PriceListSearch(string Act_Status)
        {
            PriceListDetailModel _PriceModel = new PriceListDetailModel();
            List<ListPageList> _ListPageList = new List<ListPageList>();
            ViewBag.VBSupplierList = null;
            string Comp_ID = string.Empty;
            string wfstatus = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            } 
            if (Session["CompId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {

         
                dt = _CustomerPriceList_ISERVICES.GetPriceList(Comp_ID, BrchID, userid, wfstatus, DocumentMenuId, Act_Status);
                if (dt.Rows.Count > 0)
                {
                    // FromDate = dt.Rows[0]["finstrdate"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListPageList _PriceList = new ListPageList();
                        _PriceList.list_no = dr["list_no"].ToString();
                        _PriceList.list_name = dr["list_name"].ToString();
                        _PriceList.valid_fr = dr["valid_fr"].ToString();
                        _PriceList.valid_to = dr["valid_to"].ToString();
                        _PriceList.Stauts = dr["Status"].ToString();
                        _PriceList.CreateDate = dr["CreateDate"].ToString();
                        _PriceList.create_dt2 = dr["create_dt2"].ToString();
                        _PriceList.ApproveDate = dr["ApproveDate"].ToString();
                        _PriceList.ModifyDate = dr["ModifyDate"].ToString();
                        _PriceList.ModifyDate = dr["ModifyDate"].ToString();
                        _PriceList.List_remarks = dr["List_remarks"].ToString();
                        _PriceList.create_by = dr["create_by"].ToString();
                        _PriceList.app_by = dr["app_by"].ToString();
                        _PriceList.mod_by = dr["mod_by"].ToString();
                        _PriceList.active = dr["active"].ToString();
                        _ListPageList.Add(_PriceList);
                    }
                }
                _PriceModel.CustomerPriceList = _ListPageList;
                _PriceModel.ListSearch = "ListSearch";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPriceList.cshtml", _PriceModel);
        }
        public ActionResult DownloadFile()
        {
            try
            {
                string compId = string.Empty, Br_Id = string.Empty;
                if (Session["CompID"] != null)
                    compId = Session["CompID"].ToString();
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                CommonController com_obj = new CommonController();
                DataTable ItemDetail = new DataTable();
                DataSet obj_ds = new DataSet();

                ItemDetail.Columns.Add("Item Name*(max 100 characters)", typeof(string));
                //ItemDetail.Columns.Add("UOM*", typeof(string));
                ItemDetail.Columns.Add("MRP (Incl. Tax)", typeof(string));
                ItemDetail.Columns.Add("MRP (Ex. Tax)*", typeof(string));
                ItemDetail.Columns.Add("MRP Discount(in %)", typeof(string));
                ItemDetail.Columns.Add("Discount(in %)", typeof(string));
                ItemDetail.Columns.Add("Remarks(max 200 characters)", typeof(string));

                obj_ds.Tables.Add(ItemDetail);
                DataSet ds = _CustomerPriceList_ISERVICES.GetMasterDropDownList(compId, Br_Id);
                CommonController obj = new CommonController();
                string filePath = obj.CreateExcelFile("ImportCustomerPriceListTemplate", Server);
                com_obj.AppendExcel(filePath, ds, obj_ds, "CustomerPriceList");
                string fileName = Path.GetFileName(filePath);
                return File(filePath, "application/octet-stream", fileName);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult GetPriceListItemDetail(string plist_id)
        {
            string Comp_ID = string.Empty;
            string BrchID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["CompId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {
                DataTable ItemDetail = _CustomerPriceList_ISERVICES.GetPriceListItemDetail(Comp_ID, BrchID, plist_id);
                string json = JsonConvert.SerializeObject(ItemDetail);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
    }
}




