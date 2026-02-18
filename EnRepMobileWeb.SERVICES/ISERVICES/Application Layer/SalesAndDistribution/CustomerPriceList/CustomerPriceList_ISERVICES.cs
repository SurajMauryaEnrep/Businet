using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomerPriceList;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.CustomerPriceList
{
   public interface CustomerPriceList_ISERVICES
    {
        DataTable GetCustPriceGrpDAL(string CompID);
        String InsertPriceListDetail(DataTable PriceListHeader, DataTable PriceListItemDetail, DataTable PriceListPriceGroup);
        DataSet GetviewPriceListdetail(string ListNo, string BrchID, string Comp_ID, string userid, string DocumentMenuId);
        DataTable GetPriceList(string CompId, string BrchID, string userid, string wfstatus, string DocumentMenuId, string Act_Status);
        DataSet PriceListDetailDelete(PriceListDetailModel _PriceListModel, string comp_id, string BrchID, string DocumentMenuId);
        DataSet PriceListApprove(PriceListDetailModel _PriceListModel, string comp_id, string BrchID,/* int list_no, */string mac_id, string DocumentMenuId, string app_id);
        DataTable GetCustPriceHistryDtl(string Comp_ID, string Br_ID, string Doc_no, string Doc_dt, string Item_id/*,string hd_Status*/);
        DataSet GetMasterDropDownList(string CompID, string Br_Id);
        DataTable GetPriceListName(string CompID, string Br_Id);
        DataTable GetPriceListItemDetail(string CompID, string Br_Id, string list_no);
    }
    
}
