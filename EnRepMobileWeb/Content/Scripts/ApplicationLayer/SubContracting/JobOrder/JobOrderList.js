$(document).ready(function () {
    debugger;
    
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $("#ListFilterData").val();
        var WF_Status = $("#WF_Status").val();
        var clickedrow = $(e.target).closest("tr");
        var SrcTyp = clickedrow.children("#Lstsrctyp").text();
        var JO_No = clickedrow.children("#OrderNo").text();
        var JO_Date = clickedrow.children("#OrderDt").text();
        if (JO_No != "" && JO_No != null) {
            location.href = "/ApplicationLayer/JobOrderSC/DoubleClickOnList/?DocNo=" + JO_No + "&DocDate=" + JO_Date + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status + "&SrcTyp=" + SrcTyp;
            
        }

    });
    $("#datatable-buttons >tbody").bind("click", function (e) {

        var clickedrow = $(e.target).closest("tr");
        var JO_No = clickedrow.children("#OrderNo").text();
        var JO_Date = clickedrow.children("#OrderDt").text();
        var Supp_name = clickedrow.children("#SuppName").text();
        var Doc_id = $("#DocumentMenuId").val();

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");

        $("#hdDoc_No").val(JO_No);
        GetWorkFlowDetails(JO_No, JO_Date, Doc_id, Doc_Status);
        
    });

    BindSupplierJOList();
    $('#ddl_ProductNameListPage').empty();
    BindProductNameInListPage();
    
});
function BindSupplierJOList() {
    var Branch = sessionStorage.getItem("BranchID");
    $("#JOListSupplier").select2({
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

function BindProductNameInListPage() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/JobOrderSC/GetProductNameInDDLListPage",
            data: function (params) {
                var queryParameters = {
                    JO_ItemName: params.term
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
                        $('#ddl_ProductNameListPage').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}'></optgroup>`);

                        $('#ddl_FProductNameListPage').empty();
                        $('#ddl_FProductNameListPage').append(`<optgroup class='def-cursor' id="FTextddl" label='${$("#ItemName").text()}'></optgroup>`);

                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);

                            $('#FTextddl').append(`<option value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
                        }
                       
                        $('#ddl_ProductNameListPage').select2({
                            //templateResult: function (data) {
                               
                            //    var classAttr = $(data.element).attr('class');
                            //    var hasClass = typeof classAttr != 'undefined';
                            //    classAttr = hasClass ? ' ' + classAttr : '';
                            //    var $result = $(
                            //        '<div class="row">' +
                            //        '<div class="col-md-12 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                    
                            //        '</div>'
                            //    );
                            //    return $result;
                            //    firstEmptySelect = false;
                            //}
                        });

                        $('#ddl_FProductNameListPage').select2({
                            //templateResult: function (data) {

                            //    var classAttr = $(data.element).attr('class');
                            //    var hasClass = typeof classAttr != 'undefined';
                            //    classAttr = hasClass ? ' ' + classAttr : '';
                            //    var $result = $(
                            //        '<div class="row">' +
                            //        '<div class="col-md-12 col-xs-12' + classAttr + '">' + data.text + '</div>' +

                            //        '</div>'
                            //    );
                            //    return $result;
                            //    firstEmptySelect = false;
                            //}
                        });

                        var hdn_product_idListPage = $("#hdn_product_idListPage").val();
                        if (hdn_product_idListPage != '') {
                            $("#ddl_ProductNameListPage").val(hdn_product_idListPage).trigger('change.select2');;
                        }

                        var hdn_Fproduct_idListPage = $("#hdn_Fproduct_idListPage").val();
                        if (hdn_Fproduct_idListPage != '') {
                            $("#ddl_FProductNameListPage").val(hdn_Fproduct_idListPage).trigger('change.select2');;
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
function ddl_FProductNameListPage_onchange() {
    debugger;
    var ddl_FProductNameListPage = $("#ddl_FProductNameListPage").val();
    $("#hdn_Fproduct_idListPage").val(ddl_FProductNameListPage);

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
    FilterJOList();
    ResetWF_Level();
}
function FilterJOList() {
    debugger;
    try {
        var SuppId = $("#JOListSupplier option:selected").val();
        var OutOPProdctID = $("#ddl_ProductNameListPage option:selected").val();
        var FinishProdctID = $("#ddl_FProductNameListPage option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/JobOrderSC/SearchJODetail",
            data: {
                SuppId: SuppId,
                OutOPProdctID: OutOPProdctID,
                FinishProdctID: FinishProdctID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodyJOList').html(data);
                $('#ListFilterData').val(SuppId + ',' + OutOPProdctID + ',' + Fromdate + ',' + Todate + ',' + Status + ',' + FinishProdctID);
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

function OnClickJobOrderTrackingBtn(e) {
    debugger;
   
    $("#Table_JOTracking tbody tr").remove();
    var currentrow = $(e.target).closest('tr');
    var JONo = currentrow.find("#OrderNo").text();
    var JODate = moment(currentrow.find("#OrderDt").text()).format('YYYY-MM-DD');
    var SuppName = currentrow.find("#SuppName").text();
    var LstFgItemName = currentrow.find("#LstFgItemName").text();
    var LstFgUomName = currentrow.find("#LstFgUomName").text();
    var LstOprationName = currentrow.find("#LstOprationName").text();
    var LstOpOutProductName = currentrow.find("#LstOpOutProductName").text();
    var LstOpOutProductUOM = currentrow.find("#LstOpOutProductUOM").text();
    var LstQuantity = currentrow.find("#LstQuantity").text();
    
    if (JONo != null && JONo != "" && JODate != null && JODate != "") {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/JobOrderSC/GetJOTrackingDetail",
            data: {
                JONo: JONo, JODate: JODate, SuppName: SuppName, LstFgItemName: LstFgItemName, LstFgUomName: LstFgUomName,
                LstOprationName: LstOprationName, LstOpOutProductName: LstOpOutProductName, LstOpOutProductUOM: LstOpOutProductUOM,
                LstQuantity: LstQuantity},
            //dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }
                $("#trackingJO").html(data);
                cmn_apply_datatable("#Table_JOTracking");
                $("#LstJoTrckJONumber").val(JONo);
                $("#LstJoTrckJODate").val(JODate);
                $("#LstJoTrckSupplierName").val(SuppName);
                $("#LstJoTrckFnshdPrdctName").val(LstFgItemName);
                $("#LstJoTrckFnshdPrdctUom").val(LstFgUomName);
                $("#LstJoTrckOPName").val(LstOprationName);
                $("#LstJoTrckOPOutPrdctName").val(LstOpOutProductName);
                $("#LstJoTrckOPOutPrdctUom").val(LstOpOutProductUOM);
                $("#LstJoTrckJOQty").val(LstQuantity);
            }
        });
    }
}