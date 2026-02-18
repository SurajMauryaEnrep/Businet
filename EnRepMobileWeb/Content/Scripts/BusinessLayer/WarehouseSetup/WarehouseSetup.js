function GetwareDSCntr() {
    debugger;
    $("#Wh_tbody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var Wh_Code = clickedrow.children("#whid").text();
            var wh_idhdn = clickedrow.children("#wh_id_hdn").text();
            window.location.href = "/BusinessLayer/WarehouseSetup/AddWarehouseSetupDetail/?wh_name=" + Wh_Code + "&ListFilterData=" + ListFilterData;
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    debugger;
    $("#WarehouseNamefilter").select2();
    var ddlCity = $("#warehouse_city").val();

  

    $.ajax({
        type: "POST",
        url: "/CustomerDetails/GetsuppDSCntr",        
        dataType: "json",        
        data: { SuppCity: ddlCity, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.Table.length > 0) {
                    $("#ware_district").val(arr.Table[0].district_name);
                    $("#waredistID").val(arr.Table[0].district_id);
                }
                if (arr.Table1.length > 0) {
                    $("#ware_state").val(arr.Table1[0].state_name);
                    $("#warestateID").val(arr.Table1[0].state_id);
                }
                if (arr.Table2.length > 0) {
                    $("#ware_country").val(arr.Table2[0].country_name);
                    $("#warecntryID").val(arr.Table2[0].country_id);
                }
            }
        },
        error: function (Data) {
        }
    });

};
$(document).ready(function () {
    debugger;

    Cmn_ByDefaultBranchData("#CustBrDetail", "#hdCustomerBranchId", "#cust_act_stat_", "Cust", "#HdnBranchActiveSatus");

    var ddlCity = $("#warehouse_city").val();
    if (ddlCity != "0") {
        GetwareDSCntr();
    }
    ReadBranchList();

    $("#warehouse_city").select2({
        
        ajax: {
            url: $("#warehousecity_Name").val(),
            data: function (params) {
                var queryParameters = {

                    ddlcity: params.term
                   
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,            
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },

        },

    });

})


function ReadBranchList() {
    debugger;
    var Branches = new Array();
    $("#CustBrDetail TBODY TR").each(function () {
        debugger;
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
    debugger;
    var str = JSON.stringify(Branches);
    $('#hdCustomerBranchList').val(str);
}


function FilterListwarehouse() {
    debugger;
    var wh_type = $("#WarehouseTypefilter").val();
    var wh_name = $("#WarehouseNamefilter").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/WarehouseSetup/WarehouseSetuplist",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            wh_type: wh_type,
            wh_name: wh_name,
            
        },
        success: function (data) {
            debugger;
            $('#Wh_tbody').html(data);
            $('#ListFilterData').val(wh_type + ',' + wh_name);
        },
    });
}


function OnChangeCity() {
    debugger;
    var Port = $('#supplier_city').val().trim();
    if (Port != "0") {
        document.getElementById("vmSuppCity").innerHTML = "";
        $("#supplier_city").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmSuppCity").innerHTML = $("#valueReq").text();
        $("#supplier_city").css("border-color", "red");
    }
}

function OnChangewarehouseName() {
    var WarehouseName = $('#WarehouseName').val().trim();
    if (WarehouseName != "") {
        document.getElementById("vmWarehouseName").innerHTML = "";
        $("#WarehouseName").css("border-color", "#ced4da");
        ValidationFlag = false;
    }
}


function OnChangewarehouseType() {
    if ($('#WarehouseType').val().trim() != "0") {
        document.getElementById("vmWarehouseType").innerHTML = "";
        $("#WarehouseType").css("border-color", "#ced4da");
        ValidationFlag = false;
    }
}



function validateWareHouseSetupInsertform() {
    debugger;
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }
    var WarehouseName = $('#WarehouseName').val().trim();
    var ValidationFlag = true;
    if (WarehouseName == "") {
        document.getElementById("vmWarehouseName").innerHTML = $("#valueReq").text();
        $("#WarehouseName").css("border-color", "red");
        ValidationFlag = false;
    }
    if ($('#WarehouseType').val().trim() == "0") {
        document.getElementById("vmWarehouseType").innerHTML = $("#valueReq").text();
        $("#WarehouseType").css("border-color", "red");
        ValidationFlag = false;
    }
    if (ValidationFlag == true) {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
    else {
       
        return false;
    }



}
//function onclickEditBtn(event) {
//    debugger;
//    $("#HdTransType").val("Update");
//    $('form').submit();
//}
function functionConfirm(event) {
    debugger;
    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}



