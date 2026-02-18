using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.CustomInvoice
{
    public interface CustomInvoice_ISERVICE
    {
        Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID, string CustType);
        DataSet GetAllData(string CompID, string Cust_Name, string BrchID, string CustType
            , string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus);
        Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID);
        DataTable Getcurr_details(string CompId, string BrID, string ship_no, string ship_dt);
        DataTable GetTaxTypeList(string CompID, string BranchID);
        DataTable GetOCList(string CompID, string BranchID);
        
        DataSet GetShipmentList(string Cust_id, string CompId, string BrID);
        DataSet GetShipmentDetail(string ShipmentNo, string ShipmentDate, string CompId, string BrID);
        string InsertCI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblVouGLDetail
            , DataTable DTTaxDetail, DataTable DtblOCDetail, DataTable DtblAttchDetail, string Narration);
        string Delete_CI_Detail(CustomInvoice_Model _CustomInvoice_Model, string CompId, string BrID);
        
        string Approve_CI(string CompID, string BrchID, string CI_No, string CI_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks, string SaleVouMsg);
       
        DataSet CheckSaleReturnAgainstSaleInvoice(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetStatusList(string MenuID);
        DataSet Get_CI_DetailList(string CompId, string BrchID, string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus);
        DataSet Edit_CIDetail(string CompID, string BrchID, string CINo, string CIDate,  string UserID, string DocumentMenuId);
        
        DataSet GetCIAttatchDetailEdit(string CompID, string BrchID, string CI_No, string CI_Date);
        DataSet GetCustomInvoiceDeatilsForPrint(string CompID, string BrchID, string CI_No, string SI_Date,string flag);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetTaxRecivableAcc(string comp_id, string br_id);
       
        DataTable GetCustomItemsToExportExcel(string compId, string brId, string invNo, string invDate);
        DataTable PortOfLoadingList();
        DataTable PlOfReceiptByPreCarrierList();
    }
}
