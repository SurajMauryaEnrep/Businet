using System;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountPayable
{
    public interface AccountPayable_ISERVICE
    {
        DataSet GetUserRangeDetail(string CompID, string UserID);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetsuppportDAL(string CompID);
        String InsertUserRangeDetail(string CompID, string user_id, string range1, string range2, string range3, string range4, string range5);
        DataSet GetAgingDetailList(string CompId, string BranchID, string UserID, string Supp_id, string Cat_id, string Prf_id, string Basis, string AsDate, int Curr_Id, string Flag, int Acc_Id, string PayableType, string ReportType, string brlist);
        DataTable GetInvoiceDetailList(string CompId, string BranchID, string Supp_id, string lrange, string urange, string Basis, string AsDate, int CurrId, string PayableType, string ReportType, string inv_no, string inv_dt, string brlist);
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string Doc_id);
        DataSet SearchAdvanceAmountDetail(string compId, string brId, string accId, int CurrId, string AsDate, string Basis, string PayableType, string brlist);
        DataSet SearchPaidAmountDetail(string compId, string brId, string InVNo, string InvDate, string AsOnDate,string supp_id);

        DataSet GetInvoiceDeatilsForPrint(string CompID, string BrchID, string invNo, string invDate, string dataType);
        DataSet GetGLAccountPayablePrintData(string compId, string brId, string accId, string currId, string asOnDate, string userId, string brlist);
    }
}

