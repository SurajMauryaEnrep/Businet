using EnRepMobileWeb.Hubs;
using EnRepMobileWeb.SERVICES.ISERVICES.PushNotifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.Dashboard.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Dashboard/Notification -- Latest Backup 3/6/23
        private readonly Notification_IService _notification;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        int compId, branchId, userId;
        public NotificationController(Notification_IService notification)
        {
            _notification = notification;
        }
        // GET: Notifications/Notifications
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllUnreadNotifications()
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                    int.TryParse(Session["CompId"].ToString(), out compId);
                if (Session["userid"] != null)
                    int.TryParse(Session["userid"].ToString(), out userId);
                if (Session["BranchId"] != null)
                    int.TryParse(Session["BranchId"].ToString(), out branchId);
                string lang = "en";
                if (Session["Language"] != null)
                    lang = Session["Language"].ToString();
                //using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SignalrCon"].ConnectionString))
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EnRep"].ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    //implementing sqlcommand in controller because we can't access (dependency_OnChange > NotificationHub) class inside service layer due to circular dependancy of onion architecture
                    SqlCommand cmd = new SqlCommand("SP_GetUnreadNotification", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompId", compId);
                    cmd.Parameters.AddWithValue("@BrId", branchId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@LanFlag", lang);
                    cmd.Notification = null;
                    //SqlDependency dependency = new SqlDependency(cmd);
                    //dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    // _notification.GetAllUnreadNotifications(compId, branchId, userId);
                    dt.Load(dr);
                    DataRows = Json(JsonConvert.SerializeObject(dt),JsonRequestBehavior.AllowGet);
                    return DataRows;
                }
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return View("~/Views/Shared/Error.cshtml");
                //throw exc;
            }
        }
        [HttpPost]
        public ActionResult UpdateReadStatus(string id)
        {
            //_notification.GetAllUnreadNotifications()
            //return true;
            try
            {
                int result = _notification.UpdateReadStatus(id);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return RedirectToAction("GetAllUnreadNotifications");
        }
        public void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            NotificationHub.show();
        }
    }
}