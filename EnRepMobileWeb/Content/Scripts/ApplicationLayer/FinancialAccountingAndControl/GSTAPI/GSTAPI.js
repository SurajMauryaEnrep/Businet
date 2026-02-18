//document.ready function (it will load data before page is ready to render)
$(document).ready(function () {
    //calling to date validation inside doc.ready method 
    /*EnableDisablePostEinvButton();*/
    SetMinToDate();
});
//Enable Submit button if atleast one Envoice OR Eway-Bill is selected (else disable)
function EnableDisablePostEinvButton() {
    debugger;
    var dataType = $("#ddldataType").val();
    if (dataType != "SR" && dataType != "PR") {
        var count = CountNoOfInvEntriesToPost();
        if (count > 0) {
            $('#btn_generate').removeAttr('disabled');
            $('#btn_generate').removeClass('grayScale');
        }
        else {
            $('#btn_generate').attr('disabled', true);
            $('#btn_generate').addClass('grayScale');
        }
    }
}
//check on uncheck all checkboxes inside table
function CheckUncheckAllInv() {
    debugger;
    /*CountNoOfEntriesToPost();*/
    var chkhdr = $('#chkAllInv').is(':checked');
    if (chkhdr == true) {
        $('#datatable-buttons >tbody > tr').each(function () {
            if ($(this).find('#chkinv').prop('disabled') != true) {
                $(this).find('#chkinv').prop('checked', true);
            }
        });
    }
    else {
        $('#datatable-buttons >tbody> tr').each(function () {
            if ($(this).find('#chkinv').prop('disabled') != true) {
                $(this).find('#chkinv').prop('checked', false);
            }
        });
    }
}
// check uncheck all EWB checkboxes
function CheckUncheckAllEWB(e) {
    debugger;
    var chkhdr = $('#chkAllEwb').is(':checked');
    if (chkhdr == true) {
        $('#datatable-buttons >tbody > tr').each(function () {
            if ($(this).find('#chkewb').prop('disabled') != true) {
                $(this).find('#chkewb').prop('checked', true);
            }
        });
    }
    else {
        $('#datatable-buttons >tbody> tr').each(function () {
            if ($(this).find('#chkewb').prop('disabled') != true) {
                $(this).find('#chkewb').prop('checked', false);
            }
        });
    }
}
function HideShowDiv() {
    debugger;
    var dataType = $('#ddldataType').val();
    if (dataType == "PR") {
        $('#Div_GSTR_Date').css('display', 'block');
    } else {
        $('#Div_GSTR_Date').css('display', 'none');
    }
    if (dataType == "SR" || dataType == "PR") {
        $('#divMonthYear').css('display', 'block');
        $('#divGstCat').css('display', 'block');
        $('#divFromToDate').css('display', 'none');
        $('#divStatus').css('display', 'none');
        $('#li_BtnGSTR_Preview').css('display', '');/* Added by Suraj Maurya on 15-10-2025 */
        // SET CURRENT MONTH AS SELECTED VALUE
        var currentDate = new Date();
        // Set the date to the first day of the current month
        var firstDay = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
        // Format the date in yyyy-MM-dd format
        var yyyy = firstDay.getFullYear();
        var mm = String(firstDay.getMonth() + 1).padStart(2, '0'); // Months are zero-indexed
        var dd = String(firstDay.getDate()).padStart(2, '0');
        var formattedDate = yyyy + '-' + mm + '-' + dd;
        $('#ddlMonthYear').val(formattedDate);
    }
    else {
        $('#divMonthYear').css('display', 'none');
        $('#divGstCat').css('display', 'none');
        $('#divFromToDate').css('display', 'block');
        $('#divStatus').css('display', 'block');
        $('#li_BtnGSTR_Preview').css('display', 'none');/* Added by Suraj Maurya on 15-10-2025 */
    }
}
//Get Gst Posting Data
function GetGstPostingData() {
    debugger;
    try {
        
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
        var dataType = $("#ddldataType").val();
        var mnthYear = $("#ddlMonthYear").val();
        var GstCat = $("#Gst_Cat").val();
        if (dataType == "SR" || dataType == "PR") {
            $('#btn_generate').removeAttr('disabled');
            $('#btn_generate').removeClass('grayScale');
            fromDate = mnthYear;
            toDate = mnthYear;
        }
        else {
            $('#btn_generate').attr('disabled', true);
            $('#btn_generate').addClass('grayScale');
        }
        var docStatus = $("#ddlDocStatus").val();
        ('.dttbl1')[0].id = "datatable-buttons";

        let GSTR_DateOption = "B";
        if ($("#GSTR_DateInvWise").is(":checked")) {
            GSTR_DateOption = "I"
        }

        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/GSTAPI/GetSlsInvDetails",
            data: {
                fromDate: fromDate,
                toDate: toDate,
                dataType: dataType,
                docStatus: docStatus,
                GSTR_DateOption: GSTR_DateOption,
                GstCat: GstCat
            },
            success: function (data) {
                debugger;
                $('#divGstApiPostingData').html(data);
                EnableDisablePostEinvButton();
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("GET Gst Api Posting Details Error : " + err.message);
    }
}
//Check Unc:Check Inv CheckBox
function CheckInvIfEWBChecked(e) {
    debugger;
    var clickedRow = $(e.target).closest('tr');
    var chk1 = clickedRow.find('#chkewb').is(':checked');
    if (chk1 == true) {
        if (clickedRow.find('#chkinv').prop('disabled') != true) {
            clickedRow.find('#chkinv').prop('checked', true);
        }
    }
    //else {
    //    clickedRow.find('#chkinv').prop('checked', false);
    //}
}
//on check single checkbox
function chkunchkallsingle() {
    debugger;
    var chkhdr = $('#chkallsingle').is(':checked');
    if (chkhdr == true) {
        $('#datatable-buttons >tbody > tr').each(function () {
            if ($(this).find('#chk1').prop('disabled') != true) {
                $(this).find('#chk1').prop('checked', true);
            }
        });
    }
    else {
        $('#datatable-buttons >tbody> tr').each(function () {
            if ($(this).find('#chk1').prop('disabled') != true) {
                $(this).find('#chk1').prop('checked', false);
            }
        });
    }
}
//Getting invoice item details
function BindInvItemDetails(e) {
    debugger;
    var url = "/ApplicationLayer/GSTAPI/SalesItemsDetails";
    var currentrow = $(e.target).closest("tr");
    var invNo = currentrow.find("#txtInv_no").text();
    var invDt = currentrow.find("#txtInv_dt").text();
    var dataType = currentrow.find("#txtDataType").text();
    if (dataType == "SSI" || dataType == "SSC") {
        url = "/ApplicationLayer/GSTAPI/ServiceSalesItemsDetails";
    }
    $.ajax({
        type: "POST",
        url: url,
        data: {
            dataType: dataType, invNo: invNo, invDt: invDt
        },
        success: function (data) {
            debugger;
            //cmn_delete_datatable("#tbl_InvoiceDetail");
            if (dataType == "SSI" || dataType == "SSC") {
                $("#ServiceInvoiceDetailPopUp").html(data);
                cmn_apply_datatable("#tblInvDetail1");
            }
            else {
                $("#InvoiceDetailPopUp").html(data);
                cmn_apply_datatable("#tblInvDetail");
            }

        }
    })
}
//Getting Item info 
function OnClickIconBtn(e) {
    var ItmCode = $(e.target).closest('tr').find("#Hdn_item_id").val();

    ItemInfoBtnClick(ItmCode);

}
//Post data on Gst api 
function InitiateGstInvApi() {
    debugger;
    var count = CountNoOfInvEntriesToPost();
    var cnt = 0;
    //$('#divLoader').css('display', 'block');
    var dataType = $("#ddldataType").val();
    $('#datatable-buttons >tbody > tr').each(function () {
        var chkInv = $(this).find('#chkinv').is(':checked');
        var chkewb = $(this).find('#chkewb').is(':checked');
        var chk1 = $(this).find('#chk1').is(':checked');
        var url = "/ApplicationLayer/GSTAPI/SubmitOnGstApi";

        if (dataType == "SR") {
            chkInv = true;
        }
        if (chkewb == true) {
            url = "/ApplicationLayer/GSTAPI/PostEWayBill";
        }
        debugger;
        if (chkInv == true || chk1 == true || chkewb == true) {
            var invNo = $(this).find("#txtInv_no").text();
            var invDt = $(this).find("#txtInv_dt").text();
            var invType = $(this).find("#txtInvType").text();
            $.ajax({
                async: true,
                type: "POST",
                url: url,
                data: {
                    dataType: dataType,
                    invNo: invNo,
                    invDt: invDt,
                    invType: invType
                },
                success: function (data) {
                    debugger;
                    /*$('#divLoader').css('display', 'none');*/
                    cnt = cnt + 1;
                    if (cnt == count) {
                        $('#divLoader').css('display', 'none');
                        GetGstPostingData();
                    }
                    //alert(data);
                    if (data.indexOf("Success") !== -1) {
                        swal("", data, "success");
                    }
                    else {
                        swal("", data, "warning");
                    }
                    //let text = data;
                    //if (text.includes("Success") || text.includes("Duplicate"))
                    //    $(this).css('background-color', 'green');
                    //else
                    //    $(this).css('background-color', 'red');
                    /*  PostDataOnGstApi(invNo, invDt);*/
                    /*  alert(data);*/


                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                    cnt = cnt + 1;
                    if (cnt == count) {
                        $('#divLoader').css('display', 'none');
                    }
                }

            });
        }
    });

}
function InitiateSalesPurchaseReturnData() {
    $('#divLoader').css('display', 'block');
    var dataType = $("#ddldataType").val();
    var fromDate = $("#ddlMonthYear").val();
    var toDate = $("#ddlMonthYear").val();
    let GSTR_DateOption = "B";
    if ($("#GSTR_DateInvWise").is(":checked")) {
        GSTR_DateOption = "I"
    }

    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/GSTAPI/ReconsileDataOnEmgPortal",
        data: {
            dataType: dataType,
            fromDate: fromDate,
            toDate: toDate,
            GSTR_DateOption: GSTR_DateOption
        },
        success: function (data) {
            debugger;
            $('#divLoader').css('display', 'none');
           // GetGstPostingData();
            if (data == 'GST Reconsilation Successful...!!') {
                swal("", $("#DataUploadedSuccessfully").text(), "success");
            }
            else {
                swal("", data, "warning");
            }
        },
        error: function OnError(xhr, errorType, exception) {
            $('#divLoader').css('display', 'none');
        }
    });
}

function ReconsileData_CSV() { /* Added by Suraj Maurya on 15-10-2025 */
    debugger;
    
    var dataType = $("#ddldataType").val();
    var fromDate = $("#ddlMonthYear").val();
    var toDate = $("#ddlMonthYear").val();
    let GSTR_DateOption = "B";
    if ($("#GSTR_DateInvWise").is(":checked")) {
        GSTR_DateOption = "I"
    }
    let csvName = "GSTR_Preview"+ "_" + moment().format('YYYYMMDDhhmmss');
    if (dataType == "SR" || dataType == "PR") {
        try {
            $.ajax({
                async: true,
                type: "POST",
                url: "/ApplicationLayer/GSTAPI/ReconsileDataCsvPreview",
                data: {
                    dataType: dataType,
                    fromDate: fromDate,
                    toDate: toDate,
                    GSTR_DateOption: GSTR_DateOption
                },
                //contentType: 'application/json',
                xhrFields: {
                    responseType: 'blob' // important for file download
                },
                success: function (blob) {
                    // download file

                    let link = document.createElement('a');
                    let url = window.URL.createObjectURL(blob);
                    link.href = url;
                    link.download = csvName + '.csv';
                    document.body.appendChild(link);
                    link.click();
                    link.remove();
                    window.URL.revokeObjectURL(url);
                },
                error: function OnError(xhr, errorType, exception) {
                    console.log(exception);
                    hideLoader();
                }
            });

        }
        catch (ex) {
            console.log(ex);
            hideLoader();
        }
    }
   
}


function PostDataOnGstApi(invNo, invDt) {
    $.ajax({
        async: true,
        type: "POST",
        url: "/ApplicationLayer/GSTAPI/PostEWayBill",
        data: {
            invNo: invNo,
            invDt: invDt
        },
        success: function (data) {
            debugger;
            /* alert(data);*/
            if (data.indexOf("Success") !== -1 || data.indexOf("Duplicate") !== -1) {
                swal("", data, "Success");
            }
            else {
                swal("", data, "warning");
            }
        },
        error: function OnError(xhr, errorType, exception) {
            debugger;
        }
    });
}
//Set data type as required before getting report data from DB
function DataTypeRequired() {
    var dataType = $('#ddldataType').val();
    if (dataType == "0") {
        $('#ddldataType').css("border-color", "red");
        $('#rqDataType').css('display', "block");
        $('#rqDataType').text($('#valueReq').text());
        return false;
    }
    else {
        $('#ddldataType').css("border-color", "#ced4da");
        $('#rqDataType').css('display', "none");
        return true;
    }
}
// Minimum to date validation
function SetMinToDate() {
    var txtFromdate = $('#txtFromdate').val();
    $('#txtTodate').attr('min', txtFromdate);
}
//EWAY bill request
function EwbDetails() {
    debugger;
    try {
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
        var dataType = $("#ddldataType").val();
        var docStatus = $("#ddlDocStatus").val();
        ('.dttbl1')[0].id = "datatable-buttons";
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/GSTAPI/EWBSummaryPDF",
            data: {
                invNo: "",
                invDt: "",
                returnYear: "",
                returnMonth: "",
            },
            success: function (data) {
                debugger;
                $('#divGstApiPostingData').html(data);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("GET Gst Api Posting Details Error : " + err.message);
    }
}
//Eway bill summary
function EwbSummary() {
    debugger;
    try {
        //string invNo, string invDt, string returnYear, string returnMonth
        var fromDate = $("#txtFromdate").val();
        var toDate = $("#txtTodate").val();
        var dataType = $("#ddldataType").val();
        var docStatus = $("#ddlDocStatus").val();
        ('.dttbl1')[0].id = "datatable-buttons";
        $.ajax({
            async: true,
            type: "POST",
            url: "/ApplicationLayer/GSTAPI/EWBDetailsPDF",
            data: {
                invNo: "",
                invDt: "",
                returnYear: "",
                returnMonth: "",
            },
            success: function (data) {
                debugger;
                $('#divGstApiPostingData').html(data);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("GET Gst Api Posting Details Error : " + err.message);
    }
}
// Count no of entries
function CountNoOfInvEntriesToPost() {
    debugger;
    var count = 0;
    $('#datatable-buttons >tbody > tr').each(function () {
        if ($(this).find('#chkinv').prop('checked') || $(this).find('#chkewb').prop('checked') || $(this).find('#chk1').prop('checked')) {
            count = count + 1;
        }
    });
    return count;
}
// Getting GST api document PDF
function GetGstApiDocs(docName, e) {
    //var count = CountNoOfInvEntriesToPost();
    //var cnt = 0;
    $('#divLoader').css('display', 'block');

    var dataType = $("#ddldataType").val();
    var currentrow = $(e.target).closest("tr");
    var invNo = currentrow.find("#txtInv_no").text();
    var invDt = currentrow.find("#txtInv_dt").text();
    var invType = currentrow.find("#txtInvType").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/GSTAPI/GetGstApiDocsDetails",
        data: {
            invType: invType, invNo: invNo, invDt: invDt, docName: docName, dataType: dataType
        },
        success: function (data) {
            debugger;
            if (data == "Document not generated") {
                $('#divLoader').css('display', 'none');
                //alert(data);
                if (data.indexOf("Success") !== -1) {
                    swal("", data, "Success");
                }
                else {
                    swal("", data, "warning");
                }
            }
            else {
                $('#divLoader').css('display', 'none');
                window.open(data);
            }
        },
        error: function OnError(xhr, errorType, exception) {
            debugger;
            $('#divLoader').css('display', 'none');
        }
    })
}
// Getting gst api errors
function GetApiErrorLogs(e) {
    /*var dataType = $("#ddldataType").val();*/
    var currentrow = $(e.target).closest("tr");
    var invNo = currentrow.find("#txtInv_no").text();
    var cstInvNo = currentrow.find("#txtCstInvNo").text();
    //var invDt = currentrow.find("#txtInv_dt").text();
    //var invType = currentrow.find("#txtInvType").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/GSTAPI/GetGstApiErrorDetails",
        data: {
            invNo: invNo,
            cstInvNo: cstInvNo
        },
        success: function (data) {
            debugger;
            //Bind Data
            $('#divErrorDetail').html(data);
            $('#txtInvNumber').val(invNo);
            $('#txtInvDate').val($('#inv_dt_1').text());
        }
    })
}
//Refresh page
function RefreshBtnClick() {
    $('#divLoader').css('display', 'block');
    location.reload();
}
//check type of data and send on gst api 
function PostGstData() {
    var dataType = $("#ddldataType").val();
    if (dataType == "SR" || dataType == "PR") {
        InitiateSalesPurchaseReturnData();
    }
    else {
        InitiateGstInvApi();
    }
}
/*----------------- Export Invoice Details To PDF ------------------*/
function ExportItemsToExcel(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var invNo = currentrow.find("#txtInv_no").text();
    var invDt = currentrow.find("#txtInv_dt").text();
    var invType = currentrow.find("#txtInvType").text();
    if (invNo != null && invNo != "" && invNo != undefined) {
        window.location.href = "/ApplicationLayer/GSTAPI/ExportInvoiceToPdf?invType=" + invType + "&invNo=" + invNo + "&invDt=" + invDt;
    }
}
/*----------------- Export Invoice Details To PDF END------------------*/
/*--------------------E-invoice Preview Start---------------------------*/
function ShowEinvoicePreview(e) {
    debugger;
    var currentrow = $(e.target).closest("tr");
    var dataType = $("#ddldataType").val();
    var invNo = currentrow.find("#txtInv_no").text();
    var invDt = currentrow.find("#txtInv_dt").text();
    var invType = currentrow.find("#txtInvType").text();
    window.location.href = "/ApplicationLayer/GSTAPI/GenerateEInvoicePreview?invNo=" + invNo + "&invDate=" + invDt + "&invType=" + invType + "&dataType=" + dataType;
    //$.ajax({
    //    url: '/ApplicationLayer/GSTAPI/GenerateEInvoicePreview', // Update with your controller and action
    //    method: 'GET',
    //    data: {
    //        invNo: invNo,
    //        invDate: invDt,
    //        invType: invType,
    //        dataType: dataType,
    //    },
    //    success: function (data) {
    //        // Handle the file download
    //        var blob = new Blob([data]);
    //        var link = document.createElement('a');
    //        link.href = window.URL.createObjectURL(blob);
    //        link.download = 'InvPreview.pdf'; // Specify the desired file name
    //        document.body.appendChild(link);
    //        link.click();
    //        document.body.removeChild(link);
    //    },
    //    error: function (error) {
    //        console.error('Error downloading file:', error);
    //    }
    //});
}
/*--------------------E-invoice Preview End-----------------------------*/

function functionConfirm(event) {
    debugger;
    var dataType = $("#ddldataType").val();
    if (dataType == "SR" || dataType == "PR") {
        swal({
            title: $("#DoYouWantToUploadDataToAPIPortal").text(),
            //title: $("#DoYouWantToUploadDataToAPIPortal").text(),
            //text: $("#deltext").text() + "!",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Upload Data",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                PostGstData();
                return true;
            } else {
                return false;
            }
        });
        return false;
    }
    else {
        PostGstData();
    }
}

/*--------------------------On Change Data Type-----------------------------*/
function OnChangeDataType() {
    //Checking DataType Value; 
    if (DataTypeRequired() == false) {
        //return false;
    }

    //Hide And Show Filters
    HideShowDiv();

    //Getting GST Posting Data
    GetGstPostingData();
}
/*--------------------------On Change Data Type End-----------------------------*/