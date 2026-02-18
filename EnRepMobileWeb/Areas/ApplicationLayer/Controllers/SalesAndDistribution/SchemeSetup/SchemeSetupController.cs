using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SchemeSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SchemeSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.SchemeSetup
{
    public class SchemeSetupController : Controller
    {
        string CompID, BrId, UserId, language = String.Empty;
        string DocumentMenuId = "105103110", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SchemeSetup_ISERVICES _SchemeSetup_ISERVICES;
        public SchemeSetupController(Common_IServices _Common_IServices, SchemeSetup_ISERVICES schemeSetup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._SchemeSetup_ISERVICES = schemeSetup_ISERVICES;
        }
        private void SetAuthValue()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrId = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                UserId = Session["UserId"].ToString();
            }
        }
        private void CommonPageDetails()
        {
            try
            {
                SetAuthValue();
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrId, UserId, DocumentMenuId, language);
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
        // GET: ApplicationLayer/SchemeSetup
        public ActionResult SchemeSetup()
        {
            SchemeSetupList_Model _Model = new SchemeSetupList_Model();
            //ViewBag.MenuPageName = getDocumentName();
            CommonPageDetails();
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _Model.prod_grp = a[0].Trim();
                _Model.cust_price_grp = a[1].Trim();
                _Model.status = a[2].Trim();
                _Model.act_status = a[3].Trim();
                _Model.ListFilterData = TempData["ListFilterData"].ToString();
            }
            DataSet ds = _SchemeSetup_ISERVICES.SchemeDataList(CompID, BrId, _Model.prod_grp, _Model.cust_price_grp, _Model.status, _Model.act_status);
            _Model.Title = title;
            _Model.prodGroupLists = ProdGroupLists(ds.Tables[0]);
            _Model.custPriceGroupLists = CustPriceGroupLists(ds.Tables[1]);
            _Model.statusList = GetStatusList();
            ViewBag.SchemeList = ds.Tables[2];
            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SchemeSetup/SchemeSetupList.cshtml", _Model);
        }
        public ActionResult AddSchemeSetupDetail()
        {
            UrlModel _urlModel = new UrlModel();
            _urlModel.cmd = "Add";
            _urlModel.Trp = "Save";
            _urlModel.BtnName = "BtnAddNew";

            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("SchemeSetupDetail", "SchemeSetup", _urlModel);
        }
        public ActionResult SchemeSetupDetail(UrlModel _urlModel)
        {
            try
            {
                SchemeSetup_Model _Model = new SchemeSetup_Model();
                _Model.BtnName = _urlModel.BtnName;
                _Model.TransType = _urlModel.Trp;
                _Model.Command = _urlModel.cmd;
                _Model.scheme_id = _urlModel.SchId;
                CommonPageDetails();
                GetAllPageLoadData(_Model);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    _Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                }
                _Model.Title = title;
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SchemeSetup/SchemeSetupDetail.cshtml", _Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private List<ProdGroupList> ProdGroupLists(DataTable Table)
        {
            List<ProdGroupList> _GroupList = new List<ProdGroupList>();
            foreach (DataRow data in Table.Rows)
            {
                ProdGroupList _GroupDetail = new ProdGroupList();
                _GroupDetail.grp_id = data["item_grp_id"].ToString();
                _GroupDetail.grp_name = data["ItemGroupChildNood"].ToString();
                _GroupList.Add(_GroupDetail);
            }
            _GroupList.Insert(0, new ProdGroupList() { grp_id = "0", grp_name = "---Select---" });
            return _GroupList;
        }
        private List<CustPriceGroupList> CustPriceGroupLists(DataTable Table)
        {
            List<CustPriceGroupList> _PriceGroupList = new List<CustPriceGroupList>();
            foreach (DataRow dr in Table.Rows)
            {
                CustPriceGroupList _PriceGroup = new CustPriceGroupList();
                _PriceGroup.grp_id = dr["setup_id"].ToString();
                _PriceGroup.grp_name = dr["setup_val"].ToString();
                _PriceGroupList.Add(_PriceGroup);
            }
            _PriceGroupList.Insert(0, new CustPriceGroupList() { grp_id = "0", grp_name = "---Select---" });
            return _PriceGroupList;
            
        }
        public List<Status> GetStatusList()
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                if (ViewBag.StatusList != null)
                {
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
                }
                return statusLists;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void GetAllPageLoadData(SchemeSetup_Model _Model)
        {
            try
            {
                SetAuthValue();
                string scheme_id = _Model.scheme_id;
                DataSet Table = _SchemeSetup_ISERVICES.GetAllPageLoadData(CompID, BrId, UserId, scheme_id);

                _Model.prodGroupLists = ProdGroupLists(Table.Tables[0]);
                _Model.custPriceGroupLists = CustPriceGroupLists(Table.Tables[1]);

                _Model.rev_no = "0";
                _Model.is_active = true;//default value
                if(TempData["Message"]!=null && TempData["Message"].ToString()== "Duplicate")
                {
                    var pre_Model = TempData.Peek("ModelData") as SchemeSetup_Model;
                    _Model.scheme_id = pre_Model.scheme_id;
                    _Model.scheme_name = pre_Model.scheme_name;
                    _Model.valid_from = pre_Model.valid_from;
                    _Model.valid_upto = pre_Model.valid_upto;
                    _Model.scheme_type = pre_Model.scheme_type;
                    _Model.rev_no = pre_Model.rev_no;
                    _Model.is_active = pre_Model.is_active;
                    _Model.scheme_remarks = pre_Model.scheme_remarks;
                    _Model.create_by = pre_Model.create_by;
                    _Model.create_on = pre_Model.create_on;
                    _Model.app_by = pre_Model.app_by;
                    _Model.app_on = pre_Model.app_on;
                    _Model.mod_by = pre_Model.mod_by;
                    _Model.mod_on = pre_Model.mod_on;
                    _Model.DocumentStatus = pre_Model.DocumentStatus;
                    _Model.DocStatusName = pre_Model.DocStatusName;
                    ViewBag.SchemeSlabDetail = pre_Model.SchemeSlabDetail;
                    ViewBag.SchemeProductGrpDetail = pre_Model.SchemeProductGrpDetail;
                    ViewBag.SchemeCustPriceGrpDetail = pre_Model.SchemeCustPriceGrpDetail;
                }
                else
                {
                    if (scheme_id != "" && scheme_id != "0")
                    {
                        if (Table.Tables[2].Rows.Count > 0)
                        {
                            DataRow HdRow = Table.Tables[2].Rows[0];
                            _Model.scheme_id = scheme_id;
                            _Model.scheme_name = HdRow["schm_name"].ToString();
                            _Model.valid_from = HdRow["valid_from"].ToString();
                            _Model.valid_upto = HdRow["valid_upto"].ToString();
                            _Model.scheme_type = HdRow["schm_type"].ToString();
                            _Model.rev_no = HdRow["rev_no"].ToString();
                            _Model.is_active = (HdRow["active_status"].ToString() == "Y");
                            _Model.scheme_remarks = HdRow["remarks"].ToString();
                            _Model.create_by = HdRow["create_by"].ToString();
                            _Model.create_on = HdRow["create_dt"].ToString();
                            _Model.app_by = HdRow["app_by"].ToString();
                            _Model.app_on = HdRow["app_dt"].ToString();
                            _Model.mod_by = HdRow["mod_by"].ToString();
                            _Model.mod_on = HdRow["mod_dt"].ToString();
                            _Model.rev_no = HdRow["rev_no"].ToString();
                            _Model.DocumentStatus = HdRow["schm_status"].ToString().Trim();
                            _Model.DocStatusName = HdRow["status_name"].ToString();
                            ViewBag.SchemeSlabDetail = Table.Tables[3];
                            ViewBag.SchemeProductGrpDetail = Table.Tables[4];
                            ViewBag.SchemeCustPriceGrpDetail = Table.Tables[5];
                        }
                    }
                    else
                    {
                        _Model.scheme_id = "0";
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult EditScheme(string schm_id, string ListFilterData, string WF_status)
        {/*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
            SetAuthValue();
            SchemeSetup_Model dblclick = new SchemeSetup_Model();
            dblclick.BtnName = "BtnToDetailPage";
            dblclick.TransType = "Update";
            dblclick.Command = "Update";
            dblclick.Message = "New"; ;
            dblclick.scheme_id = schm_id;
            dblclick.AppStatus = "D";
            dblclick.UserId = UserId;
            UrlModel _urlModel = new UrlModel();
            _urlModel.cmd = "Update";
            _urlModel.Trp = "Update";
            _urlModel.SchId = dblclick.scheme_id;
            if (WF_status != null && WF_status != "")
            {
                dblclick.WF_status1 = WF_status;
                _urlModel.WFS1 = WF_status;
            }
            _urlModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = dblclick;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("SchemeSetupDetail", "SchemeSetup", _urlModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SchemeSetupSave(SchemeSetup_Model _SchemeSetup_Model, string command)
        {
            try
            {
                /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_SchemeSetup_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        SetAuthValue();
                        SchemeSetup_Model _SchemeSetup_AddnewModel = new SchemeSetup_Model();
                        _SchemeSetup_AddnewModel.Message = "New";
                        _SchemeSetup_AddnewModel.Command = "Add";
                        _SchemeSetup_AddnewModel.AppStatus = "D";
                        _SchemeSetup_AddnewModel.TransType = "Save";
                        _SchemeSetup_AddnewModel.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _SchemeSetup_AddnewModel;
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.cmd = "Add";
                        _urlModel.Trp = "Save";
                        _urlModel.BtnName = "BtnAddNew";
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("SchemeSetupDetail", "SchemeSetup", _urlModel);

                    case "Edit":
                        SetAuthValue();
                        
                        _SchemeSetup_Model.Message = "New";
                        _SchemeSetup_Model.Command = command;
                        _SchemeSetup_Model.AppStatus = "D";
                        _SchemeSetup_Model.TransType = "Update";
                        _SchemeSetup_Model.BtnName = "BtnEdit";
                        _SchemeSetup_Model.scheme_id = _SchemeSetup_Model.scheme_id;
                        TempData["ModelData"] = _SchemeSetup_Model;
                        UrlModel _urlEditModel = new UrlModel();
                        _urlEditModel.cmd = command;
                        _urlEditModel.Trp = "Update";
                        _urlEditModel.BtnName = "BtnEdit";
                        _urlEditModel.SchId = _SchemeSetup_Model.scheme_id;
                        TempData["ListFilterData"] = _SchemeSetup_Model.ListFilterData1;
                        return RedirectToAction("SchemeSetupDetail", _urlEditModel);

                    case "Delete":
                        _SchemeSetup_Model.Command = command;
                        SchemeSetupDelete(_SchemeSetup_Model);
                        SchemeSetup_Model _SchemeSetup_DeleteModel = new SchemeSetup_Model();
                        _SchemeSetup_DeleteModel.Message = "Deleted";
                        _SchemeSetup_DeleteModel.Command = "Refresh";
                        _SchemeSetup_DeleteModel.AppStatus = "D";
                        _SchemeSetup_DeleteModel.TransType = "Refresh";
                        _SchemeSetup_DeleteModel.BtnName = "Refresh";
                        TempData["ModelData"] = _SchemeSetup_DeleteModel;
                        UrlModel _urlDeleteModel = new UrlModel();
                        _urlDeleteModel.cmd = "Refresh";
                        _urlDeleteModel.Trp = "Refresh";
                        _urlDeleteModel.BtnName = "Refresh";
                        TempData["ListFilterData"] = _SchemeSetup_Model.ListFilterData1;
                        return RedirectToAction("SchemeSetupDetail", _urlDeleteModel);

                    case "Save":
                        
                        SaveSchemeSetup(_SchemeSetup_Model);
                        UrlModel _urlSaveModel = new UrlModel();
                        if (TempData["Message"] != null && TempData["Message"].ToString() == "Duplicate")
                        {
                            _urlSaveModel.cmd = _SchemeSetup_Model.PreCommand;
                            _urlSaveModel.Trp = _SchemeSetup_Model.TransType;
                            _urlSaveModel.BtnName = _SchemeSetup_Model.BtnName;
                        }
                        else
                        {
                            _SchemeSetup_Model.Command = command;
                            _urlSaveModel.cmd = _SchemeSetup_Model.Command;
                            _urlSaveModel.Trp = "Update";
                            _urlSaveModel.BtnName = "BtnToDetailPage";
                        }
                        TempData["ModelData"] = _SchemeSetup_Model;
                        _urlSaveModel.SchId = _SchemeSetup_Model.scheme_id;
                        TempData["ListFilterData"] = _SchemeSetup_Model.ListFilterData1;
                        return RedirectToAction("SchemeSetupDetail", _urlSaveModel);
                    case "Approve":
                        
                        _SchemeSetup_Model.Command = command;
                        SchemeSetupApprove(_SchemeSetup_Model);
                        TempData["ModelData"] = _SchemeSetup_Model;
                        UrlModel _urlapprove = new UrlModel();
                        _urlapprove.SchId = _SchemeSetup_Model.scheme_id;
                        _urlapprove.BtnName = "BtnToDetailPage";
                        _urlapprove.Trp = _SchemeSetup_Model.TransType;
                        TempData["ListFilterData"] = _SchemeSetup_Model.ListFilterData1;
                        return RedirectToAction("SchemeSetupDetail", _urlapprove);

                    case "Refresh":
                        SchemeSetup_Model _SchemeSetupRefresh_Model = new SchemeSetup_Model();
                        _SchemeSetupRefresh_Model.BtnName = "Refresh";
                        _SchemeSetupRefresh_Model.Command = command;
                        _SchemeSetupRefresh_Model.TransType = "Save";
                        UrlModel _urlREfresh = new UrlModel();
                        _urlREfresh.cmd = _SchemeSetup_Model.Command;
                        _urlREfresh.Trp = _SchemeSetup_Model.TransType;
                        _urlREfresh.BtnName = "Refresh";
                        TempData["ListFilterData"] = _SchemeSetup_Model.ListFilterData1;
                        return RedirectToAction("SchemeSetupDetail", _urlREfresh);

                    case "Print":
                        return GenratePdfFile(_SchemeSetup_Model);
                    case "BacktoList":
                        SchemeSetupList_Model _SchemeSetupList_Model = new SchemeSetupList_Model();
                        _SchemeSetupList_Model.WF_status = _SchemeSetup_Model.WF_status1;
                        TempData["ListFilterData"] = _SchemeSetup_Model.ListFilterData1;
                        return RedirectToAction("SchemeSetup", "SchemeSetup", _SchemeSetupList_Model);

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
        public ActionResult SaveSchemeSetup(SchemeSetup_Model _SchemeSetup_Model)
        {
            try
            {
                
                SetAuthValue();

                DataTable SchemeSetupHeader = new DataTable();
                DataTable SchemeSetupSlabDetail = new DataTable();
                DataTable SchemeSetupProductGroup = new DataTable();
                DataTable SchemeSetupCustomerPriceGroup = new DataTable();
                SchemeSetupHeader = SchemeHeaderDt(_SchemeSetup_Model);
                SchemeSetupSlabDetail = SchemeSlabDt(_SchemeSetup_Model.scheme_slab_detail);
                SchemeSetupProductGroup = SchemePrdGrpDt(_SchemeSetup_Model.scheme_product_grp_detail);
                SchemeSetupCustomerPriceGroup = SchemeCstPrcGrpDt(_SchemeSetup_Model.scheme_cust_price_grp_detail);

                string result = _SchemeSetup_ISERVICES.SaveSchemeData(SchemeSetupHeader, SchemeSetupSlabDetail, SchemeSetupProductGroup, SchemeSetupCustomerPriceGroup);

                string message = result.Split('-')[0];
                string scheme_id = result.Split('-')[1];
                if (message == "Save" || message == "Update")
                {
                    TempData["Message"] = "Save";
                }
                else if(message == "Duplicate")
                {
                    TempData["Message"] = "Duplicate";
                    SchemeSetupProductGroup.Columns.Add("ItemGroupChildNood", typeof(string));
                    if (!string.IsNullOrEmpty(_SchemeSetup_Model.scheme_product_grp_detail))
                    {
                        JArray jObject = JArray.Parse(_SchemeSetup_Model.scheme_product_grp_detail);
                        for (int i = 0; i < SchemeSetupProductGroup.Rows.Count; i++)
                        {
                            for (int j = 0; j < jObject.Count; j++)
                            {

                                if(jObject[j]["prd_grp_id"].ToString()== SchemeSetupProductGroup.Rows[i]["prod_grp"].ToString())
                                {
                                    SchemeSetupProductGroup.Rows[i]["ItemGroupChildNood"] = jObject[j]["prd_grp_name"].ToString(); ;
                                }
                            }
                        }  
                    }
                    SchemeSetupCustomerPriceGroup.Columns.Add("cust_prc_grp_name", typeof(string));
                    if (!string.IsNullOrEmpty(_SchemeSetup_Model.scheme_cust_price_grp_detail))
                    {
                        JArray jObject = JArray.Parse(_SchemeSetup_Model.scheme_cust_price_grp_detail);
                        for (int i = 0; i < SchemeSetupCustomerPriceGroup.Rows.Count; i++)
                        {
                            for (int j = 0; j < jObject.Count; j++)
                            {

                                if (jObject[j]["cst_prc_grp_id"].ToString() == SchemeSetupCustomerPriceGroup.Rows[i]["cust_prc_grp"].ToString())
                                {
                                    SchemeSetupCustomerPriceGroup.Rows[i]["cust_prc_grp_name"] = jObject[j]["cst_prc_grp_name"].ToString(); ;
                                }
                            }
                        }
                    }
                    _SchemeSetup_Model.SchemeSlabDetail = SchemeSetupSlabDetail;
                    _SchemeSetup_Model.SchemeProductGrpDetail = SchemeSetupProductGroup;
                    _SchemeSetup_Model.SchemeCustPriceGrpDetail = SchemeSetupCustomerPriceGroup;
                }
                _SchemeSetup_Model.scheme_id = scheme_id;
                return RedirectToAction("SalesReturnDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        private DataTable SchemeHeaderDt(SchemeSetup_Model _Schm_Model)
        {
            try
            {
                SetAuthValue();

                DataTable SchemeSetupHeader = new DataTable();
                SchemeSetupHeader.Columns.Add("TransType", typeof(string));
                SchemeSetupHeader.Columns.Add("comp_id", typeof(string));
                SchemeSetupHeader.Columns.Add("br_id", typeof(string));
                SchemeSetupHeader.Columns.Add("schm_id", typeof(string));
                SchemeSetupHeader.Columns.Add("schm_name", typeof(string));
                SchemeSetupHeader.Columns.Add("valid_from", typeof(string));
                SchemeSetupHeader.Columns.Add("valid_upto", typeof(string));
                SchemeSetupHeader.Columns.Add("schm_type", typeof(string));
                SchemeSetupHeader.Columns.Add("rev_no", typeof(string));
                SchemeSetupHeader.Columns.Add("act_status", typeof(string));
                SchemeSetupHeader.Columns.Add("remarks", typeof(string));
                SchemeSetupHeader.Columns.Add("user_id", typeof(string));
                SchemeSetupHeader.Columns.Add("mac_id", typeof(string));

                DataRow Row = SchemeSetupHeader.NewRow();
                Row["TransType"] = _Schm_Model.TransType;
                Row["comp_id"] = CompID;
                Row["br_id"] = BrId;
                Row["schm_id"] = _Schm_Model.TransType=="Save" ? "0" : _Schm_Model.scheme_id;
                Row["schm_name"] = _Schm_Model.scheme_name;
                Row["valid_from"] = _Schm_Model.valid_from;
                Row["valid_upto"] = _Schm_Model.valid_upto;
                Row["schm_type"] = _Schm_Model.scheme_type;
                Row["rev_no"] = _Schm_Model.rev_no;
                Row["act_status"] = _Schm_Model.is_active?"Y":"N";
                Row["remarks"] = _Schm_Model.scheme_remarks;
                Row["user_id"] = UserId;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                Row["mac_id"] = mac_id;
                SchemeSetupHeader.Rows.Add(Row);
                return SchemeSetupHeader;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        private DataTable SchemeSlabDt(string _Schm_Slab)
        {
            try
            {
                DataTable SchemeSetupSlabDetail = new DataTable();
                SchemeSetupSlabDetail.Columns.Add("slab", typeof(string));
                SchemeSetupSlabDetail.Columns.Add("from_qty", typeof(string));
                SchemeSetupSlabDetail.Columns.Add("to_qty", typeof(string));
                SchemeSetupSlabDetail.Columns.Add("foc_qty", typeof(string));
                SchemeSetupSlabDetail.Columns.Add("remarks", typeof(string));

                if (!string.IsNullOrEmpty(_Schm_Slab))
                {
                    JArray jObject = JArray.Parse(_Schm_Slab);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = SchemeSetupSlabDetail.NewRow();
                        dtrowLines["slab"] = jObject[i]["slab"].ToString();
                        dtrowLines["from_qty"] = jObject[i]["fromQty"].ToString();
                        dtrowLines["to_qty"] = jObject[i]["toQty"].ToString();
                        dtrowLines["foc_qty"] = jObject[i]["focQty"].ToString();
                        dtrowLines["remarks"] = jObject[i]["remarks"].ToString();
                        SchemeSetupSlabDetail.Rows.Add(dtrowLines);
                    }
                }
                
                return SchemeSetupSlabDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable SchemePrdGrpDt(string prdGrpData)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("prod_grp", typeof(string));

                if (!string.IsNullOrEmpty(prdGrpData))
                {
                    JArray jObject = JArray.Parse(prdGrpData);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["prod_grp"] = jObject[i]["prd_grp_id"].ToString();
                        dt.Rows.Add(dtrowLines);
                    }
                }
                
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable SchemeCstPrcGrpDt(string cstPrcGrpData)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cust_prc_grp", typeof(string));

                if (!string.IsNullOrEmpty(cstPrcGrpData))
                {
                    JArray jObject = JArray.Parse(cstPrcGrpData);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["cust_prc_grp"] = jObject[i]["cst_prc_grp_id"].ToString();
                        dt.Rows.Add(dtrowLines);
                    }
                }
                
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private string SchemeSetupDelete(SchemeSetup_Model _SchemeSetup_Model)
        {
            try
            {
                SetAuthValue();
                DataSet resultSet = _SchemeSetup_ISERVICES.SchemeDelete(_SchemeSetup_Model, CompID, BrId);
                string msg = resultSet.Tables[0].Rows[0]["result"].ToString();
                TempData["Message"] = msg;
                return msg;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private string SchemeSetupApprove(SchemeSetup_Model _SchemeSetup_Model)
        {
            try
            {
                SetAuthValue();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                _SchemeSetup_Model.UserId = UserId;
                DataSet resultSet = _SchemeSetup_ISERVICES.SchemeApprove(_SchemeSetup_Model, CompID, BrId, mac_id);
                string msg = resultSet.Tables[0].Rows[0]["result"].ToString();
                TempData["Message"] = msg;
                return msg;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        [HttpPost]
        public FileResult GenratePdfFile(SchemeSetup_Model _SchemeSetup_Model)
        {
            DataTable dt = new DataTable();
           
            return File(GetPdfData(_SchemeSetup_Model.scheme_id), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");

        }
        public byte[] GetPdfData(string scheme_id)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string htmlcontent = "";
                string DelSchedule = "";
                //if (Session["CompId"] != null)
                //{
                //    CompID = Session["CompId"].ToString();
                //}
                //if (Session["BranchId"] != null)
                //{
                //    BrchID = Session["BranchId"].ToString();
                //}
                Byte[] bytes = null;

                //DataSet Details = _SalesReturn_ISERVICES.GetSalesOrderDeatilsForPrint(CompID, BrchID, srt_no, srdt.ToString("yyyy-MM-dd"), Src_Type);
                //string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());//Added by Suraj Maurya on 19-07-2025
                //ViewBag.PageName = "SR";
                //ViewBag.Title = "Sales Return/Credit Note";
                //ViewBag.Details = Details;
                //ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add  by hina sharma on 04-04-2025*/
                //ViewBag.Src_Type = Src_Type;
                //ViewBag.InvoiceTo = "";
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                //ViewBag.ApprovedBy = "Arvind Gupta";
                //ViewBag.DocStatus = Details.Tables[0].Rows[0]["srt_status"].ToString().Trim();

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
        [HttpPost]
        public ActionResult SearchSchemeDetailList(string prod_grp_id, string cust_prc_grp_id, string Status, string act_status)
        {
            SetAuthValue();
            DataSet ds = _SchemeSetup_ISERVICES.SchemeListWithFilter(CompID,BrId, prod_grp_id, cust_prc_grp_id, Status, act_status);
            ViewBag.SchemeList = ds.Tables[0];
            ViewBag.SchemeListSearch = "_Search";
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSchemeSetupList.cshtml");
        }
        public ActionResult GetProductGrpList(string fromDt,string uptoDt,string search)
        {
            try
            {
                SetAuthValue();
                JsonResult DataRows = null;
                DataSet ds = _SchemeSetup_ISERVICES.ProductGrpList(CompID, BrId, fromDt, uptoDt, search);
                DataRows = Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetCustPriceGrpList(string fromDt,string uptoDt,string search)
        {
            try
            {
                SetAuthValue();
                JsonResult DataRows = null;
                DataSet ds = _SchemeSetup_ISERVICES.CustPriceGrpList(CompID, BrId, fromDt, uptoDt, search);
                DataRows = Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult chkPrdGrpAndCstPrGrpInOtherSchm(string fromDt, string uptoDt,string scheme_id, string prodGrps,string CstPrcGrps)
        {
            try
            {
                SetAuthValue();
                JsonResult DataRows = null;
                DataSet ds = _SchemeSetup_ISERVICES.ChkGrpsAlrdyAddedInRange(CompID, BrId, fromDt, uptoDt, scheme_id, prodGrps, CstPrcGrps);
                DataRows = Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetPctGrpAndCstPrcGrpList(string fromDt, string uptoDt,string scheme_id)
        {
            try
            {
                SetAuthValue();
                JsonResult DataRows = null;
                DataSet ds = _SchemeSetup_ISERVICES.GetPctGrpAndCstPrcGrpList(CompID, BrId, fromDt, uptoDt, scheme_id);
                DataRows = Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
    }

}