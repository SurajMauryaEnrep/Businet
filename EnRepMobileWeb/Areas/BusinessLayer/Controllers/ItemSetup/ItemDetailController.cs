using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.ItemDetail;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
//***All Session Removed By Shubham Maurya 13-01-2023 ***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class ItemDetailController : Controller
    {
        string CompID, UserID, language = String.Empty;
        string DocumentMenuId = "103105",title;
        Common_IServices _Common_IServices;
        string comp_id, userid,ItemId = string.Empty;
        ItemDetailModel _ItemDetail;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
   
        ItemDetail_ISERVICES _ItemDetail_ISERVICES;
        // GET: BusinessLayer/ItemDetail

        public ItemDetailController(Common_IServices _Common_IServices,ItemDetail_ISERVICES _ItemDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ItemDetail_ISERVICES = _ItemDetail_ISERVICES;
        }
        public ActionResult ItemDetail(ItemDetailModel _ItemDetail1, string ItemCodeURL, string TransType, string BtnName, string command)
        {
            try
            {
                var _ItemDetail = TempData["ModelData"] as ItemDetailModel;
                ResetTempFiles();
                if (_ItemDetail != null)
                {
                    //_ItemDetail = new ItemDetailModel();
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    _ItemDetail.AttachMentDetailItmStp = null;
                    _ItemDetail.Guid = null;
                    string Comp_ID = string.Empty;
                    _ItemDetail.item_id = null;

                   

                    //dt = GetItemBranchList();
                    //ViewBag.CustomerBranchList = dt;
                  //  _ItemDetail.CustomerBranchList = dt;
                    ViewBag.VBRoleList = GetRoleList();               
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    GetAllDropdownData(_ItemDetail);
                    if (_ItemDetail.TransType == "Update" || _ItemDetail.Command == "Edit")
                    {
                        GetAllTableData(_ItemDetail);
                    }
                    else
                    {
                        _ItemDetail.act_status = true;
                    }                              
                    ViewBag.MenuPageName = getDocumentName();
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _ItemDetail.Title = title;
                    _ItemDetail.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/BusinessLayer/Views/ItemSetup/ItemDetail.cshtml", _ItemDetail);
                }
                else
                {
                    //_ItemDetail1 = new ItemDetailModel();
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    if (_ItemDetail1.ItemCode == null)
                    {
                        _ItemDetail1.ItemCode = ItemCodeURL;
                    }
                    if (_ItemDetail1.TransType == null)
                    {
                        _ItemDetail1.TransType = TransType;
                    }
                    if (_ItemDetail1.BtnName == null)
                    {
                        _ItemDetail1.BtnName = BtnName;
                    }
                    if (_ItemDetail1.Command == null)
                    {
                        _ItemDetail1.Command = command;
                    }
                    _ItemDetail1.AttachMentDetailItmStp = null;
                    _ItemDetail1.Guid = null;
                    string Comp_ID = string.Empty;
                    _ItemDetail1.item_id = null;

                  

                    //  dt = GetItemBranchList(); Commented By NItesh 20032024
                    //ViewBag.CustomerBranchList = dt;
                    //  _ItemDetail1.CustomerBranchList = dt; Commented By NItesh 20032024

                    ViewBag.VBRoleList = GetRoleList();

                    // dt = GetUOMList();
                 
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    GetAllDropdownData(_ItemDetail1); /**Added By Nitesh 15-03-2024 All Dropdown Bind in one Method**/
                    if (_ItemDetail1.TransType == "Update" || _ItemDetail1.Command == "Edit")
                    {
                        GetAllTableData(_ItemDetail1);  /**Modified By Nitesh 15-03-2024 And Added by Nitesh This Method :- GetAllTableData(_ItemDetail1)**/
                    }
                    else
                    {
                        _ItemDetail1.act_status = true;
                    }
                    if (_ItemDetail1.BtnName == null)
                    {
                        _ItemDetail1.BtnName = "BtnRefresh";
                    }          
                    ViewBag.MenuPageName = getDocumentName();
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _ItemDetail1.Title = title;
                    _ItemDetail1.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/BusinessLayer/Views/ItemSetup/ItemDetail.cshtml", _ItemDetail1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }

        private void GetAllTableData(ItemDetailModel _ItemDetail1)
        {
            string Comp_ID = string.Empty;
            string BrchID = string.Empty;
            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
            Boolean act_status, stock_warn, i_batch, i_capg, i_cons, i_exp, i_pur, i_qc, i_Sam, i_serial, i_sls, i_srvc, i_stk, i_wip, i_pack, i_catalog, i_ws, i_exempted, i_subitem;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                UserID = Session["UserId"].ToString();
            }

            string ItemCode = _ItemDetail1.ItemCode;
          
            DataSet ds = _ItemDetail_ISERVICES.GetviewitemdetailDAL(ItemCode, Comp_ID, BrchID);
            if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                act_status = true;
            else
                act_status = false;
            if (ds.Tables[0].Rows[0]["stkout_warn"].ToString() == "Y")
                stock_warn = true;
            else
                stock_warn = false;
            if (ds.Tables[0].Rows[0]["i_batch"].ToString() == "Y")
                i_batch = true;
            else
                i_batch = false;
            if (ds.Tables[0].Rows[0]["i_capg"].ToString() == "Y")
                i_capg = true;
            else
                i_capg = false;
            if (ds.Tables[0].Rows[0]["i_cons"].ToString() == "Y")
                i_cons = true;
            else
                i_cons = false;
            if (ds.Tables[0].Rows[0]["i_exp"].ToString() == "Y")
                i_exp = true;
            else
                i_exp = false;
            if (ds.Tables[0].Rows[0]["i_pur"].ToString() == "Y")
                i_pur = true;
            else
                i_pur = false;
            if (ds.Tables[0].Rows[0]["i_qc"].ToString() == "Y")
                i_qc = true;
            else
                i_qc = false;
            if (ds.Tables[0].Rows[0]["i_Sam"].ToString() == "Y")
                i_Sam = true;
            else
                i_Sam = false;
            if (ds.Tables[0].Rows[0]["i_serial"].ToString() == "Y")
                i_serial = true;
            else
                i_serial = false;
            if (ds.Tables[0].Rows[0]["i_sls"].ToString() == "Y")
                i_sls = true;
            else
                i_sls = false;
            if (ds.Tables[0].Rows[0]["i_srvc"].ToString() == "Y")
                i_srvc = true;
            else
                i_srvc = false;
            if (ds.Tables[0].Rows[0]["i_stk"].ToString() == "Y")
                i_stk = true;
            else
                i_stk = false;
            if (ds.Tables[0].Rows[0]["i_wip"].ToString() == "Y")
                i_wip = true;
            else
                i_wip = false;
            if (ds.Tables[0].Rows[0]["i_pack"].ToString() == "Y")
                i_pack = true;
            else
                i_pack = false;
            if (ds.Tables[0].Rows[0]["i_catalog"].ToString() == "Y")
                i_catalog = true;
            else
                i_catalog = false;
            if (ds.Tables[0].Rows[0]["i_ws"].ToString() == "Y")
                i_ws = true;
            else
                i_ws = false;
            if (ds.Tables[0].Rows[0]["tax_exemp"].ToString() == "Y")
                i_exempted = true;
            else
                i_exempted = false;
            if (ds.Tables[0].Rows[0]["sub_item"].ToString() == "Y")
                i_subitem = true;
            else
                i_subitem = false;
            _ItemDetail1.i_Sample_dependcy = ds.Tables[16].Rows[0]["i_Sample_dependcy"].ToString();
            _ItemDetail1.i_batch_dependcy = ds.Tables[15].Rows[0]["i_batch_dependcy"].ToString();
            _ItemDetail1.i_Ser_dependcy = ds.Tables[14].Rows[0]["i_Service_dependcy"].ToString();
            _ItemDetail1.i_cons_dependcy = ds.Tables[13].Rows[0]["i_consum_dependcy"].ToString();
            _ItemDetail1.ItemNameDependcy = ds.Tables[12].Rows[0]["ItemNameDependcy"].ToString();
            _ItemDetail1.item_id = ds.Tables[0].Rows[0]["item_id"].ToString();
            _ItemDetail1.itemname1 = ds.Tables[0].Rows[0]["item_name"].ToString();
            _ItemDetail1.item_name = ds.Tables[0].Rows[0]["item_name"].ToString();
            _ItemDetail1.TechSpec = ds.Tables[0].Rows[0]["item_tech_spec"].ToString();
            _ItemDetail1.item_oem_no = ds.Tables[0].Rows[0]["item_oem_no"].ToString();
            _ItemDetail1.item_sam_cd = ds.Tables[0].Rows[0]["item_sam_cd"].ToString();
            _ItemDetail1.item_sam_des = ds.Tables[0].Rows[0]["item_tech_des"].ToString();
            _ItemDetail1.item_leg_cd = ds.Tables[0].Rows[0]["item_leg_cd"].ToString();
            _ItemDetail1.item_leg_des = ds.Tables[0].Rows[0]["item_leg_des"].ToString();
            _ItemDetail1.cost_price = Convert.ToDecimal(ds.Tables[0].Rows[0]["cost_price"]).ToString(RateDigit);
            _ItemDetail1.sale_price = Convert.ToDecimal(ds.Tables[0].Rows[0]["sale_price"]).ToString(RateDigit);
            _ItemDetail1.min_stk_lvl = ds.Tables[0].Rows[0]["min_stk_lvl"].ToString();
            _ItemDetail1.min_pr_stk = ds.Tables[0].Rows[0]["min_pr_stk"].ToString();
            _ItemDetail1.re_ord_lvl = ds.Tables[0].Rows[0]["re_ord_lvl"].ToString();
            _ItemDetail1.base_uom_id = ds.Tables[0].Rows[0]["baseuomID"].ToString();
            _ItemDetail1.pur_uom_id = ds.Tables[0].Rows[0]["purcuomID"].ToString();
            _ItemDetail1.sl_uom_id = ds.Tables[0].Rows[0]["saluomID"].ToString();
            _ItemDetail1.bin_id = int.Parse(ds.Tables[0].Rows[0]["BinID"].ToString());
            _ItemDetail1.item_prf_id = int.Parse(ds.Tables[0].Rows[0]["portfID"].ToString());
            _ItemDetail1.HSN_code = ds.Tables[0].Rows[0]["hsnID"].ToString();
            _ItemDetail1.item_grp_id = ds.Tables[0].Rows[0]["GroupID"].ToString();
            _ItemDetail1.wh_id = int.Parse(ds.Tables[0].Rows[0]["wh_id"].ToString());
            _ItemDetail1.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
            _ItemDetail1.item_remarks = ds.Tables[0].Rows[0]["item_remarks"].ToString();
            _ItemDetail1.dependcy_i_capg = ds.Tables[0].Rows[0]["dependcy_i_capg"].ToString();
            _ItemDetail1.ExpiryAlertDays =Convert.ToInt32(ds.Tables[0].Rows[0]["exp_alrt_days"].ToString());
            _ItemDetail1.act_status = act_status;
            _ItemDetail1.stkout_warn = stock_warn;
            _ItemDetail1.i_batch = i_batch;
            _ItemDetail1.i_capg = i_capg;
            _ItemDetail1.i_cons = i_cons;
            _ItemDetail1.i_exp = i_exp;
            _ItemDetail1.i_pur = i_pur;
            _ItemDetail1.i_qc = i_qc;
            _ItemDetail1.i_Sam = i_Sam;
            _ItemDetail1.i_serial = i_serial;
            _ItemDetail1.i_sls = i_sls;
            _ItemDetail1.i_srvc = i_srvc;
            _ItemDetail1.i_stk = i_stk;
            _ItemDetail1.i_wip = i_wip;
            _ItemDetail1.i_pack = i_pack;
            _ItemDetail1.i_catalog = i_catalog;
            _ItemDetail1.i_ws = i_ws;
            _ItemDetail1.i_exempted = i_exempted;
            _ItemDetail1.SubItem = i_subitem;
            _ItemDetail1.app_status = ds.Tables[0].Rows[0]["app_status"].ToString();
            _ItemDetail1.Status = ds.Tables[0].Rows[0]["status_code"].ToString();

            if (_ItemDetail1.Status == "A")
            {
                //Session["AppStatus"] = 'A';
                _ItemDetail1.AppStatus = "A";
            }
            if (_ItemDetail1.Status == "D")
            {
                //Session["AppStatus"] = 'D';
                _ItemDetail1.AppStatus = "D";
            }
            if (_ItemDetail1.Status == "F")
            {
                //Session["AppStatus"] = 'F';
                _ItemDetail1.AppStatus = "F";
            }
            _ItemDetail1.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
            _ItemDetail1.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
            _ItemDetail1.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
            _ItemDetail1.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
            _ItemDetail1.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
            _ItemDetail1.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
            _ItemDetail1.loc_sls_coa = int.Parse(ds.Tables[0].Rows[0]["loc_sls_coa"].ToString());
            _ItemDetail1.exp_sls_coa = int.Parse(ds.Tables[0].Rows[0]["exp_sls_coa"].ToString());
            _ItemDetail1.loc_pur_coa = int.Parse(ds.Tables[0].Rows[0]["loc_pur_coa"].ToString());
            _ItemDetail1.imp_pur_coa = int.Parse(ds.Tables[0].Rows[0]["imp_pur_coa"].ToString());
            _ItemDetail1.stk_coa = int.Parse(ds.Tables[0].Rows[0]["stk_coa"].ToString());
            _ItemDetail1.sal_ret_coa = int.Parse(ds.Tables[0].Rows[0]["sal_ret_coa"].ToString());
            _ItemDetail1.Disc_coa = int.Parse(ds.Tables[0].Rows[0]["Disc_coa"].ToString());
            _ItemDetail1.pur_ret_coa = int.Parse(ds.Tables[0].Rows[0]["pur_ret_coa"].ToString());
            //_ItemDetail1.prov_pay_coa = int.Parse(ds.Tables[0].Rows[0]["prov_pay_coa"].ToString());
            //_ItemDetail1.cogs_coa = int.Parse(ds.Tables[0].Rows[0]["cogs_coa"].ToString());
            //_ItemDetail1.stk_adj_coa = int.Parse(ds.Tables[0].Rows[0]["stk_adj_coa"].ToString());
            _ItemDetail1.dep_coa = int.Parse(ds.Tables[0].Rows[0]["dep_coa"].ToString());
            _ItemDetail1.asset_coa = int.Parse(ds.Tables[0].Rows[0]["asset_coa"].ToString());
            _ItemDetail1.wght_kg = ds.Tables[0].Rows[0]["wght_kg"].ToString();
            _ItemDetail1.wght_ltr = ds.Tables[0].Rows[0]["wght_ltr"].ToString();
            _ItemDetail1.gr_wght = ds.Tables[0].Rows[0]["gr_wght"].ToString();
            _ItemDetail1.nt_wght = ds.Tables[0].Rows[0]["nt_wght"].ToString();
            _ItemDetail1.item_hgt = ds.Tables[0].Rows[0]["item_hgt"].ToString();
            _ItemDetail1.item_wdh = ds.Tables[0].Rows[0]["item_wdh"].ToString();
            _ItemDetail1.item_len = ds.Tables[0].Rows[0]["item_len"].ToString();
            _ItemDetail1.item_pack_sz = ds.Tables[0].Rows[0]["item_pack_sz"].ToString();
            _ItemDetail1.CatlNoPrefix = ds.Tables[0].Rows[0]["cat_no_prefix"].ToString();
            _ItemDetail1.CatlNo = ds.Tables[0].Rows[0]["item_cat_no"].ToString();
            _ItemDetail1.SalesAlias = ds.Tables[0].Rows[0]["sls_alias"].ToString();
            _ItemDetail1.PurchaseAlias = ds.Tables[0].Rows[0]["purc_alias"].ToString();
            _ItemDetail1.issue_method = ds.Tables[0].Rows[0]["issue_method"].ToString();
            _ItemDetail1.cost_method = ds.Tables[0].Rows[0]["cost_method"].ToString();
            _ItemDetail1.pack_uom = int.Parse(ds.Tables[0].Rows[0]["pack_uom"].ToString());
            _ItemDetail1.pack_length = ds.Tables[0].Rows[0]["pack_length"].ToString();
            _ItemDetail1.pack_width = ds.Tables[0].Rows[0]["pack_width"].ToString();
            _ItemDetail1.pack_height = ds.Tables[0].Rows[0]["pack_height"].ToString();
            _ItemDetail1.pack_cbm = ds.Tables[0].Rows[0]["pack_cbm"].ToString();
            //Session["i_packdetail"] = ds.Tables[0].Rows[0]["i_pack"].ToString();
            _ItemDetail1.i_packdetail = ds.Tables[0].Rows[0]["i_pack"].ToString();
            _ItemDetail1.ItemUsedInTrans = ds.Tables[11].Rows[0]["ItemUsedInTrans"].ToString();
            _ItemDetail1.ToDisableBaseUnit = ds.Tables[11].Rows[0]["ToDisableBaseUnit"].ToString();
            //ViewBag.CustomerBranchList = ds.Tables[1];
            _ItemDetail1.CustomerBranchList = ds.Tables[1];
            ViewBag.ItemAttributeList = ds.Tables[2];
            ViewBag.AttechmentDetails = ds.Tables[3];
            ViewBag.ItemCustinfoList = ds.Tables[4];
            ViewBag.ItemSuppinfoList = ds.Tables[5];
            ViewBag.PortfolioList = ds.Tables[6];
            ViewBag.VehicleList = ds.Tables[7];
            ViewBag.D_BthNStk = ds.Tables[8].Rows[0]["disableStockableAndBatchable"].ToString();
            _ItemDetail1.D_InActive = ds.Tables[9].Rows[0]["disableInActive"].ToString();
            _ItemDetail1.InterBranch_sls_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_sls_coa"].ToString());
            _ItemDetail1.InterBranch_pur_coa = int.Parse(ds.Tables[0].Rows[0]["interbr_pur_coa"].ToString());
            ViewBag.SubItem = ds.Tables[10];
            
            //--------Added by Suraj Maurya on 18-12-2024

            string tempFolderName = Comp_ID + UserID + DocumentMenuId;
            //string usertempfolders = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\TempAttachment\\";
            //string[] userTempFs = Directory.GetDirectories(usertempfolders, Comp_ID + UserID + DocumentMenuId + "*");
            //foreach (string tmpfolders in userTempFs)
            //{
            //    string[] filePathsTemp = Directory.GetFiles(tmpfolders, "*");
            //    foreach (string file in filePathsTemp)
            //    {
            //        System.IO.File.Delete(file);
            //    }
            //    Directory.Delete(tmpfolders);
            //}
            
            string AttachmentFilePath2 = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "ItemSetup" + "/";
            if (Directory.Exists(AttachmentFilePath2))
            {
                string[] filePaths1 = Directory.GetFiles(AttachmentFilePath2, Comp_ID + _ItemDetail1.item_id + "_" + "*");
                string AttachmentFilePath1 = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\TempAttachment\\" + tempFolderName + "/";

                //--------Added by Suraj Maurya on 18-12-2024 End
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    string file_name = dr["file_name"].ToString();
                    if (dr["file_def"].ToString().Trim() == "Y")
                    {

                        file_name = file_name.Substring(file_name.IndexOf('_') + 1);
                        _ItemDetail1.attatchmentDefaultdetail = file_name;
                    }
                    //--------Added by Suraj Maurya on 18-12-2024

                    foreach (string file in filePaths1)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if (dr["file_name"].ToString() == fileInfo.Name)
                        {
                            string temp_path = Path.Combine(AttachmentFilePath1 + "\\", fileInfo.Name);
                            System.IO.File.Copy(file, temp_path);
                        }

                    }
                    //--------Added by Suraj Maurya on 18-12-2024 End
                }
            }
        }

        private void ResetTempFiles()//Created by Suraj Maurya on 18-12-2024 for delete temp files
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                UserID = Session["UserId"].ToString();
            }
            string tempFolderName = CompID + UserID + DocumentMenuId;
            string AttachmentFilePath1 = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\TempAttachment\\" + tempFolderName + "/";
            if (Directory.Exists(AttachmentFilePath1))
            {
                string[] filePathsTemp = Directory.GetFiles(AttachmentFilePath1, "*");
                foreach (string file in filePathsTemp)
                {
                    System.IO.File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(AttachmentFilePath1);//Create New Temp directory 
            }
        }
        private void GetAllDropdownData(ItemDetailModel _ItemDetail1)
        {
            try
            {
                // Commented By Nitesh 20-03-2024 For All data In One Procedure

                // GetAutoCompleteSearchSuggestion(_ItemDetail1);
                // GetAutoCompleteSearchHSN(_ItemDetail1);
                // GetLocalSaleAccount(_ItemDetail1);
                //GetExportSaleAccount(_ItemDetail1);
                // GetLocalPurchaseAccount(_ItemDetail1);
                // GetImportPurchaseAccount(_ItemDetail1);
                // GetSaleReturnAccount(_ItemDetail1);
                // GetPurchaseReturnAccount(_ItemDetail1);
                //GetProvisionalPayableAccount(_ItemDetail1);
                //GetStockAccount(_ItemDetail1);
                //GetStockAdjustmentAccount(_ItemDetail1);
                //  GetDepreciationAccount(_ItemDetail1);
                // GetDiscountAccount(_ItemDetail1);
                //GetcostOfGoodsSoldAccount(_ItemDetail1);

                //   GetAssetAccount(_ItemDetail1);
                
              //  GetSuppListAuto(_ItemDetail1);
                // GetCustListAuto(_ItemDetail1);
                //   GetPortfolioListAuto(_ItemDetail1);
                //  GetVehicleListAuto(_ItemDetail1);
                //GetAttributeValue("0");
                if (TempData["ListFilterData"] != null)
                {
                    _ItemDetail1.ListFilterData1 = TempData["ListFilterData"].ToString();
                }
                ViewBag.VehicalAccord = VehicalAccord();
                string Comp_ID = string.Empty;
                string GroupName = string.Empty;
                string HSNCode = string.Empty;
                string AccName = string.Empty;
                string SupplierName = string.Empty;
                string BranchId = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchId = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ItemDetail1.ddlgroup_name))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _ItemDetail1.ddlgroup_name;
                }
                if (string.IsNullOrEmpty(_ItemDetail1.ddlhsncode))
                {
                    HSNCode = "0";
                }
                else
                {
                    HSNCode = _ItemDetail1.ddlhsncode;
                }
                if (string.IsNullOrEmpty(_ItemDetail1.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail1.ddlcoa_name;
                }
                if (string.IsNullOrEmpty(_ItemDetail1.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _ItemDetail1.SuppName;
                }

                DataSet Table = _ItemDetail_ISERVICES.GetAllDropDownData(Comp_ID, BranchId, GroupName, HSNCode, AccName, SupplierName, DocumentMenuId);

                /******************Bind Uom DropDown*************************/

                List<UOM> _ItemUOMList = new List<UOM>();
                foreach (DataRow dt in Table.Tables[0].Rows)
                {
                    UOM _UOMList = new UOM();
                    _UOMList.uom_id = dt["uom_id"].ToString();
                    _UOMList.uom_name = dt["uom_name"].ToString();
                    _ItemUOMList.Add(_UOMList);
                }
                _ItemUOMList.Insert(0, new UOM() { uom_id = "0", uom_name = "---Select---" });
                _ItemDetail1.UOMList = _ItemUOMList;

                /**********************End*************************/
                /**********************Default Bin****************************/
                List<Bin> _ItemBinList = new List<Bin>();
                foreach (DataRow dt in Table.Tables[1].Rows)
                {
                    Bin _BinList = new Bin();
                    _BinList.setup_id = dt["setup_id"].ToString();
                    _BinList.setup_val = dt["setup_val"].ToString();
                    _ItemBinList.Add(_BinList);
                }
                _ItemBinList.Insert(0, new Bin() { setup_id = "0", setup_val = "---Select---" });
                _ItemDetail1.BinList = _ItemBinList;

               /************************Item Attributes********************************************/
                List<AttrName> _ItemAttrList = new List<AttrName>();
                foreach (DataRow dt in Table.Tables[2].Rows)
                {
                    AttrName _AttrList = new AttrName();
                    _AttrList.attr_id = dt["attr_id"].ToString();
                    _AttrList.attr_name = dt["attr_name"].ToString();
                    _ItemAttrList.Add(_AttrList);
                }
                _ItemAttrList.Insert(0, new AttrName() { attr_id = "0", attr_name = "---Select---" });
                _ItemDetail1.AttrList = _ItemAttrList;
                /***************************************End*********************************************/
                /**************************************Bind Warehouse*************************************************/
               
                List<warehouse> _ItemWHList = new List<warehouse>();
                foreach (DataRow dt in Table.Tables[3].Rows)
                {
                    warehouse _WHList = new warehouse();
                    _WHList.wh_id = dt["wh_id"].ToString();
                    _WHList.wh_name = dt["wh_name"].ToString();
                    _ItemWHList.Add(_WHList);
                }
                _ItemWHList.Insert(0, new warehouse() { wh_id = "0", wh_name = "---Select---" });
                _ItemDetail1.warehouseList = _ItemWHList;

                /******************************************End************************************************************/

                /*********************************************Bind Item Group**********************************************************/
              

                List<GroupName> _GroupList = new List<GroupName>();
                foreach (DataRow data in Table.Tables[4].Rows)
                {
                    GroupName _GroupDetail = new GroupName();
                    _GroupDetail.item_grp_id = data["item_grp_id"].ToString();
                    _GroupDetail.ItemGroupChildNood = data["ItemGroupChildNood"].ToString();
                    _GroupList.Add(_GroupDetail);
                }
                _GroupList.Insert(0, new GroupName() { item_grp_id = "0", ItemGroupChildNood = "---Select---" });
                _ItemDetail1.GroupList = _GroupList;
                /********************************************End********************************************************/

              
             /**********************************Bind HSn ********************************************/

                List<HSNno> _HSNList = new List<HSNno>();
                foreach (DataRow data in Table.Tables[5].Rows)
                {
                    HSNno _HsnDetail = new HSNno();
                    _HsnDetail.setup_id = data["setup_id"].ToString();
                    _HsnDetail.setup_val = data["setup_val"].ToString();
                    _HSNList.Add(_HsnDetail);
                }
                _HSNList.Insert(0, new HSNno() { setup_id = "0", setup_val = "---Select---" });
                _ItemDetail1.HSNList = _HSNList;

                /****************************************End**************************************************/


                /****************************************Bind Local Sale Account and Export Sales Account And Sales Return Account*************************************************/
                List<IncomeCOA> _COAList = new List<IncomeCOA>();
                foreach (DataRow data in Table.Tables[6].Rows)
                {
                    IncomeCOA _COADetail = new IncomeCOA();
                    _COADetail.coa_id = data["acc_id"].ToString();
                    _COADetail.coa_name = data["acc_name"].ToString();
                    _COAList.Add(_COADetail);
                }
                _COAList.Insert(0, new IncomeCOA() { coa_id = "0", coa_name = "---Select---" });
                _ItemDetail1.IncomeCOAList = _COAList;

                /*********************************************End****************************************************/

                /******************************************Bind Local and Import Purchase Account And Purchase Return Account and Depreciation Account**************************************************/
                List<ExpenseCOA> _COAList1 = new List<ExpenseCOA>();
                foreach (DataRow data in Table.Tables[7].Rows)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data["acc_id"].ToString();
                    _COADetail.coa_name = data["acc_name"].ToString();
                    _COAList1.Add(_COADetail);
                }
                _COAList1.Insert(0, new ExpenseCOA() { coa_id = "0", coa_name = "---Select---" });
                _ItemDetail1.ExpenseCOAList = _COAList1;

                /*********************************************Bind Assest Account******************************************************/
            
                List<AssetsCOA> _COAList2 = new List<AssetsCOA>();
                foreach (DataRow data in Table.Tables[8].Rows)
                {
                    AssetsCOA _COADetail = new AssetsCOA();
                    _COADetail.coa_id = data["acc_id"].ToString(); ;
                    _COADetail.coa_name = data["acc_name"].ToString();
                    _COAList2.Add(_COADetail);
                }
                _COAList2.Insert(0, new AssetsCOA() { coa_id = "0", coa_name = "---Select---" });
                _ItemDetail1.AssetsCOAList = _COAList2;
                /****************************************************End*****************************************************************/
                
                //   GetSuppListAutoa(Table.Tables[9]);

                _ItemDetail1.CustomerBranchList = Table.Tables[10];

                var dt3 = Table.Tables[11];
                var dt1 = Table.Tables[12].Rows.Count > 0 ? Table.Tables[12].Rows[0]["param_stat"].ToString() : "";
                ViewBag.param_stat = dt1;
                bool Flag = false;
                if (dt3.Rows.Count > 0)
                {
                    if (dt3.Rows[0]["param_stat"].ToString() == "Y")
                    {
                        Flag = true;
                    }
                }
                // ViewBag.VehicalAccord = VehicalAccord();
                ViewBag.VehicalAccord = Flag;
                _ItemDetail1.AutoGen_Ref_noParameter = Table.Tables[13].Rows.Count > 0 ? Table.Tables[13].Rows[0]["AutoGen_Ref_noParameter"].ToString().Trim():"N";
                _ItemDetail1.AutoGen_catalogNoParameter = Table.Tables[13].Rows.Count > 0 ? Table.Tables[13].Rows[0]["AutoGen_catalogNoParameter"].ToString().Trim() : "N";

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        [HttpPost]
        public ActionResult GetSuppListAutoa(DataTable Abc)
        {
            JsonResult DataRows = null;
           
            try
            {
             
                DataRow dr;
                dr = Abc.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                Abc.Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(Abc));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        private bool VehicalAccord()
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string BranchId = string.Empty;
            if (Session["BranchId"] != null)
            {
                BranchId = Session["BranchId"].ToString();
            }
            DataSet dtl = _ItemDetail_ISERVICES.GetVehicalInfoAccrdn(Comp_ID,BranchId);
            var dt = dtl.Tables[0];
            var dt1 = dtl.Tables[1].Rows.Count > 0 ? dtl.Tables[1].Rows[0]["param_stat"].ToString():"";
            ViewBag.param_stat= dt1;
            bool Flag = false;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["param_stat"].ToString() == "Y")
                {
                    Flag = true;
                }
            }
            return Flag;
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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID,DocumentMenuId);
                
                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ItemSave(ItemDetailModel _ItemDetail, string command, string item_id, HttpPostedFileBase[] ItemFiles)
        {
            try
            {
                if (_ItemDetail.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Edit":
                        _ItemDetail.Message = "";
                        _ItemDetail.Command = command;
                        _ItemDetail.ItemCode = _ItemDetail.item_id;
                        _ItemDetail.Savebtn = null;
                        _ItemDetail.TransType = "Update";
                        _ItemDetail.BtnName = "BtnEdit";
                        var ItemCodeURL= _ItemDetail.item_id;
                        var TransType= "Update";
                        var BtnName= "BtnEdit";
                        TempData["ModelData"] = _ItemDetail;
                        TempData["ListFilterData"] = _ItemDetail.ListFilterData1;
                        return ( RedirectToAction("ItemDetail", "ItemDetail", new { ItemCodeURL = ItemCodeURL, TransType, BtnName, command }));                      

                    case "Add":
                        ItemDetailModel _ItemDetailAdd = new ItemDetailModel();
                        _ItemDetailAdd.Command = command;
                        _ItemDetailAdd.Savebtn = null;
                        _ItemDetailAdd.ItemCode = "";
                        _ItemDetailAdd.AppStatus = "D";
                        _ItemDetailAdd.TransType = "Save";
                        _ItemDetailAdd.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _ItemDetailAdd;
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("ItemDetail", "ItemDetail");


                    case "Delete":
                        _ItemDetail.Command = command;
                        _ItemDetail.Savebtn = null;
                        _ItemDetail.BtnName = "Delete";
                        item_id = _ItemDetail.item_id;
                        ItemDetailDelete(_ItemDetail, command);
                        TempData["ListFilterData"] = _ItemDetail.ListFilterData1;
                        return RedirectToAction("ItemDetail");

                    case "Save":
                        _ItemDetail.Command = command;
                        if (_ItemDetail.app_status == "Approved")
                        {
                            _ItemDetail.AppStatus = "A";
                        }
                        if (_ItemDetail.app_status != "Approved")
                        {
                            _ItemDetail.AppStatus = "D";
                        }                    
                            InsertItemDetail(_ItemDetail, ItemFiles);
                         if(_ItemDetail.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                         if (_ItemDetail.Message == "Duplicate")
                            {
                            _ItemDetail.AttachMentDetailItmStp = null;
                            _ItemDetail.Savebtn = null;
                            _ItemDetail.Savebtn = "Duplicate";
                            _ItemDetail.Guid = null;
                            ViewBag.VehicalAccord = VehicalAccord();
                            _ItemDetail.act_status = true;
                            ViewBag.VBRoleList = GetRoleList();

                                ViewBag.ItemCustinfoList = ViewData["ItemCustomer"];
                                ViewBag.ItemAttributeList = ViewData["ItemAttribute"];
                                ViewBag.ItemSuppinfoList = ViewData["ItemSupplier"];
                                ViewBag.PortfolioList = ViewData["ItemPortfolio"];
                                ViewBag.VehicleList = ViewData["ItemVehicle"];
                                ViewBag.SubItem = ViewData["ItemSubItem"];

                            if (_ItemDetail.item_id != null)
                            {
                                string ItemCode = _ItemDetail.item_id;
                                string BrchID = string.Empty;
                                if (Session["CompId"] != null)
                                {
                                    CompID = Session["CompId"].ToString();
                                }
                                if (Session["BranchId"] != null)
                                {
                                    BrchID = Session["BranchId"].ToString();
                                }
                                DataSet ds = _ItemDetail_ISERVICES.GetviewitemdetailDAL(ItemCode, CompID, BrchID);
                                _ItemDetail.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                                _ItemDetail.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                _ItemDetail.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                _ItemDetail.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                _ItemDetail.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                                _ItemDetail.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                _ItemDetail.CustomerBranchList = ds.Tables[1];
                            }
                            else
                            {
                                dt = GetItemBranchList();
                                _ItemDetail.CustomerBranchList = dt;
                            }
                                dt = GetUOMList();
                                List<UOM> _ItemUOMList = new List<UOM>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    UOM _UOMList = new UOM();
                                    _UOMList.uom_id = dt["uom_id"].ToString();
                                    _UOMList.uom_name = dt["uom_name"].ToString();
                                    _ItemUOMList.Add(_UOMList);
                                }
                                _ItemUOMList.Insert(0, new UOM() { uom_id = "0", uom_name = "---Select---" });
                                _ItemDetail.UOMList = _ItemUOMList;

                                dt = GetBinList();
                                List<Bin> _ItemBinList = new List<Bin>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    Bin _BinList = new Bin();
                                    _BinList.setup_id = dt["setup_id"].ToString();
                                    _BinList.setup_val = dt["setup_val"].ToString();
                                    _ItemBinList.Add(_BinList);
                                }
                                _ItemBinList.Insert(0, new Bin() { setup_id = "0", setup_val = "---Select---" });
                                _ItemDetail.BinList = _ItemBinList;

                                dt = GetAttrList();
                                List<AttrName> _ItemAttrList = new List<AttrName>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AttrName _AttrList = new AttrName();
                                    _AttrList.attr_id = dt["attr_id"].ToString();
                                    _AttrList.attr_name = dt["attr_name"].ToString();
                                    _ItemAttrList.Add(_AttrList);
                                }
                                _ItemAttrList.Insert(0, new AttrName() { attr_id = "0", attr_name = "---Select---" });
                                _ItemDetail.AttrList = _ItemAttrList;

                                dt = GetWHList();
                                List<warehouse> _ItemWHList = new List<warehouse>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    warehouse _WHList = new warehouse();
                                    _WHList.wh_id = dt["wh_id"].ToString();
                                    _WHList.wh_name = dt["wh_name"].ToString();
                                    _ItemWHList.Add(_WHList);
                                }
                                _ItemWHList.Insert(0, new warehouse() { wh_id = "0", wh_name = "---Select---" });
                                _ItemDetail.warehouseList = _ItemWHList;

                                GetAutoCompleteSearchSuggestion(_ItemDetail);
                                GetAutoCompleteSearchHSN(_ItemDetail);
                                GetLocalSaleAccount(_ItemDetail);
                                GetExportSaleAccount(_ItemDetail);
                                GetLocalPurchaseAccount(_ItemDetail);
                                GetImportPurchaseAccount(_ItemDetail);
                                GetSaleReturnAccount(_ItemDetail);
                                GetPurchaseReturnAccount(_ItemDetail);
                                GetDepreciationAccount(_ItemDetail);
                                GetDiscountAccount(_ItemDetail);
                                GetAssetAccount(_ItemDetail);
                                GetCustListAuto(_ItemDetail);
                                GetSuppListAuto(_ItemDetail);
                                GetPortfolioListAuto(_ItemDetail);
                                GetVehicleListAuto(_ItemDetail);

                            _ItemDetail.ItemCode = "";
                            _ItemDetail.AppStatus = "D";
                            _ItemDetail.TransType = "Save";
                            _ItemDetail.BtnName = "BtnAddNew";
                            _ItemDetail.Command = "Add";

                            _ItemDetail.act_status = true;
                            TempData["ListFilterData"] = _ItemDetail.ListFilterData1;
                            _ItemDetail.DocumentMenuId = DocumentMenuId;
                            return View("~/Areas/BusinessLayer/Views/ItemSetup/ItemDetail.cshtml", _ItemDetail);
                            }
                        else
                        {
                            ItemCodeURL = _ItemDetail.ItemCode;
                            TransType = _ItemDetail.TransType;
                            BtnName = _ItemDetail.BtnName;
                            TempData["ModelData"] = _ItemDetail;
                            TempData["ListFilterData"] = _ItemDetail.ListFilterData1;
                            return ( RedirectToAction("ItemDetail", new { ItemCodeURL = ItemCodeURL, TransType, BtnName, command }));
                        }            
                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        _ItemDetail.Command = command;
                        _ItemDetail.Savebtn = null;
                        _ItemDetail.Savebtn = "AllreadyclickApprovebtn";
                        item_id = _ItemDetail.item_id;
                        _ItemDetail.ItemCode = item_id;
                        ItemApprove(_ItemDetail, command);
                        TempData["ModelData"] = _ItemDetail;
                        ItemCodeURL = _ItemDetail.ItemCode;
                        TransType = _ItemDetail.TransType;
                        BtnName = _ItemDetail.BtnName;
                        TempData["ListFilterData"] = _ItemDetail.ListFilterData1;
                        return ( RedirectToAction("ItemDetail", new { ItemCodeURL = ItemCodeURL, TransType, BtnName, command }));
                    case "Refresh":
                        _ItemDetail.Savebtn = null;
                      ItemDetailModel _ItemDetailRefresh = new ItemDetailModel();
                        _ItemDetailRefresh.BtnName = "BtnRefresh";
                        _ItemDetailRefresh.Savebtn = "";
                        _ItemDetailRefresh.Command = command;
                        _ItemDetailRefresh.TransType = "Refresh";                        
                        TempData["ModelData"] = _ItemDetailRefresh;
                        TempData["ListFilterData"] = _ItemDetail.ListFilterData1;
                        return RedirectToAction("ItemDetail");
                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        TempData["ListFilterData"] = _ItemDetail.ListFilterData1;
                        return RedirectToAction("ItemList", "ItemList");
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

        public ActionResult InsertItemDetail(ItemDetailModel _ItemDetail, HttpPostedFileBase[] ItemFiles)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            try
            {
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable ItemDetail = new DataTable();
                DataTable ItemBranch = new DataTable();
                DataTable ItemAttachments = new DataTable();             
                DataTable ItemAttribute = new DataTable();
                DataTable ItemCustomer = new DataTable();
                DataTable ItemSupplier = new DataTable();
                DataTable ItemPortfolio = new DataTable();
                DataTable ItemVehicle = new DataTable();
                DataTable ItemSubItem = new DataTable();

                DataTable dt = new DataTable();
                dt.Columns.Add("TransType", typeof(string));
                dt.Columns.Add("item_id", typeof(string));
                dt.Columns.Add("item_name", typeof(string));
                dt.Columns.Add("TechSpec", typeof(string));               
                dt.Columns.Add("item_oem_no", typeof(string));
                dt.Columns.Add("item_sam_cd", typeof(string));
                dt.Columns.Add("item_tech_des", typeof(string));
                dt.Columns.Add("item_leg_cd", typeof(string));
                dt.Columns.Add("item_leg_des", typeof(string));
                dt.Columns.Add("item_grp_id", typeof(int));
                dt.Columns.Add("base_uom_id", typeof(int));
                dt.Columns.Add("pur_uom_id", typeof(int));
                dt.Columns.Add("sl_uom_id", typeof(int));
                dt.Columns.Add("cost_price", typeof(string));
                dt.Columns.Add("sale_price", typeof(string));
                dt.Columns.Add("item_prf_id", typeof(int));
                dt.Columns.Add("issue_method", typeof(string));
                dt.Columns.Add("cost_method", typeof(string));
                dt.Columns.Add("min_stk_lvl", typeof(string));
                dt.Columns.Add("min_pr_stk", typeof(string));
                dt.Columns.Add("re_ord_lvl", typeof(string));
                dt.Columns.Add("stkout_warn", typeof(string));
                dt.Columns.Add("item_remarks", typeof(string));
                dt.Columns.Add("act_status", typeof(string));
                dt.Columns.Add("inact_reason", typeof(string));
                dt.Columns.Add("wh_id", typeof(int));
                dt.Columns.Add("bin_id", typeof(int));
                dt.Columns.Add("HSN_code", typeof(string));
                dt.Columns.Add("i_sls", typeof(string));
                dt.Columns.Add("i_pur", typeof(string));
                dt.Columns.Add("i_wip", typeof(string));
                dt.Columns.Add("i_pack", typeof(string));
                dt.Columns.Add("i_capg", typeof(string));
                dt.Columns.Add("i_stk", typeof(string));
                dt.Columns.Add("i_qc", typeof(string));
                dt.Columns.Add("i_srvc", typeof(string));
                dt.Columns.Add("i_cons", typeof(string));
                dt.Columns.Add("i_serial", typeof(string));
                dt.Columns.Add("i_Sam", typeof(string));
                dt.Columns.Add("i_batch", typeof(string));
                dt.Columns.Add("i_exp", typeof(string));
                dt.Columns.Add("i_catalog", typeof(string));
                dt.Columns.Add("create_id", typeof(int));
                dt.Columns.Add("mod_id", typeof(int));
                dt.Columns.Add("app_status", typeof(string));
                dt.Columns.Add("loc_sls_coa", typeof(int));
                dt.Columns.Add("exp_sls_coa", typeof(int));
                dt.Columns.Add("loc_pur_coa", typeof(int));
                dt.Columns.Add("imp_pur_coa", typeof(int));
                dt.Columns.Add("stk_coa", typeof(int));
                dt.Columns.Add("sal_ret_coa", typeof(int));
                dt.Columns.Add("Disc_coa", typeof(int));
                dt.Columns.Add("pur_ret_coa", typeof(int));
                dt.Columns.Add("prov_pay_coa", typeof(int));
                dt.Columns.Add("cogs_coa", typeof(int));
                dt.Columns.Add("stk_adj_coa", typeof(int));
                dt.Columns.Add("dep_coa", typeof(int));
                dt.Columns.Add("asset_coa", typeof(int));
                dt.Columns.Add("wght_kg", typeof(string));
                dt.Columns.Add("wght_ltr", typeof(string));
                dt.Columns.Add("gr_wght", typeof(string));
                dt.Columns.Add("nt_wght", typeof(string));
                dt.Columns.Add("item_hgt", typeof(string));
                dt.Columns.Add("item_wdh", typeof(string));
                dt.Columns.Add("item_len", typeof(string));
                dt.Columns.Add("item_pack_sz", typeof(string));
                dt.Columns.Add("CatlNoPrefix", typeof(string));
                dt.Columns.Add("CatlNo", typeof(string));
                dt.Columns.Add("SalesAlias", typeof(string));
                dt.Columns.Add("PurchaseAlias", typeof(string));
                dt.Columns.Add("UserMacaddress", typeof(string));
                dt.Columns.Add("UserSystemName", typeof(string));
                dt.Columns.Add("UserIP", typeof(string));               
                dt.Columns.Add("pack_uom", typeof(int));
                dt.Columns.Add("pack_length", typeof(string));
                dt.Columns.Add("pack_width", typeof(string));
                dt.Columns.Add("pack_height", typeof(string));
                dt.Columns.Add("pack_cbm", typeof(string));
                dt.Columns.Add("comp_id", typeof(int));        
                dt.Columns.Add("tax_exemp", typeof(string));        
                dt.Columns.Add("sub_item", typeof(string));        
                dt.Columns.Add("interBranch_sls_coa", typeof(int));        
                dt.Columns.Add("interBranch_pur_coa", typeof(int));        

                DataRow dtrow = dt.NewRow();
                dtrow["TransType"] = _ItemDetail.TransType;

                if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                {
                    dtrow["item_id"] = _ItemDetail.item_id;
                }
                else
                {
                    dtrow["item_id"] = "0";
                }
                if(_ItemDetail.item_name =="" || _ItemDetail.item_name==null)
                {
                    dtrow["item_name"] = _ItemDetail.itemname1;
                }
                else
                {
                    dtrow["item_name"] = _ItemDetail.item_name;
                }
               
                dtrow["TechSpec"] = _ItemDetail.TechSpec;
                dtrow["item_oem_no"] = _ItemDetail.item_oem_no;
                dtrow["item_sam_cd"] = _ItemDetail.item_sam_cd;
                dtrow["item_tech_des"] = _ItemDetail.item_sam_des;
                dtrow["item_leg_cd"] = _ItemDetail.item_leg_cd;
                dtrow["item_leg_des"] = _ItemDetail.item_leg_des;
                dtrow["item_grp_id"] = _ItemDetail.item_grp_id;
                dtrow["base_uom_id"] = _ItemDetail.base_uom_id;
                //dtrow["pur_uom_id"] = _ItemDetail.pur_uom_id;
                dtrow["pur_uom_id"] = 0;
                //dtrow["sl_uom_id"] = _ItemDetail.sl_uom_id;
                dtrow["sl_uom_id"] = 0;
                dtrow["cost_price"] = IsNull(_ItemDetail.cost_price,"0");
                dtrow["sale_price"] = IsNull(_ItemDetail.sale_price,"0");
                dtrow["item_prf_id"] = _ItemDetail.item_prf_id;
                dtrow["issue_method"] = _ItemDetail.issue_method;
                dtrow["cost_method"] = _ItemDetail.cost_method;
                dtrow["min_stk_lvl"] = _ItemDetail.min_stk_lvl;
                dtrow["min_pr_stk"] = _ItemDetail.min_pr_stk;
                dtrow["re_ord_lvl"] = _ItemDetail.re_ord_lvl;
                if (_ItemDetail.stkout_warn)
                {
                    dtrow["stkout_warn"] = "Y";
                }
                else
                {
                    dtrow["stkout_warn"] = "N";
                }
                
                dtrow["item_remarks"] = _ItemDetail.item_remarks;
                //if (Session["TransType"].ToString() == "Save")
                if (_ItemDetail.TransType == "Save")
                {
                    dtrow["act_status"] = "Y";
                }
                else
                {
                    if (_ItemDetail.act_status)
                    {
                        dtrow["act_status"] = "Y";
                    }
                    else
                    {
                        dtrow["act_status"] = "N";
                    }
                }
                dtrow["inact_reason"] = _ItemDetail.inact_reason;
                //dtrow["wh_id"] = _ItemDetail.wh_id;
                dtrow["wh_id"] = 0;
               // dtrow["bin_id"] = _ItemDetail.bin_id;
                dtrow["bin_id"] = 0;
                dtrow["HSN_code"] = _ItemDetail.HSN_code;               
                if (_ItemDetail.i_sls)
                {
                    dtrow["i_sls"] = "Y";
                }
                else
                {
                    dtrow["i_sls"] = "N";
                }
                if (_ItemDetail.i_pur)
                {
                    dtrow["i_pur"] = "Y";
                }
                else
                {
                    dtrow["i_pur"] = "N";
                }
                if (_ItemDetail.i_wip)
                {
                    dtrow["i_wip"] = "Y";
                }
                else
                {
                    dtrow["i_wip"] = "N";
                }
                if (_ItemDetail.i_pack)
                {
                    dtrow["i_pack"] = "Y";
                }
                else
                {
                    dtrow["i_pack"] = "N";
                }
                if (_ItemDetail.i_capg)
                {
                    dtrow["i_capg"] = "Y";
                }
                else
                {
                    dtrow["i_capg"] = "N";
                }
                if (_ItemDetail.i_stk)
                {
                    dtrow["i_stk"] = "Y";
                }
                else
                {
                    dtrow["i_stk"] = "N";
                }
                if (_ItemDetail.i_qc)
                {
                    dtrow["i_qc"] = "Y";
                }
                else
                {
                    dtrow["i_qc"] = "N";
                }
                if (_ItemDetail.i_srvc)
                {
                    dtrow["i_srvc"] = "Y";
                }
                else
                {
                    dtrow["i_srvc"] = "N";
                }
                if (_ItemDetail.i_cons)
                {
                    dtrow["i_cons"] = "Y";
                }
                else
                {
                    dtrow["i_cons"] = "N";
                }
                if (_ItemDetail.i_serial)
                {
                    dtrow["i_serial"] = "Y";
                }
                else
                {
                    dtrow["i_serial"] = "N";
                }
                if (_ItemDetail.i_Sam)
                {
                    dtrow["i_Sam"] = "Y";
                }
                else
                {
                    dtrow["i_Sam"] = "N";
                }
                if (_ItemDetail.i_batch)
                {
                    dtrow["i_batch"] = "Y";
                }
                else
                {
                    dtrow["i_batch"] = "N";
                }
                if (_ItemDetail.i_exp)
                {
                    dtrow["i_exp"] = "Y";
                }
                else
                {
                    dtrow["i_exp"] = "N";
                }
                if (_ItemDetail.i_catalog)
                {
                    dtrow["i_catalog"] = "Y";
                }
                else
                {
                    dtrow["i_catalog"] = "N";
                }
                dtrow["create_id"] = Session["UserId"].ToString();
                dtrow["mod_id"] = Session["UserId"].ToString();
                dtrow["app_status"] = _ItemDetail.AppStatus;
                dtrow["loc_sls_coa"] = _ItemDetail.loc_sls_coa;
                dtrow["exp_sls_coa"] = _ItemDetail.exp_sls_coa;
                dtrow["loc_pur_coa"] = _ItemDetail.loc_pur_coa;
                dtrow["imp_pur_coa"] = _ItemDetail.imp_pur_coa;
                dtrow["stk_coa"] = _ItemDetail.stk_coa;
                dtrow["sal_ret_coa"] = _ItemDetail.sal_ret_coa;
                dtrow["Disc_coa"] = _ItemDetail.Disc_coa;
                dtrow["pur_ret_coa"] = _ItemDetail.pur_ret_coa;
                dtrow["prov_pay_coa"] =0;
                dtrow["cogs_coa"] = 0;
                dtrow["stk_adj_coa"] = 0;
                dtrow["dep_coa"] = _ItemDetail.dep_coa;
                dtrow["asset_coa"] = _ItemDetail.asset_coa;
                dtrow["wght_kg"] = _ItemDetail.wght_kg;
                dtrow["wght_ltr"] = _ItemDetail.wght_ltr;
                dtrow["gr_wght"] = _ItemDetail.gr_wght;
                dtrow["nt_wght"] = _ItemDetail.nt_wght;
                dtrow["item_hgt"] = _ItemDetail.item_hgt;
                dtrow["item_wdh"] = _ItemDetail.item_wdh;
                dtrow["item_len"] = _ItemDetail.item_len;
                dtrow["item_pack_sz"] = _ItemDetail.item_pack_sz;
                dtrow["CatlNoPrefix"] = _ItemDetail.CatlNoPrefix;
                dtrow["CatlNo"] = _ItemDetail.CatlNo;
                dtrow["SalesAlias"] = _ItemDetail.SalesAlias;
                dtrow["PurchaseAlias"] = _ItemDetail.PurchaseAlias;               
                dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                dtrow["UserIP"] = Session["UserIP"].ToString();
                dtrow["pack_uom"] = _ItemDetail.pack_uom;
                dtrow["pack_length"] = _ItemDetail.pack_length;
                dtrow["pack_width"] = _ItemDetail.pack_width;
                dtrow["pack_height"] = _ItemDetail.pack_height;
                dtrow["pack_cbm"] = _ItemDetail.pack_cbm;
                dtrow["comp_id"] = Session["CompId"].ToString();
                if (_ItemDetail.i_exempted)
                {
                    dtrow["tax_exemp"] = "Y";
                }
                else
                {
                    dtrow["tax_exemp"] = "N";
                }
                if (_ItemDetail.SubItem)
                {
                    dtrow["sub_item"] = "Y";
                }
                else
                {
                    dtrow["sub_item"] = "N";
                }
                dtrow["interBranch_sls_coa"] = _ItemDetail.InterBranch_sls_coa;
                dtrow["interBranch_pur_coa"] = _ItemDetail.InterBranch_pur_coa;
                dt.Rows.Add(dtrow);

                ItemDetail = dt;

                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("item_id", typeof(string));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(_ItemDetail.ItemBranchDetail);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                    {
                        dtrowBrdetails["item_id"] = _ItemDetail.item_id;
                    }
                    else
                    {
                        dtrowBrdetails["item_id"] = "0";
                    }

                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();

                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                ItemBranch = dtBranch;
                DataTable TempTable= new DataTable();
                DataTable TempAttribute = new DataTable();
                DataTable dtAttribute = new DataTable();

                dtAttribute.Columns.Add("item_id", typeof(string));
                dtAttribute.Columns.Add("attr_id", typeof(int));
                dtAttribute.Columns.Add("attr_val_id", typeof(int));
                dtAttribute.Columns.Add("comp_id", typeof(int));
                TempAttribute.Columns.Add("item_id", typeof(string));
                TempAttribute.Columns.Add("attr_name", typeof(string));
                TempAttribute.Columns.Add("attr_id", typeof(int));
                TempAttribute.Columns.Add("attr_val_name", typeof(string));
                TempAttribute.Columns.Add("attr_val_id", typeof(int));
                TempAttribute.Columns.Add("comp_id", typeof(int));

                JArray AddObject = JArray.Parse(_ItemDetail.ItemAttrDetail);
                for (int i = 0; i < AddObject.Count; i++)
                {
                    DataRow dtrowAttribute = dtAttribute.NewRow();
                    DataRow dtrowTemp = TempAttribute.NewRow();

                    if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                    {
                        dtrowAttribute["item_id"] = _ItemDetail.item_id;
                        dtrowTemp["item_id"] = _ItemDetail.item_id;
                    }
                    else
                    {
                        dtrowAttribute["item_id"] = "0";
                        dtrowTemp["item_id"] = "0";
                    }
                    dtrowAttribute["attr_id"] = AddObject[i]["AttriID"].ToString();
                    dtrowAttribute["attr_val_id"] = AddObject[i]["AttriValID"].ToString();
                    dtrowAttribute["comp_id"] = Session["CompId"].ToString();

                    dtrowTemp["attr_id"] = AddObject[i]["AttriID"].ToString();
                    dtrowTemp["attr_name"] = AddObject[i]["AttriName"].ToString();
                    dtrowTemp["attr_val_id"] = AddObject[i]["AttriValID"].ToString();
                    dtrowTemp["attr_val_name"] = AddObject[i]["AttriValName"].ToString();
                    dtrowTemp["comp_id"] = Session["CompId"].ToString();


                    dtAttribute.Rows.Add(dtrowAttribute);
                    TempAttribute.Rows.Add(dtrowTemp);
                }
               ItemAttribute = dtAttribute;
               TempTable = TempAttribute;
                
                ViewData["ItemAttribute"] = TempTable;
                DataTable dtCustomer = new DataTable();

                dtCustomer.Columns.Add("item_id", typeof(string));
                dtCustomer.Columns.Add("comp_id", typeof(int));
                dtCustomer.Columns.Add("cust_id", typeof(int));
                dtCustomer.Columns.Add("Item_code", typeof(string));
                dtCustomer.Columns.Add("item_des", typeof(string));
                dtCustomer.Columns.Add("pack_dt", typeof(string));
                dtCustomer.Columns.Add("remark", typeof(string));
 
                    JArray CustObject = JArray.Parse(_ItemDetail.ItemCustomerDetail);
                    for (int i = 0; i < CustObject.Count; i++)
                    {
                        DataRow dtrowCustomer = dtCustomer.NewRow();

                        if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                        {
                            dtrowCustomer["item_id"] = _ItemDetail.item_id;
                        }
                        else
                        {
                            dtrowCustomer["item_id"] = "0";
                        }
                        dtrowCustomer["comp_id"] = Session["CompId"].ToString();
                        dtrowCustomer["cust_id"] = CustObject[i]["CustID"].ToString();
                        dtrowCustomer["Item_code"] = CustObject[i]["ItemCode"].ToString();
                        dtrowCustomer["item_des"] = CustObject[i]["ItemDes"].ToString();
                        dtrowCustomer["pack_dt"] = CustObject[i]["Packdt"].ToString();
                        dtrowCustomer["remark"] = CustObject[i]["ItemRem"].ToString();

                        dtCustomer.Rows.Add(dtrowCustomer);                   
                    }
                    ItemCustomer = dtCustomer;
                    ViewData["ItemCustomer"] = ItemCustomer;   
                DataTable dtSupplier = new DataTable();

                dtSupplier.Columns.Add("item_id", typeof(string));
                dtSupplier.Columns.Add("comp_id", typeof(int));
                dtSupplier.Columns.Add("supp_id", typeof(int));
                dtSupplier.Columns.Add("Item_code", typeof(string));
                dtSupplier.Columns.Add("item_des", typeof(string));
                dtSupplier.Columns.Add("pack_dt", typeof(string));
                dtSupplier.Columns.Add("wt", typeof(string));
                dtSupplier.Columns.Add("remark", typeof(string));
                dtSupplier.Columns.Add("box_dt", typeof(string));

                JArray SuppObject = JArray.Parse(_ItemDetail.ItemSupplierDetail);
                    for (int i = 0; i < SuppObject.Count; i++)
                    {
                        DataRow dtrowSupplier = dtSupplier.NewRow();

                        if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                        {
                            dtrowSupplier["item_id"] = _ItemDetail.item_id;
                        }
                        else
                        {
                            dtrowSupplier["item_id"] = "0";
                        }
                        dtrowSupplier["comp_id"] = Session["CompId"].ToString();
                        dtrowSupplier["supp_id"] = SuppObject[i]["SuppID"].ToString();
                        dtrowSupplier["Item_code"] = SuppObject[i]["ItemCode"].ToString();
                        dtrowSupplier["item_des"] = SuppObject[i]["ItemDes"].ToString();
                        dtrowSupplier["pack_dt"] = SuppObject[i]["Packdt"].ToString();                        
                        dtrowSupplier["wt"] = SuppObject[i]["Wt"].ToString();
                        dtrowSupplier["remark"] = SuppObject[i]["ItemRem"].ToString();
                    dtrowSupplier["box_dt"] = SuppObject[i]["Boxdt"].ToString();

                    dtSupplier.Rows.Add(dtrowSupplier);
                    }
                    ItemSupplier = dtSupplier;
                    ViewData["ItemSupplier"] = ItemSupplier;

                DataTable dtPortfolio = new DataTable();

                dtPortfolio.Columns.Add("item_id", typeof(string));
                dtPortfolio.Columns.Add("comp_id", typeof(int));
                dtPortfolio.Columns.Add("prf_id", typeof(int));
                dtPortfolio.Columns.Add("prf_des", typeof(string));
                dtPortfolio.Columns.Add("remark", typeof(string));

                JArray PortfObject = JArray.Parse(_ItemDetail.ItemPortfolioDetail);
                for (int i = 0; i < PortfObject.Count; i++)
                {
                    DataRow dtrowPortf = dtPortfolio.NewRow();

                    if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                    {
                        dtrowPortf["item_id"] = _ItemDetail.item_id;
                    }
                    else
                    {
                        dtrowPortf["item_id"] = "0";
                    }
                    dtrowPortf["comp_id"] = Session["CompId"].ToString();
                    dtrowPortf["prf_id"] = PortfObject[i]["PortfID"].ToString();
                    dtrowPortf["prf_des"] = PortfObject[i]["PortfDes"].ToString();
                    dtrowPortf["remark"] = PortfObject[i]["PortfRem"].ToString();
                    dtPortfolio.Rows.Add(dtrowPortf);
                }
                ItemPortfolio = dtPortfolio;
                ViewData["ItemPortfolio"] = ItemPortfolio;

                DataTable dtVehicle = new DataTable();
                dtVehicle.Columns.Add("item_id", typeof(string));
                dtVehicle.Columns.Add("comp_id", typeof(int));
                dtVehicle.Columns.Add("setup_id", typeof(int));
                dtVehicle.Columns.Add("model_no", typeof(string));
                dtVehicle.Columns.Add("veh_oem_no", typeof(string));
                dtVehicle.Columns.Add("veh_part_no", typeof(string));
                dtVehicle.Columns.Add("veh_item_des", typeof(string));
                dtVehicle.Columns.Add("remark", typeof(string));
               
                JArray VehicleObject = JArray.Parse(_ItemDetail.ItemVehicleDetail);
                for (int i = 0; i < VehicleObject.Count; i++)
                {
                    DataRow dtrowVehicle = dtVehicle.NewRow();

                    if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                    {
                        dtrowVehicle["item_id"] = _ItemDetail.item_id;
                    }
                    else
                    {
                        dtrowVehicle["item_id"] = "0";
                    }
                    dtrowVehicle["comp_id"] = Session["CompId"].ToString();
                    dtrowVehicle["setup_id"] = VehicleObject[i]["VehID"].ToString();
                    dtrowVehicle["model_no"] = VehicleObject[i]["Model"].ToString();
                    dtrowVehicle["veh_oem_no"] = VehicleObject[i]["OEM"].ToString();
                    dtrowVehicle["veh_part_no"] = VehicleObject[i]["PartNo"].ToString();
                    dtrowVehicle["veh_item_des"] = VehicleObject[i]["DES"].ToString();
                    dtrowVehicle["remark"] = VehicleObject[i]["Rem"].ToString();
                    
                    dtVehicle.Rows.Add(dtrowVehicle);
                }
                ItemVehicle = dtVehicle;
                ViewData["ItemVehicle"] = ItemVehicle;

                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("comp_id", typeof(int));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_name", typeof(string));
               

                JArray SubItemObject = JArray.Parse(_ItemDetail.ItemSubItemDetail);
                for (int i = 0; i < SubItemObject.Count; i++)
                {
                    DataRow dtrowSubItem = dtSubItem.NewRow();

                    if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                    {
                        dtrowSubItem["item_id"] = _ItemDetail.item_id;
                    }
                    else
                    {
                        dtrowSubItem["item_id"] = "0";
                    }
                    dtrowSubItem["comp_id"] = Session["CompId"].ToString();
                    dtrowSubItem["sub_item_id"] = SubItemObject[i]["SubItemId"].ToString();
                    dtrowSubItem["sub_item_name"] = SubItemObject[i]["SubItemName"].ToString();
                   
                   
                    dtSubItem.Rows.Add(dtrowSubItem);
                }
                ItemSubItem = dtSubItem;
                ViewData["ItemSubItem"] = ItemSubItem;
                var ItemDetailsattc = TempData["ModelDataattch"] as ItemDetailsattch;
                TempData["ModelDataattch"] = null;
                DataTable dtAttachment = new DataTable();
                //if (_ItemDetail.attatchmentdetail != null)
                //{
                //    if (ItemDetailsattc != null)
                //    {
                //        if (ItemDetailsattc.AttachMentDetailItmStp != null)
                //        {
                //            dtAttachment = ItemDetailsattc.AttachMentDetailItmStp as DataTable;
                //        }
                //        else
                //        {
                //            dtAttachment.Columns.Add("id", typeof(string));
                //            dtAttachment.Columns.Add("file_name", typeof(string));
                //            dtAttachment.Columns.Add("file_path", typeof(string));
                //            dtAttachment.Columns.Add("file_def", typeof(char));
                //            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                //        }
                //    }
                //    else
                //    {
                //        if (_ItemDetail.AttachMentDetailItmStp != null)
                //        {
                //            dtAttachment = _ItemDetail.AttachMentDetailItmStp as DataTable;
                //        }
                //        else
                //        {
                //            dtAttachment.Columns.Add("id", typeof(string));
                //            dtAttachment.Columns.Add("file_name", typeof(string));
                //            dtAttachment.Columns.Add("file_path", typeof(string));
                //            dtAttachment.Columns.Add("file_def", typeof(char));
                //            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                //        }
                //    }   
                //    JArray jObject1 = JArray.Parse(_ItemDetail.attatchmentdetail);
                //    for (int i = 0; i < jObject1.Count; i++)
                //    {
                //        string flag = "Y";
                //        foreach (DataRow dr in dtAttachment.Rows)
                //        {
                //            string drImg = dr["file_name"].ToString();
                //            string ObjImg = jObject1[i]["file_name"].ToString();
                //            if (drImg == ObjImg)
                //            {
                //                flag = "N";
                //            }
                //        }
                //        if (flag == "Y")
                //        {

                //            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                //            if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                //            {
                //                dtrowAttachment1["id"] = _ItemDetail.item_id;
                //            }
                //            else
                //            {
                //                dtrowAttachment1["id"] = "0";
                //            }
                //            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                //            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                //            dtrowAttachment1["file_def"] = jObject1[i]["file_def"].ToString();
                //            dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                //            dtAttachment.Rows.Add(dtrowAttachment1);
                //        }
                //    }
                //    if (_ItemDetail.TransType == "Update")
                //    {

                //        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                //        if (Directory.Exists(AttachmentFilePath))
                //        {
                //            string ItmCode = string.Empty;
                //            if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                //            {
                //                ItmCode = _ItemDetail.item_id;
                //            }
                //            else
                //            {
                //                ItmCode = "0";
                //            }
                //            string[] filePaths = Directory.GetFiles(AttachmentFilePath, comp_id + ItmCode.Replace("/", "") + "*");

                //            foreach (var fielpath in filePaths)
                //            {
                //                string flag = "Y";
                //                foreach (DataRow dr in dtAttachment.Rows)
                //                {
                //                    string drImgPath = dr["file_path"].ToString();
                //                    if (drImgPath == fielpath.Replace("/",@"\"))
                //                    {
                //                        flag = "N";
                //                    }
                //                }
                //                if (flag == "Y")
                //                {
                //                    System.IO.File.Delete(fielpath);
                //                }
                //            }
                //        }
                //    }
                //    ItemAttachments = dtAttachment;
                //}
                //-------------------------------------------------Attachemnt New---------------------------
                string Guid1 = "";
                if (ItemDetailsattc != null)
                {
                    if (ItemDetailsattc.Guid != null)
                    {
                        Guid1 = ItemDetailsattc.Guid;
                    }
                }
                if (Guid1 == "")
                {
                    Guid1 = _ItemDetail.item_id;
                }
                dtAttachment.Columns.Add("id", typeof(string));
                dtAttachment.Columns.Add("file_name", typeof(string));
                dtAttachment.Columns.Add("file_path", typeof(string));
                dtAttachment.Columns.Add("file_def", typeof(char));
                dtAttachment.Columns.Add("comp_id", typeof(Int32));
                
                    string tempFolderName = comp_id + userid + DocumentMenuId;
                string AttachmentFilePath1 = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\TempAttachment\\" + tempFolderName + "/";
                string AttachmentFilePath2 = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "\\";
                string[] filePaths1 = Directory.GetFiles(AttachmentFilePath2, comp_id + Guid1 + "_" + "*");
                foreach (string file in filePaths1)
                {
                    System.IO.File.Delete(file);
                }
                if (Directory.Exists(AttachmentFilePath1))
                {
                    string ItmCode = string.Empty;
                    if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                    {
                        ItmCode = _ItemDetail.item_id;
                    }
                    else
                    {
                        ItmCode = "0";
                    }
                    string[] filePaths = Directory.GetFiles(AttachmentFilePath1, comp_id + Guid1 + "*");

                    foreach (var fielpath in filePaths)
                    {
                        
                        string file_name = "";
                        string Newfile_name = "";
                        FileInfo fileInfo = new FileInfo(fielpath);
                        
                        file_name = fileInfo.Name;
                       
                        if (file_name.Contains('§'))
                        {
                            Newfile_name= fileInfo.Name.Split('§')[0];
                        }
                        else
                        {
                            Newfile_name = fileInfo.Name;
                        }
                            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                        if (!string.IsNullOrEmpty(_ItemDetail.item_id))
                        {
                            dtrowAttachment1["id"] = _ItemDetail.item_id;
                            
                        }
                        else
                        {
                            dtrowAttachment1["id"] = "0";
                            
                        }
                        
                        string doc_path = Path.Combine(AttachmentFilePath2 , Newfile_name);

                        if (_ItemDetail.TransType == "Save")
                        {
                            dtrowAttachment1["file_name"] = Newfile_name.Replace(comp_id + Guid1 + "_", "");
                            dtrowAttachment1["file_path"] = AttachmentFilePath2;
                        }
                        else
                        {
                            dtrowAttachment1["file_name"] = Newfile_name;
                            dtrowAttachment1["file_path"] = doc_path;
                        }

                      

                        if (_ItemDetail.TransType == "Save")
                        {
                            string compareName;
                            if (file_name.Contains('§'))
                            {
                                string[] parts = file_name.Split('§');
                                compareName = parts.Length > 1 ? parts[1] : file_name;
                            }
                            else
                            {
                                compareName = file_name; // no "!" → use full name
                            }

                            if (_ItemDetail.attatchmentDefaultdetail == compareName)
                            {
                                dtrowAttachment1["file_def"] = "Y";
                            }
                            else
                            {
                                dtrowAttachment1["file_def"] = "N";
                            }
                        }
                        else
                        {
                            dtrowAttachment1["file_def"] = "N";
                        }
                           
                        dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                        dtAttachment.Rows.Add(dtrowAttachment1);
                        if (!Directory.Exists(AttachmentFilePath2))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(AttachmentFilePath2);
                        }
                        
                        System.IO.File.Move(fielpath, doc_path);

                    }
                    Directory.Delete(AttachmentFilePath1);
                    
                }
            ItemAttachments = dtAttachment;
                //-------------------------------------------------Attachemnt New End---------------------------

                if(_ItemDetail.TransType == "Update")
                {
                    foreach (DataRow dr in ItemAttachments.Rows)
                    {
                        string file_name = dr["file_name"].ToString();
                        file_name = file_name.Substring(file_name.IndexOf('_') + 1);
                        var arr = file_name.Split('.');
                        string file_name1 = arr[0].Substring(0, arr[0].Length - 1) + "." + arr[1];
                        if (_ItemDetail.attatchmentDefaultdetail == file_name || _ItemDetail.attatchmentDefaultdetail == file_name1)
                        {
                            dr["file_def"] = "Y";
                        }
                        else
                        {
                            dr["file_def"] = "N";
                        }
                    }
                }
              
                SaveMessage = _ItemDetail_ISERVICES.InsertItemSetupDetailsDAL(ItemDetail, ItemBranch, ItemAttachments, ItemAttribute, ItemCustomer, ItemSupplier, ItemPortfolio, ItemVehicle, ItemSubItem,Convert.ToInt32(_ItemDetail.ExpiryAlertDays));

                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Data_Not_Found")
                {
                    string tittle = title.Replace(" ", "");
                    var a = ItemCode.Split('-');
                    var msg = Message.Replace("_"," ")+" "+ a[0].Trim()+" in "+ tittle;  
                    //var msg = "Data Not Found" +" "+ a[0].Trim();
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _ItemDetail.Message = Message.Replace("_","") ;
                    _ItemDetail.Savebtn = null;
                    return RedirectToAction("ItemDetail");
                }
                //if (Session["TransType"].ToString() == "Save")
                if (_ItemDetail.TransType == "Save")
                {
                    string Guid = "";
                    if (ItemDetailsattc != null)
                    {
                        if (ItemDetailsattc.Guid != null)
                        {
                            Guid = ItemDetailsattc.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    if (ItemAttachments.Rows.Count > 0)
                    {
                        comCont.ResetImageLocation(comp_id, "00", guid, PageName, ItemCode, _ItemDetail.TransType, ItemAttachments);
                    }
                }
                if (Message == "Update" || Message == "Save")
                {
                    _ItemDetail.Message = "Save";
                    _ItemDetail.ItemCode = ItemCode;
                    _ItemDetail.TransType = "Update";
                }

                if (Message == "Duplicate")
                {
                    _ItemDetail.Savebtn = null;
                    _ItemDetail.TransType = "Duplicate";
                    _ItemDetail.Message = "Duplicate";
                    _ItemDetail.ItemCode = ItemCode;
                    _ItemDetail.act_status = true;

                    /*Added By Nitesh 03-01-2025 For When Save ItemNameDuplicate add Column ItemUsedInTrans For Sub_item*/
                    ViewData["ItemSubItem"] = null;
                    dtSubItem.Columns.Add("ItemUsedInTrans", typeof(string));
                    ViewData["ItemSubItem"] = ItemSubItem;
                    /*End*/
                }
                
                if (_ItemDetail.Status == "A")
                {
                    _ItemDetail.AppStatus = "A";
                }
                else
                {
                    _ItemDetail.AppStatus = "D";
                }
                _ItemDetail.BtnName = "BtnSave";
                TempData["ModelData"] = _ItemDetail;
               return RedirectToAction("ItemDetail");
             
            }
            catch (Exception ex)
            {
                _ItemDetail.Savebtn = null;
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_ItemDetail.TransType == "Save")
                    {
                        string Guid = "";
                        if (_ItemDetail.Guid != null)
                        {
                            Guid = _ItemDetail.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(comp_id, PageName, Guid,Server);
                    }
                }
                throw ex;
            }

        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        private ActionResult ItemDetailDelete(ItemDetailModel _ItemDetail, string command)
        {
            try
            {               
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();

                }
                string item_id = _ItemDetail.item_id;
                DataSet Message = _ItemDetail_ISERVICES.ItemDetailDelete(_ItemDetail, comp_id, item_id);
                if (!string.IsNullOrEmpty(item_id))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(comp_id, PageName, item_id,Server);
                }
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Delete";
                //Session["ItemCode"] = "";
                //_ItemDetail = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "Delete";
                ItemDetailModel _ItemDetailDelete = new ItemDetailModel();
                _ItemDetailDelete.Message = "Deleted";
                _ItemDetailDelete.Command = "Delete";
                _ItemDetailDelete.ItemCode = "";
                _ItemDetailDelete.TransType = "Refresh";
                _ItemDetailDelete.AppStatus = "DL";
                _ItemDetailDelete.BtnName = "Delete";
                TempData["ModelData"] = _ItemDetailDelete;
                return RedirectToAction("ItemDetail", "ItemDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult ItemApprove(ItemDetailModel _ItemDetail, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                string item_id = _ItemDetail.item_id;
                string app_id = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                DataSet Message = _ItemDetail_ISERVICES.ItemApprove(_ItemDetail, comp_id, app_id, item_id,mac_id);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                //Session["ItemCode"] = _ItemDetail.item_id;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'A';
                //Session["BtnName"] = "BtnApprove";

                _ItemDetail.TransType = "Update";
                _ItemDetail.Command = command;
                _ItemDetail.ItemCode = _ItemDetail.item_id;
                _ItemDetail.Message = "Approved";
                _ItemDetail.AppStatus = "A";
                _ItemDetail.BtnName = "BtnApprove";
                TempData["ModelData"] = _ItemDetail;
                return RedirectToAction("ItemDetail", "ItemDetail");
            }
            catch (Exception ex)
            {
                _ItemDetail.Savebtn = null;
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetAutoCompleteSearchSuggestion(ItemDetailModel _ItemDetail)
        {
            string GroupName = string.Empty;
            Dictionary<string, string> GroupList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_ItemDetail.ddlgroup_name))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _ItemDetail.ddlgroup_name;
                }
                GroupList = _ItemDetail_ISERVICES.GetGroupList(Comp_ID, GroupName);

                List<GroupName> _GroupList = new List<GroupName>();
                foreach (var data in GroupList)
                {
                    GroupName _GroupDetail = new GroupName();
                    _GroupDetail.item_grp_id = data.Key;
                    _GroupDetail.ItemGroupChildNood = data.Value;
                    _GroupList.Add(_GroupDetail);
                }
                _ItemDetail.GroupList = _GroupList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(GroupList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteSearchHSN(ItemDetailModel _ItemDetail)
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
                if (string.IsNullOrEmpty(_ItemDetail.ddlhsncode))
                {
                    HSNCode = "0";
                }
                else
                {
                    HSNCode = _ItemDetail.ddlhsncode;
                }
                HSNList = _ItemDetail_ISERVICES.ItemSetupHSNDAL(Comp_ID, HSNCode);

                List<HSNno> _HSNList = new List<HSNno>();
                foreach (var data in HSNList)
                {
                    HSNno _HsnDetail = new HSNno();
                    _HsnDetail.setup_id = data.Key;
                    _HsnDetail.setup_val = data.Value;
                    _HSNList.Add(_HsnDetail);
                }
                _ItemDetail.HSNList = _HSNList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(HSNList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteFileFromTemp(string file_name)
        {
            try
            {
                string result = "";
                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                string tempFolderName = comp_id + userid + DocumentMenuId;
                string AttachmentFilePath1 = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\TempAttachment\\" + tempFolderName;
                string doc_path = Path.Combine(AttachmentFilePath1 + "\\", file_name);
                string[] filePaths1 = Directory.GetFiles(AttachmentFilePath1, file_name);
                if (filePaths1.Length > 0)
                {
                    foreach (string file in filePaths1)
                    {
                        System.IO.File.Delete(doc_path);
                    }
                    result = "deleted";
                }
                else
                {
                    result = "NotFound";
                }
                return Json(result);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetItemBranchList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _ItemDetail_ISERVICES.GetBrList(Comp_ID).Tables[0];
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        [NonAction]
        private DataTable GetBrList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _ItemDetail_ISERVICES.GetBrListDAL(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        [NonAction]
        private DataTable GetUOMList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _ItemDetail_ISERVICES.GetUOMDAL(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
           
        }
        [NonAction]
        private DataTable GetBinList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _ItemDetail_ISERVICES.GetbinDAL(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
           
        }
        [NonAction]
        private DataTable GetAttrList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _ItemDetail_ISERVICES.GetAttributeListDAL(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetWHList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _ItemDetail_ISERVICES.GetwarehouseDAL(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
           
        }
        [NonAction]
        private DataTable GetPortfolioList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _ItemDetail_ISERVICES.GetportfDAL(Comp_ID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
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
        [HttpPost]
        public ActionResult GetSelectedParentDetail(string item_grp_id)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    string Comp_ID = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    DataSet result = _ItemDetail_ISERVICES.GetSelectedParentDetail(item_grp_id, Comp_ID);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
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

        [HttpPost]
        public ActionResult GetItemPropertyToggleDetail(string item_grp_id)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    string Comp_ID = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    DataSet result = _ItemDetail_ISERVICES.GetItemPropertyToggleDetail(item_grp_id, Comp_ID);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                }
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

        [HttpPost]
        public ActionResult GetAttributeValue(string AttributeID)
        {
            try
            {
                JsonResult DataRows = null;
                if (ModelState.IsValid)
                {
                    string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }

                    DataSet result = _ItemDetail_ISERVICES.GetAttributeValueDAL(Comp_ID, AttributeID);
                    DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                }
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
        //[HttpPost]
        //public ActionResult AutoGenerateRef_NoAndCatlog_no(string stockable, string saleable, string ItemCatalog)
        //{
        //    try
        //    {
        //        JsonResult DataRows = null;
        //        if (ModelState.IsValid)
        //        {
        //            string Comp_ID = string.Empty;
        //            if (Session["CompId"] != null)
        //            {
        //                Comp_ID = Session["CompId"].ToString();
        //            }

        //            DataSet result = _ItemDetail_ISERVICES.AutoGenerateRef_No_Catlog_no(Comp_ID, stockable, saleable, ItemCatalog);
        //            DataRows = Json(JsonConvert.SerializeObject(result.Tables[0]), JsonRequestBehavior.AllowGet);
        //        }
        //        return DataRows;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //        //throw Ex;
        //    }
        //}
        [HttpPost]
        public ActionResult AutoGenerateRef_NoAndCatlog_no(string stockable, string saleable, string ItemCatalog)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return Json(new { error = "Invalid input" });
                //}

                string Comp_ID = Session["CompId"]?.ToString() ?? "";

                DataSet result = _ItemDetail_ISERVICES.AutoGenerateRef_No_Catlog_no(Comp_ID, stockable, saleable, ItemCatalog);

                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    var row = result.Tables[0].Rows[0];

                    // Safely extract values
                    var refNo = row.Table.Columns.Contains("Generated_REF_NO") ? row["Generated_REF_NO"]?.ToString() : null;
                    var catalogNo = row.Table.Columns.Contains("CATALOG_NO") ? row["CATALOG_NO"]?.ToString() : null;

                    return Json(new
                    {
                        Generated_REF_NO = refNo,
                        CATALOG_NO = catalogNo
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Generated_REF_NO = "", CATALOG_NO = "" });
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult GetLocalSaleAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalSaleAccount(AccName, Comp_ID);

                List<IncomeCOA> _COAList = new List<IncomeCOA>();
                foreach (var data in COAList)
                {
                    IncomeCOA _COADetail = new IncomeCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.IncomeCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExportSaleAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalSaleAccount(AccName, Comp_ID);
                List<IncomeCOA> _COAList = new List<IncomeCOA>();
                foreach (var data in COAList)
                {
                    IncomeCOA _COADetail = new IncomeCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.IncomeCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLocalPurchaseAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetImportPurchaseAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //Commented by Mukesh- as said by Vishal Sir
        public ActionResult GetStockAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetStockAccount(AccName, Comp_ID);
                List<AssetsCOA> _COAList = new List<AssetsCOA>();
                foreach (var data in COAList)
                {
                    AssetsCOA _COADetail = new AssetsCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.AssetsCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSaleReturnAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalSaleAccount(AccName, Comp_ID);
                List<IncomeCOA> _COAList = new List<IncomeCOA>();
                foreach (var data in COAList)
                {
                    IncomeCOA _COADetail = new IncomeCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                
                _ItemDetail.IncomeCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDiscountAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPurchaseReturnAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);
                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //Commented by Mukesh- as said by Vishal Sir
        //public ActionResult GetProvisionalPayableAccount(ItemDetailModel _ItemDetail)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string AccName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> COAList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
        //        {
        //            AccName = "0";
        //        }
        //        else
        //        {
        //            AccName = _ItemDetail.ddlcoa_name;
        //        }
        //        COAList = _ItemDetail_ISERVICES.GetProvisionalPayableAccount(AccName, Comp_ID);

        //        List<LiabilityCOA> _COAList = new List<LiabilityCOA>();
        //        foreach (var data in COAList)
        //        {
        //            LiabilityCOA _COADetail = new LiabilityCOA();
        //            _COADetail.coa_id = data.Key;
        //            _COADetail.coa_name = data.Value;
        //            _COAList.Add(_COADetail);
        //        }
        //        _ItemDetail.LiabilityCOAList = _COAList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetcostOfGoodsSoldAccount(ItemDetailModel _ItemDetail)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string AccName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> COAList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
        //        {
        //            AccName = "0";
        //        }
        //        else
        //        {
        //            AccName = _ItemDetail.ddlcoa_name;
        //        }
        //        COAList = _ItemDetail_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

        //        List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
        //        foreach (var data in COAList)
        //        {
        //            ExpenseCOA _COADetail = new ExpenseCOA();
        //            _COADetail.coa_id = data.Key;
        //            _COADetail.coa_name = data.Value;
        //            _COAList.Add(_COADetail);
        //        }
        //        _ItemDetail.ExpenseCOAList = _COAList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetStockAdjustmentAccount(ItemDetailModel _ItemDetail)
        //{
        //    string Comp_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    string AccName = string.Empty;
        //    //string ErrorMessage = "success";
        //    Dictionary<string, string> COAList = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
        //        {
        //            AccName = "0";
        //        }
        //        else
        //        {
        //            AccName = _ItemDetail.ddlcoa_name;
        //        }
        //        COAList = _ItemDetail_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

        //        List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
        //        foreach (var data in COAList)
        //        {
        //            ExpenseCOA _COADetail = new ExpenseCOA();
        //            _COADetail.coa_id = data.Key;
        //            _COADetail.coa_name = data.Value;
        //            _COAList.Add(_COADetail);
        //        }
        //        _ItemDetail.ExpenseCOAList = _COAList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetDepreciationAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetLocalPurchaseAccount(AccName, Comp_ID);

                List<ExpenseCOA> _COAList = new List<ExpenseCOA>();
                foreach (var data in COAList)
                {
                    ExpenseCOA _COADetail = new ExpenseCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.ExpenseCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAssetAccount(ItemDetailModel _ItemDetail)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string AccName = string.Empty;
            //string ErrorMessage = "success";
            Dictionary<string, string> COAList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(_ItemDetail.ddlcoa_name))
                {
                    AccName = "0";
                }
                else
                {
                    AccName = _ItemDetail.ddlcoa_name;
                }
                COAList = _ItemDetail_ISERVICES.GetStockAccount(AccName, Comp_ID);
                List<AssetsCOA> _COAList = new List<AssetsCOA>();
                foreach (var data in COAList)
                {
                    AssetsCOA _COADetail = new AssetsCOA();
                    _COADetail.coa_id = data.Key;
                    _COADetail.coa_name = data.Value;
                    _COAList.Add(_COADetail);
                }
                _ItemDetail.AssetsCOAList = _COAList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(COAList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetCustListAuto(ItemDetailModel _ItemDetail)
        {
            JsonResult DataRows = null;
            string CustomerName = string.Empty;
            //Dictionary<string, string> SuppList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_ItemDetail.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _ItemDetail.CustName;
                }
                DataSet custlist = _ItemDetail_ISERVICES.GetCustomerList(Comp_ID, CustomerName);
                DataRow dr;
                dr = custlist.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                custlist.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(custlist));/*Result convert into Json Format for javasript*/
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
        public ActionResult GetPortfolioListAuto(ItemDetailModel _ItemDetail)
        {
            JsonResult DataRows = null;
            string PortfolioName = string.Empty;
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
                if (string.IsNullOrEmpty(_ItemDetail.PortfName))
                {
                    PortfolioName = "0";
                }
                else
                {
                    PortfolioName = _ItemDetail.PortfName;
                }
                DataSet prflist = _ItemDetail_ISERVICES.GetPortfList(Comp_ID, PortfolioName);
                DataRow dr;
                dr = prflist.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                prflist.Tables[0].Rows.InsertAt(dr, 0);

                DataRows = Json(JsonConvert.SerializeObject(prflist));/*Result convert into Json Format for javasript*/
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
        public ActionResult GetVehicleListAuto(ItemDetailModel _ItemDetail)
        {
            JsonResult DataRows = null;
            string VehicleName = string.Empty;
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
                if (string.IsNullOrEmpty(_ItemDetail.VehicleName))
                {
                    VehicleName = "0";
                }
                else
                {
                    VehicleName = _ItemDetail.VehicleName;
                }
                DataSet Vehclist = _ItemDetail_ISERVICES.GetVehicleList(Comp_ID, VehicleName);
                DataRow dr;
                dr = Vehclist.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                Vehclist.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(Vehclist));/*Result convert into Json Format for javasript*/
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
        public ActionResult GetSuppListAuto(ItemDetailModel _ItemDetail)
        {
            JsonResult DataRows = null;
            string SupplierName = string.Empty;           
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
                if (string.IsNullOrEmpty(_ItemDetail.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _ItemDetail.SuppName;
                }
                DataSet supplist = _ItemDetail_ISERVICES.GetSupplierList(Comp_ID, SupplierName);
                DataRow dr;
                dr = supplist.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                supplist.Tables[0].Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(supplist));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                ItemDetailsattch ItemDetailsattc = new ItemDetailsattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                
                //string TransType = "";
                //string ItemCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["ItemCode"] != null)
                //{
                //    ItemCode = Session["ItemCode"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                ItemDetailsattc.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, comp_id, DocNo, Files, Server, (UserID+DocumentMenuId));
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    ItemDetailsattc.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    ItemDetailsattc.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = ItemDetailsattc;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }

    }

}