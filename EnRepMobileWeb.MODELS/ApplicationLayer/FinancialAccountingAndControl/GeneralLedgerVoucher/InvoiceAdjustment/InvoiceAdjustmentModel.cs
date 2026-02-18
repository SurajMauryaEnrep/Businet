using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment
{
    public class InvoiceAdjustmentModel
    {
   
        public string hdnsaveApprovebtn { get; set; }
        public string BalanceByFilter { get; set; }
        public string BalanceBy { get; set; }
        public string InvoiceAdjustmentDate { get; set; }
        public string InvoiceAdjustmentNo { get; set; }
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string Title { get; set; }
        public string TransType { get; set; }
        public string Vou_No { get; set; }
        public string Vou_Date { get; set; }
        public string EntityType { get; set; }
        public string Entity_id { get; set; }
        public string Entity_Name { get; set; }
        public string DeleteCommand { get; set; }
        public string AdvanceAdjDetails { get; set; }
        public string BillAdjdetails { get; set; }
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string Status { get; set; }
        public string VouStatus { get; set; }
        public bool CancelFlag { get; set; }
        public bool Cancelled { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string WF_Status1 { get; set; }
        public string ListFilterData1 { get; set; }
        public List<EntityName> EntityNameList { get; set; }
        public List<AdvanceList> Advance_List { get; set; }
        public List<BillsList> Bills_List { get; set; }
    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string In_No{ get; set; }
        public string In_DT { get; set; }
        public string DMS { get; set; }

    }
    public class AdvanceList
    {
        public string Vou_No { get; set; }
        public string Vou_Dt { get; set; }
        public string Adv_Curr { get; set; }
        public string AdvCurrId { get; set; }
        public string Adv_Amt_Bs { get; set; }
        public string Adv_Amt_Sp { get; set; }
        public string Adv_Un_Adj_Amt { get; set; }
        public string Adv_Adj_Amt { get; set; }
        public string Adv_Rem_Bal { get; set; }      

    }
    public class BillsList
    {
        public string Inv_No { get; set; }
        public string Inv_Dt { get; set; }
        public string Bill_No { get; set; }
        public string Bill_Dt { get; set; }
        public string Inv_Curr { get; set; }
        public string InvCurrId { get; set; }
        public string Inv_Amt_Bs { get; set; }
        public string Inv_Amt_Sp { get; set; }
        public string Inv_Un_Adj_Amt { get; set; }
        public string Inv_Adj_Amt { get; set; }
        public string Inv_Rem_Bal { get; set; }      

    }
    public class EntityName
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class InvoiceAdjustmentListModel
    {
        public string WF_Status { get; set; }
        public string VouSearch { get; set; }
        public string Title { get; set; }
        public string MenuDocumentId { get; set; }
        public string EntyType { get; set; }
        public string EntyId { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string VouFromDate { get; set; }
        public string VouToDate { get; set; }       
        public string entity_type { get; set; }       
        public string ListFilterData { get; set; }       
        public DateTime ToDate { get; set; }       
        public List<InvoiceAdjustmentList> InvAdjList { get; set; }       
        public List<Status> StatusList { get; set; }
        public List<EntityName> EntityNameList { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class InvoiceAdjustmentList
    {
        public string VouNumber { get; set; }
        public string VouDate { get; set; }
        public string hdVouDate { get; set; }
        public string EntyType { get; set; }
        public string EntyName { get; set; }
        public string EntyId { get; set; }
        public string VouStatus { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }

    }
   
}
