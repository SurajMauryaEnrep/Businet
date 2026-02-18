using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialTransferOrder;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialTransferOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialTransferOrder
{
    public class MaterialTransferOrderController : Controller
    {
        string FromDate, Br_ID, title;
        List<MTOList> _MTOList;
        string DocumentMenuId = "105102140";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, BrchID, BranchName, UserID, userid, language = String.Empty;
        Common_IServices _Common_IServices;
        MTO_ISERVICES _MTO_ISERVICES;
        MTOList_ISERVICES _MTOList_ISERVICES;

        DataTable dt;
        DataSet dtSet;
        MTOModel _MTOModel;
        public MaterialTransferOrderController(Common_IServices _Common_IServices, MTO_ISERVICES _MTO_ISERVICES, MTOList_ISERVICES _MTOList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._MTO_ISERVICES = _MTO_ISERVICES;
            this._MTOList_ISERVICES = _MTOList_ISERVICES;
        }
        // GET: ApplicationLayer/MaterialTransferOrder
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
            DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, userid, DocumentMenuId, language);
            ViewBag.AppLevel = ds.Tables[0];
            ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
            string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
            ViewBag.VBRoleList = ds.Tables[3];
            ViewBag.StatusList = ds.Tables[4];
            //ViewBag.PackSerialization = ds.Tables[6].Rows.Count > 0 ? ds.Tables[6].Rows[0]["param_stat"].ToString() : "";
            string[] Docpart = DocumentName.Split('>');
            int len = Docpart.Length;
            if (len > 1)
            {
                title = Docpart[len - 1].Trim();
            }
            ViewBag.MenuPageName = DocumentName;
        }
        public ActionResult MaterialTransferOrderList(MTOListModel _MTOListModel)
        {
            ViewBag.DocumentMenuId = DocumentMenuId;
            _MTOListModel.DocumentMenuId = DocumentMenuId;
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
                UserID = Session["userid"].ToString();
            }
            string wfstatus = "";
            if (_MTOListModel.WF_status != null)
            {
                wfstatus = _MTOListModel.WF_status;
            }
            else
            {
                wfstatus = "";
            }
            //var other = new CommonController(_Common_IServices);
            //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
            //GetStatusList(_MTOListModel);
            CommonPageDetails();
            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _MTOListModel.StatusList = statusLists;
            _MTOListModel.Title = title;


            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;

            var flag = "ListPage";
            var PageName = "MTRequisnOrOrder";
            dtSet = MTO_GetAllDDLListAndListPageData(flag, PageName, UserID, _MTOListModel.WF_status, DocumentMenuId, startDate, CurrentDate);
            List<ToBranchList> _ToBranchList = new List<ToBranchList>();
            foreach (DataRow dr in dtSet.Tables[1].Rows)
            {
                ToBranchList _ToBranch = new ToBranchList();
                _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                _ToBranch.br_val = dr["comp_nm"].ToString();
                _ToBranchList.Add(_ToBranch);
            }
            _ToBranchList.Insert(0, new ToBranchList() { br_id = 0, br_val = "---Select---" });/*only Add this line by Hina on 12-09-2024*/

            _MTOListModel.ToBranchList = _ToBranchList;

            List<ToWharehouseList> _ToWharehouseList = new List<ToWharehouseList>();
            foreach (DataRow dr in dtSet.Tables[0].Rows)
            {
                ToWharehouseList _ToWharehouse = new ToWharehouseList();
                _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                _ToWharehouse.wh_val = dr["wh_name"].ToString();
                _ToWharehouseList.Add(_ToWharehouse);
            }
            _ToWharehouseList.Insert(0, new ToWharehouseList() { wh_id = 0, wh_val = "---Select---" });/*only Add this line by Hina on 12-09-2024*/
            _MTOListModel.ToWharehouseList = _ToWharehouseList;
            //string endDate = dtnow.ToString("yyyy-MM-dd");
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var PRData = TempData["ListFilterData"].ToString();
                var a = PRData.Split(',');
                _MTOListModel.to_wh =(a[0].Trim());
                _MTOListModel.to_br =(a[1].Trim());
                _MTOListModel.MTO_FromDate = a[2].Trim();
                _MTOListModel.MTO_ToDate = a[3].Trim();
                _MTOListModel.TRF_Type = a[4].Trim();
                _MTOListModel.TRFStatus = a[5].Trim();
                if (_MTOListModel.TRFStatus == "0")
                {
                    _MTOListModel.TRFStatus = null;
                }    
                _MTOListModel.FromDate = _MTOListModel.MTO_FromDate;
                //_MTOListModel.BindMTOList = GetMTODetailList(_MTOListModel);
                _MTOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                if (_MTOListModel.BindMTOList != null)
                {
                    _MTOListModel.WF_status = null;
                }
            }
            else
            {
                _MTOListModel.FromDate = startDate;
                _MTOListModel.MTO_FromDate = startDate;
                _MTOListModel.MTO_ToDate = CurrentDate;
                //_MTOListModel.BindMTOList = GetMTODetailList(_MTOListModel);
                //_MTOList = new List<MTOList>();
                //if (dtSet.Tables[2].Rows.Count > 0)
                //{

                //    foreach (DataRow dr in dtSet.Tables[2].Rows)
                //    {
                //        MTOList _TempMTOList = new MTOList();
                //        _TempMTOList.TRFNo = dr["trf_no"].ToString();
                //        _TempMTOList.TRFDate = dr["TrfDate"].ToString();
                //        _TempMTOList.trf_date = dr["trf_date"].ToString();
                //        _TempMTOList.TRFType = dr["TRFType"].ToString();
                //        _TempMTOList.FromBranch = dr["frombr"].ToString();
                //        _TempMTOList.FromWH = dr["fromwh"].ToString();
                //        _TempMTOList.ToBranch = dr["tobr"].ToString();
                //        _TempMTOList.ToWH = dr["towh"].ToString();
                //        _TempMTOList.TRFList_Stauts = dr["Status"].ToString();
                //        _TempMTOList.CreateDate = dr["CreateDate"].ToString();
                //        _TempMTOList.ApproveDate = dr["ApproveDate"].ToString();
                //        _TempMTOList.ModifyDate = dr["ModifyDate"].ToString();
                //        _TempMTOList.create_by = dr["create_by"].ToString();
                //        _TempMTOList.app_by = dr["app_by"].ToString();
                //        _TempMTOList.mod_by = dr["mod_by"].ToString();
                //        _MTOList.Add(_TempMTOList);
                //    }
                //}
                //_MTOListModel.BindMTOList = _MTOList;
            }
            _MTOListModel.BindMTOList = GetMTODetailList(_MTOListModel);
            /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/

            //List<ToBranchList> _ToBranchList = new List<ToBranchList>();
            //dt = GetFromBranchList();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    ToBranchList _ToBranch = new ToBranchList();
            //    _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
            //    _ToBranch.br_val = dr["comp_nm"].ToString();
            //    _ToBranchList.Add(_ToBranch);
            //}
            //_MTOListModel.ToBranchList = _ToBranchList;

            //List<ToWharehouseList> _ToWharehouseList = new List<ToWharehouseList>();

            //BrchID = Session["BranchId"].ToString();
            //dt = GetToWHList(BrchID);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    ToWharehouseList _ToWharehouse = new ToWharehouseList();
            //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
            //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
            //    _ToWharehouseList.Add(_ToWharehouse);
            //}
            //_MTOListModel.ToWharehouseList = _ToWharehouseList;



            //ViewBag.MenuPageName = getDocumentName();
            _MTOListModel.Title = title;
            //Session["MTOSearch"] = "0";
            _MTOListModel.MTOSearch = "0";
            //ViewBag.VBRoleList = GetRoleList();
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialTransferOrder/MaterialTransferOrderList.cshtml", _MTOListModel);
        }
        public ActionResult MaterialTransferOrder(UrlModel _urlModel)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();

            /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.TRFDate) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            var ModelData =  TempData["ModelData"] as MTOModel;
            
            if (ModelData != null)
            {
                ViewBag.DocumentMenuId = DocumentMenuId;
               // MTOModel ModelData = new MTOModel();
                ModelData.trf_dt = DateTime.Now;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["BranchName"] != null)
                {
                    BranchName = Session["BranchName"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                CommonPageDetails();
                string wfstatus = "";
                if (ModelData.WF_status1 != null)
                {
                    wfstatus = ModelData.WF_status1;
                }
                else
                {
                    wfstatus = "";
                }
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ModelData.to_br = BranchName;
                /*commented by Hina on 18-03-2024 to combine all  Procedure  in single Procedure*/
                //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                //dt = GetFromWHList();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    FromWharehouse _warehouse = new FromWharehouse();
                //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _warehouse.wh_val = dr["wh_name"].ToString();
                //    _warehouseList.Add(_warehouse);
                //}
                //ModelData.FromWharehouseList = _warehouseList;

                //List<FromBranch> _FromBranchList = new List<FromBranch>();
                //dt = GetFromBranchList();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    FromBranch _FromBranch = new FromBranch();
                //    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                //    _FromBranch.br_val = dr["comp_nm"].ToString();
                //    _FromBranchList.Add(_FromBranch);
                //}
                //ModelData.FromBranchList = _FromBranchList;

                //List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                //dt = GetToWHList(BrchID);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    ToWharehouse _ToWharehouse = new ToWharehouse();
                //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                //    _ToWharehouseList.Add(_ToWharehouse);
                //}
                //ModelData.ToWharehouseList = _ToWharehouseList;
                //ModelData.to_brid = BrchID;

                dtSet = MTO_GetAllDDLListAndListPageData("", "", "", "", "","","");
                //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();

                //foreach (DataRow dr in dtSet.Tables[0].Rows)
                //{
                //    FromWharehouse _warehouse = new FromWharehouse();
                //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _warehouse.wh_val = dr["wh_name"].ToString();
                //    _warehouseList.Add(_warehouse);
                //}
                //ModelData.FromWharehouseList = _warehouseList;
                List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                _warehouseList.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });
                ModelData.FromWharehouseList = _warehouseList;

                List<FromBranch> _FromBranchList = new List<FromBranch>();
                foreach (DataRow dr in dtSet.Tables[1].Rows)
                {
                    FromBranch _FromBranch = new FromBranch();
                    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    _FromBranch.br_val = dr["comp_nm"].ToString();
                    _FromBranchList.Add(_FromBranch);
                }
                _FromBranchList.Insert(0, new FromBranch() { br_id = 0, br_val = "---Select---" });
                ModelData.FromBranchList = _FromBranchList;

                List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
                //foreach (DataRow dr in dtSet.Tables[0].Rows)
                //{
                //    ToWharehouse _ToWharehouse = new ToWharehouse();
                //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                //    _ToWharehouseList.Add(_ToWharehouse);
                //}
                ModelData.ToWharehouseList = _ToWharehouseList;      
                ModelData.to_brid = BrchID;

            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    ModelData.ListFilterData1 = TempData["ListFilterData"].ToString();
                }

                //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                if (ModelData.TransType == "Update" || ModelData.TransType == "Edit")
                {
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    BrchID = Session["BranchId"].ToString();
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ModelData.RateDigit = RateDigit;
                    ModelData.ValDigit = ValDigit;
                    ModelData.QtyDigit = QtyDigit;
                    //string trf_no = Session["TRFNo"].ToString();
                    string trf_no = ModelData.TRFNo;
                    DataSet ds = _MTO_ISERVICES.GetMTODetail(CompID, trf_no, BrchID, UserID, DocumentMenuId);
                    ModelData.trf_no = ds.Tables[0].Rows[0]["trf_no"].ToString();
                    ModelData.trf_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["trf_dt"].ToString());
                    ModelData.trf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                    ModelData.from_br = Convert.ToInt32(ds.Tables[0].Rows[0]["from_br"].ToString());
                    ModelData.to_brid = ds.Tables[0].Rows[0]["to_br"].ToString();
                    ModelData.from_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["from_wh"].ToString());
                    ModelData.to_br = ds.Tables[0].Rows[0]["tobr"].ToString();
                    ModelData.to_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["to_wh"].ToString());
                    //ModelData.to_WhName = ds.Tables[0].Rows[0]["to_whname"].ToString();
                    ModelData.trf_rem = ds.Tables[0].Rows[0]["trf_rem"].ToString();
                    ModelData.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                    ModelData.Createid = ds.Tables[0].Rows[0]["creater_id"].ToString();
                    ModelData.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    ModelData.create_dt = ds.Tables[0].Rows[0]["create_dt1"].ToString();
                    ModelData.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                    ModelData.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                    ModelData.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                    ModelData.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    ModelData.trf_status = ds.Tables[0].Rows[0]["app_status"].ToString();

                    string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                    string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                    string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                    ModelData.StatusCode = doc_status;

                    List<FromWharehouse> _warehouseList1 = new List<FromWharehouse>();
                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        FromWharehouse _warehouse = new FromWharehouse();
                        _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _warehouse.wh_val = dr["wh_name"].ToString();
                        _warehouseList1.Add(_warehouse);
                    }
                    //_warehouseList1.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });/*Add by Hina on 12-09-2024*/
                    ModelData.FromWharehouseList = _warehouseList1;

                    List<ToWharehouse> _ToWharehouseList1 = new List<ToWharehouse>();
                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        ToWharehouse _ToWharehouse = new ToWharehouse();
                        _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _ToWharehouse.wh_val = dr["wh_name"].ToString();
                        _ToWharehouseList1.Add(_ToWharehouse);
                    }
                    //_ToWharehouseList1.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });/*Add by Hina on 12-09-2024*/
                    ModelData.ToWharehouseList = _ToWharehouseList1;

                    if (ModelData.StatusCode == "C")
                    {
                        ModelData.CancelFlag = true;
                        //Session["BtnName"] = "Refresh";
                        ModelData.BtnName = "Refresh";
                    }
                    else
                    {
                        ModelData.CancelFlag = false;
                    }
                    if (ModelData.StatusCode == "FC")
                    {
                        ModelData.ForceClose = true;
                        //Session["BtnName"] = "Refresh";
                        ModelData.BtnName = "Refresh";
                    }
                    else
                    {
                        ModelData.ForceClose = false;
                    }
                    ModelData.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                    ModelData.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                    //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                    if (doc_status != "D" && doc_status != "F")
                    {
                        ViewBag.AppLevel = ds.Tables[4];
                    }

                    //if (ViewBag.AppLevel.Rows.Count > 0 && Session["Command"].ToString() != "Edit")
                    if (ViewBag.AppLevel.Rows.Count > 0 && ModelData.Command != "Edit")
                    {

                        var sent_to = "";
                        var nextLevel = "";
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                        }

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                        }

                        if (doc_status == "D")
                        {
                            if (create_id != UserID)
                            {
                                //Session["BtnName"] = "Refresh";
                                ModelData.BtnName = "Refresh";
                            }
                            else
                            {
                                if (nextLevel == "0")
                                {
                                    if (create_id == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ModelData.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    ViewBag.Approve = "N";
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ModelData.BtnName = "BtnToDetailPage";
                                }

                            }
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                //Session["BtnName"] = "BtnToDetailPage";
                                ModelData.BtnName = "BtnToDetailPage";
                            }


                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ModelData.BtnName = "BtnToDetailPage";
                                }


                            }
                        }
                        if (doc_status == "F")
                        {
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                //Session["BtnName"] = "BtnToDetailPage";
                                ModelData.BtnName = "BtnToDetailPage";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                }
                                //Session["BtnName"] = "BtnToDetailPage";
                                ModelData.BtnName = "BtnToDetailPage";
                            }
                        }
                        if (doc_status == "A")
                        {
                            if (create_id == UserID || approval_id == UserID)
                            {
                                //Session["BtnName"] = "BtnToDetailPage";
                                ModelData.BtnName = "BtnToDetailPage";

                            }
                            else
                            {
                                //Session["BtnName"] = "Refresh";
                                ModelData.BtnName = "Refresh";
                            }
                        }
                    }

                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }

                    //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                    ModelData.DocumentStatus = ds.Tables[0].Rows[0]["status_code"].ToString();
                   //ViewBag.MenuPageName = getDocumentName();
                    ModelData.Title = title;
                    ViewBag.ItemDetails = ds.Tables[1];
                    ViewBag.SubItemDetails = ds.Tables[5];
                    //ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString();
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialTransferOrder/MaterialTransferOrderDetail.cshtml", ModelData);
                }
                else
                {
                    //ViewBag.MenuPageName = getDocumentName();
                    ModelData.Title = title;
                    //Session["DocumentStatus"] = "D";
                    ModelData.DocumentStatus = "D";
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialTransferOrder/MaterialTransferOrderDetail.cshtml", ModelData);
                }
            }
            else
            {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                CommonPageDetails();
                //var commCont = new CommonController(_Common_IServices);
                //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                //{
                //    TempData["Message1"] = "Financial Year not Exist";
                //}
                /*End to chk Financial year exist or not*/
                MTOModel _MTOModel = new MTOModel();
                if (_urlModel.TRFNo != null && _urlModel.TRFNo != "")
                {
                    _MTOModel.TRFNo = _urlModel.TRFNo;
                }
                if (_urlModel.TransType != null)
                {
                    _MTOModel.TransType = _urlModel.TransType;
                }
                else
                {
                    _MTOModel.TransType = "New";
                }
                if (_urlModel.BtnName != null)
                {
                    _MTOModel.BtnName = _urlModel.BtnName;
                }
                else
                {
                    _MTOModel.BtnName = "BtnRefresh";
                }
                if (_urlModel.Command != null)
                {
                    _MTOModel.Command = _urlModel.Command;
                }
                else
                {
                    _MTOModel.Command = "Refresh";
                }
                if (_urlModel.WF_status1 != null)
                {
                    _MTOModel.WF_status1 = _urlModel.WF_status1;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
               
                _MTOModel.trf_dt = DateTime.Now;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["BranchName"] != null)
                {
                    BranchName = Session["BranchName"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                _MTOModel.to_br = BranchName;
                /*commented by Hina on 18-03-2024 to combine all  Procedure  in single Procedure*/
                //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                //dt = GetFromWHList();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    FromWharehouse _warehouse = new FromWharehouse();
                //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _warehouse.wh_val = dr["wh_name"].ToString();
                //    _warehouseList.Add(_warehouse);
                //}
                //_MTOModel.FromWharehouseList = _warehouseList;

                //List<FromBranch> _FromBranchList = new List<FromBranch>();
                //dt = GetFromBranchList();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    FromBranch _FromBranch = new FromBranch();
                //    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                //    _FromBranch.br_val = dr["comp_nm"].ToString();
                //    _FromBranchList.Add(_FromBranch);
                //}
                //_MTOModel.FromBranchList = _FromBranchList;

                //List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                //dt = GetToWHList(BrchID);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    ToWharehouse _ToWharehouse = new ToWharehouse();
                //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                //    _ToWharehouseList.Add(_ToWharehouse);
                //}
                //_MTOModel.ToWharehouseList = _ToWharehouseList;
                //_MTOModel.to_brid = BrchID;

                dtSet = MTO_GetAllDDLListAndListPageData("", "","", "","","","");
                //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();

                //foreach (DataRow dr in dtSet.Tables[0].Rows)
                //{
                //    FromWharehouse _warehouse = new FromWharehouse();
                //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _warehouse.wh_val = dr["wh_name"].ToString();
                //    _warehouseList.Add(_warehouse);
                //}
                //_MTOModel.FromWharehouseList = _warehouseList;

                List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                _warehouseList.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });
                _MTOModel.FromWharehouseList = _warehouseList;
                
                List<FromBranch> _FromBranchList = new List<FromBranch>();
                _FromBranchList.Insert(0, new FromBranch() { br_id = 0, br_val = "---Select---" });
                foreach (DataRow dr in dtSet.Tables[1].Rows)
                {
                    FromBranch _FromBranch = new FromBranch();
                    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    _FromBranch.br_val = dr["comp_nm"].ToString();
                    _FromBranchList.Add(_FromBranch);
                }
                
                _MTOModel.FromBranchList = _FromBranchList;

                List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
                //foreach (DataRow dr in dtSet.Tables[0].Rows)
                //{
                //    ToWharehouse _ToWharehouse = new ToWharehouse();
                //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                //    _ToWharehouseList.Add(_ToWharehouse);
                //}
                _MTOModel.ToWharehouseList = _ToWharehouseList;
                _MTOModel.to_brid = BrchID;
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                ViewBag.DocumentMenuId = DocumentMenuId;
                _MTOModel.RateDigit = RateDigit;
                _MTOModel.ValDigit = ValDigit;
                _MTOModel.QtyDigit = QtyDigit;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    _MTOModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                }

                //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                if (_MTOModel.TransType == "Update" || _MTOModel.TransType == "Edit")
                {
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    BrchID = Session["BranchId"].ToString();
                    //string trf_no = Session["TRFNo"].ToString();
                    string trf_no = _MTOModel.TRFNo;
                    DataSet ds = _MTO_ISERVICES.GetMTODetail(CompID, trf_no, BrchID, UserID, DocumentMenuId);
                    _MTOModel.trf_no = ds.Tables[0].Rows[0]["trf_no"].ToString();
                    _MTOModel.trf_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["trf_dt"].ToString());
                    _MTOModel.trf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                    _MTOModel.from_br = Convert.ToInt32(ds.Tables[0].Rows[0]["from_br"].ToString());
                    _MTOModel.to_brid = ds.Tables[0].Rows[0]["to_br"].ToString();
                    _MTOModel.from_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["from_wh"].ToString());
                    _MTOModel.to_br = ds.Tables[0].Rows[0]["tobr"].ToString();
                    _MTOModel.to_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["to_wh"].ToString());
                    //_MTOModel.to_WhName = ds.Tables[0].Rows[0]["to_whname"].ToString();
                    _MTOModel.trf_rem = ds.Tables[0].Rows[0]["trf_rem"].ToString();
                    _MTOModel.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                    _MTOModel.Createid = ds.Tables[0].Rows[0]["creater_id"].ToString();
                    _MTOModel.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    _MTOModel.create_dt = ds.Tables[0].Rows[0]["create_dt1"].ToString();
                    _MTOModel.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                    _MTOModel.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                    _MTOModel.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                    _MTOModel.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    _MTOModel.trf_status = ds.Tables[0].Rows[0]["app_status"].ToString();

                    string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                    string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                    string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                    _MTOModel.StatusCode = doc_status;
                    List<FromWharehouse> _warehouseList2 = new List<FromWharehouse>();

                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        FromWharehouse _warehouse = new FromWharehouse();
                        _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _warehouse.wh_val = dr["wh_name"].ToString();
                        _warehouseList2.Add(_warehouse);
                    }
                    _MTOModel.FromWharehouseList = _warehouseList2;

                    List<ToWharehouse> _ToWharehouseList1 = new List<ToWharehouse>();
      
                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        ToWharehouse _ToWharehouse = new ToWharehouse();
                        _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _ToWharehouse.wh_val = dr["wh_name"].ToString();
                        _ToWharehouseList1.Add(_ToWharehouse);
                    }
                    _MTOModel.ToWharehouseList = _ToWharehouseList1;
                    _MTOModel.to_brid = BrchID;

                    if (_MTOModel.StatusCode == "C")
                    {
                        _MTOModel.CancelFlag = true;
                        //Session["BtnName"] = "Refresh";
                        _MTOModel.BtnName = "Refresh";
                    }
                    else
                    {
                        _MTOModel.CancelFlag = false;
                    }
                    if (_MTOModel.StatusCode == "FC")
                    {
                        _MTOModel.ForceClose = true;
                        //Session["BtnName"] = "Refresh";
                        _MTOModel.BtnName = "Refresh";
                    }
                    else
                    {
                        _MTOModel.ForceClose = false;
                    }
                    _MTOModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                    _MTOModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                    //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                    if (doc_status != "D" && doc_status != "F")
                    {
                        ViewBag.AppLevel = ds.Tables[4];
                    }

                    //if (ViewBag.AppLevel.Rows.Count > 0 && Session["Command"].ToString() != "Edit")
                    if (ViewBag.AppLevel.Rows.Count > 0 && _MTOModel.Command != "Edit")
                    {

                        var sent_to = "";
                        var nextLevel = "";
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                        }

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                        }

                        if (doc_status == "D")
                        {
                            if (create_id != UserID)
                            {
                                //Session["BtnName"] = "Refresh";
                                _MTOModel.BtnName = "Refresh";
                            }
                            else
                            {
                                if (nextLevel == "0")
                                {
                                    if (create_id == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _MTOModel.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    ViewBag.Approve = "N";
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _MTOModel.BtnName = "BtnToDetailPage";
                                }

                            }
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                //Session["BtnName"] = "BtnToDetailPage";
                                _MTOModel.BtnName = "BtnToDetailPage";
                            }


                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _MTOModel.BtnName = "BtnToDetailPage";
                                }


                            }
                        }
                        if (doc_status == "F")
                        {
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                //Session["BtnName"] = "BtnToDetailPage";
                                _MTOModel.BtnName = "BtnToDetailPage";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                }
                                //Session["BtnName"] = "BtnToDetailPage";
                                _MTOModel.BtnName = "BtnToDetailPage";
                            }
                        }
                        if (doc_status == "A")
                        {
                            if (create_id == UserID || approval_id == UserID)
                            {
                                //Session["BtnName"] = "BtnToDetailPage";
                                _MTOModel.BtnName = "BtnToDetailPage";

                            }
                            else
                            {
                                //Session["BtnName"] = "Refresh";
                                _MTOModel.BtnName = "Refresh";
                            }
                        }
                    }

                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }

                    //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                    _MTOModel.DocumentStatus = ds.Tables[0].Rows[0]["status_code"].ToString();
                    
                    //ViewBag.MenuPageName = getDocumentName();
                    _MTOModel.Title = title;
                    ViewBag.ItemDetails = ds.Tables[1];
                    ViewBag.SubItemDetails = ds.Tables[5];
                    //ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString();
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialTransferOrder/MaterialTransferOrderDetail.cshtml", _MTOModel);
                }
                else
                {
                    
                    //ViewBag.MenuPageName = getDocumentName();
                    _MTOModel.Title = title;
                    //Session["DocumentStatus"] = "D";
                    _MTOModel.DocumentStatus = "D";
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialTransferOrder/MaterialTransferOrderDetail.cshtml", _MTOModel);
                }
            }
           
        
        }
        public ActionResult ToRefreshByJS(string ListFilterData1,string DashBord)
        {
            MTOModel _MTOModel = new MTOModel();
            //Session["Message"] = "";
            _MTOModel.Message = "";
            var a = DashBord.Split(',');
            _MTOModel.docid = a[0].Trim();
            _MTOModel.TRFNo = a[1].Trim();
            _MTOModel.TRFDate = a[2].Trim();
            _MTOModel.WF_status1 = a[3].Trim();
            _MTOModel.TransType = "Update";
            _MTOModel.BtnName = "BtnToDetailPage";
            _MTOModel.Message = null;
            TempData["ModelData"] = _MTOModel;
            UrlModel _UrlModel = new UrlModel();
            _UrlModel.TRFNo = _MTOModel.TRFNo;
            //_UrlModel.docid =  _mrsmodel.docid;
            _UrlModel.WF_status1 = _MTOModel.WF_status1;
            _UrlModel.MRSDate =Convert.ToDateTime(_MTOModel.TRFDate);
            _UrlModel.TransType = "Update";
            _UrlModel.BtnName = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("MaterialTransferOrder", _UrlModel);
        }
        public ActionResult GetMaterialTransferOrderList(string docid, string status)
        {
            MTOListModel _MTOListModel = new MTOListModel();
            //Session["WF_status"] = status;
            _MTOListModel.WF_status = status;
            return RedirectToAction("MaterialTransferOrderList", _MTOListModel);
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1,string docid,string WF_Status1)
        {

            MTOModel _MTOModel = new MTOModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _MTOModel.trf_no = jObjectBatch[i]["trf_no"].ToString();
                    _MTOModel.trf_dt = Convert.ToDateTime(jObjectBatch[i]["create_dt"].ToString());
                    _MTOModel.trf_type = jObjectBatch[i]["trf_type"].ToString();
                    _MTOModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _MTOModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _MTOModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_MTOModel.A_Status != "Approve")
            {
                _MTOModel.A_Status = "Approve";
            }
            string command = "";
            MTOApprove(_MTOModel, command, ListFilterData1, docid, WF_Status1);
            _MTOModel.Message = "Approved";
            _MTOModel.Command = command;
            _MTOModel.TRFNo = _MTOModel.trf_no;
            _MTOModel.TransType = "Update";
            _MTOModel.AppStatus = "A";
            _MTOModel.WF_status1 = WF_Status1;
            _MTOModel.BtnName = "BtnApprove";
            TempData["ModelData"] = _MTOModel;
            UrlModel _urlModelApprove = new UrlModel();
            _urlModelApprove.Command = _MTOModel.Command;
            _urlModelApprove.TransType = _MTOModel.TransType;
            // _urlModelApprove.AppStatus = "A";
            _urlModelApprove.BtnName = _MTOModel.BtnName;
            _urlModelApprove.TRFNo = _MTOModel.trf_no;
            _urlModelApprove.WF_status1 = _MTOModel.WF_status1;
            return RedirectToAction("MaterialTransferOrder", _urlModelApprove);
        }
        private DataTable GetRoleList()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult EditMTO(string TRFNo, string TRFDt, string ListFilterData,string WF_status)
        {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
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
            MTOModel _MTOModel = new MTOModel();
            _MTOModel.Message = "New";
            _MTOModel.Command = "Add";
            _MTOModel.AppStatus = "D";
            _MTOModel.TransType = "Update";
            _MTOModel.BtnName = "BtnToDetailPage";
            _MTOModel.TRFNo = TRFNo;
            _MTOModel.TRFDate = TRFDt;
            _MTOModel.WF_status1 = WF_status;
            TempData["ModelData"] = _MTOModel;
            UrlModel _urlModel = new UrlModel();
            _urlModel.TransType = "Update";
            _urlModel.BtnName = "BtnToDetailPage";
            _urlModel.TRFNo = TRFNo;
            _urlModel.TRFDate = TRFDt;
            _urlModel.WF_status1 = WF_status;
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["TRFNo"] = TRFNo;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("MaterialTransferOrder", "MaterialTransferOrder", _urlModel);
        }
        public ActionResult AddMaterialTransferOrderDetail()
        { /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("MaterialTransferOrderList");
            }
            /*End to chk Financial year exist or not*/
            MTOModel _MTOModel = new MTOModel();
            _MTOModel.Message = "New";
            _MTOModel.Command = "Add";
            _MTOModel.AppStatus = "D";
            _MTOModel.TransType = "Save";
            _MTOModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _MTOModel;
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            TempData["ListFilterData"] = null;
            return RedirectToAction("MaterialTransferOrder", "MaterialTransferOrder");           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MaterialTransferRequisitionSave(MTOModel _MTOModel, string command)
        {
            try
            {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                string trf_no = string.Empty;
                if (_MTOModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        MTOModel _MTOModeladdnew = new MTOModel();
                        _MTOModeladdnew.Command = "Add";
                        _MTOModeladdnew.TransType = "Save";
                        _MTOModeladdnew.AppStatus = "D";
                        _MTOModeladdnew.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _MTOModeladdnew;
                        UrlModel _urlModeladd = new UrlModel();
                        _urlModeladd.TransType = "Save";
                        _urlModeladd.BtnName = "BtnAddNew";
                        _urlModeladd.Command = "Add";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_MTOModel.trf_no))
                                return RedirectToAction("EditMTO", new { TRFNo = _MTOModel.trf_no, TRFDt = _MTOModel.trf_dt, ListFilterData = _MTOModel.ListFilterData1, WF_status = _MTOModel.WFStatus });
                            else
                                _MTOModeladdnew.Command = "Refresh";
                                _MTOModeladdnew.TransType = "Refresh";
                                _MTOModeladdnew.BtnName = "Refresh";
                                _MTOModeladdnew.DocumentStatus = null;
                                TempData["ModelData"] = _MTOModeladdnew;
                                return RedirectToAction("MaterialTransferOrder", "MaterialTransferOrder");
                        }
                       /*End to chk Financial year exist or not*/

                        return RedirectToAction("MaterialTransferOrder", "MaterialTransferOrder", _urlModeladd);

                    case "Edit":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditMTO", new { TRFNo = _MTOModel.trf_no,TRFDt= _MTOModel.trf_dt, ListFilterData = _MTOModel.ListFilterData1, WF_status = _MTOModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string MtoDt = _MTOModel.trf_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MtoDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditMTO", new { TRFNo = _MTOModel.trf_no, TRFDt= _MTOModel.trf_dt, ListFilterData = _MTOModel.ListFilterData1, WF_status = _MTOModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["BtnName"] = "BtnEdit";
                        //Session["Message"] = null;
                        //Session["TRFNo"] = _MTOModel.trf_no;
                        _MTOModel.Command = command;
                        _MTOModel.TransType = "Update";
                        _MTOModel.AppStatus = "D";
                        _MTOModel.BtnName = "BtnEdit";
                        _MTOModel.TRFNo = _MTOModel.trf_no;
                        _MTOModel.TRFDate = _MTOModel.trf_dt.ToString("yyyy-MM-dd");
                        TempData["ModelData"] = _MTOModel;
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.Command = command;
                        _urlModel.TransType = "Update";
                        _urlModel.AppStatus = "D";
                        _urlModel.BtnName = "BtnEdit";
                        _urlModel.TRFNo = _MTOModel.trf_no;
                        _urlModel.TRFDate = _MTOModel.trf_dt.ToString("yyyy-MM-dd");
                        TempData["ListFilterData"] = _MTOModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferOrder", _urlModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        _MTOModel.Command = command;
                        _MTOModel.BtnName = "Refresh";
                        trf_no = _MTOModel.trf_no;
                        MTODelete(_MTOModel, command);
                        MTOModel _MRSDeleteModel = new MTOModel();
                        _MRSDeleteModel.Message = "Deleted";
                        _MRSDeleteModel.Command = "Refresh";
                        _MRSDeleteModel.TransType = "Refresh";
                        _MRSDeleteModel.AppStatus = "DL";
                        _MRSDeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = _MRSDeleteModel;
                        TempData["ListFilterData"] = _MTOModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferOrder");

                    case "Save":
                        //Session["Command"] = command;
                        _MTOModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            SaveMTODetail(_MTOModel);
                            if (_MTOModel.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            //Session["TRFNo"] = Session["TRFNo"].ToString();
                            TempData["ModelData"] = _MTOModel;
                            UrlModel _urlModelSave = new UrlModel();
                            _urlModelSave.Command = _MTOModel.Command;
                            _urlModelSave.TransType = _MTOModel.TransType;
                            _urlModelSave.AppStatus = "D";
                            _urlModelSave.BtnName = _MTOModel.BtnName;
                            //_urlModelSave.TRFNo = _MTOModel.trf_no;
                            _urlModelSave.TRFNo = _MTOModel.TRFNo;
                            _urlModelSave.TRFDate = _MTOModel.TRFDate;
                            TempData["ListFilterData"] = _MTOModel.ListFilterData1;
                            return RedirectToAction("MaterialTransferOrder", _urlModelSave);

                        }
                        else
                        {
                            TempData["ListFilterData"] = _MTOModel.ListFilterData1;
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialTransferOrder/MaterialTransferOrderDetail.cshtml", _MTOModel);
                        }

                    case "Forward":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditMTO", new { TRFNo = _MTOModel.trf_no,TRFDt= _MTOModel.trf_dt, ListFilterData = _MTOModel.ListFilterData1, WF_status = _MTOModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string MtoDt1 = _MTOModel.trf_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MtoDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditMTO", new { TRFNo = _MTOModel.trf_no, TRFDt = _MTOModel.trf_dt, ListFilterData = _MTOModel.ListFilterData1, WF_status = _MTOModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditMTO", new { TRFNo = _MTOModel.trf_no,TRFDt= _MTOModel.trf_dt, ListFilterData = _MTOModel.ListFilterData1, WF_status = _MTOModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string MtoDt2 = _MTOModel.trf_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MtoDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditMTO", new { TRFNo = _MTOModel.trf_no, TRFDt = _MTOModel.trf_dt, ListFilterData = _MTOModel.ListFilterData1, WF_status = _MTOModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _MTOModel.Command= command; 
                        trf_no = _MTOModel.trf_no;
                        //Session["TRFNo"] = trf_no;
                        _MTOModel.TRFNo = trf_no;
                        MTOApprove(_MTOModel, command,"","","");
                        _MTOModel.Message = "Approved";
                        _MTOModel.Command = command;
                        _MTOModel.TRFNo = _MTOModel.trf_no;
                        _MTOModel.TRFDate = _MTOModel.trf_dt.ToString("yyyy-MM-dd");
                        _MTOModel.TransType = "Update";
                        _MTOModel.AppStatus = "A";
                        _MTOModel.WF_status1 = _MTOModel.WF_status1;
                        _MTOModel.BtnName = "BtnApprove";
                        TempData["ModelData"] = _MTOModel;
                        UrlModel _urlModelApprove = new UrlModel();
                        _urlModelApprove.Command = _MTOModel.Command;
                        _urlModelApprove.TransType = _MTOModel.TransType;
                        _urlModelApprove.AppStatus = "D";
                        _urlModelApprove.BtnName = _MTOModel.BtnName;
                        _urlModelApprove.TRFNo = _MTOModel.trf_no;
                        _urlModelApprove.TRFNo = _MTOModel.TRFNo;
                        _urlModelApprove.TRFDate = _MTOModel.trf_dt.ToString("yyyy-MM-dd");
                        _urlModelApprove.TRFDate = _MTOModel.TRFDate;
                        _urlModelApprove.WF_status1 = _MTOModel.WF_status1;
                        TempData["ListFilterData"] = _MTOModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferOrder", _urlModelApprove);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        MTOModel _MTOModelRefesh = new MTOModel();
                        _MTOModelRefesh.Message =null;
                        _MTOModelRefesh.Command = command;
                        _MTOModelRefesh.TransType = "Save";
                        _MTOModelRefesh.BtnName = "Refresh";
                        TempData["ModelData"] = _MTOModelRefesh;
                        TempData["ListFilterData"] = _MTOModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferOrder");

                    case "Print":
                        return GenratePdfFile(_MTOModel);
                    //return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        MTOListModel _MTOListModel = new MTOListModel();
                            _MTOListModel.WF_status = _MTOModel.WF_status1;
                        TempData["ListFilterData"] = _MTOModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferOrderList", "MaterialTransferOrder", _MTOListModel);

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
        public FileResult GenratePdfFile(MTOModel _MTOModel)
        {
           
            ViewBag.DocumentMenuId = _MTOModel.docid;
            return File(GetPdfData(_MTOModel.trf_no, _MTOModel.trf_dt.ToString("yyyy-MM-dd")), "application/pdf", "MaterialTransferRequisition.pdf");
        }
        public byte[] GetPdfData(string Doc_No, string Doc_dt)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            string CompID = string.Empty;
            string BrchID = string.Empty;

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
                DataSet Details = _MTO_ISERVICES.GetMaterialTransferIssuePrint(CompID, BrchID, Doc_No, Convert.ToDateTime(Doc_dt).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[1].Rows[0]["digi_sign"].ToString();
                string trnsfer_type = Details.Tables[0].Rows[0]["src_type"].ToString();
                ViewBag.Title = "Material Transfer Issue";
                ViewBag.trf_type = trnsfer_type;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.DocStatus = Details.Tables[1].Rows[0]["trf_status"].ToString().Trim();
                string htmlcontent = string.Empty;

              htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialTransferOrder/MaterialTransferOrderPrint.cshtml"));
               

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            return null;
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
        [NonAction]
        public ActionResult SaveMTODetail(MTOModel _MTOModel)
        {
            try
            {
                if (_MTOModel.ForceClose != false)
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
                    _MTOModel.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    String SaveMessage = _MTO_ISERVICES.MTOForceClose(_MTOModel, CompID, br_id, mac_id);
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MTOModel.trf_no, "FC", userid, "");
                    string TRFNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    //Session["Message"] = "Save";
                    //Session["Command"] = "Update";
                    //TempData["TRFNo"] = _MTOModel.trf_no;
                    //TempData["TRFDate"] = _MTOModel.trf_dt;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    _MTOModel.Message = "Save";
                    _MTOModel.Command = "Update";
                    _MTOModel.TRFNo = _MTOModel.trf_no;
                    _MTOModel.TRFDate = _MTOModel.trf_dt.ToString();
                    _MTOModel.TransType = "Update";
                    _MTOModel.AppStatus = "D";
                    _MTOModel.BtnName = "Refresh";
                    TempData["ModelData"] = _MTOModel;
                    return RedirectToAction("MaterialTransferOrder");
                }
                else
                {
                    if (_MTOModel.CancelFlag == false)
                    {
                        if (Session["compid"] != null)
                        {
                            CompID = Session["compid"].ToString();
                        }
                        if (Session["userid"] != null)
                        {
                            userid = Session["userid"].ToString();
                        }

                        DataTable TRFHeader = new DataTable();
                        DataTable TRFItemDetails = new DataTable();
                        


                        DataTable dtheader = new DataTable();
                        dtheader.Columns.Add("MenuDocumentId", typeof(string));
                        dtheader.Columns.Add("TransType", typeof(string));
                        dtheader.Columns.Add("comp_id", typeof(int));
                        dtheader.Columns.Add("br_id", typeof(int));
                        dtheader.Columns.Add("trf_no", typeof(string));
                        dtheader.Columns.Add("trf_dt", typeof(DateTime));
                        dtheader.Columns.Add("trf_type", typeof(string));
                        dtheader.Columns.Add("trf_rem", typeof(string));
                        dtheader.Columns.Add("from_br", typeof(int));
                        dtheader.Columns.Add("to_br", typeof(int));
                        dtheader.Columns.Add("from_wh", typeof(int));
                        dtheader.Columns.Add("to_wh", typeof(int));                      
                        dtheader.Columns.Add("create_id", typeof(int));
                        dtheader.Columns.Add("mod_id", typeof(int));
                        dtheader.Columns.Add("trf_status", typeof(string));
                        dtheader.Columns.Add("UserMacaddress", typeof(string));
                        dtheader.Columns.Add("UserSystemName", typeof(string));
                        dtheader.Columns.Add("UserIP", typeof(string));
                       

                        DataRow dtrowHeader = dtheader.NewRow();
                        dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                        //dtrowHeader["TransType"] = Session["TransType"].ToString();
                        if(_MTOModel.trf_no != null)
                        {
                            dtrowHeader["TransType"] = "Update";
                        }
                        else
                        {
                            dtrowHeader["TransType"] = "Save";
                        }
                        dtrowHeader["comp_id"] = Session["CompId"].ToString();
                        dtrowHeader["br_id"] = Session["BranchId"].ToString();                       
                        dtrowHeader["trf_no"] = _MTOModel.trf_no;
                        dtrowHeader["trf_dt"] = _MTOModel.trf_dt;
                        dtrowHeader["trf_type"] = _MTOModel.trf_type;
                        dtrowHeader["trf_rem"] = _MTOModel.trf_rem;
                        dtrowHeader["to_br"] = Session["BranchId"].ToString();
                        if (_MTOModel.trf_type == "W")
                        {
                            dtrowHeader["from_br"] = Session["BranchId"].ToString();
                        }
                        else {
                            dtrowHeader["from_br"] = _MTOModel.from_br;
                        }
                        dtrowHeader["from_wh"] = _MTOModel.from_wh;
                        dtrowHeader["to_wh"] = _MTOModel.to_wh;
                        dtrowHeader["create_id"] = Session["UserId"].ToString();
                        dtrowHeader["mod_id"] = Session["UserId"].ToString();
                        //dtrowHeader["trf_status"] = Session["AppStatus"].ToString();
                        dtrowHeader["trf_status"] = "D";
                        dtrowHeader["UserMacaddress"] = Session["UserMacaddress"].ToString();
                        dtrowHeader["UserSystemName"] = Session["UserSystemName"].ToString();
                        dtrowHeader["UserIP"] = Session["UserIP"].ToString();
                      

                        dtheader.Rows.Add(dtrowHeader);
                        TRFHeader = dtheader;


                        DataTable dtItem = new DataTable();
                        dtItem.Columns.Add("comp_id", typeof(int));
                        dtItem.Columns.Add("br_id", typeof(int));
                        dtItem.Columns.Add("trf_no", typeof(string));
                        dtItem.Columns.Add("trf_dt", typeof(DateTime));
                        dtItem.Columns.Add("trf_type", typeof(string));
                        dtItem.Columns.Add("item_id", typeof(string));
                        dtItem.Columns.Add("uom_id", typeof(int));
                        dtItem.Columns.Add("trf_qty", typeof(string));                         
                        dtItem.Columns.Add("it_remarks", typeof(string));

                        JArray jObject = JArray.Parse(_MTOModel.Itemdetails);

                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["comp_id"] = Session["CompId"].ToString();
                            dtrowLines["br_id"] = Session["BranchId"].ToString();
                            dtrowLines["trf_no"] = _MTOModel.trf_no;
                            //dtrowLines["trf_dt"] = DateTime.Now;
                            dtrowLines["trf_dt"] = _MTOModel.trf_dt;
                            dtrowLines["trf_type"] = _MTOModel.trf_type;
                            dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                            dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                            dtrowLines["trf_qty"] = jObject[i]["RequQty"].ToString();                                      
                            dtrowLines["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                            dtItem.Rows.Add(dtrowLines);
                        }
                        TRFItemDetails = dtItem;
                        /*----------------------Sub Item ----------------------*/
                        DataTable dtSubItem = new DataTable();
                        dtSubItem.Columns.Add("item_id", typeof(string));
                        dtSubItem.Columns.Add("sub_item_id", typeof(string));
                        dtSubItem.Columns.Add("qty", typeof(string));
                        if (_MTOModel.SubItemDetailsDt != null)
                        {
                            JArray jObject2 = JArray.Parse(_MTOModel.SubItemDetailsDt);
                            for (int i = 0; i < jObject2.Count; i++)
                            {
                                DataRow dtrowItemdetails = dtSubItem.NewRow();
                                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                                dtSubItem.Rows.Add(dtrowItemdetails);
                            }
                        }

                        /*------------------Sub Item end----------------------*/
                        String SaveMessage = _MTO_ISERVICES.InsertMTO(TRFHeader, TRFItemDetails, dtSubItem);
                        string TRFNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                        if (Message == "Data_Not_Found")
                        {
                            CommonPageDetails();
                            //ViewBag.menupage = getDocumentName();
                            _MTOModel.Title = title;
                            var a = TRFNo.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim()+" in "+ _MTOModel.Title;                           
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _MTOModel.Message = Message.Replace("_", "");
                            return RedirectToAction("MaterialTransferOrder");
                        }
                        if (Message == "Update" || Message == "Save")
                        {
                            //Session["Message"] = "Save";
                            //Session["Command"] = "Update";
                            //Session["TRFNo"] = TRFNo;
                            //Session["TransType"] = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnSave";
                            _MTOModel.Message = "Save";
                            _MTOModel.Command = "Update";
                            _MTOModel.TRFNo = TRFNo;
                            _MTOModel.TRFDate = _MTOModel.trf_dt.ToString();
                            _MTOModel.TransType = "Update";
                            _MTOModel.AppStatus = "D";
                            _MTOModel.BtnName = "BtnSave";
                            TempData["ModelData"] = _MTOModel;
                        }
                        return RedirectToAction("MaterialTransferOrder");

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

                        string br_id = Session["BranchId"].ToString();
                        _MTOModel.CreatedBy = userid;
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        String SaveMessage = _MTO_ISERVICES.MTOCancel(_MTOModel, CompID, br_id, mac_id);
                        string TRFNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MTOModel.trf_no, "C", userid, "");
                        //Session["Message"] = "Cancelled";
                        //Session["Command"] = "Update";
                        //TempData["TRFNo"] = _MTOModel.trf_no;
                        //TempData["TRFDate"] = _MTOModel.trf_dt;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MTOModel.trf_no, "C", userid, "");
                        _MTOModel.Message = "Cancelled";
                        _MTOModel.Command = "Update";
                        _MTOModel.TRFNo = _MTOModel.trf_no;
                        _MTOModel.TRFDate = _MTOModel.trf_dt.ToString();
                        _MTOModel.TransType = "Update";
                        _MTOModel.AppStatus = "D";
                        _MTOModel.BtnName = "Refresh";
                        TempData["ModelData"] = _MTOModel;
                        return RedirectToAction("MaterialTransferOrder");


                    }
                }
                //return RedirectToAction("MaterialTransferOrder");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
               // return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult MTODelete(MTOModel _MTOModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                DataSet Message = _MTO_ISERVICES.MTODelete(_MTOModel, CompID, br_id, DocumentMenuId);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["TRFNo"] = "";
                //_MTOModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                // _MRSModel = null;
                MTOModel _MRSDeleteModel = new MTOModel();
                _MRSDeleteModel.Message = "Deleted";
                _MRSDeleteModel.Command = "Refresh";
                _MRSDeleteModel.TransType = "Refresh";
                _MRSDeleteModel.AppStatus = "DL";
                _MRSDeleteModel.BtnName = "BtnDelete";
                TempData["ModelData"] = _MRSDeleteModel;
                return RedirectToAction("MaterialTransferOrder", "MaterialTransferOrder");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        [NonAction]
        private ActionResult MTOApprove(MTOModel _MTOModel, string command, string ListFilterData1,string docid, string WF_Status1)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                if (docid != null)
                {
                     DocumentMenuId= docid;
                }
                string br_id = Session["BranchId"].ToString();
                string app_id = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                //string trf_type = _MTOModel.trf_type;
                DataSet Message = _MTO_ISERVICES.MTOApprove(_MTOModel, CompID, br_id,app_id,mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                //Session["TRFNo"] = _MTOModel.trf_no;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'A';
                //Session["BtnName"] = "BtnApprove";
                _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MTOModel.trf_no, "AP", userid, "");
                _MTOModel.Message = "Approved";
                _MTOModel.Command = command;
                _MTOModel.TRFNo = _MTOModel.trf_no;
                _MTOModel.TransType = "Update";
                _MTOModel.AppStatus = "A";
                _MTOModel.WF_status1 = WF_Status1;
                _MTOModel.BtnName = "BtnApprove";
                TempData["ModelData"] = _MTOModel;
                UrlModel _urlModelApprove = new UrlModel();
                _urlModelApprove.Command = _MTOModel.Command;
                _urlModelApprove.TransType = _MTOModel.TransType;
               // _urlModelApprove.AppStatus = "A";
                _urlModelApprove.BtnName = _MTOModel.BtnName;
                _urlModelApprove.TRFNo = _MTOModel.trf_no;
                _urlModelApprove.WF_status1 = _MTOModel.WF_status1;
                //_urlModelApprove.TRFNo = _MTOModel.TRFNo;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("MaterialTransferOrder", "MaterialTransferOrder", _urlModelApprove);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }

        //private string getDocumentName()
        //{

        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    if (Session["Language"] != null)
        //    {
        //        language = Session["Language"].ToString();
        //    }
        //    string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //    return DocumentName;
        //}
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
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}

        //[NonAction]
        //private DataTable GetFromWHList()
        //{
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    string BranchId = Session["BranchId"].ToString();

        //    DataTable dt = _MTO_ISERVICES.GetWhList(CompID, BranchId);
        //    return dt;
        //}

        //public DataTable GetFromBranchList()
        //{
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    if (Session["BranchId"] != null)
        //    {
        //        BrchID = Session["BranchId"].ToString();
        //    }
        //    DataTable dt = _MTO_ISERVICES.GetToBranchList(CompID, BrchID);
        //    return dt;
        //}

        //[HttpPost]
        //private DataTable GetToWHList(string Tobranch)
        //{
        //    List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();;
        //    _MTOModel = new MTOModel();
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    //string BranchId = Session["BranchId"].ToString();

        //    DataTable dt = _MTO_ISERVICES.GetToWhList(CompID, Tobranch);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        ToWharehouse _ToWharehouse = new ToWharehouse();
        //       _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
        //       _ToWharehouse.wh_val = dr["wh_name"].ToString();
        //       _ToWharehouseList.Add(_ToWharehouse);

        //    }

        //    _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
        //    _MTOModel.ToWharehouseList = _ToWharehouseList;

        //    return dt;
        //}
        [NonAction]
        private DataSet MTO_GetAllDDLListAndListPageData(string flag,string PageName, string UserID, string WF_status, string DocumentMenuId
            , string startDate, string CurrentDate)
        {
            string CompID = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            string BranchId = Session["BranchId"].ToString();
            DataSet ds = _MTO_ISERVICES.MTO_GetAllDDLListAndListPageData(CompID, BranchId, flag, PageName, UserID, WF_status, DocumentMenuId, startDate, CurrentDate);
            return ds;
        }
        [HttpPost]
        public JsonResult GetToWHList1(string SrcBrId, string DocumentMenuId)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable dt = _MTO_ISERVICES.GetToWhList(CompID, SrcBrId, DocumentMenuId);
                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);    
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
      
        //public void GetStatusList(MTOListModel _MTOListModel)
        //{
        //    try
        //    {
        //        List<Status> statusLists = new List<Status>();
        //        var other = new CommonController(_Common_IServices);
        //        var statusListsC = other.GetStatusList1(DocumentMenuId);
        //        var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
        //        _MTOListModel.StatusList = listOfStatus;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //    }
        //}
        private List<MTOList> GetMTODetailList(MTOListModel _MTOListModel)
        {
            _MTOList = new List<MTOList>();

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            string wfstatus = "";
            //if (Session["WF_status"] != null)
            if (_MTOListModel.WF_status != null)
            {
                //wfstatus = Session["WF_status"].ToString();
                wfstatus = _MTOListModel.WF_status;
            }
            else
            {
                wfstatus = "";
            }
            DataSet dt = new DataSet();
            dt = _MTOList_ISERVICES.GetMTODetailList(CompID, BrchID,  _MTOListModel.to_wh, _MTOListModel.to_br, _MTOListModel.MTO_FromDate, _MTOListModel.MTO_ToDate, _MTOListModel.TRF_Type, _MTOListModel.TRFStatus, UserID, wfstatus, DocumentMenuId);
            if (dt.Tables[1].Rows.Count > 0)
            {
                //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
            }
                if (dt.Tables[0].Rows.Count > 0)
            {
                
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    MTOList _TempMTOList = new MTOList();
                    _TempMTOList.TRFNo = dr["trf_no"].ToString();
                    _TempMTOList.TRFDate = dr["TrfDate"].ToString();
                    _TempMTOList.trf_date = dr["trf_date"].ToString();
                    _TempMTOList.TRFType = dr["TRFType"].ToString();
                    _TempMTOList.FromBranch = dr["frombr"].ToString();
                    _TempMTOList.FromWH = dr["fromwh"].ToString();
                    _TempMTOList.ToBranch = dr["tobr"].ToString();
                    _TempMTOList.ToWH = dr["towh"].ToString();
                    _TempMTOList.TRFList_Stauts = dr["Status"].ToString();
                    _TempMTOList.CreateDate = dr["CreateDate"].ToString();
                    _TempMTOList.ApproveDate = dr["ApproveDate"].ToString();
                    _TempMTOList.ModifyDate = dr["ModifyDate"].ToString();
                    _TempMTOList.create_by = dr["create_by"].ToString();
                    _TempMTOList.app_by = dr["app_by"].ToString();
                    _TempMTOList.mod_by = dr["mod_by"].ToString();
                    _MTOList.Add(_TempMTOList);
                }
            }
            return _MTOList;
        }
        [HttpPost]
        public ActionResult SearchMTODetail(int toWh, int tobranch, string Fromdate, string Todate, string TransferType, string Status)
        {
            _MTOList = new List<MTOList>();
            MTOListModel _MTOListModel = new MTOListModel();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            //string wfstatus = "";
            //if (Session["WF_status"] != null)
            //{
            //    wfstatus = Session["WF_status"].ToString();
            //}
            //else
            //{
            //    wfstatus = "";
            //}\
            _MTOListModel.WF_status = "";
            DataSet dt = new DataSet();
            dt = _MTOList_ISERVICES.GetSerchDetailList(CompID, BrchID, toWh, tobranch, Fromdate, Todate, TransferType, Status, UserID,  DocumentMenuId);
            //Session["MTOSearch"] = "MTO_Search";
            _MTOListModel.MTOSearch = "MTO_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                
               
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    MTOList _TempMTOList = new MTOList();
                    _TempMTOList.TRFNo = dr["trf_no"].ToString();
                    _TempMTOList.TRFDate = dr["TrfDate"].ToString();
                    //_TempMTOList.trf_date = dr["trf_date"].ToString();
                    _TempMTOList.TRFType = dr["TRFType"].ToString();
                    _TempMTOList.FromBranch = dr["frombr"].ToString();
                    _TempMTOList.FromWH = dr["fromwh"].ToString();
                    _TempMTOList.ToBranch = dr["tobr"].ToString();
                    _TempMTOList.ToWH = dr["towh"].ToString();
                    _TempMTOList.TRFList_Stauts = dr["Status"].ToString();
                    _TempMTOList.CreateDate = dr["CreateDate"].ToString();
                    _TempMTOList.ApproveDate = dr["ApproveDate"].ToString();
                    _TempMTOList.ModifyDate = dr["ModifyDate"].ToString();
                    _TempMTOList.create_by = dr["create_by"].ToString();
                    _TempMTOList.app_by = dr["app_by"].ToString();
                    _TempMTOList.mod_by = dr["mod_by"].ToString();
                    _MTOList.Add(_TempMTOList);
                }
            }
            _MTOListModel.BindMTOList = _MTOList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMtoList.cshtml", _MTOListModel);
        }
        public ActionResult getListOfItems(MTOModel _MTOModel)
        {
            //JsonResult DataRows = null;
            string ItmName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string ToBranchId = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BranchId = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BranchId = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_MTOModel.MTO_ItemName))
                {
                    ItmName = "0";
                }
                else
                {
                    ItmName = _MTOModel.MTO_ItemName;
                }
                if (_MTOModel.trf_type == "W" || _MTOModel.trf_type == "0" || _MTOModel.trf_type == null)
                {
                    ToBranchId= Session["BranchId"].ToString();
                }
                else
                {
                    ToBranchId =Convert.ToString(_MTOModel.to_br);
                }
                DataSet SOItmList = _MTO_ISERVICES.GetItemList(Comp_ID, BranchId, ItmName, ToBranchId);
                if (SOItmList.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    for (int i = 0; i < SOItmList.Tables[0].Rows.Count; i++)
                    {
                        string itemId = SOItmList.Tables[0].Rows[i]["Item_id"].ToString();
                        string itemName = SOItmList.Tables[0].Rows[i]["Item_name"].ToString();
                        string Uom = SOItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        ItemList.Add(itemId + "_" + Uom, itemName);
                    }
                }
                return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
                //DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            //return DataRows;
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
    , string Flag, string Status, string Doc_no, string Doc_dt)
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
                int QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(),"0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _MTO_ISERVICES.MTR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                }
                else if (Flag == "Issued")
                {
                    dt = _MTO_ISERVICES.MTR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                }
                else if (Flag == "Received")
                {
                    dt = _MTO_ISERVICES.MTR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag == "Quantity" ? Flag : "MTO",
                    //_subitemPageName = "MTO",
                    dt_SubItemDetails = dt,
                    IsDisabled=IsDisabled,
                    decimalAllowed="Y"
                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetSubItemDetails1(string Item_id,string Wh_id,string br_id)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = br_id;
                dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, Wh_id, Item_id, null, "wh").Tables[0];
                ViewBag.SubitemAvlStockDetail = dt;
                //return View("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
        string IsNull(string str, string str2)
        {
            return string.IsNullOrEmpty(str) == true ? str2 : str;
        }
        [HttpPost]
        public JsonResult GetSourceAndDestinationList(string wh_id,string doc_id=null)
        {
            try
            {
                JsonResult DataRows = null;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet dt = _MTO_ISERVICES.GetSourceAndDestinationList(CompID, Br_ID, wh_id, doc_id);
                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
    }
}