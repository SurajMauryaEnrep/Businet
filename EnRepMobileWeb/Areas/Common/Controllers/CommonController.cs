using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.Common;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.ComponentModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net;
using System.Configuration;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Data.OleDb;

namespace EnRepMobileWeb.Areas.Common.Controllers.Common
{
    public class CommonController : Controller
    {
        string Comp_id, Br_id, User_id = string.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        // GET: Common/Common
        public CommonController(Common_IServices _Common_IServices)
        {
            this._Common_IServices = _Common_IServices;
        }
        public CommonController()
        {

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAutoCompletecity(Common_Model queryParameters)
        {
            string CityName = string.Empty;
            Dictionary<string, string> SuppCity = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ddlCityName))
                {
                    CityName = "0";
                }
                else
                {
                    CityName = queryParameters.ddlCityName;
                }
                SuppCity = _Common_IServices.SuppCityDAL(CityName);
                return Json(SuppCity.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetItemGstDetails(string ItemIDs, string gst_number)
        {

            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                JsonResult DataRows;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                DataSet result = _Common_IServices.GetItemGstDetails(Comp_ID, Br_ID, ItemIDs, gst_number);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetItemList2(BindItemList bindItem, string PageName)
        {

            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                DataSet SOItmList = _Common_IServices.GetItmListDL(Comp_ID, Br_ID, bindItem.SearchName, PageName);
                if (SOItmList.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    if (PageName == "PR" || PageName == "ScrapSI" || PageName == "slsSrt"
                        || PageName == "AdHocPrt" || PageName == "RFQ" || PageName == "PQ" || PageName == "DN" || PageName == "DNCPO" || PageName == "SO" || PageName == "SQ")
                    //    #region 
                    //    //add by shubham maurya on 03-07-2025 for add AdHocPrt
                    //  //  "RFQ" "PQ" Add ed BY Nitesh 15072025 For Add Request For Quatation And Puchase Quotation
                    //#endregion


                    {
                        for (int i = 0; i < SOItmList.Tables[0].Rows.Count; i++)
                        {
                            string itemId = SOItmList.Tables[0].Rows[i]["Item_id"].ToString();
                            string itemName = SOItmList.Tables[0].Rows[i]["Item_name"].ToString();
                            string Uom = SOItmList.Tables[0].Rows[i]["uom_name"].ToString();
                            string ItemType = SOItmList.Tables[0].Rows[i]["ItemType"].ToString();
                            ItemList.Add(itemId + "_" + Uom, itemName + "_" + ItemType);
                        }
                    }
                    else if (PageName == "PPL")/* Added by Suraj Maurya on 11-11-2025 Under ticket id : 3055 */
                    {
                        return Json(JsonConvert.SerializeObject(SOItmList), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        for (int i = 0; i < SOItmList.Tables[0].Rows.Count; i++)
                        {
                            string itemId = SOItmList.Tables[0].Rows[i]["Item_id"].ToString();
                            string itemName = SOItmList.Tables[0].Rows[i]["Item_name"].ToString();
                            string Uom = SOItmList.Tables[0].Rows[i]["uom_name"].ToString();
                            ItemList.Add(itemId + "_" + Uom, itemName);
                        }
                    }
                }
                //DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemList3(BindItemList bindItem, string PageName)
        {

            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                DataSet SOItmList = _Common_IServices.GetItmListDL1(Comp_ID, Br_ID, bindItem.SearchName, PageName);
                if (SOItmList.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < SOItmList.Tables[0].Rows.Count; i++)
                    {
                        string itemId = SOItmList.Tables[0].Rows[i]["Item_id"].ToString();
                        string itemName = SOItmList.Tables[0].Rows[i]["Item_name"].ToString();
                        string Uom = SOItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        string item_type = SOItmList.Tables[0].Rows[i]["item_type"].ToString();
                        ItemList.Add(itemId + "_" + Uom, itemName + "_" + item_type);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public DataTable GetApprovalLevel(string Comp_ID, string Br_ID, string DocumentMenuId)
        {
            try
            {

                DataSet ApprovalLevelList = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId);
                DataTable DT_AppLevel = new DataTable();
                DT_AppLevel = ApprovalLevelList.Tables[0];
                return DT_AppLevel;
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
        public DataTable GetBranchList(string Comp_ID)
        {
            try
            {
                DataTable dt = _Common_IServices.GetBrList(Comp_ID).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public JsonResult GetBaseCurrency()
        {
            try
            {
                string Comp_ID = "";
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _Common_IServices.GetBaseCurrency(Comp_ID).Tables[0];
                JsonResult DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetStatusList(string DocumentMenuId)
        {
            try
            {
                DataSet result = _Common_IServices.GetStatusList(DocumentMenuId);
                DataTable DTdata = new DataTable();
                DTdata = result.Tables[0];

                return DTdata;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }
        public List<statusLists1> GetStatusList1(string DocumentMenuId)
        {
            try
            {
                DataSet result = _Common_IServices.GetStatusList(DocumentMenuId);
                DataTable DTdata = new DataTable();
                DTdata = result.Tables[0];
                List<statusLists1> statusLists1 = new List<statusLists1>();
                if (DTdata.Rows.Count > 0)
                {
                    foreach (DataRow data in DTdata.Rows)
                    {
                        statusLists1 _Statuslist = new statusLists1();
                        _Statuslist.status_id = data["status_code"].ToString();
                        _Statuslist.status_name = data["status_name"].ToString();
                        statusLists1.Add(_Statuslist);
                    }
                }

                return statusLists1;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }
        [HttpPost]
        public JsonResult InsertForwardDetails(string docno, string docdate, string doc_id, string level, string forwardedto, string fstatus, string remarks)
        {
            JsonResult Validate = Json("");/*Validate Message*/
            string Status = string.Empty;
            string Comp_ID = string.Empty;
            string forwardedby = string.Empty;
            string BranchID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    forwardedby = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }

                Status = _Common_IServices.InsertForwardDetails(Comp_ID, BranchID, doc_id, docno, docdate, fstatus, forwardedto, forwardedby, level, remarks);

                Validate = Json(Status);
                //Send Email Alert
                _Common_IServices.SendAlertEmail(Comp_ID, BranchID, doc_id, docno, fstatus, forwardedby, forwardedto);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Validate;
        }
        [HttpPost]
        public JsonResult InsertForwardDetails1(string docno, string docdate, string doc_id, string level, string forwardedto, string fstatus, string remarks, string pdfPathToSendEmailAlert)
        {
            JsonResult Validate = Json("");/*Validate Message*/
            string Status = string.Empty;
            string Comp_ID = string.Empty;
            string forwardedby = string.Empty;
            string BranchID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    forwardedby = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }

                Status = _Common_IServices.InsertForwardDetails(Comp_ID, BranchID, doc_id, docno, docdate, fstatus, forwardedto, forwardedby, level, remarks);
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                // bool exists = System.IO.Directory.Exists(path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\"));
                string fullPdfPath = "";
                if (!string.IsNullOrEmpty(pdfPathToSendEmailAlert))
                {
                    bool exists = System.IO.Directory.Exists(path + ("..\\LogsFile\\EmailAlertPDFs\\"));
                    if (!exists)
                        //System.IO.Directory.CreateDirectory(path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\"));
                        System.IO.Directory.CreateDirectory(path + ("..\\LogsFile\\EmailAlertPDFs\\"));
                    //string fullPdfPath = path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\") + pdfPathToSendEmailAlert;
                    fullPdfPath = path + ("..\\LogsFile\\EmailAlertPDFs\\") + pdfPathToSendEmailAlert;
                }//
                try
                {
                    if (System.IO.File.Exists(fullPdfPath))
                        _Common_IServices.SendAlertEmail(Comp_ID, BranchID, doc_id, docno, fstatus, forwardedby, forwardedto, fullPdfPath);
                    else
                        _Common_IServices.SendAlertEmail(Comp_ID, BranchID, doc_id, docno, fstatus, forwardedby, forwardedto);
                }
                catch (Exception ex)
                {
                    string path_err = Server.MapPath("~");
                    Errorlog.LogError(path_err, ex);
                    return Json("ErrorInMail");
                }
                Validate = Json(Status);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Validate;
        }

        [HttpPost]
        public JsonResult GetWFLevel_Detail(string Doc_No, string Doc_Date, string DocumentMenuId, string DocStatus)
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
                DataSet result = _Common_IServices.GetWFLevel_Detail(Comp_ID, BranchID, Doc_No, Doc_Date, DocumentMenuId, DocStatus);
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
        [HttpPost]
        public ActionResult GetForwarderList(string Doc_ID)
        {
            JsonResult DataRows = null;
            string CompID = string.Empty;
            string Br_ID = string.Empty;
            string UserID = string.Empty;
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

                DataSet FW_List = _Common_IServices.Getfw_List(CompID, Br_ID, UserID, Doc_ID);
                DataRows = Json(JsonConvert.SerializeObject(FW_List));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        [HttpPost]
        public ActionResult GetForwardHistoryList(string Doc_no, string Doc_ID)
        {
            JsonResult DataRows = null;
            string CompID = string.Empty;
            string Br_ID = string.Empty;
            //string UserID = string.Empty;
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
                //if (Session["UserId"] != null)
                //{
                //    UserID = Session["UserId"].ToString();
                //}

                DataSet FWH_List = _Common_IServices.GetfwHistory_List(CompID, Br_ID, Doc_no, Doc_ID);
                DataRows = Json(JsonConvert.SerializeObject(FWH_List));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        [HttpPost]
        public JsonResult GetItemDetail(string ItemID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _Common_IServices.GetItemDetailDL(ItemID, Comp_ID);
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
        public ActionResult GetAddressdetail(string CustID, string CustPros_type, string Add_type, string add_id)
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
                DataTable result = _Common_IServices.GetAddressdetail(CustID, Comp_ID, CustPros_type, BranchID, Add_type, add_id);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.VBAddresslist = result;
                //Session["ILSearch"] = "IL_Search";

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return PartialView("~/Areas/Common/Views/_AddressInfomation.cshtml");
        }
        [HttpPost]
        public ActionResult GetSuppAddressdetail(string SuppID, string SuppPros_type)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string BranchID = "0";
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (SuppPros_type == "P")
                {
                    if (Session["BranchId"] != null)
                    {
                        BranchID = Session["BranchId"].ToString();
                    }
                }
                DataTable result = _Common_IServices.GetSuppAddressdetail(SuppID, Comp_ID, BranchID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.VBAddresslist = result;
                //Session["ILSearch"] = "IL_Search";

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return PartialView("~/Areas/Common/Views/_AddressInfomation.cshtml");
        }
        [HttpPost]
        public JsonResult GetItemCustomerInfo(string ItemID, string CustID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _Common_IServices.GetItemCustomerInfo(ItemID, CustID, Comp_ID);
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
        [HttpPost]
        public JsonResult GetItemSupplierInfo(string ItemID, string SuppID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _Common_IServices.GetItemSupplierInfo(ItemID, SuppID, Comp_ID);
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

        [HttpPost]
        public ActionResult GetItemList(string PageName)
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
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
                DataSet SOItmList = _Common_IServices.GetItmListDL(Comp_ID, Br_ID, "0", PageName);
                DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult GetAccountList(string DocMenuID, string gl_curr_id, string BranchId = null)
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            //string DocMenuID = "105104115101";

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
                if (!string.IsNullOrEmpty(BranchId))
                {
                    Br_ID = BranchId;
                }
                DataSet AccountList = _Common_IServices.GetAccountListDDL(Comp_ID, Br_ID, "", DocMenuID, gl_curr_id);
                DataRow dr;
                #region Added BY Nitesh two Document ID (PR,SR)105102150, 105103142
                #endregion
                if (DocMenuID != "105103147" && DocMenuID != "105101150" && DocMenuID != "105101145" && DocMenuID != "105103140" && DocMenuID != "105103148" && DocMenuID != "105103142" && DocMenuID != "105102150")/*remove select form Sale ddl a/c on page of Service sale Invoice by Hina on 05-10-2023*/
                {
                    if (DocMenuID == "105104115120")
                    {
                        dr = AccountList.Tables[0].NewRow();
                        dr[0] = "0";
                        dr[1] = "---Select---";
                        dr[2] = "0";
                        dr[3] = "0";
                        AccountList.Tables[0].Rows.InsertAt(dr, 0);
                    }
                    else
                    {
                        dr = AccountList.Tables[0].NewRow();
                        dr[0] = "0";
                        dr[1] = "---Select---";
                        AccountList.Tables[0].Rows.InsertAt(dr, 0);
                    }
                }
                DataRows = Json(JsonConvert.SerializeObject(AccountList), JsonRequestBehavior.AllowGet);/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        public ActionResult GetAccountList2(BindItemList bindItem, string DocMenuID, string gl_curr_id, string BranchId = null)/*Created by Suraj on 04-09-2024 for Dynamic Searchable GL Account dropdown*/
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            //string DocMenuID = "105104115101";
            /* gl_curr_id passed by Suraj Maurya on 04-04-2025 */
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
                if (!string.IsNullOrEmpty(BranchId))
                {
                    Br_ID = BranchId;
                }
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                DataSet AccountList = _Common_IServices.GetAccountListDDL(Comp_ID, Br_ID, bindItem.SearchName, DocMenuID, gl_curr_id);
                DataRow dr;
                #region Added BY Nitesh two Document ID (PR,SR)105102150, 105103142
                #endregion
                if (DocMenuID != "105103147" && DocMenuID != "105101150" && DocMenuID != "105101145" && DocMenuID != "105103140" && DocMenuID != "105103148" && DocMenuID != "105103142" && DocMenuID != "105102150")/*remove select form Sale ddl a/c on page of Service sale Invoice by Hina on 05-10-2023*/
                {
                    if (DocMenuID == "105104115120")
                    {
                        dr = AccountList.Tables[0].NewRow();
                        dr[0] = "0";
                        dr[1] = "---Select---";
                        dr[2] = "0";
                        dr[3] = "0";
                        dr[4] = "0";
                        AccountList.Tables[0].Rows.InsertAt(dr, 0);
                    }
                    else
                    {
                        dr = AccountList.Tables[0].NewRow();
                        dr[0] = "0";
                        dr[1] = "---Select---";
                        AccountList.Tables[0].Rows.InsertAt(dr, 0);
                    }
                }
                DataRows = Json(JsonConvert.SerializeObject(AccountList), JsonRequestBehavior.AllowGet);/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public ActionResult GetAccountList1(BindItemList bindItem, string DocMenuID)
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            //string DocMenuID = "105104115101";

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
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                DataSet SOItmList = _Common_IServices.GetAccountListDDL(Comp_ID, Br_ID, bindItem.SearchName, DocMenuID);
                if (SOItmList.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    for (int i = 0; i < SOItmList.Tables[0].Rows.Count; i++)
                    {
                        string acc_id = SOItmList.Tables[0].Rows[i]["acc_id"].ToString();
                        string acc_name = SOItmList.Tables[0].Rows[i]["acc_name"].ToString();
                        //string Uom = SOItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        ItemList.Add(acc_id, acc_name);
                    }
                }
                //DataSet AccountList = _Common_IServices.GetAccountListDDL(Comp_ID, Br_ID, "", DocMenuID);
                //DataRow dr;
                //dr = AccountList.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "---Select---";
                //AccountList.Tables[0].Rows.InsertAt(dr, 0);
                //DataRows = Json(JsonConvert.SerializeObject(AccountList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAccountGroup(string Acc_ID)
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
                DataSet result = _Common_IServices.GetAccGrpDDL(Comp_ID, Acc_ID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        [HttpPost]
        public JsonResult GetAccountBalance(string Acc_ID, string VouDate)
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
                DataSet result = _Common_IServices.GetAccBalance(Comp_ID, Br_ID, Acc_ID, VouDate);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        public ActionResult getWarehouseWiseItemStock(string ItemId, string WarehouseId, string UomId = null, string br_id = null, string DocumentMenuId = null)
        {
            try
            {
                string Wh_ID, ItemID, LotID, BatchNo = string.Empty;
                //CompID = CompId;
                //BrID = BranchId;
                string CompID = "", BrID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                if (DocumentMenuId == "105102140")
                {
                    BrID = br_id;
                }
                Wh_ID = WarehouseId;
                ItemID = ItemId;
                LotID = null;
                BatchNo = null;
                string Stock = _Common_IServices.getWarehouseWiseItemStock(CompID, BrID, Wh_ID, ItemID, UomId, LotID, BatchNo, DocumentMenuId);
                return Json(Stock, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        [HttpPost]
        public ActionResult GetItemSOHistory(string ItemID, string CustID, string FinStDt, string Date12)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BranchID = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataTable result = _Common_IServices.GetItemSOHistory(ItemID, CustID, FinStDt, Date12, Comp_ID, BranchID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.DocumentMenuId = "105103125";
                ViewBag.VBSOhistorylist = result;
                Session["ILSearch"] = "IL_Search";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_SaleHistory.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }
        [HttpPost]
        public ActionResult GetCustomerSalesHistory(string CustID, string FinStDt, string Date12)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BranchID = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataTable result = _Common_IServices.GetCustomerSalesHistory(CustID, FinStDt, Date12, Comp_ID, BranchID);
                ViewBag.VBCustomerSaleshistorylist = result;
                Session["ILSearch"] = "IL_Search";
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                //ViewBag.DocumentMenuId = "105103125";

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialCustomerSaleHistory.cshtml");
        }

        [HttpPost]
        public ActionResult GetItemPOHistory(string ItemID, string SuppID, string FinStDt, string Date12, string DMenuId)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BranchID = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataTable result = _Common_IServices.GetItemPOHistory(ItemID, SuppID, FinStDt, Date12, DMenuId, Comp_ID, BranchID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                if (DMenuId == "105108101")
                {
                    ViewBag.DocumentMenuId = "105108101";
                }
                else
                {
                    ViewBag.DocumentMenuId = "105101130";
                }

                ViewBag.VBPOhistorylist = result;
                Session["ILSearch"] = "IL_Search";
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/_SaleHistory.cshtml");
        }
        [HttpPost]
        public JsonResult GetItemUOM(string Itm_ID, string ItemUomType)
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
                DataSet result = _Common_IServices.GetItemUOMDL(Itm_ID, Comp_ID, Br_ID, ItemUomType);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        [HttpPost]
        public JsonResult GetItemAvlStock(string Itm_ID)
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
                DataSet result = _Common_IServices.GetItemAvlStock(Comp_ID, Br_ID, Itm_ID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        [HttpPost]
        public JsonResult GetItemAvlStockShopfloor(string Itm_ID, string MaterialType, string SourceShopfloor)
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
                DataSet result = _Common_IServices.GetItemAvlStockShopfloor(Comp_ID, Br_ID, Itm_ID, MaterialType, SourceShopfloor);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        [HttpPost]
        public JsonResult GetOtherChargeList()
        {
            JsonResult DataRows = null;
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
                DataSet SO_OC = _Common_IServices.GetOtherChargeDAL(Comp_ID, Br_ID);
                DataRows = Json(JsonConvert.SerializeObject(SO_OC));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        [HttpPost]
        public ActionResult cmn_get_voucherdetails(string vou_no, string vou_dt, string flag, string narr)
        {
            //JsonResult DataRows = null;
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
                DataSet acc_details = _Common_IServices.Get_VoucherDetails(Comp_ID, Br_ID, vou_no, vou_dt, flag, narr);
                if (acc_details.Tables[0].Rows.Count > 0 && acc_details.Tables[1].Rows.Count > 0)
                {
                    ViewBag.mwvoudetailsdetail = acc_details.Tables[0];
                    ViewBag.mwvoudrcr = acc_details.Tables[1];
                }

                return PartialView("~/Areas/Common/Views/Comn_PartialGeneralLedgerVoucherDetailMW.cshtml");
                //DataRows = Json(JsonConvert.SerializeObject(acc_details));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }

        [HttpPost]
        public ActionResult cmn_getcostcenterdetails(string vou_no, string vou_dt, string acc_id)
        {
            //JsonResult DataRows = null;
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
                DataSet cc_details = _Common_IServices.get_Costcenterdetails(Comp_ID, Br_ID, vou_no, vou_dt, acc_id);

                if (cc_details.Tables[0].Rows.Count > 0 && cc_details.Tables[1].Rows.Count > 0)
                {
                    ViewBag.ccdetail = cc_details.Tables[0];
                    ViewBag.ccvaldetail = cc_details.Tables[1];
                }

                return PartialView("~/Areas/Common/Views/Cmn_PartialCostCenterDetailDisplay.cshtml");
                //DataRows = Json(JsonConvert.SerializeObject(cc_details));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }

        [HttpPost]
        public JsonResult GetTaxTypeList()
        {
            JsonResult DataRows = null;
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
                DataSet SOTaxList = _Common_IServices.GetTaxListDAL(Comp_ID, Br_ID);
                DataRows = Json(JsonConvert.SerializeObject(SOTaxList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetTaxPercentList(string TaxID)
        {
            JsonResult DataRows = null;
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
                DataSet SOTaxList = _Common_IServices.GetTaxPercentageDAL(Comp_ID, Br_ID, TaxID);
                DataRows = Json(JsonConvert.SerializeObject(SOTaxList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        public ActionResult getItemstockWareHouselWise(string ItemId, string UomId = null, string DocumentMenuId = null)
        {
            try
            {
                string CompID = "", BrchID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds;
                ds = _Common_IServices.getItemstockWarehouseWise(ItemId, UomId, CompID, BrchID, DocumentMenuId);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockWareHouselWise = ds.Tables[0];
                return PartialView("~/Areas/Common/Views/PartialItemStockWareHouseWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult getItemstockShopFloorWise(string ItemId, string UomId)
        {
            try
            {
                string CompID = "", BrchID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds;
                ds = _Common_IServices.getItemstockShopFloorWise(ItemId, UomId, CompID, BrchID);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockShopFloorWise = ds.Tables[0];

                ViewBag.ShopFloorStk = "Y";
                return PartialView("~/Areas/Common/Views/PartialItemStockWareHouseWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public JsonResult BindTaxSlablist(string DocumentMenuId)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet SOTaxSlabList = _Common_IServices.BindTaxSlablist(Comp_ID, DocumentMenuId);
                //DataRow dr;
                //dr = SOTaxSlabList.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "---Select---";
                //SOTaxSlabList.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(SOTaxSlabList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public JsonResult BindTaxTemplatelist(string DocumentMenuId, string Tmplt_type)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet SOTaxTemplateList = _Common_IServices.BindTaxTemplatelist(Comp_ID, DocumentMenuId, Tmplt_type);
                DataRows = Json(JsonConvert.SerializeObject(SOTaxTemplateList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public JsonResult BindTaxTemplateData(string tmplt_id, string TaxSlab, string ItemId, string GSTNo)
        {
            JsonResult DataRows = null;
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
                DataSet SOTaxTemplateList = _Common_IServices.BindTaxTemplateData(Comp_ID, tmplt_id, TaxSlab, Br_ID, ItemId, GSTNo);
                DataRows = Json(JsonConvert.SerializeObject(SOTaxTemplateList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        public JsonResult BindTermsTemplatelist(string DocumentMenuId)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet TermsTemplateList = _Common_IServices.BindTermsTemplatelist(Comp_ID, DocumentMenuId);
                DataRows = Json(JsonConvert.SerializeObject(TermsTemplateList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public JsonResult BindTermsTemplateData(string tmplt_id)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet termsTemplateList = _Common_IServices.BindTermsTemplateData(Comp_ID, tmplt_id);
                DataRows = Json(JsonConvert.SerializeObject(termsTemplateList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        /*---------------------Attatchment -----------------------------*/
        public void DeleteTempFile(string comp_id, string PageName, string ItmCode, HttpServerUtilityBase Server)
        {
            if (!string.IsNullOrEmpty(comp_id) && !string.IsNullOrEmpty(PageName) && !string.IsNullOrEmpty(ItmCode))
            {
                string DocPath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                if (Directory.Exists(DocPath))
                {
                    string[] filePaths = Directory.GetFiles(DocPath, comp_id + ItmCode + "*");

                    foreach (var fielpath in filePaths)
                    {
                        System.IO.File.Delete(fielpath);
                    }
                }

            }


        }
        public DataTable Upload(string PageName, string TransType, string comp_id, string DocId
         , HttpFileCollectionBase Files, HttpServerUtilityBase Server, string user_id = "0")
        {
            try
            {
                if (!string.IsNullOrEmpty(PageName) && !string.IsNullOrEmpty(TransType) && !string.IsNullOrEmpty(comp_id)
                    && !string.IsNullOrEmpty(DocId) && Files.Count > 0)
                {
                    DataTable dtAttachment = new DataTable();
                    dtAttachment.Columns.Add("id", typeof(string));
                    dtAttachment.Columns.Add("file_name", typeof(string));
                    dtAttachment.Columns.Add("file_path", typeof(string));
                    dtAttachment.Columns.Add("file_def", typeof(char));
                    dtAttachment.Columns.Add("comp_id", typeof(Int32));

                    for (int i = 0; i < Files.Count; i++)
                    {
                        HttpPostedFileBase file = Files[i]; //Uploaded file
                        //Use the following properties to get file's name, size and MIMEType
                        int fileSize = file.ContentLength;
                        char addindex = Convert.ToChar(i);

                        string OldFileName = file.FileName;
                        //string fileName = file.FileName
                        string[] fileNames = file.FileName.Split('.');
                        int sufixIndex = (fileNames.Length - 1);
                        string sufix = fileNames[sufixIndex];
                        //string fileName = file.FileName.Split('.')[0] + (i + 1) + '.' + file.FileName.Split('.')[1];
                        string fileName = file.FileName.Substring(0, (file.FileName.Length - sufix.Length - 1)).Replace('#', '_')
                            .Replace('%', '_').Replace('+', '_');// + (i + 1) + '.' + sufix;
                        int len = 0;
                        if (PageName == "ItemSetup")
                        {
                            if (TransType == "Save")
                            {
                                len = (comp_id + "_" + (i + 1) + '.' + sufix).Length + 20;
                            }
                            len = (comp_id + DocId + "_" + (i + 1) + '.' + sufix).Length;
                        }
                        else
                        {
                            len = (comp_id + DocId + "_" + (i + 1) + '.' + sufix).Length;
                        }


                        if (fileName.Length + len > 50)
                        {
                            fileName = fileName.Substring(0, (50 - len));
                        }
                        string mimeType = file.ContentType;
                        System.IO.Stream fileContent = file.InputStream;
                        //To save file, use SaveAs method
                        string NewFileName = Path.GetFileName(fileName + (i + 1) + '.' + sufix).ToString();
                        string img_nm = comp_id + DocId + "_" + NewFileName;// Path.GetFileName(fileName + (i + 1) + '.' + sufix).ToString();
                        // string doc_path = Path.Combine(Server.MapPath("~/Attachment/" + PageName + "/"), img_nm);
                        string path = System.Web.HttpContext.Current.Server.MapPath("~");
                        string doc_path = "";
                        string folderPath = "";

                        if (PageName == "ItemSetup")
                        {
                            string currentDir = Environment.CurrentDirectory;
                            DirectoryInfo directory = new DirectoryInfo(currentDir);
                            string tempFolderName = comp_id + user_id;
                            folderPath = path + ("..\\TempAttachment\\" + tempFolderName + "\\");
                            if (!Directory.Exists(folderPath))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(folderPath);
                            }


                            if (TransType == "Save")
                            {

                                doc_path = Path.Combine(folderPath + "\\", (img_nm + "§" + OldFileName));


                                if (!Directory.Exists(doc_path))
                                {
                                    file.SaveAs(doc_path);
                                }
                            }
                            else
                            {
                                doc_path = Path.Combine(folderPath + "\\", img_nm);
                                if (!Directory.Exists(doc_path))
                                {
                                    file.SaveAs(doc_path);
                                }
                            }

                        }
                        else
                        {
                            string currentDir = Environment.CurrentDirectory;
                            DirectoryInfo directory = new DirectoryInfo(currentDir);
                            folderPath = path + ("..\\Attachment\\");
                            if (!Directory.Exists(folderPath + PageName))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(folderPath + PageName);
                            }
                            doc_path = Path.Combine(folderPath + PageName + "\\", img_nm);
                            if (!Directory.Exists(doc_path))
                            {
                                file.SaveAs(doc_path);
                            }
                        }


                        //file.SaveAs(doc_path);  //File will be saved in application root
                        DataRow dtrowAttachment = dtAttachment.NewRow();
                        if (TransType == "Update")
                        {
                            dtrowAttachment["id"] = DocId;
                            dtrowAttachment["file_name"] = img_nm;
                            dtrowAttachment["file_path"] = doc_path;
                        }
                        else
                        {
                            dtrowAttachment["id"] = "0";
                            dtrowAttachment["file_name"] = NewFileName;//fileName;
                            dtrowAttachment["file_path"] = folderPath + PageName + "\\";
                        }
                        dtrowAttachment["file_def"] = 'Y';

                        dtrowAttachment["comp_id"] = comp_id;
                        dtAttachment.Rows.Add(dtrowAttachment);
                    }
                    return dtAttachment;
                }
                else
                {
                    //DataTable dt=new DataTable();
                    return null;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public string ResetImageLocation(string compId, string brId, string guid, string PageName, string FSO_Id, string transtype, DataTable Attachments)
        {
            try
            {

                string sourcePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "\\";
                if (Directory.Exists(sourcePath))
                {
                    //string[] filePaths = Directory.GetFiles(sourcePath, compId + brId + guid + "_" + "*");
                    string[] filePaths = Directory.GetFiles(sourcePath, compId + brId + guid + "_" + "*");
                    if (brId == "00")
                        filePaths = Directory.GetFiles(sourcePath, compId + guid + "_" + "*");
                    if (filePaths.Length > 0)
                    {
                        /*---------------To Remove Unused files start----------------------*/
                        string DrItmNm2 = Attachments.Rows[0]["file_name"].ToString();
                        string DrItmNm1 = DrItmNm2.Split('_')[0];
                        string[] filePaths1 = Directory.GetFiles(sourcePath, DrItmNm1 + "_" + "*");
                        foreach (string file in filePaths1)
                        {
                            string[] items = file.Split('\\');
                            string ItemName = items[items.Length - 1];
                            //ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            string flag = "N";
                            foreach (DataRow dr in Attachments.Rows)
                            {
                                string DrItmNm = dr["file_name"].ToString();
                                if (ItemName == DrItmNm)
                                {
                                    flag = "Y";
                                }
                            }
                            if (flag == "N")
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                        /*---------------To Remove Unused files end----------------------*/
                        foreach (string file in filePaths)
                        {
                            string[] items = file.Split('\\');
                            string ItemName = items[items.Length - 1];
                            ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            foreach (DataRow dr in Attachments.Rows)
                            {
                                string DrItmNm = dr["file_name"].ToString();
                                if (transtype == "Update")
                                {
                                    DrItmNm = DrItmNm.Substring(DrItmNm.IndexOf('_') + 1);
                                }
                                if (ItemName == DrItmNm)
                                {
                                    string path = System.Web.HttpContext.Current.Server.MapPath("~");
                                    string folderPath = path + ("..\\Attachment\\");
                                    string FSOID1 = FSO_Id.Replace("/", "");


                                    string img_nm = "";
                                    if (brId == "00")
                                        img_nm = compId + FSOID1 + "_" + Path.GetFileName(DrItmNm).ToString();
                                    else
                                        img_nm = compId + brId + FSOID1 + "_" + Path.GetFileName(DrItmNm).ToString();
                                    string doc_path = Path.Combine(folderPath + PageName + "\\", img_nm);
                                    string DocumentPath = folderPath + PageName + "\\";
                                    if (!Directory.Exists(DocumentPath))
                                    {
                                        DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                                    }
                                    System.IO.File.Move(file, doc_path);
                                }
                            }
                        }
                    }

                }
                //Session["Guid"] = null;
                return "";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public string ResetImageWithoutBrLocation(string compId, string brId, string guid, string PageName, string FSO_Id, string transtype, DataTable Attachments)
        {
            try
            {

                string sourcePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "\\";
                if (Directory.Exists(sourcePath))
                {
                    string[] filePaths = Directory.GetFiles(sourcePath, compId + brId + guid + "_" + "*");
                    if (brId == "00")
                        filePaths = Directory.GetFiles(sourcePath, compId + guid + "_" + "*");
                    if (filePaths.Length > 0)
                    {
                        /*---------------To Remove Unused files start----------------------*/
                        string DrItmNm2 = Attachments.Rows[0]["file_name"].ToString();
                        string DrItmNm1 = DrItmNm2.Split('_')[0];
                        string[] filePaths1 = Directory.GetFiles(sourcePath, DrItmNm1 + "_" + "*");
                        foreach (string file in filePaths1)
                        {
                            string[] items = file.Split('\\');
                            string ItemName = items[items.Length - 1];
                            //ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            string flag = "N";
                            foreach (DataRow dr in Attachments.Rows)
                            {
                                string DrItmNm = dr["file_name"].ToString();
                                if (ItemName == DrItmNm)
                                {
                                    flag = "Y";
                                }
                            }
                            if (flag == "N")
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                        /*---------------To Remove Unused files end----------------------*/
                        foreach (string file in filePaths)
                        {
                            string[] items = file.Split('\\');
                            string ItemName = items[items.Length - 1];
                            ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            foreach (DataRow dr in Attachments.Rows)
                            {
                                string DrItmNm = dr["file_name"].ToString();
                                if (transtype == "Update")
                                {
                                    DrItmNm = DrItmNm.Substring(DrItmNm.IndexOf('_') + 1);
                                }
                                if (ItemName == DrItmNm)
                                {
                                    string path = System.Web.HttpContext.Current.Server.MapPath("~");
                                    string folderPath = path + ("..\\Attachment\\");
                                    string FSOID1 = FSO_Id.Replace("/", "");
                                    //string img_nm = compId + brId + FSOID1 + "_" + Path.GetFileName(DrItmNm).ToString();
                                    string img_nm = "";
                                    if (brId == "00")
                                        img_nm = compId + FSOID1 + "_" + Path.GetFileName(DrItmNm).ToString();
                                    else
                                        img_nm = compId + brId + FSOID1 + "_" + Path.GetFileName(DrItmNm).ToString();
                                    string doc_path = Path.Combine(folderPath + PageName + "\\", img_nm);
                                    string DocumentPath = folderPath + PageName + "\\";
                                    if (!Directory.Exists(DocumentPath))
                                    {
                                        DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                                    }
                                    System.IO.File.Move(file, doc_path);
                                }
                            }
                        }
                    }

                }
                //Session["Guid"] = null;
                return "";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        //public DataTable Upload(string PageName, string TransType, string comp_id, string DocId, HttpFileCollectionBase Files, HttpServerUtilityBase Server)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(PageName) && !string.IsNullOrEmpty(TransType) && !string.IsNullOrEmpty(comp_id)
        //            && !string.IsNullOrEmpty(DocId) && Files.Count > 0)
        //        {


        //            DataTable dtAttachment = new DataTable();
        //            dtAttachment.Columns.Add("id", typeof(string));
        //            dtAttachment.Columns.Add("file_name", typeof(string));
        //            dtAttachment.Columns.Add("file_path", typeof(string));
        //            dtAttachment.Columns.Add("file_def", typeof(char));
        //            dtAttachment.Columns.Add("comp_id", typeof(Int32));

        //            for (int i = 0; i < Files.Count; i++)
        //            {
        //                HttpPostedFileBase file = Files[i]; //Uploaded file
        //                                                    //Use the following properties to get file's name, size and MIMEType
        //                int fileSize = file.ContentLength;
        //                char addindex = Convert.ToChar(i);
        //                //string fileName = file.FileName
        //                string fileName = file.FileName.Split('.')[0] + (i + 1) + '.' + file.FileName.Split('.')[1];
        //                string mimeType = file.ContentType;
        //                System.IO.Stream fileContent = file.InputStream;
        //                //To save file, use SaveAs method

        //                string img_nm = comp_id + DocId + "_" + Path.GetFileName(fileName).ToString();
        //                string doc_path = Path.Combine(Server.MapPath("~/Attachment/" + PageName + "/"), img_nm);
        //                string DocumentPath = Server.MapPath("~/Attachment/" + PageName + "/");
        //                if (!Directory.Exists(DocumentPath))
        //                {
        //                    DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
        //                }
        //                if (!Directory.Exists(doc_path))
        //                {
        //                    file.SaveAs(doc_path);
        //                }
        //                //file.SaveAs(doc_path);  //File will be saved in application root
        //                DataRow dtrowAttachment = dtAttachment.NewRow();


        //                if (TransType == "Update")
        //                {
        //                    dtrowAttachment["id"] = DocId;
        //                    dtrowAttachment["file_name"] = img_nm;
        //                    dtrowAttachment["file_path"] = doc_path;
        //                }
        //                else
        //                {
        //                    dtrowAttachment["id"] = "0";
        //                    dtrowAttachment["file_name"] = fileName;
        //                    dtrowAttachment["file_path"] = DocumentPath;
        //                }

        //                dtrowAttachment["file_def"] = 'Y';
        //                dtrowAttachment["comp_id"] = comp_id;
        //                dtAttachment.Rows.Add(dtrowAttachment);


        //            }

        //            return dtAttachment;
        //        }
        //        else
        //        {
        //            //DataTable dt=new DataTable();
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }

        //}
        /*---------------------Attatchment -----------------------------*/
        public JsonResult GetOCTaxTmpltDT(string OC_id)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet OCTaxTmplt = _Common_IServices.GetTaxTemplateByOC(Comp_ID, OC_id);
                DataRows = Json(JsonConvert.SerializeObject(OCTaxTmplt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult GetSubItemShflAvlstockDetails(string shfl_id, string Item_id, string stkType)
        {
            //JsonResult DataRows = null;
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
                DataSet ds = _Common_IServices.GetSubItemShflAvlstockDetails(Comp_ID, Br_ID, shfl_id, Item_id, stkType);
                ViewBag.SubitemAvlStockDetail = ds.Tables[0];
                ViewBag.Flag = "Shopfloor";
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                //DataRows = Json(JsonConvert.SerializeObject(OCTaxTmplt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }

        public ActionResult GetSubItemWIPstockDetails(string shfl_id, string Item_id)
        {
            //JsonResult DataRows = null;
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
                DataSet ds = _Common_IServices.GetSubItemWIPstockDetails(Comp_ID, Br_ID, shfl_id, Item_id);
                ViewBag.SubitemAvlStockDetail = ds.Tables[0];
                ViewBag.Flag = "WIP";
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                //DataRows = Json(JsonConvert.SerializeObject(OCTaxTmplt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }
        public ActionResult GetSubItemWhAvlstockDetails(string Wh_id, string Item_id, string flag, string DocumentMenuId, string UomId = null, string br_id = null)
        {
            //JsonResult DataRows = null;
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
                if (DocumentMenuId == "105102140")
                {
                    Br_ID = br_id;
                }
                DataSet ds = _Common_IServices.GetSubItemWhAvlstockDetails(Comp_ID, Br_ID, Wh_id, Item_id, UomId, flag);
                ViewBag.SubitemAvlStockDetail = ds.Tables[0];
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (flag == "whres")
                {
                    ViewBag.Flag = "ReservStock";
                }
                else
                {
                    ViewBag.Flag = "WH";
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                //DataRows = Json(JsonConvert.SerializeObject(OCTaxTmplt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }
        public string ConvertToDecimal(string Value, int DecDgt)
        {
            string Str = "";
            string QtyDgt = "0.";

            for (int i = 0; i < DecDgt; i++)
            {
                QtyDgt += "0";
            }
            if (string.IsNullOrEmpty(Value))
            {
                Str = Convert.ToDecimal(0).ToString(QtyDgt);
            }
            else
            {
                Str = Convert.ToDecimal(Value).ToString(QtyDgt);
            }

            return Str;
        }
        public ActionResult GetAccBalDetail(string acc_id, string Date, string int_br_id = null)
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
                if (!string.IsNullOrEmpty(int_br_id))
                {
                    if (int_br_id != "0")
                    {
                        Br_ID = int_br_id;
                    }
                }
                DataSet ds = _Common_IServices.GetAccountDetail(acc_id, CompID, Br_ID, Date);



                ViewBag.AccDetails = ds.Tables[0];
                ViewBag.Acctype = ds.Tables[1];
                return PartialView("~/Areas/Common/Views/Cmn_PartialAccountBalanceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }

        /*--------------------------------TDS --------------------------------*/
        public ActionResult Cmn_GetTDSDetail(string NetAmt, string TDS_data, string doc_id, string Disable, string tax_type)
        {
            try
            {

                string CompID = string.Empty;
                string Br_ID = string.Empty;
                int ValDigit = 0;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["ValDigit"] != null)
                {
                    ValDigit = Convert.ToInt32(Session["ValDigit"]);
                }
                CommonTDS_Model model = new CommonTDS_Model();
                List<TDS_Tmplt_List> tmplt_list = new List<TDS_Tmplt_List>();
                List<TDS_Name_List> tds_list = new List<TDS_Name_List>();
                ViewBag.AssValue = NetAmt;
                Br_ID = Session["BranchId"].ToString();
                DataSet ds = _Common_IServices.Cmn_GetTDSDetail(CompID, Br_ID, doc_id);
                ViewBag.TDS_Data = ToDtblTdsDetail(TDS_data);
                tmplt_list.Add(new TDS_Tmplt_List
                {
                    tmplt_id = "0",
                    tmplt_name = "---Select---",
                });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmplt_list.Add(new TDS_Tmplt_List
                    {
                        tmplt_id = dr["tmplt_id"].ToString(),
                        tmplt_name = dr["tmplt_name"].ToString(),
                    });
                }
                model.tds_tmplt_list = tmplt_list;
                tds_list.Add(new TDS_Name_List
                {
                    tds_id = "0",
                    tds_name = "---Select---",
                    tds_acc_id = "0"
                });
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    tds_list.Add(new TDS_Name_List
                    {
                        tds_id = dr["tax_id"].ToString(),
                        tds_name = dr["tax_name"].ToString(),
                        tds_acc_id = dr["tax_acc_id"].ToString()
                    });
                }
                //if(doc_id== "105101145")
                //{
                //    decimal tds_base_amt = 0;
                //    foreach (DataRow dr in ViewBag.TDS_Data.Rows)
                //    {
                //        tds_base_amt = tds_base_amt + Convert.ToDecimal(dr["tds_base_amt"]);
                //    }
                //    ViewBag.AssValue = tds_base_amt.ToString(ToFixDecimal(ValDigit));
                //}
                model.tds_name_list = tds_list;
                model.Disable = Disable;
                ViewBag.TDS_Details = ds.Tables[0];
                ViewBag.tax_type = tax_type;
                ViewBag.doc_id = doc_id;
                return PartialView("~/Areas/Common/Views/Cmn_TDSCalculation.cshtml", model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }
        public ActionResult GetTDSTemplateDetail(string tmplt_id, string doc_id, string AmtForTds)
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
                DataSet ds = _Common_IServices.Cmn_GetTDSTempltDetail(CompID, Br_ID, tmplt_id, doc_id, AmtForTds);
                return Json(JsonConvert.SerializeObject(ds));
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }

        private DataTable ToDtblTdsDetail(string tdsDetails)
        {
            try
            {
                DataTable DtblItemtdsDetail = new DataTable();
                DataTable tds_detail = new DataTable();
                tds_detail.Columns.Add("tds_name", typeof(string));
                tds_detail.Columns.Add("tds_id", typeof(string));
                tds_detail.Columns.Add("tds_rate", typeof(string));
                tds_detail.Columns.Add("tds_amt", typeof(string));
                tds_detail.Columns.Add("tds_level", typeof(string));
                tds_detail.Columns.Add("tds_apply_on", typeof(string));
                tds_detail.Columns.Add("tds_apply_on_id", typeof(string));
                tds_detail.Columns.Add("tds_acc_id", typeof(string));
                tds_detail.Columns.Add("tds_base_amt", typeof(string));
                if (tdsDetails != null)
                {
                    JArray jObjecttds = JArray.Parse(tdsDetails);
                    for (int i = 0; i < jObjecttds.Count; i++)
                    {
                        DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
                        dtrowtdsDetailsLines["tds_name"] = jObjecttds[i]["tds_name"].ToString();
                        dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["tds_id"].ToString();
                        string tds_rate = jObjecttds[i]["tds_rate"].ToString();
                        tds_rate = tds_rate.Replace("%", "");
                        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["tds_level"].ToString();
                        dtrowtdsDetailsLines["tds_amt"] = jObjecttds[i]["tds_amt"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["tds_apply_on"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on_id"] = jObjecttds[i]["tds_apply_on_id"].ToString();
                        dtrowtdsDetailsLines["tds_acc_id"] = jObjecttds[i]["tds_acc_id"].ToString();
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttds[i]["tds_base_amt"].ToString();
                        tds_detail.Rows.Add(dtrowtdsDetailsLines);
                    }
                }
                DtblItemtdsDetail = tds_detail;
                return DtblItemtdsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }


        /*--------------------------------TDS End--------------------------------*/
        /*------------------------------BOM Alternate item Info---------------------------------//Added by Suraj on 17-10-2023 */
        public ActionResult GetAlternateItemDetailInfo(string product_id, string op_id, string item_type_id, string item_id)
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
                DataTable dt = _Common_IServices.Cmn_getAlternateItemDetalInfo(CompID, Br_ID, product_id, op_id, item_type_id, item_id).Tables[0];
                ViewBag.AltenateItemDetailItemWiseInfo = dt;
                return PartialView("~/Areas/Common/Views/Cmn_PartialBOMAlternateItemDetailInfo.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        /*------------------------------BOM Alternate item Info End---------------------------------*/
        public FileResult ExportDatatableToExcel(string fName, DataTable dt)
        {
            //string path = System.Web.HttpContext.Current.Server.MapPath("~");
            //bool exists = System.IO.Directory.Exists(path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\"));
            //if (!exists)
            //    System.IO.Directory.CreateDirectory(path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\"));
            string fileName = fName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            //string filePath = path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\") + fileName;
            //if (System.IO.File.Exists(filePath))
            //    System.IO.File.Delete(filePath);

            return ConvertDataTableToCsv(dt, fileName);
            // workbook.Save(filePath);
            //Response.TransmitFile(filePath);
            //return Download(filePath, fileName);

        }
        public FileResult ExportDatatableToExcel(string fName, DataTable dt, DataTable headerOne)
        {
            #region this function is Created by Suraj Maurya on 05-03-2025 to Genarete CSV File with Multi Header
            #endregion
            //string path = System.Web.HttpContext.Current.Server.MapPath("~");
            //bool exists = System.IO.Directory.Exists(path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\"));
            //if (!exists)
            //    System.IO.Directory.CreateDirectory(path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\"));
            string fileName = fName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            //string filePath = path + ("..\\Attachment\\LogsFile\\EmailAlertPDFs\\") + fileName;
            //if (System.IO.File.Exists(filePath))
            //    System.IO.File.Delete(filePath);

            return ConvertDataTableToCsv_mulitHeader(dt, fileName, headerOne);
            // workbook.Save(filePath);
            //Response.TransmitFile(filePath);
            //return Download(filePath, fileName);

        }
        public FileResult ConvertDataTableToCsv(DataTable dataTable, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn column in dataTable.Columns)
            {
                sb.Append(column.ColumnName + ",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.Append(row[column].ToString().Replace(",", " ").Replace('\n', ' ') + ",");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            //System.IO.File.WriteAllText(filePath, sb.ToString());
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "text/csv", fileName);
        }

        public FileResult ConvertDataTableToCsv_mulitHeader(DataTable dataTable, string fileName/*filePath*/, DataTable headerOne)
        {
            #region this function is Created by Suraj Maurya on 05-03-2025 to Genarete CSV File with Multi Header
            #endregion
            StringBuilder sb = new StringBuilder();

            //Adding First Header
            foreach (DataRow row in headerOne.Rows)
            {
                foreach (DataColumn column in headerOne.Columns)
                {
                    sb.Append(row[column].ToString().Replace(",", " ").Replace('\n', ' ') + ",");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }
            //Adding Main Header
            foreach (DataColumn column in dataTable.Columns)
            {
                sb.Append(column.ColumnName + ",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);

            //Adding Row Data
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.Append(row[column].ToString().Replace(",", " ").Replace('\n', ' ') + ",");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            //System.IO.File.WriteAllText(filePath, sb.ToString());
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "text/csv", fileName);
        }
        public FileResult Download(string filePath, string fileName)
        {
            string fullName = filePath;

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        public string SaveAlertDocument(byte[] data, string fileName)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);

                string folderPath = path + ("..\\LogsFile\\EmailAlertPDFs\\");
                if (!Directory.Exists(folderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folderPath);
                }
                string finalFilePath = folderPath + fileName;
                if (data != null)
                    System.IO.File.WriteAllBytes(finalFilePath, data);
                return finalFilePath;
            }
            catch (Exception exc)
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return "Error";
            }
        }
        private DataTable PrintOptionsDtTable()
        {
            /* Created by Suraj Maurya on 08-10-2024 to Create Common Print Option Table*/
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));
            dt.Columns.Add("ShowDeliverySchedule", typeof(string));
            dt.Columns.Add("ShowHsnNumber", typeof(string));
            dt.Columns.Add("ShowRemarksBlwItm", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("SuppAliasName", typeof(string));
            dt.Columns.Add("ShowPayTerms", typeof(string));
            dt.Columns.Add("ShowSuppAliasName", typeof(string));

            /*Add Start all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/
            dt.Columns.Add("ShowReferenceNo", typeof(string));
            dt.Columns.Add("ShowCatalogueNo", typeof(string));
            dt.Columns.Add("ShowOEMNo", typeof(string));
            dt.Columns.Add("ShowDiscount", typeof(string));
            dt.Columns.Add("ShowShipTo", typeof(string));
            dt.Columns.Add("ShowBillTo", typeof(string));
            dt.Columns.Add("ShowBankDetail", typeof(string));
            dt.Columns.Add("ShowPrintImage", typeof(string));
            dt.Columns.Add("ShowCompAddress", typeof(string));
            dt.Columns.Add("ShowMRP", typeof(string));
            dt.Columns.Add("ShowPackSize", typeof(string));
            /*END Start all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/
            return dt;
        }
        public DataTable PrintOptionsDt(string PrintFormat)
        {
            /* Created by Suraj Maurya on 08-10-2024 to Create Common Print Option Table*/
            try
            {
                DataTable dt = PrintOptionsDtTable();
                DataRow dtr = dt.NewRow();
                if (!string.IsNullOrEmpty(PrintFormat))
                {
                    JArray jObject = JArray.Parse(PrintFormat);
                    dtr["PrintFormat"] = jObject[0]["PrintFormat"].ToString();
                    dtr["ShowProdDesc"] = jObject[0]["ShowProdDesc"];
                    dtr["ShowCustSpecProdDesc"] = jObject[0]["ShowCustSpecProdDesc"];
                    dtr["ShowProdTechDesc"] = jObject[0]["ShowProdTechDesc"];
                    dtr["ShowSubItem"] = jObject[0]["ShowSubItem"];
                    dtr["ItemAliasName"] = jObject[0]["ItemAliasName"];
                    dtr["CustAliasName"] = jObject[0]["CustAliasName"];
                    dtr["PrintRemarks"] = jObject[0]["PrintRemarks"];
                    dtr["ShowDeliverySchedule"] = jObject[0]["ShowDeliverySchedule"];
                    dtr["ShowHsnNumber"] = jObject[0]["ShowHsnNumber"];
                    dtr["ShowRemarksBlwItm"] = jObject[0]["ShowRemarksBlwItm"];
                    dtr["SuppAliasName"] = jObject[0]["SuppAliasName"];
                    dtr["ShowPayTerms"] = jObject[0]["ShowPayTerms"];
                    dtr["ShowSuppAliasName"] = jObject[0]["ShowSuppAliasName"];

                    /*Add Start all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/
                    dtr["ShowReferenceNo"] = jObject[0]["ShowReferenceNo"];
                    dtr["ShowCatalogueNo"] = jObject[0]["ShowCatalogueNo"];
                    dtr["ShowOEMNo"] = jObject[0]["ShowOEMNo"];
                    dtr["ShowDiscount"] = jObject[0]["ShowDiscount"];
                    dtr["ShowShipTo"] = jObject[0]["ShowShipTo"];
                    dtr["ShowBillTo"] = jObject[0]["ShowBillTo"];
                    dtr["ShowBankDetail"] = jObject[0]["ShowBankDetail"];
                    dtr["ShowPrintImage"] = jObject[0]["ShowPrintImage"];
                    dtr["ShowCompAddress"] = jObject[0]["ShowCompAddress"];
                    dtr["ShowMRP"] = jObject[0]["ShowMRP"];
                    dtr["ShowPackSize"] = jObject[0]["ShowPackSize"];
                    /*END Start all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/
                    dt.Rows.Add(dtr);
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
        public DataTable PrintOptionsDt(Cmn_PrintOptions cmn_PrintOptions)
        {
            /* Created by Suraj Maurya on 08-10-2024 to Create Common Print Option Table*/
            try
            {
                DataTable dt = PrintOptionsDtTable();
                DataRow dtr = dt.NewRow();
                dtr["PrintFormat"] = cmn_PrintOptions.PrintFormat;
                dtr["ShowProdDesc"] = cmn_PrintOptions.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = cmn_PrintOptions.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = cmn_PrintOptions.ShowProdTechDesc;
                dtr["ShowSubItem"] = cmn_PrintOptions.ShowSubItem;
                dtr["ItemAliasName"] = cmn_PrintOptions.ItemAliasName;
                dtr["CustAliasName"] = cmn_PrintOptions.CustAliasName;
                dtr["PrintRemarks"] = cmn_PrintOptions.PrintRemarks;
                dtr["ShowDeliverySchedule"] = cmn_PrintOptions.ShowDeliverySchedule;
                dtr["ShowHsnNumber"] = cmn_PrintOptions.ShowHsnNumber;
                dtr["ShowRemarksBlwItm"] = cmn_PrintOptions.ShowRemarksBlwItm;
                dtr["ShowTotalQty"] = cmn_PrintOptions.ShowTotalQty;
                dtr["SuppAliasName"] = cmn_PrintOptions.SupplierAliasName;
                dtr["ShowPayTerms"] = cmn_PrintOptions.ShowPayTerms;
                /*Add Start all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/

                dtr["ShowReferenceNo"] = cmn_PrintOptions.ReferenceNo;
                dtr["ShowCatalogueNo"] = cmn_PrintOptions.CatalogueNo;
                dtr["ShowOEMNo"] = cmn_PrintOptions.OEMNo;
                dtr["ShowDiscount"] = cmn_PrintOptions.Discount;
                dtr["ShowShipTo"] = cmn_PrintOptions.ShipTo;
                dtr["ShowBillTo"] = cmn_PrintOptions.BillTo;
                dtr["ShowBankDetail"] = cmn_PrintOptions.BankDetail;
                dtr["ShowPrintImage"] = cmn_PrintOptions.PrintImage;
                dtr["ShowCompAddress"] = cmn_PrintOptions.CompAddress;
                dtr["ShowMRP"] = cmn_PrintOptions.ShowMRP;
                dtr["ShowPackSize"] = cmn_PrintOptions.ShowPackSize;
                /*END Start all below columns by Hina Sharma on 25-07-2025 only for Sales Quotation*/

                dt.Rows.Add(dtr);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public string CheckFinancialYear(string compId, string brId)
        {
            DataTable dttbl = _Common_IServices.curFYdt(compId, brId);
            if (dttbl.Rows.Count > 0)
                return "Exist";
            else
                return
                    "Not Exist";
        }
        public string CheckFinancialYearAndPreviousYear(string compId, string brId, string DocDate)
        {
            string Result = string.Empty;
            string RtrnResult = string.Empty;
            Result = _Common_IServices.curFYdtAndPreviousFYdt(compId, brId, DocDate);
            //Result = dttbl.Rows[0]["Result"].ToString();
            if (Result == "TransAllow")
            {
                RtrnResult = "TransAllow";
            }
            if (Result == "TransNotAllow")
            {
                RtrnResult = "TransNotAllow";
            }
            return RtrnResult;

        }
        public string Fin_CheckFinancialYear(string compId, string brId, string VouDt)
        {
            DataSet ds = _Common_IServices.Fin_curFYdt(compId, brId, VouDt);
            string fflag = string.Empty;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                    return fflag = "FY Not Exist";
                else if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count == 0)
                    //return fflag = "FY Exist";/*commented and modify by Hina Sharma on 24-03-2025*/
                    //else if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count > 0)
                    //fflag = "FB Close";
                    fflag = "FY Exist";
                if (fflag == "FY Exist")
                {
                    if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count > 0)
                        return fflag = "FB Close";
                }
                else
                    //return fflag = "FY Exist";
                    fflag = "FY Exist";
                if (fflag == "FY Exist")
                {
                    if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count > 0)
                        return fflag = "FB Close";
                }
                return fflag = "FY Exist";

            }
            else
                fflag = "FY Not Exist";

            return fflag;
        }
        public JsonResult getStockUomWise(string itemId, string uomId)
        {
            JsonResult DataRows = null;
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
                DataSet Ds = _Common_IServices.GetStockUomWise(Comp_ID, Br_ID, itemId, uomId);
                DataRows = Json(JsonConvert.SerializeObject(Ds));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public JsonResult CmnGetOcTpEntityList(string OcId, string CurrId, string DocId)
        {
            JsonResult DataRows = null;
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
                DataSet Ds = _Common_IServices.CmnGetOcTpEntityList(Comp_ID, Br_ID, OcId, CurrId, DocId);
                DataRows = Json(JsonConvert.SerializeObject(Ds));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult Cmn_LogErrorInJS(string stack, string message)
        {
            try
            {
                string path = Server.MapPath("~");
                Errorlog.LogError_InJS(path, stack, "", "", "");
                return Json("Log Added");
            }
            catch (Exception ex)
            {
                return Json("Error in Adding Log : " + ex.Message);
            }
        }
        public ActionResult Cmn_ErrorPage()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
        public ActionResult OtherChargePopUp(List<Cmn_OC_Detail> OC_Detail, string Doc_Id, string Disabled)
        {
            try
            {
                DataTable DtblOCDetail = new DataTable();

                if (OC_Detail != null)
                {
                    DtblOCDetail = ToDataTable(OC_Detail);
                }
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                Br_ID = Session["BranchId"].ToString();
                ViewBag.OtherChargeDetails = DtblOCDetail;
                ViewBag.DocumentMenuId = "";
                ViewBag.RateDigit = "1";
                ViewBag.ValDigit = "1";
                DataTable dt_curr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                DataSet SO_OC = _Common_IServices.GetOtherChargeDAL(CompID, Br_ID);
                //DataTable dt = _Common_IServices.Cmn_getAlternateItemDetalInfo(CompID, Br_ID, product_id, op_id, item_type_id, item_id).Tables[0];
                //ViewBag.AltenateItemDetailItemWiseInfo = dt;
                return PartialView("~/Areas/Common/Views/Cmn_OtherChargeWithSupp.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public DataTable ToDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                object[] values = new object[props.Count];
                using (DataTable table = new DataTable())
                {
                    long _pCt = props.Count;
                    for (int i = 0; i < _pCt; ++i)
                    {
                        PropertyDescriptor prop = props[i];
                        table.Columns.Add(prop.Name, prop.PropertyType);
                    }
                    foreach (T item in data)
                    {
                        long _vCt = values.Length;
                        for (int i = 0; i < _vCt; ++i)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                        table.Rows.Add(values);
                    }
                    return table;
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetGLDetails(List<Cmn_GL_Detail> GLDetail)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();

                if (GLDetail != null)
                {
                    DtblGLDetail = ToDataTable(GLDetail);
                }
                DataSet GlDt = _Common_IServices.GetAllGLDetails(DtblGLDetail);
                Validate = Json(GlDt);
                JsonResult DataRows = null;
                DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetGLDetails1(List<Cmn_GL_Detail1> GLDetail)
        {
            /*Added by Suraj on 07-08-2024 to add new column acc_id*/
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();
                string BrId = string.Empty;

                if (GLDetail != null)
                {
                    DtblGLDetail = ToDataTable(GLDetail);
                }
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();

                DataSet GlDt = _Common_IServices.GetAllGLDetails1(DtblGLDetail, BrId);
                Validate = Json(GlDt);
                JsonResult DataRows = null;
                DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetGLDetails_StringData(string GLDetail)
        {
            /*Added by Suraj on 07-08-2024 to add new column acc_id*/
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();
                string BrId = string.Empty;

                if (GLDetail != null)
                {
                    DtblGLDetail = GlDetailTable(GLDetail);
                }
                if (Session["BranchId"] != null)
                    BrId = Session["BranchId"].ToString();

                DataSet GlDt = _Common_IServices.GetAllGLDetails1(DtblGLDetail, BrId);
                Validate = Json(GlDt);
                JsonResult DataRows = null;
                DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private DataTable GlDetailTable(string glData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("comp_id", typeof(string));
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("doctype", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("ValueInBase", typeof(string));
            dt.Columns.Add("DrAmt", typeof(string));
            dt.Columns.Add("CrAmt", typeof(string));
            dt.Columns.Add("TransType", typeof(string));
            dt.Columns.Add("gl_type", typeof(string));
            dt.Columns.Add("parent", typeof(string));
            dt.Columns.Add("DrAmtInBase", typeof(string));
            dt.Columns.Add("CrAmtInBase", typeof(string));
            dt.Columns.Add("curr_id", typeof(string));
            dt.Columns.Add("conv_rate", typeof(string));
            dt.Columns.Add("bill_no", typeof(string));
            dt.Columns.Add("bill_date", typeof(string));
            dt.Columns.Add("acc_id", typeof(string));
            if (!string.IsNullOrEmpty(glData))
            {
                JArray jObject = JArray.Parse(glData);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowItemdetails = dt.NewRow();
                    dtrowItemdetails["comp_id"] = jObject[i]["comp_id"].ToString();
                    dtrowItemdetails["id"] = jObject[i]["id"].ToString();
                    dtrowItemdetails["type"] = jObject[i]["type"].ToString();
                    dtrowItemdetails["doctype"] = jObject[i]["doctype"].ToString();
                    var value = jObject[i]["Value"].ToString();
                    var ValueInBase = jObject[i]["ValueInBase"].ToString();
                    var DrAmt = jObject[i]["DrAmt"].ToString();
                    var CrAmt = jObject[i]["CrAmt"].ToString();
                    var conv_rate = jObject[i]["conv_rate"].ToString();
                    dtrowItemdetails["Value"] = (string.IsNullOrEmpty(value)) ? "0" : value;
                    dtrowItemdetails["ValueInBase"] = (string.IsNullOrEmpty(ValueInBase)) ? "0" : ValueInBase;
                    dtrowItemdetails["DrAmt"] = (string.IsNullOrEmpty(DrAmt)) ? "0" : DrAmt;
                    dtrowItemdetails["CrAmt"] = (string.IsNullOrEmpty(CrAmt)) ? "0" : CrAmt;
                    dtrowItemdetails["TransType"] = jObject[i]["TransType"].ToString();
                    dtrowItemdetails["gl_type"] = jObject[i]["gl_type"].ToString();
                    dtrowItemdetails["parent"] = jObject[i]["parent"].ToString();
                    dtrowItemdetails["DrAmtInBase"] = "0";// jObject[i]["DrAmtInBase"].ToString();
                    dtrowItemdetails["CrAmtInBase"] = "0";// jObject[i]["CrAmtInBase"].ToString();
                    dtrowItemdetails["curr_id"] = jObject[i]["curr_id"].ToString();
                    dtrowItemdetails["conv_rate"] = (string.IsNullOrEmpty(conv_rate)) ? "0" : conv_rate;
                    dtrowItemdetails["bill_no"] = jObject[i]["bill_no"].ToString();
                    dtrowItemdetails["bill_date"] = jObject[i]["bill_date"].ToString();
                    dtrowItemdetails["acc_id"] = jObject[i]["acc_id"].ToString();
                    dt.Rows.Add(dtrowItemdetails);
                }
            }
            return dt;
        }

        public ActionResult Cmn_GetTdsDetails(string SuppId, string GrossVal, string tax_type)
        {
            try
            {
                DataTable DtblOCDetail = new DataTable();

                string CompID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                Br_ID = Session["BranchId"].ToString();

                DataSet ds = _Common_IServices.Cmn_GetTdsDetails(CompID, Br_ID, SuppId, GrossVal, tax_type);

                return Json(JsonConvert.SerializeObject(ds));
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult Cmn_SerchableTPEntityList(string SearchName)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                var result = SearchName == null ? "" : SearchName;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult Cmn_GetGL_VoucherDetail(string from_dt, string to_dt, string acc_id, string doc_id)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                }

                DataSet dtdata = _Common_IServices.Cmn_GetGernalLedgerDetails(Comp_id, Br_id, acc_id, from_dt, to_dt, doc_id);

                ViewBag.GL_Details = null;

                ViewBag.GL_listByFilter = "GLlistByFilter";

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    ViewBag.GL_Details = dtdata.Tables[0];
                }

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/Partial_GLDetails.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult Cmn_GetGeneralLedger_Detail(string from_dt, string to_dt, string acc_id, string doc_id, string curr_id)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                }

                DataSet dtdata = _Common_IServices.Cmn_Get_GernalLedger_Details(Comp_id, Br_id, acc_id, from_dt, to_dt, doc_id, curr_id);

                ViewBag.GL_Details = null;

                ViewBag.GL_listByFilter = "GLlistByFilter";

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    ViewBag.GL_Details = dtdata.Tables[0];
                }

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/Partial_GLDetails.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult Cmn_GetBrList(string flag)
        {
            DataTable br_list = new DataTable();
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                    ViewBag.vb_br_id = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_id = Session["UserId"].ToString();
                }
                br_list = _Common_IServices.Cmn_GetBrList(Comp_id, Br_id, User_id, flag);
                ViewBag.br_list = br_list;

                return View("~/Areas/Common/Views/Comn_PartialBranchList.cshtml");

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /*--------------For Getting Semi finished Item BOM------------*/
        public ActionResult Cmn_GetSFBOMDetailsItemWise(string FGItemId, string SFItemId, string Level = null)
        {
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
                ViewBag.BOMDetailsItemWise = _Common_IServices.Cmn_GetSFBOMDetailsItemWise(Comp_ID, Br_ID, FGItemId, SFItemId);
                ViewBag.BomProduct_id = FGItemId;
                ViewBag.BOMLevel = Level;
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialBillOfMaterial.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /*------------Added by Nidhi on 17-01-2025--------------*/
        public string CreateExcelFile(string fName, HttpServerUtilityBase server)
        {
            string path = server.MapPath("~");
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(currentDir);

            string FolderPath = path + ("..\\ImportExcelFiles\\");
            bool exists = System.IO.Directory.Exists(FolderPath);
            if (!exists)
                Directory.CreateDirectory(FolderPath);

            // Start with the base file name
            string fileName = fName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string filePath = FolderPath + fileName;

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            return filePath;
        }
        /*--------------------------End-----------------------*/

        [HttpPost]
        public ActionResult Cmn_GetItemSearch(string itemNameSearch) /*Added By NItesh 29-01-2025 For Item NAme Search*/
        {
            JsonResult DataRows = null;
            if (Session["CompId"] != null)
            {
                Comp_id = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_id = Session["BranchId"].ToString();

            }
            if (Session["UserId"] != null)
            {
                User_id = Session["UserId"].ToString();
            }
            DataTable dt = new DataTable();
            dt = _Common_IServices.GetItemName(Comp_id, Br_id, itemNameSearch);
            DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
            return DataRows;
        }
        public string Cmn_GenLocalFilePath(string UriLiveFilePath, string fileName, HttpServerUtilityBase Server)/*Added By Suraj Maurya on 19-02-2025 for pdf*/
        {
            string lacalFilePath = "";// Path.Combine(Path.GetTempPath(), "tmpDigiSign.png");
            try
            {
                //using (WebClient client = new WebClient())
                //{
                //    lacalFilePath = Path.Combine(Path.GetTempPath(), "pdf_" + Guid.NewGuid().ToString() + fileName);
                //    client.DownloadFile(UriLiveFilePath, lacalFilePath);
                //}
                using (WebClient client = new WebClient())
                {
                    if (!string.IsNullOrEmpty(UriLiveFilePath))
                    {
                        WebRequest request = WebRequest.Create(UriLiveFilePath);
                        request.Method = "HEAD";
                        using (WebResponse response = request.GetResponse())
                        {
                            string contentType = response.ContentType.ToLower();

                            //Ensure URL is an image file
                            if (!contentType.StartsWith("image/"))
                            {
                                return null;
                            }
                        }
                        lacalFilePath = Path.Combine(Path.GetTempPath(), "pdf_" + Guid.NewGuid().ToString() + fileName);
                        client.DownloadFile(UriLiveFilePath, lacalFilePath);
                        if (!IsValidImage(lacalFilePath))
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "";
            }

            return lacalFilePath;
        }
        public string Cmn_ServerUrl(HttpRequestBase Request)/*Added By Suraj Maurya on 19-02-2025 for pdf*/
        {
            try
            {
                string serverUrl, localIp = string.Empty;

                serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                return serverUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private bool IsValidImage(string filePath)/* Added By Suraj Maurya on 19-02-2025 */
        {
            try
            {
                using (var img = System.Drawing.Image.FromFile(filePath))
                {
                    return true;
                }
            }
            catch
            {
                System.IO.File.Delete(filePath);
                return false;
            }
        }

        public byte[] Cmn_PdfFileGenerate(string HtmlContent, string docMenuId, string DocStatus, HttpServerUtilityBase Server, Cmn_pdfGenerate_model model = null, DataSet Details = null, string LogoImage = null, string DigiSignatureImage = null)/*Added By Suraj Maurya on 19-02-2025 for pdf*/
        {
            try
            {

                StringReader reader = null;
                Document pdfDoc = null;
                PdfWriter writer = null;


                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(HtmlContent);
                    // pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
                    if (docMenuId != null)
                    {
                        if (docMenuId == "105101130")
                        {
                            pdfDoc = new Document(PageSize.A4, 0f, 0f, 155f, 20f);
                        }
                        else
                        {
                            pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                        }
                    }

                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = "";
                    if (DocStatus == "D" || DocStatus == "F")
                    {
                        draftImage = Server.MapPath("~/Content/Images/draft.png");
                    }
                    else if (DocStatus == "C")
                    {
                        draftImage = Server.MapPath("~/Content/Images/cancelled.png");
                    }

                    /*-------------------Start Code Add By Hina Sharma on 14-04-2025 to Make Common header-----------------------*/
                    BaseFont bf1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font fontnormal = new Font(bf1, 9, Font.NORMAL);
                    Font fontbold = new Font(bf1, 9, Font.BOLD);
                    //fontbold.SetStyle("bold");
                    Font fonttitle = new Font(bf1, 15, Font.BOLD);
                    //fonttitle.SetStyle("underline");
                    /*-------------------Start Code Add By Hina Sharma on 14-04-2025 to Make Common header-----------------------*/
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                //var draftimg = Image.GetInstance(draftImage);
                                //draftimg.SetAbsolutePosition(0, 160);
                                //draftimg.ScaleAbsolute(580f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (DocStatus == "D" || DocStatus == "F" || DocStatus == "C")
                                    {
                                        var draftimg = Image.GetInstance(draftImage);
                                        draftimg.SetAbsolutePosition(0, 160);
                                        draftimg.ScaleAbsolute(580f, 580f);
                                        content.AddImage(draftimg);
                                    }

                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);


                                    /*-------------------Start Code Add By Hina Sharma on 14-04-2025 to Make Common header-----------------------*/
                                    if (docMenuId != null)
                                    {
                                        if (docMenuId == "105101130")
                                        {

                                            if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["logo"].ToString()))
                                            {
                                                try
                                                {
                                                    if (!string.IsNullOrEmpty(LogoImage))
                                                    {
                                                        var Logo = Image.GetInstance(LogoImage);
                                                        //Logo.SetAbsolutePosition(475, 720);
                                                        //Logo.ScaleAbsolute(100f, 95f);
                                                        /*above Commented and add by Hina for multiple header*/
                                                        //Logo.SetAbsolutePosition(475, 770);
                                                        //Logo.ScaleAbsolute(75f, 55f);
                                                        Logo.SetAbsolutePosition(21, 790);
                                                        Logo.ScaleAbsolute(110f, 35f);
                                                        /*Commented and add by Hina for multiple header*/
                                                        if (i == 1)
                                                            content.AddImage(Logo);
                                                    }
                                                }
                                                catch
                                                {

                                                }

                                            }
                                            string comp_nm, Comp_Add, Comp_Add1, cont_num, email_id, comp_website, State, gst_no, br_pan_no, ship_to = string.Empty;

                                            comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();
                                            Comp_Add = Details.Tables[0].Rows[0]["Comp_Add"].ToString().Trim();
                                            Comp_Add1 = Details.Tables[0].Rows[0]["Comp_Add1"].ToString().Trim();
                                            cont_num = Details.Tables[0].Rows[0]["cont_num1"].ToString().Trim();
                                            email_id = Details.Tables[0].Rows[0]["email_id1"].ToString().Trim();
                                            if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["comp_website"].ToString().Trim()))
                                            {
                                                comp_website = Details.Tables[0].Rows[0]["comp_website"].ToString().Trim();
                                            }
                                            else
                                            {
                                                comp_website = "";
                                            }
                                            State = Details.Tables[0].Rows[0]["State"].ToString().Trim();
                                            gst_no = Details.Tables[0].Rows[0]["gst_no"].ToString().Trim();
                                            br_pan_no = Details.Tables[0].Rows[0]["br_pan_no"].ToString().Trim();
                                            //if (docMenuId != null)
                                            //{
                                            //    if (docMenuId == "105101130")
                                            //    {
                                            ship_to = Details.Tables[0].Rows[0]["ship_to"].ToString().Trim();
                                            //    }
                                            //}
                                            Phrase ptitle = new Phrase(String.Format("Purchase Order", i, PageCount), fonttitle);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 800, 0);

                                            Phrase compname = new Phrase(String.Format("Invoice to : {0}", comp_nm, i, PageCount), fontbold);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, compname, 21, 775, 0);

                                            Phrase address = new Phrase(String.Format("Address : {0}", Comp_Add, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, address, 21, 765, 0);
                                            Int32 H = 0;
                                            if (Comp_Add1 != null && Comp_Add1 != "")
                                            {
                                                H = 5;
                                                Phrase address1 = new Phrase(String.Format("{0}", Comp_Add1, i, PageCount), fontnormal);
                                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, address1, 62, 755, 0);
                                            }
                                            Phrase phone = new Phrase(String.Format("Ph.No.  : {0}", cont_num, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, phone, 21, 750 - H, 0);

                                            Phrase email = new Phrase(String.Format("Email : {0}", email_id, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, email, 21, 740 - H, 0);

                                            Phrase website = new Phrase(String.Format("Website : {0}", comp_website, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, website, 21, 730 - H, 0);

                                            Phrase state = new Phrase(String.Format("State Code/State : {0}", State, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, state, 21, 720 - H, 0);

                                            Phrase gstno = new Phrase(String.Format("GST No. : {0}", gst_no, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, gstno, 21, 710 - H, 0);

                                            Phrase panno = new Phrase(String.Format("PAN No. : {0}", br_pan_no, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, panno, 21, 700 - H, 0);
                                            //if (docMenuId != null)
                                            //{
                                            //    if (docMenuId == "105101130")
                                            //    {
                                            Phrase shipto = new Phrase(String.Format("Ship to : {0}", ship_to), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, shipto, 21, 690 - H, 0);
                                            //    }
                                            //}
                                            //Phrase p7 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontbold);
                                            //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p7, 70, 10, 0);
                                            //Phrase p8 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontbold);
                                            //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, p8, 350, 10, 0);

                                        }
                                    }
                                    /*-------------------End Code Add By Hina Sharma on 14-04-2025 to Make Common header-----------------------*/
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }

                    Cmn_DeleteFile(model.localLogoPath);
                    Cmn_DeleteFile(model.localDigiSignPath);

                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public void Cmn_DeleteFile(string filePath)//Created by Suraj Maurya on 18-02-2025
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    //string result = "Not Found";
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        //result = "Deleted";
                    }
                    //return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string Cmn_ConvertPartialViewToString(PartialViewResult partialView, ControllerContext C_context)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(C_context, partialView.ViewName).View;

                var vc = new ViewContext(
                  C_context, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }

        public JsonResult CheckUserRolePageAccess(string MaterPagetype, string TranctionPageName)
        {
            JsonResult DataRows = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string UserID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                DataSet GetData = _Common_IServices.CheckUserRolePageAccess(Comp_ID, Br_ID, UserID, MaterPagetype);
                DataRows = Json(JsonConvert.SerializeObject(GetData));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        /* Added By Nidhi on 05-04-2025 */
        public void AppendExcel(string filePath, DataSet ds, DataSet dataset, string pagename)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var predefinedSheets = GetPredefinedSheets(pagename, dataset);

                    // Add predefined sheets
                    AddPredefinedSheets(package, predefinedSheets);

                    // Add DataTables
                    if (ds != null && ds.Tables.Count >= 1)
                    {
                        AddDataTables(package, ds, pagename);
                    }
                    // Apply dynamic dropdowns
                    ApplyDynamicDropdowns(package, predefinedSheets, pagename);
                    if (pagename == "ItemSetup")
                    {
                        ProtectSheets(package.Workbook, new List<string> { "ItemGroup", "HSNCode", "BASEUOM", "ItemsAttributes", "BranchName", "Portfolio", "DistinctAttributeName" });
                    }
                    if (pagename == "QCItemParameter")
                    {
                        ProtectSheets(package.Workbook, new List<string> { "ItemName", "UOM", "ParameterName" });
                    }
                    if (pagename == "CustomerPriceList")
                    {
                        ProtectSheets(package.Workbook, new List<string> { "ItemName", "PriceGroup", "UOM" });
                    }
                    if (pagename == "CustomerSetup")
                    {
                        ProtectSheets(package.Workbook, new List<string> { "Category", "Portfolio", "SalesRegion", "PriceGroup", "GLAccountGroup", "BranchName", "Currency", "GLReportingGroup", "DefaultTransporter", "Country", "State", "District", "City", "CustomerGroup", "CustomerZone" });
                    }
                    if (pagename == "SupplierSetup")
                    {
                        ProtectSheets(package.Workbook, new List<string> { "Category", "Portfolio", "GLAccountGroup", "BranchName", "Currency", "GLReportingGroup", "Country", "State", "District", "City" });
                    }
                    if (pagename == "GeneralLedger")
                    {
                        ProtectSheets(package.Workbook, new List<string> { "AccountGroup", "BranchName", "Currency" });
                    }
                    if (pagename == "GRNSerialExcel" || pagename == "DPISerialExcel" || pagename == "StockTakeSerialExcel" || pagename == "ExtRcptSerialExcel"
                        || pagename == "OpeningRcptSerialExcel")
                    {
                        AddProtectedCloumn(package, predefinedSheets, pagename);
                    }
                    /*Added by Surbhi on 22/01/2026 for Asset Registration(105106101)*/
                    if (pagename == "AsssetRegistrationSetup")
                    {
                        AddProtectedCloumn(package, predefinedSheets, pagename);
                        ProtectSheets(package.Workbook, new List<string> { "Group", "AssignedRequirementArea" });
                    }
                    // Save changes
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        private void AddProtectedCloumn(ExcelPackage package, List<(string sheetName, DataTable table)> predefinedSheets, string pagename)
        {
            foreach (var sheet in predefinedSheets)
            {
                var worksheet = package.Workbook.Worksheets[sheet.sheetName]
                                ?? package.Workbook.Worksheets.Add(sheet.sheetName);


                // Load DataTable into Excel
                worksheet.Cells["A1"].LoadFromDataTable(sheet.table, true);
                worksheet.Cells.Style.Locked = false;
                if (pagename == "GRNSerialExcel")
                {
                    int sr_no = FindColumnIndexByHeader(worksheet, "SrNo.");
                    int itemDescriptionColumn = FindColumnIndexByHeader(worksheet, "Item Description*");
                    int uomColumn = FindColumnIndexByHeader(worksheet, "UOM*");
                    int quantityTypeColumn = FindColumnIndexByHeader(worksheet, "Quantity Type*");


                    // Lock those columns by index
                    if (sr_no != -1)
                        worksheet.Column(sr_no).Style.Locked = true;
                    if (itemDescriptionColumn != -1)
                        worksheet.Column(itemDescriptionColumn).Style.Locked = true;
                    if (uomColumn != -1)
                        worksheet.Column(uomColumn).Style.Locked = true;
                    if (quantityTypeColumn != -1)
                        worksheet.Column(quantityTypeColumn).Style.Locked = true;
                }
                /*Added by Surbhi on 03/02/2026 for Asset Registration(105106101)*/
                else if (pagename == "AsssetRegistrationSetup" && sheet.sheetName == "AssetDetail")
                {
                    int sr_no = FindColumnIndexByHeader(worksheet, "Asset Description*");
                    int itemDescriptionColumn = FindColumnIndexByHeader(worksheet, "Serial Number*");

                    // Lock those columns by index
                    if (sr_no != -1)
                        worksheet.Column(sr_no).Style.Locked = true;
                    if (itemDescriptionColumn != -1)
                        worksheet.Column(itemDescriptionColumn).Style.Locked = true;
                    if (sr_no != -1)
                    {
                        worksheet.View.FreezePanes(sr_no + 1, 1);
                    }
                }
                else
                {
                    int sr_no = FindColumnIndexByHeader(worksheet, "SrNo.");
                    int itemDescriptionColumn = FindColumnIndexByHeader(worksheet, "Item Description*");
                    int uomColumn = FindColumnIndexByHeader(worksheet, "UOM*");

                    // Lock those columns by index
                    if (sr_no != -1)
                        worksheet.Column(sr_no).Style.Locked = true;
                    if (itemDescriptionColumn != -1)
                        worksheet.Column(itemDescriptionColumn).Style.Locked = true;
                    if (uomColumn != -1)
                        worksheet.Column(uomColumn).Style.Locked = true;
                }
                // Protect worksheet
                string password = "enrep";
                worksheet.Protection.IsProtected = true;
                worksheet.Protection.SetPassword(password);
                worksheet.Protection.AllowSelectLockedCells = false;
                worksheet.Cells.AutoFitColumns();
            }
        }
        // Method to convert column index to Excel column letter (e.g., 1 -> "A", 2 -> "B", etc.)
        private string GetColumnLetter(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = String.Empty;
            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }
            return columnName;
        }
        private int FindColumnIndexByHeader(ExcelWorksheet ws, string headerName)
        {
            var headerRow = ws.Cells[1, 1, 1, ws.Dimension.End.Column]; // Get the header row (first row)
            var headerCell = headerRow.FirstOrDefault(c => c.Text == headerName); // Find the header by name

            if (headerCell != null)
            {
                return headerCell.Start.Column; // Return the column index (1-based)
            }
            return -1; // Return -1 if header is not found
        }
        // Method to find column index by header name
        private List<(string sheetName, DataTable table)> GetPredefinedSheets(string pagename, DataSet dataset)
        {
            var predefinedSheets = new List<(string sheetName, DataTable table)>();

            if (pagename == "QCItemParameter")
            {
                predefinedSheets.Add(("ParameterDetail", dataset.Tables[0]));
            }

            if (pagename == "QCParameterDefinition")
            {
                predefinedSheets.Add(("ParameterDefinition", dataset.Tables[0]));
            }

            if (pagename == "CustomerPriceList")
            {
                predefinedSheets.Add(("ItemDetail", dataset.Tables[0]));
            }
            if (pagename == "CustomerSetup")
            {
                predefinedSheets.Add(("CustomerDetail", dataset.Tables[0]));
                predefinedSheets.Add(("Address", dataset.Tables[1]));
                predefinedSheets.Add(("BranchMapping", dataset.Tables[2]));
            }
            if (pagename == "ItemSetup")
            {
                predefinedSheets.Add(("ItemsDetail", dataset.Tables[0]));
                predefinedSheets.Add(("SubItemDetail", dataset.Tables[1]));
                predefinedSheets.Add(("ItemAttribute", dataset.Tables[2]));
                predefinedSheets.Add(("BranchMapping", dataset.Tables[3]));
                predefinedSheets.Add(("ItemPortfolio", dataset.Tables[4]));
            }
            if (pagename == "SupplierSetup")
            {
                predefinedSheets.Add(("SupplierDetail", dataset.Tables[0]));
                predefinedSheets.Add(("Address", dataset.Tables[1]));
                predefinedSheets.Add(("BranchMapping", dataset.Tables[2]));
            }
            if (pagename == "GeneralLedger")
            {
                predefinedSheets.Add(("GeneralLedgerDetail", dataset.Tables[0]));
                predefinedSheets.Add(("BranchMapping", dataset.Tables[1]));
            }
            if (pagename == "GRNSerialExcel" || pagename == "DPISerialExcel" || pagename == "StockTakeSerialExcel" || pagename == "ExtRcptSerialExcel"
                || pagename == "OpeningRcptSerialExcel")
            {
                predefinedSheets.Add(("SerialDetail", dataset.Tables[0]));

            }
            if (pagename == "PackListSerializationExcel")
            {
                predefinedSheets.Add(("PackSerializationDetail", dataset.Tables[0]));

            }
            /*Added by Surbhi on 22/01/2026 for Asset Registration(105106101)*/
            if (pagename == "AsssetRegistrationSetup")
            {
                predefinedSheets.Add(("AssetDetail", dataset.Tables[0]));
                predefinedSheets.Add(("CapitalizedValue", dataset.Tables[1]));
            }
            return predefinedSheets;
        }
        private void AddPredefinedSheets(ExcelPackage package, List<(string sheetName, DataTable table)> predefinedSheets)
        {
            foreach (var sheet in predefinedSheets)
            {
                var worksheet = package.Workbook.Worksheets[sheet.sheetName]
                                ?? package.Workbook.Worksheets.Add(sheet.sheetName);
                ExcelSheetFormatting(worksheet, sheet.table);
            }
        }
        private void AddDataTables(ExcelPackage package, DataSet ds, string pagename)
        {
            foreach (DataTable table in ds.Tables)
            {
                RenameTableColumns(table, pagename);

                var worksheet = package.Workbook.Worksheets[table.TableName]
                                ?? package.Workbook.Worksheets.Add(table.TableName);

                worksheet.Cells.Clear();
                worksheet.Cells.LoadFromDataTable(table, true);
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                ApplyHeaderStyle(worksheet, table);
            }
        }
        private void ApplyDynamicDropdowns(ExcelPackage package, List<(string sheetName, DataTable table)> predefinedSheets, string pagename)
        {
            foreach (var sheet in predefinedSheets)
            {
                var worksheet = package.Workbook.Worksheets[sheet.sheetName];

                if (pagename == "QCItemParameter")
                {
                    ApplyDataValidation(worksheet, 3, new[] { "Observative", "Quantitative", "Qualitative" });
                    AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Item Name*(max 100 characters)"), "ItemName", "ItemNameList");
                    AddDropdown(package, worksheet, FindColumnIndex(worksheet, "UOM"), "UOM", "UOMList");
                    AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Parameter Name*(max 50 characters)"), "ParameterName", "ParameterNameList");
                }
                else if (pagename == "QCParameterDefinition")
                {
                    ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Parameter Type*"), new[] { "Observative", "Quantitative", "Qualitative" });
                }
                else if (pagename == "CustomerPriceList")
                {
                    if (sheet.sheetName == "ItemDetail")
                    {
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Item Name*(max 100 characters)"), "ItemName", "ItemNameList");
                    }
                }
                else if (pagename == "CustomerSetup")
                {
                    if (sheet.sheetName == "CustomerDetail")
                    {
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Customer Type*"), new[] { "Domestic", "Export" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "GST Category*"), new[] { "Registered", "Un-Registered", "Exempted", "Composition" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Applicable On"), new[] { "Sales Order", "Shipment" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "TCS Applicable*"), new[] { "Yes", "No" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Price Policy"), new[] { "Manual", "Price List" });

                        // Adding dynamic dropdowns 
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "GL Account Group*"), "GLAccountGroup", "AccountGroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "GL Reporting Group"), "GLReportingGroup", "GLReportingGroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Default Currency*"), "Currency", "Currencylist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Category*"), "Category", "Categorylist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Portfolio*"), "Portfolio", "Portfoliolist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Price Group"), "PriceGroup", "PriceGrouplist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Sales Region*"), "SalesRegion", "SalesRegionlist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Default Transporter"), "DefaultTransporter", "DefaultTransporterlist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Customer Group"), "CustomerGroup", "CustomerGroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Customer Zone"), "CustomerZone", "CustomerZoneList");
                    }
                    else if (worksheet.Name == "Address")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Billing Address"), new[] { "Yes", "No" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Shipping Address"), new[] { "Yes", "No" });

                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Country*"), "Country", "countrylist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "State*"), "State", "statelist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "District*"), "District", "districtlist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "City*"), "City", "citylist");
                    }
                    else if (worksheet.Name == "BranchMapping")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);

                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Active"), new[] { "Yes", "No" });
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Branch Name"), "BranchName", "branchlist");
                    }
                }
                else if (pagename == "SupplierSetup")
                {
                    if (sheet.sheetName == "SupplierDetail")
                    {
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Supplier Type*"), new[] { "Domestic", "Import" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "GST Category*"), new[] { "Registered", "Un-Registered", "Exempted", "Composition" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "TDS Applicable*"), new[] { "Yes", "No" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Applicable On*"), new[] { "Monthly Processing", "Bill Wise" });

                        // Adding dynamic dropdowns 
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "GL Account Group*"), "GLAccountGroup", "AccountGroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "GL Reporting Group"), "GLReportingGroup", "GLReportingGroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Default Currency*"), "Currency", "Currencylist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Category*"), "Category", "Categorylist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Portfolio*"), "Portfolio", "Portfoliolist");
                    }
                    else if (worksheet.Name == "Address")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Billing Address*"), new[] { "Yes", "No" });

                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Country*"), "Country", "countrylist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "State*"), "State", "statelist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "District*"), "District", "districtlist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "City*"), "City", "citylist");
                    }
                    else if (worksheet.Name == "BranchMapping")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Active"), new[] { "Yes", "No" });
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Branch Name"), "BranchName", "branchlist");
                    }

                }
                else if (pagename == "GeneralLedger")
                {
                    if (worksheet.Name == "GeneralLedgerDetail")
                    {
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Account Type*"), new[] { "Bank", "Cash", "Others", "Tax Authority" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Cash Flow Type"), new[] { "Financial", "Investment", "Operational" });
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Account Group*"), "AccountGroup", "AccountGroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Currency"), "Currency", "Currencylist");
                    }
                    else if (worksheet.Name == "BranchMapping")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Active"), new[] { "Yes", "No" });
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Branch Name"), "BranchName", "branchlist");
                    }
                }
                else if (pagename == "ItemSetup")
                {
                    if (sheet.sheetName == "ItemsDetail")
                    {
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Issue Method*"), new[] { "FIFO", "LIFO", "EXPIRY DATE" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Costing Method"), new[] { "STANDARD", "WEIGHTED AVERAGE", "MOVING AVERAGE" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Workstation"), new[] { "Yes", "No" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Tax Exempted"), new[] { "Yes", "No" });
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Sub-Item"), new[] { "Yes", "No" });

                        // Adding dynamic dropdowns 
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Items Group*"), "ItemGroup", "ItemGroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "HSN Code*"), "HSNCode", "HSNCodelist");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Base UOM*"), "BASEUOM", "baseuomlist");
                    }
                    else if (worksheet.Name == "SubItemDetail")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                    }
                    else if (worksheet.Name == "ItemAttribute")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Attribute Name"), "ItemsAttributes", "AttributeNameList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Attribute Value"), "ItemsAttributes", "AttributeValueList");
                    }
                    else if (worksheet.Name == "BranchMapping")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                        ApplyDataValidation(worksheet, FindColumnIndex(worksheet, "Active"), new[] { "Yes", "No" });
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Branch Name"), "BranchName", "branchlist");
                    }
                    else if (worksheet.Name == "ItemPortfolio")
                    {
                        AddNameDropdown(package, worksheet, pagename, 1);
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Portfolio Name"), "Portfolio", "portfoliolist");
                    }
                }
                /*Added by Surbhi on 22/01/2026 for Asset Registration(105106101)*/
                else if (pagename == "AsssetRegistrationSetup")
                {
                    if (sheet.sheetName == "AssetDetail")
                    {
                        // Adding dynamic dropdowns 
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Group*"), "Group", "GroupList");
                        AddDropdown(package, worksheet, FindColumnIndex(worksheet, "Assigned Requirement Area*"), "AssignedRequirementArea", "AssignedRequirementAreaList");
                    }

                }
            }
        }
        private void ProtectSheets(ExcelWorkbook workbook, List<string> sheetNames)
        {
            string password = "enrep";
            foreach (var sheetName in sheetNames)
            {
                var worksheet = workbook.Worksheets[sheetName];
                if (worksheet != null)
                {
                    worksheet.Protection.SetPassword(password);
                    worksheet.Protection.IsProtected = true;
                    worksheet.Protection.AllowSelectLockedCells = false;
                }
            }
        }
        private void RenameTableColumns(DataTable table, string pagename)
        {
            if (pagename == "QCItemParameter")
            {
                if (table.TableName == "Table") table.TableName = "ItemName";
                else if (table.TableName == "Table1") table.TableName = "UOM";
                else if (table.TableName == "Table2") table.TableName = "ParameterName";
            }
            else if (pagename == "CustomerPriceList")
            {
                if (table.TableName == "Table") table.TableName = "ItemName";
            }
            else if (pagename == "CustomerSetup")
            {
                if (table.TableName == "Table") table.TableName = "Category";
                else if (table.TableName == "Table1") table.TableName = "Portfolio";
                else if (table.TableName == "Table2") table.TableName = "SalesRegion";
                else if (table.TableName == "Table3") table.TableName = "PriceGroup";
                else if (table.TableName == "Table4")
                {
                    table.TableName = "GLAccountGroup";
                    if (table.Columns.Contains("acc_grp_id"))
                    {
                        table.Columns.Remove("acc_grp_id");
                    }
                    if (table.Columns.Contains("AccGroupChildNood"))
                    {
                        table.Columns["AccGroupChildNood"].ColumnName = "GL Account Group";
                    }
                }
                else if (table.TableName == "Table5") table.TableName = "BranchName";
                else if (table.TableName == "Table6") table.TableName = "Currency";
                else if (table.TableName == "Table7") table.TableName = "GLReportingGroup";
                else if (table.TableName == "Table8") table.TableName = "DefaultTransporter";
                else if (table.TableName == "Table9") table.TableName = "Country";
                else if (table.TableName == "Table10") table.TableName = "State";
                else if (table.TableName == "Table11") table.TableName = "District";
                else if (table.TableName == "Table12") table.TableName = "City";
                else if (table.TableName == "Table13") table.TableName = "CustomerGroup";
                else if (table.TableName == "Table14") table.TableName = "CustomerZone";
            }
            else if (pagename == "SupplierSetup")
            {
                if (table.TableName == "Table") table.TableName = "GLAccountGroup";
                else if (table.TableName == "Table1") table.TableName = "Category";
                else if (table.TableName == "Table2") table.TableName = "Portfolio";
                else if (table.TableName == "Table3") table.TableName = "BranchName";
                else if (table.TableName == "Table4") table.TableName = "Currency";
                else if (table.TableName == "Table5") table.TableName = "GLReportingGroup";
                else if (table.TableName == "Table6") table.TableName = "Country";
                else if (table.TableName == "Table7") table.TableName = "State";
                else if (table.TableName == "Table8") table.TableName = "District";
                else if (table.TableName == "Table9") table.TableName = "City";
            }
            else if (pagename == "ItemSetup")
            {
                if (table.TableName == "Table") table.TableName = "ItemGroup";
                else if (table.TableName == "Table1") table.TableName = "HSNCode";
                else if (table.TableName == "Table2") table.TableName = "BASEUOM";
                else if (table.TableName == "Table3") table.TableName = "ItemsAttributes";
                else if (table.TableName == "Table4") table.TableName = "BranchName";
                else if (table.TableName == "Table5") table.TableName = "Portfolio";
            }
            else if (pagename == "GeneralLedger")
            {
                if (table.TableName == "Table")
                {
                    table.TableName = "AccountGroup";
                    if (table.Columns.Contains("acc_grp_id"))
                    {
                        table.Columns.Remove("acc_grp_id");
                    }
                    if (table.Columns.Contains("AccGroupChildNood"))
                    {
                        table.Columns["AccGroupChildNood"].ColumnName = "GL Account Group";
                    }
                }
                else if (table.TableName == "Table1") table.TableName = "BranchName";
                else if (table.TableName == "Table2")
                {
                    table.TableName = "Currency";
                    if (table.Columns.Contains("curr_id"))
                    {
                        table.Columns.Remove("curr_id");
                    }
                    if (table.Columns.Contains("curr_name"))
                    {
                        table.Columns["curr_name"].ColumnName = "Currency";
                    }
                }
            }
            /*Added by Surbhi on 22/01/2026 for Asset Registration(105106101)*/
            else if (pagename == "AsssetRegistrationSetup")
            {
                if (table.TableName == "Table1") table.TableName = "Group";
                else if (table.TableName == "Table2") table.TableName = "AssignedRequirementArea";
            }
        }
        private void ApplyHeaderStyle(ExcelWorksheet worksheet, DataTable table)
        {
            var headerRow = worksheet.Cells[1, 1, 1, table.Columns.Count];
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Font.Name = "Calibri";
            headerRow.Style.Font.Size = 12;
            headerRow.Style.Font.Italic = true;
        }
        public int FindColumnIndex(ExcelWorksheet worksheet, string columnName)
        {
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                if (worksheet.Cells[1, col].Text == columnName)
                {
                    return col;
                }
            }
            return -1;
        }
        public void AddDropdown(ExcelPackage package, ExcelWorksheet worksheet, int colIndex, string sheetName, string rangeName)
        {
            string errorTitle = "Invalid";
            string errorMessage = "Please select a valid record from the list.";
            var dataSheet = package.Workbook.Worksheets[sheetName];

            if (dataSheet != null)
            {
                int rowCount = dataSheet.Dimension.End.Row;
                var dataList = new List<string>();
                for (int row = 2; row <= rowCount; row++)
                {
                    var value = "";
                    if (rangeName == "AttributeValueList")
                    {
                        value = dataSheet.Cells[row, 2].Text;
                    }
                    else
                    {
                        value = dataSheet.Cells[row, 1].Text;
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        dataList.Add(value);
                    }
                }
                if (dataList.Count > 0)
                {
                    if (rangeName == "AttributeValueList")
                    {
                        var dataRange = dataSheet.Cells[2, 2, dataList.Count + 1, 2];
                        package.Workbook.Names.Add(rangeName, dataRange);
                        ApplyDropdownValidation(worksheet, colIndex, rangeName);
                    }
                    else
                    {
                        var dataRange = dataSheet.Cells[2, 1, dataList.Count + 1, 1];
                        package.Workbook.Names.Add(rangeName, dataRange);
                        ApplyDropdownValidation(worksheet, colIndex, rangeName);
                    }
                }
            }
        }
        public void AddNameDropdown(ExcelPackage package, ExcelWorksheet worksheet, string pagename, int accnameColumnIndex)
        {
            string colname = "";
            ExcelWorksheet pricelistWorksheet = null;

            if (pagename == "CustomerPriceList")
            {
                pricelistWorksheet = package.Workbook.Worksheets["PriceDetail"];
                colname = "Price List Name*(max 50 characters)";
            }
            if (pagename == "CustomerSetup")
            {
                pricelistWorksheet = package.Workbook.Worksheets["CustomerDetail"];
                colname = "Customer Name*(max 100 characters)";
            }
            if (pagename == "SupplierSetup")
            {
                pricelistWorksheet = package.Workbook.Worksheets["SupplierDetail"];
                colname = "Supplier Name*(max 100 characters)";
            }
            if (pagename == "GeneralLedger")
            {
                pricelistWorksheet = package.Workbook.Worksheets["GeneralLedgerDetail"];
                colname = "Account Name*(max 100 characters)";
            }
            if (pagename == "ItemSetup")
            {
                pricelistWorksheet = package.Workbook.Worksheets["ItemsDetail"];
                colname = "Item Description*(max 100 characters)";
            }
            if (pricelistWorksheet == null || string.IsNullOrEmpty(colname))
                return; // Exit if not initialized properly

            int accNameColumnIndex = -1;

            for (int col = 1; col <= pricelistWorksheet.Dimension.End.Column; col++)
            {
                if (pricelistWorksheet.Cells[1, col].Text == colname)
                {
                    accNameColumnIndex = col;
                    break;
                }
            }
            if (accNameColumnIndex != -1)
            {
                var accountNameRange = pricelistWorksheet.Cells[2, accNameColumnIndex, 1048576, accNameColumnIndex];
                var accountNameRangeName = "NameList";
                package.Workbook.Names.Add(accountNameRangeName, accountNameRange);

                var startRow = 2;
                var endRow = 1048576; // Excel's max row count
                var range = worksheet.Cells[startRow, accnameColumnIndex, endRow, accnameColumnIndex];
                var validation = range.DataValidation.AddListDataValidation();
                validation.ShowErrorMessage = true;
                validation.ErrorTitle = "Invalid";
                validation.Error = "Please select a valid record from the list.";
                validation.Formula.ExcelFormula = $"={accountNameRangeName}";
            }
        }
        public void ApplyDropdownValidation(ExcelWorksheet worksheet, int columnIndex, string rangeName)
        {
            string errorTitle = "Invalid Selection";
            string errorMessage = "Please select a valid value from the list.";
            if (!string.IsNullOrEmpty(rangeName))
            {
                var startRow = 2;  // Adjust as per your worksheet
                var endRow = 1048576;  // Adjust to limit rows if necessary
                var range = worksheet.Cells[startRow, columnIndex, endRow, columnIndex];
                var validation = range.DataValidation.AddListDataValidation();
                validation.ShowErrorMessage = true;
                validation.ErrorTitle = errorTitle;
                validation.Error = errorMessage;
                validation.Formula.ExcelFormula = $"={rangeName}";  // Reference to the named range
            }
        }
        public void ApplyDataValidation(ExcelWorksheet worksheet, int columnIndex, string[] listValues)
        {
            string errorTitle = "Invalid Selection";
            string errorMessage = "Please select a valid value from the list.";
            if (listValues != null && listValues.Length > 0)
            {
                var startRow = 2;
                var endRow = 1048576;
                var range = worksheet.Cells[startRow, columnIndex, endRow, columnIndex];
                var validation = range.DataValidation.AddListDataValidation();
                validation.ShowErrorMessage = true;
                validation.ErrorTitle = errorTitle;
                validation.Error = errorMessage;
                validation.Formula.ExcelFormula = $"\"{string.Join(",", listValues)}\"";
            }
        }


        private void ApplyHeaderStyle(ExcelWorksheet worksheet, int columnCount)
        {
            var headerRow = worksheet.Cells[1, 1, 1, columnCount];
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Font.Name = "Calibri";
            headerRow.Style.Font.Size = 12;
            headerRow.Style.Font.Italic = true;
        }
        private void ExcelSheetFormatting(ExcelWorksheet worksheet, DataTable table)
        {
            int totalRows = 1048576; // +1 for header
            int totalCols = table.Columns.Count;

            // Format all target cells (header + data) as Text BEFORE loading data
            worksheet.Cells[1, 1, totalRows, totalCols].Style.Numberformat.Format = "@";

            // Now load the data (Excel will treat everything as Text now)
            worksheet.Cells.LoadFromDataTable(table, true);

            // Autofit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Style header row
            var headerRow = worksheet.Cells[1, 1, 1, totalCols];
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Font.Name = "Calibri";
            headerRow.Style.Font.Size = 12;
            headerRow.Style.Font.Italic = true;
        }
        public static DateRange Comman_GetFutureDateRange()
        {
            DateTime toDate = DateTime.Today;

            string fromDateConfig = ConfigurationManager.AppSettings["FromDate"];
            int fromdate;

            if (string.IsNullOrEmpty(fromDateConfig))
            {
                fromdate = 30;
            }
            else
            {
                fromdate = Convert.ToInt32(fromDateConfig);
            }

            DateTime fromDate = toDate.AddDays(-fromdate);

            return new DateRange
            {
                FromDate = fromDate.ToString("yyyy-MM-dd"),
                ToDate = toDate.ToString("yyyy-MM-dd")
            };
        }
        //-----------------------External Email Alert---------------------------------
        // //Added by Nidhi on 01-08-2025
        public ActionResult GetSupplierEmail(string docid, string id, string type)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                }
                //List<PendingDocumentList> _MaterialIssueDetailList = new List<PendingDocumentList>();
                DataTable suppDetail = new DataTable();

                suppDetail = _Common_IServices.GetSupplierEmail(Comp_id, Br_id, docid, id, type);
                ViewBag.suppDetail = suppDetail;
                //return Json(suppDetail, JsonRequestBehavior.AllowGet);
                return Json(JsonConvert.SerializeObject(suppDetail), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        //Added by Nidhi on 01-08-2025
        public ActionResult ViewEmailAlert(string mail, string status, string docid, string Doc_no, string Doc_dt, string FilePath)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_id = Session["UserId"].ToString();
                }
                var commonCont = new CommonController(_Common_IServices);
                DataTable dt = new DataTable();
                ViewBag.mailmessage = _Common_IServices.ViewEmailAlert(Comp_id, Br_id, User_id, docid, Doc_no, status, mail, FilePath);
                return View("~/Areas/Common/Views/Cmn_MailView.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult EmailAlertLogDetails(string docid, string Doc_no)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_id = Session["BranchId"].ToString();
                }
                ViewBag.DocumentMenuId = docid;
                if (docid == "105104135115")
                {
                    var part = Doc_no.Split('_');
                    Doc_no = part[1];
                }
                ViewBag.EmailLog = _Common_IServices.EmailAlertLogDetails(Comp_id, Br_id, docid, Doc_no);
                return PartialView("~/Areas/Common/Views/Cmn_LogView.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        //------------------External Email Alert End ---------------------------------------------
        [HttpPost]
        public JsonResult GetOCHSNDetails(string OC_ID, string SuppStateId)
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
                DataSet result = _Common_IServices.GetOCHSNDL(OC_ID, Comp_ID, Br_ID, SuppStateId);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
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
        //Added By Nidhi on 14-08-2025
        public string SaveAlertDocument_MailExt(byte[] data, string fileName)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);

                string folderPath = path + ("..\\Attachment\\LogsFile\\ExternalEmailAlertPDFs\\");
                if (!Directory.Exists(folderPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folderPath);
                }
                string finalFilePath = folderPath + fileName;
                if (data != null)
                    System.IO.File.WriteAllBytes(finalFilePath, data);
                return finalFilePath;
            }
            catch (Exception exc)
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return "Error";
            }
        }
        [HttpPost]
        public JsonResult Cmn_CheckSessionAvailablity()/* Created by Suraj Maurya on 26-08-2025 to check session availability */
        {

            try
            {
                bool CheckSession = false;
                JsonResult DataRows;
                if (Session["CompId"] != null && Session["BranchId"] != null)
                {
                    CheckSession = true;
                }
                DataRows = CheckSession ? Json("Available") : Json("Expired");
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        //Added by Nidhi on 02-09-2025
        public string SendEmailAlert(string compID, string brchID, string userID, string mail_id, string status, string docid, string Doc_no, string Doc_dt, string filepath)
        {
            if (string.IsNullOrEmpty(compID) || string.IsNullOrEmpty(brchID) || string.IsNullOrEmpty(userID))
            {
                if (Session["CompId"] != null)
                {
                    compID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    brchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    userID = Session["UserId"].ToString();
                }
            }
            string message = "";
            string mail_cont = "";
            string file_path = "";
            if (status == "A")
            {
                try
                {
                    string keyword = @"\ExternalEmailAlertPDFs\";
                    int index = filepath.IndexOf(keyword);
                    file_path = (index >= 0) ? filepath.Substring(index) : filepath;
                    message = _Common_IServices.SendAlertEmailExternal(compID, brchID, userID, docid, Doc_no, "A", mail_id, filepath);
                    if (message.Contains(","))
                    {
                        //var a = message.Split(',');
                        var a = message.Split(new[] { ',' }, 2);
                        message = a[0];
                        mail_cont = a[1];
                    }
                    if (message == "success")
                    {
                        if (docid == "105104135115")
                        {
                            string[] parts = Doc_no.Split('_');
                            //string custname = parts.Length >= 2 ? $"{parts[0]}_{parts[1]}" : string.Empty;
                            string custnameAccId = parts[4];
                            _Common_IServices.SendAlertlog(compID, brchID, "Email", custnameAccId, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            _Common_IServices.SendAlertlog(compID, brchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
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
                        if (docid == "105104135115")
                        {
                            string[] parts = Doc_no.Split('_');
                            //string custname = parts.Length >= 2 ? $"{parts[0]}_{parts[1]}" : string.Empty;
                            string custnameAccId = parts[4];
                            _Common_IServices.SendAlertlog(compID, brchID, "Email", custnameAccId, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            _Common_IServices.SendAlertlog(compID, brchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = "ErrorInMail";
                    if (message == "ErrorInMail")
                    {
                        mail_cont = "Invalid sender email configuration";
                    }
                    if (docid == "105104135115")
                    {
                        string[] parts = Doc_no.Split('_');
                        string custname = parts.Length >= 2 ? $"{parts[0]}_{parts[1]}" : string.Empty;
                        _Common_IServices.SendAlertlog(compID, brchID, "Email", custname, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                    }
                    else
                    {
                        _Common_IServices.SendAlertlog(compID, brchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                    }
                }
            }
            if (status == "C" || status == "FC" || status == "AM")
            {
                try
                {
                    message = _Common_IServices.SendAlertEmailExternal1(compID, brchID, userID, docid, Doc_no, status, mail_id);
                    if (message.Contains(","))
                    {
                        var a = message.Split(',');
                        message = a[0];
                        mail_cont = a[1];
                    }
                    if (message == "success")
                    {
                        _Common_IServices.SendAlertlog(compID, brchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
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
                        _Common_IServices.SendAlertlog(compID, brchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                    }
                }
                catch (Exception ex)
                {
                    message = "ErrorInMail";
                    if (message == "ErrorInMail")
                    {
                        mail_cont = "Invalid sender email configuration";
                    }
                    _Common_IServices.SendAlertlog(compID, brchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                }
            }
            return message;
        }
        //Added by nidhi on 12-09-2025
        public string CheckMailAttch(string Comp_ID, string Br_ID, string docid, string docstatus)
        {
            try
            {
                if (docstatus == "Forward")
                {
                    docstatus = "F";
                }
                if (docstatus == "Revert")
                {
                    docstatus = "RV";
                }
                if (docstatus == "Reject")
                {
                    docstatus = "RJ";
                }
                if (docstatus == "Approve" || docstatus == "Approved" || docstatus == "A" || docstatus == "AP")
                {
                    docstatus = "A";
                }
                if (docstatus == "Cancelled" || docstatus == "C" || docstatus == "Cancel")
                {
                    docstatus = "C";
                }
                if (docstatus == "Amend" || docstatus == "AM" || docstatus == "Amended")
                {
                    docstatus = "AM";
                }
                if (docstatus == "Force close" || docstatus == "FC")
                {
                    docstatus = "FC";
                }
                string mailattch = _Common_IServices.CheckMailAttch(Comp_ID, Br_ID, docid, docstatus);

                if (!string.IsNullOrEmpty(mailattch))
                {
                    return mailattch;
                }
                return null;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "Error";
            }
        }

        //Added by Suraj Maurya on 06-10-2025
        public ActionResult Cmn_GetDataToCsv([System.Web.Http.FromBody] DataTableRequest request, dynamic Dynmcdata, StringBuilder header1 = null)
        {
            try
            {
                var sb = new StringBuilder();
                if (header1 != null)//For Extra Header row. Added by Suraj on 06-11-2025
                {
                    sb.Append(header1);
                }
                sb.AppendLine(string.Join(",", request.ColumnsExport.Select(c => c.title)));

                foreach (var row in Dynmcdata)
                {

                    var rowValues = request.ColumnsExport.Select(c =>
                    {
                        //var value = row.GetType().GetProperty(c.data)?.GetValue(row, null)?.ToString() ?? "";
                        string value = "";
                        if (row is IDictionary<string, string> dict)
                        {
                            // Get value by key safely
                            dict.TryGetValue(c.data, out var objValue);
                            value = objValue ?? "";
                        }
                        else
                        {
                            // fallback to reflection
                            value = row.GetType().GetProperty(c.data)?.GetValue(row, null)?.ToString() ?? "";
                        }
                        value = value.Trim().Replace(",", "");
                        value = value.Replace("\"", "\"\"");   // escape double quotes
                        value = value.Replace("\r", " ").Replace("\n", " "); // clean newlines

                        return $"\"{value}\"";  // wrap in quotes
                    });

                    sb.AppendLine(string.Join(",", rowValues));
                }


                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "StockDetails.csv");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult Cmn_GetItemAliasList(string SearchAlias)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                JsonResult DataRows;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet result = _Common_IServices.Cmn_Get_ItemAliasList(Comp_ID, SearchAlias);
                DataRow dr = result.Tables[0].NewRow();
                dr[0] = "";
                dr[1] = "---Select---";
                result.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public string ReplaceDefault(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value.Replace("multiselect-all", "0");
            }
            return value;
        }
        public object IsBlank(string input, object output)//Added by Suraj Maurya on 02-12-2025
        {
            return input == "" ? output : input;
        }
        public JsonResult Cmn_GetParameterValues()// Added by Suraj Maurya on 09-12-2025
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                JsonResult DataRows;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataSet result = _Common_IServices.Cmn_GetParameterValues(Comp_ID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public ActionResult Cmn_GetItemStockWhLotBatchSerialWise(string ItemID, string DocMenuId)/* Added by Suraj Maurya on 29-12-2025 */
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BranchID = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataTable result = _Common_IServices.GetItemStockWhLotBatchSerialWise(CompID, BranchID, ItemID);
                ViewBag.ItemStockBatchWise = result;
                ViewBag.DocMenuId = DocMenuId;
                return PartialView("~/Areas/Common/Views/Cmn_PartialBatchSerialLotWiseDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }


        public ActionResult DownloadFile(string itemNamesList, string uomList, string SerialReceivedQuantity, string SerialRejQuantity,
            string SerialReworkQuantity, string TotalQty, string Flag)
        {
            try
            {
                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                DataTable ItemDetail = new DataTable();
                DataSet obj_ds = new DataSet();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("SrNo.", typeof(string));
                dtheader.Columns.Add("Item Description*", typeof(string));
                dtheader.Columns.Add("UOM*", typeof(string));
                if (Flag == "GRNSerialExcel")
                {
                    dtheader.Columns.Add("Quantity Type*", typeof(string));
                }
                dtheader.Columns.Add("Manufacturer Name (max 100 characters)", typeof(string));
                dtheader.Columns.Add("MRP", typeof(float));
                dtheader.Columns.Add("Manufactured Date(DD-MM-YYYY)", typeof(DateTime));
                dtheader.Columns.Add("Serial No.*(max 25 characters)", typeof(string));

                if (!string.IsNullOrEmpty(TotalQty))
                {

                    int totalQty = Convert.ToInt32(TotalQty);

                    int recQty = Convert.ToInt32(SerialReceivedQuantity);
                    int rejQty = Convert.ToInt32(SerialRejQuantity);
                    int rwkQty = Convert.ToInt32(SerialReworkQuantity);
                    int rowID = 1;
                    // Accepted
                    for (int i = 1; i <= recQty; i++)
                    {
                        DataRow row = dtheader.NewRow();
                        row["SrNo."] = rowID;
                        row["Item Description*"] = itemNamesList;
                        row["UOM*"] = uomList;
                        if (Flag == "GRNSerialExcel")
                        {
                            row["Quantity Type*"] = "Accepted";
                        }
                        rowID++;
                        dtheader.Rows.Add(row);
                    }
                    if (Flag == "GRNSerialExcel")
                    {
                        // Rejected
                        for (int i = 1; i <= rejQty; i++)
                        {
                            DataRow row = dtheader.NewRow();
                            row["SrNo."] = rowID;
                            row["Item Description*"] = itemNamesList;
                            row["UOM*"] = uomList;
                            row["Quantity Type*"] = "Rejected";
                            rowID++;
                            dtheader.Rows.Add(row);
                        }

                        // Reworkable
                        for (int i = 1; i <= rwkQty; i++)
                        {
                            DataRow row = dtheader.NewRow();
                            row["SrNo."] = rowID;
                            row["Item Description*"] = itemNamesList;
                            row["UOM*"] = uomList;
                            row["Quantity Type*"] = "Reworkable";
                            rowID++;
                            dtheader.Rows.Add(row);
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
                if (Flag == "GRNSerialExcel")
                {
                    ExcelFileName = "ImportGRNSerialTemplate";
                    pagename = "GRNSerialExcel";
                }
                else if (Flag == "DPISerialExcel")
                {
                    ExcelFileName = "ImportDPISerialTemplate";
                    pagename = "DPISerialExcel";
                }

                else if (Flag == "StockTakeSerialExcel")
                {
                    ExcelFileName = "ImportStkTakeSerialTemplate";
                    pagename = "StockTakeSerialExcel";
                }
                else if (Flag == "ExtRcptSerialExcel")
                {
                    ExcelFileName = "ImportExtRcptSerialTemplate";
                    pagename = "ExtRcptSerialExcel";
                }
                else if (Flag == "OpeningRcptSerialExcel")
                {
                    ExcelFileName = "ImportOpeningRcptSerialTemplate";
                    pagename = "OpeningRcptSerialExcel";
                }
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
        public ActionResult ValidateExcelFile(string uploadStatus, string ItemName, string Flag, string Doc_id)
        {
            try
            {
                ViewBag.DocumentMenuId = Doc_id;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            conString = "Invalid File";
                            break;
                    }
                    if (conString == "Invalid File")
                        return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
                    DataSet ds = new DataSet();
                    DataTable dtItems = new DataTable();

                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();

                                string itemsQuery = "SELECT * FROM [SerialDetail$] WHERE LEN([Item Description*]) > 0;";

                                //Read Data from First Sheet.

                                connExcel.Open();
                                cmdExcel.CommandText = itemsQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtItems);
                                connExcel.Close();



                                ds.Tables.Add(dtItems);

                                DataSet dts = VerifyData(ds, uploadStatus, ItemName, Flag);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportSerialPreview = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialImportSerialDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet dsItems, string uploadStatus, string ItemName, string Flag)
        {
            try
            {
                if (dsItems == null || dsItems.Tables.Count == 0 || dsItems.Tables[0].Rows.Count == 0)
                    return null;

                string compId = Session["compid"]?.ToString() ?? string.Empty;
                string BrchID = Session["BranchId"]?.ToString() ?? string.Empty;

                DataSet preparedDs = PrepareDataset(dsItems, Flag);

                DataSet result = _Common_IServices.GetVerifiedDataOfExcel(
                    compId,
                    BrchID,
                    preparedDs.Tables[0], ItemName, Flag
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

        public DataSet PrepareDataset(DataSet dsItems, string Flag)
        {
            string compId = "", brId = "", userId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();

            DataTable dtheader = new DataTable();

            //Items Details//
            dtheader.Columns.Add("Item_Description", typeof(string));
            dtheader.Columns.Add("UOM", typeof(string));
            dtheader.Columns.Add("Quantity_Type", typeof(string));
            dtheader.Columns.Add("Manufacturer_Name", typeof(string));
            dtheader.Columns.Add("MRP", typeof(string));
            dtheader.Columns.Add("Manufactured_Date", typeof(string));
            dtheader.Columns.Add("Serial_No.", typeof(string));
            for (int i = 0; i < dsItems.Tables[0].Rows.Count; i++)
            {
                DataTable dtItem = dsItems.Tables[0];
                if (dtItem.Rows[i][0].ToString() != "" && Convert.ToInt32(dtItem.Rows[i][0].ToString()) > 0)
                {
                    DataRow dtr = dtheader.NewRow();

                    dtr["Item_Description"] = dtItem.Rows[i][1].ToString().Trim();
                    dtr["UOM"] = dtItem.Rows[i][2].ToString().Trim();
                    if (Flag == "GRNSerialExcel")
                    {
                        dtr["Quantity_Type"] = dtItem.Rows[i][3].ToString().Trim();
                        dtr["Manufacturer_Name"] = dtItem.Rows[i][4].ToString().Trim();
                        dtr["MRP"] = dtItem.Rows[i][5].ToString();
                        string dateString = dtItem.Rows[i][6].ToString().Trim();

                        dtr["Manufactured_Date"] = dateString;


                        dtr["Serial_No."] = dtItem.Rows[i][7].ToString().Trim();
                    }
                    else
                    {
                        dtr["Manufacturer_Name"] = dtItem.Rows[i][3].ToString().Trim();
                        dtr["MRP"] = dtItem.Rows[i][4].ToString();
                        string dateString = dtItem.Rows[i][5].ToString().Trim();
                        dtr["Manufactured_Date"] = dateString;
                        dtr["Serial_No."] = dtItem.Rows[i][6].ToString().Trim();
                    }


                    dtheader.Rows.Add(dtr);
                }

            }

            //---------------------------End-------------------------------------
            DataSet dts = new DataSet();

            dts.Tables.Add(dtheader);

            return dts;
        }

        [HttpPost]
        public ActionResult Cmn_GetErrorDetails(List<string> errors)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ERRORMSG", typeof(string));


            foreach (var err in errors)
            {
                dt.Rows.Add(err);
            }

            ViewBag.ErrorDetails = dt;

            return PartialView("~/Areas/Common/Views/Cmn_PartialExportErrorDetail.cshtml");
        }

         [HttpPost]
        public JsonResult Cmn_getSchemeFocDetail_test(string item_id, string cust_id, string order_qty, string order_value, string ord_dt)/* Created by Suraj Maurya on 12-01-2026 */
        {
            //Created for test
            try
            {

                JsonResult DataRows;
                string compId = Session["compid"]?.ToString() ?? string.Empty;
                string BrchID = Session["BranchId"]?.ToString() ?? string.Empty;
                DataSet result = _Common_IServices.Cmn_getSchemeFocDetail(
                    compId, BrchID, item_id, cust_id, order_qty, order_value, ord_dt
                );
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }



    }
    internal class CommonFunctions
    {
        public static string IsNull(string In, string Out)
        {
            return string.IsNullOrEmpty(In) ? Out : In;
        }
        public static string InCase(string In, string Matcher, string Out)
        {
            return In == Matcher ? Out : In;
        }
        public static object IsBlank(string input, object output)//Added by Suraj Maurya on 17-12-2025
        {
            return input == "" ? output : input;
        }
    }
}