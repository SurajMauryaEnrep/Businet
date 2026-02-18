using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.MIS.MISAssetRegistration
{
    public class MISAssetRegistration_Model
    {
        public string TransType { get; set; }
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string ListFilterData { get; set; }
        public string DeleteCommand { get; set; }
        public string AssetRegId { get; set; }
        public List<AssetsGroup> AssetsGroupList { get; set; }
        public string AssetsGroupId { get; set; }
        public string AssetCategory { get; set; }
        public string AssetCategoryId { get; set; }
        public List<AssetsGroupCat> AssetsGroupCatList { get; set; }
        public string AssetsGroupCatId { get; set; }
        public List<WorkingStatus> WorkingStatusList { get; set; }
        public string WorkingStatusId { get; set; }
        public List<AssignedRequirementArea> AssignedRequirementAreaList { get; set; }
        public string AssignedRequirementAreaId { get; set; }
        public string AssignedRequirementAreaType { get; set; }
        public string CompId { get; set; }
        public string BrdId { get; set; }
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
    public class AssignedRequirementArea
    {
        public string AssignedRequirementArea_id { get; set; }
        public string AssignedRequirementArea_name { get; set; }
    }
    public class WorkingStatus
    {
        public string WorkingStatus_id { get; set; }
        public string WorkingStatus_name { get; set; }
    }
}
