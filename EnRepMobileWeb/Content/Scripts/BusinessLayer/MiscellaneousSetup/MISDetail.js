
$(document).ready(function () {
    //debugger
    //$("#datatable-buttons tbody tr,#datatable-buttons1 tbody tr,#datatable-buttons2 tbody tr,#datatable-buttons3 tbody tr,#datatable-buttons4 tbody tr").on("click",
    //    function (event) {
    //        $("#datatable-buttons tbody tr,#datatable-buttons1 tbody tr,#datatable-buttons2 tbody tr,#datatable-buttons3 tbody tr,#datatable-buttons4 tbody tr").css(
    //            "background-color", "#ffffff");
    //    $(this).css("background-color", "rgba(38, 185, 154, .16)");
    //});
    //$("#datatable-buttons5 tbody tr,#datatable-buttons7 tbody tr,#datatable-buttons8 tbody tr,#datatable-buttons9 tbody tr,#datatable-buttons10 tbody tr").on("click", function (event) {
    //    $("#datatable-buttons5 tbody tr,#datatable-buttons7 tbody tr,#datatable-buttons8 tbody tr,#datatable-buttons9 tbody tr,#datatable-buttons10 tbody tr").css("background-color", "#ffffff");
    //    $(this).css("background-color", "rgba(38, 185, 154, .16)");
    //});
    $(".button000 input:nth-child(2)").remove();
    ReadBranchList();
    /*$("#datatable-buttons11").datatable();*/
    $("#country").select2();
    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");
});


//---------------------------------------Bin Number----------------------------------------//
function EditBINNumber(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();

    var SetupId = CurrentRow.find("#setupidBIN").text();
    var SetupVal = CurrentRow.find("#setupvalBIN").text();
    $("#BINNumber").val(SetupVal).trigger('change');
    $("#BINNumber_hdnId").val(SetupId);//btn_saveBIN
    $("#btn_saveBIN").val("UpdateBin");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableBINSetup").css("display", "block");
        $("#BINNumber").attr("disabled", false);
    }
}
function BinSaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_saveBIN").css("filter", "grayscale(100%)");
        $("#btn_saveBIN").prop("disabled", true); /*End*/
        return false;
    }

    if (CheckVallidation("BINNumber", "vmBINNumber") == false) {
        return false;
    } else {
        $("#btn_saveBIN").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");

        return true;
    }
}
function OnChangeBinNumber() {
    CheckVallidation("BINNumber", "vmBINNumber");
}


//---------------------------------------Bin Number End----------------------------------------//

//---------------------------------------Item PortFolio----------------------------------------//

function EditItemPortFolio(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidItmPF").text();
    var SetupVal = CurrentRow.find("#setupvalItmPF").text();
    $("#PortfolioName").val(SetupVal).trigger('change');
    $("#PortfolioName_hdnId").val(SetupId);//btn_saveBIN
    $("#btn_saveItemPort").val("UpdateItemPort");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableItemPortfolio").css("display", "block");
        $("#PortfolioName").attr("disabled", false);
    }
}
function ItemPortSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickport") {
        $("#btn_saveItemPort").css("filter", "grayscale(100%)");
        $("#btn_saveItemPort").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("PortfolioName", "vmPortfolioName") == false) {
        return false;
    } else {
        $("#btn_saveItemPort").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickport");
        return true;
    }
}
function OnChangeItemPort() {
    CheckVallidation("PortfolioName", "vmPortfolioName");
}

//---------------------------------------Item PortFolio End----------------------------------------//

//---------------------------------------Customer PortFolio----------------------------------------//

function EditCustomerPortFolio(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidCustPF").text();
    var SetupVal = CurrentRow.find("#setupvalCustPF").text();
    $("#CustPortfolioName").val(SetupVal).trigger('change');
    $("#CustPortfolioName_hdnId").val(SetupId);
    $("#btn_saveCustPort").val("UpdateCustPort");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableCustomerPortfolio").css("display", "block");
        $("#CustPortfolioName").attr("disabled", false);
    }
}
function CustomerPortSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickCustport") {
        $("#btn_saveCustPort").css("filter", "grayscale(100%)");
        $("#btn_saveCustPort").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("CustPortfolioName", "vmCustPortfolioName") == false) {
        return false;
    } else {
        $("#btn_saveCustPort").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickCustport");

        return true;
    }
}
function OnChangeCustomerPort() {
    CheckVallidation("CustPortfolioName", "vmCustPortfolioName");
}

//---------------------------------------Customer PortFolio End----------------------------------------//

//---------------------------------------Supplier PortFolio----------------------------------------//

function EditSuppPortFolio(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidSuppPF").text();
    var SetupVal = CurrentRow.find("#setupvalSuppPF").text();
    $("#SuppPortfolioName").val(SetupVal).trigger('change');
    $("#SuppPortfolio_hdnId").val(SetupId);
    $("#btn_saveSuppPort").val("UpdateSuppPort");

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableSupplierPortfolio").css("display", "block");
        $("#SuppPortfolioName").attr("disabled", false);
    }
}
function SupplierPortSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickSuppport") {
        $("#btn_saveSuppPort").css("filter", "grayscale(100%)");
        $("#btn_saveSuppPort").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("SuppPortfolioName", "vmSuppPortfolioName") == false) {
        return false;
    } else {
        $("#btn_saveSuppPort").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSuppport");
        return true;
    }
}
function OnChangeSupplierPort() {
    CheckVallidation("SuppPortfolioName", "vmSuppPortfolioName");
}

//---------------------------------------Supplier PortFolio End----------------------------------------//

//---------------------------------------Customer Category ----------------------------------------//

function EditCustCategory(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidCustCate").text();
    var SetupVal = CurrentRow.find("#setupvalCustCate").text();
    $("#CustCategoryName").val(SetupVal).trigger('change');
    $("#CustCategory_hdnId").val(SetupId);
    $("#btn_saveCustCat").val("UpdateCustCat");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableCustomerCategory").css("display", "block");
        $("#CustCategoryName").attr("disabled", false);
    }
}
function CustCategorySaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickCustCat") {
        $("#btn_saveCustCat").css("filter", "grayscale(100%)");
        $("#btn_saveCustCat").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("CustCategoryName", "vmCustCategoryName") == false) {
        return false;
    } else {
        $("#btn_saveCustCat").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickCustCat");
        return true;
    }
}
function OnChangeCustCategory() {
    CheckVallidation("CustCategoryName", "vmCustCategoryName");
}

//---------------------------------------Customer Category End----------------------------------------//

//---------------------------------------Supplier Category----------------------------------------//

function EditSuppCategory(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidSuppCate").text();
    var SetupVal = CurrentRow.find("#setupvalSuppCate").text();
    $("#SupplierCategory").val(SetupVal).trigger('change');
    $("#SupplierCategory_hdnId").val(SetupId);
    $("#btn_saveSuppCat").val("UpdateSuppCat");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableSupplierCategory").css("display", "block");
        $("#SupplierCategory").attr("disabled", false);
    }
}
function SuppCategorySaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickSuppCat") {
        $("#btn_saveSuppCat").css("filter", "grayscale(100%)");
        $("#btn_saveSuppCat").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("SupplierCategory", "vmSupplierCategory") == false) {
        return false;
    } else {
        $("#btn_saveSuppCat").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSuppCat");
        return true;
    }
}
function OnChangeSupplierCategory() {
    CheckVallidation("SupplierCategory", "vmSupplierCategory");
}

//---------------------------------------Supplier Category End----------------------------------------//

//---------------------------------------Sales Region ----------------------------------------//

function EditSalesRegion(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidSalRegn").text();
    var SetupVal = CurrentRow.find("#setupvalSalRegn").text();
    $("#SalesRegionName").val(SetupVal).trigger('change');
    $("#SalesRegion_hdnId").val(SetupId);
    $("#btn_saveSalesReg").val("UpdateSalesReg");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableSalesRegion").css("display", "block");
        $("#SalesRegionName").attr("disabled", false);
    }
}
function SalesRegSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickSalereg") {
        $("#btn_saveSalesReg").css("filter", "grayscale(100%)");
        $("#btn_saveSalesReg").prop("disabled", true);
        /*End*/
        return false;
    }


    if (CheckVallidation("SalesRegionName", "vmSalesRegionName") == false) {
        return false;
    } else {
        $("#btn_saveSalesReg").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSalereg");
        return true;
    }
}
function OnChangeSalesRegion() {
    CheckVallidation("SalesRegionName", "vmSalesRegionName");
}

//---------------------------------------Sales Region End----------------------------------------//

//---------------------------------------Requirment Area----------------------------------------//

function EditRequirmentArea(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidReqArea").text();
    var SetupVal = CurrentRow.find("#setupvalReqArea").text();
    var Hdn_ShopFloor = CurrentRow.find("#Hdn_ShopFloor").text();
    $("#RequirementAreaName").val(SetupVal).trigger('change');
    $("#RequirementArea_hdnId").val(SetupId);
    debugger;
    if (Hdn_ShopFloor == "Y") {
        $("#ShopFloorSetup").prop("checked", true);
    }
    else {
        $("#ShopFloorSetup").prop("checked", false);
    }
    $("#ShopFloorSetup").attr("disabled", true);
    //$("#ShopFloorSetup").css("filter: grayscale(100%)");
    $("#btn_saveReqArea").val("UpdateReqArea");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableRequirementArea").css("display", "block");
        $("#RequirementAreaName").attr("disabled", false);
        //$("#ShopFloorSetup").attr("disabled", false);
    }
}

function ReqAreaSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickReq_area") {
        $("#btn_saveReqArea").css("filter", "grayscale(100%)");
        $("#btn_saveReqArea").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("RequirementAreaName", "vmRequirementAreaName") == false) {
        return false;
    } else {
        $("#ShopFloorSetup").attr("disabled", false);

        $("#btn_saveReqArea").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickReq_area");

        return true;
    }
}
function OnChangeReqArea() {
    CheckVallidation("RequirementAreaName", "vmRequirementAreaName");
}

//---------------------------------------Requirment Area End----------------------------------------//

//---------------------------------------VehicleSetup----------------------------------------//

function EditVehicleName(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidVehc").text();
    var SetupVal = CurrentRow.find("#setupvalVehc").text();
    $("#VehicleName").val(SetupVal).trigger('change');
    $("#VehicleName_hdnId").val(SetupId);
    $("#btn_saveVehicleName").val("UpdateVehicleName");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableVehicleSetup").css("display", "block");
        $("#VehicleName").attr("disabled", false);
    }
}

function VehicleNameSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickVechi_Name") {
        $("#btn_saveVehicleName").css("filter", "grayscale(100%)");
        $("#btn_saveVehicleName").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("VehicleName", "vmVehicleName") == false) {
        return false;
    } else {
        $("#btn_saveVehicleName").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickVechi_Name");
        return true;
    }
}
function OnChangeVehicleName() {
    CheckVallidation("VehicleName", "vmVehicleName");
}

//---------------------------------------VehicleSetup End----------------------------------------//

//---------------------------------------Costomer Group Setup----------------------------------------//

function EditGroupName(e) {
    var CurrentRow = $(e.target).closest('tr');

    //var SetupId = CurrentRow.find("td:eq(3)").text();
    //var SetupVal = CurrentRow.find("td:eq(1)").text();
    var SetupId = CurrentRow.find("#setupidGrpNm").text();
    var SetupVal = CurrentRow.find("#setupvalGrpNm").text();
    $("#GroupName").val(SetupVal).trigger('change');
    $("#GroupName_hdnId").val(SetupId);
    $("#btn_saveGroupName").val("UpdateGroupName");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableCustomerPriceGroup").css("display", "block");
        $("#GroupName").attr("disabled", false);
    }
}

function GroupNameSaveBtnClick() {
    debugger;
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickGroup_Name") {
        $("#btn_saveGroupName").css("filter", "grayscale(100%)");
        $("#btn_saveGroupName").prop("disabled", true);
        /*End*/
        return false;
    }

    if (CheckVallidation("GroupName", "vmGroupName") == false) {
        return false;
    } else {
        $("#btn_saveGroupName").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickGroup_Name");

        return true;
    }
}
function OnChangeGroupName() {
    CheckVallidation("GroupName", "vmGroupName");
}

//---------------------------------------Costomer Group Setup End----------------------------------------//


debugger;
function functionConfirm(e, a) {

    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm)
    {
        if (isConfirm) {
            $("#hdnAction").val("DeleteState");
            // $('form').submit();
            debugger;
            //var currentRow = $(e.target).closest('tr');
            //var setup_ID = currentRow.find("td:eq(3)").text();
            //var setup_Val = currentRow.find("td:eq(1)").text();
            //var Setup_type_id = currentRow.find("td:eq(2)").text();
            if (a == 'Bin') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidBIN").text();
                var setup_Val = currentRow.find("#setupvalBIN").text();
                var Setup_type_id = currentRow.find("#settypeBin").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else if (a == 'ItmPrf') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidItmPF").text();
                var setup_Val = currentRow.find("#setupvalItmPF").text();
                var Setup_type_id = currentRow.find("#settypeItmpf").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else if (a == 'CustPrf') {
                var currentRow = $(e.target).closest('tr');
                //Modifyd by shubham maurya on 20-12-2022 10:11 for all data wrong pass//
                //var setup_ID = currentRow.find("#setupidCustPF").text();
                //var setup_Val = currentRow.find("#settypeCustprf").text();
                //var Setup_type_id = currentRow.find("#setupidCustPF").text();
                var setup_ID = currentRow.find("#setupidCustPF").text();
                var setup_Val = currentRow.find("#setupvalCustPF").text();
                var Setup_type_id = currentRow.find("#settypeCustprf").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else if (a == 'SuppPrf') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidSuppPF").text();
                var setup_Val = currentRow.find("#setupvalSuppPF").text();
                var Setup_type_id = currentRow.find("#settypeSuppPrf").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else  if (a == 'CustCategry') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidCustCate").text();
                var setup_Val = currentRow.find("#setupvalCustCate").text();
                var Setup_type_id = currentRow.find("#settypeCustCate").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else if (a == 'SuppCategry') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidSuppCate").text();
                var setup_Val = currentRow.find("#setupvalSuppCate").text();
                var Setup_type_id = currentRow.find("#settypeSuppCate").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else if (a == 'SaleRgn') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidSalRegn").text();
                var setup_Val = currentRow.find("#setupvalSalRegn").text();
                var Setup_type_id = currentRow.find("#settypeSalRegn").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else  if (a == 'ReqArea') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidReqArea").text();
                var setup_Val = currentRow.find("#setupvalReqArea").text();
                var Setup_type_id = currentRow.find("#settypeReqArea").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else  if (a == 'Vehicle') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidVehc").text();
                var setup_Val = currentRow.find("#setupvalVehc").text();
                var Setup_type_id = currentRow.find("#settypeVehc").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else if (a == 'GroupName') {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidGrpNm").text();
                var setup_Val = currentRow.find("#setupvalGrpNm").text();
                var Setup_type_id = currentRow.find("#settypeGrpNm").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else  if (a == "SalesExecutive") {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#salesPersonId").text();
                var Setup_type_id = currentRow.find("#settypeGrpNm").text();
                window.location.href = "/BusinessLayer/MIS/DeleteSeDetail/?seId=" + setup_ID;
            }
           else if (a == "Saveport") {
                var Flag = "";
                var currentRow = $(e.target).closest('tr');
                var Portid = currentRow.find("#tblport_id").text();
                window.location.href = "/BusinessLayer/MIS/DeletePortDetail/?Portid=" + Portid ;
            }
            else if (a == "Wastage") {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#wastage_id").text();
              //  var Setup_type_id = currentRow.find("#settypeGrpNm").text();
                window.location.href = "/BusinessLayer/MIS/DeleteWastageDetail/?wastage_id=" + setup_ID;
            }
            else if (a == "Rejection") {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#Rejection_id").text();
                //  var Setup_type_id = currentRow.find("#settypeGrpNm").text();
                window.location.href = "/BusinessLayer/MIS/DeleteRejectionDetail/?Rejection_id=" + setup_ID;
            }
            else if (a == "Employee") {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#emp_id").text();
                //  var Setup_type_id = currentRow.find("#settypeGrpNm").text();
                window.location.href = "/BusinessLayer/MIS/DeleteEmployeeSetup/?Emp_id=" + setup_ID;
            }
            else if (a == "GLReporting") {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#Gl_id").text();
                //  var Setup_type_id = currentRow.find("#settypeGrpNm").text();
                window.location.href = "/BusinessLayer/MIS/DeleteGl_rptgrp/?Gl_id=" + setup_ID;
            }
            else if (a == "CustomerZone") {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setup_id").text();
                var setup_Val = currentRow.find("#setup_val").text();
                var Setup_type_id = currentRow.find("#settypeid").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else if (a == "CustomerGroup") {
                var currentRow = $(e.target).closest('tr');
                var setup_ID = currentRow.find("#setupidcust_grp").text();
                var setup_Val = currentRow.find("#setupvalcust_grp").text();
                var Setup_type_id = currentRow.find("#settypecust_grp").text();
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }
            else {
                window.location.href = "/BusinessLayer/MIS/DeleteMISDetail/?setup_ID=" + setup_ID + "&setup_Val=" + setup_Val + "&Setup_type_id=" + Setup_type_id;
            }

            return true;
        } else {
            return false;
        }
    });
    return false;
}

//------------------------------------Sales Executive Setup---------------------------------------------//
function EditSalesExecutiveName(e) {
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var seId = CurrentRow.find("#salesPersonId").text();
    var salesPersonName = CurrentRow.find("#salesPersonName").text();
    var salesPersonContNo = CurrentRow.find("#salesPersonContNo").text();
    var salesPersonEmailId = CurrentRow.find("#salesPersonEmailId").text();
    var sr_id = CurrentRow.find("#sr_id").text();
    $("#salesExecutiveId").val(seId);
    $("#SalesExecutiveName").val(salesPersonName);
    $("#SalesPersonContact").val(salesPersonContNo);
    $("#SalesPersonEmail").val(salesPersonEmailId);
    $("#country").val(sr_id).trigger("change");
    $("#btn_saveSalesExecutive").val("UpdateSalesExecutive");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableSalesExecutive").css("display", "block");
        $("#SalesExecutiveName").attr("disabled", false);
        $("#SalesPersonContact").attr("disabled", false);
        $("#SalesPersonEmail").attr("disabled", false);
    }
}

//------------------------------------Employee Setup---------------------------------------------//
function EditEmployeeSetup(e) {
    debugger;
  
    var CurrentRow = $(e.target).closest('tr');
    var Emp_id = CurrentRow.find("#emp_id").text();
    var Emp_name = CurrentRow.find("#Emp_name").text();
    var emp_cont = CurrentRow.find("#Emp_cont_no").text();
    var Emp_EmailNo = CurrentRow.find("#emp_EmailId").text();
    $("#EmployeeNameID").val(Emp_id);
    $("#EmployeeName").val(Emp_name);
    $("#Emp_ContactNo").val(emp_cont);
    $("#Emp_EmailId").val(Emp_EmailNo);
    $("#btn_saveEmployeeName").val("UpdateEmployeeName");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#btn_saveEmployeeName").css("display", "block");
        $("#EmployeeName").attr("disabled", false);
        $("#Emp_ContactNo").attr("disabled", false);
        $("#Emp_EmailId").attr("disabled", false);
    }
    $("#EmployeeName").css("border-color", "#ced4da");
    document.getElementById("vmEmployeeName").innerHTML = "";
}
function EditWastageReson(e) {
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var WAST_ID = CurrentRow.find("#wastage_id").text();
    var wastage_reason = CurrentRow.find("#wastage_reason").text();
    $("#WastageReasonId").val(WAST_ID);
    $("#WastageReason").val(wastage_reason);
    $("#btn_saveWastageReason").val("UpdateWastageReason");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableWastageReason").css("display", "block");
        $("#SalesExecutiveName").attr("disabled", false);
    }
    $("#WastageReason").css("border-color", "#ced4da");
    document.getElementById("vmWastageReason").innerHTML = "";
}
function EditRejectionReason(e) {
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var Rej_ID = CurrentRow.find("#Rejection_id").text();
    var Rej_reason = CurrentRow.find("#Rejection_reason").text();
    $("#RejectionReasonId").val(Rej_ID);
    $("#RejectionReason").val(Rej_reason);
    $("#btn_saveRejectionReason").val("UpdateRejectionReason");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#btn_saveRejectionReason").css("display", "block");
        $("#RejectionReason").attr("disabled", false);
    }
    $("#RejectionReason").css("border-color", "#ced4da");
    document.getElementById("vmRejectionReason").innerHTML = "";
}
function EditGlReportingGroup(e) {
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var Gl_id = CurrentRow.find("#Gl_id").text();
    var GL_reporting = CurrentRow.find("#GL_reporting").text();
    $("#GLReport_Id").val(Gl_id);
    $("#GLReportingGroup").val(GL_reporting);
    $("#btn_saveGLReportingGroup").val("UpdateGLReportingGroup");
    $("#hdnSavebtn").val(null);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#btn_saveGLReportingGroup").css("display", "block");
        $("#GLReportingGroup").attr("disabled", false);
    }
    $("#GLReportingGroup").css("border-color", "#ced4da");
    document.getElementById("vmGLReportingGroup").innerHTML = "";
}
function ReadBranchList() {
    debugger;
    var Branches = new Array();
    $("#CustBrDetail TBODY TR").each(function () {
        /*    debugger;*/
        var row = $(this);
        var Branch = {};
        Branch.Id = row.find("#hdCustomerBranchId").val();
        var checkBoxId = "#cust_act_stat_" + Branch.Id;
        if (row.find(checkBoxId).is(":checked")) {
            Branch.BranchFlag = "Y";
        }
        else {
            Branch.BranchFlag = "N";
        }
        Branches.push(Branch);
    });

    var str = JSON.stringify(Branches);
    $('#hdCustomerBranchList').val(str);
}
function SalesExecutiveSaveBtnClick() {
    debugger;
    var ErrorFlag = "Y";
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickSale") {
        $("#btn_saveSalesExecutive").css("filter", "grayscale(100%)");
        $("#btn_saveSalesExecutive").prop("disabled", true);
        /*End*/
        return false;
    }
    if (CheckVallidation("SalesExecutiveName", "vmSalesExecutiveName") == false) {
        return false;
    } else {

        var country = $("#country").val();
        if (country == "" || country == "0") {
            $('#validCountry').text($("#valueReq").text());
            $("#country").css("border-color", "red");
            $("[aria-labelledby='select2-country-container']").css("border-color", "red");
            $("#validCountry").css("display", "block");
            ErrorFlag = "N";
        }
        else {
            $("#country").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-country-container']").css("border-color", "#ced4da");
            $("#validCountry").css("display", "none");
        }
        if (ErrorFlag == "N") {
            return false;
        }

        ReadBranchList();
        $("#btn_saveSalesExecutive").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSale");
        return true;
    }
}
function WastageReasonSaveBtnClick() {/**Added This function NItesh 26022025 For Add New Page*/
    debugger;
    var btn = $("#hdnSavebtn").val(); 
    if (btn == "AllreadyClickSale") {
        $("#btn_saveWastageReason").css("filter", "grayscale(100%)");
        $("#btn_saveWastageReason").prop("disabled", true);
        /*End*/
        return false;
    }
    if (CheckVallidation("WastageReason", "vmWastageReason") == false) {
        return false;
    } else {
        ReadBranchList();
        $("#btn_saveWastageReason").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSale");
        return true;
    }
}
function RejectionReasonSaveBtnClick() {/**Added This function NItesh 27022025 For Add New Page*/
    debugger;
    var btn = $("#hdnSavebtn").val();
    if (btn == "AllreadyClickSale") {
        $("#btn_saveRejectionReason").css("filter", "grayscale(100%)");
        $("#btn_saveRejectionReason").prop("disabled", true);
        /*End*/
        return false;
    }
    if (CheckVallidation("RejectionReason", "vmRejectionReason") == false) {
        return false;
    } else {
        ReadBranchList();
        $("#btn_saveRejectionReason").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSale");
        return true;
    }
}
function EmployeeNameSaveBtnClick() {
    debugger;
    var btn = $("#hdnSavebtn").val();
    if (btn == "AllreadyClickSale") {
        $("#btn_saveEmployeeName").css("filter", "grayscale(100%)");
        $("#btn_saveEmployeeName").prop("disabled", true);
        /*End*/
        return false;
    }
    if (CheckVallidation("EmployeeName", "vmEmployeeName") == false) {
        return false;
    } else {
        ReadBranchList();
        $("#btn_saveEmployeeName").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSale");
        return true;
    }
}
function GLReportingGroupSaveBtnClick() {/**Added This function NItesh 27022025 For Add New Page*/
    debugger;
    var btn = $("#hdnSavebtn").val();
    if (btn == "AllreadyClickSale") {
        $("#btn_saveGLReportingGroup").css("filter", "grayscale(100%)");
        $("#btn_saveGLReportingGroup").prop("disabled", true);
        /*End*/
        return false;
    }
    if (CheckVallidation("GLReportingGroup", "vmGLReportingGroup") == false) {
        return false;
    } else {
        ReadBranchList();
        $("#btn_saveGLReportingGroup").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickSale");
        return true;
    }
}
function OnChangeGLReportingGroup() {
    CheckVallidation("GLReportingGroup", "vmGLReportingGroup")
}
function OnChangeEmployeeName() {
    CheckVallidation("EmployeeName", "vmEmployeeName")
}
function CheckSalesExecutiveValidations() {
    debugger
    if (CheckVallidation("SalesExecutiveName", "vmSalesExecutiveName") == false) {
        return false;
    } else {
        return true;
    }
}
function OnChangeCustomerZone() {
    CheckVallidation("CustomerZone", "vmCustomerZone")
}
function OnChangeCustomergroup() {
    CheckVallidation("Customergroup", "vmCustomergroup")
}
function CustomerZoneSaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val();
    if (btn == "AllreadyClick") {
        $("#btn_saveCustomerZone").css("filter", "grayscale(100%)");
        $("#btn_saveCustomerZone").prop("disabled", true); /*End*/
        return false;
    }
    if (CheckVallidation("CustomerZone", "vmCustomerZone") == false) {
        return false;
    } else {
        $("#btn_saveCustomerZone").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function CustomergroupSaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val();
    if (btn == "AllreadyClick") {
        $("#btn_saveCustomergroup").css("filter", "grayscale(100%)");
        $("#btn_saveCustomergroup").prop("disabled", true); /*End*/
        return false;
    }
    if (CheckVallidation("Customergroup", "vmCustomergroup") == false) {
        return false;
    } else {
        $("#btn_saveCustomergroup").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function OnChangeSeName() {
    CheckVallidation("SalesExecutiveName", "vmSalesExecutiveName")
}
function OnChangeWastageReason() {
    CheckVallidation("WastageReason", "vmWastageReason")
}
function OnChangeRejectionReason() {
    CheckVallidation("RejectionReason", "vmRejectionReason")
}

function CheckBranchStatusWhenUpdate(flag) {
    debugger;
    var Doc_id = "";
    if (flag == "Wastage") {
         Doc_id = $('#WastageReasonId').val();
    }
    else if (flag =="Rejection") {
        Doc_id = $('#RejectionReasonId').val();
    }
    else if (flag == "Employee") {
        Doc_id = $('#EmployeeNameID').val();
    }
    else if (flag == "GLReporting") {
        Doc_id = $('#GLReport_Id').val();
    }
   
    var branchId = "0";

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/MIS/CheckBranchStatus",
        dataType: "json",
        data: {
            branchId: branchId,
            Doc_id: Doc_id,
            flag: flag,
        },
        success: function (data) {
            debugger;
            if (data == 'Error') {
                ErrorPage();
                return false;
            }
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                //$('#Proscurr').val(arr[0].curr_id);
                $("#CustBrDetail TBODY TR").each(function () {
                    debugger;
                    var row = $(this);
                    branchId = row.find("#hdCustomerBranchId").val();
                    var checkBoxId = "#cust_act_stat_" + branchId;
                    for (var i = 0; i < arr.length; i++) {
                        if (branchId == arr[i].br_id) {
                            var chkStatus = arr[i].act_status;
                            if (chkStatus == 'Y') {
                                row.find("#cust_act_stat_" + branchId).prop("checked", true);
                            }
                            else {
                                row.find("#cust_act_stat_" + branchId).prop("checked", false);
                            }
                        }
                    }
                });
            }
        },
        error: function (Data) {
        }
    });
    /*});*/
}

function CheckSeBranchStatusWhenUpdate() {
    debugger;
    var seId = $('#salesExecutiveId').val();
    var branchId = "0";

    $.ajax({
        type: "POST",
        url: "/BusinessLayer/MIS/CheckSeBranchStatus",
        dataType: "json",
        data: {
            branchId: branchId,
            seId: seId,
        },
        success: function (data) {
            debugger;
            if (data == 'Error') {
                ErrorPage();
                return false;
            }
            var arr = [];
            arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
            if (arr.length > 0) {
                //$('#Proscurr').val(arr[0].curr_id);
                $("#CustBrDetail TBODY TR").each(function () {
                    debugger;
                    var row = $(this);
                     branchId = row.find("#hdCustomerBranchId").val();
                    var checkBoxId = "#cust_act_stat_" + branchId;
                    for (var i = 0; i < arr.length; i++) {
                        if (branchId == arr[i].br_id) {
                            var chkStatus = arr[i].act_status;
                            if (chkStatus == 'Y') {
                                row.find("#cust_act_stat_" + branchId).prop("checked", true);
                            }
                            else {
                                row.find("#cust_act_stat_" + branchId).prop("checked", false);
                            }
                        }
                    }
                });
            }
        },
        error: function (Data) {
        }
    });
    /*});*/
}
function OnChangecontlist() /*Added By Nitesh 14-12-2023 15:19 for onchange*/
{
    debugger;
    var country = $("#country").val();
    bindstate(country);

    if (country == "" || country == "0") {
        $('#validCountry').text($("#valueReq").text());
        $("#country").css("border-color", "red");
        $("[aria-labelledby='select2-country-container']").css("border-color", "red");
        $("#validCountry").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#country").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-country-container']").css("border-color", "#ced4da");
        $("#validCountry").css("display", "none");
        $("#ddlstate").val("0").trigger("change");

        $("#ddlstate").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlstate-container']").css("border-color", "#ced4da");
        $("#validstate").css("display", "none");
    }
}
function OnChangestate() /*Added By Nitesh 14-12-2023 15:19 for onchange*/
{
    var ddlstate = $("#ddlstate").val();
    if (ddlstate == "" || ddlstate == "0") {
        $('#validstate').text($("#valueReq").text());
        $("#ddlstate").css("border-color", "red");
        $("[aria-labelledby='select2-ddlstate-container']").css("border-color", "red");
        $("#validstate").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#ddlstate").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlstate-container']").css("border-color", "#ced4da");
        $("#validstate").css("display", "none");
    }
}
function OnChangeportid() { /*Added By Nitesh 14-12-2023 15:19 for onchange*/
    var PortID = $("#PortID").val();
    if (PortID == "" || PortID == "0") {
        $('#validportid').text($("#valueReq").text());
        $("#PortID").css("border-color", "red");
        $("#validportid").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PortID").css("border-color", "#ced4da");
        $("#validportid").css("display", "none");
    }
}
function OnChangeportdes() { /*Added By Nitesh 14-12-2023 15:19 for onchange*/
    var PortDescription = $("#PortDescription").val();
    if (PortDescription == "" || PortDescription == "0") {
        $('#validPortDescription').text($("#valueReq").text());
        $("#PortDescription").css("border-color", "red");
        $("#validPortDescription").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PortDescription").css("border-color", "#ced4da");
        $("#validPortDescription").css("display", "none");
    }
}
function OnChangeportcode() { /*Added By Nitesh 14-12-2023 15:19 for onchange*/
    var PinCode = $("#PinCode").val();
    if (PinCode == "" || PinCode == "0") {
        $('#validPinCode').text($("#valueReq").text());
        $("#PinCode").css("border-color", "red");
        $("#validPinCode").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PinCode").css("border-color", "#ced4da");
        $("#validPinCode").css("display", "none");
    }
}
function OnchangePorttype() { /*Added By Nitesh 18-12-2023 10:12 for onchange*/
    var ddlporttype = $("#ddlporttype").val();
    if (ddlporttype == "" || ddlporttype == "0") {
        $('#validPorttype').text($("#valueReq").text());
        $("#ddlporttype").css("border-color", "red");
        $("#validPorttype").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#ddlporttype").css("border-color", "#ced4da");
        $("#validPorttype").css("display", "none");
    }
}
function checkvalidationport() {  /*Added By Nitesh 14-12-2023 15:19 for Check Validation when Save*/
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClickPort") {
        $("#saveportdetail").css("filter", "grayscale(100%)");
        $("#saveportdetail").prop("disabled", true);
        /*End*/
        return false;
    }

    var ErrorFlag = "Y";
    $("#PortID").attr("disabled", false);
    $("#ddlporttype").attr("disabled", false)
    var ddlporttype = $("#ddlporttype").val();

    var country = $("#country").val();
    var PortID = $("#PortID").val();
    var PortDescription = $("#PortDescription").val();
    var PinCode = $("#PinCode").val();
    var ddlstate = $("#ddlstate").val();

    if (country == "" || country == "0") {
        $('#validCountry').text($("#valueReq").text());
        $("#country").css("border-color", "red");
        $("[aria-labelledby='select2-country-container']").css("border-color", "red");
        $("#validCountry").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#country").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-country-container']").css("border-color", "#ced4da");
        $("#validCountry").css("display", "none");
    }
    if (PortID == "" || PortID == "0") {
        $('#validportid').text($("#valueReq").text());
        $("#PortID").css("border-color", "red");
        $("#validportid").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PortID").css("border-color", "#ced4da");
        $("#validportid").css("display", "none");
    }
    if (PortDescription == "" || PortDescription == "0") {
        $('#validPortDescription').text($("#valueReq").text());
        $("#PortDescription").css("border-color", "red");
        $("#validPortDescription").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PortDescription").css("border-color", "#ced4da");
        $("#validPortDescription").css("display", "none");
    }
    if (PinCode == "" || PinCode == "0") {
        $('#validPinCode').text($("#valueReq").text());
        $("#PinCode").css("border-color", "red");
        $("#validPinCode").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PinCode").css("border-color", "#ced4da");
        $("#validPinCode").css("display", "none");
    }
    if (ddlstate == "" || ddlstate == "0") {
        $('#validstate').text($("#valueReq").text());
        $("#ddlstate").css("border-color", "red");
        $("[aria-labelledby='select2-ddlstate-container']").css("border-color", "red");
        $("#validstate").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#ddlstate").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlstate-container']").css("border-color", "#ced4da");
        $("#validstate").css("display", "none");
    }
 
    if (ddlporttype == "" || ddlporttype == "0") {
        $('#validPorttype').text($("#valueReq").text());
        $("#ddlporttype").css("border-color", "red");
        $("#validPorttype").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#ddlporttype").css("border-color", "#ced4da");
        $("#validPorttype").css("display", "none");
    }

    var hdnprot_id = $("#hdn_portid").val();
    var updatebtncheckvalidation = $("#saveportdetail").val();
    if (updatebtncheckvalidation != "Saveport")
    {
        $("#portsetuptabledata tbody tr").each(function () {
            debugger;
            var crow = $(this);
            var port_id = crow.find("#tblport_id").text();
            if (hdnprot_id != port_id && port_id == PortID) {
                $('#validportid').text($("#valueduplicate").text());
                $("#PortID").css("border-color", "red");
                $("#validportid").css("display", "block");
                ErrorFlag = "N";

            }
            else {
                $("#PortID").css("border-color", "#ced4da");
                $("#validportid").css("display", "none");
            }
            if (ErrorFlag == "N") {
                return false
            }
            //else {
            //    return true;
            //}
        })
    }

    if (ErrorFlag == "N")
    {
        return false
    }
    else {
        $("#saveportdetail").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClickPort");
        return true;
    }


}

function EditPortSetup(e) {    /*Added By Nitesh 14-12-2023 15:19 for Edit Port Deatil*/
    debugger;

    var CurrentRow = $(e.target).closest('tr');
    var hdncountry_id = CurrentRow.find("#hdncountry_id").text();
    var tblcountry_name = CurrentRow.find("#tblcountry_name").text();
    var tblport_id = CurrentRow.find("#tblport_id").text();
    var tblport_desc = CurrentRow.find("#tblport_desc").text();
    var tblpin_no = CurrentRow.find("#tblpin_no").text();
    var hdnstate_id = CurrentRow.find("#hdnstate_id").text();
    var tblstate_name = CurrentRow.find("#tblstate_name").text();
    var tblporttype = CurrentRow.find("#tblporttype").text();
    var hdnPorttype_id = CurrentRow.find("#hdnPorttype_id").text();
    var Checkdependcy = CurrentRow.find("#Checkdependcy").text();
    $("#country").val(hdncountry_id).trigger("change");
    $("#PortID").val(tblport_id);
    $("#hdn_portid").val(tblport_id);
    $("#PortDescription").val(tblport_desc);
    $("#PinCode").val(tblpin_no);
    $("#ddlstate").val(hdnstate_id).trigger("change");
    $("#ddlporttype").val(hdnPorttype_id).trigger("change");
    $("#saveportdetail").val("updatesaveportdetail");
    if (Checkdependcy == "Y") {/*Added By Nitesh 20-12-2023*/
        $("#PortID").attr("disabled", true);
        $("#ddlporttype").attr("disabled", true);
    }
    else {
        $("#PortID").attr("disabled", false);
        $("#ddlporttype").attr("disabled", false);
    }
    RemoveValidation();
    $("#hdnSavebtn").val(null);
    //checkDepency(tblport_id);

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnablePortSetup").css("display", "block");
        //$("#ddlporttype").attr("disabled", false);
        $("#country").attr("disabled", false);
        $("#ddlstate").attr("disabled", false);
        //$("#PortID").attr("disabled", false);
        $("#PortDescription").attr("disabled", false);
        $("#PinCode").attr("disabled", false);
    }
}



function bindstate(countryid) /*Added By Nitesh 14-12-2023 15:19 for bindState*/
{
    debugger;
    $("#ddlstate").select2({
        ajax: {
            url: $("#hfstatelistbind").val(),
            data: function (params) {
                var queryParameters = {
                    ddlstate: params.term, // search term like "a" then "an"
                    countryid: countryid,
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    Error_Page();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return {
                            id: val.ID, text: val.Name

                        };
                    })
                };
            }
        },
    });
}

function RemoveValidation() {  /*Added By Nitesh 14-12-2023 15:19 for Remove Validation*/
    var country = $("#country").val();
    var PortID = $("#PortID").val();
    var PortDescription = $("#PortDescription").val();
    var PinCode = $("#PinCode").val();
    var ddlstate = $("#ddlstate").val();

    if (country == "" || country == "0") {
        $('#validCountry').text($("#valueReq").text());
        $("#country").css("border-color", "red");
        $("[aria-labelledby='select2-country-container']").css("border-color", "red");
        $("#validCountry").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#country").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-country-container']").css("border-color", "#ced4da");
        $("#validCountry").css("display", "none");
    }
    if (PortID == "" || PortID == "0") {
        $('#validportid').text($("#valueReq").text());
        $("#PortID").css("border-color", "red");
        $("#validportid").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PortID").css("border-color", "#ced4da");
        $("#validportid").css("display", "none");
    }
    if (PortDescription == "" || PortDescription == "0") {
        $('#validPortDescription').text($("#valueReq").text());
        $("#PortDescription").css("border-color", "red");
        $("#validPortDescription").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PortDescription").css("border-color", "#ced4da");
        $("#validPortDescription").css("display", "none");
    }
    if (PinCode == "" || PinCode == "0") {
        $('#validPinCode').text($("#valueReq").text());
        $("#PinCode").css("border-color", "red");
        $("#validPinCode").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#PinCode").css("border-color", "#ced4da");
        $("#validPinCode").css("display", "none");
    }
    if (ddlstate == "" || ddlstate == "0") {
        $('#validstate').text($("#valueReq").text());
        $("#ddlstate").css("border-color", "red");
        $("[aria-labelledby='select2-ddlstate-container']").css("border-color", "red");
        $("#validstate").css("display", "block");
        ErrorFlag = "N";
    }
    else {
        $("#ddlstate").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlstate-container']").css("border-color", "#ced4da");
        $("#validstate").css("display", "none");
    }
}
//---------------------------------------Asset Category----------------------------------------//
function EditAssetCategory(e) {
    var CurrentRow = $(e.target).closest('tr');
    var SetupId = CurrentRow.find("#setupidBIN").text();
    var SetupVal = CurrentRow.find("#setupvalBIN").text();
    $("#AssetCategory").val(SetupVal).trigger('change');
    $("#AssetCategory_hdnId").val(SetupId);//btn_saveBIN
    $("#btn_saveAssetCategory").val("UpdateAssestCatgory");
    /*$("#btn_saveAssetCategory").val(null);*/

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableAssetCategorySetup").css("display", "block");
        $("#AssetCategory").attr("disabled", false);
    }
}
function EditCustomerGroup(e) {
    var CurrentRow = $(e.target).closest('tr');
    var SetupId = CurrentRow.find("#setupidcust_grp").text();
    var SetupVal = CurrentRow.find("#setupvalcust_grp").text();
    $("#Customergroup").val(SetupVal).trigger('change');
    $("#Customergroup_hdnId").val(SetupId);//btn_saveBIN
    $("#btn_saveCustomergroup").val("UpdateCustomergroup");
    

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnableAssetCategorySetup").css("display", "block");
        $("#AssetCategory").attr("disabled", false);
    }
}
function EditCustomerZone(e)
{
    debugger;
    var CurrentRow = $(e.target).closest('tr');
    var SetupId = CurrentRow.find("#setup_id").text();
    var SetupVal = CurrentRow.find("#setup_val").text();
    $("#CustomerZone").val(SetupVal).trigger('change');
    $("#CustomerZone_hdn").val(SetupId);//btn_saveBIN
    $("#btn_saveCustomerZone").val("UpdateCustomerZone");


    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#btn_saveCustomerZone").css("display", "none");
        $("#CustomerZone").attr("disabled", false);
    }
    else {
        $("#btn_saveCustomerZone").css("display", "block");
        $("#CustomerZone").attr("disabled", false);
    }
}
function AssetCategorySaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 05-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_saveAssetCategory").css("filter", "grayscale(100%)");
        $("#btn_saveAssetCategory").prop("disabled", true); /*End*/
        return false;
    }
    if (CheckVallidation("AssetCategory", "vmAssetCategory") == false) {
        return false;
    } else {
        $("#btn_saveAssetCategory").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}

function OnChangeAssestCatgory() {
    CheckVallidation("AssetCategory", "vmAssetCategory");
}


//---------------------------------------Asset Category  End----------------------------------------//
//function checkDepency(tblport_id) {
//    /*CheckDependency*/
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/BusinessLayer/MIS/checkdependcyport",
//        dataType: "json",
//        data: {
//            tblport_id: tblport_id,
          
//        },
//        success: function (data) {
//            debugger;
//            if (data == 'Error') {
//                ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                if (arr.length > 0) {
//                    var depency = arr[0]["chackdependcy"];

//                    $("#hdncheckdependcy").val(depency);
//                    if (depency == "Y") {
//                        $("#PortID").attr("disabled", true);
//                        $("#ddlporttype").attr("disabled", true)
//                    }
//                    else {
//                        $("#PortID").attr("disabled", false);
//                        $("#ddlporttype").attr("disabled", false);
//                    }
//                }
//                else {
//                    $("#hdncheckdependcy").val("");
//                }
 
//            }
//        },
//        error: function (Data) {
//        }
//    });
//}