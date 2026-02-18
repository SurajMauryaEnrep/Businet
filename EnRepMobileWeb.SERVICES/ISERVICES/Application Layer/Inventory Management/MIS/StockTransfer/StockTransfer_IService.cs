using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockTransfer
{
    public interface StockTransfer_IService
    {
        DataTable GetItemsList(string compId);
        DataSet GetBranchAndWarehouseList(string compId, string brId);
        DataTable GetUomByItemId(string compId, string itemId);
        DataTable GetStockTransferReport(string compId, string brId, string itemId, string mtType, string srcBranch,
            string dstnBranch, string srcWarehouse, string dstnWarehouse, string fromDate, string toDate);
        DataTable GetStockTransferPopupData(string actflag, string compId, string brId, string itemId, string mtType, string srcBranch,
          string dstnBranch, string srcWarehouse, string dstnWarehouse, string fromDate, string toDate);

    }
}
