/************************************************
Javascript Name:Parameter Setup List
Created By:Asif Nasim
Created Date: 31-03-2021
Description: This Javascript use for the Parameter Setup List many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    $("#itemList").select2();
    $("#GrpList").select2();
    //var PageName = sessionStorage.getItem("MenuName");
    //$('#LSOListPageName').text(PageName);

    $("#QCparametersetuptbl >tbody").bind("dblclick", function (e) {
        debugger;
        var ListFilterData = $('#ListFilterData').val();
        var clickedrow = $(e.target).closest("tr");
        /* var Item_Id = clickedrow.children("td:eq(3)").text();*/
        var Item_Id = clickedrow.children("#item_id").text();
        if (Item_Id != null && Item_Id != "") {
            window.location.href = "/BusinessLayer/ItemParameter/ItemParameterDetailListTbl/?itemID=" + Item_Id + "&ListFilterData=" + ListFilterData;
        }
   
    });
    


    if ($("#hdFromdate").val() != null) {
        debugger;
        var fromDate = new Date($("#hdFromdate").val());

        var month = (fromDate.getMonth() + 1);
        var day = fromDate.getDate();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;

        var today = fromDate.getFullYear() + '-' + day + '-' + month;
        $("#txtFromdate").val(today);
    }

    $("txtFromdate").on("onchange", function () {
        debugger;
        this.setAttribute(
            "data-date",
            moment(this.value, "YYYY-MM-DD")
                .format(this.getAttribute("data-date-format"))
        )
    }).trigger("change")

    $("#itemNameList").select2({
        ajax: {
            url: $("#ItemListName").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
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
    $("#itemGrpList").select2({
        ajax: {
            url: $("#ItemGrpName").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
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
});
function CmnGetWorkFlowDetails(e) {

}
function ItemParameterDetail() {
    debugger;
    try {
       
        $.ajax(
            {
                type: "POST",
                url: "/BusinessLayer/ItemParameter/ItemParameterDetailListTbl",
                data: { paraID: ParaId, itemID: Item_Id },
                success: function (data) {
                    debugger;
                    $("#rightPageContent").empty().html(data);
                },
            });

    }
    catch (err) {
        console.log("ItemSetup Error : " + err.message);
    }
}
function BtnSearch() {
    debugger;
    FilterListItem();
}
function FilterListItem() {
    debugger;
    var ItemID = $("#itemList").val();
    var ItemGrpID = $("#GrpList").val();



    $.ajax({
        type: "POST",
        url: "/BusinessLayer/ItemParameter/GetItemParamListFilter",/*Controller=ItemSetup and Fuction=GetItemList*/
        data: {
            ItemID: ItemID,
            ItemGrpID: ItemGrpID,
        },
        success: function (data) {
            debugger;
            $('#ItmParamListBody').html(data);
            $('#ListFilterData').val(ItemID + ',' + ItemGrpID);
        },
    });
}
function FetchQCItemData() {
    var isfileexist = $('#QCitemparamfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function SearchByUploadStatus() {
    var uploadStatus = $('#QC_Item_Status').val();
    if ($('#DttblBtnsQCItem tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }

}
function FetchAndValidateData(uploadStatus) {
    $(".loader1").show();
    var formData = new FormData();
    var file = $("#QCitemparamfile").get(0).files[0];
    formData.append("file", file);
    $('#btnqcitemparamData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + uploadStatus);
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        console.log(xhr);
        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportQCItemDetail').html(xhr.response);
            cmn_apply_datatable("#DttblBtnsQCItem")
            //$(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btnqcitemparamData').prop('disabled', true); // Keep the button disabled
                $('#btnqcitemparamData').css('background-color', '#D3D3D3')
            } else {
                $('#btnqcitemparamData').prop('disabled', false); // Enable the button
                $('#btnqcitemparamData').css('background-color', '#007bff')
            }
        }
    }
}
function OnClickErrorDetails(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var ItemName = ""; var paramname = "";
    ItemName = clickedrow.find("#ItemName").text().trim();
    paramname = clickedrow.find("#paramname").text().trim();
    var formData = new FormData();
    var file = $("#QCitemparamfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?ItemName=' + encodeURIComponent(ItemName) + '&paramname=' + encodeURIComponent(paramname));
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        hideLoader();
        if (xhr.readyState == 4 && xhr.status == 200) {
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Please fill all mandatory fields.") {
                swal("", "Please fill all mandatory fields", "warning");
                $('#ErrorDetail').css('display', 'none')
            }
            else if (responseMessage == "ErrorPage")
            {
                swal("", "something went wrong", "warning");
            }
            else {
                $('#divErrorDetail').html(xhr.response);
            }
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
        var file = $("#QCitemparamfile").get(0).files[0];
        formData.append("file", file);

        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportQCItemDetailFromExcel');
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
                        $('#btnqcitemparamData').prop('disabled', true);
                        $('#btnqcitemparamData').css('background-color', '#D3D3D3')
                        $('#DttblBtnsQCItem').DataTable().destroy();
                        $('#DttblBtnsQCItem tbody').empty()
                        $('#tblstatus_id').empty();
                        $('#customerfile').val('');
                        $('#PartialImportQCItemData').modal('hide');
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
    $('#DttblBtnsQCItem').DataTable().destroy();
    $('#DttblBtnsQCItem tbody').empty()
    $('#tblstatus_id').empty();
    $('#QCitemparamfile').val('');
    $('#btnqcitemparamData').prop('disabled', true);
    $('#btnqcitemparamData').css('background-color', '#D3D3D3')
}

function UploadExportExcel() {

    $('#export').click(function () {
        debugger;
        var ItemID = $("#itemList").val();
        var ItemGrpID = $("#GrpList").val();


        var searchValue = $("#QCparametersetuptbl_filter input[type=search]").val();
        window.location.href = "/BusinessLayer/ItemParameter/ExcelDownload?searchValue=" + searchValue + "&ItemID=" + ItemID + "&ItemGrpID=" + ItemGrpID

    });
}
