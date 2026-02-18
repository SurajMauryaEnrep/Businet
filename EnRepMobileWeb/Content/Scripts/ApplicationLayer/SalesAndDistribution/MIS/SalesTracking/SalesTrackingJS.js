$(document).ready(function () {
    //$('#ddlOrderNumberlist').select2();
    //$('#ddlCustomersList').select2();
    $('#ddlItemsList').select2();
   // $('#ddlCurrencylist').select2();
    BindOrderNumberList(); 
    DynamicSerchableItemDDL("", "#ddlItemsList", "", "", "", "SlsTracking","Y")
    BindCustomerList();/*Add by Hina shrama on 04-12-2024*/
    Cmn_initializeMultiselect([
        '#ddlOrderType',
        '#ddlSalesPerson',
        '#ddlCurrencylist',
        '#CustomerCategory',
        '#CustomerPortfolio',
        '#ddlcustzone',
        '#ddlcustgroup',
        '#ddl_branch',
    ]);
    BindCityList();
    BindStateList();
});
function GetPOTrackingMisDetails() {
    //var OldOrderType1 = $("#hdnOrdertype1_old").val();
    //if (OldOrderType1 != "" && OldOrderType1 != null) {

    //    if (OldOrderType1 == "O") {
    //        $("#Deatil")[0].id = "datatable-buttons";
    //        $("#Pending")[0].id = "datatable-buttons1";
    //        //$("#dttbl1")[0].id = "datatable-buttons2";
    //    }
    //    else if (OldOrderType1 == "P") {
    //        $("#Deatil")[0].id = "datatable-buttons";
    //        //   $("#dttbl1")[0].id = "datatable-buttons1";     
    //        $("#OverDue")[0].id = "datatable-buttons2";
    //    }
    //    else {

    //        //$("#dttbl1")[0].id = "datatable-buttons";        
    //        $("#Pending")[0].id = "datatable-buttons1";
    //        $("#OverDue")[0].id = "datatable-buttons2";
    //    }
    //}
    var NotFillterOrderType = $("#OrderType").val();
    var orderNumber = $('#ddlOrderNumberlist').val();
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    //var custId = $('#ddlCustomersList').val();
    var custId = cmn_getddldataasstring('#ddlCustomersList');
    var slsPersId = $('#ddlSalesPerson').val();
    var itemId = $("#ddlItemsList").val();
    var currId = cmn_getddldataasstring('#ddlCurrencylist');
    var orderType = cmn_getddldataasstring('#ddlOrderType');
    var custCat = cmn_getddldataasstring("#CustomerCategory");
    var custPort = cmn_getddldataasstring("#CustomerPortfolio");
    var cust_zone = cmn_getddldataasstring("#ddlcustzone");
    var cust_group = cmn_getddldataasstring("#ddlcustgroup");
    var state = $("#ddl_state").val();
    var city = $("#ddl_city").val();
   // var currId = $('#ddlCurrencylist').val();
    //var orderType = $('#ddlOrderType').val();
    if (orderNumber == 'undefined') {
        orderNumber = '0';
    }
    //if (NotFillterOrderType == "O") {
    //    $("#hdnOrdertype1_old").val(NotFillterOrderType);
    //    $("#datatable-buttons")[0].id = "Deatil";
    //    $("#datatable-buttons1")[0].id = "Pending";
    //    $("#datatable-buttons2")[0].id = "dttbl1";
    //}
    //else if (NotFillterOrderType == "P") {
    //    $("#hdnOrdertype1_old").val(NotFillterOrderType);
    //    $("#datatable-buttons")[0].id = "Deatil";
    //    $("#datatable-buttons1")[0].id = "dttbl1";
    //    $("#datatable-buttons2")[0].id = "OverDue";
    //}
    //else {
    //    $("#hdnOrdertype1_old").val(NotFillterOrderType);
    //    $("#datatable-buttons")[0].id = "dttbl1";
    //    $("#datatable-buttons1")[0].id = "Pending";
    //    $("#datatable-buttons2")[0].id = "OverDue";
    //}
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesTracking/SearchSoTrackingDetails",
        data: {
            soNo: orderNumber, custId: custId, slsPersId: slsPersId, orderType: orderType, itemId: itemId, currId: currId, fromDate: fromDate, toDate: toDate,
            NotFillterOrderType: NotFillterOrderType, custCat: custCat, custPort: custPort, cust_zone: cust_zone, cust_group: cust_group, state: state, city: city
        },
        success: function (data) {
            debugger;

            if (NotFillterOrderType == "O") {
                $("#MISsoTrackingOverdues").html(data);

            }
            else if (NotFillterOrderType == "P") {

                $("#MISsoTrackingDetails_Pending").html(data);

            }
            else {
                $("#MISsoTrackingDetails").html(data);
            }

           
            try {
                $("#dttbl1")[0].id = "datatable-buttons";
            }
            catch {}
        }
    });
}
function BindOrderListByFilter() {
    debugger;
    var custId = $('#ddlCustomersList').val();
    var orderType = $("#ddlOrderType").val();
    var currId = $('#ddlCurrencylist').val();
    $('#ddlOrderNumberlist').empty();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesTracking/GetOrderNumberList1",
        datatype:"json",
        data: {
            orderType: orderType, custId: custId, currId: currId
        },
        success: function (data) {
            debugger;
            for (var i = 0; i < data.length; i++) {
                var opt = new Option(data[i].OrderNumber, data[i].po_Value);
                $('#ddlOrderNumberlist').append(opt);
            }
        }
    });
}
//function BindOrderNumberList() {
//    debugger;
//    //var custId = $('#ddlCustomersList').val();
//    //var orderType = $("#ddlOrderType").val();
//    //var currId = $('#ddlCurrencylist').val();
//    var custId = cmn_getddldataasstring('#ddlCustomersList');
//    var orderType = cmn_getddldataasstring('#ddlOrderType');
//    var currId = cmn_getddldataasstring('#ddlCurrencylist');
//    $.ajax({
//        type: 'POST',
//        url: "/ApplicationLayer/SalesTracking/GetOrderNumberListByOrder",
//        datatype: "json",
//        data: {
//            orderType: orderType, custId: custId, currId: currId
//        },
//        success: function (data) {
//            debugger;
//            if (data == 'ErrorPage') {
//                PO_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "" && data !== "{}") {
//                var arr = [];
//                arr = JSON.parse(data);
//                if (arr.length > 0) {

//                    $("#ddlOrderNumberlist option").remove();
//                    $("#ddlOrderNumberlist optgroup").remove();
//                    $('#ddlOrderNumberlist').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
//                    for (var i = 0; i < arr.length; i++) {
//                        $('#Textddl').append(`<option data-date="${arr[i].DocDate}" value="${arr[i].so_no}">${arr[i].OrderNumber}</option>`);
//                    }
//                    var firstEmptySelect = true;
//                    $('#ddlOrderNumberlist').select2({
//                        templateResult: function (data) {
//                            var DocDate = $(data.element).data('date');
//                            var classAttr = $(data.element).attr('class');
//                            var hasClass = typeof classAttr != 'undefined';
//                            classAttr = hasClass ? ' ' + classAttr : '';
//                            var $result = $(
//                                '<div class="row">' +
//                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
//                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
//                                '</div>'
//                            );
//                            return $result;
//                            firstEmptySelect = false;
//                        }
//                    });

//                    $("#src_doc_date").val("");

//                }
//            }
//        }

//    })

//}
function BindOrderNumberList() {
    debugger;
    var custId = cmn_getddldataasstring('#ddlCustomersList');
    var orderType = cmn_getddldataasstring('#ddlOrderType');
    var currId = cmn_getddldataasstring('#ddlCurrencylist');

    $.ajax({
        type: 'POST',
        url: "/ApplicationLayer/SalesTracking/GetOrderNumberListByOrder",
        dataType: "json",
        data: {
            orderType: orderType,
            custId: custId,
            currId: currId
        },
        success: function (data) {
            debugger;
            if (data === 'ErrorPage') {
                PO_ErrorPage();
                return;
            }
            if (data !== null && data !== "" && data !== "{}") {
                var arr = JSON.parse(data);
                if (arr && arr.length > 0) {
                    var $ddl = $('#ddlOrderNumberlist');
                    $ddl.empty();
                    arr.forEach(function (item) {
                        $ddl.append(
                            `<option data-date="${item.DocDate}" value="${item.so_no}">
                                ${item.OrderNumber} (${item.DocDate})
                            </option>`
                        );
                    });
                    if ($ddl.data('multiselect')) {
                        $ddl.multiselect('rebuild'); // <-- just rebuild with new options
                    } else {
                        Cmn_initializeMultiselect(['#ddlOrderNumberlist'], {
                            numberDisplayed: 2,
                            templates: {
                                li: '<li><a tabindex="0"><label class="m-0 p-0"></label></a></li>'
                            },
                            optionLabel: function (element) {
                                var docDate = $(element).data('date');
                                return `${$(element).text()} - ${docDate || ''}`;
                            },
                            buttonText: function (options) {
                                if (options.length === 0) {
                                    return this.nonSelectedText;
                                } else if (options.length > this.numberDisplayed) {
                                    return options.length + ' selected';
                                } else {
                                    var selected = [];
                                    options.each(function () {
                                        selected.push($(this).text());
                                    });
                                    return selected.join(', ');
                                }
                            }
                        });
                    }
                    $("#src_doc_date").val("");
                }
            }
        },
        error: function (xhr, status, error) {
            console.error("Error in BindOrderNumberList:", error);
        }
    });
}


function BindItemInfoBtnClick(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemId = clickedrow.find("#hdnItemId").text();
    ItemInfoBtnClick(itemId);
}
function GetPTDeliverySchudule(e) {
    debugger;
    var poNo = $(e.target).closest('tr').find("#orderNo").text();
   // var poDate = $(e.target).closest('tr').find("#poDt").text();
    var itemId = $(e.target).closest('tr').find("#hdnItemId").text();
    var orderType = $(e.target).closest('tr').find("#orderType").text();
    var br_id = $(e.target).closest('tr').find("#br_id").text();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesTracking/DeliveryShudule",
        data: {
            orderNo: poNo,
            // poDate: poDate,
            item_id: itemId,
            OrderType: orderType, br_id
        },
        success: function (data) {
            debugger;
            $("#ddldeliverysch").html(data);
            //try {
            //    $("#dttbl1")[0].id = "datatable-buttons";
            //}
            //catch { }
        }
    });
}
function OnChangeStockByddl() {
    debugger;
    var OrderType = $("#OrderType").val();
    if (OrderType == "P") {
        $("#MISsoTrackingOverdues").css("display", "none")
        $("#MISsoTrackingDetails").css("display", "none");
        $("#MISsoTrackingDetails_Pending").css("display", "block")
    }
    else if (OrderType == "O") {
        $("#MISsoTrackingDetails").css("display", "none");
        $("#MISsoTrackingDetails_Pending").css("display", "none");
        $("#MISsoTrackingOverdues").css("display", "block");
    }
    else {
        $("#MISsoTrackingOverdues").css("display", "none")
        $("#MISsoTrackingDetails_Pending").css("display", "none")
        $("#MISsoTrackingDetails").css("display", "block")
    }
    GetPOTrackingMisDetails();

}
function BindCustomerList() {/*Add by Hina shrama on 04-12-2024*/
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    $.ajax({
        url: "/ApplicationLayer/SalesDetail/GetAutoCompleteSearchCustList",
        data: {},
        success: function (data, params) {
            debugger;
            if (data == 'ErrorPage') {
                LSO_ErrorPage();
                return false;
            }
            var s = "";
            //var s = "<option value='0'>---All---</option>";
            $.map(data, function (val, item) {
                if (val.Name.trim() != "---Select---") {
                    s += '<option value=' + val.ID + '>' + val.Name + '</option>';
                }
            })
            $("#ddlCustomersList").html(s);
            Cmn_initializeMultiselect(['#ddlCustomersList']);
            $('#ddlCustomersList').multiselect('rebuild');
            //$("#ddlCustomersList").select2({});
        }
    });

}
function GetPOTrackingMisCSVDetails() {
    debugger;
    var dt = $("#Tbl_SoTrackingDetails").DataTable(); var order = dt.order();  // [[colIndex, 'asc/desc']]
    var colIndex = order[0][0];
    var direction = order[0][1];
    var search = dt.search();
    var columnName = dt.settings().init().columns[colIndex].data; console.log(columnName); console.log(direction)

    $("#HdnSearchValue").val(search);
    $("#HdnsortColumn").val(columnName);
    $("#HdnsortColumnDir").val(direction);
}
function BindCityList() {
    debugger;
    var state_id = $("#ddl_state").val()
    $("#ddl_city").append("<option value='0'>---Select---</option>");
    $("#ddl_city").select2({
        ajax: {
            url: "/ApplicationLayer/SalesDetail/BindCityListdata",
            data: function (params) {
                var queryParameters = {
                    SearchCity: params.term,
                    state_id: state_id,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize
                pageSize = 20; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                  <div class="row"><div class="col-md-6 col-xs-6 def-cursor">${$("#span_City").text()}</div>
                  <div class="col-md-3 col-xs-6 def-cursor">${$("#span_State").text()}</div>
                  <div class="col-md-3 col-xs-6 def-cursor">${$("#span_Country").text()}</div></div>
                  </strong></li></ul>`)
                }
                //var page = 1;
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize)
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0,0,0", Name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }
                debugger;
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split(",")[0], text: val.Name, UOM: val.ID.split(",")[1], type: val.ID.split(",")[2] };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.type + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function BindStateList() {
    debugger;
    $("#ddl_state").append("<option value='0'>---Select---</option>");
    $("#ddl_state").select2({
        ajax: {
            url: "/ApplicationLayer/SalesDetail/BindStateListData",
            data: function (params) {
                var queryParameters = {
                    SearchState: params.term,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize
                pageSize = 20; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                   <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#span_State").text()}</div>
                   <div class="col-md-3 col-xs-6 def-cursor">${$("#span_Country").text()}</div></div>
                   </strong></li></ul>`)
                }
                //var page = 1;
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize)
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0,0", Name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }
                debugger;
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split(",")[0], text: val.Name, State: val.ID.split(",")[1] };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            },
            cache: true
        },
        templateResult: function (data) {
            
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.State + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function onchangeState() {
    BindCityList();
}