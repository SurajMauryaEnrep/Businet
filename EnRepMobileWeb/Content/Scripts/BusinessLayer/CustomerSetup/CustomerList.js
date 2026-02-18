

$(document).ready(function () {
    debugger;
    BingGLReporting();
    $("#CustListBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();

            var clickedrow = $(e.target).closest("tr");
            //var CustCode = clickedrow.children("#hdncustid1").text();
            var CustCode = clickedrow.find("td#hdncustid1").clone().children().remove().end().text().trim();
            var act_status = clickedrow.children("#act_status").text();
            var cust_hold = clickedrow.children("#on_hold").text();
            if (CustCode != "" && CustCode != null) {
                window.location.href = "/BusinessLayer/CustomerSetup/EditCustomer/?CustId=" + CustCode + "&act_status=" + act_status + "&cust_hold=" + cust_hold + "&ListFilterData=" + ListFilterData;
            }
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    var PageName = sessionStorage.getItem("PageName");
    $('#GRNDetailsPageName').text(PageName);
    sessionStorage.setItem("PageName", null);

    $("#custnamelist").select2();
       // ({
    //    ajax: {
    //        url: $("#CustListName").val(),
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
    //        //type: 'POST',
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
    //var Custport = $("#custport").val();
    //debugger;
    //if (Custport != "ALL") {

    //}
    //else {
    //    GetCustport();
    //}
    //var category = $("#custcat").val();
    //debugger;
    //if (category != "ALL") {

    //}
    //else {
    //    Getcategory();
    //}
});

function BingGLReporting() {
    debugger;
    $("#ID_GlRepoting_Group").select2({

        ajax: {
            type: "POST",
            url: "/BusinessLayer/CustomerSetup/GetGlReportingGrp",
            dataType: "json",

            data: function (params) {
                var queryParameters = {
                    GlRepoting_Group: params.term, // search term like "a" then "an"
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
                debugger;
                data = JSON.parse(data);
                params.page = params.page || 1;
                /*var paginatedData = data.slice((page - 1)  pageSize, page  pageSize);*/
                // if (page == 1) {
                if (data[0] != null) {
                    if (data[0].Val.trim() != "---Select---") {
                        var select = { ID: "0", Val: "---Select---" };
                        data.unshift(select);
                    }
                }
                // }
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Val };
                    }),
                };
            },

        },

    });
}
function OnChangeGlrptfrp() {
    var glrtpID = $("#ID_GlRepoting_Group").val();
    var glrtp = $("#ID_GlRepoting_Group option:selected").text().trim();
    $("#hdn_GlRepoting_Group").val(glrtpID);
    $("#hdn_GlRepoting_GroupName").val(glrtp);
}
function CustomerDetail() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/CustomerSetup/CustomerDetail",
                error: function (xhr, status, error) {
                    debugger;
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}

//function Getcategory() {
//    $.ajax({
//        type: "POST",
//        url: "/BusinessLayer/CustomerSetup/Getcategory",/*Controller=ItemSetup and Fuction=Getwarehouse*/
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: '',/*Registration pass value like model*/
//        success: function (data) {
//            if (data == 'ErrorPage') {
//                ErrorPage();
//                return false;
//            }
//            /*dynamically dropdown list of all Assessment */
//            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
//                var arr = [];
//                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
//                var s = '<option value="0">ALL</option>';
//                for (var i = 0; i < arr.Table.length; i++) {
//                    s += '<option value="' + arr.Table[i].setup_id + '">' + arr.Table[i].setup_val + '</option>';
//                }
//                $("#custcat").html(s);
//            }
//        },
//        error: function (Data) {
//        }
//    });
//};
//function GetCustport() {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/BusinessLayer/CustomerSetup/GetCustport",/*Controller=SupplierSetup and Fuction=Getsuppport*/
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: '',/*Registration pass value like model*/
//        success: function (data) {
//            if (data == 'ErrorPage') {
//                ErrorPage();
//                return false;
//            }
//            /*dynamically dropdown list of all Assessment */
//            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
//                var arr = [];
//                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
//                var s = '<option value="0">ALL</option>';
//                for (var i = 0; i < arr.Table.length; i++) {
//                    s += '<option value="' + arr.Table[i].setup_id + '">' + arr.Table[i].setup_val + '</option>';
//                }
//                $("#custport").html(s);
//            }
//        },
//        error: function (Data) {
//        }
//    });
//};
function EditCustDetail(CustID) {
    debugger;
    sessionStorage.removeItem("EditCustCode");
    sessionStorage.setItem("EditCustCode", CustID);
    
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/CustomerSetup/EditCustomer",
                dataType: "html",
                data: { CustCode: CustID },
                success: function () {
                    window.location.href = "/BusinessLayer/CustomerSetup/EditCustomer";
                }                
            });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
};

function FilterListCust() {
    debugger;
    try {
    var CustID = $("#custnamelist").val();
    var Custtype = $("#cust_type").val();
    var Custcat = $("#custcat").val();
    var Custport = $("#custport").val();
    var CustAct = $("#cust_act").val();
        var Glrtp_id = $("#hdn_GlRepoting_Group").val();
        var Glrtp_Name = $("#hdn_GlRepoting_GroupName").val();
        var CustStatus = $("#cust_status").val();
        //if (CustStatus == "0") {
        //    CustStatus = "ALL";
        //}
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/CustomerSetup/GetCustomerListFilter",
       
        data: {
            CustID: CustID,
            Custtype: Custtype,
            Custcat: Custcat,
            Custport: Custport,
            CustAct: CustAct,
            CustStatus: CustStatus,
            Glrtp_id: Glrtp_id,
           
        },
        success: function (data) {
            debugger;
            $('#CustListBody').html(data);
            $('#ListFilterData').val(CustID + ',' + Custtype + ',' + Custcat + ',' + Custport + ',' + CustAct + ',' + CustStatus + ',' + Glrtp_id + ',' + Glrtp_Name);
        },
       
      
    });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);
    }
}
function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/CustomerSetup/ErrorPage",
                data: {},
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ErrorPage Error : " + err.message);
    }
}
function OnChangeCustName() {
    $("#custnamelist").css("border-color", "#ced4da");
}
function FetchCustomerData() {
    var isfileexist = $('#customerfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function SearchByUploadStatus() {
    var uploadStatus = $('#item_Status').val();
    if ($('#DttblBtnscustomer tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
    
}
function FetchAndValidateData(uploadStatus) {
    $(".loader1").show();
    var formData = new FormData();
    var file = $("#customerfile").get(0).files[0];
    formData.append("file", file);
    $('#btncustomerImportData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + uploadStatus);
    xhr.send(formData);
    xhr.onreadystatechange = function () {

        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportCustomerDetail').html(xhr.response);
            cmn_apply_datatable("#DttblBtnscustomer")
            $(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btncustomerImportData').prop('disabled', true); // Keep the button disabled
                $('#btncustomerImportData').css('background-color', '#D3D3D3')
            } else {
                $('#btncustomerImportData').prop('disabled', false); // Enable the button
                $('#btncustomerImportData').css('background-color', '#007bff')
            }
        }
    }
}
function BindCustomerAdressDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var customerName = "";
    customerName = clickedrow.find("#customerName").text();
    var formData = new FormData();
    var file = $("#customerfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindCustomerAddress?customerName=' + encodeURIComponent(customerName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#PartialCustomerAddressDetail").css("display", "block");
            $('#PartialCustomerAddressDetail').html(xhr.response);
        }
    }
}
function BindCustomerBranchDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var customerName = "";
    customerName = clickedrow.find("#customerName").text();
    var formData = new FormData();
    var file = $("#customerfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindCustomerBranch?customerName=' + encodeURIComponent(customerName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#BranchMapping").css("display", "block");
            $('#BranchMapping').html(xhr.response);
        }
    }
}
function OnClickErrorDetails(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var customerName = "";
    customerName = clickedrow.find("#customerName").text();
    var formData = new FormData();
    var file = $("#customerfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?customerName=' + encodeURIComponent(customerName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);
        }
    }
}
function ImportDataFromExcel() {
    $(".loader1").show();
    if ($('#hdnNotOk').val() != "0") {
        $('.loader1').hide();
        swal("", 'Invalid data in excel file. Please modify the data and try again..!', "warning");
    }
    else {
        var formData = new FormData();
        var file = $("#customerfile").get(0).files[0];
        formData.append("file", file);

        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportCustomerDetailFromExcel');
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
                        $('#btncustomerImportData').prop('disabled', true);
                        $('#btncustomerImportData').css('background-color', '#D3D3D3')
                        $('#DttblBtnscustomer').DataTable().destroy();
                        $('#DttblBtnscustomer tbody').empty()
                        $('#tblstatus_id').empty();
                        $('#customerfile').val('');
                        $('#PartialImportCustomerData').modal('hide');
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
function Closemodal() {
    $('#DttblBtnscustomer').DataTable().destroy();
    $('#DttblBtnscustomer tbody').empty()
    $('#tblstatus_id').empty();
    $('#customerfile').val('');
    $('#btncustomerImportData').prop('disabled', true);
    $('#btncustomerImportData').css('background-color', '#D3D3D3')
}
function popClose_modal() {
    debugger;
    $("#PartialCustomerAddressDetail").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}
function popClose_Brmodal() {
    debugger;
    $("#BranchMapping").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}

$(document).on("click", "tr", function (event) {/*Add by Hina Sharma on 06-10-2025*/
    OnClickRowDetail(event);
});
function OnClickRowDetail(e) {/*Add by Hina Sharma on 06-10-2025*/
    debugger
    var clickedrow = $(e.target).closest("tr");
    var Cust_id = "";
    var CustAcc_id = "";
    var Curr_id = ""; 
    //Cust_id = clickedrow.find("#hdncustid1").text();
    //CustAcc_id = clickedrow.find("#hdn_custaccid").text();
    var status = clickedrow.find("#cust_status").text().trim();
    if (status == "Drafted") {
        $("#txtLedgerBalance").val("");
        $("#txtOrderValue").val("");
        $("#txtInvoiceValue").val("");
        $("#txtSaleReturn").val("");
    }
    else {
        Cust_id = clickedrow.find("td#hdncustid1").clone().children().remove().end().text().trim();
        CustAcc_id = clickedrow.find("#hdn_custaccid").text().trim();
        Curr_id = clickedrow.find("#hdn_currid").text();
        BindAllDetail_LedgrSide(Cust_id, CustAcc_id, Curr_id)
    }
}
function BindAllDetail_LedgrSide(Cust_id,CustAcc_id, Curr_id) {/*Add by Hina Sharma on 06-10-2025*/
    debugger;
    sessionStorage.setItem("ShowLoader", "N");
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/CustomerSetup/GetLedgerDetail",
                data: {
                    Cust_id: Cust_id,
                    CustAcc_id: CustAcc_id,
                    Curr_id: Curr_id
                },
                success: function (data) {
                    if (data == 'ErrorPage') {
                        LSO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#txtLedgerBalance").val(arr.Table[0].LedgerBal);
                        }
                        else {
                            $("#txtLedgerBalance").val("");
                         }
                        if (arr.Table1.length > 0) {
                            $("#txtOrderValue").val(arr.Table1[0].OrderValue);
                        }
                        else {
                             $("#txtOrderValue").val("");
                        }
                        if (arr.Table2.length > 0) {
                            $("#txtInvoiceValue").val(arr.Table2[0].InvoiceValue);
                        }
                        else {
                            $("#txtInvoiceValue").val("");
                        }
                        if (arr.Table3.length > 0) {
                            $("#txtSaleReturn").val(arr.Table3[0].ReturnValue);
                        }
                        else {
                            $("#txtSaleReturn").val("");
                        }
                    }
                    else {
                        $("#txtLedgerBalance").val("");
                        $("#txtOrderValue").val("");
                        $("#txtInvoiceValue").val("");
                        $("#txtSaleReturn").val("");
                    }
                    hideLoader();
                }
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
        hideLoader();
    }
}
