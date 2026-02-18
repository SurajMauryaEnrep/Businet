using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
   public interface ItemList_ISERVICES
    {
        DataTable BindGetItemList(string GroupName, string CompID, string BrchID);
        DataSet BindGetAllDropDownList(string GroupName, string Comp_ID, string BrchID, string Item_ID,string Itemportfolio,string AttributeName,string User_ID);
        DataSet GetItemsAvailableStock(string compId, string brId, string itemId);
      //  DataTable BindGetGroupList(string GroupName, string CompID);
     //   DataTable BindGetPortfNameList(string GroupName, string CompID);
       // DataTable BindAttributeNameList(string CompID);
        DataTable BindGetAttributeValue(string GroupName, string CompID);
        DataSet GetItemListDAL(string CompID, string UserID);
        DataSet GetItemListFilterDAL(string CompID, string ItmID, string ItmGrpID, string AttrName, string AttrValue, string ActStatus, string ItmStatus, string ItemPrfID, string Imagefilter);
        Dictionary<string, string> ItemGroupListDAL(string GroupName, string CompID);
        Dictionary<string, string> ItemPortfolioList(string GroupName, string CompID);
        Dictionary<string, string> ItemSetupGroupDAL(string GroupName, string CompID,string BrchID);
        DataSet GetAttributeListDAL(string CompID);
        DataSet GetAttributeValueDAL(string CompID, string AttributeID);
        DataSet GetItemImageListDAL(string CompID, string ItmCode);
        DataSet GetItem_OrdersDetails(string CompID, string ItmCode);

        //Import Items From Excel------ Code wrote by sanjay prasad//
        DataSet GetMasterDataForExcelFormat(string compId);
        string BulkImportItemsDetail(string compId, string brId, DataTable dtItems, DataTable dtSubItems, DataTable dtItemAttributes, DataTable dtItemBranch, DataTable dtItemPortfolio, string BranchName, string createdBy);
        DataSet GetVerifiedDataOfExcel(string compId, DataTable dtItems, DataTable dtSubItems, DataTable dtItemAttributes, DataTable dtItemBranch, DataTable dtItemPortfolio);
        DataTable ShowExcelErrorDetail(string compId, DataTable dtItems, DataTable dtSubItems, DataTable dtItemAttributes, DataTable dtItemBranch, DataTable dtItemPortfolio);
        //Import Items From Excel END//
    }
}
