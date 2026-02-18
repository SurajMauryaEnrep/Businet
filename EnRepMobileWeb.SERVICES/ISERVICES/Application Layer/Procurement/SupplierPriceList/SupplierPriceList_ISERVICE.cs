using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.SupplierPriceList
{
    public interface SupplierPriceList_ISERVICE
    {
        DataSet GetAllData(string CompID, string BranchID,string UserID, string catalog, string portfolio, string ActStatus, string ValidUpto, string Docid, string wfStatus);
        DataSet GetSupplierAndItemList(string CompID, string BranchID);
        string InsertDPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblRevisionDetail);
        string DeleteSPLDetails(string CompID, string BrchID, string supp_id);
        string ApproveSPLDetail(string supp_id, string MenuDocId, string Branch, string CompID
    , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks);
        DataSet GetDPIDetailDAL(string CompId, string BrID, string supp_id,string user_id, string DocID);
        DataSet getReplicateWith(string comp_id, string br_id, string SarchValue);
        DataSet GetReplicateWithItemdata(string comp_id, string br_id, string supp_id);
        DataTable GetCustPriceHistryDtl(string Comp_ID, string Br_ID, string supp_id, string Item_id);
    }
}
