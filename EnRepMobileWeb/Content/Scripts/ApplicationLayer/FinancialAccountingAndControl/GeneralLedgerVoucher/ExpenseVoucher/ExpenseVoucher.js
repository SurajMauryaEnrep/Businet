$(document).ready(function () {
    
    $("#ddlPayeeGlList").select2();
       
    $("#hdDoc_No").val($("#Vou_no").val())
    bindVoucher();
    CancelledRemarks("#CancelFlag", "Disabled");
});
var ValDigit = $("#ValDigit").text();
//var ValDigit = parseFloat(0).toFixed(ValDig);
function onchangeCancelledRemarks() {
    
    //var remrks = $("#Cancelledremarks").attr("title");
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}
function OnchangePayeeGlAccount() {
    
    var RowNo = 0;
    $("#TblPaymentDetail >tbody >tr").each(function (i, row) {
        
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $("#TotalPayment").text("");
    $("#PD_PlusIcon").show();
    $("#TblPaymentDetail > tbody > tr").remove();
    $('[aria-labelledby="select2-ddlPayeeGlList-container"]').css("border-color", "#ced4da");
    $("#spn_PayeeGllst").css("display", "none");
    BindAccounBalance();

    fun_GLPosting();
}

function BindAccounBalance() {
    
    var Acc_id = $("#ddlPayeeGlList option:selected").val();
    var Vou_Dt = $("#Exp_Date").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ExpenseVoucher/GetVoucherDtAndAmt",
        data: {
            acc_id: Acc_id, Vou_Dt: Vou_Dt
        },
        success: function (data) {
            
            var arr = [];
            arr = JSON.parse(data);
            if (arr.length > 0 && arr.length != null) {
                $("#AccountBalance").val(cmn_addCommas(arr[0].ClosBL));
            }
        }
    })
}
function VoucherDateValidation() {/*Add by Hina Sharma on 25-03-2025*/
    
    Cmn_VoucherDateValidation("#Exp_Date");

}
/*---- PAYMENT DETAIL SECTION-----*/

function AddRow(rowCount, RowNo) {
    
    var ValDig = $("#ValDigit").text();
    var ValDigit = parseFloat(0).toFixed(ValDig);
    //var ValDigit = $("#ValDigit").val();
    $("#TblPaymentDetail tbody").append(
        `<tr><td align="left" valign="top" class="red center bom_width_td"> <i class="fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}" onclick="onclickPDDeleteIcon(event)"></i></td>
            <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
            <td><div class="lpo_form">
            <select class="form-control" id="ddlVouchernumber_${RowNo}" name="" onchange="OnchangeVoucherNumber(event)"></select>
            <input id="hdnVoucherNumber" type="hidden" />
            <span id="spn_Voulst" class="error-message is-visible"></span></div>
            </td>
            <td>
                <input autocomplete="off" class="form-control" id="ExpVoucherDate" name="VoucherDate" placeholder="${$("#span_PaymentDate").text()}" type="text" disabled >
                <input id="hdn_expdt" type="hidden" />
            </td>
            <td>
                <input autocomplete="off" class="form-control num_right" id="ExVouAmount" name="ExVouAmount" placeholder="0000.00" type="text" disabled >
            </td>
            <td><input autocomplete="off" class="form-control num_right" id="unadj_amt" name="" placeholder="0000.00" type="text" value="" disabled></td>
            <td><input autocomplete="off" class="form-control num_right" id="" name="" placeholder="0000.00" type="text" value="" disabled></td>
            <td><input autocomplete="off" class="form-control num_right" id="" name="" placeholder="0000.00" type="text" value="" disabled></td>
            <td><textarea id="txt_narr" class="form-control remarksmessage" autocomplete="off" type="text" value="" name="" maxlength="200" placeholder="${$("#span_Narration").text()}" disabled></textarea></td>

            <td style="display: none;"><input type="hidden" id="SNohf" value="${RowNo}" /></td>
        </tr>`
    )
    //BindVoucherList(RowNo);
}
function onclickPDAddBtn() {
    
    var rowCount = $('#TblPaymentDetail >tbody >tr').length + 1
    var RowNo = 0;
    if (RowNo == "0") {
        RowNo = 1;
    }
    if (rowCount == "0") {
        rowCount = 1;
    }
    $("#TblPaymentDetail >tbody >tr").each(function (i, row) {
        
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf").val()) + 1;
    });
    if (RowNo >= 1) {
        AddRow(rowCount, RowNo);
    }
    
    BindVoucherList3('#ddlVouchernumber_', RowNo, '#TblPaymentDetail', '#SNohf', "addNew");
    //bindVoucher()
}
function bindVoucher() {
    //var RowNo = 0;
    //if (RowNo == "0") {
    //    RowNo = 1;
    //}
    $("#TblPaymentDetail >tbody >tr").each(function () {
        var currentRow = $(this);
        var RowNo = currentRow.find("#SpanRowId").text();
        BindVoucherList3('#ddlVouchernumber_', RowNo, '#TblPaymentDetail', '#SNohf',"PageLoad");

    });
}
async function BindVoucherList3(VouLstDDLName, RowID, TableID, SnoHiddenField,Flag) {
    
    var Acc_id = $("#ddlPayeeGlList option:selected").val();
    if (Flag == "addNew") {
        var s = '<option value="0">---Select---</option>';
        $(VouLstDDLName + RowID).html(s);
    } 
    $(VouLstDDLName + RowID).select2({
        ajax: {
            url: "/ApplicationLayer/ExpenseVoucher/GetVoucherList1",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    acc_id: Acc_id,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
                var pageSize,
                    pageSize = 20;//50000; // or whatever pagesize
                data = JSON.parse(data).Table;
                var arr = [];
                $("#TblPaymentDetail >tbody >tr").each(function () {
                    var currentRow = $(this);
                    var rowid = currentRow.find("#SpanRowId").text();
                    var Vouchernumber = currentRow.find("#ddlVouchernumber_" + rowid).val();
                    //var glBrId = currentRow.find("#GLbranch_" + rowid).val();
                    if ($(VouLstDDLName + RowID).val() != Vouchernumber) {
                        arr.push({ vou_no: Vouchernumber });
                    }
                });
                data = data.filter(v => !arr.filter(j => j.vou_no == v.vou_no).length > 0)
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options">
<li class="select2-results__option"><strong class="">
<div class="row">
<div class="col-md-8 col-xs-8 def-cursor">${$("#DocNo").text()}</div>
<div class="col-md-4 col-xs-4 def-cursor">${$("#DocDate").text()}</div>
</div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.vou_no, text: val.vou_no, voucher_dt: val.voucher_dt };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            },
            cache: true
        },
        templateResult: function (data) {

            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-8 col-xs-8' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-4 col-xs-4' + classAttr + '">' + IsNull(data.voucher_dt, '') + '</div>' +             
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function BindVoucherList(VouLstDDLName, RowID, TableID, SnoHiddenField) {
    
    var Acc_id = $("#ddlPayeeGlList option:selected").val();
    var s = '<option value="0">---Select---</option>';
    $(VouLstDDLName + RowID).html(s);
    $(VouLstDDLName + RowID).select2({
        ajax: {
            url: "/ApplicationLayer/ExpenseVoucher/GetVoucherList",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    acc_id: Acc_id,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                
                var pageSize,
                    pageSize = 20;
                var ItemListArrey = [];
                ItemListArrey= ConvertEVArreyList(TableID, VouLstDDLName, SnoHiddenField);
                let selected = [];
                selected.push({ id: $(VouLstDDLName + RowID).val() });
                selected = JSON.stringify(selected);
                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));
                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.vou_no));
                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].vou_no.trim() != "---Select---") {
                            var select = { ID: "0", vou_no: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }
                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID, text: val.vou_no };
                    }),
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            },
            cache: true
        },
        
    });

}
function ConvertEVArreyList(TableID, VouLstDDLName, SnoHiddenField,) {
    
    let array = [];
    $(TableID + " tbody tr").each(function () {
        
        var currentRow = $(this);
        
        var rowno = currentRow.find(SnoHiddenField).val();
        var itemId = "";
       
        itemId = currentRow.find(VouLstDDLName + rowno).val();
        
        if (itemId != "0") {
            array.push({ id: itemId });
        }
    });

    return array;
}
function OnchangeVoucherNumber(e) {
    
    var CurrentRow = $(e.target).closest('tr');
    var Sno = CurrentRow.find("#SNohf").val();
    var VoucherNum = CurrentRow.find('#ddlVouchernumber_' + Sno + ' option:selected').text();
    /*$('#ddlVouchernumber_' + Sno+'').prop('disabled', true);*/
    CurrentRow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    CurrentRow.find("#spn_Voulst").css("display", "none");
    CurrentRow.find("#hdnVoucherNumber").val(VoucherNum);
    onChangeVouNum(CurrentRow, VoucherNum);
    $("#ED_PlusIcon").show();

}

function onChangeVouNum(CurrentRow, VoucherNum) {
    
    var Acc_id = $("#ddlPayeeGlList option:selected").val();
    var Vou_Dt = $("#Exp_Date").val();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ExpenseVoucher/Get_VoucherDetails",
        data: {
            acc_id: Acc_id, Vou_no: VoucherNum, Vou_Dt: Vou_Dt
        },
        success: function (data) {
            
            var arr = [];
            arr = JSON.parse(data);
            if (arr.length > 0) {
                CurrentRow.find("#ExpVoucherDate").val(arr[0].vou_dt);
                CurrentRow.find("#hdn_expdt").val(arr[0].voudt);
                CurrentRow.find("#ExVouAmount").val(cmn_addCommas(arr[0].vouamt));
                CurrentRow.find("#unadj_amt").val(cmn_addCommas(arr[0].UnAdjAmt));
                CurrentRow.find("#txt_narr").val(arr[0].narr);
                var TotalPayment = parseFloat(CalculateTotalAmt('TblPaymentDetail', 'ExVouAmount')).toFixed(ValDigit)
                $("#TotalPayment").text(cmn_addCommas(TotalPayment));
                //$("#TotalPayment").text(parseFloat(CalculateTotalAmt('TblPaymentDetail', 'ExVouAmount')).toFixed(ValDigit));
                var TotalUnAdjAmount = parseFloat(CalculateTotalAmt('TblPaymentDetail', 'unadj_amt')).toFixed(ValDigit);
                $("#TotalUnAdjAmount").text(cmn_addCommas(TotalUnAdjAmount));
                //$("#TotalUnAdjAmount").text(parseFloat(CalculateTotalAmt('TblPaymentDetail', 'unadj_amt')).toFixed(ValDigit));
            }
        }
    })
}

function onclickPDDeleteIcon(e) {
    
    /*var CurrentRow = $(e.target).closest('tr');*/
    /*var SpanRowId = CurrentRow.find("#SpanRowId").text();*/
   /* var VouNo = CurrentRow.find('#hdnVoucherNumber').val();*/
    /*$("#ddlVouchernumber_").append(`<option value="">${VouNo}</option>`)*/
    $(e.target).closest('tr').remove();
    if ($("#TblPaymentDetail > tbody > tr").length == 0) {
        $("#ddlPayeeGlList").prop("disabled", false);
        $("#ddlPayeeGlList").attr("onchange", "");
        $("#ddlPayeeGlList").trigger('change.select2');
        $("#ddlPayeeGlList").attr("onchange", "OnchangePayeeGlAccount()");
        $("#TotalPayment").text("");
        $("#ExpDescTotal").text("");
        /*$("#PD_PlusIcon").hide();*/
        $("#ED_PlusIcon").hide();
        $("#AccountBalance").val("");
        $("#TblExpesnseDesc > tbody > tr").remove();
    }
    else {
        $("#TotalPayment").text(CalculateTotalAmt('TblPaymentDetail', 'ExVouAmount'));
    }
    updateSerialNumber('TblPaymentDetail');
}
/*---- PAYMENT DETAIL SECTION END-----*/

function CalculateTotalAmt(tblId,TxtBoxId) {
    
    var Amt = 0;
    $('#' + tblId + ' > tbody > tr').each(function () {
        
        var CurrentRow = $(this);
        var VouAmt = cmn_ReplaceCommas(CurrentRow.find('#' + TxtBoxId + '').val());
        if (VouAmt == "" || isNaN(VouAmt)) {
            VouAmt = parseFloat(0).toFixed(ValDigit);
        }
        Amt = parseFloat(VouAmt) + parseFloat(Amt);
    })
    return Amt;
}
function updateSerialNumber(TblId) {
    
    var SerialNo = 0;
    $('#' + TblId +' >tbody >tr').each(function (i, row) {
        
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};

/*---- EXPENSE DESCRIPTION SECTION -----*/
function AddNewExpenseDes() {
    
    var ValDig = $("#ValDigit").text();
    var ValDigit = parseFloat(0).toFixed(ValDig);
    var rowCount = $('#TblExpesnseDesc >tbody >tr').length + 1
    var RowNo = 0;
    $("#TblExpesnseDesc > tbody >tr").each(function (i, row) {
        
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohf1").val()) + 1;
    });
    if (RowNo == "0") {
        RowNo = 1;
    }
    $("#TblExpesnseDesc tbody").append(
        `<tr>
        <td align="left" valign="top" class="red center bom_width_td"> <i class="fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}" onclick="onclickEXPDESCDeleteIcon(event)"></i></td>
        <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
        <td><div class="lpo_form">
            <input autocomplete="off" class="form-control" id="ExpDescription" Maxlength="200" name="" placeholder="${$("#span_ExpenseDescription").text()}" type="text" value="" onchange="onchangeExpDesc(this,event)">
            <span id="spn_SuppName" class="error-message is-visible"></span></div>
         </td>
        <td><div class="lpo_form">
            <select class="form-control" id="ddlExpenseGlAccount_${RowNo}" name="" onchange="onchangeExpenseGlAcc(this,event)"></select>
            <span id="spn_ExpenseGlAcc" class="error-message is-visible"></span></div>
        </td>
        <td>
             <input autocomplete="off" disabled class="form-control remarksmessage" id="gl_group" Maxlength="200" placeholder="${$("#AccGroup").text()}" type="text" value="">
        </td>
        
       <td><div class="lpo_form">
           <input autocomplete="off" class="form-control num_right" id="ExpDescBillNumber" Maxlength="25" name="" placeholder="${$("#span_BillNumber").text()}" type="text" value="" onchange="onchangeBillNumber(this,event)">
           <span id="spn_BillNum" class="error-message is-visible"></span></div>
            </td>
           <td><div class="lpo_form">
       <input value="" autocomplete="off" class="form-control" id="ExpDescBillDate" name="ExpDescBillDate" placeholder="${$("#span_VoucherDate").text()}" type="date"  onchange="onchangeBillDate(this,event)">
    <input type="hidden" id="hdnBillDate" />
    <span id="spn_Billdate" class="error-message is-visible"></span></div>
             </td>
             <td><div class="lpo_form">
              <input autocomplete="off" class="form-control num_right" id="ExpDescAmount" name="" onchange="onchangeExpDescAmt(this,event)" onkeypress="return AmountFloatVal(this, event)" placeholder="0000.00" type="text" value="">
              <span id="spn_ExpDescAmt" class="error-message is-visible"></span></div>
                </td>
              
                <td style="display: none;"><input type="hidden" id="SNohf1" value="${RowNo}" /></td>
   </tr>`
    )
    BindExpenseAcc('#ddlExpenseGlAccount_', RowNo, '#TblExpesnseDesc', '#SNohf1');
}
function BindExpenseAcc(AccLstDDLName, RowID, TableID, SnoHiddenField) {
    
    var s = '<option value="0">---Select---</option>';
    $(AccLstDDLName + RowID).html(s);
    $(AccLstDDLName + RowID).select2({

        ajax: {
            url: "/ApplicationLayer/ExpenseVoucher/GetExpenseGlAccount",
            data: function (params) {
                var queryParameters = {
                    SearchName: params.term,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                
                //debugger
                var pageSize,
                    pageSize = 20;
                var ItemListArrey = [];
                ItemListArrey = ConvertEVArreyList(TableID, AccLstDDLName, SnoHiddenField);

                let selected = [];
                selected.push({ id: $(AccLstDDLName + RowID).val() });
                selected = JSON.stringify(selected);

                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));

                /*data = data.filter(j => !JSON.stringify(NewArrey).includes(j.acc_id));*/


                var page = params.page || 1;
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].acc_id.trim() != "---Select---") {
                            var select = { acc_id: "0", acc_name: " ---Select---" };
                            Fdata.unshift(select);
                        }
                    }
                }

                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.acc_id, text: val.acc_name };
                    }),
                    
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };
            },
            cache: true
        },
        

    });
}
function onclickEXPDESCDeleteIcon(e) {
    
   /* var CurrentRow = $(e.target).closest('tr');*/
    $(e.target).closest('tr').remove();
    if ($("#TblExpesnseDesc > tbody > tr").length == 0) {
        /*$("#ddlPayeeGlList").prop("disabled", false);*/
        $("#TotalPayment").text("");
        $("#ExpDescTotal").text("");
    }
    else {
        $("#ExpDescTotal").text(CalculateTotalAmt('TblExpesnseDesc','ExpDescAmount'));
    }
    if ($("#TblPaymentDetail > tbody > tr").length == 0) {
        $("#ED_PlusIcon").hide();
    }
    updateSerialNumber('TblExpesnseDesc');

    fun_GLPosting();
}
function AmountFloatVal(el,evt) {
    
    if (Cmn_FloatValueonly(el, evt, "#ValDigit") == false) {
        return false;
    }
    
}
function onchangeExpDescAmt(e,el) {
    
    var CurrentRow = $(el.target).closest('tr');
    var Amt
    Amt1 = CurrentRow.find("#ExpDescAmount").val();
    var Amt = cmn_ReplaceCommas(Amt1);
    if (Amt == '') {
        Amt = '0'
    }
    var Amt2 = parseFloat(Amt).toFixed(ValDigit);
    var Amt3 = cmn_addCommas(Amt2);
    CurrentRow.find("#ExpDescAmount").val(Amt3);
    //CurrentRow.find("#ExpDescAmount").val(parseFloat(Amt).toFixed(ValDigit));
    var ExpDescTotal = parseFloat(CalculateTotalAmt('TblExpesnseDesc', 'ExpDescAmount')).toFixed(ValDigit);
    var ExpDescTotal1 = cmn_addCommas(ExpDescTotal);
    $("#ExpDescTotal").text(ExpDescTotal1);
    CurrentRow.find("#ExpDescAmount").css("border-color", "#ced4da");
    CurrentRow.find("#spn_ExpDescAmt").css("display", "none");

    fun_GLPosting();
}
function onchangeBillDate(e, el) {
    
    var CurrRow = $(el.target).closest('tr');
    var BillDate = CurrRow.find("#ExpDescBillDate").val();
    CurrRow.find("#hdnBillDate").val(BillDate);
    CurrRow.find("#ExpDescBillDate").css("border-color", "#ced4da");
    CurrRow.find("#spn_Billdate").css("display", "none");
}
function onchangeExpDesc(e,el) {
    
    var CurrRow = $(el.target).closest('tr');
    CurrRow.find("#ExpDescription").css("border-color", "#ced4da");
    CurrRow.find("#spn_SuppName").css("display", "none");
}
function onchangeBillNumber(e,el) {
    
    var CurrRow = $(el.target).closest('tr');
    CurrRow.find("#ExpDescBillNumber").css("border-color", "#ced4da");
    CurrRow.find("#spn_BillNum").css("display", "none");
}
function Bindgl_accgroup(acc_id, CurrentRow) {
    
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ExpenseVoucher/Getgl_accgroup",
        data: {
            acc_id: acc_id
        },
        success: function (data) {
            
            var arr = [];
            arr = JSON.parse(data);
            if (arr.length > 0 && arr.length != null) {
                CurrentRow.find("#gl_group").val(arr[0].AccGroupChildNood);
            }
        }
    })
}
/*---- EXPENSE DESCRIPTION SECTION END -----*/

/*-- Save Expense Voucher-----*/
function SaveInsertExpVouDetail() {
    

    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var PyeeAccID = $("#ddlPayeeGlList option:selected").val();
    $("#PyeeAccID").val(PyeeAccID);
    if (VouDetailValidate() == false) {
        return false;
    }
    if (PaymentDetailValidation() == false) {
        return false;
    }
    if (ExpenseDetailvalidate() == false) {
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
    var FinalPaymentDetail = [];
    FinalPaymentDetail = InsertExpPaymentDetails();
    var PayDetail = JSON.stringify(FinalPaymentDetail);
    $('#hdExpVouPaymentDetail').val(PayDetail);

    var FinalExpenseDetail = [];
    FinalExpenseDetail = InsertExpenseDetails();
    var ExpenseDetail = JSON.stringify(FinalExpenseDetail);
    $('#hdExpenseDetail').val(ExpenseDetail);

    var Final_VouDetail = [];
    Final_VouDetail = Get_VoucherDetails();
    var VouGlDt = JSON.stringify(Final_VouDetail);
    $('#hdVouGlDetailList').val(VouGlDt);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    /*----- Attatchment End--------*/
    /*----- Attatchment start--------*/
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/

    if (Cmn_CC_DtlSaveButtonClick("exp_voudetail", "dramt", "cramt", "DocumentStatus") == false) {
        return false;
    }


    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
    return true;
}

function InsertExpPaymentDetails() {
    
    var PaymentDet = new Array();
    $("#TblPaymentDetail > tbody > tr").each(function () {
        
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf").val();
        var PaymentLst = {};
        PaymentLst.PDVou_No = currentRow.find('#ddlVouchernumber_' + rowid + '').val();
        PaymentLst.PDVou_Dt = currentRow.find('#hdn_expdt').val();
        PaymentLst.PDVou_Amt = cmn_ReplaceCommas(currentRow.find('#ExVouAmount').val());
        PaymentDet.push(PaymentLst);
    })
    return PaymentDet;
}

function InsertExpenseDetails() {
    
    var ExpenseDetails = new Array();
    $("#TblExpesnseDesc > tbody > tr").each(function () {
        
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohf1").val();
        var ExpenseLst = {};
        ExpenseLst.ExpDesc = currentRow.find('#ExpDescription').val();
        ExpenseLst.Bill_No = currentRow.find('#ExpDescBillNumber').val();
        ExpenseLst.Bill_Date = currentRow.find('#ExpDescBillDate').val();
        ExpenseLst.EDAmount = cmn_ReplaceCommas(currentRow.find('#ExpDescAmount').val());
        ExpenseLst.EDacc_id = currentRow.find('#ddlExpenseGlAccount_' + rowid + '').val();  
        ExpenseDetails.push(ExpenseLst);
    })
    return ExpenseDetails;
}

function onchangeExpenseGlAcc(e,el) {
    
    var CurrentRow = $(el.target).closest('tr');
    CurrentRow.find(".select2-container--default .select2-selection--single").css("border", "1px solid #aaa");
    CurrentRow.find("#spn_ExpenseGlAcc").css("display", "none");
    
    var hdn_rid = CurrentRow.find("#SNohf1").val();
    var acc_id = CurrentRow.find("#ddlExpenseGlAccount_" + hdn_rid + " option:selected").val();
    var acc_name = CurrentRow.find("#ddlExpenseGlAccount_" + hdn_rid + " option:selected").text();
    var acc_amt = CurrentRow.find("#ExpDescAmount").val();

    /*fun_DeleteGLPostiing(acc_id);*/
    if (acc_id != "0" && acc_id != null) {
        fun_GLPosting();
        Bindgl_accgroup(acc_id, CurrentRow);
    }
}

function Get_VoucherDetails() {
    
    var VouList = [];
    var CustVal = 0;
    var Compid = $("#CompID").text();
    var _SIType = "D";
    var TransType = "Sal";
    if ($("#exp_voudetail >tbody >tr").length > 0) {
        $("#exp_voudetail >tbody >tr").each(function () {
            var currentRow = $(this);
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var Gltype = "";
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#span_acc_name").text();
            dr_amt = cmn_ReplaceCommas(currentRow.find("#dramt").text());
            cr_amt = cmn_ReplaceCommas(currentRow.find("#cramt").text());
            Gltype = "gl_acc";
            VouList.push({ comp_id: Compid, id: acc_id, acc_name: acc_name, type: "I", doctype: _SIType, Value: CustVal, DrAmt: dr_amt, CrAmt: cr_amt, TransType: TransType, Gltype: Gltype });

        });
    }
    return VouList;
};
/*--- Expense Voucher Validation---------*/
function VouDetailValidate() {
    
    var ErrorFlag = "N";
    var VouDate = $("#Exp_Date").val();
    var PayeeGlAcc = $("#ddlPayeeGlList").val();
    if (PayeeGlAcc == '0') {
        $('[aria-labelledby="select2-ddlPayeeGlList-container"]').css("border-color", "red");
        $("#spn_PayeeGllst").text($("#valueReq").text());
        $("#spn_PayeeGllst").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
       
        $('[aria-labelledby="select2-ddlPayeeGlList-container"]').css("border-color", "#ced4da");
        $("#spn_PayeeGllst").css("display", "none");
    }
    let total_dramt = 0, total_cramt = 0
    $("#exp_voudetail > tbody > tr").each(function () {
        let row = $(this);
        let td_dramt = row.find("#dramt").text();
        let td_cramt = row.find("#cramt").text();
        total_dramt = parseFloat(cmn_ReplaceCommas(CheckNullNumber(total_dramt))) + parseFloat(cmn_ReplaceCommas(CheckNullNumber(td_dramt)));
        total_cramt = parseFloat(cmn_ReplaceCommas(CheckNullNumber(total_cramt))) + parseFloat(cmn_ReplaceCommas(CheckNullNumber(td_cramt)));
    });

    if (Math.abs(total_dramt - total_cramt) > 0) {
        ErrorFlag = "Y";
        swal("", $("#DebtCredtAmntMismatch").text(),"warning")
    }

    if (ErrorFlag == 'Y') {
        return false;
    }
    else {
        return true;
    }
}
function PaymentDetailValidation() {
    
    var ErrorFlag = "N";
    var Len = $("#TblPaymentDetail tbody tr").length;
    if (Len == 0) {
        swal("", $("#PleaseFillExpenseDetail").text(), "warning");
        ErrorFlag = 'Y';
    }
    if (Len > 0) {
        $("#TblPaymentDetail tbody tr").each(function () {
            var currentrow = $(this);
            var Rowno = currentrow.find("#SNohf").val();
            var VouNo = currentrow.find("#ddlVouchernumber_" + Rowno).val();
            if (VouNo == '0') {
                currentrow.find("[aria-labelledby='select2-ddlVouchernumber_" + Rowno + "-container']").css("border-color", "red");
                currentrow.find("#spn_Voulst").text($("#valueReq").text());
                currentrow.find("#spn_Voulst").css("display", "block");
                ErrorFlag = 'Y';
            }
            else {
                currentrow.find("[aria-labelledby='select2-ddlVouchernumber_ " + Rowno + "-container']").css("border-color", "#ced4da");
                currentrow.find("#spn_Voulst").css("display", "none");
            }
        })
    }
    if (ErrorFlag == 'Y') {
        return false;
    }
    else {
        return true;
    }
}

function ExpenseDetailvalidate() {
    
    var ErrorFlag = "N";
    var Len = $("#TblExpesnseDesc tbody tr").length;
    if (Len == '0') {
        swal("", $("#PleaseFillExpenseDetail").text(), "warning");
        ErrorFlag = 'Y';
    }
    if (Len > 0) {
        $("#TblExpesnseDesc tbody tr").each(function () {
            
            var CurrentRow = $(this);
            var Rowno = CurrentRow.find("#SNohf1").val();
            var ExpDesc = CurrentRow.find("#ExpDescription").val();
            var BillNum = CurrentRow.find("#ExpDescBillNumber").val();
            var BillDate = CurrentRow.find("#hdnBillDate").val();
            var Amount = cmn_ReplaceCommas(CurrentRow.find("#ExpDescAmount").val());
            var Acc_ID = CurrentRow.find('#ddlExpenseGlAccount_' + Rowno +'').val();
            if (ExpDesc == "" || ExpDesc == null) {
                CurrentRow.find("#ExpDescription").css("border-color", "red");
                CurrentRow.find("#spn_SuppName").text($("#valueReq").text());
                CurrentRow.find("#spn_SuppName").css("display", "block");
                ErrorFlag = 'Y';
            }
            else {
                CurrentRow.find("#ExpDescription").css("border-color", "#ced4da");
                CurrentRow.find("#spn_SuppName").css("display", "none");
            }
            if (BillNum == "" || BillNum == null) {
                CurrentRow.find("#ExpDescBillNumber").css("border-color", "red");
                CurrentRow.find("#spn_BillNum").text($("#valueReq").text());
                CurrentRow.find("#spn_BillNum").css("display", "block");
                ErrorFlag = 'Y';
            }
            else {
                CurrentRow.find("#ExpDescBillNumber").css("border-color", "#ced4da");
                CurrentRow.find("#spn_BillNum").css("display", "none");
            }
            if (BillDate == "" || BillDate == null) {
                CurrentRow.find("#ExpDescBillDate").css("border-color", "red");
                CurrentRow.find("#spn_Billdate").text($("#valueReq").text());
                CurrentRow.find("#spn_Billdate").css("display", "block");
                ErrorFlag = 'Y';
            }
            else {
                CurrentRow.find("#ExpDescBillDate").css("border-color", "#ced4da");
                CurrentRow.find("#spn_Billdate").css("display", "none");
            }
            if (Amount == "" || Amount == null) {
                CurrentRow.find("#ExpDescAmount").css("border-color", "red");
                CurrentRow.find("#spn_ExpDescAmt").text($("#valueReq").text());
                CurrentRow.find("#spn_ExpDescAmt").css("display", "block");
                ErrorFlag = 'Y';
            }
            else {
                if (parseFloat(Amount) > parseFloat(0)) {
                    CurrentRow.find("#ExpDescAmount").css("border-color", "#ced4da");
                    CurrentRow.find("#spn_ExpDescAmt").css("display", "none");
                }
                else {
                    CurrentRow.find("#ExpDescAmount").css("border-color", "red");
                    CurrentRow.find("#spn_ExpDescAmt").text($("#valueReq").text());
                    CurrentRow.find("#spn_ExpDescAmt").css("display", "block");
                    ErrorFlag = 'Y';
                }
                
            }
            if (Acc_ID == '0') {
                CurrentRow.find("[aria-labelledby='select2-ddlExpenseGlAccount_" + Rowno + "-container']").css("border-color", "red");
                CurrentRow.find("#spn_ExpenseGlAcc").text($("#valueReq").text());
                CurrentRow.find("#spn_ExpenseGlAcc").css("display", "block");
                ErrorFlag = 'Y';
            }
            else {
                CurrentRow.find("[aria-labelledby='select2-ddlExpenseGlAccount_" + Rowno + "-container']").css("border-color", "#ced4da");
                CurrentRow.find("#spn_ExpenseGlAcc").css("display", "none");
            }
        })
    }
    if (ErrorFlag == 'Y') {
        return false;
    }
    else {
        return true;
    }
}

//------Delete Expense Voucher Detail-----------//
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

//For WorkFlow 

function ForwardHistoryBtnClick() {
    
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#VoucherNumber").val();
    
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
function ForwardBtnClick() {
    
    //var ExpenVouStatus = "";
    //ExpenVouStatus = $('#DocumentStatus').val().trim();
    //if (ExpenVouStatus === "D" || ExpenVouStatus === "F") {

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
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
    //$("#Btn_Approve").attr("data-target", "");
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var Voudt = $("#Exp_Date").val();
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
                var ExpenVouStatus = "";
                ExpenVouStatus = $('#DocumentStatus').val().trim();
                if (ExpenVouStatus === "D" || ExpenVouStatus === "F") {

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
                    //$("#Btn_Approve").attr("data-target", "");
                }
                else {
                    swal("", $("#BooksAreClosedEntryCanNotBeMadeInThisFinancialYear").text(), "warning");
                    /* $("#Btn_Forward").attr("data-target", "");*/
                    $("#Forward_Pop").attr("data-target", "");
                    //$("#Btn_Approve").attr("data-target", "");
                }

            }
        }
    });
    /*End to chk Financial year exist or not*/
    return false;
}
async function OnClickForwardOK_Btn() {
    
    var fwchkval = $("input[name='forward_action']:checked").val();
    var EVNo = "";
    var EVDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status1 = "";
    var ModelData = "";
    var glVouMsg = "";

    Remarks = $("#fw_remarks").val();
    EVNo = $("#VoucherNumber").val();
    EVDate = $("#Exp_Date").val();
    docid = $("#DocumentMenuId").val();
    WF_Status1 = $("#WF_Status1").val();
    ModelData = (EVNo + ',' + EVDate + ',' + WF_Status1);
    $("#hdDoc_No").val(EVNo);
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });

    glVouMsg = $("#payglacc_narr").val()

    //var pdfAlertEmailFilePath = 'EV_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
    //GetPdfFilePathToSendonEmailAlert(EVNo, EVDate, pdfAlertEmailFilePath);
    if (fwchkval === "Forward") {
        if (fwchkval != "" && EVNo != "" && EVDate != "" && level != "") {
            
            await Cmn_InsertDocument_ForwardedDetail(EVNo, EVDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.reload();
            /*window.location.href = "/ApplicationLayer/ExpenseVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;*/
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/ExpenseVoucher/ApproveExpenseVou?Vou_No=" + EVNo + "&Vou_Date=" + EVDate + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1 + "&payglnarr=" + glVouMsg;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && EVNo != "" && EVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail(EVNo, EVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.reload();
            /*window.location.href = "/ApplicationLayer/ExpenseVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;*/
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && EVNo != "" && EVDate != "") {
            await Cmn_InsertDocument_ForwardedDetail(EVNo, EVDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.reload();
            /*window.location.href = "/ApplicationLayer/ExpenseVoucher/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&ModelData=" + ModelData;*/
        }
    }
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

//-------Cancel and WorkFlow work-------//
function OnClickCancelFlag() {
    debugger
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveInsertExpVouDetail()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    CancelledRemarks("#CancelFlag", "Enable");
}
//---------------End-------------------//
//----------Gl Detail------------------//
function fun_GLPosting() {
    
    var Disable = "Disabled";
   
    var row_Idx = ($('#exp_voudetail tbody tr').length) + 1;

    //fun_DeleteGLPostiing();
    $("#exp_voudetail tbody tr").remove();

    var tot_crdramrt = 0;
    var acc_id = $("#ddlPayeeGlList option:selected").val();
    var acc_name = $("#ddlPayeeGlList option:selected").text();

    fun_apendgldetails(row_Idx, 1, acc_name, acc_id, 0, 0, Disable);

    $("#TblExpesnseDesc > tbody > tr").each(function () {
        
        var curr_row = $(this);
        var hdn_rid = curr_row.find("#SNohf1").val();
        var accid = curr_row.find("#ddlExpenseGlAccount_" + hdn_rid + " option:selected").val();
        var accname = curr_row.find("#ddlExpenseGlAccount_" + hdn_rid + " option:selected").text();
        var accamt = cmn_ReplaceCommas(curr_row.find("#ExpDescAmount").val());
        var rowIdx = ($('#exp_voudetail tbody tr').length) + 1;

        if (accid != "0" && accid != null && accamt != null && accamt != "") {
            if (parseFloat(accamt) > 0) {
                tot_crdramrt = parseFloat(tot_crdramrt) + parseFloat(accamt);
                var flag = "N";
                var drtot_amt = 0;
                $("#exp_voudetail > tbody > tr").each(function () {
                    
                    var curr_row = $(this);
                    var hdn_accid = curr_row.find("#hfAccID").val();
                    if (hdn_accid == accid) {
                        flag = "Y";
                        drtot_amt = cmn_ReplaceCommas(curr_row.find("#dramt").text());
                        var dramt = parseFloat(parseFloat(drtot_amt) + parseFloat(accamt)).toFixed(ValDigit)
                        curr_row.find("#dramt").text(cmn_addCommas(dramt));
                        //curr_row.find("#dramt").text(parseFloat(parseFloat(drtot_amt) + parseFloat(accamt)).toFixed(ValDigit));
                    }
                })
                if (flag == "N") {
                    fun_apendgldetails(rowIdx, 0, accname, accid, accamt, 0, "Disabled");
                }
            }
        }
    })
    var sno = 0;
    $("#exp_voudetail > tbody > tr").each(function () {
        
        var curr_row = $(this);
        var hdn_rid = curr_row.find("#hdn_rid").val();
        sno = parseInt(sno) + 1;
        curr_row.find("#rid").text(sno)

        if (hdn_rid == "1") {
            var tot_crdramrt3 = parseFloat(tot_crdramrt).toFixed(ValDigit);
            var tot_crdramrt4 = cmn_addCommas(tot_crdramrt3);
            curr_row.find("#cramt").text(tot_crdramrt4);
            //curr_row.find("#cramt").text(parseFloat(tot_crdramrt).toFixed(ValDigit));
        }
    })
    var tot_crdramrt1 = parseFloat(tot_crdramrt).toFixed(ValDigit);
    var tot_crdramrt2 = cmn_addCommas(tot_crdramrt1);
    $("#exp_voudetail tfoot tr").find("#DrTotal").text(tot_crdramrt2);
    //$("#exp_voudetail tfoot tr").find("#DrTotal").text(parseFloat(tot_crdramrt).toFixed(ValDigit));
    $("#exp_voudetail tfoot tr").find("#CrTotal").text(tot_crdramrt2);
    //$("#exp_voudetail tfoot tr").find("#CrTotal").text(parseFloat(tot_crdramrt).toFixed(ValDigit));
}
function fun_DeleteGLPostiing() {
    //$("#exp_voudetail tbody tr").remove();


}
function fun_apendgldetails(rowIdx, r_id, acc_name, acc_id, dr_amt, cr_amr, Disable) {
    var dr_amt1 = parseFloat(dr_amt).toFixed(ValDigit);
    var dr_amt2 = cmn_addCommas(dr_amt1);
    var cr_amr1 = parseFloat(cr_amr).toFixed(ValDigit);
    var cr_amr2 = cmn_addCommas(cr_amr1);

    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
    }


    $('#exp_voudetail tbody').append(`<tr>
       <td><span id="rid">${rowIdx} </span><input type="hidden" id="hdn_rid" value="${r_id}" /></td>
       <td><span id="span_acc_name">${acc_name}</span> <input type="hidden" id="hfAccID" value="${acc_id}" /></td>
       <td id="dramt" class="num_right">${dr_amt2}</td>
       <td id="cramt" class="num_right">${cr_amr2}</td>
       <td class="center">
        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>
       </td>
       </tr>`);
    /*<button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="btn btn-primary btn-sm small_btn " data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_CostCenterDetail").text()}"></i></button>*/
}
//-------------End---------------------//
//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    
    var TotalAmt1 = 0;//add by sm 09-12-2024
    var Doc_ID = $("#DocumentMenuId").val();//add by sm 09-12-2024
    var clickedrow = $(e.target).closest("tr");
    var CstDbAmt = cmn_ReplaceCommas(clickedrow.find("#dramt").text());
    var CstCrtAmt = cmn_ReplaceCommas(clickedrow.find("#cramt").text());
    var GLAcc_Name = clickedrow.find("#span_acc_name").text();
    var GLAcc_id = clickedrow.find("#hfAccID").val();
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        var List = {};
        List.GlAccount = row.find("#hdntbl_GlAccountId").text();
        List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
        List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
        List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
        List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
        var amount = cmn_ReplaceCommas(row.find("#hdntbl_CstAmt").text());
        //var amount1 = parseFloat(amount).toFixed(ValDigit);
        TotalAmt1 = parseFloat(TotalAmt1) + parseFloat(amount);//add by sm 09-12-2024
        List.CC_Amount = cmn_addCommas(parseFloat(amount).toFixed(ValDigit));
        NewArr.push(List);
    });
    var TotalAmt = cmn_addCommas(toDecimal(TotalAmt1, ValDigit));//add by sm 09-12-2024
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ExpenseVoucher/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
            TotalAmt: TotalAmt,//add by sm 09-12-2024
            Doc_ID: Doc_ID//add by sm 09-12-2024   
        },
        success: function (data) {
            
            $("#CostCenterDetailPopup").html(data);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(cmn_addCommas(Amt));
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("exp_voudetail");
        },
    })
}

//-------------------Cost Center Section End-------------------//

function approveonclick() { /**Added this Condition by Nitesh 10-01-2024 for Disable Approve btn after one Click**/
    
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