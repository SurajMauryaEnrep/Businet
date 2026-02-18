using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.OCDetail;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
//Modifyed by Shubham Maurya on 14-12-2022 Removed All Session//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class OCDetailController : Controller
    {
        string comp_id, userid, OCId = string.Empty;
        string Comp_ID, Br_ID, FromDate, Language,  UserID = String.Empty;
        OCDetailModel _OCDetail;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        string CompID, language = String.Empty;
        string DocumentMenuId = "103195",title;
        Common_IServices _Common_IServices;
        OCDetail_ISERVICES _OCDetail_ISERVICES;
        // GET: BusinessLayer/OCDetail

        public OCDetailController(Common_IServices _Common_IServices,OCDetail_ISERVICES _OCDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._OCDetail_ISERVICES = _OCDetail_ISERVICES;
        }
        public ActionResult OCDetail(OCDetailModel _OCDetail1)
        {
            try
            {
                _OCDetail=TempData["ModelData"] as OCDetailModel;
                if (_OCDetail != null)
                {
                    ViewBag.VBRoleList = GetRoleList();
                    ViewBag.MenuPageName = getDocumentName();
                    ViewBag.HSN_Used = "N";
                    //_OCDetail = new OCDetailModel();
                    _OCDetail.Title = title;
                    string Comp_ID = string.Empty;
                    _OCDetail.oc_id = 0;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    //dt = GetOCBranchList();
                    ////ViewBag.CustomerBranchList = dt;
                    //_OCDetail.CustomerBranchList = dt;
                    //dt = GetCOAList();
                    //List<OCCOA> _TaxCOAList = new List<OCCOA>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    OCCOA _COAList = new OCCOA();
                    //    _COAList.acc_id = dt["acc_id"].ToString();
                    //    _COAList.acc_name = dt["acc_name"].ToString();
                    //    _TaxCOAList.Add(_COAList);
                    //}
                    //_TaxCOAList.Insert(0, new OCCOA() { acc_id = "0", acc_name = "---Select---" });
                    //_OCDetail.COAList = _TaxCOAList;
                    GetAllData(_OCDetail);
                    DataTable TaxTmpltDt = _Common_IServices.BindTaxTemplatelist(Comp_ID, DocumentMenuId,"TAX").Tables[0];
                    List<TaxTemplateList> taxTemplate = new List<TaxTemplateList>();
                    foreach (DataRow dr in TaxTmpltDt.Rows)
                    {
                        TaxTemplateList list = new TaxTemplateList();
                        list.tmplt_id = dr["tmplt_id"].ToString();
                        list.tmplt_nm = dr["tmplt_name"].ToString();
                        taxTemplate.Add(list);
                    }
                    _OCDetail.templateLists = taxTemplate;
                    //dt = GetTPCOAList();
                    //List<ThirdPartyCOA> _ThirdPartyCOAist = new List<ThirdPartyCOA>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    ThirdPartyCOA _ThirdPartyCOA = new ThirdPartyCOA();
                    //    _ThirdPartyCOA.supp_id = dt["supp_id"].ToString();
                    //    _ThirdPartyCOA.supp_name = dt["supp_name"].ToString();
                    //    _ThirdPartyCOAist.Add(_ThirdPartyCOA);
                    //}
                    //_ThirdPartyCOAist.Insert(0, new ThirdPartyCOA() { supp_id = "0", supp_name = "---Select---" });
                    //_OCDetail.ThirdPartyCOAList = _ThirdPartyCOAist;

                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _OCDetail.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (_OCDetail.TransType == "Update" || _OCDetail.Command == "Edit")
                    {
                        //string OCCode = Session["OCCode"].ToString();
                        string OCCode = _OCDetail.OCCode;
                        Boolean act_status;
                        DataSet ds = _OCDetail_ISERVICES.GetviewOCdetailDAL(OCCode, Comp_ID);
                        if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                            act_status = true;
                        else
                            act_status = false;
                        _OCDetail.oc_id = int.Parse(ds.Tables[0].Rows[0]["oc_id"].ToString());
                        _OCDetail.oc_name = ds.Tables[0].Rows[0]["oc_name"].ToString();
                        _OCDetail.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                        //_OCDetail.tp_id = int.Parse(ds.Tables[0].Rows[0]["tp_id"].ToString());
                        _OCDetail.tp_id = ds.Tables[0].Rows[0]["tp_id"].ToString();
                        _OCDetail.oc_type = ds.Tables[0].Rows[0]["oc_type"].ToString();
                        _OCDetail.app_date = ds.Tables[0].Rows[0]["app_date"].ToString();
                        _OCDetail.act_status = act_status;
                        _OCDetail.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _OCDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _OCDetail.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _OCDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        string tax_applicable = ds.Tables[0].Rows[0]["tax_applicable"].ToString();
                        if (tax_applicable == "Y")
                        {
                            _OCDetail.TaxApplicable = true;
                        }
                        else
                        {
                            _OCDetail.TaxApplicable = false;
                        }
                        _OCDetail.TaxTemplate = ds.Tables[0].Rows[0]["tax_tmplt"].ToString();
                        _OCDetail.HSN_code = ds.Tables[0].Rows[0]["HSN_code"].ToString();
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _OCDetail.CustomerBranchList = ds.Tables[1];
                        //Session["OCType"] = ds.Tables[0].Rows[0]["oc_type"].ToString();
                        _OCDetail.OCType = ds.Tables[0].Rows[0]["oc_type"].ToString();
                        ViewBag.HSN_Used = ds.Tables[0].Rows[0]["HSN_Used"].ToString();
                    }
                    else
                    {
                        _OCDetail.act_status = true;
                    }
                    _OCDetail.hdnSavebtn = "";
                    //_OCDetail.TransType = Session["TransType"].ToString();
                    _OCDetail.TransType = _OCDetail.TransType;
                    return View("~/Areas/BusinessLayer/Views/OCSetup/OCDetail.cshtml", _OCDetail);
                }
                else
                {
                    ViewBag.VBRoleList = GetRoleList();
                    ViewBag.MenuPageName = getDocumentName();
                    //_OCDetail1 = new OCDetailModel();
                    _OCDetail1.Title = title;
                    string Comp_ID = string.Empty;
                    _OCDetail1.oc_id = 0;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (_OCDetail1.BtnName == null)
                    {
                        _OCDetail1.BtnName = "BtnRefresh";
                    }
                    if (_OCDetail1.BtnName == "BtnEdit")
                    {
                        _OCDetail1.BtnName = "BtnToDetailPage";
                    }
                    if (_OCDetail1.TransType != null)
                    {
                        _OCDetail1.TransType = "Update";
                        _OCDetail1.Command = "Add";
                    }
                    // dt = GetOCBranchList();

                    //ViewBag.CustomerBranchList = dt;
                    //    _OCDetail1.CustomerBranchList = dt;
                    //dt = GetCOAList();
                    //List<OCCOA> _TaxCOAList = new List<OCCOA>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    OCCOA _COAList = new OCCOA();
                    //    _COAList.acc_id = dt["acc_id"].ToString();
                    //    _COAList.acc_name = dt["acc_name"].ToString();
                    //    _TaxCOAList.Add(_COAList);
                    //}
                    //_TaxCOAList.Insert(0, new OCCOA() { acc_id = "0", acc_name = "---Select---" });
                    //_OCDetail1.COAList = _TaxCOAList;
                    GetAllData(_OCDetail1);
                    DataTable TaxTmpltDt = _Common_IServices.BindTaxTemplatelist(Comp_ID, DocumentMenuId,"TAX").Tables[0];
                    List<TaxTemplateList> taxTemplate = new List<TaxTemplateList>();
                    foreach (DataRow dr in TaxTmpltDt.Rows)
                    {
                        TaxTemplateList list = new TaxTemplateList();
                        list.tmplt_id = dr["tmplt_id"].ToString();
                        list.tmplt_nm = dr["tmplt_name"].ToString();
                        taxTemplate.Add(list);
                    }
                    _OCDetail1.templateLists = taxTemplate;
                    //dt = GetTPCOAList();
                    //List<ThirdPartyCOA> _ThirdPartyCOAist = new List<ThirdPartyCOA>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    ThirdPartyCOA _ThirdPartyCOA = new ThirdPartyCOA();
                    //    _ThirdPartyCOA.supp_id = dt["supp_id"].ToString();
                    //    _ThirdPartyCOA.supp_name = dt["supp_name"].ToString();
                    //    _ThirdPartyCOAist.Add(_ThirdPartyCOA);
                    //}
                    //_ThirdPartyCOAist.Insert(0, new ThirdPartyCOA() { supp_id = "0", supp_name = "---Select---" });
                    //_OCDetail1.ThirdPartyCOAList = _ThirdPartyCOAist;

                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }

                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (_OCDetail1.TransType == "Update" || _OCDetail1.Command == "Edit")
                    {
                        //string OCCode = Session["OCCode"].ToString();
                        string OCCode = _OCDetail1.OCCode;
                        Boolean act_status;
                        DataSet ds = _OCDetail_ISERVICES.GetviewOCdetailDAL(OCCode, Comp_ID);
                        if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                            act_status = true;
                        else
                            act_status = false;
                        _OCDetail1.oc_id = int.Parse(ds.Tables[0].Rows[0]["oc_id"].ToString());
                        _OCDetail1.oc_name = ds.Tables[0].Rows[0]["oc_name"].ToString();
                        _OCDetail1.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                        //_OCDetail1.tp_id = int.Parse(ds.Tables[0].Rows[0]["tp_id"].ToString());
                        _OCDetail1.tp_id = ds.Tables[0].Rows[0]["tp_id"].ToString();
                        _OCDetail1.oc_type = ds.Tables[0].Rows[0]["oc_type"].ToString();
                        _OCDetail1.app_date = ds.Tables[0].Rows[0]["app_date"].ToString();
                        _OCDetail1.act_status = act_status;
                        _OCDetail1.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _OCDetail1.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _OCDetail1.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _OCDetail1.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        string tax_applicable = ds.Tables[0].Rows[0]["tax_applicable"].ToString();
                        if (tax_applicable == "Y")
                        {
                            _OCDetail1.TaxApplicable = true;
                        }
                        else
                        {
                            _OCDetail1.TaxApplicable = false;
                        }
                        _OCDetail1.TaxTemplate = ds.Tables[0].Rows[0]["tax_tmplt"].ToString();
                        _OCDetail1.HSN_code = ds.Tables[0].Rows[0]["HSN_code"].ToString();
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _OCDetail1.CustomerBranchList = ds.Tables[1];
                        //Session["OCType"] = ds.Tables[0].Rows[0]["oc_type"].ToString();
                        _OCDetail1.OCType = ds.Tables[0].Rows[0]["oc_type"].ToString();
                    }
                    else
                    {
                        _OCDetail1.act_status = true;
                    }
                    _OCDetail1.hdnSavebtn = "";
                    //_OCDetail1.TransType = Session["TransType"].ToString();
                    _OCDetail1.TransType = _OCDetail1.TransType;
                    return View("~/Areas/BusinessLayer/Views/OCSetup/OCDetail.cshtml", _OCDetail1);
                }
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }

        private void GetAllData(OCDetailModel _OCDetail1)
        {
            string CompID = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
         
            DataSet data = _OCDetail_ISERVICES.GetallData(CompID);
            _OCDetail1.CustomerBranchList = data.Tables[0];
          //  dt = GetCOAList();
            List<OCCOA> _TaxCOAList = new List<OCCOA>();
            foreach (DataRow dt in data.Tables[1].Rows)
            {
                OCCOA _COAList = new OCCOA();
                _COAList.acc_id = dt["acc_id"].ToString();
                _COAList.acc_name = dt["acc_name"].ToString();
                _TaxCOAList.Add(_COAList);
            }
            _TaxCOAList.Insert(0, new OCCOA() { acc_id = "0", acc_name = "---Select---" });
            _OCDetail1.COAList = _TaxCOAList;

         //   dt = GetTPCOAList();
            List<ThirdPartyCOA> _ThirdPartyCOAist = new List<ThirdPartyCOA>();
            foreach (DataRow dt in data.Tables[2].Rows)
            {
                ThirdPartyCOA _ThirdPartyCOA = new ThirdPartyCOA();
                _ThirdPartyCOA.supp_id = dt["supp_id"].ToString();
                _ThirdPartyCOA.supp_name = dt["supp_name"].ToString();
                _ThirdPartyCOAist.Add(_ThirdPartyCOA);
            }
            _ThirdPartyCOAist.Insert(0, new ThirdPartyCOA() { supp_id = "0", supp_name = "---Select---" });
            _OCDetail1.ThirdPartyCOAList = _ThirdPartyCOAist;

            /**********************************Bind HSn ********************************************/

            List<HSNno> _HSNList = new List<HSNno>();
            foreach (DataRow dr in data.Tables[3].Rows)
            {
                HSNno _HsnDetail = new HSNno();
                _HsnDetail.setup_id = dr["setup_id"].ToString();
                _HsnDetail.setup_val = dr["setup_val"].ToString();
                _HSNList.Add(_HsnDetail);
            }
            _HSNList.Insert(0, new HSNno() { setup_id = "0", setup_val = "---Select---" });
            _OCDetail1.HSNList = _HSNList;

            /****************************************End**************************************************/
            CommonPageDetails();
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
        private DataTable GetRoleList()
        {
            try
            {
                string UserID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult OCSave(OCDetailModel _OCDetail, string command, int oc_id)
        {
            try
            {                
                switch (command)
                {
                    case "Edit":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["OCCode"] = _OCDetail.oc_id;
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "BtnEdit";

                        _OCDetail.hdnSavebtn = null;
                        _OCDetail.Message = "";
                        _OCDetail.Command = command;
                        _OCDetail.OCCode = _OCDetail.oc_id.ToString();
                        _OCDetail.OCCode = _OCDetail.hdnoc_id;
                        _OCDetail.TransType = "Update";
                        _OCDetail.BtnName = "BtnEdit";
                        TempData["ModelData"] = _OCDetail;
                        TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                        return RedirectToAction("OCDetail", "OCDetail", _OCDetail);

                    case "Add":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["OCCode"] = "";
                        //Session["AppStatus"] = "D";
                        ////_OCDetail = null;
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        _OCDetail.hdnSavebtn = null;
                        _OCDetail.Message = "";
                        _OCDetail.Command = command;
                        _OCDetail.OCCode = "";
                        _OCDetail.AppStatus = "D";
                        //_OCDetail = null;
                        _OCDetail.TransType = "Save";
                        _OCDetail.BtnName = "BtnAddNew";
                        //_OCDetail.OCCode = "0";
                        TempData["ModelData"] = _OCDetail;
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("OCDetail", "OCDetail");


                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Delete";
                        _OCDetail.hdnSavebtn = null;
                        _OCDetail.Command = command;
                        _OCDetail.BtnName = "Delete";
                        oc_id = _OCDetail.oc_id;
                        TempData["ModelData"] = _OCDetail;
                        TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                        return RedirectToAction("OCDetail");

                    case "Save":
                        //Session["Command"] = command;
                        _OCDetail.Command = command;
                        if (ModelState.IsValid)
                        {
                            InsertOCDetail(_OCDetail);
                            if (_OCDetail.Message == "DataNotFound")
                            {
                                _OCDetail.hdnSavebtn = null;
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            //Session["OCCode"] = Session["OCCode"].ToString();

                            //if (Session["Message"].ToString() == "Duplicate")
                            if (_OCDetail.Message == "Duplicate")
                            {
                                
                                _OCDetail.hdnSavebtn = null;
                                dt = GetOCBranchList();
                                //ViewBag.CustomerBranchList = dt;
                                _OCDetail.CustomerBranchList = dt;
                                //dt = GetCOAList();
                                //List<OCCOA> _TaxCOAList = new List<OCCOA>();
                                //foreach (DataRow dt in dt.Rows)
                                //{
                                //    OCCOA _COAList = new OCCOA();
                                //    _COAList.acc_id = dt["acc_id"].ToString();
                                //    _COAList.acc_name = dt["acc_name"].ToString();
                                //    _TaxCOAList.Add(_COAList);
                                //}
                                //_TaxCOAList.Insert(0, new OCCOA() { acc_id = "0", acc_name = "---Select---" });
                                //_OCDetail.COAList = _TaxCOAList;

                                //dt = GetTPCOAList();
                                //List<ThirdPartyCOA> _ThirdPartyCOAist = new List<ThirdPartyCOA>();
                                //foreach (DataRow dt in dt.Rows)
                                //{
                                //    ThirdPartyCOA _ThirdPartyCOA = new ThirdPartyCOA();
                                //    _ThirdPartyCOA.supp_id = dt["supp_id"].ToString();
                                //    _ThirdPartyCOA.supp_name = dt["supp_name"].ToString();
                                //    _ThirdPartyCOAist.Add(_ThirdPartyCOA);
                                //}
                                GetAllData(_OCDetail);
                                string Comp_ID = string.Empty;
                                if (Session["CompId"] != null)
                                {
                                    Comp_ID = Session["CompId"].ToString();
                                }
                                var OCCode = _OCDetail.OCCode;
                                if (OCCode != "0")
                                {
                                    DataSet ds = _OCDetail_ISERVICES.GetviewOCdetailDAL(OCCode, Comp_ID);
                                    _OCDetail.CustomerBranchList = ds.Tables[1];
                                    _OCDetail.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                                    _OCDetail.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                                    _OCDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                    _OCDetail.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                    _OCDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                }                                
                                DataTable TaxTmpltDt = _Common_IServices.BindTaxTemplatelist(Comp_ID, DocumentMenuId,"TAX").Tables[0];
                                List<TaxTemplateList> taxTemplate = new List<TaxTemplateList>();
                                foreach (DataRow dr in TaxTmpltDt.Rows)
                                {
                                    TaxTemplateList list = new TaxTemplateList();
                                    list.tmplt_id = dr["tmplt_id"].ToString();
                                    list.tmplt_nm = dr["tmplt_name"].ToString();
                                    taxTemplate.Add(list);
                                }
                                _OCDetail.templateLists = taxTemplate;

                                //_ThirdPartyCOAist.Insert(0, new ThirdPartyCOA() { supp_id = "0", supp_name = "---Select---" });
                                //_OCDetail.ThirdPartyCOAList = _ThirdPartyCOAist;
                                //_OCDetail.TaxTemplate = "0";
                                //Session["OCCode"] = "";
                                //Session["AppStatus"] = "D";
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                //ViewBag.Message = Session["Message"].ToString();

                                //_OCDetail.OCCode= "";
                                _OCDetail.AppStatus = "D";
                                _OCDetail.TransType = "Save";
                                _OCDetail .BtnName= "BtnAddNew";
                                _OCDetail.oc_id = Convert.ToInt32(_OCDetail.OCCode);
                                if (_OCDetail.acc_id == 0)
                                {
                                    _OCDetail.acc_id =Convert.ToInt32(_OCDetail.ddl_acc_id);
                                }
                                if (_OCDetail.tp_id == null)
                                {
                                    _OCDetail.tp_id = "0";
                                }
                                //_OCDetail.Message = Session["Message"].ToString();
                                TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                                _OCDetail.hdnSavebtn = "";
                                _OCDetail.EnableforhdnSavebtn = "Duplicate";
                                return View("~/Areas/BusinessLayer/Views/OCSetup/OCDetail.cshtml", _OCDetail);
                            }
                            else
                            {
                                TempData["ModelData"] = _OCDetail;
                                _OCDetail.hdnSavebtn = "";
                                TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                                return RedirectToAction("OCDetail", _OCDetail);
                            }

                        }
                        else
                        {
                            TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                            _OCDetail = null;
                            _OCDetail.hdnSavebtn = "";
                            return View("~/Areas/BusinessLayer/Views/OCSetup/OCDetail.cshtml", _OCDetail);
                        }

                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        _OCDetail.hdnSavebtn = null;
                        //Session["Command"] = command;                        
                        //Session["OCCode"] = oc_id;
                        //oc_id = _OCDetail.oc_id;
                        _OCDetail.OCCode = _OCDetail.oc_id.ToString();
                        _OCDetail.Command = command;
                        TempData["ModelData"] = _OCDetail;
                        TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                        return RedirectToAction("OCDetail", _OCDetail);


                    case "Refresh":
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Refresh";
                        //Session["Message"] = "";
                        //Session["AppStatus"] = "";
                        _OCDetail.hdnSavebtn = null;
                        _OCDetail.oc_name = null;
                        _OCDetail.tp_id = null;
                        _OCDetail.hdnoc_id = null;
                        _OCDetail.oc_id = 0;
                        _OCDetail.TaxTemplate = null;
                        _OCDetail.HSN_code = null;
                        _OCDetail.app_date = null;
                        //_OCDetail.oc_name = null;
                        _OCDetail.TransType = "Refresh";
                        _OCDetail.BtnName = "BtnRefresh";
                        //_TaxDetailModel = null;
                        TempData["ModelData"] = _OCDetail;
                        //_OCDetail = null;
                        TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                        return RedirectToAction("OCDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        TempData["ListFilterData"] = _OCDetail.ListFilterData1;
                        return RedirectToAction("OCList", "OCList");

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
        public ActionResult InsertOCDetail(OCDetailModel _OCDetail)
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

                DataTable OCDetail = new DataTable();
                DataTable OCBranch = new DataTable();

                DataTable OCDt = new DataTable();
                OCDt.Columns.Add("TransType", typeof(string));
                OCDt.Columns.Add("comp_id", typeof(int));
                OCDt.Columns.Add("oc_type", typeof(string));
                OCDt.Columns.Add("oc_id", typeof(int));
                OCDt.Columns.Add("oc_name", typeof(string));
                OCDt.Columns.Add("tp_id", typeof(int));
                OCDt.Columns.Add("acc_id", typeof(int));
                OCDt.Columns.Add("app_date", typeof(string));
                OCDt.Columns.Add("act_status", typeof(string));
                OCDt.Columns.Add("UserMacaddress", typeof(string));
                OCDt.Columns.Add("UserSystemName", typeof(string));
                OCDt.Columns.Add("UserIP", typeof(string));
                OCDt.Columns.Add("create_id", typeof(int));
                OCDt.Columns.Add("mod_id", typeof(int));
                OCDt.Columns.Add("tax_applicable", typeof(string));
                OCDt.Columns.Add("tax_tmplt", typeof(string));
                OCDt.Columns.Add("HSN_code", typeof(string));
              
                DataRow OCDtrow = OCDt.NewRow();

                //OCDtrow["TransType"] = Session["TransType"].ToString();
                OCDtrow["TransType"] = _OCDetail.TransType;
                OCDtrow["comp_id"] = comp_id;               
                OCDtrow["oc_type"] = _OCDetail.oc_type;
                //OCDtrow["oc_id"] = _OCDetail.oc_id;
                if (_OCDetail.hdnoc_id == null)
                {
                    OCDtrow["oc_id"] = "0";
                }
                else
                {
                    OCDtrow["oc_id"] = _OCDetail.hdnoc_id;
                }
                //OCDtrow["oc_id"] = _OCDetail.hdnoc_id;
                OCDtrow["oc_name"] = _OCDetail.oc_name;
                if (_OCDetail.tp_id == null)
                {
                    OCDtrow["tp_id"] = "0";
                }
                else
                {
                    OCDtrow["tp_id"] = _OCDetail.tp_id;
                }               
                //OCDtrow["acc_id"] = _OCDetail.acc_id;
                if (_OCDetail.acc_id == 0)
                {
                    OCDtrow["acc_id"] = _OCDetail.ddl_acc_id;
                }
                else
                {
                    OCDtrow["acc_id"] = _OCDetail.acc_id;
                }
                OCDtrow["app_date"] = _OCDetail.app_date;                
                if (_OCDetail.act_status)
                {
                    OCDtrow["act_status"] = "Y";
                }
                else
                {
                    OCDtrow["act_status"] = "N";
                }
                OCDtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                OCDtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                OCDtrow["UserIP"] = Session["UserIP"].ToString();              
                OCDtrow["create_id"] = Session["UserId"].ToString();
                OCDtrow["mod_id"] = Session["UserId"].ToString();
                if (_OCDetail.TaxApplicable)
                {
                    OCDtrow["tax_applicable"] = "Y";
                }
                else
                {
                    OCDtrow["tax_applicable"] = "N";
                }
                
                OCDtrow["tax_tmplt"] = _OCDetail.TaxTemplate;
                OCDtrow["HSN_code"] = _OCDetail.HSN_code;
               
                OCDt.Rows.Add(OCDtrow);

                OCDetail = OCDt;

                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("oc_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(_OCDetail.OCBranchDetail);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    dtrowBrdetails["oc_id"] = _OCDetail.oc_id;
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();

                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                OCBranch = dtBranch;

                String SaveMessage = _OCDetail_ISERVICES.insertOCDetail(OCDetail, OCBranch);
                string OCCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "DataNotFound")
                {
                    ViewBag.MenuPageName = getDocumentName();
                    _OCDetail.Title = title;
                    var a = OCCode.Split('-');
                    var msg = "Data Not found" +" "+ a[0].Trim()+" in " + _OCDetail.Title;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _OCDetail.Message = Message;
                    return RedirectToAction("OCDetail");
                }
                if (Message == "Update" || Message == "Save")
                {
                    //Session["Message"] = "Save";
                    //Session["OCCode"] = OCCode;
                    //Session["TransType"] = "Update";

                    _OCDetail.Message = "Save";
                    _OCDetail.OCCode = OCCode;
                    _OCDetail.TransType = "Update";
                }
                if (Message == "Duplicate")
                {
                    //Session["TransType"] = "Duplicate";
                    //Session["Message"] = "Duplicate";
                    //Session["OCCode"] = OCCode;

                    _OCDetail.TransType = "Duplicate";
                    _OCDetail.Message = "Duplicate";
                    _OCDetail.OCCode = OCCode;
                }
                //Session["BtnName"] = "BtnSave";
                _OCDetail.BtnName = "BtnSave";
                TempData["ModelData"] = _OCDetail;
                return RedirectToAction("OCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private DataTable GetOCBranchList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _OCDetail_ISERVICES.GetBrList(Comp_ID).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        [NonAction]
        private DataTable GetBrList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _OCDetail_ISERVICES.GetBrListDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        [NonAction]
        private DataTable GetCOAList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _OCDetail_ISERVICES.GetOCcoaDAL(Comp_ID);
                return dt;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
           
        }

        [NonAction]
        private DataTable GetTPCOAList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _OCDetail_ISERVICES.GetThirdPartyDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
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
    }

}
