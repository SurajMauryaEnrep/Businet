using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.FactorySettings.ResetData;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net.Mail;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.FactorySettings.Controllers.ResetData
{
    public class ResetDataController : Controller
    {
        string CompID, language, title = String.Empty;
        string DocumentMenuId = "102125";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ResetData_IService _ResetData_IService;
        public ResetDataController(Common_IServices _Common_IServices, ResetData_IService _ResetData_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._ResetData_IService = _ResetData_IService;
        }
        // GET: FactorySettings/ResetData
        public ActionResult ResetData()
        {
            ViewBag.DocumentMenuId = DocumentMenuId;
            ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/FactorySettings/Views/ResetData/ResetData.cshtml");
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
        public JsonResult GetHo_list()
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _ResetData_IService.BindHeadOffice().Tables[0];
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetBr_list(string compid)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _ResetData_IService.BindBranchList(compid).Tables[0];
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult FactoryReset_data(string compid, string brid, string flag,string verfy_otp)
        {
            JsonResult DataRows = null;
            try
            {
                string result = string.Empty;
                string res_flag = "N";
                DateTime sessionTimeout = (Convert.ToDateTime(Session["FR_OtpTimeout"]));
                if (Session["FR_OTP"].ToString() != verfy_otp)
                {
                    result = "Invalid OTP";
                    res_flag = "Y";
                }
                if (sessionTimeout < DateTime.Now)
                {
                    result = "OTP expired";
                    res_flag = "Y";
                }
                if (res_flag == "N")
                {
                    result = _ResetData_IService.FactoryReset_data(compid, brid, flag);
                }
                DataRows = Json(JsonConvert.SerializeObject(result));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        private static string GenerateOtp()
        {
            Random rand = new Random();
            return rand.Next(111111, 999999).ToString();
        }
        private static string sendEmail(string subject, string body, string emailId)
        {
            // Sender's email address and credentials
            string senderEmail = "alerts@enrep.biz";
            string senderPassword = "AL@en20#*";

            // Create a MailMessage object
            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(emailId));
            mail.From = new MailAddress(senderEmail);
            mail.Subject = subject;
            mail.Body = body;
            //mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtpout.asia.secureserver.net";
            smtp.Port = 25;
            //smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
            smtp.EnableSsl = false;
            try
            {
                smtp.Send(mail);
                return "Success";
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
        }
        public string SendOtpTochangeResetData()
        {
            try
            {
                    string otp = GenerateOtp();
                    Session["FR_OTP"] = otp;
                    Session["FR_OtpTimeout"] = DateTime.Now.AddMinutes(5);
                string emailId = "vishal.varshney@enrep.biz";
                string subject = "Regarding Data Delete";
                    string mailBody = "Your OTP(one time password) is " + otp + ". \n Do not share your OTP info with anyone else. \n Thanks & Regards \n Businet";
                    return sendEmail(subject, mailBody, emailId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}