using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemSearch
{
    public interface ItemSearch_ISERVICES
    {
      
        DataSet GetItemListDetail(string CompID, string BranchId, string ItemID);
        DataTable BindItemList(string ItemName, string CompID, string BranchId);
        DataSet GetPrintItemSearchDeatils(string CompID, string BranchId, string itemId);
    }
}
