using EnRepMobileWeb.MODELS.BusinessLayer.FinancialYearSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.FinancialYearSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.FinancialYearSetup
{
    public class FinancialYearSetupController : Controller
    {
        string CompID, BrId, userId, language, title = String.Empty;
        string DocumentMenuId = "103160";
        DataTable dtbl = new DataTable();
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        FinancialYearModel _FYModel;
        FinancialYear_IService _FinancialYear_IService;
        public FinancialYearSetupController(Common_IServices _Common_IServices, FinancialYear_IService _FinancialYear_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._FinancialYear_IService = _FinancialYear_IService;
        }
        // GET: BusinessLayer/FinancialYearSetup
        public ActionResult FinancialYearSetup()
        {
            try
            {
                
                CommonPageDetails();
                _FYModel = new FinancialYearModel();              
                _FYModel.fy_list = GetFyList(_FYModel);
                
                _FYModel.title = title;
                return View("~/Areas/BusinessLayer/Views/FinancialYearSetup/FinancialYearSetupList.cshtml", _FYModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        //public ActionResult AddFinancialYearSetupDetail()
        //{
        //    ViewBag.TransType = "abc";
        //    Session["TransType"] = "Save";
        //    Session["BtnName"] = "BtnAddNew";
        //    return RedirectToAction("FinancialYearSetupDetail", "FinancialYearSetup");
        //}
        public ActionResult FinancialYearSetupDetail(string ttype)
        {
            try
            {
                
                CommonPageDetails();
                _FYModel = new FinancialYearModel();
                if (ttype == "new")
                {
                    dtbl = Getpn_fydetail("new");
                    if (dtbl.Rows.Count > 0)
                    {
                        _FYModel.pfy = dtbl.Rows[0]["p_fy"].ToString();
                        _FYModel.pfy_startdt = dtbl.Rows[0]["pfy_sdt"].ToString();
                        _FYModel.pfy_enddt = dtbl.Rows[0]["pfy_edt"].ToString();
                        _FYModel.nfy = dtbl.Rows[0]["n_fy"].ToString();
                        _FYModel.nfy_startdt = dtbl.Rows[0]["nfy_sdt"].ToString();
                        _FYModel.nfy_enddt = dtbl.Rows[0]["nfy_edt"].ToString();
                       // _FYModel.br_id=
                    }
                    _FYModel.transtype = "save";
                    _FYModel.command = "new";
                }
                if (TempData["fydata"] != null)
                {
                    dtbl = Getpn_fydetail("edit");
                    _FYModel = TempData["fydata"] as FinancialYearModel;
                    if (_FYModel.transtype == "update")
                    {
                        string pfy = string.Empty;
                        string nfy = string.Empty;
                        pfy = Convert.ToDateTime(_FYModel.pfy_startdt).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(_FYModel.pfy_enddt).ToString("dd-MM-yyyy");
                        nfy = Convert.ToDateTime(_FYModel.nfy_startdt).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(_FYModel.nfy_enddt).ToString("dd-MM-yyyy");

                        _FYModel.pfy = pfy;
                        _FYModel.pfy_startdt = _FYModel.pfy_startdt;
                        _FYModel.pfy_enddt = _FYModel.pfy_enddt;
                        _FYModel.nfy = nfy;
                        _FYModel.nfy_startdt = _FYModel.nfy_startdt;
                        _FYModel.nfy_enddt = _FYModel.nfy_enddt;
                        _FYModel.br_id = _FYModel.br_id;
                    }
                    if(dtbl.Rows[0]["pfy_sdt"].ToString()== _FYModel.pfy_startdt && dtbl.Rows[0]["pfy_edt"].ToString()== _FYModel.pfy_enddt && dtbl.Rows[0]["nfy_sdt"].ToString()== _FYModel.nfy_startdt && dtbl.Rows[0]["nfy_edt"].ToString() == _FYModel.nfy_enddt)
                    {
                        _FYModel.command = "edit";
                        _FYModel.transtype = "update";
                    }
                    else
                    {
                        _FYModel.command = "new";
                        _FYModel.transtype = "save";
                    }
                  
                    _FYModel.closebook = _FYModel.closebook;
                    
                }


                if(TempData["error_msg"]!=null)
                {
                    DataTable dt_error = TempData["error_msg"] as DataTable;
                    ViewBag.error_msg = dt_error;
                }

                _FYModel.br_list = GetBr_List(_FYModel);
                _FYModel.title = title;
               
                return View("~/Areas/BusinessLayer/Views/FinancialYearSetup/FinancialYearSetupDetail.cshtml", _FYModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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

        private List<FinancialYearList> GetFyList(FinancialYearModel Fy)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable dt = _FinancialYear_IService.GetFY_List(CompID);
                if (dt.Rows.Count > 0)
                {
                    List<FinancialYearList> _fylist = new List<FinancialYearList>();

                    foreach (DataRow row in dt.Rows)
                    {
                        FinancialYearList fy_list = new FinancialYearList();
                        fy_list.sno = row["sno"].ToString();
                        fy_list.fy_year = row["fy"].ToString();
                        fy_list.pfy_sdt = row["pfy_sdt"].ToString();
                        fy_list.pfy_edt = row["pfy_edt"].ToString();
                        fy_list.nfy_sdt = row["nfy_sdt"].ToString();
                        fy_list.nfy_edt = row["nfy_edt"].ToString();
                        fy_list.fy_status = row["fyclose"].ToString();
                        fy_list.book_status = row["bkclose"].ToString();
                        fy_list.bk_close = row["bk_close"].ToString();
                        _fylist.Add(fy_list);
                    }
                    Fy.fy_list = _fylist;
                }
                return Fy.fy_list;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        private List<Br_list> GetBr_List(FinancialYearModel Fy)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    userId = Session["UserId"].ToString();
                }
                DataTable br_list = new DataTable();
                br_list = _Common_IServices.Cmn_GetBrList(CompID, userId);

                DataRow dr = br_list.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                br_list.Rows.InsertAt(dr, 0);

                if (br_list.Rows.Count > 0)
                {
                    List<Br_list> _brlist = new List<Br_list>();
                    foreach (DataRow row in br_list.Rows)
                    {
                        Br_list _br_list = new Br_list();
                        _br_list.br_id = row["Comp_Id"].ToString();
                        _br_list.br_name = row["comp_nm"].ToString();
                        _brlist.Add(_br_list);
                    }
                    Fy.br_list = _brlist;
                }
                return Fy.br_list;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable Getpn_fydetail(string flag)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable dt = _FinancialYear_IService.GetPN_FYdetail(CompID, flag);

                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SaveFy_Closing(FinancialYearModel _FinancialYear, string command)
        {
            try
            {
                switch (command)
                {
                    case "Save":

                        InsertFY_ClosingDetail(_FinancialYear);
                        return RedirectToAction("FinancialYearSetupDetail");

                    case "BacktoList":
                        return RedirectToAction("FinancialYearSetup", "FinancialYearSetup");

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

        public ActionResult InsertFY_ClosingDetail(FinancialYearModel _FY)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }

                DataSet SaveMessage = _FinancialYear_IService.Insert_FyClosingDetail(CompID,_FY.br_id, _FY.pfy_startdt, _FY.pfy_enddt, _FY.nfy_startdt, _FY.nfy_enddt, _FY.closebook, _FY.transtype);

                _FY.transtype = "update";
                _FY.command = "edit";
                TempData["fydata"] = _FY;
                
                TempData["error_msg"] = SaveMessage.Tables[0];
                return RedirectToAction("FinancialYearSetupDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public ActionResult EditFY_Closing(string pfy_sdt, string pfy_edt, string nfy_sdt, string nfy_edt, string bk_close)
        {
            try
            {
                _FYModel = new FinancialYearModel();
                string pfy = string.Empty;
                string nfy = string.Empty;
                bool bkclose = false;
                pfy = Convert.ToDateTime(pfy_sdt).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(pfy_edt).ToString("dd-MM-yyyy");
                nfy = Convert.ToDateTime(nfy_sdt).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(nfy_edt).ToString("dd-MM-yyyy");
                if (bk_close == "N")
                {
                    bkclose = true;
                }
                if (bk_close == "Y")
                {
                    bkclose = false;
                }
                _FYModel.pfy = pfy;
                _FYModel.pfy_startdt = pfy_sdt;
                _FYModel.pfy_enddt = pfy_edt;
                _FYModel.nfy = nfy;
                _FYModel.nfy_startdt = nfy_sdt;
                _FYModel.nfy_enddt = nfy_edt;
                _FYModel.closebook = bkclose;
                _FYModel.transtype = "update";
                _FYModel.command = "edit";


                TempData["fydata"] = _FYModel;
                return (RedirectToAction("FinancialYearSetupDetail"));
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
                    userId = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrId, userId, DocumentMenuId, language);
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
    }
}








































