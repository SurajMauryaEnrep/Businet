using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.DocApproval
{
    public interface DocApproval_ISERVICES
    {
        DataSet getAppDocDetails(string flag,string CompID, string create_id);
        DataSet getDocAppList(string CompID, string br_id);
        DataSet getDocAppListFilter(string CompID, string br_id, string doc_id);
        DataSet getDocAppEditDetails(string Br_ID,string Doc_ID, string CompID);
        Dictionary<string, string> doc_list(string GroupName, string CompID,string branch_id);
        Dictionary<string, string> user_list(string GroupName, string CompID,string branch_id,string DocName);
        string InsertDocAppDetails(DataTable DocAppHeader, DataTable DocAppUserList);
        string DeleteDocAddDetails(string br_id,string doc_id,string CompID);
        string checkEditClick(string CompID,string br_id,string doc_id);
    }
}
