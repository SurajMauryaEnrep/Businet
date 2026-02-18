using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentAdvice
{
   public class PaymentAdviceModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string BalanceByFilter { get; set; }
        public string BalanceBy { get; set; }
        public string AdviceDate { get; set; }
        public string AdviceNo { get; set; }
        public string FromDate_Dt { get; set; }
        public string ToDate_Dt { get; set; }
        public string bank_acc_id { get; set; }
        public string BankName { get; set; }
        public List<BankAccName> BankAccNameList { get; set; }
        public string InstrumentType { get; set; }
        public string InsTypeID { get; set; }
        public string VouSearch { get; set; }
        public string PayAdvItemDetails { get; set; }
        public string TblPayAdvItemDetail { get; set; }
        public string PayAdvStatus { get; set; }
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        
        public string TransType { get; set; }
       
        public string DeleteCommand { get; set; }
       
        public string Create_id { get; set; }
        
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string AppStatus { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public bool CancelFlag { get; set; }
        public bool Cancelled { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string WF_Status1 { get; set; }
        public string ListFilterData1 { get; set; }
       
       
        public List<VouItemList> VouItm_List { get; set; }
    }
    
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Adv_No { get; set; }
        public string Adv_Dt { get; set; }
        public string wf { get; set; }
        public string ListFilterData1 { get; set; }
    }

    public class VouItemList
    {
        public string Vou_No { get; set; }
        public string Vou_Dt { get; set; }
        public string Vou_Date { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountId { get; set; }
        public string GLAccount { get; set; }
        public string GLAccountId { get; set; }
        public string Amount { get; set; }
        public string Instrument_Type { get; set; }
        public string InsType_Id { get; set; }
        public string Instrument_No { get; set; }
        public string Instrument_Dt { get; set; }

        public string ChkPadv { get; set; }
    }
   
    public class BankAccName
    {
        public string bank_acc_id { get; set; }
        public string bank_acc_name { get; set; }
    }
    public class PaymentAdviceListModel
    {
        public string WF_Status { get; set; }
        public string PASearch { get; set; }
        public string Title { get; set; }
        public string MenuDocumentId { get; set; }
        
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string VouToDate { get; set; }
        public string VouFromDate { get; set; }
        public string ListFilterData { get; set; }
        public DateTime ToDate { get; set; }
        public List<PaymentAdviceList> PAList { get; set; }
        public List<Status> StatusList { get; set; }
        
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class PaymentAdviceList
    {
        public string PANumber { get; set; }
        public string PADate { get; set; }
        public string hdPADate { get; set; }
        
        public string PAStatus { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }

    }
}

