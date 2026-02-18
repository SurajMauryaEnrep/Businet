using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticPacking;
using EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.DomesticPackingIServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.Packing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Web;
using System.Configuration;
using System.Data.OleDb;
using OfficeOpenXml;
using ExcelDataReader;
using System.Text;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution
{
    public class DomesticPackingController : Controller
    {
        DomesticPacking_IServices _DomesticPacking_IServices;
        string DocumentMenuId = "", title;
        Common_IServices _Common_IServices;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, BrchID, language, pack_no, userid = String.Empty;
        DataTable dt;
        DataTable dt1;
        DataSet ds;
        string FromDate;
        public DomesticPackingController(Common_IServices _Common_IServices, DomesticPacking_IServices _DomesticPacking_IServices)
        {
            this._DomesticPacking_IServices = _DomesticPacking_IServices;
            this._Common_IServices = _Common_IServices;
        }
        public ActionResult DomesticPackingList(DomesticPackingList_Model _DomesticPackingList_Model)
        {
            try
            {
                //return   ExportItemsORSerializationDetailsToExcel("ITEMDETAILS", "B1/12/23/DPI0000233");
                //   ExportItemsORSerializationDetailsToExcel("SERIALIZATIONDETAILS", "B1/12/23/DPI0000233");
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DocumentMenuId = "105103130";
                CommonPageDetails();

                // DomesticPackingList_Model _DomesticPackingList_Model = new DomesticPackingList_Model();
                _DomesticPackingList_Model.DocumentMenuId = DocumentMenuId;
                GetStatusList(_DomesticPackingList_Model);
                //List<ListCustomerName> _CustomerNameList = new List<ListCustomerName>();
                //ListCustomerName _ListCustomerName = new ListCustomerName();
                //_ListCustomerName.cust_name = "---Select---";
                //_ListCustomerName.cust_id = "0";
                //_CustomerNameList.Add(_ListCustomerName);
                //_DomesticPackingList_Model.CustomerNameList = _CustomerNameList;

                var cus_typ = "D";
                #region Coommented By Nitesh 05-04-2024 for All DropDown And List In One Procedure
                #endregion
                //DataTable dt = new DataTable();
                //List<ListCustomerName> CustLists = new List<ListCustomerName>();

                //dt = GetCustNameList(_DomesticPackingList_Model, cus_typ);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    ListCustomerName _RAList = new ListCustomerName();
                //    _RAList.cust_id = dr["cust_id"].ToString();
                //    _RAList.cust_name = dr["cust_name"].ToString();
                //    CustLists.Add(_RAList);
                //}
                //CustLists.Insert(0, new ListCustomerName() { cust_id = "0", cust_name = "---Select---" });
                //_DomesticPackingList_Model.CustomerNameList = CustLists;


                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    var CustID = a[0].Trim();
                    var Date = a[1].Trim();
                    var tdt = a[2].Trim();
                    DateTime Fromdate = Convert.ToDateTime(a[1].Trim());
                    DateTime Todate = Convert.ToDateTime(a[2].Trim());
                    var Status = a[3].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }

                    // Session["DPDSearch"] = "DPD_Search";
                    _DomesticPackingList_Model.FromDate = Date;
                    // _DomesticPackingList_Model.cust_id = CustID;
                    _DomesticPackingList_Model.customerID = CustID;
                    _DomesticPackingList_Model.Status = Status;
                    _DomesticPackingList_Model.ToDate = tdt;
                    _DomesticPackingList_Model.Flag = "Filter";
                    _DomesticPackingList_Model.ListFilterData = TempData["ListFilterData"].ToString();

                }
                else
                {
                    //DateTime dt2 = DateTime.Now;
                    //DateTime dtBegin = dt2.AddDays(-(dt2.Day - 1));

                    var range = CommonController.Comman_GetFutureDateRange();
                    string dtBegin = range.FromDate;
                    string CurrentDate = range.ToDate;

                    //_DomesticPackingList_Model.FromDate = dtBegin.ToString("yyyy-MM-dd");
                    _DomesticPackingList_Model.FromDate = dtBegin;
                    _DomesticPackingList_Model.ToDate = CurrentDate;

                    _DomesticPackingList_Model.Flag = "NotFilter";
                    //_DomesticPackingList_Model.PackingListDetail = getPackinlistDetails(_DomesticPackingList_Model);
                }
                GetAllData(_DomesticPackingList_Model, cus_typ);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _DomesticPackingList_Model.Title = title;
                _DomesticPackingList_Model.DPDSearch = "0";
                //Session["DPDSearch"] = "0";
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingList.cshtml", _DomesticPackingList_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        #region Added By (GetAllData) Nitesh 05-04-2024 for All DropDown And List In One Procedure
        #endregion
        private void GetAllData(DomesticPackingList_Model _DomesticPackingList_Model, string cus_typ)
        {
            try
            {
                string CustomerName = string.Empty;
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_DomesticPackingList_Model.cust_id) || _DomesticPackingList_Model.cust_id == "0")
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _DomesticPackingList_Model.cust_id;
                }
                string UserID = "";
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string wfstatus = "";
                if (_DomesticPackingList_Model.WF_Status != null)
                {
                    wfstatus = _DomesticPackingList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                DataSet AllData = _DomesticPacking_IServices.GetAllData(CompID, BrchID, CustomerName, cus_typ, _DomesticPackingList_Model.customerID,
                    _DomesticPackingList_Model.FromDate, _DomesticPackingList_Model.ToDate, _DomesticPackingList_Model.Status, DocumentMenuId, UserID, wfstatus);
                List<ListCustomerName> CustLists = new List<ListCustomerName>();
                foreach (DataRow dr in AllData.Tables[0].Rows)
                {
                    ListCustomerName _RAList = new ListCustomerName();
                    _RAList.cust_id = dr["cust_id"].ToString();
                    _RAList.cust_name = dr["cust_name"].ToString();
                    CustLists.Add(_RAList);
                }
                CustLists.Insert(0, new ListCustomerName() { cust_id = "0", cust_name = "---Select---" });
                _DomesticPackingList_Model.CustomerNameList = CustLists;
                if (_DomesticPackingList_Model.customerID != null && _DomesticPackingList_Model.customerID != "")
                    _DomesticPackingList_Model.cust_id = _DomesticPackingList_Model.customerID;
                SetAllDataInListTable(_DomesticPackingList_Model, AllData);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        #region Added By (SetAllDataInListTable) Nitesh 05-04-2024 Set Data  In List Model
        #endregion
        private void SetAllDataInListTable(DomesticPackingList_Model _DomesticPackingList_Model, DataSet dt)
        {
            DataTable TableData = new DataTable();
            List<PackingList> _PackingList = new List<PackingList>();
            /**
             Flag Use = When Data is search Use Flag= Filter(Deatil Back List Page) And NotFilter is use normally
             **/
            if (_DomesticPackingList_Model.Flag == "NotFilter" && _DomesticPackingList_Model.Flag != "Filter")
            {
                TableData = dt.Tables[1];
            }
            else
            {
                TableData = dt.Tables[3];
            }

            if (TableData.Rows.Count > 0)
            {

                foreach (DataRow dr in TableData.Rows)
                {
                    PackingList _PackList = new PackingList();
                    _PackList.PackStatus = dr["pack_status"].ToString();
                    _PackList.pack_dt = dr["pack_dt"].ToString();
                    _PackList.PackingListNO = dr["pack_no"].ToString();
                    _PackList.CustomerName = dr["cust_name"].ToString();
                    _PackList.CreatedON = dr["create_dt"].ToString();
                    _PackList.ApprovedOn = dr["app_dt"].ToString();
                    _PackList.pack_type = dr["pack_type"].ToString();
                    _PackList.ModifiedOn = dr["mod_dt"].ToString();
                    _PackList.packing_date = dr["packing_date"].ToString();
                    _PackList.create_by = dr["create_by"].ToString();
                    _PackList.app_by = dr["app_by"].ToString();
                    _PackList.mod_by = dr["mod_by"].ToString();
                    _PackingList.Add(_PackList);
                }
            }
            _DomesticPackingList_Model.PackingListDetail = _PackingList;

            if (dt.Tables[2].Rows.Count > 0)
            {
                FromDate = dt.Tables[2].Rows[0]["finstrdate"].ToString();
            }
        }
        public ActionResult ExportPackingList(DomesticPackingList_Model _ExportPackingList_Model)
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
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //    } 
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                DocumentMenuId = "105103145115";
                //    }
                //}
                CommonPageDetails();
                //DomesticPackingList_Model _ExportPackingList_Model = new DomesticPackingList_Model();
                _ExportPackingList_Model.DocumentMenuId = DocumentMenuId;
                GetStatusList(_ExportPackingList_Model);
                //List<ListCustomerName> _CustomerNameList = new List<ListCustomerName>();
                //ListCustomerName _ListCustomerName = new ListCustomerName();
                //_ListCustomerName.cust_name = "---Select---";
                //_ListCustomerName.cust_id = "0";
                //_CustomerNameList.Add(_ListCustomerName);
                //_ExportPackingList_Model.CustomerNameList = _CustomerNameList;
                DataTable dt = new DataTable();
                List<ListCustomerName> CustLists = new List<ListCustomerName>();
                var cus_typ = "E";
                #region Commented this customer Data Bind BY Nitesh 05-04-2024 For All Data is One Procedure
                #endregion
                //dt = GetCustNameList(_ExportPackingList_Model, cus_typ);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    ListCustomerName _RAList = new ListCustomerName();
                //    _RAList.cust_id = dr["cust_id"].ToString();
                //    _RAList.cust_name = dr["cust_name"].ToString();
                //    CustLists.Add(_RAList);
                //}
                //CustLists.Insert(0, new ListCustomerName() { cust_id = "0", cust_name = "---Select---" });
                //_ExportPackingList_Model.CustomerNameList = CustLists;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    var CustID = a[0].Trim();
                    var Date = a[1].Trim();
                    var tdt = a[2].Trim();
                    DateTime Fromdate = Convert.ToDateTime(a[1].Trim());
                    DateTime Todate = Convert.ToDateTime(a[2].Trim());
                    var Status = a[3].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    #region Commented this table Data Bind BY Nitesh 05-04-2024 For All Data is One Procedure
                    #endregion
                    //List<PackingList> _PackingList = new List<PackingList>(); 
                    //dt1 = _DomesticPacking_IServices.GetPackingListFilter(CustID, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                    //Session["DPDSearch"] = "DPD_Search";
                    _ExportPackingList_Model.FromDate = Date;
                    //_ExportPackingList_Model.cust_id = CustID;
                    _ExportPackingList_Model.customerID = CustID;
                    _ExportPackingList_Model.Status = Status;
                    _ExportPackingList_Model.ToDate = tdt;
                    _ExportPackingList_Model.Flag = "Filter";
                    _ExportPackingList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    #region Commented this table Data Bind BY Nitesh 05-04-2024 For All Data is One Procedure
                    #endregion
                    //foreach (DataRow dr in dt1.Rows)
                    //{
                    //    PackingList _PackList = new PackingList();
                    //    _PackList.PackStatus = dr["pack_status"].ToString();
                    //    _PackList.pack_dt = dr["pack_dt"].ToString();
                    //    //_PackList.SO_Number = dr["so_no"].ToString();
                    //    _PackList.PackingListNO = dr["pack_no"].ToString();
                    //    //_PackList.SO_DATE = dr["so_dt"].ToString();
                    //    _PackList.CustomerName = dr["cust_name"].ToString();
                    //    _PackList.CreatedON = dr["create_dt"].ToString();
                    //    _PackList.ApprovedOn = dr["app_dt"].ToString();
                    //    _PackList.pack_type = dr["pack_type"].ToString();
                    //    _PackList.ModifiedOn = dr["mod_dt"].ToString();
                    //    _PackList.packing_date = dr["packing_date"].ToString();
                    //    _PackList.create_by = dr["create_by"].ToString();
                    //    _PackList.app_by = dr["app_by"].ToString();
                    //    _PackList.mod_by = dr["mod_by"].ToString();
                    //    _PackingList.Add(_PackList);
                    //}
                    //_ExportPackingList_Model.PackingListDetail = _PackingList;
                }
                else
                {
                    //DateTime dt2 = DateTime.Now;
                    //DateTime dtBegin = dt2.AddDays(-(dt2.Day - 1));
                    //_ExportPackingList_Model.FromDate = dtBegin.ToString("yyyy-MM-dd");

                    var range = CommonController.Comman_GetFutureDateRange();
                    string dtBegin = range.FromDate;
                    string CurrentDate = range.ToDate;


                    _ExportPackingList_Model.FromDate = dtBegin;
                    _ExportPackingList_Model.ToDate = CurrentDate;

                    //_ExportPackingList_Model.ToDate = dt.ToString();
                    _ExportPackingList_Model.Flag = "NotFilter";
                    //  _ExportPackingList_Model.PackingListDetail = getPackinlistDetails(_ExportPackingList_Model);
                }
                #region Added this Function (GetAllData ) By Nitesh 05-04-2024 for All Data is one procedure
                #endregion
                GetAllData(_ExportPackingList_Model, cus_typ);

                ViewBag.DocumentMenuId = DocumentMenuId;
                _ExportPackingList_Model.Title = title;
                //Session["DPDSearch"] = "0";
                _ExportPackingList_Model.DPDSearch = "0";
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingList.cshtml", _ExportPackingList_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataTable GetCustNameList(DomesticPackingList_Model _DomesticPackingList_Model, string cus_typ)
        {
            try
            {
                string CustomerName = string.Empty;
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_DomesticPackingList_Model.cust_id))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _DomesticPackingList_Model.cust_id;
                }
                DataTable dt = _DomesticPacking_IServices.GetCustNameList(CompID, BrchID, CustomerName, cus_typ, _DomesticPackingList_Model.DocumentMenuId);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
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
        public void GetStatusList(DomesticPackingList_Model _DomesticPackingList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                //var other = new CommonController(_Common_IServices);
                //var statusListsC = other.GetStatusList1(DocumentMenuId);
                //var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _DomesticPackingList_Model.StatusList = statusLists;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        public ActionResult AddNewDomesticPacking(string DocumentMenuId)
        {
            //Session["Message"] = null;
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnAddNew";
            //Session["TransType"] = "Save";
            //Session["Command"] = "New";
            DomesticPackingDetail_Model AddNew_Model = new DomesticPackingDetail_Model();
            AddNew_Model.Command = "New";
            AddNew_Model.AppStatus = "D";
            AddNew_Model.TransType = "Save";
            AddNew_Model.BtnName = "BtnAddNew";
            AddNew_Model.DocumentMenuId = DocumentMenuId;
            TempData["ModelData"] = AddNew_Model;
            UrlModel AddNewModel = new UrlModel();
            AddNewModel.bt = "BtnAddNew";
            AddNewModel.Docid = DocumentMenuId;
            AddNewModel.Cmd = "Add";
            AddNewModel.tp = "Save";
            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                if (DocumentMenuId == "105103130")
                {
                    return RedirectToAction("DomesticPackingList");
                }
                else
                {
                    return RedirectToAction("ExportPackingList");
                }

            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("DomesticPackingDetail", AddNewModel);
        }
        public ActionResult EditDomesticPacking(string PackigListNumber,string PackigListDate, string ListFilterData, string WF_Status, string Docid)
        {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
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
            //Session["Command"] = "Update";
            //Session["PackigListNumber"] = PackigListNumber;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnEdit";
            DomesticPackingDetail_Model dblclick = new DomesticPackingDetail_Model();
            UrlModel _dbl_click = new UrlModel();
            dblclick.Packing_No = PackigListNumber;
            //dblclick.pack_dt =Convert.ToDateTime(PackigListDate);
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnEdit";
            dblclick.Command = "Update";
            dblclick.DocumentMenuId = Docid;
            if (WF_Status != null && WF_Status != "")
            {
                _dbl_click.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            _dbl_click.tp = "Update";
            _dbl_click.Docid = Docid;
            _dbl_click.bt = "BtnEdit";
            _dbl_click.PAC_No = PackigListNumber;
            _dbl_click.PAC_Dt = PackigListDate;
            _dbl_click.Cmd = "Update";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("DomesticPackingDetail", "DomesticPacking", _dbl_click);
        }
        public ActionResult DomesticPackingDetail(UrlModel _urlModel)
        {
            try
            {
                DomesticPackingDetail_Model _DomesticPackingDetail_Model = new DomesticPackingDetail_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string UserID = "";
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (_urlModel.Docid != null)
                {
                    DocumentMenuId = _urlModel.Docid;

                }
                /*Add by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.PAC_Dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                //if(_DomesticPackingDetail_Model.pack_no==null)
                //if (TempData["Message"] == "Financial Year not Exist")
                //{
                //    DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                //}
                DataTable dtCurr = new DataTable();
                CommonPageDetails();
                var _DomesticPackingDetail_Model1 = TempData["ModelData"] as DomesticPackingDetail_Model;
                if (_DomesticPackingDetail_Model1 != null)
                {
                    _DomesticPackingDetail_Model1.MenuDocumentId = _DomesticPackingDetail_Model1.DocumentMenuId;

                    GetAutoCompleteCustomerName(_DomesticPackingDetail_Model1);
                    List<CurrList> CurrList = new List<CurrList>();
                    if (DocumentMenuId == "105103130")
                    {
                        dtCurr = Getcurr("D");
                    }
                    else
                    {
                        dtCurr = Getcurr(null);
                        CurrList.Add(new CurrList() { curr_id = "0", curr_nm = "---Select---" });
                    }
                    foreach (DataRow dr in dtCurr.Rows)
                    {
                        CurrList.Add(new CurrList() { curr_id = dr["curr_id"].ToString(), curr_nm = dr["curr_name"].ToString() });
                    }
                    _DomesticPackingDetail_Model1.currLists = CurrList;

                    List<CustomerName> _CustomerNameList = new List<CustomerName>();
                    CustomerName _CustomerName = new CustomerName();
                    _CustomerName.cust_name = "---Select---";
                    _CustomerName.cust_id = "0";
                    _CustomerNameList.Add(_CustomerName);
                    _DomesticPackingDetail_Model1.CustomerNameList = _CustomerNameList;
                    List<OrderNumber> _OrderNumberList = new List<OrderNumber>();
                    OrderNumber _OrderNumber = new OrderNumber();
                    _OrderNumber.so_no = "---Select---";
                    _OrderNumber.so_dt = "0";
                    _OrderNumberList.Add(_OrderNumber);
                    _DomesticPackingDetail_Model1.OrderNumberList = _OrderNumberList;
                    _DomesticPackingDetail_Model1.pack_dt = DateTime.Now;
                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    if (_DomesticPackingDetail_Model1.MenuDocumentId == "105103145115")
                    {
                        _DomesticPackingDetail_Model1.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                        _DomesticPackingDetail_Model1.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"]));
                    }
                    else
                    {
                        _DomesticPackingDetail_Model1.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                        _DomesticPackingDetail_Model1.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                    }
                    _DomesticPackingDetail_Model1.WgtDigit = ToFixDecimal(Convert.ToInt32(Session["WeightDigit"]));

                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DomesticPackingDetail_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_DomesticPackingDetail_Model1.TransType == "Update")
                    {
                        //string pack_no = Session["PackigListNumber"].ToString();
                        string pack_no = _DomesticPackingDetail_Model1.Packing_No;
                        DataSet ds = _DomesticPacking_IServices.GetPackingListDetailByNo(CompID, pack_no, BrchID, UserID, DocumentMenuId);
                        _DomesticPackingDetail_Model1.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _DomesticPackingDetail_Model1.pack_type = ds.Tables[0].Rows[0]["pack_type"].ToString();
                        _DomesticPackingDetail_Model1.PL_SerialiizationDT = ds.Tables[9];
                        if (ds.Tables[9].Rows.Count == 0)
                        {
                            ViewBag.Message = "PLSerialiizationDTValidate";
                        }
                        GetAutoCompleteCustomerName(_DomesticPackingDetail_Model1);
                        _DomesticPackingDetail_Model1.filterCustomerName = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        //GetPakingListSONO(_DomesticPackingDetail_Model1);
                        _DomesticPackingDetail_Model1.pack_no = ds.Tables[0].Rows[0]["pack_no"].ToString();
                        _DomesticPackingDetail_Model1.pack_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["pack_dt"].ToString());
                        _DomesticPackingDetail_Model1.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _DomesticPackingDetail_Model1.CustomerID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _DomesticPackingDetail_Model1.hdnItemorientation = ds.Tables[0].Rows[0]["Item_Orien"].ToString();
                        _DomesticPackingDetail_Model1.Itemorientation = ds.Tables[0].Rows[0]["Item_Orien"].ToString();
                       
                        _DomesticPackingDetail_Model1.TotalGrossWgt = ds.Tables[0].Rows[0]["tot_gr_wght"].ToString();
                        _DomesticPackingDetail_Model1.TotalNetWgt = ds.Tables[0].Rows[0]["tot_net_wght"].ToString();
                        _DomesticPackingDetail_Model1.TotalCBM = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_cbm"]).ToString(_DomesticPackingDetail_Model.QtyDigit);

                        _DomesticPackingDetail_Model1.pack_remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        _DomesticPackingDetail_Model1.pack_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticPackingDetail_Model1.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _DomesticPackingDetail_Model1.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _DomesticPackingDetail_Model1.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _DomesticPackingDetail_Model1.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _DomesticPackingDetail_Model1.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _DomesticPackingDetail_Model1.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _DomesticPackingDetail_Model1.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticPackingDetail_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                        _DomesticPackingDetail_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        string StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        _DomesticPackingDetail_Model1.create_id = create_id;
                        _DomesticPackingDetail_Model1.StatusCode = StatusCode;
                        if (_DomesticPackingDetail_Model1.Status == "Cancelled")
                        {
                            _DomesticPackingDetail_Model1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _DomesticPackingDetail_Model1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _DomesticPackingDetail_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            _DomesticPackingDetail_Model1.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _DomesticPackingDetail_Model1.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[6].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[7].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DomesticPackingDetail_Model1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnEdit";
                                        _DomesticPackingDetail_Model1.BtnName = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _DomesticPackingDetail_Model1.BtnName = "BtnEdit";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model1.BtnName = "BtnEdit";
                                }

                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _DomesticPackingDetail_Model1.BtnName = "BtnEdit";
                                    }
                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model1.BtnName = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model1.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model1.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DomesticPackingDetail_Model1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        getWarehouse(_DomesticPackingDetail_Model1);
                        //ViewBag.MenuPageName = getDocumentName();
                        _DomesticPackingDetail_Model1.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemOrderQtyDetail = ds.Tables[2];
                        ViewBag.ItemStockBatchWise = ds.Tables[3];
                        ViewBag.OrderReservedItemStockBatchWise = ds.Tables[4];
                        ViewBag.ItemStockSerialWise = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[10];
                        ViewBag.SubItemResDetails = ds.Tables[11];
                        ViewBag.SubItemPackResDetails = ds.Tables[12];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticPackingDetail_Model1.DocumentStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        //ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocID = DocumentMenuId;
                        _DomesticPackingDetail_Model1.Qty_pari_Command = _DomesticPackingDetail_Model1.Command;
                        _DomesticPackingDetail_Model1.Qty_TransType = _DomesticPackingDetail_Model1.TransType;
                        ViewBag.TransType = _DomesticPackingDetail_Model1.TransType;
                        ViewBag.Command = _DomesticPackingDetail_Model1.Command;
                        _DomesticPackingDetail_Model1.UserID = UserID;
                        if (_DomesticPackingDetail_Model1.Amend == "Amend")
                        {
                            ViewBag.DocumentCode = "D";
                            _DomesticPackingDetail_Model1.DocumentStatus = "D";
                            ViewBag.DocumentStatus = "D";
                        }

                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingDetail.cshtml", _DomesticPackingDetail_Model1);
                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                        //ViewBag.MenuPageName = getDocumentName();
                        _DomesticPackingDetail_Model1.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _DomesticPackingDetail_Model1.DocumentStatus = "New";
                        //ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocID = DocumentMenuId;
                        _DomesticPackingDetail_Model1.Qty_pari_Command = _DomesticPackingDetail_Model1.Command;
                        //_DomesticPackingDetail_Model1.Qty_TransType = _DomesticPackingDetail_Model1.TransType;
                        ViewBag.TransType = _DomesticPackingDetail_Model1.TransType;
                        ViewBag.Command = _DomesticPackingDetail_Model1.Command;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingDetail.cshtml", _DomesticPackingDetail_Model1);
                    }
                }
                else
                {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
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
                    if (_urlModel != null)
                    {
                        _DomesticPackingDetail_Model.Command = _urlModel.Cmd;
                        _DomesticPackingDetail_Model.Packing_No = _urlModel.PAC_No;
                        //_DomesticPackingDetail_Model.pack_dt = Convert.ToDateTime(_urlModel.PAC_Dt);
                        _DomesticPackingDetail_Model.TransType = _urlModel.tp;
                        _DomesticPackingDetail_Model.BtnName = _urlModel.bt;
                        _DomesticPackingDetail_Model.WF_Status1 = _urlModel.wf;
                        _DomesticPackingDetail_Model.DocumentStatus = _urlModel.DMS;
                        _DomesticPackingDetail_Model.MenuDocumentId = _urlModel.Docid;
                        _DomesticPackingDetail_Model.DocumentMenuId = _urlModel.Docid;
                        _DomesticPackingDetail_Model.Amend = _urlModel.Amend;
                        _DomesticPackingDetail_Model.CmdType = _urlModel.CmdType;
                    }
                    GetAutoCompleteCustomerName(_DomesticPackingDetail_Model);

                    List<CurrList> CurrList = new List<CurrList>();
                    if (_DomesticPackingDetail_Model.DocumentMenuId == "105103130")
                    {
                        dtCurr = Getcurr("D");
                    }
                    else
                    {
                        dtCurr = Getcurr(null);
                        CurrList.Add(new CurrList() { curr_id = "0", curr_nm = "---Select---" });
                    }
                    foreach (DataRow dr in dtCurr.Rows)
                    {
                        CurrList.Add(new CurrList() { curr_id = dr["curr_id"].ToString(), curr_nm = dr["curr_name"].ToString() });
                    }
                    _DomesticPackingDetail_Model.currLists = CurrList;
                    List<CustomerName> _CustomerNameList = new List<CustomerName>();
                    CustomerName _CustomerName = new CustomerName();
                    _CustomerName.cust_name = "---Select---";
                    _CustomerName.cust_id = "0";
                    _CustomerNameList.Add(_CustomerName);
                    _DomesticPackingDetail_Model.CustomerNameList = _CustomerNameList;
                    List<OrderNumber> _OrderNumberList = new List<OrderNumber>();
                    OrderNumber _OrderNumber = new OrderNumber();
                    _OrderNumber.so_no = "---Select---";
                    _OrderNumber.so_dt = "0";
                    _OrderNumberList.Add(_OrderNumber);
                    _DomesticPackingDetail_Model.OrderNumberList = _OrderNumberList;
                    _DomesticPackingDetail_Model.pack_dt = DateTime.Now;
                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    if (_DomesticPackingDetail_Model.MenuDocumentId == "105103145115")
                    {
                        _DomesticPackingDetail_Model.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                        _DomesticPackingDetail_Model.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"]));
                    }
                    else
                    {
                        _DomesticPackingDetail_Model.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                        _DomesticPackingDetail_Model.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                    }
                    //_DomesticPackingDetail_Model.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                    //_DomesticPackingDetail_Model.ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                    _DomesticPackingDetail_Model.WgtDigit = ToFixDecimal(Convert.ToInt32(Session["WeightDigit"]));
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DomesticPackingDetail_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_DomesticPackingDetail_Model.TransType == "Update")
                    {
                        //string pack_no = Session["PackigListNumber"].ToString();
                        string pack_no = _DomesticPackingDetail_Model.Packing_No;
                        DataSet ds = _DomesticPacking_IServices.GetPackingListDetailByNo(CompID, pack_no, BrchID, UserID, DocumentMenuId);
                        _DomesticPackingDetail_Model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _DomesticPackingDetail_Model.pack_type = ds.Tables[0].Rows[0]["pack_type"].ToString();
                        _DomesticPackingDetail_Model.hdnItemorientation = ds.Tables[0].Rows[0]["Item_Orien"].ToString();
                        _DomesticPackingDetail_Model.Itemorientation = ds.Tables[0].Rows[0]["Item_Orien"].ToString();
                       
                        _DomesticPackingDetail_Model.PL_SerialiizationDT = ds.Tables[9];
                        if (ds.Tables[9].Rows.Count == 0)
                        {
                            ViewBag.Message = "PLSerialiizationDTValidate";
                        }
                        GetAutoCompleteCustomerName(_DomesticPackingDetail_Model);
                        _DomesticPackingDetail_Model.filterCustomerName = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        //GetPakingListSONO(_DomesticPackingDetail_Model);
                        _DomesticPackingDetail_Model.pack_no = ds.Tables[0].Rows[0]["pack_no"].ToString();
                        _DomesticPackingDetail_Model.pack_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["pack_dt"].ToString());
                        _DomesticPackingDetail_Model.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _DomesticPackingDetail_Model.CustomerID = ds.Tables[0].Rows[0]["cust_id"].ToString();

                        _DomesticPackingDetail_Model.TotalGrossWgt = ds.Tables[0].Rows[0]["tot_gr_wght"].ToString();
                        _DomesticPackingDetail_Model.TotalNetWgt = ds.Tables[0].Rows[0]["tot_net_wght"].ToString();
                        _DomesticPackingDetail_Model.TotalCBM = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_cbm"]).ToString(_DomesticPackingDetail_Model.QtyDigit);

                        _DomesticPackingDetail_Model.pack_remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        _DomesticPackingDetail_Model.pack_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticPackingDetail_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _DomesticPackingDetail_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _DomesticPackingDetail_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _DomesticPackingDetail_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _DomesticPackingDetail_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _DomesticPackingDetail_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _DomesticPackingDetail_Model.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticPackingDetail_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                        _DomesticPackingDetail_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        string StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        _DomesticPackingDetail_Model.create_id = create_id;
                        _DomesticPackingDetail_Model.StatusCode = StatusCode;
                        if (_DomesticPackingDetail_Model.Status == "Cancelled")
                        {
                            _DomesticPackingDetail_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _DomesticPackingDetail_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _DomesticPackingDetail_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _DomesticPackingDetail_Model.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _DomesticPackingDetail_Model.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[6].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[7].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DomesticPackingDetail_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnEdit";
                                        _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                                }

                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnEdit";
                                        _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                                    }
                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnEdit";
                                    _DomesticPackingDetail_Model.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DomesticPackingDetail_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        getWarehouse(_DomesticPackingDetail_Model);
                        //ViewBag.MenuPageName = getDocumentName();
                        _DomesticPackingDetail_Model.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemOrderQtyDetail = ds.Tables[2];
                        ViewBag.ItemStockBatchWise = ds.Tables[3];
                        ViewBag.OrderReservedItemStockBatchWise = ds.Tables[4];
                        ViewBag.ItemStockSerialWise = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[10];
                        ViewBag.SubItemResDetails = ds.Tables[11];
                        ViewBag.SubItemPackResDetails = ds.Tables[12];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticPackingDetail_Model.DocumentStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        //ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocID = DocumentMenuId;
                        _DomesticPackingDetail_Model.Qty_pari_Command = _DomesticPackingDetail_Model.Command;
                        _DomesticPackingDetail_Model.Qty_TransType = _DomesticPackingDetail_Model.TransType;
                        ViewBag.TransType = _DomesticPackingDetail_Model.TransType;
                        ViewBag.Command = _DomesticPackingDetail_Model.Command;
                        _DomesticPackingDetail_Model.UserID = UserID;
                        if (_DomesticPackingDetail_Model.Amend == "Amend")
                        {
                            ViewBag.DocumentCode = "D";
                            _DomesticPackingDetail_Model.DocumentStatus = "D";
                            ViewBag.DocumentStatus = "D";
                        }
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingDetail.cshtml", _DomesticPackingDetail_Model);
                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                        //ViewBag.MenuPageName = getDocumentName();
                        _DomesticPackingDetail_Model.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _DomesticPackingDetail_Model.DocumentStatus = "New";
                        //ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocID = DocumentMenuId;
                        _DomesticPackingDetail_Model.Qty_pari_Command = _DomesticPackingDetail_Model.Command;
                        _DomesticPackingDetail_Model.Qty_TransType = _DomesticPackingDetail_Model.TransType;
                        ViewBag.TransType = _DomesticPackingDetail_Model.TransType;
                        ViewBag.Command = _DomesticPackingDetail_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingDetail.cshtml", _DomesticPackingDetail_Model);
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
            //if (Session["MenuDocumentId"] != null)
            //{
            //    if (Session["MenuDocumentId"].ToString() == "105103125")
            //    {
            //        DocumentMenuId = "105103125";
            //    }
            //    if (Session["MenuDocumentId"].ToString() == "105103145110")
            //    {
            //        DocumentMenuId = "105103145110";
            //    }
            //}

            DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, userid, DocumentMenuId, language);
            ViewBag.AppLevel = ds.Tables[0];
            ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
            string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
            ViewBag.VBRoleList = ds.Tables[3];
            ViewBag.StatusList = ds.Tables[4];
            ViewBag.PackSerialization = ds.Tables[6].Rows.Count > 0 ? ds.Tables[6].Rows[0]["param_stat"].ToString() : "";
            string[] Docpart = DocumentName.Split('>');
            int len = Docpart.Length;
            if (len > 1)
            {
                title = Docpart[len - 1].Trim();
            }
            ViewBag.MenuPageName = DocumentName;
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
        private DataTable Getcurr(string CurrType)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _DomesticPacking_IServices.GetCurrencies(Comp_ID, CurrType);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType,string Mailerror)
        {
            //Session["Message"] = "";
            DomesticPackingDetail_Model ToRefreshByJS = new DomesticPackingDetail_Model();
            UrlModel ToRefreshByJS_Model = new UrlModel();
            var a = TrancType.Split(',');
            ToRefreshByJS.Packing_No = a[0].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            ToRefreshByJS.Message = Mailerror;
            ToRefreshByJS.DocumentMenuId = a[1].Trim();
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                ToRefreshByJS_Model.wf = a[2].Trim();
            }
            ToRefreshByJS_Model.bt = "BtnEdit";
            ToRefreshByJS_Model.Docid = ToRefreshByJS.DocumentMenuId;
            ToRefreshByJS_Model.PAC_No = ToRefreshByJS.Packing_No;
            ToRefreshByJS_Model.PAC_Dt = ToRefreshByJS.pack_dt.ToString("yyyy-MM-dd");
            ToRefreshByJS_Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("DomesticPackingDetail", ToRefreshByJS_Model);
        }
        public ActionResult GetDomesticPackingList(string docid, string status)
        {
            //Session["MenuDocumentId"] = docid;
            //Session["WF_status"] = status;
            DomesticPackingList_Model dashbordList_Model = new DomesticPackingList_Model();
            dashbordList_Model.DocumentMenuId = docid;
            dashbordList_Model.WF_Status = status;
            if (dashbordList_Model.DocumentMenuId == "105103130")
            {
                return RedirectToAction("DomesticPackingList", dashbordList_Model);
            }
            else if (dashbordList_Model.DocumentMenuId == "105103145115")
            {
                return RedirectToAction("ExportPackingList", dashbordList_Model);
            }
            else
            {
                return RedirectToAction("DomesticPackingList");
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
                if (Session["MenuDocumentId"] != null)
                {
                    if (Session["MenuDocumentId"].ToString() == "105103130")
                    {
                        DocumentMenuId = "105103130";
                    }
                    if (Session["MenuDocumentId"].ToString() == "105103145115")
                    {
                        DocumentMenuId = "105103145115";
                    }
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
        [HttpPost]
        public ActionResult PackingListSave(DomesticPackingDetail_Model _DomesticPackingDetail_Model, string dn_no, string command)
        {
            try
            { /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_DomesticPackingDetail_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNewDeliveryNote":
                        DomesticPackingDetail_Model adddnew = new DomesticPackingDetail_Model();
                        adddnew.Command = "Add";
                        adddnew.TransType = "Save";
                        adddnew.BtnName = "BtnAddNew";
                        adddnew.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                        TempData["ModelData"] = adddnew;
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "Add";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";
                        NewModel.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_DomesticPackingDetail_Model.pack_no))
                                return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate=_DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                            else
                                adddnew.Command = "Refresh";
                            adddnew.TransType = "Refresh";
                            adddnew.BtnName = "Refresh";
                            adddnew.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                            adddnew.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                            TempData["ModelData"] = adddnew;
                            return RedirectToAction("DomesticPackingDetail", adddnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("DomesticPackingDetail", NewModel);
                    case "Edit":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate = _DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string PKDate = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PKDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate = _DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                        }

                        /*End to chk Financial year exist or not*/
                        if (CheckShipmentAgainstPackingList(_DomesticPackingDetail_Model.pack_no, _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd")) == "Used")
                        {
                            _DomesticPackingDetail_Model.Command = "Refresh";
                            _DomesticPackingDetail_Model.Message = "Used";
                            _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                            _DomesticPackingDetail_Model.TransType = "Update";
                            _DomesticPackingDetail_Model.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                            _DomesticPackingDetail_Model.Packing_No = _DomesticPackingDetail_Model.pack_no;
                            
                            TempData["ModelData"] = _DomesticPackingDetail_Model;
                            UrlModel Used_model = new UrlModel();
                            Used_model.Cmd = "Refresh";
                            Used_model.tp = "Update";
                            Used_model.bt = "BtnEdit";
                            Used_model.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                            Used_model.PAC_No = _DomesticPackingDetail_Model.pack_no;
                            Used_model.PAC_Dt = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                            TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                            return RedirectToAction("DomesticPackingDetail", Used_model);
                        }
                        else
                        {
                            _DomesticPackingDetail_Model.Amend = "";
                            _DomesticPackingDetail_Model.Command = command;
                            _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                            _DomesticPackingDetail_Model.TransType = "Update";
                            _DomesticPackingDetail_Model.CmdType = "Edit";
                            _DomesticPackingDetail_Model.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                            _DomesticPackingDetail_Model.Packing_No = _DomesticPackingDetail_Model.pack_no;
                            UrlModel EditModel = new UrlModel();
                            EditModel.Cmd = command;
                            EditModel.tp = "Update";
                            EditModel.bt = "BtnEdit";
                            EditModel.CmdType = "Edit";
                            EditModel.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                            EditModel.PAC_No = _DomesticPackingDetail_Model.pack_no;
                            EditModel.PAC_Dt = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                            TempData["ModelData"] = _DomesticPackingDetail_Model;
                            TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                            return RedirectToAction("DomesticPackingDetail", EditModel);
                        }
                    case "Delete":
                        pack_no = _DomesticPackingDetail_Model.pack_no;
                        PackingListDelete(_DomesticPackingDetail_Model, command);
                        DomesticPackingDetail_Model DeleteModel = new DomesticPackingDetail_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnDelete";
                        Delete_Model.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                        TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                        return RedirectToAction("DomesticPackingDetail", Delete_Model);
                    case "Save":
                        if (ModelState.IsValid)
                        {
                            SaveUpdatePackingList(_DomesticPackingDetail_Model);
                            if (_DomesticPackingDetail_Model.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            if (_DomesticPackingDetail_Model.Message == "DocModify")
                            {
                                DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                                CommonPageDetails();
                                ViewBag.DocumentMenuId = DocumentMenuId;
                                ViewBag.DocumentStatus = "D";

                                List<CustomerName> _CustomerNameList = new List<CustomerName>();
                                CustomerName _CustomerName = new CustomerName();
                                _CustomerName.cust_name = _DomesticPackingDetail_Model.CustomerName;
                                _CustomerName.cust_id = "0";
                                _CustomerNameList.Add(_CustomerName);
                                _DomesticPackingDetail_Model.CustomerNameList = _CustomerNameList;

                                List<CurrList> CurrList = new List<CurrList>();
                                DataTable dtCurr = new DataTable();
                                if (DocumentMenuId == "105103130")
                                {
                                    dtCurr = Getcurr("D");
                                }
                                else
                                {
                                    dtCurr = Getcurr(null);
                                    CurrList.Add(new CurrList() { curr_id = "0", curr_nm = "---Select---" });
                                }
                                foreach (DataRow dr in dtCurr.Rows)
                                {
                                    CurrList.Add(new CurrList() { curr_id = dr["curr_id"].ToString(), curr_nm = dr["curr_name"].ToString() });
                                }
                                _DomesticPackingDetail_Model.currLists = CurrList;

                                List<OrderNumber> _OrderNumberList = new List<OrderNumber>();
                                OrderNumber _OrderNumber = new OrderNumber();
                                _OrderNumber.so_no = "---Select---";
                                _OrderNumber.so_dt = "0";
                                _OrderNumberList.Add(_OrderNumber);
                                _DomesticPackingDetail_Model.OrderNumberList = _OrderNumberList;
                                _DomesticPackingDetail_Model.CustomerName = _DomesticPackingDetail_Model.CustomerName;

                                getWarehouse(_DomesticPackingDetail_Model);
                                _DomesticPackingDetail_Model.pack_dt = _DomesticPackingDetail_Model.pack_dt;
                                ViewBag.ItemDetails = ViewData["ItemDetails"];
                                ViewBag.ItemStockBatchWise = ViewData["BatchDetails"];
                                ViewBag.OrderReservedItemStockBatchWise = ViewData["ResBatchDetails"];
                                ViewBag.ItemStockSerialWise = ViewData["SerialDetails"];
                                ViewBag.ItemOrderQtyDetail = ViewData["OrderDetails"];
                                ViewBag.SubItemDetails = ViewData["SubitemDetails"];
                                ViewBag.SubItemResDetails = ViewData["SubitemResDetails"];
                                ViewBag.SubItemPackResDetails = ViewData["SubitemPackResDetails"];

                                _DomesticPackingDetail_Model.BtnName = "Refresh";
                                _DomesticPackingDetail_Model.Command = "Refresh";
                                _DomesticPackingDetail_Model.DocumentStatus = "D";

                                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                                ViewBag.ValDigit = ValDigit;
                                ViewBag.QtyDigit = QtyDigit;
                                ViewBag.RateDigit = RateDigit;
                                _DomesticPackingDetail_Model.QtyDigit = QtyDigit;
                                _DomesticPackingDetail_Model.ValDigit = ValDigit;
                                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingDetail.cshtml", _DomesticPackingDetail_Model);

                            }
                            else
                            {
                                _DomesticPackingDetail_Model.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                                //Session["PackigListNumber"] = Session["PackigListNumber"].ToString();
                                TempData["ModelData"] = _DomesticPackingDetail_Model;
                                UrlModel SaveModel = new UrlModel();
                                SaveModel.bt = _DomesticPackingDetail_Model.BtnName;
                                SaveModel.PAC_No = _DomesticPackingDetail_Model.Packing_No;
                                SaveModel.PAC_Dt = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                                SaveModel.tp = _DomesticPackingDetail_Model.TransType;
                                SaveModel.Cmd = _DomesticPackingDetail_Model.Command;
                                SaveModel.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                                TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                                return RedirectToAction("DomesticPackingDetail", SaveModel);
                            }
                        }
                        else
                        {
                            _DomesticPackingDetail_Model.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                            TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                            _DomesticPackingDetail_Model.ItemDetailsList = null;
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/DomesticPackingDetail.cshtml", _DomesticPackingDetail_Model);
                        }
                    case "Amendment":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate = _DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string PKDate1 = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PKDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate = _DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                        }
                        if (CheckShipmentAgainstPackingList(_DomesticPackingDetail_Model.pack_no, _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd")) == "Used")
                        {
                            _DomesticPackingDetail_Model.Command = "Refresh";
                            _DomesticPackingDetail_Model.Message = "Used";
                            _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                            _DomesticPackingDetail_Model.TransType = "Update";
                            _DomesticPackingDetail_Model.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                            _DomesticPackingDetail_Model.Packing_No = _DomesticPackingDetail_Model.pack_no;
                            TempData["ModelData"] = _DomesticPackingDetail_Model;
                            UrlModel Used_model = new UrlModel();
                            Used_model.Cmd = "Refresh";
                            Used_model.tp = "Update";
                            Used_model.bt = "BtnEdit";
                            Used_model.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                            Used_model.PAC_No = _DomesticPackingDetail_Model.pack_no;
                            Used_model.PAC_Dt = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                            TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                            return RedirectToAction("DomesticPackingDetail", Used_model);
                        }
                        else
                        {
                            AmmendDocument(_DomesticPackingDetail_Model);
                            /*End to chk Financial year exist or not*/
                            UrlModel URLModelAmendment = new UrlModel();
                            _DomesticPackingDetail_Model.Command = "Edit";
                            _DomesticPackingDetail_Model.TransType = "Update";
                            _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                            _DomesticPackingDetail_Model.Amend = "Amend";
                            _DomesticPackingDetail_Model.Packing_No = _DomesticPackingDetail_Model.pack_no;
                            TempData["ModelData"] = _DomesticPackingDetail_Model;
                            URLModelAmendment.Cmd = "Edit";
                            URLModelAmendment.tp = "Update";
                            URLModelAmendment.bt = "BtnEdit";
                            URLModelAmendment.DMS = "D";
                            URLModelAmendment.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                            URLModelAmendment.PAC_No = _DomesticPackingDetail_Model.pack_no;
                            URLModelAmendment.PAC_Dt = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                            //URLModelAmendment.Amend = "Amend";
                            return RedirectToAction("DomesticPackingDetail", URLModelAmendment);
                        }

                    case "Forward":
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string PKDate2 = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PKDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate = _DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                        }
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate = _DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string PKDate3 = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PKDate3) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPacking", new { PackigListNumber = _DomesticPackingDetail_Model.pack_no, PackigListDate = _DomesticPackingDetail_Model.pack_dt, ListFilterData = _DomesticPackingDetail_Model.ListFilterData1, WF_Status = _DomesticPackingDetail_Model.WFStatus, Docid = _DomesticPackingDetail_Model.DocumentMenuId, });
                        }
                        /*End to chk Financial year exist or not*/
                        pack_no = _DomesticPackingDetail_Model.pack_no;
                        PackingListApprove(_DomesticPackingDetail_Model, _DomesticPackingDetail_Model.pack_no, _DomesticPackingDetail_Model.pack_dt, "", "", "", "", "", "");
                        TempData["ModelData"] = _DomesticPackingDetail_Model;
                        UrlModel Approve_Model = new UrlModel();
                        Approve_Model.tp = "Update";
                        Approve_Model.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                        Approve_Model.PAC_No = _DomesticPackingDetail_Model.Packing_No;
                        Approve_Model.PAC_Dt = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                        Approve_Model.bt = "BtnEdit";
                        TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                        return RedirectToAction("DomesticPackingDetail", Approve_Model);

                    case "Refresh":
                        DomesticPackingDetail_Model RefreshModel = new DomesticPackingDetail_Model();
                        RefreshModel.Command = command;
                        RefreshModel.DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh_Model = new UrlModel();
                        refesh_Model.tp = "Save";
                        refesh_Model.bt = "Refresh";
                        refesh_Model.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                        refesh_Model.Cmd = command;
                        TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                        return RedirectToAction("DomesticPackingDetail", refesh_Model);

                    case "Print":
                        return GenratePdfFile(_DomesticPackingDetail_Model);
                    case "BacktoList":
                        DomesticPackingList_Model backtolist_Model = new DomesticPackingList_Model();
                        backtolist_Model.WF_Status = _DomesticPackingDetail_Model.WF_Status1;
                        if (_DomesticPackingDetail_Model.DocumentMenuId == "105103130")
                        {
                            TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                            return RedirectToAction("DomesticPackingList", backtolist_Model);
                        }
                        else if (_DomesticPackingDetail_Model.DocumentMenuId == "105103145115")
                        {
                            TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                            return RedirectToAction("ExportPackingList", backtolist_Model);
                        }
                        else
                        {
                            TempData["ListFilterData"] = _DomesticPackingDetail_Model.ListFilterData1;
                            return RedirectToAction("DomesticPackingList", "DomesticPacking");
                        }
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
        public ActionResult GetAutoCompleteCustomerName(DomesticPackingDetail_Model _DomesticPackingDetail_Model)
        {
            string CustName = string.Empty;
            string CustType = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_DomesticPackingDetail_Model.filterCustomerName))
                {
                    CustName = "0";
                }
                else
                {
                    CustName = _DomesticPackingDetail_Model.filterCustomerName;
                    //_DomesticPackingDetail_Model.Customer_type
                }
                BrchID = Session["BranchId"].ToString();
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        CustType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        CustType = "E";
                //    }
                //}
                if (_DomesticPackingDetail_Model.DocumentMenuId == "105103130")
                {
                    CustType = "D";
                }
                else if (_DomesticPackingDetail_Model.DocumentMenuId == "105103145115")
                {
                    CustType = "E";
                }
                CustList = _DomesticPacking_IServices.GetCustomerList(Comp_ID, CustName, BrchID, CustType, _DomesticPackingDetail_Model.DocumentMenuId);

                List<CustomerName> _CustomerNameList = new List<CustomerName>();
                foreach (var dr in CustList)
                {
                    CustomerName _CustomerName = new CustomerName();
                    _CustomerName.cust_id = dr.Key;
                    _CustomerName.cust_name = dr.Value;
                    _CustomerNameList.Add(_CustomerName);
                }
                _DomesticPackingDetail_Model.CustomerNameList = _CustomerNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //public ActionResult PackingListSONoList(DomesticPackingDetail_Model _DomesticPackingDetail_Model)
        //{
        //    try
        //    {
        //        string SONumber, OrderType = string.Empty;
        //        DataSet OrderNumberList = new DataSet();
        //        string Cust_id = string.Empty;
        //        List<OrderNumber> _OrderNumberList = new List<OrderNumber>();

        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();

        //        }
        //        Cust_id = _DomesticPackingDetail_Model.filterCustomerName;
        //        SONumber = _DomesticPackingDetail_Model.FilterOrderNumber;
        //        string BrchID = Session["BranchId"].ToString();
        //        OrderNumberList = _DomesticPacking_IServices.getPackingListSONo(CompID, BrchID, Cust_id, SONumber);

        //        DataRow Drow = OrderNumberList.Tables[0].NewRow();
        //        Drow[0] = "---Select---";
        //        Drow[1] = "0";
        //        OrderNumberList.Tables[0].Rows.InsertAt(Drow, 0);

        //        foreach (DataRow dr in OrderNumberList.Tables[0].Rows)
        //        {
        //            OrderNumber _OrderNumber = new OrderNumber();
        //            _OrderNumber.so_no = dr["app_so_no"].ToString();
        //            _OrderNumber.so_dt = dr["so_dt"].ToString();
        //            _OrderNumberList.Add(_OrderNumber);
        //        }
        //        _DomesticPackingDetail_Model.OrderNumberList = _OrderNumberList;
        //        return Json(_OrderNumberList.Select(c => new { Name = c.so_no, ID = c.so_dt }).ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //}
        [HttpPost]
        public JsonResult GetPackingListSONoLists(string Cust_id, string Curr_Id)
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
                //DataSet result = _DomesticSaleInvoice_ISERVICE.GetShipmentList(Cust_id, Comp_ID, Br_ID);
                DataSet result = _DomesticPacking_IServices.getPackingListSONo(Comp_ID, Br_ID, Cust_id, Curr_Id);
                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";
                Drow[2] = "0";

                result.Tables[0].Rows.InsertAt(Drow, 0);
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
        public ActionResult getDetailByOrderNo(string OrderNumber, string PackingNumber, string PackType,string DocumentMenuId)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _DomesticPacking_IServices.getDetailByOrderNo(CompID, BrchID, OrderNumber, PackingNumber, PackType, DocumentMenuId);
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
        public ActionResult AmmendDocument(DomesticPackingDetail_Model _DomesticPackingDetail_Model)
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
            string mac = Session["UserMacaddress"].ToString();
            string system = Session["UserSystemName"].ToString();
            string ip = Session["UserIP"].ToString();
            string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
            String cancelMsg = _DomesticPacking_IServices.PackingListCancel(_DomesticPackingDetail_Model, CompID, userid, br_id, mac_id, _DomesticPackingDetail_Model.DocumentMenuId, "Amendment");
            return RedirectToAction("DomesticPackingList", "DomesticPacking");
        }
        public ActionResult SaveUpdatePackingList(DomesticPackingDetail_Model _DomesticPackingDetail_Model)
        {
            try
            {
                if (_DomesticPackingDetail_Model.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }
                    DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                    DataTable PackingListHeader = new DataTable();
                    DataTable PackingListItemDetails = new DataTable();
                    DataTable PackingListSoItemDetails = new DataTable();
                    DataTable PackingListItemBatchDetails = new DataTable();
                    DataTable PackingListOrderResItemBatchDetails = new DataTable();
                    DataTable PackingListItemSerialDetails = new DataTable();
                    DataTable PL_SerializationDetails = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(string));
                    dtheader.Columns.Add("br_id", typeof(string));
                    dtheader.Columns.Add("pack_type", typeof(string));
                    dtheader.Columns.Add("pack_no", typeof(string));
                    dtheader.Columns.Add("pack_dt", typeof(string));
                    dtheader.Columns.Add("cust_id", typeof(string));
                    dtheader.Columns.Add("remarks", typeof(string));
                    dtheader.Columns.Add("exp_file_no", typeof(string));
                    dtheader.Columns.Add("curr_id", typeof(string));
                    dtheader.Columns.Add("Ex_rate", typeof(string));
                    dtheader.Columns.Add("user_id", typeof(string));
                    dtheader.Columns.Add("pack_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));
                    dtheader.Columns.Add("total_gr_wght", typeof(string));
                    dtheader.Columns.Add("total_net_wght", typeof(string));
                    dtheader.Columns.Add("total_cbm", typeof(string));
                    dtheader.Columns.Add("Item_Orien", typeof(string));
                  

                    DataRow dtHeaderrow = dtheader.NewRow();
                    dtHeaderrow["MenuDocumentId"] = DocumentMenuId;
                    if (_DomesticPackingDetail_Model.pack_no != null)
                    {
                        dtHeaderrow["TransType"] = "Update";
                    }
                    else
                    {
                        dtHeaderrow["TransType"] = "Save";
                    }
                    dtHeaderrow["comp_id"] = Session["CompId"].ToString();
                    dtHeaderrow["br_id"] = Session["BranchId"].ToString();
                    dtHeaderrow["pack_type"] = _DomesticPackingDetail_Model.pack_type;
                    dtHeaderrow["pack_no"] = _DomesticPackingDetail_Model.pack_no;
                    dtHeaderrow["pack_dt"] = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                    dtHeaderrow["cust_id"] = _DomesticPackingDetail_Model.CustomerID;
                    dtHeaderrow["remarks"] = _DomesticPackingDetail_Model.pack_remarks;

                    decimal totgrwgt, totnetwgt, totcbm;

                    if (string.IsNullOrEmpty(_DomesticPackingDetail_Model.TotalGrossWgt))
                    {
                        totgrwgt = 0;
                    }
                    else
                    {
                        totgrwgt = Convert.ToDecimal(_DomesticPackingDetail_Model.TotalGrossWgt.ToString());
                    }
                    if (string.IsNullOrEmpty(_DomesticPackingDetail_Model.TotalNetWgt))
                    {
                        totnetwgt = 0;
                    }
                    else
                    {
                        totnetwgt = Convert.ToDecimal(_DomesticPackingDetail_Model.TotalNetWgt.ToString());
                    }
                    if (_DomesticPackingDetail_Model.TotalCBM != null)
                    {
                        if (_DomesticPackingDetail_Model.TotalCBM.ToString() == "")
                        {
                            totcbm = 0;
                        }
                        else
                        {
                            totcbm = Convert.ToDecimal(_DomesticPackingDetail_Model.TotalCBM.ToString());
                        }
                    }
                    else
                    {
                        totcbm = 0;
                    }
                    dtHeaderrow["total_gr_wght"] = totgrwgt.ToString();
                    dtHeaderrow["total_net_wght"] = totnetwgt.ToString();
                    dtHeaderrow["total_cbm"] = totcbm.ToString();
                    dtHeaderrow["exp_file_no"] = "";
                    dtHeaderrow["curr_id"] = _DomesticPackingDetail_Model.curr_id;
                    dtHeaderrow["Ex_rate"] = "0";
                    dtHeaderrow["user_id"] = Session["UserId"].ToString();
                    dtHeaderrow["pack_status"] = "D";
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    dtHeaderrow["mac_id"] = SystemDetail;
                    dtHeaderrow["Item_Orien"] = _DomesticPackingDetail_Model.hdnItemorientation;
                  
                    dtheader.Rows.Add(dtHeaderrow);
                    PackingListHeader = dtheader;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(string));
                    dtItem.Columns.Add("ord_qty", typeof(string));
                    dtItem.Columns.Add("bal_qty", typeof(string));
                    dtItem.Columns.Add("pack_nos", typeof(string));
                    dtItem.Columns.Add("gross_wght", typeof(string));
                    dtItem.Columns.Add("net_wght", typeof(string));
                    dtItem.Columns.Add("pack_qty", typeof(string));
                    dtItem.Columns.Add("wh_id", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("packaging_item_id", typeof(string));
                    dtItem.Columns.Add("tot_cbm", typeof(string));
                    dtItem.Columns.Add("phy_pack", typeof(string));
                    dtItem.Columns.Add("sr_no", typeof(int));
                    dtItem.Columns.Add("PackSize", typeof(string));

                    JArray jObject = JArray.Parse(_DomesticPackingDetail_Model.DnItemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal Ord_qty, bal_qty, NumberOfPacks, GrossWeight, NetWeight, PackedQuantity, Total_cbm;

                        if (jObject[i]["OrderQuantity"].ToString() == "")
                        {
                            Ord_qty = 0;
                        }
                        else
                        {
                            Ord_qty = Convert.ToDecimal(jObject[i]["OrderQuantity"].ToString());
                        }
                        if (jObject[i]["BalanceQuantity"].ToString() == "")
                        {
                            bal_qty = 0;
                        }
                        else
                        {
                            bal_qty = Convert.ToDecimal(jObject[i]["BalanceQuantity"].ToString());
                        }
                        if (jObject[i]["NumberOfPacks"].ToString() == "")
                        {
                            NumberOfPacks = 0;
                        }
                        else
                        {
                            NumberOfPacks = Convert.ToDecimal(jObject[i]["NumberOfPacks"].ToString());
                        }
                        if (jObject[i]["GrossWeight"].ToString() == "")
                        {
                            GrossWeight = 0;
                        }
                        else
                        {
                            GrossWeight = Convert.ToDecimal(jObject[i]["GrossWeight"].ToString());
                        }
                        if (jObject[i]["NetWeight"].ToString() == "")
                        {
                            NetWeight = 0;
                        }
                        else
                        {
                            NetWeight = Convert.ToDecimal(jObject[i]["NetWeight"].ToString());
                        }
                        if (jObject[i]["PackedQuantity"].ToString() == "")
                        {
                            PackedQuantity = 0;
                        }
                        else
                        {
                            PackedQuantity = Convert.ToDecimal(jObject[i]["PackedQuantity"].ToString());
                        }
                        if (jObject[i]["CBM"].ToString() == "")
                        {
                            Total_cbm = 0;
                        }
                        else
                        {
                            Total_cbm = Convert.ToDecimal(jObject[i]["CBM"].ToString());
                        }

                        DataRow dtrowItemdetails = dtItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowItemdetails["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowItemdetails["ord_qty"] = Ord_qty;
                        dtrowItemdetails["bal_qty"] = bal_qty;
                        dtrowItemdetails["pack_nos"] = NumberOfPacks;
                        dtrowItemdetails["gross_wght"] = GrossWeight;
                        dtrowItemdetails["net_wght"] = NetWeight;
                        dtrowItemdetails["pack_qty"] = PackedQuantity;
                        dtrowItemdetails["wh_id"] = jObject[i]["WhID"].ToString();
                        dtrowItemdetails["it_remarks"] = jObject[i]["remarks"].ToString();
                        dtrowItemdetails["packaging_item_id"] = jObject[i]["PackagingItemId"].ToString();
                        dtrowItemdetails["tot_cbm"] = Total_cbm;
                        dtrowItemdetails["phy_pack"] = jObject[i]["PhysicalPacks"].ToString();
                        dtrowItemdetails["sr_no"] = jObject[i]["sr_no"].ToString();
                        dtrowItemdetails["PackSize"] = jObject[i]["PackSize"].ToString();
                        dtItem.Rows.Add(dtrowItemdetails);
                    }
                    PackingListItemDetails = dtItem;
                    ViewData["ItemDetails"] = dtitemdetail(jObject);

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("uom_id", typeof(int));
                    Batch_detail.Columns.Add("ship_qty", typeof(string));
                    Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                    Batch_detail.Columns.Add("expiry_date", typeof(string));
                    Batch_detail.Columns.Add("issue_qty", typeof(string));
                    Batch_detail.Columns.Add("mfg_name", typeof(string));
                    Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                    Batch_detail.Columns.Add("mfg_date", typeof(string));
                    if (_DomesticPackingDetail_Model.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_DomesticPackingDetail_Model.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                            dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                            dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();

                            if (jObjectBatch[i]["BatchResStock"].ToString() != "" && jObjectBatch[i]["BatchResStock"].ToString() != null)
                            {
                                dtrowBatchDetailsLines["ship_qty"] = jObjectBatch[i]["BatchResStock"].ToString();
                            }
                            else
                            {
                                dtrowBatchDetailsLines["ship_qty"] = "0";
                            }

                            dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["BatchAvlStock"].ToString();

                            if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null)
                            {
                                dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }

                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["mfg_name"] = IsBlank(jObjectBatch[i]["mfg_name"].ToString(),null);
                            dtrowBatchDetailsLines["mfg_mrp"] = IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(),null);
                            dtrowBatchDetailsLines["mfg_date"] = IsBlank(jObjectBatch[i]["mfg_date"].ToString(),null);
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                        ViewData["BatchDetails"] = dtbatchdetail(jObjectBatch);
                    }
                    PackingListItemBatchDetails = Batch_detail;


                    DataTable OrderResBatch_detail = new DataTable();
                    OrderResBatch_detail.Columns.Add("doc_no", typeof(string));
                    OrderResBatch_detail.Columns.Add("doc_dt", typeof(string));
                    OrderResBatch_detail.Columns.Add("item_id", typeof(string));
                    OrderResBatch_detail.Columns.Add("uom_id", typeof(int));
                    OrderResBatch_detail.Columns.Add("lot_no", typeof(string));
                    OrderResBatch_detail.Columns.Add("batch_no", typeof(string));
                    OrderResBatch_detail.Columns.Add("res_qty", typeof(string));
                    OrderResBatch_detail.Columns.Add("issue_qty", typeof(string));
                    OrderResBatch_detail.Columns.Add("mfg_name", typeof(string));
                    OrderResBatch_detail.Columns.Add("mfg_mrp", typeof(string));
                    OrderResBatch_detail.Columns.Add("mfg_date", typeof(string));
                    if (_DomesticPackingDetail_Model.OrderReservedItemBatchWiseDetail != null)
                    {
                        JArray jObjectOResBatch = JArray.Parse(_DomesticPackingDetail_Model.OrderReservedItemBatchWiseDetail);
                        for (int i = 0; i < jObjectOResBatch.Count; i++)
                        {
                            DataRow dtrowOrderResBatchDetailsLines = OrderResBatch_detail.NewRow();
                            dtrowOrderResBatchDetailsLines["doc_no"] = jObjectOResBatch[i]["OrderNo"].ToString();
                            dtrowOrderResBatchDetailsLines["doc_dt"] = jObjectOResBatch[i]["OrderDt"].ToString();
                            dtrowOrderResBatchDetailsLines["item_id"] = jObjectOResBatch[i]["ItemId"].ToString();
                            dtrowOrderResBatchDetailsLines["uom_id"] = jObjectOResBatch[i]["UOMId"].ToString();
                            dtrowOrderResBatchDetailsLines["lot_no"] = jObjectOResBatch[i]["LotNo"].ToString();
                            dtrowOrderResBatchDetailsLines["batch_no"] = jObjectOResBatch[i]["BatchNo"].ToString();
                            dtrowOrderResBatchDetailsLines["res_qty"] = jObjectOResBatch[i]["ResQty"].ToString();
                            dtrowOrderResBatchDetailsLines["issue_qty"] = jObjectOResBatch[i]["IssueQty"].ToString();
                            dtrowOrderResBatchDetailsLines["mfg_name"] = IsBlank(jObjectOResBatch[i]["mfg_name"].ToString(),null);
                            dtrowOrderResBatchDetailsLines["mfg_mrp"] = IsBlank(jObjectOResBatch[i]["mfg_mrp"].ToString(),null);
                            dtrowOrderResBatchDetailsLines["mfg_date"] = IsBlank(jObjectOResBatch[i]["mfg_date"].ToString(),null);
                            OrderResBatch_detail.Rows.Add(dtrowOrderResBatchDetailsLines);
                        }
                        ViewData["ResBatchDetails"] = dtResbatchdetail(jObjectOResBatch);
                    }
                    PackingListOrderResItemBatchDetails = OrderResBatch_detail;

                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    Serial_detail.Columns.Add("uom_id", typeof(int));
                    Serial_detail.Columns.Add("ship_qty", typeof(string));
                    Serial_detail.Columns.Add("lot_no", typeof(string));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("issue_qty", typeof(string));
                    Serial_detail.Columns.Add("mfg_name", typeof(string));
                    Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                    Serial_detail.Columns.Add("mfg_date", typeof(string));

                    if (_DomesticPackingDetail_Model.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_DomesticPackingDetail_Model.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["ship_qty"] = "0";
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["mfg_name"] = IsBlank(jObjectSerial[i]["mfg_name"].ToString(),null);
                            dtrowSerialDetailsLines["mfg_mrp"] = IsBlank(jObjectSerial[i]["mfg_mrp"].ToString(),null);
                            dtrowSerialDetailsLines["mfg_date"] = IsBlank(jObjectSerial[i]["mfg_date"].ToString(),null);
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["SerialDetails"] = dtSerialdetail(jObjectSerial);
                    }
                    PackingListItemSerialDetails = Serial_detail;
                    /*---------Packing Serialization Details Table--------------------*/
                    DataTable PackSerial_detail = new DataTable();
                    PackSerial_detail.Columns.Add("item_id", typeof(string));
                    PackSerial_detail.Columns.Add("uom_id", typeof(int));
                    PackSerial_detail.Columns.Add("pack_qty", typeof(string));
                    PackSerial_detail.Columns.Add("sr_from", typeof(string));
                    PackSerial_detail.Columns.Add("sr_to", typeof(string));
                    PackSerial_detail.Columns.Add("qty_pr_pack", typeof(string));
                    PackSerial_detail.Columns.Add("phy_pack", typeof(string));
                    PackSerial_detail.Columns.Add("net_wt", typeof(string));
                    PackSerial_detail.Columns.Add("gross_wt", typeof(string));
                    PackSerial_detail.Columns.Add("gross_wt_pc", typeof(string)); /*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/
                    PackSerial_detail.Columns.Add("net_wt_pc", typeof(string));/*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
                    PackSerial_detail.Columns.Add("inner_qty", typeof(string));
                    PackSerial_detail.Columns.Add("tot_inner_qty", typeof(string));

                    if (_DomesticPackingDetail_Model.PackingSrlznDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_DomesticPackingDetail_Model.PackingSrlznDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = PackSerial_detail.NewRow();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["pack_qty"] = jObjectSerial[i]["PackQty"].ToString();
                            dtrowSerialDetailsLines["sr_from"] = jObjectSerial[i]["SerialFrom"].ToString();
                            dtrowSerialDetailsLines["sr_to"] = jObjectSerial[i]["SerialTo"].ToString();
                            dtrowSerialDetailsLines["qty_pr_pack"] = jObjectSerial[i]["QtyPerPack"].ToString();
                            dtrowSerialDetailsLines["phy_pack"] = jObjectSerial[i]["PhyPerPack"].ToString();
                            dtrowSerialDetailsLines["net_wt"] = jObjectSerial[i]["NetWeight"].ToString();
                            dtrowSerialDetailsLines["gross_wt"] = jObjectSerial[i]["GrossWeight"].ToString();
                            dtrowSerialDetailsLines["net_wt_pc"] = jObjectSerial[i]["Netweight_perpiece"].ToString();/*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
                            dtrowSerialDetailsLines["gross_wt_pc"] = jObjectSerial[i]["GrossWeight_perpiece"].ToString();/*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/
                            dtrowSerialDetailsLines["inner_qty"] = jObjectSerial[i]["QtyPerInner"].ToString();
                            dtrowSerialDetailsLines["tot_inner_qty"] = jObjectSerial[i]["tblTotalInnerBox"].ToString();
                            PackSerial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["PackSerializeDetails"] = dtPackSerializedetail(jObjectSerial);
                        _DomesticPackingDetail_Model.PL_SerialiizationDT = dtPackSerializedetail(jObjectSerial);
                    }
                    PL_SerializationDetails = PackSerial_detail;
                    /*---------Packing Serialization Details Table end--------------------*/


                    DataTable Order_detail = new DataTable();
                    Order_detail.Columns.Add("item_id", typeof(string));
                    Order_detail.Columns.Add("so_no", typeof(string));
                    Order_detail.Columns.Add("so_dt", typeof(string));
                    Order_detail.Columns.Add("uom_id", typeof(string));
                    Order_detail.Columns.Add("ord_qty", typeof(string));
                    Order_detail.Columns.Add("bal_qty", typeof(string));
                    Order_detail.Columns.Add("pack_qty", typeof(string));
                    Order_detail.Columns.Add("ord_foc_qty", typeof(string));
                    Order_detail.Columns.Add("bal_foc_qty", typeof(string));
                    Order_detail.Columns.Add("pack_foc_qty", typeof(string));

                    if (_DomesticPackingDetail_Model.ItemOrderQtyDetail != null)
                    {
                        JArray jObjectOrderQty = JArray.Parse(_DomesticPackingDetail_Model.ItemOrderQtyDetail);
                        for (int i = 0; i < jObjectOrderQty.Count; i++)
                        {
                            DataRow dtrowOrderDetails = Order_detail.NewRow();
                            dtrowOrderDetails["item_id"] = jObjectOrderQty[i]["itemid"].ToString();
                            dtrowOrderDetails["so_no"] = jObjectOrderQty[i]["docno"].ToString();
                            dtrowOrderDetails["so_dt"] = jObjectOrderQty[i]["docdate"].ToString();
                            dtrowOrderDetails["uom_id"] = jObjectOrderQty[i]["uomid"].ToString();
                            dtrowOrderDetails["ord_qty"] = jObjectOrderQty[i]["orderqty"].ToString();
                            dtrowOrderDetails["bal_qty"] = jObjectOrderQty[i]["pendingqty"].ToString();
                            dtrowOrderDetails["pack_qty"] = jObjectOrderQty[i]["packedqty"].ToString();
                            dtrowOrderDetails["ord_foc_qty"] = jObjectOrderQty[i]["orderfocqty"].ToString();
                            dtrowOrderDetails["bal_foc_qty"] = jObjectOrderQty[i]["pendingfocqty"].ToString();
                            dtrowOrderDetails["pack_foc_qty"] = jObjectOrderQty[i]["packedfocqty"].ToString();
                            Order_detail.Rows.Add(dtrowOrderDetails);
                        }
                        ViewData["OrderDetails"] = dtOrderdetail(jObjectOrderQty);
                    }
                    PackingListSoItemDetails = Order_detail;

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    dtSubItem.Columns.Add("src_doc_number", typeof(string));
                    dtSubItem.Columns.Add("src_doc_date", typeof(string));
                    dtSubItem.Columns.Add("OrdrQty", typeof(string));
                    dtSubItem.Columns.Add("PendQty", typeof(string));
                    dtSubItem.Columns.Add("foc_qty", typeof(string));
                    dtSubItem.Columns.Add("OrdrFocQty", typeof(string));
                    dtSubItem.Columns.Add("PendFocQty", typeof(string));

                    if (_DomesticPackingDetail_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_DomesticPackingDetail_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["src_doc_number"] = jObject2[i]["subItemSrcNo"].ToString();
                            dtrowItemdetails["src_doc_date"] = jObject2[i]["subItemSrcDate"].ToString();
                            dtrowItemdetails["OrdrQty"] = jObject2[i]["subItemPackOrdQty"].ToString();
                            dtrowItemdetails["PendQty"] = jObject2[i]["subItemPackPenQty"].ToString();
                            dtrowItemdetails["foc_qty"] = jObject2[i]["foc_qty"].ToString();
                            dtrowItemdetails["OrdrFocQty"] = jObject2[i]["subItemPackOrdfocQty"].ToString();
                            dtrowItemdetails["PendFocQty"] = jObject2[i]["subItemPackPenfocQty"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubitemDetails"] = dtSubitemdetail(jObject2);
                    }

                    DataTable dtSubItemRes = new DataTable();
                    dtSubItemRes.Columns.Add("item_id", typeof(string));
                    dtSubItemRes.Columns.Add("sub_item_id", typeof(string));
                    dtSubItemRes.Columns.Add("qty", typeof(string));
                    dtSubItemRes.Columns.Add("src_doc_number", typeof(string));
                    dtSubItemRes.Columns.Add("src_doc_date", typeof(string));
                    dtSubItemRes.Columns.Add("res_qty", typeof(string));


                    if (_DomesticPackingDetail_Model.SubItemResDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_DomesticPackingDetail_Model.SubItemResDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItemRes.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["src_doc_number"] = jObject2[i]["src_doc_no"].ToString();
                            dtrowItemdetails["src_doc_date"] = jObject2[i]["src_doc_date"].ToString();
                            dtrowItemdetails["res_qty"] = jObject2[i]["res_qty"].ToString();
                            dtSubItemRes.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubitemResDetails"] = dtSubitemResdetail(jObject2);
                    }

                    DataTable dtSubItemPackRes = new DataTable();

                    dtSubItemPackRes.Columns.Add("item_id", typeof(string));
                    dtSubItemPackRes.Columns.Add("sub_item_id", typeof(string));
                    dtSubItemPackRes.Columns.Add("qty", typeof(string));
                    dtSubItemPackRes.Columns.Add("ord_pack_qty", typeof(string));


                    if (_DomesticPackingDetail_Model.SubItemPackResDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_DomesticPackingDetail_Model.SubItemPackResDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItemPackRes.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["ord_pack_qty"] = jObject2[i]["ord_pack_qty"].ToString();
                            dtSubItemPackRes.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubitemPackResDetails"] = dtSubitemPackResdetail(jObject2);
                    }


                    /*------------------Sub Item end----------------------*/
                    //if(_DomesticPackingDetail_Model.Amend== "Amend")
                    //{
                    //    string br_id = Session["BranchId"].ToString();
                    //    string mac = Session["UserMacaddress"].ToString();
                    //    string system = Session["UserSystemName"].ToString();
                    //    string ip = Session["UserIP"].ToString();
                    //    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    //    //String cancelMsg = _DomesticPacking_IServices.PackingListCancel(_DomesticPackingDetail_Model, CompID, userid, br_id, mac_id, DocumentMenuId, "Amendment");
                    //}

                    string SaveMessage = _DomesticPacking_IServices.InsertUpdatePackingList(PackingListHeader, PackingListItemDetails
                        , PackingListSoItemDetails, PackingListItemBatchDetails,
                        PackingListOrderResItemBatchDetails, PackingListItemSerialDetails, PL_SerializationDetails
                        , dtSubItem, dtSubItemRes, dtSubItemPackRes);
                    if (SaveMessage == "DocModify")
                    {
                        _DomesticPackingDetail_Model.Message = "DocModify";
                        _DomesticPackingDetail_Model.BtnName = "Refresh";
                        _DomesticPackingDetail_Model.Command = "Refresh";
                        TempData["ModelData"] = _DomesticPackingDetail_Model;
                        return RedirectToAction("DomesticPackingDetail");
                    }
                    else
                    {
                        string PackingNumber = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                        if (Message == "Data_Not_Found")
                        {
                            ViewBag.MenuPageName = getDocumentName();
                            _DomesticPackingDetail_Model.Title = title;
                            var a = PackingNumber.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + _DomesticPackingDetail_Model.Title;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _DomesticPackingDetail_Model.Message = Message.Replace("_", "");
                            return RedirectToAction("DomesticPackingDetail");
                        }
                        if (Message == "Update" || Message == "Save")
                            _DomesticPackingDetail_Model.Message = "Save";
                        _DomesticPackingDetail_Model.Packing_No = PackingNumber;
                        _DomesticPackingDetail_Model.TransType = "Update";
                        _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                        _DomesticPackingDetail_Model.AppStatus = "D";
                        return RedirectToAction("DomesticPackingDetail");
                    }
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
                    DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    String SaveMessage = _DomesticPacking_IServices.PackingListCancel(_DomesticPackingDetail_Model, CompID, userid, br_id, mac_id, DocumentMenuId, "");
                    string PackingNumberNo = SaveMessage.Split(',')[0];
                    try
                    {
                        //string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "PackingList_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_DomesticPackingDetail_Model.pack_no, _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd"), fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _DomesticPackingDetail_Model.pack_no, "C", userid, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _DomesticPackingDetail_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    if (SaveMessage.Split(',')[1] == "StockNotAvailable")
                    {
                        _DomesticPackingDetail_Model.Message = "StockNotAvailable";
                    }
                    else
                    {
                        //_DomesticPackingDetail_Model.Message = "Cancelled";
                        _DomesticPackingDetail_Model.Message = _DomesticPackingDetail_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    }
                    _DomesticPackingDetail_Model.Packing_No = PackingNumberNo;
                    _DomesticPackingDetail_Model.TransType = "Update";
                    _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                    _DomesticPackingDetail_Model.DocumentMenuId = DocumentMenuId;
                    return RedirectToAction("DomesticPackingList", "DomesticPacking");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        private object IsBlank(string input, object output)//Added by Suraj Maurya on 27-11-2025
        {
            return input == "" ? output : input;
        }
        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(string));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("Avlstock", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("ord_qty", typeof(string));
            dtItem.Columns.Add("bal_qty", typeof(string));
            dtItem.Columns.Add("pack_nos", typeof(string));
            dtItem.Columns.Add("gr_wght", typeof(string));
            dtItem.Columns.Add("itmgross_wght", typeof(string));
            dtItem.Columns.Add("net_wght", typeof(string));
            dtItem.Columns.Add("itmnet_wght", typeof(string));
            dtItem.Columns.Add("pack_qty", typeof(string));
            dtItem.Columns.Add("wh_id", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("pack_item_id", typeof(string));
            dtItem.Columns.Add("pack_item_name", typeof(string));
            dtItem.Columns.Add("tot_cbm", typeof(string));
            dtItem.Columns.Add("phy_pack", typeof(string));
            dtItem.Columns.Add("PackSize", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                decimal Ord_qty, bal_qty, NumberOfPacks, GrossWeight, NetWeight, PackedQuantity, Total_cbm;

                if (jObject[i]["OrderQuantity"].ToString() == "")
                {
                    Ord_qty = 0;
                }
                else
                {
                    Ord_qty = Convert.ToDecimal(jObject[i]["OrderQuantity"].ToString());
                }
                if (jObject[i]["BalanceQuantity"].ToString() == "")
                {
                    bal_qty = 0;
                }
                else
                {
                    bal_qty = Convert.ToDecimal(jObject[i]["BalanceQuantity"].ToString());
                }
                if (jObject[i]["NumberOfPacks"].ToString() == "")
                {
                    NumberOfPacks = 0;
                }
                else
                {
                    NumberOfPacks = Convert.ToDecimal(jObject[i]["NumberOfPacks"].ToString());
                }
                if (jObject[i]["GrossWeight"].ToString() == "")
                {
                    GrossWeight = 0;
                }
                else
                {
                    GrossWeight = Convert.ToDecimal(jObject[i]["GrossWeight"].ToString());
                }

                if (jObject[i]["NetWeight"].ToString() == "")
                {
                    NetWeight = 0;
                }
                else
                {
                    NetWeight = Convert.ToDecimal(jObject[i]["NetWeight"].ToString());
                }
                if (jObject[i]["PackedQuantity"].ToString() == "")
                {
                    PackedQuantity = 0;
                }
                else
                {
                    PackedQuantity = Convert.ToDecimal(jObject[i]["PackedQuantity"].ToString());
                }
                if (jObject[i]["CBM"].ToString() == "")
                {
                    Total_cbm = 0;
                }
                else
                {
                    Total_cbm = Convert.ToDecimal(jObject[i]["CBM"].ToString());
                }

                DataRow dtrowItemdetails = dtItem.NewRow();

                dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowItemdetails["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowItemdetails["uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowItemdetails["uom_name"] = jObject[i]["UOMName"].ToString();
                dtrowItemdetails["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowItemdetails["Avlstock"] = jObject[i]["Avlstock"].ToString();
                dtrowItemdetails["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowItemdetails["i_serial"] = jObject[i]["i_serial"].ToString();
                dtrowItemdetails["ord_qty"] = Ord_qty;
                dtrowItemdetails["bal_qty"] = bal_qty;
                dtrowItemdetails["pack_nos"] = NumberOfPacks;
                dtrowItemdetails["gr_wght"] = GrossWeight;
                dtrowItemdetails["itmgross_wght"] = GrossWeight;
                dtrowItemdetails["net_wght"] = NetWeight;
                dtrowItemdetails["itmnet_wght"] = NetWeight;
                dtrowItemdetails["pack_qty"] = PackedQuantity;
                dtrowItemdetails["wh_id"] = jObject[i]["WhID"].ToString();
                dtrowItemdetails["it_remarks"] = jObject[i]["remarks"].ToString();
                dtrowItemdetails["pack_item_id"] = jObject[i]["PackagingItemId"].ToString();
                dtrowItemdetails["pack_item_name"] = jObject[i]["PackagingItemName"].ToString();
                dtrowItemdetails["tot_cbm"] = Total_cbm;
                dtrowItemdetails["phy_pack"] = jObject[i]["PhysicalPacks"].ToString();
                dtrowItemdetails["PackSize"] = jObject[i]["PackSize"].ToString();
                dtItem.Rows.Add(dtrowItemdetails);
            }
            return dtItem;
        }
        public DataTable dtbatchdetail(JArray jObjectBatch)
        {

            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("lot_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("uom_id", typeof(int));
            Batch_detail.Columns.Add("ship_qty", typeof(float));
            Batch_detail.Columns.Add("avl_batch_qty", typeof(float));
            Batch_detail.Columns.Add("resqty", typeof(float));
            Batch_detail.Columns.Add("totalavlqty", typeof(float));
            Batch_detail.Columns.Add("expiry_date", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));
            Batch_detail.Columns.Add("issue_qty", typeof(float));
            Batch_detail.Columns.Add("bt_sale", typeof(string));



            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                dtrowBatchDetailsLines["lot_id"] = jObjectBatch[i]["LotNo"].ToString();
                dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();

                if (jObjectBatch[i]["BatchResStock"].ToString() != "" && jObjectBatch[i]["BatchResStock"].ToString() != null)
                {
                    dtrowBatchDetailsLines["ship_qty"] = jObjectBatch[i]["BatchResStock"].ToString();
                }
                else
                {
                    dtrowBatchDetailsLines["ship_qty"] = "0";
                }

                dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["BatchAvlStock"].ToString();
                dtrowBatchDetailsLines["resqty"] = "0";
                dtrowBatchDetailsLines["totalavlqty"] = "0";

                if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null)
                {
                    dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                }
                else
                {
                    dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                }
                dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["ExpiryDate"].ToString();
                dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                dtrowBatchDetailsLines["bt_sale"] = "";
                Batch_detail.Rows.Add(dtrowBatchDetailsLines);
            }

            return Batch_detail;
        }
        public DataTable dtResbatchdetail(JArray jObjectOResBatch)
        {

            DataTable OrderResBatch_detail = new DataTable();
            OrderResBatch_detail.Columns.Add("doc_no", typeof(string));
            OrderResBatch_detail.Columns.Add("doc_dt", typeof(string));
            OrderResBatch_detail.Columns.Add("item_id", typeof(string));
            OrderResBatch_detail.Columns.Add("uom_id", typeof(int));
            OrderResBatch_detail.Columns.Add("lot_no", typeof(string));
            OrderResBatch_detail.Columns.Add("batch_no", typeof(string));
            OrderResBatch_detail.Columns.Add("res_qty", typeof(float));
            OrderResBatch_detail.Columns.Add("issue_qty", typeof(float));

            for (int i = 0; i < jObjectOResBatch.Count; i++)
            {
                DataRow dtrowOrderResBatchDetailsLines = OrderResBatch_detail.NewRow();
                dtrowOrderResBatchDetailsLines["doc_no"] = jObjectOResBatch[i]["OrderNo"].ToString();
                dtrowOrderResBatchDetailsLines["doc_dt"] = jObjectOResBatch[i]["OrderDt"].ToString();
                dtrowOrderResBatchDetailsLines["item_id"] = jObjectOResBatch[i]["ItemId"].ToString();
                dtrowOrderResBatchDetailsLines["uom_id"] = jObjectOResBatch[i]["UOMId"].ToString();
                dtrowOrderResBatchDetailsLines["lot_no"] = jObjectOResBatch[i]["LotNo"].ToString();
                dtrowOrderResBatchDetailsLines["batch_no"] = jObjectOResBatch[i]["BatchNo"].ToString();
                dtrowOrderResBatchDetailsLines["res_qty"] = jObjectOResBatch[i]["ResQty"].ToString();
                dtrowOrderResBatchDetailsLines["issue_qty"] = jObjectOResBatch[i]["IssueQty"].ToString();
                OrderResBatch_detail.Rows.Add(dtrowOrderResBatchDetailsLines);
            }

            return OrderResBatch_detail;
        }
        public DataTable dtSerialdetail(JArray jObjectSerial)
        {

            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("uom_id", typeof(int));
            Serial_detail.Columns.Add("ship_qty", typeof(string));
            Serial_detail.Columns.Add("lot_no", typeof(string));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("issue_qty", typeof(float));

            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                dtrowSerialDetailsLines["ship_qty"] = "0";
                dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                Serial_detail.Rows.Add(dtrowSerialDetailsLines);
            }

            return Serial_detail;
        }
        public DataTable dtPackSerializedetail(JArray jObjectSerial)
        {

            DataTable PackSerial_detail = new DataTable();
            PackSerial_detail.Columns.Add("item_id", typeof(string));
            PackSerial_detail.Columns.Add("item_name", typeof(string));
            PackSerial_detail.Columns.Add("uom_id", typeof(int));
            PackSerial_detail.Columns.Add("uom_name", typeof(string));
            PackSerial_detail.Columns.Add("pack_qty", typeof(string));
            PackSerial_detail.Columns.Add("sr_from", typeof(string));
            PackSerial_detail.Columns.Add("sr_to", typeof(string));
            PackSerial_detail.Columns.Add("qty_pr_pack", typeof(string));
            PackSerial_detail.Columns.Add("phy_pack", typeof(string));
            PackSerial_detail.Columns.Add("net_wt", typeof(string));
            PackSerial_detail.Columns.Add("gross_wt", typeof(string));
            PackSerial_detail.Columns.Add("total_packs", typeof(string));
            PackSerial_detail.Columns.Add("total_qty", typeof(string));
            PackSerial_detail.Columns.Add("total_net_wt", typeof(string));
            PackSerial_detail.Columns.Add("total_gross_wt", typeof(string));
            PackSerial_detail.Columns.Add("inner_qty", typeof(string));
            PackSerial_detail.Columns.Add("net_wt_pc", typeof(string));/*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
            PackSerial_detail.Columns.Add("gross_wt_pc", typeof(string)); /*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/
           // PackSerial_detail.Columns.Add("tot_inner_qty", typeof(string)); /*Added By Nitesh 23-01-2026 for Total Inner Qty*/

            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = PackSerial_detail.NewRow();
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                dtrowSerialDetailsLines["item_name"] = jObjectSerial[i]["ItemName"].ToString();
                dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                dtrowSerialDetailsLines["uom_name"] = jObjectSerial[i]["UOMName"].ToString();
                dtrowSerialDetailsLines["pack_qty"] = jObjectSerial[i]["PackQty"].ToString();
                dtrowSerialDetailsLines["sr_from"] = jObjectSerial[i]["SerialFrom"].ToString();
                dtrowSerialDetailsLines["sr_to"] = jObjectSerial[i]["SerialTo"].ToString();
                dtrowSerialDetailsLines["qty_pr_pack"] = jObjectSerial[i]["QtyPerPack"].ToString();
                dtrowSerialDetailsLines["phy_pack"] = jObjectSerial[i]["PhyPerPack"].ToString();
                dtrowSerialDetailsLines["net_wt"] = jObjectSerial[i]["NetWeight"].ToString();
                dtrowSerialDetailsLines["gross_wt"] = jObjectSerial[i]["GrossWeight"].ToString();
                dtrowSerialDetailsLines["total_packs"] = jObjectSerial[i]["TotalPack"].ToString();
                dtrowSerialDetailsLines["total_qty"] = jObjectSerial[i]["TotalQty"].ToString();
                dtrowSerialDetailsLines["total_net_wt"] = jObjectSerial[i]["TotalNetWeight"].ToString();
                dtrowSerialDetailsLines["total_gross_wt"] = jObjectSerial[i]["TotalGrossWeight"].ToString();
                dtrowSerialDetailsLines["inner_qty"] = jObjectSerial[i]["QtyPerInner"].ToString();
                dtrowSerialDetailsLines["net_wt_pc"] = jObjectSerial[i]["Netweight_perpiece"].ToString();/*Added By Nitesh 24-11-2023 for Net_weight Per Piece*/
                dtrowSerialDetailsLines["gross_wt_pc"] = jObjectSerial[i]["GrossWeight_perpiece"].ToString();/*Added By Nitesh 24-11-2023 for Gross_weight Per Piece*/
               // dtrowSerialDetailsLines["tot_inner_qty"] = jObjectSerial[i]["tblTotalInnerBox"].ToString();  /*Added By Nitesh 23-01-2026 for Total Inner Qty*/
                PackSerial_detail.Rows.Add(dtrowSerialDetailsLines);
            }

            return PackSerial_detail;
        }
        public DataTable dtOrderdetail(JArray jObjectOrderQty)
        {

            DataTable Order_detail = new DataTable();
            Order_detail.Columns.Add("item_id", typeof(string));
            Order_detail.Columns.Add("so_no", typeof(string));
            Order_detail.Columns.Add("so_dt", typeof(string));
            Order_detail.Columns.Add("uom_id", typeof(string));
            Order_detail.Columns.Add("ord_qty", typeof(string));
            Order_detail.Columns.Add("bal_qty", typeof(string));
            Order_detail.Columns.Add("pack_qty", typeof(string));

            for (int i = 0; i < jObjectOrderQty.Count; i++)
            {
                DataRow dtrowOrderDetails = Order_detail.NewRow();
                dtrowOrderDetails["item_id"] = jObjectOrderQty[i]["itemid"].ToString();
                dtrowOrderDetails["so_no"] = jObjectOrderQty[i]["docno"].ToString();
                dtrowOrderDetails["so_dt"] = jObjectOrderQty[i]["docdate"].ToString();
                dtrowOrderDetails["uom_id"] = jObjectOrderQty[i]["uomid"].ToString();
                dtrowOrderDetails["ord_qty"] = jObjectOrderQty[i]["orderqty"].ToString();
                dtrowOrderDetails["bal_qty"] = jObjectOrderQty[i]["pendingqty"].ToString();
                dtrowOrderDetails["pack_qty"] = jObjectOrderQty[i]["packedqty"].ToString();
                Order_detail.Rows.Add(dtrowOrderDetails);
            }

            return Order_detail;
        }
        public DataTable dtSubitemdetail(JArray jObject2)
        {

            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
            dtSubItem.Columns.Add("src_doc_number", typeof(string));
            dtSubItem.Columns.Add("src_doc_date", typeof(string));
            dtSubItem.Columns.Add("OrdrQty", typeof(string));
            dtSubItem.Columns.Add("PendQty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["src_doc_number"] = jObject2[i]["subItemSrcNo"].ToString();
                dtrowItemdetails["src_doc_date"] = jObject2[i]["subItemSrcDate"].ToString();
                dtrowItemdetails["OrdrQty"] = jObject2[i]["subItemPackOrdQty"].ToString();
                dtrowItemdetails["PendQty"] = jObject2[i]["subItemPackPenQty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }

            return dtSubItem;
        }
        public DataTable dtSubitemResdetail(JArray jObject2)
        {

            DataTable dtSubItemRes = new DataTable();
            dtSubItemRes.Columns.Add("item_id", typeof(string));
            dtSubItemRes.Columns.Add("sub_item_id", typeof(string));
            dtSubItemRes.Columns.Add("qty", typeof(string));
            dtSubItemRes.Columns.Add("src_doc_number", typeof(string));
            dtSubItemRes.Columns.Add("src_doc_date", typeof(string));
            dtSubItemRes.Columns.Add("res_qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItemRes.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["src_doc_number"] = jObject2[i]["src_doc_no"].ToString();
                dtrowItemdetails["src_doc_date"] = jObject2[i]["src_doc_date"].ToString();
                dtrowItemdetails["res_qty"] = jObject2[i]["res_qty"].ToString();
                dtSubItemRes.Rows.Add(dtrowItemdetails);
            }

            return dtSubItemRes;
        }
        public DataTable dtSubitemPackResdetail(JArray jObject2)
        {

            DataTable dtSubItemPackRes = new DataTable();

            dtSubItemPackRes.Columns.Add("item_id", typeof(string));
            dtSubItemPackRes.Columns.Add("sub_item_id", typeof(string));
            dtSubItemPackRes.Columns.Add("qty", typeof(string));
            dtSubItemPackRes.Columns.Add("ord_pack_qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItemPackRes.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["ord_pack_qty"] = jObject2[i]["ord_pack_qty"].ToString();
                dtSubItemPackRes.Rows.Add(dtrowItemdetails);
            }

            return dtSubItemPackRes;
        }
        private DataTable ToGenDynamicDataTable(DataTable dt, Dt_FieldsModel _fld)
        {
            foreach (var obj in _fld._Dt_Fields)
            {
                if (obj.Field_dataType == "string")
                    dt.Columns.Add(obj.Field_name, typeof(string));
                if (obj.Field_dataType == "float")
                    dt.Columns.Add(obj.Field_name, typeof(float));
                else if (obj.Field_dataType == "char")
                    dt.Columns.Add(obj.Field_name, typeof(char));
                else if (obj.Field_dataType == "int")
                    dt.Columns.Add(obj.Field_name, typeof(int));
            }
            return dt;
        }
        private List<PackingList> getPackinlistDetails(DomesticPackingList_Model _DomesticPackingList_Model)
        {
            try
            {
                List<PackingList> _PackingList = new List<PackingList>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
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
                if (_DomesticPackingList_Model.WF_Status != null)
                {
                    wfstatus = _DomesticPackingList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //    }
                //}
                BrchID = Session["BranchId"].ToString();
                DataSet dt = new DataSet();
                dt = _DomesticPacking_IServices.GetPackingListAll(CompID, BrchID, UserID, wfstatus, DocumentMenuId);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        PackingList _PackList = new PackingList();
                        _PackList.PackStatus = dr["pack_status"].ToString();
                        _PackList.pack_dt = dr["pack_dt"].ToString();
                        //_PackList.SO_Number = dr["so_no"].ToString();
                        _PackList.PackingListNO = dr["pack_no"].ToString();
                        //_PackList.SO_DATE = dr["so_dt"].ToString();
                        _PackList.CustomerName = dr["cust_name"].ToString();
                        _PackList.CreatedON = dr["create_dt"].ToString();
                        _PackList.ApprovedOn = dr["app_dt"].ToString();
                        _PackList.pack_type = dr["pack_type"].ToString();
                        _PackList.ModifiedOn = dr["mod_dt"].ToString();
                        _PackList.packing_date = dr["packing_date"].ToString();
                        _PackList.create_by = dr["create_by"].ToString();
                        _PackList.app_by = dr["app_by"].ToString();
                        _PackList.mod_by = dr["mod_by"].ToString();
                        _PackingList.Add(_PackList);
                    }
                }
                return _PackingList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public ActionResult PackingListSearch(string CustID, DateTime Fromdate, DateTime Todate, string Status, string Docid)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                // Session["WF_status"] = null;
                BrchID = Session["BranchId"].ToString();
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //    }
                //}
                DomesticPackingList_Model _DomesticPackingList_Model = new DomesticPackingList_Model();
                if (Docid != null)
                {
                    DocumentMenuId = Docid;
                    _DomesticPackingList_Model.DocumentMenuId = Docid;
                }
                List<PackingList> _PackingList = new List<PackingList>();
                dt = _DomesticPacking_IServices.GetPackingListFilter(CustID, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                //Session["DPDSearch"] = "DPD_Search";
                _DomesticPackingList_Model.DPDSearch = "DPD_Search";

                foreach (DataRow dr in dt.Rows)
                {
                    PackingList _PackList = new PackingList();
                    _PackList.PackStatus = dr["pack_status"].ToString();
                    _PackList.pack_dt = dr["pack_dt"].ToString();
                    //_PackList.SO_Number = dr["so_no"].ToString();
                    _PackList.PackingListNO = dr["pack_no"].ToString();
                    //_PackList.SO_DATE = dr["so_dt"].ToString();
                    _PackList.CustomerName = dr["cust_name"].ToString();
                    _PackList.CreatedON = dr["create_dt"].ToString();
                    _PackList.ApprovedOn = dr["app_dt"].ToString();
                    _PackList.pack_type = dr["pack_type"].ToString();
                    _PackList.ModifiedOn = dr["mod_dt"].ToString();
                    _PackList.packing_date = dr["packing_date"].ToString();
                    _PackList.create_by = dr["create_by"].ToString();
                    _PackList.app_by = dr["app_by"].ToString();
                    _PackList.mod_by = dr["mod_by"].ToString();
                    _PackingList.Add(_PackList);
                }
                _DomesticPackingList_Model.PackingListDetail = _PackingList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPackingListDetail.cshtml", _DomesticPackingList_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        //public ActionResult GetPakingListSONO(DomesticPackingDetail_Model _DomesticPackingDetail_Model)
        //{
        //    try
        //    {
        //        string SONumber, OrderType = string.Empty;
        //        DataSet OrderNumberList = new DataSet();
        //        string Cust_id = string.Empty;
        //        List<OrderNumber> _OrderNumberList = new List<OrderNumber>();

        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();

        //        }
        //        Cust_id = _DomesticPackingDetail_Model.filterCustomerName;
        //        SONumber = _DomesticPackingDetail_Model.FilterOrderNumber;
        //        string BrchID = Session["BranchId"].ToString();
        //        OrderNumberList = _DomesticPacking_IServices.getPackingListSONo(CompID, BrchID, Cust_id, SONumber);
        //        foreach (DataRow dr in OrderNumberList.Tables[0].Rows)
        //        {
        //            OrderNumber _OrderNumber = new OrderNumber();
        //            _OrderNumber.so_no = dr["app_so_no"].ToString();
        //            _OrderNumber.so_dt = dr["app_so_no"].ToString();
        //            _OrderNumberList.Add(_OrderNumber);
        //        }
        //        _DomesticPackingDetail_Model.OrderNumberList = _OrderNumberList;
        //        return Json(_OrderNumberList.Select(c => new { Name = c.so_no, ID = c.so_dt }).ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }


        //}
        private ActionResult PackingListDelete(DomesticPackingDetail_Model _DomesticPackingDetail_Model, string command)
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

                DataSet Message = _DomesticPacking_IServices.PackingListDelete(_DomesticPackingDetail_Model, CompID, BrchID);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["PackingNumber"] = _DomesticPackingDetail_Model.pack_no;
                //Session["TransType"] = command;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("DomesticPackingList", "DomesticPacking");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult PackingListApprove(DomesticPackingDetail_Model _DomesticPackingDetail_Model, string PackNo, DateTime PackDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string docid, string WF_Status1)
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
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //    }
                //}
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                    _DomesticPackingDetail_Model.DocumentMenuId = DocumentMenuId;
                }
                else
                {
                    DocumentMenuId = _DomesticPackingDetail_Model.DocumentMenuId;
                }
                // DomesticPackingDetail_Model _DomesticPackingDetail_Model = new DomesticPackingDetail_Model();
                _DomesticPackingDetail_Model.CreatedBy = Session["UserId"].ToString();
                _DomesticPackingDetail_Model.pack_no = PackNo;
                _DomesticPackingDetail_Model.pack_dt = PackDate;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Message = _DomesticPacking_IServices.PackingListApprove(_DomesticPackingDetail_Model, CompID, DocumentMenuId, BrchID, mac_id, A_Status, A_Level, A_Remarks);
                if (Message != "Approved")
                {
                    _DomesticPackingDetail_Model.StockMessage = Message;
                    Message = "StockNotAvail";
                }
                if (Message == "Approved")
                {
                    try
                    {
                        //string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "PackingList_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_DomesticPackingDetail_Model.pack_no, _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd"), fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _DomesticPackingDetail_Model.pack_no, "AP", userid, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _DomesticPackingDetail_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                   
                    //Session["Message"] = "Approved";
                    //_DomesticPackingDetail_Model.Message = "Approved";
                    _DomesticPackingDetail_Model.Message = _DomesticPackingDetail_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                if (Message == "StockNotAvail")
                {
                    //Session["Message"] = "StockNotAvail";
                    _DomesticPackingDetail_Model.Message = "StockNotAvail";
                }
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["PackigListNumber"] = _DomesticPackingDetail_Model.pack_no;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel ApproveModel = new UrlModel();
                _DomesticPackingDetail_Model.TransType = "Update";
                _DomesticPackingDetail_Model.Packing_No = _DomesticPackingDetail_Model.pack_no;
                //_DomesticPackingDetail_Model.Message = "Approved";
                _DomesticPackingDetail_Model.BtnName = "BtnEdit";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _DomesticPackingDetail_Model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _DomesticPackingDetail_Model;

                ApproveModel.tp = "Update";
                ApproveModel.PAC_No = _DomesticPackingDetail_Model.Packing_No;
                ApproveModel.PAC_Dt = _DomesticPackingDetail_Model.pack_dt.ToString("yyyy-MM-dd");
                ApproveModel.Docid = _DomesticPackingDetail_Model.DocumentMenuId;
                ApproveModel.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("DomesticPackingDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult getItemOrderQuantityDetail(string ItemID, string Status, string SelectedItemdetail, 
            string SubItemId, string TransType, string Command, string docid, string Amend
            )
        {
            try
            {
                DataTable DTableOrderQty = new DataTable();
              
                    if (SelectedItemdetail != null && SelectedItemdetail != "")
                    {
                        DataTable dtorderqty = new DataTable();
                        dtorderqty.Columns.Add("so_no", typeof(string));
                        dtorderqty.Columns.Add("so_dt", typeof(string));
                        dtorderqty.Columns.Add("item_id", typeof(string));
                        dtorderqty.Columns.Add("uom_id", typeof(string));
                        dtorderqty.Columns.Add("ord_qty", typeof(string));
                        dtorderqty.Columns.Add("bal_qty", typeof(string));
                        dtorderqty.Columns.Add("pack_qty", typeof(string));
                        dtorderqty.Columns.Add("ord_foc_qty", typeof(string));
                        dtorderqty.Columns.Add("bal_foc_qty", typeof(string));
                        dtorderqty.Columns.Add("pack_foc_qty", typeof(string));

                        JArray jObjectBatch = JArray.Parse(SelectedItemdetail);

                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemID").ToString() == ItemID.ToString())
                            {
                                DataRow dtorderqtyrow = dtorderqty.NewRow();
                                dtorderqtyrow["so_no"] = item.GetValue("DocNo").ToString();
                                dtorderqtyrow["so_dt"] = item.GetValue("DocDate").ToString();
                                dtorderqtyrow["item_id"] = item.GetValue("ItemID").ToString();
                                dtorderqtyrow["uom_id"] = item.GetValue("UomID").ToString();
                                dtorderqtyrow["ord_qty"] = item.GetValue("OrderQty").ToString();
                                dtorderqtyrow["bal_qty"] = item.GetValue("PendingQty").ToString();
                                dtorderqtyrow["pack_qty"] = item.GetValue("PackedQty").ToString();
                                dtorderqtyrow["ord_foc_qty"] = item.GetValue("OrdFocQty").ToString();
                                dtorderqtyrow["bal_foc_qty"] = item.GetValue("PendFocQty").ToString();
                                dtorderqtyrow["pack_foc_qty"] = item.GetValue("PackedFocQty").ToString();

                                dtorderqty.Rows.Add(dtorderqtyrow);
                            }
                        }
                        DTableOrderQty = dtorderqty;
                    }
                
                             
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = docid;
                ViewBag.SubItemId = SubItemId;
                ViewBag.Command = Command;
                ViewBag.TransType = TransType;
                ViewBag.Amend = Amend;
                if (DTableOrderQty.Rows.Count > 0)
                {
                    ViewBag.ItemOrderQtyDetail = DTableOrderQty;
                }
                return PartialView("~/Areas/Common/Views/Comn_PartialQuantityDetail.cshtml");

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getItemStockBatchWise(string ItemId, string Status, string WarehouseId, string SelectedItemdetail, string SelectedDocdetail, string docid, string TransType, string Command, string Amend)
        {
            try
            {
                string Docnolist = string.Empty;
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                if (SelectedDocdetail != null && SelectedDocdetail != "")
                {
                    JArray jObjectDoc = JArray.Parse(SelectedDocdetail);

                    foreach (JObject item in jObjectDoc.Children())
                    {
                        if (string.IsNullOrEmpty(Docnolist))
                        {
                            Docnolist = item.GetValue("docno").ToString().Trim();
                        }
                        else
                        {
                            Docnolist = Docnolist + "," + item.GetValue("docno").ToString().Trim();
                        }
                    }
                }
                 ds = _DomesticPacking_IServices.getItemStockBatchWise(ItemId, WarehouseId, CompID, BrchID, Docnolist);

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("BatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty").ToString().Trim();
                                //ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty");
                            }
                        }
                    }
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.Amend = Amend;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPackingListItemStockBatchWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetOrderResItemStockBatchWise(string ItemId, string WarehouseId, string Status, string LotNo, string BatchNo,
            string SelectedItemdetail, string SelectedDocdetail, string docid, string TransType, string Command)
        {
            try
            {
                string Docnolist = string.Empty;
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //    }
                //}
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                if (SelectedDocdetail != null && SelectedDocdetail != "")
                {
                    JArray jObjectDoc = JArray.Parse(SelectedDocdetail);

                    foreach (JObject item in jObjectDoc.Children())
                    {
                        if (string.IsNullOrEmpty(Docnolist))
                        {
                            Docnolist = item.GetValue("docno").ToString().Trim();
                        }
                        else
                        {
                            Docnolist = Docnolist + "," + item.GetValue("docno").ToString().Trim();
                        }
                    }
                }
                ds = _DomesticPacking_IServices.GetOrderResItemStockBatchWise(CompID, BrchID, ItemId, WarehouseId, LotNo, BatchNo, Docnolist);

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("OrderNo").ToString().Trim() == ds.Tables[0].Rows[i]["srcno"].ToString().Trim() && item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["itemid"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lotno"].ToString().Trim() && item.GetValue("BatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batchno"].ToString().Trim())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty");
                            }
                        }
                    }
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.OrderReservedItemStockBatchWise = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPackingOrderWiseReservedDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult GetOrderResItemStockForSubItemBatchWise(string ItemId, string WarehouseId, string Status
            , string SelectedItemdetail, string SelectedDocdetail, string docid, string TransType, string Command)
        {
            try
            {
                string Docnolist = string.Empty;
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //    }
                //}
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                var QtyDigit = Convert.ToInt32(Session["QtyDigit"].ToString());
                string DecQtyDigit = "0.";
                for (var i = 0; i < QtyDigit; i++)
                {
                    ViewBag.QtyDigit += "0";
                }
                if (SelectedDocdetail != null && SelectedDocdetail != "")
                {
                    JArray jObjectDoc = JArray.Parse(SelectedDocdetail);

                    foreach (JObject item in jObjectDoc.Children())
                    {
                        if (string.IsNullOrEmpty(Docnolist))
                        {
                            Docnolist = item.GetValue("docno").ToString().Trim();
                        }
                        else
                        {
                            Docnolist = Docnolist + "," + item.GetValue("docno").ToString().Trim();
                        }
                    }
                }
                if (Status == "D" || Status == "F" || Status == "")
                {
                    ds = _DomesticPacking_IServices.GetOrderResItemStockForSubItemBatchWise(CompID, BrchID, ItemId, WarehouseId, Docnolist, Status);

                    if (SelectedItemdetail != null && SelectedItemdetail != "")
                    {
                        JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            decimal IssudQty = 0;
                            foreach (JObject item in jObjectBatch.Children())
                            {
                                if (item.GetValue("OrderNo").ToString().Trim() == ds.Tables[0].Rows[i]["srcno"].ToString().Trim() && item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["itemid"].ToString().Trim())
                                {
                                    //ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty");
                                    IssudQty = Convert.ToDecimal(IssudQty) + Convert.ToDecimal(item.GetValue("IssueQty"));
                                }
                            }
                            ds.Tables[0].Rows[i]["issue_qty"] = IssudQty.ToString(DecQtyDigit);
                        }
                    }
                }
                else
                {
                    ds = _DomesticPacking_IServices.GetOrderResItemStockForSubItemBatchWise(CompID, BrchID, ItemId, WarehouseId, Docnolist, Status);

                }

                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.OrderReservedItemStockBatchWise = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPackingOrderWiseSubItemReservedDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetOrderResItemStockBatchWiseAfterInsert(string Pack_No, string Pack_Date, string ItemId, string Status, string LotNo, string BatchNo, string docid, string TransType, string Command)
        {
            try
            {
                string Docnolist = string.Empty;
                string PType = string.Empty;

                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //        PType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //        PType = "E";
                //    }
                //}
                if (docid != null && docid != "")
                {
                    if (docid == "105103130")
                    {
                        DocumentMenuId = docid;
                        PType = "D";
                    }
                    else if (docid == "105103145115")
                    {
                        DocumentMenuId = docid;
                        PType = "E";
                    }
                }
                ds = _DomesticPacking_IServices.GetOrderResItemStockBatchWiseAfterInsert(CompID, BrchID, PType, Pack_No, Pack_Date, ItemId, LotNo, BatchNo);

                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.OrderReservedItemStockBatchWise = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPackingOrderWiseReservedDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getItemStockBatchWiseAfterInsert(string PL_No, string PL_Date, string Status, string ItemId, string docid, string TransType, string Command)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string Type = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //        Type = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //        Type = "E";
                //    }
                //}
                if (docid != null && docid != "")
                {
                    if (docid == "105103130")
                    {
                        DocumentMenuId = docid;
                        Type = "D";
                    }
                    else if (docid == "105103145115")
                    {
                        DocumentMenuId = docid;
                        Type = "E";
                    }
                }
                ds = _DomesticPacking_IServices.getItemStockBatchWiseAfterInsert(Comp_ID, Br_ID, Type, PL_No, PL_Date, ItemId, docid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = Status;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                //return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialShipmenItemStockBatchWise.cshtml");
                //return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPackingListItemStockBatchWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getWarehouseWiseItemStock(string ItemId, string WarehouseId)
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
                string Stock = _Common_IServices.getWarehouseWiseItemStock(CompID, BrchID, WarehouseId, ItemId);
                return Json(Stock, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        //public ActionResult getItemstockWareHouselWise(string ItemId)
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BrchID = Session["BranchId"].ToString();
        //        }

        //        ds = _Common_IServices.getItemstockWarehouseWise(ItemId, CompID, BrchID);
        //        if (ds.Tables[0].Rows.Count > 0)
        //            ViewBag.ItemStockWareHouselWise = ds.Tables[0];
        //        return PartialView("~/Areas/Common/Views/PartialItemStockWareHouseWise.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }

        //}
        public ActionResult getPackagingItemDetail(string Itemid)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataSet ds = _DomesticPacking_IServices.getPackagingItemDetails(CompID, Itemid);
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
        public ActionResult getItemstockSerialWise(string ItemId, string Status, string WarehouseId, string SelectedItemSerial, string docid, string TransType, string Command, string Amend)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                ds = _Common_IServices.getItemstockSerialWise(ItemId, WarehouseId, CompID, BrchID);

                if (SelectedItemSerial != null && SelectedItemSerial != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemSerial);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LOTId").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("SerialNO").ToString().Trim() == ds.Tables[0].Rows[i]["serial_no"].ToString().Trim())
                            {
                                ds.Tables[0].Rows[i]["SerailSelected"] = "Y";
                            }
                        }
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockSerialWise = ds.Tables[0];

                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.Amend = Amend;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getItemstockSerialWiseAfterInsert(string PL_No, string PL_Date, string Status, string ItemId, string docid, string TransType, string Command)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string PL_Type = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103130")
                //    {
                //        DocumentMenuId = "105103130";
                //        PL_Type = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145115")
                //    {
                //        DocumentMenuId = "105103145115";
                //        PL_Type = "E";
                //    }
                //}
                if (docid != null && docid != "")
                {
                    if (docid == "105103130")
                    {
                        DocumentMenuId = docid;
                        PL_Type = "D";
                    }
                    else if (docid == "105103145115")
                    {
                        DocumentMenuId = docid;
                        PL_Type = "E";
                    }
                }
                ds = _DomesticPacking_IServices.getItemstockSerialWiseAfterInsert(Comp_ID, Br_ID, PL_Type, PL_No, PL_Date, ItemId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = Status;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [NonAction]
        private void getWarehouse(DomesticPackingDetail_Model _DomesticPackingDetail_Model)
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
                DataSet result = _Common_IServices.GetWarehouseList(Comp_ID, Br_ID);
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
                _DomesticPackingDetail_Model.WarehouseList = _WarehouseList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }
        }
        public ActionResult getItemPackagingDetail(string Status, string docid, string TransType, string Command, string Amend)
        {
            try
            {
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.Amend = Amend;
                return PartialView("~/Areas/Common/Views/Comn_PartialPackagingDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        //public JsonResult CheckShipmentAgainstPackingList(string DocNo, string DocDate)
        //{
        //    JsonResult DataRows = null;
        //    try
        //    {
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        DataSet Deatils = _DomesticPacking_IServices.CheckShipmentAgainstPackingList(Comp_ID, Br_ID, DocNo, DocDate);
        //        DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return DataRows;
        //}
        public string CheckShipmentAgainstPackingList(string DocNo, string DocDate)
        {
            //JsonResult DataRows = null;
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
                DataSet Deatils = _DomesticPacking_IServices.CheckShipmentAgainstPackingList(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return str;
        }

        public FileResult GenratePdfFile(DomesticPackingDetail_Model _model)
        {
            string fileName = "";

            switch (_model.PrintFormat)
            {
                case "F1":
                    fileName = "PackingList.pdf";
                    break;

                case "F2":
                    fileName = "PackingListF2.pdf";
                    break;

                default:
                    fileName = "PackingList.pdf";
                    break;
            }

            return File(GetPdfData(_model.pack_no, _model.pack_dt, _model.PrintFormat), "application/pdf", fileName);
        }
        public byte[] GetPdfData(string packageNo, DateTime packageDate,string printFormet)
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
                Byte[] bytes;
                DataSet Details = _DomesticPacking_IServices.GetPackingListDeatilsForPrint(CompID, BrchID, packageNo, packageDate.ToString("yyyy-MM-dd"), printFormet);
                ViewBag.PageName = "PL";
               
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["pack_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                string htmlcontent = string.Empty;
                if (printFormet=="F1" || printFormet == "" || printFormet == null)
                {
                    ViewBag.Title = "Packing List";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/PackingListPrint.cshtml"));
                }
                else
                {
                    ViewBag.Title = "Packing ListF2";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticPacking/PackingListPrintF2.cshtml"));
                }
               
                //string DelSchedule = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_PrintReportDeliverySchedule.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 15f, 15f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, Details);
                    //BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    //iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    //using (var reader1 = new PdfReader(bytes))
                    //{
                    //    using (var ms = new MemoryStream())
                    //    {
                    //        using (var stamper = new PdfStamper(reader1, ms))
                    //        {
                    //            int PageCount = reader1.NumberOfPages;
                    //            for (int i = 1; i <= PageCount; i++)
                    //            {

                    //                Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                    //                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                    //            }
                    //        }
                    //        bytes = ms.ToArray();
                    //    }
                    //}
                }
                return bytes.ToArray();
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
        private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details)
        {
            var docstatus = Details.Tables[0].Rows[0]["pack_status"].ToString().Trim();
            BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.BOLDITALIC, BaseColor.BLACK);
            string draftImage = string.Empty;
            if (docstatus == "C")
            {
                 draftImage = Server.MapPath("~/Content/Images/cancelled.png");
            }
            else
            {
                 draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
               

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        var draftimg = Image.GetInstance(draftImage);
                        draftimg.SetAbsolutePosition(20, 220);
                        draftimg.ScaleAbsolute(550f, 600f);

                        int PageCount = reader1.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            var content = stamper.GetUnderContent(i);
                            if (docstatus == "D" || docstatus == "F" || docstatus == "C")
                            {
                                content.AddImage(draftimg);
                            }
                            Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                        }
                    }
                    bytes = ms.ToArray();
                }
            }

            return bytes;
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
        public JsonResult GetWarehouseList1()
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
                DataSet result = _DomesticPacking_IServices.GetWarehouseList(Comp_ID, Br_ID);
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
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
        , string Flag, string Status, string Doc_no, string Doc_dt, string SoNo, string SoDate, string pack_type, string Wh_id, string Amend,string DocumentMenuId)
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
                int QtyDigit = 0;
                if (DocumentMenuId== "105103145115")
                {
                    QtyDigit = Convert.ToInt32(Session["ExpImpQtyDigit"]);
                }
                else
                {
                    QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (Flag == "PackList" || Flag == "FocPackList")//"FocPackList" Added by Suraj Maurya on 15-01-2026
                {
                    if (Status == "D" || Status == "F" || Status == "" || Amend == "Amend")
                    {
                        dt = _DomesticPacking_IServices.GetSubItemDetailsBySO(CompID, BrchID, Doc_no, Doc_dt, SoNo, SoDate, Item_id, pack_type).Tables[0];

                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        
                            for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Flag == "FocPackList")
                            {
                                dt.Rows[i]["ord_qty"] = dt.Rows[i]["ord_foc_qty"];
                                dt.Rows[i]["PendingQty"] = dt.Rows[i]["PendingFocQty"];
                            }
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString() && item.GetValue("SoNo").ToString() == dt.Rows[i]["app_so_no"].ToString())
                                {
                                    if (Flag == "FocPackList")
                                    {
                                        dt.Rows[i]["PackQty"] = Convert.ToDecimal(IsNull(item.GetValue("foc_qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                    }
                                    else
                                    {
                                        dt.Rows[i]["PackQty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                    }
                                    //dt.Rows[i]["PackQty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                    //dt.Rows[i]["PackFocQty"] = Convert.ToDecimal(IsNull(item.GetValue("foc_qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _DomesticPacking_IServices.GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, SoNo, SoDate, Flag).Tables[0];
                    }
                }
                else if (Flag == "Order" || Flag == "Pending")
                {
                    if (Status == "D" || Status == "F" || Status == "" || Amend == "Amend")
                    {
                        dt = _DomesticPacking_IServices.GetOrdrPendSubItemDetailsBySO(CompID, BrchID, Item_id, Doc_no, Doc_dt, SoNo, SoDate, "0", pack_type).Tables[0];
                    }
                    else
                    {
                        dt = _DomesticPacking_IServices.GetOrdrPendSubItemDetailsBySO(CompID, BrchID, Item_id, Doc_no, Doc_dt, SoNo, SoDate, "0", Flag).Tables[0];
                    }
                }
                else if (Flag == "PackResQuantity")
                {
                    if (Status == "D" || Status == "F" || Status == "" || Amend == "Amend")
                    {
                        dt = _DomesticPacking_IServices.GetOrdrPendSubItemDetailsBySO(CompID, BrchID, Item_id, Doc_no, Doc_dt, SoNo, SoDate, Wh_id, "PackResQty").Tables[0];
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _DomesticPacking_IServices.GetOrdrPendSubItemDetailsBySO(CompID, BrchID, Item_id, Doc_no, Doc_dt, SoNo, SoDate, Wh_id, "PackResQtyAfterSave").Tables[0];
                    }
                }
                else if (Flag == "PackResTotalQty")
                {
                    if (Status == "D" || Status == "F" || Status == "" || Amend == "Amend")
                    {
                        dt = _DomesticPacking_IServices.GetOrdrPendSubItemDetailsBySO(CompID, BrchID, Item_id, Doc_no, Doc_dt, SoNo, SoDate, Wh_id, "PackResQty").Tables[0];
                        dt.Columns.Add("ord_pack_qty", typeof(string));
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            decimal ord_pack_qty = 0;
                            decimal Qty = 0;
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    ord_pack_qty = ord_pack_qty + Convert.ToDecimal(IsNull(item.GetValue("ord_pack_qty").ToString(), "0"));
                                    Qty = Qty + Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0"));
                                }
                            }
                            dt.Rows[i]["ord_pack_qty"] = ord_pack_qty.ToString(ToFixDecimal(QtyDigit));
                            dt.Rows[i]["Qty"] = Qty.ToString(ToFixDecimal(QtyDigit));
                        }
                    }
                    else
                    {
                        dt = _DomesticPacking_IServices.GetOrdrPendSubItemDetailsBySO(CompID, BrchID, Item_id, Doc_no, Doc_dt, SoNo, SoDate, Wh_id, Flag).Tables[0];
                    }
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag == "PackList" ? Flag : Flag == "FocPackList" ? Flag: Flag == "PackResQuantity" ? Flag : Flag == "PackResTotalQty" ? Flag : "PackOrderPending",
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

        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
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
                        var data = GetPdfData(Doc_no, Convert.ToDateTime(Doc_dt),"F1");
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

        [HttpPost]
        public JsonResult checkorderqtymorethenpackingqty()
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
                DataSet result = _DomesticPacking_IServices.checkorderqtymorethenpackingqty(Comp_ID, Br_ID);
                // result.Tables[0].Rows.InsertAt(Drow, 0);
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
        // code wrote by - Sanjay Prasad on 14-12-2023 (Use - Export Items and Serialization details to Excel)
        public FileResult ExportItemsORSerializationDetailsToExcel(string act, string packNo)
        {
            string fileName = act;
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            string UserID = "";
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DocumentMenuId = "105103130";
            //ITEMDETAILS
            DataTable ds = _DomesticPacking_IServices.GetPLDetailsToExportExcel(act, CompID, BrchID, DocumentMenuId, UserID, packNo);

            var commonController = new CommonController(_Common_IServices);
            return commonController.ExportDatatableToExcel(fileName, ds);
        }
        public ActionResult GetOrderQtyAndBalenceQty(string custtomerID, string ItemID, string PackType ,string DocumentMenuId ,string packingNo)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                //DataSet ds = _DomesticPacking_IServices.getDetailByOrderNo(CompID, BrchID, OrderNumber, PackingNumber, PackType, DocumentMenuId);
                DataSet ds = _DomesticPacking_IServices.GetOrderQty(CompID, BrchID, custtomerID, ItemID, PackType, DocumentMenuId, packingNo);
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
        //Added by Nidhi on 19-08-2025
        public string SavePdfDocToSendOnEmailAlert_Ext(string PLNo, string PLDate, string fileName)
        {
            var data = GetPdfData(PLNo, Convert.ToDateTime(PLDate),"F1");
            var commonCont = new CommonController(_Common_IServices);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        //Added by Nidhi on 19-08-2025
        public ActionResult SendEmailAlert(string mail_id, string status, string docid, string Doc_no, string Doc_dt, string statusAM, string filepath)
        {
            try
            {
                string UserID = "";
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
                    UserID = Session["userid"].ToString();
                }
                if (statusAM == "Amendment")
                {
                    status = "AM";
                }
                var commonCont = new CommonController(_Common_IServices);
                //var _Model = TempData["ModelData"] as PODetailsModel;
                DataTable dt = new DataTable();
                //PODetailsModel modal = new PODetailsModel();
                string message = "";
                string mail_cont = "";
                string file_path = "";
                if (status == "A")
                {
                    try
                    {
                        if (filepath == "" || filepath == null)
                        {
                            //dt = PrintFormatDataTable(_Model);
                            //ViewBag.PrintOption = dt;
                            var data = GetPdfData(Doc_no, Convert.ToDateTime(Doc_dt),"F1");
                            string fileName = "PL_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            filepath = commonCont.SaveAlertDocument_MailExt(data, fileName);
                        }
                        string keyword = @"\ExternalEmailAlertPDFs\";
                        int index = filepath.IndexOf(keyword);
                        file_path = (index >= 0) ? filepath.Substring(index) : filepath;
                        message = _Common_IServices.SendAlertEmailExternal(CompID, BrchID, UserID, docid, Doc_no, "A", mail_id, filepath);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                }
                if (status == "C" || status == "FC" || status == "AM")
                {
                    try
                    {
                        message = _Common_IServices.SendAlertEmailExternal1(CompID, BrchID, UserID, docid, Doc_no, status, mail_id);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }

                }
                return Json(message);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }


        public ActionResult DownloadFileExcel(string ItemTableData)
        {
            try
            {
                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                DataTable ItemDetail = new DataTable();
                DataSet obj_ds = new DataSet();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("Sr.No.", typeof(string));
                dtheader.Columns.Add("Item Name*", typeof(string));
                dtheader.Columns.Add("UOM*", typeof(string));
                dtheader.Columns.Add("Packed Quantity*", typeof(float));
                dtheader.Columns.Add("Serial From", typeof(string));
                dtheader.Columns.Add("Serial To", typeof(string));
                dtheader.Columns.Add("Qty/Outer Pack*", typeof(string));
                dtheader.Columns.Add("Qty/Inner Pack", typeof(string));
                dtheader.Columns.Add("Net Weight/Pack*", typeof(string));
                dtheader.Columns.Add("Gross Weight/Pack", typeof(string));
                if(ItemTableData != "" && ItemTableData != null)
                {
                    if (ItemTableData != null)
                    {
                        JArray jObjectSerial = JArray.Parse(ItemTableData);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = dtheader.NewRow();
                            dtrowSerialDetailsLines["Sr.No."] = jObjectSerial[i]["id"].ToString();
                            dtrowSerialDetailsLines["Item Name*"] = jObjectSerial[i]["ItemName"].ToString();
                            dtrowSerialDetailsLines["UOM*"] = jObjectSerial[i]["UOMName"].ToString();
                            dtrowSerialDetailsLines["Packed Quantity*"] = jObjectSerial[i]["PackedQuantity"].ToString();

                            dtheader.Rows.Add(dtrowSerialDetailsLines);
                        }
                    }
                }
                
                    ItemDetail = dtheader;
                obj_ds.Tables.Add(ItemDetail);


                DataSet ds = new DataSet();
                CommonController com_obj = new CommonController();
                string filePath = "";
                var ExcelFileName = "";
                var pagename = "";
              
                    ExcelFileName = "ImportPackSerializationDetail";
                    pagename = "PackListSerializationExcel";
               
                filePath = com_obj.CreateExcelFile(ExcelFileName, Server);
                com_obj.AppendExcel(filePath, ds, obj_ds, pagename);

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

        //  [HttpPost]
        //  public ActionResult ValidateExcelFile(HttpPostedFileBase file, string uploadStatus, string ItemTableData)
        //  {
        //      try
        //      {
        //          string CompID = string.Empty;
        //          if (Session["CompId"] != null)
        //              CompID = Session["CompId"].ToString();

        //          string filePath = string.Empty;

        //          if (Request.Files.Count > 0)
        //          {
        //              HttpPostedFileBase postedFile = Request.Files[0];
        //              string path = Server.MapPath("~");

        //              string FolderPath = path + ("..\\ImportExcelFiles\\");
        //              if (!Directory.Exists(FolderPath))
        //                  Directory.CreateDirectory(FolderPath);

        //              filePath = FolderPath + Path.GetFileName(postedFile.FileName);
        //              string extension = Path.GetExtension(postedFile.FileName);
        //              postedFile.SaveAs(filePath);

        //              string conString = string.Empty;

        //              switch (extension)
        //              {
        //                  case ".xls":
        //                      conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
        //                      break;

        //                  case ".xlsx":
        //                      conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
        //                      break;

        //                  default:
        //                      return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
        //              }

        //              DataTable dtItems = new DataTable();

        //              // 🔥 IMPORTANT: IMEX=1 applied here
        //              conString = string.Format(conString, filePath);

        //              using (OleDbConnection connExcel = new OleDbConnection(conString))
        //              using (OleDbCommand cmdExcel = new OleDbCommand())
        //              using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
        //              {
        //                  cmdExcel.Connection = connExcel;

        //                  // Get sheet schema (unchanged)
        //                  connExcel.Open();
        //                  DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                  connExcel.Close();

        //                  // 🔥 FIXED QUERY (NO LEN FILTER)
        //                  //  string itemsQuery = "SELECT * FROM [PackSerializationDetail$] WHERE LEN([Item Name*]) >0";
        //                  string itemsQuery =
        //"SELECT * FROM [PackSerializationDetail$] " +
        //"WHERE [Item Name*] IS NOT NULL";
        //                  //                   string itemsQuery =
        //                  //"SELECT * FROM [PackSerializationDetail$] " +
        //                  //"WHERE [Item Name*] IS NOT NULL " +
        //                  //"AND LEN(TRIM(CSTR([Item Name*]))) > 0";

        //                  connExcel.Open();
        //                  cmdExcel.CommandText = itemsQuery;
        //                  odaExcel.SelectCommand = cmdExcel;
        //                  odaExcel.Fill(dtItems);
        //                  connExcel.Close();
        //              }

        //              if (dtItems.Rows.Count == 0)
        //                  return Json("Excel file is empty. Please fill data in excel file and try again");

        //              DataSet ds = new DataSet();
        //              ds.Tables.Add(dtItems);

        //              DataSet result = VerifyData(ds, uploadStatus, ItemTableData);
        //              if (result == null)
        //                  return Json("Excel file is empty. Please fill data in excel file and try again");

        //              ViewBag.ImportPackingSerializationPreview = result;

        //              return PartialView(
        //                  "~/Areas/ApplicationLayer/Views/Shared/PartialPackingSerializationImportDetail.cshtml",
        //                  result
        //              );
        //          }

        //          return Json("No file uploaded");
        //      }
        //      catch (Exception ex)
        //      {
        //          Errorlog.LogError(Server.MapPath("~"), ex);
        //          return Json("ErrorPage");
        //      }
        //  }

        [HttpPost]
        public ActionResult ValidateExcelFile(HttpPostedFileBase file, string uploadStatus, string ItemTableData)
        {
            try
            {
                string CompID = Session["CompId"]?.ToString() ?? "";

                if (file == null || file.ContentLength == 0)
                    return Json("No file uploaded");

                string extension = Path.GetExtension(file.FileName).ToLower();

                //if (extension != ".xls" && extension != ".xlsx")
                //    return Json("Invalid File. Please upload .xls or .xlsx");

                DataTable dtItems;

                using (var stream = file.InputStream)
                {
                    System.Text.Encoding.RegisterProvider(
                        System.Text.CodePagesEncodingProvider.Instance);

                    using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                    {
                        var conf = new ExcelDataReader.ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataReader.ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        DataSet dsExcel = reader.AsDataSet(conf);

                        // 🔥 EXACT SHEET NAME
                        dtItems = dsExcel.Tables["PackSerializationDetail"];

                        if (dtItems == null)
                            return Json("Sheet 'PackSerializationDetail' not found");
                    }
                }

                // 🔥 REMOVE EMPTY ROWS SAFELY (NO DATA LOSS)
                var validRows = dtItems.AsEnumerable()
                    .Where(r =>
                        r.ItemArray.Any(c =>
                            c != null &&
                            !string.IsNullOrWhiteSpace(c.ToString())))
                    .ToList();

                if (!validRows.Any())
                    return Json("Excel file is empty");

                dtItems = validRows.CopyToDataTable();

                DataSet ds = new DataSet();
                ds.Tables.Add(dtItems);

                DataSet result = VerifyData(ds, uploadStatus, ItemTableData);

                if (result == null || result.Tables.Count == 0)
                    return Json("No valid data found");

                ViewBag.ImportPackingSerializationPreview = result;

                return PartialView(
                    "~/Areas/ApplicationLayer/Views/Shared/PartialPackingSerializationImportDetail.cshtml",
                    result
                );
            }
            catch (Exception ex)
            {
                Errorlog.LogError(Server.MapPath("~"), ex);
                return Json("Error occurred while reading Excel");
            }
        }


        private DataSet VerifyData(DataSet dsItems, string uploadStatus, string ItemTableData)
        {
            try
            {
                if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
                    return null;

                string compId = Session["compid"]?.ToString() ?? string.Empty;
                string BrchID = Session["BranchId"]?.ToString() ?? string.Empty;

                DataSet preparedDs = PrepareDataset(dsItems, ItemTableData);

                DataSet result = _DomesticPacking_IServices.GetVerifiedDataOfExcel(
                    compId,
                    BrchID,
                    preparedDs.Tables[0], preparedDs.Tables[1]
                );

                if (uploadStatus.Trim() == "0")
                    return result;

                var filteredRows = result.Tables[0].AsEnumerable().Where(x => x.Field<string>("UploadStatus").ToUpper() == uploadStatus.ToUpper()).ToList();
                DataTable newDataTable = filteredRows.Any() ? filteredRows.CopyToDataTable() : result.Tables[0].Clone();
                result.Tables[0].Clear();

                for (int i = 0; i < newDataTable.Rows.Count; i++)
                {
                    result.Tables[0].ImportRow(newDataTable.Rows[i]);
                }
                result.Tables[0].AcceptChanges();
                return result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public DataSet PrepareDataset(DataSet dsItems,string ItemTableData)
        {
            string compId = "", brId = "", userId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();

            DataTable ExcelData = new DataTable();

            //Items Details//
            ExcelData.Columns.Add("Item_Name", typeof(string));
            ExcelData.Columns.Add("UOM", typeof(string));
            ExcelData.Columns.Add("Packed_Quantity", typeof(string));
            ExcelData.Columns.Add("Serial_From", typeof(string));
            ExcelData.Columns.Add("Serial_To", typeof(string));
            ExcelData.Columns.Add("Qty/Outer_Pack", typeof(string));
            ExcelData.Columns.Add("Qty/Inner_Pack", typeof(string));
            ExcelData.Columns.Add("Net_Weight/Pack", typeof(string));
            ExcelData.Columns.Add("Gross_Weight/Pack", typeof(string));
            //ExcelData.Columns.Add("Net_Weight/Piece", typeof(string));
            //ExcelData.Columns.Add("Gross_Weight/Piece", typeof(string));
            for (int i = 0; i < dsItems.Tables[0].Rows.Count; i++)
            {
                DataTable dtItem = dsItems.Tables[0];
                if (dtItem.Rows[i][0].ToString() != "" && Convert.ToInt32(dtItem.Rows[i][0].ToString()) > 0)
                {
                    DataRow dtr = ExcelData.NewRow();

                    dtr["Item_Name"] = dtItem.Rows[i][1].ToString().Trim();
                    dtr["UOM"] = dtItem.Rows[i][2].ToString().Trim();
                    dtr["Packed_Quantity"] = dtItem.Rows[i][3].ToString().Trim();
                    dtr["Serial_From"] = dtItem.Rows[i][4].ToString();
                    dtr["Serial_To"] = dtItem.Rows[i][5].ToString().Trim();
                    dtr["Qty/Outer_Pack"] = dtItem.Rows[i][6].ToString().Trim();
                    dtr["Qty/Inner_Pack"] = dtItem.Rows[i][7].ToString().Trim();
                    dtr["Net_Weight/Pack"] = dtItem.Rows[i][8].ToString().Trim();
                    dtr["Gross_Weight/Pack"] = dtItem.Rows[i][9].ToString().Trim();



                    ExcelData.Rows.Add(dtr);
                }

            }
           

            DataTable PageLevelData = new DataTable();
          
            PageLevelData.Columns.Add("Item_Name", typeof(string));
            PageLevelData.Columns.Add("UOM", typeof(string));
            PageLevelData.Columns.Add("Packed_Quantity", typeof(string));
            PageLevelData.Columns.Add("Serial_From", typeof(string));
            PageLevelData.Columns.Add("Serial_To", typeof(string));
            PageLevelData.Columns.Add("Qty/Outer_Pack", typeof(string));
            PageLevelData.Columns.Add("Qty/Inner_Pack", typeof(string));
            PageLevelData.Columns.Add("Net_Weight/Piece", typeof(string));
            PageLevelData.Columns.Add("Gross_Weight/Piece", typeof(string));
            if (ItemTableData != "" && ItemTableData != null)
            {
                if (ItemTableData != null)
                {
                    JArray jObjectSerial = JArray.Parse(ItemTableData);
                    for (int i = 0; i < jObjectSerial.Count; i++)
                    {
                        DataRow Pageleveldata = PageLevelData.NewRow();                    
                        Pageleveldata["Item_Name"] = jObjectSerial[i]["ItemName"].ToString();
                        Pageleveldata["UOM"] = jObjectSerial[i]["UOMName"].ToString();
                        Pageleveldata["Packed_Quantity"] = jObjectSerial[i]["PackedQuantity"].ToString();
                        PageLevelData.Rows.Add(Pageleveldata);
                    }
                }
            }

            //---------------------------End-------------------------------------
            DataSet dts = new DataSet();

            dts.Tables.Add(ExcelData);
            dts.Tables.Add(PageLevelData);

            return dts;
        }
    }
}
