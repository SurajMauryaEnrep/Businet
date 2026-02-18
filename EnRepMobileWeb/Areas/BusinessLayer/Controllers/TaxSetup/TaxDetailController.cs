using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.TaxDetail;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System.Web.UI.WebControls;
//***Modifyed by shubham maurya on 12-12-2022 12:10 remove all session and using Model to Transfer Data***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class TaxDetailController : Controller
    {
        string comp_id, userid, TaxId = string.Empty;
        TaxDetailModel _TaxDetailModel;
        string CompID, language = String.Empty;
        string DocumentMenuId = "103155",title;
        Common_IServices _Common_IServices;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        TaxDetail_ISERVICES _TaxDetail_ISERVICES;
        // GET: BusinessLayer/TaxDetailGL
        public TaxDetailController(Common_IServices _Common_IServices,TaxDetail_ISERVICES _TaxDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._TaxDetail_ISERVICES = _TaxDetail_ISERVICES;
        }
        public ActionResult TaxDetail(TaxDetailModel _TaxDetailModel1, string TaxCodeURL, string TransType, string BtnName, string command)
        {
            try
            {
                var _TaxDetailModel=TempData["ModelData"] as TaxDetailModel;
                if (_TaxDetailModel != null)
                {
                    ViewBag.MenuPageName = getDocumentName();
                    _TaxDetailModel.Title = title;
                    string Comp_ID = string.Empty;
                    _TaxDetailModel.tax_id = 0;
                    GetAllData(_TaxDetailModel);
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    if (TempData["ListFilterData"] != null)
                    {
                        _TaxDetailModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_TaxDetailModel.TransType == "Update" || _TaxDetailModel.Command == "Edit")
                    {
                        string TaxCode = _TaxDetailModel.TaxCode.ToString();

                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        Boolean act_status, recov, mancalc;
                        DataSet ds = _TaxDetail_ISERVICES.GetviewTaxdetailDAL(TaxCode, Comp_ID);
                        if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                        {

                        }
                        else
                        {
                            _TaxDetailModel.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        }
                        if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                        {
                            //act_status = true;
                            _TaxDetailModel.act_status = true;
                        }
                        else
                        {
                            // act_status = false;
                            _TaxDetailModel.act_status = false;
                        }
                        if (ds.Tables[0].Rows[0]["recov"].ToString() == "Y")
                            recov = true;
                        else
                            recov = false;
                        if (ds.Tables[0].Rows[0]["manual_calc"].ToString() == "Y")
                            mancalc = true;
                        else
                            mancalc = false;
                        _TaxDetailModel.tax_id = int.Parse(ds.Tables[0].Rows[0]["tax_id"].ToString());
                        _TaxDetailModel.tax_name = ds.Tables[0].Rows[0]["tax_name"].ToString();
                        _TaxDetailModel.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                        _TaxDetailModel.tax_perc = ds.Tables[0].Rows[0]["tax_perc"].ToString();
                        _TaxDetailModel.tax_auth_id = int.Parse(ds.Tables[0].Rows[0]["tax_auth_id"].ToString());
                        _TaxDetailModel.tax_type = ds.Tables[0].Rows[0]["tax_type"].ToString();
                        _TaxDetailModel.app_date = ds.Tables[0].Rows[0]["app_date"].ToString();
                        _TaxDetailModel.recov = recov;
                        _TaxDetailModel.manual_calc = mancalc;
                        _TaxDetailModel.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _TaxDetailModel.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _TaxDetailModel.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _TaxDetailModel.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        if (_TaxDetailModel.tax_type == "TDS")
                        {
                            _TaxDetailModel.SectionCode = ds.Tables[0].Rows[0]["sec_code"].ToString();
                        }
                        else
                        {
                            _TaxDetailModel.SectionCode = "";
                        }
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _TaxDetailModel.CustomerBranchList = ds.Tables[1];
                        DataTable dt2 = GetAccountGroup(_TaxDetailModel, "tax");
                        List<AccountGroup> _AccountGroupList1 = new List<AccountGroup>();
                        foreach (DataRow Row1 in dt2.Rows)
                        {
                            AccountGroup _AccountGroup = new AccountGroup();
                            _AccountGroup.acc_grp_id = Row1["acc_grp_id"].ToString();
                            _AccountGroup.AccGroupChildNood = Row1["AccGroupChildNood"].ToString();
                            _AccountGroupList1.Add(_AccountGroup);
                        }
                        _AccountGroupList1.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                        _TaxDetailModel.AccountGroupList = _AccountGroupList1;
                    }
                    else
                    {
                        _TaxDetailModel.act_status = true;
                    }
                    _TaxDetailModel.TransType = _TaxDetailModel.TransType;
                    _TaxDetailModel.Delete = null;
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/TaxSetup/TaxDetail.cshtml", _TaxDetailModel);
                }
                else
                {
                    ViewBag.MenuPageName = getDocumentName();                   
                    _TaxDetailModel1.Title = title;
                    string Comp_ID = string.Empty;
                    _TaxDetailModel1.tax_id = 0;
                    if (_TaxDetailModel1.TaxCode == 0)
                    {
                        _TaxDetailModel1.TaxCode =Convert.ToInt32(TaxCodeURL);
                    }
                    if (_TaxDetailModel1.TransType == null)
                    {
                        _TaxDetailModel1.TransType = TransType;
                    }
                    if (_TaxDetailModel1.BtnName == null)
                    {
                        _TaxDetailModel1.BtnName = BtnName;
                    }
                    if (_TaxDetailModel1.Command == null)
                    {
                        _TaxDetailModel1.Command = command;
                    }
                    if (_TaxDetailModel1.BtnName == null)
                    {
                        _TaxDetailModel1.BtnName = "BtnRefresh";
                    }

                    GetAllData(_TaxDetailModel1);

                    /****/
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    if (TempData["ListFilterData"] != null)
                    {
                        _TaxDetailModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_TaxDetailModel1.TransType == "Update" || _TaxDetailModel1.Command == "Edit")
                    {
                        string TaxCode = _TaxDetailModel1.TaxCode.ToString();
                        if (_TaxDetailModel1.TaxCode != 0)
                        {
                             TaxCode = _TaxDetailModel1.TaxCode.ToString();
                        }                        
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        Boolean act_status, recov, mancalc;
                        DataSet ds = _TaxDetail_ISERVICES.GetviewTaxdetailDAL(TaxCode, Comp_ID);
                        if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                        {

                        }
                        else
                        {
                            _TaxDetailModel1.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        }
                        if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                            act_status = true;
                        else
                            act_status = false;
                        if (ds.Tables[0].Rows[0]["recov"].ToString() == "Y")
                            recov = true;
                        else
                            recov = false;
                        if (ds.Tables[0].Rows[0]["manual_calc"].ToString() == "Y")
                            mancalc = true;
                        else
                            mancalc = false;
                        _TaxDetailModel1.tax_id = int.Parse(ds.Tables[0].Rows[0]["tax_id"].ToString());
                        _TaxDetailModel1.tax_name = ds.Tables[0].Rows[0]["tax_name"].ToString();
                        _TaxDetailModel1.acc_id = int.Parse(ds.Tables[0].Rows[0]["acc_id"].ToString());
                        _TaxDetailModel1.tax_perc = ds.Tables[0].Rows[0]["tax_perc"].ToString();
                        _TaxDetailModel1.tax_auth_id = int.Parse(ds.Tables[0].Rows[0]["tax_auth_id"].ToString());
                        _TaxDetailModel1.tax_type = ds.Tables[0].Rows[0]["tax_type"].ToString();
                        _TaxDetailModel1.app_date = ds.Tables[0].Rows[0]["app_date"].ToString();
                        _TaxDetailModel1.act_status = act_status;
                        _TaxDetailModel1.recov = recov;
                        _TaxDetailModel1.manual_calc = mancalc;
                        _TaxDetailModel1.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _TaxDetailModel1.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _TaxDetailModel1.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _TaxDetailModel1.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _TaxDetailModel1.CustomerBranchList = ds.Tables[1];
                        DataTable  dt2 = GetAccountGroup(_TaxDetailModel1, "tax");
                        List<AccountGroup> _AccountGroupList1 = new List<AccountGroup>();
                        foreach (DataRow Row1 in dt2.Rows)
                        {
                            AccountGroup _AccountGroup = new AccountGroup();
                            _AccountGroup.acc_grp_id = Row1["acc_grp_id"].ToString();
                            _AccountGroup.AccGroupChildNood = Row1["AccGroupChildNood"].ToString();
                            _AccountGroupList1.Add(_AccountGroup);
                        }
                        _AccountGroupList1.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                        _TaxDetailModel1.AccountGroupList = _AccountGroupList1;
                    }
                    else
                    {
                        _TaxDetailModel1.act_status = true;
                    }
                    _TaxDetailModel1.TransType = _TaxDetailModel1.TransType;
                    _TaxDetailModel1.Message = null;
                    _TaxDetailModel1.Delete = null;
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/TaxSetup/TaxDetail.cshtml", _TaxDetailModel1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private void GetAllData(TaxDetailModel _TaxDetailModel1)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }

         

            //  DataTable dt = _TaxDetail_ISERVICES.GetBrList(Comp_ID).Tables[0];
            DataSet Table = _TaxDetail_ISERVICES.GetAllData(Comp_ID);

           // dt = GetTaxBranchList();
            _TaxDetailModel1.CustomerBranchList = Table.Tables[0];

            DataTable dt1 = GetAccountGroup(_TaxDetailModel1, "tax");
            List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
            foreach (DataRow Row2 in dt1.Rows)
            {
                AccountGroup _AccountGroup = new AccountGroup();
                _AccountGroup.acc_grp_id = Row2["acc_grp_id"].ToString();
                _AccountGroup.AccGroupChildNood = Row2["AccGroupChildNood"].ToString();
                _AccountGroupList.Add(_AccountGroup);
            }
            _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
            _TaxDetailModel1.AccountGroupList = _AccountGroupList;

           // dt = GetCOAAuthorityList();
            List<AuthorityCOA> _AuthorityCOAList = new List<AuthorityCOA>();
            foreach (DataRow dt in Table.Tables[1].Rows)
            {
                AuthorityCOA _COAAuthorityList = new AuthorityCOA();
                _COAAuthorityList.acc_id = dt["acc_id"].ToString();
                _COAAuthorityList.acc_name = dt["acc_name"].ToString();
                _AuthorityCOAList.Add(_COAAuthorityList);
            }

            _AuthorityCOAList.Insert(0, new AuthorityCOA() { acc_id = "0", acc_name = "---Select---" });
            _TaxDetailModel1.COAAuthorityList = _AuthorityCOAList;
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
                throw ex;
            }


        }
        private void CommonPageDetails()
        {
            try
            {
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                string UserID = string.Empty;
                string language = string.Empty;
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
        public ActionResult TaxSave(TaxDetailModel _TaxDetailModel, string command, int tax_id)
        {
            try
            {
                if (_TaxDetailModel.Delete == "Delete")
                {
                    command = "Delete";
                    // DeletePQDetails(_Model);
                }
                switch (command)
                {
                    case "Edit":
                        _TaxDetailModel.hdnSavebtn = "";
                        _TaxDetailModel.Message = "";
                        _TaxDetailModel.Command = command;
                        _TaxDetailModel.TransType = "Update";
                        _TaxDetailModel.BtnName = "BtnEdit";
                        var TaxCodeURL = "";
                        if (tax_id != 0)
                        {
                            TaxCodeURL = tax_id.ToString();
                        }
                        else
                        {
                            TaxCodeURL = _TaxDetailModel.tax_id.ToString();
                        }
                       var TransType= "Update";
                        var BtnName= "BtnEdit";
                        TempData["ModelData"] = _TaxDetailModel;
                        TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                        return ( RedirectToAction("TaxDetail", "TaxDetail", new { TaxCodeURL = TaxCodeURL, TransType, BtnName, command }));

                    case "Add":
                        TaxDetailModel _TaxDetailModelAdd = new TaxDetailModel();
                        //_TaxDetailModel.Message = "";
                        _TaxDetailModel.hdnSavebtn = "";
                        _TaxDetailModelAdd.Command = command;
                        _TaxDetailModelAdd.TaxCode = 0;
                        _TaxDetailModelAdd.AppStatus= "D";
                        _TaxDetailModelAdd.TransType = "Save";
                        _TaxDetailModelAdd.BtnName = "BtnAddNew";
                        _TaxDetailModelAdd.tax_id = 0;
                        TempData["ModelData"] = _TaxDetailModelAdd;
                        TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                        //return RedirectToAction("TaxDetail", "TaxDetail", _TaxDetailModel);
                        return RedirectToAction("TaxDetail", "TaxDetail");

                    case "Delete":
                        tax_id = _TaxDetailModel.tax_id;
                        _TaxDetailModel.Command = command;
                        Deletetax(_TaxDetailModel);
                        if (_TaxDetailModel.Message == "Dependency Exist")
                        {
                            _TaxDetailModel.tax_id = tax_id;
                            _TaxDetailModel.hdnSavebtn = "";
                            _TaxDetailModel.Command = "Update";
                            _TaxDetailModel.Message = "Dependency Exist";
                            _TaxDetailModel.TransType = "Update";
                            _TaxDetailModel.BtnName = "BtnRefresh";
                            TempData["ModelData"] = _TaxDetailModel;
                        }
                        else
                        {
                            _TaxDetailModel.Message = "Deleted";
                            _TaxDetailModel.hdnSavebtn = "";
                            _TaxDetailModel.Command = "Refresh";
                            _TaxDetailModel.TransType = "Refresh";
                            _TaxDetailModel.BtnName = "Delete";

                            TempData["ModelData"] = _TaxDetailModel;
                            TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                        }                     
                        //_TaxDetailModel.hdnSavebtn = "";
                        //TempData["ModelData"] = _TaxDetailModel;
                        TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                        return RedirectToAction("TaxDetail");

                    case "Save":
                        _TaxDetailModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            InsertTaxDetail(_TaxDetailModel);
                            _TaxDetailModel.TaxCode = _TaxDetailModel.TaxCode;
                            if (_TaxDetailModel.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            if (_TaxDetailModel.Message == "Duplicate" || _TaxDetailModel.Message == "DuplicateGL")
                            {
                                dt = GetTaxBranchList();
                                CommonPageDetails();
                                _TaxDetailModel.CustomerBranchList = dt;
                                if (_TaxDetailModel.TaxCode != 0)
                                {
                                    string Comp_ID = "";
                                    string TaxCode = _TaxDetailModel.TaxCode.ToString();
                                    if (Session["CompId"] != null)
                                    {
                                        Comp_ID = Session["CompId"].ToString();
                                    }
                                    DataSet ds = _TaxDetail_ISERVICES.GetviewTaxdetailDAL(TaxCode, Comp_ID);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        _TaxDetailModel.createby = ds.Tables[0].Rows[0]["create_id"].ToString();
                                        _TaxDetailModel.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                        _TaxDetailModel.modby = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                        _TaxDetailModel.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                        if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                                        {

                                        }
                                        else
                                        {
                                            _TaxDetailModel.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                                        }
                                    }
                         
                                    
                                    _TaxDetailModel.CustomerBranchList = ds.Tables[1];
                                }
                                dt = GetAccountGroup(_TaxDetailModel, "tax");
                                List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroup = new AccountGroup();
                                    _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                                    _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                                    _AccountGroupList.Add(_AccountGroup);
                                }
                                _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                                _TaxDetailModel.AccountGroupList = _AccountGroupList;
                                dt = GetCOAAuthorityList();
                                List<AuthorityCOA> _AuthorityCOAList = new List<AuthorityCOA>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AuthorityCOA _COAAuthorityList = new AuthorityCOA();
                                    _COAAuthorityList.acc_id = dt["acc_id"].ToString();
                                    _COAAuthorityList.acc_name = dt["acc_name"].ToString();
                                    _AuthorityCOAList.Add(_COAAuthorityList);
                                }
                                _TaxDetailModel.COAAuthorityList = _AuthorityCOAList;
                                _TaxDetailModel.TaxCode = 0;
                                _TaxDetailModel.AppStatus = "D";
                                _TaxDetailModel.TransType = "Save";
                                _TaxDetailModel.BtnName = "BtnAddNew";
                                _TaxDetailModel.Message = _TaxDetailModel.Message;
                                if (_TaxDetailModel.act_status == false)
                                {
                                    _TaxDetailModel.act_status = true;
                                }
                                _TaxDetailModel.hdnSavebtn = "";
                                TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                                return View("~/Areas/BusinessLayer/Views/TaxSetup/TaxDetail.cshtml", _TaxDetailModel);
                            }
                            else
                            {
                                _TaxDetailModel.hdnSavebtn = "";
                                _TaxDetailModel.BtnName = "BtnSave";
                                TaxCodeURL = _TaxDetailModel.TaxCode.ToString();
                                TransType = _TaxDetailModel.TransType;
                                BtnName = _TaxDetailModel.BtnName;
                                TempData["ModelData"] = _TaxDetailModel;
                                TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                                return ( RedirectToAction("TaxDetail", new { TaxCodeURL = TaxCodeURL, TransType, BtnName, command }));
                            }
                        }
                        else
                        {
                            _TaxDetailModel.hdnSavebtn = "";
                            TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                            _TaxDetailModel = null;
                            return View("~/Areas/BusinessLayer/Views/TaxSetup/TaxDetail.cshtml", _TaxDetailModel);
                        }

                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        _TaxDetailModel.Command= command;
                        tax_id = _TaxDetailModel.tax_id;
                        _TaxDetailModel.TaxCode = tax_id;
                        _TaxDetailModel.hdnSavebtn = "";
                        TempData["ModelData"] = _TaxDetailModel;
                        TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                        return RedirectToAction("TaxDetail");


                    case "Refresh":
                        TaxDetailModel _TaxDetailModelRefresh = new TaxDetailModel();
                        //_TaxDetailModel.tax_type = null;
                        //_TaxDetailModel.tax_perc = null;
                        //_TaxDetailModel.tax_name = null;
                        //_TaxDetailModel.tax_id = 0;
                        //_TaxDetailModel.tax_auth_id = 0;
                        //_TaxDetailModel.acc_id = 0;
                        //_TaxDetailModel.app_date = null;
                        //_TaxDetailModel.Message = "";
                        _TaxDetailModel.hdnSavebtn = "";
                        _TaxDetailModelRefresh.Command = command;
                        //_TaxDetailModel.AppStatus = "";
                        _TaxDetailModelRefresh.TransType = "Refresh";
                        _TaxDetailModelRefresh.BtnName = "BtnRefresh";
                        //_TaxDetailModel = null;
                        TempData["ModelData"] = _TaxDetailModelRefresh;
                        TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                        return RedirectToAction("TaxDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
                        return RedirectToAction("TaxList", "TaxList");

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

        private ActionResult Deletetax(TaxDetailModel _TaxDetailModel)
        {
            string Comp_ID = string.Empty;
            string BrchID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            int tax_id = _TaxDetailModel.tax_id;
            DataTable Delete = _TaxDetail_ISERVICES.deletetaxdata(Comp_ID, BrchID, tax_id);

            string Message = Delete.Rows[0]["dependcy"].ToString();// .Rows["dependcy"].ToString();
            if(Message== "Dependency Exist")
            {
                _TaxDetailModel.tax_id = tax_id;
                _TaxDetailModel.Command = "Update";
                _TaxDetailModel.Message = "Dependency Exist";
                _TaxDetailModel.TransType = "Update";
                _TaxDetailModel.BtnName = "BtnRefresh";
                TempData["ModelData"] = _TaxDetailModel;
            }
            else
            {
                _TaxDetailModel.Message = "Delete";
                _TaxDetailModel.hdnSavebtn = "";
                _TaxDetailModel.Command = "Refresh";
                _TaxDetailModel.TransType = "Refresh";
                _TaxDetailModel.BtnName = "BtnRefresh";
              
                TempData["ModelData"] = _TaxDetailModel;
                TempData["ListFilterData"] = _TaxDetailModel.ListFilterData1;
            }
            return RedirectToAction("TaxDetail");
        }

        [NonAction]
        private DataTable GetAccountGroup(TaxDetailModel _TaxDetailModel, string type)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                var acc_groupid = "";
                if (_TaxDetailModel.acc_id != 0)
                {
                    var acc_id = _TaxDetailModel.acc_id.ToString();
                    var a = acc_id.Split('0');

                    var groupid_sign = a[0].Trim();
                    if (groupid_sign == "1")
                    {
                        acc_groupid = "101";
                    }

                    else if (groupid_sign == "2")
                    {
                        acc_groupid = "201";
                    }
                    else if (groupid_sign == "3")
                    {
                        acc_groupid = "301";
                    }
                    else if (groupid_sign == "4")
                    {
                        acc_groupid = "401";
                    }
                }
                else
                {
                    acc_groupid = null;
                }
                DataTable dt1;
                DataTable dt2;
                if (_TaxDetailModel.Command == "Edit" && _TaxDetailModel.TransType == "Update" && _TaxDetailModel.acc_id != 0)
                {
                    dt2 = _TaxDetail_ISERVICES.GetAccountGroupList(Comp_ID, acc_groupid);
                    return dt2;
                }
                else
                {
                     dt1 = _TaxDetail_ISERVICES.GetAccountGroupDAL(Comp_ID, type);
                    return dt1;
                }
               
              //  return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult InsertTaxDetail(TaxDetailModel _TaxDetailModel)
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

                DataTable TaxDetail = new DataTable();
                DataTable TaxBranch = new DataTable();

                DataTable TaxDt = new DataTable();
                TaxDt.Columns.Add("TransType", typeof(string));
                TaxDt.Columns.Add("comp_id", typeof(int));               
                TaxDt.Columns.Add("tax_id", typeof(int));
                TaxDt.Columns.Add("tax_name", typeof(string));           
                TaxDt.Columns.Add("acc_id", typeof(int));
                TaxDt.Columns.Add("tax_perc", typeof(string));
                TaxDt.Columns.Add("recov", typeof(string));
                TaxDt.Columns.Add("app_date", typeof(string));
                TaxDt.Columns.Add("act_status", typeof(string));
                TaxDt.Columns.Add("UserMacaddress", typeof(string));
                TaxDt.Columns.Add("UserSystemName", typeof(string));
                TaxDt.Columns.Add("UserIP", typeof(string));
                TaxDt.Columns.Add("create_id", typeof(int));
                TaxDt.Columns.Add("mod_id", typeof(int));
                TaxDt.Columns.Add("tax_type", typeof(string));
                TaxDt.Columns.Add("manual_calc", typeof(string));
                TaxDt.Columns.Add("tax_auth_id", typeof(int));
                TaxDt.Columns.Add("acc_grp_id", typeof(int));
                TaxDt.Columns.Add("section_code", typeof(string));

                DataRow TaxDtrow = TaxDt.NewRow();

                TaxDtrow["TransType"] = _TaxDetailModel.TransType;
                TaxDtrow["comp_id"] = comp_id;
                TaxDtrow["tax_id"] = _TaxDetailModel.TaxCode;
                TaxDtrow["tax_name"] = _TaxDetailModel.tax_name;
                TaxDtrow["acc_id"] = _TaxDetailModel.acc_id;             
                TaxDtrow["tax_perc"] = _TaxDetailModel.tax_perc;
                if (_TaxDetailModel.recov)
                {
                    TaxDtrow["recov"] = "Y";
                }
                else
                {
                    TaxDtrow["recov"] = "N";
                }
                TaxDtrow["app_date"] = _TaxDetailModel.app_date;
                if (_TaxDetailModel.TransType == "Save")
                {
                    TaxDtrow["act_status"] = "Y";
                }
                else
                {
                    if (_TaxDetailModel.act_status)
                    {
                        TaxDtrow["act_status"] = "Y";
                    }
                    else
                    {
                        TaxDtrow["act_status"] = "N";
                    }
                }             
                TaxDtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                TaxDtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                TaxDtrow["UserIP"] = Session["UserIP"].ToString();
                TaxDtrow["create_id"] = Session["UserId"].ToString();
                TaxDtrow["mod_id"] = Session["UserId"].ToString();
                TaxDtrow["tax_type"] = _TaxDetailModel.tax_type;
                if (_TaxDetailModel.manual_calc)
                {
                    TaxDtrow["manual_calc"] = "Y";
                }
                else
                {
                    TaxDtrow["manual_calc"] = "N";
                }
                if (_TaxDetailModel.tax_auth_id == 0)
                {
                    TaxDtrow["tax_auth_id"] = _TaxDetailModel.ddl_tax_auth_id;
                }
                else
                {
                    TaxDtrow["tax_auth_id"] = _TaxDetailModel.tax_auth_id;
                }
                TaxDtrow["acc_grp_id"] = _TaxDetailModel.acc_grp_id;
                if (_TaxDetailModel.tax_type== "TDS")
                {
                    TaxDtrow["section_code"] = _TaxDetailModel.SectionCode;
                }
                else
                {
                    TaxDtrow["section_code"] = "";
                }
                
                
                TaxDt.Rows.Add(TaxDtrow);

                TaxDetail = TaxDt;

                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));               
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("tax_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(_TaxDetailModel.TaxBranchDetail);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();                    
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["tax_id"] = _TaxDetailModel.tax_id;
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();

                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                TaxBranch = dtBranch;

                String SaveMessage = _TaxDetail_ISERVICES.insertTaxDetail(TaxDetail, TaxBranch);

                string TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "DataNotFound")
                {
                    var a = TaxCode.Split('-');
                    var msg = "Data Not found" + a[0].Trim();
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _TaxDetailModel.hdnSavebtn = "";
                    _TaxDetailModel.Message = Message;
                    return RedirectToAction("TaxDetail");
                }
                if (Message == "Update" || Message == "Save")
                {
                    _TaxDetailModel.Message = "Save";
                    _TaxDetailModel.TaxCode =Convert.ToInt32(TaxCode);
                    _TaxDetailModel.TransType = "Update";
                }
                if (Message == "Duplicate")
                {
                    _TaxDetailModel.hdnSavebtn = "";
                    _TaxDetailModel.TransType = "Duplicate";
                    _TaxDetailModel.Message = "Duplicate";
                    _TaxDetailModel.TaxCode = Convert.ToInt32(TaxCode);
                }
                if (Message == "DuplicateGL")
                {
                    _TaxDetailModel.hdnSavebtn = "";
                    _TaxDetailModel.TransType = "Duplicate";
                    _TaxDetailModel.Message = "DuplicateGL";
                    _TaxDetailModel.TaxCode = Convert.ToInt32(TaxCode);
                }
                _TaxDetailModel.BtnName= "BtnSave";
                TempData["ModelData"] = _TaxDetailModel;
                return RedirectToAction("TaxDetail");
            }
            catch (Exception ex)
            {
                _TaxDetailModel.hdnSavebtn = "";
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataTable GetTaxBranchList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _TaxDetail_ISERVICES.GetBrList(Comp_ID).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
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

                DataTable dt = _TaxDetail_ISERVICES.GetBrListDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        //[NonAction]
        //private DataTable GetCOAList()
        //{
        //    string Comp_ID = string.Empty;
        //    string BrchID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }           
        //    DataTable dt = _TaxDetail_ISERVICES.GetTaxcoaDAL(Comp_ID);
        //    return dt;
        //}

        [NonAction]
        private DataTable GetCOAAuthorityList()
        {
            try
            {
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _TaxDetail_ISERVICES.GetTaxAuthrityCoaDAL(Comp_ID);
                return dt;
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