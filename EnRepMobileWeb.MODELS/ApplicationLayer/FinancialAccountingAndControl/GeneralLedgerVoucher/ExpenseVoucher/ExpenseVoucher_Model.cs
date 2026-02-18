using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher
{
    public class ExpenseVoucher_Model
    {
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Vou_No { get; set; }
        public string Vou_Date { get; set; }
        public string PayeeAcc_Id { get; set; }
        public string Status { get; set; }
        public string Create_Id { get; set; }
        public string Create_Dt { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string PaymentVouDetails { get; set; }
        public string ExpenseDescDetails { get; set; }
        public string CC_DetailList { get; set; }
        public string VouGlDetails { get; set; }
        public string DocumentStatus { get; set; }
        public string DeleteCommand { get; set; }
        public string ListFilterData1 { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string TransType { get; set; }
        public string BtnCommand { get; set; }
        public string Command { get; set; }
        public string DocumentMenuId { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_by { get; set; }
        
        public string PayeeGlList { get; set; }
        public string AccountDetail { get; set; }
        public string TotalPay { get; set; }
        public string TotalAdj { get; set; }
        public string TotalUnAdj { get; set; }
        public string TotalPend { get; set; }
        public string TotalExp { get; set; }
        public List<PayeeGlAccList> payeeGlAccLists { get; set; }

        /*--Attachment Section---*/
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public bool CancelFlag { get; set; }
        public string GLVoucherType { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherDt { get; set; }
        public string payGLvoucher_narr { get; set; }
    }
    public class UrlData
    {
        public string DocStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Vou_No { get; set; }
        public string Vou_Dt { get; set; }
        public string ListFilterData1 { get; set; }
        public string gl_Vou_No { get; set; }
        public string gl_Vou_Dt { get; set; }
    }
    public class PayeeGlAccList
    {
        public string Payee_acc_id { get; set; }
        public string payee_acc_name { get; set; }
    }

    public class ExpenseVoucher_model
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    //public class CostCenterDt
    //{
    //    public List<CostcntrType> costcntrtype { get; set; }
    //    public string cc_type_id { get; set; }
    //    public DataTable _CCItemDetails { get; set; }

    //    public string disflag { get; set; }

    //}

    //public class CostcntrType
    //{
    //    public string cc_id { get; set; }
    //    public string cc_name { get; set; }
    //}
}
