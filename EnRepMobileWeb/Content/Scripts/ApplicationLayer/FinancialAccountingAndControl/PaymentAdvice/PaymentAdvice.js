$(document).ready(function () {
    debugger;

    var CurrentDate = moment().format('YYYY-MM-DD');
    if ($("#txt_AdviceNumber").val() == "" || $("#txt_AdviceNumber").val() == null) {
        $("#txt_AdviceDate").val(CurrentDate);
    }
    BindBankAccList();
    var len = $("#ItemDetailTbl TBODY TR").length;
    if (len > 0) {
        $("#ItemDetailTbl TBODY TR").each(function () {
            debugger;
            var currentRow = $(this);
            var AccountList = {};
            AccountList.chkpadv = currentRow.find("#hdn_AdvCheck").val();
            if (AccountList.chkpadv == "Y") {
                currentRow.find("#AdvCheck").prop("checked", true);
            }
            else {
                currentRow.find("#AdvCheck").prop("checked", false);
            }

        });
    }
    InvAdjNo = $("#txt_AdviceNumber").val();
    $("#hdDoc_No").val(InvAdjNo);
    //$("#BtnInvoiceDetail").prop("disabled", true);
    
    
    
    $("#datatable-buttons tbody tr").bind("dblclick", function (e) {
        try {
            debugger;
            var ListFilterData = $("#ListFilterData").val();
            var WF_Status = $("#WF_Status").val();
            var clickedrow = $(e.target).closest("tr");
            var PAdvNo = clickedrow.children("#PAdvNumber_lst").text().trim();
            var PAdvdt = clickedrow.children("#hdPAdvDate_lst").text().trim();
            if (PAdvNo != null && PAdvNo != "") {
                window.location.href = "/ApplicationLayer/PaymentAdvice/EditPA_ByList/?PAdvNo=" + PAdvNo + "&PAdvdt=" + PAdvdt + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });
    //$("#datatable-buttons >tbody").bind("click", function (e) {
    //    debugger;
    //    var clickedrow = $(e.target).closest("tr");
    //    var PAdvNo = clickedrow.children("#PAdvNumber_lst").text();
    //    var PAdvdt = clickedrow.children("#hdPAdvDate_lst").text();
    //    var Doc_id = $("#DocumentMenuId").val();
    //    $("#hdDoc_No").val(PAdvNo);
    //    var Doc_Status = clickedrow.children("#PAdv_Status_lst").text().trim();
    //    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    //    GetWorkFlowDetails(PAdvNo, PAdvdt, Doc_id, Doc_Status);

    //});
    //var PANumber = $("#txt_AdviceNumber").val();
    //var PADate = $("#txt_AdviceDate").val();
    ///* var hdStatus = $('#hdnPAStatus').val().trim();*/
    //var hdStatus = $('#hdnPAStatus').val();
    //GetWorkFlowDetails(PANumber, PADate, "105104118", hdStatus);
    //$("#hdDoc_No").val(PANumber);
})

/*---------------------------List Page---------------------*/
function BtnSearch() {
    debugger;
    try {
        
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val().trim();

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/PaymentAdvice/SearchPayAdvListDetail",
            data: {
                
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#divTblPayAdvList').html(data);
                $('#ListFilterData').val(Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MTO Error : " + err.message);

    }
    ResetWF_Level();
}

function FromToDateValidation() {
    debugger;
    var FromDate = $("#txtFromdate").val();
    var ToDate = $("#txtTodate").val();
    //$("#hdFromdate").val(FromDate)
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
            debugger;
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
function CmnGetWorkFlowDetails(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var PAdv_No = clickedrow.children("#PAdvNumber_lst").text();
    var PAdv_Date = clickedrow.children("#hdPAdvDate_lst").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(PAdv_No);
    var Doc_Status = clickedrow.children("#PAdv_Status_lst").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(PAdv_No, PAdv_Date, Doc_id, Doc_Status);
    //var a = 1;
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

function OtherFunctions(StatusC, StatusName) {
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#txt_AdviceNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

function ForwardBtnClick() {
    debugger
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var PAdvDate = $("#txt_AdviceDate").val();
        $.ajax({
            type: "POST",
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: PAdvDate
            },
            success: function (data) {
                if (data == "TransAllow") {
                    var PAStatus = "";
                    PAStatus = $('#hdnPAStatus').val().trim();
                    if (PAStatus === "D" || PAStatus === "F") {

                        if ($("#hd_nextlevel").val() === "0") {
                            $("#Btn_Forward").attr("data-target", "");
                        }
                        else {
                            $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                            $("#Btn_Approve").attr("data-target", "");
                        }
                        var Doc_ID = $("#DocumentMenuId").val();
                        $("#OKBtn_FW").attr("data-dismiss", "modal");
                        Cmn_GetForwarderList(Doc_ID);
                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "");
                        $("#Btn_Forward").attr('onclick', '');
                        $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
                    }
                }
                else {
                    swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
                    $("#Btn_Approve").attr("data-target", "");
                    $("#Forward_Pop").attr("data-target", "");

                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    

    Remarks = $("#fw_remarks").val();
    DocNo = $("#txt_AdviceNumber").val();
    DocDate = $("#txt_AdviceDate").val();
    docid = $("#DocumentMenuId").val();
    var ListFilterData1 = $('#ListFilterData1').val();
    var WF_status1 = $("#WF_status1").val();
    
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        debugger
        var currentRow = $(this);
       
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });

    //$("#forwardDetailsTbl >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    //debugger;
    //    var Userid = currentRow.find("#user").text();
    //    if ($("#r_" + Userid).is(":checked")) {
    //        forwardedto = currentRow.find("#user").text();
    //        level = currentRow.find("#level").text();
    //    }
    //});
    
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks);
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Approve") {

        var list = [{
            DocNo: DocNo, DocDate: DocDate
            , A_Status: "Approve", A_Level: $("#hd_currlevel").val()
            , A_Remarks: Remarks
            
        }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/PaymentAdvice/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 ;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            showLoader();
            window.location.reload();
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            showLoader();
            window.location.reload();
        }
    }
}

/*---------------------------List Page---------------------*/
var ValDecDigit = $("#ValDigit").text();
function VoucherDateValidation() {
    debugger;
   Cmn_VoucherDateValidation("#txtTodate_Dtl");
}
function InstrumentDateValidation(e) {
    debugger
    var Crow = $(e.target).closest("tr");
    var VouDate = Crow.find("#tbl_InsDt").val();
   
   if ((VouDate != null || VouDate == "")) {
        var date = VouDate.split('-');
        var dt1 = date[0];
        var dt2 = parseInt(dt1)
        var len = dt2.toString().length;
        if (len == 4) {
            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            //$("#txtTodate").val(today);
            if (VouDate > today) {

                //var fromDate = new Date($("#hdFromdate").val());

                //var month = (fromDate.getMonth() + 1);
                //var day = fromDate.getDate();
                //if (month < 10)
                //    month = "0" + month;
                //if (day < 10)
                //    day = "0" + day;
                //var today = fromDate.getFullYear() + '-' + month + '-' + day;
                $(VouDt).val(today);
                swal("", $("#VoucherDateCannotGrthenCurrDate").text(), "warning");
            }
            if (VouDate < today) {
                /*start Add by Hina on 24-03-2025 to chk Financial year exist or not*/
                var compId = $("#CompID").text();
                var brId = $("#BrId").text();
                //var Voudt = $("#txtJVDate").val();
                $.ajax({
                    type: "POST",
                    url: "/Common/Common/Fin_CheckFinancialYear",
                    data: {
                        compId: compId,
                        brId: brId,
                        Voudt: VouDate
                    },
                    success: function (data) {
                        if (data == "FY Exist") { /*End to chk Financial year exist or not*/

                        }
                        else {/* to chk Financial year exist or not*/
                            if (data == "FY Not Exist") {
                                //var today = new Date();
                                swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                                Crow.find("#tbl_InsDt").val(today);
                                
                            }
                            else {
                                swal("", $("#BooksAreClosedEntryCanNotBeMadeInThisFinancialYear").text(), "warning");
                                Crow.find("#tbl_InsDt").val(today);
                                
                            }
                        }
                    }
                });
                /*End to chk Financial year exist or not*/
                //return false;
            }
        }
    }
}

//function FromToDateValidation_Dtl() {
//    debugger;
//    var FromDate = $("#txtFromdate_Dtl").val();
//    var ToDate = $("#txtTodate_Dtl").val();
//    //$("#hdFromdate").val(FromDate)
//    if ((FromDate != null || FromDate == "") && (ToDate != null || ToDate == "")) {
//        if (FromDate > ToDate) {

//            var now = new Date();
//            var month = (now.getMonth() + 1);
//            var day = now.getDate();
//            if (month < 10)
//                month = "0" + month;
//            if (day < 10)
//                day = "0" + day;
//            var today = now.getFullYear() + '-' + month + '-' + day;
//            $("#txtTodate_Dtl").val(today);
//            $("#hdn_ToDateDtl").val(today);
            
//            debugger;
//            var fromDate = new Date($("#hdn_FromdateDtl").val());

//            var month = (fromDate.getMonth() + 1);
//            var day = fromDate.getDate();
//            if (month < 10)
//                month = "0" + month;
//            if (day < 10)
//                day = "0" + day;
//            var today = fromDate.getFullYear() + '-' + month + '-' + day;
//            $("#txtFromdate_Dtl").val(today);
//            swal("", $("#fromtodatemsg").text(), "warning");
//        }
//    }

//}
function OnchangeInstrumentType() {
    debugger;
    var InstrumentType = $("#ddl_InstrumentType").val();
    $("#hdInstrumentType").val(InstrumentType);
}
function BindBankAccList() {

    $("#ddlBankAccName").select2({
        ajax: {
            url: $("#hdBankList").val(),
            data: function (params) {
                var queryParameters = {
                    BankName: params.term // search term like "a" then "an"
                    //Group: params.page
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                //params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}
function OnChangeBankCoa() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var ExchDecDigit = $("#ExchDigit").text();

    var BankAccId = $("#ddlBankAccName").val();
    $("#hdnBankName").val(BankAccId);
    if (BankAccId == "" || BankAccId == "0" || BankAccId == null) {
        $("[aria-labelledby='select2-ddlBankAccName-container']").css("border-color", "red");
        $("#vmBankAccName").text($("#valueReq").text());
        $("#vmBankAccName").css("display", "block");
        $("#ddlBankAccName").css("border-color", "red");
    }
    else {
        $("#vmBankAccName").css("display", "none");
        //$("#ddlBankAccName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlBankAccName-container']").css("border-color", "#ced4da");
    }
}
function BankValidation() {
    debugger;
    var BankAccId = $("#ddlBankAccName").val();
    var Flag = 'N';
    if (BankAccId == "" || BankAccId == "0" || BankAccId == null) {
        $("[aria-labelledby='select2-ddlBankAccName-container']").css("border-color", "red");
        $("#vmBankAccName").text($("#valueReq").text());
        $("#vmBankAccName").css("display", "block");
        $("#ddlBankAccName").css("border-color", "red");
        Flag = 'Y';
    }
    else {
        $("#vmBankAccName").css("display", "none");
        //$("#ddlBankAccName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlBankAccName-container']").css("border-color", "#ced4da");
    }
    if (Flag == 'Y') {
        return false;
    }
    else {
        return true;
    }
}
function SearchPayAdvDetails() {
    debugger;
    try {
        if (BankValidation() == false) {
            return false;
        }
        //var ValDigit = $("#ValDigit").text();
        var Acc_Id = $("#ddlBankAccName").val();
        var FromDate = $("#txtFromdate_Dtl").val();
        var ToDate = $("#txtTodate_Dtl").val();
        var InsType = $("#ddl_InstrumentType option:selected").val();
        

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/PaymentAdvice/SearchPAItemDetail",
            data: {
               
                FromDate: FromDate,
                ToDate: ToDate,
                InsType: InsType,
                BankAcc_id: Acc_Id
            },
            success: function (data) {
                debugger;
                $('#DivPaymentAdviceItemDetail').html(data);

               
                var t_length = $("#ItemDetailTbl tbody tr").length;
                 if (t_length == 0) {
                     $("#BtnSearch").css("display", "block");

                     $("#ddlBankAccName").attr("disabled", false);
                     $("#txtFromdate_Dtl").attr("disabled", false);
                     $("#txtTodate_Dtl").attr("disabled", false);
                     $("#ddl_InstrumentType").attr("disabled", false);

                    //$("#EntityType").prop("disabled", false);
                    //$("#entityid").prop("disabled", false);
                    return false;
                }
                else {
                    $("#BtnSearch").css("display", "none");
                     $("#ddlBankAccName").attr("disabled", true);
                     $("#txtFromdate_Dtl").attr("disabled", true);
                     $("#txtTodate_Dtl").attr("disabled", true);
                     $("#ddl_InstrumentType").attr("disabled", true);
                }
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("Grn Error : " + err.message);

    }
}
function OnClickAllAdvCheck() {
    debugger;
    var Payamount = 0;
    var PendingAmt = 0;
    var Allcheck = "";
    if ($("#AllAdvCheck").is(":checked")) {
        Allcheck = 'Y';
    }
    else {
        Allcheck = 'N';
    }

    if (Allcheck == 'Y') {
        $("#ItemDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            //RemAmt = cmn_ReplaceCommas(CurrentRow.find("#tbl_Amt").val());
            //PendingAmt = cmn_ReplaceCommas(CurrentRow.find("#HdAdvUnAdjAmt").val());
            CurrentRow.find("#AdvCheck").prop("checked", true);
            CurrentRow.find("#AdvCheck").is(":checked")
            check = 'Y';
            if (check == 'Y') {
                CurrentRow.find("#hdn_AdvCheck").val('Y');
            }
        });
    }
    else {
        $("#ItemDetailTbl tbody tr").each(function () {
            
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#AdvCheck").prop("checked", false);
            
            if (CurrentRow.find("#AdvCheck").is(":checked")) {
                check = 'Y';
            }
            else {
                check = 'N';
            }
            if (check == 'N') {
                CurrentRow.find("#AdvCheck").prop("checked", false);
                CurrentRow.find("#hdn_AdvCheck").val('N');
            }
        });
    }
   
}
function OnClickAdvCheck(e) {
    debugger;
    var check = "";
    var CurrentRow = $(e.target).closest("tr");
    
    if (CurrentRow.find("#AdvCheck").is(":checked")) {
        check = 'Y';
    }
    else {
        check = 'N';
    }
    if (check == 'Y') {
        CurrentRow.find("#AdvCheck").prop("checked", true);
        CurrentRow.find("#hdn_AdvCheck").val('Y');
    }
    else {
        CurrentRow.find("#AdvCheck").prop("checked", false);
        CurrentRow.find("#hdn_AdvCheck").val('N');
    }
    
}
function PA_FilterItemDetail(e) {
    debugger
    Cmn_FilterTableData(e, "ItemDetailTbl", [{ "FieldId": "tbl_VouNo", "FieldType": "input", "SrNo": "SNohiddenfiled" }]);
}

function InsertPayAdvDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    
    
    //if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
    //    $("#btn_save").attr("disabled", true);
    //    $("#btn_save").css("filter", "grayscale(100%)");
    //    return false;
    //}


    if (BankValidation() == false) {
        return false;
    }
    if (ItemValidation() == false) {/*Add by Hina Sharma on 05-08-2025*/

        return false;
    }
    if (CheckVouValidate() == false) {/*Add by Hina Sharma on 05-08-2025*/

        return false;
    }
    var FinalPayAdvItemDetail = [];
    FinalPayAdvItemDetail = InsertPayAdvItemDetails();
    var ItemDt = JSON.stringify(FinalPayAdvItemDetail);
    $('#hdn_PayAdvItemDetail').val(ItemDt);

    var Instyp = $('#ddl_InstrumentType').val();
    if (Instyp == "0" || Instyp == "" || Instyp == null) {
        $('#hdInstrumentType').val("0");
    }
    
    debugger;
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
};

function InsertPayAdvItemDetails() {
    debugger;
    var ItemDetail = new Array();
    //var entity_name = $("#entityid option:selected").val();
    //var entity_type = $("#EntityType").val();
    $("#ItemDetailTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var AccountList = {};
        AccountList.chkpadv = currentRow.find("#hdn_AdvCheck").val();
        if (AccountList.chkpadv == '') {
            AccountList.chkpadv="N"
        }
       /* AccountList.vou_no = currentRow.find("#tbl_VouNo").val();*/
        AccountList.vou_no = currentRow.find("#tbl_VouNo").val();
        AccountList.vou_dt = currentRow.find("#tbl_VouDt").val();
        AccountList.bankacc_id = currentRow.find("#tbl_AccId").val();
        AccountList.glacc_id = currentRow.find("#tbl_GlAccId").val();
        AccountList.amt = currentRow.find("#tbl_Amt").val();
        AccountList.instype = currentRow.find("#tbl_InsTypeId").val();
        AccountList.ins_no = currentRow.find("#tbl_InsNo").val();
        AccountList.ins_dt = currentRow.find("#tbl_InsDt").val();
       
        ItemDetail.push(AccountList);
    });
    return ItemDetail;
};
$("#ItemDetailTbl TBODY TR").each(function () {
    debugger;
    var currentRow = $(this);
    var AccountList = {};
    AccountList.chkpadv = currentRow.find("#hdn_AdvCheck").val();
    if (AccountList.chkpadv == "Y") {
        currentRow.find("#AdvCheck").prop("checked", true);
    }
    else {
        currentRow.find("#AdvCheck").prop("checked", false);
    }

});
function ItemValidation() {
    debugger
    var flag = 'N';
    var len = $("#ItemDetailTbl >tbody >tr").length;
    if (len == "0") {
        flag = 'N';
    }
    else {
        flag = 'Y';
    }
    
    if (flag == 'N') {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return false; 
    }
    else {
        return true; 
    }
}
function CheckVouValidate() {
    debugger
    var flag = 'N';
    $("#ItemDetailTbl >tbody >tr").each(function (i, row) {
        debugger
        var currentRow = $(this);
        var rowId = "";
       
        var rowIdQ = $(this).find("#SNohiddenfiled").text().trim();

        if (currentRow.find("#AdvCheck").prop("checked") == true) {
            flag = 'Y';
        }
    });
    if (flag == 'N') {
        swal("", $("#AtleastOneItemPropertymustbeselected").text(), "warning");
        return false; 
    }
    else {
        return true; 
    }
}