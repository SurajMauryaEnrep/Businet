using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.GLDetail;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
    public interface GLDetail_ISERVICES
    {
        DataSet GetBrList(string CompID);
        DataTable GetBrListDAL(string CompID);
        //Dictionary<string, string> AccGrpListGroupDAL(string GroupName, string CompID);
        //string InsertGLDetailDAL(GLDetailModel _GLDetail, string compid, string userid);
        //DataSet InsertBrDetailDAL(string Comp_ID, string GLcode, string GLBrID, string Flag, string TransType);
        string insertGLDetail(DataTable GLDetail, DataTable GLBranch);
        string GLDetailDelete(string GLCode, string CompID);
        DataSet GetviewGLdetailDAL(string GLCode, string CompID);
        DataTable GetAccountGroupDAL(string CompID);
        DataTable GetAccountGroupList(string CompID, string acc_id);
        DataTable GetCurr(string CompID);
    }
}
