using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup
{
    public interface MISDetail_ISERVICES
    {
        string SaveMISData(string TransType, string setup_type_id, string setup_type_name, string setup_id, string setup_val, string mac_id, string comp_id, string setup_flag, string br_id, string user_id);
        string SaveSalesExecutiveData(string action, string compId, string slsPersId, string slsPersName, string slsContno, string slsEmail, DataTable cstBranchList,string SalesRegion);
        string SaveEmployeeData(string action, string compId, string slsPersId, string slsPersName, string slsContno, string slsEmail, DataTable cstBranchList);
        string SaveWastageReasonDetails(string action, string compId, string wastage_id, string wastage_reason, DataTable cstBranchList);
        string SaveRejectionReasonDetails(string action, string compId, string Rejection_id, string Rejection_reason, DataTable cstBranchList);
        string SaveGLReport(string action, string compId, string GL_ID, string GL_Reporting, DataTable cstBranchList);
        string DeleteMISData(string setup_type_id, string setup_id, string setup_val, string comp_id, string br_id);
        DataSet Get_MISAllTables(string comp_id, string br_id);
        DataTable checkSalesExecutiveBranchStatus(string compId, string branchId, string seId);
        DataTable checkBranchStatus(string compId, string branchId, string Doc_id, string flag);
        string DeleteSeDetail(string compId,string brId, string seId);
        string DeleteWastageDetail(string CompID, string br_id, string wastage_id);
        string DeleteRejectionDetail(string CompID, string br_id, string rej_id);
        string DeleteEmployeeSetup(string CompID, string br_id, string emp_id);
        string DeleteGLrpt_grp(string CompID, string br_id, string gl_id);
     
        string DeletePortDetail(string Portid);/*Added by Nitesh 14-12-2023 delete port Data but not Working*/
        DataTable getstatelist(string countryid,string hdnstate_id);/*Added by Nitesh 14-12-2023 get State list*/
        string Saveportdata(string Country, string portid, string PortDescription, string PinCode, string state,string Command, string hdnprot_id,string Porttype);/*Added BY NItesh 14-12-2023 Save Data*/
       // DataTable checkdependcyport(string compId, string brId, string tblport_id);
    }
}
