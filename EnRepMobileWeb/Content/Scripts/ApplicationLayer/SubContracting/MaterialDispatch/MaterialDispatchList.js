$(document).ready(function () {
    debugger;

    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $("#ListFilterData").val();
        var WF_Status = $("#WF_Status").val();
        var clickedrow = $(e.target).closest("tr");
        var MD_No = clickedrow.children("#MDIssueNoList").text();
        var MD_Date = clickedrow.children("#MDIssueDtList").text();
        if (MD_No != "" && MD_No != null) {
            location.href = "/ApplicationLayer/MaterialDispatchSC/DoubleClickOnList/?DocNo=" + MD_No + "&DocDate=" + MD_Date + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {

        var clickedrow = $(e.target).closest("tr");
        var MD_No = clickedrow.children("#MDIssueNoList").text();
        var MD_Date = clickedrow.children("#MDIssueDtList").text();
        var Supp_name = clickedrow.children("#SuppName").text();
        var Doc_id = $("#DocumentMenuId").val();
        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        $("#hdDoc_No").val(MD_No);
        GetWorkFlowDetails(MD_No, MD_Date, Doc_id, Doc_Status);

    });

    BindSupplierMDList();
    $('#MDddl_ProductNameListPage').empty();
    MDBindProductNameInListPage();

});
function BindSupplierMDList() {
    var Branch = sessionStorage.getItem("BranchID");
    $("#MDListSupplier").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch
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
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}

function MDBindProductNameInListPage() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialDispatchSC/GetMDProductNameInDDLListPage",
            data: function (params) {
                var queryParameters = {
                    MD_ItemName: params.term
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
                        $('#MDddl_ProductNameListPage').empty();
                        $('#MDddl_ProductNameListPage').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}'></optgroup>`);

                        $('#MDddl_FProductNameListPage').empty();
                        $('#MDddl_FProductNameListPage').append(`<optgroup class='def-cursor' id="FTextddl" label='${$("#ItemName").text()}'></optgroup>`);

                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);

                            $('#FTextddl').append(`<option value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
                        }

                        $('#MDddl_ProductNameListPage').select2({
                        });

                        $('#MDddl_FProductNameListPage').select2({
                        });

                        var hdn_product_idListPage = $("#MDhdn_product_idListPage").val();
                        if (hdn_product_idListPage != '') {
                            $("#MDddl_ProductNameListPage").val(hdn_product_idListPage).trigger('change.select2');;
                        }

                        var hdn_Fproduct_idListPage = $("#MDhdn_Fproduct_idListPage").val();
                        if (hdn_Fproduct_idListPage != '') {
                            $("#MDddl_FProductNameListPage").val(hdn_Fproduct_idListPage).trigger('change.select2');;
                        }

                    }
                }
            },
        });
}

function MDddl_ProductNameListPage_onchange() {
    debugger;
    var ddl_ProductNameListPage = $("#MDddl_ProductNameListPage").val();
    $("#MDhdn_product_idListPage").val(ddl_ProductNameListPage);
}
function MDddl_FProductNameListPage_onchange() {
    debugger;
    var ddl_FProductNameListPage = $("#MDddl_FProductNameListPage").val();
    $("#MDhdn_Fproduct_idListPage").val(ddl_FProductNameListPage);

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
            //alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    else {
        //alert('please select from and to date');
    }
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
function BtnSearch() {
    debugger;
    FilterMDList();
    ResetWF_Level();
}
function FilterMDList() {
    debugger;
    try {
        var SuppId = $("#MDListSupplier option:selected").val();
        var OPOutProdctID = $("#MDddl_ProductNameListPage option:selected").val();
        var FProdctID = $("#MDddl_FProductNameListPage option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialDispatchSC/SearchMDDetail",
            data: {
                SuppId: SuppId,
                OPOutProdctID: OPOutProdctID,
                FProdctID: FProdctID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodyMDList').html(data);
                $('#ListFilterData').val(SuppId + ',' + FProdctID + ',' + OPOutProdctID + ',' + Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}
