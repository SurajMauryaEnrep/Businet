using System;
using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.AccountPayable
{
    public class AccountPayableModel
    {
        public string SuppName { get; set; }
        public string Title { get; set; }
        public string supp_id { get; set; }
        public string Hdnsupp_id { get; set; }
        public string Basis { get; set; }
        public string To_dt { get; set; }
        public string Range1 { get; set; }
        public string Range2 { get; set; }
        public string Range3 { get; set; }
        public string Range4 { get; set; }
        public string Range5 { get; set; }
        public string APAging { get; set; }
        public string PrintPDFData { get; set; }
        public string RangeList { get; set; }//Added by Suraj
        public string CmdPDFPrint { get; set; }
        //public string DocCode_InvDtlPopupPDF { get; set; }
        public string hdnBasis { get; set; }
        public string hdnAsonDate { get; set; }
        public List<SuppCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<SuppPortFolioList> portFolioLists { get; set; }
        public string portFolio { get; set; }
        public List<APList> AccountPayableList { get; set; }
        public List<InvoiceList> InvoiceList { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public string searchValue { get; set; }
        public string hdnCSVPrint { get; set; }
        public string hdnInsightCSVPrint { get; set; }
        public string hdnPaidAmtInsightCSVPrint { get; set; }
        public string hdnAdvAmtInsightCSVPrint { get; set; }
        public string PayableType { get; set; }
        public string PayablePdf { get; set; }
        public string ReportType { get; set; }
        public string hdnbr_ids { get; set; }
        public string Hdncatg_id { get; set; }
        public string Hdnport_id { get; set; }
    }
    //public class SupplierName
    //{
    //    public string supp_id { get; set; }
    //    public string supp_name { get; set; }
    //}
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class SuppCategoryList
    {
        public string Cat_id { get; set; }
        public string Cat_val { get; set; }

    }
    public class SuppPortFolioList
    {
        public string Portfolio_id { get; set; }
        public string Portfolio_val { get; set; }

    }
    public class APList
    {
        public Int64 SrNo { get; set; }
        public string SuppName { get; set; }
        public string inv_no { get; set; }
        public string inv_dt { get; set; }
        public string bill_no { get; set; }
        public string bill_dt { get; set; }
        public string inv_amt { get; set; }
        public string SuppId { get; set; }
        public string AccId { get; set; }
        public string Curr { get; set; }
        public string CurrId { get; set; }
        public string AmtRange1 { get; set; }
        public string AmtRange2 { get; set; }
        public string AmtRange3 { get; set; }
        public string AmtRange4 { get; set; }
        public string AmtRange5 { get; set; }
        public string AmtRange6 { get; set; }
        public string totamt_sp { get; set; }
        public string totamt_bs { get; set; }
        public string AdvanceAmount { get; set; }
        public string advamt_bs { get; set; }
        public string TotalAmt { get; set; }
        public string totnetamt_bs { get; set; }

    }

    public class InvoiceList
    {
        public string due_Date { get; set; }
        public string due_days { get; set; }
        public string Bill_No { get; set; }
        public string Bill_Dt { get; set; }
        public string Invoice_No { get; set; }
        public string Invoice_Dt { get; set; }
        public string Invoice_Date { get; set; }
        public string Invoice_Amt { get; set; }
        public string Paid_Amt { get; set; }
        public string Balance_Amt { get; set; }
        public string Total_Invoice_Amt { get; set; }
        public string Total_Paid_Amt { get; set; }
        public string Total_Balance_Amt { get; set; }
        public string Payment_Terms { get; set; }/*Add by Hina sharma on 25-12-2024 for invoice detail popup*/
        public string DocCode { get; set; }

    }
}
