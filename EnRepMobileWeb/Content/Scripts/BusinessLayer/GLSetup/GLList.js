$(document).ready(function () {
    debugger;
    $("#GLAccount #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var clickedrow = $(e.target).closest("tr");
            var GLCode = clickedrow.children("#accid").text();
            var act_status_tr = clickedrow.children("#act_status_tr").text();
            var acc_grp_id = clickedrow.children("#acc_grp_id").text();
            var acc_type = clickedrow.children("#acc_type").text();
            var roa = clickedrow.children("#roa").text();
            var plr = clickedrow.children("#plr").text();
            var ibt = clickedrow.children("#ibt").text();
            var iwt = clickedrow.children("#iwt").text();
            var egl = clickedrow.children("#egl").text();
            var sta = clickedrow.children("#sta").text();
           
            if (GLCode != "" && GLCode != null) {
                window.location.href = "/BusinessLayer/GLList/EditGL/?GLId=" + GLCode + "&act_status_tr=" + act_status_tr + "&acc_grp_id=" + acc_grp_id + "&acc_type=" + acc_type + "&roa=" + roa + "&plr=" + plr + "&ibt=" + ibt + "&iwt=" + iwt + "&egl=" + egl + "&sta=" + sta + "&ListFilterData=" + ListFilterData;
            }
        }
        catch (err) {
            debugger
            alert(err.message);
        }
    });

    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var acc_id = clickedrow.children("#accid").text();
        var acc_grpid = clickedrow.children("#acc_grp_id").text();
        var acc_type = clickedrow.children("#acc_type").text();
        GetGLSetup_Balance(acc_id, acc_grpid, acc_type);
    });

    var PageName = sessionStorage.getItem("MenuName");
    $('#GLListPageName').text(PageName);
    $("#GLNameList").select2({
        //ajax: {
        //    url: $("#GLListName").val(),
        //    data: function (params) {
        //        var queryParameters = {
        //            //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
        //            ddlGroup: params.term, // search term like "a" then "an"
        //            Group: params.page
        //        };
        //        return queryParameters;
        //    },
        //    dataType: "json",
        //    cache: true,
        //    delay: 250,            
        //    contentType: "application/json; charset=utf-8",
        //    processResults: function (data, params) {
        //        debugger;
        //        params.page = params.page || 1;
        //        return {
        //            results: $.map(data, function (val, item) {
        //                return { id: val.ID, text: val.Name };
        //            })
        //        };
        //    }
        //},
    });
    $("#AccountGroupName").select2({

        //ajax: {
        //    url: $("#GLAccGrp").val(),

        //    data: function (params) {
        //        var queryParameters = {
        //            ddlGroup: params.term,
        //            Group: params.page
        //        };
        //        return queryParameters;
        //    },
        //    dataType: "json",
        //    cache: true,
        //    delay: 250,            
        //    contentType: "application/json; charset=utf-8",
        //    processResults: function (data, params) {
        //        debugger;
        //        params.page = params.page || 1;
        //        return {
        //            results: $.map(data, function (val, Item) {
        //                return { id: val.ID, text: val.Name };
        //            }),
        //        };
        //    },

        //},
    });
    
}); 
function FilterGLList() {
    debugger;
    var GLID = $("#GLNameList").val();
    var GRPID = $("#AccountGroupName").val();
    var GLAct = $("#GLActStatus").val();
    var GLAcctype = $("#acc_type").val();
    $.ajax({
        type: "POST",
        url: "/BusinessLayer/GLList/GetGLListFilter",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            GLID: GLID,
            GRPID: GRPID,
            GLAct: GLAct,
            GLAcctype: GLAcctype, 
        },
        success: function (data) {
            debugger;
            $('#GLAccount').html(data);
            $('#ListFilterData').val(GLID + ',' + GRPID + ',' + GLAct + ',' + GLAcctype);
        },


    });
}


function GLDetail() {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/GLAccountSetup/GLAccountSetup",
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
function EditGLDetail(GLID) {
    debugger;
    sessionStorage.removeItem("EditGLCode");
    sessionStorage.setItem("EditGLCode", GLID);
    try {
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/GLAccountSetup/GLAccountSetup",
                //data: {},
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
                url: "/BusinessLayer/GLAccountSetup/ErrorPage",
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
function GetGLSetup_Balance(acc_id, acc_grpid, acc_type) // This Function Added By Nitesh 03112023 12:12 for opening and Closing Balence
{
    debugger;
    var ValDecDigit = $("#ValDigit").text();///Amount
    if (acc_id != null && acc_id != "") {
        sessionStorage.setItem("ShowLoader", "N");
        $.ajax({
    
            type: "POST",
            url: "/BusinessLayer/GLList/GetGL_list", //C: //\BusinessLayer\Controllers\GLSetup\GLListController.cs
            data: { acc_id:acc_id, acc_grpid:acc_grpid, acc_type:acc_type },
            dataType: "json",
            success: function (data) {
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
                    return false;
                }
                if (data != null && data != "") /*For Chekin Json Data is return or not */ {
                    var arr = [];
                    //debugger;
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        $("#curr_list").val(arr.Table[0].curr_name);
                        $("#curr_list1").val(arr.Table[0].curr_name);
                        $("#op_balence").val(arr.Table[0].opening_bal);
                        $("#Op_BalanceType").val(arr.Table[0].Op_BalanceType);
                        $("#cl_balence").val(arr.Table[0].cl_Balance);
                        $("#BalanceType").val(arr.Table[0].Cl_bal_type);
                    }
                    else {
                        $("#curr_list").text("");

                    }
                }
                else {
                    $("#curr_list").text("");

                }
            }
        });
    }
}
function FetchGLData() {
    var isfileexist = $('#GLfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function SearchByUploadStatus() {
    var uploadStatus = $('#Search_Status').val();
    if ($('#DttblGL tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
}
function FetchAndValidateData(uploadStatus) {
    debugger
    $(".loader1").show();
    var formData = new FormData();
    var file = $("#GLfile").get(0).files[0];
    formData.append("file", file);
    $('#btnGLImportData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + uploadStatus);
    xhr.send(formData);
    xhr.onreadystatechange = function () {

        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportGLDetail').html(xhr.response);
            cmn_apply_datatable("#DttblGL")
            $(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btnGLImportData').prop('disabled', true); // Keep the button disabled
                $('#btnGLImportData').css('background-color', '#D3D3D3')
            } else {
                $('#btnGLImportData').prop('disabled', false); // Enable the button
                $('#btnGLImportData').css('background-color', '#007bff')
            }
        }
    }
}
function OnClickErrorDetails(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var accountname = "";
    accountname = clickedrow.find("#accountname").text();
    var formData = new FormData();
    var file = $("#GLfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?accountname=' + encodeURIComponent(accountname));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#divErrorDetail').html(xhr.response);
        }
    }
}
function BindGLBranchDetails(e) {
    var clickedrow = $(e.target).closest("tr");
    var accountname = "";
    accountname = clickedrow.find("#accountname").text();
    var formData = new FormData();
    var file = $("#GLfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/BindGLBranch?accountname=' + encodeURIComponent(accountname));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            $("#BranchMapping").css("display", "block")
            $('#BranchMapping').html(xhr.response);
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
        var file = $("#GLfile").get(0).files[0];
        formData.append("file", file);

        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportGLDetailFromExcel');
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
                        $('#btnGLImportData').prop('disabled', true);
                        $('#btnGLImportData').css('background-color', '#D3D3D3')
                        $('#DttblGL').DataTable().destroy();
                        $('#DttblGL tbody').empty()
                        $('#tblstatus_id').empty();
                        $('#GLfile').val('');
                        $('#PartialImportGLData').modal('hide');
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
    $('#DttblGL').DataTable().destroy();
    $('#DttblGL tbody').empty()
    $('#tblstatus_id').empty();
    $('#GLfile').val('');
    $('#btnGLImportData').prop('disabled', true);
    $('#btnGLImportData').css('background-color', '#D3D3D3')
}
function popClose_Brmodal() {
    debugger;
    $("#BranchMapping").css("display", "none")
    $(".modal-backdrop").css("display", "none")
}
