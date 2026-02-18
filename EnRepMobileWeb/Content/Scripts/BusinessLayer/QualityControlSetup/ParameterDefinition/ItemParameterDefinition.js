
$(document).ready(function () {
  
});

function CmnGetWorkFlowDetails(e) {

}

function EditPrmtrDef(e) {
    var CurrentRow = $(e.target).closest('tr');
    debugger
    $("#hdnSavebtn").val(null);
    //var ParamID = CurrentRow.find("td:eq(0)").text();
    //var ParamName = CurrentRow.find("td:eq(2)").text();
    //var Paratype = CurrentRow.find("td:eq(3)").text();
    var ParamID = CurrentRow.find("#paramid").text();
    var ParamName = CurrentRow.find("#paramname").text();
    var Paratype = CurrentRow.find("#parmtypval").text();
    $("#ParameterName").val(ParamName).trigger('change');
    $("#ParamID").val(ParamID);
    $("#ParameterType").val(Paratype).trigger('change');
   // $("#remarks").val(HsnRemarks);//btn_saveBIN
    $("#btn_save").val("Update");

    var AddNewData = $("#AddNewData").val();
    if (AddNewData == "N") {
        $("#SaveBtnEnable").css("display", "block");
        $("#ParameterName").attr("disabled", false);
        $("#ParameterType").attr("disabled", false);
    }
}
function BinSaveBtnClick() {
    debugger
    var btn = $("#hdnSavebtn").val(); /**Added this NItesh 03-01-2024 for Only One Click save btn when dblclick save btn is Disable**/
    if (btn == "AllreadyClick") {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").prop("disabled", true); /*End*/
    }

    var ErrorFlag = "N";

    if (CheckVallidation("ParameterName", "vmParameterName") == false) {
        ErrorFlag = "Y";
    }
    if (CheckVallidation("ParameterType", "vmParameterType") == false) {
        ErrorFlag = "Y";
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#hdnSavebtn").val("AllreadyClick");
        return true;
    }
}
function OnChangeParameterType() {
    CheckVallidation("ParameterType", "vmParameterType");
}
function OnChangeParameterName() {
    CheckVallidation("ParameterName", "vmParameterName");
}


function functionConfirm(e) {

    swal({
        title: $("#deltital").text() + "?",
        text: $("#deltext").text() + "!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (isConfirm) {
            debugger
            var currentRow = $(e.target).closest('tr');
            /*var ParamID = currentRow.find("td:eq(0)").text();*/
            var ParamID = currentRow.find("#paramid").text();
            window.location.href = "/BusinessLayer/ParameterDefinition/QCParameterItemDelete/?paramId=" + ParamID;
            //$.ajax({
            //    type: 'POST',
            //    url: '/BusinessLayer/ParameterDefinition/QCParameterItemDelete',
            //    data: {
            //        ParamID
            //    },
            //    success: function (data) {
            //        if (data == "Used") {
            //            swal("", $("#DependencyExist").text(), "warning");
            //        }
            //        if (data == "Delete") {
            //            swal("", $("#deletemsg").text(), "success");

            //            currentRow.html("");
            //            $("#ParameterName").val("");
            //            $("#ParamID").val("");
            //            $("#ParameterType").val(0);
            //            $("#btn_save").val("Save");
            //        }
            //        return true;
            //    }
            //})
           

        } else {
            return false;
        }
    });
    return false;
}
function FetchQCPDData() {
    debugger;
    var isfileexist = $('#qcpdfile').val();
    if (isfileexist != null && isfileexist != "") {
        FetchAndValidateData('0');
    }
    else {
        swal("", "Please choose file", "warning");
    }
}
function SearchData() {
    var uploadStatus = $('#qc_pd_ActStatus').val();
    if ($('#DttblBtnsqc_pd tbody tr').length > 0) {
        FetchAndValidateData(uploadStatus);
    }
}
function FetchAndValidateData(uploadStatus) {
    $(".loader1").show();
    var formData = new FormData();
    var file = $("#qcpdfile").get(0).files[0];
    formData.append("file", file);
    $('#btnqcImportData').prop('disabled', true);
    var PName = window.location.pathname.split('/')
    var addr = '/' + PName[1] + '/' + PName[2]
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ValidateExcelFile?uploadStatus=' + uploadStatus);
    xhr.send(formData);
    xhr.onreadystatechange = function () {

        if (xhr.readyState == 4 && xhr.status == 200) {
            $('#PartialImportQCPDDetail').html(xhr.response);
            cmn_apply_datatable("#DttblBtnsqc_pd")
            $(".loader1").hide();
            var responseMessage = xhr.responseText.trim().replace(/^"|"$/g, '');
            if (responseMessage == "Excel file is empty. Please fill data in excel file and try again" || responseMessage == "ErrorPage") {
                $('#btnqcImportData').prop('disabled', true); // Keep the button disabled
                $('#btnqcImportData').css('background-color', '#D3D3D3')
            } else {
                $('#btnqcImportData').prop('disabled', false); // Enable the button
                $('#btnqcImportData').css('background-color', '#007bff')
            }
        }
    }
}
function OnClickErrorDetails(e) {
    debugger
    var clickedrow = $(e.target).closest("tr");
    var paramname = '';
    paramname = clickedrow.find("#param_name").text().trim();
    var formData = new FormData();
    var file = $("#qcpdfile").get(0).files[0];
    formData.append("file", file);
    var PName = window.location.pathname.split('/');
    var addr = '/' + PName[1] + '/' + PName[2];
    var xhr = new XMLHttpRequest();
    xhr.open('POST', addr + '/ShowValidationError?paramname=' + encodeURIComponent(paramname));
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
        var file = $("#qcpdfile").get(0).files[0];
        formData.append("file", file);

        var PName = window.location.pathname.split('/')
        var addr = '/' + PName[1] + '/' + PName[2]
        var xhr = new XMLHttpRequest();
        xhr.open('POST', addr + '/ImportPDDetailFromExcel');
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
                        $('#btnqcImportData').prop('disabled', true);
                        $('#btnqcImportData').css('background-color', '#D3D3D3')
                        $('#DttblBtnsqc_pd').DataTable().destroy();
                        $('#DttblBtnsqc_pd tbody').empty()
                        $('#tblpdstatusid').empty();
                        $('#qcpdfile').val('');
                        $('#Partial_QC_PD').modal('hide');
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
    $('#DttblBtnsqc_pd').DataTable().destroy();
    $('#DttblBtnsqc_pd tbody').empty()
    $('#tblpdstatusid').empty();
    $('#qcpdfile').val('');
    $('#btnqcImportData').prop('disabled', true);
    $('#btnqcImportData').css('background-color', '#D3D3D3')
}






