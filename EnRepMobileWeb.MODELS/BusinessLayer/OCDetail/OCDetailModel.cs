using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EnRepMobileWeb.MODELS.BusinessLayer.OCDetail
{
   public class OCDetailModel
    {
        public string EnableforhdnSavebtn { get; set; }
        public string hdnSavebtn { get; set; }
        public string hdnoc_id { get; set; }
        public string ddl_acc_id { get; set; }
        public string OCType { get; set; }
        public DataTable CustomerBranchList { get; set; }
        public string OCCode { get; set; }
        public string AppStatus { get; set; }
        public string OCSearch { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public static string ValueRequired { get; set; }
        public Boolean ApprovedFlag { get; set; }
        public string TransType { get; set; }
        public int oc_id { get; set; }
        public string oc_name { get; set; }
        public int acc_id { get; set; }
        public string oc_type { get; set; }
        public string ListFilterData { get; set; }
        public string ListFilterData1 { get; set; }
        public string OTC { get; set; }
        public string tp_id { get; set; }
        public string app_date { get; set; }
        public Boolean act_status { get; set; }
        public Boolean TaxApplicable { get; set; }
        public string HdnTaxApplicable { get; set; }
        public string HSN_code { get; set; }
        public List<HSNno> HSNList { get; set; }
        public string TaxTemplate { get; set; }
        public string mac_id { get; set; }
        public int create_id { get; set; }
        public string createby { get; set; }
        public int mod_id { get; set; }
        public string modby { get; set; }
        public int comp_id { get; set; }


        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CompId { get; set; }

        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }

        public string UserIP { get; set; }
        public string creat_dt { get; set; }
        public string item_ActStatus { get; set; }
        public string OTCType { get; set; }
        public string mod_dt { get; set; }

        //public List<OCBranch> CustomerBranchList { get; set; }
        public List<TaxTemplateList> templateLists { get; set; }
        public List<OTCharge> OTCLists { get; set; }
        public string OCBranchDetail { get; set; }

        public List<OCCOA> COAList { get; set; }

        public List<ThirdPartyCOA> ThirdPartyCOAList { get; set; }
    }

    //public class OCBranch
    //{
    //    public string comp_id { get; set; }
    //    public string comp_nm { get; set; }
    //}
    public class HSNno
    {
        public string setup_val { get; set; }
        public string setup_id { get; set; }
    }
    public class TaxTemplateList
    {
        public string tmplt_id { get; set; }
        public string tmplt_nm { get; set; }
    } 
    public class OTCharge
    {
        public string OT_id { get; set; }
        public string OT_Val { get; set; }
    }

    public class OCCOA
    {
        public string acc_id { get; set; }
        public string acc_name { get; set; }

    }
    public class ThirdPartyCOA
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }

    }
}
