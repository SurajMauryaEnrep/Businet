using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.FSNAnalysis;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.FSNAnalysis;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.FSNAnalysis
{
    public class FSNAnalysisController : Controller
    {
        string CompID, BrId, UserId, language = String.Empty;
        string DocumentMenuId = "105102180108", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        FSNAnalysis_IService _FSNAnalysis_IService;


        public FSNAnalysisController(Common_IServices _Common_IServices, FSNAnalysis_IService fSNAnalysis_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._FSNAnalysis_IService = fSNAnalysis_IService;
        }
        // GET: ApplicationLayer/FSNAnalysis
        public ActionResult FSNAnalysis()
        {
            FSNAnalysis_Model _model = new FSNAnalysis_Model();
            SetAuthValue();

            ViewBag.MenuPageName = getDocumentName();
            _model.AsOnDate = DateTime.Now.ToString("yyyy-MM-dd");
            getAllPageLoadData(_model);//Setting All Page Load Data

            _model.BranchId = BrId;
            _model.Title = title;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/FSNAnalysis/FSNAnalysis.cshtml", _model);
        }
        private void getAllPageLoadData(FSNAnalysis_Model _model)
        {
            DataSet ds = _FSNAnalysis_IService.GetFSNAnalysisPageLoad(CompID, BrId, UserId);

            _model.fsnRanges = SetAgeingRanges(ds.Tables[0]);
            //_model.FromDt = ds.Tables[4].Rows[0]["fin_st_date"].ToString();
            List<ItemGroup> _GroupList = new List<ItemGroup>();
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                ItemGroup ddlGroupList = new ItemGroup();
                ddlGroupList.item_group_id = dr["item_grp_id"].ToString();
                ddlGroupList.item_group_name = dr["itemGroupChildNood"].ToString();
                _GroupList.Add(ddlGroupList);
            }
           // _GroupList.Insert(0, new ItemGroup() { item_group_id = "0", item_group_name = "All" });
            _model.itemGroupsList = _GroupList;


            List<ItemPortFolio> _portFolio = new List<ItemPortFolio>();
            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                ItemPortFolio prfOptions = new ItemPortFolio();
                prfOptions.portfolio_id = dr["portfolio_id"].ToString();
                prfOptions.portfolio_name = dr["portfolio_name"].ToString();
                _portFolio.Add(prfOptions);
            }
            //_portFolio.Insert(0, new ItemPortFolio() { portfolio_id = "0", portfolio_name = "All" });
            _model.itemPortFoliosList = _portFolio;

            List<Branch> _branches = new List<Branch>();
            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                Branch brOpt = new Branch();
                brOpt.br_id = dr["br_id"].ToString();
                brOpt.br_name = dr["br_name"].ToString();
                _branches.Add(brOpt);
            }
            //_branches.Insert(0, new Branch() { br_id = "0", br_name = "All" });
            _model.BranchList = _branches;

            if (ds.Tables[4].Rows.Count > 0 && ds.Tables[5].Rows.Count > 0)
            {
                _model.FromDt = ds.Tables[4].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = ds.Tables[4].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = ds.Tables[4].Rows[0]["fy_enddt"].ToString();
                ViewBag.ToFyMindate = ds.Tables[4].Rows[0]["currfy_startdt"].ToString();/*Add by Hina on 14-08-2024 for current date*/
                ViewBag.fylist = ds.Tables[5];
            }

        }
        private FSNRanges SetAgeingRanges(DataTable dt)
        {
            FSNRanges fsnRanges = new FSNRanges();
            if (dt.Rows.Count > 0)
            {
                fsnRanges = new FSNRanges()
                {
                    range_1 = Convert.ToInt32(dt.Rows[0]["range_1"]),
                    range_2 = Convert.ToInt32(dt.Rows[0]["range_2"])
                };
            }
            return fsnRanges;
        }
        private void SetAuthValue()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrId = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                UserId = Session["UserId"].ToString();
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
        public ActionResult FSNAnalysisActions(string Command, FSNAnalysis_Model _model)
        {
            try
            {
                switch (Command)
                {
                    case "Save":
                        TempData["Message"] = SaveFSNRange(_model.fsnRanges);
                        return RedirectToAction("FSNAnalysis");
                    default:
                        return RedirectToAction("FSNAnalysis");
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        private string SaveFSNRange(FSNRanges _model)
        {
            try
            {
                SetAuthValue();
                string Result = _FSNAnalysis_IService.SaveFSNRange(CompID, UserId, _model);
                return Result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "Error";
            }
        }
        public JsonResult LoadData(string ItemListFilter)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                DataSet DSet = new DataSet();

                (DSet, recordsTotal) = getDtList(ItemListFilter, skip, pageSize, searchValue, sortColumn, sortColumnDir);

                var data = ToDynamicList(DSet.Tables[0]);
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        public List<Dictionary<string, string>> ToDynamicList(DataTable dt)
        {
            var list = new List<Dictionary<string, string>>();

            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col].ToString();
                }
                list.Add(dict);
            }

            return list;
        }
        private (DataSet, int) getDtList(string ItemListFilter, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir, string Flag = "")
        {
            DataSet DSet = new DataSet();
            CommonController cmn = new CommonController();
            int Total_Records = 0;
            try
            {
                string ReportType, itemGrpId, ItemPrtFloId, fromDate, upToDate;
                SetAuthValue();

                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        // fdata = Fstring.Split(',');
                        fdata = Fstring.Split('_');

                        //ReportType = fdata[0];
                        //itemGrpId = fdata[1];
                        //ItemPrtFloId = fdata[2];
                        //fromDate = fdata[3];
                        //upToDate = fdata[4];

                        ReportType = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        itemGrpId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        ItemPrtFloId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        fromDate = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        upToDate = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));

                        DSet = _FSNAnalysis_IService.GetFSNAnalysisMISData(CompID, BrId, UserId, ReportType, itemGrpId, ItemPrtFloId,
                            fromDate, upToDate, skip, pageSize, searchValue, sortColumn, sortColumnDir, Flag);
                    }
                    else
                    {

                    }
                }
                else
                {

                }

                if (DSet.Tables.Count >= 2)
                {

                    if (DSet.Tables[1].Rows.Count > 0)
                    {
                        Total_Records = Convert.ToInt32(DSet.Tables[1].Rows[0]["total_rows"]);
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
            return (DSet, Total_Records);
        }
        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request)
        {
            string keyword = "";
            DataSet Dset = new DataSet();
            // Apply search filter
            if (!string.IsNullOrEmpty(request.search?.value))
            {
                keyword = request.search.value;//.ToLower();
            }
            int recordsTotal = 0;
            string sortColumn = "SrNo";
            string sortColumnDir = "asc";
            if (request.order != null && request.order.Any())
            {
                var colIndex = request.order[0].column;
                sortColumnDir = request.order[0].dir;
                sortColumn = request.columns[colIndex].data;

            }

            (Dset, recordsTotal) = getDtList(request.ItemListFilter, 0, request.length, keyword, sortColumn, sortColumnDir, "CSV");

            var data = ToDynamicList(Dset.Tables[0]);
            var hd1 = new StringBuilder();
            hd1.AppendLine(string.Join(",", request.ColumnsExport.Select((c, index) => {
                string text = "";
                if (index == 3)
                    text = "Fast Moving";
                if (index == 7)
                    text = "Slow Moving";
                if (index == 11)
                    text = "Non Moving";
                return text;
            })));

            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data, hd1);


        }
        public ActionResult GetFSN_InSightData(string ReportType, string FromDt, string upToDate, string ItmCode,string flag)
        {
            SetAuthValue();
            FSNAnalysis_Model _model = new FSNAnalysis_Model();
            DataSet ds = _FSNAnalysis_IService.GetFSN_InSightDatail(CompID, BrId, ReportType
                , FromDt, upToDate, ItmCode, flag);
            ViewBag.InSightData = ds.Tables[0];
            if (flag == "InwordDetail")
            {
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/Partial_MISFSNInwardDetail.cshtml");
            }
            else if (flag == "SaleDetail")
            {
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/Partial_MISFSNSaleDetail.cshtml");
            }
            else // for WhStockDetail
            {
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/Partial_MISFSNStockDetail.cshtml");
            }
        }
    }

}