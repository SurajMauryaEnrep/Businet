/************************************************
Javascript Name:Local Sale Quotation List
Created By:Mukesh
Created Date: 29-06-2021
Description: This Javascript use for the Local Sale Quotation List many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {
    debugger;
    $("#SE_ddlSlsPerson").select2();
    $("#tbodySlsEnqryList #datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var DocumentMenuId = $("#DocumentMenuId").val();
            //var WF_status = $("#WF_status").val();
            var EnqTyp = $("#EnqryTyp_List").val();
            var CustTyp = $("#CustTyp_List").val();
            var clickedrow = $(e.target).closest("tr");
            var SENo = clickedrow.children("#ptbl_SENumber").text();
            
            if (SENo != null && SENo != "") {
                window.location.href = "/ApplicationLayer/SalesEnquiry/EditUpdateSEDetail/?SENo=" + SENo +
                    "&ListFilterData=" + ListFilterData + "&DocumentMenuId=" + DocumentMenuId +
                    "&EnqTyp=" + EnqTyp + "&CustTyp=" + CustTyp;

            }
        }
        catch (err) {
            debugger
        }
    });
    $("#datatable-buttons >tbody").bind("click", function (e) {
        //  debugger;
        var clickedrow = $(e.target).closest("tr");
        var SE_No = clickedrow.children("#ptbl_SENumber").text();
        //var Q_Date = clickedrow.children("#QuotationDate").text();
        //var date = Q_Date.split("-");
        //var FDate = date[2] + '-' + date[1] + '-' + date[0];
        var Doc_id = $("#DocumentMenuId").val();
        $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
        $("#hdDoc_No").val(SE_No);
        var Doc_Status = clickedrow.children("#Doc_Status").text();
       // Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        //GetWorkFlowDetails(QT_No, FDate, Doc_id, Doc_Status);


    });
    var CustPros_type = $("#SE_ddlCusttype").val();
    var Enquiry_type = $("#SE_ddlEnqtype").val();
    if (Enquiry_type != "0" && CustPros_type != "0") {
        BindCustomerList();
    }
    var SE_No = $("#txt_EnquiryNo").val();
    $("#hdDoc_No").val(SE_No);
});


function BindCustomerList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    //var ErrorFlag = "N";
    var CustPros_type = $("#SE_ddlCusttype").val();
    var Enquiry_type = $("#SE_ddlEnqtype").val();
    //if (ValiationEnqtypCustTyp() == false) {
    //    return false;
    //}
    //else {

        if (Enquiry_type != "0" && CustPros_type != "0") {
            $("#SE_ddlCustName").select2({

                ajax: {
                    url: $("#hdnCustNameList_List").val(),
                    data: function (params) {
                        debugger;
                        var queryParameters = {
                            CustName: params.term, // search term like "a" then "an"
                            //CustPage: params.page,
                            //BrchID: Branch,
                            CustPros_type: CustPros_type,
                            Enquiry_type: Enquiry_type
                        };
                        return queryParameters;
                    },
                    dataType: "json",
                    cache: true,
                    delay: 250,
                    //type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    processResults: function (data, params) {
                        if (data == 'ErrorPage') {
                            LSO_ErrorPage();
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

        /*}*/
    }
       

    }
    


function OnChangeEnquiryType() {
    
    debugger;
    var Enquiry_type = $("#SE_ddlEnqtype").val();
   /* if (Enquiry_type != "0") {*/
        document.getElementById("vmEnquiryTypeonList").innerHTML = null;
        $("#SE_ddlEnqtype").css("border-color", "#ced4da");
    //}
    //else {
    //    document.getElementById("vmEnquiryTypeonList").innerHTML = $("#valueReq").text();
    //    $("#SE_ddlEnqtype").css("border-color", "red");
    //    $("#SE_ddlEnqtype").val("0");
    //    $('#SE_ddlCustName').empty().append('<option value="0" selected="selected">---Select---</option>');
    //    ErrorFlag = "Y";
    //}
    //document.getElementById("vmEnquiryTypeonList").innerHTML = null;
    //$("#SE_ddlEnqtype").css("border-color", "#ced4da");
}
function OnChangeCustTypelist() {
    
    debugger;
    //if (ValiationEnqtypCustTyp() == false) {
    //    return false;
    //}
    //else {
        var CustPros_type = $("#SE_ddlCusttype").val();
        var Enquiry_type = $("#SE_ddlEnqtype").val();
        if (Enquiry_type != "0" && CustPros_type != "0") {
            document.getElementById("vmCusttyponList").innerHTML = null;
            $("#SE_ddlCusttype").css("border-color", "#ced4da");
            BindCustomerList();
       /* }*/
        
    }
}
function BtnSearch() {
    debugger;
    //if (ValiationEnqtypCustTyp() == false) {
    //    return false;
    //}
    //else {
        FilterSlsEnquiryList();
    /*}*/
    //ResetWF_Level();

}

function ValiationEnqtypCustTyp() {
    debugger;
    var ErrorFlag = "N";
    var CustPros_type = $("#SE_ddlCusttype").val();
    var Enquiry_type = $("#SE_ddlEnqtype").val();

    if (Enquiry_type != "0") {
        document.getElementById("vmEnquiryTypeonList").innerHTML = null;
        $("#SE_ddlEnqtype").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmEnquiryTypeonList").innerHTML = $("#valueReq").text();
        $("#SE_ddlEnqtype").css("border-color", "red");
        $("#SE_ddlEnqtype").val("0");
        $('#SE_ddlCustName').empty().append('<option value="0" selected="selected">---Select---</option>');
        ErrorFlag = "Y";
    }
    if (CustPros_type != "0") {
        document.getElementById("vmCusttyponList").innerHTML = null;
        $("#SE_ddlCusttype").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmCusttyponList").innerHTML = $("#valueReq").text();
        $("#SE_ddlCusttype").css("border-color", "red");
        $("#SE_ddlCusttype").val("0");
        $('#SE_ddlCustName').empty().append('<option value="0" selected="selected">---Select---</option>');
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

//function DateFormate_yyyyMMdd(Date) {
//    var arr = Date.split('-');
//    var ReturnDate = Date;
//    if (arr[2].length > 2) {
//        ReturnDate = arr[2] + '-' + arr[1] + '-' + arr[0];
//    }
//    return ReturnDate;
//}
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
function FilterSlsEnquiryList() {
    debugger;
    try {
        var CustId = $("#SE_ddlCustName option:selected").val();
        //var OrderType = $("#ddlOrderType").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        var CustType = $("#SE_ddlCusttype").val();
        var EnqType = $("#SE_ddlEnqtype").val();
        var EnqSrc = $("#SE_ddlEnqSrc").val();
        var Catgry = $("#SE_ddlCatgry").val();
        var PrtFlio = $("#SE_ddlPort").val();
        var Regn = $("#SE_ddlRegion").val();
        var SlsPrsn = $("#SE_ddlSlsPerson option:selected").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/SalesEnquiry/SearchSlsEnqryDetail",
            data: {
                
                EnqType: EnqType,
                CustType: CustType,
                EnqSrc: EnqSrc,
                CustId: CustId,
                Catgry: Catgry,
                PrtFlio: PrtFlio,
                Regn: Regn,
                SlsPrsn: SlsPrsn,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
            },
            success: function (data) {
                debugger;
                $('#tbodySlsEnqryList').html(data);
                $('#ListFilterData').val(EnqType + ',' + CustType + ',' + EnqSrc + ',' + CustId + ',' + Catgry + ',' + PrtFlio + ',' + Regn + ',' + SlsPrsn + ',' + Fromdate + ',' + Todate + ',' + Status );
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
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }

}


