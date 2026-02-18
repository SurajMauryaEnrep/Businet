using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.JournalBook
{
   public interface JournalBook_ISERVICE
    {
        DataSet Get_FYList(string Compid, string Brid);
        Dictionary<string, string> AccGrpListGroupDAL(string GroupName, string CompID);
        //DataSet GetGLAccountList(string Comp_ID, string Br_ID, string  GLGroupId);
        Dictionary<string, string> GetGLAccountList(string Comp_ID, string Br_ID, string  GLGroupId, string GroupName);
        Dictionary<string, string> GLSetupGroupDAL(string GroupName, string CompID);
        DataSet GetAllDDLDetails(string CompID, string BrID, string UserID, string DocumentMenuId, string language);
        DataTable GetJournalBookDetailsMIS(string CompID, string BrID, string FromDate, string ToDate, string GroupId, string AccountID, string AmtFrom,
            string AmtTo, string VouTyp, string CreatBy, string CreatOn, string AppBy, string AppOn, string Narr, string Status);
        DataSet GetCostCenterData(string Comp_ID, string Br_ID, string Vou_No, string Vou_Dt, string GLAcc_id);

    }
}
