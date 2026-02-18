using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.PendingAdvances;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.PendingAdvances;
using System.Data;
using System.Collections.Generic;
using EnRepMobileWeb.Areas.Common.Controllers.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.MIS.PendingAdvances
{
    public class PendingAdvancesController : Controller
    {
        string CompID, Br_ID,language = String.Empty, UserID = String.Empty;
        string DocumentMenuId = "105104135122", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        PendingAdvances_ISERVICE _PendingAdvances_ISERVICE;
        public PendingAdvancesController(Common_IServices _Common_IServices, PendingAdvances_ISERVICE _PendingAdvances_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._PendingAdvances_ISERVICE = _PendingAdvances_ISERVICE;
        }
        // GET: ApplicationLayer/PendingAdvances
        public ActionResult PendingAdvances(PendingAdvancesModel _PAModel, string cmd, string TYP, string BTN)
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                var _PAModel1 = TempData["ModelData"] as PendingAdvancesModel;
                if (_PAModel1 != null)
                {
                    _PAModel1.Title = title;
                    DataSet ds = _PendingAdvances_ISERVICE.GetUserRangeDetail(CompID, UserID);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _PAModel1.Range1 = ds.Tables[0].Rows[0]["range_1"].ToString();
                        _PAModel1.Range2 = ds.Tables[0].Rows[0]["range_2"].ToString();
                        _PAModel1.Range3 = ds.Tables[0].Rows[0]["range_3"].ToString();
                        _PAModel1.Range4 = ds.Tables[0].Rows[0]["range_4"].ToString();
                        _PAModel1.Range5 = ds.Tables[0].Rows[0]["range_5"].ToString();
                    }
                    return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/PendingAdvances/PendingAdvances.cshtml", _PAModel1);
                }
                else
                {

                    _PAModel.Title = title;
                    _PAModel.Command = cmd;
                    _PAModel.TransType = TYP;
                    _PAModel.BtnName = BTN;
                    DataSet ds = _PendingAdvances_ISERVICE.GetUserRangeDetail(CompID, UserID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _PAModel.Range1 = ds.Tables[0].Rows[0]["range_1"].ToString();
                        _PAModel.Range2 = ds.Tables[0].Rows[0]["range_2"].ToString();
                        _PAModel.Range3 = ds.Tables[0].Rows[0]["range_3"].ToString();
                        _PAModel.Range4 = ds.Tables[0].Rows[0]["range_4"].ToString();
                        _PAModel.Range5 = ds.Tables[0].Rows[0]["range_5"].ToString();
                    }
                        _PAModel.EntityType = "Cust";
                        _PAModel.ReportType = "S";
                        _PAModel.PendingAdvancesList = GetPendingAdvances_Detail(DateTime.Now.ToString("yyyy-MM-dd"), _PAModel.EntityType, _PAModel.ReportType);
                    return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/MIS/PendingAdvances/PendingAdvances.cshtml", _PAModel);
                }


            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
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
        
        public ActionResult UserRangeSave(PendingAdvancesModel _PaccModel, string command)
        {
            try
            {
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (_PaccModel.hdnCSVPrint == "CsvPrint")
                {
                    command = "CsvPrint";
                }
                switch (command)
                {
                    case "Save":
                        _PaccModel.Command = command;
                        SaveUserRange(_PaccModel);
                        TempData["ModelData"] = _PaccModel;
                        var cmd = _PaccModel.Command;
                        var TYP = _PaccModel.TransType;
                        var BTN = _PaccModel.BtnName;
                        Session["UserID"] = UserID;
                    return RedirectToAction("PendingAdvances", new { cmd, TYP, BTN });
                    case "Print":
                    case "CsvPrint":
                        return ExportPendingAdvancesData(_PaccModel, command);
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
        public ActionResult SaveUserRange(PendingAdvancesModel _PaccModel)
        {
            string SaveMessage = "";
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
                string range1 = _PaccModel.Range1;
                string range2 = _PaccModel.Range2;
                string range3 = _PaccModel.Range3;
                string range4 = _PaccModel.Range4;
                string range5 = _PaccModel.Range5;

                SaveMessage = _PendingAdvances_ISERVICE.InsertUserRangeDetail(CompID, user_id, range1, range2, range3, range4, range5);
                _PaccModel.Message = "Save";
                _PaccModel.Command = "Update";
                _PaccModel.TransType = "Update";
                _PaccModel.AppStatus = "D";
                _PaccModel.BtnName = "BtnToDetailPage";
                TempData["ModelData"] = _PaccModel;
                Session["UserID"] = UserID;
                return RedirectToAction("PendingAdvances");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private List<PRList> GetPendingAdvances_Detail(string AsDate, string EntityType,string ReportType)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<PRList> _PAListDetail = new List<PRList>();
                PendingAdvancesModel _PAModel = new PendingAdvancesModel();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
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
                Session["PAAging"] = "Search";
                ds = _PendingAdvances_ISERVICE.GetPendingAdvancesList(CompID, Br_ID, UserID, EntityType, AsDate,ReportType);
                _PAModel.PAAging = "Search";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PRList _PAList = new PRList();
                        _PAList.Entityname = dr["Entityname"].ToString();
                        _PAList.id = dr["id"].ToString();
                        _PAList.Curr = dr["curr"].ToString();
                        _PAList.CurrId = dr["curr_id"].ToString();
                        _PAList.AmtRange1 = dr["r1"].ToString();
                        _PAList.AmtRange2 = dr["r2"].ToString();
                        _PAList.AmtRange3 = dr["r3"].ToString();
                        _PAList.AmtRange4 = dr["r4"].ToString();
                        _PAList.AmtRange5 = dr["r5"].ToString();
                        _PAList.AmtRange6 = dr["gtr5"].ToString();
                        _PAList.AccId = dr["acc_id"].ToString();
                        _PAList.TotalAmt = dr["tamt"].ToString();
                        _PAModel.Range1 = dr["range1"].ToString();
                        _PAModel.Range2 = dr["range2"].ToString();
                        _PAModel.Range3 = dr["range3"].ToString();
                        _PAModel.Range4 = dr["range4"].ToString();
                        _PAModel.Range5 = dr["range5"].ToString();
                        _PAListDetail.Add(_PAList);
                    }
                }
                return _PAListDetail;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
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
        public ActionResult SearchPendingAdvancesDetail(string EntityType, string AsDate, string ReportType)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<PRList> _PAListDetail = new List<PRList>();
                PendingAdvancesModel _PendingAdvModel = new PendingAdvancesModel();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
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
                Session["PAAging"] = "Search";
                ds = _PendingAdvances_ISERVICE.GetPendingAdvancesList(CompID, Br_ID, UserID, EntityType, AsDate, ReportType);
                _PendingAdvModel.PAAging = "Search";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PRList _PRList = new PRList();
                        _PRList.Entityname = dr["Entityname"].ToString();
                        _PRList.id = dr["id"].ToString();
                        _PRList.Curr = dr["curr"].ToString();
                        _PRList.CurrId = dr["curr_id"].ToString(); 
                       
                        _PRList.AmtRange1 = dr["r1"].ToString();
                        _PRList.AmtRange2 = dr["r2"].ToString();
                        _PRList.AmtRange3 = dr["r3"].ToString();
                        _PRList.AmtRange4 = dr["r4"].ToString();
                        _PRList.AmtRange5 = dr["r5"].ToString();
                        _PRList.AmtRange6 = dr["gtr5"].ToString();
                        _PRList.AccId = dr["acc_id"].ToString();
                        _PRList.TotalAmt = dr["tamt"].ToString();
                        _PendingAdvModel.Range1 = dr["range1"].ToString();
                        _PendingAdvModel.Range2 = dr["range2"].ToString();
                        _PendingAdvModel.Range3 = dr["range3"].ToString();
                        _PendingAdvModel.Range4 = dr["range4"].ToString();
                        _PendingAdvModel.Range5 = dr["range5"].ToString();
                        if(ReportType == "D")
                        {
                            _PRList.voucher_no = dr["src_doc_no"].ToString();
                            _PRList.voucher_dt = dr["src_doc_dt"].ToString();
                        }
                        _PAListDetail.Add(_PRList);
                    }
                }

                _PendingAdvModel.PendingAdvancesList = _PAListDetail;
                ViewBag.EntityType = EntityType;
                ViewBag.ReportType = ReportType;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPendingAdvancesDetail.cshtml", _PendingAdvModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetPendingAdvancesList(string Acc_ID, string lrange, string urange, string EntityType, string AsDate, int CurrId, string ReportType)
        {
            try
            {
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                List<PendingAdvList> _AdvancesPayment = new List<PendingAdvList>();
                PendingAdvancesModel _PendingAdvModel = new PendingAdvancesModel();
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
                dt = _PendingAdvances_ISERVICE.GetPendingAdvancesAgingList(CompID, Br_ID,Acc_ID, lrange, urange, EntityType, AsDate, CurrId, ReportType);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PendingAdvList _AdvPaymentlist = new PendingAdvList();
                        _AdvPaymentlist.vou_date = dr[3].ToString();
                        _AdvPaymentlist.vou_no = dr[4].ToString();
                        _AdvPaymentlist.vou_type = dr[5].ToString();
                        _AdvPaymentlist.pend_amt = dr[7].ToString();
                        _AdvPaymentlist.Totalpendamt = dr[8].ToString();
                        _AdvancesPayment.Add(_AdvPaymentlist);
                    }
                }
                _PendingAdvModel.PendingAdvList = _AdvancesPayment;
                ViewBag.AdvancesPayment = _AdvancesPayment;
                ViewBag.DocID = DocumentMenuId;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAdvancePaymentDetails.cshtml", _PendingAdvModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public FileResult ExportPendingAdvancesData(PendingAdvancesModel _PaccModel, string command)
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
                //List<PRList> _PAListDetail = new List<PRList>();
                if (command == "CsvPrint")
                {
                    DataTable dataTable = new DataTable();
                    DataSet ds = new DataSet();
                    ds = _PendingAdvances_ISERVICE.GetPendingAdvancesList(CompID, Br_ID, UserID, _PaccModel.HdnEntityType, DateTime.Now.ToString("yyyy-MM-dd"), _PaccModel.ReportType);

                    var range1 = Convert.ToInt32(_PaccModel.Range1) + 1;
                    var range2 = Convert.ToInt32(_PaccModel.Range2) + 1;
                    var range3 = Convert.ToInt32(_PaccModel.Range3) + 1;
                    var range4 = Convert.ToInt32(_PaccModel.Range4) + 1;
                    var range5 = Convert.ToInt32(_PaccModel.Range5) + 1;

                    var r1 = "0" + "-" + _PaccModel.Range1 + ' ' + "Days";
                    var r2 = range1 + "-" + _PaccModel.Range2 + ' ' + "Days";
                    var r3 = range2 + "-" + _PaccModel.Range3 + ' ' + "Days";
                    var r4 = range3 + "-" + _PaccModel.Range4 + ' ' + "Days";
                    var r5 = range4 + "-" + _PaccModel.Range5 + ' ' + "Days";
                    var r6 = ">" + range5 + ' ' + "Days";


                    dataTable.Columns.Add("Sr. No.", typeof(int));
                    dataTable.Columns.Add("Entity Name", typeof(string));
                    dataTable.Columns.Add("Currency", typeof(string));
                    if(_PaccModel.ReportType == "D")
                    {
                        dataTable.Columns.Add("Voucher Namuber", typeof(string));
                        dataTable.Columns.Add("Voucher Date", typeof(string));
                    }
                    dataTable.Columns.Add(r1, typeof(decimal));
                    dataTable.Columns.Add(r2, typeof(decimal));
                    dataTable.Columns.Add(r3, typeof(decimal));
                    dataTable.Columns.Add(r4, typeof(decimal));
                    dataTable.Columns.Add(r5, typeof(decimal));
                    dataTable.Columns.Add(r6, typeof(decimal));
                    dataTable.Columns.Add("Total", typeof(decimal));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var SrNo = 1;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow rows = dataTable.NewRow();
                            rows["Sr. No."] = SrNo;
                            rows["Entity Name"] = dr["Entityname"].ToString();
                            rows["Currency"] = dr["curr"].ToString();
                            if (_PaccModel.ReportType == "D")
                            {
                                rows["Voucher Namuber"] = dr["src_doc_no"].ToString();
                                rows["Voucher Date"] = dr["src_doc_dt"].ToString();
                            }
                            rows[r1] = dr["r1"].ToString();
                            rows[r2] = dr["r2"].ToString();
                            rows[r3] = dr["r3"].ToString();
                            rows[r4] = dr["r4"].ToString();
                            rows[r5] = dr["r5"].ToString();
                            rows[r6] = dr["gtr5"].ToString();
                            rows["Total"] = dr["tamt"].ToString();
                            dataTable.Rows.Add(rows);
                            SrNo = SrNo + 1;
                        }
                        dt1 = dataTable;
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Pending Advances", dt1);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
        }
    }
}