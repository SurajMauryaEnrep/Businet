using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetTransfer
{
    public class AssetTransfer_Model
    {
        public string CompId { get; set; }
        public string BrdId { get; set; }
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string WFStatus { get; set; }
        public string WFBarStatus { get; set; }
        public string Headerdetails { get; set; }
        public string Itemdetails { get; set; }
        public string StatusName { get; set; }
        public string DocumentStatus { get; set; }
        public string TransType { get; set; }
        public string FormMode { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string TransferDate { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string ListFilterData { get; set; }
        public string DeleteCommand { get; set; }
        public string Attatchmentdetail { get; set; }
        public string Remarks { get; set; }
        public List<AssetItemsLists> AssetItems { get; set; }
        public string AssetItemsId { get; set; }
        public string AssetLabel { get; set; }
        public List<AssetSerialNoLists> AssetSerialNo { get; set; }
        public string SerialNumber { get; set; }
        public List<AssignedRequirementArea> AssignedRequirementAreaList { get; set; }
        public string AssignedRequirementAreaId { get; set; }
        public string AssignedRequirementAreaType { get; set; }
        public List<DestinationAssignedRequirementArea> DestinationAssignedRequirementAreaList { get; set; }
        public string DestinationAssignedRequirementAreaId { get; set; }
        public string DestinationAssignedRequirementAreaType { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public string CurrentYearFirstDate { get; set; }
        public int? Mod_id { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
    }
    public class AssetTransferattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Doc_No { get; set; }
        public string Doc_date { get; set; }
        public string ListFilterData { get; set; }
    }
    public class AssetTransferList_Model
    {
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AT_ToDate { get; set; }
        public string AT_FromDate { get; set; }
        public string AT_status { get; set; }
        public string WF_status { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string PQASearch { get; set; }
        public string FinStDt { get; set; }
        public string Message { get; set; }
        public string DocumentMenuId { get; set; }
        public List<StatusLists> StatusLists { get; set; }
        public string StatusId { get; set; }
        public List<AssignedRequirementArea> AssignedRequirementAreaList { get; set; }
        public string AssignedRequirementAreaId { get; set; }
        public string AssignedRequirementAreaType { get; set; }
        public List<AssetItemsLists> AssetItems { get; set; }
        public string AssetItemsId { get; set; }
    }
    public class StatusLists
    {
        public string Status_id { get; set; }
        public string Status_name { get; set; }
    }
   
   
    public class AssetItemsLists
    {
        public string AssetItem_id { get; set; }
        public string AssetItem_name { get; set; }
    }
    public class AssetSerialNoLists
    {
        public string SerialNo_id { get; set; }
        public string SerialNo_name { get; set; }
    }
    public class AssignedRequirementArea
    {
        public string AssignedRequirementArea_id { get; set; }
        public string AssignedRequirementArea_name { get; set; }
    }
    public class DestinationAssignedRequirementArea
    {
        public string DestinationAssignedRequirementArea_id { get; set; }
        public string DestinationAssignedRequirementArea_name { get; set; }
    }
}
