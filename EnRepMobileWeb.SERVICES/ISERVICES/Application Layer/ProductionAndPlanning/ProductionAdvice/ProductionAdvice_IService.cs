using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionAdvice
{
    public interface ProductionAdvice_IService
    {
        DataTable Bind_ProductList1(string CompID, string BrID, string SrcType, string ffy, string Period, string SearchName);
        DataSet Bind_ProductList(string CompID, string BrID, string SrcType, string ffy, string Period, string SearchName);
        DataSet BindFinancialYear(string CompID, string BrID);
        DataSet Bind_PeriodList(string CompID, string BrID, string ffy);
        DataSet Bind_PeriodRAndProductList(string CompID, string BrID, string ffy, string Period);
        DataSet Bind_RevisionNoList(string CompID, string BrID, string productid);
        DataSet Get_MaterialDetail(string CompID, string BrID, string productid, string revno);
        string InsertUpdateProductionAdvice_Details(DataTable PAdviceHeader, DataTable PAdviceItemDetails
            , DataTable ProdAdvAttachments,DataTable dtSubItem);
        string Cancelled_ProductionAdviceDetail(string comp_id, string br_id, string adv_no, string adv_date, string userid, string mac_id, string MenuDocid);
        DataSet GetAdviceDetailByNo(string CompID, string BrchID, string Adv_no, string Adv_Date, string UserID, string DocumentMenuId);
        DataTable GetAdviceList(string ProductId, string Source, string Fromdate, string Todate, string Status, string CompID, string BrchID, string UserID, string DocumentMenuId,string wfstatus);
        DataSet Delete_PAdviceDetails(string comp_id, string br_id, string adviceno, string advicedt);
        string Approved_PAdviceDetails(string comp_id, string br_id, string user_id, string adviceno, string advicedt, string DocumentMenuID, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet GetProdAdv_Detail(string CompId, string BrID, string AdvNo, string AdvDate);
        DataSet ADV_GetSubItemDetails(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag);



    }
}
