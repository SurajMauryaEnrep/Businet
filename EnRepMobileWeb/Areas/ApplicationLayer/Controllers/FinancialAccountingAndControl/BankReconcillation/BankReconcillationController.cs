using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.BankRecancellation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.BankRecancellation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.BankRecancellation.BankRecancellation_Model;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.BankReconcillation
{
    public class BankReconcillationController : Controller
    {
        string CompID, BrchID, language = string.Empty, FromDate = string.Empty, ToDate = string.Empty, UserID=string.Empty;
        string MinDate = string.Empty,  ToMindate = string.Empty;
        string DocumentMenuId = "105104120", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        BankRecancellation_Iservices _BankRecancellation_Iservices;

        public BankReconcillationController(Common_IServices _Common_IServices, BankRecancellation_Iservices _bankRecancellation_Iservices)
        {
            this._Common_IServices = _Common_IServices;
            this._BankRecancellation_Iservices = _bankRecancellation_Iservices;
        }
        // GET: ApplicationLayer/BankReconcillation
        public ActionResult BankReconcillation(CommadsModel _commandsModel)
        {
            try
            {
                BankRecancellation_Model _bankRecancellation = new BankRecancellation_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                List<BankList> banklists = new List<BankList>();
                var ListBanks = GetBankLists(CompID);
                var ListOfBanks = ListBanks.ConvertAll(x => new BankList { bank_id = x.bank_id, bank_name = x.bank_name });
                _bankRecancellation._bankLists = ListOfBanks;

                DataSet dt = new DataSet();
                dt = GetFindates();
                FromDate = dt.Tables[0].Rows[0]["StartDate"].ToString();
                ToDate = dt.Tables[0].Rows[0]["EndDate"].ToString();
                MinDate = dt.Tables[0].Rows[0]["MinDate"].ToString();
                ToMindate = dt.Tables[1].Rows[0]["ToMindate"].ToString();
                _bankRecancellation.Fy_OpDate = FromDate;
                _bankRecancellation.Fy_ClDate = ToDate;
                _bankRecancellation.FyMinDate = MinDate;
                _bankRecancellation.ToMinDate = ToMindate;

                if (_commandsModel.Cmd == "Saved") {
                    _bankRecancellation.Message = "SaveReco";
                    _commandsModel.Cmd = null;
                }
                else if (_commandsModel.Cmd == "Used")
                {
                    _bankRecancellation.Message = "Used";
                    _commandsModel.Cmd = null;
                }
                ViewBag.MenuPageName = getDocumentName();
                _bankRecancellation.Title = title;
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/BankReconcillation/BankReconcillation.cshtml", _bankRecancellation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BankRecoCommands(BankRecancellation_Model ApproveModel)
        {
            try
            {/*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                CommadsModel _commandsModel = new CommadsModel();
                if (ApproveModel.Command == null)
                {
                    if (ApproveModel.HdnCsvPrint == "CsvPrint")
                    {
                        ApproveModel.Command = "CsvPrint";
                    }
                }
                switch (ApproveModel.Command)
                {
                    case "save":
                        /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string Voudt1 = ApproveModel.FyMinDate;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, BrchID, Voudt1);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (Msg == "FY Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                            }
                            else
                            {
                                TempData["FBMessage"] = "Financial Book Closing";
                            }
                            
                                ApproveModel.Command = "Refresh";
                                TempData["ModelData"] = ApproveModel;
                                return RedirectToAction("BankReconcillation", ApproveModel);
                        }
                        /*End to chk Financial year exist or not*/

                        SaveVoucherList(ApproveModel);
                        _commandsModel.Cmd = TempData["Message"].ToString();
                        return RedirectToAction("BankReconcillation", _commandsModel);

                    case "CsvPrint":
                        return BankReconcillationExporttoExcelDt(ApproveModel);
                }
                return RedirectToAction("BankReconcillation");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult BankReconcillationExporttoExcelDt(BankRecancellation_Model _Model)
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
                var searchValue = _Model.searchValue;
                if (searchValue == null)
                    searchValue = "0";
                DataSet Dt = _BankRecancellation_Iservices.GetSearchedData(CompID, BrchID, _Model.Acc_id, _Model.Fy_OpDate, _Model.ToDate, _Model.TransactionType, _Model.Status, searchValue);

                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("Bank Name", typeof(string));
                dt.Columns.Add("Voucher Number", typeof(string));
                dt.Columns.Add("Voucher Date", typeof(string));
                dt.Columns.Add("Voucher Type", typeof(string));
                dt.Columns.Add("Instrument Number", typeof(string));
                dt.Columns.Add("Instrument Date", typeof(string));
                dt.Columns.Add("Amount (In Base)", typeof(decimal));
                dt.Columns.Add("Amount (In Specific)", typeof(decimal));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Clearing Date", typeof(string));
                dt.Columns.Add("Reason for Return", typeof(string));

                if (Dt.Tables[0].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in Dt.Tables[0].Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        var StatusName = "";
                        var Status = dr["BankReco_Status"].ToString();
                        if (Status == "C")
                        {
                            StatusName = "Cleared";
                        }
                        else if (Status == "R")
                        {
                            StatusName = "Returned";
                        }
                        else
                        {
                            StatusName = "Un-Cleared";
                        }
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["Bank Name"] = dr["Bank_Name"].ToString();
                        dtrowLines["Voucher Number"] = dr["Vou_no"].ToString();
                        dtrowLines["Voucher Date"] = dr["Vou_dt"].ToString();
                        dtrowLines["Voucher Type"] = dr["Vou_Type"].ToString();
                        dtrowLines["Instrument Number"] = dr["Ins_Num"].ToString();
                        dtrowLines["Instrument Date"] = dr["Ins_dt"].ToString();
                        dtrowLines["Amount (In Base)"] = dr["Amount_Base"].ToString();
                        dtrowLines["Amount (In Specific)"] = dr["Amount_Spec"].ToString();
                        dtrowLines["Status"] = StatusName;
                        dtrowLines["Clearing Date"] = dr["Cl_Date"].ToString();
                        dtrowLines["Reason for Return"] = dr["ResnFrReturn"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                _Model.HdnCsvPrint = null;
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("OpeningBalance", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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

        public List<BankList> GetBankLists(string Comp_id)
        {

            try {
                DataSet Result = _BankRecancellation_Iservices.GetBankLists(Comp_id);
                DataTable DTdata = new DataTable();
                DTdata = Result.Tables[0];
                List<BankList> BankList1 = new List<BankList>();
                if (DTdata.Rows.Count > 0)
                {
                    foreach (DataRow data in DTdata.Rows)
                    {
                        BankList _Banklist = new BankList();
                        _Banklist.bank_id = data["acc_id"].ToString();
                        _Banklist.bank_name = data["acc_name"].ToString();
                        //_Banklist.Curr_Id = data["curr_id"].ToString();

                        BankList1.Add(_Banklist);
                    }
                    BankList1.Insert(0, new BankList() { bank_id = "0", bank_name = "---Select---" });

                }

                return BankList1;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public JsonResult GetCurrName(string BankId,string FromDate,string ToDate)
        {
            try
            {
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
                DataSet Result = _BankRecancellation_Iservices.GetBankCurr(CompID, BrchID, BankId, FromDate, ToDate);
                var DataRows = Json(JsonConvert.SerializeObject(Result), JsonRequestBehavior.AllowGet);

                //bankRecancellation_Model.Account_Bal =Result.Tables[2].Rows[0]["ClosBL"].ToString();
                return DataRows;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }

        public DataSet GetFindates()
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

                DataSet Result = _BankRecancellation_Iservices.GetFinYearDates(CompID, BrchID);
                return Result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult OnchangeGetFyDate(string ToDate,string FromDate,string Year)
        {
            try
            {
                Fy_Todate fy_Todate = new Fy_Todate();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataSet Dt = _BankRecancellation_Iservices.GetFyToDate(CompID, BrchID, ToDate, FromDate,Year);
                fy_Todate.FyToDate = Dt.Tables[0].Rows[0]["EndDate"].ToString();
                fy_Todate.ParMinDate = Dt.Tables[1].Rows[0]["MinDate"].ToString();
                return PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/BankReconcillation/PartialBankrecancelationTodate.cshtml", fy_Todate);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public JsonResult GetAccBalDetail(string acc_id, string Date)
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
                DataSet Result = _Common_IServices.GetAccountDetail(acc_id, CompID, Br_ID, Date);
                var DataRows = Json(JsonConvert.SerializeObject(Result), JsonRequestBehavior.AllowGet);
                return DataRows;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }

        public ActionResult SearchBankRecandetails(string acc_id,string FromDate, string ToDate,string TransType,string Status)
        {
            try
            {
                List<SerchedList>  _BankRecnclList = new List<SerchedList>();
                BankRecancellation_Model _bankRecancellation = new BankRecancellation_Model();
                
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
                var searchValue = "0";
                DataSet Dt = _BankRecancellation_Iservices.GetSearchedData(CompID, BrchID, acc_id, FromDate, ToDate, TransType, Status, searchValue);
                _bankRecancellation.VouSearch = "Vou_Search";
                
                if (Dt.Tables[0].Rows.Count > 0)
                {
                    
                    foreach (DataRow dr in Dt.Tables[0].Rows)
                    {
                        SerchedList _serchedList = new SerchedList();
                        _serchedList.Bank_Name = dr["Bank_Name"].ToString();
                        _serchedList.Vou_No = dr["Vou_no"].ToString();
                        _serchedList.Vou_dt = formatDate(dr["Vou_dt"].ToString()).ToString("yyyy-dd-MM");
                        _serchedList.VouDt = dr["Vou_dt"].ToString();
                        _serchedList.Vou_type = dr["Vou_Type"].ToString();
                        _serchedList.Ins_Num = dr["Ins_Num"].ToString();
                        if (dr["Ins_dt"].ToString() != "")
                        {
                            _serchedList.InsDt = formatDate(dr["Ins_dt"].ToString()).ToString("yyyy-dd-MM");
                            _serchedList.Ins_dt = dr["Ins_dt"].ToString();
                            _serchedList.InstrDate = dr["InstrDate"].ToString();
                        }
                        else
                        {
                            _serchedList.InsDt = "";
                            _serchedList.Ins_dt = "";
                            //_serchedList.InstrDate = dr["Ins_dt"].ToString();
                        }
                        

                        _serchedList.Amt_bs = dr["Amount_Base"].ToString();
                        _serchedList.Amt_sp = dr["Amount_Spec"].ToString();
                        _serchedList.Status = dr["BankReco_Status"].ToString();
                        if (_serchedList.Cl_dt != null || _serchedList.Cl_dt != "")
                        {
                            _serchedList.Cl_dt = dr["Cl_Date"].ToString();
                        }
                        else
                        {
                            _serchedList.Cl_dt = "";
                        }
                        //_serchedList.ClearDate = formatDate(dr["Cl_Date"].ToString()).ToString("yyyy-dd-MM");
                        _serchedList.Rsn_Ret = dr["ResnFrReturn"].ToString();
                        if(_serchedList.Status.ToString() == "C")
                        {
                            _serchedList.Status = "Cleared";
                        }
                        else if (_serchedList.Status.ToString() == "R")
                        {
                            _serchedList.Status = "Returned";
                        }
                        else 
                        {
                            _serchedList.Status = "Un-Cleared";
                        }
                        
                        _BankRecnclList.Add(_serchedList);
                    }
                    var a = Dt.Tables[1].Rows[0]["ClReceipt"].ToString();
                    _bankRecancellation.ClReceipt = Dt.Tables[1].Rows[0]["ClReceipt"].ToString();
                    _bankRecancellation.ClPayment = Dt.Tables[1].Rows[0]["ClPayment"].ToString();
                    _bankRecancellation.ReReceipt = Dt.Tables[1].Rows[0]["ReReceipt"].ToString();
                    _bankRecancellation.RePayment = Dt.Tables[1].Rows[0]["RePayment"].ToString();
                    _bankRecancellation.UnReceipt = Dt.Tables[1].Rows[0]["UnReceipt"].ToString();
                    _bankRecancellation.UnPayment = Dt.Tables[1].Rows[0]["UnPayment"].ToString();
                }
                _bankRecancellation._VoucherList = _BankRecnclList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBankReconcillation.cshtml", _bankRecancellation);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public DateTime formatDate(string date)
        {
            System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-US");
            string[] validformats = new[] { "MM-dd-yyyy", "MM/dd/yyyy","MM.dd.yyyy",
                                "dd-MM-yyyy","dd/MM/yyyy", "dd.MM.yyyy" ,
                                "yyyy-MM-dd","yyyy/MM/dd", "yyyy.MM.dd" ,
                                "yyyy-dd-MM","yyyy/dd/MM", "yyyy.dd.MM" };
           
                DateTime dateTime = DateTime.ParseExact(date, validformats, provider, System.Globalization.DateTimeStyles.None);
            return dateTime;
            
        }
        public ActionResult SaveVoucherList(BankRecancellation_Model _bankRecancellation_Model)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable BankRecoListSave = new DataTable();
               

                if (_bankRecancellation_Model.SaveVouList != "[]" && _bankRecancellation_Model.SaveVouList != null && _bankRecancellation_Model.SaveVouList != "")
                {
                    //BankRecoListSave.Columns.Add("comp_id", typeof(string));
                    //BankRecoListSave.Columns.Add("br_id", typeof(string));
                    //BankRecoListSave.Columns.Add("Bank_Name", typeof(string));
                    //BankRecoListSave.Columns.Add("Acc_id", typeof(string));
                    //BankRecoListSave.Columns.Add("vou_no", typeof(string));
                    //BankRecoListSave.Columns.Add("vou_dt", typeof(string));
                    //BankRecoListSave.Columns.Add("vou_type", typeof(string));
                    //BankRecoListSave.Columns.Add("ins_no", typeof(string));
                    //BankRecoListSave.Columns.Add("ins_dt", typeof(string));
                    //BankRecoListSave.Columns.Add("Vou_Status", typeof(string));
                    //BankRecoListSave.Columns.Add("amt_bs", typeof(string));
                    //BankRecoListSave.Columns.Add("amt_sp", typeof(string));
                    //BankRecoListSave.Columns.Add("Ins_type", typeof(string));
                    //BankRecoListSave.Columns.Add("BankReco_Status", typeof(string));
                    //BankRecoListSave.Columns.Add("ClearingDt", typeof(string));
                    //BankRecoListSave.Columns.Add("RsonFrRetrun", typeof(string));


                    //JArray JAObj = JArray.Parse(_bankRecancellation_Model.SaveVouList);
                    //for (int i = 0; i < JAObj.Count; i++)
                    //{
                    //    DataRow dtrowLines = BankRecoListSave.NewRow();

                    //    dtrowLines["comp_id"] = CompID;
                    //    dtrowLines["br_id"] = BrchID;
                    //    dtrowLines["Bank_Name"] = JAObj[i]["BankName"].ToString();
                    //    dtrowLines["Acc_id"] = JAObj[i]["Acc_id"].ToString();
                    //    dtrowLines["vou_no"] = JAObj[i]["VoucherNum"].ToString();
                    //    dtrowLines["vou_dt"] = formatDate(JAObj[i]["VoucherDate"].ToString()).ToString("yyyy-MM-dd");
                    //    dtrowLines["vou_type"] = JAObj[i]["Vouchertype"].ToString();
                    //    dtrowLines["ins_no"] = JAObj[i]["InstrumentNum"].ToString();
                    //    dtrowLines["ins_dt"] = formatDate(JAObj[i]["InstrumentDate"].ToString()).ToString("yyyy-MM-dd");
                    //    dtrowLines["Vou_Status"] = "";
                    //    dtrowLines["amt_bs"] = JAObj[i]["AmountBase"].ToString();
                    //    dtrowLines["amt_sp"] = JAObj[i]["AmoountSpec"].ToString();
                    //    dtrowLines["Ins_type"] = "";
                    //    if (JAObj[i]["Status"].ToString() != "")
                    //    {
                    //        dtrowLines["BankReco_Status"] = JAObj[i]["Status"].ToString();
                    //    }
                    //    else
                    //    {
                    //        dtrowLines["BankReco_Status"] = null;
                    //    }
                    //    if (JAObj[i]["ClearDate"].ToString() != "")
                    //    {
                    //        dtrowLines["ClearingDt"] = formatDate(JAObj[i]["ClearDate"].ToString()).ToString("yyyy-MM-dd");
                    //    }
                    //    else
                    //    {
                    //        dtrowLines["ClearingDt"] = null;
                    //    }
                    //    if (JAObj[i]["RsonFrReturn"].ToString() != "")
                    //    {
                    //        dtrowLines["RsonFrRetrun"] = JAObj[i]["RsonFrReturn"].ToString();
                    //    }
                    //    else
                    //    {
                    //        dtrowLines["RsonFrRetrun"] = null;
                    //    }

                    //    BankRecoListSave.Rows.Add(dtrowLines);
                    //}
                    BankRecoListSave = GetBankRecoListDataTable(_bankRecancellation_Model.SaveVouList);
                    var result = CheckAdvanceBeforeInsertUpdate(BankRecoListSave);
                    if (result.Item1 == "Used")
                    {
                        TempData["Message"] = "Used";
                    }
                    else
                    {
                        string SaveMessage = _BankRecancellation_Iservices.InsertUpdateBankreco(BankRecoListSave);
                        TempData["Message"] = "Saved";
                    }
                    
                    _bankRecancellation_Model.SaveVouList = null;
                    
                }
                return RedirectToAction("BankReconcillation");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
               // return View("~/Views/Shared/Error.cshtml");
            }

        }
        private DataTable GetBankRecoListDataTable( string RecoList)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable BankRecoListSave = new DataTable();
                BankRecoListSave.Columns.Add("comp_id", typeof(string));
                BankRecoListSave.Columns.Add("br_id", typeof(string));
                BankRecoListSave.Columns.Add("Bank_Name", typeof(string));
                BankRecoListSave.Columns.Add("Acc_id", typeof(string));
                BankRecoListSave.Columns.Add("vou_no", typeof(string));
                BankRecoListSave.Columns.Add("vou_dt", typeof(string));
                BankRecoListSave.Columns.Add("vou_type", typeof(string));
                BankRecoListSave.Columns.Add("ins_no", typeof(string));
                BankRecoListSave.Columns.Add("ins_dt", typeof(string));
                BankRecoListSave.Columns.Add("Vou_Status", typeof(string));
                BankRecoListSave.Columns.Add("amt_bs", typeof(string));
                BankRecoListSave.Columns.Add("amt_sp", typeof(string));
                BankRecoListSave.Columns.Add("Ins_type", typeof(string));
                BankRecoListSave.Columns.Add("BankReco_Status", typeof(string));
                BankRecoListSave.Columns.Add("ClearingDt", typeof(string));
                BankRecoListSave.Columns.Add("RsonFrRetrun", typeof(string));

                if (RecoList != "")
                {
                    JArray JAObj = JArray.Parse(RecoList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = BankRecoListSave.NewRow();

                        dtrowLines["comp_id"] = CompID;
                        dtrowLines["br_id"] = BrchID;
                        dtrowLines["Bank_Name"] = JAObj[i]["BankName"].ToString();
                        dtrowLines["Acc_id"] = JAObj[i]["Acc_id"].ToString();
                        dtrowLines["vou_no"] = JAObj[i]["VoucherNum"].ToString();
                        dtrowLines["vou_dt"] = formatDate(JAObj[i]["VoucherDate"].ToString()).ToString("yyyy-MM-dd");
                        dtrowLines["vou_type"] = JAObj[i]["Vouchertype"].ToString();
                        dtrowLines["ins_no"] = JAObj[i]["InstrumentNum"].ToString();
                        dtrowLines["ins_dt"] = formatDate(JAObj[i]["InstrumentDate"].ToString()).ToString("yyyy-MM-dd");
                        dtrowLines["Vou_Status"] = "";
                        dtrowLines["amt_bs"] = JAObj[i]["AmountBase"].ToString();
                        dtrowLines["amt_sp"] = JAObj[i]["AmoountSpec"].ToString();
                        dtrowLines["Ins_type"] = "";
                        if (JAObj[i]["Status"].ToString() != "")
                        {
                            dtrowLines["BankReco_Status"] = JAObj[i]["Status"].ToString();
                        }
                        else
                        {
                            dtrowLines["BankReco_Status"] = null;
                        }
                        if (JAObj[i]["ClearDate"].ToString() != "")
                        {
                            dtrowLines["ClearingDt"] = formatDate(JAObj[i]["ClearDate"].ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            dtrowLines["ClearingDt"] = null;
                        }
                        if (JAObj[i]["RsonFrReturn"].ToString() != "")
                        {
                            dtrowLines["RsonFrRetrun"] = JAObj[i]["RsonFrReturn"].ToString();
                        }
                        else
                        {
                            dtrowLines["RsonFrRetrun"] = null;
                        }

                        BankRecoListSave.Rows.Add(dtrowLines);
                    }

                }
                return BankRecoListSave;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public (string,DataTable) CheckAdvanceBeforeInsertUpdate(DataTable RecoList)
        {
            try
            {
                string result = "";

                DataSet ds = _BankRecancellation_Iservices.CheckBeforeInsertUpdateBankreco(RecoList);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        result = "Used";
                    }
                }

                return (result,ds.Tables[0]);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult CheckAdvancePayment(string DocNo, string DocDate,string doc_List)
        {
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
                if (doc_List != "")
                {
                    DataTable dt = GetBankRecoListDataTable(doc_List);
                    var result = CheckAdvanceBeforeInsertUpdate(dt);
                    if (result.Item1 == "Used")
                    {
                        return Json(JsonConvert.SerializeObject(result.Item2), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    DataSet Deatils = _BankRecancellation_Iservices.CheckAdvancePayment(Comp_ID, Br_ID, DocNo, DocDate);
                    if (Deatils.Tables[1].Rows.Count > 0)
                    {
                        str = "Used";
                    }
                    if (Deatils.Tables[2].Rows.Count > 0) /*Add by Hina sharma on 31-07-2024 for autogenerated*/
                    {
                        str = "AutoGen";
                    }
                    if (Deatils.Tables[3].Rows.Count > 0) /*Added by Suraj Maurya on 16-09-2024 Advance to customer*/
                    {
                        str = "Used";
                    }
                    if (Deatils.Tables[4].Rows.Count > 0) /*Added by Suraj Maurya on 16-09-2024 Advance to customer*/
                    {
                        str = "Used";
                    }
                    if (Deatils.Tables[0].Rows.Count > 0) /*Added by Suraj Maurya on 16-09-2024 Advance to customer*/
                    {
                        str = "Used";
                    }
                }
                
                //if (str != "" && str != null)
                //{
                //    _bankRecancellation_Model.BankPaymentNo = DocNo;
                //    _bankRecancellation_Model.BankPaymentDate = DocDate;
                //    _bankRecancellation_Model.TransType = "Update";
                //    _bankRecancellation_Model.BtnName = "BtnToDetailPage";
                //}
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json(ex.Message);
            }
            return Json(str);
        }

    }
}