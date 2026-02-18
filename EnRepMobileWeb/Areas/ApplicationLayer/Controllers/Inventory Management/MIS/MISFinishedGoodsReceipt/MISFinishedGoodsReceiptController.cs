using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISFinishedGoodsReceipt;
using System.Collections.Generic;
using System.Linq;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISFinishedGoodsReceipt;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.FinishedGoodsReceipt;
using EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialReceipt.FinishedGoodsReceipt;
using System.Web.UI.WebControls;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.MISFinishedGoodsReceipt
{
    public class MISFinishedGoodsReceiptController : Controller
    {
        string CompID, language, BrchID, UserID = String.Empty;
        string DocumentMenuId/* = "105102180140"*/, title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MISFinishedGoodsReceipt_ISERVICES _FGRMIS;
        FinishedGoodsReceipt_SERVICE _FGR;
        private readonly GeneralLedger_ISERVICE _GeneralLedger_ISERVICE;
        public MISFinishedGoodsReceiptController(Common_IServices _Common_IServices, MISFinishedGoodsReceipt_ISERVICES _FGRMIS, GeneralLedger_ISERVICE GeneralLedger_ISERVICE, FinishedGoodsReceipt_SERVICE _FGR)
        {
            this._Common_IServices = _Common_IServices;
            this._FGRMIS = _FGRMIS;
            this._FGR = _FGR;
            _GeneralLedger_ISERVICE = GeneralLedger_ISERVICE;
        }
        public void CompDataWithID()
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
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        // GET: ApplicationLayer/MISFinishedGoodsReceipt

        public ActionResult MISFinishedGoodsReceipt()
        {
            DocumentMenuId = "105102180140";
            MISFinishedGoodsReceipt_Model MISFGRModel = new MISFinishedGoodsReceipt_Model();
            CommonPageDetails();
            DataSet dttbl = new DataSet();
            DateTime dtnow = DateTime.Now;
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                MISFGRModel.Fromdate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string FromDate = MISFGRModel.Fromdate;
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            MISFGRModel.ToDate = ToDate;
            GetItemList(MISFGRModel);
            GetItemGrpList(MISFGRModel);
            BindShopflore(MISFGRModel);
            // StatusBind(MISFGRModel);
            MISFGRModel.ShowAs = "P";
            ViewBag.ShowAs = "P";
            DataTable dt = _FGRMIS.GetDataTableMISFGR(CompID, BrchID, "", "", MISFGRModel.Fromdate, MISFGRModel.ToDate, "", "", "P");
            ViewBag.MISFGRData = dt;
            MISFGRModel.Title = title;
            MISFGRModel.DocumentMenuId = DocumentMenuId;
            // ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/MISFinishedGoodsReceipt/MISFinishedGoodsReceiptDetail.cshtml", MISFGRModel);
        }
        [HttpPost]
        public JsonResult LoadData1(string ItemListFilter, string ReportType)
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn1 = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();
                //  var pageValue = Request.Form.GetValues("paginate_button active[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (ReportType == "P")
                {
                    List<MISFGRTableList> _ItemListModel = new List<MISFGRTableList>();

                    _ItemListModel = GetMISFGR(ItemListFilter);
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData =

                            ItemListData.Where(m => m.item_name.ToUpper().Contains(searchValue) || m.item_id.ToUpper().Contains(searchValue)
                        || m.uom_alias.ToUpper().Contains(searchValue) || m.out_qty.ToUpper().Contains(searchValue) || m.Qty_in_Sp.ToUpper().Contains(searchValue)
                        || m.shfl_name.ToUpper().Contains(searchValue) || m.shfl_id.ToUpper().Contains(searchValue) || m.rcpt_no.ToUpper().Contains(searchValue)
                        || m.rcpt_dt.ToUpper().Contains(searchValue) || m.batch_no.ToUpper().Contains(searchValue) || m.lot_no.ToUpper().Contains(searchValue)
                        || m.exp_dt.ToUpper().Contains(searchValue) || m.cost_price.ToUpper().Contains(searchValue) || m.total_value.ToUpper().Contains(searchValue)
                       );
                    }


                    recordsTotal = ItemListData.Count();

                    var data = ItemListData.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else
                {
                    List<MISFGRTableList> _ItemListModel = new List<MISFGRTableList>();
                    _ItemListModel = GetMISFGRDocumentWise(ItemListFilter);
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn1) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn1 + " " + sortColumnDir);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => (m.rcpt_no.ToUpper().Contains(searchValue)
                        || m.rcpt_dt.ToUpper().Contains(searchValue) ||
                          m.input_item_name.ToUpper().Contains(searchValue) ||
                           m.input_item_id.ToUpper().Contains(searchValue) ||
                             m.input_uom_name.ToUpper().Contains(searchValue) ||
                                m.input_uom_id.ToUpper().Contains(searchValue) ||
                                m.cons_qty.ToUpper().Contains(searchValue) ||
                        m.item_name.ToUpper().Contains(searchValue)
                        || m.item_id.ToUpper().Contains(searchValue)
                        || m.uom_alias.ToUpper().Contains(searchValue) ||
                        m.out_qty.ToUpper().Contains(searchValue)
                        || m.Qty_in_Sp.ToUpper().Contains(searchValue)
                        || m.shfl_name.ToUpper().Contains(searchValue) ||
                        m.shfl_id.ToUpper().Contains(searchValue)
                        || m.batch_no.ToUpper().Contains(searchValue)
                        || m.lot_no.ToUpper().Contains(searchValue)
                        || m.exp_dt.ToUpper().Contains(searchValue) ||
                        m.cost_price.ToUpper().Contains(searchValue) ||
                        m.total_value.ToUpper().Contains(searchValue))
                        && m.AbcNull != "NotValid"

                        );
                    }


                    recordsTotal = ItemListData.Where(m => m.AbcNull != "NotValid").Count();

                    var data = ItemListData.Where(m => m.AbcNull != "NotValid").Skip(skip).Take(pageSize).ToList();
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
        private List<MISFGRTableList> GetMISFGR(string ItemListFilter)
        {
            string itmid, GroupID, txtFromdate, txtTodate, MultiselectItemHdn, ShopFloor_id, ddl_ShowAs = "";
            List<MISFGRTableList> _ItemListModel = new List<MISFGRTableList>();
            CommonController cmn = new CommonController();
            try
            {
                CompDataWithID();
                DataTable DSet = new DataTable();
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        //fdata = Fstring.Split(',');
                        fdata = Fstring.Split('_');

                        //itmid = fdata[0];
                        //GroupID = fdata[1];
                        //txtFromdate = fdata[2];
                        //txtTodate = fdata[3];
                        //MultiselectItemHdn = fdata[4];
                        //ShopFloor_id = fdata[5];
                        //ddl_ShowAs = fdata[6];

                        itmid = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        GroupID = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        txtFromdate = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        txtTodate = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        MultiselectItemHdn = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        ShopFloor_id = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        ddl_ShowAs = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));

                        DSet = _FGRMIS.GetDataTableMISFGR(CompID, BrchID, itmid, GroupID, txtFromdate, txtTodate, MultiselectItemHdn, ShopFloor_id, ddl_ShowAs);
                    }
                    else
                    {

                    }
                }
                else
                {

                }
                if (DSet.Rows.Count > 0)
                {
                    int rowno = 0;

                    foreach (DataRow dr in DSet.Rows)
                    {
                        MISFGRTableList _STList = new MISFGRTableList();
                        _STList.SrNo = rowno + 1;
                        _STList.item_name = dr["item_name"].ToString().Trim();
                        _STList.item_id = dr["item_id"].ToString().Trim();
                        _STList.uom_alias = dr["uom_alias"].ToString().Trim();
                        _STList.out_qty = dr["out_qty"].ToString().Trim();
                        _STList.Qty_in_Sp = dr["Qty_in_Sp"].ToString().Trim();
                        _STList.shfl_name = dr["shfl_name"].ToString().Trim();
                        _STList.shfl_id = dr["shfl_id"].ToString().Trim();
                        _STList.rcpt_no = dr["rcpt_no"].ToString().Trim();
                        _STList.rcpt_dt = dr["rcpt_dt"].ToString().Trim();
                        _STList.batch_no = dr["batch_no"].ToString().Trim();
                        _STList.lot_no = dr["lot_no"].ToString().Trim();
                        _STList.exp_dt = dr["exp_dt"].ToString().Trim();
                        _STList.cost_price = dr["cost_price"].ToString().Trim();
                        _STList.total_value = dr["total_value"].ToString().Trim();
                        _ItemListModel.Add(_STList);
                        rowno = rowno + 1;
                    }
                }
                return _ItemListModel;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }


        private List<MISFGRTableList> GetMISFGRDocumentWise(string ItemListFilter)
        {
            string itmid, GroupID, txtFromdate, txtTodate, MultiselectItemHdn, ShopFloor_id, ddl_ShowAs = "";
            List<MISFGRTableList> _ItemListModel = new List<MISFGRTableList>();
            List<MISFGRTableList> _ItemListModel1 = new List<MISFGRTableList>();
            CommonController cmn = new CommonController();
            try
            {
            
                CompDataWithID();
                DataTable DSet = new DataTable();
                if (ItemListFilter != null)
                {
                    if (ItemListFilter != "")
                    {
                        string Fstring = string.Empty;
                        string[] fdata;
                        Fstring = ItemListFilter;
                        //fdata = Fstring.Split(',');
                        fdata = Fstring.Split('_');

                        //itmid = fdata[0];
                        //GroupID = fdata[1];
                        //txtFromdate = fdata[2];
                        //txtTodate = fdata[3];
                        //MultiselectItemHdn = fdata[4];
                        //ShopFloor_id = fdata[5];
                        //ddl_ShowAs = fdata[6];
                        itmid = cmn.ReplaceDefault(fdata[0].Trim('[', ']'));
                        GroupID = cmn.ReplaceDefault(fdata[1].Trim('[', ']'));
                        txtFromdate = cmn.ReplaceDefault(fdata[2].Trim('[', ']'));
                        txtTodate = cmn.ReplaceDefault(fdata[3].Trim('[', ']'));
                        MultiselectItemHdn = cmn.ReplaceDefault(fdata[4].Trim('[', ']'));
                        ShopFloor_id = cmn.ReplaceDefault(fdata[5].Trim('[', ']'));
                        ddl_ShowAs = cmn.ReplaceDefault(fdata[6].Trim('[', ']'));

                        DSet = _FGRMIS.GetDataTableMISFGR(CompID, BrchID, itmid, GroupID, txtFromdate, txtTodate, MultiselectItemHdn, ShopFloor_id, ddl_ShowAs);
                    }
                    else
                    {

                    }
                }
                else
                {

                }
                if (DSet.Rows.Count > 0)
                {
                    int rowno = 0;

                    int count = 0;
                    foreach (DataRow dr in DSet.Rows)
                    {
                        MISFGRTableList _STList = new MISFGRTableList();
                        MISFGRTableList _STList1 = new MISFGRTableList();

                        _STList1.SNO_rcptno = dr["SNO_rcptno"].ToString().Trim();
                        _STList1.Count_rcpt_no = dr["Count_rcpt_no"].ToString().Trim();
                        _STList1.SNO_INPUT = dr["SNO_INPUT"].ToString().Trim();
                        _STList1.count_input = dr["count_input"].ToString().Trim();
                        _STList1.SNO_OUTPUT = dr["SNO_OUTPUT"].ToString().Trim();
                        _STList1.count_output = dr["count_output"].ToString().Trim();


                        if (_STList1.SNO_rcptno == "1")
                        {
                            _STList.SrNo1 = (rowno + 1).ToString();
                            _STList.rcpt_no = dr["rcpt_no"].ToString().Trim();
                            _STList.rcpt_dt = dr["rcpt_dt"].ToString().Trim();

                            _STList.shfl_name = dr["shfl_name"].ToString().Trim();
                            _STList.shfl_id = dr["shfl_id"].ToString().Trim();
                            _STList.rowspanrcptno = "";
                            rowno = rowno + 1;
                        }
                        else
                        {
                            if (_STList1.Count_rcpt_no == "1")
                            {
                                _STList.SrNo1 = (rowno + 1).ToString();
                                _STList.rcpt_no = dr["rcpt_no"].ToString().Trim();
                                _STList.rcpt_dt = dr["rcpt_dt"].ToString().Trim();
                                _STList.shfl_name = dr["shfl_name"].ToString().Trim();
                                _STList.shfl_id = dr["shfl_id"].ToString().Trim();
                                _STList.rowspanrcptno = _STList.SNO_rcptno;
                                rowno = rowno + 1;

                            }
                            else
                            {
                                _STList.SrNo1 = "";
                                _STList.rcpt_no = "";
                                _STList.rcpt_dt = "";
                                _STList.shfl_name = "";
                                _STList.shfl_id = "";
                                _STList.rowspanrcptno = "";

                            }

                        }
                        if (_STList1.SNO_INPUT == "1")
                        {
                            _STList.input_item_name = dr["input_item_name"].ToString().Trim();
                            _STList.input_item_id = dr["input_item_id"].ToString().Trim();
                            _STList.input_uom_name = dr["input_uom_name"].ToString().Trim();
                            _STList.input_uom_id = dr["input_uom_id"].ToString().Trim();
                            _STList.cons_qty = dr["cons_qty"].ToString().Trim();
                            _STList.Batchable = dr["batchable"].ToString().Trim();
                            _STList.rowspanInputItem = "";
                            _STList.Doc_no = dr["rcpt_no"].ToString().Trim();
                            _STList.Doc_dt = dr["Doc_dt"].ToString().Trim();
                        }
                        else
                        {
                            if (_STList1.count_input == "1")
                            {
                                _STList.input_item_name = dr["input_item_name"].ToString().Trim();
                                _STList.input_item_id = dr["input_item_id"].ToString().Trim();
                                _STList.input_uom_name = dr["input_uom_name"].ToString().Trim();
                                _STList.input_uom_id = dr["input_uom_id"].ToString().Trim();
                                _STList.cons_qty = dr["cons_qty"].ToString().Trim();
                                _STList.Batchable = dr["batchable"].ToString().Trim();
                                _STList.rowspanInputItem = _STList.SNO_INPUT;
                                _STList.Doc_no = dr["rcpt_no"].ToString().Trim();
                                _STList.Doc_dt = dr["Doc_dt"].ToString().Trim();
                            }
                            else
                            {
                                _STList.input_item_name = "";
                                _STList.input_item_id = "";
                                _STList.input_uom_name = "";
                                _STList.input_uom_id = "";
                                _STList.cons_qty = "";
                                _STList.Batchable = "";
                                _STList.rowspanInputItem = "";
                                _STList.Doc_no = "";
                                _STList.Doc_dt = "";
                            }
                        }
                        if (_STList1.SNO_OUTPUT == "1")
                        {
                            _STList.item_name = dr["item_name"].ToString().Trim();
                            _STList.item_name = dr["item_name"].ToString().Trim();
                            _STList.item_id = dr["item_id"].ToString().Trim();
                            _STList.uom_alias = dr["uom_alias"].ToString().Trim();
                            _STList.out_qty = dr["out_qty"].ToString().Trim();
                            _STList.Qty_in_Sp = dr["Qty_in_Sp"].ToString().Trim();
                            _STList.batch_no = dr["batch_no"].ToString().Trim();
                            _STList.lot_no = dr["lot_no"].ToString().Trim();
                            _STList.exp_dt = dr["exp_dt"].ToString().Trim();
                            _STList.cost_price = dr["cost_price"].ToString().Trim();
                            _STList.total_value = dr["total_value"].ToString().Trim();
                            _STList.rowspanOutputItem = "";
                        }
                        else
                        {
                            if (_STList1.count_output == "1")
                            {
                                _STList.item_name = dr["item_name"].ToString().Trim();
                                _STList.item_name = dr["item_name"].ToString().Trim();
                                _STList.item_id = dr["item_id"].ToString().Trim();
                                _STList.uom_alias = dr["uom_alias"].ToString().Trim();
                                _STList.out_qty = dr["out_qty"].ToString().Trim();
                                _STList.Qty_in_Sp = dr["Qty_in_Sp"].ToString().Trim();
                                _STList.batch_no = dr["batch_no"].ToString().Trim();
                                _STList.lot_no = dr["lot_no"].ToString().Trim();
                                _STList.exp_dt = dr["exp_dt"].ToString().Trim();
                                _STList.cost_price = dr["cost_price"].ToString().Trim();
                                _STList.total_value = dr["total_value"].ToString().Trim();
                                _STList.rowspanOutputItem = _STList.SNO_OUTPUT;
                            }
                            else
                            {
                                _STList.item_name = "";
                                _STList.item_name = "";
                                _STList.item_id = "";
                                _STList.uom_alias = "";
                                _STList.out_qty = "";
                                _STList.Qty_in_Sp = "";
                                _STList.batch_no = "";
                                _STList.lot_no = "";
                                _STList.exp_dt = "";
                                _STList.cost_price = "";
                                _STList.total_value = "";
                                _STList.rowspanOutputItem = "";
                            }
                        }
                        if ((_STList.rcpt_no != null && _STList.rcpt_no != "") ||
                             (_STList.input_item_id != null && _STList.input_item_id != "") ||
                              (_STList.item_id != null && _STList.item_id != ""))
                        {
                            _STList.AbcNull = "Valid";
                        }
                        else
                        {
                            _STList.AbcNull = "NotValid";
                        }

                        _ItemListModel.Add(_STList);

                    }
                }

                return _ItemListModel;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult MISFinishedGoodsReceiptPPL()
        {
            DocumentMenuId = "105105155135";
            MISFinishedGoodsReceipt_Model MISFGRModel = new MISFinishedGoodsReceipt_Model();
            CommonPageDetails();
            DataSet dttbl = new DataSet();
            DateTime dtnow = DateTime.Now;
            dttbl = GetFyList();
            if (dttbl.Tables[0].Rows.Count > 0 && dttbl.Tables[1].Rows.Count > 0)
            {
                MISFGRModel.Fromdate = dttbl.Tables[0].Rows[0]["currfy_startdt"].ToString();
                ViewBag.FromFyMindate = dttbl.Tables[0].Rows[0]["fy_startdt"].ToString();
                ViewBag.ToFyMaxdate = dttbl.Tables[0].Rows[0]["fy_enddt"].ToString();

                ViewBag.fylist = dttbl.Tables[1];
            }
            string FromDate = MISFGRModel.Fromdate;
            string ToDate = dtnow.ToString("yyyy-MM-dd");
            MISFGRModel.ToDate = ToDate;
            GetItemList(MISFGRModel);
            GetItemGrpList(MISFGRModel);
            BindShopflore(MISFGRModel);
            MISFGRModel.ShowAs = "P";
            ViewBag.ShowAs = "P";
            // StatusBind(MISFGRModel);
            DataTable dt = _FGRMIS.GetDataTableMISFGR(CompID, BrchID, "", "", MISFGRModel.Fromdate, MISFGRModel.ToDate, "", "", "P");
            ViewBag.MISFGRData = dt;
            MISFGRModel.Title = title;
            MISFGRModel.DocumentMenuId = DocumentMenuId;
            // ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/MISFinishedGoodsReceipt/MISFinishedGoodsReceiptDetail.cshtml", MISFGRModel);
        }
        public void BindShopflore(MISFinishedGoodsReceipt_Model MISFGRModel)
        {
            CompDataWithID();
            DataSet dt = _FGR.GetAllDropDownList(CompID, BrchID);
            List<ShopfloorListDropDown> list = new List<ShopfloorListDropDown>();
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                ShopfloorListDropDown shp = new ShopfloorListDropDown();
                shp.shop_id = dr["shfl_id"].ToString();
                shp.shop_name = dr["shfl_name"].ToString();
                list.Add(shp);
            }
            // list.Insert(0, new ShopfloorListDropDown() { shop_id = "0", shop_name = "All" });
            MISFGRModel.ShopfloorList = list;
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
        //private string getDocumentName()
        //{
        //    try
        //    {
        //        CompDataWithID();
        //        string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //        string[] Docpart = DocumentName.Split('>');
        //        int len = Docpart.Length;
        //        if (len > 1)
        //        {
        //            title = Docpart[len - 1].Trim();
        //        }
        //        return DocumentName;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
        public ActionResult GetItemList(MISFinishedGoodsReceipt_Model queryParameters)
        {
            CompDataWithID();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ItemName))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.ItemName;
                }
                ItemList = _FGRMIS.ItemList(ItemName, CompID, BrchID);

                List<ItemName> _ItemList = new List<ItemName>();
                foreach (var data in ItemList)
                {
                    ItemName _ItemDetail = new ItemName();
                    _ItemDetail.Item_Id = data.Key;
                    _ItemDetail.Item_Name = data.Value;
                    _ItemList.Add(_ItemDetail);
                }
                queryParameters.ItemNameList = _ItemList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemGrpList(MISFinishedGoodsReceipt_Model queryParameters)
        {
            CompDataWithID();
            string GroupName = string.Empty;
            Dictionary<string, string> ItemGroupList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.GroupName))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.GroupName;
                }
                ItemGroupList = _FGRMIS.ItemGroupList(GroupName, CompID);

                List<ItemGroupName> _ItemGroupList = new List<ItemGroupName>();
                foreach (var data in ItemGroupList)
                {
                    ItemGroupName _ItemGroupDetail = new ItemGroupName();
                    _ItemGroupDetail.Group_Id = data.Key;
                    _ItemGroupDetail.Group_Name = data.Value;
                    _ItemGroupList.Add(_ItemGroupDetail);
                }
                queryParameters.ItemGroupNameList = _ItemGroupList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemGroupList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public void StatusBind(MISFinishedGoodsReceipt_Model objModel)
        {

            List<Status> list2 = new List<Status>();
            foreach (var dr in ViewBag.StatusList.Rows)
            {
                if (dr["status_code"] == "0" || dr["status_code"] == "F" || dr["status_code"] == "C" || dr["status_code"] == "D")
                {

                }
                else
                {
                    Status Status = new Status();
                    Status.status_id = dr["status_code"].ToString();
                    Status.status_name = dr["status_name"].ToString();
                    list2.Add(Status);
                }

            }
            objModel.StatusList = list2;
        }
        private void CommonPageDetails()
        {
            try
            {
                CompDataWithID();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, "105102180140", language);
                //ViewBag.AppLevel = ds.Tables[0];
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                //ViewBag.VBRoleList = ds.Tables[3];
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
        public ActionResult SerchDataMIS(string itmid, string GroupID, string txtFromdate, string txtTodate,
            string MultiselectItemHdn, string ShopFloor_id, string ddl_ShowAs)
        {
            MISFinishedGoodsReceipt_Model Search_Model = new MISFinishedGoodsReceipt_Model();
            CompDataWithID();
            Search_Model.SearchParaMeter = "SearchData";
            DataTable dt = new DataTable();
            dt = _FGRMIS.GetDataTableMISFGR(CompID, BrchID, itmid, GroupID, txtFromdate, txtTodate, MultiselectItemHdn, ShopFloor_id, ddl_ShowAs);
            ViewBag.MISFGRData = dt;
            ViewBag.ShowAs = ddl_ShowAs;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISFinishedGoodsReceipt.cshtml", Search_Model);
        }
        public ActionResult GetDataBatchDeatil(string rcpt_no, string rcpt_dt, string item_id, string DocumentMenuId)
        {
            try
            {
                CompDataWithID();
                DataSet GetDataBatchDeatil = new DataSet();
                GetDataBatchDeatil = _FGRMIS.GetBatchDeatilData(CompID, BrchID, rcpt_no, rcpt_dt, item_id);
                if (GetDataBatchDeatil.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = GetDataBatchDeatil.Tables[0];
                ViewBag.Transtype = "Update";
                ViewBag.Command = "Refresh";
                ViewBag.DocID = DocumentMenuId;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }

        public FileResult ExcelDownload(string searchValue, string filters, string ReportType)
        {
            try
            {
                CompDataWithID();
                var sortColumn = "SrNo";// Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = "asc";// Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                DataTable dt = new DataTable();
                if (ReportType == "P")
                {
                    List<MISFGRTableList> _ItemListModel = new List<MISFGRTableList>();

                    _ItemListModel = GetMISFGR(filters);
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => m.item_name.ToUpper().Contains(searchValue) || m.item_id.ToUpper().Contains(searchValue)
                        || m.uom_alias.ToUpper().Contains(searchValue) || m.out_qty.ToUpper().Contains(searchValue) || m.Qty_in_Sp.ToUpper().Contains(searchValue)
                        || m.shfl_name.ToUpper().Contains(searchValue) || m.shfl_id.ToUpper().Contains(searchValue) || m.rcpt_no.ToUpper().Contains(searchValue)
                        || m.rcpt_dt.ToUpper().Contains(searchValue) || m.batch_no.ToUpper().Contains(searchValue) || m.lot_no.ToUpper().Contains(searchValue)
                        || m.exp_dt.ToUpper().Contains(searchValue) || m.cost_price.ToUpper().Contains(searchValue) || m.total_value.ToUpper().Contains(searchValue)
                       );
                    }


                    var data = ItemListData.ToList();

                      dt = FGRProductWiseExcel(data, ReportType);

                }
                else
                {
                    List<MISFGRTableList> _ItemListModel = new List<MISFGRTableList>();
                    _ItemListModel = GetMISFGRDocumentWise(filters);
                    var ItemListData = (from tempitem in _ItemListModel select tempitem);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        searchValue = searchValue.ToUpper();
                        ItemListData = ItemListData.Where(m => (m.rcpt_no.ToUpper().Contains(searchValue)
                        || m.rcpt_dt.ToUpper().Contains(searchValue) ||
                          m.input_item_name.ToUpper().Contains(searchValue) ||
                           m.input_item_id.ToUpper().Contains(searchValue) ||
                             m.input_uom_name.ToUpper().Contains(searchValue) ||
                                m.input_uom_id.ToUpper().Contains(searchValue) ||
                                m.cons_qty.ToUpper().Contains(searchValue) ||
                        m.item_name.ToUpper().Contains(searchValue)
                        || m.item_id.ToUpper().Contains(searchValue)
                        || m.uom_alias.ToUpper().Contains(searchValue) ||
                        m.out_qty.ToUpper().Contains(searchValue)
                        || m.Qty_in_Sp.ToUpper().Contains(searchValue)
                        || m.shfl_name.ToUpper().Contains(searchValue) ||
                        m.shfl_id.ToUpper().Contains(searchValue)
                        || m.batch_no.ToUpper().Contains(searchValue)
                        || m.lot_no.ToUpper().Contains(searchValue)
                        || m.exp_dt.ToUpper().Contains(searchValue) ||
                        m.cost_price.ToUpper().Contains(searchValue) ||
                        m.total_value.ToUpper().Contains(searchValue))
                        && m.AbcNull != "NotValid"

                        );
                    }


                    var data = ItemListData.Where(m => m.AbcNull != "NotValid").ToList();
                    dt = FGRProductWiseExcel(data, ReportType);
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Finished Good Receipt", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable FGRProductWiseExcel(List<MISFGRTableList> _ItemListModel,string ReportType)
        {
            try
            {
                DataTable dataTable = new DataTable();

                if(ReportType=="P")
                {
                    dataTable.Columns.Add("Sr No.", typeof(int));
                    dataTable.Columns.Add("Product Name", typeof(string));
                    dataTable.Columns.Add("UOM", typeof(string));
                    dataTable.Columns.Add("Receipt Quantity", typeof(decimal));
                    dataTable.Columns.Add("Receipt Quantity(In Specific)", typeof(string));
                    dataTable.Columns.Add("Shopfloor Name", typeof(string));
                    dataTable.Columns.Add("Receipt Number", typeof(string));
                    dataTable.Columns.Add("Receipt Date", typeof(string));
                    dataTable.Columns.Add("Batch Number", typeof(string));
                    dataTable.Columns.Add("Lot#", typeof(string));
                    dataTable.Columns.Add("Expiry Date", typeof(string));
                    dataTable.Columns.Add("Cost Price", typeof(decimal));
                    dataTable.Columns.Add("Value", typeof(decimal));


                    foreach (var item in _ItemListModel)
                    {
                        DataRow rows = dataTable.NewRow();
                        rows["Sr No."] = item.SrNo;
                        rows["Product Name"] = item.item_name;
                        rows["UOM"] = item.uom_alias;
                        rows["Receipt Quantity"] = item.out_qty;
                        rows["Receipt Quantity(In Specific)"] = item.Qty_in_Sp;
                        rows["Shopfloor Name"] = item.shfl_name;
                        rows["Receipt Number"] = item.rcpt_no;
                        rows["Receipt Date"] = item.rcpt_dt;
                        rows["Batch Number"] = item.batch_no;
                        rows["Lot#"] = item.lot_no;
                        rows["Expiry Date"] = item.exp_dt;
                        rows["Cost Price"] = item.cost_price;
                        rows["Value"] = item.total_value;
                        dataTable.Rows.Add(rows);
                    }
                }
                else
                {
                    dataTable.Columns.Add("Sr No.", typeof(string));
                    dataTable.Columns.Add("Receipt Number", typeof(string));
                    dataTable.Columns.Add("Receipt Date", typeof(string));
                    dataTable.Columns.Add("Shopfloor Name", typeof(string));
                    dataTable.Columns.Add("Input Material Name", typeof(string));
                    dataTable.Columns.Add("Input Material UOM", typeof(string));                
                    dataTable.Columns.Add("Consumed Quantity", typeof(string));
                    dataTable.Columns.Add("Output Product Name", typeof(string));
                    dataTable.Columns.Add("Output Product UOM", typeof(string));
                    dataTable.Columns.Add("Receipt Quantity", typeof(string));
                    dataTable.Columns.Add("Receipt Quantity(In Specific)", typeof(string));                                    
                    dataTable.Columns.Add("Batch Number", typeof(string));
                    dataTable.Columns.Add("Lot#", typeof(string));
                    dataTable.Columns.Add("Expiry Date", typeof(string));
                    dataTable.Columns.Add("Cost Price", typeof(string));
                    dataTable.Columns.Add("Value", typeof(string));


                    foreach (var item in _ItemListModel)
                    {
                        DataRow rows = dataTable.NewRow();
                       if( item.rcpt_no =="" || item.rcpt_no == null)
                        {
                            rows["Sr No."] = item.SrNo1;
                        }
                        else
                        {
                            rows["Sr No."] = item.SrNo1;
                        }
                       
                        rows["Receipt Number"] = item.rcpt_no;
                        rows["Receipt Date"] = item.rcpt_dt;
                        rows["Shopfloor Name"] = item.shfl_name;
                        rows["Input Material Name"] = item.input_item_name;
                        rows["Input Material UOM"] = item.input_uom_name;
                        rows["Consumed Quantity"] = item.cons_qty;
                        rows["Output Product Name"] = item.item_name;
                        rows["Output Product UOM"] = item.uom_alias;
                        rows["Receipt Quantity"] = item.out_qty;
                        rows["Receipt Quantity(In Specific)"] = item.Qty_in_Sp;                                           
                        rows["Batch Number"] = item.batch_no;
                        rows["Lot#"] = item.lot_no;
                        rows["Expiry Date"] = item.exp_dt;
                        rows["Cost Price"] = item.cost_price;
                        rows["Value"] = item.total_value;
                        dataTable.Rows.Add(rows);
                    }
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