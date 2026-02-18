using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.TaxTemplate
{
    public class TaxTemplate_Model
    {
        public string hdnSavebtn { get; set; }
        public string Title { get; set; }
        public string TemplateName { get; set; }
        public int TemplateId { get; set; }
        public bool TemplateStatus { get; set; }
        public string BaseAmount { get; set; }
        public string HSN_Number { get; set; }
        public string TaxDetails { get; set; }
        public string TaxDetailsForDuplicate { get; set; }
        public string BranchMapping { get; set; }
        public string ModuleMapping { get; set; }
        public string HSNDetail { get; set; }
        public string create_id { get; set; }
        public string creat_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public DataTable CustomerBranchList { get; set; }
        public DataTable TaxTemplateList { get; set; }
        public DataTable ModuleList { get; set; }
        public DataTable TaxDetailList { get; set; }
        public DataTable HSNList { get; set; }
        public DataSet DuplicateData { get; set; }
        public string TransType { get; set; }
        public string TTBtnName { get; set; }
        public string TTMessage { get; set; }
        public string DeleteCommand { get; set; }
        public string tax_type { get; set; }
        public List<HSNNumbarList> hSNNumbars { get; set; }
    }
    public class HSNNumbarList
    {
        public string HSNNumberId { get; set; }
        public string HSNNumber { get; set; }
    }
}
