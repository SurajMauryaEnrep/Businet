using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher
{
   public interface JournalVoucher_ISERVICES
    {
       
        string InsertUpdateJV(DataTable JVHeader, DataTable JVAccountDetails, DataTable JVAttachments,DataTable JVCostCenterDetails);
        DataSet GetJVDetailList(string CompId, string BrchID,string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId, string searchValue);
        DataSet getdetailsJV(string CompId, string BranchId, string Doc_no, string Doc_date, string UserID, string DocumentMenuId);
        string JVDelete(JournalVoucher_Model _JVModel, string CompID, string br_id);
        //DataSet JVApprove(JournalVoucher_Model _JVModel, string CompID, string br_id, string app_id, string mac_id);
        DataSet GetGLVoucherPrintDeatils(string CompID, string br_id, string JVNo, string JVDate,string Vou_type);
        String JVCancel(JournalVoucher_Model _JVModel, string CompID, string br_id, string mac_id);
        string InsertJornlVoucApproveDetails(string JVNo, string JVDate, string CompID, string BrchID, string DocumentMenuId, string UserID, string mac_id, string A_Status, string A_Level, string A_Remarks, string Flag);
        DataSet CheckJVDetail(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet getReplicateWith(string comp_id, string br_id, string OrderType, string SarchValue);
        DataSet GetReplicateWithItemdata(string comp_id, string br_id, string Vou_no, string vou_dt,string vou_type);
    }
}
