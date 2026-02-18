using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISGoodReceiptDetail
{
    public interface MISGoodReceiptDetail_IService
    {
        DataTable GetGoodReceiptNoteMISReport(string compId, string brId, string showAs, string fromdate, string toDate,
            string suppId, string itemId,string MultiselectStatusHdn,string ReceiptType,string EntityType);

        DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType);
        DataTable GetBatchDeatilMIS(string compId, string BrID, string recept_no, string recept_dt, string Item_id);
        DataTable GetMISSerialDetailData(string compId, string BrID, string recept_no, string recept_dt, string Item_id);
    }
}
