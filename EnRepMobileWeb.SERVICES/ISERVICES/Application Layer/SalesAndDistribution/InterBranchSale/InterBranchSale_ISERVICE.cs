using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.InterBranchSale
{
    public interface InterBranchSale_ISERVICE
    {
        DataSet GetAllData(string CompID, string CustName, string BranchID, string User_ID, string CustId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetWarehouseList(string CompId, string BrID);
        Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType,string DocId);
        DataSet GetCustAddrDetailDL(string Cust_id, string CompId, string br_id, string DocumentMenuId);
        DataSet GetIBSDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID);
        Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID);
        DataTable Scrap_GetSubItemDetails(string CompId, string BranchId, string itmid, string docno, string Sinv_dt, string flag);
        DataSet getItemStockBatchWise(string ItemId, string UomId, string WarehouseId, string CompId, string BranchId);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string Doc_No, string Doc_dt, string ItemID, string UomId);
        DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string Wh_ID);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string Doc_no, string Doc_dt, string ItemID);
        string InsertIBSDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
           , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
           , DataTable CRCostCenterDetails, DataTable SubitemINVQty/*, DataTable DtblTdsDetail, string Tds_amt*/,
           DataTable ItemBatchDetails, DataTable DtblOCTdsDetail, string Narr, string CN_Narr
           , DataTable ItemSerialDetails);
        string DeleteIBSDetails(string CompID, string BrchID, string Inv_no, string Inv_dt);
        string ApproveIBSDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID, string mac_id
           , string wf_status, string wf_level, string wf_remarks, string SaleVouMsg, string PV_VoucherNarr, string BP_VoucherNarr
           , string DN_VoucherNarr, string DN_Nurr_Tcs);
        DataSet GetIBSDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet GetIBSGstDtlForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet checkDependencyIBP(string Comp_ID, string brchId, string SI_No, string SI_Date);
    }
}
