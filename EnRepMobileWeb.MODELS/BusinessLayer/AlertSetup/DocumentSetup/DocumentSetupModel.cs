using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.DocumentSetup
{
    public class DocumentSetupModel
    {
        public string hdnSavebtn { get; set; }
        public string rowId { get; set; }
        public string transType { get; set; }
        public string activeTab { get; set; }
        public string title { get; set; }
        public string alertType { get; set; }
        public string userType { get; set; }
        public List<DocumentListModel> DocList { get; set; }
        public List<DocumentListModel> EmailDocList { get; set; }
        public List<DocumentListModel> SmsDocList { get; set; }
        public List<DocumentListModel> WhatsappDocList { get; set; }
        public List<DocumentListModel> DashboardDocList { get; set; }
        public List<DocEventListModel> DocEventList { get; set; }
        public string docId { get; set; }
        public string eventId { get; set; }
        public string Doctype { get; set; }
        public string User { get; set; }
        public DocumentSetupCommonData mailCommonData { get; set; }
        public DocumentSetupCommonData smsCommonData { get; set; }
        public DocumentSetupCommonData whatsappCommonData { get; set; }
        public DocumentSetupCommonData dashboardCommonData { get; set; }
        public string msg { get; set; }
        public string DeleteCommand { get; set; }
    
    }
    public class DocumentSetupCommonData
    {
        public string docId { get; set; }
        public string eventId { get; set; }
        public string Doctype { get; set; }
        public string User { get; set; }
    }
    public class UserType
    {
        public int value { get; set; }
        public string text { get; set; }
    }
}
