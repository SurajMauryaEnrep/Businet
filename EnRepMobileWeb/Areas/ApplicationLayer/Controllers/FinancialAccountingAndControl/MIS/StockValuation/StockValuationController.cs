
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.StockValuation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.StockValuation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.StockValuation
{
    public class StockValuationController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105104135165", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        StockValuation_ISERVICES _StockValuation_ISERVICES;
        public StockValuationController(Common_IServices _Common_IServices, StockValuation_ISERVICES _StockValuation_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._StockValuation_ISERVICES = _StockValuation_ISERVICES;
        }
        // GET: ApplicationLayer/StockValuation
        public ActionResult StockValuation(StockValuationModel _StkValModel)
        {
            CommonPageDetails();
            List<GroupName> _GroupList = new List<GroupName>();
            DataTable dtbl = GetGroupList(_StkValModel);
            if (dtbl.Rows.Count > 0)
            {
                foreach (DataRow data in dtbl.Rows)
                {
                    GroupName _GroupDetail = new GroupName();
                    _GroupDetail.ID = data["item_grp_id"].ToString();
                    _GroupDetail.Name = data["ItemGroupChildNood"].ToString();
                    _GroupList.Add(_GroupDetail);
                }
            }
            _GroupList.Insert(0, new GroupName() { ID = "0", Name = "---All---" });
            _StkValModel.GroupList = _GroupList;

            //List<Finyear> _FinyrList = new List<Finyear>();
            //DataTable dt = GetfinyearsGlAccs();
            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        Finyear _FinYr = new Finyear();
            //        _FinYr.FinyrId = dr["YrId"].ToString();
            //        _FinYr.Finyrs = dr["YrName"].ToString();
            //        _FinyrList.Add(_FinYr);
            //    }
            //}
            //_FinyrList.Insert(0, new Finyear() { FinyrId = "0", Finyrs = "---Select---" });
            //_StkValModel.Finyrlist = _FinyrList;
            //List<Month> plist = new List<Month>();
            //Month pObj = new Month();
            //pObj.id = "0";
            //pObj.name = "---Select---";
            //plist.Add(pObj);
            //_StkValModel.ddl_MonthList = plist;
            DataSet dttbl = new DataSet();
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                ViewBag.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();/*Add by Hina on 14-08-2024 for current date*/
                ViewBag.fylist = dttbl.Tables[1];
            }
            _StkValModel.ddlPriceLists = GetPriceList("", ViewBag.FromDate, DateTime.Now.ToString("yyyy-MM-dd"));
            _StkValModel.Title = title;
            ViewBag.DocumentMenuId = DocumentMenuId;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/StockValuation/StockValuation.cshtml", _StkValModel);
        }
        [HttpPost]
        public ActionResult StockValuationDetail(StockValuationModel _StkValModel, string command)
        {
            try
            {
                if (_StkValModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "CsvPrint":
                        return ExportStockValuationData(_StkValModel);
                    default:
                        return new EmptyResult();
                        //return new EmptyResult();

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public FileResult ExportStockValuationData(StockValuationModel _StkValModel)
        {
            try
            {
                DataTable Details = new DataTable();

                JArray jObject = JArray.Parse(_StkValModel.StockValuationData);
                string ItmGrpID = jObject[0]["ItmGrpID"].ToString();
                //string FinYear = jObject[0]["FinYear"].ToString();
                //string Month = jObject[0]["Month"].ToString();
                string ReportType = jObject[0]["ReportType"].ToString();
                string costbase = jObject[0]["costbase"].ToString();
                string inc_shfl = jObject[0]["inc_shfl"].ToString();
                string inc_zero = jObject[0]["inc_zero"].ToString();
                string Acc_id = jObject[0]["Acc_id"].ToString();
                string from_dt = jObject[0]["from_dt"].ToString();
                string to_dt = jObject[0]["to_dt"].ToString();
                string ftype = jObject[0]["ftype"].ToString();
                string sp_uom_id = jObject[0]["sp_uom_id"].ToString();

                //DataTable stkvaldata = new DataTable();
                DataSet stkvaldata = new DataSet();
                stkvaldata = GetStkValListDetails(ItmGrpID,ReportType, costbase, inc_shfl, inc_zero, Acc_id, from_dt, to_dt, ftype, sp_uom_id,"");

                List<StockValuationList> _BB_Detail_List1 = new List<StockValuationList>();
                if (stkvaldata.Tables[0].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow row in stkvaldata.Tables[0].Rows)
                    {
                        StockValuationList bb_list = new StockValuationList();
                        bb_list.SrNo = rowno + 1;
                        if (ReportType == "I"|| ftype=="popup")
                        {
                            bb_list.Item_Name = row["item_name"].ToString();
                            bb_list.UOM = row["Uom"].ToString();
                            if(ftype == "search")
                            {
                                bb_list.Group_Name = row["grp_name"].ToString();
                                bb_list.Group_Structure = row["group_str"].ToString();
                            }
                            bb_list.Opening_Quantity = row["opening_qty"].ToString();
                            bb_list.Opening_Value = row["opening_val"].ToString();
                            bb_list.Receipt_Quantity = row["rcpt_qty"].ToString();
                            bb_list.Receipt_Value = row["rcpt_val"].ToString();
                            bb_list.Issue_Quantity = row["issue_qty"].ToString();
                            bb_list.Issue_Value = row["issue_val"].ToString();
                            bb_list.Closing_Quantity = row["closing_qty"].ToString();
                            bb_list.Closing_Quantity_Specific = row["closing_qty_in_sp_uom"].ToString();
                            bb_list.Closing_Value = row["closing_val"].ToString();
                        }
                        else if (ReportType == "A")
                        {
                            bb_list.Acc_name = row["acc_name"].ToString();
                            bb_list.Opening_Quantity = row["opening_qty"].ToString();
                            bb_list.Opening_Value = row["opening_val"].ToString();
                            bb_list.Receipt_Quantity = row["rcpt_qty"].ToString();
                            bb_list.Receipt_Value = row["rcpt_val"].ToString();
                            bb_list.Issue_Quantity = row["issue_qty"].ToString();
                            bb_list.Issue_Value = row["issue_val"].ToString();
                            bb_list.Closing_Quantity = row["closing_qty"].ToString();
                            bb_list.Closing_Value = row["closing_val"].ToString();
                        }
                        else
                        {
                            bb_list.Group_Name = row["grp_name"].ToString();
                            bb_list.Group_Structure = row["group_str"].ToString();
                            bb_list.Opening_Quantity = row["opening_qty"].ToString();
                            bb_list.Opening_Value = row["opening_val"].ToString();
                            bb_list.Receipt_Quantity = row["rcpt_qty"].ToString();
                            bb_list.Receipt_Value = row["rcpt_val"].ToString();
                            bb_list.Issue_Quantity = row["issue_qty"].ToString();
                            bb_list.Issue_Value = row["issue_val"].ToString();
                            bb_list.Closing_Quantity = row["closing_qty"].ToString();
                            bb_list.Closing_Quantity_Specific = row["closing_qty_in_sp_uom"].ToString();
                            bb_list.Closing_Value = row["closing_val"].ToString();
                        }
                        _BB_Detail_List1.Add(bb_list);
                        rowno = rowno + 1;
                    }
                }
                var ItemListData = (from tempitem in _BB_Detail_List1 select tempitem);

                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    ItemListData = ItemListData.OrderBy(p => p.Acc_Name);
                //}

                string searchValue = _StkValModel.searchValue;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    if (ReportType == "I" || ftype == "popup")
                    {
                        if(ftype == "popup")
                        {
                            ItemListData = ItemListData.Where(m => m.Item_Name.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue)
                   || m.Opening_Quantity.ToUpper().Contains(searchValue)
                   || m.Opening_Value.ToUpper().Contains(searchValue) || m.Receipt_Quantity.ToUpper().Contains(searchValue) || m.Receipt_Value.ToUpper().Contains(searchValue)
                   || m.Issue_Quantity.ToUpper().Contains(searchValue) || m.Issue_Value.ToUpper().Contains(searchValue) || m.Closing_Quantity.ToUpper().Contains(searchValue)
                   || m.Closing_Quantity_Specific.ToUpper().Contains(searchValue) || m.Closing_Value.ToUpper().Contains(searchValue));
                        }
                        else
                        {
                            ItemListData = ItemListData.Where(m => m.Item_Name.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue)
                   || m.Group_Name.ToUpper().Contains(searchValue) || m.Group_Structure.ToUpper().Contains(searchValue) || m.Opening_Quantity.ToUpper().Contains(searchValue)
                   || m.Opening_Value.ToUpper().Contains(searchValue) || m.Receipt_Quantity.ToUpper().Contains(searchValue) || m.Receipt_Value.ToUpper().Contains(searchValue)
                   || m.Issue_Quantity.ToUpper().Contains(searchValue) || m.Issue_Value.ToUpper().Contains(searchValue) || m.Closing_Quantity.ToUpper().Contains(searchValue)
                   || m.Closing_Quantity_Specific.ToUpper().Contains(searchValue) || m.Closing_Value.ToUpper().Contains(searchValue));
                        }
                    }
                    else if (ReportType == "A")
                    {
                        ItemListData = ItemListData.Where(m => m.Acc_name.ToUpper().Contains(searchValue) || m.Opening_Quantity.ToUpper().Contains(searchValue)
                        || m.Opening_Value.ToUpper().Contains(searchValue) || m.Receipt_Quantity.ToUpper().Contains(searchValue) || m.Receipt_Value.ToUpper().Contains(searchValue)
                        || m.Issue_Quantity.ToUpper().Contains(searchValue) || m.Issue_Value.ToUpper().Contains(searchValue) || m.Closing_Quantity.ToUpper().Contains(searchValue)
                        || m.Closing_Value.ToUpper().Contains(searchValue));
                    }
                    else
                    {
                       ItemListData = ItemListData.Where(m => m.Group_Name.ToUpper().Contains(searchValue) || m.Group_Structure.ToUpper().Contains(searchValue) || m.Opening_Quantity.ToUpper().Contains(searchValue)
                       || m.Opening_Value.ToUpper().Contains(searchValue) || m.Receipt_Quantity.ToUpper().Contains(searchValue) || m.Receipt_Value.ToUpper().Contains(searchValue)
                       || m.Issue_Quantity.ToUpper().Contains(searchValue) || m.Issue_Value.ToUpper().Contains(searchValue) || m.Closing_Quantity.ToUpper().Contains(searchValue)
                       || m.Closing_Quantity_Specific.ToUpper().Contains(searchValue) || m.Closing_Value.ToUpper().Contains(searchValue));
                    }   
                }
                var data = ItemListData.ToList();
                _StkValModel.hdnCSVPrint = null;
                DataTable dt1 = new DataTable();
                dt1 = ToStockValuationDetailExl(data, ReportType, ftype);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("StockValuation", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToStockValuationDetailExl(List<StockValuationList> _ItemListModel,string ReportType,string ftype)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr. No.", typeof(int));
            if (ReportType == "I"|| ftype=="popup")
            {
                dataTable.Columns.Add("Item Name", typeof(string));
                dataTable.Columns.Add("UOM", typeof(string));
                if(ftype == "search")
                {
                    dataTable.Columns.Add("Group Name", typeof(string));
                    dataTable.Columns.Add("Group Structure", typeof(string));
                }
                dataTable.Columns.Add("Opening Quantity", typeof(decimal));
                dataTable.Columns.Add("Opening Value", typeof(decimal));
                dataTable.Columns.Add("Receipt Quantity", typeof(decimal));
                dataTable.Columns.Add("Receipt Value", typeof(decimal));
                dataTable.Columns.Add("Issue Quantity", typeof(decimal));
                dataTable.Columns.Add("Issue Value", typeof(decimal));
                dataTable.Columns.Add("Closing Quantity", typeof(decimal));
                dataTable.Columns.Add("Closing Quantity (In Specific)", typeof(string));
                dataTable.Columns.Add("Closing Value", typeof(decimal));
            }
            else if (ReportType == "G")
            {
                dataTable.Columns.Add("Group Name", typeof(string));
                dataTable.Columns.Add("Group Structure", typeof(string));
                dataTable.Columns.Add("Opening Quantity", typeof(decimal));
                dataTable.Columns.Add("Opening Value", typeof(decimal));
                dataTable.Columns.Add("Receipt Quantity", typeof(decimal));
                dataTable.Columns.Add("Receipt Value", typeof(decimal));
                dataTable.Columns.Add("Issue Quantity", typeof(decimal));
                dataTable.Columns.Add("Issue Value", typeof(decimal));
                dataTable.Columns.Add("Closing Quantity", typeof(decimal));
                dataTable.Columns.Add("Closing Quantity (In Specific)", typeof(string));
                dataTable.Columns.Add("Closing Value", typeof(decimal));
            }
            else 
            {
                dataTable.Columns.Add("GL Account", typeof(string));
                dataTable.Columns.Add("Opening Quantity", typeof(decimal));
                dataTable.Columns.Add("Opening Value", typeof(decimal));
                dataTable.Columns.Add("Receipt Quantity", typeof(decimal));
                dataTable.Columns.Add("Receipt Value", typeof(decimal));
                dataTable.Columns.Add("Issue Quantity", typeof(decimal));
                dataTable.Columns.Add("Issue Value", typeof(decimal));
                dataTable.Columns.Add("Closing Quantity", typeof(decimal));
                dataTable.Columns.Add("Closing Value", typeof(decimal));
            }

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr. No."] = item.SrNo;
                if (ReportType == "I"|| ftype == "popup")
                {
                    rows["Item Name"] = item.Item_Name;
                    rows["UOM"] = item.UOM;
                    if (ftype == "search")
                    {
                        rows["Group Name"] = item.Group_Name;
                        rows["Group Structure"] = item.Group_Structure;
                    }
                    rows["Opening Quantity"] = item.Opening_Quantity;
                    rows["Opening Value"] = item.Opening_Value;
                    rows["Receipt Quantity"] = item.Receipt_Quantity;
                    rows["Receipt Value"] = item.Receipt_Value;
                    rows["Issue Quantity"] = item.Issue_Quantity;
                    rows["Issue Value"] = item.Issue_Value;
                    rows["Closing Quantity"] = item.Closing_Quantity;
                    rows["Closing Quantity (In Specific)"] = IsNull(item.Closing_Quantity_Specific, "0");
                    rows["Closing Value"] = item.Closing_Value;
                }
                else if (ReportType == "G")
                {
                    rows["Group Name"] = item.Group_Name;
                    rows["Group Structure"] = item.Group_Structure;
                    rows["Opening Quantity"] = item.Opening_Quantity;
                    rows["Opening Value"] = item.Opening_Value;
                    rows["Receipt Quantity"] = item.Receipt_Quantity;
                    rows["Receipt Value"] = item.Receipt_Value;
                    rows["Issue Quantity"] = item.Issue_Quantity;
                    rows["Issue Value"] = item.Issue_Value;
                    rows["Closing Quantity"] = item.Closing_Quantity;
                    rows["Closing Quantity (In Specific)"] = IsNull(item.Closing_Quantity_Specific, "0");
                    rows["Closing Value"] = item.Closing_Value;
                }
                else
                {
                    rows["GL Account"] = item.Acc_name;
                    rows["Opening Quantity"] = item.Opening_Quantity;
                    rows["Opening Value"] = item.Opening_Value;
                    rows["Receipt Quantity"] = item.Receipt_Quantity;
                    rows["Receipt Value"] = item.Receipt_Value;
                    rows["Issue Quantity"] = item.Issue_Quantity;
                    rows["Issue Value"] = item.Issue_Value;
                    rows["Closing Quantity"] = item.Closing_Quantity;
                    rows["Closing Value"] = item.Closing_Value;
                }
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        private DataTable GetGroupList(StockValuationModel _StkValModel)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string GroupName = string.Empty;
                if (string.IsNullOrEmpty(_StkValModel.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _StkValModel.ddlGroup;
                }
                DataTable dt = _StockValuation_ISERVICES.BindGetGroupList(GroupName, Comp_ID);
                
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;

            }
        }
        // For Financial year Dropdowns
        public DataTable GetfinyearsGlAccs()
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
                DataTable dt = _StockValuation_ISERVICES.GetFinYearList(CompID, BrchID);
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
        //public ActionResult BindMonth(string fin_sfy)
        //{
        //    JsonResult DataRows = null;
        //    //string product_id = string.Empty;
        //    string Comp_ID = string.Empty;
        //    string Br_ID = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();

        //            if (Session["BranchId"] != null)
        //            {
        //                Br_ID = Session["BranchId"].ToString();
        //            }
        //            //string[] splitFY = financial_year.Split(',');
        //            DataSet ds = _StockValuation_ISERVICES.BindFinancialYearMonths(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), fin_sfy);
        //            DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/

        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return DataRows;
        //}
        public DataSet GetStkValListDetails(string ItmGrpID,string ReportType, string costbase, string inc_shfl
            , string inc_zero,string acc_id, string from_dt, string to_dt, string ftype,string sp_uom_id, string priceList)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataSet dt = _StockValuation_ISERVICES.GetStkValDetailsMIS(CompID, BrchID, ItmGrpID,ReportType, costbase
                    , inc_shfl, inc_zero, acc_id, from_dt, to_dt, ftype, sp_uom_id, priceList);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return null;
            }
        }
        public ActionResult SearchStkValDetails(string ItmGrpID, string ReportType,string costbase,string inc_shfl
            , string inc_zero, string Acc_id, string from_dt, string to_dt, string ftype,string sp_uom_id,string priceList)
        {
            try
            {
                StockValuationModel objModel = new StockValuationModel();
                objModel.SearchStatus = "SEARCH";
                DataSet stkvaldata = new DataSet();
                stkvaldata = GetStkValListDetails(ItmGrpID, ReportType, costbase, inc_shfl, inc_zero, Acc_id, from_dt, to_dt, ftype, sp_uom_id,priceList);

                ViewBag.rpt_type = ReportType;
                ViewBag.cost_type = costbase;
                if (costbase == "PL")
                {
                    ViewBag.PLNameList = stkvaldata.Tables[2];
                }
                if (ftype== "popup")
                {
                    ViewBag.StkVal_AccItemDetails = null;
                    ViewBag.rpttype = null;
                    ViewBag.StkVal_AccItemDetails = stkvaldata;
                    ViewBag.rpttype = ReportType;
                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockValuationItemDetails.cshtml");
                }
                else
                {
                    ViewBag.StkValDetails = null;
                    if (stkvaldata != null)/*Add by Hina sharma on 03-02-2025 for show total of all values*/
                    {
                        ViewBag.StkValDetails = stkvaldata.Tables[0];
                        ViewBag.TotalVal_StkValuation = stkvaldata.Tables[1];
                        
                        
                    }

                    return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockValuation.cshtml", objModel);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return Json("ErrorPage");
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
        private DataSet GetFyList()
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
                DataSet dt = _StockValuation_ISERVICES.Get_FYList(CompID, BrchID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult get_recpissuedetail(string id, string fromdt, string todt, string flag, string rpttype, string shflflag, string cost_type,string priceList)
        {
            try
            {

                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataSet dt = _StockValuation_ISERVICES.Get_rcptIssueDetail(CompID, BrchID, id, flag, rpttype, shflflag, fromdt, todt, cost_type, priceList);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    ViewBag.tblrcptissue = dt.Tables[0];
                }
                else
                {
                    ViewBag.tblrcptissue = null;
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    ViewBag.tbltot_rcptissue = dt.Tables[1];
                }
                else
                {
                    ViewBag.tbltot_rcptissue = null;
                }
                if (cost_type == "PL")
                {
                    ViewBag.PLNameList = dt.Tables[2];
                }
                ViewBag.popflag = flag;
                ViewBag.rpttype = rpttype;
                ViewBag.cost_type = cost_type;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockValuation_receissue.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public List<PriceList> GetPriceList(string SearchValue = "",string fromDt="",string toDt="")/* Added by Suraj Maurya on 16-10-2025 */
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                List<PriceList> priceList = new List<PriceList>();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = "";
                }
                DataSet SOItmList = _StockValuation_ISERVICES.Get_priceList(Comp_ID, Br_ID, SearchValue, fromDt,toDt);
                priceList = SOItmList.Tables[0].AsEnumerable().Select((row, index) => new PriceList
                {
                    
                    list_no = row.Field<int>("list_no"),
                    list_name = row.Field<string>("list_name"),
                    valid_fr = row.Field<string>("valid_fr"),
                    valid_to = row.Field<string>("valid_to")
                }).ToList();
                priceList.Insert(0, new PriceList { list_no = 0, list_name = "---Select---" });
                return priceList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
    }

}
























