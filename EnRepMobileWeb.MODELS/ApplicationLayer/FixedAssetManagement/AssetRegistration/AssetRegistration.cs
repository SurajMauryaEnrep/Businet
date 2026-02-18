using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRegistration
{
    public class AssetRegistration_Model
    {
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string StatusName { get; set; }
        public string DocumentStatus { get; set; }
        public string TransType { get; set; }
        public string FormMode { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        
        public string title { get; set; }
        public string DocumentMenuId { get; set; }
        public string ListFilterData { get; set; }
        public string DeleteCommand { get; set; }
        public string attatchmentdetail { get; set; }
        public string AssetRegId { get; set; }
        public List<AssetItemsLists> AssetItems { get; set; }
        public List<AssetSerialNoLists> AssetSerialNo { get; set; }
        public string AssetItemsId { get; set; }
        public string AssetLabel { get; set; }
        public string SerialNumber { get; set; }
        public string SerialNumber1 { get; set; }
        public List<AssetsGroup> AssetsGroupList { get; set; }
        public string AssetsGroupId { get; set; }
        public string AssetCategory { get; set; }
        public string AssetCategoryId { get; set; }
        public string ProcurementDate { get; set; }
        public string SupplierName { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
        public string ProcuredValue { get; set; }
        public string CurrentValue { get; set; }
        public string AsOn { get; set; }
        public string AssetLife { get; set; }
        public string AssetLifeLabel { get; set; }
        public string DepreciationPer { get; set; }
        public string Depreciationfreq { get; set; }
        public string DepreciationMethod { get; set; }
        public string AddDepreciationPer { get; set; } = "0";

        public string ValidUpto { get; set; }
        public string AssetWorkingDate { get; set; }
        public string DepreciationStartDate { get; set; }
        public List<AssignedRequirementArea> AssignedRequirementAreaList { get; set; }
        public string AssignedRequirementAreaId { get; set; }
        public string AssignedRequirementAreaType { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public List<WorkingStatus> WorkingStatusList { get; set; }
        public string WorkingStatusId { get; set; }
        public string CompId { get; set; }
        public string BrdId { get; set; }
        public string CurrentYearFirstDate { get; set; }
        public string CurrecyID { get; set; }

        public int? Mod_id { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public ProcurementDetail_Model ProcurementDetail { get; set; }
        public string PD_DetailList { get; set; }
    }
    public class AssetRegistrationattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Asset_reg_Id { get; set; }
        public string ListFilterData1 { get; set; }
    }
    public class ARList_Model
    {
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string title { get; set; }
        public string AR_status { get; set; }
        public string WF_status { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string PQASearch { get; set; }
        public string FinStDt { get; set; }
        public string Message { get; set; }
        public string DocumentMenuId { get; set; }
        public List<statusLists> statusLists { get; set; }
        public string statusId { get; set; }
        public List<AssetsGroup> AssetsGroupList { get; set; }
        public string AssetsGroupId { get; set; }
        public List<AssetsGroupCat> AssetsGroupCatList { get; set; }
        public string AssetsGroupCatId { get; set; }
        public List<RequirementArea> RequirementAreaList { get; set; }
        public string RequirementAreaId { get; set; }
        public List<WorkingStatus> WorkingStatusList { get; set; }
        public string WorkingStatusId { get; set; }
    }
    public class statusLists
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class AssetsGroup
    {
        public string assetgrp_id { get; set; }
        public string assetgrp_name { get; set; }
    }
    public class AssetsGroupCat
    {
        public string assetgrpcat_id { get; set; }
        public string assetgrpcat_name { get; set; }
    }
    public class RequirementArea
    {
        public string RequirementArea_id { get; set; }
        public string RequirementArea_name { get; set; }
    }
    public class WorkingStatus
    {
        public string WorkingStatus_id { get; set; }
        public string WorkingStatus_name { get; set; }
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

    public class ProcurementDetail_Model
    {
        public string PDAssetItemsId { get; set; }
        public string PDAssetLabel { get; set; }
        public string PDSerialNumber { get; set; }
        public string PDSource { get; set; }
        public string PDBillNumber { get; set; }
        public string PDBillDate { get; set; }
        public string PDSupplierName { get; set; }
        public string GSTNo { get; set; }
        public string CurrencyId { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public string PurchasePrice { get; set; }
        public string TaxAmount { get; set; }
        public string OtherCharges { get; set; }
        public string TotalCost { get; set; }
        public string CapitalizedValue { get; set; }
        public string DisableFlag { get; set; }

    }
    public class Currency
    {
        public string curr_id { get; set; }
        public string curr_val { get; set; }

    }
}
