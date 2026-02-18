using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.TDSPosting;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.TDSPosting;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static EnRepMobileWeb.Areas.Common.Controllers.Common.CommonFunctions;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.TDSPosting
{
    public class TDSPostingController : Controller
    {
        string CompID, BrId, UserID, language = String.Empty;
        string DocumentMenuId = "105104130", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        readonly Common_IServices _Common_IServices;
        readonly TDSPosting_ISERVICES _TDSPosting_ISERVICES;
        public TDSPostingController(Common_IServices _Common_IServices, TDSPosting_ISERVICES tdsPostingIservices)
        {
            this._Common_IServices = _Common_IServices;
            this._TDSPosting_ISERVICES = tdsPostingIservices;
        }
        // GET: ApplicationLayer/TDSPosting
        public ActionResult TDSPosting(string wfStatus)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrId, DocumentMenuId);

                TdsListModel _model = new TdsListModel();
                DataSet ds = new DataSet();
                if (wfStatus != null)
                {
                    ViewBag.ListFilterData1 = "0,0,0," + wfStatus;
                }
                if (TempData["UrlData"] != null)
                {
                    if (TempData["UrlData"].ToString() != "")
                    {
                        UrlData urlData = TempData["UrlData"] as UrlData;
                        if (urlData.ListFilterData1 != null)
                        {
                            var arr = urlData.ListFilterData1.Split(',');
                            _model.Year = arr[0];
                            _model.Month = arr[1];
                            _model.Status = arr[2];
                            if (wfStatus == null)
                            {
                                wfStatus = arr[3];
                            }

                            ViewBag.ListFilterData1 = _model.Year + "," + _model.Month + "," + _model.Status + "," + wfStatus;
                        }
                        
                    }
                }
                var searchValue = "0";
                ds = _TDSPosting_ISERVICES.GetTdsPostngList(CompID,BrId, InCase(_model.Year,"0",null), InCase(_model.Month,"0",null), InCase(_model.Status,"0",null), DocumentMenuId,wfStatus, UserID, searchValue);

                //Binding Month Dropdown
                List<MonthList> months = new List<MonthList>();
                months.Add(new MonthList { month_no = "0", month_name = "---Select---" });
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    months.Add(new MonthList { month_no = dr["month_no"].ToString(), month_name = dr["month_name"].ToString() });
                }

                //Binding Year Dropdown
                List<YearList> years = new List<YearList>();
                years.Add(new YearList { Year = "0", YearVal = "---Select---" });
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    years.Add(new YearList { Year = dr["Years"].ToString(), YearVal = dr["Years"].ToString() });
                }

                _model.monthList = months;
                _model.yearList = years;
                _model.DocumentMenuId = DocumentMenuId;
                ViewBag.TdsPostingList = ds.Tables[0];
                ViewBag.MenuPageName = getDocumentName();
                ViewBag.VBRoleList = GetRoleList();

                List<statusLists> statusLists = new List<statusLists>();
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    statusLists list = new statusLists();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _model.statusLists = statusLists;

                _model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/TDSPosting/TDSPostingList.cshtml", _model);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult TDSListSearch(string Year,string month,string status,string wfStatus)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet ds = new DataSet();
                var searchValue = "0";
                ds = _TDSPosting_ISERVICES.GetTdsPostngList(CompID, BrId, InCase(Year,"0",null), InCase(month, "0", null), InCase(status, "0", null), DocumentMenuId,null, UserID, searchValue);
                ViewBag.TdsPostingList = ds.Tables[0];
                ViewBag.TdsSearch = "TdsSearch";
                ViewBag.ListFilterData1 = Year + "," + month + "," + status + "," + wfStatus;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialTDSPosting.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult AddTDSPostingDetail(string tds_id,string Month,string Year,string ListFilterData1)
        {
            UrlData urlData = new UrlData();
            string BtnName = Month == null ? "BtnAddNew" : "BtnToDetailPage";
            string TransType = Month == null ? "Save" : "Update";
            SetUrlData(urlData,"Add", TransType, BtnName, null,Month,Year,tds_id, ListFilterData1);

            return RedirectToAction("TDSPostingDetail", "TDSPosting", urlData);
        }
        // Set URL data by passing parameters
        private void SetUrlData(UrlData urlData,string Command, string TransType, string BtnName,string Message = null
            , string month = null, string year = null,string tds_id=null,string ListFilterData1=null)
        {
            
            urlData.Command = Command;
            urlData.TransType = TransType;
            urlData.BtnName = BtnName;
            urlData.month = month;
            urlData.year = year;
            urlData.tds_id = tds_id;
            urlData.ListFilterData1 = ListFilterData1;
            TempData["UrlData"] = urlData;
            TempData["Message"] = Message;

        }
        //Main Datail Page Load method
        public ActionResult TDSPostingDetail(UrlData urlData)
        {
            try
            {
                TDSPosting_Model _tdsPostingModel = new TDSPosting_Model(); 
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrId, DocumentMenuId);
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    _tdsPostingModel.Message = TempData["Message"]!=null? TempData["Message"].ToString():null;
                }
              
                _tdsPostingModel.BtnCommand = urlData.Command;
                _tdsPostingModel.TransType = urlData.TransType;
                _tdsPostingModel.BtnName = urlData.BtnName;
                _tdsPostingModel.MonthNo = urlData.month;
                _tdsPostingModel.Year = urlData.year;
                _tdsPostingModel.tds_id = urlData.tds_id;
                _tdsPostingModel.ListFilterData1 = urlData.ListFilterData1;

                ViewBag.MenuPageName = getDocumentName();
                DataSet ds = new DataSet();
                ds = _TDSPosting_ISERVICES.GetTdsPostngDetail(CompID, BrId, UserID, IsNull(_tdsPostingModel.MonthNo,"0")
                    , IsNull(_tdsPostingModel.Year, "0"), IsNull(_tdsPostingModel.tds_id, "0"));
                

                List<MonthList> months = new List<MonthList>();
                months.Add(new MonthList { month_no = "0", month_name = "---Select---" });
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    months.Add(new MonthList { month_no = dr["month_no"].ToString(), month_name = dr["month_name"].ToString() });
                //}

                List<YearList> years = new List<YearList>();
                years.Add(new YearList { Year = "0", YearVal = "---Select---" });
                //foreach (DataRow dr in ds.Tables[1].Rows)
                //{
                //    years.Add(new YearList { Year = dr["Years"].ToString(), YearVal = dr["Years"].ToString() });
                //}

                List<GlAccList> glAccounts = new List<GlAccList>();
                glAccounts.Add(new GlAccList { acc_id = "0", acc_name = "---Select---" });
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    glAccounts.Add(new GlAccList { acc_id = dr["acc_id"].ToString(), acc_name = dr["acc_name"].ToString() });
                }
                //-------------------------
                string month_no = "0",year ="0";
                if (IsNull(_tdsPostingModel.MonthNo, "0") != "0" && IsNull(_tdsPostingModel.Year, "0") != "0" && IsNull(_tdsPostingModel.tds_id, "0") != "0")
                {
                    
                    foreach (DataRow dr in ds.Tables[9].Rows)
                    {
                        month_no = dr["month_no"].ToString();
                        months.Add(new MonthList { month_no = month_no, month_name = dr["YearMonth"].ToString() });
                    }
                    foreach (DataRow dr in ds.Tables[9].Rows)
                    {
                        year = dr["Years"].ToString();
                        years.Add(new YearList { Year = year, YearVal = year });
                    }
                    _tdsPostingModel.MonthNo = month_no;
                    _tdsPostingModel.Year = year;
                }
                else
                {
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        month_no = dr["month_no"].ToString();
                        months.Add(new MonthList { month_no = dr["month_no"].ToString(), month_name = dr["YearMonth"].ToString() });
                    }
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        year = dr["Years"].ToString();
                        years.Add(new YearList { Year = year, YearVal = year });
                    }
                    _tdsPostingModel.MonthNo = month_no;
                    _tdsPostingModel.Year = year;
                }
                //-------------------------
                _tdsPostingModel.monthList = months;
                _tdsPostingModel.yearList = years;
                _tdsPostingModel.glAccList = glAccounts;
                _tdsPostingModel.Title = title;
                SetTdsModelDetails(_tdsPostingModel, ds);
                
                ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/TDSPosting/TDSPostingDetail.cshtml", _tdsPostingModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //Set Data in Model When Calling Main page Load method
        private void SetTdsModelDetails(TDSPosting_Model _model,DataSet Ds)
        {
            
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            _model.DocumentMenuId = DocumentMenuId;
            if (Ds.Tables.Count > 4)
            {
                string month_name="", year = "";
                year = Ds.Tables[3].Rows[0]["year"].ToString();
                month_name = Ds.Tables[3].Rows[0]["month_name"].ToString() + '-' + year;
                _model.monthList.Add(new MonthList { month_no = Ds.Tables[3].Rows[0]["month"].ToString(), month_name = month_name });
                _model.yearList.Add(new YearList { Year = year, YearVal = year });

                // Setting up Models Data for detail page
                _model.MonthNo = Ds.Tables[3].Rows[0]["month"].ToString();
                _model.Year = Ds.Tables[3].Rows[0]["year"].ToString();
                _model.PrePeriod = Ds.Tables[3].Rows[0]["pre_period"].ToString();
                _model.Period = Ds.Tables[3].Rows[0]["period"].ToString();
                _model.acc_id = Ds.Tables[3].Rows[0]["acc_id"].ToString();
                _model.cal_on = Ds.Tables[3].Rows[0]["cal_on"].ToString();
                _model.Create_by = Ds.Tables[3].Rows[0]["create_nm"].ToString();
                _model.Create_dt = Ds.Tables[3].Rows[0]["create_dt"].ToString();
                _model.App_By = Ds.Tables[3].Rows[0]["app_nm"].ToString();
                _model.App_dt = Ds.Tables[3].Rows[0]["app_dt"].ToString();
                _model.from_dt = Ds.Tables[3].Rows[0]["start_dt"].ToString();
                _model.to_dt = Ds.Tables[3].Rows[0]["end_dt"].ToString();
                _model.DocumentStatus = Ds.Tables[3].Rows[0]["tds_status"].ToString();
                _model.DocStatus = Ds.Tables[3].Rows[0]["status_name"].ToString();
                _model.PreStDt = Ds.Tables[3].Rows[0]["pre_StDt"].ToString();
                _model.PreEndDt = Ds.Tables[3].Rows[0]["pre_EndDt"].ToString();
                _model.CurrStDt = Ds.Tables[3].Rows[0]["Curr_StDt"].ToString();
                _model.CurrEndDt = Ds.Tables[3].Rows[0]["Curr_EndDt"].ToString();
                _model.tds_id = Ds.Tables[3].Rows[0]["tds_id"].ToString();
                _model.Tds_dt = Ds.Tables[3].Rows[0]["tds_dt"].ToString();
                string create_id = Ds.Tables[3].Rows[0]["create_id"].ToString();
                _model.create_id = create_id;
                string Statuscode = _model.DocumentStatus;
                
                string approval_id = Ds.Tables[3].Rows[0]["approval_id"].ToString();

                // Passing Supplier Table To view
                ViewBag.TdsDetails = Ds.Tables[4];
                ViewBag.TdsSlabDetails = Ds.Tables[5];
                ViewBag.CostCenterData = Ds.Tables[11];
                _model.TdsSlabDetails = JsonConvert.SerializeObject(Ds.Tables[5]);
                _model.vouDetail = JsonConvert.SerializeObject(Ds.Tables[10]);
                _model.TdsSuppInvDetails = JsonConvert.SerializeObject(Ds.Tables[12]);

                //For WorkFlow...
                _model.WFBarStatus = DataTableToJSONWithStringBuilder(Ds.Tables[8]);
                _model.WFStatus = DataTableToJSONWithStringBuilder(Ds.Tables[7]);
                if (ViewBag.AppLevel != null && _model.BtnCommand != "Edit")
                {

                    var sent_to = "";
                    var nextLevel = "";
                    if (Ds.Tables[6].Rows.Count > 0)
                    {
                        sent_to = Ds.Tables[6].Rows[0]["sent_to"].ToString();
                    }

                    if (Ds.Tables[7].Rows.Count > 0)
                    {
                        nextLevel = Ds.Tables[7].Rows[0]["nextlevel"].ToString().Trim();
                    }

                    if (Statuscode == "D")
                    {
                        if (create_id != UserID)
                        {
                            _model.BtnName = "Refresh";
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
                                _model.BtnName = "BtnToDetailPage";
                            }
                            else
                            {
                                ViewBag.Approve = "N";
                                ViewBag.ForwardEnbl = "Y";
                                _model.BtnName = "BtnToDetailPage";
                            }

                        }
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            _model.BtnName = "BtnToDetailPage";
                        }


                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                                _model.BtnName = "BtnToDetailPage";
                            }


                        }
                    }
                    if (Statuscode == "F")
                    {
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            _model.BtnName = "BtnToDetailPage";
                        }
                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                            }
                            _model.BtnName = "BtnToDetailPage";
                        }
                    }
                    if (Statuscode == "A")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            _model.BtnName = "BtnToDetailPage";

                        }
                        else
                        {
                            _model.BtnName = "Refresh";
                        }
                    }
                }
                if (ViewBag.AppLevel.Rows.Count == 0)
                {
                    ViewBag.Approve = "Y";
                }
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

        private DataTable GetRoleList()
        {
            try
            {
                string UseId = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UseId = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UseId, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult TdsPostingAction(TDSPosting_Model _model,string Command)
        {
            try
            {
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        SetUrlData(urlData,"Add", "Save", "BtnAddNew", null, null, null, null, _model.ListFilterData1);
                        return RedirectToAction("TDSPostingDetail", urlData);
                    case "Edit":
                        SetUrlData(urlData,"Edit", "Update", "BtnEdit", null, _model.MonthNo, _model.Year,_model.tds_id,_model.ListFilterData1);
                        return RedirectToAction("TDSPostingDetail", urlData);
                    case "Save":
                        SaveTdsDetails(_model);
                        SetUrlData(urlData,"Add", "Update", "BtnToDetailPage", _model.Message,_model.MonthNo,_model.Year,_model.tds_id, _model.ListFilterData1);
                        return RedirectToAction("TDSPostingDetail", urlData);
                        
                    case "Approve":
                        ApproveTdsDetails(_model.tds_id, _model.A_Status, _model.A_Level, _model.A_Remarks,_model,"","");
                        SetUrlData(urlData,"Add", "Update", "BtnToDetailPage", _model.Message,_model.MonthNo,_model.Year,_model.tds_id, _model.ListFilterData1);
                        return RedirectToAction("TDSPostingDetail", urlData);

                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh",null, null, null, null, _model.ListFilterData1);
                        return RedirectToAction("TDSPostingDetail", urlData);

                    case "Delete":
                        DeleteTdsDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, null, _model.ListFilterData1);
                        return RedirectToAction("TDSPostingDetail", urlData);

                    case "BacktoList":
                        SetUrlData(urlData, "", "", "",null,null,null,null,_model.ListFilterData1);
                        return RedirectToAction("TDSPosting");

                    //case "CsvPrint":
                    //    _model.HdnCsvPrint = null;
                    //    return financeBudgetExporttoExcelDt(_model);
                    default:
                        SetUrlData(urlData,"Add", "Save", "BtnAddNew");
                        return RedirectToAction("TDSPostingDetail", urlData);      
                }               
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult TDSPostingExporttoExcelDt(string tds_year,string tds_month_val,string ddlStatus,string searchValue)
        {
            try
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
                    UserID = Session["UserId"].ToString();
                }
                if (searchValue == null)
                    searchValue = "0";
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                ds = _TDSPosting_ISERVICES.GetTdsPostngList(CompID, BrId, InCase(tds_year, "0", null), InCase(tds_month_val, "0", null), InCase(ddlStatus, "0", null), DocumentMenuId, null, UserID, searchValue);
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("Year", typeof(string));
                dt.Columns.Add("Month", typeof(string));
                dt.Columns.Add("Taxable Value", typeof(decimal));
                dt.Columns.Add("TDS Amount", typeof(decimal));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Created By", typeof(string));
                dt.Columns.Add("Created On", typeof(string));
                dt.Columns.Add("Approved By", typeof(string));
                dt.Columns.Add("Approved On", typeof(string));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["Year"] = dr["year"].ToString();
                        dtrowLines["Month"] = dr["month_name"].ToString();
                        dtrowLines["Taxable Value"] = dr["texable_val"].ToString();
                        dtrowLines["TDS Amount"] = dr["tds_amt"].ToString();
                        dtrowLines["Status"] = dr["status_name"].ToString();
                        dtrowLines["Created By"] = dr["create_nm"].ToString();
                        dtrowLines["Created On"] = dr["create_dt"].ToString();
                        dtrowLines["Approved By"] = dr["app_nm"].ToString();
                        dtrowLines["Approved On"] = dr["app_dt"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("TDSPosting", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private void DeleteTdsDetail(TDSPosting_Model model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                string Result = _TDSPosting_ISERVICES.DeleteTdsPostingDetails(CompID,BrId,model.MonthNo,model.Year);

                model.Message = Result.Split(',')[0];

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult SaveTdsDetails(TDSPosting_Model _model)
        {
            try
            {
                DataTable TdsPostingHeader = new DataTable();
                DataTable TdsPostingDetail = new DataTable();
                DataTable TdsPostingSlabDetail = new DataTable();
                DataTable TdsPostingGLDetail = new DataTable();
                DataTable TdsPostingGLDetailCC = new DataTable();
                DataTable TdsPostingSuppInvoice = new DataTable();
                DataTable TdsPostingSuppInvSlab = new DataTable();
                DataTable dt = new DataTable();

                dt.Columns.Add("MenuDocumentId", typeof(string));
                dt.Columns.Add("TransType", typeof(string));
                dt.Columns.Add("comp_id", typeof(int));
                dt.Columns.Add("br_id", typeof(int));
                dt.Columns.Add("tds_id", typeof(string));
                dt.Columns.Add("month", typeof(string));
                dt.Columns.Add("year", typeof(string));
                dt.Columns.Add("start_dt", typeof(string));
                dt.Columns.Add("end_dt", typeof(string));
                dt.Columns.Add("acc_id", typeof(string));
                dt.Columns.Add("cal_on", typeof(string));
                dt.Columns.Add("user_id", typeof(string));
                dt.Columns.Add("mac_id", typeof(string));
                
                
                DataRow dtrow = dt.NewRow();

                dtrow["MenuDocumentId"] = DocumentMenuId;
                dtrow["TransType"] = _model.TransType;
                dtrow["comp_id"] = Session["CompId"].ToString();
                dtrow["br_id"] = Session["BranchId"].ToString();
                dtrow["tds_id"] = IsNull(_model.tds_id,"0");
                dtrow["month"] = _model.MonthNo;
                dtrow["year"] = _model.Year;
                dtrow["start_dt"] = _model.from_dt;
                dtrow["end_dt"] = _model.to_dt;
                dtrow["acc_id"] = _model.acc_id;
                dtrow["user_id"] = Session["UserId"].ToString();
                dtrow["cal_on"] = _model.cal_on;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrow["mac_id"] = mac_id;// _model.cal_on;
                dt.Rows.Add(dtrow);

                TdsPostingHeader = dt;
                TdsPostingDetail = TdsDetails(_model.TdsDetails);
                TdsPostingSlabDetail = TdsSlabDetails(_model.TdsSlabDetails);
                TdsPostingGLDetail = ToDtblvouDetail(_model.vouDetail);
                TdsPostingGLDetailCC = ToDtblccDetail(_model.CC_DetailList);
                TdsPostingSuppInvoice = ToDtblSuppInvDetail(_model.TdsSuppInvDetails);
                TdsPostingSuppInvSlab = ToDtblSuppInvSlabDetail(_model.TdsSuppInvSlabDetails);
                

                string Result = _TDSPosting_ISERVICES.InsertTdsPostingDetails(TdsPostingHeader, TdsPostingDetail, TdsPostingSlabDetail
                    , TdsPostingGLDetail, TdsPostingGLDetailCC, TdsPostingSuppInvoice, TdsPostingSuppInvSlab);
                
                _model.Message = Result.Split(',')[0];
                _model.tds_id = Result.Split(',')[3];

                return RedirectToAction("TDSPostingDetail");
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ApproveTdsDetails(string tds_id, string A_Status, string A_Level, string A_Remarks, TDSPosting_Model _model
            , string ListFilterData1, string WF_Status1)
        {
            try
            {
                UrlData urlData = new UrlData();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Result = _TDSPosting_ISERVICES.ApproveTdsPostingDetails(CompID,BrId,tds_id,A_Status
                    ,A_Level,A_Remarks,UserID,mac_id,DocumentMenuId);

                _model.Message = Result.Split(',')[0]=="A"? "Approved" : "Error";

                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[2], Result.Split(',')[1], tds_id,ListFilterData1);
                return RedirectToAction("TDSPostingDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        private DataTable TdsDetails(string data)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("supp_id", typeof(string));
            dt.Columns.Add("pre_val", typeof(string));
            dt.Columns.Add("curr_mnth_val", typeof(string));
            dt.Columns.Add("tot_val", typeof(string));
            dt.Columns.Add("taxable_val", typeof(string));
            dt.Columns.Add("tds_amt", typeof(string));
            dt.Columns.Add("tds_payable", typeof(string));
            dt.Columns.Add("dn_flag", typeof(string));
            dt.Columns.Add("dn_no", typeof(string));
            dt.Columns.Add("dn_date", typeof(string));

            if (data != null)
            {
                JArray obj = JArray.Parse(data);
                for (int i = 0; i < obj.Count; i++)
                {
                   
                    DataRow dtrow = dt.NewRow();
                    dtrow["supp_id"] = obj[i]["supp_id"];
                    dtrow["pre_val"] = obj[i]["pre_val"];
                    dtrow["curr_mnth_val"] = obj[i]["curr_mnth_val"];
                    dtrow["tot_val"] = obj[i]["tot_val"];
                    dtrow["taxable_val"] = obj[i]["taxable_val"];
                    dtrow["tds_amt"] = obj[i]["tds_amt"];
                    dtrow["tds_payable"] = obj[i]["tds_payable"];
                    dtrow["dn_flag"] = obj[i]["dn_flag"];
                    dtrow["dn_no"] = IsBlank(obj[i]["dn_no"].ToString().Trim(),null);
                    dtrow["dn_date"] = IsBlank(obj[i]["dn_date"].ToString().Trim(),null);
                    dt.Rows.Add(dtrow);
                }
            }
           
                
            return dt;
        }
        private DataTable TdsSlabDetails(string data)
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("supp_id", typeof(string));
            dt.Columns.Add("tds_amt", typeof(string));
            dt.Columns.Add("tds_perc", typeof(string));
            dt.Columns.Add("tds_payble", typeof(string));
            if (data != null)
            {
                JArray obj = JArray.Parse(data);
                for (int i = 0; i < obj.Count; i++)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["supp_id"] = obj[i]["supp_id"];
                    dtrow["tds_amt"] = obj[i]["tds_amt"];
                    dtrow["tds_perc"] = obj[i]["tds_perc"];
                    dtrow["tds_payble"] = obj[i]["tds_payble"];
                    dt.Rows.Add(dtrow);
                }
            }
            

            return dt;
        }
        public ActionResult GetTdsPosting(string tds_month,string tds_year,string from_dt,string to_dt)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }

                DataSet ds = new DataSet();
                ds = _TDSPosting_ISERVICES.GetTdsPostngDetailToAdd(CompID, BrId,from_dt,to_dt);

                return Json(JsonConvert.SerializeObject(ds, Formatting.Indented));
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            

        }
        public ActionResult GetTdsSuppWiseInvoice(string supp_id,string PreVlStD, string PreVlEdD, string Period, string PreVal, string status, string tds_id)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }

                DataSet ds = new DataSet();
                ds = _TDSPosting_ISERVICES.GetTdsSuppWiseInvoiceDetails(CompID, BrId, supp_id, PreVlStD, PreVlEdD, status, tds_id);
                ViewBag.TdsValueInvoice = ds.Tables[0];
                ViewBag.TdsTotalValueInvoice = ds.Tables[1];
                ViewBag.TdsValueSearch = "Y";
                ViewBag.Period = Period;
                ViewBag.PreVal = PreVal;
                //return Json(JsonConvert.SerializeObject(ds, Formatting.Indented));
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_TdsPreviousBalanceInfo.cshtml");
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            

        }
        public ActionResult GetTdsSuppWiseTaxableValueDetail(string SuppId,string StartDate, string EndDate, string Year
            , string Month, string TaxableValue,string SuppName)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }

                DataSet ds = new DataSet();
                ds = _TDSPosting_ISERVICES.GetTdsSuppWiseTaxableValueDetails(CompID, BrId,Year,Month, SuppId, StartDate, EndDate);
                ViewBag.TdsTaxableValueDetail = ds.Tables[0];
                ViewBag.TdsTotalTaxableValueDetail = ds.Tables[1];
                ViewBag.SuppName = SuppName;
                ViewBag.TaxableValue = TaxableValue;
                ViewBag.Searching = true;
                //return Json(JsonConvert.SerializeObject(ds, Formatting.Indented));
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_TdsTaxableValue.cshtml");
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            

        }
        public ActionResult GetMonthOnBehalfYear(string tds_year)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }

                DataSet ds = new DataSet();
                ds = _TDSPosting_ISERVICES.GetMonthOnBehalfYear(CompID, BrId, tds_year);

                return Json(JsonConvert.SerializeObject(ds, Formatting.Indented));
            }
            catch(Exception ex)
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
        public ActionResult GetTdsSuppWiseVoucherDetail(string suppId, string glDetail,string editable)
        {
            try
            {
                DataTable dtGlDetails = new DataTable();
                if (!string.IsNullOrWhiteSpace(glDetail))
                {
                    // Directly deserialize the JSON string into a DataTable
                    dtGlDetails = JsonConvert.DeserializeObject<DataTable>(glDetail);
                }
                ViewBag.GLAccount = dtGlDetails;
                ViewBag.suppId = suppId;
                ViewBag.Editable = editable;
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (editable == "Y")
                {
                    ViewBag.Disable = false;
                }
                else
                {
                    ViewBag.Disable = true;
                }
                return PartialView("~/Areas/Common/Views/Comn_PartialPopupGlVoucher.cshtml");
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error : " + ex.Message);
            }
        }
        private DataTable ToDtblvouDetail(string vouDetail)
        {
            try
            {
                DataTable vou_Details = new DataTable();
                vou_Details.Columns.Add("comp_id", typeof(string));
                vou_Details.Columns.Add("vou_sr_no", typeof(string));
                vou_Details.Columns.Add("gl_sr_no", typeof(string));
                vou_Details.Columns.Add("id", typeof(string));
                vou_Details.Columns.Add("type", typeof(string));
                vou_Details.Columns.Add("doctype", typeof(string));
                vou_Details.Columns.Add("Value", typeof(string));
                vou_Details.Columns.Add("ValueInBase", typeof(string));
                vou_Details.Columns.Add("DrAmt", typeof(string));
                vou_Details.Columns.Add("CrAmt", typeof(string));
                vou_Details.Columns.Add("TransType", typeof(string));
                vou_Details.Columns.Add("Gltype", typeof(string));
                vou_Details.Columns.Add("parent", typeof(string));
                vou_Details.Columns.Add("DrAmtInBase", typeof(string));
                vou_Details.Columns.Add("CrAmtInBase", typeof(string));
                vou_Details.Columns.Add("curr_id", typeof(string));
                vou_Details.Columns.Add("conv_rate", typeof(string));
                vou_Details.Columns.Add("vou_type", typeof(string));
                vou_Details.Columns.Add("bill_no", typeof(string));
                vou_Details.Columns.Add("bill_date", typeof(string));
                vou_Details.Columns.Add("gl_narr", typeof(string));

                if (vouDetail != null && vouDetail != "[]")
                {
                    JArray jObjectVOU = JArray.Parse(vouDetail);
                    for (int i = 0; i < jObjectVOU.Count; i++)
                    {
                        DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                        dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                        dtrowVouDetailsLines["vou_sr_no"] = jObjectVOU[i]["VouSrNo"].ToString();
                        dtrowVouDetailsLines["gl_sr_no"] = jObjectVOU[i]["GlSrNo"].ToString();
                        dtrowVouDetailsLines["id"] = jObjectVOU[i]["id"].ToString();
                        dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                        dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                        dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                        dtrowVouDetailsLines["ValueInBase"] = jObjectVOU[i]["ValueInBase"].ToString();
                        dtrowVouDetailsLines["DrAmt"] = jObjectVOU[i]["DrAmt"].ToString();
                        dtrowVouDetailsLines["CrAmt"] = jObjectVOU[i]["CrAmt"].ToString();
                        dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                        dtrowVouDetailsLines["Gltype"] = jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["parent"] = "0";// jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["DrAmtInBase"] = jObjectVOU[i]["DrAmtInBase"].ToString();
                        dtrowVouDetailsLines["CrAmtInBase"] = jObjectVOU[i]["CrAmtInBase"].ToString();
                        dtrowVouDetailsLines["curr_id"] = jObjectVOU[i]["curr_id"].ToString();
                        dtrowVouDetailsLines["conv_rate"] = jObjectVOU[i]["conv_rate"].ToString();
                        dtrowVouDetailsLines["vou_type"] = jObjectVOU[i]["vou_type"].ToString();
                        dtrowVouDetailsLines["bill_no"] = jObjectVOU[i]["bill_no"].ToString();
                        dtrowVouDetailsLines["bill_date"] = jObjectVOU[i]["bill_date"].ToString();
                        dtrowVouDetailsLines["gl_narr"] = jObjectVOU[i]["gl_narr"].ToString();
                        vou_Details.Rows.Add(dtrowVouDetailsLines);
                    }
                    //ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
                }
                return vou_Details;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblccDetail(string CC_DetailList)
        {
            try
            {
                DataTable CC_Details = new DataTable();
                CC_Details.Columns.Add("vou_sr_no", typeof(string));
                CC_Details.Columns.Add("gl_sr_no", typeof(string));
                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(string));

                if (CC_DetailList != null && CC_DetailList != "[]")
                {
                    JArray JAObj = JArray.Parse(CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();
                        dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                        dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                        CC_Details.Rows.Add(dtrowLines);
                    }
                    //ViewData["CCdetail"] = dtCCdetail(JAObj);
                }
                return CC_Details;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblSuppInvDetail(string SuppInvDtl)
        {
            try
            {
                DataTable dt = new DataTable();
                if (SuppInvDtl != null && SuppInvDtl != "[]")
                {
                    dt = JsonConvert.DeserializeObject<DataTable>(SuppInvDtl);
                }
                else
                {
                    dt.Columns.Add("supp_id", typeof(string));
                    dt.Columns.Add("inv_no", typeof(string));
                    dt.Columns.Add("inv_dt", typeof(string));
                    dt.Columns.Add("bill_no", typeof(string));
                    dt.Columns.Add("bill_dt", typeof(string));
                    dt.Columns.Add("gr_val", typeof(string));
                    dt.Columns.Add("tax_val", typeof(string));
                    dt.Columns.Add("oc_val", typeof(string));
                    dt.Columns.Add("net_val_bs", typeof(string));
                    dt.Columns.Add("tds_val", typeof(string));
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
        private DataTable ToDtblSuppInvSlabDetail(string SuppInvSlbDtl)
        {
            try
            {
                DataTable dt = new DataTable();
                if (SuppInvSlbDtl != null && SuppInvSlbDtl != "[]")
                {
                    dt = JsonConvert.DeserializeObject<DataTable>(SuppInvSlbDtl);
                }
                else
                {
                    dt.Columns.Add("supp_id", typeof(string));
                    dt.Columns.Add("inv_no", typeof(string));
                    dt.Columns.Add("inv_dt", typeof(string));
                    dt.Columns.Add("bill_no", typeof(string));
                    dt.Columns.Add("bill_dt", typeof(string));
                    dt.Columns.Add("inv_amt", typeof(string));
                    dt.Columns.Add("taxable_value", typeof(string));
                    dt.Columns.Add("tds_perc", typeof(string));
                    dt.Columns.Add("tds_val", typeof(string));
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
    }

}