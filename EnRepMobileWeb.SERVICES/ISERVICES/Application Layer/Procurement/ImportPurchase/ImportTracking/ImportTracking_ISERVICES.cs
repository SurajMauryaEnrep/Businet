using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ImportPurchase.ImportTracking
{
    public interface ImportTracking_ISERVICES
    {
        Dictionary<string, string> GetSupplierList(string CompID, string BranchID, string SuppType, string SuppName);
        DataSet GetSrcDocNumberList(string CompId, string BrID, string Supp_id);
        DataSet GetPONumberDetail(string CompId, string BrID,string PONo);
        string InsertImportTrackingDetails(DataTable DTHeaderDetail, DataTable DTItemDetail/*, DataTable DtblAttchDetail*/);
        DataSet Edit_ImpTrackDetail(string CompId, string BrID, string PONo);
        DataSet GetAllDocImpTrackList(string CompId, string BrchID, string suppID, string SrcDocNo, string UserID);

        


    }
}
