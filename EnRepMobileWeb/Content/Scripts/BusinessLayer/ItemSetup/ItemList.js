
$(document).ready(function () {
    debugger;
    DynamicSerchableItemDDL("", "#itemNameList", "", "", "", "Itemsetup")
   // $("#itemNameList").select2();
    $("#itemGrpList").select2();
    $("#itemPortfList").select2();
    $("#itemAttrList").select2();
    $("#ItmListBody #Item_List tbody").bind("dblclick", function (e) {

        try {
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var ItemCode = clickedrow.children("td:eq(2)").text();
            if (ItemCode != "" && ItemCode != null) {
                window.location.href = "/BusinessLayer/ItemList/EditItem/?ItemId=" + ItemCode + "&ListFilterData=" + ListFilterData;
            }
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    $("#itemAttrList").select2();
    var PageName = sessionStorage.getItem("MenuName");
    $('#ItmListPageName').text(PageName);

    ShowImageInPageLoad();

    $("#ItmListBody").bind("click", function (e) {

        var clickedrow = $(e.target).closest("tr");
        debugger;
        debugger;
        var ItmCode = clickedrow.children("td:eq(2)").text();
        //$("#ItmListBody tr").css("background-color", "#ffffff");
        //$(clickedrow).css("background-color", "rgba(38, 185, 154, .16)"); 
        BindAvlStock(ItmCode);
        GetItemImageList(ItmCode);
        //GetItemOrderDetails(ItmCode);
    });

    //$("#itemNameList").select2({
    //    ajax: {
    //        url: $("#ItemListName").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                // pass your own parameter                
    //                ddlGroup: params.term, // search term like "a" then "an"
    //                Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,            
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            var PageSize = 20;
    //             var page = params.page || 1;
    //            var Fdata = data.slice((page - 1) * PageSize, page * PageSize);
    //            return {
    //                results: $.map(Fdata, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                }),
    //                pagination: {
    //                    more: page * PageSize < data.length
    //                }
    //            };
    //        }
    //    },
    //});
    //$("#itemGrpList").select2({
    //    ajax: {
    //        url: $("#ItemGrpName").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
    //                ddlGroup: params.term, // search term like "a" then "an"
    //                Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,            
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            params.page = params.page || 1;
    //            return {

    //                results: $.map(data, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                })
    //            };
    //        }
    //    },
    //});
    //$("#itemPortfList").select2({
    //    ajax: {
    //        url: $("#ItemPortfolio").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
    //                ddlGroup: params.term, // search term like "a" then "an"
    //                Group: params.page
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            params.page = params.page || 1;
    //            return {

    //                results: $.map(data, function (val, item) {
    //                    return { id: val.ID, text: val.Name };
    //                })
    //            };
    //        }
    //    },
    //});
    //GetAttributeName();

    Export();

    $("#DivMyCarousel").html("");

    $("#btnCloseItemInfo").click(function () {
        $("#DivMyCarousel").html("");
    });
    $("#btnCrossItemInfo").click(function () {
        $("#DivMyCarousel").html("");
    });
});

function OnchangeItemName() {
    debugger;
    var ItemID = "";
    $("#SpanSerchItemID").css("display", "none");
    $("#itemNameList").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-itemNameList-container']").css("border-color", "#ced4da");
    ItemID = $("#itemNameList").val();

        $("#hf_ItemID").val(ItemID);



}
function BindAvlStock(itemId) {
    debugger;
    sessionStorage.setItem("ShowLoader", "N");
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemList/GetItemsAvlStk",/*Controller=ItemSetup and Fuction=Getwarehouse*/
        dataType: "json",
        async: true,
        data: {
            itemId: itemId
        },
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var parsedData = JSON.parse(data);
                // Access and process the individual rows
                var row = parsedData.Table[0];
                var row1 = parsedData.Table1;
                var row2 = parsedData.Table2;
                // Accessing properties of row
                if (parsedData.Table[0] != undefined) {
                    $('#TxtAvlStock').val(row.ho_avl_stk_bs);
                    $('#TxtRwkStock').val(row.ho_rwk_stk_bs);
                    $('#TxtRejStock').val(row.ho_rej_stk_bs);
                }
                else {
                    //Binding data to textbox
                    $('#TxtAvlStock').val("0");
                    $('#TxtRwkStock').val("0");
                    $('#TxtRejStock').val("0");
                }
                if (parsedData.Table1 != undefined) {
                    $('#TxtPendingReceipt').val(row1[0].Pending_recipt);
                    
                }
                else {
                    //Binding data to textbox
                    $('#TxtPendingReceipt').val("0");
                }
                if (parsedData.Table2 != undefined) {
                    $('#TxtPendingShipment').val(row2[0].Pending_Shipment);

                }
                else {
                    //Binding data to textbox
                    $('#TxtPendingShipment').val("0");
                }
            }
            else {
                $('#TxtAvlStock').val("0");
                $('#TxtRwkStock').val("0");
                $('#TxtRejStock').val("0");
                $('#TxtPendingReceipt').val("0");
                $('#TxtPendingShipment').val("0");
            }
        },
        error: function (Data) {

        }
    });
}
function ShowImageInPageLoad() {
    var $row = $('#ItmListBody').find('tr:first');
    debugger;
    debugger;
    GetItemImageList($row.children("td:eq(2)").text());
    //GetItemOrderDetails($row.children("td:eq(2)").text());
    $($row).css("background-color", "#ffffff");
    $($row).css("background-color", "rgba(38, 185, 154, .16)");
}
function OnchangeAttribute() {
    debugger;
    var AttributeID = $("#itemAttrList").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemList/GetAttributeValue",/*Controller=SupplierSetup and Fuction=Getsuppport*/
        dataType: "json",
        data: { AttributeID: AttributeID, },/*Registration pass value like model*/
        success: function (data) {
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].attr_val_id + '">' + arr.Table[i].attr_val_name + '</option>';
                }
                $("#itemAttrValList").html(s);
            }
        },
        error: function (Data) {
        }
    });
};
function BtnSearch() {
    debugger;
    FilterListItem();
}
function BtnAddNewItem() {
    debugger;
    ItemDetail();
}
function onchangeitem_ActStatus() {
    debugger;
    var ActStatus = $("#listActStatus").val();
    console.log("Selected Value:", ActStatus);

    if (ActStatus != null && ActStatus !== "") {
        $("#hdnitem_ActStatus").val(ActStatus);
    } else {
        $("#hdnitem_ActStatus").val("0");
    }
}
function FilterListItem() {
    debugger;
    var ItemID = $("#itemNameList").val();
    var ItemGrpID = $("#itemGrpList").val();
    var AttrName = $("#itemAttrList").val();
    var AttrValue = $("#itemAttrValList").val();
    var FromDate = $("#single_cal1").val();
    var ToDate = $("#single_cal2").val();
    var ActStatus = $("#hdnitem_ActStatus").val();
    var ItmStatus = $("#item_Status").val();
    var ItemPrfID = $("#itemPortfList").val();
    var ImageFilter = $("#ImageFilter").val();


    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemList/GetItemListFilter",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            ItemID: ItemID,
            ItemGrpID: ItemGrpID,
            AttrName: AttrName,
            AttrValue: AttrValue,
            ActStatus: ActStatus,
            ItmStatus: ItmStatus,
            ItemPrfID: ItemPrfID,
            ImageFilter: ImageFilter
        },
        success: function (data) {
            debugger;
            $('#ItmListBody').html(data);
            //$('#ListFilterData').val(ItemID + ',' + ItemGrpID + ',' + ItemPrfID + ',' + AttrName + ',' + AttrValue + ',' + ActStatus + ',' + ItmStatus);
        },

    });
}
function ItemDetail() {
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemSetup/ItemDetail",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function EditItemDetail(ItemID) {
    sessionStorage.removeItem("EditItemCode");
    sessionStorage.setItem("EditItemCode", ItemID);
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemSetup/ItemDetail",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#ItmIdSpan").text();
    $("#DivMyCarousel").html(`<div id="myCarousel" class="carousel slide" data-ride="carousel"></div>`);
    ItemInfoBtnClick(ItmCode);
}
function Closemodal() {
    debugger
    $('#DttblBtns').DataTable().destroy();
    $('#DttblBtns tbody').empty()
    $('#tblstatusid').empty();
    $('#chooseFile').val('');
    $('#btnImportData').prop('disabled', true);
    $('#btnImportData').css('background-color', '#D3D3D3')
}
//function GetItemList() {
//    $.ajax({
//        type: "POST",
//        url: "/ItemSetup/GetItemList",/*Controller=ItemSetup and Fuction=GetItemList*/
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: '',/*Registration pass value like model*/
//        success: function (data) {
//            /*dynamically dropdown list of all Assessment */
//            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
//                var arr = [];
//                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/                

//                $("#ItmListBody tr").detach();
//                var tr;

//                //Append each row to html table
//                for (var i = 0; i < arr.Table.length; i++) {
//                    tr = $('<tr/>');
//                    tr.append("<td>" + arr.Table[i].SNo + "</td>");
//                    tr.append("<td>" + arr.Table[i].Item_name + "</td>");                    
//                    tr.append("<td>" + arr.Table[i].item_group_name + "</td>");
//                    tr.append("<td>" + arr.Table[i].uom_name + "</td>");
//                    tr.append("<td>" + arr.Table[i].item_oem_no + "</td>");
//                    tr.append("<td>" + arr.Table[i].item_sam_cd + "</td>");
//                    if (arr.Table[i].act_stat === 'N') {
//                        tr.append('<td><i class="fa fa-times-circle text-danger"></i></td>');
//                    }
//                    else {
//                        tr.append('<td><i class="fa fa-check text-success"></i></td>');
//                    }                    
//                    tr.append("<td>" + arr.Table[i].cost_price + "</td>");
//                    tr.append("<td>" + arr.Table[i].sale_price + "</td>");
//                    tr.append("<td>" + arr.Table[i].create_dt + "</td>");
//                    tr.append("<td>" + arr.Table[i].app_dt + "</td>");
//                    tr.append("<td>" + arr.Table[i].mod_dt + "</td>");
//                    $('#ItmListBody').append(tr);
//                }
//            }
//        },
//        error: function (Data)
//        {
//            debugger;
//        }
//    });
//};
function GetItemNameList() {
    $.ajax({
        type: "POST",
        url: "/ItemSetup/GetItemNameList",/*Controller=ItemSetup and Fuction=GetItemList*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/

                var s = '<option value="-1">Choose option</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].Item_id + '">' + arr.Table[i].Item_name + '</option>';
                }
                $("#itemNameList").html(s);
            }
        },
        error: function (data) {

        }
    });
};
function GetItemImageList(ItmCode) {
    sessionStorage.setItem("ShowLoader", "N");
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemList/GetItemImageList",/*Controller=ItemSetup and Fuction=GetItemList*/
        dataType: "json",
        data: { ItmCode: ItmCode },/*Registration pass value like model*/
        success: function (data) {
            debugger;
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/

                var OL = '<ol class="carousel-indicators">';
                var Div = '<div class="carousel-inner">';
                var j = 0;
                for (var i = 0; i < arr.Table.length; i++) {
                    var ImgName = arr.Table[i].item_img_name;
                    var fileType = ImgName.split(".").pop().toLowerCase();
                    if (fileType == "jpg" || fileType == "png" || fileType == "gif") {
                        var lclSrvrUrl = $('#hdnLocalServer').val();
                        var origin = lclSrvrUrl + "/ItemSetup/" + ImgName;
                        if (j === 0) {
                            OL += '<li data-target="#myCarousel" data-slide-to="' + j + '" class="active"> <img src="' + origin + '" /></li>'
                            Div += '<div class="carousel-item active"><img src = "' + origin + '" class ="img-fluid" /></div>'
                        }
                        else {
                            OL += '<li data-target="#myCarousel" data-slide-to="' + j + '"><img src="' + origin + '" /></li>'
                            Div += '<div class="carousel-item"><img src = "' + origin + '" class ="img-fluid" /></div>'
                        }
                        j++;
                    }

                }
                OL += '</ol>'
                Div += '</div>'

                var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
                var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

                $("#myCarousel").html(OL + Div + Ach + Ach1);
            }
        },
        error: function (data) {

        }
    });
};
function GetItemOrderDetails(ItmCode) {
    var QtyDecDigit = $("#QtyDigit").text();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemList/GetItem_OrdersDetails",/*Controller=ItemSetup and Fuction=GetItemList*/
        dataType: "json",
        data: { ItmCode: ItmCode },/*Registration pass value like model*/
        success: function (data) {
            debugger;
            var ExportDataPernission = $("#Per_ExportData").val();
            if (ExportDataPernission != null && ExportDataPernission != "") {
                $("#datatable-buttons_wrapper .buttons-csv").css("display", "block");
            }
            else {
                $("#datatable-buttons_wrapper .buttons-csv").css("display", "none");
            }

            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                if (arr.Table.length > 0) {
                    for (var i = 0; i < arr.Table.length; i++) {
                        var AvlStock = arr.Table[i].HOStk;
                        var RwkStock = arr.Table[i].RWStk;
                        var RejStock = arr.Table[i].RejStk;
                        var PendingReceipt = arr.Table[i].PendingReceipt;
                        var PendingShipment = arr.Table[i].PendingShipment;

                        $("#TxtAvlStock").val(parseFloat(AvlStock).toFixed(QtyDecDigit));
                        $("#TxtRwkStock").val(parseFloat(RwkStock).toFixed(QtyDecDigit));
                        $("#TxtRejStock").val(parseFloat(RejStock).toFixed(QtyDecDigit));
                        $("#TxtPendingReceipt").val(parseFloat(PendingReceipt).toFixed(QtyDecDigit));
                        $("#TxtPendingShipment").val(parseFloat(PendingShipment).toFixed(QtyDecDigit));
                    }
                }
                else {
                    $("#TxtAvlStock").val(parseFloat("0").toFixed(QtyDecDigit));
                    $("#TxtRwkStock").val(parseFloat("0").toFixed(QtyDecDigit));
                    $("#TxtRejStock").val(parseFloat("0").toFixed(QtyDecDigit));
                    $("#TxtPendingReceipt").val(parseFloat("0").toFixed(QtyDecDigit));
                    $("#TxtPendingShipment").val(parseFloat("0").toFixed(QtyDecDigit));
                }
            }
            else {
                $("#TxtAvlStock").val(parseFloat("0").toFixed(QtyDecDigit));
                $("#TxtRwkStock").val(parseFloat("0").toFixed(QtyDecDigit));
                $("#TxtRejStock").val(parseFloat("0").toFixed(QtyDecDigit));
                $("#TxtPendingReceipt").val(parseFloat("0").toFixed(QtyDecDigit));
                $("#TxtPendingShipment").val(parseFloat("0").toFixed(QtyDecDigit));
            }
        },
        error: function (data) {

        }
    });
};
function GetAttributeName() {
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemList/GetAttributeName",/*Controller=ItemSetup and Fuction=Getwarehouse*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: '',/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="0">ALL</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].attr_id + '">' + arr.Table[i].attr_name + '</option>';
                }
                $("#itemAttrList").html(s);

            }
        },
        error: function (Data) {

        }
    });
};
function GetAttributeValue() {
    var AttrName = $("#itemAttrList").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemList/GetAttributeValue",/*Controller=ItemSetup and Fuction=Getwarehouse*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: {
            AttrName: AttrName,
        },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="0">ALL</option>';
                for (var i = 0; i < arr.Table.length; i++) {
                    s += '<option value="' + arr.Table[i].attr_val_id + '">' + arr.Table[i].attr_val_name + '</option>';
                }
                $("#itemAttrValList").html(s);

            }
        },
        error: function (Data) {

        }
    });
};

function Export() {


    $('#export').click(function () {
        var titles = [];
        var data = [];

        /*
         * Get the table headers, this will be CSV headers
         * The count of headers will be CSV string separator
         */
        var ItemListFilter = $("#ItemListFilter").val();
        var searchValue = $("#Item_List_filter input[type=search]").val();
        window.location.href = "/BusinessLayer/ItemList/ExportData?searchValue=" + searchValue + "&ItemListFilter=" + ItemListFilter;
        //$.ajax({
        //    type: "POST",
        //    url: "/BusinessLayer/ItemList/ExportData",
        //    data: { searchValue, ItemListFilter },
        //    success: function (d) {
        //        debugger;
        //        $('.dataTable th').each(function () {
        //            titles.push($(this).text());
        //        });

        //        for (var i = 0; i < d.data.length; i++) {

        //            data.push(d.data[i].SrNo);
        //            data.push(d.data[i].ItemName);
        //            data.push(d.data[i].ItemID);
        //            data.push(d.data[i].Group);
        //            data.push(d.data[i].UOM);
        //            data.push(d.data[i].OEMNo);
        //            data.push(d.data[i].SampleCode);
        //            data.push(d.data[i].Active);
        //            data.push(d.data[i].Status);
        //            data.push(d.data[i].ItemCost);
        //            data.push(d.data[i].ItemPrice);
        //            data.push(d.data[i].CreatedBy);
        //            data.push(d.data[i].Createdon);
        //            data.push(d.data[i].ApprovedBy);
        //            data.push(d.data[i].ApprovedOn);
        //            data.push(d.data[i].AmmendedBy);
        //            data.push(d.data[i].AmendedOn);
        //        }


        //        /*
        // * Convert our data to CSV string
        // */
        //        var CSVString = prepCSVRow(titles, titles.length, '');
        //        CSVString = prepCSVRow(data, titles.length, CSVString);

        //        /*
        //         * Make CSV downloadable
        //         */
        //        var downloadLink = document.createElement("a");
        //        var blob = new Blob(["\ufeff", CSVString]);
        //        var url = URL.createObjectURL(blob);
        //        var csvtitle = $("title").text();
        //        downloadLink.href = url;
        //        downloadLink.download = csvtitle+".csv";

        //        /*
        //         * Actually download CSV
        //         */
        //        document.body.appendChild(downloadLink);
        //        downloadLink.click();
        //        document.body.removeChild(downloadLink);
        //    }
        //})


    });
}
/*
* Convert data array to CSV string
* @param arr {Array} - the actual data
* @param columnCount {Number} - the amount to split the data into columns
* @param initial {String} - initial string to append to CSV string
* return {String} - ready CSV string
*/
function prepCSVRow(arr, columnCount, initial) {
    var row = ''; // this will hold data
    var delimeter = ','; // data slice separator, in excel it's `;`, in usual CSv it's `,`
    var newLine = '\r\n'; // newline separator for CSV row

    /*
     * Convert [1,2,3,4] into [[1,2], [3,4]] while count is 2
     * @param _arr {Array} - the actual array to split
     * @param _count {Number} - the amount to split
     * return {Array} - splitted array
     */
    function splitArray(_arr, _count) {
        var splitted = [];
        var result = [];
        _arr.forEach(function (item, idx) {
            if ((idx + 1) % _count === 0) {
                splitted.push(item);
                result.push(splitted);
                splitted = [];
            } else {
                splitted.push(item);
            }
        });
        return result;
    }
    var plainArr = splitArray(arr, columnCount);
    // don't know how to explain this
    // you just have to like follow the code
    // and you understand, it's pretty simple
    // it converts `['a', 'b', 'c']` to `a,b,c` string
    plainArr.forEach(function (arrItem) {
        arrItem.forEach(function (item, idx) {
            row += item + ((idx + 1) === arrItem.length ? '' : delimeter);
        });
        row += newLine;
    });
    return initial + row;
}



/*Import Items From Excel */
$(document).on('renderComplete', function () {
    $(".loader1").hide();
});
function ImportDataFromExcel() {
    debugger;
    $(".loader1").show();
    /*alert($('#hdnNotOk').val());*/
    if ($('#hdnNotOk').val() != "0") {
        $('.loader1').hide();
        swal("", 'Invalid data in excel file. Please modify the data and try again..!', "warning");
    }
    else {
        var formData = new FormData();
        var file = $("#chooseFile").get(0).files[0];
        formData.append("file", file);

        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportItemsDetailFromExcel');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            $(".loader1").hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                //alert(xhr.responseText);
                if (xhr.responseText.toLowerCase().includes("Success")) {
                    swal("", xhr.responseText, "success");
                    $('.loader1').hide();
                }
               else {
                    var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
                    if (responseMessage == "Data saved successfully") {
                        swal("", xhr.responseText, "success");
                        $('#btnImportData').addClass('disabled', true);
                        $('#btnImportData').css('background-color', '#D3D3D3')
                        $('#DttblBtns').DataTable().destroy();
                        $('#DttblBtns tbody').empty()
                        $('#tblstatusid').empty();
                        $('#chooseFile').val('');
                        $('#PartialImportItemData').modal('hide');
                        $('.loader1').hide();
                    }
                    else {
                        swal("", xhr.responseText, "warning");
                        $('.loader1').hide();
                    }
                }
            }
        }
    }
}
/*Import Items From Excel END */
function FetchData() {
    debugger;
    var isfileexist = $('#chooseFile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("","Please choose file", "warning");
    }
}

function SearchDataByUploadStatus() {
    var uploadStatus = $('#item_ActStatus').val();
    if ($('#DttblBtns tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
}
function FetchAndValidateData(uploadStatus) {
    try {
        $(".loader1").show();
        var formData = new FormData();
        var file = $("#chooseFile").get(0).files[0];
        formData.append("file", file);
        $('#btnImportData').prop('disabled', false);
        $('#btnImportData').css('background-color', '#007bff')
        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + uploadStatus);
        xhr.send(formData);
        xhr.onreadystatechange = function () {

            if (xhr.readyState == 4 && xhr.status == 200) {
                $('#PartialImportItemsDetail').html(xhr.response);
                //$('#DttblBtns').DataTable({
                //    dom: 'l B f p',
                //    buttons: ['csv']
                //});
                cmn_apply_datatable("#DttblBtns")
                var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
                if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                    $('#btnImportData').addClass('disabled', true);
                    $('#btnImportData').css('background-color', '#D3D3D3')
                } else {
                    $('#btnImportData').prop('disabled', false);
                    $('#btnImportData').css('background-color', '#007bff')
                }
            }
            /* $(".loader1").hide();*/
        }
    }
    catch (error) {
        console.log(error)
    }
}
function OnClickErrorDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemName = "";
    itemName = clickedrow.find("#tdItmName").text();
    var formData = new FormData();
    var file = $("#chooseFile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?itemName=' + itemName);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        /*  hideLoader();*/
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);

        }
    }
}
function OnClickSubItemDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemName = "";
    itemName = clickedrow.find("#tdItmName").text();
    var formData = new FormData();
    var file = $("#chooseFile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?itemName=' + itemName);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);
            $("#txtItemName").val(itemName);
            $("#txtUom").val(itemName);
        }
    }
}
function BindExcelSubItemDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemName = "";
    itemName = clickedrow.find("#tdItmName").text();
    var formData = new FormData();
    var file = $("#chooseFile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindExcelSubItems?itemName=' + itemName);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divSubItemDetail').html(xhr.response);
            $("#txtItemName").val(itemName);
            $("#txtUom").val(clickedrow.find("#tdBaseUom").text());
        }
    }
}
function BindItemBranchDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemName = "";
    itemName = clickedrow.find("#tdItmName").text();
    var formData = new FormData();
    var file = $("#chooseFile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindItemBranch?itemName=' + itemName);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#BranchMapping").css("display", "block")
            $('#BranchMapping').html(xhr.response);
        }
    }
}
function BindItemPortfolioDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var itemName = "";
    itemName = clickedrow.find("#tdItmName").text();
    var formData = new FormData();
    var file = $("#chooseFile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindItemPortfolio?itemName=' + itemName);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#ItemPortfolioPopUp").css("display", "block")
            $('#ItemPortfolioPopUp').html(xhr.response);
        }
    }
}
function popClose_Brmodal() {
    debugger;
    $("#BranchMapping").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}
function popClose_portodal() {
    debugger;
    $("#ItemPortfolioPopUp").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}
