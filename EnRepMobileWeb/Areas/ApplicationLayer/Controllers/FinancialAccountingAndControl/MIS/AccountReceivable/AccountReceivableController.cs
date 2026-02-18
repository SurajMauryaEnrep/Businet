using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountReceivable;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.AccountReceivable;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using System.Configuration;
using ZXing;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.AccountReceivable
{
    public class AccountReceivableController : Controller
    {
        string CompID, language = String.Empty;
        string Comp_ID, Br_ID, Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104135115";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AccountReceivable_ISERVICE _AccountReceivable_ISERVICE;
        public AccountReceivableController(Common_IServices _Common_IServices, AccountReceivable_ISERVICE _AccountReceivable_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._AccountReceivable_ISERVICE = _AccountReceivable_ISERVICE;
        }
        // GET: ApplicationLayer/AccountReceivable
        public ActionResult AccountReceivable(AccountReceivableModel _AccRecModel, string cmd, string TYP, string BTN)
        {
            try
            {
                //AccountReceivableModel _AccRecModel = new AccountReceivableModel();
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                    ViewBag.vb_br_id = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/

                DataTable br_list = new DataTable();
                br_list = _Common_IServices.Cmn_GetBrList(Comp_ID, UserID);
                ViewBag.br_list = br_list;

                var _AccRecModel1 = TempData["ModelData"] as AccountReceivableModel;
                if (_AccRecModel1 != null)
                {
                   _AccRecModel1.categoryLists = custCategoryList();
                    _AccRecModel1.portFolioLists = custPortFolioLists();
                    _AccRecModel1.regionLists = regionLists();
                    GetCustomerDropdown(_AccRecModel1);
                    _AccRecModel1.Title = title;
                    
                    _AccRecModel1.GstApplicable = ViewBag.GstApplicable;
                    DataSet ds = _AccountReceivable_ISERVICE.GetUserRangeDetail(Comp_ID, UserID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _AccRecModel1.Range1 = ds.Tables[0].Rows[0]["range_1"].ToString();
                        _AccRecModel1.Range2 = ds.Tables[0].Rows[0]["range_2"].ToString();
                        _AccRecModel1.Range3 = ds.Tables[0].Rows[0]["range_3"].ToString();
                        _AccRecModel1.Range4 = ds.Tables[0].Rows[0]["range_4"].ToString();
                        _AccRecModel1.Range5 = ds.Tables[0].Rows[0]["range_5"].ToString();
                    }
                    //if (Session["ARAging_basis"] != null)
                    if (_AccRecModel1.ARAging_basis != null)
                    {
                        //string basis = Session["ARAging_basis"].ToString();
                        string basis = _AccRecModel1.ARAging_basis;

                        _AccRecModel1.Basis = basis;

                        _AccRecModel1.AccountRecivableList = GetAccountReceivable_Detail("0", "0", "0", "0", basis, DateTime.Now.ToString("yyyy-MM-dd"),"A","S","0", "0", "0", "0", "0");
                    }
                    if (_AccRecModel1.ARReceivableType != null)
                    {
                        string ReceivableType = _AccRecModel1.ARReceivableType;
                        string basis = _AccRecModel1.Basis;
                        _AccRecModel1.Basis = basis;
                        _AccRecModel1.AccountRecivableList = GetAccountReceivable_Detail("0", "0", "0", "0", basis, DateTime.Now.ToString("yyyy-MM-dd"), ReceivableType,"S","0", "0", "0", "0", "0");
                    }
                    _AccRecModel.To_dt = DateTime.Now.ToString("yyyy-MM-dd");
                    //Session["ARAging"] = "0";
                    //Session["ARAging_basis"] = null;
                    _AccRecModel1.ARAging_basis = null;
                    _AccRecModel1.ARAging = "0";
                    _AccRecModel1.SalesPersons = GetSalesPersonList();
                    return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountReceivable/AccountReceivable.cshtml", _AccRecModel1);
                }
                else
                {
                    _AccRecModel.categoryLists = custCategoryList();
                    _AccRecModel.portFolioLists = custPortFolioLists();
                    _AccRecModel.regionLists = regionLists();
                    GetCustomerDropdown(_AccRecModel);
                    _AccRecModel.Title = title;
                    _AccRecModel.Command = cmd;
                    _AccRecModel.TransType = TYP;
                    _AccRecModel.BtnName = BTN;
                    _AccRecModel.GstApplicable = ViewBag.GstApplicable;
                    DataSet ds = _AccountReceivable_ISERVICE.GetUserRangeDetail(Comp_ID, UserID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _AccRecModel.Range1 = ds.Tables[0].Rows[0]["range_1"].ToString();
                        _AccRecModel.Range2 = ds.Tables[0].Rows[0]["range_2"].ToString();
                        _AccRecModel.Range3 = ds.Tables[0].Rows[0]["range_3"].ToString();
                        _AccRecModel.Range4 = ds.Tables[0].Rows[0]["range_4"].ToString();
                        _AccRecModel.Range5 = ds.Tables[0].Rows[0]["range_5"].ToString();
                    }
                    //if (Session["ARAging_basis"] != null)
                    if (_AccRecModel.ARAging_basis != null)
                    {
                        string basis = _AccRecModel.ARAging_basis;
                        _AccRecModel.Basis = basis;
                        _AccRecModel.AccountRecivableList = GetAccountReceivable_Detail("0", "0", "0", "0", basis, DateTime.Now.ToString("yyyy-MM-dd"),"A","S","0","0","0","0","0");
                    }
                    else if (_AccRecModel.ARReceivableType != null)
                    {
                        string ReceivableType = _AccRecModel.ARReceivableType;
                        string basis = _AccRecModel.Basis;
                        _AccRecModel.Basis = basis;
                        _AccRecModel.AccountRecivableList = GetAccountReceivable_Detail("0", "0", "0", "0", basis, DateTime.Now.ToString("yyyy-MM-dd"), ReceivableType,"S","0", "0", "0", "0", "0");
                    }
                    else
                    {
                        _AccRecModel.Basis = "I";
                        _AccRecModel.ReceivableType = "A";
                        _AccRecModel.AccountRecivableList = GetAccountReceivable_Detail("0", "0", "0", "0", "I", DateTime.Now.ToString("yyyy-MM-dd"), "A","S","0", "0", "0", "0", "0");
                    }
                    _AccRecModel.To_dt = DateTime.Now.ToString("yyyy-MM-dd");
                    //Session["ARAging"] = "0";
                    //Session["ARAging_basis"] = null;
                    _AccRecModel.ARAging_basis = null;
                    _AccRecModel.ARAging = "0";
                    _AccRecModel.SalesPersons = GetSalesPersonList();
                    return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountReceivable/AccountReceivable.cshtml", _AccRecModel);
                }
                

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ShowReportFromDashBoard(string ToDt)
        {
            AccountReceivableModel _AccRecModel = new AccountReceivableModel();
            CommonPageDetails();
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                Language = Session["Language"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
                ViewBag.vb_br_id = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }

            ViewBag.DocumentMenuId = DocumentMenuId;

            DataTable br_list = new DataTable();
            br_list = _Common_IServices.Cmn_GetBrList(Comp_ID, UserID);
            ViewBag.br_list = br_list;

            _AccRecModel.categoryLists = custCategoryList();
            _AccRecModel.portFolioLists = custPortFolioLists();
            _AccRecModel.regionLists = regionLists();
            GetCustomerDropdown(_AccRecModel);
            _AccRecModel.Title = title;
            _AccRecModel.Command = null;
            _AccRecModel.TransType = null;
            _AccRecModel.BtnName = null;
            _AccRecModel.GstApplicable = ViewBag.GstApplicable;
            DataSet ds = _AccountReceivable_ISERVICE.GetUserRangeDetail(Comp_ID, UserID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                _AccRecModel.Range1 = ds.Tables[0].Rows[0]["range_1"].ToString();
                _AccRecModel.Range2 = ds.Tables[0].Rows[0]["range_2"].ToString();
                _AccRecModel.Range3 = ds.Tables[0].Rows[0]["range_3"].ToString();
                _AccRecModel.Range4 = ds.Tables[0].Rows[0]["range_4"].ToString();
                _AccRecModel.Range5 = ds.Tables[0].Rows[0]["range_5"].ToString();
            }

            _AccRecModel.Basis = "I";
            _AccRecModel.ReceivableType = "A";
            _AccRecModel.AccountRecivableList = GetAccountReceivable_Detail("0", "0", "0", "0", "I", ToDt, "A", "S", "0", "0", "0", "0", "0");

            _AccRecModel.ARAging_basis = null;
            _AccRecModel.ARAging = "0";
            _AccRecModel.To_dt = ToDt;
            _AccRecModel.SalesPersons = GetSalesPersonList();

            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountReceivable/AccountReceivable.cshtml", _AccRecModel);
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];
                ViewBag.GstApplicableForExport = ds.Tables[5].Rows.Count > 0 ? ds.Tables[5].Rows[0]["param_stat"].ToString() : "";
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                ViewBag.ExpComrcPrintOptions = ds.Tables[9].Rows.Count > 0 ? ds.Tables[9].Rows[0]["param_stat"].ToString() : "";

                #region Commented By Nitesh 24-05-2024 for Gst Applicable And Other Parameter table Name And Param_id is changed
                #endregion
                //foreach (DataRow Row in ds.Tables[1].Rows)
                //{
                //    if (Row["param_id"].ToString() == "101")
                //    {
                //        ViewBag.GstApplicable = Row["param_stat"].ToString();
                //    }
                //    else if (Row["param_id"].ToString() == "107")
                //    {
                //        ViewBag.ExpComrcPrintOptions = Row["param_stat"].ToString();
                //    }
                //}
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
        private List<RegionList> regionLists()
        {
            List<RegionList> regionLists = new List<RegionList>();
            DataTable dt = GetRegion();
            foreach (DataRow dr in dt.Rows)
            {
                RegionList list = new RegionList();
                list.region_id = dr["setup_id"].ToString();
                list.region_val = dr["setup_val"].ToString();
                regionLists.Add(list);
            }
           // regionLists.Insert(0, new RegionList() { region_id = "0", region_val = "---All---" });
            return regionLists;
        }
        private List<CustCategoryList> custCategoryList()
        {
            List<CustCategoryList> lists = new List<CustCategoryList>();
            DataTable dt = GetCustomerCategory();
            foreach (DataRow dr in dt.Rows)
            {
                CustCategoryList list = new CustCategoryList();
                list.Cat_id = dr["setup_id"].ToString();
                list.Cat_val = dr["setup_val"].ToString();
                lists.Add(list);
            }
            //lists.Insert(0, new CustCategoryList() { Cat_id = "0", Cat_val = "---All---" });
            return lists;
        }
        private List<CustPortFolioList> custPortFolioLists()
        {
            List<CustPortFolioList> portFolioLists = new List<CustPortFolioList>();
            DataTable dt1 = GetCustomerPortfolio();
            foreach (DataRow dr in dt1.Rows)
            {
                CustPortFolioList custPortFolio = new CustPortFolioList();
                custPortFolio.CatPort_id = dr["setup_id"].ToString();
                custPortFolio.CatPort_val = dr["setup_val"].ToString();
                portFolioLists.Add(custPortFolio);
            }
           // portFolioLists.Insert(0, new CustPortFolioList() { CatPort_id = "0", CatPort_val = "---All---" });
            return portFolioLists;
        }
        public DataTable GetRegion()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _AccountReceivable_ISERVICE.GetRegionDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetCustomerCategory()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _AccountReceivable_ISERVICE.GetcategoryDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetCustomerPortfolio()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _AccountReceivable_ISERVICE.GetCustportDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private void GetCustomerDropdown(AccountReceivableModel _AccRecModel)
        {
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            DataSet datalist = _AccountReceivable_ISERVICE.GetCustomerDropdowns(Comp_ID,"0","0");

            List<customerZoneList> _SuppList = new List<customerZoneList>();
            foreach (DataRow data in datalist.Tables[0].Rows)
            {
                customerZoneList _SuppDetail = new customerZoneList();
                _SuppDetail.cust_zone_id = data["setup_id"].ToString();
                _SuppDetail.cust_zone_name = data["setup_val"].ToString();
                _SuppList.Add(_SuppDetail);
            }
            _AccRecModel.customerZoneLists = _SuppList;

            List<CustomerGroupList> _CustomerGroupList = new List<CustomerGroupList>();
            foreach (DataRow data in datalist.Tables[1].Rows)
            {
                CustomerGroupList _SuppDetail = new CustomerGroupList();
                _SuppDetail.cust_grp_id = data["setup_id"].ToString();
                _SuppDetail.cust_grp_name = data["setup_val"].ToString();
                _CustomerGroupList.Add(_SuppDetail);
            }
            _AccRecModel.CustomerGroupLists = _CustomerGroupList;

            List<CityList> _CityList = new List<CityList>();
            _AccRecModel.CityLists = _CityList;

            List<StateList> _StateList = new List<StateList>();
            _AccRecModel.StateLists = _StateList;
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UserRangeSave(AccountReceivableModel _AccRecModel, string command)
        {
            try
            {
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (_AccRecModel.hdnPDFPrint == "Print")
                {
                    command = "Print";
                }
                if (_AccRecModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                } 
                if (_AccRecModel.hdnCSVInsight == "CSVInsight")
                {
                    command = "CSVInsight";
                } 
                if (_AccRecModel.hdnPaidAmtCSVInsight == "PaidAmtCSVInsight")
                {
                    command = "PaidAmtCSVInsight";
                } 
                if (_AccRecModel.hdnAdvAmtCSVInsight == "AdvAmtCSVInsight")
                {
                    command = "AdvAmtCSVInsight";
                }
                switch (command)
                {

                    case "Save":
                        // Session["Command"] = command;
                        _AccRecModel.Command = command;
                        SaveUserRange(_AccRecModel);
                        TempData["ModelData"] = _AccRecModel;
                        var cmd = _AccRecModel.Command;
                        var TYP = _AccRecModel.TransType;
                        var BTN = _AccRecModel.BtnName;
                        Session["UserID"] = UserID;
                        return RedirectToAction("AccountReceivable", new { cmd, TYP, BTN });
                    case "Print":
                        return GenratePdfFile(_AccRecModel);
                    case "CsvPrint":
                        return ExportTrialBalanceData(_AccRecModel, command);
                    case "CSVInsight":
                        return ExportTrialBalanceData(_AccRecModel, command);
                    case "PaidAmtCSVInsight":
                        return ExportTrialBalanceData(_AccRecModel, command);
                    case "AdvAmtCSVInsight":
                        return ExportTrialBalanceData(_AccRecModel, command);
                    default:
                        return new EmptyResult();
                        //return new EmptyResult();

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult SaveUserRange(AccountReceivableModel _AccRecModel)
        {
            string SaveMessage = "";
            CommonPageDetails();
            //getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }

                string user_id = Session["UserId"].ToString();
                string range1 = _AccRecModel.Range1;
                string range2 = _AccRecModel.Range2;
                string range3 = _AccRecModel.Range3;
                string range4 = _AccRecModel.Range4;
                string range5 = _AccRecModel.Range5;

                //dtheader.Rows.Add(dtrowHeader);
                //    Header = dtheader;

                SaveMessage = _AccountReceivable_ISERVICE.InsertUserRangeDetail(CompID, user_id, range1, range2, range3, range4, range5);

                //string BankPaymentNo = SaveMessage.Split(',')[1].Trim();
                //string BP_Number = BankPaymentNo.Replace("/", "");
                //string Message = SaveMessage.Split(',')[0].Trim();
                //string BankPaymentDate = SaveMessage.Split(',')[2].Trim();


                //Session["Message"] = "Save";
                //Session["Command"] = "Update";
                //Session["UserID"] = UserID;                  
                //Session["TransType"] = "Update";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnToDetailPage";
                _AccRecModel.Message = "Save";
                _AccRecModel.Command = "Update";
                _AccRecModel.TransType = "Update";
                _AccRecModel.AppStatus = "D";
                _AccRecModel.BtnName = "BtnToDetailPage";
                TempData["ModelData"] = _AccRecModel;
                Session["UserID"] = UserID;
                return RedirectToAction("AccountReceivable");



            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
        [HttpPost]
        public ActionResult SearchAccountReceivableDetail(string Cust_id, string Cat_id, string Prf_id, string Reg_id, string Basis, string AsDate,string ReceivableType,string ReportType, string brlist,string sales_per,string customerZone,string CustomerGroup,string state_id,string city_id)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<ARList> _ARListDetail = new List<ARList>();
                AccountReceivableModel _AccountRecModel = new AccountReceivableModel();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                //DataTable dt = new DataTable();/*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/ 
                DataSet ds = new DataSet();
                if(Cust_id == "")
                {
                    Cust_id = "0";
                }
                if(Cat_id == "")
                {
                    Cat_id = "0";
                }
                if(Prf_id == "")
                {
                    Prf_id = "0";
                }
                if(Reg_id == "")
                {
                    Reg_id = "0";
                }
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
                // Session["ARAging"] = "Search";
                ds = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, Cust_id, Cat_id, Prf_id, Reg_id, Basis, AsDate,0,"List",0, ReceivableType, ReportType, brlist, sales_per, customerZone,CustomerGroup,state_id,city_id);
                _AccountRecModel.ARAging = "Search";
                ViewBag.flag = ReportType;
                //if (dt.Rows.Count > 0)/*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/ 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //foreach (DataRow dr in dt.Rows)/*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/ 
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ARList _ARList = new ARList();
                        _ARList.CustName = dr["custname"].ToString();
                        _ARList.CustId = dr["custid"].ToString();
                        _ARList.Curr = dr["curr"].ToString();
                        _ARList.CurrId = dr["curr_id"].ToString();/*add by Hina sharma on 17-12-2024 for filter*/
                        //_ARList.AmtRange1 = Convert.ToDecimal(dr["r1"]).ToString(ValDigit);
                        //_ARList.AmtRange2 = Convert.ToDecimal(dr["r2"]).ToString(ValDigit);
                        //_ARList.AmtRange3 = Convert.ToDecimal(dr["r3"]).ToString(ValDigit);
                        //_ARList.AmtRange4 = Convert.ToDecimal(dr["r4"]).ToString(ValDigit);
                        //_ARList.AmtRange5 = Convert.ToDecimal(dr["r5"]).ToString(ValDigit);
                        //_ARList.AmtRange6 = Convert.ToDecimal(dr["gtr5"]).ToString(ValDigit);
                        //_ARList.TotalAmt = Convert.ToDecimal(dr["tamt"]).ToString(ValDigit);
                        if (ReportType == "D")
                        {
                            _ARList.Inv_no = dr["inv_no"].ToString();
                            _ARList.Inv_dt = dr["inv_dt"].ToString();
                            _ARList.Invoice_Amt = dr["inv_amt"].ToString();
                            _ARList.salesPerson = dr["salesPerson"].ToString();
                        }
                        if (ReportType == "S")
                        {
                            _ARList.totamt_sp = dr["tot_amt"].ToString();
                            _ARList.totamt_bs = dr["tot_amt_bs"].ToString();
                            _ARList.advamt_bs = dr["Adv_Amt_bs"].ToString();
                            _ARList.totnetamt_bs = dr["tamt_bs"].ToString();
                        }
                           _ARList.AmtRange1 = dr["r1"].ToString();
                        _ARList.AmtRange2 = dr["r2"].ToString();
                        _ARList.AmtRange3 = dr["r3"].ToString();
                        _ARList.AmtRange4 = dr["r4"].ToString();
                        _ARList.AmtRange5 = dr["r5"].ToString();
                        _ARList.AmtRange6 = dr["gtr5"].ToString();
                        _ARList.AdvanceAmount = dr["AdvanceAmount"].ToString();                      
                        _ARList.AccId = dr["acc_id"].ToString();
                        _ARList.TotalAmt = dr["tamt"].ToString();
                      
                        _AccountRecModel.Range1 = dr["range1"].ToString();
                        _AccountRecModel.Range2 = dr["range2"].ToString();
                        _AccountRecModel.Range3 = dr["range3"].ToString();
                        _AccountRecModel.Range4 = dr["range4"].ToString();
                        _AccountRecModel.Range5 = dr["range5"].ToString();

                        _ARList.cust_catg = dr["cust_catg"].ToString();
                        _ARList.cust_port = dr["cust_port"].ToString();
                        _ARList.cust_regin = dr["cust_regin"].ToString();
                        _ARList.cust_zone = dr["cust_zone"].ToString();
                        _ARList.cust_grp = dr["cust_grp"].ToString();
                        _ARList.stateLData = dr["state"].ToString();
                        _ARList.cityLdata = dr["city"].ToString();
                        _ARListDetail.Add(_ARList);
                    }
                }

                _AccountRecModel.AccountRecivableList = _ARListDetail;
                _AccountRecModel.ReceivableType = ReceivableType;
                //if (dt.Rows.Count > 0)/*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/ 
                //{
                //    ViewBag.AccRecvDetails = dt;
                //}
                if (ds.Tables[0].Rows.Count > 0) 
                {
                    ViewBag.AccRecvDetails = ds.Tables[0];
                }
                if (ds.Tables[1].Rows.Count > 0)/*Add by Hina Sharma on 19-07-2025 to show bucket wise total*/
                {
                    ViewBag.BucketWiseTotal = ds.Tables[1];
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAccountReceivable.cshtml", _AccountRecModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult GetInvoiceListDetail(string Cust_id, string lrange, string urange, string Basis, string AsDate,int CurrId,string ReceivableType,string ReportType,string inv_no,string inv_dt, string brlist,string sls_per)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<InvoiceList> _InvoiceListDetail = new List<InvoiceList>();
                AccountReceivableModel _AccountRecModel = new AccountReceivableModel();
                string CompID = string.Empty;
                DataTable dt = new DataTable();

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
                dt = _AccountReceivable_ISERVICE.GetInvoiceDetailList(CompID, Br_ID, Cust_id, lrange, urange, Basis, AsDate, CurrId, ReceivableType, ReportType, inv_no, inv_dt, brlist,sls_per, UserID);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //var inv_no = dr["inv_no"].ToString();
                        //var Invno = inv_no.Split('/');
                        //var INo = Invno[3];
                        //var code = INo.Split('0');
                        //var Doccode = code[0];

                        InvoiceList _InvoiceList = new InvoiceList();
                        _InvoiceList.Invoice_No = dr["inv_no"].ToString();
                        _InvoiceList.Invoice_Dt = dr["inv_dt"].ToString();
                        _InvoiceList.Invoice_Date = dr["inv_date"].ToString();
                        //_InvoiceList.Invoice_Amt = Convert.ToDecimal(dr["inv_amt_sp"]).ToString(ValDigit);
                        //_InvoiceList.Paid_Amt = Convert.ToDecimal(dr["paid_amt"]).ToString(ValDigit);
                        //_InvoiceList.Balance_Amt = Convert.ToDecimal(dr["pend_amt"]).ToString(ValDigit);
                        //_InvoiceList.Total_Invoice_Amt = Convert.ToDecimal(dr["totalInvAmt"]).ToString(ValDigit);
                        //_InvoiceList.Total_Paid_Amt = Convert.ToDecimal(dr["totalPaidAmt"]).ToString(ValDigit);
                        //_InvoiceList.Total_Balance_Amt = Convert.ToDecimal(dr["totalPendAmt"]).ToString(ValDigit);
                        _InvoiceList.Invoice_Amt = dr["inv_amt_sp"].ToString();
                        _InvoiceList.Paid_Amt = dr["paid_amt"].ToString();
                        _InvoiceList.Balance_Amt = dr["pend_amt"].ToString();
                        _InvoiceList.Total_Invoice_Amt = dr["totalInvAmt"].ToString();
                        _InvoiceList.Total_Paid_Amt = dr["totalPaidAmt"].ToString();
                        _InvoiceList.Total_Balance_Amt = dr["totalPendAmt"].ToString();
                        _InvoiceList.Pay_term = dr["paym_term"].ToString();
                        _InvoiceList.due_days = dr["due_days"].ToString();
                        _InvoiceList.due_Date = dr["due_date"].ToString();
                        //_InvoiceList.DocCode = Doccode;/*Add by Hina sharma on 17-04-2025*/
                        _InvoiceList.DocCode = dr["src_type"].ToString();/*Add by Hina sharma on 17-04-2025 for show hide pdf of insight partial*/
                        _InvoiceList.salesPerson = dr["salesPerson"].ToString();/*Add by Hina sharma on 17-04-2025 for show hide pdf of insight partial*/
                        _InvoiceListDetail.Add(_InvoiceList);
                    }
                }

                _AccountRecModel.InvoiceList = _InvoiceListDetail;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAccountReceivableInvoiceDetails.cshtml", _AccountRecModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetOverDueReceiptsList(string Basis, string AsDate,string ReceivableType)
        {
            //Session["ARAging_basis"] = Basis;
            AccountReceivableModel _AccRecModel = new AccountReceivableModel();
            if (Basis == "D")
            {
                _AccRecModel.Basis = Basis;
                _AccRecModel.ARAging_basis = Basis;
            }
            else
            {
                _AccRecModel.ReceivableType = ReceivableType;
                _AccRecModel.ARReceivableType = ReceivableType;
                _AccRecModel.Basis = Basis;
            }
            return RedirectToAction("AccountReceivable", _AccRecModel);

        }
        private List<ARList> GetAccountReceivable_Detail(string Cust_id, string Cat_id, string Prf_id, string Reg_id, string Basis, string AsDate,string ReceivableType,string ReportType,string sales_per, string customerZone, string CustomerGroup, string state_id, string city_id)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<ARList> _ARListDetail = new List<ARList>();
                AccountReceivableModel _AccountRecModel = new AccountReceivableModel();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                //DataTable dt = new DataTable();/*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/ 
                DataSet ds = new DataSet();

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
                //Session["ARAging"] = "Search";
                ds = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, Cust_id, Cat_id, Prf_id, Reg_id, Basis, AsDate,0,"List",0, ReceivableType, ReportType, Br_ID, sales_per, customerZone, CustomerGroup, state_id, city_id);
                _AccountRecModel.ARAging = "Search";
                ViewBag.flag = ReportType;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.AccRecvDetails = ds.Tables[0];
                }
                //if (dt.Rows.Count > 0)/*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/ 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //foreach (DataRow dr in dt.Rows)/*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/ 
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ARList _ARList = new ARList();
                        _ARList.CustName = dr["custname"].ToString();
                        _ARList.CustId = dr["custid"].ToString();
                        _ARList.Curr = dr["curr"].ToString();
                        _ARList.CurrId = dr["curr_id"].ToString();/*add by Hina sharma on 17-12-2024 for filter*/
                        //_ARList.AmtRange1 = Convert.ToDecimal(dr["r1"]).ToString(ValDigit);
                        //_ARList.AmtRange2 = Convert.ToDecimal(dr["r2"]).ToString(ValDigit);
                        //_ARList.AmtRange3 = Convert.ToDecimal(dr["r3"]).ToString(ValDigit);
                        //_ARList.AmtRange4 = Convert.ToDecimal(dr["r4"]).ToString(ValDigit);
                        //_ARList.AmtRange5 = Convert.ToDecimal(dr["r5"]).ToString(ValDigit);
                        //_ARList.AmtRange6 = Convert.ToDecimal(dr["gtr5"]).ToString(ValDigit);
                        //_ARList.TotalAmt = Convert.ToDecimal(dr["tamt"]).ToString(ValDigit);

                        _ARList.AmtRange1 = dr["r1"].ToString();
                        _ARList.AmtRange2 = dr["r2"].ToString();
                        _ARList.AmtRange3 = dr["r3"].ToString();
                        _ARList.AmtRange4 = dr["r4"].ToString();
                        _ARList.AmtRange5 = dr["r5"].ToString();
                        _ARList.AmtRange6 = dr["gtr5"].ToString();
                        _ARList.totamt_sp = dr["tot_amt"].ToString();
                        _ARList.totamt_bs = dr["tot_amt_bs"].ToString();
                        _ARList.AdvanceAmount = dr["AdvanceAmount"].ToString();
                        _ARList.advamt_bs = dr["Adv_Amt_bs"].ToString();
                        _ARList.AccId = dr["acc_id"].ToString();
                        _ARList.TotalAmt = dr["tamt"].ToString();
                        _ARList.totnetamt_bs = dr["tamt_bs"].ToString();

                        //_ARList.AmtRange1 = dr["r1"].ToString();
                        //_ARList.AmtRange2 = dr["r2"].ToString();
                        //_ARList.AmtRange3 = dr["r3"].ToString();
                        //_ARList.AmtRange4 = dr["r4"].ToString();
                        //_ARList.AmtRange5 = dr["r5"].ToString();
                        //_ARList.AmtRange6 = dr["gtr5"].ToString();
                        //_ARList.AdvanceAmount = dr["AdvanceAmount"].ToString();
                        //_ARList.AccId = dr["acc_id"].ToString();
                        //_ARList.TotalAmt = dr["tamt"].ToString();
                        _AccountRecModel.Range1 = dr["range1"].ToString();
                        _AccountRecModel.Range2 = dr["range2"].ToString();
                        _AccountRecModel.Range3 = dr["range3"].ToString();
                        _AccountRecModel.Range4 = dr["range4"].ToString();
                        _AccountRecModel.Range5 = dr["range5"].ToString();

                        _ARList.cust_catg = dr["cust_catg"].ToString();
                        _ARList.cust_port = dr["cust_port"].ToString();
                        _ARList.cust_regin = dr["cust_regin"].ToString();
                        _ARList.cust_zone = dr["cust_zone"].ToString();
                        _ARList.cust_grp = dr["cust_grp"].ToString();
                        _ARList.stateLData = dr["state"].ToString();
                        _ARList.cityLdata = dr["city"].ToString();
                        _ARListDetail.Add(_ARList);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)/*Add by Hina Sharma on 19-07-2025 to show bucket wise total*/
                {
                    ViewBag.BucketWiseTotal = ds.Tables[1];
                }
                return _ARListDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        /*--------------------------For PDF Print Start--------------------------*/
        [HttpPost]
        [Obsolete]
        public FileResult GenratePdfFile(AccountReceivableModel _AccRecModel)
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet ds = new DataSet();
                //ds = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, _AccRecModel.cust_id, _AccRecModel.category, _AccRecModel.portFolio, _AccRecModel.Region, _AccRecModel.Basis, _AccRecModel.To_dt, 0, "List", 0, _AccRecModel.ReceivableType);
                ds = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, _AccRecModel.HiddenCustId, _AccRecModel.Hidcategory, _AccRecModel.HidportFolioLists, _AccRecModel.HidRegionName, _AccRecModel.Basis, _AccRecModel.To_dt, 0, "List", 0, _AccRecModel.ReceivableType, _AccRecModel.ReportType,_AccRecModel.hdnbr_ids, _AccRecModel.Sales_per, _AccRecModel.HidcustomerZone, _AccRecModel.HidCustomerGroup, _AccRecModel.state, _AccRecModel.city);
                //DataTable Details = new DataTable();
                //DataTable dt = new DataTable();
                List<ARList> _ARListDetail = new List<ARList>();
                //Details = ToDataTable(_AccRecModel.AccRcvablPDFData,"PDFData");
                //dt = Details;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ARList _ARList = new ARList();
                        _ARList.CustName = dr["custname"].ToString();
                        if (_AccRecModel.ReportType == "D")
                        {
                            _ARList.Inv_no = dr["inv_no"].ToString();
                            _ARList.Inv_dt = dr["inv_dt"].ToString();
                            _ARList.Invoice_Amt = dr["inv_amt"].ToString();
                        }
                        _ARList.CustId = dr["custid"].ToString();
                        _ARList.Curr = dr["curr"].ToString();
                        //_ARList.AmtRange1 = Convert.ToDecimal(dr["r1"]).ToString(ValDigit);
                        //_ARList.AmtRange2 = Convert.ToDecimal(dr["r2"]).ToString(ValDigit);
                        //_ARList.AmtRange3 = Convert.ToDecimal(dr["r3"]).ToString(ValDigit);
                        //_ARList.AmtRange4 = Convert.ToDecimal(dr["r4"]).ToString(ValDigit);
                        //_ARList.AmtRange5 = Convert.ToDecimal(dr["r5"]).ToString(ValDigit);
                        //_ARList.AmtRange6 = Convert.ToDecimal(dr["gtr5"]).ToString(ValDigit);
                        //_ARList.TotalAmt = Convert.ToDecimal(dr["tamt"]).ToString(ValDigit);
                        _ARList.AmtRange1 = dr["r1"].ToString();
                        _ARList.AmtRange2 = dr["r2"].ToString();
                        _ARList.AmtRange3 = dr["r3"].ToString();
                        _ARList.AmtRange4 = dr["r4"].ToString();
                        _ARList.AmtRange5 = dr["r5"].ToString();
                        _ARList.AmtRange6 = dr["gtr5"].ToString();
                        _ARList.AdvanceAmount = dr["AdvanceAmount"].ToString();//Commented by Suraj on 29-06-2024
                        //_ARList.AccId = dr["acc_id"].ToString();//Commented by Suraj on 29-06-2024
                        _ARList.TotalAmt = dr["tamt"].ToString();
                        _AccRecModel.Range1 = dr["range1"].ToString();
                        _AccRecModel.Range2 = dr["range2"].ToString();
                        _AccRecModel.Range3 = dr["range3"].ToString();
                        _AccRecModel.Range4 = dr["range4"].ToString();
                        _AccRecModel.Range5 = dr["range5"].ToString();

                        _ARList.cust_catg = dr["cust_catg"].ToString();
                        _ARList.cust_port = dr["cust_port"].ToString();
                        _ARList.cust_regin = dr["cust_regin"].ToString();
                        _ARList.cust_zone = dr["cust_zone"].ToString();
                        _ARList.cust_grp = dr["cust_grp"].ToString();
                        _ARList.stateLData = dr["state"].ToString();
                        _ARList.cityLdata = dr["city"].ToString();
                        _ARListDetail.Add(_ARList);
                    }
                    //Details = ds.Tables[0];
                }

                _AccRecModel.AccountRecivableList = _ARListDetail;
                //DataView data = new DataView();
                //data = Details.DefaultView;
                //data.Sort = "acc_name";
                //Details = data.ToTable();
                //DataTable dt = new DataTable();
                //dt = data.ToTable(true, "acc_name");
                if (_AccRecModel.Basis == "I")
                {
                    _AccRecModel.Basis = "Invoice Date";
                }
                else
                {
                    _AccRecModel.Basis = "Due Date";
                }
                if (_AccRecModel.ReceivableType == "A")
                {
                    ViewBag.ReceivableTypePrint = "All";
                }
                else if (_AccRecModel.ReceivableType == "O")
                {
                    ViewBag.ReceivableTypePrint = "Overdue";
                }
                else
                {
                    ViewBag.ReceivableTypePrint = "Upcoming Due";
                }

                _AccRecModel.To_dt = Convert.ToDateTime(_AccRecModel.To_dt).ToString("dd-MM-yyyy"); ;
                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, Br_ID);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                //ViewBag.Details = Details;
                //ViewBag.FromDate = Convert.ToDateTime(_AccRecModel.From_dt).ToString("dd-MM-yyyy");
                ViewBag.FromDate = _AccRecModel.From_dt;
                ViewBag.ToDate = _AccRecModel.To_dt;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.DocName = "Account Receivable";
                ViewBag.TotalDetails = ds.Tables[1];/*Add by Hina Sharma on 19-07-2025 for show bucket wise total*/
                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountReceivable/AccountReceivablePrint.cshtml", _AccRecModel));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(GLVoucherHtml);
                    pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    //string draftImage = Server.MapPath("~/Content/Images/draft.png");
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                //var draftimg = Image.GetInstance(draftImage);
                                //draftimg.SetAbsolutePosition(100, 0);
                                //draftimg.ScaleAbsolute(650f, 650f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    //if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                    //{
                                    //    content.AddImage(draftimg);
                                    //}
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return File(bytes.ToArray(), "application/pdf", "AccountReceivable.pdf");
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
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
        private DataTable ToDataTable(string Details,string command)
        {
            try
            {
                DataTable DtblDetail = new DataTable();
                DataTable dtItem = new DataTable();
                if(command== "CSVInsight")
                {
                    dtItem.Columns.Add("Inv_no", typeof(string));
                    dtItem.Columns.Add("Inv_dt", typeof(string));
                    dtItem.Columns.Add("Invoice_Amt", typeof(string));
                    dtItem.Columns.Add("Paid_amt", typeof(string));
                    dtItem.Columns.Add("Balance_Amt", typeof(string));
                    dtItem.Columns.Add("Due_date", typeof(string));
                    dtItem.Columns.Add("due_days", typeof(string));
                    
                    if (Details != null)
                    {
                        JArray jObject = JArray.Parse(Details);

                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["Inv_no"] = jObject[i]["inv_no"].ToString();
                            dtrowLines["Inv_dt"] = jObject[i]["inv_dt"].ToString();
                            dtrowLines["Invoice_Amt"] = jObject[i]["ApInvoice_Amt"].ToString();
                            dtrowLines["Paid_amt"] = jObject[i]["Paid_amt"].ToString();
                            dtrowLines["Balance_Amt"] = jObject[i]["ApBalance_Amt"].ToString();
                            dtrowLines["Due_date"] = jObject[i]["Apdue_Date"].ToString();
                            dtrowLines["due_days"] = jObject[i]["APdue_days"].ToString();
                            dtItem.Rows.Add(dtrowLines);
                        }
                    }
                }
                else if(command== "PaidAmtCSVInsight")
                {
                    dtItem.Columns.Add("VouNo", typeof(string));
                    dtItem.Columns.Add("VouDate", typeof(string));
                    dtItem.Columns.Add("VouType", typeof(string));
                    dtItem.Columns.Add("paid_amt", typeof(string));

                    if (Details != null)
                    {
                        JArray jObject = JArray.Parse(Details);

                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["VouNo"] = jObject[i]["AR_VouNo"].ToString();
                            dtrowLines["VouDate"] = jObject[i]["AR_VouDate"].ToString();
                            dtrowLines["VouType"] = jObject[i]["AR_VouType"].ToString();
                            dtrowLines["paid_amt"] = jObject[i]["AR_paid_amt"].ToString();
                            dtItem.Rows.Add(dtrowLines);
                        }
                    }
                }
                else if (command == "AdvAmtCSVInsight")
                {
                    dtItem.Columns.Add("VouNo", typeof(string));
                    dtItem.Columns.Add("VouDate", typeof(string));
                    dtItem.Columns.Add("VouType", typeof(string));
                    dtItem.Columns.Add("amt_sp", typeof(string));
                    dtItem.Columns.Add("adj_amt", typeof(string));
                    dtItem.Columns.Add("pend_amt", typeof(string));

                    if (Details != null)
                    {
                        JArray jObject = JArray.Parse(Details);

                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["VouNo"] = jObject[i]["VouNo"].ToString();
                            dtrowLines["VouDate"] = jObject[i]["VouDt"].ToString();
                            dtrowLines["VouType"] = jObject[i]["VouType"].ToString();
                            dtrowLines["amt_sp"] = jObject[i]["amt_sp"].ToString();
                            dtrowLines["adj_amt"] = jObject[i]["adj_amt"].ToString();
                            dtrowLines["pend_amt"] = jObject[i]["pend_amt"].ToString();
                            dtItem.Rows.Add(dtrowLines);
                        }
                    }
                }
                else
                {
                    dtItem.Columns.Add("custname", typeof(string));
                    dtItem.Columns.Add("custid", typeof(string));
                    dtItem.Columns.Add("curr", typeof(string));
                    dtItem.Columns.Add("r1", typeof(string));
                    dtItem.Columns.Add("r2", typeof(string));
                    dtItem.Columns.Add("r3", typeof(string));
                    dtItem.Columns.Add("r4", typeof(string));
                    dtItem.Columns.Add("r5", typeof(string));
                    dtItem.Columns.Add("gtr5", typeof(string));
                    dtItem.Columns.Add("tamt", typeof(string));
                    dtItem.Columns.Add("AdvanceAmount", typeof(string));
                    dtItem.Columns.Add("range1", typeof(string));
                    dtItem.Columns.Add("range2", typeof(string));
                    dtItem.Columns.Add("range3", typeof(string));
                    dtItem.Columns.Add("range4", typeof(string));
                    dtItem.Columns.Add("range5", typeof(string));


                    if (Details != null)
                    {
                        JArray jObject = JArray.Parse(Details);

                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["custname"] = jObject[i]["cust_name"].ToString();
                            dtrowLines["custid"] = jObject[i]["cust_id"].ToString();
                            dtrowLines["curr"] = jObject[i]["Curr"].ToString();
                            dtrowLines["r1"] = jObject[i]["r1"].ToString();
                            dtrowLines["r2"] = jObject[i]["r2"].ToString();
                            dtrowLines["r3"] = jObject[i]["r3"].ToString();
                            dtrowLines["r4"] = jObject[i]["r4"].ToString();
                            dtrowLines["r5"] = jObject[i]["r5"].ToString();
                            dtrowLines["gtr5"] = jObject[i]["gtr5"].ToString();
                            dtrowLines["tamt"] = jObject[i]["tamt"].ToString();
                            dtrowLines["AdvanceAmount"] = jObject[i]["AdvanceAmount"].ToString();
                            dtrowLines["range1"] = jObject[i]["range1"].ToString();
                            dtrowLines["range2"] = jObject[i]["range2"].ToString();
                            dtrowLines["range3"] = jObject[i]["range3"].ToString();
                            dtrowLines["range4"] = jObject[i]["range4"].ToString();
                            dtrowLines["range5"] = jObject[i]["range5"].ToString();

                            dtItem.Rows.Add(dtrowLines);
                        }
                    }
                }              
                DtblDetail = dtItem;
                return DtblDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        /*--------------------------For PDF Print End--------------------------*/
        public FileResult ExportTrialBalanceData(AccountReceivableModel _AccRecModel,string command)
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
                DataTable Details = new DataTable();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                List<ARList> _ARListDetail = new List<ARList>();
                if(command== "CSVInsight")
                {
                    Details = ToDataTable(_AccRecModel.hdnCSVInsightData, command);

                    dt1.Columns.Add("Sr. No.", typeof(int));
                    dt1.Columns.Add("Invoice No.", typeof(string));
                    dt1.Columns.Add("Invoice Date", typeof(string));
                    dt1.Columns.Add("Invoice Amount", typeof(decimal));
                    dt1.Columns.Add("Paid Amount", typeof(decimal));
                    dt1.Columns.Add("Balance Amount", typeof(decimal));
                    dt1.Columns.Add("Due Date", typeof(string));
                    dt1.Columns.Add("Overdue Days", typeof(string));

                    if (Details.Rows.Count > 0)
                    {                       
                        int rowno = 0;
                        foreach (DataRow dr in Details.Rows)
                        {
                            //ARList _ARList = new ARList();
                            DataRow rows = dt1.NewRow();
                            rows["Sr. No."] = rowno + 1;
                            rows["Invoice No."] = dr["Inv_no"];
                            rows["Invoice Date"] = dr["Inv_dt"].ToString();
                            rows["Invoice Amount"] = dr["Invoice_Amt"].ToString();
                            rows["Paid Amount"] = dr["Paid_amt"].ToString();
                            rows["Balance Amount"] = dr["Balance_Amt"].ToString();
                            rows["Due Date"] = dr["Due_date"].ToString();
                            rows["Overdue Days"] = dr["due_days"].ToString();
                            dt1.Rows.Add(rows);
                            //_ARListDetail.Add(_ARList);
                            rowno = rowno + 1;
                        }
                    }
                    //_AccRecModel.AccountRecivableList = _ARListDetail;
                    //var ItemListData = (from tempitem in _ARListDetail select tempitem);
                    //string searchValue = _AccRecModel.searchValue;
                    //if (!string.IsNullOrEmpty(searchValue))
                    //{
                    //    searchValue = searchValue.ToUpper();
                    //    ItemListData = ItemListData.Where(m => m.Inv_no.ToUpper().Contains(searchValue) || m.Inv_dt.ToUpper().Contains(searchValue)
                    //    || m.Invoice_Amt.ToUpper().Contains(searchValue) || m.Paid_amt.ToUpper().Contains(searchValue) || m.Balance_Amt.ToUpper().Contains(searchValue)
                    //    || m.Due_date.ToUpper().Contains(searchValue) || m.due_days.ToUpper().Contains(searchValue));
                    //}
                    //var data = ItemListData.ToList();
                    //_AccRecModel.hdnCSVPrint = null;
                    //dt1 = ToGernalLedgerDetailExl(data, _AccRecModel, command);
                }
                else if (command == "PaidAmtCSVInsight")
                {
                    Details = ToDataTable(_AccRecModel.hdnPaidAmtCSVInsightData, command);
                    if (Details.Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in Details.Rows)
                        {
                            ARList _ARList = new ARList();
                            _ARList.SrNo = rowno + 1;
                            _ARList.VouNo = dr["VouNo"].ToString();
                            _ARList.VouDate = dr["VouDate"].ToString();
                            _ARList.VouType = dr["VouType"].ToString();
                            _ARList.Paid_amt = dr["paid_amt"].ToString();
                            _ARListDetail.Add(_ARList);
                            rowno = rowno + 1;
                        }
                    }
                    _AccRecModel.AccountRecivableList = _ARListDetail;
                    var ItemListData = (from tempitem in _ARListDetail select tempitem);
                    string searchValue = _AccRecModel.searchValue;
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => m.VouNo.ToUpper().Contains(searchValue) || m.VouDate.ToUpper().Contains(searchValue)
                        || m.VouType.ToUpper().Contains(searchValue) || m.Paid_amt.ToUpper().Contains(searchValue));
                    }
                    var data = ItemListData.ToList();
                    _AccRecModel.hdnCSVPrint = null;
                    dt1 = ToGernalLedgerDetailExl(data, _AccRecModel, command);
                }
                else if (command == "AdvAmtCSVInsight")
                {
                    Details = ToDataTable(_AccRecModel.AccRcvablPDFData, command);

                    dt1.Columns.Add("Sr. No.", typeof(int));
                    dt1.Columns.Add("Voucher Number", typeof(string));
                    dt1.Columns.Add("Voucher Date", typeof(string));
                    dt1.Columns.Add("Voucher Type", typeof(string));
                    dt1.Columns.Add("Amount", typeof(decimal));
                    dt1.Columns.Add("Adjusted Amount", typeof(decimal));
                    dt1.Columns.Add("Pending Amount", typeof(decimal));
                    if (Details.Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in Details.Rows)
                        {
                            DataRow rows = dt1.NewRow();
                            rows["Sr. No."] = rowno + 1;
                            rows["Voucher Number"] = dr["VouNo"].ToString();
                            rows["Voucher Date"] = dr["VouDate"].ToString();
                            rows["Voucher Type"] = dr["VouType"].ToString();
                            rows["Amount"] = dr["amt_sp"].ToString();
                            rows["Adjusted Amount"] = dr["adj_amt"].ToString();
                            rows["Pending Amount"] = dr["pend_amt"].ToString();
                            dt1.Rows.Add(rows);
                            rowno = rowno + 1;
                        }
                    }                  
                }
                else
                {
                    DataTable dataTable = new DataTable();
                    DataSet ds = new DataSet();
                    //ds = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, _AccRecModel.cust_id, _AccRecModel.category, _AccRecModel.portFolio, _AccRecModel.Region, _AccRecModel.Basis, _AccRecModel.To_dt, 0, "List", 0, _AccRecModel.ReceivableType);
                    ds = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, _AccRecModel.cust_id, _AccRecModel.category, _AccRecModel.portFolio, _AccRecModel.RegionName, _AccRecModel.Basis, _AccRecModel.To_dt, 0, "List", 0, _AccRecModel.ReceivableType, _AccRecModel.ReportType,_AccRecModel.hdnbr_ids, _AccRecModel.Sales_per, _AccRecModel.customerZone, _AccRecModel.CustomerGroup, _AccRecModel.state, _AccRecModel.city);

                    var range1 = Convert.ToInt32(_AccRecModel.Range1) + 1;
                    var range2 = Convert.ToInt32(_AccRecModel.Range2) + 1;
                    var range3 = Convert.ToInt32(_AccRecModel.Range3) + 1;
                    var range4 = Convert.ToInt32(_AccRecModel.Range4) + 1;
                    var range5 = Convert.ToInt32(_AccRecModel.Range5) + 1;

                    var r1 = "0" + "-" + _AccRecModel.Range1 + ' ' + "Days";
                    var r2 = range1 + "-" + _AccRecModel.Range2 + ' ' + "Days";
                    var r3 = range2 + "-" + _AccRecModel.Range3 + ' ' + "Days";
                    var r4 = range3 + "-" + _AccRecModel.Range4 + ' ' + "Days";
                    var r5 = range4 + "-" + _AccRecModel.Range5 + ' ' + "Days";
                    var r6 = ">" + range5 + ' ' + "Days";


                    dataTable.Columns.Add("Sr. No.", typeof(int));
                    dataTable.Columns.Add("Customer Name", typeof(string));
                    if (_AccRecModel.ReportType == "D")
                    {
                        dataTable.Columns.Add("Invoice No.", typeof(string));
                        dataTable.Columns.Add("Invoice Date", typeof(string));
                    }
                    dataTable.Columns.Add("Currency", typeof(string));
                    if (_AccRecModel.ReportType == "D")
                    {
                        dataTable.Columns.Add("Invoice Amount", typeof(decimal));
                    }
                        dataTable.Columns.Add(r1, typeof(decimal));
                    dataTable.Columns.Add(r2, typeof(decimal));
                    dataTable.Columns.Add(r3, typeof(decimal));
                    dataTable.Columns.Add(r4, typeof(decimal));
                    dataTable.Columns.Add(r5, typeof(decimal));
                    dataTable.Columns.Add(r6, typeof(decimal));
                    if (_AccRecModel.ReportType != "D")
                    {
                        dataTable.Columns.Add("Advance Payment", typeof(decimal));
                    }       
                    dataTable.Columns.Add("Total", typeof(decimal));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var SrNo = 1;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow rows = dataTable.NewRow();
                            rows["Sr. No."] = SrNo;
                            rows["Customer Name"] = dr["custname"].ToString();
                            if (_AccRecModel.ReportType == "D")
                            {
                                rows["Invoice No."] = dr["inv_no"].ToString();
                                rows["Invoice Date"] = dr["inv_dt"].ToString();
                            }
                            rows["Currency"] = dr["curr"].ToString();
                            if (_AccRecModel.ReportType == "D")
                            {
                                rows["Invoice Amount"] = dr["inv_amt"].ToString();
                            }
                                rows[r1] = dr["r1"].ToString();
                            rows[r2] = dr["r2"].ToString();
                            rows[r3] = dr["r3"].ToString();
                            rows[r4] = dr["r4"].ToString();
                            rows[r5] = dr["r5"].ToString();
                            rows[r6] = dr["gtr5"].ToString();
                            if (_AccRecModel.ReportType != "D")
                            {
                                rows["Advance Payment"] = dr["AdvanceAmount"].ToString();
                            }                                
                            rows["Total"] = dr["tamt"].ToString();
                            dataTable.Rows.Add(rows);
                            SrNo = SrNo + 1;
                        }
                        dt1 = dataTable;
                    }

                    //Details = ToDataTable(_AccRecModel.AccRcvablPDFData, command);
                    //dt = Details;
                    //if (dt.Rows.Count > 0)
                    //{
                    //    int rowno = 0;
                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        ARList _ARList = new ARList();
                    //        _ARList.SrNo = rowno + 1;
                    //        _ARList.CustName = dr["custname"].ToString();
                    //        _ARList.CustId = dr["custid"].ToString();
                    //        _ARList.Curr = dr["curr"].ToString();
                    //        _ARList.AmtRange1 = dr["r1"].ToString();
                    //        _ARList.AmtRange2 = dr["r2"].ToString();
                    //        _ARList.AmtRange3 = dr["r3"].ToString();
                    //        _ARList.AmtRange4 = dr["r4"].ToString();
                    //        _ARList.AmtRange5 = dr["r5"].ToString();
                    //        _ARList.AmtRange6 = dr["gtr5"].ToString();
                    //        _ARList.TotalAmt = dr["tamt"].ToString();
                    //        _ARList.AdvanceAmount = dr["AdvanceAmount"].ToString();
                    //        _AccRecModel.Range1 = dr["range1"].ToString();
                    //        _AccRecModel.Range2 = dr["range2"].ToString();
                    //        _AccRecModel.Range3 = dr["range3"].ToString();
                    //        _AccRecModel.Range4 = dr["range4"].ToString();
                    //        _AccRecModel.Range5 = dr["range5"].ToString();
                    //        _ARListDetail.Add(_ARList);
                    //        rowno = rowno + 1;
                    //    }
                    //}
                    //_AccRecModel.AccountRecivableList = _ARListDetail;
                    //var ItemListData = (from tempitem in _ARListDetail select tempitem);
                    //string searchValue = _AccRecModel.searchValue;
                    //if (!string.IsNullOrEmpty(searchValue))
                    //{
                    //    searchValue = searchValue.ToUpper();
                    //    ItemListData = ItemListData.Where(m => m.CustName.ToUpper().Contains(searchValue) || m.CustId.ToUpper().Contains(searchValue)
                    //    || m.Curr.ToUpper().Contains(searchValue) || m.AmtRange1.ToUpper().Contains(searchValue) || m.AmtRange2.ToUpper().Contains(searchValue)
                    //    || m.AmtRange3.ToUpper().Contains(searchValue) || m.AmtRange4.ToUpper().Contains(searchValue) || m.AmtRange5.ToUpper().Contains(searchValue)
                    //    || m.AmtRange6.ToUpper().Contains(searchValue) || m.TotalAmt.ToUpper().Contains(searchValue));
                    //}
                    //var data = ItemListData.ToList();
                    //_AccRecModel.hdnCSVPrint = null;                  
                    //dt1 = ToGernalLedgerDetailExl(data, _AccRecModel, command);
                }               
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Account Receivable", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToGernalLedgerDetailExl(List<ARList> _ItemListModel, AccountReceivableModel _AccRecModel,string command)
        {
            DataTable dataTable = new DataTable();
            if (command == "CSVInsight")
            {
                dataTable.Columns.Add("Sr. No.", typeof(int));
                dataTable.Columns.Add("Invoice No.", typeof(string));
                dataTable.Columns.Add("Invoice Date", typeof(string));
                dataTable.Columns.Add("Invoice Amount", typeof(decimal));
                dataTable.Columns.Add("Paid Amount", typeof(decimal));
                dataTable.Columns.Add("Balance Amount", typeof(decimal));
                dataTable.Columns.Add("Due Date", typeof(string));
                dataTable.Columns.Add("Overdue Days", typeof(string));

                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr. No."] = item.SrNo;
                    rows["Invoice No."] = item.Inv_no;
                    rows["Invoice Date"] = item.Inv_dt;
                    rows["Invoice Amount"] = item.Invoice_Amt;
                    rows["Paid Amount"] = item.Paid_amt;
                    rows["Balance Amount"] = item.Balance_Amt;
                    rows["Due Date"] = item.Due_date;
                    rows["Overdue Days"] = item.due_days;
                    dataTable.Rows.Add(rows);
                }
            }
            else if(command== "PaidAmtCSVInsight")
            {
                dataTable.Columns.Add("Sr. No.", typeof(int));
                dataTable.Columns.Add("Voucher Number", typeof(string));
                dataTable.Columns.Add("Voucher Date", typeof(string));
                dataTable.Columns.Add("Voucher Type", typeof(string));
                dataTable.Columns.Add("Amount", typeof(decimal));

                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr. No."] = item.SrNo;
                    rows["Voucher Number"] = item.VouNo;
                    rows["Voucher Date"] = item.VouDate;
                    rows["Voucher Type"] = item.VouType;
                    rows["Amount"] = item.Paid_amt;
                    dataTable.Rows.Add(rows);
                }
            }
            else
            {
                var range1 = Convert.ToInt32(_AccRecModel.Range1) + 1;
                var range2 = Convert.ToInt32(_AccRecModel.Range2) + 1;
                var range3 = Convert.ToInt32(_AccRecModel.Range3) + 1;
                var range4 = Convert.ToInt32(_AccRecModel.Range4) + 1;
                var range5 = Convert.ToInt32(_AccRecModel.Range5) + 1;

                var r1 = "0" + "-" + _AccRecModel.Range1 + ' ' + "Days";
                var r2 = range1 + "-" + _AccRecModel.Range2 + ' ' + "Days";
                var r3 = range2 + "-" + _AccRecModel.Range3 + ' ' + "Days";
                var r4 = range3 + "-" + _AccRecModel.Range4 + ' ' + "Days";
                var r5 = range4 + "-" + _AccRecModel.Range5 + ' ' + "Days";
                var r6 = ">" + range5 + ' ' + "Days";

                
                dataTable.Columns.Add("Sr. No.", typeof(int));
                dataTable.Columns.Add("Customer Name", typeof(string));
                dataTable.Columns.Add("Currency", typeof(string));
                dataTable.Columns.Add(r1, typeof(decimal));
                dataTable.Columns.Add(r2, typeof(decimal));
                dataTable.Columns.Add(r3, typeof(decimal));
                dataTable.Columns.Add(r4, typeof(decimal));
                dataTable.Columns.Add(r5, typeof(decimal));
                dataTable.Columns.Add(r6, typeof(decimal));
                dataTable.Columns.Add("Advance Payment", typeof(decimal));
                dataTable.Columns.Add("Total", typeof(decimal));

                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr. No."] = item.SrNo;
                    rows["Customer Name"] = item.CustName;
                    rows["Currency"] = item.Curr;
                    rows[r1] = item.AmtRange1;
                    rows[r2] = item.AmtRange2;
                    rows[r3] = item.AmtRange3;
                    rows[r4] = item.AmtRange4;
                    rows[r5] = item.AmtRange5;
                    rows[r6] = item.AmtRange6;
                    rows["Advance Payment"] = item.AdvanceAmount;
                    rows["Total"] = item.TotalAmt;
                    dataTable.Rows.Add(rows);
                }
            }            
            return dataTable;
        }
        public ActionResult SearchAdvanceAmountDetail(string accId,int CurrId, string AsDate, string Basis, string ReceivableType, string brlist,string CurrName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                //if (Session["userid"] != null)
                //    UserID = Session["userid"].ToString();
                //DataTable dt = _AccountReceivable_ISERVICE.SearchAdvanceAmountDetail(CompID, Br_ID, accId);
                DataSet ds = _AccountReceivable_ISERVICE.SearchAdvanceAmountDetail(CompID, Br_ID, accId, CurrId, AsDate, Basis, ReceivableType, brlist);
                ViewBag.AdvanceAmountDetail = ds.Tables[0];
                ViewBag.AdvanceAmountDetailTotal = ds.Tables[1];
                ViewBag.CurrName = CurrName;
                //ViewBag.AdvanceAmountDetail = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAdvancePaymentDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SearchPaidAmountDetail(string InVNo, string InvDate,string cust_id)/*Add by Hina sharma on 10-12-2024*/
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                //if (Session["userid"] != null)
                //    UserID = Session["userid"].ToString();
                //DataTable dt = _AccountReceivable_ISERVICE.SearchAdvanceAmountDetail(CompID, Br_ID, accId);
                DataSet dt = _AccountReceivable_ISERVICE.SearchPaidAmountDetail(CompID, Br_ID, InVNo, InvDate, cust_id);
                ViewBag.PaidAmountDetail = dt.Tables[0];
                ViewBag.PaidAmountDetailTotal = dt.Tables[1];
                //ViewBag.AdvanceAmountDetail = dt;
                return PartialView("~/Areas/Common/Views/Cmn_PartialPaidAmountDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        /*-----------For get Invoice detail Popup Pdf by particular Invoice no------  */
        //public FileResult GenerateInvoiceDetails(string invNo, string invDate, /*string invType,*/ string dataType)
        //{
        //    if (dataType == "DSI")
        //    {
        //        //ViewBag.Title = "Sales Invoice";
        //        ViewBag.Title = "Tax Invoice";
        //    }
        //    else if (dataType == "ESI")
        //    {
        //        ViewBag.Title = "Commercial Invoice";
        //    }
        //    else if (dataType == "CI")
        //    {
        //        ViewBag.Title = "Custom Invoice";
        //    }
        //    else if (dataType == "IPI")
        //    {
        //        ViewBag.Title = "Import Invoice";
        //    }
        //    //ViewBag.Title = "EInvoicePreview";
        //    return File(GetIvDtlPopupPrintData(invNo, invDate, /*invType,*/ dataType), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        //}
        //public byte[] GetIvDtlPopupPrintData(string invNo, string invDate, string dataType)
        //{
        //    StringReader reader = null;
        //    Document pdfDoc = null;
        //    PdfWriter writer = null;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        string inv_type = "";
        //        string ReportType = "common";
        //        DataSet Details = new DataSet();
        //        if (dataType == "DSI" || dataType == "ESI")
        //        {
        //            Details = _AccountReceivable_ISERVICE.GetInvoiceDeatilsForPrint(CompID, Br_ID, invNo, invDate, dataType);
        //            //ViewBag.PageName = "PI";
        //            //if (dataType == "SSI")
        //            //{
        //            //    string path1 = Server.MapPath("~") + "..\\Attachment\\";
        //            //    string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
        //            //    ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
        //            //}
        //            //else
        //            //{
        //                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
        //                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
        //                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
        //                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
        //                else
        //                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
        //                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
        //                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
        //            //}
        //        }
        //        ViewBag.Details = Details;
        //        //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
        //        ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
        //        ViewBag.InvoiceTo = "";

        //        string htmlcontent = "";
        //        if (dataType == "DSI" )
        //        {
        //            ViewBag.PageName = "SI";
        //            //ViewBag.Title = "Sales Invoice";
        //            ViewBag.Title = "Tax Invoice";
        //            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrint.cshtml"));
        //        }
        //        using (MemoryStream stream = new System.IO.MemoryStream())
        //        {
        //            reader = new StringReader(htmlcontent);
        //            if (ReportType == "common")
        //            {
        //                if (inv_type == "SI")
        //                {
        //                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 60f);
        //                }
        //                else
        //                {
        //                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 70f, 90f);
        //                }
        //            }
        //            writer = PdfWriter.GetInstance(pdfDoc, stream);
        //            pdfDoc.Open();
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
        //            pdfDoc.Close();
        //            Byte[] bytes = stream.ToArray();
        //            bytes = GSTHeaderFooterPagination(bytes, Details, ReportType, inv_type);
        //            return bytes.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //    finally
        //    {

        //    }
        //}
        public FileResult GenerateInvoiceDetails(AccountReceivableModel _AccRecModel, string invNo, string invDate, string dataType)
        {
            var docId = "";
            if (dataType == "ESI")
            {
                docId = "105103145125";
            }
            if (dataType == "DSI")
            {
                docId = "105103140";
            }
            CommonPageDetails();
            _AccRecModel.GstApplicable = ViewBag.GstApplicable;

            if (_AccRecModel.GstApplicable == "Y" && docId != "105103145125")
                return File(GetPdfDataOfGstInv(docId, invNo, invDate, dataType), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
            else
                return File(GetPdfData(docId, invNo, invDate, dataType), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");

        }
        public byte[] GetPdfData(string docId, string invNo, string invDt, string dataType)
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
                    Br_ID = Session["BranchId"].ToString();
                }
                string inv_type = "";
                string ReportType = "common";
                if (docId == "105103140")
                {
                    inv_type = "SI";
                }
                if (docId == "105103145125")
                {
                    inv_type = "CI";
                }
                DataSet Details = _AccountReceivable_ISERVICE.GetSalesInvoiceDeatilsForPrint(CompID, Br_ID, invNo, invDt, inv_type);
                ViewBag.PageName = "SI";

                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();

                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                if (invType == "D")
                {
                    ViewBag.Title = "Sales Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoicePrint.cshtml"));
                }
                else
                {
                    ViewBag.Title = "Commercial Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/CommercialInvoicePrint.cshtml"));
                }



                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {
                        if (inv_type == "SI")
                        {
                            pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                        }
                        else
                        {
                            pdfDoc = new Document(PageSize.A4, 20f, 20f, 70f, 90f);
                        }
                    }

                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, Details, ReportType, inv_type);
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
        public byte[] GetPdfDataOfGstInv(/*DataTable dt,*/string docId, string invNo, string invDt, string dataType)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                string inv_type = "";
                string ReportType = "common";
                switch (docId)
                {
                    case "105103140":
                        inv_type = "SI";
                        break;
                    case "105103145125":
                        inv_type = "CI";
                        break;
                    default:
                        inv_type = "";
                        break;
                }
                DataSet Details = new DataSet();
                if (dataType == "DSI" || dataType == "ESI")
                {
                    Details = _AccountReceivable_ISERVICE.GetSlsInvGstDtlForPrint(CompID, Br_ID, invNo, invDt, inv_type);
                    ViewBag.PageName = "SI";
                }
                if (dataType == "SSI"|| dataType == "SCI")
                {
                    Details = _AccountReceivable_ISERVICE.GetInvoiceDeatilsForPrint(CompID, Br_ID, invNo, invDt, dataType);
                    if (dataType == "SSI")
                    {
                        ViewBag.PageName = "SSI";
                    }
                    else
                    {
                        ViewBag.PageName = "SI";
                    }
                    
                }
                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                //ViewBag.ProntOption = ProntOption;
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                string htmlcontent = "";
                ViewBag.Title = "Tax Invoice";
                //if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                //{
                //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrintF2.cshtml"));
                //}
                //else
                //{
                if (dataType == "DSI" || dataType == "ESI")
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrint.cshtml"));

                }
                //}
                else if (dataType == "SSI")
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoice/ServiceSaleInvoiceWithGSTPrint.cshtml"));

                }
                else if (dataType == "SCI")
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceWithGSTPrint.cshtml"));

                }
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {
                        if (inv_type == "SI" || dataType == "SSI" || dataType == "SCI")
                        {
                            pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 60f);
                        }
                        else
                        {
                            pdfDoc = new Document(PageSize.A4, 10f, 10f, 70f, 90f);
                        }
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = GSTHeaderFooterPagination(bytes, Details, ReportType, inv_type, dataType);
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
        private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, string inv_type)
        {
            var docstatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 13, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = Server.MapPath(Details.Tables[0].Rows[0]["logo"].ToString());
            //     string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string draftImage = Server.MapPath("~/Content/Images/draft.png");

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        if (ReportType == "common")
                        {
                            if (inv_type == "SI")
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 220);
                                draftimg.ScaleAbsolute(550f, 600f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                }
                            }
                            else
                            {


                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 220);
                                draftimg.ScaleAbsolute(550f, 600f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    try
                                    {
                                        //var image = Image.GetInstance(logo);
                                        //image.SetAbsolutePosition(31, 794);
                                        //image.ScaleAbsolute(68f, 15f);
                                        //content.AddImage(image);
                                    }
                                    catch { }
                                    content.Rectangle(34.5, 28, 526, 60);


                                    BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                                    content.BeginText();
                                    content.SetColorFill(CMYKColor.BLACK);
                                    content.SetFontAndSize(baseFont1, 9);
                                    content.CreateTemplate(20, 10);
                                    content.SetLineWidth(25);

                                    var txt = Details.Tables[0].Rows[0]["declar"].ToString();
                                    string[] stringSeparators = new string[] { "\r\n" };
                                    string text = txt;
                                    string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

                                    var y = 65;
                                    for (var j = 0; j < lines.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
                                        y = y - 10;
                                    }

                                    txt = Details.Tables[0].Rows[0]["inv_head"].ToString();
                                    text = txt;
                                    string[] lines1 = text.Split(stringSeparators, StringSplitOptions.None);

                                    content.SetFontAndSize(baseFont1, 8);
                                    y = 771;
                                    for (var j = 0; j < lines1.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_CENTER, lines1[j], 300, y, 0);
                                        y = y - 10;
                                    }
                                    content.SetFontAndSize(baseFont1, 9);

                                    content.EndText();

                                    //content.Rectangle(450, 25, 120, 35);
                                    string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                                    Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
                                    Phrase ptitle = new Phrase(String.Format("Commercial Invoice", i, PageCount), fonttitle);
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
                                    //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
                                    Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
                                    Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
                                    //Phrase p1 = new Phrase(Details.Tables[0].Rows[0]["declar"].ToString(), font1);
                                    //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                                    //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                                    //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
                                    /*------------------Header ---------------------------*/
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

                                    /*------------------Header end---------------------------*/

                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
                                    //ColumnText.AddText(p1);
                                    //content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Details.Tables[0].Rows[0]["declar"].ToString(), 300, 400, 45);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
                                }
                            }
                        }


                    }
                    bytes = ms.ToArray();
                }
            }

            return bytes;
        }
        private Byte[] GSTHeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, string inv_type, string dataType)
        {
            string Br_ID = string.Empty;
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var docstatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 15, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
            string State_Name = "";
            if (dataType == "DSI" || dataType == "ESI")
            {
                State_Name = Details.Tables[9].Rows[0]["state_name"].ToString();
                //String StateName = (State_Name).ToUpper();
            }
            else if (dataType == "SSI")
            {
                State_Name = Details.Tables[5].Rows[0]["state_name"].ToString();
                //String StateName = (State_Name).ToUpper();
            }
            else if (dataType == "SCI")
            {
                State_Name = Details.Tables[7].Rows[0]["state_name"].ToString();
                //String StateName = (State_Name).ToUpper();
            }
            String StateName = (State_Name).ToUpper();
            ViewBag.QrCode = QR;
            string draftImage = Server.MapPath("~/Content/Images/draft.png");
            string bnetImage = Server.MapPath("~/images/businet.png");

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        if (ReportType == "common")
                        {
                            if (inv_type == "SI" || dataType == "SSI" || dataType == "SCI")
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 40);
                                draftimg.ScaleAbsolute(650f, 600f);
                                var qrCode = Image.GetInstance(QR);
                                qrCode.SetAbsolutePosition(720, 475);
                                qrCode.ScaleAbsolute(100f, 95f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    //try
                                    //{
                                    //    var image = Image.GetInstance(logo);
                                    //    image.SetAbsolutePosition(30, 540);
                                    //    image.ScaleAbsolute(90f, 25f);
                                    //    if (i == 1)
                                    //        content.AddImage(image);
                                    //}
                                    //catch { }
                                    if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                                    {
                                        if (i == 1)
                                            content.AddImage(qrCode);
                                    }
                                    //Phrase ptitle = new Phrase(String.Format("Tax Invoice", i, PageCount), fonttitle);
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    if (i == PageCount)
                                    {
                                        try
                                        {
                                            var bnetlogo = Image.GetInstance(bnetImage);
                                            bnetlogo.SetAbsolutePosition(322, 35);
                                            bnetlogo.ScaleAbsolute(70f, 16f);
                                            content.AddImage(bnetlogo);

                                        }
                                        catch { }
                                        Phrase ftr = new Phrase(String.Format("This Document is generated by ", i, PageCount), font);
                                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, ftr, 320, 40, 0);
                                        Phrase ftr1 = new Phrase(String.Format("SUBJECT TO " + StateName + " JURISDICTION ", i, PageCount), font);
                                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ftr1, 300, 60, 0);
                                        // 500,560 is for left right alignment of text and 40,60 is for above or below text of align.

                                    }
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, p, 800, 15, 0);
                                    //if(i == 1)
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 400, 550, 0);
                                }
                            }
                            else
                            {
                                //var image = Image.GetInstance(logo);
                                //image.SetAbsolutePosition(31, 794);
                                //image.ScaleAbsolute(68f, 15f);

                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 220);
                                draftimg.ScaleAbsolute(550f, 600f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    //content.AddImage(image);
                                    content.Rectangle(34.5, 28, 526, 60);


                                    BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                                    content.BeginText();
                                    content.SetColorFill(CMYKColor.BLACK);
                                    content.SetFontAndSize(baseFont1, 9);
                                    content.CreateTemplate(20, 10);
                                    content.SetLineWidth(25);

                                    var txt = Details.Tables[0].Rows[0]["declar"].ToString();
                                    string[] stringSeparators = new string[] { "\r\n" };
                                    string text = txt;
                                    string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

                                    var y = 65;
                                    for (var j = 0; j < lines.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
                                        y = y - 10;
                                    }

                                    txt = Details.Tables[0].Rows[0]["inv_head"].ToString();
                                    text = txt;
                                    string[] lines1 = text.Split(stringSeparators, StringSplitOptions.None);

                                    content.SetFontAndSize(baseFont1, 8);
                                    y = 771;
                                    for (var j = 0; j < lines1.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_CENTER, lines1[j], 300, y, 0);
                                        y = y - 10;
                                    }
                                    content.SetFontAndSize(baseFont1, 9);
                                    content.EndText();
                                    //content.Rectangle(450, 25, 120, 35);
                                    string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                                    Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
                                    Phrase ptitle = new Phrase(String.Format("Commercial Invoice", i, PageCount), fonttitle);
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
                                    //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
                                    Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
                                    Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
                                    //Phrase p1 = new Phrase(Details.Tables[0].Rows[0]["declar"].ToString(), font1);
                                    //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                                    //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                                    //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
                                    /*------------------Header ---------------------------*/
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

                                    /*------------------Header end---------------------------*/

                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
                                    //ColumnText.AddText(p1);
                                    //content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Details.Tables[0].Rows[0]["declar"].ToString(), 300, 400, 45);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
                                }
                            }
                        }

                    }
                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }
        private string GenerateQRCode(string qrcodeText)
        {
            if (string.IsNullOrEmpty(qrcodeText))
                qrcodeText = "N/A";
            string path = Server.MapPath("~");
            string fileName = "QR_" + Guid.NewGuid().ToString();
            string folderPath = path + ("..\\LogsFile\\EmailAlertPDFs\\");
            string imagePath = folderPath + fileName + ".jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = imagePath;
            var barcodeBitmap = new System.Drawing.Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
        }
        /*-----------------------Print Section Begin--------------------*/
        public FileResult GenratePdfFile1(string accId, string currId, string asOndate,int Curr_Id,int Acc_Id,string ReportType, string brlist,string sales_per,string customerZone,string CustomerGroup,string state_id,string city_id)
        {
            //string curr, string r1, string r2, string r3, string r4, string r5, string r6,
            //string rv1, string rv2, string rv3, string rv4, string rv5, string rv6, string advPmt, string totalAmt
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet dtdata = _AccountReceivable_ISERVICE.GetGLAccountReceivablePrintData(CompID, Br_ID, accId, currId, asOndate, UserID, brlist);

                ViewBag.RangeValues = dtdata.Tables[1];
                /*commented and Modify by Hina sharma on 24-12-2024 to also show pending invoces and advance payments details*/
                /*Commented and modify by Hina sharma on 24-12-2024 to change as dataset instead of datatable*/
                //DataTable dtAccPbl = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, accId, "0", "0", "0", "I", asOndate);
                //DataView dv = new DataView(dtAccPbl);
                //dv.RowFilter = "curr='" + currId + "' ";
                //ViewBag.AccPbl = dv.ToTable();
                
                DataSet dsAllTbl = _AccountReceivable_ISERVICE.GetAgingDetailList(CompID, Br_ID, UserID, accId, "0", "0", "0", "I", asOndate, Curr_Id, "Print", Acc_Id, "A", ReportType, brlist, sales_per,customerZone, CustomerGroup, state_id,city_id);
                //ViewBag.AccPbl = dsAllTbl.Tables[0];
                DataView dv = new DataView(dsAllTbl.Tables[0]);
                dv.RowFilter = "curr='" + currId + "' ";
                ViewBag.AccPbl = dv.ToTable();
                ViewBag.PendingInvoicesDetail = dsAllTbl.Tables[1];
                ViewBag.AdvancePaymentsDetail = dsAllTbl.Tables[2];
                ViewBag.TotalAdvancePaymentsDetail = dsAllTbl.Tables[3];/*Add by Hina sharma on 06-01-2025*/
                ViewBag.TotalPendingInvoicesDetail = dsAllTbl.Tables[4];/*Add by Hina sharma on 06-01-2025*/
                ViewBag.CustAddress = dsAllTbl.Tables[5];/*Add by Hina sharma on 30-04-2025*/
                DataTable Details = new DataTable();
                Details = dtdata.Tables[0]; //ToDataTable(_model.PrintData);
                DataView data = new DataView();
                data = Details.DefaultView;
                DataTable dt = new DataTable();
                dt = data.ToTable(true, "acc_name");

                DataTable dtlogo = _Common_IServices.GetCompLogo(CompID, Br_ID);
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                ViewBag.CompLogoDtl = dtlogo;
                string LogoPath = path1 + dtlogo.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Details = Details;
                ViewBag.FromDate = dtdata.Tables[2].Rows[0][0].ToString();
                ViewBag.ToDate = Convert.ToDateTime(asOndate).ToString("dd-MM-yyyy");
                //ViewBag.CompanyName = "Alaska Exports";
                ViewBag.DocName = "Customer Statement";
                ViewBag.Currency = currId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 30f, 80f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    data = Details.DefaultView;
                    DataTable PrintGL = new DataTable();
                    pdfDoc.Open();
                    string Start = "Y";

                    //data = Details.DefaultView;
                    PrintGL = data.ToTable();
                    ViewBag.PrintGL = PrintGL;
                    //ViewBag.GLAccountName = PrintGL.Rows[0]["op_bal"].ToString();
                    ViewBag.GLAccountName = "";
                    if (PrintGL.Rows.Count > 0)
                        ViewBag.GLAccountName = PrintGL.Rows[0]["br_op_bal_bs"].ToString();
                    //ViewBag.GLAccountName = PrintGL.Rows[PrintGL.Rows.Count-1]["closing_bal"].ToString();

                    ViewBag.GLAccountName = "";
                    if (PrintGL.Rows.Count > 0)
                        ViewBag.GLAccountName = PrintGL.Rows[PrintGL.Rows.Count - 1]["br_acc_bal_bs"].ToString();
                    htmlcontent = ConvertPartialViewToString1(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/AccountReceivable/AccountReceivablePrintLedger.cshtml"));
                    reader = new StringReader(htmlcontent);
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font font = new Font(bf, 9, Font.BOLDITALIC, BaseColor.BLACK);
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return File(bytes.ToArray(), "application/pdf", "CustomerStatement.pdf");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
            finally
            {

            }
        }
        protected string ConvertPartialViewToString1(PartialViewResult partialView)
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
        /*-----------------------Print Section END--------------------*/
        public string SavePdfDocToSendOnEmailAlert_Ext(string Cust_id, string Curr, int Curr_id, int Acc_id, string AsDate, string ReportType, string fileName,string br_list,string sales_per, string customerZone, string CustomerGroup, string state_id, string city_id)
        {
            try
            {
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                var fileResult = GenratePdfFile1(Cust_id, Curr, AsDate, Curr_id, Acc_id, ReportType, br_list, sales_per, customerZone, CustomerGroup, state_id, city_id);
                var fileContentResult = fileResult as FileContentResult;
                byte[] data = fileContentResult.FileContents;
                return commonCont.SaveAlertDocument_MailExt(data, fileName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
        }
        private List<SalesPersList> GetSalesPersonList()
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }

                DataTable dt = _AccountReceivable_ISERVICE.GetSalesPersonList(CompID, Br_ID, UserID);
                List<SalesPersList> slsperslist = new List<SalesPersList>();
                foreach (DataRow dr in dt.Rows)
                {
                    SalesPersList slspers = new SalesPersList
                    {
                        sls_pers_id = dr["sls_pers_id"].ToString(),
                        sls_pers_name = dr["sls_pers_name"].ToString()
                    };
                    slsperslist.Add(slspers);
                }
                //slsperslist.Insert(0, new SalesPersList { sls_pers_id = "0", sls_pers_name = "---All---" });
                return slsperslist;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public ActionResult BindStateListData(AccountReceivableModel _AccRecModel)
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
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (_AccRecModel.SearchState == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _AccRecModel.SearchState;
                    }
                    DataSet ProductList = _AccountReceivable_ISERVICE.BindStateListData(CompID, Br_ID, SarchValue);
                    if (ProductList.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[2].Rows.Count; i++)
                        {
                            string state_id = ProductList.Tables[2].Rows[i]["state_id"].ToString();
                            string state_name = ProductList.Tables[2].Rows[i]["state_name"].ToString();
                            string country_name = ProductList.Tables[2].Rows[i]["country_name"].ToString();
                            ItemList.Add(state_id+','+ country_name, state_name);
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
        public ActionResult BindCityListdata(AccountReceivableModel _AccRecModel)
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
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (_AccRecModel.SearchCity == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _AccRecModel.SearchCity;
                    }
                    DataSet ProductList = _AccountReceivable_ISERVICE.BindCityListdata(CompID, Br_ID, SarchValue, _AccRecModel.state_id);
                    if (ProductList.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[3].Rows.Count; i++)
                        {
                            string city_id = ProductList.Tables[3].Rows[i]["city_id"].ToString();
                            string city_name = ProductList.Tables[3].Rows[i]["city_name"].ToString();
                            string state_name = ProductList.Tables[3].Rows[i]["state_name"].ToString();
                            string country_name = ProductList.Tables[3].Rows[i]["country_name"].ToString();
                            ItemList.Add(city_id+','+ state_name+','+ country_name, city_name);
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
        //public ActionResult ViewEmailAlert(string mail, string status, string docid, string custname, string curr, string Totalamount,string FilePath)
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            UserID = Session["userid"].ToString();
        //        }
        //        var commonCont = new CommonController(_Common_IServices);
        //        DataTable dt = new DataTable();
        //        ViewBag.mailmessage = _Common_IServices.ViewEmailAlert(CompID, Br_ID, UserID, docid, custname, curr, mail, FilePath);
        //        return View("~/Areas/Common/Views/Cmn_MailView.cshtml");
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //}
    }
}