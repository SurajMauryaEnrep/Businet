$(document).ready(function () {
    //$('#ddlOrderNumberlist').select2();
   // $('#ddlSuppliersList').select2();

    $('#ddlItemsList').select2();
    DynamicSerchableItemDDL("", "#ddlItemsList", "", "", "", "AllItems","Y")

    //$('#ddlCurrencylist').select2();
    BindOrderNumberList();
    Cmn_initializeMultiselect([
        '#ddlOrderType',
        '#ddlSuppliersList',
        '#ddlCurrencylist',
        '#SupplierCategory',
        '#SupplierPortfolio',
        '#ddl_branch',
    ]);
    
});
function GetPOTrackingMisDetails() {
    debugger
    var OldOrderType1 = $("#hdnOrdertype1_old").val();
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
    var orderNumber = $('#ddlOrderNumberlist option:selected').text();
    var fromDate = $("#txtFromdate").val();
    var toDate = $("#txtTodate").val();
    var suppId = $('#ddlSuppliersList').val();
    var itemId = $("#ddlItemsList").val();
    var currId = $('#ddlCurrencylist').val();
    var orderType = $('#ddlOrderType').val();
    var ItemType = $('#ddlItemType').val();
    if (orderNumber == 'undefined') {
        orderNumber = '0'
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
    if (NotFillterOrderType == "A")
        POTrackingDtList();
    else if (NotFillterOrderType == "P")
        POTrackingPendingDtList();
    else if (NotFillterOrderType == "O")
        POTrackingOverDueDtList();
    //$.ajax({
    //    type: "POST",
    //    url: "/ApplicationLayer/PurchaseTracking/SearchPoTrackingDetails",
    //    data: {
    //        poNo: orderNumber, suppId: suppId, orderType: orderType, itemId: itemId, currId: currId, fromDate: fromDate, toDate: toDate, ItemType: ItemType,
    //        NotFillterOrderType: NotFillterOrderType
    //    },
    //    success: function (data) {
    //        debugger;
    //        if (NotFillterOrderType == "O") {
    //            $("#MISpoTrackingDeatilOverdue").html(data);

    //        }
    //        else if (NotFillterOrderType == "P") {

    //            $("#MISpoTrackingDetailsPending").html(data);

    //        }
    //        else {
    //            $("#MISpoTrackingDetails").html(data);
    //        }
           
    //        try {
    //            //$("#dttbl1")[0].id = "datatable-buttons";
    //        }
    //        catch {}
    //    }
    //});
}
//function BindOrderListByFilter() {
//    debugger;
//    var suppId = $('#ddlSuppliersList').val();
//    var orderType = $("#ddlOrderType").val();
//    var currId = $('#ddlCurrencylist').val();
//    $('#ddlOrderNumberlist').empty();

//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/PurchaseTracking/GetOrderNumberListByOrder",
//        datatype:"json",
//        data: {
//            orderType: orderType, suppId: suppId, currId: currId
//        },
//        success: function (data) {
//            debugger;
//            $("#ddlOrderNumberlist option").remove();
//            $("#ddlOrderNumberlist optgroup").remove();
//            $('#ddlOrderNumberlist').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
//            for (var i = 0; i < data.length; i++) {
//                var opt = new Option(data[i].OrderNumber, data[i].po_Value);
//                $('#ddlOrderNumberlist').append(opt);
//            }
//        }
//    });
//}
//function BindOrderNumberList() {
//    debugger;
//    //var suppId = $('#ddlSuppliersList').val();
//    //var orderType = $("#ddlOrderType").val();
//    //var currId = $('#ddlCurrencylist').val();
//    $("#ddlOrderNumberlist").select2({

//        ajax: {
//            url: "/ApplicationLayer/PurchaseTracking/GetOrderNumberListByOrder",
//            data: function (params) {
//                var queryParameters = {
//                    orderType: $("#ddlOrderType").val(), suppId: $('#ddlSuppliersList').val(), currId: $('#ddlCurrencylist').val(),
//                    SearchName: params.term,
//                    page: params.page || 1
//                };
//                return queryParameters;
//            },
//            multiple: true,
//            cache: true,
//            processResults: function (data, params) {
//                debugger;
//                var pageSize=20;
//                var page = params.page || 1;
//                data = JSON.parse(data);
//                if ($(".select2-search__field").parent().find("ul").length == 0) {
//                    $(".select2-search__field").parent().append(`<ul class="select2-results__options">
//<li class="select2-results__option"><strong class="">
//<div class="row bpbr">
//<div class="col-md-6 col-xs-6 def-cursor">${$("#DocNo").text()}</div>
//<div class="col-md-6 col-xs-6 def-cursor">${$("#DocDate").text()}</div>
//</div>
//</strong></li></ul>`)
//                }
//                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
//                if (page == 1) {
//                    if (Fdata[0] != null) {
//                        if (Fdata[0].OrderNumber.trim() != "---All---") {
//                            var select = { DATE1: "0", DocDate: "0", OrderNumber: " ---All---", so_no:"0" };
//                            Fdata.unshift(select);
//                        }
//                    }
//                }

//                return {
//                    results: $.map(Fdata, function (val, Item) {
//                        return { id: val.so_no, text: val.OrderNumber, DocDate: val.DocDate };
//                    }),
//                    pagination: {
//                        more: (page * pageSize) < data.length
//                    }
//                };
//            },
//            cache: true
//        },
//        templateResult: function (data) {

//            var classAttr = $(data.element).attr('class');
//            var hasClass = typeof classAttr != 'undefined';
//            classAttr = hasClass ? ' ' + classAttr : '';
//            var $result;
//            $result = $(
//                '<div class="row">' +
//                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
//                '<div class="col-md-6 col-xs-6' + classAttr + '">' + IsNull(data.DocDate, '') + '</div>' +
//                '</div>'
//            );
//            return $result;
//            firstEmptySelect = false;
//        },
//    });

//     //$.ajax({
//     //       type: 'POST',
//     //    url: "/ApplicationLayer/PurchaseTracking/GetOrderNumberListByOrder",
//     //       datatype: "json",
//     //       data: {
//     //           orderType: orderType, suppId: suppId, currId: currId
//     //       },
//     //       success: function (data) {
//     //           debugger;
//     //           if (data == 'ErrorPage') {
//     //               PO_ErrorPage();
//     //               return false;
//     //           }
//     //           if (data !== null && data !== "" && data !== "{}") {
//     //               var arr = [];
//     //               arr = JSON.parse(data);
//     //               if (arr.length > 0) {

//     //                   $("#ddlOrderNumberlist option").remove();
//     //                   $("#ddlOrderNumberlist optgroup").remove();
//     //                   $('#ddlOrderNumberlist').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
//     //                   for (var i = 0; i < arr.length; i++) {
//     //                       $('#Textddl').append(`<option data-date="${arr[i].DocDate}" value="${arr[i].OrderNumber/*po_no*/}">${arr[i].OrderNumber}</option>`);
//     //                   }
//     //                   var firstEmptySelect = true;
//     //                   $('#ddlOrderNumberlist').select2({
//     //                       templateResult: function (data) {
//     //                           var DocDate = $(data.element).data('date');
//     //                           var classAttr = $(data.element).attr('class');
//     //                           var hasClass = typeof classAttr != 'undefined';
//     //                           classAttr = hasClass ? ' ' + classAttr : '';
//     //                           var $result = $(
//     //                               '<div class="row">' +
//     //                               '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
//     //                               '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
//     //                               '</div>'
//     //                           );
//     //                           return $result;
//     //                           firstEmptySelect = false;
//     //                       }
//     //                   });

//     //                   $("#src_doc_date").val("");

//     //               }
//     //           }
//     //       }

//     //   })
    
//}
function BindOrderNumberList() {
    debugger;
    var suppId = cmn_getddldataasstring('#ddlSuppliersList');
    var orderType = cmn_getddldataasstring('#ddlOrderType');
    var currId = cmn_getddldataasstring('#ddlCurrencylist');
    var custCat = cmn_getddldataasstring("#SupplierCategory");
    var custPort = cmn_getddldataasstring("#SupplierPortfolio");

    $.ajax({
        type: 'POST',
        url: "/ApplicationLayer/PurchaseTracking/GetOrderNumberListByOrder",
        dataType: "json",
        data: {
            orderType: orderType,
            suppId: suppId,
            currId: currId,
            SuppCat: custCat,
            SuppPort: custPort
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

                    // Check if multiselect is already initialized
                    if ($ddl.data('multiselect')) {
                        $ddl.multiselect('rebuild');
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
function GetPTDeliverySchudule(e) {
    debugger;
    var poNo = $(e.target).closest('tr').find("#poNo").text();
    var poDate = $(e.target).closest('tr').find("#poDt").text();
    var itemId = $(e.target).closest('tr').find("#itemId").text();
    var orderType = $(e.target).closest('tr').find("#poType").text();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PurchaseTracking/DeliveryShudule",
        data: {
            orderNo: poNo,
            // poDate: poDate,
            item_id: itemId,
            OrderType: orderType
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
function GetPTDeliverySchudule(e) {
    debugger;
    var poNo = $(e.target).closest('tr').find("#poNo").text();
    var poDate = $(e.target).closest('tr').find("#poDt").text();
    var itemId = $(e.target).closest('tr').find("#itemId").text();
    var orderType = $(e.target).closest('tr').find("#poType").text();
    var br_id = $(e.target).closest('tr').find("#br_id").text();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/PurchaseTracking/DeliveryShudule",
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
        $("#MISpoTrackingDeatilOverdue").css("display", "none")
        $("#MISpoTrackingDetails").css("display", "none");
        $("#MISpoTrackingDetailsPending").css("display", "block")
    }
    else if (OrderType == "O") {
        $("#MISpoTrackingDetails").css("display", "none");
        $("#MISpoTrackingDetailsPending").css("display", "none");
        $("#MISpoTrackingDeatilOverdue").css("display", "block");
    }
    else {
        $("#MISpoTrackingDeatilOverdue").css("display", "none")
        $("#MISpoTrackingDetailsPending").css("display", "none")
        $("#MISpoTrackingDetails").css("display", "block")
    }
    GetPOTrackingMisDetails();
    
}
function GetPOTrackingMisCSVDetails() {
    debugger;
    var dt = $("#Tbl_POTracking").DataTable(); var order = dt.order();  // [[colIndex, 'asc/desc']]
    var colIndex = order[0][0];
    var direction = order[0][1];
    var search = dt.search();
    var columnName = dt.settings().init().columns[colIndex].data; console.log(columnName); console.log(direction)

    $("#HdnSearchValue").val(search);
    $("#HdnsortColumn").val(columnName);
    $("#HdnsortColumnDir").val(direction);
}