using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EnRepMobileWeb.MODELS.BusinessLayer.TaxDetail
{
   public class TaxDetailModel
    {
        //public Boolean act_statusY { get; set; }
        public string hdnSavebtn { get; set; }
        public int TaxCode { get; set; }
        public string AppStatus { get; set; }
        public string TaxSearch { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string TransType { get; set; }
        public string Title { get; set; }
        public static string ValueRequired { get; set; }
        public Boolean ApprovedFlag { get; set; }
        public string Command { get; set; }
        public int tax_id { get; set; }
        public string tax_name { get; set; }
        public string SectionCode { get; set; }
        
        public int acc_id { get; set; }
        public string ddl_acc_id { get; set; }

        public int tax_auth_id { get; set; }
        public string ddl_tax_auth_id { get; set; }

        public string tax_perc { get; set; }
            public Boolean recov { get; set; }
      
        public string app_date { get; set; }
            public Boolean act_status { get; set; }
            public Boolean manual_calc { get; set; }
            public string mac_id { get; set; }
            public string tax_type { get; set; }
        public int create_id { get; set; }
        public string createby { get; set; }
        public string ListFilterData1 { get; set; }
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
        public string mod_dt { get; set; }
        public string tax_act_stat { get; set; }
        public string TaxListDDL { get; set; }
        public string ListFilterData { get; set; }
        public DataTable CustomerBranchList { get; set; }
        //***Modifyed by shubham maurya on 12-12-2022 12:10 commend CustomerBranchList this code and change DataTable Typ***//

        //public List<TaxBranch> CustomerBranchList { get; set; }
        public string TaxBranchDetail { get; set; }

        public List<TaxCOA> COAList { get; set; }

        public List<AuthorityCOA> COAAuthorityList { get; set; }
        public int acc_grp_id { get; set; }
        public List<AccountGroup> AccountGroupList { get; set; }
        public List<TaxName> TaxList { get; set; }
        public string Delete { get; set; }
    }
    public class TaxName
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class AccountGroup
    {
        public string acc_grp_id { get; set; }
        public string AccGroupChildNood { get; set; }
    }
    //public class TaxBranch
    //{
    //    public string comp_id { get; set; }
    //    public string comp_nm { get; set; }
    //}

    public class TaxCOA
    {
        public string acc_id { get; set; }
        public string acc_name { get; set; }

    }
    public class AuthorityCOA
    {
        public string acc_id { get; set; }
        public string acc_name { get; set; }
    }
}
