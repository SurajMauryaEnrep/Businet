using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.CostCenterReAllocation;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CostCenterReAllocation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.CostCenterReAllocation
{
    public class CostCenterReAllocationController : Controller
    {
        string CompID, BrID, language = String.Empty;
        string DocumentMenuId = "105104115155", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CostCenterReAllocation_ISERVICE _CCRA_ISERVICE;
        public CostCenterReAllocationController(Common_IServices _Common_IServices, CostCenterReAllocation_ISERVICE _CCRA_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._CCRA_ISERVICE = _CCRA_ISERVICE;
        }
        // GET: ApplicationLayer/CostCenterReAllocation
        public ActionResult CostCenterReAllocation()
        {
            CostCenterReAllocation_Model _Model = new CostCenterReAllocation_Model();
            ViewBag.MenuPageName = getDocumentName();
            GetAllDropDownData(_Model);/**Get All Data in One Procedure**/
            _Model.Title = title;
            ViewBag.DocumentMenuId = DocumentMenuId;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CostCenterReAllocation/CostCenterReAllocationDetail.cshtml", _Model);
        }
        private void GetAllDropDownData(CostCenterReAllocation_Model _Model)/*Created by Hina sharma on 16-11-2024 to multiselect dropdown*/
        {
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string GLAccName = string.Empty;
            string User_ID = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            if (Session["UserId"] != null)
            {
                User_ID = Session["UserId"].ToString();
            }
            if (string.IsNullOrEmpty(_Model.GLAccount))
            {
                GLAccName = "0";
            }
            else
            {
                GLAccName = _Model.GLAccount;
            }
            
            
            DataSet ds = _CCRA_ISERVICE.GetAllDDLData(Comp_ID, Br_ID, GLAccName);
            //------Bind GLAccountName List------------
            List<GLAccountName> _AccList = new List<GLAccountName>();
            foreach (DataRow data in ds.Tables[0].Rows)
            {
                GLAccountName _AccDetail = new GLAccountName();
                _AccDetail.acc_id = data["acc_id"].ToString();
                _AccDetail.acc_name = data["acc_name"].ToString();
                _AccList.Add(_AccDetail);
            }
            _AccList.Insert(0, new GLAccountName() { acc_id = "0", acc_name = "---Select---" });
            _Model.GLAccNameList = _AccList;

            //------Bind Cost Center Type List------------
            List<CostCenterTypeList> typelist = new List<CostCenterTypeList>();

            foreach (DataRow data in ds.Tables[1].Rows)
            {
                CostCenterTypeList list = new CostCenterTypeList();
                list.cctyp_id = data["cc_id"].ToString();
                list.cctyp_name = data["cc_name"].ToString();
                typelist.Add(list);
            }
            typelist.Insert(0, new CostCenterTypeList() { cctyp_id = "0", cctyp_name = "---Select---" });
            _Model.CostCenterTypLists = typelist;

            if (ds.Tables[3].Rows.Count > 0)
            {
                _Model.FromDate = ds.Tables[3].Rows[0]["from_dt"].ToString();
                _Model.ToDate = ds.Tables[3].Rows[0]["to_dt"].ToString();
            }
        }
        public ActionResult GetCostCenterValueList(string CCTypeIDS)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ds = _CCRA_ISERVICE.GetCostCenterValueListByCostCenterType(CompID, BrID, CCTypeIDS);
                return Json(JsonConvert.SerializeObject(ds.Tables[0]), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult OnSrchGetCostCenterReAllocationReport(string GlAcc_id, string CCTyp_id, string CC_Val_id,string AllocationTyp_id, string From_Dt, string To_Dt)
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
                TempData["CCRAFilter"] = "CCRAFilter";
                DataTable dt = _CCRA_ISERVICE.OnSrchGetCCRAReport(CompID, BrID, GlAcc_id, CCTyp_id, CC_Val_id, AllocationTyp_id, From_Dt, To_Dt).Tables[0];
                ViewBag.CostCenterReAllocationDetail = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialCostCenterReAllocationDetail.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }

        }

        public ActionResult FetchCostCenterData(/*string Int_Br_Id, */string Vou_No, string Vou_Dt, string GLAcc_id,string Flag, string CC_rowdata)
        {
            try
            {
                CostCenterDt _CC_Model = new CostCenterDt();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                
                DataSet ds = _CCRA_ISERVICE.GetCostCenterData(CompID, BrID, /*Int_Br_Id,*/ Vou_No, Vou_Dt, GLAcc_id);
                //ViewBag.CostCenterReAllocationDetail = ds;
                ViewBag.CC_type = ds.Tables[0];
                    
                ViewBag.CC_Item = ds.Tables[1];
                if (CC_rowdata.ToString() != null && CC_rowdata.ToString() != "" && CC_rowdata.ToString() != "[]")
                {
                    DataTable Cctype = new DataTable();
                    Cctype.Columns.Add("GlAccount", typeof(string));
                    Cctype.Columns.Add("ddl_cc_type", typeof(string));
                    Cctype.Columns.Add("dd_cc_type_id", typeof(string));
                    JArray arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow dtrowLines = Cctype.NewRow();
                        dtrowLines["GlAccount"] = arr[i]["GlAccount"].ToString();
                        dtrowLines["ddl_cc_type"] = arr[i]["ddl_CC_Type"].ToString();
                        dtrowLines["dd_cc_type_id"] = arr[i]["ddl_Type_Id"].ToString();
                        Cctype.Rows.Add(dtrowLines);
                    }
                    DataView dv = new DataView();
                    dv = Cctype.DefaultView;
                    ViewBag.CC_type = dv.ToTable(true, "GlAccount", "ddl_cc_type", "dd_cc_type_id");

                    DataTable ccitem = new DataTable();
                    ccitem.Columns.Add("dd_cc_typ_id", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name_id", typeof(string));
                    ccitem.Columns.Add("cc_Amount", typeof(string));

                    JArray Arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow DtrowLines = ccitem.NewRow();
                        DtrowLines["dd_cc_typ_id"] = arr[i]["ddl_Type_Id"].ToString();
                        DtrowLines["ddl_cc_name"] = arr[i]["ddl_CC_Name"].ToString();
                        DtrowLines["ddl_cc_name_id"] = arr[i]["ddl_Name_Id"].ToString();
                        DtrowLines["cc_Amount"] = arr[i]["CC_Amount"].ToString();
                        ccitem.Rows.Add(DtrowLines);
                    }
                    ViewBag.CC_Item = ccitem;
                }
                //ViewBag.CC_Item = ds.Tables[1];

                DataSet ds1 = _Common_IServices.GetCstCntrData(CompID, BrID, "0", Flag);

                List<CostcntrType> Cctypelist = new List<CostcntrType>();

                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    CostcntrType Cc_type = new CostcntrType();
                    Cc_type.cc_id = dr["cc_id"].ToString();
                    Cc_type.cc_name = dr["cc_name"].ToString();
                    Cctypelist.Add(Cc_type);
                }
                Cctypelist.Insert(0, new CostcntrType() { cc_id = "0", cc_name = "---Select---" });
                _CC_Model.costcntrtype = Cctypelist;
                ViewBag.CCTotalAmt = ds.Tables[2].Rows[0]["total_cc_Amount"];//add by sm 11-12-2024
                ViewBag.DocId = DocumentMenuId;//add by sm 11-12-2024
                _CC_Model.disflag = "N";
                return PartialView("~/Areas/Common/Views/Cmn_PartialCostCenterDetail.cshtml", _CC_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw Ex;
            }

        }
        public string SaveInsertReAllocationCostCenterData(string CC_int_br_id, string Vou_No, string Vou_Dt, string Vou_type, string GLAcc_id,string CCDetails,CostCenterReAllocation_Model _Model)
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
                string SaveMessage = "";
                DataTable CostCenterDetails = new DataTable();
                DataTable ReAllocateCCDetails = new DataTable();
                //if (CCDetails.ToString() != null && CCDetails.ToString() != "" && CCDetails.ToString() != "[]")
                //{
                    ReAllocateCCDetails.Columns.Add("acc_id", typeof(string));
                    ReAllocateCCDetails.Columns.Add("cc_id", typeof(int));
                    ReAllocateCCDetails.Columns.Add("cc_val_id", typeof(int));
                    //CC_Details.Columns.Add("cc_amt", typeof(float));
                    ReAllocateCCDetails.Columns.Add("cc_amt", typeof(string));


                    JArray JAObj = JArray.Parse(CCDetails);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = ReAllocateCCDetails.NewRow();

                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                        ReAllocateCCDetails.Rows.Add(dtrowLines);
                    }
                    CostCenterDetails = ReAllocateCCDetails;
                    SaveMessage = _CCRA_ISERVICE.InsertUpdateCCDetails(CompID, BrID, CC_int_br_id, Vou_No, Vou_Dt, Vou_type, GLAcc_id,CostCenterDetails);
                    return SaveMessage;
                //}
                //else
                //{
                //    SaveMessage = _CCRA_ISERVICE.InsertUpdateCCDetails(CompID, BrID, Vou_No, Vou_Dt, Vou_type, GLAcc_id, CostCenterDetails);
                //    return SaveMessage;
                //}
                //return SaveMessage;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw Ex;
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
        public FileResult CCReAllocationExporttoExcelDt(string GlAcc_id, string CCTyp_id, string CC_Val_id, string AllocationTyp_id, string From_Dt,string To_Dt)
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
                DataTable dt1 = _CCRA_ISERVICE.OnSrchGetCCRAReport(CompID, BrID, GlAcc_id, CCTyp_id, CC_Val_id, AllocationTyp_id, From_Dt, To_Dt).Tables[0];
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("Voucher Number", typeof(string));
                dt.Columns.Add("Voucher Date", typeof(string));
                dt.Columns.Add("GL Account", typeof(string));
                dt.Columns.Add("Account Type", typeof(string));
                dt.Columns.Add("Amount", typeof(decimal));

                if (dt1.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["Voucher Number"] = dr["vou_no"].ToString();
                        dtrowLines["Voucher Date"] = dr["vou_dt"].ToString();
                        dtrowLines["GL Account"] = dr["acc_name"].ToString();
                        dtrowLines["Account Type"] = dr["account_type_name"].ToString();
                        dtrowLines["Amount"] = dr["gl_amt"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Cost Center Re-Allocation", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
    }

}