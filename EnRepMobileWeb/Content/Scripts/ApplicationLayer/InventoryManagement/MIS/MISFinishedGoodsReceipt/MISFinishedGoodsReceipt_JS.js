$(document).ready(function () {
    debugger;
    $('#ddlItemName').select2();
   // $('#ddlShopfloorID').select2();
    BindItemNameDetail();
    BindItemGroup();
    $('td').bind('click', function () {
        debugger;

        var $td = $(this);
        $("#datatable-buttons tbody tr").removeClass('rowFGR')
        var $row = $td.closest('tr');
        var $tds = $row.find('td');
        var rowspan = ~~$(this).attr('rowspan');
        while (--rowspan > 0) {
            $row = $row.add($row.next());
        }

        $row.toggleClass('rowFGR');
    });
    Cmn_initializeMultiselect(['#ddlShopfloorID']);
})
function DownloadExcel() {

    $('#DownloadExcel').click(function () {
        debugger;
        var itmid = $("#ddlItemName").val();
        //var GroupID = $("#ddlItemGroupName").val();
        var GroupID = cmn_getddldataasstring("#ddlItemGroupName");
        var txtFromdate = $("#txtFromdate").val();
        var txtTodate = $("#txtTodate").val();
        var MultiselectItemHdn = $("#MultiselectItemHdn").val();
        var ShopFloor_id = $("#Hdnshopfloor_id").val();
        var ddl_ShowAs = $("#ddl_ShowAs").val();

       // var filters = itmid + "," + GroupID + "," + txtFromdate + "," + txtTodate + "," + MultiselectItemHdn + "," + ShopFloor_id + "," + ddl_ShowAs;

        var filterArray = [itmid, GroupID, txtFromdate, txtTodate, MultiselectItemHdn, ShopFloor_id, ddl_ShowAs];
        var filters = filterArray.map(x => `[${x}]`).join('_');

        var searchValue = $("#MISFGRReport_filter input[type=search]").val();
        window.location.href = "/ApplicationLayer/MISFinishedGoodsReceipt/ExcelDownload?searchValue=" + searchValue + "&filters=" + filters + "&ReportType=" + ddl_ShowAs;

    });
}
function ItemNameOnchange() {
    debugger;
    var selected = [];
    var abc = "";
    $('#ddlItemName option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    $("#MultiselectItemHdn").val(abc);

}
function Onchngeshopflore() {
    debugger;
    var selected = [];
    var abc = "";
    $('#ddlShopfloorID option:selected').each(function () {
        if (abc == "") {

            abc += selected[$(this).text()] = $(this).val();
        }
        else {
            abc += selected[$(this).text()] = "," + $(this).val();
        }
    });
    $("#Hdnshopfloor_id").val(abc);

}
function BindItemNameDetail() {
    /* $("#ddlItemName").select2({*/
    $("#ddlItemName").select2({
        ajax: {
            url: $("#ItemListName").val(),
            data: function (params) {
                var queryParameters = {
                    ItemName: params.term // search term like "a" then "an"
                    //Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                //params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
                  
            }
        },
    });
     
}
//function BindItemGroup() {
//    $("#ddlItemGroupName").select2({
//        ajax: {
//            url: $("#ItemGrpName").val(),
//            data: function (params) {
//                var queryParameters = {
//                    GroupName: params.term // search term like "a" then "an"
//                    //Group: params.page
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                debugger;
//                //params.page = params.page || 1;
//                return {
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}
function BindItemGroup() {
    Cmn_initializeMultiselect(["#ddlItemGroupName"],
        {
            ajax: {
                url: $("#ItemGrpName").val(),
                data: function (params) {
                    return {
                        GroupName: params.term
                    };
                },
                dataType: "json",
                cache: true,
                delay: 250,
                contentType: "application/json; charset=utf-8",
                processResults: function (data, params) {

                    params.page = params.page || 1;

                    return {
                        results: $.map(data, function (val) {
                            return { id: val.ID, text: val.Name };
                        })
                    };
                }
            }
        }
    );
}
function BtnSearch() {
    debugger;

   var itmid= $("#ddlItemName").val();
    //var GroupID = $("#ddlItemGroupName").val();
    var GroupID = cmn_getddldataasstring("#ddlItemGroupName");
    var txtFromdate = $("#txtFromdate").val();
    var txtTodate = $("#txtTodate").val();
    var MultiselectItemHdn = $("#MultiselectItemHdn").val();
    var ShopFloor_id = $("#Hdnshopfloor_id").val();
    var ddl_ShowAs = $("#ddl_ShowAs").val();
    $.ajax({
        type: "post",
        url: "/ApplicationLayer/MISFinishedGoodsReceipt/SerchDataMIS",
        data: {
            itmid: itmid,
            GroupID: GroupID,
            txtFromdate: txtFromdate,
            txtTodate: txtTodate,
            MultiselectItemHdn: MultiselectItemHdn,
            ShopFloor_id: ShopFloor_id,
            ddl_ShowAs: ddl_ShowAs
        },
        success: function (data) {
            $("#tblDataFGRMIS").html(data)
        }

    })
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickIconBtn1(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hfItemID1").val();
    ItemInfoBtnClick(ItmCode);
}
function onchangeShowAs() {
    debugger
    var ddl_ShowAs = $("#ddl_ShowAs").val();
    if (ddl_ShowAs == "D") {
        $("#ddlItemName").attr("disabled", true)
       // $("#ddlItemGroupName").attr("disabled", true)
        $("#ddlItemGroupName").prop("disabled", true).multiselect('disable').next('.btn-group').find('button').css("background-color", "#C9E3FB"); 
    }
    else {
        $("#ddlItemName").attr("disabled", false)
       // $("#ddlItemGroupName").attr("disabled", false)
        $("#ddlItemGroupName").prop("disabled", false).multiselect('enable');//Added by nidhi on 06-12-2025
        $("#ddlItemGroupName").prop("disabled", true).multiselect('enable').next('.btn-group').find('button').css("background-color", "");
    }
     BtnSearch();
}
function GetDataBatchDeatil(e) {
    debugger;
    var curr = $(e.target).closest("tr");
    var rcpt_no = curr.find("#hidden_Doc_no").val();
    var rcpt_dt = curr.find("#Doc_dt").val().trim();
    var item_id = curr.find("#hfItemID1").val().trim();
    var item_Name = curr.find("#ItmNameSpan1").text().trim();
    var Con_Qty = curr.find("#Consume_qty").text().trim();
    var UOM = curr.find("#UOM").text().trim();
    var UOM_ID = curr.find("#UOM_ID").val();
    var DocumentMenuId = $("#DocumentMenuId").val();
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/MISFinishedGoodsReceipt/GetDataBatchDeatil",
        data: {
            rcpt_no: rcpt_no,
            rcpt_dt: rcpt_dt,
            item_id: item_id,
            DocumentMenuId: DocumentMenuId
        },
        success: function (data) {
            debugger;
            $("#BatchNumber").html(data);
            $("#ItemNameBatchWise").val(item_Name);
            $("#HDItemNameBatchWise").val(item_id);
            $("#UOMBatchWise").val(UOM);
            $("#HDUOMBatchWise").val(UOM_ID);
            $("#QuantityBatchWise").val(Con_Qty);
        }

    })
}