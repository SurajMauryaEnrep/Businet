using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.GatePassOutward
{
    public interface GatePassOutwardDetail_IServices 
    {
        string InsertUpdateData(DataTable FGRHeader, DataTable ItemDetails, DataTable Attachments);
        DataSet GetFGRDeatilData(string CompID, string BrchID, string gpass_no, string gpass_dt, string UserID, string DocumentMenuId);
        DataSet DeleteData(string comp_id, string br_id, string gpass_no, string gpass_dt);
        string Approve_FinishedGoodsReceipt(string comp_id, string br_id, string DocumentMenuID, string gpass_no, string gpass_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet CancelAndReturnData(string CompID, string br_id, string rcpt_No, string rcpt_Date, string UserID, string DocumentMenuId, string mac_id);
        DataSet GetDataForPrint(string CompID, string BrchID, string Doc_No, string Doc_dt);
        DataSet GetCustandSuppAddrDetailDL(string Entity_Name, string CompID, string BrchID, string Entity_Type);
    }
}
