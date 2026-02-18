using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.SampleTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.SampleTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.SampleTracking
{
    public class SampleTrackingController : Controller
    {
        string FromDate = string.Empty;
        string DocumentMenuId = "105102180110";
        string CompID, Br_ID, language, title = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SampleTracking_ISERVICES sampleTracking_ISERVICES;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public SampleTrackingController(Common_IServices _Common_IServices, SampleTracking_ISERVICES sampleTracking_ISERVICES
            , GeneralLedger_ISERVICE GeneralLedger_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.sampleTracking_ISERVICES = sampleTracking_ISERVICES;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        // GET: ApplicationLayer/SampleTracking
        public ActionResult SampleTrackingDetail()
        {
            DateTime dtnow = DateTime.Now;
           // string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
          //  string startDate = new DateTime(dtnow.Year, 4, 1).ToString("yyyy-MM-dd");/*Modified Month By Nitesh 05-12-2023 add Fincacial Year in From Date*/
            //string endDate = dtnow.ToString("yyyy-MM-dd");
            SampleTracking_Model sampleTracking_Model = new SampleTracking_Model();
            DataSet dttbl = new DataSet();
            #region Added By Nitesh  02-01-2024 for Financial Year 
            #endregion
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                //sampleTracking_Model.FromDate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                sampleTracking_Model.FromDate = "2000-01-01";
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }

            List<EntityNameList> _EntityName = new List<EntityNameList>();
            EntityNameList _EntityNameList = new EntityNameList();
            _EntityNameList.entity_name = "---Select---";
            _EntityNameList.entity_id = "0";
            _EntityName.Add(_EntityNameList);
            sampleTracking_Model.EntityNameList = _EntityName;

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            DataSet ds1 = sampleTracking_ISERVICES.GetIssuedByData(CompID, Br_ID);
            List<Issuedby> _IssuedbyList = new List<Issuedby>();
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                Issuedby _Issuedby = new Issuedby();
                _Issuedby.Issuedby_id = dr["emp_id"].ToString();
                _Issuedby.Issuedby_Name = dr["emp_name"].ToString();
                _IssuedbyList.Add(_Issuedby);

            }
            Issuedby _Issuedby2 = new Issuedby();
            _Issuedby2.Issuedby_id = "0";
            _Issuedby2.Issuedby_Name = "---Select---";
            _IssuedbyList.Insert(0, _Issuedby2);
            sampleTracking_Model.IssuedbyList = _IssuedbyList;
            ViewBag.MenuPageName = getDocumentName();
            sampleTracking_Model.SampleTrackingLists = GetSTDetailList(sampleTracking_Model);
            sampleTracking_Model.Title = title;
           // sampleTracking_Model.FromDate = startDate;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/SampleTracking/SampleTracking.cshtml", sampleTracking_Model);
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
        //public ActionResult GetAutoCompleteSTItems(SampleTracking_Model sampleTracking_Model)
        //{
        //    try
        //    {
        //        string ItmName = string.Empty;
        //        Dictionary<string, string> ST_itemsList = new Dictionary<string, string>();
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        try
        //        {
        //            if (string.IsNullOrEmpty(Convert.ToString(sampleTracking_Model.ST_Item)))
        //            {
        //                ItmName = "0";
        //            }
        //            else
        //            {
        //                ItmName = Convert.ToString(sampleTracking_Model.ST_Item);
        //            }
        //            //ST_itemsList = sampleTracking_ISERVICES.GetSampleTrackingItmList(CompID, Br_ID, ItmName);

        //            List<ItemList> itemLists = new List<ItemList>();
        //            foreach (var dr in ST_itemsList)
        //            {
        //                ItemList itemList = new ItemList();
        //                itemList.Item_id = dr.Key;
        //                itemList.Item_name = dr.Value;
        //                itemLists.Add(itemList);
        //            }
        //            sampleTracking_Model.itemLists = itemLists;
        //        }
        //        catch (Exception ex)
        //        {
        //            string path = Server.MapPath("~");
        //            Errorlog.LogError(path, ex);
        //            return Json("ErrorPage");
        //        }
        //        return Json(ST_itemsList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //}
        [HttpPost]
        public ActionResult GetST_ItemList(SampleTracking_Model sampleTracking_Model)
        {
            JsonResult DataRows = null;
            string ItmName = string.Empty;
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
                if (string.IsNullOrEmpty(sampleTracking_Model.ST_Item))
                {
                    ItmName = "0";
                }
                else
                {
                    ItmName = sampleTracking_Model.ST_Item;
                }
                //ST_itemsList = sampleTracking_ISERVICES.GetSampleTrackingItmList(CompID, Br_ID, ItmName);
                DataSet ItmList = sampleTracking_ISERVICES.GetSampleTrackingItmList(Comp_ID, Br_ID, ItmName);
                DataRow Drow = ItmList.Tables[0].NewRow();
                Drow[0] = "0";
                Drow[1] = "---Select---";
                Drow[2] = "0";
                ItmList.Tables[0].Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(ItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        private List<SampleTrackingList> GetSTDetailList(SampleTracking_Model sampleTracking_Model)
        {
            try
            {
                List<SampleTrackingList> sampleTrackingLists = new List<SampleTrackingList>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                //var Fromdate =  startDate;
                var Fromdate = sampleTracking_Model.FromDate;
                var Todate = DateTime.Now.ToString("yyyy-MM-dd");
                DataSet dt = sampleTracking_ISERVICES.GetSTDetailsList(CompID, Br_ID,"EW","0","0", Fromdate, Todate,"",null,"");
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                //ViewBag.SampleWisedata = "EW";
                sampleTracking_Model.SampleWisedata = "EW";
                //Session["SampleTrackingS"] = "0";
                sampleTracking_Model.SampleTrackingS = "0";
                if (dt.Tables[0].Rows.Count > 0)
                {
                    
                  
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        SampleTrackingList _STList = new SampleTrackingList();
                        _STList._Entity = dr["issue_to"].ToString();
                        _STList._EntityID = dr["suppid"].ToString();
                        _STList._EntityType = dr["entity_type"].ToString();
                        _STList._EntityTypeCode = dr["entity_type1"].ToString();
                        _STList.ItemName = dr["item_name"].ToString();
                        _STList.ItemID = dr["item_id"].ToString();
                        _STList.UOM = dr["uom_name"].ToString();
                        _STList.uom_id = dr["uom_id"].ToString();
                        _STList.Opening_balence = dr["opening"].ToString();
                        _STList.Issued = dr["issue_qty"].ToString();
                        _STList.Receipts = dr["rec_qty"].ToString(); 
                        _STList.Balance = dr["balance"].ToString();
                        //_STList.sr_type = dr["sr_type"].ToString();
                        //_STList.other_dtl = dr["other_dtl"].ToString();
                        //_STList.issue_date = dr["issue_date"].ToString();
                        //_STList.receive_date = dr["receive_date"].ToString();
                        //_STList.elsp_days = dr["elsp_days"].ToString();
                        sampleTrackingLists.Add(_STList);
                    }
                }
                    return sampleTrackingLists;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public JsonResult LoadData1(string ItemListFilter, string ReportType,string ShowAs)
        {
            try
            {
               
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn1 = Request.Form.GetValues("columns[" + Request.Form.GetValues
                    ("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (ReportType == "EW")
                {
                    List<SampleTrackingList> _ItemListModel = new List<SampleTrackingList>();

                    _ItemListModel = getStockDtList(ItemListFilter);
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir);
                    }
                    if(ShowAs=="D")
                    {
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchValue = searchValue.ToUpper();
                            ItemListData = ItemListData.Where(m => m._Entity.ToUpper().Contains(searchValue) 
                            || m._EntityType.ToUpper().Contains(searchValue)  || m.ItemName.ToUpper().Contains(searchValue) || m.Issue_Number_Summ.ToUpper().Contains(searchValue)
                            || m.ItemID.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.uom_id.ToUpper().Contains(searchValue)
                          
                            
                            );
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchValue = searchValue.ToUpper();
                            ItemListData = ItemListData.Where(m => m._Entity.ToUpper().Contains(searchValue) || m._EntityID.ToUpper().Contains(searchValue)
                            || m._EntityType.ToUpper().Contains(searchValue) || m._EntityTypeCode.ToUpper().Contains(searchValue) || m.ItemName.ToUpper().Contains(searchValue)
                            || m.ItemID.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.uom_id.ToUpper().Contains(searchValue)
                            || m.Issued.ToUpper().Contains(searchValue) || m.Receipts.ToUpper().Contains(searchValue)
                            || m.Balance.ToUpper().Contains(searchValue) /*|| m.sr_type.ToUpper().Contains(searchValue) || m.other_dtl.ToUpper().Contains(searchValue)*/
                            //|| m.elsp_days.ToUpper().Contains(searchValue)
                            );
                        }
                    }
                   

                 
                    recordsTotal = ItemListData.Count();
                    
                    var  data = ItemListData.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }

                else if ( ReportType == "PS")
                {
                    List<SampleTrackingList> _ItemListModel1 = new List<SampleTrackingList>();

                    _ItemListModel1 = getStockDtList(ItemListFilter);
                    var ItemListData = (from tempitem in _ItemListModel1 select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir);
                    }
                    if (ShowAs == "D")
                    {
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchValue = searchValue.ToUpper();
                            ItemListData = ItemListData.Where(m => m._Entity.ToUpper().Contains(searchValue)
                            || m._EntityType.ToUpper().Contains(searchValue) || m.ItemName.ToUpper().Contains(searchValue)
                            || m.UOM.ToUpper().Contains(searchValue) || m.uom_id.ToUpper().Contains(searchValue)
                            || m.Issued.ToUpper().Contains(searchValue) 
                           
                            );
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchValue = searchValue.ToUpper();
                            ItemListData = ItemListData.Where(m => m._Entity.ToUpper().Contains(searchValue) || m._EntityID.ToUpper().Contains(searchValue)
                            || m._EntityType.ToUpper().Contains(searchValue) || m._EntityTypeCode.ToUpper().Contains(searchValue) || m.ItemName.ToUpper().Contains(searchValue)
                            || m.ItemID.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.uom_id.ToUpper().Contains(searchValue)
                           || m.Issued.ToUpper().Contains(searchValue) || m.Receipts.ToUpper().Contains(searchValue)
                            || m.Balance.ToUpper().Contains(searchValue) /*|| m.sr_type.ToUpper().Contains(searchValue) || m.other_dtl.ToUpper().Contains(searchValue)*/
                            //|| m.elsp_days.ToUpper().Contains(searchValue)
                            );
                        }
                    }
                  


                    recordsTotal = ItemListData.Count();

                    var data = ItemListData.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else if(ReportType == "BW" || ReportType == "IB")
                {
                    List<SampleTrackingList_BinWise> _ItemList_Model = new List<SampleTrackingList_BinWise>();

                    _ItemList_Model = getTrackingBinWiseandIssuedby(ItemListFilter);
                    var ItemListData1 = (from tempitem1 in _ItemList_Model select tempitem1);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData1 = ItemListData1.OrderBy(sortColumn1 + " " + sortColumnDir);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData1 = ItemListData1.Where(m => m.ItemName_Bin.ToUpper().Contains(searchValue) || m.doc_no_Bin.ToUpper().Contains(searchValue) || m.sr_dt_Bin.ToUpper().Contains(searchValue)
                        || m.entity_type_Bin.ToUpper().Contains(searchValue) || m.bin_loc_Bin.ToUpper().Contains(searchValue) || m.sr_type_Bin.ToUpper().Contains(searchValue)
                        || m.entityName_Bin.ToUpper().Contains(searchValue) || m.issued_by_Bin.ToUpper().Contains(searchValue)
                        || m.issued_date_Bin.ToUpper().Contains(searchValue) || m.receive_date_Bin.ToUpper().Contains(searchValue)
                        || m.other_dtl_Bin.ToUpper().Contains(searchValue) || m.issued_qty_Bin.ToUpper().Contains(searchValue) || m.qty_Bin.ToUpper().Contains(searchValue)
                        || m.elsp_days_Bin.ToUpper().Contains(searchValue) || m.remarks_Bin.ToUpper().Contains(searchValue)
                        );
                    }


                    recordsTotal = ItemListData1.Count();

                    var data = ItemListData1.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else
                {
                    List<SampleTrackingList_sampleWise> _ItemListModel = new List<SampleTrackingList_sampleWise>();

                    _ItemListModel = getStockDtList_SampleWise(ItemListFilter);
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => m.doc_no_Tracking.ToUpper().Contains(searchValue) || m.sr_dt_Tracking.ToUpper().Contains(searchValue)
                        || m.entity_type_Tracking.ToUpper().Contains(searchValue) || m.bin_loc_Tracking.ToUpper().Contains(searchValue) || m.sr_type_Tracking.ToUpper().Contains(searchValue)
                        || m.entityName_Tracking.ToUpper().Contains(searchValue) || m.issued_by_Tracking.ToUpper().Contains(searchValue)
                        || m.issued_date_Tracking.ToUpper().Contains(searchValue) || m.receive_date_Tracking.ToUpper().Contains(searchValue)
                        || m.other_dtl_Tracking.ToUpper().Contains(searchValue) || m.issued_qty_Tracking.ToUpper().Contains(searchValue) || m.qty_Tracking.ToUpper().Contains(searchValue)
                        || m.elsp_days_Tracking.ToUpper().Contains(searchValue) || m.remarks_Tracking.ToUpper().Contains(searchValue) 
                        );
                    }


                    recordsTotal = ItemListData.Count();
                    
                   var data = ItemListData.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                             
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private List<SampleTrackingList> getStockDtList(string ItemListFilter)
        {
            List<SampleTrackingList> _ItemListModel = new List<SampleTrackingList>();
            List<SampleTrackingList_sampleWise> _SampleTrackingListSampleWise = new List<SampleTrackingList_sampleWise>();
            CommonController cmn = new CommonController();
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string EntityType, EntityId, ItemId, Issuedby, FromDate, ToDate, ReportType = "";
                string ShowAs = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
              //  DataTable DSet = new DataTable();
                DataSet DSet = new DataSet();
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        //fdata = Fstring.Split(',');
                        fdata = Fstring.Split('_');

                        //EntityType = fdata[0];
                        //EntityId = fdata[1];
                        //ItemId = fdata[2];
                        //FromDate = fdata[3];
                        //ToDate = fdata[4];
                        //ReportType = fdata[5];
                        //Issuedby = fdata[6];
                        //ShowAs = fdata[7];
                        EntityType = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        EntityId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        ItemId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        FromDate = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        ToDate = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        ReportType = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        Issuedby = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));
                        ShowAs = cmn.ReplaceDefault(fdata[7].Trim('[', ']'));
                        DSet = sampleTracking_ISERVICES.GetSTDetailsList(CompID, Br_ID, ReportType, ItemId, EntityId, FromDate, ToDate, EntityType, Issuedby, ShowAs);
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        int rowno = 0;

                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        SampleTrackingList _STList = new SampleTrackingList();
                        if (ReportType == "PS" && ShowAs == "S")
                        {
                            _STList.SrNo = rowno + 1;
                            _STList._Entity = dr["issue_to"].ToString().Trim();
                            _STList._EntityID = dr["suppid"].ToString().Trim();
                            _STList._EntityType = dr["entity_type"].ToString().Trim();
                            _STList._EntityTypeCode = dr["entity_type1"].ToString().Trim();
                            _STList.ItemName = dr["item_name"].ToString().Trim();
                            _STList.ItemID = dr["item_id"].ToString().Trim();
                            _STList.UOM = dr["uom_name"].ToString().Trim();
                            _STList.uom_id = dr["uom_id"].ToString().Trim();

                            _STList.Issued = dr["issue_qty"].ToString().Trim();
                            _STList.Receipts = dr["rec_qty"].ToString().Trim();
                            _STList.Balance = dr["balance"].ToString().Trim();
                            //  _STList.sr_type = dr["sr_type"].ToString().Trim();
                            //  _STList.other_dtl = dr["other_dtl"].ToString().Trim();
                            //_STList.issue_date = dr["issue_date"].ToString().Trim();
                            //_STList.receive_date = dr["receive_date"].ToString().Trim();
                            // _STList.elsp_days = dr["elsp_days"].ToString().Trim();
                        }
                        else if((ReportType == "PS" || ReportType == "EW") && ShowAs == "D")
                        {
                            _STList.SrNo = rowno + 1;
                            _STList._EntityType = dr["entity_type"].ToString().Trim();
                            _STList._Entity = dr["issue_to"].ToString().Trim();
                            _STList.ItemName = dr["item_name"].ToString().Trim();
                            _STList.ItemID = dr["item_id"].ToString().Trim();
                            _STList.UOM = dr["uom_name"].ToString().Trim();
                            _STList.uom_id = dr["uom_id"].ToString().Trim();
                            _STList.Issue_Number_Summ = dr["issue_no"].ToString().Trim();
                            _STList.issued_dt_Summ = dr["issue_dt"].ToString().Trim();
                            _STList.Bin_loc_Summ = dr["bin_loc"].ToString().Trim();
                            _STList.sr_type = dr["sr_type"].ToString().Trim();
                            _STList.Issued_by_Summ = dr["emp_name"].ToString().Trim();
                            _STList.issue_date = dr["issue_date"].ToString().Trim();
                            _STList.receive_date = dr["receive_date"].ToString().Trim();
                            _STList.other_dtl = dr["other_dtl"].ToString().Trim();
                            _STList.Issued = dr["issue_qty"].ToString().Trim();
                            _STList.Receipts = dr["rec_qty"].ToString().Trim();
                            _STList.Balance = dr["balance"].ToString().Trim();
                            _STList.elsp_days = dr["elsp_days"].ToString().Trim();
                            _STList.Remarks_Summ = dr["it_remarks"].ToString().Trim();
                        }
                        else
                        {
                          
                                _STList.SrNo = rowno + 1;
                                _STList._Entity = dr["issue_to"].ToString().Trim();
                                _STList._EntityID = dr["suppid"].ToString().Trim();
                                _STList._EntityType = dr["entity_type"].ToString().Trim();
                                _STList._EntityTypeCode = dr["entity_type1"].ToString().Trim();
                                _STList.ItemName = dr["item_name"].ToString().Trim();
                                _STList.ItemID = dr["item_id"].ToString().Trim();
                                _STList.UOM = dr["uom_name"].ToString().Trim();
                                _STList.uom_id = dr["uom_id"].ToString().Trim();
                                _STList.Opening_balence = dr["opening"].ToString().Trim();
                                _STList.Issued = dr["issue_qty"].ToString().Trim();
                                _STList.Receipts = dr["rec_qty"].ToString().Trim();
                                _STList.Balance = dr["balance"].ToString().Trim();
                              
                            

                        }

                        _ItemListModel.Add(_STList);
                        rowno = rowno + 1;
                    }
                }
                    return _ItemListModel;            
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
           
        }

        private List<SampleTrackingList_sampleWise> getStockDtList_SampleWise(string ItemListFilter)
        {         
            List<SampleTrackingList_sampleWise> _SampleTrackingListSampleWise = new List<SampleTrackingList_sampleWise>();
            try
            {
                CommonController cmn = new CommonController();
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string EntityType, EntityId, ItemId, FromDate, Issuedby,ToDate, ReportType;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //  DataTable DSet = new DataTable();
                DataSet DSet = new DataSet();
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        fdata = Fstring.Split('_');
                        //fdata = Fstring.Split(',');

                        //EntityType = fdata[0];
                        //EntityId = fdata[1];
                        //ItemId = fdata[2];
                        //FromDate = fdata[3];
                        //ToDate = fdata[4];
                        //ReportType = fdata[5];
                        //Issuedby = fdata[6];

                        EntityType = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        EntityId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        ItemId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        FromDate = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        ToDate = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        ReportType = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        Issuedby = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));

                        DSet = sampleTracking_ISERVICES.GetSTDetailsList(CompID, Br_ID, ReportType, ItemId, EntityId, FromDate, ToDate, EntityType, Issuedby,"");
                    }
                    else
                    {

                    }
                }
                else
                {

                }           
               
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        int rowno1 = 0;
                        foreach (DataRow dr in DSet.Tables[0].Rows)
                        {
                            SampleTrackingList_sampleWise _STList = new SampleTrackingList_sampleWise();
                            _STList.SrNo_Tracking = rowno1 + 1;
                            _STList.doc_no_Tracking = dr["doc_no"].ToString();
                            _STList.sr_dt_Tracking = dr["sr_dt"].ToString();
                            _STList.entity_type_Tracking = dr["entity_type"].ToString();
                            _STList.bin_loc_Tracking = dr["bin_loc"].ToString();
                            _STList.sr_type_Tracking = dr["sr_type"].ToString();
                            _STList.entityName_Tracking = dr["entityName"].ToString();
                            _STList.issued_by_Tracking = dr["issued_by"].ToString();
                            _STList.issued_date_Tracking = dr["issued_date"].ToString();
                            _STList.receive_date_Tracking = dr["receive_date"].ToString();
                            _STList.other_dtl_Tracking = dr["other_dtl"].ToString();
                            _STList.issued_qty_Tracking = dr["issued_qty"].ToString();
                            _STList.qty_Tracking = dr["qty"].ToString();
                            _STList.elsp_days_Tracking = dr["elsp_days"].ToString();
                            _STList.remarks_Tracking = dr["remarks"].ToString();
                            _SampleTrackingListSampleWise.Add(_STList);
                            rowno1 = rowno1 + 1;
                        }
                    }
                    return _SampleTrackingListSampleWise;
               

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        private List<SampleTrackingList_BinWise> getTrackingBinWiseandIssuedby(string ItemListFilter)
        {
            List<SampleTrackingList_BinWise> _SampleTrackingListBinWise = new List<SampleTrackingList_BinWise>();
            try
            {
                CommonController cmn = new CommonController();
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string EntityType, EntityId, ItemId, FromDate, Issuedby, ToDate, ReportType="";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //  DataTable DSet = new DataTable();
                DataSet DSet = new DataSet();
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        //fdata = Fstring.Split(',');
                        fdata = Fstring.Split('_');

                        //EntityType = fdata[0];
                        //EntityId = fdata[1];
                        //ItemId = fdata[2];
                        //FromDate = fdata[3];
                        //ToDate = fdata[4];
                        //ReportType = fdata[5];
                        //Issuedby = fdata[6];

                        EntityType = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        EntityId = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        ItemId = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        FromDate = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        ToDate = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        ReportType = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        Issuedby = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));

                        DSet = sampleTracking_ISERVICES.GetSTDetailsList(CompID, Br_ID, ReportType, ItemId, EntityId, FromDate, ToDate, EntityType, Issuedby,"");
                    }
                    else
                    {

                    }
                }
                else
                {

                }

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    int rowno1 = 0;
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        SampleTrackingList_BinWise _STList = new SampleTrackingList_BinWise();
                        _STList.SrNo_Bin = rowno1 + 1;
                        if(ReportType== "BW")
                        {
                            _STList.bin_loc_Bin = dr["bin_loc"].ToString();
                        }
                        else
                        {
                            _STList.issued_by_Bin = dr["issued_by"].ToString();
                        }
                      

                        _STList.ItemName_Bin = dr["item_name"].ToString();
                        _STList.ItemId_Bin = dr["item_id"].ToString();
                        _STList.Uom_Bin = dr["uom_alias"].ToString();
                        _STList.UomId_Bin = dr["uom_id"].ToString();
                        _STList.doc_no_Bin = dr["doc_no"].ToString();
                        _STList.sr_dt_Bin = dr["sr_dt"].ToString();
                        _STList.entity_type_Bin = dr["entity_type"].ToString();
                        if (ReportType == "IB")
                        {
                            _STList.bin_loc_Bin = dr["bin_loc"].ToString();
                        }
                        //_STList.entity_id_Bin = dr["entity_type"].ToString();                      
                        _STList.sr_type_Bin = dr["sr_type"].ToString();
                        _STList.entityName_Bin = dr["entityName"].ToString();
                        if (ReportType == "BW")
                        {
                            _STList.issued_by_Bin = dr["issued_by"].ToString();
                        }
                     
                        _STList.issued_date_Bin = dr["issued_date"].ToString();
                        _STList.receive_date_Bin = dr["receive_date"].ToString();
                        _STList.other_dtl_Bin = dr["other_dtl"].ToString();
                        _STList.issued_qty_Bin = dr["issued_qty"].ToString();
                        _STList.qty_Bin = dr["qty"].ToString();
                        _STList.elsp_days_Bin = dr["elsp_days"].ToString();
                        _STList.remarks_Bin = dr["remarks"].ToString();
                        _SampleTrackingListBinWise.Add(_STList);
                        rowno1 = rowno1 + 1;
                    }
                }
                return _SampleTrackingListBinWise;


            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }

        }

        public ActionResult GetSupp_CustList(SampleTracking_Model sampleTracking_Model)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet SuppCustList = new DataSet();
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                string CompID = string.Empty;
                string BrchID = string.Empty;

                List<EntityNameList> _EntityName = new List<EntityNameList>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();

                }
                if (string.IsNullOrEmpty(sampleTracking_Model.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = sampleTracking_Model.EntityName.ToString();
                }

                if (string.IsNullOrEmpty(sampleTracking_Model.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = sampleTracking_Model.entity_type.ToString();
                }
                SuppCustList = sampleTracking_ISERVICES.getSuppCustList(CompID, BrchID, EntityName, EntityType);

                DataRow Drow = SuppCustList.Tables[0].NewRow();
                Drow[0] = "0";
                Drow[1] = "---Select---";
                SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                {
                    EntityNameList _EntityNameList = new EntityNameList();
                    _EntityNameList.entity_name = dr["val"].ToString();
                    _EntityNameList.entity_id = dr["id"].ToString();
                    _EntityName.Add(_EntityNameList);
                }
                sampleTracking_Model.EntityNameList = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }


        }
        public ActionResult GetAutoCompleteEntity(SampleTracking_Model sampleTracking_Model)
        {
            string Entity = string.Empty;
            Dictionary<string, string> EntityList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

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
                if (string.IsNullOrEmpty(sampleTracking_Model.EntityName))
                {
                    Entity = "0";
                }
                else
                {
                    Entity = sampleTracking_Model.EntityName;
                }
                EntityList = sampleTracking_ISERVICES.IssueToList(Comp_ID, Entity, Br_ID);

                return Json(EntityList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
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

        [HttpPost]
        public JsonResult GetItemIssuedReceivedDetailList(string ItemID, string EntityID,
            string EntityTypeCode, string Type,string issue_date,string receive_date,
            string sr_type, string other_dtl,string ST_UOM, string fromdate, string todate,string Issuedby)
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
                DataSet result = sampleTracking_ISERVICES.GetItemIssued_ReceivedList(Comp_ID, Br_ID, EntityID, 
                    EntityTypeCode, ItemID, Type, issue_date, receive_date, 
                    sr_type, other_dtl, ST_UOM, fromdate, todate, Issuedby);

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
        public ActionResult FilterSampleTrackingDetail(string ReportType, string ItemId, string EntityId, 
            string FromDate, string ToDate,string EntityType, string Issuedby, string ShowAs)
        {
            try
            {
                List<SampleTrackingList> _SampleTrackingList = new List<SampleTrackingList>();
                List<SampleTrackingList_sampleWise> _SampleTrackingListSampleWise = new List<SampleTrackingList_sampleWise>();
                SampleTracking_Model _SampleTracking_Model = new SampleTracking_Model();
                List<SampleTrackingList_BinWise> _SampleTrackingListBinWise = new List<SampleTrackingList_BinWise>();
                string CompID = string.Empty;
                string Partial_View = string.Empty;
                DataSet dt = new DataSet();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if(!string.IsNullOrEmpty(EntityId))
                {
                    EntityId = EntityId.Replace("C","").Replace("S","");
                }
                 dt = sampleTracking_ISERVICES.GetSTDetailsList(CompID, Br_ID, ReportType, ItemId, EntityId, FromDate, ToDate, EntityType, Issuedby, ShowAs);
                if (ReportType == "EW" && ShowAs == "S")
                {
                   
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            //Session["SampleTrackingS"] = "S_Tracking";
                            //_SampleTracking_Model.SampleTrackingS = "S_Tracking";
                            foreach (DataRow dr in dt.Tables[0].Rows)
                            {
                                SampleTrackingList _STList = new SampleTrackingList();
                                _STList._Entity = dr["issue_to"].ToString();
                                _STList._EntityID = dr["suppid"].ToString();
                                _STList._EntityType = dr["entity_type"].ToString();
                                _STList._EntityTypeCode = dr["entity_type1"].ToString();
                                _STList.ItemName = dr["item_name"].ToString();
                                _STList.ItemID = dr["item_id"].ToString();
                                _STList.UOM = dr["uom_name"].ToString();
                                _STList.uom_id = dr["uom_id"].ToString();
                                _STList.Opening_balence = dr["opening"].ToString();
                                _STList.Issued = dr["issue_qty"].ToString();
                                _STList.Receipts = dr["rec_qty"].ToString();
                                _STList.Balance = dr["balance"].ToString();
                                //_STList.sr_type = dr["sr_type"].ToString();
                                //_STList.other_dtl = dr["other_dtl"].ToString();
                                //_STList.issue_date = dr["issue_date"].ToString();
                                //_STList.receive_date = dr["receive_date"].ToString();
                                //_STList.elsp_days = dr["elsp_days"].ToString();
                                _SampleTrackingList.Add(_STList);
                            }
                        }
                        _SampleTracking_Model.SummaryDetail = "S";
                   

                    //ViewBag.SampleWisedata = "EW";
                    _SampleTracking_Model.SampleWisedata = "EW";
                    _SampleTracking_Model.SampleTrackingS = "S_Tracking";
                    _SampleTracking_Model.SampleTrackingLists = _SampleTrackingList;
                }
                else if((ReportType == "PS" || ReportType == "EW") && ShowAs == "D")
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                      
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            SampleTrackingList _STList = new SampleTrackingList();
                            //_STList.SrNo = rowno + 1;
                            _STList._EntityType = dr["entity_type"].ToString().Trim();
                            _STList._Entity = dr["issue_to"].ToString().Trim();
                            _STList.ItemName = dr["item_name"].ToString().Trim();
                            _STList.ItemID = dr["item_id"].ToString().Trim();
                            _STList.UOM = dr["uom_name"].ToString().Trim();
                            _STList.uom_id = dr["uom_id"].ToString().Trim();
                            _STList.Issue_Number_Summ = dr["issue_no"].ToString().Trim();
                            _STList.issued_dt_Summ = dr["issue_dt"].ToString().Trim();
                            _STList.Bin_loc_Summ = dr["bin_loc"].ToString().Trim();
                            _STList.sr_type = dr["sr_type"].ToString().Trim();
                            _STList.Issued_by_Summ = dr["emp_name"].ToString().Trim();
                            _STList.issue_date = dr["issue_date"].ToString().Trim();
                            _STList.receive_date = dr["receive_date"].ToString().Trim();
                            _STList.other_dtl = dr["other_dtl"].ToString().Trim();
                            _STList.Issued = dr["issue_qty"].ToString().Trim();
                            _STList.Receipts = dr["rec_qty"].ToString().Trim();
                            _STList.Balance = dr["balance"].ToString().Trim();
                            _STList.elsp_days = dr["elsp_days"].ToString().Trim();
                            _STList.Remarks_Summ = dr["it_remarks"].ToString().Trim();
                            _SampleTrackingList.Add(_STList);
                        }
                    }
                    _SampleTracking_Model.SummaryDetail = "D";
                    _SampleTracking_Model.SampleWisedata = ReportType;
                    _SampleTracking_Model.SampleTrackingS = "S_Tracking";
                    _SampleTracking_Model.SampleTrackingLists = _SampleTrackingList;
                }
                else if (ReportType == "PS" && ShowAs == "S")
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        //Session["SampleTrackingS"] = "S_Tracking";
                        //_SampleTracking_Model.SampleTrackingS = "S_Tracking";
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            SampleTrackingList _STList = new SampleTrackingList();
                            _STList._Entity = dr["issue_to"].ToString();
                            _STList._EntityID = dr["suppid"].ToString();
                            _STList._EntityType = dr["entity_type"].ToString();
                            _STList._EntityTypeCode = dr["entity_type1"].ToString();
                            _STList.ItemName = dr["item_name"].ToString();
                            _STList.ItemID = dr["item_id"].ToString();
                            _STList.UOM = dr["uom_name"].ToString();
                            _STList.uom_id = dr["uom_id"].ToString();
                            _STList.Opening_balence = dr["opening"].ToString();
                            _STList.Issued = dr["issue_qty"].ToString();
                            _STList.Receipts = dr["rec_qty"].ToString();
                            _STList.Balance = dr["balance"].ToString();
                            //_STList.sr_type = dr["sr_type"].ToString();
                            //_STList.other_dtl = dr["other_dtl"].ToString();
                            //_STList.issue_date = dr["issue_date"].ToString();
                            //_STList.receive_date = dr["receive_date"].ToString();
                           // _STList.elsp_days = dr["elsp_days"].ToString();
                            _SampleTrackingList.Add(_STList);
                        }
                    }
                    _SampleTracking_Model.SummaryDetail = "S";
                    //ViewBag.SampleWisedata = "EW";
                    _SampleTracking_Model.SampleWisedata = "PS";
                    _SampleTracking_Model.SampleTrackingS = "S_Tracking";
                    _SampleTracking_Model.SampleTrackingLists = _SampleTrackingList;
                }
                else if(ReportType == "BW" || ReportType == "IB")
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        int rowno1 = 0;
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            SampleTrackingList_BinWise _STList = new SampleTrackingList_BinWise();
                            _STList.SrNo_Bin = rowno1 + 1;

                            _STList.bin_loc_Bin = dr["bin_loc"].ToString();

                            _STList.ItemName_Bin = dr["item_name"].ToString();
                            _STList.ItemId_Bin = dr["item_id"].ToString();
                            _STList.Uom_Bin = dr["uom_alias"].ToString();
                            _STList.UomId_Bin = dr["uom_id"].ToString();
                            _STList.doc_no_Bin = dr["doc_no"].ToString();
                            _STList.sr_dt_Bin = dr["sr_dt"].ToString();
                            _STList.entity_type_Bin = dr["entity_type"].ToString();
                            //_STList.entity_id_Bin = dr["entity_type"].ToString();                      
                            _STList.sr_type_Bin = dr["sr_type"].ToString();
                            _STList.entityName_Bin = dr["entityName"].ToString();
                            _STList.issued_by_Bin = dr["issued_by"].ToString();
                            _STList.issued_date_Bin = dr["issued_date"].ToString();
                            _STList.receive_date_Bin = dr["receive_date"].ToString();
                            _STList.other_dtl_Bin = dr["other_dtl"].ToString();
                            _STList.issued_qty_Bin = dr["issued_qty"].ToString();
                            _STList.qty_Bin = dr["qty"].ToString();
                            _STList.elsp_days_Bin = dr["elsp_days"].ToString();
                            _STList.remarks_Bin = dr["remarks"].ToString();
                            _SampleTrackingListBinWise.Add(_STList);
                            rowno1 = rowno1 + 1;
                        }
                    }
                    _SampleTracking_Model.SampleTrackingLists_BinWise = _SampleTrackingListBinWise;
                    _SampleTracking_Model.SampleTrackingS = "S_Tracking";
                    //  ViewBag.SWTrackingLists = dt.Tables[0];
                    //ViewBag.SampleWisedata = "SW";
                    if (ReportType == "BW")
                    {
                        _SampleTracking_Model.SampleWisedata = "BW";
                    }
                    else
                    {
                        _SampleTracking_Model.SampleWisedata = "IB";
                    }
                       
                  
                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            SampleTrackingList_sampleWise _STList = new SampleTrackingList_sampleWise();
                            _STList.doc_no_Tracking = dr["doc_no"].ToString();
                            _STList.sr_dt_Tracking = dr["sr_dt"].ToString();
                            _STList.entity_type_Tracking = dr["entity_type"].ToString();
                            _STList.bin_loc_Tracking = dr["bin_loc"].ToString();
                            _STList.sr_type_Tracking = dr["sr_type"].ToString();
                            _STList.entityName_Tracking = dr["entityName"].ToString();
                            _STList.issued_by_Tracking = dr["issued_by"].ToString();
                            _STList.issued_date_Tracking = dr["issued_date"].ToString();
                            _STList.receive_date_Tracking = dr["receive_date"].ToString();
                            _STList.other_dtl_Tracking = dr["other_dtl"].ToString();
                            _STList.issued_qty_Tracking = dr["issued_qty"].ToString();
                            _STList.qty_Tracking = dr["qty"].ToString();
                            _STList.elsp_days_Tracking = dr["elsp_days"].ToString();                         
                            _STList.remarks_Tracking = dr["remarks"].ToString();                         
                            _SampleTrackingListSampleWise.Add(_STList);
                        }
                    }                                   
                    _SampleTracking_Model.SampleTrackingLists_SampleWise = _SampleTrackingListSampleWise;              
                    _SampleTracking_Model.SampleTrackingS = "S_Tracking";
                    //  ViewBag.SWTrackingLists = dt.Tables[0];
                    //ViewBag.SampleWisedata = "SW";
                    _SampleTracking_Model.SampleWisedata = "SW";
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSampleTrackingList.cshtml", _SampleTracking_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public FileResult SampleWiseExcelDownload(string searchValue, string filters,string ReportType,string ShowAs)
        {
            try
            {
                string ExcelName = string.Empty;
                string User_ID = string.Empty;
                string CompID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                var sortColumn = "SrNo";// Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = "asc";// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                DataTable dt = new DataTable();
                if (ReportType =="EW" &&  ShowAs=="S")
                {
                    List<SampleTrackingList> _ItemListModel = new List<SampleTrackingList>();

                    _ItemListModel = getStockDtList(filters);

                    // Getting all Customer data    
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search
                 
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchValue = searchValue.ToUpper();
                            ItemListData = ItemListData.Where(m => m._Entity.ToUpper().Contains(searchValue) || m._EntityID.ToUpper().Contains(searchValue)
                            || m._EntityType.ToUpper().Contains(searchValue) || m._EntityTypeCode.ToUpper().Contains(searchValue) || m.ItemName.ToUpper().Contains(searchValue)
                            || m.ItemID.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.uom_id.ToUpper().Contains(searchValue)
                            || m.Opening_balence.ToUpper().Contains(searchValue) || m.Issued.ToUpper().Contains(searchValue) || m.Receipts.ToUpper().Contains(searchValue)
                            || m.Balance.ToUpper().Contains(searchValue) /*|| m.sr_type.ToUpper().Contains(searchValue) || m.other_dtl.ToUpper().Contains(searchValue)*/
                            // || m.elsp_days.ToUpper().Contains(searchValue)
                            );
                        }
                   
                   

                    var data = ItemListData.ToList();
                   

                    dt = SampleWiseExcel(data);
                    ExcelName = "SampleTrackingEWSummary";
                }
                else if(ReportType == "PS" && ShowAs == "S")
                {
                    List<SampleTrackingList> _ItemListModel1 = new List<SampleTrackingList>();

                    _ItemListModel1 = getStockDtList(filters);

                    // Getting all Customer data    
                    var ItemListData1 = (from tempitem in _ItemListModel1 select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData1 = ItemListData1.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search
                  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData1 = ItemListData1.Where(m => m._Entity.ToUpper().Contains(searchValue) || m._EntityID.ToUpper().Contains(searchValue)
                        || m._EntityType.ToUpper().Contains(searchValue) || m._EntityTypeCode.ToUpper().Contains(searchValue) || m.ItemName.ToUpper().Contains(searchValue)
                        || m.ItemID.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.uom_id.ToUpper().Contains(searchValue)
                        || m.Issued.ToUpper().Contains(searchValue) || m.Receipts.ToUpper().Contains(searchValue)
                        || m.Balance.ToUpper().Contains(searchValue) /*|| m.sr_type.ToUpper().Contains(searchValue) || m.other_dtl.ToUpper().Contains(searchValue)*/
                        // || m.elsp_days.ToUpper().Contains(searchValue)
                        );
                    }
                   
                    var data = ItemListData1.ToList();


                    dt = PendingSampleWiseExcel(data);
                    ExcelName = "PendingSampleTrackingSummary";
                }
                else if (ReportType == "BW" || ReportType == "IB")
                {
                     sortColumn = "SrNo_Bin";
                    List<SampleTrackingList_BinWise> _ItemList_Model = new List<SampleTrackingList_BinWise>();

                    _ItemList_Model = getTrackingBinWiseandIssuedby(filters);
                    var ItemListData1 = (from tempitem1 in _ItemList_Model select tempitem1);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData1 = ItemListData1.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData1 = ItemListData1.Where(m => m.ItemName_Bin.ToUpper().Contains(searchValue) || m.doc_no_Bin.ToUpper().Contains(searchValue) || m.sr_dt_Bin.ToUpper().Contains(searchValue)
                        || m.entity_type_Bin.ToUpper().Contains(searchValue) || m.bin_loc_Bin.ToUpper().Contains(searchValue) || m.sr_type_Bin.ToUpper().Contains(searchValue)
                        || m.entityName_Bin.ToUpper().Contains(searchValue) || m.issued_by_Bin.ToUpper().Contains(searchValue)
                        || m.issued_date_Bin.ToUpper().Contains(searchValue) || m.receive_date_Bin.ToUpper().Contains(searchValue)
                        || m.other_dtl_Bin.ToUpper().Contains(searchValue) || m.issued_qty_Bin.ToUpper().Contains(searchValue) || m.qty_Bin.ToUpper().Contains(searchValue)
                        || m.elsp_days_Bin.ToUpper().Contains(searchValue) || m.remarks_Bin.ToUpper().Contains(searchValue)
                        );
                    }


                    var data1 = ItemListData1.ToList();

                    // var data = ItemListData1.Skip(skip).Take(pageSize).ToList();
                    dt = BinWiseandIssuedbyExcel(data1, ReportType);
                    if (ReportType == "BW")
                    {
                        ExcelName = "BinWise Sample Tracking";
                    }
                    else
                    {
                        ExcelName = "IssuedBy Sample Tracking";
                    }
                        
                 
                }
                else if((ReportType == "EW" || ReportType == "PS") && ShowAs == "D")
                {
                    List<SampleTrackingList> _ItemListModel1 = new List<SampleTrackingList>();

                    _ItemListModel1 = getStockDtList(filters);

                    // Getting all Customer data    
                    var ItemListData1 = (from tempitem in _ItemListModel1 select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData1 = ItemListData1.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData1 = ItemListData1.Where(m => m._Entity.ToUpper().Contains(searchValue)
                       || m.Issue_Number_Summ.ToUpper().Contains(searchValue) || m.issued_dt_Summ.ToUpper().Contains(searchValue)
                        || m._EntityType.ToUpper().Contains(searchValue)  || m.ItemName.ToUpper().Contains(searchValue)
                        || m.ItemID.ToUpper().Contains(searchValue) || m.UOM.ToUpper().Contains(searchValue) || m.uom_id.ToUpper().Contains(searchValue)
                        || m.Issued.ToUpper().Contains(searchValue) || m.Receipts.ToUpper().Contains(searchValue)
                         || m.sr_type.ToUpper().Contains(searchValue) || m.other_dtl.ToUpper().Contains(searchValue)
                         || m.elsp_days.ToUpper().Contains(searchValue)
                        );
                    }

                    var data = ItemListData1.ToList();


                    dt = EWAndPSDetailsWiseExcel(data);
                    ExcelName = "SampleTrackingDetail"+"_"+ReportType ;
                }
                else
                {
                    var sortColumn1 = "SrNo_Tracking";// Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir1 = "asc";// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    List<SampleTrackingList_sampleWise> _ItemListModel = new List<SampleTrackingList_sampleWise>();

                    _ItemListModel = getStockDtList_SampleWise(filters);
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir1)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir1);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => m.doc_no_Tracking.ToUpper().Contains(searchValue) || m.sr_dt_Tracking.ToUpper().Contains(searchValue)
                        || m.entity_type_Tracking.ToUpper().Contains(searchValue) || m.bin_loc_Tracking.ToUpper().Contains(searchValue) || m.sr_type_Tracking.ToUpper().Contains(searchValue)
                        || m.entityName_Tracking.ToUpper().Contains(searchValue) || m.issued_by_Tracking.ToUpper().Contains(searchValue)
                        || m.issued_date_Tracking.ToUpper().Contains(searchValue) || m.receive_date_Tracking.ToUpper().Contains(searchValue)
                        || m.other_dtl_Tracking.ToUpper().Contains(searchValue) || m.issued_qty_Tracking.ToUpper().Contains(searchValue) || m.qty_Tracking.ToUpper().Contains(searchValue)
                        || m.elsp_days_Tracking.ToUpper().Contains(searchValue) 
                        || m.remarks_Tracking.ToUpper().Contains(searchValue)
                        );
                    }
                    var data = ItemListData.ToList();


                    dt = SampleWiseExcel_SW(data);
                     ExcelName = "Sample Tracking SW";
                }
              


                

                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel(ExcelName, dt);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public DataTable SampleWiseExcel(List<SampleTrackingList> _ItemListModel)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Sr No.", typeof(int));
                dataTable.Columns.Add("Entity Type", typeof(string));
                dataTable.Columns.Add("Item Name", typeof(string));
                //dataTable.Columns.Add("Sample Type", typeof(string));
                //dataTable.Columns.Add("Other Detail", typeof(string));
                //dataTable.Columns.Add("Issue Date", typeof(string));
                //dataTable.Columns.Add("Received  Date", typeof(string));
                dataTable.Columns.Add("UOM", typeof(string));
                dataTable.Columns.Add("Entity", typeof(string));
               // dataTable.Columns.Add("Elapsed Days", typeof(string));
                dataTable.Columns.Add("Opening Balance", typeof(decimal));
                dataTable.Columns.Add("Issued", typeof(decimal));
                dataTable.Columns.Add("Receipts", typeof(decimal));
                dataTable.Columns.Add("Balance", typeof(decimal));


                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr No."] = item.SrNo;
                    rows["Entity Type"] = item._EntityType;
                    rows["Item Name"] = item.ItemName;
                    //rows["Sample Type"] = item.sr_type;
                    //rows["Other Detail"] = item.other_dtl;
                    //rows["Issue Date"] = item.issue_date;
                    //rows["Received  Date"] = item.receive_date;
                    rows["UOM"] = item.UOM;
                    rows["Entity"] = item._Entity;
                    //rows["Elapsed Days"] = item.elsp_days;
                    rows["Opening Balance"] = item.Opening_balence;
                    rows["Issued"] = item.Issued;
                    rows["Receipts"] = item.Receipts;
                    rows["Balance"] = item.Balance;
                    dataTable.Rows.Add(rows);
                }

                return dataTable;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
          

        }
        public DataTable EWAndPSDetailsWiseExcel(List<SampleTrackingList> _ItemListModel)
        {
            try
            {

             

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Sr No.", typeof(int));
                dataTable.Columns.Add("Entity Type", typeof(string));
                dataTable.Columns.Add("Entity Name", typeof(string));
                dataTable.Columns.Add("Item Name", typeof(string));
                dataTable.Columns.Add("UOM", typeof(string));
                dataTable.Columns.Add("Transaction Number", typeof(string));
                dataTable.Columns.Add("Transaction Date", typeof(string));
                dataTable.Columns.Add("Bin Location", typeof(string));
                dataTable.Columns.Add("Sample Type", typeof(string));
                dataTable.Columns.Add("Issued By", typeof(string));
                dataTable.Columns.Add("Issue Date", typeof(string));
                dataTable.Columns.Add("Received Date", typeof(string));
                dataTable.Columns.Add("Other Detail", typeof(string));
                dataTable.Columns.Add("Issue Quantity", typeof(decimal));
                dataTable.Columns.Add("Received Quantity", typeof(decimal));
                dataTable.Columns.Add("Balance", typeof(decimal));
                 dataTable.Columns.Add("Elapsed Days", typeof(string));
                dataTable.Columns.Add("Remarks", typeof(string));
                foreach (var item in _ItemListModel)
                {

                    DataRow rows = dataTable.NewRow();
                      rows["Sr No."] = item.SrNo;
                rows["Entity Type"] = item._EntityType;
                rows["Entity Name"] = item._Entity;
                rows["Item Name"] = item.ItemName;
                rows["UOM"] = item.UOM;
                rows["Transaction Number"] = item.Issue_Number_Summ;
                rows["Transaction Date"] = item.issued_dt_Summ;
                rows["Bin Location"] = item.Bin_loc_Summ;
                rows["Sample Type"] = item.sr_type;
                rows["Issued By"] = item.Issued_by_Summ;
                rows["Issue Date"] = item.issue_date;
                rows["Received Date"] = item.receive_date;
                rows["Other Detail"] = item.other_dtl;
                rows["Issue Quantity"] = item.Issued;
                rows["Received Quantity"] = item.Receipts;
                rows["Balance"] = item.Balance;
                 rows["Elapsed Days"] = item.elsp_days;
                rows["Remarks"] = item.Remarks_Summ;
                    dataTable.Rows.Add(rows);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        public DataTable PendingSampleWiseExcel(List<SampleTrackingList> _ItemListModel)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Sr No.", typeof(int));
                dataTable.Columns.Add("Entity Type", typeof(string));
                dataTable.Columns.Add("Item Name", typeof(string));
                //dataTable.Columns.Add("Sample Type", typeof(string));
                //dataTable.Columns.Add("Other Detail", typeof(string));
                //dataTable.Columns.Add("Issue Date", typeof(string));
                //dataTable.Columns.Add("Received  Date", typeof(string));
                dataTable.Columns.Add("UOM", typeof(string));
                dataTable.Columns.Add("Entity", typeof(string));
                // dataTable.Columns.Add("Elapsed Days", typeof(string));
               // dataTable.Columns.Add("Opening Balance", typeof(decimal));
                dataTable.Columns.Add("Issued", typeof(decimal));
                dataTable.Columns.Add("Receipts", typeof(decimal));
                dataTable.Columns.Add("Balance", typeof(decimal));


                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr No."] = item.SrNo;
                    rows["Entity Type"] = item._EntityType;
                    rows["Item Name"] = item.ItemName;
                    //rows["Sample Type"] = item.sr_type;
                    //rows["Other Detail"] = item.other_dtl;
                    //rows["Issue Date"] = item.issue_date;
                    //rows["Received  Date"] = item.receive_date;
                    rows["UOM"] = item.UOM;
                    rows["Entity"] = item._Entity;
                    //rows["Elapsed Days"] = item.elsp_days;
                 //   rows["Opening Balance"] = item.Opening_balence;
                    rows["Issued"] = item.Issued;
                    rows["Receipts"] = item.Receipts;
                    rows["Balance"] = item.Balance;
                    dataTable.Rows.Add(rows);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        public DataTable SampleWiseExcel_SW(List<SampleTrackingList_sampleWise> _ItemListModel)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Sr No.", typeof(int));
                dataTable.Columns.Add("Transaction Number", typeof(string));
                dataTable.Columns.Add("Transaction Date", typeof(string));
                dataTable.Columns.Add("Entity Type", typeof(string));
                dataTable.Columns.Add("Bin Location", typeof(string));
                dataTable.Columns.Add("Sample Type", typeof(string));
                dataTable.Columns.Add("Entity Name", typeof(string));
                dataTable.Columns.Add("Issued By", typeof(string));
                dataTable.Columns.Add("Issue Date", typeof(string));
                dataTable.Columns.Add("Received Date", typeof(string));
                dataTable.Columns.Add("Other Detail", typeof(string));
                dataTable.Columns.Add("Issue Quantity", typeof(decimal));
                dataTable.Columns.Add("Received Quantity", typeof(decimal));
                dataTable.Columns.Add("Elapsed Days", typeof(string));
                dataTable.Columns.Add("Remarks", typeof(string));


                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr No."] = item.SrNo_Tracking;
                    rows["Transaction Number"] = item.doc_no_Tracking;
                    rows["Transaction Date"] = item.sr_dt_Tracking;
                    rows["Entity Type"] = item.entity_type_Tracking;
                    rows["Bin Location"] = item.bin_loc_Tracking;
                    rows["Sample Type"] = item.sr_type_Tracking;
                    rows["Entity Name"] = item.entityName_Tracking;
                    rows["Issued By"] = item.issued_by_Tracking;
                    rows["Issue Date"] = item.issued_date_Tracking;
                    rows["Received Date"] = item.receive_date_Tracking;
                    rows["Other Detail"] = item.other_dtl_Tracking;
                    rows["Issue Quantity"] = item.issued_qty_Tracking;
                    rows["Received Quantity"] = item.qty_Tracking;
                    rows["Elapsed Days"] = item.elsp_days_Tracking;
                    rows["Remarks"] = item.remarks_Tracking;
                    dataTable.Rows.Add(rows);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }

        public DataTable BinWiseandIssuedbyExcel(List<SampleTrackingList_BinWise> _ItemListModel,string ReportType)
        {
            try
            {

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Sr No.", typeof(int));
                if (ReportType == "BW")
                {
                    dataTable.Columns.Add("Bin Location", typeof(string));
                }
                else
                {
                    dataTable.Columns.Add("Issued By", typeof(string));
                }
              
                dataTable.Columns.Add("Item Name", typeof(string));
                dataTable.Columns.Add("UOM", typeof(string));
                dataTable.Columns.Add("Transaction Number", typeof(string));
                dataTable.Columns.Add("Transaction Date", typeof(string));
                dataTable.Columns.Add("Entity Type", typeof(string));
                if (ReportType == "IB")
                {
                    dataTable.Columns.Add("Bin Location", typeof(string));
                }
                dataTable.Columns.Add("Sample Type", typeof(string));
                dataTable.Columns.Add("Entity Name", typeof(string));
                if (ReportType == "BW")
                {
                    dataTable.Columns.Add("Issued By", typeof(string));
                }
                dataTable.Columns.Add("Issue Date", typeof(string));
                dataTable.Columns.Add("Received Date", typeof(string));
                dataTable.Columns.Add("Other Detail", typeof(string));
                dataTable.Columns.Add("Issue Quantity", typeof(decimal));
                dataTable.Columns.Add("Received Quantity", typeof(decimal));
                dataTable.Columns.Add("Elapsed Days", typeof(string));
                dataTable.Columns.Add("Remarks", typeof(string));


                foreach (var item in _ItemListModel)
                {
                    DataRow rows = dataTable.NewRow();
                    rows["Sr No."] = item.SrNo_Bin;
                    if (ReportType == "BW")
                    {
                        rows["Bin Location"] = item.bin_loc_Bin;
                    }
                    else
                    {
                        rows["Issued By"] = item.issued_by_Bin;
                    }
                    rows["Item Name"] = item.ItemName_Bin;
                    rows["UOM"] = item.Uom_Bin;
                    rows["Transaction Number"] = item.doc_no_Bin;
                    rows["Transaction Date"] = item.sr_dt_Bin;
                    rows["Entity Type"] = item.entity_type_Bin;
                    if (ReportType == "IB")
                    {
                        rows["Bin Location"] = item.bin_loc_Bin;
                    }
                    rows["Sample Type"] = item.sr_type_Bin;
                    rows["Entity Name"] = item.entityName_Bin;
                    if (ReportType == "BW")
                    {
                        rows["Issued By"] = item.issued_by_Bin;
                    }
                    rows["Issue Date"] = item.issued_date_Bin;
                    rows["Received Date"] = item.receive_date_Bin;
                    rows["Other Detail"] = item.other_dtl_Bin;
                    rows["Issue Quantity"] = item.issued_qty_Bin;
                    rows["Received Quantity"] = item.qty_Bin;
                    rows["Elapsed Days"] = item.elsp_days_Bin;
                    rows["Remarks"] = item.remarks_Bin;
                    dataTable.Rows.Add(rows);
                }

                return dataTable;
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


