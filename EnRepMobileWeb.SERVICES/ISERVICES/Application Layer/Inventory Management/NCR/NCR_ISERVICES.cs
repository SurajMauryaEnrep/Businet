using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.NCR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.NCR
{
    public interface NCR_ISERVICES
    {
        DataSet GetNcrDetails(string CompID, string BrId, string FromDt, string ToDt, string SrcDocNo, string Status);
        string SaveNcrAckDetails(string compID, string brchID, AckListDataModel ackListData);
        DataSet GetNcrDetailonAcknowledge(string compID, string brchID, string item_id, string uom_id, string src_type
            , string doc_no, string doc_dt, string entity_id);
    }
}
