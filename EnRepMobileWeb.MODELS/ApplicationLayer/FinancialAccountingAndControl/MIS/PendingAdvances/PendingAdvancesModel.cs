using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.AccountReceivable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.PendingAdvances
{
    public class PendingAdvancesModel
    {
        public string PAAging { get; set; }
        public string Title { get; set; }
        public string Range1 { get; set; }
        public string Range2 { get; set; }
        public string Range3 { get; set; }
        public string Range4 { get; set; }
        public string Range5 { get; set; }
        public string Range6 { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string AppStatus { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string To_dt { get; set; }
        public string EntityType { get; set; }
        public string ReportType { get; set; }
        public List<PRList> PendingAdvancesList { get; set; }
        public List<PendingAdvList> PendingAdvList { get; set; }
        public string hdnPDFPrint { get; set; }
        public string hdnCSVPrint { get; set; }
        public string HdnEntityType { get; set; }
    }
    public class PRList
    {
        public Int64 SrNo { get; set; }
        public string Entityname { get; set; }
        public string id { get; set; }
        public string AccId { get; set; }
        public string Curr { get; set; }
        public string CurrId { get; set; }
        public string voucher_no { get; set; }
        public string voucher_dt { get; set; }
        public string AmtRange1 { get; set; }
        public string AmtRange2 { get; set; }
        public string AmtRange3 { get; set; }
        public string AmtRange4 { get; set; }
        public string AmtRange5 { get; set; }
        public string AmtRange6 { get; set; }
        public string TotalAmt { get; set; }

    }
    public class PendingAdvList
    {
        public string vou_date { get; set; }
        public string vou_no { get; set; }
        public string vou_type { get; set; }
        public string pend_amt { get; set; }
        public string Totalpendamt { get; set; }
        
    }
}
