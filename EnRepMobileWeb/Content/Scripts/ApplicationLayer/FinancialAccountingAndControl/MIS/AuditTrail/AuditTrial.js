$(document).ready(function () {
    debugger;
    //$("a.btn.btn-default.buttons-print.btn-sm").remove();
    //$("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").hide();
    //$("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
    $("#ddl_GLAccountList").select2({
        ajax: {
            url: $("#GLListName").val(),
            data: function (params) {
                var queryParameters = {
                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
                    ddlGroup: params.term, // search term like "a" then "an"
                    Group: params.page,
                    GLGroupId: $("#acc_group").val(), // pass your own parameter       


                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
    $("#ddl_CreatedBy").select2();
    $("#ddl_ApprovedBy").select2();
    $("#acc_group").select2({
        ajax: {
            url: $("#GLAccGrp").val(),
            data: function (params) {
                var queryParameters = {
                    ddlGroup: params.term,
                    Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                };
            },
        },
    });
    
});

function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdateAT").val();
    var ToDate = $("#txtTodateAT").val();

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
            $("#txtTodateAT").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdateAT").val(today);

            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    
}
function OnChangeGLGroup() {
    debugger;
    var GLGroup_Id = $("#acc_group").val();
    $("#hdn_GLGroupId").val(GLGroup_Id);
}
function OnChangeGLAccount() {
    debugger;
    var GLAcc_id = $("#ddl_GLAccountList").val();
    if (GLAcc_id != null && GLAcc_id != null && GLAcc_id != "0") {
        $("#hdn_GLAccountID").val(GLAcc_id);
    }
}
function OnchangeVouType() {
    debugger;
    var vouTyp = $("#ddl_Voutype").val();
    if (vouTyp != null && vouTyp != null && vouTyp != "0") {
        $("#hdn_VouType").val(vouTyp);
    }
}
function OnChangeVouAmtFrom() {
    debugger;

    var ValDecDigit = $("#ValDigit").text();
    var VouAmt_From = $("#txt_VouAmountFrom").val();
    var VouAmtFrom = cmn_ReplaceCommas(VouAmt_From);
    if (AvoidDot(VouAmtFrom) == false) {
        VouAmtFrom = 0;
    }
    if (VouAmtFrom == '0.00') {
        $("#txt_VouAmountFrom").val("");
    }
    else {
        $("#txt_VouAmountFrom").val(parseFloat(VouAmtFrom).toFixed(ValDecDigit));
    }

}
function OnChangeVouAmtTo() {
    debugger;

    var ValDecDigit = $("#ValDigit").text();
    var VouAmt_To = $("#txt_VouAmountTo").val();
    var VouAmtTo = cmn_ReplaceCommas(VouAmt_To);
    if (AvoidDot(VouAmtTo) == false) {
        VouAmtTo = 0;
    }
    if (VouAmtTo == '0.00') {
        $("#txt_VouAmountTo").val("");
    }
    else {
        $("#txt_VouAmountTo").val(parseFloat(VouAmtTo).toFixed(ValDecDigit));
    }
}
function OnchangeNarr() {
    debugger;
    var Narr = $("#txt_Narration").val().trim();
    if (Narr != "") {
        $("#hdn_Narr").val(Narr);
    }

}
function SearchAllVouchersDetails() {
    debugger;
    try {
        var FromDate = $("#txtFromdateAT").val();
        var ToDate = $("#txtTodateAT").val();
        var GroupId = $('#acc_group').val();
        $('#hdn_GLGroupId').val(GroupId);
        var AccountID = $('#ddl_GLAccountList').val();
        $('#hdn_GLAccountID').val(AccountID);
        var AmtFrom = $('#txt_VouAmountFrom').val();
        var AmtTo = $('#txt_VouAmountTo').val();
        if (AmtFrom == "") {
            AmtFrom = "0";
        }
        if (AmtTo == "") {
            AmtTo = "0";
        }
       
        var VouTyp = $('#ddl_Voutype').val();
        $('#hdn_VouType').val(VouTyp);
        var CreatBy = $('#ddl_CreatedBy').val();
        var CreatOn = $('#txt_CreatedOn').val();
        var AppBy = $('#ddl_ApprovedBy').val();
        var AppOn = $('#txt_ApprovedOn').val();
        var Narr = $("#txt_Narration").val().trim();
        var Status = $("#ddlStatus").val();

        var FromDate1 = $("#txtFromdateAT").val().split("-").reverse().join("-");
        $("#hdn_From_VouDate").val(FromDate1);
        var ToDate1 = $("#txtTodateAT").val().split("-").reverse().join("-");
        $("#hdn_To_VouDate").val(ToDate1);
        var CreatOn1 = $("#txt_CreatedOn").val().split("-").reverse().join("-");
        $("#hdn_CreateDate").val(CreatOn1);
        var AppOn1 = $("#txt_ApprovedOn").val().split("-").reverse().join("-");
        $("#hdn_ApproveDate").val(AppOn1);
        var GroupName = $("#acc_group option:selected").text();
        $("#hdn_ddlGroupName").val(GroupName);
        var AccountName = $("#ddl_GLAccountList option:selected").text();
        $("#hdn_GLAccountName").val(AccountName);
        var VouTypName = $("#ddl_Voutype option:selected").text();
        $("#hdn_VouTypeName").val(VouTypName);
        var StatusName = $("#ddlStatus option:selected").text();
        $("#hdn_StatusName").val(StatusName);
        var CreatorName = $("#ddl_CreatedBy option:selected").text();
        $("#hdn_CreatorName").val(CreatorName);
        var ApproverName = $("#ddl_ApprovedBy option:selected").text();
        $("#hdn_ApproverName").val(ApproverName);

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/AuditTrail/SearchAuditTrailDetails",
            data: {
                FromDate: FromDate, ToDate: ToDate, GroupId: GroupId, AccountID: AccountID, AmtFrom: AmtFrom, AmtTo: AmtTo,
                VouTyp: VouTyp, CreatBy: CreatBy, CreatOn: CreatOn, AppBy: AppBy, AppOn: AppOn, Narr: Narr, Status: Status
            },
            success: function (data) {
                debugger;
                $("#MISAuditTrailDetails").html(data);
                $("a.btn.btn-default.buttons-print.btn-sm").remove();
                $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
                //$(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm" onclick="AuditTrailPrint()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>Print</span></button>')
                 hideLoader();/*Add by Hina sharma on 19-11-2024 */
            }
        });
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }

}
function RemoveSession() {
    sessionStorage.removeItem("FromDateToDate");
}
function AuditTrailPrint() {
    debugger;
    var arr = [];
    var FromDate = $("#txtFromdateAT").val();
    var ToDate = $("#txtTodateAT").val();
    var GroupId = $("#acc_group option:selected").val();
    var AccountID = $("#ddl_GLAccountList option:selected").val();
    var VouTyp = $("#ddl_Voutype option:selected").val();
    var AmtFrom = $('#txt_VouAmountFrom').val();
    var AmtTo = $('#txt_VouAmountTo').val();
    if (AmtFrom == "") {
        AmtFrom = "0";
    }
    if (AmtTo == "") {
        AmtTo = "0";
    }
    var CreatBy = $('#ddl_CreatedBy').val();
    var CreatOn = $('#txt_CreatedOn').val();
    var AppBy = $('#ddl_ApprovedBy').val();

    var AppOn = $('#txt_ApprovedOn').val();
    var Narr = $("#txt_Narration").val().trim();
    var Status = $("#ddlStatus").val();

    arr.push({
        FromDate: FromDate, ToDate: ToDate, GroupId: GroupId, AccountID: AccountID, AmtFrom: AmtFrom, AmtTo: AmtTo,
        VouTyp: VouTyp, CreatBy: CreatBy, CreatOn: CreatOn, AppBy: AppBy, AppOn: AppOn, Narr: Narr, Status: Status
    });
    var len = $("#datatable-buttons5 > tbody> tr").length;
    var array = JSON.stringify(arr);
    $("#ATPrintData").val(array);
    $("#hdnATCSVPrint").val(null);
    $("#hdnATPDFPrint").val("Print");
    $('form').submit();

}
function AuditTrailCSV() {
    debugger;
    var arr = [];
    var FromDate = $("#txtFromdateAT").val();
    var ToDate = $("#txtTodateAT").val();
    var GroupId = $("#acc_group option:selected").val();
    var AccountID = $("#ddl_GLAccountList option:selected").val();
    var VouTyp = $("#ddl_Voutype option:selected").val();
    var AmtFrom = $('#txt_VouAmountFrom').val();
    var AmtTo = $('#txt_VouAmountTo').val();
    if (AmtFrom == "") {
        AmtFrom = "0";
    }
    if (AmtTo == "") {
        AmtTo = "0";
    }
    var CreatBy = $('#ddl_CreatedBy').val();
    var CreatOn = $('#txt_CreatedOn').val();
    var AppBy = $('#ddl_ApprovedBy').val();

    var AppOn = $('#txt_ApprovedOn').val();
    var Narr = $("#txt_Narration").val().trim();
    var Status = $("#ddlStatus").val();

    arr.push({
        FromDate: FromDate, ToDate: ToDate, GroupId: GroupId, AccountID: AccountID, AmtFrom: AmtFrom, AmtTo: AmtTo,
        VouTyp: VouTyp, CreatBy: CreatBy, CreatOn: CreatOn, AppBy: AppBy, AppOn: AppOn, Narr: Narr, Status: Status
    });
    //var len = $("#datatable-buttons5 > tbody> tr").length;
    var array = JSON.stringify(arr);
    $("#ATPrintData").val(array);

    var filters = FromDate + "," + ToDate + "," + GroupId + "," + AccountID + "," + AmtFrom + "," + AmtTo + "," + VouTyp + "," + CreatBy + "," + CreatOn + "," + AppBy + "," + AppOn + "," + Narr + "," + Status;
    $("#Allfilters").val(filters);
    $("#hdnATPDFPrint").val(null);
    $("#hdnATCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#ATsearchValue").val(searchValue);

    $('form').submit();
    //var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    //window.location.href = "/ApplicationLayer/TrialBalance/ExportTrialBalanceData?searchValue=" + searchValue + "&filters=" + filters;

}