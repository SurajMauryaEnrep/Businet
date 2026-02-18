using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ScrapSaleInvoice;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ScrapSaleInvoice
{
    public interface ScrapSI_ISERVICE
    {
        //Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType);
        DataSet GetCustAddrDetailDL(string Cust_id, string CompId,string br_id,string DocumentMenuId);
        DataSet GetScrapVerifcationList(string Supp_id, string CompId, string BrID);
        DataSet GetScrapVerifcationDetail(string VerNo, string VerDate, string CompId, string BrID);
        string InsertscrpSSI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            , DataTable CRCostCenterDetails, DataTable SubitemINVQty/*, DataTable DtblTdsDetail, string Tds_amt*/, 
            DataTable ItemBatchDetails,DataTable DtblOCTdsDetail, string Narr, string CN_Narr,string slprsn_id
            ,DataTable DtblTcsDetail,DataTable ItemSerialDetails, string pay_term, string del_term, string decl_1
            , string decl_2, string inv_heading, string nontaxable,string ShipTo,DataTable DTPaymentSchedule, string corp_addr, string pvt_mark);
        DataSet EditSSIDetail(string CompId, string BrID, string PINo, string PIDate, string Type, string UserID, string DocID);
        DataSet GetSSIAttatchDetailEdit(string CompID, string BrchID, string PI_No, string PI_Date);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetSSIList(string CompId, string BrchID, string UserID, string CustID, string Fromdate, string Todate, string Status, string Docid, string wfStatus,string sales_person);
        DataSet GetScrapSalesInvoiceDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID);
        string ScrapSIDelete(ScrapSIModel _ScrapPIModel, string CompID, string br_id, string DocumentMenuId);
        string ApproveSSI(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID, string mac_id
            , string wf_status, string wf_level, string wf_remarks, string SaleVouMsg, string PV_VoucherNarr, string BP_VoucherNarr
            , string DN_VoucherNarr, string DN_Nurr_Tcs);
        DataSet getsubitem(string ItemId, string CompId, string BranchId);
        DataSet GetWarehouseList(string CompId, string BranchId);
        DataTable Scrap_GetSubItemDetails(string CompId, string BranchId,string itmid, string docno, string Sinv_dt,string flag);
        DataTable Scrap_GetSubItemDetails_INV_QTY(string CompId, string BranchId,string itmid, string wh_id, string UOMID);
        DataSet getItemStockBatchWise(string ItemId, string UomId, string WarehouseId, string CompId, string BranchId);

        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID,string Doc_No, string Doc_dt, string ItemID, string UomId);
        DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string Wh_ID);
        DataSet GetAllData(string CompID, string SuppName, string BranchID, string CustType, string UserID, string CustID, string Fromdate, string Todate, string Status, string Docid, string wfStatu,string SalesPerson);
        DataSet GetScrapSaleInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet GetScrapSlsInvGstDtlForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet CheckSSIDetail(string CompId, string BrchID, string DocNo, string DocDate);
        //DataTable GetTransporterList(string CompID, string BranchID);
        Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID,string Sinv_no,string Sinv_dt);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string Doc_no, string Doc_dt, string ItemID);
        DataSet BindDispatchDetail(string CompID, string BrchID);
        DataTable GetDistrictOnStateDDL(string ddlStateID);
        DataTable GetCityOnDistrictDDL(string ddlDistrictID);
    }
}
