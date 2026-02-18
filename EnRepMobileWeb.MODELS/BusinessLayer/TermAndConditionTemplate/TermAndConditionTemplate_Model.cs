using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.TermAndConditionTemplate
{
    public class TermAndConditionTemplate_Model
    {
        public string hdnSavebtn { get; set; }
        public string BtnName { get; set; }
        public string create_id { get; set; }
        public string DeleteCommand { get; set; }
        public string creat_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }
        public string Title { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string TemplateName { get; set; }
        public string TemplateId { get; set; }
        public bool TemplateStatus { get; set; }
        public string TermsAndConditions { get; set; }
        public string TermsConditions { get; set; }
        public string BranchMapping { get; set; }
        public string TermAndConditionForDuplicate { get; set; }
        public DataSet DuplicateData { get; set; }
        public DataTable CustomerBranchList { get; set; }
    }
}
