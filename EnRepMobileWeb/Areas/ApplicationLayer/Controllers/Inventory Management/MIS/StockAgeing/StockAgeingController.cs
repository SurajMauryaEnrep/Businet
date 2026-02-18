using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockAgeing;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockAgeing;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.StockAgeing
{
    public class StockAgeingController : Controller
    {
        string CompID, BrId, UserId, language = String.Empty;
        string DocumentMenuId = "105102180107", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        StockAgeingDetail_ISERVICES _StockAgeingDetail_ISERVICES;
        public StockAgeingController(Common_IServices _Common_IServices, StockAgeingDetail_ISERVICES _StockAgeingDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._StockAgeingDetail_ISERVICES = _StockAgeingDetail_ISERVICES;
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
        // GET: ApplicationLayer/StockAgeing
        public ActionResult StockAgeing()
        {
            StockAgeingDetail_Model _model = new StockAgeingDetail_Model();
            SetAuthValue();
            
            ViewBag.MenuPageName = getDocumentName();
            _model.AsOnDate = DateTime.Now.ToString("yyyy-MM-dd");

            getAllPageLoadData(_model);//Setting All Page Load Data

            _model.BranchId = BrId;
            _model.Title = title;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/StockAgeing/StockAgeingDetail.cshtml", _model);
        }
        private void getAllPageLoadData(StockAgeingDetail_Model _model)
        {
            DataSet ds = _StockAgeingDetail_ISERVICES.GetAgiengPageLoad(CompID, UserId);

            _model.ageingRanges = SetAgeingRanges(ds.Tables[0]);

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
           // _portFolio.Insert(0, new ItemPortFolio() { portfolio_id = "0", portfolio_name = "All" });
            _model.itemPortFoliosList = _portFolio;

            List<HsnCode> _hsnCodes = new List<HsnCode>();
            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                HsnCode hsnCodeOpt = new HsnCode();
                hsnCodeOpt.hsn_code = dr["hsn_no"].ToString();
                hsnCodeOpt.hsn_code_name = dr["hsn_no"].ToString();
                _hsnCodes.Add(hsnCodeOpt);
            }
           // _hsnCodes.Insert(0, new HsnCode() { hsn_code = "0", hsn_code_name = "All" });
            _model.hsnCodesList = _hsnCodes;

            List<Branch> _branches = new List<Branch>();
            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                Branch brOpt = new Branch();
                brOpt.br_id = dr["br_id"].ToString();
                brOpt.br_name= dr["br_name"].ToString();
                _branches.Add(brOpt);
            }
            //_branches.Insert(0, new Branch() { br_id = "0", br_name = "All" });
            _model.BranchList = _branches;

        }
        private AgeingRanges SetAgeingRanges(DataTable dt)
        {
            AgeingRanges ageingRanges = new AgeingRanges();
            if (dt.Rows.Count > 0)
            {

                ageingRanges = new AgeingRanges()
                {
                    range_1 = Convert.ToInt32(dt.Rows[0]["range_1"]),
                    range_2 = Convert.ToInt32(dt.Rows[0]["range_2"]),
                    range_3 = Convert.ToInt32(dt.Rows[0]["range_3"]),
                    range_4 = Convert.ToInt32(dt.Rows[0]["range_4"]),
                    range_5 = Convert.ToInt32(dt.Rows[0]["range_5"])

                };
            }
            return ageingRanges;
        }
        public ActionResult StockAgeingActions(string Command, StockAgeingDetail_Model _model)
        {
            try
            {
                switch (Command)
                {
                    case "Save":
                        TempData["Message"] = SaveAgeingRange(_model.ageingRanges);
                        return RedirectToAction("StockAgeing");
                    default:
                        return RedirectToAction("StockAgeing");
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        private string SaveAgeingRange(AgeingRanges _model)
        {
            try
            {
                SetAuthValue();
                string Result = _StockAgeingDetail_ISERVICES.SaveAgeingRange(CompID, UserId, _model);
                return Result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "Error";
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
                string ReportType, itemGrpId, ItemPrtFloId, hsnCode, BrnchId, upToDate;
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
                        //hsnCode = fdata[3];
                        //BrnchId = fdata[4];
                        //upToDate = fdata[5];

                        ReportType = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        itemGrpId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        ItemPrtFloId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        hsnCode = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        upToDate = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        BrnchId = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));

                        DSet = _StockAgeingDetail_ISERVICES.GetStockAgeingMISData(CompID, BrId, UserId, ReportType, itemGrpId, ItemPrtFloId, hsnCode
                            , BrnchId, upToDate, skip, pageSize, searchValue, sortColumn, sortColumnDir, Flag);
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
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);


        }

    }

}