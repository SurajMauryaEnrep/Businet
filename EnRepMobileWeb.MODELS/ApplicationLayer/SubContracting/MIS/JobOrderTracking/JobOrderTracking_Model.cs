using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MIS.JobOrderTracking
{
   public class JobOrderTracking_Model
    {
		public string Title { get; set; }
		public string JOTrackSearch { get; set; }
		public string JO_ItemName { get; set; }
		public string Supp_Name { get; set; }
		public string Supp_ID { get; set; }
		public List<SupplierName> SupplierNameList { get; set; }
		public string FinishProduct_Name { get; set; }
		public string FinishProduct_Id { get; set; }
		public string Finish_Uom { get; set; }
		public string Finish_UomId { get; set; }

		public string Operation_Name { get; set; }
		public string Op_Id { get; set; }

		public string OpOut_ItemName { get; set; }
		public string OpOut_ItemId { get; set; }
		public string Status { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public List<Status> StatusList { get; set; }
		//public List<ItemName> ItemNameList { get; set; }
		//public List<ItemGroupName> ItemGroupNameList { get; set; }

		//public List<ItemPortfolio> ItemPortfolioList { get; set; }
		//public List<Branch> BranchList { get; set; }
		//public List<WarehouseName> WarehouseNameList { get; set; }
		public List<JobOrderTrackListDetail> JobOrderTrackList { get; set; }
		//public List<HsnCodeModel> HsnCodeList { get; set; }
		public string JTFilter { get; set; }
	}
	public class SupplierName
	{
		public string supp_id { get; set; }
		public string supp_name { get; set; }
	}
	public class Status
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}

	public class JobOrderTrackListDetail
	{
		public string OrderNo { get; set; }
		public string OrderDate { get; set; }
		public string OrderDt { get; set; }
		public string SupplierName { get; set; }
		public string SupplierId { get; set; }
		public string FinishProductName { get; set; }
		public string FinishProductId { get; set; }
		public string FinishProducUOM { get; set; }
		public string FinishProducUOMId { get; set; }
		public string Branch { get; set; }
		public string BranchID { get; set; }
		public string OperationName { get; set; }
		public string OperationId { get; set; }
		public string OpOutProductName { get; set; }
		public string OpOutProductID { get; set; }
		public string OpOutUOM { get; set; }
		public string OpOutUOMId { get; set; }
		public string OrderQuantity { get; set; }
		public string DispatchQuantity { get; set; }
		public string ReceviedQuantity { get; set; }
		public string AcceptedQuantity { get; set; }
		public string RejectedQuantity { get; set; }
		public string ReworkableQuantity { get; set; }
		public string Status { get; set; }
		public string StatusName { get; set; }
		public string StatusId { get; set; }
	}
	//public class JobOrderTrackDataModel
	//{
	//	//public string ILSearch { get; set; }
	//	//public string IL_SSearch { get; set; }
	//	public Int64 SrNo { get; set; }
	//	public string compid { get; set; }
	//	public string brid { get; set; }
	//	public string brname { get; set; }
	//	//public string itemid { get; set; }
	//	//[DisplayName("Item Name")]
	//	//public string itemname { get; set; }
	//	public string OrdNo { get; set; }
	//	public string OrdDate { get; set; }
	//	public string OrdDt { get; set; }
	//	public string SuppID { get; set; }
	//	public string SuppName { get; set; }
	//	public string FPrductName { get; set; }
	//	public string FPrductID { get; set; }
	//	public string FPrductUOM { get; set; }
	//	public string FPrductUOMId { get; set; }
	//	public string Branch { get; set; }
	//	public string BranchID { get; set; }
	//	public string OpName { get; set; }
	//	public string OPId { get; set; }
	//	public string OpOutPrdctName { get; set; }
	//	public string OpOutPrdctID { get; set; }
	//	public string OpOutPrdctUOM { get; set; }
	//	public string OpOutPrdctUOMId { get; set; }
	//	public string OrdQty { get; set; }
	//	public string DispQty { get; set; }
	//	public string RecevQty { get; set; }
	//	public string AcceptQty { get; set; }
	//	public string RejectQty { get; set; }
	//	public string ReworkQty { get; set; }
	//	public string Status { get; set; }
	//	public string Status_Name { get; set; }
	//	public string Status_Id { get; set; }
	//}
}
