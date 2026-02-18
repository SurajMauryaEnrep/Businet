using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.JobOrder
{
   public interface JobOrder_ISERVICES
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId);

        DataSet GetAllData(string CompId, string BrchID, string SuppName, JOListModel _JOListModel, string UserID,string DocumentMenuId);
        DataSet GetProducORDDocList(string Comp_ID, string Br_ID);
        DataTable GetItemUOM(string CompId, string Item_id);// Bind UOM

        DataSet GetDetailsAgainstProducOrdNo(string Comp_ID, string Br_ID, string ProductionOrd_no, string ProductionOrd_date);
        string InsertJO_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblOutputItemDetail, DataTable DtblInputItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail, DataTable DTDeliSchDetail, DataTable DTTermsDetail, DataTable dtSubItem, DataTable DTAttachmentDetail);
        DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName);//Bind product name
        //DataSet GetJODetailList(string CompId, string BrchID);
        //DataSet GetJOSrchDetail(string CompID, string BrID, string SuppId, string ProdctID, string Fromdate, string Todate, string Status);
        DataSet GetJODetailEditUpdate(string CompId, string BrchID, string JOSC_NO, string JOSC_Date, string UserID, string DocID);
        string JO_DeleteDetail(JobOrderModel _JobOrderModel, string CompId, string BrID);
        DataSet GetJOListandSrchDetail(string CompId, string BrchID, JOListModel _JOListModel, string UserID, string wfstatus, string DocumentMenuId);
        string JOApproveDetails(string CompID, string BrchID, string JONo, string JODate, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks);

        DataSet CheckMtrialDisptchDagainstJobOrdr(string CompID, string BrID, string JONo, string JODate);
        DataSet JO_GetSubItemDetails(string CompID, string br_id, string Item_id, string jc_no, string jc_dt, string Flag, string Status, string JobOrdNo, string JobOrdDt);

        DataSet GetJobOrderDeatils(string CompID, string BrchID, string OrderNo, string OrderDate);
        DataSet GetJOTrackingDetail(string CompId, string BrID, string JONo, string JODate);

    }

}
