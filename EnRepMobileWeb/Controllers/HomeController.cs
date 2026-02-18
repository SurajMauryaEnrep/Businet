using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES;
using EnRepMobileWeb.SERVICES.SERVICES;
using System.Data;
using Newtonsoft.Json;
using System.Threading;
using System.Globalization;
using EnRepMobileWeb.MODELS.LOGIN;
using System.Net;
using EnRepMobileWeb.MODELS.DASHBOARD;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace EnRepMobileWeb.Controllers
{
    public class HomeController : Controller
    {
        DataSet MenuDs, BranchDs;
        MENU_SERVICES _MENU_SERVICES;
        LOGIN_ISERVICES _LOGIN_ISERVICES;
        private readonly Common_IServices _common;
        public HomeController(LOGIN_ISERVICES _LOGIN_ISERVICES, Common_IServices common)
        {
            this._LOGIN_ISERVICES = _LOGIN_ISERVICES;
            this._common = common;
        }
        // GET: Home
        public ActionResult Index()
        {
            LOGIN_MODEL _LOGIN_MODEL = new LOGIN_MODEL();
            ClassLanguage _Language = new ClassLanguage();
            if (Session["validateMessage"] != null)
            {
                string dtl = Session["UserNameAndPassword"].ToString();
                Session["UserNameAndPassword"] = null;
                _LOGIN_MODEL.UserName = dtl.Substring(0, dtl.IndexOf(" "));
                _LOGIN_MODEL.UserPassword = dtl.Substring(dtl.IndexOf(' ') + 1);
                string str = Session["validateMessage"].ToString();
                Session["validateMessage"] = null;
                if (str == "Invailid User")
                {
                    _LOGIN_MODEL.UserMessage = str;
                    _LOGIN_MODEL.ClassForUser = "field-validation-error";
                    _LOGIN_MODEL.UserBorder = "border-red";
                }
                if (str == "Invailid Password")
                {
                    _LOGIN_MODEL.PassMessage = str;
                    _LOGIN_MODEL.ClassForPass = "field-validation-error";
                    _LOGIN_MODEL.UserPassword = "border-red";
                }
            }
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                _LOGIN_MODEL.UserName = Request.Cookies["UserName"].Value;
                _LOGIN_MODEL.UserPassword = Request.Cookies["Password"].Value;
            }
            return View("Index", _LOGIN_MODEL);
        }
        public JsonResult ValidUser(string userName)
        {
            JsonResult DataRows = null;
            try
            {
                DataSet dsUsers = _LOGIN_ISERVICES.GetValidUser(userName, null,null);
                if (dsUsers.Tables.Contains("Table"))
                {
                    if (dsUsers.Tables[0].Rows.Count > 0)
                    {
                        DataRows = Json(JsonConvert.SerializeObject(dsUsers));
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DataRows;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212012";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        [NoDirectAccess]
        public JsonResult GetBranchList(string userName, string passward, string Comp_id)
        {
            try
            {
                List<ClassBranchName> _BranchList = new List<ClassBranchName>();
                LOGIN_MODEL _LOGIN_MODEL = new LOGIN_MODEL();
                JsonResult DataRows = null;
                DataSet ds = _LOGIN_ISERVICES.GetValidUser(userName, Comp_id,null);
                if (ds.Tables.Contains("Table"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                       // string UserName = ds.Tables[0].Rows[0][1].ToString();
                        string UserName = ds.Tables[0].Rows[0]["login_id"].ToString();
                        string pass = ds.Tables[0].Rows[0][2].ToString();
                        pass = Decrypt(pass);
                        if (UserName.ToLower() == userName.ToLower() && pass == passward)
                        {
                            DataRows = Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                        }
                    }


                }
                return DataRows;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [NoDirectAccess]
        public ActionResult UserPassValidate(string userName, string passward)
        {

            List<ClassCompanyName> _CompanyList = new List<ClassCompanyName>();
            List<ClassLanguage> _LanguageList = new List<ClassLanguage>();
            List<ClassBranchName> _BranchList = new List<ClassBranchName>();

            LOGIN_MODEL _LOGIN_MODEL = new LOGIN_MODEL();

            try
            {
                DataSet ds = _LOGIN_ISERVICES.GetValidUser(userName, null,null);
                if (ds.Tables.Contains("Table"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                       // string UserName = ds.Tables[0].Rows[0][1].ToString();
                        string UserName = ds.Tables[0].Rows[0]["login_id"].ToString();
                        string pass = ds.Tables[0].Rows[0][2].ToString();
                        pass = Decrypt(pass);
                        if (UserName.ToLower() == userName.ToLower() && pass == passward)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ClassCompanyName _companyName = new ClassCompanyName();
                                _companyName.Comp_name = dr["comp_nm"].ToString();
                                _companyName.Comp_id = dr["Comp_id"].ToString();
                                _CompanyList.Add(_companyName);
                            }
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                ClassLanguage _Language = new ClassLanguage();
                                _Language.lang_id = dr["lang_id"].ToString();
                                _Language.lang_name = dr["lang_name"].ToString();
                                _LanguageList.Add(_Language);
                            }
                            if (ds.Tables[4].Rows[0]["br_id"].ToString() == "-1")
                            {
                                _BranchList.Add(new ClassBranchName { Br_id = "-1", Br_name = " Choose Branch" });
                            }

                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {
                                ClassBranchName _branchName = new ClassBranchName();
                                _branchName.Br_name = dr["comp_nm"].ToString();
                                _branchName.Br_id = dr["Comp_id"].ToString();
                                _BranchList.Add(_branchName);
                            }

                            _LOGIN_MODEL.CompanyNameList = _CompanyList;
                            _LOGIN_MODEL.LanguageList = _LanguageList;
                            _LOGIN_MODEL.BranchNameList = _BranchList;
                            _LOGIN_MODEL.BranchId = ds.Tables[4].Rows[0]["br_id"].ToString();
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("~/Views/Shared/Home/_Indexlanguage.cshtml", _LOGIN_MODEL);


        }
        [HttpPost]
        public ActionResult demo()
        {
            return View();
        }
        [NoDirectAccess]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(LOGIN_MODEL objLOGIN_MODEL)
        {
            if (ModelState.IsValid)
            {
                if (objLOGIN_MODEL.Language != null)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(objLOGIN_MODEL.Language);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(objLOGIN_MODEL.Language);
                }
                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = objLOGIN_MODEL.Language;
                Response.Cookies.Add(cookie);

                if (objLOGIN_MODEL.RememberMe == true)
                {
                    HttpCookie rmUserName = new HttpCookie("UserName");
                    rmUserName.Value = objLOGIN_MODEL.UserName;
                    Response.Cookies.Add(rmUserName);
                    HttpCookie rmPassword = new HttpCookie("Password");
                    rmPassword.Value = objLOGIN_MODEL.UserPassword;
                    Response.Cookies.Add(rmPassword);
                }
                try
                {
                    DataSet ds = _LOGIN_ISERVICES.GetValidUser(objLOGIN_MODEL.UserName, objLOGIN_MODEL.CompanyName, objLOGIN_MODEL.BranchId);
                    //if (ds.Tables.Contains("Table"))
                    //{
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string UserName = ds.Tables[0].Rows[0][1].ToString();
                        string Login_id = ds.Tables[0].Rows[0]["login_id"].ToString();
                        string pass = ds.Tables[0].Rows[0][2].ToString();
                        pass = Decrypt(pass);
                        if (Login_id.ToLower() == objLOGIN_MODEL.UserName.ToLower() && pass == objLOGIN_MODEL.UserPassword)
                        {
                            Session["UserId"] = ds.Tables[0].Rows[0][0].ToString();
                            Session["UserName"] = UserName.Trim();
                            Session["CompId"] = objLOGIN_MODEL.CompanyName.Trim();
                            Session["BranchId"] = objLOGIN_MODEL.BranchId;
                            Session["BranchName"] = objLOGIN_MODEL.BranchName;
                            Session["CompanyName"] = ds.Tables[0].Rows[0][3].ToString();
                            Session["Language"] = objLOGIN_MODEL.Language;
                            Session["UserIP"] = GetIPAddress();
                            Session["UserMacaddress"] = GetClientMAC(GetIPAddress());
                            //Session["UserSystemNameOld"] = Environment.MachineName;
                            //Session["UserIPOld"] = GetLocalIPAddress();
                            Session["QtyDigit"] = ds.Tables[0].Rows[0][5];
                            Session["RateDigit"] = ds.Tables[0].Rows[0][6];
                            Session["ValDigit"] = ds.Tables[0].Rows[0][7];
                            Session["WeightDigit"] = ds.Tables[0].Rows[0][9];//Added By Nitesh 24-11-2023 for Weight Qty
                            Session["ExchDigit"] = ds.Tables[0].Rows[0][10];//Added By Shubham Maurya 06-02-2024 for Exchange Qty
                            Session["ExpImpQtyDigit"] = ds.Tables[0].Rows[0][11];//Added By Shubham Maurya 06-02-2024 for Exchange Qty
                            Session["ExpImpRateDigit"] = ds.Tables[0].Rows[0][12];//Added By Shubham Maurya 06-02-2024 for Exchange Qty
                            Session["ExpImpValDigit"] = ds.Tables[0].Rows[0][13];//Added By Shubham Maurya 06-02-2024 for Exchange Qty
                            Session["crm"] = ds.Tables[0].Rows[0]["crm"];
                            Session["rpt_id"] = ds.Tables[0].Rows[0]["rpt_id"];
                            Session["user_nm"] = ds.Tables[0].Rows[0]["user_nm"];

                            Session["PurCostingRateDigit"] = ds.Tables[0].Rows[0]["pur_costing_rate_digit"];//Added By Shubham Maurya 06-02-2024 for Exchange Qty
                            Session["RegTo"] = ds.Tables[2].Rows[0][0];
                            Session["RegNo"] = ds.Tables[2].Rows[0][1];
                            Session["curr_format"] = ds.Tables[0].Rows[0]["curr_format"];//Updated by Suraj Maurya on 23-12-2024
                            Session["curr_format"] = ds.Tables[5].Rows[0]["curr_format"];//Updated by SHubham Maurya on 13-12-2024
                            Session["UserSystemName"] = Systemname();
                            Session["Available"] = ds.Tables[0].Rows[0][8].ToString();
                            Session.Remove("DBFilterType");
                            Session.Remove("DBToDt");
                            Session.Remove("DBFromDt");

                            return RedirectToAction("Index", "DashboardHome", new { area = "Dashboard" });
                        }
                        else//comp_nm
                        {
                            Session["validateMessage"] = "Invailid Password";
                            Session["UserNameAndPassword"] = objLOGIN_MODEL.UserName + " " + objLOGIN_MODEL.UserPassword;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        Session["validateMessage"] = "Invailid User";
                        Session["UserNameAndPassword"] = objLOGIN_MODEL.UserName + " " + objLOGIN_MODEL.UserPassword;
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [NoDirectAccess]
        public ActionResult ComingSoon()
        {

            DASHBOARD_MODEL _DASHBOARD_MODEL = new DASHBOARD_MODEL();
            List<Branch> _BranchList = new List<Branch>();
            List<String> Menu = new List<string>();
            MenuDs = new DataSet();
            BranchDs = new DataSet();
            string Comp_ID = string.Empty;
            string Language = string.Empty;
            string userid = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                Language = Session["Language"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            _MENU_SERVICES = new MENU_SERVICES();
            MenuDs = _MENU_SERVICES.GetAllMenuDAL(Comp_ID, Language, userid);
            ViewBag.Menu1 = MenuDs.Tables[0];
            ViewBag.Menu2 = MenuDs.Tables[1];
            ViewBag.Menu3 = MenuDs.Tables[2];
            ViewBag.Menu4 = MenuDs.Tables[3];



            string brid = string.Empty;
            if (Session["BranchId"] != null)
            {
                brid = Session["BranchId"].ToString();
            }
            BranchDs = _MENU_SERVICES.GetAllTopNavBrchList(Comp_ID, userid, brid);
            foreach (DataRow dr in BranchDs.Tables[0].Rows)
            {
                Branch _Branch = new Branch();
                _Branch.comp_nm = dr["comp_nm"].ToString();
                _Branch.Comp_Id = dr["Comp_id"].ToString();

                _BranchList.Add(_Branch);

            }
            _DASHBOARD_MODEL.BranchList = _BranchList;
            return View("ComingSoon", _DASHBOARD_MODEL);
        }
        private static string GenerateOtp()
        {
            Random rand = new Random();
            return rand.Next(111111, 999999).ToString();
        }
        public string SendOtpTochangePassword(string act, string userName, string password)
        {
            try
            {
                if (ValidatePasswordToChange(userName, password).ToUpper() != "VALID" && act == "changepass")
                    return "Invalid old password";
                DataSet dsUsers = _LOGIN_ISERVICES.ValidateUserToForgetPassword(userName);
                if (dsUsers.Tables.Count > 0)
                {
                    string otp = GenerateOtp();
                    Session["OTP"] = otp;
                    Session["OtpTimeout"] = DateTime.Now.AddMinutes(2);
                    string emailId = dsUsers.Tables[0].Rows[0]["user_email"].ToString();
                    string subject = "Change Password";
                    string mailBody = "Your OTP(one time password) to change your businet password is " + otp + ". \n Do not share your password and account info with anyone else. \n Thanks & Regards \n Businet";
                    return sendEmail(subject, mailBody, emailId);
                }
                else
                {
                    return "Something went wrong..";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ValidatePasswordToChange(string userName, string password)
        {
            DataSet ds = _LOGIN_ISERVICES.GetValidUser(userName, null,null);
            if (ds.Tables.Contains("Table"))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string pswrd = ds.Tables[0].Rows[0][2].ToString();
                    pswrd = Decrypt(pswrd);
                    if (password == pswrd)
                    {
                        return "Valid";
                    }
                    else
                    {
                        return "Invalid Old Password";
                    }
                }
                else
                {
                    return "Invalid old password";
                }
            }
            else
            {
                return "Invalid old password";
            }
        }
        private static string SendSms(string mobileNo, string otp)
        {
            return "Success";
        }
        public string Systemname()
        {
            try
            {

                string[] PCName = Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName.Split(new Char[] { '.' });
                return PCName[0].ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }



        }
        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);
        private static string GetClientMAC(string strClientIP)
        {
            string mac_dest = "";
            mac_dest = GetClientMacAddress();
            return mac_dest;
            //try
            //{
            //    Int32 ldest = inet_addr(strClientIP);
            //    Int32 lhost = inet_addr("");
            //    Int64 macinfo = new Int64();
            //    Int32 len = 6;
            //    int res = SendARP(ldest, 0, ref macinfo, ref len);
            //    string mac_src = macinfo.ToString("X");

            //    while (mac_src.Length < 12)
            //    {
            //        mac_src = mac_src.Insert(0, "0");
            //    }

            //    for (int i = 0; i < 11; i++)
            //    {
            //        if (0 == (i % 2))
            //        {
            //            if (i == 10)
            //            {
            //                mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
            //            }
            //            else
            //            {
            //                mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw new Exception("L?i " + err.Message);
            //}
            //return mac_dest;
        }
        public string erifyOtp(string userName, string password, string newPassword, string otp)
        {
            return "";
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
        public string ChangePassword(string act, string userName, string oldPassword, string newPassword, string otp)
        {
            try
            {
                DateTime sessionTimeout = (Convert.ToDateTime(Session["OtpTimeout"]));
                if (Session["OTP"].ToString() != otp)
                    return "Invalid OTP";
                if (sessionTimeout < DateTime.Now)
                    return "OTP expired";
                if (ValidatePasswordToChange(userName, oldPassword).ToUpper() != "VALID" && act == "changepass")
                    return "Invalid old password";
                string oldPass = Encrypt(oldPassword);
                string newPass = Encrypt(newPassword);
                int result = _LOGIN_ISERVICES.ChangePassword(userName, newPass);
                if (result > 0)
                    return "Password changed successfully";
                else
                    return "Something went wrong";
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
        }
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212012";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string GetClientMacAddress()
        {
            string clientMac = "";
            // Get all network interfaces on the computer
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Loop through each network interface
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // Check if the network interface is operational and not a loopback or tunnel adapter
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    // Get the physical address (MAC address) of the network interface
                    PhysicalAddress macAddress = networkInterface.GetPhysicalAddress();

                    // Convert the MAC address to a string in hexadecimal format
                    string macAddressString = BitConverter.ToString(macAddress.GetAddressBytes());

                    // Display the MAC address
                    clientMac = macAddressString;
                }
            }
            return clientMac;
        }
    }
}