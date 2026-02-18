using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ProductCatalouge;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ProductCatalouge
{
    public interface ProductCatalouge_ISERVICES
    {
       string InsertUpdateProdCatalogDetail(int CompID, int BrchID, string catal_No,string catal_Date,string cust_Id,string remark,string create_id,string Transtype,string DocumentMenuId, string mac_id, DataTable CatalogItemDetail,string Cust_type);
        Dictionary<string, string> GetCustomerListProdCata(string CompID, string BranchID, string CustomerName,string CustPros_type);
        //Dictionary<string, string> GetCustomerListtoEdit(string CompID, string BranchID);
        DataTable GetCustomerListtoEdit(string CompID, string BranchID, string CustomerName,string CustPros_type);
        DataSet BindItemName(string CompID, string BrID, string ItmName);
        Dictionary<string, string> GetGroupList(string CompID, string GroupName);
        DataSet GetPortFolioList(string CompID, string PortfName);
        DataSet GetVehicleList(string CompID, string VehicleName);
        DataSet GetVehOEMNoDetail(string CompID, string VehOEM_No);
        DataSet GetRefNoDetail(string CompID, string RefNo);
        DataSet GetTechSpecDetail(string CompID, string Techspec);
        DataSet GetFilterItem(string CompID, string fltrvalue, string fltrtype);
        DataSet GetListOfProdCatalogDetails(string comp_id, string br_id,string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetSearchListOfProdCatalogDetails(string CompID, string BrchID, string CustID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetProdCatalogueDetails(string CompID, string BrchID, string Doc_no, string Doc_date, string UserID, string DocumentMenuId);
        DataTable GetCustNameList(string CompId, string br_id, string CustomerName);
        DataSet GetAllData(string CompId, string br_id, string CustomerName, string UserID, string wfstatus, 
            string DocumentMenuId, string CustID, string Fromdate, string Todate, string Status);
        string DeleteProdCatlogDetails(string CompID, string BrchID, string doc_no, string doc_date);
        string InsertProdCatalogApproveDetails(string CTLNo, string CTLDate, string CompID, string BrchID, string DocumentMenuId, string UserID, string mac_id, string A_Status, string A_Level, string A_Remarks, string Flag);
        DataSet GetCatalogueDeatils(string CompID, string BrchID, string CTLNo, string CTLDate);


    }
}
