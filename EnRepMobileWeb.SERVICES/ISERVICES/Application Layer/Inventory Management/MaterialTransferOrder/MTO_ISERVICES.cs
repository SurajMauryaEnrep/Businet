using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialTransferOrder;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialTransferOrder
{
    public interface MTO_ISERVICES
    {
        //DataTable GetWhList(string CompId, string BrchID);
        //DataTable GetToBranchList(string CompId,string BrchID);
        DataTable GetToWhList(string CompId, string SrcBrId, string DocumentMenuId);

        DataSet MTO_GetAllDDLListAndListPageData(string CompId, string BrchID,string flag,string PageName, string UserID, string wfstatus, string DocumentMenuId,
            string startDate, string CurrentDate);
        DataSet GetMTODetail(string CompID, string trf_no, string BrchID, string UserID, string DocumentMenuId);
        string InsertMTO(DataTable MRSHeader, DataTable MRSItemDetails, DataTable dtSubItem);       
        DataSet MTOApprove(MTOModel _MTOModel,string CompID, string br_id,string app_id, string mac_id, string DocumentMenuId);       
        DataSet MTODelete(MTOModel _MTOModel, string CompID, string br_id, string DocumentMenuID);     
        String MTOCancel(MTOModel _MTOModel, string CompID, string br_id, string mac_id);      
        String MTOForceClose(MTOModel _MTOModel, string CompID, string br_id, string mac_id);
        DataSet GetItemList(string CompId, string BranchId, string ItmName, string ToBranchId);
        //For Sub-Item Data fetch
        DataSet MTR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetSourceAndDestinationList(string CompId, string BrID, string wh_id,string doc_id);
        DataSet GetMaterialTransferIssuePrint(string CompID, string BrchID, string Doc_No, string Doc_dt);
    }
    public interface MTOList_ISERVICES
    {
        DataSet GetMTODetailList(string CompId, string BrchID, string To_WH, string To_BR, string Fromdate, string Todate, string TRF_Type, string Status, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetSerchDetailList(string CompId, string BrchID, int To_WH, int To_BR, string Fromdate, string Todate, string TRF_Type, string Status, string UserID, string DocumentMenuId);
        DataSet GetStatusList(string MenuID);
    }
}
