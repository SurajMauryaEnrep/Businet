using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentAdvice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.PaymentAdvice
{
   public interface PaymentAdvice_ISERVICES
    {
        DataSet GetFinYearDates(string CompID, string BrchID);
        Dictionary<string, string> AutoGetBankAccList(string CompID, string AccName, string BrchID);
        DataSet SearchPayAdvItemDetails(string comp_id, string br_id, string FromDate, string ToDate, string InsType, string BankAcc_id);
        String InsertPayAdvDetail(DataTable PayAdviceHeaderDetail,  DataTable PayAdviceItemDetail);
        DataSet GetPayAdvDetailOnView(string CompId, string BrID,string PAdv_No, string PAdv_Date,string UserID, string DocID);
        DataSet GetPayAdvListDetail(string CompID, string BrchID, string Fromdate, string Todate, string Status, string wfstatus, string UserID, string DocumentMenuId);
        string PADelete(PaymentAdviceModel _PaymentAdviceModel, string CompID, string br_id, string DocumentMenuId);
        string ApprovePADetail(string PAdv_No, string PAdv_Date, string MenuDocId, string Branch, string CompID, string UserID, string mac_id
           , string wf_status, string wf_level, string wf_remarks);
        DataSet GetPAPrintDeatils(string Comp_ID, string BrchID, string PAdvNo, string PAdvDate, string Status);

    }
}
