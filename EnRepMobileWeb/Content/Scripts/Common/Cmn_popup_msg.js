
function ShowCommonPopup(msg) {
    debugger;
    if (!msg) return;

   

    // ✅ 1️⃣ SweetAlert popup message mappings
    const PopuP = {
        "Save": ["#savemsg", "success"],
        "Saved": ["#savemsg", "success"],
        "Deleted": ["#deletemsg", "success"],
        "Approved": ["#approvemsg", "success"],
        "Used": ["#DependencyExist", "warning"],
          "Cancelled": ["#cancelmsg", "success"]
    };

    // ✅ 2️⃣ Duplicate field mappings
    const duplicates = {
        "DuplicateBIN": ["BINNumber", "vmBINNumber"],
        "DuplicateITEM PORTFOLIO": ["PortfolioName", "vmPortfolioName"],
        "DuplicateCustomer Portfolio": ["CustPortfolioName", "vmCustPortfolioName"],
        "Duplicate": ["SalesExecutiveName", "vmSalesExecutiveName"],
        "DuplicateSupplier Portfolio": ["SuppPortfolioName", "vmSuppPortfolioName"],
        "DuplicateCustomer Category": ["CustCategoryName", "vmCustCategoryName"],
        "DuplicateSupplier Category": ["SupplierCategory", "vmSupplierCategory"],
        "DuplicateSales Region": ["SalesRegionName", "vmSalesRegionName"],
        "DuplicateRequirement Area": ["RequirementAreaName", "vmRequirementAreaName"],
        "DuplicateVehicle Setup": ["VehicleName", "vmVehicleName"],
        "Duplicate Customer Price Setup": ["GroupName", "vmGroupName"],
        "Duplicate_PORTID": ["PortID", "validportid"],
        "Duplicate_wastage": ["WastageReason", "vmWastageReason"],
        "DuplicateRejectionReason": ["RejectionReason", "vmRejectionReason"],
        "Duplicate_Employee": ["EmployeeName", "vmEmployeeName"],
        "DuplicateGLRptGrp": ["GLReportingGroup", "vmGLReportingGroup"],
        "DuplicateAssestCatgory": ["AssetCategory", "vmAssetCategory"],
        "DuplicateCustomerZone": ["CustomerZone", "vmCustomerZone"],
        "DuplicateCustomergroup": ["Customergroup", "vmCustomergroup"]
    };

    // ✅ 3️⃣ Show popup messages
    if (PopuP[msg]) {
        const [selector, type] = PopuP[msg];
        swal("", $(selector).text(), type);
        return;
    }

    // ✅ 4️⃣ Highlight duplicate fields
    if (duplicates[msg]) {
        const [fieldId, messageId] = duplicates[msg];
        $("#" + fieldId).css("border-color", "red");
       //document.getElementById(messageId).innerHTML = $("#valueduplicate").text();
        $("#" + messageId).html($("#valueduplicate").text());
    }
}
