using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockReservation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.StockReservation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.StockReservation
{
    public class StockReservationController : Controller
    {
        string  language = string.Empty;
        string DocumentMenuId = "105102153";
        string CompID, BrchID, BranchName,  title, userid = string.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        StockReservation_ISERVICES _StockReservation_ISERVICES;
        DataTable dt;
        DataSet dtSet;
        StockReservation_Model _StockReservation_Model;
        public StockReservationController(Common_IServices _Common_IServices, StockReservation_ISERVICES _StockReservation_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._StockReservation_ISERVICES = _StockReservation_ISERVICES;
        }
        // GET: ApplicationLayer/StockReservation
        public ActionResult AddStockReservation()
        {
          
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            StockReservation_Model _StockReservationaddnew_Model = new StockReservation_Model();
            _StockReservationaddnew_Model.Message= "New";
            _StockReservationaddnew_Model.Command = "Add";
            _StockReservationaddnew_Model.AppStatus = "D";
            _StockReservationaddnew_Model.TransType = "Save";
            _StockReservationaddnew_Model.BtnName = "BtnAddNew";
            TempData["ModelData"] = _StockReservationaddnew_Model;
            UrlModel _urlmodel = new UrlModel();
            _urlmodel.TransType = "Save";
            _urlmodel.BtnName = "BtnAddNew";
            _urlmodel.Command = "New";
            /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                _StockReservationaddnew_Model.TransType = "Refresh";
                _StockReservationaddnew_Model.Command = "Refresh";
                _StockReservationaddnew_Model.BtnName = "Refresh";
                _urlmodel.BtnName = "Refresh";
                _urlmodel.TransType = "Refresh";
                _urlmodel.Command = "Refresh";
                //_DeliveryNoteDetail_MODELS.Message = "Financial Year not Exist";
                return RedirectToAction("StockReservation", "StockReservation", _urlmodel);
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("StockReservation", "StockReservation", _urlmodel);
        }
        public ActionResult StockReservation(UrlModel _urlmodel)
        {
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
            try
            {
                var STKModelData = TempData["ModelData"] as StockReservation_Model;
                if (STKModelData != null)
                {
                    CommonPageDetails();
                    //var other = new CommonController(_Common_IServices);
                    STKModelData.to_br = BranchName;
                    STKModelData.to_brid = BrchID;
                    /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                    //dt = GetWHList(BrchID);
                    //List<Warehouse> _WarehouseList = new List<Warehouse>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    Warehouse _Warehouse = new Warehouse();
                    //    _Warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _Warehouse.wh_name = dr["wh_name"].ToString();
                    //    _WarehouseList.Add(_Warehouse);
                    //}
                    //_WarehouseList.Insert(0, new Warehouse() { wh_id = 0, wh_name = "---Select---" });
                    //STKModelData.WarehouseList = _WarehouseList;
                    dtSet = GetWHAndCustNameAndStkRsrvList(STKModelData);
                    List<Warehouse> _WarehouseList = new List<Warehouse>();
                    foreach (DataRow dr in dtSet.Tables[0].Rows)
                     {
                        Warehouse _Warehouse = new Warehouse();
                        _Warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _Warehouse.wh_name = dr["wh_name"].ToString();
                        _WarehouseList.Add(_Warehouse);
                    }
                    _WarehouseList.Insert(0, new Warehouse() { wh_id = 0, wh_name = "---Select---" });
                    STKModelData.WarehouseList = _WarehouseList;

                    List<Lot> _LotList = new List<Lot>();
                    Lot _Lot = new Lot();
                    _Lot.Lot_id = "0";
                    _Lot.Lot_name = "---Select---";
                    _LotList.Add(_Lot);
                    STKModelData.LotList = _LotList;

                    List<Batch> _BatchList = new List<Batch>();
                    Batch _Batch = new Batch();
                    _Batch.Batch_id = "0";
                    _Batch.Batch_name = "---Select---";
                    _BatchList.Add(_Batch);
                    STKModelData.BatchList = _BatchList;

                    List<Serial> _SerialList = new List<Serial>();
                    Serial _Serial = new Serial();
                    _Serial.Serial_id = "0";
                    _Serial.Serial_name = "---Select---";
                    _SerialList.Add(_Serial);
                    STKModelData.SerialList = _SerialList;

                    //GetAutoCompleteSearchCustList(STKModelData);

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    STKModelData.DocumentNumberList = _DocumentNumberList;

                    /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                    //DataTable dt1 = new DataTable();
                    //List<CustomerName> CustLists = new List<CustomerName>();
                    //dt1 = GetCustNameList(STKModelData);
                    //foreach (DataRow dr in dt1.Rows)
                    //{
                    //    CustomerName _RAList = new CustomerName();
                    //    _RAList.cust_id = dr["cust_id"].ToString();
                    //    _RAList.cust_name = dr["cust_name"].ToString();
                    //    CustLists.Add(_RAList);
                    //}
                    //CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = "---Select---" });
                    //STKModelData.CustomerNameList = CustLists;
                    List<CustomerName> CustLists = new List<CustomerName>();
                    foreach (DataRow dr in dtSet.Tables[1].Rows)
                    {
                        CustomerName _RAList = new CustomerName();
                        _RAList.cust_id = dr["cust_id"].ToString();
                        _RAList.cust_name = dr["cust_name"].ToString();
                        CustLists.Add(_RAList);
                    }
                    CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = "---Select---" });
                    STKModelData.CustomerNameList = CustLists;

                    ViewBag.ItemDetails = dtSet.Tables[2];
                    /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                    //ViewBag.ItemDetails = GetStockReservationList();
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    //ViewBag.MenuPageName = getDocumentName();
                    STKModelData.Title = title;
                    STKModelData.warehouse_id = null;
                    STKModelData.DocumentMenuId = DocumentMenuId;
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockReservation/StockReservation.cshtml", STKModelData);
                }
                else
                {
                    StockReservation_Model _StockReservation_Model = new StockReservation_Model();
                    if (_urlmodel.TransType != null)
                    {
                        _StockReservation_Model.TransType = _urlmodel.TransType;
                    }
                    else
                    {
                        _StockReservation_Model.TransType = "Save";
                    }
                    if (_urlmodel.BtnName != null)
                    {
                        _StockReservation_Model.BtnName = _urlmodel.BtnName;
                    }
                    else
                    {
                        _StockReservation_Model.BtnName = "Refresh";
                    }
                    if (_urlmodel.Command != null)
                    {
                        _StockReservation_Model.Command = _urlmodel.Command;
                    }
                    else
                    {
                        _StockReservation_Model.Command = "Refresh";
                    }
                    CommonPageDetails();
                    //var other = new CommonController(_Common_IServices);
                    _StockReservation_Model.to_br = BranchName;
                    _StockReservation_Model.to_brid = BrchID;

                    /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                    //dt = GetWHList(BrchID);
                    //List<Warehouse> _WarehouseList = new List<Warehouse>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    Warehouse _Warehouse = new Warehouse();
                    //    _Warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _Warehouse.wh_name = dr["wh_name"].ToString();
                    //    _WarehouseList.Add(_Warehouse);
                    //}
                    //_WarehouseList.Insert(0, new Warehouse() { wh_id = 0, wh_name = "---Select---" });
                    //_StockReservation_Model.WarehouseList = _WarehouseList;
                    dtSet = GetWHAndCustNameAndStkRsrvList(_StockReservation_Model);
                    List<Warehouse> _WarehouseList = new List<Warehouse>();
                    foreach (DataRow dr in dtSet.Tables[0].Rows)
                    {
                        Warehouse _Warehouse = new Warehouse();
                        _Warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _Warehouse.wh_name = dr["wh_name"].ToString();
                        _WarehouseList.Add(_Warehouse);
                    }
                    _WarehouseList.Insert(0, new Warehouse() { wh_id = 0, wh_name = "---Select---" });
                    _StockReservation_Model.WarehouseList = _WarehouseList;



                    List<Lot> _LotList = new List<Lot>();
                    Lot _Lot = new Lot();
                    _Lot.Lot_id = "0";
                    _Lot.Lot_name = "---Select---";
                    _LotList.Add(_Lot);
                    _StockReservation_Model.LotList = _LotList;

                    List<Batch> _BatchList = new List<Batch>();
                    Batch _Batch = new Batch();
                    _Batch.Batch_id = "0";
                    _Batch.Batch_name = "---Select---";
                    _BatchList.Add(_Batch);
                    _StockReservation_Model.BatchList = _BatchList;

                    List<Serial> _SerialList = new List<Serial>();
                    Serial _Serial = new Serial();
                    _Serial.Serial_id = "0";
                    _Serial.Serial_name = "---Select---";
                    _SerialList.Add(_Serial);
                    _StockReservation_Model.SerialList = _SerialList;

                    //GetAutoCompleteSearchCustList(_StockReservation_Model);

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _StockReservation_Model.DocumentNumberList = _DocumentNumberList;

                    /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                    //DataTable dt1 = new DataTable();
                    //List<CustomerName> CustLists = new List<CustomerName>();
                    //dt1 = GetCustNameList(_StockReservation_Model);
                    //foreach (DataRow dr in dt1.Rows)
                    //{
                    //    CustomerName _RAList = new CustomerName();
                    //    _RAList.cust_id = dr["cust_id"].ToString();
                    //    _RAList.cust_name = dr["cust_name"].ToString();
                    //    CustLists.Add(_RAList);
                    //}
                    //CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = "---Select---" });
                    //_StockReservation_Model.CustomerNameList = CustLists;
                    List<CustomerName> CustLists = new List<CustomerName>();
                    foreach (DataRow dr in dtSet.Tables[1].Rows)
                    {
                        CustomerName _RAList = new CustomerName();
                        _RAList.cust_id = dr["cust_id"].ToString();
                        _RAList.cust_name = dr["cust_name"].ToString();
                        CustLists.Add(_RAList);
                    }
                    CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = "---Select---" });
                    _StockReservation_Model.CustomerNameList = CustLists;


                    /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                    //ViewBag.ItemDetails = GetStockReservationList();
                    ViewBag.ItemDetails = dtSet.Tables[2];
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    //ViewBag.MenuPageName = getDocumentName();
                    //ViewBag.VBRoleList = GetRoleList();
                    _StockReservation_Model.Title = title;
                    _StockReservation_Model.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockReservation/StockReservation.cshtml", _StockReservation_Model);
                }
          
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            //StockReservation_Model _StockReservation_Model = new StockReservation_Model();                     
        }
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
        //private DataTable GetCustNameList(StockReservation_Model _StockReservation_Model)
        //{
        //    try
        //    {
        //        string CustomerName = string.Empty;
        //        string CompID = string.Empty;
        //        string BrchID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BrchID = Session["BranchId"].ToString();
        //        }
        //        if (string.IsNullOrEmpty(_StockReservation_Model.CustID))
        //        {
        //            CustomerName = "0";
        //        }
        //        else
        //        {
        //            CustomerName = _StockReservation_Model.CustID;
        //        }
        //        DataTable dt = _StockReservation_ISERVICES.GetCustNameList(CompID, BrchID, CustomerName);
        //        return dt;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
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
        //[HttpPost]
        //public ActionResult GetItemName(StockReservation_Model _StockReservation_Model)
        //{
        //    JsonResult DataRows = null;
        //    string ItmName = string.Empty;
        //    string Comp_ID = string.Empty;
        //    string Br_ID = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();

        //            if (Session["BranchId"] != null)
        //            {
        //                Br_ID = Session["BranchId"].ToString();
        //            }
        //            if (string.IsNullOrEmpty(_StockReservation_Model.ItemName))
        //            {
        //                ItmName = "0";
        //            }
        //            else
        //            {
        //                ItmName = _StockReservation_Model.ItemName;
        //            }
        //            DataSet ProductList = _StockReservation_ISERVICES.BindItemName(Comp_ID, Br_ID,ItmName);
        //            DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return DataRows;
        //}
        

        private DataSet GetWHAndCustNameAndStkRsrvList(StockReservation_Model _StockReservation_Model)
        {
            string CustomerName = string.Empty;
            string CompID = string.Empty;
            string BrchID = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (string.IsNullOrEmpty(_StockReservation_Model.CustID))
            {
                CustomerName = "0";
            }
            else
            {
                CustomerName = _StockReservation_Model.CustID;
            }
            DataSet ds = _StockReservation_ISERVICES.GetWHAndCustNameAndStkRsrvList(CompID, BrchID, CustomerName);
            return ds;
        }
        //[HttpPost]
        //private DataTable GetWHList(string branch)
        //{
        //    List<Warehouse> _WarehouseList = new List<Warehouse>();
        //    _StockReservation_Model = new StockReservation_Model();
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    //string BranchId = Session["BranchId"].ToString();

        //    DataTable dt = _StockReservation_ISERVICES.GetWhList(CompID, branch);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        Warehouse _Warehouse = new Warehouse();
        //        _Warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
        //        _Warehouse.wh_name = dr["wh_name"].ToString();
        //        _WarehouseList.Add(_Warehouse);

        //    }
        //    _WarehouseList.Insert(0, new Warehouse() { wh_id = 0, wh_name = "---Select---" });
        //    _StockReservation_Model.WarehouseList = _WarehouseList;

        //    return dt;
        //}



        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockReservationSave(StockReservation_Model _StockReservation_Model, string command)
        {
            try
            {
                string trf_no = string.Empty;
                ///*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                //if (Session["CompId"] != null)
                //    CompID = Session["CompId"].ToString();
                //if (Session["BranchId"] != null)
                //    BrchID = Session["BranchId"].ToString();
                //var commCont = new CommonController(_Common_IServices);
                //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                //{
                //    TempData["Message"] = "Financial Year not Exist";
                //    command = "Refresh";
                //}
                ///*End to chk Financial year exist or not*/
                switch (command)
                {
                    case "AddNew":
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        StockReservation_Model _StockReservationadd_Model = new StockReservation_Model();
                        _StockReservationadd_Model.Message = null;
                        _StockReservationadd_Model.AppStatus = "D";
                        _StockReservationadd_Model.BtnName = "BtnAddNew";
                        _StockReservationadd_Model.TransType = "Save";
                        _StockReservationadd_Model.Command = "New";
                          TempData["ModelData"] = _StockReservationadd_Model;
                        UrlModel _urlmodel = new UrlModel();
                        _urlmodel.TransType = "Save";
                        _urlmodel.BtnName = "BtnAddNew";
                        _urlmodel.Command = "New";
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        var commCont = new CommonController(_Common_IServices);
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            command = "Refresh";
                            _StockReservationadd_Model.TransType = "Refresh";
                            _StockReservationadd_Model.Command = "Refresh";
                            _StockReservationadd_Model.BtnName = "Refresh";
                            _urlmodel.BtnName = "Refresh";
                            _urlmodel.TransType = "Refresh";
                            _urlmodel.Command = "Refresh";
                            return RedirectToAction("StockReservation", "StockReservation", _urlmodel);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("StockReservation", "StockReservation", _urlmodel);

                    case "Save":
                        //Session["Command"] = command;
                        _StockReservation_Model.Command = command;
                        if (ModelState.IsValid)
                        {
                            SaveStockReserveDetail(_StockReservation_Model);
                            if (_StockReservation_Model.Message == "DocModify")
                            {
                                CommonPageDetails();
                                //var other = new CommonController(_Common_IServices);
                                _StockReservation_Model.to_br = BranchName;
                                _StockReservation_Model.to_brid = BrchID;

                                var WhName = _StockReservation_Model.warehouse_Name;
                                /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                                //dt = GetWHList(BrchID);
                                //List<Warehouse> _WarehouseList = new List<Warehouse>();
                                //foreach (DataRow dr in dt.Rows)
                                //{
                                //    Warehouse _Warehouse = new Warehouse();
                                //    _Warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                //    _Warehouse.wh_name = dr["wh_name"].ToString();
                                //    _WarehouseList.Add(_Warehouse);
                                //}
                                //_WarehouseList.Insert(0, new Warehouse() { wh_id = 0, wh_name = WhName });
                                //_StockReservation_Model.WarehouseList = _WarehouseList;
                                dtSet= GetWHAndCustNameAndStkRsrvList(_StockReservation_Model);
                                List<Warehouse> _WarehouseList = new List<Warehouse>();
                                foreach (DataRow dr in dtSet.Tables[0].Rows)
                                {
                                    Warehouse _Warehouse = new Warehouse();
                                    _Warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                    _Warehouse.wh_name = dr["wh_name"].ToString();
                                    _WarehouseList.Add(_Warehouse);
                                }
                                _WarehouseList.Insert(0, new Warehouse() { wh_id = 0, wh_name = WhName });
                                _StockReservation_Model.WarehouseList = _WarehouseList;



                                List<Lot> _LotList = new List<Lot>();
                                Lot _Lot = new Lot();
                                _Lot.Lot_id = "0";
                                _Lot.Lot_name = "---Select---";
                                _LotList.Add(_Lot);
                                _StockReservation_Model.LotList = _LotList;

                                List<Batch> _BatchList = new List<Batch>();
                                Batch _Batch = new Batch();
                                _Batch.Batch_id = "0";
                                _Batch.Batch_name = "---Select---";
                                _BatchList.Add(_Batch);
                                _StockReservation_Model.BatchList = _BatchList;

                                List<Serial> _SerialList = new List<Serial>();
                                Serial _Serial = new Serial();
                                _Serial.Serial_id = "0";
                                _Serial.Serial_name = "---Select---";
                                _SerialList.Add(_Serial);
                                _StockReservation_Model.SerialList = _SerialList;

                                //GetAutoCompleteSearchCustList(STKModelData);
                                var DocNo = _StockReservation_Model.Doc_Number;
                                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                                DocumentNumber _DocumentNumber = new DocumentNumber();
                                _DocumentNumber.doc_no = DocNo;
                                _DocumentNumber.doc_dt = "0";
                                _DocumentNumberList.Add(_DocumentNumber);
                                _StockReservation_Model.DocumentNumberList = _DocumentNumberList;

                                /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                                //DataTable dt1 = new DataTable();
                                //var CustNam = _StockReservation_Model.CustNam;
                                //List<CustomerName> CustLists = new List<CustomerName>();
                                //dt1 = GetCustNameList(_StockReservation_Model);
                                //foreach (DataRow dr in dt1.Rows)
                                //{
                                //    CustomerName _RAList = new CustomerName();
                                //    _RAList.cust_id = dr["cust_id"].ToString();
                                //    _RAList.cust_name = dr["cust_name"].ToString();
                                //    CustLists.Add(_RAList);
                                //}
                                //CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = CustNam });
                                //_StockReservation_Model.CustomerNameList = CustLists;
                                var CustNam = _StockReservation_Model.CustNam;
                                List<CustomerName> CustLists = new List<CustomerName>();
                                foreach (DataRow dr in dtSet.Tables[1].Rows)
                                {
                                    CustomerName _RAList = new CustomerName();
                                    _RAList.cust_id = dr["cust_id"].ToString();
                                    _RAList.cust_name = dr["cust_name"].ToString();
                                    CustLists.Add(_RAList);
                                }
                                CustLists.Insert(0, new CustomerName() { cust_id = "0", cust_name = CustNam });
                                _StockReservation_Model.CustomerNameList = CustLists;



                                _StockReservation_Model.doc_dt = _StockReservation_Model.Doc_Date;

                                /*commented by Hina on 19-03-2024 to combine all list Procedure  in single Procedure*/
                                //ViewBag.ItemDetails = GetStockReservationList();
                                ViewBag.ItemDetails = dtSet.Tables[2];
                                ViewBag.DocumentMenuId = DocumentMenuId;
                                _StockReservation_Model.Title = title;
                                _StockReservation_Model.DocumentMenuId = DocumentMenuId;
                                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockReservation/StockReservation.cshtml", _StockReservation_Model);
                            }
                            if (_StockReservation_Model.Message == "Save")
                            {
                                StockReservation_Model Save_Model = new StockReservation_Model();
                                Save_Model.Message = "Save";
                                Save_Model.Command = "New";
                                Save_Model.TransType = "Save";
                                Save_Model.AppStatus = "D";
                                Save_Model.BtnName = "BtnAddNew";
                                TempData["ModelData"] = Save_Model;
                                UrlModel _url_Savemodel = new UrlModel();
                                _url_Savemodel.TransType = "Save";
                                _url_Savemodel.BtnName = "BtnAddNew";
                                _url_Savemodel.Command = "New";
                                _url_Savemodel.Docid = "New";
                                return RedirectToAction("StockReservation", _url_Savemodel);
                            }
                            else
                            {
                                StockReservation_Model SaveModel = new StockReservation_Model();
                                SaveModel.Command = "Refresh";
                                SaveModel.TransType = "Save";
                                SaveModel.AppStatus = "D";
                                SaveModel.BtnName = "Refresh";
                                TempData["ModelData"] = SaveModel;
                                UrlModel _urlSavemodel = new UrlModel();
                                _urlSavemodel.TransType = "Save";
                                _urlSavemodel.BtnName = "Refresh";
                                _urlSavemodel.Command = "Refresh";
                                _urlSavemodel.Docid = "New";
                                return RedirectToAction("StockReservation", _urlSavemodel);
                            }
                            
                          
                        }
                        else
                        {
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockReservation/StockReservation.cshtml");
                        }

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        StockReservation_Model _STKRefresh_Model = new StockReservation_Model();
                        _STKRefresh_Model.BtnName= "Refresh";
                        _STKRefresh_Model.Command = command;
                        _STKRefresh_Model.TransType = "Save";                    
                        _STKRefresh_Model.DocumentStatus = null;
                      UrlModel refeshmodel = new UrlModel();
                        refeshmodel.TransType = "Save";
                        refeshmodel.BtnName = "Refresh";
                        refeshmodel.Command = "Refresh";
                        refeshmodel.Docid = "New";
                        TempData["ModelData"] = _STKRefresh_Model;                    
                        return RedirectToAction("StockReservation", refeshmodel);                 
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
        public ActionResult SaveStockReserveDetail(StockReservation_Model _StockReservation_Model)
        {
            try
            {
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }

                DataTable StockReserve = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("wh_id", typeof(int));
                dtheader.Columns.Add("item_id", typeof(string));
                dtheader.Columns.Add("uom_id", typeof(int));
                dtheader.Columns.Add("docno", typeof(string));
                dtheader.Columns.Add("docdt", typeof(string));
                dtheader.Columns.Add("docid", typeof(string));
                dtheader.Columns.Add("trans_type", typeof(string));
                dtheader.Columns.Add("lot_no", typeof(string));
                dtheader.Columns.Add("batch_no", typeof(string));
                dtheader.Columns.Add("ex_date", typeof(string));
                dtheader.Columns.Add("serial_no", typeof(string));
                dtheader.Columns.Add("quantity", typeof(string));
                dtheader.Columns.Add("i_batch", typeof(string));
                dtheader.Columns.Add("i_serial", typeof(string));
                dtheader.Columns.Add("mfg_name", typeof(string));
                dtheader.Columns.Add("mfg_mrp", typeof(string));
                dtheader.Columns.Add("mfg_date", typeof(string));

                //dtheader.Columns.Add("wh_id", typeof(int));
                //dtheader.Columns.Add("wh_res_stk", typeof(float));
                //dtheader.Columns.Add("wh_tot_stk", typeof(float));
                //dtheader.Columns.Add("wh_avl_stk", typeof(float));
                //dtheader.Columns.Add("doc_type", typeof(string));
                //dtheader.Columns.Add("entity_id", typeof(int));
                //dtheader.Columns.Add("doc_no", typeof(string));
                //dtheader.Columns.Add("doc_dt", typeof(string));
                //dtheader.Columns.Add("doc_qty", typeof(float));
                //dtheader.Columns.Add("pend_qty", typeof(float));
                //dtheader.Columns.Add("unres_qty", typeof(float));

                //dtheader.Columns.Add("avl_stk", typeof(float));
                //dtheader.Columns.Add("res_qty", typeof(float));
                //dtheader.Columns.Add("create_id", typeof(int));
                //dtheader.Columns.Add("UserMacaddress", typeof(string));
                //dtheader.Columns.Add("UserSystemName", typeof(string));
                //dtheader.Columns.Add("UserIP", typeof(string));

                

                if (_StockReservation_Model.Res_UnresStockDetails != null && _StockReservation_Model.Res_UnresStockDetails != "")
                {
                    JArray jObjectBatch = JArray.Parse(_StockReservation_Model.Res_UnresStockDetails);
                    foreach (JObject item in jObjectBatch.Children())
                    {
                        DataRow dtrowHeader = dtheader.NewRow();

                        dtrowHeader["comp_id"] = Session["CompId"].ToString();
                        dtrowHeader["br_id"] = Session["BranchId"].ToString();
                        dtrowHeader["wh_id"] = item.GetValue("whid").ToString().Trim();
                        dtrowHeader["item_id"] = item.GetValue("itemid").ToString().Trim();
                        dtrowHeader["uom_id"] = item.GetValue("uomid").ToString().Trim();
                        dtrowHeader["docno"] = item.GetValue("docno").ToString().Trim();
                        dtrowHeader["docdt"] = item.GetValue("docdt").ToString().Trim();
                        dtrowHeader["docid"] = "105103125";
                        dtrowHeader["trans_type"] = item.GetValue("transtype").ToString().Trim();
                        dtrowHeader["lot_no"] = item.GetValue("lotno").ToString().Trim();
                        dtrowHeader["batch_no"] = item.GetValue("batchno").ToString().Trim();
                        dtrowHeader["ex_date"] = item.GetValue("expirydt").ToString().Trim();
                        dtrowHeader["serial_no"] = item.GetValue("serialno").ToString().Trim();
                        dtrowHeader["quantity"] = item.GetValue("res_unresQty").ToString().Trim();
                        dtrowHeader["i_batch"] = _StockReservation_Model.hdn_i_batch;
                        dtrowHeader["i_serial"] = _StockReservation_Model.hdn_i_serial;
                        dtrowHeader["mfg_name"] = IsBlank(item.GetValue("mfg_name").ToString().Trim(),null);
                        dtrowHeader["mfg_mrp"] = IsBlank(item.GetValue("mfg_mrp").ToString().Trim(),null);
                        dtrowHeader["mfg_date"] = IsBlank(item.GetValue("mfg_date").ToString().Trim(),null);

                        dtheader.Rows.Add(dtrowHeader);
                    }
                }
                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                if (_StockReservation_Model.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(_StockReservation_Model.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                    ViewData["SubItem"] = dtSubItemDetails(jObject2);
                }

                /*------------------Sub Item end----------------------*/



                //dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                //dtrowHeader["comp_id"] = Session["CompId"].ToString();
                //dtrowHeader["br_id"] = Session["BranchId"].ToString();
                //dtrowHeader["tr_type"] = _StockReservation_Model.Transaction_Type;
                //dtrowHeader["item_id"] = _StockReservation_Model.item_id;
                //dtrowHeader["uom_id"] = _StockReservation_Model.uom_id;
                //dtrowHeader["wh_id"] = _StockReservation_Model.warehouse_id;
                //dtrowHeader["wh_res_stk"] = _StockReservation_Model.Res_stk;
                //dtrowHeader["wh_tot_stk"] = _StockReservation_Model.Tot_stk;
                //dtrowHeader["wh_avl_stk"] = _StockReservation_Model.Aval_stk;
                //dtrowHeader["doc_type"] = _StockReservation_Model.Doc_Type;
                //dtrowHeader["entity_id"] = _StockReservation_Model.CustID;
                //dtrowHeader["doc_no"] = _StockReservation_Model.doc_no;
                //dtrowHeader["doc_dt"] = _StockReservation_Model.doc_dt;
                //dtrowHeader["doc_qty"] = _StockReservation_Model.doc_qty;
                //dtrowHeader["pend_qty"] = _StockReservation_Model.pending_qty;
                //dtrowHeader["unres_qty"] = _StockReservation_Model.unres_qty;
                //dtrowHeader["lot_no"] = _StockReservation_Model.lot_id;
                //dtrowHeader["batch_no"] = _StockReservation_Model.batch_id;
                //dtrowHeader["serial_no"] = _StockReservation_Model.serial_id;
                //dtrowHeader["avl_stk"] = _StockReservation_Model.BtSrAvalStk;
                //dtrowHeader["res_qty"] = _StockReservation_Model.ResQty;
                //dtrowHeader["create_id"] = Session["UserId"].ToString();
                //dtrowHeader["UserMacaddress"] = Session["UserMacaddress"].ToString();
                //dtrowHeader["UserSystemName"] = Session["UserSystemName"].ToString();
                //dtrowHeader["UserIP"] = Session["UserIP"].ToString();


                StockReserve = dtheader;

                string SaveMessage = _StockReservation_ISERVICES.InsertStockReserve(StockReserve, dtSubItem);
                if (SaveMessage == "DocModify")
                {
                    _StockReservation_Model.Message = "DocModify";
                    _StockReservation_Model.BtnName = "Refresh";
                    _StockReservation_Model.Command = "Refresh";
                    TempData["ModelData"] = _StockReservation_Model;
                    return RedirectToAction("StockReservation");
                }
                if (SaveMessage== "StockNotAvail")
                {
                        _StockReservation_Model.Message = "StockNotAvailable";
                        _StockReservation_Model.BtnName = "Refresh";
                        _StockReservation_Model.Command = "Refresh";
                        TempData["ModelData"] = _StockReservation_Model;
                        return RedirectToAction("StockReservation");
                }
                if (SaveMessage == "Update" || SaveMessage == "Save")
                {
                    _StockReservation_Model.Message = "Save";
                    return RedirectToAction("StockReservation");
                }
                return RedirectToAction("StockReservation");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        public DataTable dtSubItemDetails(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        [HttpPost]
        public JsonResult GetDocumentNo(string Entity_id, string Itm_ID,string wh_id ,string Type)
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
                //DataSet result = _DomesticSaleInvoice_ISERVICE.GetShipmentList(Cust_id, Comp_ID, Br_ID);
                DataSet result = _StockReservation_ISERVICES.GetDocumentNo(Comp_ID, Br_ID, Entity_id, Itm_ID,wh_id, Type);
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
        public JsonResult GetStock(string item_id, string wh_id)
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
                DataSet result = _StockReservation_ISERVICES.GetStock(Comp_ID, Br_ID, item_id,wh_id);
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
        public JsonResult GetDocdetail(string wh_id, string item_id, string Docno)
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
                DataSet result = _StockReservation_ISERVICES.GetDocdetail(Comp_ID, Br_ID, wh_id, item_id, Docno);
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
        public JsonResult GetBatchSerialDetail(string item_id, string wh_id, string lot_id)
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
                DataSet result = _StockReservation_ISERVICES.GetBatchSerialDetail(Comp_ID, Br_ID, item_id, wh_id, lot_id);
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
        public JsonResult GetBatchSerialAvalStock(string item_id, string wh_id, string lot_id, string BatchNo)
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
                DataSet result = _StockReservation_ISERVICES.GetBatchSerialAvalStock(Comp_ID, Br_ID, item_id, wh_id, lot_id, BatchNo);
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
        private DataTable GetStockReservationList()
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
                DataTable RoleList = _StockReservation_ISERVICES.GetStockReservationList(CompID, BrchID);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetReservedItemDetail(string ItemID, string wh_id,string flag, string entity_id, string DocNo, string SelectedItemLotBatchdetail)
        {
            try
            {
                JsonResult DataRows = null;              
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _StockReservation_ISERVICES.GetReservedItemDetail(CompID, BrchID, ItemID, wh_id, flag, entity_id, DocNo);

                if (flag == "rev" || flag == "unrev")
                {
                    if (SelectedItemLotBatchdetail != null && SelectedItemLotBatchdetail != "")
                    {
                        JArray jObjectBatch = JArray.Parse(SelectedItemLotBatchdetail);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            foreach (JObject item in jObjectBatch.Children())
                            {
                                if (flag == "rev")
                                {
                                    if (item.GetValue("itemid").ToString().Trim() == ds.Tables[0].Rows[i]["itemid"].ToString().Trim() && item.GetValue("lotno").ToString().Trim() == ds.Tables[0].Rows[i]["lotno"].ToString().Trim() && item.GetValue("batchno").ToString().Trim() == ds.Tables[0].Rows[i]["batchno"].ToString().Trim() && item.GetValue("serialno").ToString().Trim() == ds.Tables[0].Rows[i]["serialno"].ToString().Trim())
                                    {
                                        ds.Tables[0].Rows[i]["res_qty"] = item.GetValue("res_unresQty");
                                    }
                                }
                                if (flag == "unrev")
                                {
                                    if (item.GetValue("docno").ToString().Trim() == ds.Tables[0].Rows[i]["srcno"].ToString().Trim() && item.GetValue("itemid").ToString().Trim() == ds.Tables[0].Rows[i]["itemid"].ToString().Trim() && item.GetValue("lotno").ToString().Trim() == ds.Tables[0].Rows[i]["lotno"].ToString().Trim() && item.GetValue("batchno").ToString().Trim() == ds.Tables[0].Rows[i]["batchno"].ToString().Trim() && item.GetValue("serialno").ToString().Trim() == ds.Tables[0].Rows[i]["serialno"].ToString().Trim())
                                    {
                                        ds.Tables[0].Rows[i]["unres_qty"] = item.GetValue("res_unresQty");
                                    }
                                }
                            }
                        }

                    }
                }
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string Flag
, string doc_type, string cust_id, string doc_no, string doc_dt, string wh_id, string TransType)
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
                string Flag2 = "";
                string IsDisabled = "Y";
                DataTable dt = new DataTable();
                if (Flag == "PPTotalResQuantity")
                {
                    Flag = "StkResTotalResQty";
                    IsDisabled = "N";
                    Flag2 = "Res";
                    //dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    //dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, "wh").Tables[0];
                    dt = _StockReservation_ISERVICES.StockRes_GetSubItemDetails(CompID, BrchID, Item_id, wh_id, doc_type, cust_id, doc_no, doc_dt, "", TransType).Tables[0];
                    dt.Columns.Add("Qty", typeof(string));
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = item.GetValue("qty");
                            }
                        }
                    }
                }
                else if(Flag == "PPTotalUnResQuantity")
                {
                    Flag = "StkResTotalResQty";
                    IsDisabled = "N";
                    Flag2 = "UnRes";
                    //dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    dt = _StockReservation_ISERVICES.StockRes_GetSubItemDetails(CompID, BrchID, Item_id, wh_id, doc_type, cust_id, doc_no, doc_dt, Flag, TransType).Tables[0];
                    dt.Columns.Add("Qty", typeof(string));
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = item.GetValue("qty");
                            }
                        }
                    }
                }
                else
                {
                    Flag = "StkResDocDetail";
                    dt = _StockReservation_ISERVICES.StockRes_GetSubItemDetails(CompID, BrchID, Item_id, wh_id, doc_type, cust_id, doc_no, doc_dt, Flag, TransType).Tables[0];
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y",
                    _subitemPageName=Flag2
                    //ShowStock=Flag== "Quantity"?"Y":"N"
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

        public string CheckFinYearExist()
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            string data = commCont.CheckFinancialYear(CompID, BrchID);
            return data;
        }
        //public ActionResult GetAutoCompleteSearchCustList(StockReservation_Model _StockReservation_Model)
        //{
        //    string CustomerName = string.Empty;
        //    Dictionary<string, string> CustList = new Dictionary<string, string>();
        //    string Comp_ID = string.Empty;
        //    string Br_ID = string.Empty;
        //    string CustType = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }

        //        if (string.IsNullOrEmpty(_StockReservation_Model.CustName))
        //        {
        //            CustomerName = "0";
        //        }
        //        else
        //        {
        //            CustomerName = _StockReservation_Model.CustName;
        //        }
        //        CustList = _StockReservation_ISERVICES.GetCustomerList(Comp_ID, CustomerName, Br_ID);

        //        List<CustomerName> _CustList = new List<CustomerName>();
        //        foreach (var data in CustList)
        //        {
        //            CustomerName _CustDetail = new CustomerName();
        //            _CustDetail.cust_id = data.Key;
        //            _CustDetail.cust_name = data.Value;
        //            _CustList.Add(_CustDetail);
        //        }
        //        _StockReservation_Model.CustomerNameList = _CustList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        //return Json("ErrorPage");
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        private object IsBlank(string input, object output)//Added by Suraj Maurya on 27-11-2025
        {
            return input == "" ? output : input;
        }
    }
}