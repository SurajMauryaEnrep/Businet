using System.Collections.Generic;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ServiceSaleInvoice;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ServiceSaleInvoice
{
    public interface ServiceSI_ISERVICE
    {
        //Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType);
        DataSet GetAllData(string CompID, string SuppName, string BranchID, string CustType, string UserID, string CustID, string Fromdate, string Todate, string Status, string Docid, string wfStatu,string SalesPerson);
        DataSet GetCustAddrDetailDL(string Cust_id, string CompId,string br_id,string DocumentMenuId);
        DataSet GetServiceVerifcationList(string Supp_id, string CompId, string BrID);
        DataSet GetServiceVerifcationDetail(string VerNo, string VerDate, string CompId, string BrID);
        DataSet CheckSSIDetail(string CompId, string BrchID, string DocNo, string DocDate);
        string InsertSSI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail,DataTable DtblOCTdsDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            , DataTable CRCostCenterDetails,string Narr,string CN_Narr,string slprsn_id,string cust_ref,string pay_term, string del_term,string decl_1
            ,string decl_2,string inv_heading,string nontaxable,string Ship_Add_Id,string PlcOfSupply, string roundof, string pm_flag,string ShipTo, DataTable DTPaymentSchedule);
        DataSet EditSSIDetail(string CompId, string BrID, string PINo, string PIDate, string Type, string UserID, string DocID);
        DataSet GetSSIAttatchDetailEdit(string CompID, string BrchID, string PI_No, string PI_Date);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetSSIList(string CompId, string BrchID, string UserID, string CustID, string Fromdate, string Todate, string Status, string Docid, string wfStatus,string sales_person);
        DataSet GetServiceSalesInvoiceDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID);
        string ServiceSIDelete(ServiceSIModel _ServicePIModel, string CompID, string br_id, string DocumentMenuId);
        string ApproveSSI(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string SaleVouMsg, string PV_VoucherNarr, string BP_VoucherNarr,string DN_VoucherNarr);
        DataSet GetSlsInvGstDtlForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID,string Sinv_no,string Sinv_dt);

        DataSet GetSrcDocNumberList(string Cust_id, string CompId, string BrID);
        DataSet GetItemDetailData(string Cust_id, string Comp_ID, string Br_ID, string SourceDocNo, string SourceDocdt);
    }
}
