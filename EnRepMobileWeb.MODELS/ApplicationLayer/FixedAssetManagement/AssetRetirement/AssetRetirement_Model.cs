using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRetirement
{
    public class AssetRetirement_Model
    {
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
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
        public string AssetRegId { get; set; }
        public List<AssetItemsLists> AssetItems { get; set; }
        public List<AssetSerialNoLists> AssetSerialNo { get; set; }
        public string AssetItemsId { get; set; }
        public string AssetDesDetails { get; set; }
        public string SerialNumberDetails { get; set; }
        public string GroupDetails { get; set; }
        public string CategoryDetails { get; set; }
        public string AssetLabel { get; set; }
        public string SerialNumber { get; set; }
        public List<AssetsGroup> AssetsGroupList { get; set; }
        public string AssetsGroupId { get; set; }
        public string AssetCategory { get; set; }
        public string CurrentValue { get; set; }
        public string ProcuredValue { get; set; }
        public string AsOn { get; set; }
        public string AssetLife { get; set; }
        public string AssetWorkingDate { get; set; }
        public string DepreciationStartDate { get; set; }
        public string AssignedRequirementArea { get; set; }
        public string AccumulatedDepreciation { get; set; }
        public string CompId { get; set; }
        public string BrdId { get; set; }
        public string CurrecyID { get; set; }
        public int? Mod_id { get; set; }
        public string Doc_no { get; set; }
        public string Doc_date { get; set; }
        public string VouGlDetails { get; set; }
        public string GLVoucherType { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherDt { get; set; }
        public string CC_DetailList { get; set; }
        public string Headerdetails { get; set; }
        public string ScrapValue { get; set; }
        public string RetDate { get; set; }
        public string Remarks { get; set; }
        public string Narration { get; set; }
        public string curr_id { get; set; }
        public string conv_rate { get; set; }
        public string asset_coa { get; set; }
        public string AssetAccount { get; set; }
        public string dep_coa { get; set; }
        public string DepreciationAccount { get; set; }
    }
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Asset_reg_Id { get; set; }
        public string ListFilterData { get; set; }
        public string Doc_No { get; set; }
        public string Doc_date { get; set; }
    }
    public class AssetRetirementList_Model
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
    public class GL_Detail
    {
        public string comp_id { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string doctype { get; set; }
        public string Value { get; set; }
        public string DrAmt { get; set; }
        public string CrAmt { get; set; }
        public string Transtype { get; set; }
        public string gl_type { get; set; }
    }
}
