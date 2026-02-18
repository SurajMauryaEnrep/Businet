namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GSTAPI
{
    public class CancelInvIrn
    {
        public string supplyType { get; set; }
        public string transactionType { get; set; }
        public string invoiceDate { get; set; }
        public string invoiceNumber { get; set; }
        public string reasonforIRNCancel { get; set; }
        public string reasonforEWBCancel { get; set; }
        public string ewbRemarks { get; set; }
        public string irnRemarks { get; set; }

    }
}
