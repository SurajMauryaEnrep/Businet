using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISFinishedGoodsReceipt
{
    public interface MISFinishedGoodsReceipt_ISERVICES
    {
        Dictionary<string, string> ItemList(string GroupName, string CompID, string BrchID);
        Dictionary<string, string> ItemGroupList(string GroupName, string CompID);
        DataTable GetDataTableMISFGR(string CompID, string BrchID, string itmid, string GroupID,
            string txtFromdate, string txtTodate,string MultiselectItemHdn,string ShopFloor_id,string ddl_ShowAs);

        DataSet GetBatchDeatilData(string CompID, string BrchID, string rcpt_no, string rcpt_dt, string item_id);
    }
}
