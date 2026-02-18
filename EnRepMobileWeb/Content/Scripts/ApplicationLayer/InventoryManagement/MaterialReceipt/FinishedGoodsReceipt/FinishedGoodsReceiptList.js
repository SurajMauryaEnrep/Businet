$(document).ready(function () {
    $("#ddlShopfloorID").select2();
    $("#ddloperationname").select2();
    $("#datatable-buttons tbody tr").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var rcpt_no = clickedrow.children("#rcpt_no").text();
            var rcpt_dt = clickedrow.children("#rcpt_dt").text();
            if (rcpt_no != null && rcpt_no != "" && rcpt_dt != null && rcpt_dt != "") {
                window.location.href = "/ApplicationLayer/FinishedGoodsReceipt/FRGDblClick/?rcpt_no=" + rcpt_no + "&rcpt_dt=" + rcpt_dt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {

        debugger;
        var clickedrow = $(e.target).closest("tr");
        var rcpt_no = clickedrow.children("#rcpt_no").text();
        var rcpt_dt = clickedrow.children("#rcpt_dt").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(rcpt_no, rcpt_dt, Doc_id, Doc_Status);
        $("#hdDoc_No").val(rcpt_no);
    });
    BindProductNameDDL();

       
})
function BindProductNameDDL() {
    debugger;
    $("#ddlItemName").append("<option value='0'>All</option>");
    $("#ddlItemName").select2({

        ajax: {
            url: "/ApplicationLayer/FinishedGoodsReceipt/GetItemList",
            data: function (params) {
                var queryParameters = {
                    ProductName: params.term,                  
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 2000; // or whatever pagesize
                data = JSON.parse(data).Table;
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                      <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
                       <div class="col-md-3 col-xs-6 def-cursor">UOM</div></div>
                       </strong></li></ul>`)
                }

                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                return {
                    results: $.map(data, function (val, Item) {                       
                            return { id: val.Item_id, text: val.Item_name, UOM: val.uom_name };                    
                    }),
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
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '</div>'
            );

            return $result;

            firstEmptySelect = false;
        },

    });
   
   
}
function BtnSearch() {
    debugger;
    try {
        var Source_type = $("#dllSource_type").val().trim();
        var Item_id = $("#ddlItemName option:selected").val().trim();
        var shopfloreid = $("#ddlShopfloorID").val();
        var opertionid = $("#ddloperationname").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate1").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/FinishedGoodsReceipt/FinishGoodsReciept_Search",
            data: {
                shopfloreid: shopfloreid,
                opertionid: opertionid,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                Source_type: Source_type,
                Item_id: Item_id
            },
            success: function (data) {
                debugger;
                $('#tbodyFGRList').html(data);
                $('#ListFilterData').val(shopfloreid + ',' + opertionid + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + Source_type + ',' + Item_id );
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
    ResetWF_Level();
}