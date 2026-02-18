using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionAdvice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionAdvice;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.ProductionAdvice
{
    public class ProductionAdviceController : Controller
    {
        string CompID, BrID, UserID, language, title = String.Empty;
        string DocumentMenuId = "105105120";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ProductionAdvice_IService _ProductionAdvice_IService;
        public ProductionAdviceController(Common_IServices _Common_IServices, ProductionAdvice_IService _ProductionAdvice_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._ProductionAdvice_IService = _ProductionAdvice_IService;
        }
        // GET: ApplicationLayer/ProductionAdvice
        public ActionResult ProductionAdvice(string WF_Status)
        {
            ProductionAdvice_Model _ProductionAdvice_Model = new ProductionAdvice_Model();
            _ProductionAdvice_Model.WF_Status = WF_Status;
            SearchItem search = new SearchItem();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }
            CommonPageDetails();
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            string endDate = dtnow.ToString("yyyy-MM-dd");

            List<sourcetype> fflist = new List<sourcetype>();
            sourcetype src_Obj2 = new sourcetype();
            src_Obj2.id = "A";
            src_Obj2.name = "ALL";
            fflist.Add(src_Obj2);
            sourcetype src_Obj1 = new sourcetype();
            src_Obj1.id = "D";
            src_Obj1.name = "Direct";
            fflist.Add(src_Obj1);
            sourcetype src_Obj = new sourcetype();
            src_Obj.id = "P";
            src_Obj.name = "Production Plan";
            fflist.Add(src_Obj);
            _ProductionAdvice_Model.ddl_src_typeList = fflist;
            GetStatusList(_ProductionAdvice_Model);
            var other = new CommonController(_Common_IServices);
            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrID, DocumentMenuId);
            List<ProductName> _ProductName = new List<ProductName>();
            DataTable dt1 = GetProductList(search, "A", null, null);
            foreach (DataRow dr in dt1.Rows)
            {
                ProductName ddlProductName = new ProductName();
                ddlProductName.ID = dr["item_id"].ToString();
                ddlProductName.Name = dr["item_name"].ToString();
                _ProductName.Add(ddlProductName);
            }
            //_ProductName.Insert(0, new ProductName() { ID = 0, Name = "---Select---" });
            _ProductionAdvice_Model.ProductNameList = _ProductName;
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                var Productid = a[0].Trim();
                var Source = a[1].Trim();
                var Fromdate = a[2].Trim();
                var Todate = a[3].Trim();
                var Status = a[4].Trim();
                DataTable dt = new DataTable();
                dt = _ProductionAdvice_IService.GetAdviceList(Productid, Source, Fromdate, Todate, Status, CompID, BrID, "", "", "");
                ViewBag.AdvMaterialDetails = dt;
                _ProductionAdvice_Model.txtfromdate = Fromdate;
                _ProductionAdvice_Model.txttodate = Todate;
                _ProductionAdvice_Model.status = Status;
                _ProductionAdvice_Model.ddl_ProductName = Productid;
                _ProductionAdvice_Model.ListFilterData = TempData["ListFilterData"].ToString();
                _ProductionAdvice_Model.ddl_src_type = Source;
            }
            else
            {
                _ProductionAdvice_Model.txtfromdate = startDate;
                _ProductionAdvice_Model.txttodate = endDate;
                ViewBag.AdvMaterialDetails = GetAdvicelistDetails(_ProductionAdvice_Model);
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
            //ViewBag.VBRoleList = GetRoleList();

            //Session["ADVSearch"] = "0";
            _ProductionAdvice_Model.ADVSearch = "0";

            //ViewBag.MenuPageName = getDocumentName();
            _ProductionAdvice_Model.title = title;
            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionAdvice/ProductionAdviceList.cshtml", _ProductionAdvice_Model);
        }
        public ActionResult AddProductionAdviceDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "";
            ProductionAdvice_Model AddNewModel = new ProductionAdvice_Model();
            AddNewModel.Command = "Add";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = AddNewModel;
            UrlModel AddNew_Model = new UrlModel();
            AddNew_Model.bt = "BtnAddNew";
            AddNew_Model.Cmd = "Add";
            AddNew_Model.tp = "Save";
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ProductionAdvice");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("ProductionAdviceDetail", "ProductionAdvice", AddNew_Model);
        }
        public ActionResult ProductionAdviceDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
              
                var _ProductionAdvice_Model1 = TempData["ModelData"] as ProductionAdvice_Model;
                if (_ProductionAdvice_Model1 != null)
                {                  
                    //ProductionAdvice_Model _ProductionAdvice_Model = new ProductionAdvice_Model();

                    List<sourcetype> fflist = new List<sourcetype>();
                    CommonPageDetails();
                    sourcetype src_Obj1 = new sourcetype();
                    src_Obj1.id = "D";
                    src_Obj1.name = "Direct";
                    fflist.Add(src_Obj1);
                    sourcetype src_Obj = new sourcetype();
                    src_Obj.id = "P";
                    src_Obj.name = "Production Plan";
                    fflist.Add(src_Obj);
                    _ProductionAdvice_Model1.ddl_src_typeList = fflist;

                    List<period> plist = new List<period>();
                    period pObj = new period();
                    pObj.id = "0";
                    pObj.name = "---Select---";
                    plist.Add(pObj);
                    _ProductionAdvice_Model1.ddl_periodList = plist;
                    DataSet ds = _ProductionAdvice_IService.BindFinancialYear(CompID, BrID);
                    List<financial_year> fylist = new List<financial_year>();
                    financial_year fyObj = new financial_year();
                    fyObj.id = "0";
                    fyObj.name = "---Select---";
                    fylist.Add(fyObj);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow data in ds.Tables[0].Rows)
                        {
                            financial_year fyObj1 = new financial_year();
                            fyObj1.id = data["id"].ToString();
                            fyObj1.name = data["name"].ToString();
                            fylist.Add(fyObj1);
                        }
                    }
                    _ProductionAdvice_Model1.ddl_financial_yearList = fylist;

                    List<revisionnumber> _revision = new List<revisionnumber>();
                    revisionnumber rev = new revisionnumber();
                    rev.rev_no = "---Select---";
                    rev.rev_text = "---Select---";
                    _revision.Add(rev);
                    _ProductionAdvice_Model1.revisionnolist = _revision;
                    _ProductionAdvice_Model1.title = title;
                    _ProductionAdvice_Model1.advice_dt = DateTime.Now.ToString("yyyy-MM-dd");
                    _ProductionAdvice_Model1.product_id = "0";
                    _ProductionAdvice_Model1.product_name = "---Select---";
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ProductionAdvice_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"] != null)
                    //{
                    //if (Session["TransType"].ToString() == "Update")
                    if (_ProductionAdvice_Model1.TransType == "Update")
                    {
                        DataSet dset = new DataSet();
                        //string adv_no = Session["PA_Number"].ToString();
                        //string adv_date = Session["PA_Date"].ToString();

                        string adv_no = _ProductionAdvice_Model1.PA_Number;
                        string adv_date = _ProductionAdvice_Model1.PA_Date;
                        UserID = Session["UserID"].ToString();
                        dset = _ProductionAdvice_IService.GetAdviceDetailByNo(CompID, BrID, adv_no, adv_date, UserID, DocumentMenuId);
                        _ProductionAdvice_Model1.src_doc_no = dset.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _ProductionAdvice_Model1.src_doc_dt = dset.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _ProductionAdvice_Model1.advice_no = dset.Tables[0].Rows[0]["adv_no"].ToString();
                        _ProductionAdvice_Model1.advice_dt = dset.Tables[0].Rows[0]["adv_dt"].ToString();
                        _ProductionAdvice_Model1.uom_name = dset.Tables[0].Rows[0]["uom_alias"].ToString();
                        _ProductionAdvice_Model1.uom_id = dset.Tables[0].Rows[0]["uom_id"].ToString();
                        _ProductionAdvice_Model1.advice_qty = dset.Tables[0].Rows[0]["adv_qty"].ToString();
                        _ProductionAdvice_Model1.txtfromdate = dset.Tables[0].Rows[0]["from_date"].ToString();
                        _ProductionAdvice_Model1.txttodate = dset.Tables[0].Rows[0]["to_date"].ToString();
                        _ProductionAdvice_Model1.hdnfromdate = dset.Tables[0].Rows[0]["fromdate"].ToString();
                        _ProductionAdvice_Model1.hdntodate = dset.Tables[0].Rows[0]["todate"].ToString();
                        _ProductionAdvice_Model1.product_id = dset.Tables[0].Rows[0]["product_id"].ToString();
                        _ProductionAdvice_Model1.product_name = dset.Tables[0].Rows[0]["item_name"].ToString();
                        _ProductionAdvice_Model1.sub_item = dset.Tables[0].Rows[0]["sub_item"].ToString();
                        _ProductionAdvice_Model1.batch_no = dset.Tables[0].Rows[0]["batch_no"].ToString();
                        _ProductionAdvice_Model1.completiondate = dset.Tables[0].Rows[0]["cmpl_dt"].ToString();
                        _ProductionAdvice_Model1.remarks = dset.Tables[0].Rows[0]["remarks"].ToString();
                        _ProductionAdvice_Model1.ddlsrctype = dset.Tables[0].Rows[0]["src_typeid"].ToString();
                        _ProductionAdvice_Model1.createdon = dset.Tables[0].Rows[0]["create_dt"].ToString();
                        _ProductionAdvice_Model1.createby = dset.Tables[0].Rows[0]["create_user"].ToString();
                        _ProductionAdvice_Model1.ammendedon = dset.Tables[0].Rows[0]["mod_dt"].ToString();
                        _ProductionAdvice_Model1.ammendedby = dset.Tables[0].Rows[0]["mod_user"].ToString();
                        _ProductionAdvice_Model1.approvedon = dset.Tables[0].Rows[0]["app_dt"].ToString();
                        _ProductionAdvice_Model1.approvedby = dset.Tables[0].Rows[0]["app_user"].ToString();
                        _ProductionAdvice_Model1.status = dset.Tables[0].Rows[0]["status_name"].ToString();
                        string StatusCode = dset.Tables[0].Rows[0]["adv_status"].ToString().Trim();
                        //string create_id = dset.Tables[0].Rows[0]["Creator_id"].ToString().Trim();
                        //var approval_id = dset.Tables[0].Rows[0]["approval_id"].ToString();
                        //_ProductionAdvice_Model1.create_id = create_id;

                        string create_id = dset.Tables[0].Rows[0]["creator_id"].ToString();

                        string approval_id = dset.Tables[0].Rows[0]["approval_id"].ToString();
                        _ProductionAdvice_Model1.create_id = create_id;
                        _ProductionAdvice_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(dset.Tables[6]);
                        _ProductionAdvice_Model1.WFStatus = DataTableToJSONWithStringBuilder(dset.Tables[5]);

                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = dset.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _ProductionAdvice_Model1.Command != "Edit")
                        {
                            var sent_to = "";
                            var nextLevel = "";
                            if (dset.Tables[4].Rows.Count > 0)
                            {
                                sent_to = dset.Tables[4].Rows[0]["sent_to"].ToString();
                            }

                            if (dset.Tables[5].Rows.Count > 0)
                            {
                                nextLevel = dset.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                            }
                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _ProductionAdvice_Model1.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionAdvice_Model1.BtnName = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionAdvice_Model1.BtnName = "BtnEdit";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model1.BtnName = "BtnEdit";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionAdvice_Model1.BtnName = "BtnEdit";
                                    }


                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model1.BtnName = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model1.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model1.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _ProductionAdvice_Model1.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _ProductionAdvice_Model1.statuscode = StatusCode;
                        if (StatusCode == "C")
                        {
                            _ProductionAdvice_Model1.cancelflag = true;
                        }
                        else
                        {
                            _ProductionAdvice_Model1.cancelflag = false;
                        }

                        List<sourcetype> ffnlist = new List<sourcetype>();
                        sourcetype nsrc_Obj = new sourcetype();
                        nsrc_Obj.id = dset.Tables[0].Rows[0]["src_typeid"].ToString();
                        nsrc_Obj.name = dset.Tables[0].Rows[0]["src_type"].ToString();
                        ffnlist.Add(nsrc_Obj);
                        _ProductionAdvice_Model1.ddl_src_typeList = ffnlist;

                        List<financial_year> nfyList = new List<financial_year>();
                        financial_year nfyObj = new financial_year();
                        nfyObj.id = dset.Tables[0].Rows[0]["f_fy"].ToString();
                        nfyObj.name = dset.Tables[0].Rows[0]["f_fyval"].ToString();
                        nfyList.Add(nfyObj);
                        _ProductionAdvice_Model1.ddl_financial_yearList = nfyList;

                        List<period> nplist = new List<period>();
                        period pnObj = new period();
                        pnObj.id = dset.Tables[0].Rows[0]["f_period"].ToString();
                        pnObj.name = dset.Tables[0].Rows[0]["f_periodval"].ToString();
                        nplist.Add(pnObj);
                        _ProductionAdvice_Model1.ddl_periodList = nplist;

                        DataRow Drow = dset.Tables[3].NewRow();
                        Drow[0] = "---Select---";
                        dset.Tables[3].Rows.InsertAt(Drow, 0);

                        List<revisionnumber> _nrevision = new List<revisionnumber>();
                        for (int x = 0; x < dset.Tables[3].Rows.Count; x++)
                        {
                            revisionnumber nrev = new revisionnumber();
                            nrev.rev_no = dset.Tables[3].Rows[x]["rev_no"].ToString();
                            nrev.rev_text = dset.Tables[3].Rows[x]["rev_no"].ToString();
                            _nrevision.Add(nrev);
                        }
                        _ProductionAdvice_Model1.revisionnolist = _nrevision;

                        _ProductionAdvice_Model1.ddl_revisionno = dset.Tables[0].Rows[0]["rev_no"].ToString();
                        _ProductionAdvice_Model1.hdnddl_revisionno = dset.Tables[0].Rows[0]["rev_no"].ToString();

                        _ProductionAdvice_Model1.hdnopdetail = Json(JsonConvert.SerializeObject(dset.Tables[1])).Data.ToString();
                        _ProductionAdvice_Model1.hdnopitemdetail = Json(JsonConvert.SerializeObject(dset.Tables[2])).Data.ToString();

                        /*----------Attachment Section Start----------*/
                        ViewBag.AttechmentDetails = dset.Tables[7];
                        /*----------Attachment Section end----------*/
                        ViewBag.SubItemDetails = dset.Tables[8];

                        ViewBag.DocumentCode = StatusCode;
                        //Session["DocumentStatus"] = StatusCode;
                        _ProductionAdvice_Model1.DocumentStatus = StatusCode;
                        //ViewBag.MenuPageName = getDocumentName();
                        //ViewBag.VBRoleList = GetRoleList();
                        _ProductionAdvice_Model1.title = title;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionAdvice/ProductionAdviceDetail.cshtml", _ProductionAdvice_Model1);
                    }

                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        //ViewBag.VBRoleList = GetRoleList();
                        _ProductionAdvice_Model1.title = title;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionAdvice/ProductionAdviceDetail.cshtml", _ProductionAdvice_Model1);
                    }
                }
                else
                {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrID = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                    {
                        TempData["Message1"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    ProductionAdvice_Model _ProductionAdvice_Model = new ProductionAdvice_Model();
                    if (_urlModel != null)
                    {
                        _ProductionAdvice_Model.BtnName = _urlModel.bt;                 
                        _ProductionAdvice_Model.PA_Number = _urlModel.PAC_No;
                        _ProductionAdvice_Model.PA_Date = _urlModel.PAC_dt;
                        _ProductionAdvice_Model.Command = _urlModel.Cmd;
                        _ProductionAdvice_Model.TransType = _urlModel.tp;
                        _ProductionAdvice_Model.WF_Status1 = _urlModel.wf;
                        _ProductionAdvice_Model.DocumentStatus = _urlModel.DMS;
                    }
                    CommonPageDetails();
                    List<sourcetype> fflist = new List<sourcetype>();

                    sourcetype src_Obj1 = new sourcetype();
                    src_Obj1.id = "D";
                    src_Obj1.name = "Direct";
                    fflist.Add(src_Obj1);
                    sourcetype src_Obj = new sourcetype();
                    src_Obj.id = "P";
                    src_Obj.name = "Production Plan";
                    fflist.Add(src_Obj);
                    _ProductionAdvice_Model.ddl_src_typeList = fflist;

                    List<period> plist = new List<period>();
                    period pObj = new period();
                    pObj.id = "0";
                    pObj.name = "---Select---";
                    plist.Add(pObj);
                    _ProductionAdvice_Model.ddl_periodList = plist;
                    DataSet ds = _ProductionAdvice_IService.BindFinancialYear(CompID, BrID);
                    List<financial_year> fylist = new List<financial_year>();
                    financial_year fyObj = new financial_year();
                    fyObj.id = "0";
                    fyObj.name = "---Select---";
                    fylist.Add(fyObj);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow data in ds.Tables[0].Rows)
                        {
                            financial_year fyObj1 = new financial_year();
                            fyObj1.id = data["id"].ToString();
                            fyObj1.name = data["name"].ToString();
                            fylist.Add(fyObj1);
                        }
                    }
                    _ProductionAdvice_Model.ddl_financial_yearList = fylist;

                    List<revisionnumber> _revision = new List<revisionnumber>();
                    revisionnumber rev = new revisionnumber();
                    rev.rev_no = "---Select---";
                    rev.rev_text = "---Select---";
                    _revision.Add(rev);
                    _ProductionAdvice_Model.revisionnolist = _revision;
                   
                    _ProductionAdvice_Model.advice_dt = DateTime.Now.ToString("yyyy-MM-dd");
                  
                    _ProductionAdvice_Model.product_id = "0";
                    _ProductionAdvice_Model.product_name = "---Select---";
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ProductionAdvice_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"] != null)
                    //{
                    //if (Session["TransType"].ToString() == "Update")
                    if (_ProductionAdvice_Model.TransType == "Update")
                    {
                        DataSet dset = new DataSet();
                        //string adv_no = Session["PA_Number"].ToString();
                        //string adv_date = Session["PA_Date"].ToString();

                        string adv_no = _ProductionAdvice_Model.PA_Number;
                        string adv_date = _ProductionAdvice_Model.PA_Date;
                        UserID = Session["UserID"].ToString();
                        dset = _ProductionAdvice_IService.GetAdviceDetailByNo(CompID, BrID, adv_no, adv_date, UserID, DocumentMenuId);
                        _ProductionAdvice_Model.src_doc_no = dset.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _ProductionAdvice_Model.src_doc_dt = dset.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _ProductionAdvice_Model.advice_no = dset.Tables[0].Rows[0]["adv_no"].ToString();
                        _ProductionAdvice_Model.advice_dt = dset.Tables[0].Rows[0]["adv_dt"].ToString();
                        _ProductionAdvice_Model.uom_name = dset.Tables[0].Rows[0]["uom_alias"].ToString();
                        _ProductionAdvice_Model.uom_id = dset.Tables[0].Rows[0]["uom_id"].ToString();
                        _ProductionAdvice_Model.advice_qty = dset.Tables[0].Rows[0]["adv_qty"].ToString();
                        _ProductionAdvice_Model.txtfromdate = dset.Tables[0].Rows[0]["from_date"].ToString();
                        _ProductionAdvice_Model.txttodate = dset.Tables[0].Rows[0]["to_date"].ToString();
                        _ProductionAdvice_Model.hdnfromdate = dset.Tables[0].Rows[0]["fromdate"].ToString();
                        _ProductionAdvice_Model.hdntodate = dset.Tables[0].Rows[0]["todate"].ToString();
                        _ProductionAdvice_Model.product_id = dset.Tables[0].Rows[0]["product_id"].ToString();
                        _ProductionAdvice_Model.product_name = dset.Tables[0].Rows[0]["item_name"].ToString();
                        _ProductionAdvice_Model.sub_item = dset.Tables[0].Rows[0]["sub_item"].ToString();
                        _ProductionAdvice_Model.batch_no = dset.Tables[0].Rows[0]["batch_no"].ToString();
                        _ProductionAdvice_Model.completiondate = dset.Tables[0].Rows[0]["cmpl_dt"].ToString();
                        _ProductionAdvice_Model.remarks = dset.Tables[0].Rows[0]["remarks"].ToString();
                        _ProductionAdvice_Model.ddlsrctype = dset.Tables[0].Rows[0]["src_typeid"].ToString();
                        _ProductionAdvice_Model.createdon = dset.Tables[0].Rows[0]["create_dt"].ToString();
                        _ProductionAdvice_Model.createby = dset.Tables[0].Rows[0]["create_user"].ToString();
                        _ProductionAdvice_Model.ammendedon = dset.Tables[0].Rows[0]["mod_dt"].ToString();
                        _ProductionAdvice_Model.ammendedby = dset.Tables[0].Rows[0]["mod_user"].ToString();
                        _ProductionAdvice_Model.approvedon = dset.Tables[0].Rows[0]["app_dt"].ToString();
                        _ProductionAdvice_Model.approvedby = dset.Tables[0].Rows[0]["app_user"].ToString();
                        _ProductionAdvice_Model.status = dset.Tables[0].Rows[0]["status_name"].ToString();
                        string StatusCode = dset.Tables[0].Rows[0]["adv_status"].ToString().Trim();
                        //string create_id = dset.Tables[0].Rows[0]["Creator_id"].ToString().Trim();
                        //var approval_id = dset.Tables[0].Rows[0]["approval_id"].ToString();
                        //_ProductionAdvice_Model.create_id = create_id;

                        string create_id = dset.Tables[0].Rows[0]["creator_id"].ToString();

                        string approval_id = dset.Tables[0].Rows[0]["approval_id"].ToString();
                        _ProductionAdvice_Model.create_id = create_id;
                        _ProductionAdvice_Model.WFBarStatus = DataTableToJSONWithStringBuilder(dset.Tables[6]);
                        _ProductionAdvice_Model.WFStatus = DataTableToJSONWithStringBuilder(dset.Tables[5]);

                        if (StatusCode != "D" || StatusCode != "F")
                        {
                            ViewBag.AppLevel = dset.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _ProductionAdvice_Model.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (dset.Tables[4].Rows.Count > 0)
                            {
                                sent_to = dset.Tables[4].Rows[0]["sent_to"].ToString();
                            }

                            if (dset.Tables[5].Rows.Count > 0)
                            {
                                nextLevel = dset.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                            }
                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _ProductionAdvice_Model.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionAdvice_Model.BtnName = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionAdvice_Model.BtnName = "BtnEdit";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model.BtnName = "BtnEdit";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionAdvice_Model.BtnName = "BtnEdit";
                                    }


                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model.BtnName = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionAdvice_Model.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _ProductionAdvice_Model.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _ProductionAdvice_Model.statuscode = StatusCode;
                        if (StatusCode == "C")
                        {
                            _ProductionAdvice_Model.cancelflag = true;
                        }
                        else
                        {
                            _ProductionAdvice_Model.cancelflag = false;
                        }

                        List<sourcetype> ffnlist = new List<sourcetype>();
                        sourcetype nsrc_Obj = new sourcetype();
                        nsrc_Obj.id = dset.Tables[0].Rows[0]["src_typeid"].ToString();
                        nsrc_Obj.name = dset.Tables[0].Rows[0]["src_type"].ToString();
                        ffnlist.Add(nsrc_Obj);
                        _ProductionAdvice_Model.ddl_src_typeList = ffnlist;

                        List<financial_year> nfyList = new List<financial_year>();
                        financial_year nfyObj = new financial_year();
                        nfyObj.id = dset.Tables[0].Rows[0]["f_fy"].ToString();
                        nfyObj.name = dset.Tables[0].Rows[0]["f_fyval"].ToString();
                        nfyList.Add(nfyObj);
                        _ProductionAdvice_Model.ddl_financial_yearList = nfyList;

                        List<period> nplist = new List<period>();
                        period pnObj = new period();
                        pnObj.id = dset.Tables[0].Rows[0]["f_period"].ToString();
                        pnObj.name = dset.Tables[0].Rows[0]["f_periodval"].ToString();
                        nplist.Add(pnObj);
                        _ProductionAdvice_Model.ddl_periodList = nplist;

                        DataRow Drow = dset.Tables[3].NewRow();
                        Drow[0] = "---Select---";
                        dset.Tables[3].Rows.InsertAt(Drow, 0);

                        List<revisionnumber> _nrevision = new List<revisionnumber>();
                        for (int x = 0; x < dset.Tables[3].Rows.Count; x++)
                        {
                            revisionnumber nrev = new revisionnumber();
                            nrev.rev_no = dset.Tables[3].Rows[x]["rev_no"].ToString();
                            nrev.rev_text = dset.Tables[3].Rows[x]["rev_no"].ToString();
                            _nrevision.Add(nrev);
                        }
                        _ProductionAdvice_Model.revisionnolist = _nrevision;

                        _ProductionAdvice_Model.ddl_revisionno = dset.Tables[0].Rows[0]["rev_no"].ToString();
                        _ProductionAdvice_Model.hdnddl_revisionno = dset.Tables[0].Rows[0]["rev_no"].ToString();

                        _ProductionAdvice_Model.hdnopdetail = Json(JsonConvert.SerializeObject(dset.Tables[1])).Data.ToString();
                        _ProductionAdvice_Model.hdnopitemdetail = Json(JsonConvert.SerializeObject(dset.Tables[2])).Data.ToString();

                        /*----------Attachment Section Start----------*/
                        ViewBag.AttechmentDetails = dset.Tables[7];
                        /*----------Attachment Section end----------*/
                        ViewBag.SubItemDetails = dset.Tables[8];

                        ViewBag.DocumentCode = StatusCode;
                        //Session["DocumentStatus"] = StatusCode;
                        _ProductionAdvice_Model.DocumentStatus = StatusCode;
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = getDocumentName();
                        _ProductionAdvice_Model.title = title;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionAdvice/ProductionAdviceDetail.cshtml", _ProductionAdvice_Model);
                    }

                    else
                    {
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = CommonPageDetails();
                        _ProductionAdvice_Model.title = title;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionAdvice/ProductionAdviceDetail.cshtml", _ProductionAdvice_Model);
                    }
                }
                
                //}
                //else
                //{
                //    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionAdvice/ProductionAdviceDetail.cshtml", _ProductionAdvice_Model);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CommonPageDetails()
        {
            try
            {
                var BrchID = "";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductionAdviceSave(ProductionAdvice_Model _ProductionAdvice_Model, string command)
        {
            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ProductionAdvice_Model.deletecommand == "Delete")
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
                        //Session["Command"] = "Add";
                        //Session["DocumentStatus"] = "";
                        ProductionAdvice_Model adddnew = new ProductionAdvice_Model();
                        adddnew.Command = "Add";
                        adddnew.TransType = "Save";
                        adddnew.BtnName = "BtnAddNew";
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "Add";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ProductionAdvice_Model.advice_no))
                                return RedirectToAction("EditDomesticAdviceDetails", new { PAdviceNo = _ProductionAdvice_Model.advice_no, PAdviceDate= _ProductionAdvice_Model.advice_dt, ListFilterData = _ProductionAdvice_Model.ListFilterData1, WF_Status = _ProductionAdvice_Model.WFStatus });
                            else
                                adddnew.Command = "Refresh";
                            adddnew.TransType = "Refresh";
                            adddnew.BtnName = "BtnRefresh";
                            adddnew.DocumentStatus = null;
                            TempData["ModelData"] = adddnew;
                            return RedirectToAction("ProductionAdviceDetail", adddnew);
                        }
                       
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ProductionAdviceDetail", NewModel);

                    case "Edit":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("EditDomesticAdviceDetails", new { PAdviceNo = _ProductionAdvice_Model.advice_no, PAdviceDate = _ProductionAdvice_Model.advice_dt, ListFilterData = _ProductionAdvice_Model.ListFilterData1, WF_Status = _ProductionAdvice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["Message"] = null;
                        //Session["PA_Number"] = _ProductionAdvice_Model.advice_no;
                        //Session["PA_Date"] = _ProductionAdvice_Model.advice_dt;
                        _ProductionAdvice_Model.Command = command;
                        _ProductionAdvice_Model.BtnName = "BtnEdit";
                        _ProductionAdvice_Model.TransType = "Update";
                        _ProductionAdvice_Model.PA_Number = _ProductionAdvice_Model.advice_no;
                        _ProductionAdvice_Model.PA_Date = _ProductionAdvice_Model.advice_dt;
                        UrlModel EditModel = new UrlModel();
                        EditModel.Cmd = command;
                        EditModel.tp = "Update";
                        EditModel.bt = "BtnEdit";
                        EditModel.PAC_No = _ProductionAdvice_Model.advice_no;
                        EditModel.PAC_dt = _ProductionAdvice_Model.advice_dt;
                        TempData["ListFilterData"] = _ProductionAdvice_Model.ListFilterData1;
                        return RedirectToAction("ProductionAdviceDetail", EditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "BtnRefresh";
                        Delete_AdviceDetails(_ProductionAdvice_Model, command);
                        ProductionAdvice_Model DeleteModel = new ProductionAdvice_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = _ProductionAdvice_Model.ListFilterData1;
                        return RedirectToAction("ProductionAdviceDetail", Delete_Model);

                    case "Save":
                        //Session["Command"] = command;
                        SaveUpdateProductionAdvice_Details(_ProductionAdvice_Model);
                        if (_ProductionAdvice_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        //Session["PA_Number"] = Session["PA_Number"].ToString();
                        //Session["PA_Date"] = Session["PA_Date"].ToString();
                        TempData["ModelData"] = _ProductionAdvice_Model;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = _ProductionAdvice_Model.BtnName;
                        SaveModel.PAC_No = _ProductionAdvice_Model.PA_Number;
                        SaveModel.PAC_dt = _ProductionAdvice_Model.PA_Date;
                        SaveModel.tp = _ProductionAdvice_Model.TransType;
                        SaveModel.Cmd = _ProductionAdvice_Model.Command;
                        return RedirectToAction("ProductionAdviceDetail", SaveModel);

                    case "Forward":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("EditDomesticAdviceDetails", new { PAdviceNo = _ProductionAdvice_Model.advice_no, PAdviceDate = _ProductionAdvice_Model.advice_dt, ListFilterData = _ProductionAdvice_Model.ListFilterData1, WF_Status = _ProductionAdvice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("EditDomesticAdviceDetails", new { PAdviceNo = _ProductionAdvice_Model.advice_no, PAdviceDate = _ProductionAdvice_Model.advice_dt, ListFilterData = _ProductionAdvice_Model.ListFilterData1, WF_Status = _ProductionAdvice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        // Session["Command"] = command;
                        //Session["PA_Number"] = _ProductionAdvice_Model.advice_no;
                        //Session["PA_Date"] = _ProductionAdvice_Model.advice_dt;
                        Approve_AdviceDetails(_ProductionAdvice_Model, command, "","");
                        TempData["ModelData"] = _ProductionAdvice_Model;
                        UrlModel Approve = new UrlModel();
                        Approve.tp = "Update";
                        Approve.PAC_No = _ProductionAdvice_Model.PA_Number;
                        Approve.PAC_dt = _ProductionAdvice_Model.PA_Date;
                        Approve.bt = "BtnEdit";
                        TempData["ListFilterData"] = _ProductionAdvice_Model.ListFilterData1;
                        return RedirectToAction("ProductionAdviceDetail", Approve);

                    case "Refresh":
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        ProductionAdvice_Model RefreshModel = new ProductionAdvice_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh_Model = new UrlModel();
                        refesh_Model.tp = "Save";
                        refesh_Model.bt = "BtnRefresh";
                        refesh_Model.Cmd = command;
                        TempData["ListFilterData"] = _ProductionAdvice_Model.ListFilterData1;
                        return RedirectToAction("ProductionAdviceDetail", refesh_Model);

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");                     
                         var WF_Status = _ProductionAdvice_Model.WF_Status1;
                        TempData["ListFilterData"] = _ProductionAdvice_Model.ListFilterData1;
                        return RedirectToAction("ProductionAdvice", new { WF_Status });

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
        public ActionResult SaveUpdateProductionAdvice_Details(ProductionAdvice_Model _ProductionAdvice_Model)
        {
            string SaveMessage = "";
            // getDocumentName(); /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");
            try
            {
                if (_ProductionAdvice_Model.cancelflag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }

                    DataTable PAdviceHeader = new DataTable();
                    DataTable PAdviceItemDetails = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(string));
                    dtheader.Columns.Add("br_id", typeof(string));
                    dtheader.Columns.Add("adv_no", typeof(string));
                    dtheader.Columns.Add("adv_dt", typeof(string));
                    dtheader.Columns.Add("src_type", typeof(string));
                    dtheader.Columns.Add("f_fy", typeof(string));
                    dtheader.Columns.Add("f_period", typeof(string));
                    dtheader.Columns.Add("from_date", typeof(string));
                    dtheader.Columns.Add("to_date", typeof(string));
                    dtheader.Columns.Add("product_id", typeof(string));
                    dtheader.Columns.Add("uom_id", typeof(string));
                    dtheader.Columns.Add("rev_no", typeof(string));
                    dtheader.Columns.Add("adv_qty", typeof(string));
                    dtheader.Columns.Add("batch_no", typeof(string));
                    dtheader.Columns.Add("cmpl_dt", typeof(string));
                    dtheader.Columns.Add("remarks", typeof(string));
                    dtheader.Columns.Add("user_id", typeof(string));
                    dtheader.Columns.Add("adv_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));
                    dtheader.Columns.Add("src_doc_no", typeof(string));
                    dtheader.Columns.Add("src_doc_dt", typeof(string));

                    DataRow dtHeaderrow = dtheader.NewRow();
                   // dtHeaderrow["TransType"] = Session["TransType"].ToString();
                    if(_ProductionAdvice_Model.advice_no != null)
                    {
                        dtHeaderrow["TransType"] = "Update";
                    }
                    else
                    {
                        dtHeaderrow["TransType"] = "Save";
                    }
                    dtHeaderrow["MenuDocumentId"] = DocumentMenuId;
                    //dtHeaderrow["comp_id"] = CompID;
                    dtHeaderrow["comp_id"] = Session["compid"].ToString();
                    dtHeaderrow["br_id"] = BrID;
                    dtHeaderrow["adv_no"] = _ProductionAdvice_Model.advice_no;
                    dtHeaderrow["adv_dt"] = _ProductionAdvice_Model.advice_dt;
                    dtHeaderrow["src_type"] = _ProductionAdvice_Model.ddlsrctype;
                    dtHeaderrow["f_fy"] = _ProductionAdvice_Model.hdnddl_finyear;
                    dtHeaderrow["f_period"] = _ProductionAdvice_Model.hdnddlperiod;
                    dtHeaderrow["from_date"] = _ProductionAdvice_Model.hdnfromdate;
                    dtHeaderrow["to_date"] = _ProductionAdvice_Model.hdntodate;
                    dtHeaderrow["product_id"] = _ProductionAdvice_Model.product_id;
                    dtHeaderrow["uom_id"] = _ProductionAdvice_Model.uom_id;
                    dtHeaderrow["rev_no"] = _ProductionAdvice_Model.hdnddl_revisionno;
                    dtHeaderrow["adv_qty"] = _ProductionAdvice_Model.advice_qty;
                    dtHeaderrow["batch_no"] = _ProductionAdvice_Model.batch_no;
                    dtHeaderrow["cmpl_dt"] = _ProductionAdvice_Model.completiondate;
                    dtHeaderrow["remarks"] = _ProductionAdvice_Model.remarks;
                    dtHeaderrow["user_id"] = UserID;
                    dtHeaderrow["adv_status"] = "D";
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    dtHeaderrow["mac_id"] = SystemDetail;
                    dtHeaderrow["src_doc_no"] = null;
                    dtHeaderrow["src_doc_dt"] = null;

                    dtheader.Rows.Add(dtHeaderrow);
                    PAdviceHeader = dtheader;


                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("op_id", typeof(string));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(string));
                    dtItem.Columns.Add("Item_type", typeof(string));
                    dtItem.Columns.Add("req_qty", typeof(string));

                    JArray jObject = JArray.Parse(_ProductionAdvice_Model.hdnmaterialdetail);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtItem.NewRow();
                        dtrowItemdetails["op_id"] = jObject[i]["OpId"].ToString();
                        dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowItemdetails["uom_id"] = jObject[i]["UomId"].ToString();
                        dtrowItemdetails["Item_type"] = jObject[i]["InputTypeId"].ToString();
                        dtrowItemdetails["req_qty"] = jObject[i]["Qty"].ToString();
                        dtItem.Rows.Add(dtrowItemdetails);
                    }
                    PAdviceItemDetails = dtItem;

                    /*-----------------Attachment Section Start------------------------*/
                    DataTable ProdAdvAttachments = new DataTable();
                    DataTable prad_dtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as Pro_Model;
                    TempData["IMGDATA"] = null;
                    if (_ProductionAdvice_Model.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            //{
                            //    prad_dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //}
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                prad_dtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                prad_dtAttachment.Columns.Add("id", typeof(string));
                                prad_dtAttachment.Columns.Add("file_name", typeof(string));
                                prad_dtAttachment.Columns.Add("file_path", typeof(string));
                                prad_dtAttachment.Columns.Add("file_def", typeof(char));
                                prad_dtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_ProductionAdvice_Model.AttachMentDetailItmStp != null)
                            {
                                prad_dtAttachment = _ProductionAdvice_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                prad_dtAttachment.Columns.Add("id", typeof(string));
                                prad_dtAttachment.Columns.Add("file_name", typeof(string));
                                prad_dtAttachment.Columns.Add("file_path", typeof(string));
                                prad_dtAttachment.Columns.Add("file_def", typeof(char));
                                prad_dtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_ProductionAdvice_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in prad_dtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = prad_dtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_ProductionAdvice_Model.advice_no).ToString()))
                                {
                                    dtrowAttachment1["id"] = _ProductionAdvice_Model.advice_no;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                prad_dtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_ProductionAdvice_Model.TransType == "Update")
                        {
                            //string br_id = Session["BranchId"].ToString();
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string PrdAd_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_ProductionAdvice_Model.advice_no).ToString()))
                                {
                                    PrdAd_CODE = (_ProductionAdvice_Model.advice_no).ToString();

                                }
                                else
                                {
                                    PrdAd_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrID + PrdAd_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in prad_dtAttachment.Rows)
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
                        ProdAdvAttachments = prad_dtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/

                    /*------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    if (_ProductionAdvice_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_ProductionAdvice_Model.SubItemDetailsDt);
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

                    //string br_id = Session["BranchId"].ToString();

                    SaveMessage = _ProductionAdvice_IService.InsertUpdateProductionAdvice_Details(PAdviceHeader, PAdviceItemDetails, ProdAdvAttachments, dtSubItem);
                  var A = SaveMessage.Split(',');
                    string date = A[2].Trim();
                    string PA_Number = A[1].Trim();
                   // string PA_Number = SaveMessage.Substring(SaveMessage.IndexOf(',') + 1);
                    string PrdAd_Number = PA_Number.Replace("/", "");
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf(","));
                    if (Message == "Data_Not_Found")
                    {
                        //var a = SaveMessage.Split(',');
                        var msg = Message.Replace("_", " ") + " " + PA_Number+" in "+PageName;//PA_Number is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _ProductionAdvice_Model.Message = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("ProductionAdviceDetail");
                    }
                    /*-----------------Attachment Section Start------------------------*/
                    if (Message == "Save")

                    {
                        //if (Session["Guid"] != null)
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (attachData != null)
                        {
                            if (attachData.Guid != null)
                            {
                                Guid = attachData.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrID, guid, PageName, PA_Number, _ProductionAdvice_Model.TransType, ProdAdvAttachments);
                        //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                        //if (Directory.Exists(sourcePath))
                        //{

                        //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrID + Guid + "_" + "*");
                        //    foreach (string file in filePaths)
                        //    {
                        //        string[] items = file.Split('\\');
                        //        string ItemName = items[items.Length - 1];
                        //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                        //        foreach (DataRow dr in ProdAdvAttachments.Rows)
                        //        {
                        //            string DrItmNm = dr["file_name"].ToString();
                        //            if (ItemName == DrItmNm)
                        //            {
                        //                string img_nm = CompID + BrID + PrdAd_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                        //    Session["Message"] = "Save";
                        //Session["Command"] = "Update";
                        //Session["PA_Number"] = PA_Number;
                        //Session["PA_Date"] = _ProductionAdvice_Model.advice_dt;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnEdit";
                        _ProductionAdvice_Model.Message = "Save";
                    _ProductionAdvice_Model.PA_Number = PA_Number;
                   // _ProductionAdvice_Model.PA_Date = _ProductionAdvice_Model.advice_dt;
                    _ProductionAdvice_Model.PA_Date = date;
                    _ProductionAdvice_Model.TransType = "Update";
                    _ProductionAdvice_Model.BtnName = "BtnEdit";
                    return RedirectToAction("ProductionAdviceDetail");
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
                    string AdviceNo = _ProductionAdvice_Model.advice_no;
                    string AdviceDate = _ProductionAdvice_Model.advice_dt;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    SaveMessage = _ProductionAdvice_IService.Cancelled_ProductionAdviceDetail(CompID, br_id, AdviceNo, AdviceDate, UserID, mac_id, DocumentMenuId);
                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["PA_Number"] = AdviceNo;
                    //Session["PA_Date"] = AdviceDate;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "BtnEdit";
                    _ProductionAdvice_Model.Message = "Cancelled";
                    _ProductionAdvice_Model.PA_Number = AdviceNo;
                    _ProductionAdvice_Model.PA_Date = AdviceDate;
                    _ProductionAdvice_Model.TransType = "Update";
                    _ProductionAdvice_Model.BtnName = "BtnEdit";
                    return RedirectToAction("ProductionAdviceDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ProductionAdvice_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ProductionAdvice_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ProductionAdvice_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
            }

        }
        //private DataTable GetRoleList()
        //{
        //    try
        //    {
        //        string UserID = string.Empty;
        //        string CompID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            UserID = Session["userid"].ToString();
        //        }
        //        DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

        //        return RoleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        public void GetStatusList(ProductionAdvice_Model _ProductionAdvice_Model)
        {
            try
            {
                List<status> statusLists = new List<status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new status { status_id = x.status_id, status_name = x.status_name });
                _ProductionAdvice_Model.statuslist = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private DataTable GetAdvicelistDetails(ProductionAdvice_Model _ProductionAdvice_Model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                string UserID = "";
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
               if( _ProductionAdvice_Model.WF_Status != null)
                {
                    wfstatus = _ProductionAdvice_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                DataTable dt = new DataTable();
                dt = _ProductionAdvice_IService.GetAdviceList("0", "A", _ProductionAdvice_Model.txtfromdate, _ProductionAdvice_Model.txttodate, _ProductionAdvice_Model.status, CompID, BrID, UserID, DocumentMenuId, wfstatus);

                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        //[HttpPost]
        private DataTable GetProductList(SearchItem search, string SrcType, string fy, string period)
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
                    BrID = Session["BranchId"].ToString();
                }
                DataTable dt = _ProductionAdvice_IService.Bind_ProductList1(CompID, BrID, SrcType, fy, period, search.SearchName);
                //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult BindProductList(SearchItem search, string SrcType, string fy, string period)
        {
            //JsonResult DataRows = null;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(search.SearchName))
                    {
                        search.SearchName = "";
                    }
                    DataSet ds = _ProductionAdvice_IService.Bind_ProductList(CompID, BrID, SrcType, fy, period, search.SearchName);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //ItemList.Add("0" + "_" + "H1", "Heading");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string itemId = ds.Tables[0].Rows[i]["Item_id"].ToString();
                            string itemName = ds.Tables[0].Rows[i]["Item_name"].ToString();
                            string Uom = ds.Tables[0].Rows[i]["uom_name"].ToString();
                            ItemList.Add(itemId + "_" + Uom, itemName);
                        }
                    }

                    //DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
                return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            //return DataRows;
        }
        [HttpPost]
        public ActionResult BindPeriodList(string fy)
        {
            JsonResult DataRows = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    DataSet ds = _ProductionAdvice_IService.Bind_PeriodList(CompID, BrID, fy);

                    DataRow Drow = ds.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    ds.Tables[0].Rows.InsertAt(Drow, 0);

                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
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
        [HttpPost]
        public ActionResult BindPeriodRAndProductList(string fy, string period)
        {
            JsonResult DataRows = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    DataSet ds = _ProductionAdvice_IService.Bind_PeriodRAndProductList(CompID, BrID, fy, period);

                    DataRow Drow = ds.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    Drow[2] = "0";
                    Drow[3] = "0";
                    Drow[4] = "0";
                    Drow[5] = "0";
                    Drow[6] = "0";
                    ds.Tables[0].Rows.InsertAt(Drow, 0);

                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
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
        [HttpPost]
        public ActionResult BindRevisionNnoList(string productid)
        {
            JsonResult DataRows = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    DataSet ds = _ProductionAdvice_IService.Bind_RevisionNoList(CompID, BrID, productid);

                    DataRow Drow = ds.Tables[0].NewRow();
                    Drow[0] = "---Select---";
                    ds.Tables[0].Rows.InsertAt(Drow, 0);

                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
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
        [HttpPost]
        public ActionResult GetMtaterialDetail(string productid, string revno)
        {
            JsonResult DataRows = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    DataSet ds = _ProductionAdvice_IService.Get_MaterialDetail(CompID, BrID, productid, revno);

                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
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
        public ActionResult ProductionAdviceListSearch(string Productid, string Source, string Fromdate, string Todate, string Status)
        {
            try
            {
                ProductionAdvice_Model _ProductionAdvice_Model = new ProductionAdvice_Model();
                //Session.Remove("WF_status");
                _ProductionAdvice_Model.WF_Status= null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                dt = _ProductionAdvice_IService.GetAdviceList(Productid, Source, Fromdate, Todate, Status, CompID, BrID, "", "", "");
                //Session["ADVSearch"] = "ADV_Search";
                _ProductionAdvice_Model.ADVSearch = "ADV_Search";
                ViewBag.AdvMaterialDetails = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_ProductionAdvice.cshtml", _ProductionAdvice_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult EditDomesticAdviceDetails(string PAdviceNo, string PAdviceDate, string ListFilterData,string WF_Status)
        { /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
            {
                TempData["Message1"] = "Financial Year not Exist";
            }
            /*End to chk Financial year exist or not*/
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["PA_Number"] = PAdviceNo;
            //Session["PA_Date"] = PAdviceDate;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnEdit";
            ProductionAdvice_Model dblclick = new ProductionAdvice_Model();
            UrlModel _url = new UrlModel();
            dblclick.PA_Number = PAdviceNo;
            dblclick.PA_Date = PAdviceDate;
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnEdit";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            //_url.Cmd = "Update";
            _url.tp = "Update";
            _url.bt = "BtnEdit";
            _url.PAC_No = PAdviceNo;
            _url.PAC_dt = PAdviceDate;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("ProductionAdviceDetail", _url);
        }
        private ActionResult Delete_AdviceDetails(ProductionAdvice_Model _ProductionAdvice_Model, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                string prdadNo = _ProductionAdvice_Model.advice_no;
                string PrdAdvice_Number = prdadNo.Replace("/", "");
                DataSet Message = _ProductionAdvice_IService.Delete_PAdviceDetails(CompID, BrID, _ProductionAdvice_Model.advice_no, _ProductionAdvice_Model.advice_dt);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(prdadNo))
                {
                    //getDocumentName(); /* To set Title*/
                    CommonPageDetails();
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + BrID, PageName, PrdAdvice_Number, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["BtnName"] = "BtnRefresh";
                //Session["Command"] = "Refresh";
                //Session["TransType"] = "Save";
                //Session["Message"] = "Deleted";
                //Session["DocumentStatus"] = null;

                return RedirectToAction("ProductionAdviceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult Approve_AdviceDetails(ProductionAdvice_Model _ProductionAdvice_Model, string command, string ListFilterData1,string WF_Status1)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();

                }
                string UserID = Session["UserId"].ToString();
                string AdviceNo = _ProductionAdvice_Model.advice_no;
                string AdviceDate = _ProductionAdvice_Model.advice_dt;
                string A_Status = _ProductionAdvice_Model.A_Status;
                string A_Level = _ProductionAdvice_Model.A_Level;
                string A_Remarks = _ProductionAdvice_Model.A_Remarks;


                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Message = _ProductionAdvice_IService.Approved_PAdviceDetails(CompID, BrID, UserID, AdviceNo, AdviceDate, DocumentMenuId, mac_id, A_Status, A_Level, A_Remarks);

                if (Message == "Approved")
                {
                    // Session["Message"] = "Approved";
                    _ProductionAdvice_Model.Message = "Approved";
                }
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["PA_Number"] = _ProductionAdvice_Model.advice_no;
                //Session["PA_Date"] = _ProductionAdvice_Model.advice_dt;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel ApproveModel = new UrlModel();
                _ProductionAdvice_Model.TransType = "Update";
                _ProductionAdvice_Model.PA_Number = _ProductionAdvice_Model.advice_no;
                _ProductionAdvice_Model.PA_Date = _ProductionAdvice_Model.advice_dt;
                _ProductionAdvice_Model.Message = "Approved";
                _ProductionAdvice_Model.BtnName = "BtnEdit";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _ProductionAdvice_Model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _ProductionAdvice_Model;

                ApproveModel.tp = "Update";
                ApproveModel.PAC_No = _ProductionAdvice_Model.PA_Number;
                ApproveModel.PAC_dt = _ProductionAdvice_Model.PA_Date;
                ApproveModel.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("MaterialRequirementPlanDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1,string ModelData)
        {
            //Session["Message"] = "";
            ProductionAdvice_Model ToRefreshByJS = new ProductionAdvice_Model();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.PA_Number = a[0].Trim();
            ToRefreshByJS.PA_Date = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "BtnEdit";
            Model.PAC_No = ToRefreshByJS.PA_Number;
            Model.PAC_dt = ToRefreshByJS.PA_Date;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ProductionAdviceDetail", Model);
        }
        public ActionResult GetProductionAdviceList(string docid, string status)
        {

            //Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("ProductionAdvice",new { WF_Status });
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1,string WF_Status1)
        {

            ProductionAdvice_Model _ProductionAdvice_Model = new ProductionAdvice_Model();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _ProductionAdvice_Model.advice_no = jObjectBatch[i]["DocNo"].ToString();
                    _ProductionAdvice_Model.advice_dt = jObjectBatch[i]["DocDate"].ToString();
                    _ProductionAdvice_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _ProductionAdvice_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _ProductionAdvice_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_ProductionAdvice_Model.A_Status != "Approve")
            {
                _ProductionAdvice_Model.A_Status = "Approve";
            }
            string command = "";
            Approve_AdviceDetails(_ProductionAdvice_Model, command, ListFilterData1, WF_Status1);
            UrlModel ApproveModel = new UrlModel();
            if (WF_Status1 != null && WF_Status1 != "")
            {
                ApproveModel.wf = WF_Status1;
            }
            TempData["ModelData"] = _ProductionAdvice_Model;
            ApproveModel.tp = "Update";
            ApproveModel.PAC_No = _ProductionAdvice_Model.advice_no;
            ApproveModel.PAC_dt = _ProductionAdvice_Model.advice_dt;
            ApproveModel.bt = "BtnEdit";
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ProductionAdviceDetail", ApproveModel);
        }
        [HttpPost]
        public JsonResult GetProductionAdvice_Detail(string Adv_No, string Adv_Date)
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
                DataSet result = _ProductionAdvice_IService.GetProdAdv_Detail(Comp_ID, BranchID, Adv_No, Adv_Date);
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

        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                     Pro_Model _attachmentModel = new Pro_Model();
                //string TransType = "";
                //string ProdAdCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["PA_Number"] != null)
                //{
                //    ProdAdCode = Session["PA_Number"].ToString();
                //}
                if (TransType == "Save")
                {
                    //ProdAdCode = gid.ToString();
                    DocNo = gid.ToString();
                }
                //ProdAdCode = ProdAdCode.Replace("/", "");
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = ProdAdCode;
                _attachmentModel.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
               // getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _attachmentModel.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _attachmentModel.AttachMentDetailItmStp = null;
                }
                TempData["IMGDATA"] = _attachmentModel;
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

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
            , string Flag, string Status, string doc_no, string doc_dt)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();

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
                    dt = _ProductionAdvice_IService.ADV_GetSubItemDetails(CompID, BrID, Item_id, doc_no, doc_dt, Flag).Tables[0];
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled
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


    }

}