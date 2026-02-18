using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISDeliveryNoteDetail
{
    public interface MISDeliveryNoteDetail_IService
    {
        DataSet Get_FYList(string Compid, string Brid);
        DataSet GetSuppliersAndItemList(string CompID, string BrchID, string Supp_Name, string Item_Name);
        DataTable GetDeliveryNoteMISReport(string compId, string brId, string showAs, string fromdate, string toDate,
            string suppId, string itemId);
        DataTable GetSubItemDetails(string comp_ID, string br_ID,string DnNo, string DnDate, string item_id);
    }
}
