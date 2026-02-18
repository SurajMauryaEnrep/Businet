$(document).ready(function () {
    //$('#ddl_SupplierName').select2();
    $('#ddl_TDSId').select2();
    $('#ddl_TDSId').select2();
    $('#ddl_TDSId').select2();
    BindSupplierList();
    GetTDSMISDetails();
})
async function OnChangeTax_Type() {
    let tax_type = $("#Tax_Type").val();
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/TDSDetail/getTdsNameList",
        data: {
            tax_type: tax_type
        },
        success: function (data) {
            debugger;
            if (data != null && data != "") {
                let arr = JSON.parse(data).Table;
                var s = `<option value="0" data-acc_id="0">---Select---</option>`;
                arr.map((item) => {
                    s += `<option value="${item.tax_id}" data-acc_id="${item.tax_acc_id}">${item.tax_name}</option>`;
                });
                $("#TDS_Type").html(s);
            }
            hideLoader();/*Add by Hina sharma on 19-11-2024 */
        }
    });
}
function BindSupplierList() {
    try {
        var Branch = sessionStorage.getItem("BranchID");
        //let tax_type = $("#Tax_Type").val();
        $("#ddl_SupplierName").select2({
            ajax: {
                url: $("#SuppNameList").val(),
                data: function (params) {
                    var queryParameters = {
                        SuppName: params.term, // search term like "a" then "an"
                        SuppPage: params.page,
                        BrchID: Branch,
                        tax_type: $("#Tax_Type").val()
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
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
    
}
function GetTDSMISDetails() {
    debugger;
    try {
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
        var suppId = $('#ddl_SupplierName').val();
        var TDSID = $("#TDS_Type option:selected").attr("data-acc_id")
        let tax_type = $("#Tax_Type").val();
        //var sec_code = $("#ddl_SecCode option:selected").attr("data-acc_id");/*add by hina on 12-08-2025*/
        var sec_code = $("#ddl_SecCode").val();/*add by hina on 12-08-2025*/
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/TDSDetail/SearchTDSDetails",
            data: {
                TDSID: TDSID, suppId: suppId, fromDate: fromDate, toDate: toDate, tax_type: tax_type, sec_code: sec_code
            },
            success: function (data) {
                debugger;
                $("#MISTDSDetails").html(data);
                //$("a.btn.btn-default.buttons-csv buttons-html5 btn-sm").remove();
                //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="AccountRecevablCSV()" tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                hideLoader();/*Add by Hina sharma on 19-11-2024 */
            }
        });
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
    
}
function AccountRecevablCSV() {
    debugger;
    try {
        var arr = [];
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
        var suppId = $('#ddl_SupplierName').val();
        var TDSID = $("#TDS_Type option:selected").attr("data-acc_id");
        var tax_type = $('#Tax_Type').val();
        //var sec_code = $("#ddl_SecCode option:selected").attr("data-acc_id");/*add by hina on 12-08-2025*/
        var sec_code = $("#ddl_SecCode").val();/*add by hina on 12-08-2025*/
        var list = {};
        list.fromDate = fromDate
        list.toDate = toDate
        list.suppId = suppId
        list.TDSID = TDSID
        list.tax_type = tax_type
        list.sec_code = sec_code
        arr.push(list);

        var array = JSON.stringify(arr);
        $("#hdnTDSDetailData").val(array);

        $("#hdnCSVPrint").val("CsvPrint");
        var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
        $("#searchValue").val(searchValue);

        $('form').submit();
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }
    
}