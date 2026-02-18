using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
   public interface GLList_ISERVICES
    {
        DataTable BindGetGLNameList(string GroupName, string CompID);
        DataSet GetAllDropDownGL(string GroupName, string CompID,string Br_ID, string ddlGLGroup, string GLID, string GRPID, string GLAct, string GLAcctype,string Flag);
        DataTable BindGetGLGroupList(string GroupName, string CompID);
        DataTable GetGLListDAL(string CompID);
        Dictionary<string, string> GLSetupGroupDAL(string GroupName, string CompID);
        Dictionary<string, string> AccGrpListGroupDAL(string GroupName, string CompID);
        DataSet GetGLListFilterDAL(string CompID, string GLID, string GRPID, string GLAct, string GLAcctype);

        //added By Nitesh (GetGLSetup_data) 03-11-2023 for get Opning and closing Balence and Currency
        DataSet GetGLSetup_data(string CompID, string BranchID, string acc_id, string acc_grpid, string acc_type);
        DataSet GetMasterDataForExcelFormat(string compId);
        DataSet GetVerifiedDataOfExcel(string CompID, DataTable GLDetail, DataTable GLBranch);
        DataTable ShowExcelErrorDetail(string CompID, DataTable GLDetail, DataTable GLBranch);
        string BulkImportGLDetail(string CompID,string BranchName,string Userid, DataTable GLDetail, DataTable GLBranch);
    }
}
