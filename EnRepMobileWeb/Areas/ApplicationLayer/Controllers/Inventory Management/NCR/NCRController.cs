using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.NCR;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.NCR;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.NCR
{
    public class NCRController : Controller
    {

        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105102122", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        readonly Common_IServices _Common_IServices;
        readonly NCR_ISERVICES nCR_ISERVICES;
        public NCRController(Common_IServices _Common_IServices, NCR_ISERVICES nCR_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this.nCR_ISERVICES = nCR_ISERVICES;
        }
        // GET: ApplicationLayer/NCR
        public ActionResult NCR()
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
                NCR_Model nCR_Model = new NCR_Model();

                //List<StatusList> statusLists = new List<StatusList>();
                //statusLists.Add(new StatusList { id = "0", text = "All" });
                List<SrcDocList> srcDocLists = new List<SrcDocList>();
                srcDocLists.Add(new SrcDocList { SrcDocId = "0", SrcDocVal = "All" });


                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new StatusList { id = x.status_id, text = x.status_name });

                nCR_Model.statusList = listOfStatus;


                string FromDt, ToDt, SrcDocNo, Status;
                //DateTime dtnow = DateTime.Now;
                //FromDt = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                //ToDt = dtnow.ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                 FromDt = range.FromDate;
                 ToDt = range.ToDate;

                SrcDocNo = "0";
                Status = "P";

                nCR_Model.FromDt = FromDt;
                nCR_Model.Todt = ToDt;
                nCR_Model.SrcDoc = SrcDocNo;
                nCR_Model.Status = Status;
                DataSet ds = nCR_ISERVICES.GetNcrDetails(CompID, BrchID, FromDt, ToDt, SrcDocNo, Status);

                srcDocLists.Add(new SrcDocList { SrcDocId = "PUR", SrcDocVal = "Purchase QC" });
                srcDocLists.Add(new SrcDocList { SrcDocId = "RQC", SrcDocVal = "Ad-Hoc QC" });

                nCR_Model.srcDocLists = srcDocLists;
                ViewBag.NcrDetails = ds.Tables[0];
                ViewBag.MenuPageName = getDocumentName();
                ViewBag.VBRoleList = GetRoleList();
                nCR_Model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/NCR/NCRDetail.cshtml", nCR_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult SerchNCR(string FromDt, string ToDt, string SrcDocNo, string Status)
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
                DataSet ds = nCR_ISERVICES.GetNcrDetails(CompID, BrchID, FromDt, ToDt, SrcDocNo, Status);
                ViewBag.NcrDetails = ds.Tables[0];
                ViewBag.NCRSearch = "NCRSearch";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialNCRDetails.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

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
        public string SaveNCRAck(AckListDataModel AckListData)
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
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string result = nCR_ISERVICES.SaveNcrAckDetails(CompID, BrchID, AckListData);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }
        public ActionResult Acknowledgement(string item_id, string uom_id, string src_type, string doc_no, string doc_dt, string entity_id)
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
                DataSet ds = nCR_ISERVICES.GetNcrDetailonAcknowledge(CompID, BrchID, item_id, uom_id, src_type, doc_no, doc_dt, entity_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ack_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                    ViewBag.ack_by = ds.Tables[0].Rows[0]["ack_by"].ToString();
                    ViewBag.ack_dt = ds.Tables[0].Rows[0]["ack_dt"].ToString();
                    ViewBag.act_taken = ds.Tables[0].Rows[0]["act_taken"].ToString();
                    ViewBag.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                }
                ViewBag.VBRoleList = GetRoleList();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialNCRAcknowledgement.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
    }
}