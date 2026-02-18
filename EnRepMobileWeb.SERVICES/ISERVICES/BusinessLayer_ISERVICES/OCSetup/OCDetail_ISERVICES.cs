using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.OCDetail;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
    public interface OCDetail_ISERVICES
    {
        DataTable GetBrListDAL(string CompID);
        DataSet GetBrList(string CompID);
        DataSet GetallData(string CompID);
        DataTable GetThirdPartyDAL(string CompID);
        DataTable GetOCcoaDAL(string CompID);
        DataSet GetviewOCdetailDAL(string OCcode, string CompID);
        //DataSet InsertBrDetailDAL(string Comp_ID, string OCcode, string BrID, string Flag, string TransType);
        //string InsertOCDetailDAL(OCDetailModel _OCDetailModel, string CompID, string UserID);
        string insertOCDetail(DataTable OCDetail, DataTable OCBranch);
    }
}
