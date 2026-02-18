using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISGatePassDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISGatePassDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MIS.MISGatePassDetail
{
    public class MISGatePassDetailController : Controller
    {
        string CompID, language, BrchID, UserID = String.Empty;
        string DocumentMenuId = "105102180135", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MISGatePassDetail_IServices _MISGatePassDetail_IServices;
        public MISGatePassDetailController(Common_IServices _Common_IServices, MISGatePassDetail_IServices _MISGatePassDetail_IServices)
        {
            this._Common_IServices = _Common_IServices;
            this._MISGatePassDetail_IServices = _MISGatePassDetail_IServices;
        }
        // GET: ApplicationLayer/MISGatePassDetail
        public ActionResult MISGatePassDetail()
        {
            CommonPageDetails();
            MISGatePassDetailModel ListModel =new MISGatePassDetailModel();
            List<EntityNameList1> _EntityName = new List<EntityNameList1>();
            EntityNameList1 _EntityNameList = new EntityNameList1();
            _EntityNameList.entity_name = "All";
            _EntityNameList.entity_id = "0";
            _EntityName.Add(_EntityNameList);
            ListModel.EntityNameList = _EntityName;
            CompDataWithID();
            ListModel.Title = title;
            DateTime dtnow = DateTime.Now;

            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            ListModel.FromDate = startDate;
            ListModel.ToDate = dtnow.ToString("yyyy-MM-dd");
            DataTable dt = _MISGatePassDetail_IServices.SearchDataFilter("0", "0", "0",  ListModel.FromDate , ListModel.ToDate, "0", CompID, BrchID, DocumentMenuId);
            ViewBag.GatePassReciptList = dt;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MIS/MISGatePassDetail/MISGatePassDetail.cshtml", ListModel);
        }
        [HttpPost]
        public ActionResult DataSearch_Search(string Source_type, string Entity_type, string Entity_id, string Fromdate, string Todate, string Status)
        {
            try
            {
                MISGatePassDetailModel SearchModel = new MISGatePassDetailModel();
                SearchModel.GatePassSearch = "";
                CompDataWithID();
                       
                DataTable dt = _MISGatePassDetail_IServices.SearchDataFilter(Source_type, Entity_type, Entity_id, Fromdate, Todate, Status, CompID, BrchID, DocumentMenuId);
                SearchModel.GatePassSearch = "Search";
                ViewBag.Source_type = Source_type;
                    ViewBag.GatePassReciptList = dt;     
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMISGatePassDetail.cshtml", SearchModel);
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

                CompDataWithID();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
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
        public ActionResult GetSupp_CustList(MISGatePassDetailModel ListModel)
        {
            try
            {
                CompDataWithID();
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                List<EntityNameList1> _EntityName = new List<EntityNameList1>();
                if (string.IsNullOrEmpty(ListModel.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = ListModel.EntityName.ToString();
                }
                if (string.IsNullOrEmpty(ListModel.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = ListModel.entity_type.ToString();
                }
                DataSet SuppCustList = _MISGatePassDetail_IServices.getSuppCustList(CompID, BrchID, EntityName, EntityType);
                if (EntityType == "0")
                {
                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                else
                {
                    DataRow Drow = SuppCustList.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    //SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                ListModel.EntityNameList = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

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
    }

}