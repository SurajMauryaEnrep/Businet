$(document).ready(function () {
    debugger;    
    
    BingGLReporting();
    $("#SuppListBody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData ').val();
            var clickedrow = $(e.target).closest("tr");
            /*var SuppCode = clickedrow.children("#suppid").text();*/
            var SuppCode = clickedrow.find("td#suppid").clone().children().remove().end().text().trim();
            var act_status = clickedrow.children("#act_status").text();
            var on_hold = clickedrow.children("#on_hold").text();
            if (SuppCode != "" && SuppCode != null) {
                window.location.href = "/BusinessLayer/SupplierList/EditSupplier/?SuppId=" + SuppCode + "&act_status=" + act_status + "&on_hold=" + on_hold + "&ListFilterData=" + ListFilterData;
            };
        }
        
        catch (err) {
            debugger
            alert(err.message);
        }
    });
    var PageName = sessionStorage.getItem("MenuName");
    $('#SupplierListPageName').text(PageName);


    $("#suppnamelist").select2()
//    {
    //    ajax: {
    //        url: $("#SuppListName").val(),
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

    //var category = $("#suuppcat").val();
    //debugger;
    //if (category != null) {

    //}
    //else {
    //    Getcategory();
    //}
    //var suppport = $("#suppport").val();
    //debugger;
    //if (suppport != null) {

    //}
    //else {
    //    Getsuppport();
    //}
});
function BingGLReporting() {
    debugger;
    $("#ID_GlRepoting_Group").select2({

        ajax: {
            type: "POST",
            url: "/BusinessLayer/SupplierList/GetGlReportingGrp",
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
function FilterListSupp() {
    debugger;
    var SuppID = $("#suppnamelist").val();
    var Supptype = $("#supp_type").val();
    var Suppcat = $("#suuppcat").val();
    var Suppport = $("#suppport").val();
    var SuppAct = $("#supp_act").val();
    /*   var SuppStatus = $("#supp_status option:selected").text();*/
    var SuppStatus = $("#supp_status").val();
    var Glrtp_id = $("#hdn_GlRepoting_Group").val();
    var Glrtp_Name = $("#hdn_GlRepoting_GroupName").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/SupplierList/GetSupplierListFilter",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            SuppID: SuppID,
            Supptype: Supptype,
            Suppcat: Suppcat,
            Suppport: Suppport,
            SuppAct: SuppAct,
            SuppStatus: SuppStatus,
            Glrtp_id: Glrtp_id,

        },
        success: function (data) {
            debugger;
            $('#SuppListBody').html(data);
            $('#ListFilterData').val(SuppID + ',' + Supptype + ',' + Suppcat + ',' + Suppport + ',' + SuppAct + ',' + SuppStatus + ',' + Glrtp_id + ',' + Glrtp_Name);
        },


    });
}


//function Getcategory() {
//    $.ajax({
//        type: "POST",
//        url: "/BusinessLayer/SupplierList/Getcategory",/*Controller=ItemSetup and Fuction=Getwarehouse*/
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
//                var s = '<option value="-1">ALL</option>';
//                for (var i = 0; i < arr.Table.length; i++) {
//                    s += '<option value="' + arr.Table[i].setup_id + '">' + arr.Table[i].setup_val + '</option>';
//                }
//                $("#suuppcat").html(s);

//            }
//        },
//        error: function (Data) {

//        }
//    });
//};

//function Getsuppport() {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/BusinessLayer/SupplierList/Getsuppport",/*Controller=SupplierSetup and Fuction=Getsuppport*/
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
//                var s = '<option value="-1">ALL</option>';
//                for (var i = 0; i < arr.Table.length; i++) {
//                    s += '<option value="' + arr.Table[i].setup_id + '">' + arr.Table[i].setup_val + '</option>';
//                }
//                $("#suppport").html(s);

//            }
//        },
//        error: function (Data) {

//        }
//    });
//};

function SupplierDetail() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/SupplierList/SupplierDetail",
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

function EditSuppDetail(SuppID) {
    debugger;
    sessionStorage.removeItem("EditSuppCode");
    sessionStorage.setItem("EditSuppCode", SuppID);
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/SupplierSetup/SupplierDetail",
                
                data: {},
                success: function (data) {
                    if (data == 'ErrorPage') {
                        ErrorPage();
                        return false;
                    }
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });
    } catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}

function ErrorPage() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/SupplierList/ErrorPage",
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
function FetchSupplierData() {
    var isfileexist = $('#supplierfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function SearchByUploadStatus() {
    debugger
    var uploadStatus = $('#supplier_status').val();
    if ($('#DttblBtnssupplier tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
}
function FetchAndValidateData(uploadStatus) {
    debugger
    $(".loader1").show();
    var formData = new FormData();
    var file = $("#supplierfile").get(0).files[0];
    formData.append("file", file);
    $('#btnsupplierImportData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + uploadStatus);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportSupplierDetail').html(xhr.response);
            cmn_apply_datatable("#DttblBtnssupplier")
            $(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btnsupplierImportData').prop('disabled', true); // Keep the button disabled
                $('#btnsupplierImportData').css('background-color', '#D3D3D3')
            } else {
                $('#btnsupplierImportData').prop('disabled', false); // Enable the button
                $('#btnsupplierImportData').css('background-color', '#007bff')
            }
        }
    }
}
function BindSupplierAdressDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var supplierName = "";
    supplierName = clickedrow.find("#supplierName").text();
    var formData = new FormData();
    var file = $("#supplierfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindSupplierAddress?supplierName=' + encodeURIComponent(supplierName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#PartialCustomerAddressDetail").css("display", "block")
            $('#PartialCustomerAddressDetail').html(xhr.response);
        }
    }
}
function BindSupplierBranchDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var supplierName = "";
    supplierName = clickedrow.find("#supplierName").text();
    var formData = new FormData();
    var file = $("#supplierfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindSupplierBranch?supplierName=' + encodeURIComponent(supplierName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#BranchMapping").css("display", "block")
            $('#BranchMapping').html(xhr.response);
        }
    }
}
function OnClickErrorDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var supplierName = "";
    supplierName = clickedrow.find("#supplierName").text();
    var formData = new FormData();
    var file = $("#supplierfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?supplierName=' + encodeURIComponent(supplierName));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);
        }
    }
}
function ImportDataFromExcel() {
    debugger;
    $(".loader1").show();
    if ($('#hdnNotOk').val() != "0") {
        $('.loader1').hide();
        swal("", 'Invalid data in excel file. Please modify the data and try again..!', "warning");
    }
    else {
        var formData = new FormData();
        var file = $("#supplierfile").get(0).files[0];
        formData.append("file", file);

        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportSupplierDetailFromExcel');
        xhr.send(formData);
        xhr.onreadystatechange = function () {
            $(".loader1").hide();
            if (xhr.readyState == 4 && xhr.status == 200) {
                if (xhr.responseText.toLowerCase().includes("Success")) {
                    swal("", xhr.responseText, "success");
                    $('.loader1').hide();
                }
                else {
                    var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
                    if (responseMessage == "Data saved successfully") {
                        swal("", xhr.responseText, "success");
                        $('#btnsupplierImportData').prop('disabled', true); 
                        $('#btnsupplierImportData').css('background-color', '#D3D3D3');
                        $('#DttblBtnssupplier').DataTable().destroy();
                        $('#DttblBtnssupplier tbody').empty()
                        $('#tblstatus_id').empty();
                        $('#supplierfile').val('');
                        $('#PartialImportSupplierData').modal('hide');
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
    debugger
    $('#DttblBtnssupplier').DataTable().destroy();
    $('#DttblBtnssupplier tbody').empty()
    $('#tblstatus_id').empty();
    $('#supplierfile').val('');
    $('#btnsupplierImportData').prop('disabled', true);
    $('#btnsupplierImportData').css('background-color', '#D3D3D3')
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


$(document).on("click", "tr", function (event) {/*Add by Hina Sharma on 08-10-2025*/
    OnClickRowDetail(event);
});
function OnClickRowDetail(e) {/*Add by Hina Sharma on 08-10-2025*/
    debugger
    var clickedrow = $(e.target).closest("tr");
    var Supp_id = "";
    var SuppAcc_id = "";
    var Curr_id = "";
    var status = clickedrow.find("#supp_status").text().trim();
    if (status == "Drafted") {
        $("#txt_SuppLedgerBalance").val("");
        $("#txt_SuppOrderValue").val("");
        $("#txt_SuppPendingReceiptVal").val("");
        
    }
    else {
        Supp_id = clickedrow.find("td#suppid").clone().children().remove().end().text().trim();
        SuppAcc_id = clickedrow.find("#hdn_suppaccid").text().trim();
        Curr_id = clickedrow.find("#hdnsupp_currid").text();
        BindAllDetail_LedgrSide(Supp_id, SuppAcc_id, Curr_id)
    }
    
}
function BindAllDetail_LedgrSide(Supp_id, SuppAcc_id, Curr_id) {/*Add by Hina Sharma on 08-10-2025*/
    debugger;
    sessionStorage.setItem("ShowLoader", "N");
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/SupplierList/GetLedgerDetail",
                data: {
                    Supp_id: Supp_id,
                    SuppAcc_id: SuppAcc_id,
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
                            $("#txt_SuppLedgerBalance").val(arr.Table[0].LedgerBal);
                        }
                        else {
                            $("#txt_SuppLedgerBalance").val("");
                        }
                        if (arr.Table1.length > 0) {
                            $("#txt_SuppOrderValue").val(arr.Table1[0].OrderValue);
                        }
                        else {
                            $("#txt_SuppOrderValue").val("");
                        }
                        if (arr.Table2.length > 0) {
                            $("#txt_SuppPendingReceiptVal").val(arr.Table2[0].PendingRecpt_val);
                        }
                        else {
                            $("#txt_SuppPendingReceiptVal").val("");
                        }
                        
                    }
                    else {
                        $("#txt_SuppLedgerBalance").val("");
                        $("#txt_SuppOrderValue").val("");
                        $("#txt_SuppPendingReceiptVal").val("");
                    }
                    hideLoader();
                }
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
        hideLoader();
    }
}
