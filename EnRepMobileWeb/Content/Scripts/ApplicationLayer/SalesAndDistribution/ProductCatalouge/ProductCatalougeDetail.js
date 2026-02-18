$(document).ready(function () {

    //$('#datatable-buttons').DataTable({
    //        "order": [[3, "asc"]]
    //    });
    //$("#CustomerNameList2").select2()
    $("#ItmListBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var WF_status = $("#WF_status").val();
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var doc_no = clickedrow.find("#cat_no").text();
            var doc_date = clickedrow.find("#cat_date").text();
            if (doc_no != null && doc_no != "") {
                window.location.href = "/ApplicationLayer/ProductCatalouge/ProductCatalougeDetail/?doc_no=" + doc_no + "&doc_date=" + doc_date + "&ListFilterData=" + ListFilterData + "&WF_status=" + WF_status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var CTLNo = clickedrow.children("#cat_no").text();
        var CTLDate = clickedrow.children("#cat_date").text();
        var Doc_id = $("#DocumentMenuId").val();
        debugger;
        $("#hdDoc_No").val(CTLNo);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(CTLNo, CTLDate, Doc_id, Doc_Status);

    });
    BindCustomerList();
    //BindCustomerList1();
    BindItemName();
    BindSearchableDDLGrouptData();
    BindDLLPortfolioList();
    BindDLLVehicleList();
    BindDLLVehOEMNo();
    BindDLLReferenceNo();
    BindDLLTechSpecification();
    $('#datatable-buttons tbody').on('click', '.deleteIcon', function () {
        debugger
        $(this).closest('tr').remove();
        debugger;
        SerialNoAfterDelete();
     });

    
    var CTLNo =$("#catalogNumber").val();
    $("#hdDoc_No").val(CTLNo);
    $("#CustomerNameList2").select2();
    //var status = $("#hfStatus").val().trim();
    var status = $("#hfStatus").val();
    CustPros_type = "C";
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
        $("#Prosbtn").css("display", "none");
    }
    debugger;
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
        $("#Prosbtn").css("display", "block");
        if (status == "D" || status == "F" || status == "A" || status == "C") {
            $("#Prosbtn").css("display", "none");
        }
    }
    $("#hdn_CustPros_type").val(CustPros_type);
});
//-------------Searchable Criteria-----------------//
function OnChangeCustName(CustID) {
    debugger;
    var CustomerNameList = $("#CustomerNameList").val();
    if (CustomerNameList == "0") {
        $("[aria-labelledby='select2-CustomerNameList-container']").css("border-color", "red");
        $("#vmCustomerNameList").text($("#valueReq").text());
        $("#vmCustomerNameList").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("[aria-labelledby='select2-CustomerNameList-container']").css("border-color", "#ced4da");
        $("#vmCustomerNameList").text("");
        $("#vmCustomerNameList").css("display", "none");
    }
    

}

function BindCustomerList() {
    debugger;
    var CustPros_type;
    var Cust_type;
    var status = $("#hfStatus").val();
    //var status = status1.trim();
    CustPros_type = "C";
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
        $("#Prosbtn").css("display", "none");
    }
    debugger;
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
        $("#Prosbtn").css("display", "block");
        if (status == "D" || status == "F" || status == "A" || status == "C") {
            $("#Prosbtn").css("display", "none");
        }
    }
    $("#hdn_CustPros_type").val(CustPros_type);
    $("#CustomerNameList").select2({

        ajax: {
            url: $("#CustNameList").val(),
            data: function (params) {
                var queryParameters = {
                    CustName: params.term, // search term like "a" then "an"
                    CustPros_type: CustPros_type,
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}
//function BindCustomerList1() {
//    debugger;

//    $("#CustomerNameList1").select2({
//        ajax: {
//            url: $("#CustNameList1").val(),
//            data: function (params) {
//                var queryParameters = {
//                    CustName: params.term // search term like "a" then "an"
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    ErrorPage();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}

function BindItemName() {
    debugger;
    DynamicSerchableItemDDL("", "#ddl_ProductName", "", "", "", "PC");
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/ProductCatalouge/GetItemName",
    //        data: function (params) {
    //            var queryParameters = {
    //                ItemName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
                        
    //                    $('#ddl_ProductName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#ddl_ProductName').select2({
    //                        templateResult: function (data) {
    //                            var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;
    //                        }
    //                    });



    //                }
    //            }
    //        },
    //    });
}

function OnChangeItem(e) {
    debugger;
    var Itm_ID = $("#ddl_ProductName").val();
    if (Itm_ID != null) {
        $("#hdn_product_id").val(Itm_ID);
    }
    else if (Itm_ID == null) {
        Itm_ID = $("#hdn_product_id").val();
    }
    try {
        Cmn_BindUOM(e, Itm_ID, "", "", "");
        //$.ajax({
        //    success: function () {
                if (Itm_ID == "0") {

                }
                else {
                    debugger;
                    $("#group_name").val("0").trigger('change'); /* set value on Searchable ddl  */

                    $("#ddl_PortFolioName").val("0").trigger('change');

                    $("#DdlReferenceNo").val("0").trigger('change');
                    $("#DdlTechnicalSpecification").val("0").trigger('change');
                    $("#CustomerNameList1").val("0").trigger('change');
                    $("#DdlVehicleName").val("0").trigger('change');
                    $("#DdlOEMNo").val("0").trigger('change');
                }

        //    }
        //});
} 
catch (err) {
}
  
}


function BindSearchableDDLGrouptData() {
    debugger
    $("#group_name").select2({
        ajax: {
            url: $("#ajaxUrlGetAutoCompleteSearchableGroupList").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlgroup_name: params.term, // search term like "a" then "an"
                    Group: params.page
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
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}

function BindDLLPortfolioList() {
    debugger;
    $("#ddl_PortFolioName").select2({
        ajax: {
            type: "POST",
            url: "/ApplicationLayer/ProductCatalouge/GetPortfolioListAuto",
            data: function (params) {
                var queryParameters = {
                    PortfName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            //contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                data = JSON.parse(data).Table;
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.setup_id, text: val.setup_val };
                    })
                };
            }
        },
    });
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/ProductCatalouge/GetPortfolioListAuto",
    //        data: function (params) {
    //            var queryParameters = {
    //                PortfName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //             debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $("#ddl_PortFolioName").append(`<option value="${arr.Table[i].setup_id}">${arr.Table[i].setup_val}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $("#ddl_PortFolioName").select2({
    //                        templateResult: function (data) {
    //                            //var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;

    //                        }
    //                    });
    //                }
    //            }
    //        },

    //    });

            
}

function BindDLLVehicleList() {
    debugger;
    $("#DdlVehicleName").select2({
        ajax: {
            type: "POST",
            url: "/ApplicationLayer/ProductCatalouge/GetVehicleListAuto",
            data: function (params) {
                var queryParameters = {
                    VehicleName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            //contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                data = JSON.parse(data).Table;
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.setup_id, text: val.setup_val };
                    })
                };
            }
        },
    });
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/ProductCatalouge/GetVehicleListAuto",
    //        data: function (params) {
    //            var queryParameters = {
    //                VehicleName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            // debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                   for (var i = 0; i < arr.Table.length; i++) {
    //                        $("#DdlVehicleName").append(`<option value="${arr.Table[i].setup_id}">${arr.Table[i].setup_val}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $("#DdlVehicleName").select2({
    //                        templateResult: function (data) {
    //                            //var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;

    //                        }
    //                    });
    //                }
    //            }
    //        },

    //    });


}

function BindDLLVehOEMNo() {
    debugger;
    $("#DdlOEMNo").select2({
        ajax: {
            type: "POST",
            url: "/ApplicationLayer/ProductCatalouge/GetVehOEMNoAuto",
            data: function (params) {
                var queryParameters = {
                    VehOEM_No: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            //contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                data = JSON.parse(data).Table;
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.oem_id, text: val.veh_oem_no };
                    })
                };
            }
        },
    });
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/ProductCatalouge/GetVehOEMNoAuto",
    //        data: function (params) {
    //            var queryParameters = {
    //                veh_oem_no: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            // debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $("#DdlOEMNo").append(`<option value="${arr.Table[i].oem_id}">${arr.Table[i].veh_oem_no}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $("#DdlOEMNo").select2({
    //                        templateResult: function (data) {
    //                            //var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;

    //                        }
    //                    });
    //                }
    //            }
    //        },

    //    });
}

function BindDLLReferenceNo() {
    debugger;
    $("#DdlReferenceNo").select2({
        ajax: {
            type: "POST",
            url: "/ApplicationLayer/ProductCatalouge/GetReferenceNoAuto",
            data: function (params) {
                var queryParameters = {
                    ReferenceNo: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            //contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                data = JSON.parse(data).Table;
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.item_leg_cd_ID, text: val.item_leg_cd };
                    })
                };
            }
        },
    });
    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/ProductCatalouge/GetReferenceNoAuto",
    //        data: function (params) {
    //            var queryParameters = {
    //                item_leg_cd: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $("#DdlReferenceNo").append(`<option value="${arr.Table[i].item_leg_cd_ID}">${arr.Table[i].item_leg_cd}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $("#DdlReferenceNo").select2({
    //                        templateResult: function (data) {
    //                            //var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-12' + classAttr + '">' + data.text + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;

    //                        }
    //                    });
    //                }
    //            }
    //        },

    //    });
}

function BindDLLTechSpecification() {
    debugger;
    $("#DdlTechnicalSpecification").select2({
        ajax: {
            type: "POST",
            url: "/ApplicationLayer/ProductCatalouge/GetTechnSpecAuto",
            data: function (params) {
                var queryParameters = {
                    TechSpecific: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            //contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                if (data == 'ErrorPage') {
                    ErrorPage();
                    return false;
                }
                data = JSON.parse(data).Table;
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.item_tech_spec_ID, text: val.item_tech_spec };
                    })
                };
            }
        },
    });
}

function OnChangeGroup() {
    var grpname = $("#group_name").val();
    if (grpname == "0") {

    }
    else {
        $("#ddl_ProductName").val("0").trigger('change');
        $("#UOM").val("0").trigger('change');
         $("#ddl_PortFolioName").val("0").trigger('change');
        $("#DdlReferenceNo").val("0").trigger('change');
        $("#DdlTechnicalSpecification").val("0").trigger('change');
        $("#CustomerNameList1").val("0").trigger('change');
        $("#DdlVehicleName").val("0").trigger('change');
        $("#DdlOEMNo").val("0").trigger('change');
     }
}
function OnChangePortfolio() {
    var prtfolio = $("#ddl_PortFolioName").val();
    if (prtfolio == "0") {

    }
    else {
        $("#ddl_ProductName").val("0").trigger('change');
        $("#UOM").val("0").trigger('change');
        $("#group_name").val("0").trigger('change');
        $("#DdlReferenceNo").val("0").trigger('change');
        $("#DdlTechnicalSpecification").val("0").trigger('change');
        $("#CustomerNameList1").val("0").trigger('change');
        $("#DdlVehicleName").val("0").trigger('change');
        $("#DdlOEMNo").val("0").trigger('change');
    }
}

function OnChangeRefNo() {
    var refno = $("#DdlReferenceNo").val();
    if (refno == "0") {

    }
    else {
        $("#ddl_ProductName").val("0").trigger('change');
        $("#UOM").val("0").trigger('change');
        $("#group_name").val("0").trigger('change');
        $("#ddl_PortFolioName").val("0").trigger('change');
        $("#DdlTechnicalSpecification").val("0").trigger('change');
        $("#CustomerNameList1").val("0").trigger('change');
        $("#DdlVehicleName").val("0").trigger('change');
        $("#DdlOEMNo").val("0").trigger('change');
    }
}

function OnChangeTechSpeci() {
    var techspeci = $("#DdlTechnicalSpecification").val();
    if (techspeci == "0") {

    }
    else {
        $("#ddl_ProductName").val("0").trigger('change');
        $("#UOM").val("0").trigger('change');
        $("#group_name").val("0").trigger('change');
        $("#ddl_PortFolioName").val("0").trigger('change');
        $("#DdlReferenceNo").val("0").trigger('change');
        $("#CustomerNameList1").val("0").trigger('change');
        $("#DdlVehicleName").val("0").trigger('change');
        $("#DdlOEMNo").val("0").trigger('change');
    }
}
function OnChangeCustName1() {
    var custname = $("#CustomerNameList1").val();
    if (custname == "0") {

    }
    else {
        $("#ddl_ProductName").val("0").trigger('change');
        $("#UOM").val("0").trigger('change');
        $("#group_name").val("0").trigger('change');
        $("#ddl_PortFolioName").val("0").trigger('change');
        $("#DdlReferenceNo").val("0").trigger('change');
        $("#DdlTechnicalSpecification").val("0").trigger('change');
        $("#DdlVehicleName").val("0").trigger('change');
        $("#DdlOEMNo").val("0").trigger('change');
    }
}

function OnChangeVehName() {
    var vehname = $("#DdlVehicleName").val();
    if (vehname == "0") {

    }
    else {
        $("#ddl_ProductName").val("0").trigger('change');
        $("#UOM").val("0").trigger('change');
        $("#group_name").val("0").trigger('change');
        $("#ddl_PortFolioName").val("0").trigger('change');
        $("#DdlReferenceNo").val("0").trigger('change');
        $("#DdlTechnicalSpecification").val("0").trigger('change');
        $("#CustomerNameList1").val("0").trigger('change');
        $("#DdlOEMNo").val("0").trigger('change');
    }
}


function OnChangeOEMNo() {
    var oemno = $("#DdlOEMNo").val();
    if (oemno == "0") {

    }
    else {
        $("#ddl_ProductName").val("0").trigger('change');
        $("#UOM").val("0").trigger('change');
        $("#group_name").val("0").trigger('change');
        $("#ddl_PortFolioName").val("0").trigger('change');
        $("#DdlReferenceNo").val("0").trigger('change');
        $("#DdlTechnicalSpecification").val("0").trigger('change');
        $("#CustomerNameList1").val("0").trigger('change');
        $("#DdlVehicleName").val("0").trigger('change');
    }
}
//----------------End of Searchable Criteria------------------------//
function AddItemDetail() {
    //$("#datatable-buttons #FilterItemDetail tr").each(function () {
        //var Rows = $(this);
        debugger
       var fltr = "";
        var fltrType = "";
        var ItemName = $("#ddl_ProductName").val();
        if (ItemName != "0" && ItemName != "") {
            fltr = ItemName;
            fltrType = "Item";
        }
        
        var GrpnName = $("#group_name").val();
        if (GrpnName != "0" && GrpnName != "") {
            fltr = GrpnName;
            fltrType = "Group";
        }
        
        
        var PrtFolio = $("#ddl_PortFolioName").val();
        if (PrtFolio != "0" && PrtFolio != "") {
            fltr = PrtFolio;
            fltrType = "PortFolio";
        }
        
        var RefNo = $("#DdlReferenceNo").val();
        if (RefNo != "0" && RefNo != "") {
            fltr = RefNo;
            fltrType = "ReferenceNo";
        }
        
        var TechSpeci = $("#DdlTechnicalSpecification").val();
        if (TechSpeci != "0" && TechSpeci != "") {
            fltr = TechSpeci;
            fltrType = "TechnicalSpecification";
        }
       
        var CustName1 = $("#CustomerNameList1").val();
        if (CustName1 != "0" && CustName1 != "") {
            fltr = CustName1;
            fltrType = "CustomerName";
        }
       
        var VehicName = $("#DdlVehicleName").val();
        if (VehicName != "0" && VehicName != "") {
            fltr = VehicName;
            fltrType = "VehicleName";
        }
        
        var OEMNo = $("#DdlOEMNo").val();
        if (OEMNo != "0" && OEMNo != "") {
            fltr = OEMNo;
            fltrType = "OEMNo";
        }
     if (fltr != "0" && fltr != "" && fltr != null) {
               BindFilterItemDetail(fltr, fltrType);
        }
 //});
}

function BindFilterItemDetail(fltr,fltrtype) {
    debugger;
   //check duplicacy in search criteria
    var itmExist = "No";
    var Itm_ID = $("#ddl_ProductName").val();
    var Itmcode = $("#datatable-buttons >tbody >tr").find("#hdnProdItem[value=" + Itm_ID + "]").val();
    if (Itmcode != null) {
        itmExist = "Yes";
    }
    //$("#datatable-buttons >tbody >tr").each(function (i, row) {
    //    var Row = $(this)
    //    debugger;
    //   var Itmcode = $(this).find("#hdnProdItem").val();
    //    if (Itm_ID == Itmcode) {
    //        itmExist = "Yes";
    //    }

    //});
    debugger
    if (itmExist == "No") {

        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ProductCatalouge/GetFilterItemDetail",
                data: { fltrvalue: fltr, fltrtype: fltrtype },
                dataType: "json",
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        LSO_ErrorPage();
                        return false;
                    }

                    if (data !== null && data !== "") {
                        var arr = [];
                        var t = $('#datatable-buttons').DataTable({
                            destroy: true, paging: false, ordering: true, info: true, searching: true, dom: 'Bfrtip', buttons: ['copy','csv','excel','pdf']
                        });
                        /*var t = $('#datatable-buttons').DataTable();*/

                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var RowId = 0;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger
                                //check dulicacy in table before add row
                                var tblexist = "No"
                                var tblItmId = arr.Table[i].item_id;
                                var Itmcode = $("#datatable-buttons >tbody >tr").find("#hdnProdItem[value=" + tblItmId+"]").val();
                                if (Itmcode != null) {
                                    tblexist = "Yes";
                                }
                                //$("#datatable-buttons >tbody >tr").each(function (i, row) {
                                //    var Row = $(this);
                                //    debugger;
                                //    var Itmcode = $(this).find("#hdnProdItem").val();
                                //    if (Itmcode == tblItmId) {
                                //        tblexist = "Yes";
                                //    }
                                //});
                                debugger
                                if (tblexist == "No") {
                                    
                                    t.row.add([
                                        `<td><input type="checkbox" value=""></td>`,                                        
                                        `<td><span id="SrNo_">${++RowId}</span></td>`,
                                        `<td><div class="col-sm-11" style="padding:0px;">${arr.Table[i].item_name}<input class="" type="hidden" id="hdnProdItem" value="${arr.Table[i].item_id}"></div><div class="col-sm-1 i_Icon"><button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"></button> </div></td >`,
                                        `<td>${arr.Table[i].uom_name}<input type="hidden" id="hdnPrdItmUOM" value="${arr.Table[i].uom_id}"></td>`,
                                        `<td>${arr.Table[i].groupname}<input class="" type="hidden" id="hdnPrdGrp" value="${arr.Table[i].item_grp_id}"></td>`,
                                        `<td>${arr.Table[i].prf_name}<input class="" type="hidden" id="hdnPrdPrf" value="${arr.Table[i].prf_id}"></td>`,
                                        `<td id="item_leg_cd" class="num_right">${arr.Table[i].item_leg_cd}</td>`,
                                        `<td id="item_tech_spec" class="num_right">${arr.Table[i].item_tech_spec}</td>`,
                                        `<td>${arr.Table[i].veh_name}<input class="" type="hidden" id="hdnPrdVehc" value="${arr.Table[i].veh_id}"></td>`,
                                        `<td id="model_no">${arr.Table[i].model_no}</td>`,
                                        `<td id="veh_oem_no">${arr.Table[i].veh_oem_no}</td>`,
                                        `<td id="item_cat_no">${arr.Table[i].item_cat_no}</td>`,
                                    ]).draw();
                                }
                            }
                            var sno = 0;
                            $("#datatable-buttons >tbody >tr").each(function (i, row) {
                                debugger;
                                sno = parseInt(sno) + 1
                                $(this).closest('tr').find("#SrNo_").text(sno);

                            });
                        }
                    }
                },

            });
    }
}
//-------Work on Delete button and Delete Trashbin on item table-----------//
function DeleteItem() {
    debugger
    // Get checked checkboxes and delete row from dynamic table
    var table = $('#datatable-buttons').DataTable();
    $('#datatable-buttons input[type=checkbox]').each(function (event) {
        if (jQuery(this).is(":checked")) {
            table.row($(this).parents('tr')).remove().draw();
            SerialNoAfterDelete();
        }
    });
   // SerialNoAfterDelete();
 
}

function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#datatable-buttons >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SrNo_").text(SerialNo);
    });
};
function pcfunctionConfirm(event) {
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
            debugger;
            $("#hdn_Action").val("Delete");
            //location.href = "/ApplicationLayer/ProductCatalouge/ProdCatalogActionCommands";
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
//-------End Delete------------//

//--------------Validation------------------//
function HeaderValidation() {
    debugger
    var ValidationFlag = true;
    if ($("#CustomerNameList").val() == '0' || $("#CustomerNameList").val() == "" || $("#CustomerNameList").val() == null) {
    $("[aria-labelledby='select2-CustomerNameList-container']").css("border-color", "red");
    $("#vmCustomerNameList").text($("#valueReq").text());
    $("#vmCustomerNameList").css("display", "block");
        ValidationFlag = false;
    }
    if (ValidationFlag == true) {
        return true;
    }
    else {
        return false;
        }
}
function ItemValidation() {
    debugger
    var ErrorFlag = "N";
    if ($("#datatable-buttons tbody tr").length > 0) {
        $("#datatable-buttons tbody tr").each(function () {
            debugger
            var currentrow = $(this);
           /* var Rowno = currentrow.find("#hSRNoID").val();*/
            var ItemName = currentrow.find("#hdnProdItem").val();
            if (ItemName == "0" || ItemName == null || ItemName=="") {
                swal("", $("#noitemselectedmsg").text(), "warning");
                ErrorFlag = "Y";
            }
          });
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
//-------------Valida end---------------//
function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    if ($("#CustomerTypeC").is(":checked")) {
        CustPros_type = "C";
        $("#Prosbtn").css("display", "none");
    }
    debugger;
    if ($("#CustomerTypeP").is(":checked")) {
        CustPros_type = "P";
        $("#Prosbtn").css("display", "block");
        
    }
    $("#hdn_CustPros_type").val(CustPros_type);
    if (HeaderValidation() == false) {
        debugger
        return false;
    }
   
    if (ItemValidation() == false) {
        debugger
        return false;
    }
    var FinalItemDetail = [];
    debugger
    FinalItemDetail = InsertProdCataItemDetail();
    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");

    return true;
  }




function InsertProdCataItemDetail() {
    debugger;
    var ItemDetailList = new Array();
    $("#datatable-buttons TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        //var rowid = currentRow.find("#SNohiddenfiled").val();
        var ItemList = {};
        ItemList.item_id = currentRow.find("#hdnProdItem").val();
        ItemList.uom_id = currentRow.find("#hdnPrdItmUOM").val();
        ItemList.item_grp_id = currentRow.find("#hdnPrdGrp").val();
        ItemList.prf_id = currentRow.find("#hdnPrdPrf").val();
        ItemList.item_ref_no = currentRow.find("#item_leg_cd").text();
        ItemList.item_tech_spec = currentRow.find("#item_tech_spec").text();
        ItemList.veh_id = currentRow.find("#hdnPrdVehc").val();
        ItemList.model_no = currentRow.find("#model_no").text();
        ItemList.veh_oem_no = currentRow.find("#veh_oem_no").text();
        ItemList.item_cat_no = currentRow.find("#item_cat_no").text();
        

        ItemDetailList.push(ItemList);
        debugger;
    });

    return ItemDetailList;
};
//------ItemInformation Bind on Item (I) icon buton----------------//
function OnClickIconBtn(e) {
   /* var row_id = $(e.target).closest('tr').find("#hSRNoID").val();*/
    var ItmCode = $(e.target).closest('tr').find("#hdnProdItem" ).val();
    ItemInfoBtnClick(ItmCode);
}

//-----------------------------Work on Search Button of Product Catalogue List -------------------------------------------//
function BtnSearch() {
    debugger
    FilterProdCatList();
    ResetWF_Level();
}

function FilterProdCatList() {
    debugger;
    try {
        var CustID = $("#CustomerNameList2").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ProductCatalouge/SearchProdCataList",
            data: {
                CustID: CustID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#ItmListBody').html(data);
                $('#ListFilterData').val(CustID + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    }
    catch (err) {
        debugger;
        console.log("MRS Error : " + err.message);

    }
}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
        if (FromDate > ToDate) {

            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            $("#txtTodate").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + month + '-' + day;
            $("#txtFromdate").val(today);
            swal("", $("#fromtodatemsg").text(), "warning");

            /* $("#txtFromdate").val(FromDate);*/
        }
    }

}

//-----------------------------------Forward And Workflow and Approve----------------------------//
//--------------------Approve section-----//
function ForwardBtnClick() {
    debugger;
    //var ProdcataStatus = "";
    //ProdcataStatus = $('#hfStatus').val().trim();
    //if (ProdcataStatus === "D" || ProdcataStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        $.ajax({
            type: "POST",
            url: "/Common/Common/CheckFinancialYear",
            data: {
                compId: compId,
                brId: brId
            },
            success: function (data) {
                if (data == "Exist") { /*End to chk Financial year exist or not*/
                    var ProdcataStatus = "";
                    ProdcataStatus = $('#hfStatus').val().trim();
                    if (ProdcataStatus === "D" || ProdcataStatus === "F") {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#Btn_Forward").attr("data-target", "");
                        }
                        else {
                            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");
                        }
                        var Doc_ID = $("#DocumentMenuId").val();
                        $("#OKBtn_FW").attr("data-dismiss", "modal");

                        Cmn_GetForwarderList(Doc_ID);

                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "");
                        $("#Btn_Forward").attr('onclick', '');
                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                }
                else {/* to chk Financial year exist or not*/
                    swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");

                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    /*End to chk Financial year exist or not*/
    return false;

}
//check on PartialForward of this function
function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var CTLNo = "";
    var CTLDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";

    Remarks = $("#fw_remarks").val();
    CTLNo = $("#catalogNumber").val();
    CTLDate = $("#txtCatalDate").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (CTLNo + ',' + CTLDate + ',' + WF_status1)
    var ListFilterData1 = $("#ListFilterData1").val();

    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && CTLNo != "" && CTLDate != "" && level != "") {
            debugger;
            Cmn_InsertDocument_ForwardedDetail(CTLNo, CTLDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ProductCatalouge/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/ProductCatalouge/ApproveProdCatalogDetails?CTLNo=" + CTLNo + "&CTLDate=" + CTLDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;
        // InsertPOApproveDetails("Approve", $("#hd_currlevel").val(), Remarks);
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && CTLNo != "" && CTLDate != "") {
           Cmn_InsertDocument_ForwardedDetail(CTLNo, CTLDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ProductCatalouge/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && CTLNo != "" && CTLDate != "") {
            Cmn_InsertDocument_ForwardedDetail(CTLNo, CTLDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ProductCatalouge/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
}

function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#catalogNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

function OtherFunctions(StatusC, StatusName) {

}

function ResetWF_Level() {
    var li_count = $("#wizard ul li").length;
    if (li_count > 0) {
        for (var y = 0; y < li_count; y++) {
            var id = parseInt(y) + 1;
            $("#a_" + id).removeClass("done");
            $("#a_" + id).removeClass("selected");
            $("#a_" + id).addClass("disabled");
        }
    }
}

//----------End of Workflow------------//

function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}

function OnChangeCustProsType() {
    debugger;
    //DisableItemDetail();

    $('#CustomerNameList').empty().append('<option value="0" selected="selected">---Select---</option>');
    $('#SpanCustNameErrorMsg').text("");
   // $("#TxtBillingAddr").val("");
    $("#txtRemarks").val("");
   // $("#TxtShippingAddr").val("");
   // $("#txtCurrency").val("");
   // $("#conv_rate").val("");
    //$('#sales_person').empty().append('<option value="0" selected="selected">---Select---</option>');
    //$("#dremarks").val("");
    $("#hdn_product_id").val("0");
    $("#vmCustomerNameList").css("display", "none");
    $("#CustomerNameList").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-CustomerNameList-container']").css("border-color", "#ced4da");
}
function AddNewProspect() {
    try {
        var ProspectFromProd = "Y";
        var SrcDocumentMenuID = $("#DocumentMenuId").val();
        window.location.href = "/BusinessLayer/ProspectSetup/CreateReferencePageProspectSetup/?ProspectFromProd=" + ProspectFromProd + "&SrcDocumentMenuID=" + SrcDocumentMenuID;

    }
    catch (err) {
        console.log(PFName + " Error : " + err.message);
    }

}

/*----------------For Print Popup Add by Hina Sharma on 24-07-2025-------------------------------*/
function OnCheckedChangeProdDesc() {
    debugger
    if ($('#chkproddesc').prop('checked')) {
        //$('#chkshowcustspecproddesc').prop('checked', false);
        //$('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $("#ShowProdDesc").val("Y");
        $('#txtproddesc').prop("disabled", false)
        
        //$('#ShowCustSpecProdDesc').val('N');
        //$('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $("#ShowProdDesc").val("N");
        $('#txtproddesc').prop("disabled", true)
        $("#txtproddesc").val("");
    }
}
function OnCheckedChangeUOM() {
    debugger
    if ($('#chkshowUOM').prop('checked')) {
        $('#hdn_ShowUOM').val('Y');
        $('#txtuom').prop("disabled", false)
    }
    else {
        $('#hdn_ShowUOM').val('N');
        $('#txtuom').prop("disabled", true)
        $("#txtuom").val("");
    }
}
function OnCheckedChangeItmAliasName() {/*Add by Hina sharma on 13-11-2024 */
    debugger;
    if ($('#chkshowItemAliasName').prop('checked')) {
        $('#Hdn_ShowItemAliasName').val('Y');
        $('#txtitemalias').prop("disabled", false)
    }
    else {
        $('#Hdn_ShowItemAliasName').val('N');
        $('#txtitemalias').prop("disabled", true)
        $("#txtitemalias").val("");
    }
}
function OnCheckedChangeProdTechDesc() {
    if ($('#chkprodtechdesc').prop('checked')) {
        $('#ShowProdTechDesc').val('Y');
        $('#txtprodtechdesc').prop("disabled", false)
    }
    else {
        $('#ShowProdTechDesc').val('N');
        $('#txtprodtechdesc').prop("disabled", true)
        $("#txtprodtechdesc").val("");
    }
}
function OnCheckedChangeOEM() {
    if ($('#chkshowOEM').prop('checked')) {
        $('#Hdn_ShowOEMNumber').val('Y');
        $('#txtoemnum').prop("disabled", false)
    }
    else {
        $('#Hdn_ShowOEMNumber').val('N');
        $('#txtoemnum').prop("disabled", true)
        $("#txtoemnum").val("");
    }
}
function OnCheckedChangeItmTechSpecific() {
    debugger;
    if ($('#chkshowItmTechSpecific').prop('checked')) {
        $('#Hdn_ShowProdTechSpec').val('Y');
        $('#txtprodtechspec').prop("disabled", false)
    }
    else {
        $('#Hdn_ShowProdTechSpec').val('N');
        $('#txtprodtechspec').prop("disabled", true)
        $("#txtprodtechspec").val("");
    }
}
function OnCheckedChangeCatalogueNum() {
    debugger;
    if ($('#chkshowCatalogueNum').prop('checked')) {
        $('#Hdn_ShowCatalougeNumber').val('Y');
        $('#txtcatnum').prop("disabled", false)
    }
    else {
        $('#Hdn_ShowCatalougeNumber').val('N');
        $('#txtcatnum').prop("disabled", true)
        $("#txtcatnum").val("");
    }
}
function OnCheckedChangePrintItemImage() {
    debugger;
    if ($('#chkshowItemImage').prop('checked')) {
        $('#HdnPrintItemImage').val('Y');
    }
    else {
        $('#HdnPrintItemImage').val('N');
    }
}
function OnCheckedChangeRefNum() {
    debugger;
    if ($('#chkshowRefNum').prop('checked')) {
        $('#Hdn_ShowRefNumber').val('Y');
        $('#txtrefnum').prop("disabled", false)
    }
    else {
        $('#Hdn_ShowRefNumber').val('N');
        $('#txtrefnum').prop("disabled", true)
        $("#txtrefnum").val("");
    }
}
function OnCheckedChangeVehicleName() {
    debugger;
    if ($('#chkshowVehicleName').prop('checked')) {
        $('#Hdn_ShowVehicleName').val('Y');
        $('#txtvehiclname').prop("disabled", false)
    }
    else {
        $('#Hdn_ShowVehicleName').val('N');
        $('#txtvehiclname').prop("disabled", true)
        $("#txtvehiclname").val("");
    }
}
function OnCheckedChangeModelNum() {
    debugger;
    if ($('#chkshowModelNum').prop('checked')) {
        $('#Hdn_ShowModelNumber').val('Y');
        $('#txtmodelnum').prop("disabled", false)
    }
    else {
        $('#Hdn_ShowModelNumber').val('N');
        $('#txtmodelnum').prop("disabled", true)
        $("#txtmodelnum").val("");
    }
}

/*Code start add by Hina on 01-09-2025*/
function OnTextChangeProdDesc(e) {
    debugger
    if ($('#chkproddesc').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtproddesc").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        
        var ProdDesc = $("#txtproddesc").val();
        $("#hdn_lblProdDesc").val(ProdDesc);
    }
    else {
        $("#hdn_lblProdDesc").val("");
    }
}
function OnTextChangeUOM(e) {
    debugger
    if ($('#chkshowUOM').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtuom").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        
        var UOM = $("#txtuom").val();
        $("#hdn_lblUOM").val(UOM);
    }
    else {
        $("#hdn_lblUOM").val("");
    }
}
function OnTextChangeItemAlias(e) {
    debugger
    if ($('#chkshowItemAliasName').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtitemalias").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var ItmAlias = $("#txtitemalias").val();
        $("#hdn_lblItemAlias").val(ItmAlias);
    }
    else {
        $("#hdn_lblItemAlias").val("");
    }
}
function OnTextChangeProdTechDesc(e) {
    debugger
    if ($('#chkprodtechdesc').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtprodtechdesc").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var ProdDesc = $("#txtprodtechdesc").val();
        $("#hdn_lblProdTechDesc").val(ProdDesc);
    }
    else {
        $("#hdn_lblProdTechDesc").val("");
    }
}
function OnTextChangeOEM(e) {
    debugger
    if ($('#chkshowOEM').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtoemnum").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var ProdDesc = $("#txtoemnum").val();
        $("#hdn_lblOEMNumber").val(ProdDesc);
    }
    else {
        $("#hdn_lblOEMNumber").val("");
    }
}
function OnTextChangeProdTechSpec(e) {
    debugger
    if ($('#chkshowItmTechSpecific').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtprodtechspec").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var ProdTechSpec = $("#txtprodtechspec").val();
        $("#hdn_lblProdTechSpec").val(ProdTechSpec);
    }
    else {
        $("#hdn_lblProdTechSpec").val("");
    }
}
function OnTextChangeCatNum(e) {
    debugger
    if ($('#chkshowCatalogueNum').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtcatnum").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var CatNum = $("#txtcatnum").val();
        $("#hdn_lblCatalougeNum").val(CatNum);
    }
    else {
        $("#hdn_lblCatalougeNum").val("");
    }
}
function OnTextChangeRefNum(e) {
    debugger
    if ($('#chkshowRefNum').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtrefnum").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var RefNum = $("#txtrefnum").val();
        $("#hdn_lblRefNumber").val(RefNum);
    }
    else {
        $("#hdn_lblRefNumber").val("");
    }
}
function OnTextChangeVehicleName(e) {
    debugger
    if ($('#chkshowVehicleName').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtvehiclname").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var VehName = $("#txtvehiclname").val();
        $("#hdn_lblVehicleName").val(VehName);
    }
    else {
        $("#hdn_lblVehicleName").val("");
    }
}
function OnTextChangeModelNum(e) {
    debugger
    if ($('#chkshowModelNum').prop('checked')) {
        // Apply on blur (when user leaves input)
        $("#txtmodelnum").on("blur", function () {
            var text = $(this).val();
            $(this).val(capitalizeWords(text));
        });
        var ModelNum = $("#txtmodelnum").val();
        $("#hdn_lblModelNumber").val(ModelNum);
    }
    else {
        $("#hdn_lblModelNumber").val("");
    }
}
function capitalizeWords(str) {
    debugger
    return str.toLowerCase().replace(/\b\w/g, function (char) {
        return char.toUpperCase();
    });
}
// Apply on blur (when user leaves input)
//$("#txtproddesc").on("blur", function () {
//    var text = $(this).val();
//    $(this).val(capitalizeWords(text));
//});
/*Code End add by Hina on 01-09-2025*/