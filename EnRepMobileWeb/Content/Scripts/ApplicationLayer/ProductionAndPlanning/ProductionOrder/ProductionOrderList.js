$(document).ready(function () {
    debugger;
    $("#ddl_ShopfloorName").select2();
    $("#ddl_OperationName").select2();
    $('#ddl_ProductNameListPage').empty();
    
    BindProductNameInListPage();
    $("#datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var WF_Status = $('#WF_Status').val();
            var clickedrow = $(e.target).closest("tr");
            var hdn_jc_no = clickedrow.children("#jc_no").text();
            var jc_dt = clickedrow.children("#jc_dt").text();
            var date = jc_dt.split("-");
            var FDate = date[2] + '-' + date[1] + '-' + date[0];
            //BindProductNameDDL();
            window.location = "/ApplicationLayer/ProductionOrder/dbClickEdit/?jc_no=" + hdn_jc_no + "&jc_dt=" + FDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
        }
        catch (err) {
            debugger;
        }
    });
    //$("#datatable-buttons >tbody").bind("click", function (e) {
    //    debugger;
    //    var clickedrow = $(e.target).closest("tr");
    //    var hdn_jc_no = clickedrow.children("#jc_no").text();
    //    var jc_dt = clickedrow.children("#jc_dt").text();
    //    var date = jc_dt.split("-");
    //    var FDate = date[2] + '-' + date[1] + '-' + date[0];
    //    var Doc_id = $("#DocumentMenuId").val();
    //    var Doc_Status = clickedrow.children("#Doc_Status").text();
    //    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    //    debugger;
    //    GetWorkFlowDetails(hdn_jc_no, FDate, Doc_id, Doc_Status);
    //    $("#hdDoc_No").val(hdn_jc_no);
    //});
});

function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var DocNo = clickedrow.children("#jc_no").text();
    var DocDate = clickedrow.children("#jc_dt").text();
    var Doc_id = $("#DocumentMenuId").val();
    var Doc_Status = clickedrow.children("#Doc_Status").text();
    $("#hdDoc_No").val(DocNo);
    if (DocDate.split('-')[2].length == 4) {
        DocDate = DocDate.split('-').reverse().join('-');
    }
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(DocNo, DocDate, Doc_id, Doc_Status);
    var a = 1;
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
function BindProductNameInListPage() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/ProductionOrder/GetProductNameInDDLListPage",
            data: function (params) {
                var queryParameters = {
                    SO_ItemName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        $('#ddl_ProductNameListPage').empty();
                        $('#ddl_ProductNameListPage').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
                        }
                        //var firstEmptySelect = true;
                        $('#ddl_ProductNameListPage').select2({
                            templateResult: function (data) {
                                var UOM = $(data.element).data('uom');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });


                        var hdn_product_idListPage = $("#hdn_product_idListPage").val();
                        if (hdn_product_idListPage != '') {
                            $("#ddl_ProductNameListPage").val(hdn_product_idListPage).trigger('change.select2');;
                        }
                        var hdn_ddlStatus = $("#hdn_ddlStatus").val();
                        if (hdn_ddlStatus != '') {
                            $("#ddlStatus").val(hdn_ddlStatus).trigger('change');;
                        }

                    }
                }
            },
        });
}
function ddl_ProductNameListPage_onchange() {
    debugger;
    var ddl_ProductNameListPage = $("#ddl_ProductNameListPage").val();
    $("#hdn_product_idListPage").val(ddl_ProductNameListPage);
}
function ddl_Status_onchange() {
    debugger;
    var ddlStatus = $("#ddlStatus").val();
    $("#hdn_ddlStatus").val(ddlStatus);
}
function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtToDate").val();
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
            $("#txtToDate").val(today);
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
        }
    }
}