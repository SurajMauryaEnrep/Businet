$(document).ready(function () {
    debugger;
   
    hdnAccGrpval();
    $("#JvListtbl #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $("#ListFilterData").val();
            var WF_Status = $("#WF_Status").val();
            var clickedrow = $(e.target).closest("tr");
            var JVId = clickedrow.children("#vou_no").text();
            var JVDate = clickedrow.children("#vou_date").text();
            if (JVId != null && JVId != "") {
                //window.location.href = "/ApplicationLayer/JournalVoucher/JournalVoucherDetail/?JVId=" + JVId + "&JVDate=" + JVDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
                window.location.href = "/ApplicationLayer/JournalVoucher/Dblclick/?JVId=" + JVId + "&JVDate=" + JVDate + "&ListFilterData=" + ListFilterData + "&WF_Status=" + WF_Status;
            }
        }
        catch (err) {
            debugger
        }
    });

    $("#datatable-buttons >tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        var JV_No = clickedrow.children("#vou_no").text().trim();
        var JV_Date = clickedrow.children("#vou_date").text();
        var Doc_id = $("#DocumentMenuId").val();
        debugger;

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(JV_No, JV_Date, Doc_id, Doc_Status);
        $("#hdDoc_No").val(JV_No);
    });
    BindDDLAccountList();
    $('#JVouAccDetailsTbl tbody').on('click', '.deleteIcon', function () {
        debugger;
         var txtDisable = $("#txtdisable").val();
        if (txtDisable != "Y") {
            var hfAccID = $(this).closest('tr').find('#hfAccID').val();

            if (hfAccID != "") {
                deleteCCdetail(hfAccID);
            }
            $(this).closest('tr').remove();
            CalculateTotalDrCrValue();
            SerialNoAfterDelete();
        }
       });
    
    //--show detail on worlflow bar in detail page--//
    var JV_No = $("#JVNumber").val();
    $("#hdDoc_No").val(JV_No);
    ReplicateWithJV();
    //$(document).ready(function () {
    //    debugger;
    //    /*$("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
    //    $("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="JournalVoucherCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
    //});
    CancelledRemarks("#CancelFlag", "Disabled");
});
function JournalVoucherCSV() {
    debugger;
    var Fromdate = $("#txtFromdate").val();
    var Todate = $("#txtTodate").val();
    var Status = $("#ddlStatus").val();
    var searchValue = $("#datatable-buttons_filter input[type=search]").val();

    window.location.href = "/ApplicationLayer/JournalVoucher/JournalVoucherExporttoExcelDt?Fromdate=" + Fromdate + "&Todate=" + Todate + "&Status=" + Status + "&searchValue=" + searchValue;
}
function ReplicateWithJV() {
    debugger;
    var item = $("#ddlReplicateWithJV").val();
   
    $("#ddlReplicateWithJV").append("<option value='0'>---Select---</option>");
    $("#ddlReplicateWithJV").select2({
        ajax: {
            url: "/ApplicationLayer/JournalVoucher/BindReplicateWithlist",
            data: function (params) {
                var queryParameters = {
                    item: params.term,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 2000; // or whatever pagesize

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
                    <div class="row">
                    <div class="col-md-6 col-xs-12 def-cursor">${$("#DocNo").text()}</div>
                    <div class="col-md-6 col-xs-12 def-cursor" id="soDocDate">${$("#DocDate").text()}</div>
                    </div>
                    </strong></li></ul>`)
                }
                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.ID.split(",")[0], document: val.Name, };
                    }),
                };
            },
            cache: true
        },
        templateResult: function (data) {
            debugger
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row rwjv">' +
                '<div class="col-md-8 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-4 col-xs-12' + classAttr + '">' + data.document + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function ddlReplicateWithJV_onchange() {
    debugger;
    var replicate = $("#ddlReplicateWithJV").val();

    var Vou_no = replicate.split(",")[0];
    var Vou_dt1 = replicate.split(",")[1]

    var date = Vou_dt1.split("-");
    var vou_dt = date[2] + '-' + date[1] + '-' + date[0];
    $('#JVouAccDetailsTbl >tbody >tr').remove();
    $("#ddlReplicateWithJV").attr("disabled", true);
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/JournalVoucher/GetReplicateWithVouNumber",
            data: {
                Vou_no: Vou_no,
                vou_dt: vou_dt
            },
            success: function (data) {
                debugger;
                if (data !== null && data !== "" && data != "ErrorPage") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.Table.length > 0) {
                        for (var i = 0; i < arr.Table.length; i++) {
                            for (var i = 0; i < arr.Table.length; i++) {
                                var acc_id = arr.Table[i].acc_id;
                                var acc_name = arr.Table[i].acc_name;
                                var vou_no = arr.Table[i].vou_no;
                                var vou_dt = arr.Table[i].vou_dt;
                                var vou_type = arr.Table[i].vou_type;
                                var acc_type = arr.Table[i].acc_type;
                                var curr_id = arr.Table[i].curr_id;
                                var acc_grp_id = arr.Table[i].acc_grp_id;
                                var acc_group_name = arr.Table[i].acc_group_name;
                                var conv_rate = arr.Table[i].conv_rate;
                                var dr_amt_bs = arr.Table[i].dr_amt_bs;
                                var dr_amt_sp = arr.Table[i].dr_amt_sp;
                                var cr_amt_bs = arr.Table[i].cr_amt_bs;
                                var cr_amt_sp = arr.Table[i].cr_amt_sp;
                                var narr = arr.Table[i].narr;
                                var Avl_bal = arr.Table[i].Avl_bal;
                                onchangereplicate_item(acc_id, acc_name, vou_no, vou_dt, vou_type, acc_type, curr_id, acc_grp_id, acc_group_name, conv_rate, dr_amt_bs, dr_amt_sp, cr_amt_bs, cr_amt_sp, narr, Avl_bal);
                            }
                        }
                        debugger
                        $("#JVouAccDetailsTbl tbody tr").each(function () {
                            debugger
                            var currentRow = $(this);
                            OnChangeCreditAmount(currentRow);
                        });
                    }
                }
            }
        })
}
function onchangereplicate_item(acc_id, acc_name, vou_no, vou_dt, vou_type, acc_type, curr_id, acc_grp_id, acc_group_name, conv_rate, dr_amt_bs, dr_amt_sp, cr_amt_bs, cr_amt_sp, narr, Avl_bal) {
    debugger;
      var rowIdx = 0;
    debugger;
    
    var rowCount = $('#JVouAccDetailsTbl >tbody >tr').length + 1
    var RowNo = 0;

    $("#JVouAccDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);

        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;

    });
    if (RowNo == "0") {
        RowNo = 1; 
    }
    /*------------Code Start Added by Hina on 28-08-2025 for disabled fields----------------------*/
    DebitAmt = "";
    CreditAmt = "";
    if (dr_amt_bs == "0.00") {/*Added by Hina on 28-08-2025*/
        DebitAmt = `<td><div class="lpo_form"><input id="DebitAmountInBase"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${dr_amt_bs}" class="form-control num_right" disabled autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00"><span id="Dbt_Amnt_Error" class="error-message is-visible"></span></div></td>`
        CreditAmt = `<td><div class="lpo_form"><input id="CreditAmountInBase"  onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${cr_amt_bs}" class="form-control num_right"  autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00"><span id="Crdt_Amnt_Error" class="error-message is-visible"></span></div></td>`

    }
    else {
        DebitAmt = `<td><div class="lpo_form"><input id="DebitAmountInBase"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${dr_amt_bs}" class="form-control num_right"  autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00"><span id="Dbt_Amnt_Error" class="error-message is-visible"></span></div></td>`
        CreditAmt = `<td><div class="lpo_form"><input id="CreditAmountInBase"  onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${cr_amt_bs}" class="form-control num_right" disabled autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00"><span id="Crdt_Amnt_Error" class="error-message is-visible"></span></div></td>`

    }
    if (cr_amt_bs == "0.00") {/*Added by Hina on 28-08-2025*/
        DebitAmt = `<td><div class="lpo_form"><input id="DebitAmountInBase"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${dr_amt_bs}" class="form-control num_right"  autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00"><span id="Dbt_Amnt_Error" class="error-message is-visible"></span></div></td>`
        CreditAmt = `<td> <div class="lpo_form"><input id="CreditAmountInBase" onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${cr_amt_bs}" class="form-control num_right" disabled autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00"><span id="Crdt_Amnt_Error" class="error-message is-visible"></span></div></td>`
    }
    else {
        DebitAmt = `<td><div class="lpo_form"><input id="DebitAmountInBase"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${dr_amt_bs}" class="form-control num_right" disabled autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00"><span id="Dbt_Amnt_Error" class="error-message is-visible"></span></div></td>`
        CreditAmt = `<td> <div class="lpo_form"><input id="CreditAmountInBase" onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" value="${cr_amt_bs}" class="form-control num_right"  autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00"><span id="Crdt_Amnt_Error" class="error-message is-visible"></span></div></td>`

    }
    /*------------Code End Added by Hina on 28-08-2025----------------------*/
    $('#JVouAccDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td>
<select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange ="OnChangeAccountName(${RowNo},event)"></select>
<input  type="hidden" id="hfAccID" value="${acc_id}" />
<input  type="hidden" id="hfAccName" value="${acc_name}" />
<span id="AccountNameError" class="error-message is-visible"></span>
</td>


<td>
<div class="col-sm-10 lpo_form no-padding"><input id="RowWiseAccBal" class="form-control num_right" value="${Avl_bal}" autocomplete="off" type="text"  name="accountbalance" placeholder="0000.00" readonly></div>
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','txtJVDate',${RowNo})" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_AccountBalanceDetail").text()}"> </button></div>
<input id="hdn_RowWiseAccBal"  type="hidden" />
</td>


<td><textarea id="GLGroup" class="form-control remarksmessage" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="GLGroup" placeholder="${$("#AccGroup").text()}" readonly>${acc_group_name}</textarea>
<input id="hdGLGroupID" value="${acc_grp_id}" type="hidden" />
<input id="hdAccType" value="${acc_type}" type="hidden" />
<input id="hdCurrId"value="${curr_id}"  type="hidden" />
<input id="hdConvsRateId" value="${conv_rate}"  type="hidden" />
<input id="hdnAccGrpval3_4"  type="hidden" />
</td>

 `+ DebitAmt + `
 `+ CreditAmt + `

<td>
<div class="lpo_form">
<textarea id="Narration" class="form-control remarksmessage"  type="text" onmouseover="OnMouseOver(this)" onchange="OnchangeNarr(event)"  onkeyup="OnchangeNarr(event)"   name="Narration" maxlength="200" placeholder="${$("#span_Narration").text()}">${narr}</textarea>
<span id="Narr_Error" class="error-message is-visible"></span>
</div>
</td>
<td class="center"><button type="button" id="BtnCostCenterDetail" onclick="OnclickCcBtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" disabled data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button></td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" />

</td>
</tr>`);
    /*commented and modify above by Hina on 28-08-2025 to add account balance for particular account*/
    //<td><div class="col-sm-11 lpo_form no-padding"><select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange="OnChangeAccountName(${RowNo},event)"></select>
    //    <input type="hidden" id="hfAccID" value="${acc_id}" />
    //    <input type="hidden" id="hfAccName" value="${acc_name}" />
    //    <span id="AccountNameError" class="error-message is-visible"></span></div>
    //    <div class="col-md-1 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','txtJVDate',${RowNo})" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$(" #span_AccountBalanceDetail").text()}"> </button></div>
    //</td>
    BindDDLAccountList();
}
var ValDigit = $("#ValDigit").text();
function hdnAccGrpval() {
    debugger;
    $("#JVouAccDetailsTbl > tbody > tr").each(function () {
        debugger;
        var CurrRow = $(this);
        var hdngrpval_id = CurrRow.find("#hdnAccGrpval3_4").val();
        var debitAmt12 = CurrRow.find("#DebitAmountInBase").val();
        var debitAmt = cmn_ReplaceCommas(debitAmt12);
        if (AvoidDot(debitAmt) == false) {
            debitAmt = "";
        }
        var creditAmt12 = CurrRow.find("#CreditAmountInBase").val();
        var creditAmt = cmn_ReplaceCommas(creditAmt12);
        if (AvoidDot(creditAmt) == false) {
            creditAmt = "";
        }
        if (debitAmt != "") {
            if (hdngrpval_id == "4") {
                CurrRow.find("#BtnCostCenterDetail").attr("disabled", false);
            }
            else {
                CurrRow.find("#BtnCostCenterDetail").attr("disabled", true);
            }
        }
        //else {
        //    CurrRow.find("#BtnCostCenterDetail").attr("disabled", true);
        //}
       
        else if (creditAmt != "") {
            if (hdngrpval_id == "3") {
                CurrRow.find("#BtnCostCenterDetail").attr("disabled", false);
            }
            else {
                CurrRow.find("#BtnCostCenterDetail").attr("disabled", true);
            }
        }
        else {
            CurrRow.find("#BtnCostCenterDetail").attr("disabled", true);
        }
        //if (hdngrpval_id == "3" || hdngrpval_id == "4") {
        //    CurrRow.find("#BtnCostCenterDetail").attr("disabled", false);
        //}
        //else {
        //    CurrRow.find("#BtnCostCenterDetail").attr("disabled", true);
        //}
    })
}
function deleteCCdetail(hfAccID) {
    debugger;
    if ($("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").length > 0) {
        $("#tbladdhdn > tbody >tr #hdntbl_GlAccountId:contains(" + hfAccID + ")").closest('tr').remove();
    }
}
//----Bind Account Searchable Dropdown------//
function BindDDLAccountList() {
    debugger;
    /*commented and modify by Hina Sharma on 18-07-2025 for resolve performence issue*/
     //Cmn_BindAccountList("#GLAccount_", "1", "#JVouAccDetailsTbl", "#SNohf", "BindData", "105104115101");
   /* if ($("#StatusCode").val() != "A") {*/


        $("#JVouAccDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var rowid = currentRow.find("#SNohf").val();

            var AccID = currentRow.find("#hfAccID").val();
            var AccName = currentRow.find("#hfAccName").val();
            var glBrId = currentRow.find("#GLbranch_" + rowid).val();
            $("#GLAccount_" + rowid).append(`<option value="${AccID}">${AccName}</option>`);
                      
            Cmn_BindAccountListAsync("#GLAccount_", rowid, "#JVouAccDetailsTbl", "#SNohf", "", "105104115101", null);
        });

    /*}*/
  }
function BindData() {
    debugger;

    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#JVouAccDetailsTbl >tbody >tr").each(function () {

                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                rowid = parseFloat(rowid) + 1;
                if (rowid > $("#JVouAccDetailsTbl >tbody >tr").length) {
                    return false;
                }
                $("#GLAccount_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}'></optgroup>`);
                for (var i = 0; i < AccountListData.length; i++) {
                    $('#Textddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                }
                var firstEmptySelect = true;
                $("#GLAccount_" + rowid).select2({

                    templateResult: function (data) {
                        var selected = $("#GLAccount_" + rowid).val();
                        if (check(data, selected, "#JVouAccDetailsTbl", "#SNohf", "#GLAccount_") == true) {
                         var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row">' +
                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                              '</div>'
                            );
                            return $result;
                        }
                        firstEmptySelect = false;
                    }
                });

            });
        }
    }
    $("#JVouAccDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        var AccountID = '#GLAccount_' + rowid;
        if (AccID != '0' && AccID != "") {
            currentRow.find("#GLAccount_" + rowid).attr("onchange", "");
            currentRow.find("#GLAccount_" + rowid).val(AccID).trigger('change.select2');
            currentRow.find("#GLAccount_" + rowid).attr("onchange", "OnChangeAccountName(this, event)");
        }
        

    });

}

//------Bind Group on change of Account Dropdown-------//
function OnChangeAccountName(RowID, e) {
    debugger;
    BindJornlVoucAccountList(e);
}
function BindJornlVoucAccountList(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    var hdn_acc_id = clickedrow.find("#hfAccID").val();
    if (hdn_acc_id != "") {
        var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        if (Len > 0) {
            $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').remove();
        }
    }
    Acc_ID = clickedrow.find("#GLAccount_" + SNo).val();
    clickedrow.find("#hfAccID").val(Acc_ID);

    if (Acc_ID != "0") {
        clickedrow.find("#DebitAmountInBase").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        clickedrow.find("#CreditAmountInBase").val(""/*parseFloat(0).toFixed(ValDigit)*/);
        clickedrow.find("#CreditAmountInBase").attr("disabled", false);
        clickedrow.find("#DebitAmountInBase").attr("disabled", false);
    }

    if (Acc_ID == "0") {
        clickedrow.find("#AccountNameError").text($("#valueReq").text());
        clickedrow.find("#AccountNameError").css("display", "block");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid red");
    }
    else {
        clickedrow.find("#AccountNameError").css("display", "none");
        clickedrow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");

    }
    Cmn_BindGroup(clickedrow, Acc_ID, "")
/*-----------Code Start Add by Hina Sharma on 26-08-2025 to get account Balance Account wise-------- */
    var VouDate = $("#txtJVDate").val();
    CMN_BindAccountBalance(clickedrow, Acc_ID, VouDate);
    /*-----------Code End Add by Hina Sharma on 26-08-2025 to get account Balance Account wise-------- */
 }

//---Work on add new row Button in GL DEtail Section-----//
function AddNewRow() {
    debugger
    var rowIdx = 0;
    debugger;
    
    var rowCount = $('#JVouAccDetailsTbl >tbody >tr').length + 1
    var RowNo = 0;

    $("#JVouAccDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);

        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;

    });
    if (RowNo == "0") {
        RowNo = 1; 
    }
    $('#JVouAccDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red"> <i class="deleteIcon fa fa-trash" id="delBtnIcon${RowNo}" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
 <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
<td><div class="col-sm-12 lpo_form" style="padding:0px;"><select class="form-control" id="GLAccount_${RowNo}" name="GLAccount_${RowNo}" onchange ="OnChangeAccountName(${RowNo},event)"></select>
<input  type="hidden" id="hfAccID" value="" />
<input  type="hidden" id="hfAccName" value="" />
<span id="AccountNameError" class="error-message is-visible"></span></div>

</td>

<td>
<div class="col-sm-10 lpo_form no-padding"><input id="RowWiseAccBal" class="form-control num_right" autocomplete="off" type="text"  name="accountbalance" placeholder="0000.00" readonly></div>
<div class="col-md-2 col-sm-12 i_Icon"><button type="button" id="" class="calculator" onclick="onlickCalculatorIcon(event,'Y','txtJVDate',${RowNo})" data-toggle="modal" data-target="#AccountBalanceDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_AccountBalanceDetail").text()}"></button></div>
<input id="hdn_RowWiseAccBal"  type="hidden" />
</td>


<td><textarea id="GLGroup" class="form-control remarksmessage" autocomplete="off" type="text" onmouseover="OnMouseOver(this)" name="GLGroup" placeholder="${$("#AccGroup").text()}" readonly></textarea>
<input id="hdGLGroupID" type="hidden" />
<input id="hdAccType" type="hidden" />
<input id="hdCurrId"  type="hidden" />
<input id="hdConvsRateId"  type="hidden" />
<input id="hdnAccGrpval3_4"  type="hidden" />
</td>
<td>
<div class="lpo_form">
<input id="DebitAmountInBase"  onchange="OnChangeDebitAmount(event)" onkeyup="OnKeyupDebtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="DebitAmount" placeholder="0000.00">
<span id="Dbt_Amnt_Error" class="error-message is-visible"></span>
 </div>
</td>
<td>
<div class="lpo_form">
<input id="CreditAmountInBase"  onchange="OnChangeCreditAmount(event)" onkeyup="OnKeyupCrdtAmnt(event)" onkeypress="return AmountFloatVal(this, event)" class="form-control num_right" autocomplete="off" type="text" name="CreditAmount" placeholder="0000.00">
<span id="Crdt_Amnt_Error" class="error-message is-visible"></span>
 </div>
</td>
<td>
<div class="lpo_form">
<textarea id="Narration" class="form-control remarksmessage"  type="text" onmouseover="OnMouseOver(this)" onchange="OnchangeNarr(event)"  onkeyup="OnchangeNarr(event)"   name="Narration" maxlength="200" placeholder="${$("#span_Narration").text()}">${$("#txtJvNarrat").val()}</textarea>
<span id="Narr_Error" class="error-message is-visible"></span>
</div>
</td>
<td class="center"><button type="button" id="BtnCostCenterDetail" onclick="OnclickCcBtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" disabled data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button></td>
<td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" />

</td>

</tr>`);
    //--------- FOR Value of text Field(For Narration Text Field) insert on add Account Button, Value Gives Between TEXTAREA as ${$("#txtJvNarrat").val()}---------//
    BindAcountList(RowNo);
}

function BindAcountList(ID) {
    debugger; 
    /*commented and modify by Hina Sharma on 18-07-2025 for resolve performence issue*/
    //Cmn_BindAccountList("#GLAccount_", ID, "#JVouAccDetailsTbl", "#SNohf", "", "105104115101");
   /* if ($("#StatusCode").val() != "A") {*/


        $("#JVouAccDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var rowid = currentRow.find("#SNohf").val();

            var AccID = currentRow.find("#hfAccID").val();
            //var AccName = currentRow.find("#hfAccName").val();
            var glBrId = currentRow.find("#GLbranch_" + rowid).val();
            $("#GLAccount_" + rowid).append(`<option value="${'0'}">${'---Select---'}</option>`);

            Cmn_BindAccountListAsync("#GLAccount_", rowid, "#JVouAccDetailsTbl", "#SNohf", "", "105104115101", null);
        });

    /*}*/

}

//-------work on Delete Trash Button on GL Detail and other work on Js Page Load(Above in document.ready) -----//
function SerialNoAfterDelete() {
    var SerialNo = 0;
    $("#JVouAccDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });

};

//--------------Validation------------------//

function AccountValidation() {
    debugger
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    if ($("#JVouAccDetailsTbl tbody tr").length > 0) {
        $("#JVouAccDetailsTbl tbody tr").each(function () {
            debugger
            var currentrow = $(this);
            var Rowno = currentrow.find("#SNohf").val();
            var AccName = currentrow.find("#GLAccount_" + Rowno).val();
            var DbtAmnt = cmn_ReplaceCommas(currentrow.find("#DebitAmountInBase").val());
            var CrdtAmnt = cmn_ReplaceCommas(currentrow.find("#CreditAmountInBase").val());          
            var Jv_Narrat = currentrow.find("#Narration").val().trim();         
            if (AccName == "0") {
                currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "red");
                currentrow.find("#AccountNameError").text($("#valueReq").text());
                currentrow.find("#AccountNameError").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentrow.find("[aria-labelledby='select2-GLAccount_" + Rowno + "-container']").css("border-color", "#ced4da");
                currentrow.find("#AccountNameError").text("");
                currentrow.find("#AccountNameError").css("display", "none");
            }
            var CrdtAmnt1 = toDecimal(cmn_ReplaceCommas(CrdtAmnt), ValDigit);
            var DbtAmnt1 = toDecimal(cmn_ReplaceCommas(DbtAmnt), ValDigit)
           // if ((DbtAmnt == "" && parseFloat(CrdtAmnt).toFixed(ValDigit) == parseFloat(0).toFixed(ValDigit)) || (parseFloat(DbtAmnt).toFixed(ValDigit) == parseFloat(0).toFixed(ValDigit) && CrdtAmnt == "") || (CrdtAmnt == "" && DbtAmnt == "") || (CrdtAmnt == parseFloat(0).toFixed(ValDigit) && DbtAmnt == parseFloat(0).toFixed(ValDigit))) {
            if ((DbtAmnt == "" && CrdtAmnt1 == parseFloat(0).toFixed(ValDigit)) || (DbtAmnt1 == parseFloat(0).toFixed(ValDigit) && CrdtAmnt == "") || (CrdtAmnt == "" && DbtAmnt == "") || (CrdtAmnt == parseFloat(0).toFixed(ValDigit) && DbtAmnt == parseFloat(0).toFixed(ValDigit))) {
              
                currentrow.find("#DebitAmountInBase").css("border-color", "red");
                currentrow.find("#Dbt_Amnt_Error").text($("#valueReq").text());
                currentrow.find("#Dbt_Amnt_Error").css("display", "block");
                currentrow.find("#CreditAmountInBase").css("border-color", "red");
                currentrow.find("#Crdt_Amnt_Error").text($("#valueReq").text());
                currentrow.find("#Crdt_Amnt_Error").css("display", "block");
                ErrorFlag = "Y";
            }
            if (Jv_Narrat == "") {
                currentrow.find("#Narration").css("border-color", "red");
                currentrow.find("#Narr_Error").text($("#valueReq").text());
                currentrow.find("#Narr_Error").css("display", "block");
                ErrorFlag = "Y";
            }
            else {
                currentrow.find("#Narration").css("border-color", "#ced4da");
                currentrow.find("#Narr_Error").text("");
                currentrow.find("#Narr_Error").css("display", "none");
            }
         
          });
    }
    else {
        swal("", $("#NoGLAccountSelected").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function OnchangeNarr(e) {

    var currentrow = $(e.target).closest('tr');
    var Narrat = currentrow.find("#Narration").val();
    if (Narrat == "") {
        currentrow.find("#Narration").css("border-color", "red");
        currentrow.find("#Narr_Error").text($("#valueReq").text());
        currentrow.find("#Narr_Error").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        currentrow.find("#Narration").css("border-color", "#ced4da");
        currentrow.find("#Narr_Error").text("");
        currentrow.find("#Narr_Error").css("display", "none");
    }
}

//-------------Validation end---------------//

//------Work For Narration Text Field ------//
 //--------- FOR Value of text Field(For Narration Text Field) insert on AddNewRow Account Button, Value Gives Between TEXTAREA as ${$("#txtJvNarrat").val()}---------//
//-------Narration Text field End-----//

//--------Work for Debit Credit Amount in Gl Details Table------------//
//----Validation on debit credit amount on onchange also---//

function AmountFloatVal(el, evt) {
    //debugger;
   
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        OnKeyupDebtAmnt(evt);
        OnKeyupCrdtAmnt(evt)
        return false;
    }
}
function OnChangeDebitAmount(e) {
    debugger;
    var ValDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var debitAmt12 = currentrow.find("#DebitAmountInBase").val();
    var debitAmt = cmn_ReplaceCommas(debitAmt12);
    var hfAccID = currentrow.find('#hfAccID').val();
    if (AvoidDot(debitAmt) == false) {
        debitAmt = "";
        currentrow.find("#DebitAmountInBase").val("")
    }
    else {
       // var debitAmt2 = parseFloat(debitAmt).toFixed(ValDigit);
        var debitAmt2 = toDecimal(cmn_ReplaceCommas(debitAmt), ValDigit);
        var debitAmt1 = cmn_addCommas(debitAmt2);
        currentrow.find("#DebitAmountInBase").val(debitAmt1);
    }
    if (debitAmt != "") {
        var AccgrpVal = currentrow.find("#hdnAccGrpval3_4").val();
        //if (AccgrpVal == "3" || AccgrpVal == "4") {
        if (AccgrpVal == "4") {
            currentrow.find("#CreditAmountInBase").attr("disabled", true).css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#BtnCostCenterDetail").attr("disabled", false);
        }
        else {
            currentrow.find("#CreditAmountInBase").attr("disabled", true).css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#BtnCostCenterDetail").attr("disabled", true);
            if (hfAccID != "") {
                deleteCCdetail(hfAccID);
            }
        }
    }
    else {
        currentrow.find("#CreditAmountInBase").attr("disabled", false).css("border-color", "#ced4da");
        currentrow.find("#Crdt_Amnt_Error").text("").css("display", "none");
        currentrow.find("#BtnCostCenterDetail").attr("disabled", true);
        if (hfAccID != "") {
            deleteCCdetail(hfAccID);
        }
    }
    CalculateTotalDrCrValue();
}

function OnChangeCreditAmount(e) {
    debugger;
    var ValDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var creditAmt12 = currentrow.find("#CreditAmountInBase").val();
    var creditAmt = cmn_ReplaceCommas(creditAmt12);
    var hfAccID = currentrow.find('#hfAccID').val();
    if (AvoidDot(creditAmt) == false) {
        creditAmt = "";
        currentrow.find("#CreditAmountInBase").val("")
    }
    else {
       // var creditAmt2 = parseFloat(creditAmt).toFixed(ValDigit);
        var creditAmt2 = toDecimal(cmn_ReplaceCommas(creditAmt), ValDigit);
        var creditAmt1 = cmn_addCommas(creditAmt2);
        currentrow.find("#CreditAmountInBase").val(creditAmt1);
    }
    if (creditAmt != "") {
        var AccgrpVal = currentrow.find("#hdnAccGrpval3_4").val();
        //if (AccgrpVal == "3" || AccgrpVal == "4") {
        if (AccgrpVal == "3") {
            currentrow.find("#DebitAmountInBase").attr("disabled", true).css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#BtnCostCenterDetail").attr("disabled", false);
        }
        else {
            currentrow.find("#DebitAmountInBase").attr("disabled", true).css("border-color", "#ced4da");
            currentrow.find("#Dbt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
            currentrow.find("#Crdt_Amnt_Error").text("").css("display", "none");
            currentrow.find("#BtnCostCenterDetail").attr("disabled", true);
            if (hfAccID != "") {
                deleteCCdetail(hfAccID);
            }
            //currentrow.find("#hdn_txtGlAccount").val(null);
            //currentrow.find("#txtGlAccount").val(null);
            //$("#tbladdhidden > tbody > tr #GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').remove();
        }
    }
    else {
        currentrow.find("#DebitAmountInBase").attr("disabled", false);
        currentrow.find("#BtnCostCenterDetail").attr("disabled", true);
        if (hfAccID != "") {
            deleteCCdetail(hfAccID);
        }
    }
    CalculateTotalDrCrValue();
}

function OnKeyupDebtAmnt(e) {
    //debugger
    var currentrow = $(e.target).closest('tr');
    var debitAmt = currentrow.find("#DebitAmountInSpecific").val();
    if (debitAmt != "") {
        currentrow.find("#DebitAmountInBase").css("border-color", "#ced4da");
        currentrow.find("#DebitAmountInSpecific").css("border-color", "#ced4da");
        currentrow.find("#Dbt_Amnt_Error").text("");
        currentrow.find("#Dbt_Amnt_Error").css("display", "none");
    }
}
function OnKeyupCrdtAmnt(e) {

    var currentrow = $(e.target).closest('tr');
    var creditAmt = currentrow.find("#CreditAmountInSpecific").val();
    if (creditAmt != "") {
        currentrow.find("#CreditAmountInBase").css("border-color", "#ced4da");
        currentrow.find("#CreditAmountInSpecific").css("border-color", "#ced4da");
        currentrow.find("#Crdt_Amnt_Error").text("");
        currentrow.find("#Crdt_Amnt_Error").css("display", "none");
    }
}
//-------Insert Details-------//

function CalculateTotalDrCrValue() {
    var debitAmount = 0;
    var creditAmount = 0;
   var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    $("#JVouAccDetailsTbl tbody tr").each(function () {
        var currentrow = $(this);
        debugger;
        var DbtAmnt1 = currentrow.find("#DebitAmountInBase").val();
        var DbtAmnt = cmn_ReplaceCommas(DbtAmnt1);
        var CrdtAmnt1 = currentrow.find("#CreditAmountInBase").val();
        var CrdtAmnt = cmn_ReplaceCommas(CrdtAmnt1);
      if (AvoidDot(DbtAmnt) == false) {
            DbtAmnt = 0;
        }
        if (AvoidDot(CrdtAmnt) == false) {
            CrdtAmnt = 0;
        }
        debitAmount = parseFloat(parseFloat(debitAmount).toFixed(ValDigit)) + parseFloat(parseFloat(DbtAmnt).toFixed(ValDigit));
        creditAmount = parseFloat(parseFloat(creditAmount).toFixed(ValDigit)) + parseFloat(parseFloat(CrdtAmnt).toFixed(ValDigit));
    })
   // var debitAmount2 = parseFloat(debitAmount).toFixed(ValDigit);
    var debitAmount2 = toDecimal(cmn_ReplaceCommas(debitAmount), ValDigit);
    var debitAmount1 = cmn_addCommas(debitAmount2);
    $("#dbtTtlAmnt").text(debitAmount1);
   // var creditAmount2 = parseFloat(creditAmount).toFixed(ValDigit);
    var creditAmount2 = toDecimal(cmn_ReplaceCommas(creditAmount), ValDigit);
    var creditAmount1 = cmn_addCommas(creditAmount2);
    $("#crdtTtlAmnt").text(creditAmount1);
    debugger;
    var Dbt = "Dr";
    var Crdt = "Cr";
   if (debitAmount > creditAmount) {
       var difference = toDecimal(debitAmount, ValDigit) - toDecimal(creditAmount, ValDigit);
       $("#spanamttype").text(Dbt);

      // var difference2 = parseFloat(difference).toFixed(ValDigit);
       var difference2 = toDecimal(cmn_ReplaceCommas(difference), ValDigit);
       var difference1 = cmn_addCommas(difference2);
       $("#DiffrncAmnt").text(difference1);
     
     }
    else {
       var difference = toDecimal(debitAmount, ValDigit) - toDecimal(creditAmount, ValDigit);
       //if (parseFloat(difference).toFixed(ValDigit) == parseFloat(0).toFixed(ValDigit)) {
       if (toDecimal(cmn_ReplaceCommas(difference), ValDigit) == parseFloat(0).toFixed(ValDigit)) {
           $("#spanamttype").text("");
       }
       else {
           $("#spanamttype").text(Crdt);
       }
       //var difference2 = parseFloat(difference).toFixed(ValDigit);
       var difference2 = toDecimal(cmn_ReplaceCommas(difference), ValDigit)
       var difference1 = cmn_addCommas(difference2);
       $("#DiffrncAmnt").text(difference1);
     }
   if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function SaveInsertJVDetail() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    if (AccountValidation() == false) {
        return false;
    }

    if ($("#CancelFlag").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            return false;
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

    }

    var FinalAccountDetail = [];
    FinalAccountDetail = InsertJVAccountDetails();
    var AccountDt = JSON.stringify(FinalAccountDetail);
    $('#hdAccountDetailList').val(AccountDt);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);

    CalculateTotalDrCrValue();
    if (saveDrCrEqualAmnt() == false) {
        return false;
    }
    if (Cmn_CC_DtlSaveButtonClick("JVouAccDetailsTbl", "DebitAmountInBase", "CreditAmountInBase","StatusCode") == false) {
        return false;
    }

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    /*----- Attatchment End--------*/
    /*----- Attatchment start--------*/
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
            /*----- Attatchment End--------*/
    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
return true;
   
};
function saveDrCrEqualAmnt() {
    debugger;
    var ErrorFlag = "N";
    var ValDigit = $("#ValDigit").text();
    var dbTotlAmnt = $("#dbtTtlAmnt").text();
    var crTotAmnt = $("#crdtTtlAmnt").text();
    debugger;
    if (dbTotlAmnt == crTotAmnt) {
            var difference = 0;
            var Dbt = "";
            $("#spanamttype").text(Dbt);
          //  $("#DiffrncAmnt").text(parseFloat(difference).toFixed(ValDigit));
        
        $("#DiffrncAmnt").text(toDecimal(cmn_ReplaceCommas(difference), ValDigit));
        }
        else {
            swal("", $("#DebtCredtAmntMismatch").text(), "warning");
            ErrorFlag = "Y";
        }
   var vouamnt = $("#dbtTtlAmnt").text();
   // $("#hdnVouAmnt").val(parseFloat(vouamnt).toFixed(ValDigit));
    $("#hdnVouAmnt").val(toDecimal(cmn_ReplaceCommas(vouamnt), ValDigit));
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

function InsertJVAccountDetails() {
    debugger;
    var AccountDetailList = new Array();
    $("#JVouAccDetailsTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var SpanRowId = currentRow.find("#SpanRowId").text();
        var AccountList = {};
        /*commented by Hina on 01-07-2024 to seprate acc_id and SpanRowId as seq_no */
        //AccountList.acc_id = (currentRow.find("#GLAccount_" + rowid).val()) + '_' + Spanrowid;
        AccountList.acc_id = currentRow.find("#GLAccount_" + rowid).val();
        AccountList.acc_name = currentRow.find("#GLAccount_" + rowid + " option:selected").text();
        AccountList.acc_group_name = currentRow.find("#GLGroup").val();
        AccountList.acc_grp_id = currentRow.find("#hdGLGroupID").val();
        AccountList.acc_type = currentRow.find("#hdAccType").val();
        AccountList.curr_id = currentRow.find("#hdCurrId").val();
        AccountList.conv_rate = currentRow.find("#hdConvsRateId").val();
        //AccountList.dr_amt_bs = currentRow.find("#DebitAmountInBase").val();
        var DebitAmountInBase = currentRow.find("#DebitAmountInBase").val();
        AccountList.dr_amt_bs = cmn_ReplaceCommas(DebitAmountInBase);
        AccountList.dr_amt_sp = cmn_ReplaceCommas(DebitAmountInBase);

        //AccountList.cr_amt_bs = currentRow.find("#CreditAmountInBase").val();
        var CreditAmountInBase = currentRow.find("#CreditAmountInBase").val();
        AccountList.cr_amt_bs = cmn_ReplaceCommas(CreditAmountInBase);
        AccountList.cr_amt_sp = cmn_ReplaceCommas(CreditAmountInBase);
        //AccountList.cr_amt_sp = currentRow.find("#CreditAmountInBase").val();
        AccountList.narr = currentRow.find("#Narration").val();
        AccountList.seq_no = SpanRowId;
        AccountDetailList.push(AccountList);   
    });
    return AccountDetailList;
};

//-----------For List Page--------------//
function BtnSearch() {
    debugger;
    FilterPRList();
    ResetWF_Level();
}
function FilterPRList() {
    debugger;
    try {
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/JournalVoucher/SearchJVDetail",
            data: {
               
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#JvListtbl').html(data);
               /* $("#datatable-buttons_wrapper a.btn.btn-default.buttons-csv.buttons-html5.btn-sm").remove();*/
                //$("#datatable-buttons_wrapper .dt-buttons").append('<button class="btn btn-default buttons-csv buttons-html5 btn-sm Csv" id="" onclick="JournalVoucherCSV()"  tabindex="0" aria-controls="datatable-buttons5" href="#"><span>CSV</span></button>')
                $('#ListFilterData').val(Fromdate + ',' + Todate + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    }
    catch (err) {
        debugger;
        console.log("MRS Error : " + err.message);

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

//------Delete Jv Detail-----------//
function functionConfirm(event) {
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}

//-------Cancel and WorkFlow work-------//
function OnClickCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveInsertJVDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#CancelFlag", "Enable");
}
function onchangeCancelledRemarks() {
    debugger;
    //var remrks = $("#Cancelledremarks").attr("title");
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
function ForwardBtnClick() {
    debugger;
    //var JVStatus = "";
    //JVStatus = $('#StatusCode').val().trim();
    //if (JVStatus === "D" || JVStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");
    //    }
    //    var Doc_ID = $("#DocumentMenuId").val();
    //    $("#OKBtn_FW").attr("data-dismiss", "modal");

    //    Cmn_GetForwarderList(Doc_ID);

    //}
    //else {
    //$("#Btn_Approve").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;
    /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
    //$("#Btn_Approve").attr("data-target", "");
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var Voudt = $("#txtJVDate").val();
        $.ajax({
            type: "POST",
            url: "/Common/Common/Fin_CheckFinancialYear",
            data: {
                compId: compId,
                brId: brId,
                Voudt: Voudt
            },
            success: function (data) {
                if (data == "FY Exist") { /*End to chk Financial year exist or not*/
                    var JVStatus = "";
                    JVStatus = $('#StatusCode').val().trim();
                    if (JVStatus === "D" || JVStatus === "F") {

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
                else {/* to chk Financial year exist or not*/
                    if (data == "FY Not Exist") {
                        swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                        //$("#Btn_Forward").attr("data-target", "");
                        $("#Forward_Pop").attr("data-target", "");
                        $("#Btn_Approve").attr("data-target", "");
                    }
                    else {
                        swal("", $("#BooksAreClosedEntryCanNotBeMadeInThisFinancialYear").text(), "warning");
                        /* $("#Btn_Forward").attr("data-target", "");*/
                        $("#Forward_Pop").attr("data-target", "");
                        $("#Btn_Approve").attr("data-target", "");
                    }

                }
            }
        });
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
    /*End to chk Financial year exist or not*/
    return false;
}
//check on PartialForward of this function
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var JVNo = "";
    var JVDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    JVNo = $("#JVNumber").val();
    JVDate = $("#txtJVDate").val();
    docid = $("#DocumentMenuId").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (JVNo + ',' + JVDate + ',' + WF_Status1);
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "JournalVoucher_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(JVNo, JVDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/JournalVoucher/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && JVNo != "" && JVDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(JVNo, JVDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/JournalVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/JournalVoucher/ApproveJOVDetails?JVNo=" + JVNo + "&JVDate=" + JVDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && JVNo != "" && JVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(JVNo, JVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/JournalVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && JVNo != "" && JVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(JVNo, JVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/JournalVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(JVNo, JVDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/JournalVoucher/SavePdfDocToSendOnEmailAlert",
//        data: { JVNo: JVNo, JVDate: JVDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#JVNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function OtherFunctions(StatusC, StatusName) {

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
function VoucherDateValidation() {
    debugger;
Cmn_VoucherDateValidation("#txtJVDate");

}

// ----------Cost Center Section Start ------------//

function OnclickCcBtn(flag,e) {
    debugger;
    var TotalAmt1 = 0;//add by sm 04-12-2024
    var Doc_ID = $("#DocumentMenuId").val();//add by sm 04-12-2024
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = clickedrow.find("#DebitAmountInBase").val();
    var CstCrtAmt = clickedrow.find("#CreditAmountInBase").val();
    var SpanRowId = clickedrow.find("#SpanRowId").text();
    var GLAcc_Name = clickedrow.find('option:selected').text();
    var GLAcc_id = clickedrow.find('option:selected').val();
    var NewArr = new Array();
    var disableflag = ($("#txtdisable").val());
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        var List = {};
        List.GlAccount = row.find("#hdntbl_GlAccountId").text();
        List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
        List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
        List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
        List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
        var amount = row.find("#hdntbl_CstAmt").text();
       // List.CC_Amount = parseFloat(amount).toFixed(ValDigit);
        var amt = toDecimal(cmn_ReplaceCommas(amount), ValDigit);
        var amt1 = cmn_addCommas(amt);
        List.CC_Amount = amt1;
        //List.CC_Amount = toDecimal(cmn_ReplaceCommas(amount), ValDigit)
        TotalAmt1 = parseFloat(TotalAmt1) + parseFloat(amt);//add by sm 04-12-2024
        NewArr.push(List);
    })
    var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));//add by sm 06-12-2024
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
       Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt; 
    }
                 $.ajax({
                type: "POST",
                url: "/ApplicationLayer/JournalVoucher/GetCstCntrtype",
                     data: {
                         Flag: flag,
                         Disableflag: disableflag,
                         CC_rowdata: JSON.stringify(NewArr),
                         TotalAmt: TotalAmt,//add by sm 06-12-2024
                         Doc_ID: Doc_ID//add by sm 06-12-2024        
                     },
                     success: function (data) {
                         debugger;
                        $("#CostCenterDetailPopup").html(data);
                        $("#CC_GLAccount").val(GLAcc_Name);
                        $("#hdnGLAccount_Id").val(GLAcc_id);
                        $("#GLAmount").val(Amt);
                        $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
                        $("#hdnTable_Id").text("JVouAccDetailsTbl");
                     },
                 })
}

// ----------Cost Center Section END ------------//

//--------On Click Icon Button Voucher Details ------//

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");

    var vou_no = clickedrow.find("#Jv_Vouno").text();
    var voudt = clickedrow.find("#Jv_Date").text();
    var vou_dt = clickedrow.find("#vou_date").text();
    //var narr = clickedrow.find("#td_narr").text();

    var vochinfo = vou_no.split("_");
    if (vochinfo.length > 1) {
        var cflag = vochinfo[1];
        vou_no = vochinfo[0]
    }

    Cmn_GetVouDetails(vou_no, voudt, vou_dt, cflag, '');
}

function OnclickCCIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var vou_no = $("#GLVoucherNo").val();
    var vou_dt = $("#mwhdn_voudt").val();
    var acc_id = clickedrow.find("#hdn_accid").val();
    var acc_name = clickedrow.find("#spn_glname").text();
    //var dr_amt = clickedrow.find("#spn_dramt").text();/*Comment by Hina on 03-08-2024 to remove comma from amount*/
    //var cr_amt = clickedrow.find("#spn_cramt").text();
    var debit_amt = clickedrow.find("#spn_dramt").text();
    var credit_amt = clickedrow.find("#spn_cramt").text();
    var dr_amt = cmn_ReplaceCommas(debit_amt); /*Add start by Hina on 03-08-2024 to remove comma from amount*/
    var cr_amt = cmn_ReplaceCommas(credit_amt); /*Add start by Hina on 03-08-2024 to remove comma from amount*/
    var amt = 0;
    if (dr_amt > 0) {
        amt = dr_amt;
    }
    if (cr_amt > 0) {
        amt = cr_amt;
    }
    Cmn_GetcostcenterDetails(vou_no, vou_dt, acc_id, acc_name, amt);
}

//--------Voucher Details End------//

function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickApprove") {
        $("#btn_approve").attr("disabled", true);
        $("#btn_approve").css("filter", "grayscale(100%)");
    }
    else {
        $("#btn_approve").css("filter", "grayscale(100%)");
        $("#hdnsavebtn").val("AllreadyclickApprove");
    }

}