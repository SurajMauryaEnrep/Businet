using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.OpeningBalance
{
    public class OpeningBalanceModel
    {
      
            public string transtyp { get; set; }  
            public string Btn { get; set; }  
            public string OpeningBal { get; set; }  
            public string Finyear { get; set; }  
            public string AccID { get; set; }  
            public string OpBalFindt { get; set; }  
            public string OpFin_year { get; set; }  
            public string OpFindate { get; set; }  
            public string AppStatus { get; set; }  
            public string Disabled { get; set; }  
            public string BtnName { get; set; }  
            public string Message { get; set; }  
            public string Command { get; set; }  
            public string Title { get; set; }  
            public string TransType { get; set; }
            public int acc_id { get; set; }
            public string acc_name { get; set; }
            public int acc_type { get; set; }
             //public DateTime op_year  { get; set; }
             public string acc_grp_name { get; set; }
             public int acc_grp_id { get; set; }
            public string op_bal_sp { get; set; }
        public string op_bal_bs { get; set; }
        public string op_bal_type { get; set; }         
            public string mac_id { get; set; }         
            public int create_id { get; set; }
            public string createby { get; set; }          
            public int comp_id { get; set; }
            public int br_id { get; set; }
        public string fin_year { get; set; }
        public string hd_Finyear { get; set; }
        public int? curr { get; set; }
        public string conv_rate { get; set; }
        public string UserMacaddress { get; set; }
            public string UserSystemName { get; set; }
            public string UserIP { get; set; }
            public string creat_dt { get; set; }     
            public string DeleteCommand { get; set; }
        public string OPBilldetails { get; set; }
        public string Hdn_fin_year { get; set; }
        public string HdnCsvPrint { get; set; }
        public string searchValue { get; set; }//add by Shubham Maurya on 28-07-2025
        public string SalePerson { get; set; }//add by Shubham Maurya on 19-01-2026
        public List<CoaName>CoaNameList { get; set; }
        public List<OPYear> op_yearList { get; set; }
        public List<curr> currList { get; set; }
        public List<SalePersonList> SalePersonList { get; set; }
    }
    public class SalePersonList
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

    }
    public class CoaName
    {
        public string acc_id { get; set; }
        public string acc_name { get; set; }

    }
    public class OPYear
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class curr
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }

    }
}

