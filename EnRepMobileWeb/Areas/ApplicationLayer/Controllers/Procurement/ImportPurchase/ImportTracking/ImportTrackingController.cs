using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ImportPurchase.ImportTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ImportPurchase.ImportTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.ImportPurchase.ImportTracking
{/*----------------THIS PAGE IS CREATED BY HINA SHARMA ON 13-02-2025----------------------------------*/
    public class ImportTrackingController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105101140130", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        ImportTracking_ISERVICES _ImportTracking_ISERVICES;
        Common_IServices _Common_IServices;
        public ImportTrackingController(Common_IServices _Common_IServices, ImportTracking_ISERVICES _ImportTracking_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ImportTracking_ISERVICES = _ImportTracking_ISERVICES;
        }
        // GET: ApplicationLayer/ImportTracking
        public ActionResult ImportTracking(URLModelDetails URLModel /*ImportTracking_Model _ITModel*/)
        {
            //ViewBag.MenuPageName = getDocumentName();
            //CommonPageDetails();
            GetCompDetail();
            
            //TempData["ListFilterData"] = ListFilterData;
            
            var ITModel = TempData["ModelData"] as ImportTracking_Model;
            if (ITModel != null)
            {
                ITModel.UserID = UserID;
                CommonPageDetails();
                ITModel.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        ITModel.SuppID = a[0].Trim();
                        ITModel.SrcDocNo = a[1].Trim();

                        ITModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                }
                ITModel.Title = title;
                ITModel.ImpTrckSearch = "0";
                GetAutoCompleteSearchSuppList(ITModel);
                if(ITModel.Command=="Edit")
                {
                    List<SrcDocNoList> srdocLists = new List<SrcDocNoList>();
                    srdocLists.Insert(0, new SrcDocNoList() { SrcDocnoId = ITModel.SrcDocNo, SrcDocnoVal = ITModel.SrcDocNo });
                    ITModel.DocNoLists = srdocLists;

                    List<SupplierName> _SuppList = new List<SupplierName>();
                    _SuppList.Add(new SupplierName { supp_id = ITModel.SuppID, supp_name = ITModel.SuppName });
                    ITModel.SupplierNameList = _SuppList;
                    
                }
                else
                {
                    List<SrcDocNoList> srdocLists = new List<SrcDocNoList>();
                    srdocLists.Insert(0, new SrcDocNoList() { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                    ITModel.DocNoLists = srdocLists;
                }
                if (ITModel.TransType == "Update" || ITModel.TransType == "Edit")
                {
                    ShowDataAfterSave(ITModel);
                }
                
                return View("~/Areas/ApplicationLayer/Views/Procurement/ImportPurchase/ImportTracking/ImportTrackingDetail.cshtml", ITModel);
                
            }
            else
            {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                //    var commCont = new CommonController(_Common_IServices);
                //    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                //    {
                //        TempData["Message1"] = "Financial Year not Exist";
                //    }
                //    /*End to chk Financial year exist or not*/
                ImportTracking_Model ITModel1 = new ImportTracking_Model();
                ITModel1.UserID = UserID;
                
                //ITModel1.Message = "New";
                //ITModel1.DocumentStatus = "D";
                ITModel1.BtnName = "BtnToDetailPage";
                ITModel1.TransType = "";
                ITModel1.Command = "New";
                
                CommonPageDetails();
                ITModel1.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ITModel1.ImpTrckSearch = "0";
                GetAutoCompleteSearchSuppList(ITModel1);
                List<SrcDocNoList> srdocLists1 = new List<SrcDocNoList>();
                srdocLists1.Insert(0, new SrcDocNoList() { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
                ITModel1.DocNoLists = srdocLists1;
                ITModel1.Title = title;
                if (URLModel.Src_DocNo != null || URLModel.Src_DocDate != null)
                {
                    ITModel1.SrcDocNo = URLModel.Src_DocNo;
                    ITModel1.Message = "New";
                    
                }
                if (URLModel.BtnName != null)
                {
                    ITModel1.BtnName = URLModel.BtnName;
                }
                if (URLModel.Command != null)
                {
                    ITModel1.Command = URLModel.Command;
                }
                if (URLModel.TransType != null)
                {
                    ITModel1.TransType = URLModel.TransType;
                }

                if (ITModel1.TransType == "Update" || ITModel1.TransType == "Edit")
                {
                    ShowDataAfterSave(ITModel1);
                   
                }

                return View("~/Areas/ApplicationLayer/Views/Procurement/ImportPurchase/ImportTracking/ImportTrackingDetail.cshtml", ITModel1);
            }
            
        }
        private void ShowDataAfterSave(ImportTracking_Model ITModel)
        {
            try
            {
                GetCompDetail();
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    ITModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                }
                DataSet ds = new DataSet();
                string PONo = ITModel.SrcDocNo;
                ds = _ImportTracking_ISERVICES.Edit_ImpTrackDetail(CompID, BrchID,PONo);

                ITModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                ITModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                ITModel.SrcDocNo = ds.Tables[0].Rows[0]["ord_no"].ToString();
                ITModel.SrcDocDate = ds.Tables[0].Rows[0]["ord_dt"].ToString();
                ITModel.ImpFileNo = ds.Tables[0].Rows[0]["imp_file_no"].ToString();
                ITModel.Curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                ITModel.Currency = ds.Tables[0].Rows[0]["curr_name"].ToString();

                List<SupplierName> _SuppList = new List<SupplierName>();
                _SuppList.Add(new SupplierName { supp_id = ITModel.SuppID, supp_name = ITModel.SuppName });
                ITModel.SupplierNameList = _SuppList;

                List<SrcDocNoList> srdocLists = new List<SrcDocNoList>();
                srdocLists.Insert(0, new SrcDocNoList() { SrcDocnoId = ITModel.SrcDocNo, SrcDocnoVal = ITModel.SrcDocNo });
                ITModel.DocNoLists = srdocLists;

                ITModel.Trade_Term = ds.Tables[0].Rows[0]["trade_terms"].ToString();
                ITModel.CntryOrigin = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                ITModel.PortOrigin = ds.Tables[0].Rows[0]["port_origin"].ToString();
                ITModel.Remarks = ds.Tables[0].Rows[0]["remark"].ToString();
                ITModel.Create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString();

                ITModel.NewDocumentDetail = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                
                //string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                string create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString();
                //string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                //ITModel.Status = doc_status;
                
                if (create_id != UserID)
                {
                   ITModel.BtnName = "Refresh";
                }
                ITModel.Title = title;
                //List<ImpTrck_Document_Details> ArrItem = new List<ImpTrck_Document_Details>();
                //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //{
                //    ImpTrck_Document_Details fid = new ImpTrck_Document_Details();
                //    fid.SNo = Convert.ToString(ds.Tables[1].Rows[i]["sno"]);
                //    fid.Date = Convert.ToString(ds.Tables[1].Rows[i]["date"]);
                //    fid.DocNo = Convert.ToString(ds.Tables[1].Rows[i]["doc_no"]);
                //    fid.DocDate = Convert.ToString(ds.Tables[1].Rows[i]["doc_dt"]);
                //    fid.Status = Convert.ToString(ds.Tables[1].Rows[i]["status"]);
                //    fid.EntryFlag_List = Convert.ToString(ds.Tables[1].Rows[i]["entry_flag"]);
                //    fid.SrcDocPONo_List = Convert.ToString(ds.Tables[1].Rows[i]["ord_no"]);
                //    ArrItem.Add(fid);
                //}
                //ITModel.ImpTrck_Document_Details_List = ArrItem;
                ViewBag.DocumentDetails = ds.Tables[1];
                //ViewBag.AttechmentDetails = ds.Tables[2];
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        public ActionResult GetAutoCompleteSearchSuppList(ImportTracking_Model _ITModel)
        {
            string SupplierName = string.Empty;
            //string SrcType = string.Empty;
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
                if (string.IsNullOrEmpty(_ITModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _ITModel.SuppName;
                }
                SuppType = "I";

                CustList = _ImportTracking_ISERVICES.GetSupplierList(Comp_ID,Br_ID, SuppType, SupplierName);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }

                _ITModel.SupplierNameList = _SuppList;
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
        [HttpPost]
        public JsonResult GetSrcDocPONumberList(string Supp_id)
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
                DataSet result = _ImportTracking_ISERVICES.GetSrcDocNumberList(Comp_ID, Br_ID,Supp_id);

                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";
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
        [HttpPost]
        public JsonResult GetSrcPONumDetail(string PONo)
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
                DataSet result = _ImportTracking_ISERVICES.GetPONumberDetail(Comp_ID, Br_ID, PONo);
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
        [ValidateAntiForgeryToken]
        public ActionResult ImportTrackingBtnCommand(ImportTracking_Model _ITModel, string command)
        {
            try
            {
                
                switch (command)
                {
                    case "Edit":
                        _ITModel.TransType = "Update";
                        _ITModel.Command = command;
                        _ITModel.BtnName = "BtnEdit";
                        _ITModel.Message = null;
                        _ITModel.SrcDocDate = _ITModel.SrcDocDate;
                        TempData["ModelData"] = _ITModel;
                        URLModelDetails URLModel = new URLModelDetails();
                        URLModel.Src_DocDate = _ITModel.SrcDocDate;
                        URLModel.Src_DocNo = _ITModel.SrcDocNo;
                        URLModel.TransType = "Update";
                        URLModel.BtnName = "BtnEdit";
                        URLModel.Command = command;
                        TempData["ListFilterData"] = _ITModel.ListFilterData1;
                        return RedirectToAction("ImportTracking", URLModel);

                    

                    case "Save":
                        if (_ITModel.TransType == null)
                        {
                            _ITModel.TransType = command;
                        }
                        SaveImportTrackingData(_ITModel);
                        //if (_ITModel.Message == "DataNotFound")
                        //{
                        //    return View("~/Views/Shared/Error.cshtml");
                        //}
                        URLModelDetails URLModelSave = new URLModelDetails();
                        URLModelSave.Src_DocDate = _ITModel.SrcDocDate;
                        
                        URLModelSave.Src_DocNo = _ITModel.SrcDocNo;
                        URLModelSave.Command = _ITModel.Command;
                        URLModelSave.TransType = _ITModel.TransType;
                        URLModelSave.BtnName = _ITModel.BtnName;
                        TempData["ModelData"] = _ITModel;
                        TempData["ListFilterData"] = _ITModel.ListFilterData1;
                        return RedirectToAction("ImportTracking", URLModelSave);

                    case "Refresh":
                        ImportTracking_Model ITModelRefresh = new ImportTracking_Model();
                        ITModelRefresh.BtnName = "Refresh";
                        ITModelRefresh.Command = command;
                        //ITModelRefresh.TransType = "Update";
                        ITModelRefresh.TransType = "";
                        TempData["ModelData"] = ITModelRefresh;
                        TempData["ListFilterData"] = _ITModel.ListFilterData1;
                        return RedirectToAction("ImportTracking");

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
        [NonAction]
        public ActionResult SaveImportTrackingData(ImportTracking_Model _ITModel)
        {
            string SaveMessage = "";
            string PageName = _ITModel.Title.Replace(" ", "");
            try
            {
                GetCompDetail();
                
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                //dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("CompId", typeof(int));
                dtheader.Columns.Add("BrId", typeof(int));
                dtheader.Columns.Add("Supp_id", typeof(int));
                dtheader.Columns.Add("SrcDocNo", typeof(string));
                dtheader.Columns.Add("SrcDocDate", typeof(string));
                dtheader.Columns.Add("ImpFileNo", typeof(string));
                dtheader.Columns.Add("Curr_id", typeof(string));
                dtheader.Columns.Add("TradeTerms", typeof(string));
                dtheader.Columns.Add("CountryofOrigin", typeof(string));
                dtheader.Columns.Add("PortofOrigin", typeof(string));
                dtheader.Columns.Add("Remarks", typeof(string));
                dtheader.Columns.Add("user_id", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                

                DataRow dtrowHeader = dtheader.NewRow();
                //dtrowHeader["TransType"] = _ITModel.TransType;
                dtrowHeader["TransType"] = "Update";
                //dtrowHeader["MenuID"] = DocumentMenuId;

                dtrowHeader["CompId"] = Session["CompId"].ToString();
                dtrowHeader["BrId"] = Session["BranchId"].ToString();
                dtrowHeader["Supp_id"] = _ITModel.SuppID;
                dtrowHeader["SrcDocNo"] = _ITModel.SrcDocNo;
                dtrowHeader["SrcDocDate"] = _ITModel.SrcDocDate;
                dtrowHeader["ImpFileNo"] = _ITModel.ImpFileNo;
                dtrowHeader["Curr_id"] = _ITModel.Curr_id;
                dtrowHeader["TradeTerms"] = _ITModel.Trade_Term;
                dtrowHeader["CountryofOrigin"] = _ITModel.CntryOrigin;
                dtrowHeader["PortofOrigin"] = _ITModel.PortOrigin;
                dtrowHeader["Remarks"] = _ITModel.Remarks;
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                //dtrowHeader["op_status"] = "D";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
               

                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;

                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("SNo", typeof(int));
                dtItem.Columns.Add("Date", typeof(string));
                dtItem.Columns.Add("DocNo", typeof(string));
                dtItem.Columns.Add("DocDate", typeof(string));
                dtItem.Columns.Add("Status", typeof(string));
                dtItem.Columns.Add("EntryFlag", typeof(string));
                

                JArray jObject = JArray.Parse(_ITModel.NewDocumentDetail);

                for (int i = 0; i < jObject.Count; i++)
                {
                
                   DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["SNo"] = jObject[i]["SRno"].ToString();
                    dtrowLines["Date"] = jObject[i]["Date"].ToString();
                    dtrowLines["DocNo"] = jObject[i]["DocNo"].ToString();
                    dtrowLines["DocDate"] = jObject[i]["DocDate"].ToString();
                    dtrowLines["Status"] = jObject[i]["Status"].ToString();
                    dtrowLines["EntryFlag"] = jObject[i]["Entryflag"].ToString();
                    
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;

                DataTable dtAttachment = new DataTable();
                var _ImpTrackattch = TempData["ModelDataattch"] as ImpTrackModelattch;
                TempData["ModelDataattch"] = null;
                if (_ITModel.attatchmentdetail != null)
                {
                    if (_ImpTrackattch != null)
                    {
                        if (_ImpTrackattch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _ImpTrackattch.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    else
                    {
                        if (_ITModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _ITModel.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    JArray jObject1 = JArray.Parse(_ITModel.attatchmentdetail);
                    for (int i = 0; i < jObject1.Count; i++)
                    {
                        string flag = "Y";
                        foreach (DataRow dr in dtAttachment.Rows)
                        {
                            string drImg = dr["file_name"].ToString();
                            string ObjImg = jObject1[i]["file_name"].ToString();
                            if (drImg == ObjImg)
                            {
                                flag = "N";
                            }
                        }
                        if (flag == "Y")
                        {

                            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                            if (!string.IsNullOrEmpty(_ITModel.DocNo))
                            {
                                dtrowAttachment1["id"] = _ITModel.DocNo;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = CompID;
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_ITModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string SQTCode = string.Empty;
                            if (!string.IsNullOrEmpty(_ITModel.DocNo))
                            {
                                SQTCode = _ITModel.DocNo;
                            }
                            else
                            {
                                SQTCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + SQTCode.Replace("/", "") + "*");

                            foreach (var fielpath in filePaths)
                            {
                                string flag = "Y";
                                foreach (DataRow dr in dtAttachment.Rows)
                                {
                                    string drImgPath = dr["file_path"].ToString();
                                    if (drImgPath == fielpath.Replace("/", @"\"))
                                    {
                                        flag = "N";
                                    }
                                }
                                if (flag == "Y")
                                {
                                    System.IO.File.Delete(fielpath);
                                }
                            }
                        }
                    }
                    DtblAttchDetail = dtAttachment;
                }

                SaveMessage = _ImportTracking_ISERVICES.InsertImportTrackingDetails(DtblHDetail, DtblItemDetail);
                string[] FDetail = SaveMessage.Split(',');
                string Message = FDetail[0].ToString();
                string po_no = FDetail[1].ToString();
                string po_dt = FDetail[2].ToString();
                /*-----------------Attachment Section Start------------------------*/
                if (Message == "Save")
                {
                    string Guid = "";
                    if (_ImpTrackattch != null)
                    {
                        //if (Session["Guid"] != null)
                        if (_ImpTrackattch.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ImpTrackattch.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, BrchID, guid, PageName, po_no, _ITModel.TransType, DtblAttchDetail);


                }
                /*-----------------Attachment Section End------------------------*/


                if (Message == "Update" || Message == "Save")
                {
                    _ITModel.Message = "Save";
                    _ITModel.Command = "Update";
                    //_ITModel.SrcDocDate = po_dt;
                    _ITModel.SrcDocDate = Convert.ToDateTime(po_dt).ToString("yyyy-MM-dd");
                    _ITModel.SrcDocNo = po_no;
                    _ITModel.TransType = "Update";
                    //_ITModel.AppStatus = "D";
                    _ITModel.BtnName = "BtnSave";
                }
                return RedirectToAction("ImportTracking");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ITModel.TransType == "Update")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ITModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ITModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public ActionResult SearchImpTrckAutoAndNewDetail(string SuppId, string SrcDocNo)
        {
            try
            {
                ImportTracking_Model _ITModel = new ImportTracking_Model(); 
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                
                DataSet dt1 = _ImportTracking_ISERVICES.GetAllDocImpTrackList(CompID, BrchID, SuppId, SrcDocNo, UserID);
                ViewBag.DocumentDetails = dt1.Tables[0];
                _ITModel.ImpTrckSearch = "ImpTrck_Search";
                _ITModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;
                //ViewBag.MenuPageName = getDocumentName();
                CommonPageDetails();
               return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialImportTrackingList.cshtml", _ITModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void GetCompDetail()
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
        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                ImpTrackModelattch _ImpTrackModelattch = new ImpTrackModelattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string SQ_No = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["SQ_No"] != null)
                //{
                //    SQ_No = Session["SQ_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _ImpTrackModelattch.Guid = DocNo;
                GetCompDetail();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _ImpTrackModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _ImpTrackModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _ImpTrackModelattch;
                //return Json("Uploaded " + Request.Files.Count + " files");
                return Json(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }

        public ActionResult GetAttachmentDetail(string AttachIdSRNoTBlDt, string AttachmentListwithPageData,string IsDisabled,string Attachflag,string Editflag)
        {
            try
            {
                GetCompDetail();
                
                DataTable dttempAttachment = new DataTable();
                DataTable dtattach = new DataTable();
                dtattach.Columns.Add("id", typeof(string));
                dtattach.Columns.Add("file_name", typeof(string));
                dtattach.Columns.Add("file_path", typeof(string));
                dtattach.Columns.Add("file_def", typeof(char));
                dtattach.Columns.Add("comp_id", typeof(Int32));
                

                    ViewBag.DocumentMenuId = DocumentMenuId;

                if (AttachmentListwithPageData != null)
                {
                    JArray jObject = JArray.Parse(AttachmentListwithPageData);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtattach.NewRow();
                        dtrowLines["id"] = jObject[i]["id"].ToString();
                        dtrowLines["file_name"] = jObject[i]["file_name"].ToString();
                        dtrowLines["file_path"] = jObject[i]["file_path"].ToString();
                        dtrowLines["file_def"] = 'Y';

                        dtrowLines["comp_id"] = CompID;


                        dtattach.Rows.Add(dtrowLines);
                    }
                }

                dttempAttachment = dtattach;

            ViewBag.AttechmentDetails = dttempAttachment;
                //
                //ViewBag.Disable = IsDisabled;
                if (Attachflag == "New" && Editflag == "")
                {
                    ViewBag.Disable = false;
                    ViewBag.AttechmentDetails = null;

                }
                else if (Attachflag == "New" && Editflag == "Yes")
                {
                    ViewBag.Disable = false;
                }
                else
                {
                    ViewBag.Disable = true;
                }

                ViewBag.FlagHideSaveCloseBtn = Attachflag;
                //ViewBag.IsDisabled = IsDisabled;
                
                //if (Attachflag == "New" || Attachflag == "View")
                //{
                    
                //}
                if (Attachflag == "New" && Editflag == "Yes")
                {
                    //Partial("~/Areas/Common/Views/cmn_imagebind.cshtml")
                    return PartialView("~/Areas/Common/Views/cmn_imagebind.cshtml");
                }
                return PartialView("~/Areas/Common/Views/Comn_PartialAttatchmentDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /*--------------------------For Attatchment End--------------------------*/
    }

}