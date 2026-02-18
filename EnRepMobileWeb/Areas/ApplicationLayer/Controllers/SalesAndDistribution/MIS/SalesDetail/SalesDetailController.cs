
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.SalesDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.SalesDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.MIS.SalesDetail
{
    public class SalesDetailController : Controller
    {
        string DocumentMenuId = "105103190101";
        string CompID, BrID, userid, title, language = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SalesDetail_ISERVICES Sales_ISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public SalesDetailController(Common_IServices _Common_IServices, SalesDetail_ISERVICES Sales_ISERVICES, 
            GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.Sales_ISERVICES = Sales_ISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/SalesDetail
        public ActionResult SalesDetail()
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
                    ViewBag.vb_br_id = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                //Session["SDFilter"] = null;              
                SalesDetail_Model _Model = new SalesDetail_Model();
                _Model.SDFilter = null;
                ViewBag.MenuPageName = getDocumentName();
                DateTime dtnow = DateTime.Now;
                //string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");


                DataSet dttbl = new DataSet();
                #region Added By Nitesh  02-01-2024 for Financial Year 
                #endregion
                dttbl = GetFyList();
                if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
                {
                    _Model.From_dt = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                    ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                    ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                    ViewBag.fylist = dttbl.Tables[1];
                }

                string ToDate = dtnow.ToString("yyyy-MM-dd");
               // _Model.From_dt = FromDate;
                _Model.To_dt = ToDate;

                DataTable br_list = new DataTable();
                br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
                ViewBag.br_list = br_list;

                _Model.categoryLists = custCategoryList();
                _Model.portFolioLists = custPortFolioLists();
                _Model.custzoneList = CustZoneLists();
                _Model.custgroupList = CustGroupLists();
                _Model.regionLists = regionLists();
                List<CityList> _CityList = new List<CityList>();
                _Model.CityLists = _CityList;

                List<StateList> _StateList = new List<StateList>();
                _Model.StateLists = _StateList;
                GetAutoCompleteSearchHSN(_Model);
                _Model.Title = title;
                ViewBag.flag = "Summary";
                //ViewBag.SalesDetailList = GetSales_Details("","", "", "0","", "", "", "","","","", FromDate, ToDate, "Summary");
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/SalesDetail/SalesDetail.cshtml", _Model);

            }
           
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
           
        }
      
        private DataSet GetFyList()
        {
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
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
                DataSet dt = _GeneralLedger_ISERVICE.Get_FYList(Comp_ID, Br_Id);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult showReportFromDashBoard(string salesby,string FromDt,string ToDt)
        {
            try
            {
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                    ViewBag.vb_br_id = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                //Session["SDFilter"] = null;
                SalesDetail_Model _Model = new SalesDetail_Model();
                _Model.SDFilter = null;
                ViewBag.MenuPageName = getDocumentName();
                DateTime dtnow = DateTime.Now;
                //string FromDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                //string ToDate = dtnow.ToString("yyyy-MM-dd");
                _Model.From_dt = FromDt;
                _Model.To_dt = ToDt;
                DataTable br_list = new DataTable();
                br_list = _Common_IServices.Cmn_GetBrList(CompID, userid);
                ViewBag.br_list = br_list;
                _Model.categoryLists = custCategoryList();
                _Model.portFolioLists = custPortFolioLists();
                _Model.regionLists = regionLists();
                _Model.custzoneList = CustZoneLists();
                _Model.custgroupList = CustGroupLists();

                List<CityList> _CityList = new List<CityList>();
                _Model.CityLists = _CityList;

                List<StateList> _StateList = new List<StateList>();
                _Model.StateLists = _StateList;

                GetAutoCompleteSearchHSN(_Model);
                _Model.Sales_by = salesby;
                _Model.Title = title;
                //ViewBag.SalesDetailList = GetSales_Details("", "", "", "0", "", "", "", "", "", "", "", FromDt, ToDt, "Summary");
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/MIS/SalesDetail/SalesDetail.cshtml", _Model);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private List<RegionList> regionLists()
        {
            List<RegionList> regionLists = new List<RegionList>();
            DataTable dt = GetRegion();
            foreach (DataRow dr in dt.Rows)
            {
                RegionList list = new RegionList();
                list.region_id = dr["setup_id"].ToString();
                list.region_val = dr["setup_val"].ToString();
                regionLists.Add(list);
            }
           //regionLists.Insert(0, new RegionList() { region_id = "0", region_val = "---All---" });
            return regionLists;
        }
        private List<CustCategoryList> custCategoryList()
        {
            List<CustCategoryList> lists = new List<CustCategoryList>();
            DataTable dt = GetCustomerCategory();
            foreach (DataRow dr in dt.Rows)
            {
                CustCategoryList list = new CustCategoryList();
                list.Cat_id = dr["setup_id"].ToString();
                list.Cat_val = dr["setup_val"].ToString();
                lists.Add(list);
            }
           // lists.Insert(0, new CustCategoryList() { Cat_id = "0", Cat_val = "---All---" });
            return lists;
        }
        private List<CustPortFolioList> custPortFolioLists()
        {
            List<CustPortFolioList> portFolioLists = new List<CustPortFolioList>();
            DataTable dt1 = GetCustomerPortfolio();
            foreach (DataRow dr in dt1.Rows)
            {
                CustPortFolioList custPortFolio = new CustPortFolioList();
                custPortFolio.CatPort_id = dr["setup_id"].ToString();
                custPortFolio.CatPort_val = dr["setup_val"].ToString();
                portFolioLists.Add(custPortFolio);
            }
           // portFolioLists.Insert(0, new CustPortFolioList() { CatPort_id = "0", CatPort_val = "---All---" });
            return portFolioLists;
        }
        public DataTable GetRegion()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = Sales_ISERVICES.GetRegionDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetAutoCompleteSearchHSN(SalesDetail_Model _Model)
        {
            string HSNCode = string.Empty;
            Dictionary<string, string> HSNList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_Model.ddlhsncode))
                {
                    HSNCode = "0";
                }
                else
                {
                    HSNCode = _Model.ddlhsncode;
                }
                HSNList = Sales_ISERVICES.ItemSetupHSNDAL(Comp_ID, HSNCode);

                List<HSNno> _HSNList = new List<HSNno>();
                foreach (var data in HSNList)
                {
                    HSNno _HsnDetail = new HSNno();
                    _HsnDetail.setup_id = data.Key;
                    _HsnDetail.setup_val = data.Value;
                    _HSNList.Add(_HsnDetail);
                }
                _Model.HSNList = _HSNList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(HSNList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public DataTable GetCustomerCategory()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = Sales_ISERVICES.GetcategoryDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetCustomerPortfolio()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = Sales_ISERVICES.GetCustportDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable GetSales_Details(string cust_id,string reg_name, string sale_type,string curr_id, string productGrp,string Product_Id,string productPort,
            string custCat,string CustPort,string inv_no,string inv_dt, string sale_per, string From_dt, string To_dt, string Flag, string HSN_code,string cust_zone,string cust_group,string state,string city,string brlist,string uom_id)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                    userid = Session["UserId"].ToString();

                ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, userid, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag, HSN_code, cust_zone, cust_group, state, city, brlist, uom_id);
                dt = ds.Tables[0];
                
                if (Flag == "Summary")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if(Flag == "Detail")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailListFooter = ds.Tables[1];
                    ViewBag.GstHeaderColumns = ds.Tables[2];/* Added by Suraj Maurya on 24-09-2025 */
                    //dt = ds.Tables[0];
                }
                else if (Flag == "SDInterBrchWiseSummary") /*--add by Hina Sharma on 27-06-2025---*/
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailIntBrchListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if (Flag == "SDInterBrchWiseDetail")/*add by Hina Sharma on 27-06-2025*/
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailIntBrchListFooter = ds.Tables[1];
                    ViewBag.GstHeaderColumns = ds.Tables[2];/* Added by Suraj Maurya on 24-09-2025 */
                    //dt = ds.Tables[0];
                }
                else if(Flag== "SDCostomerWiseSummary")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailCostomerWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if (Flag == "SDCostomerWiseDetail")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailCostomerWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if(Flag== "SDProductWiseSummary")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailProductWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if (Flag == "SDProductWiseDetail")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailProductWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if(Flag== "SDProductGroupWiseSummary")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailProductGroupWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if(Flag== "SDProductGroupWiseDetail")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailProductGroupWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if(Flag== "SDRegionWiseSummary")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailRegionWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if(Flag == "SDRegionWiseDetail")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailRegionWiseListFooter = ds.Tables[1];
                    //dt = ds.Tables[0];
                }
                else if(Flag== "SDSalePerWiseSummary")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailSalePerWiseListFooter = ds.Tables[1];
                    ViewBag.Saletype = sale_type;
                    //dt = ds.Tables[0];
                }
                else if (Flag == "SDSalePerWiseDetail")
                {
                    //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                    ViewBag.SalesDetailSalePerWiseListFooter = ds.Tables[1];
                    ViewBag.Saletype = sale_type;
                    //dt = ds.Tables[0];
                }
                else if (Flag == "ItemDetails"|| Flag == "ItemDetailsInterBrch" /*add ItemDetailsInterBrch by Hina Sharma on 27-06-2025*/)
                {
                    ViewBag.TotalItemDetails = ds.Tables[1];
                }
                else if(Flag== "CW_Inv_Details")
                {
                    ViewBag.TotalCW_Inv_Details = ds.Tables[1];
                }
                else if(Flag== "SalePersonWiseCW_Inv_Details")
                {
                    ViewBag.TotalCW_Inv_Details = ds.Tables[1];
                }
                else if(Flag== "CW_Inv_Item_Details")
                {
                    ViewBag.TotalCW_Inv_Item_Details = ds.Tables[1];
                }
                else if(Flag== "SDProductWiseInvoce")
                {
                    ViewBag.TotalSDProductWiseInvoce = ds.Tables[1];
                }
                else if(Flag== "SDProductWise")
                {
                    ViewBag.TotalSDProductWise = ds.Tables[1];
                }
                else if(Flag== "SDCostomerWise")
                {
                    ViewBag.TotalSDCostomerWise = ds.Tables[1];
                }
                else if (Flag == "SalePersonWiseSDCostomerWise")
                {
                    ViewBag.TotalSDCostomerWise = ds.Tables[1];
                }

                //{
                //    ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name, sale_type, curr_id, productGrp, Product_Id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag);
                //    if(Flag== "ItemDetails")
                //    {
                //        ViewBag.TotalItemDetails = ds.Tables[1];
                //    }
                //    dt = ds.Tables[0];
                //}
                //dt = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name,sale_type,curr_id, productGrp, Product_Id, productPort,custCat,CustPort, inv_no,inv_dt,sale_per, From_dt, To_dt, Flag).Tables[0];
                //ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, cust_id, reg_name,sale_type,curr_id, productGrp, Product_Id, productPort,custCat,CustPort, inv_no,inv_dt,sale_per, From_dt, To_dt, Flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetAutoCompleteSearchCustList(string Cust_Typ)
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string CustType = string.Empty;
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
                if(Cust_Typ=="B")
                {
                    CustType = "B";
                }
                else
                {
                    CustType = "";
                }
                
                DataSet dt = Sales_ISERVICES.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        CustList.Add(dt.Tables[0].Rows[i]["cust_id"].ToString(), dt.Tables[0].Rows[i]["cust_name"].ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        /*----------------------------------Invoice Wise Sales Details---------------------------------*/

        public ActionResult GetSales_DetailsByFilter(string cust_id, string reg_name, string sale_type, string product_id, string sale_per, string From_dt, 
            string To_dt,string ShowAs,string cust_zone, string cust_group,string custCat, string custPort,string state,string city,string brlist)
        {
            try
            {
                var flag = "Summary";
                SalesDetail_Model _SalesDetail_Model = new SalesDetail_Model();
                if (ShowAs == "S")
                {
                    flag = "Summary";
                }
                else if(ShowAs == "D")
                {
                    flag = "Detail";
                }
                ViewBag.flag = flag;
                ViewBag.SalesDetailList = GetSales_Details(cust_id, reg_name, sale_type,"", product_id, "", "", custCat, custPort, "", "", sale_per, From_dt, To_dt, flag,null, cust_zone, cust_group,state,city, brlist,"");
                _SalesDetail_Model.SDFilter = "SDFilter";                
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsInvoiceWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        public ActionResult GetSales_Item_Details(string inv_no, string inv_dt, string curr_id,  string From_dt, string To_dt, string Flag, string brlist)/*Add Sale type by Hina on 17-09-2024 for service sale invoice */
        {
            try
            {
                ViewBag.SalesItemDetailList = GetSales_Details("","", "", curr_id, "0","","","","", inv_no, inv_dt, "", From_dt, To_dt, Flag, null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        /*----------------------------------Invoice Wise Sales Details End---------------------------------*/

        /*----------------------------------Inter Branch Wise Sales Details  add by Hina Sharma on 27-06-2025--------------------------------*/
        public ActionResult GetSales_DetailsInterBrchWiseByFilter(string cust_id, string reg_name, string sale_type, string product_id, string sale_per, string From_dt, string To_dt, string ShowAs,string brlist)
        {
            try
            {
                var flag = "SDInterBrchWiseSummary";
                if (ShowAs == "S")
                {
                    flag = "SDInterBrchWiseSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "SDInterBrchWiseDetail";
                }
                ViewBag.flag = flag;
                SalesDetail_Model _SalesDetail_Model = new SalesDetail_Model();
                ViewBag.SalesDetailInterBrchList = GetSales_Details(cust_id, reg_name, sale_type, "", product_id, "", "", "", "", "", "", sale_per, From_dt, To_dt, flag, null, "", "", "", "", brlist,"");
                _SalesDetail_Model.SDFilter = "SDFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsInterBranchWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        /*----------------------------------Inter Branch Wise Sales Details End---------------------------------*/

        /*----------------------------------Customer Wise Sales Details---------------------------------*/
        public ActionResult GetSales_DetailsCostomerWiseByFilter(string cust_id, string reg_name, string sale_type,string curr_id, string custCat, string custPort, string From_dt, string To_dt, string ShowAs,string cust_zone, string cust_group,string state, string city,string brlist)
        {
            try
            {
                var flag = "SDCostomerWiseSummary";
                if (ShowAs == "S")
                {
                    flag = "SDCostomerWiseSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "SDCostomerWiseDetail";
                }
                ViewBag.flag = flag;
                SalesDetail_Model _SalesDetail_Model = new SalesDetail_Model();
                ViewBag.SalesDetailCostomerWiseList = GetSales_Details(cust_id, reg_name, sale_type,curr_id, "", "", "", custCat, custPort, "", "", "", From_dt, To_dt, flag,null, cust_zone, cust_group, state, city, brlist,"");
                //Session["SDFilter"] = "SDFilter";
                _SalesDetail_Model.SDFilter = "SDFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsCustomerWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetCW_Sales_Invoice_Details(string Cust_Id, string curr_id, string From_dt, string To_dt,string brlist/*,string sale_type*/)/*Add Sale type by Hina on 17-09-2024 for service sale invoice */
        {
            try
            {
                ViewBag.CWInvoiceDetailList = GetSales_Details(Cust_Id, "", "", curr_id, "0","", "", "", "","","","", From_dt, To_dt, "CW_Inv_Details",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialCustomerInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetCW_Sales_Invoice_Item_Details(string Cust_Id, string inv_no, string inv_dt, string curr_id, string From_dt, string To_dt, string brlist)
        {
            try
            {
                ViewBag.CWInvoiceItemDetailList = GetSales_Details(Cust_Id, "", "",curr_id, "0", "", "", "", "", inv_no, inv_dt, "", From_dt, To_dt, "CW_Inv_Item_Details",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/CustomerInvoiceProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        /*----------------------------------Customer Wise Sales Details End---------------------------------*/

        /*----------------------------------Product Wise Sales Details---------------------------------*/
        public ActionResult GetSales_DetailsProductWiseByFilter(string sale_type,string curr_id, string productgrp, string productPort, string From_dt, string To_dt,string ShowAs, string HSN_code,string brlist)
        {
            try
            {
                var flag = "SDProductWiseSummary";
                if (ShowAs == "S")
                {
                    flag = "SDProductWiseSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "SDProductWiseDetail";
                }
                ViewBag.flag = flag;
                SalesDetail_Model _SalesDetail_Model = new SalesDetail_Model();
                ViewBag.SalesDetailProductWiseList = GetSales_Details(sale_type, "", sale_type,curr_id, productgrp, "", productPort, "", "", "", "", "", From_dt, To_dt, flag, HSN_code,"","", "", "", brlist,"");
                //TempData["SDFilter"] = "SDFilter";
                _SalesDetail_Model.SDFilter = "SDFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsProductWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetPW_Sales_Invoice_Details(string Product_Id, string sale_type,string curr_id, string productgrp, string productPort,
        string From_dt, string To_dt,string brlist)
        {
            try
            {
                ViewBag.SalesDetailProductWiseInvoiceList = GetSales_Details("", "", sale_type,curr_id, productgrp, Product_Id, productPort, "", "", "", "", "", From_dt, To_dt, "SDProductWiseInvoce",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProductWiseInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        /*----------------------------------Product Wise Sales Details End---------------------------------*/

        /*----------------------------------Product Group Wise Sales Details---------------------------------*/
        public ActionResult GetSales_DetailsProductGroupWiseByFilter(string sale_type,string curr_id, string productgrp,string productPort, string From_dt, string To_dt,string ShowAs,string brlist)
        {
            try
            {
                var flag = "SDProductGroupWiseSummary";
                if (ShowAs == "S")
                {
                    flag = "SDProductGroupWiseSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "SDProductGroupWiseDetail";
                }
                ViewBag.flag = flag;
                SalesDetail_Model _SalesDetail_Model = new SalesDetail_Model();
                ViewBag.SalesDetailProductGroupWiseList = GetSales_Details(sale_type, "", sale_type,curr_id, productgrp, "", productPort, "", "", "", "", "", From_dt, To_dt, flag,null, "", "", "", "", brlist,"");
                //TempData["SDFilter"] = "SDFilter";
                _SalesDetail_Model.SDFilter = "SDFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsProductGroupWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetPGW_Sales_Invoice_product_Details(string sale_type,string curr_id, string productgrp, string productPort,
     string From_dt, string To_dt,string brlist,string uom_id)
        {
            try
            {
                ViewBag.SalesDetailProductGrpWisePrdctList = GetSales_Details("", "", sale_type,curr_id, productgrp, "", productPort, "", "", "", "", "", From_dt, To_dt, "SDProductWise",null, "", "", "", "", brlist, uom_id);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProductGroupWiseProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }
        public ActionResult GetPGW_Sales_Invoice_Details(string Product_Id, string sale_type,string curr_id, string productgrp, string productPort,
      string From_dt, string To_dt, string brlist, string uom_id)
        {
            try
            {
                ViewBag.SalesDetailProductWiseInvoiceList = GetSales_Details("", "", sale_type,curr_id, productgrp, Product_Id, productPort, "", "", "", "", "", From_dt, To_dt, "SDProductWiseInvoce",null, "", "", "", "", brlist, uom_id);
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProductGroupWiseInvoiceDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        /*----------------------------------Product Group Wise Sales Details End---------------------------------*/

        /*---------------------------------- Region Wise Sales Details ------------------------------------------ */
        public ActionResult GetSales_DetailsRegionWiseByFilter(string sale_type,string curr_id, string RegionName,string From_dt, string To_dt,string ShowAs,string brlist)
        {
            try
            {
                var flag = "SDRegionWiseSummary";
                if (ShowAs == "S")
                {
                    flag = "SDRegionWiseSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "SDRegionWiseDetail";
                }
                ViewBag.flag = flag;
                ViewBag.SalesDetailRegionWiseList = GetSales_Details(sale_type, RegionName, sale_type,curr_id, "", "", "", "", "", "", "", "", From_dt, To_dt, flag,null, "", "", "", "", brlist,"");
                //TempData["SDFilter"] = "SDFilter";
                SalesDetail_Model _SalesDetail_Model = new SalesDetail_Model();
                _SalesDetail_Model.SDFilter = "SDFilter";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsRegionWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetRegionWise_Customer_Details(string sale_type,string curr_id, string Region_Id, string productPort, string From_dt, string To_dt,string brlist)
        {
            try
            {
                ViewBag.SalesDetailRegionWiseCustomerList = GetSales_Details("", Region_Id, sale_type,curr_id, "0", "", productPort, "", "", "", "", "", From_dt, To_dt, "SDCostomerWise",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialRegionWiseCustomerDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetRegionWise_Sales_Invoice_Details(string Cust_Id, string Region_Id, string sale_type,string curr_id, string From_dt, string To_dt,string brlist)
        {
            try
            {
                ViewBag.RWInvoiceDetailList = GetSales_Details(Cust_Id, Region_Id, sale_type,curr_id, "0", "", "", "", "", "", "", "", From_dt, To_dt, "CW_Inv_Details",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialRegionWiseInvoiceSummary.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        public ActionResult GetRegionWise_Sales_Invoice_Product_Details(string Cust_Id, string Region_Id, string sale_type,string curr_id,
          string invoice_no, string invoice_dt, string From_dt, string To_dt,string brlist)
        {
            try
            {
                ViewBag.RWInvoiceItemDetailList = GetSales_Details(Cust_Id, Region_Id, sale_type,curr_id, "0", "", "", "", "", invoice_no, invoice_dt, "", From_dt, To_dt, "CW_Inv_Item_Details",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialRegionWiseInvoiceProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }


        /*---------------------------------- Region Wise Sales Details End------------------------------------------ */


        /*---------------------------------- Sales Representative Wise Sales Details End------------------------------------------ */
        public ActionResult GetSales_DetailsSalePersonWiseByFilter(string sale_type,string curr_id,string sale_per,string From_dt, string To_dt,string ShowAs,string brlist)
        {
            try
            {
                var flag = "SDSalePerWiseSummary";
                if (ShowAs == "S")
                {
                    flag = "SDSalePerWiseSummary";
                }
                else if (ShowAs == "D")
                {
                    flag = "SDSalePerWiseDetail";
                }
                ViewBag.flag = flag;
                ViewBag.SalesDetailSalePerWiseList = GetSales_Details("", "", sale_type,curr_id, "", "", "", "", "", "", "", sale_per, From_dt, To_dt, flag,null, "", "", "", "", brlist,"");
                //TempData["SDFilter"] = "SDFilter";               
                SalesDetail_Model _SalesDetail_Model = new SalesDetail_Model();
                _SalesDetail_Model.SDFilter = "SDFilter";
                ViewBag.Saletype = sale_type;
                ViewBag.ShowAs = ShowAs;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsSalesExecutiveWiseList.cshtml", _SalesDetail_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetSalePersonWise_Customer_Details(string sale_type,string curr_id, string sale_per, string From_dt, string To_dt,string brlist)
        {
            try
            {
                ViewBag.SalesDetailSalePersonWiseCustomerList = GetSales_Details("", "", sale_type,curr_id, "0", "", "", "", "", "", "", sale_per, From_dt, To_dt, "SalePersonWiseSDCostomerWise",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesExecutiveWiseCustomerDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetSalePersonWise_Customer_Invoice_Details(string sale_type,string curr_id, string sale_per, string cust_id, string From_dt, string To_dt,string brlist)
        {
            try
            {
                ViewBag.SalesDetailSalePersonWiseCustomerInvoiceList = GetSales_Details(cust_id, "", sale_type,curr_id, "0", "", "", "", "", "", "", sale_per, From_dt, To_dt, "SalePersonWiseCW_Inv_Details",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesExecutiveWiseInvoiceSummary.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetSalePersonWise_Customer_Invoice_Item_Details(string inv_no, string inv_dt,string curr_id, string From_dt, string To_dt,string brlist)
        {
            try
            {
                ViewBag.SalesDetailSalePersonWiseCustomerInvoiceItemList = GetSales_Details("", "", "",curr_id, "0", "", "", "", "", inv_no, inv_dt, "", From_dt, To_dt, "CW_Inv_Item_Details",null, "", "", "", "", brlist,"");
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesExecutiveWiseProductDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        /*---------------------------------- Sales Representative Wise Sales Details End------------------------------------------ */


        public ActionResult GetSales_DetailsCostomerWise()
        {
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMIS_SalesDetailsCustomerWiseList.cshtml");
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
        public FileResult SalesDetailExporttoExcelDt(string cust_id, string reg_name,string DataShow, 
            string sale_type, string product_id, string sale_per, string From_dt, string To_dt, string ShowAs
            ,string curr_id,string productGrp,string productPort,string custCat,string CustPort, string inv_no,string inv_dt,string HSN_code)
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
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                var Flag = "";
                if(DataShow== "InoviceWise")
                {
                    if (ShowAs == "S")
                    {
                        Flag = "Summary";
                    }
                    else if (ShowAs == "D")
                    {
                        Flag = "Detail";
                    }
                }
                else if (DataShow == "InterBrchWise")/*--add by Hina Sharma on 27-06-2025---*/
                {
                    if (ShowAs == "S")
                    {
                        Flag = "SDInterBrchWiseSummary";
                    }
                    else if (ShowAs == "D")
                    {
                        Flag = "SDInterBrchWiseDetail";
                    }
                }
                else if (DataShow == "CustomerWise")
                {
                    if (ShowAs == "S")
                    {
                        Flag = "SDCostomerWiseSummary";
                    }
                    else if (ShowAs == "D")
                    {
                        Flag = "SDCostomerWiseDetail";
                    }
                }
                else if (DataShow == "ProductWise")
                {
                    if (ShowAs == "S")
                    {
                        Flag = "SDProductWiseSummary";
                    }
                    else if (ShowAs == "D")
                    {
                        Flag = "SDProductWiseDetail";
                    }
                }
                else if (DataShow == "ProductGroupWise")
                {
                    if (ShowAs == "S")
                    {
                        Flag = "SDProductGroupWiseSummary";
                    }
                    else if (ShowAs == "D")
                    {
                        Flag = "SDProductGroupWiseDetail";
                    }
                }
                else if (DataShow == "RegionWise")
                {
                    if (ShowAs == "S")
                    {
                        Flag = "SDRegionWiseSummary";
                    }
                    else if (ShowAs == "D")
                    {
                        Flag = "SDRegionWiseDetail";
                    }
                }
                else if (DataShow == "SalesExecutiveWise")
                {
                    if (ShowAs == "S")
                    {
                        Flag = "SDSalePerWiseSummary";
                    }
                    else if (ShowAs == "D")
                    {
                        Flag = "SDSalePerWiseDetail";
                    }
                }
                else if (DataShow == "ItemDetails")
                {
                    Flag = "ItemDetails";
                }
                else if (DataShow == "ItemDetailsInterBrch")/*--add by Hina Sharma on 27-06-2025---*/
                {
                    Flag = "ItemDetailsInterBrch";
                }
                else if (DataShow == "CW_Inv_Details")
                {
                    Flag = "CW_Inv_Details";
                } 
                else if (DataShow == "SalePersonWiseCW_Inv_Details")
                {
                    Flag = "SalePersonWiseCW_Inv_Details";
                }
                else if (DataShow == "CW_Inv_Item_Details")
                {
                    Flag = "CW_Inv_Item_Details";
                }
                else if (DataShow == "SDProductWiseInvoce")
                {
                    Flag = "SDProductWiseInvoce";
                }
                else if (DataShow == "SDProductWise")
                {
                    Flag = "SDProductWise";
                }
                else if (DataShow == "SDCostomerWise")
                {
                    Flag = "SDCostomerWise";
                }
                else if (DataShow == "SalePersonWiseSDCostomerWise")
                {
                    Flag = "SalePersonWiseSDCostomerWise";
                }
                if (Session["UserId"] != null)
                    userid = Session["UserId"].ToString();
                ds = Sales_ISERVICES.GetSales_Detail(CompID, BrID, userid, cust_id, reg_name, sale_type, curr_id, productGrp, product_id, productPort, custCat, CustPort, inv_no, inv_dt, sale_per, From_dt, To_dt, Flag, HSN_code,"","","","","","");
                if (DataShow == "InoviceWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Invoice Number", typeof(string));
                    dt.Columns.Add("Invoice Date", typeof(string));
                    dt.Columns.Add("Customer Name", typeof(string));
                    dt.Columns.Add("GST Number", typeof(string));
                    dt.Columns.Add("Region Name", typeof(string));
                    dt.Columns.Add("Sale Type", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Sales Representative", typeof(string));
                    dt.Columns.Add("Customer Ref No. & Date", typeof(string));
                    
                    if (Flag == "Summary")
                    {
                        dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                        dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    }
                    if (Flag == "Detail")
                    {
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("HSN/SAC Code", typeof(string));
                        dt.Columns.Add("Sale Quantity", typeof(decimal));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                        dt.Columns.Add("Value in Specific(Before Discount)", typeof(decimal));
                        dt.Columns.Add("Value in Base(Before Discount)", typeof(decimal));
                        dt.Columns.Add("Discount Value(In Specific)", typeof(decimal));
                        dt.Columns.Add("Discount Value (In Base)", typeof(decimal));
                        dt.Columns.Add("Value in Specific(After Discount)", typeof(decimal));
                        dt.Columns.Add("Value in Base(After Discount)", typeof(decimal));
                        dt.Columns.Add("IGST", typeof(decimal));
                        dt.Columns.Add("CGST", typeof(decimal));
                        dt.Columns.Add("SGST", typeof(decimal));
                        dt.Columns.Add("Tax Amount", typeof(decimal));
                    }
                    //dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    //dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    //dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (Flag == "Summary")
                    {
                        dt.Columns.Add("Paid Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Paid Amount (In Base)", typeof(decimal));
                        dt.Columns.Add("Remaining Balance (In Specific)", typeof(decimal));
                        dt.Columns.Add("Remaining Balance (In Base)", typeof(decimal));
                        dt.Columns.Add("Payment Terms", typeof(int));
                        dt.Columns.Add("Due Date", typeof(string));
                        dt.Columns.Add("Overdue Days", typeof(int));
                        dt.Columns.Add("Approved On", typeof(string));
                    }
                    if (Flag == "Detail")
                    {
                        dt.Columns.Add("Remarks", typeof(string));
                    }

                        if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Invoice Number"] = dr["app_inv_no"].ToString();
                            dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                            dtrowLines["Customer Name"] = dr["cust_name"].ToString(); 
                            dtrowLines["GST Number"] = dr["cust_gst_no"].ToString();
                            dtrowLines["Region Name"] = dr["cust_region"].ToString();
                            dtrowLines["Sale Type"] = dr["sale_type"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Sales Representative"] = dr["sale_per"].ToString(); 
                            dtrowLines["Customer Ref No. & Date"] = dr["ref_doc_No_Dt"].ToString();
                            
                            if (Flag == "Detail")
                            {
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["HSN/SAC Code"] = dr["HSN_code"].ToString();
                                dtrowLines["Sale Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();

                                dtrowLines["Value in Specific(Before Discount)"] = dr["ValBeforeDiscInSpec"].ToString();
                                dtrowLines["Value in Base(Before Discount)"] = dr["ValBeforeDiscInBs"].ToString(); 
                                dtrowLines["Discount Value(In Specific)"] = dr["disc_val_spec"].ToString();
                                dtrowLines["Discount Value (In Base)"] = dr["disc_val_bs"].ToString();
                                dtrowLines["Value in Specific(After Discount)"] = dr["ValAfterDiscInSpec"].ToString();
                                dtrowLines["Value in Base(After Discount)"] = dr["ValAfterDiscInBs"].ToString();
                                dtrowLines["IGST"] = dr["igst"].ToString();
                                dtrowLines["CGST"] = dr["cgst"].ToString();
                                dtrowLines["SGST"] = dr["sgst"].ToString();
                                //dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                                //dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                                //dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount"] = dr["item_tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                                dtrowLines["Remarks"] = dr["it_remarks"].ToString();
                            }
                            else
                            {
                                dtrowLines["Sale Amount (In Specific)"] = dr["sale_amount_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["sale_amount_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();

                                dtrowLines["Paid Amount (In Specific)"] = dr["paid_amt_spec"].ToString();
                                dtrowLines["Paid Amount (In Base)"] = dr["paid_amt_bs"].ToString();
                                dtrowLines["Remaining Balance (In Specific)"] = dr["pend_amt_spec"].ToString();
                                dtrowLines["Remaining Balance (In Base)"] = dr["pend_amt_bs"].ToString();
                                dtrowLines["Payment Terms"] = dr["payment_term"].ToString();
                                dtrowLines["Due Date"] = dr["due_date"].ToString();
                                dtrowLines["Overdue Days"] = dr["overdue_days"].ToString();
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
               else if (DataShow == "InterBrchWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Customer Name", typeof(string));
                    dt.Columns.Add("GST Number", typeof(string));
                    dt.Columns.Add("Region Name", typeof(string));
                    dt.Columns.Add("Sale Type", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Sales Representative", typeof(string));
                    dt.Columns.Add("Customer RefNoDate", typeof(string));
                    dt.Columns.Add("Invoice Number", typeof(string));
                    dt.Columns.Add("Invoice Date", typeof(string));
                    if (Flag == "SDInterBrchWiseSummary")
                    {
                        dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                        dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                        dt.Columns.Add("Tax Amount", typeof(decimal));
                        
                    }
                    if (Flag == "SDInterBrchWiseDetail")
                    {
                        dt.Columns.Add("Item Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("HSN/SAC Code", typeof(string));
                        dt.Columns.Add("Sale Quantity", typeof(decimal));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                        dt.Columns.Add("Value in Specific(Before Discount)", typeof(decimal));
                        dt.Columns.Add("Value in Base(Before Discount)", typeof(decimal));
                        dt.Columns.Add("Discount Value(In Specific)", typeof(decimal));
                        dt.Columns.Add("Discount Value (In Base)", typeof(decimal));
                        dt.Columns.Add("Value in Specific(After Discount)", typeof(decimal));
                        dt.Columns.Add("Value in Base(After Discount)", typeof(decimal));
                        dt.Columns.Add("IGST", typeof(decimal));
                        dt.Columns.Add("CGST", typeof(decimal));
                        dt.Columns.Add("SGST", typeof(decimal));
                        dt.Columns.Add("Tax Amount", typeof(decimal));
                    }
                    //dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    //dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    //dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (Flag == "SDInterBrchWiseSummary")
                    {
                        dt.Columns.Add("Approved On", typeof(string));
                    }
                    if (Flag == "SDInterBrchWiseDetail")
                    {
                        dt.Columns.Add("Remarks", typeof(string));
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                            dtrowLines["GST Number"] = dr["cust_gst_no"].ToString();
                            dtrowLines["Region Name"] = dr["cust_region"].ToString();
                            dtrowLines["Sale Type"] = dr["sale_type"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Sales Representative"] = dr["sale_per"].ToString();
                            dtrowLines["Customer RefNoDate"] = dr["ref_doc_No_Dt"].ToString();
                            dtrowLines["Invoice Number"] = dr["app_inv_no"].ToString();
                            dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                            if (Flag == "SDInterBrchWiseDetail")
                            {
                                dtrowLines["Item Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["HSN/SAC Code"] = dr["HSN_code"].ToString();
                                dtrowLines["Sale Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();

                                dtrowLines["Value in Specific(Before Discount)"] = dr["ValBeforeDiscInSpec"].ToString();
                                dtrowLines["Value in Base(Before Discount)"] = dr["ValBeforeDiscInBs"].ToString();
                                dtrowLines["Discount Value(In Specific)"] = dr["disc_val_spec"].ToString();
                                dtrowLines["Discount Value (In Base)"] = dr["disc_val_bs"].ToString();
                                dtrowLines["Value in Specific(After Discount)"] = dr["ValAfterDiscInSpec"].ToString();
                                dtrowLines["Value in Base(After Discount)"] = dr["ValAfterDiscInBs"].ToString();
                                dtrowLines["IGST"] = dr["igst"].ToString();
                                dtrowLines["CGST"] = dr["cgst"].ToString();
                                dtrowLines["SGST"] = dr["sgst"].ToString();
                                //dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                                //dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                                //dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount"] = dr["item_tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                                dtrowLines["Remarks"] = dr["it_remarks"].ToString();
                            }
                            else
                            {
                                dtrowLines["Sale Amount (In Specific)"] = dr["sale_amount_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["sale_amount_bs"].ToString();
                                dtrowLines["Tax Amount"] = dr["tax_amt_bs"].ToString();
                                //dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                //dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "CustomerWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Customer Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Region Name", typeof(string));
                    if (Flag == "SDCostomerWiseDetail")
                    {
                        dt.Columns.Add("Invoice No.", typeof(string));
                        dt.Columns.Add("Invoice Date", typeof(string));
                        dt.Columns.Add("Sales Representative", typeof(string));
                    }
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (Flag == "SDCostomerWiseDetail")
                    {
                        dt.Columns.Add("Approved On", typeof(string));
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Region Name"] = dr["cust_region"].ToString();
                            if (Flag == "SDCostomerWiseDetail")
                            {
                                dtrowLines["Invoice No."] = dr["app_inv_no"].ToString();
                                dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                                dtrowLines["Sales Representative"] = dr["sale_per"].ToString();
                            }
                            dtrowLines["Sale Amount (In Specific)"] = dr["sale_amount_spec"].ToString();
                            dtrowLines["Sale Amount (In Base)"] = dr["sale_amount_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            if (Flag == "SDCostomerWiseDetail")
                            {
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                                dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "ProductWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Product Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("HSN/SAC Code", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    if(Flag == "SDProductWiseDetail")
                    {
                        dt.Columns.Add("Invoice No.", typeof(string));
                        dt.Columns.Add("Invoice date", typeof(string));
                        dt.Columns.Add("Customer Name", typeof(string));
                        dt.Columns.Add("Region Name", typeof(string));
                        dt.Columns.Add("Quantity", typeof(string));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                    }
                    else
                    {
                        dt.Columns.Add("Quantity", typeof(string));
                        dt.Columns.Add("Average Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Average Price (In Base)", typeof(decimal));
                    }                  
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (Flag == "SDProductWiseDetail")
                    {
                        dt.Columns.Add("Approved On", typeof(string));
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Product Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["HSN/SAC Code"] = dr["HSN_code"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            if (Flag == "SDProductWiseDetail")
                            {
                                dtrowLines["Invoice No."] = dr["app_inv_no"].ToString();
                                dtrowLines["Invoice date"] = dr["inv_dt"].ToString();
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Region Name"] = dr["cust_region"].ToString();
                                dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Average Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Average Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }
                            dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            if (Flag == "SDProductWiseDetail")
                            {
                                dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "ProductGroupWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Group Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    if(Flag == "SDProductGroupWiseDetail")
                    {
                        dt.Columns.Add("Product Name", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Quantity", typeof(string));
                        dt.Columns.Add("Average Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Average Price (In Base)", typeof(decimal));
                    }                         
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Group Name"] = dr["item_group_name"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            if (Flag == "SDProductGroupWiseDetail")
                            {
                                dtrowLines["Product Name"] = dr["item_name"].ToString();
                                dtrowLines["UOM"] = dr["uom_alias"].ToString();
                                dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Average Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Average Price (In Base)"] = dr["item_rate_bs"].ToString();
                            }                                                    
                            dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "RegionWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Region Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    if(Flag == "SDRegionWiseDetail")
                    {
                        dt.Columns.Add("Customer Name", typeof(string));
                    }                         
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Region Name"] = dr["cust_region"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            if (Flag == "SDRegionWiseDetail")
                            {
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Sale Amount (In Specific)"] = dr["sale_amount_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["sale_amount_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "SalesExecutiveWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Sales Executive Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    if(Flag == "SDSalePerWiseDetail")
                    {
                        dt.Columns.Add("Customer Name", typeof(string));
                    }                         
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Sales Executive Name"] = dr["sale_per"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            if (Flag == "SDSalePerWiseDetail")
                            {
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Sale Amount (In Specific)"] = dr["sale_amount_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["sale_amount_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            }
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "ItemDetails"|| DataShow == "CW_Inv_Item_Details"|| DataShow == "ItemDetailsInterBrch" /*--add ItemDetailsInterBrch by Hina Sharma on 27-06-2025---*/)
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Item Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Sale Quantity", typeof(decimal));
                    dt.Columns.Add("Price (In Specific)", typeof(decimal));
                    dt.Columns.Add("Price (In Base)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Item Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["Sale Quantity"] = dr["item_qty"].ToString();
                            dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                            dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                            dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "CW_Inv_Details" || DataShow == "SDProductWiseInvoce"|| DataShow == "SalePersonWiseCW_Inv_Details")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Invoice No.", typeof(string));
                    dt.Columns.Add("Invoice Date", typeof(string));
                    if(DataShow == "SDProductWiseInvoce")
                    {
                        dt.Columns.Add("Customer Name", typeof(string));
                        dt.Columns.Add("Region Name", typeof(string));
                        dt.Columns.Add("Quantity", typeof(decimal));
                        dt.Columns.Add("Price (In Specific)", typeof(decimal));
                        dt.Columns.Add("Price (In Base)", typeof(decimal));
                    }
                    else
                    {
                        dt.Columns.Add("Sales Representative", typeof(string));
                    }      
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Approved On", typeof(string));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Invoice No."] = dr["app_inv_no"].ToString();                            
                            if (DataShow == "SDProductWiseInvoce")
                            {
                                dtrowLines["Invoice Date"] = dr["inv_dt"].ToString();
                                dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                                dtrowLines["Region Name"] = dr["cust_region"].ToString();
                                dtrowLines["Quantity"] = dr["item_qty"].ToString();
                                dtrowLines["Price (In Specific)"] = dr["item_rate_spec"].ToString();
                                dtrowLines["Price (In Base)"] = dr["item_rate_bs"].ToString();
                                dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            }
                            else
                            {
                                dtrowLines["Invoice Date"] = dr["inv_dt1"].ToString();
                                dtrowLines["Sales Representative"] = dr["sale_per"].ToString();
                                dtrowLines["Sale Amount (In Specific)"] = dr["sale_amount_spec"].ToString();
                                dtrowLines["Sale Amount (In Base)"] = dr["sale_amount_bs"].ToString();
                                dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                                dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                                dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                                dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                                dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                                dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            }                                
                            dtrowLines["Approved On"] = dr["app_dt"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "SDProductWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Product Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Quantity", typeof(decimal));
                    dt.Columns.Add("Average Price (In Specific)", typeof(decimal));
                    dt.Columns.Add("Average Price (In Base)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Product Name"] = dr["item_name"].ToString();
                            dtrowLines["UOM"] = dr["uom_alias"].ToString();
                            dtrowLines["Quantity"] = dr["item_qty"].ToString();
                            dtrowLines["Average Price (In Specific)"] = dr["item_rate_spec"].ToString();
                            dtrowLines["Average Price (In Base)"] = dr["item_rate_bs"].ToString();
                            dtrowLines["Sale Amount (In Specific)"] = dr["item_gr_val_spec"].ToString();
                            dtrowLines["Sale Amount (In Base)"] = dr["item_gr_val_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["item_tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["item_tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["item_oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["item_oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["item_net_val_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["item_net_val_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                else if (DataShow == "SDCostomerWise"|| DataShow == "SalePersonWiseSDCostomerWise")
                {
                    dt.Columns.Add("Sr.No", typeof(string));
                    dt.Columns.Add("Customer Name", typeof(string));
                    dt.Columns.Add("Currency", typeof(string));
                    dt.Columns.Add("Sale Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Sale Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Tax Amount (In Base)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Specific)", typeof(decimal));
                    dt.Columns.Add("Other Charges (In Base)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Specific)", typeof(decimal));
                    dt.Columns.Add("Invoice Amount (In Base)", typeof(decimal));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow dtrowLines = dt.NewRow();
                            dtrowLines["Sr.No"] = rowno + 1;
                            dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                            dtrowLines["Currency"] = dr["curr_symbol"].ToString();
                            dtrowLines["Sale Amount (In Specific)"] = dr["sale_amount_spec"].ToString();
                            dtrowLines["Sale Amount (In Base)"] = dr["sale_amount_bs"].ToString();
                            dtrowLines["Tax Amount (In Specific)"] = dr["tax_amt_spec"].ToString();
                            dtrowLines["Tax Amount (In Base)"] = dr["tax_amt_bs"].ToString();
                            dtrowLines["Other Charges (In Specific)"] = dr["oc_amt_spec"].ToString();
                            dtrowLines["Other Charges (In Base)"] = dr["oc_amt_bs"].ToString();
                            dtrowLines["Invoice Amount (In Specific)"] = dr["invoice_amt_spec"].ToString();
                            dtrowLines["Invoice Amount (In Base)"] = dr["invoice_amt_bs"].ToString();
                            dt.Rows.Add(dtrowLines);
                            rowno = rowno + 1;
                        }
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("SalesDetail", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult OnclickPaidAmountDetail(string InVNo, string InvDate,string Curr_id, string Fromdate, string Todate,string Flag)/*Add by Hina sharma on 21-03-2025*/
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrID = Session["BranchId"].ToString();
                //if (Session["userid"] != null)
                //    UserID = Session["userid"].ToString();
                //DataTable dt = _AccountReceivable_ISERVICE.SearchAdvanceAmountDetail(CompID, Br_ID, accId);
                DataSet dt = Sales_ISERVICES.PaidAmountDetail(CompID, BrID, InVNo, InvDate, Curr_id, Fromdate, Todate, Flag);
                ViewBag.PaidAmountDetail = dt.Tables[0];
                ViewBag.PaidAmountDetailTotal = dt.Tables[1];
                //ViewBag.AdvanceAmountDetail = dt;
                return PartialView("~/Areas/Common/Views/Cmn_PartialPaidAmountDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        //Added by Nidhi on 22-12-2025
        private List<CustZoneList> CustZoneLists()
        {
            List<CustZoneList> custzoneList = new List<CustZoneList>();
            DataSet ds = GetCustomerDropdown();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CustZoneList custzone = new CustZoneList();
                custzone.custzone_id = dr["setup_id"].ToString();
                custzone.custzone_val = dr["setup_val"].ToString();
                custzoneList.Add(custzone);
            }
            return custzoneList;
        }
        private List<CustGroupList> CustGroupLists()
        {
            List<CustGroupList> custgroupList = new List<CustGroupList>();
            DataSet ds = GetCustomerDropdown();
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                CustGroupList custgroup = new CustGroupList();
                custgroup.CustGrp_id = dr["setup_id"].ToString();
                custgroup.CustGrp_val = dr["setup_val"].ToString();
                custgroupList.Add(custgroup);
            }
            return custgroupList;
        }
        public DataSet GetCustomerDropdown()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet dt = Sales_ISERVICES.GetCustCommonDropdownDAL(Comp_ID,"0","0");
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        // END
        public ActionResult BindStateListData(SalesDetail_Model _SalesDetail_Model)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    if (_SalesDetail_Model.SearchState == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _SalesDetail_Model.SearchState;
                    }
                    DataSet ProductList = Sales_ISERVICES.GetCustCommonDropdownDAL(CompID,SarchValue,"0");
                    if (ProductList.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[2].Rows.Count; i++)
                        {
                            string state_id = ProductList.Tables[2].Rows[i]["state_id"].ToString();
                            string state_name = ProductList.Tables[2].Rows[i]["state_name"].ToString();
                            string country_name = ProductList.Tables[2].Rows[i]["country_name"].ToString();
                            ItemList.Add(state_id + ',' + country_name, state_name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult BindCityListdata(SalesDetail_Model _SalesDetail_Model)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    if (_SalesDetail_Model.SearchCity == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _SalesDetail_Model.SearchCity;
                    }
                    var state_id = _SalesDetail_Model.state_id;
                    DataSet ProductList = Sales_ISERVICES.GetCustCommonDropdownDAL(CompID, SarchValue, state_id);
                    if (ProductList.Tables[3].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[3].Rows.Count; i++)
                        {
                            string city_id = ProductList.Tables[3].Rows[i]["city_id"].ToString();
                            string city_name = ProductList.Tables[3].Rows[i]["city_name"].ToString();
                            string state_name = ProductList.Tables[3].Rows[i]["state_name"].ToString();
                            string country_name = ProductList.Tables[3].Rows[i]["country_name"].ToString();
                            ItemList.Add(city_id + ',' + state_name + ',' + country_name, city_name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
    }

}