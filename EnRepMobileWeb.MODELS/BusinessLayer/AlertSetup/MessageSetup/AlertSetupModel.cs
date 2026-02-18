using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetup
{
    public class AlertSetupModel
    {
        public string hdnSavebtn { get; set; }
        public string alertType { get; set; }
        public string action { get; set; }
        public int compId { get; set; }
        public int brId { get; set; }
        public string docId { get; set; }
        public string events { get; set; }
        public string msg { get; set; }
        public string msgSubject { get; set; }
        public string msgHeader { get; set; }
        public string msgBody { get; set; }
        public string msgFooter { get; set; }
        public string msgAttachment { get; set; }
        public string crtOrModId { get; set; }
        public string CreateDate { get; set; }
        public string createBy { get; set; }
        public string amendedon { get; set; }
        public string amendedBy { get; set; }
        public string SearchStatus { get; set; }

        public List<DocumentListModel> DocList { get; set; }
        public List<DocEventListModel> DocEventList { get; set; }
        public string title { get; set; }
        public string asData { get; set; }
        public string transactionType { get; set; }
        public string message { get; set; }
        public string deleteCommand { get; set; }
        public string command { get; set; }
        public string appStatus { get; set; }
        public string btnName { get; set; }

    }
    public class DocumentListModel
    {
        public string docId { get; set; }
        public string docName { get; set; }
    }
    public class DocEventListModel
    {
        public string statusCode { get; set; }
        public string statusName { get; set; }
    }
}
