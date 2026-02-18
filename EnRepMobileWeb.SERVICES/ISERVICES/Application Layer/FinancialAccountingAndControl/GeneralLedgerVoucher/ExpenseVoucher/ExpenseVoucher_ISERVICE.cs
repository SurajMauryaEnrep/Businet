using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher
{
    public interface ExpenseVoucher_ISERVICE
    {
        DataTable GetGLAccList(string CompId,string BrId);
        DataSet GetExpenseAcc(string CompId, string BrID, string searchval);
        DataTable Getgl_accgroup(string CompId, string BrID, string acc_id);
        DataTable GetClosingBal(string CompId, string BrID, string acc_id, string Vou_Dt);
        DataTable Get_VouDetails(string compid, string brid, string acc_id, string vou_no, string vou_dt);
        string InsertExpenseVouDetail(DataTable ExpenseVoucherHeader, DataTable ExpVouPaymentDetail, DataTable ExpVouExpenseDesc, DataTable BPAttachments, DataTable DtblVouGLDetail, DataTable CostCenterDetails);
        DataSet GetVouList(string CompId, string BrID, string acc_id, string searchval);

        DataSet GetExpenseVouDetails(string compId, string brId, string UserID, string vou_no, string vou_dt);

        string DeleteExpenseDetail(string compID, string brId, string Vou_No, string Vou_Dt);

        DataSet GetExpenseVouList(string CompId, string BrId, string FromDate, string ToDate);

        DataSet SerachListExpenseVoucher(string compId, string BrId, string Acc_Id,string FormDate,string Todate, string Status);
        string ApproveExpVouDt(string compID, string brId, string vou_no, string vou_dt, string a_Status, string a_Level, string a_Remarks, string userID, string mac_id, string documentMenuId, string narr);
        string CancelExpVouDt(string compID, string brId, string vou_no, string vou_dt, 
            string userID, string mac_id, string CancelledRemarks);
    }
}
