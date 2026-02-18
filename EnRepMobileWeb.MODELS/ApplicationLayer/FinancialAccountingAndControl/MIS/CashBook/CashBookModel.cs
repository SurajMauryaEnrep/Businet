using System;
using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.CashBook
{
    public class CashBookModel
    {
        public string Title { get; set; }
        public string AccName { get; set; }
        public string AccId { get; set; }//Added by Suraj Maurya on 20-06-2025
        public string curr { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Opening_Bal { get; set; }
        public string lbl_cb_opbaltype { get; set; }
        public string lbl_cb_clbaltype { get; set; }
        public string Opening_BalType { get; set; }
        public string Closing_Bal { get; set; }
        public string Closing_BalType { get; set; }
        public string PrintData { get; set; }
        public List<CashBookList> CashBook_List { get; set; }
        public List<curr> currList { get; set; }
        public string searchValue { get; set; } = "";
        public string hdnPDFPrint { get; set; }
        public string hdnCSVPrint { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
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
        public string CurrId { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string Flag { get; set; } = "";

    }
    public class CashBookList
    {
        public Int64 SrNo { get; set; }
        public string voudt { get; set; }
        public string acc_name { get; set; }
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
        public string narr { get; set; }
        public string dr_amt { get; set; }
        public string cr_amt { get; set; }
        public string cloasing_amt { get; set; }
        public string closing_type { get; set; }

    }
}
