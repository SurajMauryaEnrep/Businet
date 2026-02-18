using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.DepreciationProcess
{
    public class DepreciationProcess_Model
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
        public string WFStatus { get; set; }
        public string WFBarStatus { get; set; }
        public string Delete { get; set; }
        public string title { get; set; }
        public string DocumentMenuId { get; set; }
        public string ListFilterData { get; set; }
        public string DeleteCommand { get; set; }
        public string attatchmentdetail { get; set; }
        public string Doc_no { get; set; }
        public string Doc_date { get; set; }
        public string AssetLabel { get; set; }
        public string SerialNumber { get; set; }
        public List<AssetsGroup> AssetsGroupList { get; set; }
        public string AssetsGroupId { get; set; }
        public string AssetsGroupName { get; set; }
        public string Depreciationfreq { get; set; }
        public string DepreciationfreqId { get; set; }
        public string ValidUpto { get; set; }
        public int? Mod_id { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string ddl_f_frequency { get; set; }
        public List<f_frequency> ddl_f_frequencyList { get; set; }
        public string ddl_financial_year { get; set; }
        public List<financial_year> ddl_financial_yearList { get; set; }
        public string ddl_period { get; set; }
        public List<period> ddl_periodList { get; set; }
        public string txtFromDate { get; set; }
        public string txtToDate { get; set; }
        public string Itemdetails { get; set; }
        public string Headerdetails { get; set; }
        public bool CancelFlag { get; set; }
        public string CancelledRemarks { get; set; }
        public string GLVoucherType { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherDt { get; set; }
        public string VouGlDetails { get; set; }
        public string Narration { get; set; }
        public string curr_id { get; set; }
        public string conv_rate { get; set; }
        public string asset_coa { get; set; }
        public string AssetAccount { get; set; }
        public string dep_coa { get; set; }
        public string DepreciationAccount { get; set; }
        public string CC_DetailList { get; set; }
    }
    public class period
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class f_frequency
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class financial_year
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class DepreciationProcessattch
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
        public string Doc_No { get; set; }
        public string Doc_date { get; set; }
        public string ListFilterData { get; set; }
    }
    public class DepreciationProcessList_Model
    {
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string title { get; set; }
        public string DP_status { get; set; }
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
        public string ddl_periodId { get; set; }
        public List<period> ddl_periodList { get; set; }
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
    public class DPtemDetail
    {
        public string AssetRegId { get; set; }
        public string AssetId { get; set; }
        public string AssetName { get; set; }
        public string serial_no { get; set; }
        public string asset_label { get; set; }
        public string curr_val { get; set; }
        public string DepreciationMethod { get; set; }
        public string dep_per { get; set; }
        public string dep_val { get; set; }
        public string add_dep_per { get; set; }
        public string add_dep_val { get; set; }
        public string RevisedVal { get; set; }
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
