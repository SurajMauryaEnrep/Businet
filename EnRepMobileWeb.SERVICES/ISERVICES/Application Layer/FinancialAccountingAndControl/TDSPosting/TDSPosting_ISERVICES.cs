using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.TDSPosting
{
    public interface TDSPosting_ISERVICES
    {
        DataSet GetTdsPostngList(string compId, string brId, string Year, string month, string status, string documentMenuId,string wfStatus,string UserID,string searchValue);
        DataSet GetTdsPostngDetail(string compId, string brId, string UserID, string month, string year,string tds_id);
        DataSet GetTdsPostngDetailToAdd(string compID, string brId, string from_dt, string to_dt);
        string InsertTdsPostingDetails(DataTable tdsPostingHeader, DataTable tdsPostingDetail, DataTable tdsPostingSlabDetail
            , DataTable TdsPostingGLDetail, DataTable TdsPostingGLDetailCC, DataTable TdsPostingSuppInvoice, DataTable TdsPostingSuppInvSlab);
        string DeleteTdsPostingDetails(string compID, string brId, string monthNo, string year);
        DataSet GetMonthOnBehalfYear(string compID, string brId, string tds_year);
        DataSet GetTdsSuppWiseInvoiceDetails(string compID, string brId, string supp_id, string preVlStD, string preVlEdD, string status, string tds_id);
        DataSet GetTdsSuppWiseTaxableValueDetails(string compID, string brId,string Year,string Month, string suppId, string StartDate, string EndDate);
        string ApproveTdsPostingDetails(string compID, string brId, string tds_id, string a_Status, string a_Level, string a_Remarks, string userID, string mac_id, string documentMenuId);
    }
}
