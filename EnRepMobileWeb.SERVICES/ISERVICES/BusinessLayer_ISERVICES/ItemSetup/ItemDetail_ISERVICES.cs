using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.ItemDetail;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
    public interface ItemDetail_ISERVICES
    {
        Dictionary<string, string> ItemSetupHSNDAL(string CompID,string HSNName );
        Dictionary<string, string> GetGroupList(string CompID, string GroupName);
        string InsertItemSetupDetailsDAL(DataTable ItemDetail, DataTable ItemBranch, DataTable ItemAttachments, DataTable ItemAttribute, DataTable ItemCustomer, DataTable ItemSupplier, DataTable ItemPortfolio,DataTable ItemVehicle,DataTable ItemSubItem,int ExpiryAlertDays);
        DataTable GetBrListDAL(string CompID);
        DataSet GetBrList(string CompID);
        DataSet GetVehicalInfoAccrdn(string CompID, string br_id);
        DataTable GetportfDAL(string CompID);
        DataTable GetbinDAL(string CompID);
        DataTable GetAttributeListDAL(string CompID);
        DataSet GetAttributeValueDAL(string CompID, string AttriID);
        DataSet AutoGenerateRef_No_Catlog_no(string Comp_ID, string stockable, string saleable, string ItemCatalog);
        DataTable GetwarehouseDAL(string CompID);
        DataTable GetUOMDAL(string CompID);
        DataSet GetAllDropDownData(string CompID,string BranchId, string GroupName,string HSNCode,string  AccName, string SupplierName, string DocumentMenuId);
        DataSet GetviewitemdetailDAL(string itmcode, string CompID,string BrchID);
        DataSet GetSelectedParentDetail(string item_grp_id, string CompId);
        Dictionary<string, string> GetLocalSaleAccount(string AccName, string CompID);
        Dictionary<string, string> GetLocalPurchaseAccount(string AccName, string CompID);
        Dictionary<string, string> GetStockAccount(string AccName, string CompID);
        Dictionary<string, string> GetProvisionalPayableAccount(string AccName, string CompID);
        DataSet GetItemPropertyToggleDetail(string item_grp_id, string CompId);
        DataSet ItemDetailDelete(ItemDetailModel _ItemDetail, string comp_id, string item_id);
        DataSet ItemApprove(ItemDetailModel _ItemDetail, string comp_id, string app_id, string item_id,string mac_id);
        DataSet GetCustomerList(string CompID, string CustName);
        DataSet GetSupplierList(string CompID, string SuppName);
       DataSet GetPortfList(string CompID, string PortfName);
        DataSet GetVehicleList(string CompID, string VehicleName);
    }
}
