using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.TDSDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TDSDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.TDSDetail
{
    public class TDSDetailController : Controller
    {
        string CompID,BrID ,UserId,language = String.Empty;
        string DocumentMenuId = "105104135160", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        TDSDetail_ISERVICES _TDSDetail_ISERVICES;
        DataSet ds;
        public TDSDetailController(Common_IServices _Common_IServices, TDSDetail_ISERVICES _TDSDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._TDSDetail_ISERVICES = _TDSDetail_ISERVICES;
        }
        // GET: ApplicationLayer/TDSDetail
        public ActionResult TDSDetail()
        {
            TDSDetailModel _TDSModel = new TDSDetailModel();
            CommonPageDetails();
            //ViewBag.MenuPageName = getDocumentName();
            DateTime dtnow = DateTime.Now;
            DataSet dtset = new DataSet();

           
            List<TDS_Name_List> tds_list = new List<TDS_Name_List>();
            ds = GetTDSNameDetail("0");
            tds_list.Add(new TDS_Name_List
            {
                tds_id = "0",
                tds_name = "---Select---",
                tds_acc_id = "0"
            });
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tds_list.Add(new TDS_Name_List
                {
                    tds_id = dr["tax_id"].ToString(),
                    tds_name = dr["tax_name"].ToString(),
                    tds_acc_id = dr["tax_acc_id"].ToString()
                });
            }
            _TDSModel.tds_name_list = tds_list;

            /*----------Add Sec_Code_List by Hina on 12-08-2025----------------*/
            List<Sec_Code_List> seccod_list = new List<Sec_Code_List>();
            
            seccod_list.Add(new Sec_Code_List
            {
                seccode_id = "0",
                seccode_name = "---Select---"
                
            });

            DataTable sdt = new DataTable();
            
            DataView dv = new DataView(ds.Tables[0]);

            dv.RowFilter = "sec_code<>''";

            sdt = dv.ToTable();

            foreach (DataRow dr in sdt.Rows)
            {
                seccod_list.Add(new Sec_Code_List
                {
                    seccode_id = dr["sec_code"].ToString(),
                    seccode_name = dr["sec_code"].ToString()
                    
                });
            }
            _TDSModel.seccode_list = seccod_list;

            dtset = GetFyList();
            if (dtset.Tables[0].Rows.Count > 0 && dtset.Tables[1].Rows.Count > 0)
            { 
                _TDSModel.From_dt = dtset.Tables[0].Rows[0]["currfy_startdt"].ToString();
            }
            string FromDate = _TDSModel.From_dt;
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            _TDSModel.To_dt = ToDate;
            _TDSModel.Title = title;
            //ViewBag.TDSDetails = GetTDSListDetails("", "", FromDate, ToDate);
            ViewBag.DocumentMenuId = DocumentMenuId;/*Add by Hina shrama on 19-11-2024 for loader (work on _footer.cshtml)*/
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/TDSDetail/TDSDetail.cshtml", _TDSModel);
        }
        [HttpPost]
        public ActionResult TDSDetailData(TDSDetailModel _TDSModel, string command)
        {
            try
            {
                if (_TDSModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "CsvPrint":
                        return ExportTDSDetailData(_TDSModel);
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
        public FileResult ExportTDSDetailData(TDSDetailModel _TDSModel)
        {
            try
            {
                DataTable Details = new DataTable();

                JArray jObject = JArray.Parse(_TDSModel.TDSDetailData);
                string fromDate = jObject[0]["fromDate"].ToString();
                string toDate = jObject[0]["toDate"].ToString();
                string suppId = jObject[0]["suppId"].ToString();
                string TDSId = jObject[0]["TDSID"].ToString();
                string tax_type = jObject[0]["tax_type"].ToString();
                string sec_code = jObject[0]["sec_code"].ToString();

                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                ds = _TDSDetail_ISERVICES.GetTDSDetailsMIS(CompID, BrID, TDSId, suppId, fromDate, toDate, tax_type,sec_code);
                dt = ds.Tables[0];
                List<TDS_Detail_List> _BB_Detail_List = new List<TDS_Detail_List>();
                var sortColumn = "itemname";
                var sortColumnDir = "asc";
                //dt = Details;
                if (dt.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        TDS_Detail_List bb_list = new TDS_Detail_List();
                        bb_list.SrNo = rowno + 1;
                        bb_list.Deduction_Date = row["Deduct_dt"].ToString();
                        bb_list.TDS_Name = row["tds_name"].ToString();
                        bb_list.Section_Code = row["sec_code"].ToString();
                        bb_list.Supplier_Name = row["supp_name"].ToString();
                        bb_list.TAN_Number = row["tan_No"].ToString();
                        bb_list.PAN_number = row["Pan_No"].ToString();
                        bb_list.City_State = row["CityState"].ToString();
                        bb_list.Bill_Number = row["bill_no"].ToString();
                        bb_list.Bill_Date = row["bill_dt"].ToString();
                        bb_list.Taxable_Amount = row["bill_amnt"].ToString();
                        bb_list.TDS_Percentage = row["TdsTcsPercnt"].ToString();
                        bb_list.TDS_Amount = row["tds_amnt"].ToString();
                        bb_list.Debit_Note_Number = row["vou_no"].ToString();
                        bb_list.Debit_Note_Date = row["vou_dt"].ToString();
                        _BB_Detail_List.Add(bb_list);
                        rowno = rowno + 1;
                    }
                }
                var ItemListData = (from tempitem in _BB_Detail_List select tempitem);

                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    ItemListData = ItemListData.OrderBy(p => p.Acc_Name);
                //}

                string searchValue = _TDSModel.searchValue;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToUpper();
                    ItemListData = ItemListData.Where(m => m.Deduction_Date.ToUpper().Contains(searchValue) || m.TDS_Name.ToUpper().Contains(searchValue) || m.Section_Code.ToUpper().Contains(searchValue)
                    || m.Supplier_Name.ToUpper().Contains(searchValue) || m.TAN_Number.ToUpper().Contains(searchValue) || m.PAN_number.ToUpper().Contains(searchValue)
                    || m.City_State.ToUpper().Contains(searchValue) || m.Bill_Number.ToUpper().Contains(searchValue) || m.Bill_Date.ToUpper().Contains(searchValue)
                    || m.Taxable_Amount.ToUpper().Contains(searchValue) || m.TDS_Amount.ToUpper().Contains(searchValue) || m.Debit_Note_Number.ToUpper().Contains(searchValue)
                    || m.Debit_Note_Date.ToUpper().Contains(searchValue));
                }
                var data = ItemListData.ToList();
                _TDSModel.hdnCSVPrint = null;
                DataTable dt1 = new DataTable();
                dt1 = ToTDSDetailExl(data);

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("TDS Detail", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
        public DataTable ToTDSDetailExl(List<TDS_Detail_List> _ItemListModel)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sr. No.", typeof(int));
            dataTable.Columns.Add("Deduction Date", typeof(string));
            dataTable.Columns.Add("TDS Name", typeof(string));
            dataTable.Columns.Add("Section Code", typeof(string));
            dataTable.Columns.Add("Supplier Name", typeof(string));
            dataTable.Columns.Add("TAN Number", typeof(string));
            dataTable.Columns.Add("PAN number", typeof(string));
            dataTable.Columns.Add("City/State", typeof(string));
            dataTable.Columns.Add("Bill Number", typeof(string));
            dataTable.Columns.Add("Bill Date", typeof(string));
            dataTable.Columns.Add("Taxable Amount", typeof(decimal));
            dataTable.Columns.Add("TDS Percentage", typeof(decimal));
            dataTable.Columns.Add("TDS Amount", typeof(decimal));
            dataTable.Columns.Add("Debit Note Number", typeof(string));
            dataTable.Columns.Add("Debit Note Date", typeof(string));

            foreach (var item in _ItemListModel)
            {
                DataRow rows = dataTable.NewRow();
                rows["Sr. No."] = item.SrNo;
                rows["Deduction Date"] = item.Deduction_Date;
                rows["TDS Name"] = item.TDS_Name;
                rows["Section Code"] = item.Section_Code;
                rows["Supplier Name"] = item.Supplier_Name;
                rows["TAN Number"] = item.TAN_Number;
                rows["PAN number"] = item.PAN_number;
                rows["City/State"] = item.City_State;
                rows["Bill Number"] = item.Bill_Number;
                rows["Bill Date"] = item.Bill_Date;
                rows["Taxable Amount"] = item.Taxable_Amount;
                rows["TDS Percentage"] = item.TDS_Percentage;
                rows["TDS Amount"] = item.TDS_Amount;
                rows["Debit Note Number"] = item.Debit_Note_Number;
                rows["Debit Note Date"] = item.Debit_Note_Date;
                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public ActionResult GetAutoCompleteSearchSuppList(TDSDetailModel _TDSModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SuppType = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_TDSModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _TDSModel.SuppName;
                }
                CustList = _TDSDetail_ISERVICES.GetSupplierList(Comp_ID, SupplierName, Br_ID, _TDSModel.tax_type);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _TDSModel.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        private DataSet GetFyList()
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_Id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                DataSet dt = _TDSDetail_ISERVICES.Get_FYList(Comp_ID, Br_Id);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataSet GetTDSNameDetail(string tax_type)
        {
            try
            {
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                Br_ID = Session["BranchId"].ToString();
               
                DataSet ds = _TDSDetail_ISERVICES.GetTDSNameList(CompID, Br_ID, tax_type);
                return ds;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
                //throw Ex;
            }
        }
        [HttpPost]
        public JsonResult getTdsNameList(string tax_type)
        {
            try
            {
                DataSet ds = GetTDSNameDetail(tax_type);
                return Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage"); ;
                //throw Ex;
            }
        }
        public DataTable GetTDSListDetails(string TDSId, string suppId,string fromDate, string toDate,string tax_type,string sec_code)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                ds = _TDSDetail_ISERVICES.GetTDSDetailsMIS(CompID, BrID, TDSId, suppId,fromDate, toDate, tax_type, sec_code);
                ViewBag.TotalTDSDetails = ds.Tables[1];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
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
                //return Json("ErrorPage");
                //  return View("~/Views/Shared/Error.cshtml");
                return null;
            }
        }
        public ActionResult SearchTDSDetails(string TDSId, string suppId, string fromDate, string toDate,string tax_type,string sec_code)
        {
            try
            {
                TDSDetailModel objModel = new TDSDetailModel();
                objModel.SearchStatus = "SEARCH";
                ViewBag.TDSDetails = GetTDSListDetails(TDSId, suppId,fromDate, toDate, tax_type, sec_code);

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialTDSDetail.cshtml", objModel);
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
                    BrID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserId = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrID, UserId, DocumentMenuId, language);
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

    }

}












