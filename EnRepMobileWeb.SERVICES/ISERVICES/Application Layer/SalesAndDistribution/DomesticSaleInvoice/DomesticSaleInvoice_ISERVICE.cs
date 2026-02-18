using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSaleInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSaleInvoice
{
    public interface DomesticSaleInvoice_ISERVICE
    {
        Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID, string CustType,string DocId);
       DataSet GetAllData(string CompID, string Cust_Name, string BrchID, string CustType, string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus,string SalesPerson);
        Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID,string SI_Number,string SI_Date);
        DataTable Getcurr_details(string CompId, string BrID, string ship_no, string ship_dt);
        DataTable GetSalePerson_details(string CompId, string BrID, string ship_no, string ship_dt);
        DataTable GetTaxTypeList(string CompID, string BranchID);
        DataTable GetOCList(string CompID, string BranchID);
        
        DataSet GetShipmentList(string Cust_id, string CompId, string BrID);
        DataSet GetShipmentDetail(string ShipmentNo, string ShipmentDate, string CompId, string BrID, string Flag,string Inv_type);
        string InsertSI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblVouGLDetail, DataTable DTTaxDetail
            , DataTable DTOCTaxDetail, DataTable DTOCDetail,DataTable DtblOCTdsDetail, DataTable dtSubItem, DataTable DtblAttchDetail
            ,DataTable CRCostCenterDetails,DataTable DtblTcsDetail, string Nurr,string CN_Nurr, string inv_disc_amt, string inv_disc_perc, string decl_1
            , string decl_2, string inv_heading, string nontaxable,string ShipTo, DataTable DTPaymentSchedule,string corp_addr,string remarks);
        string Delete_SI_Detail(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model, string CompId, string BrID);
        
        string Approve_SI(string CompID, string BrchID, string SI_No, string SI_Date, string InvType,string UserID
            , string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks, string SaleVouMsg
            , string PV_VoucherNarr, string BP_VoucherNarr,string DN_VoucherNarr,string DN_Nurr_Tcs);
       
        DataSet CheckSaleReturnAgainstSaleInvoice(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetStatusList(string MenuID);
        DataSet GetSI_DetailList(string CompId, string BrchID, string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus, string Order_type,string sales_person);
        DataSet Edit_SIDetail(string CompID, string BrchID,string VouType, string SINo, string SIDate, string Type, string UserID, string DocumentMenuId);
        
        DataSet GetSIAttatchDetailEdit(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet GetSalesInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date, string inv_type);
        DataSet GetSlsInvGstDtlForPrint(string CompID, string BrchID, string SI_No, string SI_Date, string inv_type);

        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetRoundOffGLDetails(string comp_id, string br_id);
        DataSet SI_GetSubItemDetails(string CompID, string Br_id, string ItemId, string SrcDoc_no, string SrcDoc_dt, string doc_no, string doc_dt, string Flag,string DocumentMenuId);
        DataSet SI_GetSubItemDetailsafterapprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt,string DocumentMenuId,string Flag);

        //DataSet GetOCGLDetails(string OCID, string CompId);
        //DataSet GetTaxGLDetails(string TaxID, string CompId);
        DataTable GetBankdetail(string CompID, string BranchID);
        DataSet GetAllData(string CompID, string BranchID, string Cust_Name,string CustType,string SalesPersonName);
        DataSet getbankdeatils(string bankName, string CompID, string BranchID);
        DataTable GetSaleInvoiceItemstoExportExcel(string compId, string brId, string invNo, string invDate, string docId);
        DataSet BindDispatchDetail(string CompID, string BrchID);
        DataTable GetDistrictOnStateDDL(string ddlStateID);
        DataTable GetCityOnDistrictDDL(string ddlDistrictID);
    }
}
