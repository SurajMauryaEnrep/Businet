$(document).ready(function () {
    debugger;
   
    var GLGroup_Id = $("#acc_group").val();
   /* if (GLGroup_Id == "0") {*/
        debugger;
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
    /*}*/
  
   
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
    var FromDate = $("#txtFromdateJB").val();
    var ToDate = $("#txtTodateJB").val();

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
            $("#txtTodateJB").val(today);

            var fromDate = new Date($("#hdFromdate").val());

            var month = (fromDate.getMonth() + 1);
            var day = fromDate.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdateJB").val(today);

            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    //if (ToDate != "") {
    //    validatefydate(FromDate, ToDate);
    //}
}
function OnChangeGLGroup() {
    debugger;
    /*$("#ddl_GLAccountList").select2();*/
    var GLGroup_Id = $("#acc_group").val();
    //if (GLGroup_Id != "0") {
    //    /*var last3Digit = GLGroup_Id.slice(-3);*/
    //    //if (last3Digit != null && last3Digit != "" && last3Digit != "0") {
    //    //    $("#hdn_GLGroupId").val(last3Digit);
    //    $('#ddl_GLAccountList').empty();
    //    $('#ddl_GLAccountList').append(`<option value=0>---Select---</option>`);
    //    if (GLGroup_Id != null && GLGroup_Id != "" && GLGroup_Id != "0") {
           $("#hdn_GLGroupId").val(GLGroup_Id);

    //        $("#ddl_GLAccountList").select2({
    //            ajax: {
    //                url: $("#GLListName").val(),
    //                data: function (params) {
    //                    var queryParameters = {
    //                        //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
    //                        ddlGroup: params.term, // search term like "a" then "an"
    //                        Group: params.page,
    //                        GLGroupId: GLGroup_Id,
    //                        type: "Onchange",
    //                        //GlGroupId: $("#acc_group").val()

    //                    };
    //                    return queryParameters;
    //                },
    //                dataType: "json",
    //                cache: true,
    //                delay: 250,
    //                contentType: "application/json; charset=utf-8",
    //                processResults: function (data, params) {
    //                    debugger;
    //                    params.page = params.page || 1;
    //                    return {
    //                        results: $.map(data, function (val, item) {
    //                            return { id: val.ID, text: val.Name };
    //                        })
    //                    };
    //                }
    //            },
    //        });
    //        //$.ajax(
    //        //    {
    //        //        type: "POST",
    //        //        url: "/ApplicationLayer/JournalBook/BindGLAccountList",
    //        //        data: {
    //        //            GLGroupId: last3Digit,
    //        //        },
    //        //        dataType: "json",
    //        //        success: function (data) {
    //        //            debugger;
    //        //            if (data !== null && data !== "") {
    //        //                var arr = [];
    //        //                $('#ddl_GLAccountList').empty();
    //        //                arr = JSON.parse(data);
    //        //                if (arr.Table.length > 0) {
    //        //                    $('#ddl_GLAccountList').append(`<option value=0>---Select---</option>`);
    //        //                    for (var i = 0; i < arr.Table.length; i++) {
    //        //                        $('#ddl_GLAccountList').append(`<option value="${arr.Table[i].acc_id}">${arr.Table[i].acc_name}</option>`);
    //        //                    }
    //        //                }
    //        //                else {
    //        //                    $('#ddl_GLAccountList').append(`<option value=0>---Select---</option>`);
    //        //                }
    //        //            }
    //        //        },
    //        //    });
    //    }
    //}
    //else {
    //    $("#hdn_GLGroupId").val("");
    //    $('#ddl_GLAccountList').empty();
    //    $('#ddl_GLAccountList').append(`<option value=0>---Select---</option>`);
    //    $("#ddl_GLAccountList").select2({
    //        ajax: {
    //            url: $("#GLListName").val(),
    //            data: function (params) {
    //                var queryParameters = {
    //                    //restrictedCountry: $("#resCountry").val(),  // pass your own parameter                
    //                    ddlGroup: params.term, // search term like "a" then "an"
    //                    Group: params.page,
    //                    GLGroupId: GLGroup_Id,
    //                    type: "",
    //                    //GlGroupId: $("#acc_group").val()

    //                };
    //                return queryParameters;
    //            },
    //            dataType: "json",
    //            cache: true,
    //            delay: 250,
    //            contentType: "application/json; charset=utf-8",
    //            processResults: function (data, params) {
    //                debugger;
    //                params.page = params.page || 1;
    //                return {
    //                    results: $.map(data, function (val, item) {
    //                        return { id: val.ID, text: val.Name };
    //                    })
    //                };
    //            }
    //        },
    //    });
    //}




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
        var FromDate = $("#txtFromdateJB").val();
        var ToDate = $("#txtTodateJB").val();
        
        //var GroupId = $('#hdn_GLGroupId').val();
        //var AccountID = $('#hdn_GLAccountID').val();
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
        /* var VouTyp = $('#hdn_VouType').val();*/
        var VouTyp = $('#ddl_Voutype').val();
        $('#hdn_VouType').val(VouTyp);
        var CreatBy = $('#ddl_CreatedBy').val();
        var CreatOn = $('#txt_CreatedOn').val();
        var AppBy = $('#ddl_ApprovedBy').val();
        var AppOn = $('#txt_ApprovedOn').val();
        //var TDSID = $("#TDS_Type option:selected").attr("data-acc_id")
        var Narr = $("#txt_Narration").val().trim(); 
        //var Narr = $("#hdn_Narr").val();
        var Status = $("#ddlStatus").val();

        var FromDate1 = $("#txtFromdateJB").val().split("-").reverse().join("-");
        $("#hdn_From_VouDate").val(FromDate1);
        var ToDate1 = $("#txtTodateJB").val().split("-").reverse().join("-");
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
            url: "/ApplicationLayer/JournalBook/SearchJournalBookDetails",
            data: {
                FromDate: FromDate, ToDate: ToDate, GroupId: GroupId, AccountID: AccountID, AmtFrom: AmtFrom, AmtTo: AmtTo,
                VouTyp: VouTyp, CreatBy: CreatBy, CreatOn: CreatOn, AppBy: AppBy, AppOn: AppOn, Narr: Narr, Status: Status
            },
            success: function (data) {
                debugger;
                $("#MISJournalBookDetails").html(data);
                $("a.btn.btn-default.buttons-print.btn-sm").remove();
                $("a.btn.btn-default.buttons-pdf.buttons-html5.btn-sm").remove();
                $(".dt-buttons").append('<button class="btn btn-default buttons-pdf buttons-html5 btn-sm" onclick="JournalBookPrint()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>Print</span></button>')
                //$(".dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="CsvData" onclick="JournalBookCSV()" tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                hideLoader();/*Add by Hina sharma on 19-11-2024 */
            }
        });
    }
    catch (ex) {
        console.log(ex);
        hideLoader();
    }

}

function Onclick_CCbtnJB(e) {
    debugger;
    
    var ValDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Vou_No = "";
    var JBVou_No = clickedrow.find("#JBPrtl_VouNo").text().trim();
    var hdnJBVou_No = clickedrow.find("#hdnJBPrtl_VouNo").text().trim();
    /*if (JBVou_No == "") {*/
        Vou_No = hdnJBVou_No;
    /*}*/
    //if (hdnJBVou_No == "") {
    //    Vou_No = JBVou_No;
    //}
    var Vou_Dt = clickedrow.find("#JBPartl_VouDate").text().trim();
    var Vou_Typ = clickedrow.find("#JBPartl_VouType").text().trim();
    var GLAcc_Name = clickedrow.find("#JBPartl_AccName").text().trim();
    var GLAcc_id = clickedrow.find("#JBPartl_AccID").text().trim();
    var DRAmount = clickedrow.find("#JBPartl_dr_amt_bs").text().trim();
    var CRAmount = clickedrow.find("#JBPartl_cr_amt_bs").text().trim();
    var Amount = "";
    if (DRAmount == "0.00") {
        Amount = CRAmount;
    }
    if (CRAmount == "0.00") {
        Amount = DRAmount;
    }
    
    var disableflag = "N";
    
    //var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/JournalBook/JB_GetCostCenterData",
        data: {
            
            Vou_No: Vou_No,
            Vou_Dt: Vou_Dt,
            GLAcc_id: GLAcc_id,
            
        },
        success: function (data) {
            debugger;
            if (data == null && data == "") {

            }
            $("#CostCenterDetailPopup").html(data);
            //$("#CC_int_br_id").val(Int_Br_Id);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#hdnVou_No").val(Vou_No);
            $("#hdnVou_Dt").val(Vou_Dt);
            $("#hdnVou_Typ").val(Vou_Typ);
            $("#GLAmount").val(Amount);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            //$("#hdnTable_Id").text("JVouAccDetailsTbl");



        },
    })
}
function OnClickBillAdjIconBtn(e) {
    debugger;
    var ToDate = $("#txtTodateJB").val().trim();
    var ValDecDigit = $("#ValDigit").text().trim();
    var clickedrow = $(e.target).closest("tr");
    var AccID = clickedrow.find("#JBPartl_AccID").text().trim();
    var fromdt = $("#txtFromdateJB").val().trim();
    var todt = $("#txtTodateJB").val().trim();
    var DocumentNumber = "";
    /*var DocumentNumber = clickedrow.find("#JBPrtl_VouNo").text().trim();*/
    var JBVou_No = clickedrow.find("#JBPrtl_VouNo").text().trim();
    var hdnJBVou_No = clickedrow.find("#hdnJBPrtl_VouNo").text().trim();
   /* if (JBVou_No == "") {*/
        DocumentNumber = hdnJBVou_No;
   /* }*/
    //if (hdnJBVou_No == "") {
    //    DocumentNumber = JBVou_No;
    //}
    //if (hdnJBVou_No != "" && JBVou_No!="") {
    //}
    var VouTyp = clickedrow.find("#JBPartl_VouType").text().trim();
    var Status = clickedrow.find("#JBPartl_VouStatus").text().trim();
    if (Status == "D") {
        Status = "A";
    }
    var Curr = clickedrow.find("#JBPartl_CurrId").text().trim();
    var AccName = clickedrow.find("#JBPartl_AccName").text().trim();
    var DRAmount = clickedrow.find("#JBPartl_dr_amt_bs").text().trim();
    var CRAmount = clickedrow.find("#JBPartl_cr_amt_bs").text().trim();
    var Amount = "";
    if (DRAmount == "0.00") {
        Amount = CRAmount;
    }
    if (CRAmount == "0.00") {
        Amount = DRAmount;
    }
    $("#IconHdEntityID").val(AccID);
    $("#IconEntityName").val(AccName);
    $("#IconPayAmt").val(Amount);
   

    //sessionStorage.removeItem("FromDateToDate");
    //sessionStorage.setItem("FromDateToDate", "N");

    var Entity_Type = clickedrow.find("#JBPartl_AccType").text().trim();
    var flag = '';
    if (Entity_Type == "2") {
        flag = "P";
    }
    
   
    if (AccID != "" && AccID != null) {
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/BankPayment/GetBillDetail",
                data: {
                    AccId: AccID, fromdt: fromdt, todt: todt, flag: flag, DocumentNumber: DocumentNumber, Status: Status, Curr: Curr
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        $('#BillDetailTbl tbody tr').remove();
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;

                            for (var i = 0; i < arr.Table.length; i++) {
                                //debugger;
                                var inv_no = arr.Table[i].inv_no;
                                var inv_dt = arr.Table[i].inv_dt;
                                var acc_id = arr.Table[i].acc_id;
                                var vou_no = arr.Table[i].vou_no;

                                var Pay_Amount = 0;
                                //var Rem_Bal = parseFloat(cmn_ReplaceCommas(arr.Table[i].pend_amt)).toFixed(ValDecDigit);
                                var Rem_Bal = toDecimal(cmn_ReplaceCommas(arr.Table[i].pend_amt), ValDecDigit);

                                var Pend_Amount = 0;
                                var Pay_Amount = toDecimal(cmn_ReplaceCommas(arr.Table[i].paid_amt), ValDecDigit);
                                var Pend_Amount = toDecimal(cmn_ReplaceCommas(arr.Table[i].pendamt), ValDecDigit);
                                //if (AccID == acc_id && DocumentNumber == vou_no ) {

                                //                if ((Pay_Amount == "") || (Pay_Amount == null) || (Pay_Amount == 0) || (Pay_Amount == "NaN"))
                                //                {
                                //                    Pay_Amount = 0;
                                //                }
                                //                else {
                                //                    Pay_Amount = Pay_Amount;
                                //                }
                                //                if ((Rem_Bal == "") || (Rem_Bal == null) || (Rem_Bal == 0) || (Rem_Bal == "NaN")) {
                                //                    Rem_Bal = 0;
                                //                }
                                //                else {
                                //                    Rem_Bal = Rem_Bal;
                                //                }
                                //                if ((Pend_Amount == "") || (Pend_Amount == null) || (Pend_Amount == 0) || (Pend_Amount == "NaN")) {
                                //                    Pend_Amount = 0;
                                //                }
                                //                else {
                                //                    Pend_Amount = Pend_Amount;
                                //                }

                                //}
                                var FBillDetails = JSON.parse(sessionStorage.getItem("BillDetailSession"));
                                //if (FBillDetails != null && FBillDetails != "") {
                                //    if (FBillDetails.length > 0) {
                                //        for (j = 0; j < FBillDetails.length; j++) {
                                //            debugger;
                                //            var InvNo = FBillDetails[j].InvoiceNo;
                                //            var InvDt = FBillDetails[j].InvoiceDate;
                                //            var SSAccID = FBillDetails[j].AccID;
                                //            var PayAmount = parseFloat(cmn_ReplaceCommas(FBillDetails[j].PayAmount));
                                //            var RemBal = parseFloat(cmn_ReplaceCommas(FBillDetails[j].RemBal));
                                //            var PendAmount = parseFloat(cmn_ReplaceCommas(FBillDetails[j].PendAmount));
                                //            if (SSAccID == acc_id && InvNo == inv_no && InvDt == inv_dt) {
                                //                if ((PayAmount == "") || (PayAmount == null) || (PayAmount == 0) || (PayAmount == "NaN")) {
                                //                    Pay_Amount = 0;
                                //                }
                                //                else {
                                //                    Pay_Amount = PayAmount;
                                //                }
                                //                if ((RemBal == "") || (RemBal == null) || (RemBal == 0) || (RemBal == "NaN")) {
                                //                    Rem_Bal = 0;
                                //                }
                                //                else {
                                //                    Rem_Bal = RemBal;
                                //                }
                                //                if ((PendAmount == "") || (PendAmount == null) || (PendAmount == 0) || (PendAmount == "NaN")) {
                                //                    Pend_Amount = 0;
                                //                }
                                //                else {
                                //                    Pend_Amount = PendAmount;
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                                if (AvoidDot(Pay_Amount) == false) {
                                    Pay_Amount = 0;
                                }
                                if (AvoidDot(Pend_Amount) == false) {
                                    Pend_Amount = 0;
                                }
                                if (AvoidDot(Rem_Bal) == false) {
                                    Rem_Bal = 0;
                                }
                                var UnAdjustedAmount = parseFloat(cmn_ReplaceCommas(Pay_Amount)) + parseFloat(cmn_ReplaceCommas(Rem_Bal));
                                
                                var td_billNoAndDate = '';
                                if (/*flag == "P" || */VouTyp == "BP" || VouTyp == "CP" || VouTyp == "DN") {
                                    
                                    $("#BillDetailTbl > thead > tr > #cmn_bill_number").attr("hidden", false);
                                    $("#BillDetailTbl > thead > tr > #cmn_bill_date").attr("hidden", false);

                                    td_billNoAndDate = `<td><input id="BillNo" class="form-control" autocomplete="off" type="text" name="InvoiceDate" disabled value="${arr.Table[i].bill_no}"></td>
                        <td><input id="BillDate" class="form-control " autocomplete="off" type="date" name="InvoiceDate" disabled value="${arr.Table[i].bill_dt}"></td>
                        `
                                }
                                else {
                                    $("#BillDetailTbl > thead > tr > #cmn_bill_number").attr("hidden", true);
                                    $("#BillDetailTbl > thead > tr > #cmn_bill_date").attr("hidden", true);
                                  }

                                ++rowIdx;
                                if (/*flag == "P" || */VouTyp == "BP" || VouTyp == "CP" || VouTyp == "DN") {
                                    $('#BillDetailTbl tbody').append(` <tr id="${rowIdx}">
                        <td class="center"><input type="checkbox" class="tableflat" id="BillCheck" onclick="OnClickBillCheck(event)"></td>
                        <td class="center">${rowIdx}</td>
                            <td>
                        <div class="col-sm-10 no-padding">
                            <input id="InvoiceNumber" class="form-control" autocomplete="off" type="text" name="InvoiceNumber"  placeholder="" disabled value="${arr.Table[i].inv_no}">
                         <input id="HdAccId" hidden value="${AccID}">
                        </div>
                        <div class="col-sm-2 i_Icon">
                            <button type="button" class="calculator" onclick="" data-toggle="modal" data-target="#InvoiceDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_InvoiceDetail").text()}"> </button>
                        </div>
                        </td>
                        <td><input id="InvoiceDate" class="form-control " autocomplete="off" type="date" name="InvoiceDate" disabled value="${arr.Table[i].inv_dt}"></td>
                        `+ td_billNoAndDate + `<td><input id="Currency" class="form-control center" autocomplete="off" type="text" disabled value="${arr.Table[i].curr}"></td>
                        <td><input id="InvoiceAmountInBase" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInBase" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_bs}"></td>
                        <td><input id="InvoiceAmountInSpecific" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInSpecific" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_sp}"></td>
                        <td><input id="UnAdjustedAmount"  class="form-control num_right" autocomplete="off" type="text" name="UnAdjustedAmount" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(UnAdjustedAmount), ValDecDigit))}"></td>
                        <td><div class="lpo_form"><input id="PayAmount"  class="form-control num_right" onchange="OnChangePayAmount(event)" onkeypress="return AmountFloatVal(this, event)" value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Pay_Amount), ValDecDigit))}" type="text" placeholder="0000.00">
                        <span id="PayAmount_Error" class="error-message is-visible"></span></div></td>                        
                        <td><input id="RemainingBalance" class="form-control num_right" autocomplete="off" type="text" name="RemainingBalance" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Rem_Bal), ValDecDigit))}"></td>
                        </tr>`);
                                }
                                else {
                                    $('#BillDetailTbl tbody').append(` <tr id="${rowIdx}">
                        <td class="center"><input type="checkbox" class="tableflat" id="BillCheck" onclick="OnClickBillCheck(event)"></td>
                        <td class="center">${rowIdx}</td>
                            <td>
                        <div class="col-sm-10 no-padding">
                            <input id="InvoiceNumber" class="form-control" autocomplete="off" type="text" name="InvoiceNumber"  placeholder="" disabled value="${arr.Table[i].inv_no}">
                         <input id="HdAccId" hidden value="${AccID}">
                        </div>
                        <div class="col-sm-2 i_Icon">
                            <button type="button" class="calculator" onclick="" data-toggle="modal" data-target="#InvoiceDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_InvoiceDetail").text()}"> </button>
                        </div>
                        </td>
                        <td><input id="InvoiceDate" class="form-control " autocomplete="off" type="date" name="InvoiceDate" disabled value="${arr.Table[i].inv_dt}"></td>
                        <td><input id="Currency" class="form-control center" autocomplete="off" type="text" disabled value="${arr.Table[i].curr}"></td>
                        <td><input id="InvoiceAmountInBase" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInBase" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_bs}"></td>
                        <td><input id="InvoiceAmountInSpecific" class="form-control num_right" autocomplete="off" type="text" name="InvoiceAmountInSpecific" placeholder="0000.00" disabled value="${arr.Table[i].inv_amt_sp}"></td>
                        <td><input id="UnAdjustedAmount"  class="form-control num_right" autocomplete="off" type="text" name="UnAdjustedAmount" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(UnAdjustedAmount), ValDecDigit))}"></td>
                        <td><div class="lpo_form"><input id="PayAmount"  class="form-control num_right" onchange="OnChangePayAmount(event)" onkeypress="return AmountFloatVal(this, event)" value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Pay_Amount), ValDecDigit))}" type="text" placeholder="0000.00">
                        <span id="PayAmount_Error" class="error-message is-visible"></span></div></td>                        
                        <td><input id="RemainingBalance" class="form-control num_right" autocomplete="off" type="text" name="RemainingBalance" placeholder="0000.00" disabled value="${cmn_addCommas(toDecimal(cmn_ReplaceCommas(Rem_Bal), ValDecDigit))}"></td>
                        </tr>`);
                                }
                                //debugger;
                                
                                //document.getElementById("txtFromdate").setAttribute('min', arr.Table[0].min_dt);
                                //document.getElementById("txtTodate").setAttribute('min', arr.Table[0].min_dt);
                                $("#txtTodate").val(todt);
                                //document.getElementById("txtTodate").val(ToDate);
                                //$("#txtFromdate").val(arr.Table[0].min_dt);

                                
                            }
                            DisableDetail(Status);
                            CalculateTotalAmt();
                        }
                        else {
                            //Cmn_BindVouDate(todt, todt);/*Add by Hina on 12-11-2024 to change discuss by vishal sir*/
                        }
                        
                    }
                    DisableData();
                    BillDetailCheckbox();
                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

    }
}
function DisableDetail(Status) {
    debugger;
    
   $('#BillDetailTbl tbody tr').each(function () {
            var currentrow = $(this);
            currentrow.find("#PayAmount").attr("disabled", true);
            currentrow.find("#BillCheck").attr("disabled", true);
        });
   
}
function RemoveSession() {
    sessionStorage.removeItem("BillDetailSession");
    sessionStorage.removeItem("FromDateToDate");
    
}
function DisableData() {
    debugger;
    var Disable = "Disable";
    if (Disable == "Disable") {
        $("#BillDetailTbl tbody tr").each(function () {
            debugger;
            var CurrentRow = $(this);
            CurrentRow.find("#BillCheck").prop("disabled", true);
        });
    }
    var ValDecDigit = $("#ValDigit").text();
    var TotalAmount = parseFloat(0).toFixed(ValDecDigit);
    var len = $("#BillDetailTbl tbody tr").length;
    if (len == 0) {
        $("#TotalPaid").text(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalAmount), ValDecDigit)));
        $("#txtTodate").val("");
       $("#txtFromdate").val("");
    }
    

}
function CalculateTotalAmt() {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var TotalAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#BillDetailTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PayAmount = cmn_ReplaceCommas(CurrentRow.find("#PayAmount").val());
        if (PayAmount != null && PayAmount != "") {
            TotalAmount = parseFloat(cmn_ReplaceCommas(TotalAmount)) + parseFloat(cmn_ReplaceCommas(PayAmount));
        }
    });

    //$("#TotalPaid").text(parseFloat(cmn_ReplaceCommas(TotalAmount)).toFixed(ValDecDigit));
    var len = $("#BillDetailTbl tbody tr").length;
    if (len > 0) {
        $("#TotalPaid").text(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalAmount), ValDecDigit)));
    }
    else {
        $("#TotalPaid").text(cmn_addCommas(toDecimal(cmn_ReplaceCommas(TotalAmount), ValDecDigit)));
    }
    
}
function BillDetailCheckbox() {
    var check1 = "Y"
    $("#BillDetailTbl tbody tr").each(function () {
        debugger;
        var CurrentRow = $(this);
        if (CurrentRow.find("#BillCheck").is(":checked")) {
            check1 = 'Y';
        }
        else {
            check1 = 'N';
            return false;
        }
    });
    if (check1 == "Y") {
        $("#AllBillCheck").prop("checked", true);
    }
    else {
        $("#AllBillCheck").prop("checked", false);
    }
}

function JournalBookPrint() {
    debugger;
    var arr = [];
    var FromDate = $("#txtFromdateJB").val();
    var ToDate = $("#txtTodateJB").val();
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
    $("#JBPrintData").val(array);
    $("#hdnJBCSVPrint").val(null);
    $("#hdnJBPDFPrint").val("Print");
    $('form').submit();

}
function JournalBookCSV() {
    debugger;
    var arr = [];
    var FromDate = $("#txtFromdateJB").val();
    var ToDate = $("#txtTodateJB").val();
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
    $("#JBPrintData").val(array);

    var filters = FromDate + "," + ToDate + "," + GroupId + "," + AccountID + "," + AmtFrom + "," + AmtTo + "," + VouTyp + "," + CreatBy + "," + CreatOn + "," + AppBy + "," + AppOn + "," + Narr + "," + Status;
    $("#Allfilters").val(filters);
    $("#hdnJBPDFPrint").val(null);
    $("#hdnJBCSVPrint").val("CsvPrint");
    var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    $("#JBsearchValue").val(searchValue);

    $('form').submit();
    //var searchValue = $("#datatable-buttons5_filter input[type=search]").val();
    //window.location.href = "/ApplicationLayer/TrialBalance/ExportTrialBalanceData?searchValue=" + searchValue + "&filters=" + filters;

}