using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.GeneralLedger
{
    public class GeneralLedgerModel
    {
        public string GL_listByFilter { get; set; }
        public string GL_DetailData{ get; set; }
        public string GLAccount { get; set; }
        public string AccountType { get; set; }
        public string FromDate { get; set; }
        public string BrList { get; set; }
        public string ToDate { get; set; }
        public string Title { get; set; }
        public string ddlGroup { get; set; }
        public string PrintData { get; set; }
        public string curr { get; set; }
        public string curr_Name { get; set; }
        public List<curr> currList { get; set; }
        public List<GLDetail_List> GL_Detail_List { get; set; }
        public string hdnPDFPrint { get; set; }
        public string hdnCSVPrint { get; set; }
        public string searchValue { get; set; }

    }
    public class curr
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }

    }
    public class Search_model //Added by Suraj Maurya on 20-06-2025
    {
        public Search_Parmeters search_prm { get; set; }
    }
    public class Search_Parmeters
    {
        public string CompId { get; set; }
        public string BrId { get; set; }
        public string AccID { get; set; }
        public string AccGrp { get; set; }
        public string AccType { get; set; }
        public string CurrId { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string Rpt_As { get; set; }
        public string Flag { get; set; } = "";
        public string br_ids { get; set; } = "";

    }

    public class GLDetail_List
    {
        public Int64 SrNo { get; set; }
        public string acc_name { get; set; }
        public string acc_id { get; set; }
        public string src_doc_no { get; set; }
        public string src_doc_dt { get; set; }
        public string srcdocdt { get; set; }
        public string narr { get; set; }
        public string br_op_bal_bs { get; set; }
        public string br_op_bal_type { get; set; }
        public string dr_amt { get; set; }
        public string cr_amt { get; set; }
        public string br_acc_bal_bs { get; set; }
        public string br_acc_bal_type { get; set; }

    }
}
