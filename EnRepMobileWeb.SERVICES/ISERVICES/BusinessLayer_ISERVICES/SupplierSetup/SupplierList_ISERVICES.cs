using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
    public interface SupplierList_ISERVICES
    {
        DataSet GetSuppListDAL(string supp_id, string CompID);
        DataSet GetSupplierListFilterDAL(string CompID, string SuppID, string Supptype, string Suppcat, 
            string Suppport, string SuppAct, string SuppStatus, string Glrtp_id);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetsuppportDAL(string CompID);
        DataTable Getsuppplier(string GroupName, string CompID);
        DataSet GetAllDropdown(string GroupName, string CompID,string supp_id);
        DataTable GetGlReportingGrp(string Comp_ID, string Br_ID, string gl_repoting);
        //Dictionary<string, string> SuppSetupGroupDAL(string GroupName, string CompID);
        DataSet GetVerifiedDataOfExcel(string compId, DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAddress);
        DataTable ShowExcelErrorDetail(string compId, DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAddress);
        string BulkImportSupplierDetail(string compId, string UserID, string BranchName, DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAddress);
        DataSet GetMasterDropDownList(string Comp_id, string Br_ID);
        DataSet GetSuppLedgerDtl(string Comp_id, string Br_ID, string Supp_id, string SuppAcc_id, string Curr_id); 
    }
}
