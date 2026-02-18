using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesForecast;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesForecast;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.SalesForecast
{
    public class SalesForecastController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105103150", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SalesForecast_ISERVICES _SalesForecast_ISERVICES;
        public SalesForecastController(Common_IServices _Common_IServices, SalesForecast_ISERVICES _SalesForecast_ISERVICES)
        {
            this._SalesForecast_ISERVICES = _SalesForecast_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/SalesForecast
        SalesForecastModel _SalesForecastModel = new SalesForecastModel();
        
        public ActionResult SalesForecastList(SalesForecastModel _SalesForecastModel)//List page
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //if (Session["userid"] != null)
                //{
                //    UserID = Session["userid"].ToString();
                //}
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (Session["CompId"] != null)
                {
                    ViewBag.MenuPageName = getDocumentName();
                    _SalesForecastModel.Title = title;
                    ViewBag.GetFCList = GetForeCastList(_SalesForecastModel);
                    //Session["SFCSearch"]= "";
                    ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesForecast/SalesForecastList.cshtml", _SalesForecastModel);
                }
                else
                {
                    RedirectToAction("Home", "Index");
                }
                return RedirectToAction("Home", "Index");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable GetRoleList()
        {
            try
            {
                string UserID = string.Empty;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetForeCastList(SalesForecastModel _SalesForecastModel)
        {

            DataTable dt = new DataTable();
            try
            {
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
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if(_SalesForecastModel.WF_Status != null)
                {
                    wfstatus = _SalesForecastModel.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }

                //string Comp_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    //CompID = Session["CompId"].ToString();
                    //string br_id = Session["BranchId"].ToString();
                    dt = _SalesForecast_ISERVICES.GetFCList(Convert.ToInt32(CompID), Convert.ToInt32(BrchID), UserID, wfstatus, DocumentMenuId);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
            return dt;
        }
        
        public ActionResult dbClickEdit(string sf_id/*, string f_fy, string f_period, string f_status*/,string WF_Status)
        {
            try
            {
                if (Session["CompId"] != null)
                {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message1"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    SalesForecastModel _dblclick = new SalesForecastModel();
                    UrlModel _urlModel = new UrlModel();
                    _dblclick.Commandfc = "View";
                    _dblclick.TransTypefc = "Update";
                    _dblclick.BtnNamefc = "BtnToDetailPage";
                    //_SalesForecast_addnewModel.TransTypefc = "View";
                    _dblclick.sf_id = sf_id;
                    _dblclick.dbclickfc = "dbclick";
                    if(WF_Status != null && WF_Status != "")
                    {
                        _dblclick.WF_Status1 = WF_Status;
                        _urlModel.WF_Status1 = WF_Status;
                    }                   
                    TempData["ModelData"] = _dblclick;                 
                    _urlModel.Commandfc = "View";
                    _urlModel.TransTypefc = "Update";
                    _urlModel.BtnNamefc = "BtnToDetailPage";
                    _urlModel.sf_id = sf_id;
                    _urlModel.dbclickfc = "dbclick";
                    
                    //Session["Messagefc"] = "";
                    //Session["Commandfc"] = "View";
                    //Session["TransTypefc"] = "EditNew";
                    //Session["BtnNamefc"] = "BtnToDetailPage";
                    //Session["TransTypefc"] = "Update";
                    //Session["sf_id"] = sf_id;
                    ////Session["f_freq"] = f_freq;
                    ////Session["f_fy"] = f_fy;
                    ////Session["f_period"] = f_period;
                    ////Session["f_status"] = f_status;
                    //Session["SaveUpd"] = "0";
                    //Session["dbclickfc"] = "dbclick";
                    string Comp_ID = Session["CompId"].ToString();
                    return RedirectToAction("AddSalesForecastDetail", _urlModel);
                }
                else
                {
                    RedirectToAction("Home", "Index");
                }
                return RedirectToAction("AddSalesForecastDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public ActionResult AddNewSalesForecastDetail()//List page add button to detail page
        {
            //try
            //{
               SalesForecastModel _AddnewSalesForecast = new SalesForecastModel();
            //    ViewBag.DocumentMenuId = DocumentMenuId;
            //    var other = new CommonController(_Common_IServices);
            //    if (Session["CompId"] != null)
            //    {
            //        CompID = Session["CompId"].ToString();
            //    }
            //    if (Session["BranchId"] != null)
            //    {
            //        BrchID = Session["BranchId"].ToString();
            //    }
            //    if (Session["userid"] != null)
            //    {
            //        UserID = Session["userid"].ToString();
            //    }
               
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                //Session.Remove("TransTypefc");
                //Session["Messagefc"] = "New";
                //Session["Commandfc"] = "Add";
                //Session["TransTypefc"] = "Save";
                //Session["BtnNamefc"] = "BtnAddNew";
                //Session["DocumentStatusfc"] = "";
                _AddnewSalesForecast.Commandfc = "Add";
                _AddnewSalesForecast.TransTypefc = "Save";
                _AddnewSalesForecast.BtnNamefc = "BtnAddNew";
               // BindDDLOnPageLoad(_AddnewSalesForecast);
                TempData["ModelData"] = _AddnewSalesForecast;
                UrlModel _urladdnewModel = new UrlModel();
                _urladdnewModel.Commandfc = "Add";
                _urladdnewModel.TransTypefc = "Save";
                _urladdnewModel.BtnNamefc = "BtnAddNew";
            //ViewBag.MenuPageName = getDocumentName();
            //ViewBag.MenuPageName = getDocumentName();
            //_AddnewSalesForecast.Title = title;
            //ViewBag.VBRoleList = GetRoleList();
            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("SalesForecastList");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("AddSalesForecastDetail", _urladdnewModel);
                //return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesForecast/SalesForecastDetail.cshtml", _AddnewSalesForecast);
            //}
            //catch (Exception ex)
            //{
            //    string path = Server.MapPath("~");
            //    Errorlog.LogError(path, ex);
            //    return Json("ErrorPage");
            //}
        }
        public ActionResult AddSalesForecastDetail(UrlModel _urlmodel)
        {
            try
            {
                ViewBag.DocumentMenuId = DocumentMenuId;
                var other = new CommonController(_Common_IServices);
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
               var _SalesForecastModels =  TempData["ModelData"] as SalesForecastModel;
                if(_SalesForecastModels != null)
                {
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    BindDDLOnPageLoad(_SalesForecastModels);
                    ViewBag.MenuPageName = getDocumentName();
                    _SalesForecastModels.Title = title;
                    //if (Session["dbclickfc"] != null && Session["TransTypefc"] != null)
                    if (_SalesForecastModels.dbclickfc != null && _SalesForecastModels.TransTypefc != null)
                    {
                        DataSet ds = new DataSet();
                        //string sf_id = Session["sf_id"].ToString();
                        string sf_id = _SalesForecastModels.sf_id;
                        //Session["f_status"].ToString()
                        ds = _SalesForecast_ISERVICES.BinddbClick(Convert.ToInt32(CompID)
                            , Convert.ToInt32(BrchID)
                            /*, Session["f_freq"].ToString(), Session["f_fy"].ToString(),Convert.ToInt32(Session["f_period"].ToString())*/,
                            sf_id, UserID, DocumentMenuId);
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //Session["DocumentStatusfc"] = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                _SalesForecastModels.DocumentStatusfc = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                string jcstatus = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                //Session["f_status"] = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                _SalesForecastModels.f_status = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                if (jcstatus == "D")
                                {
                                    //Session["f_status"] = jcstatus;
                                    _SalesForecastModels.f_status = jcstatus;
                                }
                                if (jcstatus == "C")
                                {
                                    //Session["BtnNamefc"] = "BtnToDetailPage";
                                    _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
                                    _SalesForecastModels.CancelFlag = true;
                                }
                                if (jcstatus == "F")
                                {
                                    //Session["BtnNamefc"] = "BtnToDetailPage";
                                    _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
                                    _SalesForecastModels.CancelFlag = false;
                                }
                                if (jcstatus == "A")
                                {
                                    //Session["BtnNamefc"] = "BtnToDetailPage";
                                    _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
                                    _SalesForecastModels.CancelFlag = false;
                                }
                                _SalesForecastModels.sf_id = Convert.ToString(ds.Tables[0].Rows[0]["sf_id"]);
                                _SalesForecastModels.ddl_f_frequency = Convert.ToString(ds.Tables[0].Rows[0]["f_freq"]);
                                _SalesForecastModels.ddl_financial_year = Convert.ToString(ds.Tables[0].Rows[0]["f_fy1"]);
                                //BindPeriod(_SalesForecastModels.ddl_f_frequency, _SalesForecastModels.ddl_financial_year);
                                _SalesForecastModels.ddl_period = Convert.ToString(ds.Tables[0].Rows[0]["f_period1"]);
                                _SalesForecastModels.txtFromDate = Convert.ToString(ds.Tables[0].Rows[0]["from_date"]);
                                _SalesForecastModels.txtToDate = Convert.ToString(ds.Tables[0].Rows[0]["to_date"]);
                                _SalesForecastModels.CreatedByName = Convert.ToString(ds.Tables[0].Rows[0]["created_id"]);
                                _SalesForecastModels.CreatedOn = Convert.ToString(ds.Tables[0].Rows[0]["create_dt"]);
                                _SalesForecastModels.CreatedDate = Convert.ToString(ds.Tables[0].Rows[0]["create_date"]);
                                _SalesForecastModels.ApprovedBy = Convert.ToString(ds.Tables[0].Rows[0]["app_id"]);
                                _SalesForecastModels.ApprovedOn = Convert.ToString(ds.Tables[0].Rows[0]["app_dt"]);
                                _SalesForecastModels.AmmendedBy = Convert.ToString(ds.Tables[0].Rows[0]["mod_id"]);
                                _SalesForecastModels.AmmendedOn = Convert.ToString(ds.Tables[0].Rows[0]["mod_dt"]);
                                _SalesForecastModels.f_status = Convert.ToString(ds.Tables[0].Rows[0]["status_name"]);
                                _SalesForecastModels.f_statusId = Convert.ToString(ds.Tables[0].Rows[0]["f_status"]);
                                _SalesForecastModels.Createid = ds.Tables[0].Rows[0]["creator_id"].ToString();

                                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                                string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                                string doc_status = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                _SalesForecastModels.StatusCode = doc_status;

                                _SalesForecastModels.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                                _SalesForecastModels.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                                if (doc_status != "D" && doc_status != "F")
                                {
                                    ViewBag.AppLevel = ds.Tables[5];
                                }
                                //if (ViewBag.AppLevel != null && Session["Commandfc"].ToString() != "Edit")
                                if (ViewBag.AppLevel != null && _SalesForecastModels.Commandfc != "Edit")
                                {

                                    var sent_to = "";
                                    var nextLevel = "";
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                                    }

                                    if (ds.Tables[4].Rows.Count > 0)
                                    {
                                        nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
                                    }

                                    if (doc_status == "D")
                                    {
                                        if (create_id != UserID)
                                        {
                                            //Session["BtnNamefc"] = "Refresh";
                                            _SalesForecastModels.BtnNamefc = "Refresh";
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
                                                //Session["BtnNamefc"] = "BtnToDetailPage";
                                                _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
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
                                                //Session["BtnNamefc"] = "BtnToDetailPage";
                                                _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
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
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
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
                                                //Session["BtnNamefc"] = "BtnToDetailPage";
                                                _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
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
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
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
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecastModels.BtnNamefc = "BtnToDetailPage";
                                        }
                                    }
                                    if (doc_status == "A")
                                    {
                                        if (create_id == UserID || approval_id == UserID)
                                        {
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecastModels.BtnNamefc = "BtnToDetailPage";

                                        }
                                        else
                                        {
                                            //Session["BtnNamefc"] = "Refresh";
                                            _SalesForecastModels.BtnNamefc = "Refresh";
                                        }
                                    }
                                }
                                if (ViewBag.AppLevel.Rows.Count == 0)
                                {
                                    ViewBag.Approve = "Y";
                                }
                                string fy = _SalesForecastModels.ddl_financial_year;
                                string[] splitFY = fy.Split(',');
                                //DataSet ds1 = _SalesForecast_ISERVICES.BindFinancialYear(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), _SalesForecastModels.ddl_f_frequency, splitFY[0], ds.Tables[0].Rows[0]["f_period1"].ToString());
                                DataSet ds1 = _SalesForecast_ISERVICES.BindFinancialYear(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), _SalesForecastModels.ddl_f_frequency, splitFY[0], ds.Tables[0].Rows[0]["f_period1"].ToString());
                                //List<period> plist = new List<period>();
                                //period pObj = new period();
                                //pObj.id = "0";
                                //pObj.name = "---Select---";
                                //plist.Add(pObj);
                                //_SalesForecastModels.ddl_periodList = plist;
                                //GetItemNameInDDL(_SalesForecastModels);
                                List<period> fyList = new List<period>();
                                if (ds1.Tables[1].Rows.Count > 0)
                                {
                                    //period fyObj1 = new period();/*commented by Hina on 13-09-2024 and add below out of table*/
                                    //fyObj1.id = "0";
                                    //fyObj1.name = "---Select---";
                                    //fyList.Add(fyObj1);
                                    foreach (DataRow data in ds1.Tables[1].Rows)
                                    {
                                        period fyObj = new period();
                                        fyObj.id = data["id"].ToString();
                                        fyObj.name = data["name"].ToString();
                                        fyList.Add(fyObj);
                                    }
                                }
                                period fyObj1 = new period();/*ADD by Hina on 13-09-2024 */
                                fyObj1.id = "0";
                                fyObj1.name = "---Select---";
                                fyList.Add(fyObj1);
                                _SalesForecastModels.ddl_periodList = fyList;


                                List<FC_Item_details> ArrItem = new List<FC_Item_details>();
                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                {
                                    FC_Item_details fid = new FC_Item_details();
                                    fid.item_id = Convert.ToString(ds.Tables[1].Rows[i]["item_id"]);
                                    fid.item_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                    fid.sub_item = Convert.ToString(ds.Tables[1].Rows[i]["sub_item"]);
                                    fid.uom_id = Convert.ToString(ds.Tables[1].Rows[i]["uom_id"]);
                                    fid.uom_alias = Convert.ToString(ds.Tables[1].Rows[i]["uom_alias"]);
                                    fid.pys_q = Convert.ToString(ds.Tables[1].Rows[i]["pys_q"]);
                                    fid.pys_v = Convert.ToString(ds.Tables[1].Rows[i]["pys_v"]);
                                    fid.inc_by = Convert.ToString(ds.Tables[1].Rows[i]["inc_by"]);
                                    fid.red_by = Convert.ToString(ds.Tables[1].Rows[i]["red_by"]);
                                    fid.tgt_qty = Convert.ToString(ds.Tables[1].Rows[i]["tgt_qty"]);
                                    fid.sale_price = Convert.ToString(ds.Tables[1].Rows[i]["sale_price"]);
                                    fid.tgt_val = Convert.ToString(ds.Tables[1].Rows[i]["tgt_val"]);
                                    fid.actual_sale_q = Convert.ToString(ds.Tables[1].Rows[i]["actual_sale_q"]);
                                    fid.actual_sale_v = Convert.ToString(ds.Tables[1].Rows[i]["actual_sale_v"]);
                                    ArrItem.Add(fid);
                                }
                                _SalesForecastModels.FC_Item_Details_List = ArrItem;


                                _SalesForecastModels.footer_PreviousYearSalesInValue = Convert.ToString(ds.Tables[2].Rows[0]["pys_v"]);
                                _SalesForecastModels.footer_TargetSalesInValue = Convert.ToString(ds.Tables[2].Rows[0]["tgt_val"]);
                                _SalesForecastModels.footer_ActualSaleInValue = Convert.ToString(ds.Tables[2].Rows[0]["actual_sale_v"]);
                            }
                            ViewBag.SubItemDetails = ds.Tables[6];
                        }

                        //Session.Remove("f_freq");
                        //Session.Remove("f_fy");
                        //Session.Remove("f_period");
                    }
                    //if(Session["err"] !=null)
                    if (_SalesForecastModels.err != null)
                    {
                        //if (Session["err"].ToString() == "1")
                        if (_SalesForecastModels.err == "1")
                        {
                            //Session.Remove("dbclickfc");
                            _SalesForecastModels.dbclickfc = null;
                            //Session.Remove("err");
                            _SalesForecastModels.err = null;
                            string path = Server.MapPath("~");
                            //Errorlog.LogError(path, ex);
                            return View("~/Views/Shared/Error.cshtml");
                        }
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    _SalesForecastModels.Title = title;
                    ViewBag.VBRoleList = GetRoleList();
                   
                    //Session["DocumentStatusfc"] = Session["DocumentStatusfc"]==null?"": Session["DocumentStatusfc"];
                    _SalesForecastModels.DocumentStatusfc = _SalesForecastModels.DocumentStatusfc == null ? "" : _SalesForecastModels.DocumentStatusfc;
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesForecast/SalesForecastDetail.cshtml", _SalesForecastModels);
                }
                else
                {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    var commCont = new CommonController(_Common_IServices);
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {
                        TempData["Message1"] = "Financial Year not Exist";
                    }
                    /*End to chk Financial year exist or not*/
                    SalesForecastModel _SalesForecast_Model = new SalesForecastModel();
                    if(_urlmodel != null)
                    {
                        _SalesForecast_Model.TransTypefc = _urlmodel.TransTypefc;
                        _SalesForecast_Model.Commandfc = _urlmodel.Commandfc;
                        _SalesForecast_Model.BtnNamefc = _urlmodel.BtnNamefc;
                        _SalesForecast_Model.dbclickfc = _urlmodel.dbclickfc;
                        _SalesForecast_Model.sf_id = _urlmodel.sf_id;
                    }
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    BindDDLOnPageLoad(_SalesForecast_Model);
                    ViewBag.MenuPageName = getDocumentName();
                    _SalesForecast_Model.Title = title;
                    //if (Session["dbclickfc"] != null && Session["TransTypefc"] != null)
                    if (_SalesForecast_Model.dbclickfc != null && _SalesForecast_Model.TransTypefc != null)
                    {
                        DataSet ds = new DataSet();
                        //string sf_id = Session["sf_id"].ToString();
                        string sf_id = _SalesForecast_Model.sf_id;
                        //Session["f_status"].ToString()
                        ds = _SalesForecast_ISERVICES.BinddbClick(Convert.ToInt32(CompID)
                            , Convert.ToInt32(BrchID)
                            /*, Session["f_freq"].ToString(), Session["f_fy"].ToString(),Convert.ToInt32(Session["f_period"].ToString())*/,
                            sf_id, UserID, DocumentMenuId);
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {



                                //Session["DocumentStatusfc"] = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                _SalesForecast_Model.DocumentStatusfc = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                string jcstatus = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                //Session["f_status"] = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                _SalesForecast_Model.f_status = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                if (jcstatus == "D")
                                {
                                    //Session["f_status"] = jcstatus;
                                    _SalesForecast_Model.f_status = jcstatus;
                                }
                                if (jcstatus == "C")
                                {
                                    //Session["BtnNamefc"] = "BtnToDetailPage";
                                    _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
                                    _SalesForecast_Model.CancelFlag = true;
                                }
                                if (jcstatus == "F")
                                {
                                    //Session["BtnNamefc"] = "BtnToDetailPage";
                                    _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
                                    _SalesForecast_Model.CancelFlag = false;
                                }
                                if (jcstatus == "A")
                                {
                                    //Session["BtnNamefc"] = "BtnToDetailPage";
                                    _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
                                    _SalesForecast_Model.CancelFlag = false;
                                }
                                _SalesForecast_Model.sf_id = Convert.ToString(ds.Tables[0].Rows[0]["sf_id"]);
                                _SalesForecast_Model.ddl_f_frequency = Convert.ToString(ds.Tables[0].Rows[0]["f_freq"]);
                                _SalesForecast_Model.ddl_financial_year = Convert.ToString(ds.Tables[0].Rows[0]["f_fy1"]);
                                //BindPeriod(_SalesForecast_Model.ddl_f_frequency, _SalesForecast_Model.ddl_financial_year);
                                _SalesForecast_Model.ddl_period = Convert.ToString(ds.Tables[0].Rows[0]["f_period1"]);
                                _SalesForecast_Model.txtFromDate = Convert.ToString(ds.Tables[0].Rows[0]["from_date"]);
                                _SalesForecast_Model.txtToDate = Convert.ToString(ds.Tables[0].Rows[0]["to_date"]);
                                _SalesForecast_Model.CreatedByName = Convert.ToString(ds.Tables[0].Rows[0]["created_id"]);
                                _SalesForecast_Model.CreatedOn = Convert.ToString(ds.Tables[0].Rows[0]["create_dt"]);
                                _SalesForecast_Model.CreatedDate = Convert.ToString(ds.Tables[0].Rows[0]["create_date"]);
                                _SalesForecast_Model.ApprovedBy = Convert.ToString(ds.Tables[0].Rows[0]["app_id"]);
                                _SalesForecast_Model.ApprovedOn = Convert.ToString(ds.Tables[0].Rows[0]["app_dt"]);
                                _SalesForecast_Model.AmmendedBy = Convert.ToString(ds.Tables[0].Rows[0]["mod_id"]);
                                _SalesForecast_Model.AmmendedOn = Convert.ToString(ds.Tables[0].Rows[0]["mod_dt"]);
                                _SalesForecast_Model.f_status = Convert.ToString(ds.Tables[0].Rows[0]["status_name"]);
                                _SalesForecast_Model.f_statusId = Convert.ToString(ds.Tables[0].Rows[0]["f_status"]);
                                _SalesForecast_Model.Createid = ds.Tables[0].Rows[0]["creator_id"].ToString();

                                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                                string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                                string doc_status = ds.Tables[0].Rows[0]["f_status"].ToString().Trim();
                                _SalesForecast_Model.StatusCode = doc_status;

                                _SalesForecast_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                                _SalesForecast_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                                if (doc_status != "D" && doc_status != "F")
                                {
                                    ViewBag.AppLevel = ds.Tables[5];
                                }
                                //if (ViewBag.AppLevel != null && Session["Commandfc"].ToString() != "Edit")
                                if (ViewBag.AppLevel != null && _SalesForecast_Model.Commandfc != "Edit")
                                {

                                    var sent_to = "";
                                    var nextLevel = "";
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                                    }

                                    if (ds.Tables[4].Rows.Count > 0)
                                    {
                                        nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
                                    }

                                    if (doc_status == "D")
                                    {
                                        if (create_id != UserID)
                                        {
                                            //Session["BtnNamefc"] = "Refresh";
                                            _SalesForecast_Model.BtnNamefc = "Refresh";
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
                                                //Session["BtnNamefc"] = "BtnToDetailPage";
                                                _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
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
                                                //Session["BtnNamefc"] = "BtnToDetailPage";
                                                _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
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
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
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
                                                //Session["BtnNamefc"] = "BtnToDetailPage";
                                                _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
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
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
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
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";
                                        }
                                    }
                                    if (doc_status == "A")
                                    {
                                        if (create_id == UserID || approval_id == UserID)
                                        {
                                            //Session["BtnNamefc"] = "BtnToDetailPage";
                                            _SalesForecast_Model.BtnNamefc = "BtnToDetailPage";

                                        }
                                        else
                                        {
                                            //Session["BtnNamefc"] = "Refresh";
                                            _SalesForecast_Model.BtnNamefc = "Refresh";
                                        }
                                    }
                                }
                                if (ViewBag.AppLevel.Rows.Count == 0)
                                {
                                    ViewBag.Approve = "Y";
                                }
                                string fy = _SalesForecast_Model.ddl_financial_year;
                                string[] splitFY = fy.Split(',');
                                //DataSet ds1 = _SalesForecast_ISERVICES.BindFinancialYear(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), _SalesForecast_Model.ddl_f_frequency, splitFY[0], ds.Tables[0].Rows[0]["f_period1"].ToString());
                                DataSet ds1 = _SalesForecast_ISERVICES.BindFinancialYear(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), _SalesForecast_Model.ddl_f_frequency, splitFY[0], ds.Tables[0].Rows[0]["f_period1"].ToString());
                                //List<period> plist = new List<period>();
                                //period pObj = new period();
                                //pObj.id = "0";
                                //pObj.name = "---Select---";
                                //plist.Add(pObj);
                                //_SalesForecast_Model.ddl_periodList = plist;
                                //GetItemNameInDDL(_SalesForecast_Model);
                                List<period> fyList = new List<period>();
                                if (ds1.Tables[1].Rows.Count > 0)
                                {
                                    //period fyObj1 = new period();/*commented by Hina on 13-09-2024 and add below out of table*/
                                    //fyObj1.id = "0";
                                    //fyObj1.name = "---Select---";
                                    //fyList.Add(fyObj1);
                                    foreach (DataRow data in ds1.Tables[1].Rows)
                                    {
                                        period fyObj = new period();
                                        fyObj.id = data["id"].ToString();
                                        fyObj.name = data["name"].ToString();
                                        fyList.Add(fyObj);
                                    }
                                }
                                period fyObj1 = new period();/*Add By Hina on 13-09-2024*/
                                fyObj1.id = "0";
                                fyObj1.name = "---Select---";
                                fyList.Add(fyObj1);
                                _SalesForecast_Model.ddl_periodList = fyList;


                                List<FC_Item_details> ArrItem = new List<FC_Item_details>();
                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                {
                                    FC_Item_details fid = new FC_Item_details();
                                    fid.item_id = Convert.ToString(ds.Tables[1].Rows[i]["item_id"]);
                                    fid.item_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                    fid.sub_item = Convert.ToString(ds.Tables[1].Rows[i]["sub_item"]);
                                    fid.uom_id = Convert.ToString(ds.Tables[1].Rows[i]["uom_id"]);
                                    fid.uom_alias = Convert.ToString(ds.Tables[1].Rows[i]["uom_alias"]);
                                    fid.pys_q = Convert.ToString(ds.Tables[1].Rows[i]["pys_q"]);
                                    fid.pys_v = Convert.ToString(ds.Tables[1].Rows[i]["pys_v"]);
                                    fid.inc_by = Convert.ToString(ds.Tables[1].Rows[i]["inc_by"]);
                                    fid.red_by = Convert.ToString(ds.Tables[1].Rows[i]["red_by"]);
                                    fid.tgt_qty = Convert.ToString(ds.Tables[1].Rows[i]["tgt_qty"]);
                                    fid.sale_price = Convert.ToString(ds.Tables[1].Rows[i]["sale_price"]);
                                    fid.tgt_val = Convert.ToString(ds.Tables[1].Rows[i]["tgt_val"]);
                                    fid.actual_sale_q = Convert.ToString(ds.Tables[1].Rows[i]["actual_sale_q"]);
                                    fid.actual_sale_v = Convert.ToString(ds.Tables[1].Rows[i]["actual_sale_v"]);
                                    ArrItem.Add(fid);
                                }
                                _SalesForecast_Model.FC_Item_Details_List = ArrItem;


                                _SalesForecast_Model.footer_PreviousYearSalesInValue = Convert.ToString(ds.Tables[2].Rows[0]["pys_v"]);
                                _SalesForecast_Model.footer_TargetSalesInValue = Convert.ToString(ds.Tables[2].Rows[0]["tgt_val"]);
                                _SalesForecast_Model.footer_ActualSaleInValue = Convert.ToString(ds.Tables[2].Rows[0]["actual_sale_v"]);
                            }
                            ViewBag.SubItemDetails = ds.Tables[6];
                        }
                        //Session.Remove("f_freq");
                        //Session.Remove("f_fy");
                        //Session.Remove("f_period");
                    }
                    //if(Session["err"] !=null)
                    if (_SalesForecast_Model.err != null)
                    {
                        //if (Session["err"].ToString() == "1")
                        if (_SalesForecast_Model.err == "1")
                        {
                            //Session.Remove("dbclickfc");
                            _SalesForecast_Model.dbclickfc = null;
                            //Session.Remove("err");
                            _SalesForecast_Model.err = null;
                            string path = Server.MapPath("~");
                            //Errorlog.LogError(path, ex);
                            return View("~/Views/Shared/Error.cshtml");
                        }
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    _SalesForecast_Model.Title = title;
                    ViewBag.VBRoleList = GetRoleList();
                    //Session["DocumentStatusfc"] = Session["DocumentStatusfc"]==null?"": Session["DocumentStatusfc"];
                    _SalesForecast_Model.DocumentStatusfc = _SalesForecast_Model.DocumentStatusfc == null ? "" : _SalesForecast_Model.DocumentStatusfc;
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesForecast/SalesForecastDetail.cshtml", _SalesForecast_Model);
                }
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public void BindDDLOnPageLoad(SalesForecastModel _SalesForecastModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    string comp_id = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    List<f_frequency> fflist = new List<f_frequency>();
                    f_frequency ffObj0 = new f_frequency();
                    ffObj0.id = "0";
                    ffObj0.name = "---Select---";
                    fflist.Add(ffObj0);
                    f_frequency ffObj = new f_frequency();
                    ffObj.id = "M";
                    ffObj.name = "Monthly";
                    fflist.Add(ffObj);
                    f_frequency stObj1 = new f_frequency();
                    stObj1.id = "Q";
                    stObj1.name = "Quarterly";
                    fflist.Add(stObj1);
                    f_frequency stObj2 = new f_frequency();
                    stObj2.id = "H";
                    stObj2.name = "Half Yearly";
                    fflist.Add(stObj2);
                    f_frequency stObj3 = new f_frequency();
                    stObj3.id = "Y";
                    stObj3.name = "Yearly";
                    fflist.Add(stObj3);
                    _SalesForecastModel.ddl_f_frequencyList = fflist;
                    string f_freq = ""; string StartDate = "";
                    DataSet ds = _SalesForecast_ISERVICES.BindFinancialYear(Convert.ToInt32(comp_id), Convert.ToInt32(br_id), f_freq, StartDate,"");
                    List<financial_year> fyList = new List<financial_year>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        financial_year fyObj1 = new financial_year();
                        fyObj1.id = "0";
                        fyObj1.name = "---Select---";
                        fyList.Add(fyObj1);
                        foreach (DataRow data in ds.Tables[0].Rows)
                        {
                            financial_year fyObj = new financial_year();
                            fyObj.id = data["id"].ToString();
                            fyObj.name = data["name"].ToString();
                            fyList.Add(fyObj);
                        }
                    }
                    _SalesForecastModel.ddl_financial_yearList = fyList;

                    List<period> plist = new List<period>();
                    period pObj = new period();
                    pObj.id = "0";
                    pObj.name = "---Select---";
                    plist.Add(pObj);
                    _SalesForecastModel.ddl_periodList = plist;
                    if(ds.Tables[1].Rows.Count==0)
                    {
                        _SalesForecastModel.ddl_f_frequency = "0";
                    }
                    _SalesForecastModel.ddl_f_frequency = Convert.ToString(ds.Tables[1].Rows[0]["param_stat"]);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public ActionResult BindPeriod(string f_frequency, string financial_year)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    string[] splitFY = financial_year.Split(',');
                    DataSet ds = _SalesForecast_ISERVICES.BindFinancialYear(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), f_frequency, splitFY[0],"");
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        public ActionResult BindDateRange(string f_frequency, string financial_year, string period)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string start_year = "";
                    string end_year = "";
                    int months = 0;
                    string fy_datefrom = "";
                    string fy_dateto = "";
                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    string[] splitPeriod = period.Split(',');
                    if (splitPeriod.Length > 1)
                    {
                        int start_year1 = Convert.ToDateTime(splitPeriod[0]).Year;
                        start_year = Convert.ToString(start_year1);
                        int end_year1 = Convert.ToDateTime(splitPeriod[1]).Year;
                        end_year = Convert.ToString(end_year1);
                        fy_datefrom = splitPeriod[0];
                        fy_dateto = splitPeriod[1];
                    }
                    else
                    {
                        string[] split_fy_year = financial_year.Split(',');
                        int start_year1 = Convert.ToDateTime(split_fy_year[0]).Year;
                        start_year = Convert.ToString(start_year1);
                        int end_year1 = Convert.ToDateTime(split_fy_year[1]).Year;
                        end_year = Convert.ToString(end_year1);
                        months = Convert.ToInt32(period);
                    }
                    string ItmName = "0";
                    DataSet ds = _SalesForecast_ISERVICES.BindDateRangeCal(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), f_frequency, start_year, end_year, months, fy_datefrom, fy_dateto,ItmName, financial_year);
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public ActionResult GetItemNameInDDL(SalesForecastModel _SalesForecastModel)
        {
            JsonResult DataRows = null;
            string itemname = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_SalesForecastModel.item_name))
                    {
                        itemname = "0";
                    }
                    else
                    {
                        itemname = _SalesForecastModel.item_name;
                    }
                    DataSet ds = _SalesForecast_ISERVICES.BindItemNameInDDL(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), itemname);
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetSOItemUOM(string Itm_ID, string fromdate, string todate)
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
                //fromdate = _SalesForecastModel.txtFromDate;
                DateTime fromdate1 = DateTime.ParseExact(fromdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string fromdate2 = fromdate1.ToString("yyyy-MM-dd");

                //todate = _SalesForecastModel.txtToDate;
                DateTime todate1 = DateTime.ParseExact(todate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string todate2 = todate1.ToString("yyyy-MM-dd");

                DataSet result = _SalesForecast_ISERVICES.GetSOItemUOMDL(Comp_ID, Br_ID, Itm_ID, fromdate2, todate2);
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
        public ActionResult SalesForecastDetail()//Detail page
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/SalesForecast/SalesForecastDetail.cshtml", _SalesForecastModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        public ActionResult SalesForecastSave(SalesForecastModel _SalesForecastModel, string command)
        {
            try
            {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (Session["compid"] != null)
                {
                    //Session.Remove("err");
                    _SalesForecastModel.err=null;
                    if (_SalesForecastModel.DeleteCommand == "Delete")
                    {
                        command = "Delete";
                    }
                    switch (command)
                    {
                        case "Edit":
                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                BrchID = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                return RedirectToAction("dbClickEdit", new { sf_id = _SalesForecastModel.sf_id, WF_status = _SalesForecastModel.WFStatus });
                            }
                            /*End to chk Financial year exist or not*/
                            UrlModel _urledit_model = new UrlModel();
                            if (CheckSalesForecastAgainstPP(_SalesForecastModel.ddl_financial_year, _SalesForecastModel.ddl_period, _SalesForecastModel.txtFromDate, _SalesForecastModel.txtToDate) == "UsedInPP")
                            {
                                _SalesForecastModel.Messagefc = "UsedInPP";
                                _SalesForecastModel.Commandfc = "View";
                                _SalesForecastModel.BtnNamefc = "BtnToDetailPage";
                                _SalesForecastModel.TransTypefc = "Update";
                                _SalesForecastModel.dbclickfc = "dbclick";
                                _SalesForecastModel.sf_id = _SalesForecastModel.sf_id;                              
                                _urledit_model.Commandfc = "View";
                                _urledit_model.BtnNamefc = "BtnToDetailPage";
                                _urledit_model.TransTypefc = "Update";
                                _urledit_model.dbclickfc = "dbclick";
                                _urledit_model.sf_id = _SalesForecastModel.sf_id;
                            }
                            else
                            {
                                _SalesForecastModel.Commandfc = "Edit";
                                _SalesForecastModel.BtnNamefc = "BtnAddNew";
                                _SalesForecastModel.TransTypefc = "Update";
                                _SalesForecastModel.dbclickfc = "dbclick";
                                _SalesForecastModel.sf_id = _SalesForecastModel.sf_id;                                
                                _urledit_model.Commandfc = "Edit";
                                _urledit_model.BtnNamefc = "BtnAddNew";
                                _urledit_model.TransTypefc = "Update";
                                _urledit_model.dbclickfc = "dbclick";
                                _urledit_model.sf_id = _SalesForecastModel.sf_id;
                            }
                            TempData["ModelData"] = _SalesForecastModel;
                            return RedirectToAction("AddSalesForecastDetail", _urledit_model);
                        case "AddNew":

                            SalesForecastModel _SalesForecast_AddNewModel = new SalesForecastModel();
                            _SalesForecast_AddNewModel.Commandfc = "Add";
                            _SalesForecast_AddNewModel.TransTypefc = "Save";
                            _SalesForecast_AddNewModel.BtnNamefc = "BtnAddNew";
                            TempData["ModelData"] = _SalesForecast_AddNewModel;
                            UrlModel _urlAddNew_model = new UrlModel();
                            _urlAddNew_model.Commandfc = "Add";
                            _urlAddNew_model.BtnNamefc = "BtnAddNew";
                            _urlAddNew_model.TransTypefc = "Save";
                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                BrchID = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                if (!string.IsNullOrEmpty(_SalesForecastModel.sf_id))
                                    return RedirectToAction("dbClickEdit", new { sf_id = _SalesForecastModel.sf_id, WF_status = _SalesForecastModel.WFStatus });
                                else
                                    _SalesForecast_AddNewModel.Commandfc = "Refresh";
                                _SalesForecast_AddNewModel.TransTypefc = "Refresh";
                                _SalesForecast_AddNewModel.BtnNamefc = "Refresh";
                                _SalesForecast_AddNewModel.DocumentStatusfc = null;
                                TempData["ModelData"] = _SalesForecast_AddNewModel;
                                return RedirectToAction("ScrapSaleInvoiceDetail", "ScrapSaleInvoice");
                            }
                           /*End to chk Financial year exist or not*/
                            return RedirectToAction("AddSalesForecastDetail", _urlAddNew_model);

                        case "Approve":

                            _SalesForecastModel.Commandfc= command;
                            SalesForecast_Approve(_SalesForecastModel);
                            TempData["ModelData"] = _SalesForecastModel;
                            UrlModel _urlapp_model = new UrlModel();
                            if (_SalesForecastModel.WF_Status1 != null)
                            {
                                _urlapp_model.WF_Status1 = _SalesForecastModel.WF_Status1;
                            }
                            _urlapp_model.TransTypefc = _SalesForecastModel.TransTypefc;
                            _urlapp_model.Commandfc = _SalesForecastModel.Commandfc;
                            _urlapp_model.BtnNamefc = _SalesForecastModel.BtnNamefc;
                            _urlapp_model.dbclickfc = _SalesForecastModel.dbclickfc;
                            _urlapp_model.sf_id = _SalesForecastModel.sf_id;
                            return RedirectToAction("AddSalesForecastDetail", _urlapp_model);

                        case "Delete":

                            _SalesForecastModel.Commandfc = command;
                            _SalesForecastModel.TransTypefc = "Delete";
                            SaveSalesForecast(_SalesForecastModel);
                            SalesForecastModel _SalesForecastDelete_Model = new SalesForecastModel();
                            _SalesForecastDelete_Model.Messagefc = "Deleted";
                            _SalesForecastDelete_Model.Commandfc = "Refresh";
                            _SalesForecastDelete_Model.TransTypefc = "Refresh";
                            _SalesForecastDelete_Model.BtnNamefc = "BtnDelete";
                            TempData["ModelData"] = _SalesForecastDelete_Model;
                            UrlModel _urlDeleteModel = new UrlModel();
                            _urlDeleteModel.TransTypefc = _SalesForecastDelete_Model.TransTypefc;
                            _urlDeleteModel.Commandfc = _SalesForecastDelete_Model.Commandfc;
                            _urlDeleteModel.BtnNamefc = _SalesForecastDelete_Model.BtnNamefc;
                            _SalesForecastModel = null;
                            return RedirectToAction("AddSalesForecastDetail", _urlDeleteModel);

                        case "Save":

                            _SalesForecastModel.Commandfc = command;
                            SaveSalesForecast(_SalesForecastModel);
                            if (_SalesForecastModel.Messagefc == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            TempData["ModelData"] = _SalesForecastModel;
                            UrlModel _urlSaveModel = new UrlModel();
                            _urlSaveModel.TransTypefc = _SalesForecastModel.TransTypefc;
                            _urlSaveModel.Commandfc = _SalesForecastModel.Commandfc;
                            _urlSaveModel.BtnNamefc = _SalesForecastModel.BtnNamefc;
                            _urlSaveModel.sf_id = _SalesForecastModel.sf_id;
                            _urlSaveModel.dbclickfc = _SalesForecastModel.dbclickfc;

                            return RedirectToAction("AddSalesForecastDetail", _urlSaveModel);
                        case "Refresh":

                            SalesForecastModel _SalesForecast_DiscardModel = new SalesForecastModel();
                            _SalesForecast_DiscardModel.Commandfc = command;
                            _SalesForecast_DiscardModel.TransTypefc = "Refresh";
                            _SalesForecast_DiscardModel.BtnNamefc = "BtnRefresh";
                            TempData["ModelData"] = _SalesForecast_DiscardModel;
                            UrlModel _urlRefModel = new UrlModel();
                            _urlRefModel.TransTypefc = _SalesForecast_DiscardModel.TransTypefc;
                            _urlRefModel.Commandfc = _SalesForecast_DiscardModel.Commandfc;
                            _urlRefModel.BtnNamefc = _SalesForecast_DiscardModel.BtnNamefc;
                            return RedirectToAction("AddSalesForecastDetail", _urlRefModel);
                        case "BacktoList":

                            SalesForecastModel _SalesForecast_backModel = new SalesForecastModel();
                            _SalesForecast_backModel.WF_Status = _SalesForecastModel.WF_Status1;
                            _SalesForecastModel = null;
                            return RedirectToAction("SalesForecastList", "SalesForecast", _SalesForecast_backModel);
                    }
                }
            }

            catch (Exception ex)
            {
                //Session["err"] = "1";
                _SalesForecastModel.err = "1";
                //Session.Remove("dbclickfc");
                _SalesForecastModel.dbclickfc= null;
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return RedirectToAction("");
        }
        public string CheckSalesForecastAgainstPP( string financial_year, string Period,string FromDate,string ToDate)
        {
            string DataRows = "N";
            try
            {
                //LSODetailModel _LSODetailModelS = new LSODetailModel();
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
                var f_date = FromDate.Split('-');
                var t_date = ToDate.Split('-');
                var fromdt = f_date[2] + '-' + f_date[1] + '-' + f_date[0];
                var todt = t_date[2] + '-' + t_date[1] + '-' + t_date[0];
                DataSet Deatils = _SalesForecast_ISERVICES.CheckSalesForecastAgainstPP(Comp_ID, Br_ID, financial_year, Period, fromdt, todt);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    DataRows = "UsedInPP";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
            return DataRows;
        }
        public ActionResult SaveSalesForecast(SalesForecastModel _SalesForecastModel)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    string command = "";
                    if (_SalesForecastModel.sf_id != null)
                    {
                       command = "Update";
                    }
                    else
                    {
                        command = "Save";
                    }
                    if(_SalesForecastModel.Commandfc == "Approve")
                    {
                        command = _SalesForecastModel.Commandfc;
                    }
                    //string command = Session["TransTypefc"].ToString();
                    if (_SalesForecastModel.CancelFlag == false)
                    {
                        _SalesForecastModel.comp_id = Convert.ToInt32(Session["compid"].ToString());
                        _SalesForecastModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        _SalesForecastModel.CreatedById = Convert.ToInt32(Session["UserId"].ToString());
                        // Int32 create_id = _SalesForecastModel.CreatedById;
                        _SalesForecastModel.mac_id = Session["UserMacaddress"].ToString();
                        string mac_id = _SalesForecastModel.mac_id;
                        if (_SalesForecastModel.DeleteCommand == "Delete")
                        {
                            command = "Delete";
                            _SalesForecastModel.f_status = "";
                        }
                        DataTable dtFCHeader = new DataTable();
                        dtFCHeader.Columns.Add("comp_id", typeof(int));
                        dtFCHeader.Columns.Add("br_id", typeof(int));
                        dtFCHeader.Columns.Add("f_freq", typeof(string));
                        dtFCHeader.Columns.Add("f_fy", typeof(string));
                        dtFCHeader.Columns.Add("f_period", typeof(int));
                        dtFCHeader.Columns.Add("from_date", typeof(string));
                        dtFCHeader.Columns.Add("to_date", typeof(string));
                        dtFCHeader.Columns.Add("create_id", typeof(int));
                        dtFCHeader.Columns.Add("f_status", typeof(string));
                        dtFCHeader.Columns.Add("mac_id", typeof(string));
                        dtFCHeader.Columns.Add("f_tgt_val", typeof(string));
                        dtFCHeader.Columns.Add("TransType", typeof(string));
                        dtFCHeader.Columns.Add("sf_id", typeof(string));
                        DataRow drJCHeader = dtFCHeader.NewRow();

                        drJCHeader["comp_id"] = _SalesForecastModel.comp_id;
                        drJCHeader["br_id"] = _SalesForecastModel.br_id;
                        drJCHeader["f_freq"] = _SalesForecastModel.ddl_f_frequency;
                        drJCHeader["f_fy"] = _SalesForecastModel.ddl_financial_year;
                        drJCHeader["f_period"] = _SalesForecastModel.ddl_period;
                        string dateFrom = _SalesForecastModel.txtFromDate;
                        DateTime dateFrom1 = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        _SalesForecastModel.txtFromDate = dateFrom1.ToString("yyyy-MM-dd");
                        drJCHeader["from_date"] = _SalesForecastModel.txtFromDate;
                        string dateTo = _SalesForecastModel.txtToDate;
                        DateTime dateTo1 = DateTime.ParseExact(dateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        _SalesForecastModel.txtToDate = dateTo1.ToString("yyyy-MM-dd");
                        drJCHeader["to_date"] = _SalesForecastModel.txtToDate;
                        drJCHeader["create_id"] = _SalesForecastModel.CreatedById;

                        if (command == "Update")
                        {
                            if (_SalesForecastModel.CancelFlag == true)
                            {
                                _SalesForecastModel.f_status = "C";
                            }
                            else if (_SalesForecastModel.CancelFlag == false)
                            {
                                //_SalesForecastModel.f_status = Session["f_status"].ToString();
                                _SalesForecastModel.f_status = "D";
                                _SalesForecastModel.f_status = _SalesForecastModel.f_status;
                            }
                            //else if (_SalesForecastModel.CancelFlag == false)
                            //{
                            //    _SalesForecastModel.f_status = "A";
                            //}
                        }
                        if(command == "Save")
                        {
                            _SalesForecastModel.f_status = "D";
                        }
                        if(command== "Approve")
                        {
                            _SalesForecastModel.f_status = "A";
                            
                        }

                        drJCHeader["f_status"] = _SalesForecastModel.f_status;

                        drJCHeader["mac_id"] = _SalesForecastModel.mac_id;
                        

                        DataTable dtFCItemDetail = new DataTable();
                        dtFCItemDetail.Columns.Add("item_id", typeof(string));
                        dtFCItemDetail.Columns.Add("uom_id", typeof(string));
                        dtFCItemDetail.Columns.Add("inc_by", typeof(string));
                        dtFCItemDetail.Columns.Add("red_by", typeof(string));
                        dtFCItemDetail.Columns.Add("tgt_qty", typeof(string));
                        dtFCItemDetail.Columns.Add("sale_price", typeof(string));
                        dtFCItemDetail.Columns.Add("tgt_val", typeof(string));

                        double SumTargetVal = 0;

                        if (_SalesForecastModel.SFCItemdetails !=null)
                        {
                            JArray jObject = JArray.Parse(_SalesForecastModel.SFCItemdetails);
                            for (int i = 0; i < jObject.Count; i++)
                            { 
                                string itmid=Convert.ToString( jObject[i]["item_id"]);
                                if (!string.IsNullOrEmpty(itmid))
                                {
                                    DataRow drAddItemDetail = dtFCItemDetail.NewRow();

                                    drAddItemDetail["item_id"] = jObject[i]["item_id"];
                                    drAddItemDetail["uom_id"] = jObject[i]["uom_id"];
                                    drAddItemDetail["inc_by"] = jObject[i]["inc_by"];
                                    drAddItemDetail["red_by"] = jObject[i]["red_by"];
                                    drAddItemDetail["tgt_qty"] = jObject[i]["tgt_qty"];
                                    drAddItemDetail["sale_price"] = jObject[i]["sale_price"];
                                    drAddItemDetail["tgt_val"] = jObject[i]["tgt_val"];
                                    dtFCItemDetail.Rows.Add(drAddItemDetail);

                                    SumTargetVal = SumTargetVal + Convert.ToDouble(jObject[i]["tgt_val"]);
                                }
                            }
                        }

                        drJCHeader["f_tgt_val"] = SumTargetVal.ToString();
                        drJCHeader["TransType"] = command;// Session["TransTypefc"].ToString();// 
                        drJCHeader["sf_id"] = _SalesForecastModel.sf_id==null? "0": _SalesForecastModel.sf_id;// Session["TransTypefc"].ToString();// 
                        dtFCHeader.Rows.Add(drJCHeader);

                        /*----------------------Sub Item ----------------------*/
                        DataTable dtSubItem = new DataTable();
                        dtSubItem.Columns.Add("item_id", typeof(string));
                        dtSubItem.Columns.Add("sub_item_id", typeof(string));
                        dtSubItem.Columns.Add("qty", typeof(string));
                        if (_SalesForecastModel.SubItemDetailsDt != null)
                        {
                            JArray jObject2 = JArray.Parse(_SalesForecastModel.SubItemDetailsDt);
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



                        string Message = _SalesForecast_ISERVICES.insertFCDetail(dtFCHeader, dtFCItemDetail, dtSubItem);
                        string[] splitmsg = Message.Split('|');
                        if (splitmsg[0].ToString().Trim() == "Data_Not_Found")
                        {
                            ViewBag.MenuPageName = getDocumentName();
                            _SalesForecastModel.Title = title;
                            var msg = splitmsg[0].ToString().Trim().Replace("_", " ") + " " + splitmsg[1].ToString().Trim() + " in " + _SalesForecastModel.Title;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _SalesForecastModel.Messagefc = splitmsg[0].ToString().Trim().Replace("_", "");
                            return RedirectToAction("AddSalesForecastDetail");
                        }
                        if (splitmsg[0].ToString().Trim() == "Update" || splitmsg[0].ToString().Trim() == "Save")
                        {
                            //Session["Messagefc"] = "Save";
                            //Session["Commandfc"] = "EditNew";
                            //Session["TransTypefc"] = "Update";
                            //Session["BtnNamefc"] = "BtnSave";
                            //ViewBag.Message = Session["Messagefc"].ToString();
                            //Session["SaveUpd"] = "AfterSaveUpdate";
                            //Session["br_id"] = Session["BranchId"].ToString();
                            //Session["DocumentStatusfc"] = "c";
                            //Session["dbclickfc"] = "dbclick";
                            //Session.Remove("jc_status");
                            //Session["sf_id"] = splitmsg[4];
                            //Session["f_freq"] = splitmsg[3];
                            //Session["f_fy"] = splitmsg[1];
                            //Session["f_period"] = splitmsg[2];
                            //Session["f_status"] = _SalesForecastModel.f_status;
                            //return RedirectToAction("AddSalesForecastDetail");
                            _SalesForecastModel.Messagefc = "Save";
                            _SalesForecastModel.Commandfc = "EditNew";
                            _SalesForecastModel.TransTypefc = "Update";
                            _SalesForecastModel.BtnNamefc = "BtnSave";
                            _SalesForecastModel.SaveUpd = "AfterSaveUpdate";
                            _SalesForecastModel.DocumentStatusfc = "c";
                            _SalesForecastModel.dbclickfc = "dbclick";
                            _SalesForecastModel.sf_id = splitmsg[4];
                            _SalesForecastModel.f_freq = splitmsg[3];
                            _SalesForecastModel.f_fy = splitmsg[1];
                            _SalesForecastModel.f_period = splitmsg[2];
                            _SalesForecastModel.f_status = _SalesForecastModel.f_status;
                        }
                        else if (splitmsg[0].ToString().Trim() == "Delete")
                        {
                            //Session["Messagefc"] = "Deleted";
                            //Session["Commandfc"] = "Refresh";
                            //Session["TransTypefc"] = "Refresh";
                            //Session["BtnNamefc"] = "BtnDelete";
                            //ViewBag.Message = Session["Messagefc"].ToString();
                            //Session.Remove("dbclickfc");
                            //_SalesForecastModel = null;
                            //return RedirectToAction("AddSalesForecastDetail");
                            SalesForecastModel _SalesForecastDelete_Model = new SalesForecastModel();
                            _SalesForecastDelete_Model.Messagefc= "Deleted";
                            _SalesForecastDelete_Model.Commandfc = "Refresh";
                            _SalesForecastDelete_Model.TransTypefc = "Refresh";
                            _SalesForecastDelete_Model.BtnNamefc = "BtnDelete";
                            TempData["ModelData"] = _SalesForecastDelete_Model;
                        }
                        else if (splitmsg[0].ToString().Trim() == "Approve")
                        {
                            //Session["Messagefc"] = "Approved";
                            //Session["Commandfc"] = "EditNew";
                            ////Session["Commandfc"] = "Approve";
                            //Session["TransTypefc"] = "Approve";
                            //Session["BtnNamefc"] = "BtnSave";
                            ////Session["BtnNamefc"] = "BtnEdit";
                            //ViewBag.Message = Session["Messagefc"].ToString();
                            //Session["SaveUpd"] = "AfterSaveUpdate";
                            //Session["br_id"] = Session["BranchId"].ToString();
                            //Session["f_status"] = _SalesForecastModel.f_status;
                            //// _JobOrderModel.jc_no = splitmsg[1].ToString().Trim();
                            //Session["dbclickfc"] = "dbclick";

                            _SalesForecastModel.Messagefc = "Approved";
                            _SalesForecastModel.Commandfc = "EditNew";
                            _SalesForecastModel.TransTypefc = "Approve";
                            _SalesForecastModel.BtnNamefc = "BtnSave";
                            _SalesForecastModel.SaveUpd = "AfterSaveUpdate";
                            _SalesForecastModel.dbclickfc = "dbclick"; ;
                            _SalesForecastModel.sf_id = splitmsg[4];
                            _SalesForecastModel.f_status = _SalesForecastModel.f_status;
                            TempData["ModelData"] = _SalesForecastModel;
                        }
                        else if (splitmsg[0].ToString().Trim() == "Duplicate")
                        {
                            //Session["Messagefc"] = "Duplicate";
                            //ViewBag.Message = Session["Messagefc"].ToString();
                            _SalesForecastModel.Messagefc= "Duplicate";
                        }
                        else
                        {
                            return RedirectToAction("AddSalesForecastDetail");
                        }
                    }
                    else
                    {
                        string comp_id = Session["CompId"].ToString();
                        string br_id = Session["BranchId"].ToString();
                        string f_freq = _SalesForecastModel.ddl_f_frequency;
                        string f_fy = _SalesForecastModel.ddl_financial_year;
                        string f_period = _SalesForecastModel.ddl_period;
                        Int32 create_id = Convert.ToInt32(Session["UserId"].ToString());// _SalesForecastModel.CreatedById;
                        string Message = _SalesForecast_ISERVICES.cancelFCDetail(Convert.ToInt32(Session["CompId"].ToString()),Convert.ToInt32(Session["BranchId"].ToString()), f_freq, f_fy,f_period,Convert.ToString(create_id), command);
                        string[] splitmsg = Message.Split('|');
                        if (splitmsg[0].ToString().Trim() == "Update")
                        {
                            //Session["Messagefc"] = "Cancel";
                            //Session["Commandfc"] = "EditNew";
                            //Session["TransTypefc"] = "Approve";
                            //Session["BtnNamefc"] = "BtnSave";
                            //ViewBag.Message = Session["Messagefc"].ToString();
                            //Session["SaveUpd"] = "AfterSaveUpdate";
                            //Session["br_id"] = Session["BranchId"].ToString();
                            //Session["f_freq"] = splitmsg[3];
                            //Session["f_fy"] = splitmsg[1];
                            //Session["f_period"] = splitmsg[2];
                            //Session["f_status"] ="C";
                            //Session["dbclickfc"] = "dbclick";
                            _SalesForecastModel.Messagefc = "Cancel";
                            _SalesForecastModel.Commandfc = "EditNew";
                            _SalesForecastModel.TransTypefc = "Approve";
                            _SalesForecastModel.BtnNamefc = "BtnSave";
                            _SalesForecastModel.SaveUpd = "AfterSaveUpdate";
                            _SalesForecastModel.f_status = "C";
                            _SalesForecastModel.dbclickfc = "dbclick"; ;
                            //_SalesForecastModel.sf_id = splitmsg[4];
                            _SalesForecastModel.f_freq = splitmsg[3];
                            _SalesForecastModel.f_fy = splitmsg[1];
                            _SalesForecastModel.f_period = splitmsg[2];
                            _SalesForecastModel.f_status = _SalesForecastModel.f_status;
                            TempData["ModelData"] = _SalesForecastModel;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Session.Remove("dbclickfc");
                _SalesForecastModel.dbclickfc= null;
                //Session["err"] = "1";
                _SalesForecastModel.err = "1";
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
            return RedirectToAction("");
        }

        public ActionResult SalesForecast_Approve(SalesForecastModel _SalesForecastModel)
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
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                //string sfc_No = _SalesForecastModel.Sforca_no;
                string sfc_Date = _SalesForecastModel.CreatedDate;
                string A_Status = _SalesForecastModel.A_Status;
                string A_Level = _SalesForecastModel.A_Level;
                string A_Remarks = _SalesForecastModel.A_Remarks;
                string f_fy = _SalesForecastModel.ddl_financial_year;
                string status = _SalesForecastModel.f_status;
                string period = _SalesForecastModel.ddl_period;
                string sf_id = _SalesForecastModel.sf_id;
                string CreatedBy = Session["UserId"].ToString();

                string Message = _SalesForecast_ISERVICES.Approve_SalesForecast(CompID, BrchID, sf_id/*, sfc_No*/, sfc_Date, A_Status, A_Level, A_Remarks, CreatedBy, mac_id,f_fy,status,period, DocumentMenuId);
                string ApMessage = Message.Split(',')[0].Trim();
                if (ApMessage == "A")
                {
                    //Session["Messagefc"] = "Approved";
                    _SalesForecastModel.Messagefc = "Approved";
                }
                //ViewBag.Message = Session["Messagefc"].ToString();
                //Session["TransTypefc"] = "Update";
                //Session["Commandfc"] = "Approve";
                ////Session["f_status"] = 'A';
                //Session["f_status"] = _SalesForecastModel.f_status;
                //Session["BtnNamefc"] = "BtnEdit";
                //Session["dbclickfc"] = "dbclick";
                //Session["SaveUpd"] = "AfterSaveUpdate";
                _SalesForecastModel.TransTypefc = "Update";
                _SalesForecastModel.Commandfc = "Approve";
                _SalesForecastModel.f_status = _SalesForecastModel.f_status;
                _SalesForecastModel.BtnNamefc = "BtnEdit";
                _SalesForecastModel.dbclickfc = "dbclick";
                _SalesForecastModel.SaveUpd = "AfterSaveUpdate";
                _SalesForecastModel.sf_id = sf_id;
                TempData["ModelData"] = _SalesForecastModel;
                UrlModel _urlapp_model = new UrlModel();
                if(_SalesForecastModel.WF_Status1 != null)
                {
                    _urlapp_model.WF_Status1 = _SalesForecastModel.WF_Status1;
                }
                _urlapp_model.TransTypefc = _SalesForecastModel.TransTypefc;
                _urlapp_model.Commandfc = _SalesForecastModel.Commandfc;
                _urlapp_model.BtnNamefc = _SalesForecastModel.BtnNamefc;
                _urlapp_model.dbclickfc = _SalesForecastModel.dbclickfc;
                _urlapp_model.sf_id = sf_id;
                return RedirectToAction("AddSalesForecastDetail", _urlapp_model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
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
        public ActionResult ErrorPage()
        {
            try
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
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

        public ActionResult ToRefreshByJS(string TrancType)
        {
            //Session["Message"] = "";
            SalesForecastModel _SalesForecastForward_Model = new SalesForecastModel();
            UrlModel _urlModel = new UrlModel();
            var a = TrancType.Split(',');
            _SalesForecastForward_Model.sf_id = a[0].Trim();
            if (a[1].Trim() != null && a[1].Trim() !="")
            {
                _SalesForecastForward_Model.WF_Status1 = a[1].Trim();
                _urlModel.WF_Status1 = a[1].Trim();
            }           
            _SalesForecastForward_Model.dbclickfc = "dbclick";
            _SalesForecastForward_Model.BtnNamefc = "BtnToDetailPage";
            _SalesForecastForward_Model.TransTypefc = "Update";
            TempData["ModelData"] = _SalesForecastForward_Model;          
            _urlModel.sf_id = a[0].Trim();           
            _urlModel.dbclickfc = "dbclick";
            _urlModel.BtnNamefc = "BtnToDetailPage";
            _urlModel.TransTypefc = _SalesForecastForward_Model.TransTypefc;
            return RedirectToAction("AddSalesForecastDetail", _urlModel);
        }

        public ActionResult ApproveDocByWorkFlow(string AppDtList1,string WF_Status1)
        {
            //Session["TransTypefc"] = "Approve";

            SalesForecastModel _SalesForecastModel = new SalesForecastModel();
            if (AppDtList1 != null)
            {
                JArray jObjectBatch1 = JArray.Parse(AppDtList1);
                for (int i = 0; i < jObjectBatch1.Count; i++)
                {
                    
                    _SalesForecastModel.sf_id = jObjectBatch1[i]["sf_id"].ToString();
                    //_SalesForecastModel.Sforca_no = jObjectBatch1[i]["sfc_no"].ToString();
                    _SalesForecastModel.CreatedDate = jObjectBatch1[i]["sfc_dt"].ToString();
                    _SalesForecastModel.f_status = jObjectBatch1[i]["status"].ToString();
                   _SalesForecastModel.ddl_period = jObjectBatch1[i]["period"].ToString();
                    _SalesForecastModel.ddl_financial_year = jObjectBatch1[i]["fyear"].ToString();
                    _SalesForecastModel.A_Status = jObjectBatch1[i]["A_Status"].ToString();
                    _SalesForecastModel.A_Level = jObjectBatch1[i]["A_Level"].ToString();
                    _SalesForecastModel.A_Remarks = jObjectBatch1[i]["A_Remarks"].ToString();
                }
            }
            if (_SalesForecastModel.A_Status != "Approve")
            {
                _SalesForecastModel.A_Status = "Approve";
            }
            _SalesForecastModel.TransType = "Approve";
            SalesForecast_Approve(_SalesForecastModel);
            UrlModel _urlapp_model = new UrlModel();
            if (WF_Status1 != null && WF_Status1 != "")
            {
                _SalesForecastModel.WF_Status1 = WF_Status1;
                _urlapp_model.WF_Status1 = _SalesForecastModel.WF_Status1;
            }
            TempData["ModelData"] = _SalesForecastModel;
           
            _urlapp_model.TransTypefc = _SalesForecastModel.TransTypefc;
            _urlapp_model.Commandfc = _SalesForecastModel.Commandfc;
            _urlapp_model.BtnNamefc = _SalesForecastModel.BtnNamefc;
            _urlapp_model.dbclickfc = _SalesForecastModel.dbclickfc;
            _urlapp_model.sf_id = _SalesForecastModel.sf_id;
            return RedirectToAction("AddSalesForecastDetail", _urlapp_model);
        }
        public ActionResult GetSFRCastDetailDashbrd(string docid, string status)
        {

            //Session["WF_status"] = status;
            SalesForecastModel _SalesForecast_DashModel = new SalesForecastModel();
            _SalesForecast_DashModel.WF_Status = status;
            return RedirectToAction("SalesForecastList", _SalesForecast_DashModel);
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
       , string Flag, string Status, string Doc_no, string Doc_dt, string fromdate,string todate)
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
                                    //dt.Rows[i]["Qty"] = item.GetValue("qty");
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));

                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _SalesForecast_ISERVICES.SF_GetSubItemDetailsAftrApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt).Tables[0];
                    }

                }
                if (Flag == "PYearSalQty")
                {
                    DateTime fromdate1 = DateTime.ParseExact(fromdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string fromdate2 = fromdate1.ToString("yyyy-MM-dd");

                    //todate = _SalesForecastModel.txtToDate;
                    DateTime todate1 = DateTime.ParseExact(todate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string todate2 = todate1.ToString("yyyy-MM-dd");
                    if (Status == "" || Status == "D" || Status == "F" || Status == "A" || Status == "C")
                    {
                        dt = _SalesForecast_ISERVICES.PreviousSalQty_GetSubItemDetails(CompID, BrchID, Item_id, fromdate2, todate2).Tables[0];
                    }
                    
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    //_subitemPageName = "MTO",
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
    }

}