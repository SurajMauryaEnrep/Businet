$(document).ready(function () {
    debugger;
    BindDDlContractorList();
    var Supp_id = $("#SupplierName").val();
    debugger;
    var SuppName = $("#SupplierName").val();
    $("#Hdn_SupplierName").val(SuppName);
    var InvNum = $("#JobInvoiceNumber").val();
    if (InvNum == "" || InvNum == null) {     
    }
    else {
        var GRNNo1 = $("#Hdn_GRNNumber").val();
        $("#ddlGoodReceiptNoteNo").val(GRNNo1);
    }
    //else {
    //    $('#ddlGoodReceiptNoteNo').empty().append('<option value="---Select----" selected="selected">---Select---</option>');
    //}
   
    BindDDLAccountList();
    ji_no = $("#JobInvoiceNumber").val();
    $("#hdDoc_No").val(ji_no);

    $("#TxtGrossValue").on('change', function () {
        debugger;
        var GstCat = $("#Hd_GstCat").val();
        var ToTdsAmt = 0;
        if (GstCat == "UR") {
            ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));// + parseFloat(CheckNullNumber($("#TxtOtherCharges").val()));

        } else {
            ToTdsAmt = parseFloat(CheckNullNumber($("#TxtGrossValue").val()));
        }
        ResetTDS_CalOnchangeDocDetail(ToTdsAmt, "#TxtTDS_Amount");
    });
    BindDocumentNumberList();
});

//----------------Header Section Start------------------------//
function BindDDlContractorList() {
    debugger;
    var Branch = sessionStorage.getItem("BranchID");
    $("#SupplierName").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    PO_ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });

}
function OnChangeContractorName(SuppID) {
    debugger;
    var Supp_id = SuppID.value;
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
    }
    else {
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_JISupplierName").val(SuppName)

        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
    }
    GetSuppAddress(Supp_id);
    //BindGoodReceiptNoteLists(Supp_id)
    BindDocumentNumberList();
    
}
function GetSuppAddress(Supp_id) {
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/JobInvoiceSC/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    //debugger;
                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#Rqrdaddr").css("display", "");
                            $("#Address").val(arr.Table[0].BillingAddress);
                            $("#bill_add_id").val(arr.Table[0].bill_add_id);
                            $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                            $("#supp_acc_id").val(arr.Table[0].supp_acc_id);
                            $("#Ship_StateCode").val(arr.Table[0].state_code);
                            var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            $("#ddlCurrency").html(s);
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#RateDigit").text()));

                            $("#Hd_GstCat").val(arr.Table[0].gst_cat);

                            if ($("#Address").val() === "") {
                                $('#SpanSuppAddrErrorMsg').text($("#valueReq").text());
                                $("#Address").css("border-color", "Red");
                                $("#SpanSuppAddrErrorMsg").css("display", "block");
                                ErrorFlag = "Y";
                            }
                            else {
                                $("#SpanSuppAddrErrorMsg").css("display", "none");
                                $("#Address").css("border-color", "#ced4da");
                            }
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function BindDocumentNumberList() {
    debugger;
    var SuppID = $("#SupplierName").val()
    $("#ddlGoodReceiptNoteNo").select2({
        ajax: {
            url: $("#hdsrc_doc_no").val(),
            data: function (params) {
                var queryParameters = {
                    DocumentNo: params.term, // search term like "a" then "an"
                    SuppID: SuppID
                };
                return queryParameters;
            },
            cache: true,
            multiple: true,
            processResults: function (data, params) {
                debugger;
                var DocList = ConvertIntoArreyDocNoList("#JOInvItmDetailsTbl", "#src_doc_no");
                data = data.filter(j => !JSON.stringify(DocList).includes(j.Name));
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
                        <div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#DocNo").text()}</div>
                        <div class="col-md-3 col-xs-6 def-cursor">${$("#DocDate").text()}</div></div>
                        </strong></li></ul>`)
                }
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.Name, text: val.Name, date: val.ID };
                    }),
                };
            },
        },
        templateResult: function (data) {

            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result;
            $result = $(
                '<div class="row">' +
                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.date + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
//function BindGoodReceiptNoteLists(Supp_id) {

//    try {
//        debugger;

//        $.ajax({
//            type: "POST",
//            url: "/ApplicationLayer/JobInvoiceSC/GetGoodReceiptNoteLists",

//            data: { Supp_id: Supp_id },
//            success: function (data) {
//                debugger;
//                if (data == 'ErrorPage') {
//                    PI_ErrorPage();
//                    return false;
//                }
//                if (data !== null && data !== "") {
//                    var arr = [];
//                    arr = JSON.parse(data);
//                    //var DocList = ConvertIntoArreyDocNoList("#JOInvItmDetailsTbl", "#src_doc_no");
//                    //data = arr.Table.filter(j => !JSON.stringify(DocList).includes(j.mr_no));
//                    if (arr.Table.length > 0) {
//                        $("#ddlGoodReceiptNoteNo option").remove();
//                        $("#ddlGoodReceiptNoteNo optgroup").remove();
//                        $('#ddlGoodReceiptNoteNo').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
//                        for (var i = 0; i < arr.Table.length; i++) {
//                            $('#Textddl').append(`<option data-date="${arr.Table[i].mr_dt}" value="${arr.Table[i].mr_no + ',' + arr.Table[i].bill_no + ',' + arr.Table[i].bill_date}">${arr.Table[i].mr_no}</option>`);
//                        }
//                        var firstEmptySelect = true;
//                        $('#ddlGoodReceiptNoteNo').select2({
//                            templateResult: function (data) {
//                                var DocDate = $(data.element).data('date');
//                                var classAttr = $(data.element).attr('class');
//                                var hasClass = typeof classAttr != 'undefined';
//                                classAttr = hasClass ? ' ' + classAttr : '';
//                                var $result = $(
//                                    '<div class="row">' +
//                                    '<div class="col-md-7 col-xs-12' + classAttr + '">' + data.text + '</div>' +
//                                    '<div class="col-md-5 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
//                                    '</div>'
//                                );
//                                return $result;
//                                firstEmptySelect = false;
//                            }
//                        });

//                        $("#GRN_Date").val(""); 
//                        $("#Bill_No").val("");
//                        $("#GRN_Date").val("");
//                    }
//                }
//            },
//        });
//    } catch (err) {
//        console.log("GetMenuData Error : " + err.message);
//    }
//}
function ConvertIntoArreyDocNoList(TableID, DDLName) {
    let array = [];
    $(TableID + " tbody tr").each(function () {
        let currentRow = $(this);
        let DocNo = currentRow.find(DDLName).val();
        if (DocNo != "0" && DocNo != "") {
            array.push({ id: DocNo });
        }
    });
    return array;
}
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfStatus").val().trim();
    var JInvTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        JInvTransType = "Update";
    }

    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, JInvTransType);
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function OnChangeGoodReceiptNoteNo(GRN_No) {
    debugger;
    //var GRNNo = GRN_No.value;
    var GRNNo = $('#ddlGoodReceiptNoteNo').val();
    var GRNDate = $('#ddlGoodReceiptNoteNo option:selected').data().data.date.trim();
    //var GRNNum = GRN_No.value;
    //var grn_num = GRNNum.split(',');
    //var GRNNo = grn_num[0];
    //var BillNo = grn_num[1];
    //var Billdt = grn_num[2];
    //var bill_dt = Billdt.split('T');
    //var BillDate = bill_dt[0];
    $("#Hdn_GRNNumber").val(GRNNo);
    //var GRNDate = $('#ddlGoodReceiptNoteNo').select2("data")[0].element.attributes[0].value;
    var GRNDP = GRNDate.split("-");
    var FGRNDate = (GRNDP[2] + "-" + GRNDP[1] + "-" + GRNDP[0]);
    if (GRNNo == "---Select---") {
        $("#GRN_Date").val("");
        $('#SpanGRNNoErrorMsg').text($("#valueReq").text());
        $("#SpanGRNNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "red");
    }
    else {
        $("#GRN_Date").val(FGRNDate);
        $("#SpanGRNNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
        //$("#Bill_No").val(BillNo);
        //$("#Bill_Date").val(BillDate);
        //$("#SpanBillNoErrorMsg").css("display", "none");
        //$("#Bill_No").css("border-color", "#ced4da");
        //$("#SpanBillDateErrorMsg").css("display", "none");
        //$("#Bill_Date").css("border-color", "#ced4da");
    }
}
function OnChangeBillNo() {
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
        RefreshBillNoBillDateInGLDetail();
        
    }
}
function OnChangeBillDate() {
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
    }
    else {
        /*------Code Added by Suraj Maurya on 22-10-2024------*/
        var minDate = new Date('1900-01-01')._format('Y-m-d');
        var billDate = new Date($("#Bill_Date").val())._format('Y-m-d');

        if (billDate > minDate) {
            $("#SpanBillDateErrorMsg").css("display", "none");
            $("#Bill_Date").css("border-color", "#ced4da");
            
            RefreshBillNoBillDateInGLDetail();
        } else {
            $('#SpanBillDateErrorMsg').text($("#JC_InvalidDate").text());
            $("#SpanBillDateErrorMsg").css("display", "block");
            $("#Bill_Date").css("border-color", "Red");
        }
        /*------Code Added by Suraj Maurya on 22-10-2024 End------*/
    }
}
function OnClickAddButton() {
    debugger;
    if (CheckJI_Validations() == false) {
        return false;
    }
    var DocMenuId = $("#DocumentMenuId").val();
    var GRNNum = $('#ddlGoodReceiptNoteNo').val();
    var grn_num = GRNNum.split(',');
    var GRNNo = grn_num[0];
    var GRNDate = $('#GRN_Date').val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    if (GRNNo == "---Select---" || GRNNo == "0") {
        $('#SpanGRNNoErrorMsg').text($("#valueReq").text());
        $("#SpanGRNNoErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "red");
        $("#ddlGoodReceiptNoteNo").css("border-color", "red");
    }
    else {
        $("#SpanGRNNoErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
        $("#ddlGoodReceiptNoteNo").css("border-color", "#ced4da");
        $("#SupplierName").attr("disabled", "disabled");
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/JobInvoiceSC/GetJOandGoodReceiptNoteSCDetails",
                data: { GRNNo: GRNNo, GRNDate: GRNDate },
                success: function (data) {
                    debugger;
                    if (data == 'ErrorPage') {
                        PI_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            /*  $('#JOInvItmDetailsTbl tbody tr').remove();*/
                            var tbllength = $('#JOInvItmDetailsTbl tbody tr').length;
                            for (var k = 0; k < arr.Table.length; k++) {

                                if (arr.Table[k].state_code == $("#Ship_StateCode").val()) {
                                    $("#Hd_GstType").val("Both")
                                } else {
                                    $("#Hd_GstType").val("IGST")
                                }

                                var S_NO = $('#JOInvItmDetailsTbl tbody tr').length + 1;
                                var GstApplicable = $("#Hdn_GstApplicable").text();
                                var ManualGst = "";
                                if (GstApplicable == "Y") {
                                    if (arr.Table[k].manual_gst == "Y") {
                                        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" disabled checked class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                                    }
                                    else {
                                        ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" disabled class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
                                    }
                                    ManualGst += `<td class="qt_to">
                                                     <div class="custom-control custom-switch sample_issue">
                                                             <input type="checkbox" checked class="custom-control-input  margin-switch" onclick="OnClickClaimITCCheckBox(event)" id="ClaimITC">
                                                         <label class="custom-control-label" disabled="" for="" style="padding: 3px 0px;"> </label>
                                                     </div>
                                                 </td>`;
                                }
                                var checked = "";
                                if (arr.Table[k].tax_expted=="Y") {
                                    checked = "checked";
                                }
                               /* var DisableForDomestic = DocMenuId == "105101145" ? "style='display:none;'" : "";*/
                                $('#JOInvItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
                           <td class="sr_padding">${S_NO}</td>
                           <td class="ItmNameBreak itmStick tditemfrz">
                            <input id="src_doc_no" class="form-control num_right" autocomplete="" type="text" name="src_doc_no" value="${arr.Table[k].mr_no}" placeholder='${$("#span_SourceDocumentNumber").text()}' disabled>
                           </td>
                           <td>
                           <input id="src_doc_date" class="form-control num_right" autocomplete="" type="text" name="src_doc_date" value="${arr.Table[k].mr_dt}" placeholder='${$("#span_SourceDocumentDate").text()}' disabled>
                           <input class="" type="hidden" id="src_doc_dt" value="${arr.Table[k].mr_date}" />
                           </td>
                                                        
                                                        <td>
                                                           <div class=" col-sm-11 no-padding">
                                                            <input id="TxtItemName" class="form-control time" autocomplete="off" type="text" name="" value='${arr.Table[k].item_name}' placeholder='${$("#ItemName").text()}'  onblur="this.placeholder='Item Name'" disabled>
                                                                <input  type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" />
                                                                <input class="" type="hidden" id="hdn_item_gl_acc" value="${arr.Table[k].loc_pur_coa}" />
                                                                <input  type="hidden" id="ItemHsnCode" value="${arr.Table[k].HSN_code}" />
                                                           </div>
                                                                    <div class="col-sm-1 i_Icon">
                                                                        <button type="button" id="ItmInfoBtnIcon" class="calculator ItmInfoBtnIcon" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="@Resource.ItemInformation">  </button>
                                                                    </div>
                                                        </td>
                                                        <td>
                                                        <input id="TxtReceivedQuantity" class="form-control num_right" autocomplete="" type="text" name="ReceivedQuantity" value="${parseFloat(arr.Table[k].invoice_qty).toFixed(QtyDecDigit)}" placeholder="0000.00"  disabled>
                                                        </td>
                                                        <td>
                                                            <input id="TxtRate" class="form-control num_right" autocomplete="" type="text" name="Rate" placeholder="0000.00" value="${parseFloat(arr.Table[k].item_rate).toFixed(RateDecDigit)}"  disabled>
                                                        </td>
                                                        <td>
                                                            <input id="TxtItemGrossValue" class="form-control num_right" autocomplete="off" type="text" name="GrossValue" value="${parseFloat(arr.Table[k].item_gross_val).toFixed(ValDecDigit)}" placeholder="0000.00"  disabled>
                                                        </td>
                                                         <td class="qt_to">
                                                             <div class="custom-control custom-switch sample_issue">
                                                                 <input type="checkbox" disabled ${checked} class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted">
                                                                 <label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label>
                                                             </div>
                                                         </td>
                                                         `+ ManualGst + `
                                                        <td>
                                                            <div class=" col-sm-10 num_right no-padding">
                                                            <input id="Txtitem_tax_amt" class="form-control num_right" value="${parseFloat(arr.Table[k].tax_amt).toFixed(QtyDecDigit)}" autocomplete="off" type="text" name="item_tax_amt" placeholder="0000.00"  disabled>
                                                            </div>
                                                            <div class=" col-sm-2 num_right no-padding"><button type="button" class="calculator item_pop" id="BtnTxtCalculation" data-toggle="modal" onclick="OnClickTaxCalBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${$("#Span_TaxCalculator_Title").text()}"></i></button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="TxtTotalValue" class="form-control num_right" value="${parseFloat(arr.Table[k].item_gross_val).toFixed(ValDecDigit)}" autocomplete="off" type="text" name="NetValue"  placeholder="0000.00"  disabled>
                                                        </td>
                                                        
                            </tr>`);
                                debugger;
                                var Itm_ID = arr.Table[k].item_id;
                                    //if (GstApplicable == "Y") {
                                    //    $("#HdnTaxOn").val("Item");
                                    //    Cmn_ApplyGSTToAtable("JOInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", arr.Table1, "Y");
                                    //}
                                    //else {
                                    //    $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                    //    if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                    //        $("#HdnTaxOn").val("Item");
                                    //        $("#TaxCalcItemCode").val(Itm_ID);
                                    //        //$("#HiddenRowSNo").val(k);
                                    //        $("#Tax_AssessableValue").val(arr.Table[k].item_gross_val);
                                    //        //$("#TaxCalcGRNNo").val(arr.Table[k].mr_no);
                                    //        //$("#TaxCalcGRNDate").val(arr.Table[k].mr_date);
                                    //        var TaxArr = arr.Table1;
                                    //        let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                                    //        selected = JSON.stringify(selected);
                                    //        TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                    //        selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                                    //        selected = JSON.stringify(selected);
                                    //        TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                    //        if (TaxArr.length > 0) {
                                    //            AddTaxByHSNCalculation(TaxArr);
                                    //            OnClickSaveAndExit("Y");
                                    //            var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                    //            Reset_ReOpen_LevelVal(lastLevel);
                                    //        }
                                    //    }
                                    //}
                               
                            }
                            if (arr.Table4.length > 0) {
                                var ArrTaxList = [];
                                for (var l = 0; l < arr.Table4.length; l++) {//Tax On Item
                                    $('#Hdn_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo">${arr.Table4[l].mr_no}</td>
                                    <td id="DocDate">${arr.Table4[l].mr_dt}</td>
                                    <td id="TaxItmCode">${arr.Table4[l].item_id}</td>
                                    <td id="TaxName">${arr.Table4[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table4[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table4[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table4[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table4[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${arr.Table4[l].tax_val}</td>
                                    <td id="TotalTaxAmount">${arr.Table4[l].item_tax_amt}</td>
                                    <td id="TaxApplyOnID">${arr.Table4[l].tax_apply_on}</td>
                                    <td id="TaxRecov">${arr.Table4[l].recov}</td>
                                    <td id="TaxAccId">${arr.Table4[l].tax_acc_id}</td>
                                        </tr>`);

                                    if (ArrTaxList.findIndex(v => (v.taxID) == (arr.Table4[l].tax_id)) > -1) {
                                        let getIndex = ArrTaxList.findIndex(v => (v.taxID) == (arr.Table4[l].tax_id));
                                        ArrTaxList[getIndex].totalTaxAmt = parseFloat(CheckNullNumber(ArrTaxList[getIndex].totalTaxAmt)) + parseFloat(CheckNullNumber(arr.Table4[l].tax_val));
                                    } else {
                                        ArrTaxList.push({ taxID: arr.Table4[l].tax_id, taxAccID: arr.Table4[l].tax_acc_id, taxName: arr.Table4[l].tax_name, totalTaxAmt: arr.Table4[l].tax_val, TaxRecov: arr.Table4[l].recov })
                                    }
                                }
                                let TotalTAmt = 0;
                                var length = $("#Tbl_ItemTaxAmountList tbody tr").length;
                                for (var l = 0; l < ArrTaxList.length; l++) {
                                    if (length > 0) {
                                        $("#Tbl_ItemTaxAmountList tbody tr").each(function () {
                                            var currentrow = $(this);
                                            var taxID = currentrow.find("#taxID").text();
                                            if (taxID == ArrTaxList[l].taxID) {
                                                var taxAmt = parseFloat(currentrow.find("#TotalTaxAmount").text()) + parseFloat(ArrTaxList[l].totalTaxAmt);
                                                currentrow.find("#TotalTaxAmount").text(parseFloat(taxAmt).toFixed(ValDecDigit))
                                            }
                                            //else {
                                            //    $("#Tbl_ItemTaxAmountList tbody").append(`<tr>
                                            //        <td>${ArrTaxList[l].taxName}</td>
                                            //        <td id="taxRecov" class="center">${(ArrTaxList[l].TaxRecov == 'Y' ? '<i class="fa fa-check text-success " aria-hidden="true"></i>' : '<i class="fa fa-times-circle text-danger" aria-hidden="true"></i>')}</td>
                                            //        <td id="TotalTaxAmount" class="num_right">${parseFloat(ArrTaxList[l].totalTaxAmt).toFixed(ValDecDigit)}</td>
                                            //        <td hidden="hidden" id="taxID">${ArrTaxList[l].taxID}</td>
                                            //        <td hidden="hidden" id="taxAccID">${ArrTaxList[l].taxAccID}</td>
                                            //    </tr>`)
                                            //}


                                        });
                                    }
                                    else {
                                        $("#Tbl_ItemTaxAmountList tbody").append(`<tr>
                                                    <td>${ArrTaxList[l].taxName}</td>
                                                    <td id="taxRecov" class="center">${(ArrTaxList[l].TaxRecov == 'Y' ? '<i class="fa fa-check text-success " aria-hidden="true"></i>' : '<i class="fa fa-times-circle text-danger" aria-hidden="true"></i>')}</td>
                                                    <td id="TotalTaxAmount" class="num_right">${parseFloat(ArrTaxList[l].totalTaxAmt).toFixed(ValDecDigit)}</td>
                                                    <td hidden="hidden" id="taxID">${ArrTaxList[l].taxID}</td>
                                                    <td hidden="hidden" id="taxAccID">${ArrTaxList[l].taxAccID}</td>
                                                </tr>`)
                                    }
                                    
                                    TotalTAmt = parseFloat(TotalTAmt) + parseFloat(ArrTaxList[l].totalTaxAmt);

                                }
                                $("#_ItemTaxAmountTotal").text(parseFloat(TotalTAmt).toFixed(ValDecDigit))

                            }
                            if (tbllength == 0) {                               
                                if (arr.Table3.length > 0) {
                                    for (var l = 0; l < arr.Table3.length; l++) {//Tax On OC
                                        $('#Hdn_OC_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table3[l].item_id}</td>
                                    <td id="TaxName">${arr.Table3[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table3[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table3[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table3[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table3[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${arr.Table3[l].tax_val}</td>
                                    <td id="TotalTaxAmount">${arr.Table3[l].item_tax_amt}</td>
                                    <td id="TaxApplyOnID">${arr.Table3[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table3[l].tax_acc_id}</td>
                                        </tr>`);

                                        $('#Hdn_OCTemp_TaxCalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${arr.Table3[l].item_id}</td>
                                    <td id="TaxName">${arr.Table3[l].tax_name}</td>
                                    <td id="TaxNameID">${arr.Table3[l].tax_id}</td>
                                    <td id="TaxPercentage">${arr.Table3[l].tax_rate}</td>
                                    <td id="TaxLevel">${arr.Table3[l].tax_level}</td>
                                    <td id="TaxApplyOn">${arr.Table3[l].tax_apply_Name}</td>
                                    <td id="TaxAmount">${arr.Table3[l].tax_val}</td>
                                    <td id="TotalTaxAmount">${arr.Table3[l].item_tax_amt}</td>
                                    <td id="TaxApplyOnID">${arr.Table3[l].tax_apply_on}</td>
                                    <td id="TaxAccId">${arr.Table3[l].tax_acc_id}</td>
                                        </tr>`);
                                    }
                                }
                                debugger;
                                let TotalAMount = 0;
                                let TotalTaxAMount = 0;
                                let TotalAMountWT = 0;
                                let TotalSuppOCAmtWT = 0;
                                if (arr.Table2.length > 0) {
                                    for (var l = 0; l < arr.Table2.length; l++) {//Tax On OC
                                        $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
                        <td id="OC_name">${arr.Table2[l].oc_name}</td>
                        <td id="OC_HSNCode">${arr.Table2[l].HSN_code}</td>
                        <td id="OC_Curr">${arr.Table2[l].curr_name}</td>
                        <td id="HdnOC_CurrId">${arr.Table2[l].curr_id}</td>
                        <td id="td_OCSuppName">${IsNull(arr.Table2[l].supp_name, '')}</td>
                        <td id="td_OCSuppID">${IsNull(arr.Table2[l].supp_id, '')}</td>
                        <td id="td_OCSuppType">${IsNull(arr.Table2[l].supp_type, '')}</td>
                        <td id="OC_Conv">${arr.Table2[l].conv_rate}</td>
                        <td hidden="hidden" id="OC_ID">${arr.Table2[l].oc_id}</td>
                        <td class="num_right" id="OCAmtSp">${arr.Table2[l].oc_val}</td>
                        <td class="num_right" id="OCAmtBs">${arr.Table2[l].OCValBs}</td>
                        <td class="num_right" id="OCTaxAmt">${arr.Table2[l].tax_amt}</td>
                        <td class="num_right" id="OCTotalTaxAmt">${arr.Table2[l].total_amt}</td>
                        <td id="OCBillNo">${arr.Table2[l].bill_no == null ? "" : arr.Table2[l].bill_no}</td>
                        <td id="OCBillDate">${arr.Table2[l].bill_dt == null ? "" : arr.Table2[l].bill_dt}</td>
                        <td id="OCBillDt" hidden>${arr.Table2[l].bill_date == null ? "" : arr.Table2[l].bill_date}</td>
                        <td id="OCAccId" hidden>${arr.Table2[l].oc_acc_id}</td>
                        <td id="OCSuppAccId" hidden>${IsNull(arr.Table2[l].supp_acc_id, '')}</td>
                        </tr>`);

                                        $('#Tbl_OC_Deatils tbody').append(`<tr id="R${l}">
                        <td id="deletetext" class=" red center"><i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" "></i></td>
                        <td id="OCName" >${arr.Table2[l].oc_name}</td>
                        <td id="OC_HSNCode" >${arr.Table2[l].HSN_code}</td>
                        <td id="OCCurr" >${arr.Table2[l].curr_name}</td>
                        <td id="HdnOCCurrId" hidden>${arr.Table2[l].curr_id}</td>
                        <td id="td_OCSuppName" >${IsNull(arr.Table2[l].supp_name, '')}</td>
                        <td id="td_OCSuppID" style="display:none">${IsNull(arr.Table2[l].supp_id, '')}</td>
                        <td id="td_OCSuppType" style="display:none">${IsNull(arr.Table2[l].supp_type, '')}</td>
                        <td id="OCConv" class="num_right">${parseFloat(arr.Table2[l].conv_rate).toFixed(RateDecDigit)}</td>
                        <td hidden="hidden" id="OCValue">${arr.Table2[l].oc_id}</td>
                        <td class="num_right" id="OCAmount">${parseFloat(arr.Table2[l].oc_val).toFixed(ValDecDigit)}</td>
                        <td class="num_right" id="OcAmtBs" >${parseFloat(arr.Table2[l].OCValBs).toFixed(ValDecDigit)}</td>
                          <td class="num_right">
                        <div class="col-sm-10 lpo_form" id="OCTaxAmt">${parseFloat(CheckNullNumber(arr.Table2[l].tax_amt)).toFixed(ValDecDigit)}</div>
                         <div class="col-md-2 col-sm-12 no-padding"><button type="button" id="OCTaxBtnCal" class="calculator" data-toggle="modal" onclick="OnClickOCTaxCalculationBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title=""></i></button></div>
                    </td>
                    <td class="num_right" id="OCTotalTaxAmt">${parseFloat(CheckNullNumber(arr.Table2[l].total_amt)).toFixed(ValDecDigit)}</td>
                    <td id="OCBillNo">${arr.Table2[l].bill_no == null ? "" : arr.Table2[l].bill_no}</td>
                    <td id="OCBillDate">${arr.Table2[l].bill_dt == null ? "" : arr.Table2[l].bill_dt}</td>
                    <td id="OCBillDt" hidden>${arr.Table2[l].bill_date == null ? "" : arr.Table2[l].bill_date}</td>
                    <td id="OCAccId" hidden>${arr.Table2[l].oc_acc_id}</td>
                    <td id="OCSuppAccId" hidden>${IsNull(arr.Table2[l].supp_acc_id, '')}</td>
                    </tr>`);
                                        $("#Tbl_OtherChargeList tbody").append(`<tr>
                                                    <td id="othrChrg_Name">${arr.Table2[l].oc_name}</td>
                                                    <td>${arr.Table2[l].supp_name}</td>
                                                    <td align="right" id="OCAmtSp1">${parseFloat(arr.Table2[l].OCValBs).toFixed(ValDecDigit)}</td>
                                                    <td hidden="hidden" id="OCID">${arr.Table2[l].oc_id}</td>
                                                    <td align="right">${parseFloat(CheckNullNumber(arr.Table2[l].tax_amt)).toFixed(ValDecDigit)}</td>
                                                    <td align="right">${parseFloat(CheckNullNumber(arr.Table2[l].total_amt)).toFixed(ValDecDigit)}</td>
                                                </tr>`)
                                        TotalAMount = parseFloat(TotalAMount) + parseFloat(arr.Table2[l].OCValBs);
                                        TotalTaxAMount = parseFloat(TotalTaxAMount) + parseFloat(arr.Table2[l].tax_amt);
                                        TotalAMountWT = parseFloat(TotalAMountWT) + parseFloat(arr.Table2[l].total_amt);
                                        if (arr.Table2[l].bill_no == "") {
                                            TotalSuppOCAmtWT = parseFloat(TotalSuppOCAmtWT) + parseFloat(arr.Table2[l].total_amt);
                                        }
                                    }
                                    $("#_OtherChargeTotal").text(TotalAMount.toFixed(ValDecDigit));
                                    $("#_OtherChargeTotalTax").text(TotalTaxAMount.toFixed(ValDecDigit));
                                    $("#_OtherChargeTotalAmt").text(TotalAMountWT.toFixed(ValDecDigit));
                                    $("#TxtOtherCharges").val(TotalAMountWT.toFixed(ValDecDigit))
                                    $("#TxtDocSuppOtherCharges").val(TotalSuppOCAmtWT.toFixed(ValDecDigit))
                                }
                                $("#OC_HeaderFields").css("display", "none");
                                $("#Tbl_OC_Deatils > tbody > tr  #OCDelIcon").attr("disabled", true);
                                //$("#OC_buttons").css("display", "none");
                                //$("#OC_buttons button").remove();
                            }
                            else {
                                $('#Tbl_OC_Deatils >tbody >tr').remove();
                                $('#ht_Tbl_OC_Deatils >tbody >tr').remove();
                                $('#Hdn_OC_TaxCalculatorTbl >tbody >tr').remove();
                                $('#Hdn_OC_TDS_CalculatorTbl >tbody >tr').remove();
                                $('#Tbl_OtherChargeList >tbody >tr').remove();
                                $("#_OtherChargeTotal").text(parseFloat(0).toFixed(ValDecDigit));
                                $("#_OtherChargeTotalTax").text(parseFloat(0).toFixed(ValDecDigit));
                                $("#_OtherChargeTotalAmt").text(parseFloat(0).toFixed(ValDecDigit));
                                $("#OC_HeaderFields").css("display", "block");
                                $("#DisableOCDlt").val("Enable");
                                //$("#OC_buttons").css("display", "");
                                $("#TxtOtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
                                $("#TxtDocSuppOtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
                            }
                            //CalTaxAmount_ItemWise(arr);
                            calculatetotaltax();
                            var TxtOC = CheckNullNumber($("#TxtOtherCharges").val());
                            var _OCTotalAmt = $("#_OtherChargeTotalAmt").text();
                            if (parseFloat(TxtOC) == parseFloat(_OCTotalAmt)) {
                                Calculate_OC_AmountItemWise(_OCTotalAmt)
                            }
                            CalculateAmount();

                            let SuppId = $("#SupplierName").val();
                            let GrossAmt = $("#TxtGrossValue").val();
                            AutoTdsApply(SuppId, GrossAmt).then(() => {
                            CalculateVoucherTotalAmount();
                                    //OnClickOtherChargeBtn();
                            GetAllGLID();
                            });
                            debugger;
                            //var DocNo = $('#src_doc_no').val();
                            //$("#ddlGoodReceiptNoteNo").val("---Select---,null,null").trigger('change');
                            //$("#SpanGRNNoErrorMsg").css("display", "none");
                            //$("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
                            //$("#JI_AddBtn").css("display", "none");
                            $('#ddlGoodReceiptNoteNo').attr("onchange", "");
                            $('#ddlGoodReceiptNoteNo').val("0").trigger('change');
                            $('#GRN_Date').val(0);
                            $('#ddlGoodReceiptNoteNo').attr("onchange", "OnChangeGoodReceiptNoteNo()");
                            CalculateVoucherTotalAmount();
                            GetAllGLID();
                        }
                    }
                },
            });
        } catch (err) {
            console.log("GoodReceiptInvoice Error : " + err.message);
        }
    }
}
function calculatetotaltax() {
    var ttltaxAmt = 0;
    var ValDecDigit = $("#ValDigit").text();///Amount
    $("#Tbl_ItemTaxAmountList tbody tr").each(function () {
        var currentrow = $(this);
        var taxAmt = currentrow.find("#TotalTaxAmount").text();
        ttltaxAmt = parseFloat(ttltaxAmt) + parseFloat(taxAmt);
    });
    $("#_ItemTaxAmountTotal").text(parseFloat(ttltaxAmt).toFixed(ValDecDigit))
}
async function AutoTdsApply(SuppId, GrossAmt) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();///Amount
    await $.ajax({
        type: "POST",
        url: "/Common/Common/Cmn_GetTdsDetails",
        data: { SuppId: SuppId, GrossVal: GrossAmt },
        success: function (data) {
            debugger;
            var arr = JSON.parse(data);
            let tds_amt = 0;
            if (arr.Table1 != null) {
                let checkResult = arr.Table1.length > 0 ? arr.Table1[0].result : "";

                if (checkResult == "Invalid Slab") {
                    swal("", $("#InvailidTdsSlabFound").text(), "warning")
                } else {
                    var checkTdsAcc = "Y";
                    $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
                    for (var i = 0; i < arr.Table1.length; i++) {
                        //let td_tds_amt = Math.round(arr.Table1[i].tds_amt);/* commented by Suraj Maurya on 03-04-2025 */
                        let td_tds_amt = Cmn_RoundValue(arr.Table1[i].tds_amt);
                        if (arr.Table1[i].tds_id == "") {
                            checkTdsAcc = "N";
                        }
                        if (checkTdsAcc == "Y") {
                            $('#Hdn_TDS_CalculatorTbl tbody').append(`<tr id="R${i + 1}">
                                <td id="td_TDS_Name">${arr.Table1[i].tds_name}</td>
                                <td id="td_TDS_NameID">${arr.Table1[i].tds_id}</td>
                                <td id="td_TDS_Percentage">${arr.Table1[i].tds_perc}</td>
                                <td id="td_TDS_Level">${arr.Table1[i].tds_level}</td>
                                <td id="td_TDS_ApplyOn">${arr.Table1[i].tds_apply_on}</td>
                                <td id="td_TDS_Amount">${td_tds_amt}</td>
                                <td id="td_TDS_ApplyOnID">${arr.Table1[i].tds_apply_on_id}</td>
                                <td id="td_TDS_BaseAmt">${arr.Table1[i].tds_bs_amt}</td>
                                <td id="td_TDS_AccId">${arr.Table1[i].tds_acc_id}</td>
                                    </tr>`);
                            tds_amt += parseFloat(td_tds_amt);
                        }
                        $("#TxtTDS_Amount").val(tds_amt.toFixed(ValDecDigit));

                    }

                    if (checkTdsAcc == "N") {
                        $('#Hdn_TDS_CalculatorTbl tbody tr').remove();
                        $("#TxtTDS_Amount").val(parseFloat(0).toFixed(ValDecDigit));
                        swal("", $("#TDSAccountIsNotLinkedWithTDSSlab").text(), "warning");
                    }
                }
            }


        }
    })
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode)
}
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        CalculateTaxExemptedAmt(e)
        GetAllGLID();
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "JOInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", "", "")
        CalculateTaxExemptedAmt(e)
        GetAllGLID();
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var ValDecDigit = $("#ValDigit").text();
    var currentrow = $(e.target).closest('tr');
    var item = currentrow.find("#hfItemID").val();
    $("#Tax_ItemID").val(item);
    if (currentrow.find("#ManualGST").is(":checked")) {
        currentrow.find('#TaxExempted').prop("checked", false);
        $("#TaxCalculatorTbl tbody tr").remove();
        $("#taxTemplate").text("Template")
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        $("#TotalTaxAmount").text(parseFloat(0).toFixed(ValDecDigit))
        CalculateTaxExemptedAmt(e)
        GetAllGLID();
        currentrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));

    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        $("#TaxCalculatorTbl tbody tr").remove();
        var gst_number = $("#ship_add_gstNo").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "JOInvItmDetailsTbl", "hfItemID", "", "TxtItemGrossValue", "", "")
        CalculateTaxExemptedAmt(e)
        $("#taxTemplate").text("Template")
    }
}
function CalculateTaxExemptedAmt(e) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    var RateDecDigit = $("#RateDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var OrderQty = clickedrow.find("#TxtReceivedQuantity").val();
    var ItemName = clickedrow.find("#hfItemID").val();
    var ItmRate = clickedrow.find("#TxtRate").val();
    var oc_amt = clickedrow.find("#TxtOtherCharge").val();
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (oc_amt == ".") {
        oc_amt = 0;
    }
    if (oc_amt != "" && oc_amt != ".") {
        oc_amt = parseFloat(oc_amt);
    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        debugger;
        clickedrow.find("#TxtItemGrossValue").val(FinVal);
        clickedrow.find("#NetOrderValueSpe").val(FinVal);
        FinalVal = FinVal * ConvRate

        FinalVal = FinalVal + oc_amt
        //clickedrow.find("#TxtNetValue").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        clickedrow.find("#TxtTotalValue").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount();
    }
    clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
        clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDigit));
    }
    else if (GstApplicable == "Y") {
             if (clickedrow.find("#ManualGST").is(":checked")) {
                 clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDigit));
                 CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
                 clickedrow.find("#Txtitem_tax_amt").val(parseFloat(0).toFixed(ValDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
        }
    }
    else {
        CalculateTaxAmount_ItemWise(ItemName, clickedrow.find("#TxtItemGrossValue").val());
    }
}
function CalculateTaxAmount_ItemWise(ItmCode, AssAmount) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    if (AssAmount != "" && AssAmount != null) {
        var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;//JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
            var NewArray = [];
            $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(DecDigit);
                if (TaxItemID == ItmCode) {

                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxPercentage = "";
                    TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    currentRow.find("#TaxAmount").text(TaxAmount);
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
                var TaxName = currentRow.find("#TaxName").text();
                var TaxNameID = currentRow.find("#TaxNameID").text();
                var TaxPercentage = currentRow.find("#TaxPercentage").text();
                var TaxLevel = currentRow.find("#TaxLevel").text();
                var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                var TaxAmount = currentRow.find("#TaxAmount").text();
                var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();

                NewArray.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });

            });
            $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            //$("#POInvItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().parent().each(function () {
            $("#JOInvItmDetailsTbl >tbody >tr #hfItemID[value=" + ItmCode + "]").closest("tr").each(function () {
                debugger;
                var currentRow = $(this);
                var ItemNo = currentRow.find("#hfItemID").val();
                if (ItemNo == ItmCode) {
                    //var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").closest("tr").find("#DocNo:contains('" + GrnNo + "')").closest("tr");
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                            }
                            var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val();
                            var TaxAmt = parseFloat(0).toFixed(DecDigit);
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(DecDigit)
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(DecDigit));
                                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                                currentRow.find("#NetOrderValueSpe").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                                var oc_amt = currentRow.find("#TxtOtherCharge").val();
                                if (oc_amt == ".") {
                                    oc_amt = 0;
                                }
                                if (oc_amt != "" && oc_amt != ".") {
                                    oc_amt = parseFloat(oc_amt);
                                }
                                FinalNetOrderValueBase = FinalNetOrderValueBase + oc_amt
                                NetOrderValueSpec = parseFloat(NetOrderValueSpec) + oc_amt;
                                //currentRow.find("#TxtNetValue").val(parseFloat(NetOrderValueSpec).toFixed(DecDigit));
                                currentRow.find("#TxtTotalValue").val(parseFloat(FinalNetOrderValueBase).toFixed(DecDigit));
                            }
                        });
                    }
                    else {
                        debugger;
                        var TaxAmt = parseFloat(0).toFixed(DecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                        currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(DecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(DecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(DecDigit);
                        FinalFGrossAmtOR = ConvRate * FGrossAmtOR
                        var oc_amt = currentRow.find("#TxtOtherCharge").val();
                        if (oc_amt == ".") {
                            oc_amt = 0;
                        }
                        if (oc_amt != "" && oc_amt != ".") {
                            oc_amt = parseFloat(oc_amt);
                        }
                        FinalFGrossAmtOR = FinalFGrossAmtOR + oc_amt;
                        FGrossAmtOR = FGrossAmtOR + oc_amt;
                        //currentRow.find("#TxtNetValue").val(parseFloat(FGrossAmtOR).toFixed(DecDigit));
                        currentRow.find("#TxtTotalValue").val(parseFloat(FinalFGrossAmtOR).toFixed(DecDigit));
                    }
                }
            });
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function ResetGRN_DDL_Detail() {
    debugger;
    $("#GRN_Date").val("");
    var DocNo = $('#ddlGoodReceiptNoteNo').val();

    $("#ddlGoodReceiptNoteNo>optgroup>option[value='" + DocNo + "']").select2().hide();
    $('#ddlGoodReceiptNoteNo').val("---Select---,null,null").trigger('change'); // Notify any JS components that the value changed
    $('#ddlGoodReceiptNoteNo').select2('close');

    $("#SpanGRNNoErrorMsg").css("display", "none");
    $("[aria-labelledby='select2-ddlGoodReceiptNoteNo-container']").css("border-color", "#ced4da");
    $("#ddlGoodReceiptNoteNo").css("border-color", "red");
}
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
        debugger;
        if (isConfirm) {
            debugger;
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            RemoveSessionNew();
            return true;
        } else {
            return false;
        }
    });
    return false;
}
function RemoveSessionNew() {
    sessionStorage.removeItem("JI_TaxCalcDetails");
    sessionStorage.removeItem("JI_TransType");
}

//------------------Tax Amount and Other Charge and OtherCharge Tax Amount Calculation------------------//
function OnClickTaxCalBtn(e) {
    debugger;
    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "#ced4da");
    $("#SpanTax_Template").text("");
    $("#SpanTax_Template").css("display", "none");

    var JIItemListName = "#TxtItemName";
    var SNohiddenfiled = "JI";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        var currentRow = $(e.target).closest("tr");
        if (currentRow.find("#ManualGST").is(":checked")) {
            $("#taxTemplate").text("GST Slab")
        }
        else {
            $("#taxTemplate").text("Template")
        }
    }
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, JIItemListName)
    if (GstApplicable == "Y") {
        if ($("#txtdisableManual").val() == "Y") {
            $("#Tax_Template").attr("disabled", true);
            //$("#SaveAndExitBtn").prop("disabled", true);
            $("#SaveAndExitBtn").css("display", "none");
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", false);
                //$("#SaveAndExitBtn").prop("disabled", false);
                $("#SaveAndExitBtn").css("display", "block");
            }
        }
    }
}
function OnClickSaveAndExit(OnAddGRN) {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var TotalAmount = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    
    debugger;
    let NewArr = [];
    var rowIdx = 0;
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    if ($("#" + HdnTaxCalculateTable + " >tbody >tr").length > 0) {
        $("#" + HdnTaxCalculateTable + " >tbody >tr").each(function () {
            var currentRow = $(this);
            //var TaxUserID = currentRow.find("#UserID").text(); /* commented by Suraj on 01-02-2022 due to not in use*/
            var DocNo = currentRow.find("#DocNo").text();
            var DocDate = currentRow.find("#DocDate").text();
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            if (TaxOn == "OC") {
                if (TaxItemID == TaxItmCode) {
                    $(this).remove();
                }
            }
            else {
                if (/*TaxUserID == UserID && */ TaxItemID == TaxItmCode) {
                    $(this).remove();
                }
                else {
                    //debugger;


                    var TaxName = currentRow.find("#TaxName").text();
                    var TaxNameID = currentRow.find("#TaxNameID").text();
                    var TaxPercentage = currentRow.find("#TaxPercentage").text();
                    var TaxLevel = currentRow.find("#TaxLevel").text();
                    var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
                    var TaxAmount = currentRow.find("#TaxAmount").text();
                    var TotalTaxAmount = currentRow.find("#TotalTaxAmount").text();
                    var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();

                    NewArr.push({ /*UserID: UserID, *//*DocNo: GRNNo, DocDate: GRNDate,*/ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })

                }
            }

        });

        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();
            //debugger
            $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">
            <td id="DocNo"></td>
            <td id="DocDate"></td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                </tr>`);
            //            <td id="DocNo">${GRNNo}</td>
            //<td id="DocDate">${GRNDate}</td>
            NewArr.push({ /*UserID: UserID, *//*DocNo: GRNNo, DocDate: GRNDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
    }
    else {
        //  var TaxCalculationList = [];
        $("#TaxCalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TaxName = currentRow.find("#taxname").text();
            var TaxNameID = currentRow.find("#taxid").text();
            var TaxPercentage = currentRow.find("#taxrate").text();
            var TaxLevel = currentRow.find("#taxlevel").text();
            var TaxApplyOn = currentRow.find("#taxapplyonname").text();
            var TaxAmount = currentRow.find("#taxval").text();
            var TaxApplyOnID = currentRow.find("#taxapplyon").text();

            //debugger
            $("#" + HdnTaxCalculateTable + " tbody").append(`<tr id="R${++rowIdx}">    
            <td id="DocNo"></td>
            <td id="DocDate"></td>
            <td id="TaxItmCode">${TaxItmCode}</td>
            <td id="TaxName">${TaxName}</td>
            <td id="TaxNameID">${TaxNameID}</td>
            <td id="TaxPercentage">${TaxPercentage}</td>
            <td id="TaxLevel">${TaxLevel}</td>
            <td id="TaxApplyOn">${TaxApplyOn}</td>
            <td id="TaxAmount">${TaxAmount}</td>
            <td id="TotalTaxAmount">${TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxApplyOnID}</td>
                </tr>`);
            //<td id="DocNo">${GRNNo}</td>
            //<td id="DocDate">${GRNDate}</td>
            NewArr.push({ /*UserID: UserID,*/ /*DocNo: GRNNo, DocDate: GRNDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
    }
    if (TaxOn != "OC") {
        //BindTaxAmountDeatils(NewArr, "");
        BindTaxAmountDeatils(NewArr);
    }
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            // debugger;
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(DecDigit)
                currentRow.find("#OCTotalTaxAmt").text(Total);
            }
        });
        Calculate_OCAmount();
    } else {
        $("#JOInvItmDetailsTbl >tbody >tr").each(function (i, row) {
            // debugger;
            var currentRow = $(this);
            //var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            //var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItmCode = currentRow.find("#hfItemID").val();
            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
            if (TaxAmt == "" || TaxAmt == "NaN") {
                TaxAmt = (parseFloat(0)).toFixed(DecDigit);
            }
            OC_Amt = (parseFloat(0)).toFixed(DecDigit);
            /*if (ItmCode == TaxItmCode && GRNNoTbl == GRNNo && GRNDateTbl == GRNDate) {*/
            if (ItmCode == TaxItmCode) {
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = (parseFloat(0)).toFixed(DecDigit);
                }
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                } 
                AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                TotalAmount = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(DecDigit);
                currentRow.find("#TxtTotalValue").val(TotalAmount);     
            }
            var TaxAmt1 = parseFloat(0).toFixed(DecDigit)
            var ItemTaxAmt1 = currentRow.find("#Txtitem_tax_amt").val();
            if (ItemTaxAmt1 != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();      
    }
    if ($("#taxTemplate").text() == "GST Slab") {
        GetAllGLID();
    }
}
function OnClickReplicateOnAllItems() {
    var DecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    //var GRNNo = $("#TaxCalcGRNNo").val();
    //var GRNDate = $("#TaxCalcGRNDate").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(DecDigit);
    var AssessableValue = parseFloat(0).toFixed(DecDigit);
    //var NetOrderValueSpec = parseFloat(0).toFixed(DecDigit);
    //var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);
    var TotalAmount = parseFloat(0).toFixed(DecDigit);
    var UserID = $("#UserID").text();
    //var ConvRate = $("#conv_rate").val();
    //if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
    //    ConvRate = 1;
    //}
    debugger;
    var TaxOn = $("#HdnTaxOn").val();
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();
        TaxCalculationList.push({ /*UserID: UserID,*/ /*DocNo: GRNNo, DocDate: GRNDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ /*UserID: UserID,*//* DocNo: GRNNo, DocDate: GRNDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            var TableOnwhichTaxApply = "JOInvItmDetailsTbl";
            if (TaxOn == "OC") {
                TableOnwhichTaxApply = "Tbl_OC_Deatils";
            }
            $("#" + TableOnwhichTaxApply + " >tbody >tr").each(function () {
                //debugger;
                var currentRow = $(this);
                //var GRNNoTbl;
                //var GRNDateTbl;
                var ItemCode;
                var AssessVal;
                //GRNNoTbl = currentRow.find("#TxtGrnNo").val();
                //GRNDateTbl = currentRow.find("#hfGRNDate").val();
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OCAmount").text();
                } else {
                    ItemCode = currentRow.find("#hfItemID").val();
                    AssessVal = currentRow.find("#TxtItemGrossValue").val();
                }


                var NewArray = [];
                var TotalTaxAmt = parseFloat(0).toFixed(DecDigit);
                for (i = 0; i < TaxCalculationList.length; i++) {
                    var TaxPercentage = "";
                    var TaxName = TaxCalculationList[i].TaxName;
                    var TaxNameID = TaxCalculationList[i].TaxNameID;
                    var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                    TaxPercentage = TaxCalculationList[i].TaxPercentage;
                    var TaxLevel = TaxCalculationList[i].TaxLevel;
                    var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                    var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                    var TaxAmount;
                    var TaxPec;
                    TaxPec = TaxPercentage.replace('%', '');

                    if (TaxApplyOn == "Immediate Level") {
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(DecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(DecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(DecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(DecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(DecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(DecDigit);
                        }
                    }
                    NewArray.push({ /*UserID: UserID,*/ /*DocNo: GRNNoTbl, DocDate: GRNDateTbl,*/ TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                }
                if (NewArray != null) {
                    if (NewArray.length > 0) {
                        for (k = 0; k < NewArray.length; k++) {
                            var TaxName = NewArray[k].TaxName;
                            var TaxNameID = NewArray[k].TaxNameID;
                            //var DocNo = NewArray[k].DocNo;
                            //var DocDate = NewArray[k].DocDate;
                            var TaxItmCode = NewArray[k].TaxItmCode;
                            var TaxPercentage = NewArray[k].TaxPercentage;
                            var TaxLevel = NewArray[k].TaxLevel;
                            var TaxApplyOn = NewArray[k].TaxApplyOn;
                            var TaxAmount = NewArray[k].TaxAmount;
                            var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                            if (CitmTaxItmCode != TaxItmCode /*&& GRNNo == DocNo && GRNDate == DocDate*/) {
                                TaxCalculationListFinalList.push({ /*UserID: UserID,*/ /*DocNo: DocNo, DocDate: DocDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            }
                            if (CitmTaxItmCode == TaxItmCode /*&& GRNNo != DocNo && GRNDate == DocDate*/) {
                                if (TaxOn != "OC") {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ /*DocNo: DocNo, DocDate: DocDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                            /***Modifyed by Shubham Maurya on 11-10-2023 12:49 for Replictae multiple time Tax on Other Charge***/
                            //if (CitmTaxItmCode != TaxItmCode /*&& GRNNo != DocNo && GRNDate == DocDate*/) {
                            //    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ /*DocNo: DocNo, DocDate: DocDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //}
                            //if (CitmTaxItmCode == TaxItmCode /*&& GRNNo != DocNo && GRNDate != DocDate*/) {
                            //    if (TaxOn != "OC") {
                            //        TaxCalculationListFinalList.push({ /*UserID: UserID,*/ /*DocNo: DocNo, DocDate: DocDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //    }
                            //}
                            //if (CitmTaxItmCode != TaxItmCode /*&& GRNNo != DocNo && GRNDate != DocDate*/) {
                            //    TaxCalculationListFinalList.push({ /*UserID: UserID,*/ /*DocNo: DocNo, DocDate: DocDate,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                            //}
                        }
                    }
                }
            });
        }
    }
    $("#" + HdnTaxCalculateTable + " >tbody >tr").remove();
    //sessionStorage.setItem("PI_TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
        var rowIdx = 0;
            //        <td id="DocNo">${TaxCalculationListFinalList[i].DocNo}</td>
            //<td id="DocDate">${TaxCalculationListFinalList[i].DocDate}</td>
        $('#' + HdnTaxCalculateTable + ' tbody').append(`<tr id="R${++rowIdx}">          
            <td id="DocNo"></td>
            <td id="DocDate"></td>
            <td id="TaxItmCode">${TaxCalculationListFinalList[i].TaxItmCode}</td>
            <td id="TaxName">${TaxCalculationListFinalList[i].TaxName}</td>
            <td id="TaxNameID">${TaxCalculationListFinalList[i].TaxNameID}</td>
            <td id="TaxPercentage">${TaxCalculationListFinalList[i].TaxPercentage}</td>
            <td id="TaxLevel">${TaxCalculationListFinalList[i].TaxLevel}</td>
            <td id="TaxApplyOn">${TaxCalculationListFinalList[i].TaxApplyOn}</td>
            <td id="TaxAmount">${TaxCalculationListFinalList[i].TaxAmount}</td>
            <td id="TotalTaxAmount">${TaxCalculationListFinalList[i].TotalTaxAmount}</td>
            <td id="TaxApplyOnID">${TaxCalculationListFinalList[i].TaxApplyOnID}</td>
        </tr>`);
    }
    if (TaxOn != "OC") {
        BindTaxAmountDeatils(TaxCalculationListFinalList);
    }
    //debugger;
    if (TaxOn == "OC") {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
            var OCValue = currentRow.find("#OCValue").text();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        var TaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;
                        if (OCValue == TaxItmCode) {
                            currentRow.find("#OCTaxAmt").text(TotalTaxAmtF);
                            var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                            var NetAmt = (parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)).toFixed(DecDigit)
                            currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                        }
                    }
                }
                else {
                    currentRow.find("#OCTaxAmt").text(parseFloat(0).toFixed(DecDigit));
                    var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                    var NetAmt = (parseFloat(OCAmt) + parseFloat(0)).toFixed(DecDigit)
                    currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                }
            }
        });
        Calculate_OCAmount();
    }
    else {
        $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            // debugger;
            //var GRNNoTbl = currentRow.find("#TxtGrnNo").val();
            //var GRNDateTbl = currentRow.find("#hfGRNDate").val();
            var ItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        //var AGRNNo = TaxCalculationListFinalList[i].DocNo;
                        //var AGRNDate = TaxCalculationListFinalList[i].DocDate;
                        var AItemID = TaxCalculationListFinalList[i].TaxItmCode;

                        if (ItemID == AItemID /*&& GRNNoTbl == AGRNNo && GRNDateTbl == AGRNDate*/) {
                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(DecDigit);
                            currentRow.find("#Txtitem_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(DecDigit));
                            var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                            if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                            } 
                            AssessableValue = (parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
                            //NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            //NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            //currentRow.find("#TxtNetValue").val(NetOrderValueSpec);
                            //FinalNetOrderValueBase = (NetOrderValueBase * ConvRate).toFixed(DecDigit);
                            //currentRow.find("#TxtNetValueInBase").val(FinalNetOrderValueBase);
                            TotalAmount = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(DecDigit);
                            currentRow.find("#TxtTotalValue").val(TotalAmount);
                            
                        }
                    }
                }
                else {
                    var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                    currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                    var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                    if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                        OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                    }
                    var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                    currentRow.find("#TxtTotalValue").val(FGrossAmt);
                    
                }
            }
            else {
                var GrossAmt = parseFloat(currentRow.find("#TxtItemGrossValue").val()).toFixed(DecDigit);
                currentRow.find("#Txtitem_tax_amt").val(TaxAmt);
                var OC_Amt = (parseFloat(0)).toFixed(DecDigit);
                if (currentRow.find("#TxtOtherCharge").val() != null && currentRow.find("#TxtOtherCharge").val() != "") {
                    OC_Amt = parseFloat(currentRow.find("#TxtOtherCharge").val()).toFixed(DecDigit);
                }
                var FGrossAmt = (parseFloat(OC_Amt) + parseFloat(GrossAmt)).toFixed(DecDigit);
                currentRow.find("#TxtTotalValue").val(FGrossAmt);
                
            }
        });
        CalculateAmount();
        GetAllGLID();
    }

}
function BindTaxAmountDeatils(TaxAmtDetail, bindval) {
    debugger;

    var PI_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";/*"#PI_ItemTaxAmountList"; //commented by suraj for Changed to common*/
    var PI_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";/*"#PI_ItemTaxAmountTotal"; //commented by suraj for Changed to common*/

    CMNBindTaxAmountDeatils(TaxAmtDetail, PI_ItemTaxAmountList, PI_ItemTaxAmountTotal);
    if (bindval == "") {
        GetAllGLID();
    }
}

function OnClickSaveAndExit_OC_Btn() {
    debugger;
    CMNOnClickSaveAndExit_OC_Btn('#TxtOtherCharges', "#NetOrderValue", "#NetOrderValue")
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    var DecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#TxtGrossValue").val();
    //var ConvRate = $("#conv_rate").val();
    //if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
    //    ConvRate = 1;
    //}

    $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        //var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#TxtItemGrossValue").val();
        var Total_Amount = currentRow.find("#TxtTotalValue").val();
        

        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(DecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(DecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#TxtOtherCharge").val(parseFloat(OCAmtItemWise).toFixed(DecDigit));
            }
            else {
                currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
            }
        } else {//Added by Suraj Maurya on 14-02-2025
            currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed(DecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(DecDigit);
    $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        var OCValue = currentRow.find("#TxtOtherCharge").val();
        if (OCValue != null && OCValue != "") {
            if (parseFloat(OCValue) > 0) {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(OCValue);
            }
            else {
                TotalOCAmount = parseFloat(TotalOCAmount) + parseFloat(0);
            }
        }
    });
    if (parseFloat(TotalOCAmount) > parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmount) - parseFloat(TotalOCAmt);
        var Sno = 0;
        $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            Sno = Sno + 1;
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (Sno === "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        var Sno = 0;
        $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            Sno = Sno + 1;
            var currentRow = $(this);
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#TxtOtherCharge").val();
            if (Sno == "1") {
                currentRow.find("#TxtOtherCharge").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(DecDigit));
            }
        });
    }
    $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);

        var POItm_GrossValue = currentRow.find("#TxtItemGrossValue").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(DecDigit);
        var POItm_OCAmt = currentRow.find("#TxtOtherCharge").val();
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#Txtitem_tax_amt").val() != null && currentRow.find("#Txtitem_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#Txtitem_tax_amt").val()).toFixed(DecDigit);
        }
        var Total_Amount = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        
        currentRow.find("#TxtTotalValue").val((parseFloat(Total_Amount)).toFixed(DecDigit));
        
    });
    CalculateAmount();
};
function CalculateAmount() {
     debugger;
    var DecDigit = $("#ValDigit").text();
    //var ConvRate = $("#conv_rate").val();
    //if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
    //    ConvRate = 1;
    //}
    var GrossValue = parseFloat(0).toFixed(DecDigit);
    //var AssessableValue = parseFloat(0).toFixed(DecDigit);
    var TaxValue = parseFloat(0).toFixed(DecDigit);
    var TotalAmount = parseFloat(0).toFixed(DecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(DecDigit);

    $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);

        if (currentRow.find("#TxtItemGrossValue").val() == "" || currentRow.find("#TxtItemGrossValue").val() == null || currentRow.find("#TxtItemGrossValue").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#TxtItemGrossValue").val())).toFixed(DecDigit);
        }
        if (currentRow.find("#Txtitem_tax_amt").val() == "" || currentRow.find("#Txtitem_tax_amt").val() == "0" || currentRow.find("#Txtitem_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#Txtitem_tax_amt").val())).toFixed(DecDigit);
        }

        if (currentRow.find("#TxtTotalValue").val() == "" || currentRow.find("#TxtTotalValue").val() == null || currentRow.find("#TxtTotalValue").val() == "NaN") {
            TotalAmount = (parseFloat(TotalAmount) + parseFloat(0)).toFixed(DecDigit);
        }
        else {
            TotalAmount = (parseFloat(TotalAmount) + parseFloat(currentRow.find("#TxtTotalValue").val())).toFixed(DecDigit);
        }
    });
    $("#TxtGrossValue").val(GrossValue).trigger('change');
    var oc_amount = $("#TxtDocSuppOtherCharges").val();
    NetOrderValueBase = parseFloat(TaxValue) + parseFloat(GrossValue) + parseFloat(CheckNullNumber(oc_amount));
    //NetOrderValueSpec = NetOrderValueBase / parseFloat(CheckNullNumber(ConvRate));
    //$("#TxtAssessableValue").val(AssessableValue);
    $("#TxtTaxAmount").val(parseFloat(TaxValue).toFixed(DecDigit));
    //$("#NetOrderValue").val(TotalAmount);
    $("#NetOrderValue").val(parseFloat(NetOrderValueBase).toFixed(DecDigit));
   
}
function SetOtherChargeVal() {
    $("#JOInvItmDetailsTbl > tbody > tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TxtOtherCharge").val(parseFloat(0).toFixed($("#ValDigit").text()));
    })
}
function BindOtherChargeDeatils(val) {
    //debugger;
    var DecDigit = $("#ValDigit").text();

    //var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(DecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(DecDigit);
    $("#Tbl_OtherChargeList >tbody >tr").remove();
    //$("#PI_OtherChargeTotal").text(parseFloat(0).toFixed(DecDigit));
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;  
            var td = "";
           /* if (DocumentMenuId == "105101145") {*/
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
                      <td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
           /* }*/

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td >${currentRow.find("#OCName").text()}</td>
<td >${currentRow.find("#td_OCSuppName").text()}</td>
<td id="OCAmtSp1" align="right">${currentRow.find("#OCAmount").text()}</td>
<td hidden="hidden" id="OCID">${currentRow.find("#OCValue").text()}</td>
`+ td + `
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(DecDigit);
           /* if (DocumentMenuId == "105101145") {*/
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(DecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(DecDigit);
         /*   }*/

        });
       
    }
   
    //$("#SI_OtherChargeTotal").text(TotalAMount);
    $("#_OtherChargeTotal").text(TotalAMount);
   /* if (DocumentMenuId == "105101145") {*/
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    /*}*/
    if (val == "") {
        GetAllGLID();
    }

}
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//------------------End------------------//
//--------------Gl Detail Start--------------//
//function CalculateVoucherTotalAmount() {
//    debugger;
//    var ValDecDigit = $("#ValDigit").text();
//    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    $("#VoucherDetail >tbody >tr").each(function () {
//        //debugger;
//        var currentRow = $(this);
//        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
//        }
//        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//        }
//        else {
//            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
//        }
//    });
//    debugger;
//    $("#DrTotal").text(DrTotAmt);
//    $("#CrTotal").text(CrTotAmt);
//    debugger;
//    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//        debugger;
//        AddRoundOffGL();
//    }
//}
//function AddRoundOffGL() {
//    debugger;
//    var ValDecDigit = $("#ValDigit").text();///Amount
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({}),
//        success: function (data) {
//            debugger;
//            if (data == 'ErrorPage') {
//                SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                debugger;
//                if (arr.Table.length > 0) {
//                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//                    $("#VoucherDetail >tbody >tr").each(function () {
//                        //debugger;
//                        var currentRow = $(this);
//                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
//                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//                        }
//                        else {
//                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
//                        }
//                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
//                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
//                        }
//                        else {
//                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
//                        }
//                    });
//                    debugger;
//                    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//                        if (parseFloat(DrTotAmt) < parseFloat(CrTotAmt)) {
//                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;">
//                                     <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                     <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="RO"/></td>
//                            </tr>`);
//                                }
//                            }
//                        }
//                        else {
//                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
//                            var rowIdx = $('#VoucherDetail tbody tr').length;
//                            for (var j = 0; j < arr.Table.length; j++) {
//                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>  
//                                    <td style="display: none;">
//                                     <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                     <input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="RO"/></td>
//                            </tr>`);
//                                }
//                            }
//                        }
//                    }
//                    debugger;
//                    CalculateVoucherTotalAmount();
//                }
//            }
//        }
//    });
//}
function BindDDLAccountList() {
    debugger;
    Cmn_BindAccountList("#Acc_name_", "1", "#VoucherDetail", "#SNohf", "BindData", "105108123");
}
function BindData() {
    var AccountListData = JSON.parse(sessionStorage.getItem("accountList"));
    if (AccountListData != null) {
        if (AccountListData.length > 0) {
            $("#VoucherDetail >tbody >tr").each(function () {
                var currentRow = $(this);
                var rowid = currentRow.find("#SNohf").val();
                rowid = parseFloat(rowid) ;
                if (rowid > $("#VoucherDetail >tbody >tr").length) {
                    return false;
                }
                var type = currentRow.find("#type").val();
                if (type == "Itm") {
                    $("#Acc_name_" + rowid).empty();
                    $("#Acc_name_" + rowid).append(`<optgroup class='def-cursor' id="Textddl${rowid}" label='${$("#AccName").text()}'}'></optgroup>`);
                    for (var i = 0; i < AccountListData.length; i++) {
                        $('#Textddl' + rowid).append(`<option value="${AccountListData[i].acc_id}">${AccountListData[i].acc_name}</option>`);
                    }
                    var firstEmptySelect = true;
                    $("#Acc_name_" + rowid).select2({

                        templateResult: function (data) {
                            var selected = $("#Acc_name_" + rowid).val();
                            if (check(data, selected, "#VoucherDetail", "#SNohf", "#Acc_name_") == true) {
                                /* var UOM = $(data.element).data('uom');*/
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    /*'<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +*/
                                    '</div>'
                                );
                                return $result;
                            }
                            firstEmptySelect = false;
                        }
                    });
                }
            });
        }
    }
    $("#VoucherDetail >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var AccID = currentRow.find("#hfAccID").val();
        var rowid = currentRow.find("#SNohf").val();
        var type = currentRow.find("#type").val();
        //var AccountID = '#Acc_name_' + rowid;
        if (AccID != '0' && AccID != "" && type =='Itm') {
            currentRow.find("#Acc_name_" + rowid).val(AccID).trigger('change.select2');
        }

    });

}
function RateFloatValueonly(el, evt) {
    debugger;
    if (CmnAmtFloatVal(el, evt, "#RateDigit") == false) {
        return false;
    }
    else {
        return true;
    }

}
function OnChangeAccountName(RowID, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Acc_ID;
    var SNo = clickedrow.find("#SNohf").val();
    Acc_ID = clickedrow.find("#Acc_name_" + SNo).val();
    var hdn_acc_id = clickedrow.find("#hfAccID").val();
    if (hdn_acc_id != Acc_ID || hdn_acc_id == "") {
        var Len = $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").length;
        if (Len > 0) {
            $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + hdn_acc_id + ")").closest('tr').remove();
        }
        $("#JOInvItmDetailsTbl >tbody >tr #hdn_item_gl_acc[value='" + hdn_acc_id + "']").closest('tr').each(function () {
            var row = $(this);
            row.find("#hdn_item_gl_acc").val(Acc_ID);
        });
    }
    if (Acc_ID.substring(0, 1) == "3" || Acc_ID.substring(0, 1) == "4") {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", false);
    }
    else {
        clickedrow.find("#BtnCostCenterDetail").attr("disabled", true);
    }
    clickedrow.find("#hfAccID").val(Acc_ID);
    $("#hdnAccID").val(Acc_ID);
}


//-----------------Item Detail Section Start-----------------------//
function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var DecDigit = $("#ValDigit").text();
    /*var INSDTransType = sessionStorage.getItem("INSTransType");*/
    var JINum = $("#JobInvoiceNumber").val();
    
    if (JINum == null || JINum == "") {
        $("#hdtranstype").val("Save");
        $("#hdn_TransType").val($("#hdtranstype").val());
    }
    else {
        $("#hdtranstype").val("Update");
        $("#hdn_TransType").val($("#hdtranstype").val());
    }
    if (CheckJI_Validations() == false) {
        return false;
    }
    if (CheckJI_ItemValidations() == false) {
        swal("", $("#noitemselectedmsg").text(), "warning");
        return false;
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (Cmn_taxVallidation("JOInvItmDetailsTbl", "Txtitem_tax_amt", "hfItemID", "Hdn_TaxCalculatorTbl", "TxtItemName") == false) {
            return false;
        }
    }
    if (CheckJI_VoucherValidations() == false) {
        return false;
    }
    //if ($("#Cancelled").is(":checked")) {
    //    Cancelled = "Y";
    //}
    //else {
    //    Cancelled = "N";
    //}
   
    
    debugger;
    $("#SupplierName").attr("disabled", false);

    var Narration = $("#DebitNoteRaisedAgainstInv").text()
    $('#hdNarration').val(Narration);

    var FinalItemDetail = [];
    FinalItemDetail = InsertJIItemDetails();
    var str = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(str);

    var TaxDetail = [];
    TaxDetail = InsertTaxDetails();
    var str_TaxDetail = JSON.stringify(TaxDetail);
    $('#hdItemTaxDetail').val(str_TaxDetail);

    var TdsDetail = [];
    TdsDetail = InsertTdsDetails();
    var str_TdsDetail = JSON.stringify(TdsDetail);
    $('#hdn_tds_details').val(str_TdsDetail);

    var OC_TaxDetail = [];
    OC_TaxDetail = OC_InsertTaxDetails();
    var str_OC_TaxDetail = JSON.stringify(OC_TaxDetail);
    $('#hdOC_ItemTaxDetail').val(str_OC_TaxDetail);

    var OCDetail = [];
    OCDetail = GetJI_OtherChargeDetails();
    var str_OCDetail = JSON.stringify(OCDetail);
    $('#hdItemOCDetail').val(str_OCDetail);

    var vou_Detail = [];
    vou_Detail = GetJI_VoucherDetails();
    var str_vou_Detail = JSON.stringify(vou_Detail);
    $('#hdItemvouDetail').val(str_vou_Detail);

    var FinalCostCntrDetails = [];
    FinalCostCntrDetails = Cmn_InsertCCDetails();
    var CCDetails = JSON.stringify(FinalCostCntrDetails);
    $('#hdn_CC_DetailList').val(CCDetails);

    var Final_OC_TdsDetails = [];
    Final_OC_TdsDetails = Cmn_Insert_OC_Tds_Details();
    var Oc_Tds_Details = JSON.stringify(Final_OC_TdsDetails);
    $('#hdn_oc_tds_details').val(Oc_Tds_Details);

    if (Cmn_CC_DtlSaveButtonClick("VoucherDetail", "dramt1", "cramt1","hfStatus") == false) {
        return false;
    }

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $("#hdn_Attatchment_details").val(ItemAttchmentDt);
        /*----- Attatchment End--------*/

    $("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
    var SuppName = $("#SupplierName").val();
    $("#Hdn_SupplierName").val(SuppName);
    var GRNNum = $("#ddlGoodReceiptNoteNo").val();
    var grn_num = GRNNum.split(',');
    var GRNNo = grn_num[0];
    $("#Hdn_GRNNumber").val(GRNNo);
    var SuppName = $("#SupplierName option:selected").text();
    $("#Hdn_JISupplierName").val(SuppName)

    $("#hdnsavebtn").val("AllreadyclickSaveBtn");
   return true;
}

function CheckJI_Validations() {
    debugger;
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SupplierName").css("border-color", "Red");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
        $("#SupplierName").css("border-color", "#ced4da");
    }
    if ($("#Bill_No").val() == null || $("#Bill_No").val() == "") {
        $("#Bill_No").css("border-color", "Red");
        $('#SpanBillNoErrorMsg').text($("#valueReq").text());
        $("#SpanBillNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanBillNoErrorMsg").css("display", "none");
        $("#Bill_No").css("border-color", "#ced4da");
    }
    if ($("#Bill_Date").val() == "") {
        $('#SpanBillDateErrorMsg').text($("#valueReq").text());
        $("#SpanBillDateErrorMsg").css("display", "block");
        $("#Bill_Date").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#Bill_Date").css("border-color", "#ced4da");
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else
    {
       return true;
    }




}
function CheckJI_ItemValidations() {
    debugger;
    var ErrorFlag = "N";
    if ($("#JOInvItmDetailsTbl >tbody >tr").length > 0) {

    }
    else {
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckJI_VoucherValidations() {
    debugger;

    if (Cmn_CheckGlVoucherValidations() == false) {/*Added by Suraj on 07-08-2024*/
        return false;
    } else {
        return true;
    }

    /*Commented by Suraj on 07-08-2024 to add new common gl validations*/

    //var ErrorFlag = "N";
    //var ValDigit = $("#ValDigit").text();
    //var DrTotal = $("#DrTotal").text();
    //var CrTotal = $("#CrTotal").text();
    //$("#VoucherDetail >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    //var AccID = currentRow.find("#hfAccID").val();
    //    var rowid = currentRow.find("#SNohf").val();
    //    var AccountID = '#Acc_name_' + rowid;
    //    var AccID = currentRow.find('#Acc_name_' + rowid).val();
    //    if (AccID != '0' && AccID != "") {
    //        ErrorFlag = "N";
    //    }
    //    else {
    //        swal("", $("#GLPostingNotFound").text(), "warning");
    //        ErrorFlag = "Y";
    //        return false;
    //    }

    //});
    //debugger;
    //if (DrTotal == '' || DrTotal == 'NaN') {
    //    DrTotal = 0;
    //}
    //if (CrTotal == '' || CrTotal == 'NaN') {
    //    CrTotal = 0;
    //}

    //debugger;
    //if (DrTotal == CrTotal && DrTotal != "0" && CrTotal != "0") {
    //}
    //else {
    //    swal("", $("#DebtCredtAmntMismatch").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}
    //if (DrTotal == parseFloat(0).toFixed(ValDigit) && CrTotal == parseFloat(0).toFixed(ValDigit)) {
    //    swal("", $("#GLPostingNotFound").text(), "warning");
    //    ErrorFlag = "Y";
    //}


    //if (ErrorFlag == "Y") {
    //    return false;
    //}
    //else {
    //    return true;
    //}
}

function InsertJIItemDetails() {
    var JI_ItemsDetail = [];
    $("#JOInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var mr_no = "";
        var mr_date = "";
        var item_id = "";
        var item_name = "";
        var uom_id = "";
        var jiinv_qty = "";
        var item_rate = "";
        var item_gr_val = "";
        var item_tax_amt = "";
        var item_oc_amt = "";
        var item_net_val = "";
        //var item_net_val_bs = "";
        var gl_vou_no = null;
        var gl_vou_dt = null;
        var TaxExempted = "";
        var hsn_code = "";
        var ManualGST = "";
        var ClaimITC = "";
        var item_acc_id = "";

        var currentRow = $(this);
        mr_no = currentRow.find("#src_doc_no").val();
        mr_date = currentRow.find("#src_doc_dt").val();
        //var GRNDP = mr_dt.split("-");
       // mr_date = (GRNDP[2] + "-" + GRNDP[1] + "-" + GRNDP[0]);
        item_id = currentRow.find("#hfItemID").val();
        item_name = currentRow.find("#TxtItemName").val();
        uom_id = currentRow.find("#hfUOMID").val();
        jiinv_qty = currentRow.find("#TxtReceivedQuantity").val();
        item_rate = currentRow.find("#TxtRate").val();
        item_gr_val = currentRow.find("#TxtItemGrossValue").val();
        item_tax_amt = currentRow.find("#Txtitem_tax_amt").val();
        if (item_tax_amt == "" && item_tax_amt == null) {
            item_tax_amt = "0";
        }
        item_oc_amt = currentRow.find("#TxtOtherCharge").val();
        if (item_oc_amt == "" && item_oc_amt == null) {
            item_oc_amt = "0";
        }
        item_net_val = currentRow.find("#TxtTotalValue").val();
        //item_net_val_bs = currentRow.find("#TxtNetValueInBase").val();
        gl_vou_no = null;
        gl_vou_dt = null;
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxExempted = "Y"
        }
        else {
            TaxExempted = "N"
        }
        hsn_code = currentRow.find("#ItemHsnCode").val();
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (currentRow.find("#ManualGST").is(":checked")) {
                ManualGST = "Y"
            }
            else {
                ManualGST = "N"
            }
            if (currentRow.find("#ClaimITC").is(":checked")) {
                ClaimITC = "Y"
            }
            else {
                ClaimITC = "N"
            }
        }
        else {
            ManualGST = "N";
            ClaimITC = "Y"
        }
        item_acc_id = currentRow.find("#hdn_item_gl_acc").val();
        JI_ItemsDetail.push({
            mr_no: mr_no, mr_date: mr_date, item_id: item_id, item_name: item_name, uom_id: uom_id, jiinv_qty: jiinv_qty, item_rate: item_rate, item_gr_val: item_gr_val,
            item_tax_amt: item_tax_amt, item_oc_amt: item_oc_amt, item_net_val: item_net_val, gl_vou_no: gl_vou_no, gl_vou_dt: gl_vou_dt,
            TaxExempted: TaxExempted, hsn_code: hsn_code, ManualGST: ManualGST, ClaimITC: ClaimITC, item_acc_id: item_acc_id
        });
    });
    return JI_ItemsDetail;
}
function InsertTaxDetails() {
    debugger;
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var TaxDetails = [];
    $("#JOInvItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var ItmCode = currentRow.find("#hfItemID").val();
            var TxtGrnNo = currentRow.find("#src_doc_no").val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    //$("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").find("#DocNo:contains('" + TxtGrnNo + "')").closest("tr").each(function () {
                        debugger;
                        var mr_no = "";
                        var mr_date = "";
                        var item_id = "";
                        var tax_id = "";
                        var TaxName = "";
                        var tax_rate = "";
                        var tax_level = "";
                        var tax_val = "";
                        var tax_apply_on = "";
                        var tax_apply_onName = "";
                        var totaltax_amt = "";
                        var tax_recov = "";

                        var currentRow = $(this);
                        mr_no = currentRow.find("#DocNo").text();
                        mr_date = currentRow.find("#DocDate").text();
                        item_id = currentRow.find("#TaxItmCode").text();
                        tax_id = currentRow.find("#TaxNameID").text();
                        TaxName = currentRow.find("#TaxName").text().trim();
                        tax_rate = currentRow.find("#TaxPercentage").text();
                        tax_level = currentRow.find("#TaxLevel").text();
                        tax_val = currentRow.find("#TaxAmount").text();
                        tax_apply_on = currentRow.find("#TaxApplyOnID").text();
                        tax_apply_onName = currentRow.find("#TaxApplyOn").text();
                        totaltax_amt = currentRow.find("#TotalTaxAmount").text();
                        tax_recov = currentRow.find("#TaxRecov").text();

                        TaxDetails.push({ mr_no: mr_no, mr_date: mr_date, item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt, tax_recov: tax_recov });
                    });
                }
            }
        }
    });
    return TaxDetails;
    //$("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
    //    debugger;
    //    //var mr_no = "";
    //    //var mr_date = "";
    //    var item_id = "";
    //    var tax_id = "";
    //    var tax_rate = "";
    //    var tax_level = "";
    //    var tax_val = "";
    //    var tax_apply_on = "";

    //    var currentRow = $(this);
    //    //mr_no = currentRow.find("#DocNo").text();
    //    //mr_date = currentRow.find("#DocDate").text();
    //    item_id = currentRow.find("#TaxItmCode").text();
    //    tax_id = currentRow.find("#TaxNameID").text();
    //    tax_rate = currentRow.find("#TaxPercentage").text();
    //    tax_level = currentRow.find("#TaxLevel").text();
    //    tax_val = currentRow.find("#TaxAmount").text();
    //    tax_apply_on = currentRow.find("#TaxApplyOnID").text();

    //    TaxDetails.push({/* mr_no: mr_no, mr_date: mr_date,*/ item_id: item_id, tax_id: tax_id, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on });
    //});
    //return TaxDetails;
}
function InsertTdsDetails() {
    debugger;
    var TDS_Details = [];
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        debugger;

        var Tds_id = "";
        var Tds_rate = "";
        var Tds_level = "";
        var Tds_val = "";
        var Tds_apply_on = "";
        var Tds_name = "";
        var Tds_applyOnName = "";
        var Tds_totalAmnt = "";
        var Tds_AssValApplyOn = "";

        var currentRow = $(this);

        Tds_id = currentRow.find("#td_TDS_NameID").text();
        Tds_rate = currentRow.find("#td_TDS_Percentage").text();
        Tds_level = currentRow.find("#td_TDS_Level").text();
        Tds_val = currentRow.find("#td_TDS_Amount").text();
        Tds_apply_on = currentRow.find("#td_TDS_ApplyOnID").text();
        Tds_name = currentRow.find("#td_TDS_Name").text();
        Tds_applyOnName = currentRow.find("#td_TDS_ApplyOn").text();
        Tds_totalAmnt = currentRow.find("#td_TDS_BaseAmt").text();
        Tds_AssValApplyOn = currentRow.find("#td_TDS_AssValApplyOn").text();

        TDS_Details.push({
            Tds_id: Tds_id, Tds_rate: Tds_rate, Tds_level: Tds_level, Tds_val: Tds_val
            , Tds_apply_on: Tds_apply_on, Tds_name: Tds_name, Tds_applyOnName: Tds_applyOnName
            , Tds_totalAmnt: Tds_totalAmnt, Tds_AssValApplyOn: Tds_AssValApplyOn
        });
    });
    return TDS_Details;
}
function OC_InsertTaxDetails() {
    debugger;
    var TaxDetails = [];
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        debugger;
        var mr_no = "";
        var mr_date = "";
        var item_id = "";
        var tax_id = "";
        var TaxName = "";
        var tax_rate = "";
        var tax_level = "";
        var tax_val = "";
        var tax_apply_on = "";
        var tax_apply_onName = "";
        var totaltax_amt = "";

        var currentRow = $(this);
        mr_no = "";//currentRow.find("#DocNo").text();
        mr_date = "";//currentRow.find("#DocDate").text();
        item_id = currentRow.find("#TaxItmCode").text();
        tax_id = currentRow.find("#TaxNameID").text();
        TaxName = currentRow.find("#TaxName").text().trim();
        tax_rate = currentRow.find("#TaxPercentage").text();
        tax_level = currentRow.find("#TaxLevel").text();
        tax_val = currentRow.find("#TaxAmount").text();
        tax_apply_on = currentRow.find("#TaxApplyOnID").text();
        tax_apply_onName = currentRow.find("#TaxApplyOn").text();
        totaltax_amt = currentRow.find("#TotalTaxAmount").text();

        TaxDetails.push({ mr_no: mr_no, mr_date: mr_date, item_id: item_id, tax_id: tax_id, TaxName: TaxName, tax_rate: tax_rate, tax_level: tax_level, tax_val: tax_val, tax_apply_on: tax_apply_on, tax_apply_onName: tax_apply_onName, totaltax_amt: totaltax_amt });
    });
    return TaxDetails;
}
function GetJI_OtherChargeDetails() {
    debugger;
    var JI_OCList = [];
    //if ($("#ht_Tbl_OC_Deatils >tbody >tr").length > 0) {
    //    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function () {  
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var oc_id = "";
            var oc_val = "";
            var OCTaxAmt = "";
            var OCTotalTaxAmt = "";
            var curr_id = "";
            var supp_id = "";
            oc_id = currentRow.find("#OCValue").text();
            oc_val = currentRow.find("#OCAmount").text();
            OCTaxAmt = currentRow.find("#OCTaxAmt").text();
            OCTotalTaxAmt = currentRow.find("#OCTotalTaxAmt").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            curr_id = currentRow.find("#HdnOCCurrId").text();
            supp_id = currentRow.find("#td_OCSuppID").text();
            let supp_type = currentRow.find("#td_OCSuppType").text(); // Added by Suraj on 12-04-2024
            let bill_no = currentRow.find("#OCBillNo").text(); // Added by Suraj on 12-04-2024
            let bill_date = currentRow.find("#OCBillDt").text(); // Added by Suraj on 12-04-2024
            let tds_amt = currentRow.find("#OC_TDSAmt").text(); // Added by Suraj on 03-07-2024
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            var OC_RoundOff = currentRow.find("#oc_chk_roundoff").is(":checked") == true ? "Y" : "N";
            var OC_PM_Flag = currentRow.find("#oc_p_round").is(":checked") == true ? "P" :
                currentRow.find("#oc_m_round").is(":checked") == true ? "M" : "";
            /* Added by Suraj Maurya on 03-12-2024 for third party round off */
            JI_OCList.push({
                oc_id: oc_id, oc_val: oc_val, tax_amt: OCTaxAmt, total_amt: OCTotalTaxAmt, OCName: OCName
                , OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, supp_id: supp_id, curr_id: curr_id
                , supp_type: supp_type, bill_no: bill_no, bill_date: bill_date, tds_amt: tds_amt
                , round_off: OC_RoundOff, pm_flag: OC_PM_Flag
            });
        });
    }
    return JI_OCList;
};
function GetJI_VoucherDetails() {
    debugger;
    var JI_VouList = [];
    var SuppVal = 0;
    var SuppValInBase = 0;
    var Compid = $("#CompID").text();
    var InvType = "D";
    var TransType = "JobInv";
    if ($("#VoucherDetail >tbody >tr").length > 0) {
        $("#VoucherDetail >tbody >tr").each(function () {
            var currentRow = $(this);
            //var acc_id = "";
            //var acc_name = "";
            //var dr_amt = "";
            //var cr_amt = "";
            //var Gltype = "";
            //acc_id = currentRow.find("#hfAccID").val();
            //acc_name = currentRow.find("#txthfAccID").val();
            //dr_amt = currentRow.find("#dramt").text();
            //cr_amt = currentRow.find("#cramt").text();
            //Gltype = currentRow.find("#type").val();
            //JI_VouList.push({ comp_id: Compid, id: acc_id, acc_name: acc_name, type: "I", doctype: InvType, Value: SuppVal, DrAmt: dr_amt, CrAmt: cr_amt, Gltype: Gltype, TransType: TransType });
            var acc_id = "";
            var acc_name = "";
            var dr_amt = "";
            var cr_amt = "";
            var dr_amt_bs = "";
            var cr_amt_bs = "";
            var Gltype = "";
            //var bill_date = "";

            let VouSrNo = currentRow.find("#td_vou_sr_no").text();
            let GlSrNo = currentRow.find("#td_GlSrNo").text();
            acc_id = currentRow.find("#hfAccID").val();
            acc_name = currentRow.find("#txthfAccID").val();
            dr_amt = currentRow.find("#dramt").text();
            cr_amt = currentRow.find("#cramt").text();
            dr_amt_bs = currentRow.find("#dramtInBase").text();
            cr_amt_bs = currentRow.find("#cramtInBase").text();
            Gltype = currentRow.find("#type").val();
            var curr_id = currentRow.find("#gl_curr_id").val();
            var conv_rate = currentRow.find("#gl_conv_rate").val();
            var vou_type = currentRow.find("#glVouType").val();
            var bill_no = currentRow.find("#gl_bill_no").val();
            var bill_date = currentRow.find("#gl_bill_date").val();
            var gl_narr = currentRow.find("#gl_narr").text();
            if (bill_date == "null") {
                bill_date = "";
            }
            JI_VouList.push({
                comp_id: Compid, VouSrNo: VouSrNo, GlSrNo: GlSrNo, id: IsNull(acc_id, '0'), acc_name: acc_name
                , type: "I", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
                , DrAmt: dr_amt, CrAmt: cr_amt, DrAmtInBase: dr_amt_bs, CrAmtInBase: cr_amt_bs
                , Gltype: IsNull(Gltype, '0'), TransType: TransType, curr_id: curr_id, conv_rate: conv_rate
                , vou_type: vou_type, bill_no: bill_no, bill_date: bill_date, gl_narr: gl_narr
            });
        });
    }
    return JI_VouList;
};
/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    //var JIStatus = "";
    //JIStatus = $('#hfStatus').val().trim();
    //if (JIStatus === "D" || JIStatus === "F") {

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

    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var jinvDt = $("#JobInvoiceDate").val();
        $.ajax({
            type: "POST",
            /* url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: jinvDt
            },
            success: function (data) {
                /*  if (data == "Exist") { *//*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var JIStatus = "";
                    JIStatus = $('#hfStatus').val().trim();
                    if (JIStatus === "D" || JIStatus === "F") {

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
                    /* swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                    /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
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
    /*End to chk Financial year exist or not*/
    return false;
}
 function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var JINo = "";
    var JIDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status = "";


    docid = $("#DocumentMenuId").val();
    JINo = $("#JobInvoiceNumber").val();
    JIDate = $("#JobInvoiceDate").val();
    WF_Status = $("#WF_Status1").val();
    $("#hdDoc_No").val(JINo);
    Remarks = $("#fw_remarks").val();
    var VoucherNarr = $("#PurchaseVoucherRaisedAgainstInv").text()
    var ListFilterData1 = $("#ListFilterData1").val();
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("td:eq(3)").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("td:eq(3)").text();
            level = currentRow.find("td:eq(4)").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && JINo != "" && JIDate != "" && level != "") {
            debugger;
               Cmn_InsertDocument_ForwardedDetail(JINo, JIDate, docid, level, forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/JobInvoiceSC/ToRefreshByJS";
            var list = [{ JINo: JINo, JIDate: JIDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/JobInvoiceSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
    if (fwchkval === "Approve") {
        var list = [{ JINo: JINo, JIDate: JIDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/JobInvoiceSC/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&VoucherNarr=" + VoucherNarr +  "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/JobInvoiceSC/SIListApprove?SI_No=" + JINo + "&SI_Date=" + JIDate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && JINo != "" && JIDate != "") {
            Cmn_InsertDocument_ForwardedDetail(JINo, JIDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            var list = [{ JINo: JINo, JIDate: JIDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/JobInvoiceSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && JINo != "" && JIDate != "") {
             Cmn_InsertDocument_ForwardedDetail(JINo, JIDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/JobInvoiceSC/ToRefreshByJS";
            var list = [{ JINo: JINo, JIDate: JIDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/JobInvoiceSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#JobInvoiceNumber").val();
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

/*-----------End Workflow--------------*/
function CheckedCancelled() {
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return SaveBtnClick()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}

//-------------Cost Center Section----------//

function Onclick_CCbtn(flag, e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    //var CstDbAmt = clickedrow.find("#dramt").text(); commented by Suraj on 30-03-2024
    //var CstCrtAmt = clickedrow.find("#cramt").text();
    var CstDbAmt = clickedrow.find("#dramtInBase").text();
    var CstCrtAmt = clickedrow.find("#cramtInBase").text();
    var vou_sr_no = clickedrow.find("#td_vou_sr_no").text();
    var gl_sr_no = clickedrow.find("#td_GlSrNo").text();
    var RowNo = clickedrow.find("#SNohf").val();

    var GLAcc_Name = clickedrow.find("#Acc_name_" + RowNo + " option:selected").text();
    var GLAcc_id = clickedrow.find("#Acc_name_" + RowNo + " option:selected").val();
    if (GLAcc_Name == null || GLAcc_Name == "") {
        GLAcc_Name = clickedrow.find("#txthfAccID").val();
        GLAcc_id = clickedrow.find("#hfAccID").val();
    }
    var disableflag = ($("#txtdisable").val());
    var NewArr = new Array();
    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
        var row = $(this);
        let cc_vou_sr_no = row.find("#hdntbl_vou_sr_no").text();
        if (cc_vou_sr_no == vou_sr_no) {
            var List = {};
            List.GlAccount = row.find("#hdntbl_GlAccountId").text();
            List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
            List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
            List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
            List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
            List.vou_sr_no = row.find('#hdntbl_vou_sr_no').text();
            List.gl_sr_no = row.find('#hdntbl_gl_sr_no').text();
            var amount = row.find("#hdntbl_CstAmt").text();
            List.CC_Amount = parseFloat(cmn_ReplaceCommas(amount)).toFixed(ValDigit);
            NewArr.push(List);
        }
    })
    var Amt;
    if (CheckNullNumber(CstDbAmt) != "0") {
        Amt = CstDbAmt;
    }
    else {
        Amt = CstCrtAmt;
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/JobInvoiceSC/GetCstCntrtype",
        data: {
            Flag: flag,
            Disableflag: disableflag,
            CC_rowdata: JSON.stringify(NewArr),
        },
        success: function (data) {
            debugger;
            $("#CostCenterDetailPopup").html(data);
            $("#CC_vou_sr_no").val(vou_sr_no);
            $("#CC_gl_sr_no").val(gl_sr_no);
            $("#CC_GLAccount").val(GLAcc_Name);
            $("#hdnGLAccount_Id").val(GLAcc_id);
            $("#GLAmount").val(Amt);
            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
            $("#hdnTable_Id").text("VoucherDetail");
        },
    })
}

//function Onclick_CCbtn(flag, e) {
//    debugger;
//    var clickedrow = $(e.target).closest("tr");
//    var CstDbAmt = clickedrow.find("#dramt").text();
//    var CstCrtAmt = clickedrow.find("#cramt").text();
//    var GLAcc_Name = clickedrow.find("#txthfAccID").val();
//    var GLAcc_id = clickedrow.find("#hfAccID").val();
//    var disableflag = ($("#txtdisable").val());
//    var NewArr = new Array();
//    $("#tbladdhdn > tbody > tr #hdntbl_GlAccountId:contains(" + GLAcc_id + ")").closest('tr').each(function () {
//        var row = $(this);
//        var List = {};
//        List.GlAccount = row.find("#hdntbl_GlAccountId").text();
//        List.ddl_CC_Type = row.find("#hdntbl_CCtype").text();
//        List.ddl_Type_Id = row.find('#hdntbl_CCtype_Id').text();
//        List.ddl_CC_Name = row.find('#hdntbl_CC_Name').text();
//        List.ddl_Name_Id = row.find('#hdntbl_CCName_Id').text();
//        var amount = row.find("#hdntbl_CstAmt").text();
//        List.CC_Amount = parseFloat(amount).toFixed(ValDigit);
//        NewArr.push(List);
//    })
//    var Amt;
//    if (CheckNullNumber(CstDbAmt) != "0") {
//        Amt = CstDbAmt;
//    }
//    else {
//        Amt = CstCrtAmt;
//    }
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/JobInvoiceSC/GetCstCntrtype",
//        data: {
//            Flag: flag,
//            Disableflag: disableflag,
//            CC_rowdata: JSON.stringify(NewArr),
//        },
//        success: function (data) {
//            debugger;
//            $("#CostCenterDetailPopup").html(data);
//            $("#CC_GLAccount").val(GLAcc_Name);
//            $("#hdnGLAccount_Id").val(GLAcc_id);
//            $("#GLAmount").val(Amt);
//            $("#ddl_CC_Name").append(`<option value="0">---Select---</option>`);
//            $("#hdnTable_Id").text("VoucherDetail");
//        },
//    })
//}

//-------------------Cost Center Section End-------------------//


/*----------------------------TDS Section-----------------------------*/
function OnClickTDSCalculationBtn() {
    let GrVal = $("#TxtGrossValue").val();
    //const OCVal = $("#TxtOtherCharges").val();/***Modifyed By Shubham maurya on 13-08-2024***/
    let ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    let NetVal = $("#NetOrderValue").val();
        //st OCVal = $("#TxtOtherCharges").val();/***Modifyed By Shubham maurya on 13-08-2024***/
    let ToTdsAmt_IT = parseFloat(CheckNullNumber(NetVal));
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, null, null, ToTdsAmt_IT);

}
function OnClickTDS_SaveAndExit() {
    debugger
    if ($("#hdn_tds_on").val() == "OC") {
        OnClickTP_TDS_SaveAndExit();
        $("#hdn_tds_on").val("");
    } else {

        var DecDigit = $("#ValDigit").text();
        var TotalTDS_Amount = $('#TotalTDS_Amount').text();
        var ConvRate = $("#conv_rate").val();
        if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
            ConvRate = 1;
        }
        //Added by Suraj Maurya on 06-12-2024
        let TdsAssVal_applyOn = "ET";
        if ($("#TdsAssVal_IncluedTax").is(":checked")) {
            TdsAssVal_applyOn = "IT";
        }
        //Added by Suraj Maurya on 06-12-2024 End
        //let NewArr = [];
        var rowIdx = 0;
        var GstCat = $("#Hd_GstCat").val();
        //Commented Code Removed At 09-12-2024 by Suraj Maurya
        if ($("#Hdn_TDS_CalculatorTbl >tbody >tr").length > 0) {

            $("#Hdn_TDS_CalculatorTbl >tbody >tr").remove();
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $('#Hdn_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
            });
        }
        else {
            $("#TDS_CalculatorTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var TDS_Name = currentRow.find("#TDS_name").text();
                var TDS_NameID = currentRow.find("#TDS_id").text();
                var TDS_Percentage = currentRow.find("#TDS_rate").text();
                var TDS_Level = currentRow.find("#TDS_level").text();
                var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                var TDS_Amount = currentRow.find("#TDS_val").text();
                var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
                var TDS_AccId = currentRow.find("#TDS_acc_id").text();
                var ToTdsAmt = currentRow.find("#TDS_base_amt").text();

                $("#Hdn_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);

                //var currentRow = $(this);
                //var TDS_Name = currentRow.find("#TDS_name").text();
                //var TDS_NameID = currentRow.find("#TDS_id").text();
                //var TDS_Percentage = currentRow.find("#TDS_rate").text();
                //var TDS_Level = currentRow.find("#TDS_level").text();
                //var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
                //var TDS_Amount = currentRow.find("#TDS_val").text();
                //var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();

                //$("#" + HdnTDS_CalculateTable + " tbody").append(`<tr id="R${++rowIdx}">
                //<td id="td_TDS_Name">${TDS_Name}</td>
                //<td id="td_TDS_NameID">${TDS_NameID}</td>
                //<td id="td_TDS_Percentage">${TDS_Percentage}</td>
                //<td id="td_TDS_Level">${TDS_Level}</td>
                //<td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
                //<td id="td_TDS_Amount">${TDS_Amount}</td>
                //<td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
                //<td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
                //    </tr>`);

                //NewArr.push({ /*UserID: UserID,*/ DocNo: GRNNo, DocDate: GRNDate, TDS_ItmCode: TDS_ItmCode, TDS_Name: TDS_Name, TDS_NameID: TDS_NameID, TDS_Percentage: TDS_Percentage, TDS_Percentage: TDS_Percentage, TDS_Level: TDS_Level, TDS_ApplyOn: TDS_ApplyOn, TDS_Amount: TDS_Amount, TotalTDS_Amount: TotalTDS_Amount, TDS_ApplyOnID: TDS_ApplyOnID })
            });

        }
        var TotalAMount = parseFloat(0).toFixed(DecDigit);

        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(DecDigit);
        });

        $("#TxtTDS_Amount").val(TotalAMount);
        $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
        //$("#PopUp_TDSCalculate").html("");
        GetAllGLID();
    }
}
/*----------------------------TDS Section End-----------------------------*/
function approveonclick() { /**Added this Condition by Nitesh 15-01-2024 for Disable Approve btn after one Click**/
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
/***---------------------------GL Voucher Entry-----------------------------***/
function GetAllGLID() {
    GetAllGL_WithMultiSupplier();
}
/*GetAllGLIDForImport() function is use for both domestinc and import*/
async function GetAllGL_WithMultiSupplier() {
    debugger;
    if ($("#JOInvItmDetailsTbl > tbody > tr").length == 0) {
        return false;
    }
    if (CheckJI_ItemValidations() == false) {
        return false;
    }
    var GstType = $("#Hd_GstType").val();
    var GstCat = $("#Hd_GstCat").val();
    var NetInvValue = $("#NetOrderValue").val();
    var conv_rate = $("#conv_rate").val();
    var ValDecDigit = $("#ValDigit").text();
    var supp_id = $("#SupplierName").val();
    var supp_acc_id = $("#supp_acc_id").val();
    var SuppVal = 0;
    var SuppValInBase = 0; 
    var GLDetail = [];

    SuppValInBase = (parseFloat(NetInvValue)).toFixed(ValDecDigit);
    SuppVal = (parseFloat(SuppValInBase) / parseFloat(conv_rate)).toFixed(ValDecDigit)

    var Compid = $("#CompID").text();
    var InvType = "D";
    var curr_id = $("#ddlCurrency").val();
    var bill_no = $("#Bill_No").val();
    var bill_dt = $("#Bill_Date").val();
    //var TransType = 'JobInv';
    var TransType = 'Pur';
    var GLDetail = [];
    var TxaExantedItemList = [];
    GLDetail.push({
        comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal, ValueInBase: SuppValInBase
        , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp", parent: 0, Entity_id: supp_acc_id
        , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
    });
    $("#JOInvItmDetailsTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var item_id = currentRow.find("#hfItemID").val();
        var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
        var ItmGrossValInBase = currentRow.find("#TxtItemGrossValue").val();
        var TxtGrnNo = currentRow.find("#src_doc_no").val();
        var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
        var item_acc_id = currentRow.find("#hdn_item_gl_acc").val()
        var TaxAmt = parseFloat(0).toFixed($("#ValDigit").text())
        var TxaExanted = "N";
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TxaExanted = "Y";
            TxaExantedItemList.push({ item_id: item_id, doc_no: TxtGrnNo });
        }
        if (ItemTaxAmt == TaxAmt) {
            if (currentRow.find("#ManualGST").is(":checked")) {
                TxaExanted = "Y";
                TxaExantedItemList.push({ item_id: item_id, doc_no: TxtGrnNo });
            }
        }
        GLDetail.push({
            comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal
            , ValueInBase: ItmGrossValInBase, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm", parent: supp_acc_id
            , Entity_id: 0, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: item_acc_id
        });
    });

    $("#ht_Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var oc_id = currentRow.find("#OC_ID").text();
        var oc_acc_id = currentRow.find("#OCAccId").text();
        var oc_amt = currentRow.find("#OCAmtSp").text();
        var oc_amt_bs = currentRow.find("#OCAmtBs").text();
        var oc_supp_id = currentRow.find("#td_OCSuppID").text();
        var oc_supp_acc_id = currentRow.find("#OCSuppAccId").text();
        var oc_supp_type = currentRow.find("#td_OCSuppType").text();
        var oc_amt_bs_wt = currentRow.find("#OCTotalTaxAmt").text();
        var oc_conv_rate = currentRow.find("#OC_Conv").text();
        var oc_curr_id = currentRow.find("#HdnOC_CurrId").text();
        var oc_supp_bill_no = currentRow.find("#OCBillNo").text();
        var oc_supp_bill_dt = currentRow.find("#OCBillDt").text();
        var oc_amt_wt = (parseFloat(CheckNullNumber(oc_amt_bs_wt)) / parseFloat(oc_conv_rate)).toFixed(ValDecDigit); //with tax

        var oc_parent = (oc_supp_id == "0" || oc_supp_id == "") ? supp_acc_id : oc_supp_acc_id;

        GLDetail.push({
            comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt
            , ValueInBase: oc_amt_bs, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC", parent: oc_parent
            , Entity_id: oc_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate
            , bill_no: oc_supp_bill_no == "" ? bill_no : oc_supp_bill_no
            , bill_date: oc_supp_bill_dt == "" ? bill_dt : oc_supp_bill_dt, acc_id: ""
        });

        if (oc_supp_id != "" && oc_supp_id != "0") {
            let gl_type = "Supp";
            if (oc_supp_type == "2")
                gl_type = "Supp";
            if (oc_supp_type == "7")
                gl_type = "Bank";

            GLDetail.push({
                comp_id: Compid, id: oc_supp_id, type: gl_type, doctype: InvType, Value: oc_amt_wt
                , ValueInBase: oc_amt_bs_wt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: gl_type, parent: 0
                , Entity_id: oc_supp_acc_id, curr_id: oc_curr_id, conv_rate: oc_conv_rate, bill_no: oc_supp_bill_no
                , bill_date: oc_supp_bill_dt, acc_id: ""
            });
        } else {
            //if (GLDetail.findIndex((obj => obj.id == supp_id)) > -1) {
            //    objIndex = GLDetail.findIndex((obj => obj.id == supp_id));
            //    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(oc_amt_wt);
            //    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(oc_amt_bs_wt);
            //}
        }
    });
    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var oc_id = currentRow.find("#TaxItmCode").text();
        var TaxPerc = currentRow.find("#TaxPercentage").text();
        var TaxPerc_id = TaxPerc.replace("%", "");
        var ArrOcGl = GLDetail.filter(v => (v.id == oc_id && v.type == "OC"));
        GLDetail.push({
            comp_id: Compid, id: tax_id, type: "OcTax", doctype: InvType, Value: tax_amt
            , ValueInBase: tax_amt, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: ArrOcGl[0].id
            , Entity_id: tax_acc_id, curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate,
            bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date, acc_id: ""
        });
    });
    $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tax_id = currentRow.find("#TaxNameID").text();
        var tax_acc_id = currentRow.find("#TaxAccId").text();
        var tax_amt = currentRow.find("#TaxAmount").text();
        var TaxRecov = currentRow.find("#TaxRecov").text();
        var TaxItmCode = currentRow.find("#TaxItmCode").text();
        var DocNo = currentRow.find("#DocNo").text();

        if (parseFloat(CheckNullNumber(tax_amt)) > 0) {
            var GstApplicable = $("#Hdn_GstApplicable").text();
            var ItemRow = $("#JOInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr').find("#src_doc_no[value='" + DocNo + "']").closest("tr");
            var ClaimItc = true;
            if (GstApplicable == "Y") {
                ClaimItc = ItemRow.find("#ClaimITC").is(":checked");
            }
            if (TaxRecov == "N" || !ClaimItc) {
                if (GLDetail.findIndex((obj => obj.id == TaxItmCode)) > -1) {
                    var objIndex = GLDetail.findIndex((obj => obj.id == TaxItmCode));
                    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(tax_amt);
                    GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(tax_amt);
                }
            } else {
                GLDetail.push({
                    comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt
                    , ValueInBase: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax", parent: supp_acc_id
                    , Entity_id: tax_acc_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
                });
            }
        }
    });
    if (GstCat == "UR") {
        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
            var currentRow = $(this);
            var TaxID = currentRow.find("#TaxNameID").text();
            var TaxAccID = currentRow.find("#TaxAccId").text();
            var TaxItmCode = currentRow.find("#TaxItmCode").text();
            var DocNo = currentRow.find("#DocNo").text();
            TaxID = "R" + TaxID;
            var TaxPerc = currentRow.find("#TaxPercentage").text();
            var TaxPerc_id = TaxPerc.replace("%", "");
            var TaxVal = currentRow.find("#TaxAmount").text();
            var TaxRecov = currentRow.find("#TaxRecov").text();

            var ItemRow = $("#JOInvItmDetailsTbl >tbody >tr #hfItemID[value='" + TaxItmCode + "']").closest('tr').find("#src_doc_no[value='" + DocNo + "']").closest("tr");
            var ClaimITC = ItemRow.find("#ClaimITC").is(":checked");
            if (ClaimITC) {
                if (TaxRecov == "N") {

                }
                else {
                    if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
                        if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
                            objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
                            GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
                            GLDetail[objIndex].ValueInBase = parseFloat(GLDetail[objIndex].ValueInBase) + parseFloat(TaxVal);
                        } else {
                            GLDetail.push({
                                comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal
                                , ValueInBase: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType, parent: supp_acc_id
                                , Entity_id: TaxAccID, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt,acc_id: ""

                            });
                        }

                    }
                }
            }
        });
    }
    var Cal_Tds_Amt = 0;
    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        Cal_Tds_Amt = parseFloat(Cal_Tds_Amt) + parseFloat(tds_amt);
        GLDetail.push({
            comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
            , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: supp_acc_id
            , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    });
    if (Cal_Tds_Amt > 0) {
        GLDetail.push({
            comp_id: Compid, id: supp_id, type: "TSupp", doctype: InvType
            , Value: Cal_Tds_Amt, ValueInBase: Cal_Tds_Amt
            , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: supp_acc_id
            , curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
        });
    }

    var Oc_Tds = [];
    $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
        // debugger;
        var currentRow = $(this);
        var tds_id = currentRow.find("#td_TDS_NameID").text();
        var tds_acc_id = currentRow.find("#td_TDS_AccId").text();
        var tds_amt = currentRow.find("#td_TDS_Amount").text();
        var tds_supp_id = currentRow.find("#td_TDS_Supp_Id").text();

        var ArrOcGl = GLDetail.filter(v => (v.id == tds_supp_id && v.type == "Supp"));
        var tdsIndex = Oc_Tds.findIndex(v => v.supp_id == tds_supp_id);
        if (tdsIndex > -1) {
            Oc_Tds[tdsIndex].tds_amt = parseFloat(Oc_Tds[tdsIndex].tds_amt) + parseFloat(tds_amt);
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            });
        } else {
            Oc_Tds.push({
                supp_id: tds_supp_id, supp_acc_id: ArrOcGl[0].id, tds_amt: tds_amt
                , bill_no: ArrOcGl[0].bill_no, bill_date: ArrOcGl[0].bill_date
                , curr_id: ArrOcGl[0].curr_id, conv_rate: ArrOcGl[0].conv_rate
            });
            GLDetail.push({
                comp_id: Compid, id: tds_acc_id, type: "Tds", doctype: InvType, Value: tds_amt
                , ValueInBase: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds", parent: ArrOcGl[0].Entity_id
                , Entity_id: tds_id, curr_id: curr_id, conv_rate: conv_rate, bill_no: bill_no, bill_date: bill_dt, acc_id: ""
            });
        }
    });
    if (Oc_Tds.length > 0) {
        Oc_Tds.map((item, idx) => {
            GLDetail.push({
                comp_id: Compid, id: item.supp_id, type: "TSupp", doctype: InvType
                , Value: item.tds_amt, ValueInBase: item.tds_amt
                , DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "TSupp", parent: 0, Entity_id: item.supp_acc_id
                , curr_id: item.curr_id, conv_rate: item.conv_rate, bill_no: item.bill_no, bill_date: item.bill_date, acc_id: ""
            });
        });
    }
    debugger;
    await Cmn_GLTableBind(supp_acc_id, GLDetail,"Purchase")  
}
function Get_Gl_Narration(VouType, bill_no, bill_date) {
    return (VouType == "DN" ? $("#hdn_DN_Nurration").val() : VouType == "BP" ? $("#hdn_BP_Nurration").val() : $("#hdn_Nurration").val()).replace("_bill_no", bill_no).replace("_bill_dt", (bill_date != "" && bill_date != null) ? moment(bill_date).format("DD-MM-YYYY") : "");
}

function GlTableRenderHtml(SrNo, GlSrNo, rowSpan, type, acc_name, acc_id, DrValue, DrValueInBase, CrValue, CrValueInBase, VouType
    , curr_id, conv_rate, bill_bo, bill_date) {

    var Acc_Id = acc_id;
    acc_id_start_no = Acc_Id.toString().substring(0, 1);
    var Disable;
    if (acc_id_start_no == "3" || acc_id_start_no == "4") {
        Disable = "";
    }
    else {
        Disable = "disabled";
    }
    if (parseFloat(DrValue) < 0) {
        CrValue = Math.abs(DrValue);
        DrValue = 0;
    }
    if (parseFloat(DrValueInBase) < 0) {
        CrValueInBase = Math.abs(DrValueInBase);
        DrValueInBase = 0;
    }
    if (parseFloat(CrValue) < 0) {
        DrValue = Math.abs(CrValue);
        CrValue = 0;
    }
    if (parseFloat(CrValueInBase) < 0) {
        DrValueInBase = Math.abs(CrValueInBase);
        CrValueInBase = 0;
    }
    let hfSrNo = $('#VoucherDetail tbody tr').length + 1;
    var FieldType = "";
    if (type == 'Itm') {
        FieldType = `<div class="col-sm-11 lpo_form no-padding">
                            <select class="form-control" id="Acc_name_${hfSrNo}" onchange ="OnChangeAccountName(${hfSrNo},event)">
                              </select>
                        </div>`;
        $("#hdnAccID").val(acc_id);
    }
    else {
        FieldType = `${acc_name}`;
    }
    let Table_tds = `<td id="td_GlSrNo">${GlSrNo}</td>
                                    <td id="td_vou_sr_no" hidden>${SrNo}</td>
                                    <td id="td_GlAccName" class="no-padding">
                                        `+ FieldType + `
                                    </td>
                                    <td id="tdhdn_GlAccId" hidden>${acc_id}</td>
                                    <td class="num_right" id="dramt">${parseFloat(DrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="dramtInBase">${parseFloat(DrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramt">${parseFloat(CrValue).toFixed(ValDecDigit)}</td>
                                    <td class="num_right" id="cramtInBase">${parseFloat(CrValueInBase).toFixed(ValDecDigit)}</td>
                                    <td class="center" id="td_CC">
                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
                                    </td>
                                <td hidden>
                                    <input type="hidden" id="SNohf" value="${hfSrNo}" />
                                    <input type="hidden" id="type" value="${type}"/>
                                    <input  type="hidden" id="txthfAccID" value="${acc_name}"/>
                                    <input  type="hidden" id="hfAccID"  value="${acc_id}" />
                                    <input  type="hidden" id="glVouType"  value="${VouType}" />
                                    <input  type="hidden" id="gl_curr_id"  value="${curr_id}" />
                                    <input  type="hidden" id="gl_conv_rate"  value="${conv_rate}" />
                                    <input  type="hidden" id="gl_bill_no"  value="${bill_bo}" />
                                    <input  type="hidden" id="gl_bill_date"  value="${bill_date}" />
                                    <input  type="hidden" id="dramt1" value="${parseFloat(DrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="dramtInBase1" value="${parseFloat(DrValueInBase).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramt1" value="${parseFloat(CrValue).toFixed(ValDecDigit)}"/>
                                    <input  type="hidden" id="cramtInBase1" value="${parseFloat(CrValueInBase).toFixed(ValDecDigit)}"/>
                                </td>`
    if (type == "Supp" || type == "Bank") {
        $('#VoucherDetail tbody').append(`<tr id="tr_GlRow${SrNo}">
                                <td rowspan="${rowSpan}" id="td_SrNo">${SrNo}</td>
                                `+ Table_tds + `
                                <td rowspan="${rowSpan}" id="td_VouType">${VouType}</td>
                                <td rowspan="${rowSpan}" id="td_VouNo"></td>
                                <td rowspan="${rowSpan}" id="td_VouDate"></td>
                                
                            </tr>`)
    } else {
        $('#VoucherDetail tbody').append(`<tr>
                                     `+ Table_tds + `
                                </tr>`)
    }
}
async function AddRoundOffGL() {
    debugger;
    var curr_id = $("#ddlCurrency").val();
    var ValDecDigit = $("#ValDigit").text();///Amount
    await $.ajax({
        type: "POST",
        url: "/ApplicationLayer/SalesReturn/GetRoundOffGLDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        data: JSON.stringify({}),
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                SI_ErrorPage();
                return false;
            }
            if (data !== null && data !== "") {
                var arr = [];
                arr = JSON.parse(data);
                debugger;
                if (arr.Table.length > 0) {
                    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
                    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
                    $("#VoucherDetail >tbody >tr").each(function () {
                        //debugger;
                        var currentRow = $(this);
                        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
                            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
                        }
                        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
                            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
                        }
                        else {
                            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
                        }
                    });
                    debugger;
                    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
                        if (parseFloat(DrTotAmtInBase) < parseFloat(CrTotAmtInBase)) {
                            var Diff = parseFloat(CrTotAmt) - parseFloat(DrTotAmt);
                            var DiffInBase = parseFloat(CrTotAmtInBase) - parseFloat(DrTotAmtInBase);
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    //        $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
                                    //        <td class="sr_padding">${rowIdx}</td>
                                    //        <td id="txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    //        <td id="dramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>
                                    //        <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
                                    //        <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                    //         <input type="hidden" id="type" value="RO"/></td>
                                    //</tr>`);
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;
                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });

                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , Diff, DiffInBase, 0, 0, "PV", curr_id
                                        , $("#conv_rate").val()
                                        , $("#Bill_No").val(), $("#Bill_Date").val())

                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                    var table = document.getElementById("VoucherDetail");
                                    rows = table.tBodies[0].rows;
                                    let ib = spanRowCount - 1;
                                    let LastRow = $('#VoucherDetail tbody tr').length;
                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                }
                            }
                        }
                        else {
                            var Diff = parseFloat(DrTotAmt) - parseFloat(CrTotAmt);
                            var DiffInBase = parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase);
                            var rowIdx = $('#VoucherDetail tbody tr').length;
                            for (var j = 0; j < arr.Table.length; j++) {
                                if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
                                    //        $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
                                    //        <td class="sr_padding">${rowIdx}</td>
                                    //        <td id="txthfAccID">${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>
                                    //        <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
                                    //        <td id="cramt" class="num_right">${parseFloat(Diff).toFixed(ValDecDigit)}</td>  
                                    //        <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
                                    //         <input type="hidden" id="type" value="RO"/></td>
                                    //</tr>`);
                                    var SrRows = $('#VoucherDetail > tbody > tr > #td_SrNo');
                                    GlSrNo = SrRows[SrRows.length - 1].textContent;
                                    let spanRowCount = 1;
                                    $('#VoucherDetail > tbody > tr > #td_vou_sr_no').each(function () {
                                        var row = $(this);
                                        if (row.text() == 1) {
                                            spanRowCount++;
                                        }
                                    });
                                    GlTableRenderHtml(1, spanRowCount, spanRowCount, "RO", arr.Table[j].acc_name, arr.Table[j].acc_id
                                        , 0, 0, Diff, DiffInBase, "PV"
                                        , curr_id, $("#conv_rate").val(), $("#Bill_No").val()
                                        , $("#Bill_Date").val())
                                    var vouRow = $('#VoucherDetail tbody #tr_GlRow1');
                                    vouRow.find("#td_SrNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouType").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouNo").attr("rowspan", spanRowCount);
                                    vouRow.find("#td_VouDate").attr("rowspan", spanRowCount);

                                    var table = document.getElementById("VoucherDetail");
                                    rows = table.tBodies[0].rows;
                                    let ib = spanRowCount - 1;
                                    let LastRow = $('#VoucherDetail tbody tr').length;
                                    rows[ib].parentNode.insertBefore(rows[LastRow - 1], rows[ib]);
                                }
                            }
                        }

                    }
                    debugger;
                    CalculateVoucherTotalAmount();
                }
            }
        }
    });
}
async function CalculateVoucherTotalAmount() {

    var ValDecDigit = $("#ValDigit").text();
    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var DrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
    var CrTotAmtInBase = parseFloat(0).toFixed(ValDecDigit);
    $("#VoucherDetail >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#dramt").text() == "" || currentRow.find("#dramt").text() == "NaN") {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmt = (parseFloat(DrTotAmt) + parseFloat(currentRow.find("#dramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#dramtInBase").text() == "" || currentRow.find("#dramtInBase").text() == "NaN") {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            DrTotAmtInBase = (parseFloat(DrTotAmtInBase) + parseFloat(currentRow.find("#dramtInBase").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramt").text() == "" || currentRow.find("#cramt").text() == "NaN") {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmt = (parseFloat(CrTotAmt) + parseFloat(currentRow.find("#cramt").text())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#cramtInBase").text() == "" || currentRow.find("#cramtInBase").text() == "NaN") {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            CrTotAmtInBase = (parseFloat(CrTotAmtInBase) + parseFloat(currentRow.find("#cramtInBase").text())).toFixed(ValDecDigit);
        }
    });

    $("#DrTotal").text(DrTotAmt);
    $("#DrTotalInBase").text(DrTotAmtInBase);
    $("#CrTotal").text(CrTotAmt);
    $("#CrTotalInBase").text(CrTotAmtInBase);

    if (parseFloat(DrTotAmtInBase) != parseFloat(CrTotAmtInBase)) {
        debugger;
        if (Math.abs(parseFloat(DrTotAmtInBase) - parseFloat(CrTotAmtInBase)) < 1) {
            await AddRoundOffGL();
        }
    }

}
/***---------------------------GL Voucher Entry End-----------------------------***/
/***--------------------------------Claim ITC--------------------------------***/
function OnClickClaimITCCheckBox(e) {
    GetAllGLID();
}
/***------------------------------Claim ITC ENd------------------------------***/
/***------------------------------TDS On Third Party------------------------------***/
var CC_Clicked_Row;
function OnClickTP_TDSCalculationBtn(e) {
    const row = $(e.target).closest("tr");
    CC_Clicked_Row = row;
    const GrVal = row.find("#OCAmount").text();
    const TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    const OcVal_IT = CC_Clicked_Row.find("#OCTotalTaxAmt").text();
    const ToTdsAmt = parseFloat(CheckNullNumber(GrVal));
    $("#hdn_tds_on").val("OC");
    CMN_OnClickTDSCalculationBtn(ToTdsAmt, "Hdn_OC_TDS_CalculatorTbl", TDS_OcId, OcVal_IT);

}
function OnClickTP_TDS_SaveAndExit() {
    debugger

    var TotalTDS_Amount = $('#TotalTDS_Amount').text();
    var ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    //var TDS_OcId = $("#TDS_oc_id").text();
    var TDS_OcId = CC_Clicked_Row.find("#OCValue").text();
    //var TDS_SuppId = $("#TDS_supp_id").text();
    var TDS_SuppId = CC_Clicked_Row.find("#td_OCSuppID").text();
    var rowIdx = 0;
    var GstCat = $("#Hd_GstCat").val();
    
    //Added by Suraj Maurya on 06-12-2024
    var ToTdsAmt = 0;
    let TdsAssVal_applyOn = "ET";
    ToTdsAmt = parseFloat(CheckNullNumber(CC_Clicked_Row.find("#OCAmount").text()));
    if ($("#TdsAssVal_IncluedTax").is(":checked")) {
        TdsAssVal_applyOn = "IT";
        ToTdsAmt = parseFloat(CheckNullNumber(CC_Clicked_Row.find("#OCTotalTaxAmt").text()));
    }
        //Added by Suraj Maurya on 06-12-2024 End
    
    if ($("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").length > 0) {

        $("#Hdn_OC_TDS_CalculatorTbl >tbody >tr").each(function () {
            let row = $(this);
            if (row.find("#td_TDS_OC_Id").text() == TDS_OcId) {
                $(this).remove();
            }
        });
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();

            $('#Hdn_OC_TDS_CalculatorTbl tbody').append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });
    }
    else {
        $("#TDS_CalculatorTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var TDS_Name = currentRow.find("#TDS_name").text();
            var TDS_NameID = currentRow.find("#TDS_id").text();
            var TDS_Percentage = currentRow.find("#TDS_rate").text();
            var TDS_Level = currentRow.find("#TDS_level").text();
            var TDS_ApplyOn = currentRow.find("#TDS_applyonname").text();
            var TDS_Amount = currentRow.find("#TDS_val").text();
            var TDS_ApplyOnID = currentRow.find("#TDS_applyon").text();
            var TDS_AccId = currentRow.find("#TDS_acc_id").text();

            $("#Hdn_OC_TDS_CalculatorTbl tbody").append(`<tr id="R${++rowIdx}">
            <td id="td_TDS_Name">${TDS_Name}</td>
            <td id="td_TDS_NameID">${TDS_NameID}</td>
            <td id="td_TDS_Percentage">${TDS_Percentage}</td>
            <td id="td_TDS_Level">${TDS_Level}</td>
            <td id="td_TDS_ApplyOn">${TDS_ApplyOn}</td>
            <td id="td_TDS_Amount">${TDS_Amount}</td>
            <td id="td_TDS_ApplyOnID">${TDS_ApplyOnID}</td>
            <td id="td_TDS_BaseAmt">${ToTdsAmt}</td>
            <td id="td_TDS_AccId">${TDS_AccId}</td>
            <td id="td_TDS_OC_Id">${TDS_OcId}</td>
            <td id="td_TDS_Supp_Id">${TDS_SuppId}</td>
            <td id="td_TDS_AssValApplyOn">${TdsAssVal_applyOn}</td>
                </tr>`);
        });

    }
    SetTds_Amt_To_OC();
    $("#TDS_SaveAndExitBtn").attr("data-dismiss", "modal");
    //GetAllGLID();
}
function SetTds_Amt_To_OC() {
    var TotalAMount = parseFloat(0).toFixed(DecDigit);
    var DecDigit = $("#ValDigit").text();
    $("#TDS_CalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#TDS_val").text())).toFixed(DecDigit);
    });

    CC_Clicked_Row.find("#OC_TDSAmt").text(TotalAMount);
    CC_Clicked_Row = null;
}

/***------------------------------TDS On Third Party End------------------------------***/
//function GetAllGLID() {
//    debugger;
//    var GstType = $("#Hd_GstType").val();
//    var GstCat = $("#Hd_GstCat").val();
//    var DocStatus = $('#hfStatus').val().trim();
//    var NetInvValue = $("#NetOrderValue").val();
//    var NetTaxValue = $("#TxtTaxAmount").val();
//    var ValueWithoutTax = (parseFloat(NetInvValue) - parseFloat(NetTaxValue));
//    var ValDecDigit = $("#ValDigit").text();///Amount
//    var supp_id = $("#SupplierName").val();
//    var SuppVal = 0;
//    var Compid = $("#CompID").text();
//    var InvType = "D";
//    var TransType = 'JobInv';
//    var GLDetail = [];

//    GLDetail.push({ comp_id: Compid, id: supp_id, type: "Supp", doctype: InvType, Value: SuppVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Supp" });
//    var TxaExantedItemList = [];
//    $("#JOInvItmDetailsTbl >tbody >tr").each(function (i, row) {
//        //debugger;
//        var currentRow = $(this);
//        var item_id = currentRow.find("#hfItemID").val();
//        var ItmGrossVal = currentRow.find("#TxtItemGrossValue").val();
//        var ItemTaxAmt = currentRow.find("#Txtitem_tax_amt").val()
//        var TaxAmt = parseFloat(0).toFixed($("#ValDigit").text())
//        var TxaExanted = "N";
//        if (currentRow.find("#TaxExempted").is(":checked")) {
//            TxaExanted = "Y";
//            TxaExantedItemList.push({ item_id: item_id });
//        }
//        var GstApplicable = $("#Hdn_GstApplicable").text();
//        if (GstApplicable == "Y") {
//            if (ItemTaxAmt == TaxAmt) {
//                if (currentRow.find("#ManualGST").is(":checked")) {
//                    TxaExanted = "Y";
//                    TxaExantedItemList.push({ item_id: item_id });
//                }
//            }
//        }
//        GLDetail.push({ comp_id: Compid, id: item_id, type: "Itm", doctype: InvType, Value: ItmGrossVal, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Itm" });
//    });

//    $("#Hdn_OC_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
//        //debugger;
//        var currentRow = $(this);
//        var tax_id = currentRow.find("#TaxNameID").text();
//        var tax_amt = currentRow.find("#TaxAmount").text();
//        GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
//    });
//    $("#Tbl_ItemTaxAmountList >tbody >tr").each(function (i, row) {
//        // debugger;
//        var currentRow = $(this);
//        var tax_id = currentRow.find("#taxID").text();
//        var tax_amt = currentRow.find("#TotalTaxAmount").text();
//        var TaxPerc = currentRow.find("#TaxPerc").text();
//        GLDetail.push({ comp_id: Compid, id: tax_id, type: "Tax", doctype: InvType, Value: tax_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tax" });
//    });
//    $("#Hdn_TDS_CalculatorTbl >tbody >tr").each(function (i, row) {
//        // debugger;
//        var currentRow = $(this);
//        var tds_id = currentRow.find("#td_TDS_NameID").text();
//        var tds_amt = currentRow.find("#td_TDS_Amount").text();
//        GLDetail.push({ comp_id: Compid, id: tds_id, type: "Tds", doctype: InvType, Value: tds_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "Tds" });
//    });
//    $("#Tbl_OtherChargeList >tbody >tr").each(function (i, row) {
//        //debugger;
//        var currentRow = $(this);
//        var oc_id = currentRow.find("#OCID").text();
//        var oc_amt = currentRow.find("#OCAmtSp1").text();
//        GLDetail.push({ comp_id: Compid, id: oc_id, type: "OC", doctype: InvType, Value: oc_amt, DrAmt: 0, CrAmt: 0, TransType: TransType, gl_type: "OC" });
//    });
//    debugger;
//    if (GstCat == "UR") {
//        $("#Hdn_TaxCalculatorTbl >tbody >tr").each(function (i, row) {
//            debugger;
//            var currentRow = $(this);
//            TransType = 'Pur';
//            var TaxID = currentRow.find("#TaxNameID").text();
//            var TaxItmCode = currentRow.find("#TaxItmCode").text();
//            var DocNo = currentRow.find("#DocNo").text();
//            TaxID = "R" + TaxID;
//            var TaxPerc = currentRow.find("#TaxPercentage").text();
//            var TaxPerc_id = TaxPerc.replace("%", "");
//            var TaxVal = currentRow.find("#TaxAmount").text();

//            if (TxaExantedItemList.findIndex((obj => (obj.item_id) == (TaxItmCode))) == -1) {
//                if (GLDetail.findIndex((obj => obj.id == TaxID)) > -1) {
//                    objIndex = GLDetail.findIndex((obj => obj.id == TaxID));
//                    GLDetail[objIndex].Value = parseFloat(GLDetail[objIndex].Value) + parseFloat(TaxVal);
//                    //GLDetail.push({ comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType });
//                } else {
//                    GLDetail.push({ comp_id: Compid, id: TaxID, type: "RCM", doctype: InvType, Value: TaxVal, DrAmt: TaxPerc_id, CrAmt: 0, TransType: TransType, gl_type: GstType });
//                }
//            }
//        });
//    }

//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/JobInvoiceSC/GetGLDetails",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        data: JSON.stringify({ GLDetail: GLDetail }),
//        success: function (data) {
//            debugger;
//            if (data == 'ErrorPage') {
//                SI_ErrorPage();
//                return false;
//            }
//            if (data !== null && data !== "") {
//                var arr = [];
//                arr = JSON.parse(data);
//                var Voudet = 'Y';
//                $('#VoucherDetail tbody tr').remove();
//                if (arr.Table1.length > 0) {
//                    var errors = [];
//                    var step = [];
//                    for (var i = 0; i < arr.Table1.length; i++) {
//                        debugger;
//                        if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
//                            errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
//                            step.push(parseInt(i));
//                            Voudet = 'N';
//                        }
//                    }
//                    var arrayOfErrorsToDisplay = [];
//                    $.each(errors, function (i, error) {
//                        arrayOfErrorsToDisplay.push({ text: error });
//                    });
//                    Swal.mixin({
//                        confirmButtonText: 'Ok',
//                        type: "warning",
//                    }).queue(arrayOfErrorsToDisplay)
//                        .then((result) => {
//                            if (result.value) {

//                            }
//                        });
//                }
//                debugger;
//                if (Voudet == 'Y') {
//                    if (arr.Table.length > 0) {
//                        $('#VoucherDetail tbody tr').remove();
//                        var rowIdx = $('#VoucherDetail tbody tr').length;
//                        for (var j = 0; j < arr.Table.length; j++) {
//                            ++rowIdx;
//                            debugger;

//                            var Acc_Id = arr.Table[j].acc_id;
//                            acc_Id = Acc_Id.substring(0, 1);
//                            var Disable;
//                            if (acc_Id == "3" || acc_Id == "4") {
//                                Disable = "";
//                            }
//                            else {
//                                Disable = "disabled";
//                            }

//                            var FieldType = "";
//                            if (arr.Table[j].type == 'Itm') {
//                                FieldType = `<div class="col-sm-11 lpo_form no-padding"><select class="form-control" id="Acc_name_${rowIdx}" onchange ="OnChangeAccountName(${rowIdx},event)">
//                                        </select>
//                                    <input  type="hidden" id="hfAccID"  value="${arr.Table[j].acc_id}" /></div> `;
//                            }
//                            else {
//                                FieldType = `${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" />`;
//                            }

//                            if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                                //debugger;
//                                if (arr.Table[j].type == "RCM" || arr.Table[j].type == "Tds") {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>`+ FieldType + `</td>
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
//                                   <td class="center">
//                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                     </td>
//                                    <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                    <input  type="hidden" id="dramt1" value="${parseFloat(0).toFixed(ValDecDigit)}"/>
//                                    <input  type="hidden" id="cramt1" value="${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}"/>

//                            </tr>`);
//                                }
//                                else {
//                                    $('#VoucherDetail tbody').append(`<tr id="R${rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>`+ FieldType + `</td>
//                                    <td id="dramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td style="display: none;"><input type="hidden" id="SNohf" value="${rowIdx}" />
//                                     <input type="hidden" id="type" value="${arr.Table[j].type}"/></td>
//                                   <td class="center">
//                                        <button type="button" ${Disable} id="BtnCostCenterDetail" onclick="Onclick_CCbtn('type',event)" class="calculator subItmImg" data-toggle="modal" data-target="#CostCenterDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" title="${$("#span_CostCenterDetail").text()}"></i></button>
//                                     </td>
//                                    <input  type="hidden" id="txthfAccID" value="${arr.Table[j].acc_name}"/>
//                                    <input  type="hidden" id="dramt1" value="${parseFloat(0).toFixed(ValDecDigit)}"/>
//                                    <input  type="hidden" id="cramt1" value="${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}"/>

//                            </tr>`);
//                                }
//                            }
//                        }
//                        var tds_amt = $("#TxtTDS_Amount").val();
//                        if (GstCat == "UR") {
//                            var CrAmt = parseFloat(parseFloat(ValueWithoutTax) - parseFloat(CheckNullNumber(tds_amt))).toFixed(ValDecDigit);
//                            $("#VoucherDetail >tbody >tr:first").find("#cramt").text(CrAmt);

//                        }
//                        else {
//                            var CrAmt = parseFloat(parseFloat($("#NetOrderValue").val()) - parseFloat(CheckNullNumber(tds_amt))).toFixed(ValDecDigit);
//                            $("#VoucherDetail >tbody >tr:first").find("#cramt").text(CrAmt);
//                        }
//                    }
//                }
//                BindDDLAccountList();
//                CalculateVoucherTotalAmount();
//            }
//        }
//    });
//}

function RefreshBillNoBillDateInGLDetail() {
    debugger
    /*This Function is Created by Suraj on 22-10-2024 to reset bill number and bill date to GL Details*/
    let supp_acc_id = $("#supp_acc_id").val();
    let bill_no = $("#Bill_No").val();
    let bill_dt = $("#Bill_Date").val();
    let change_vou_sr_no = "";
    //$('#VoucherDetail > tbody > tr #hfAccID[value="' + supp_acc_id + '"]').closest("tr").each(function () {
    $('#VoucherDetail > tbody > tr ').closest("tr").each(function () {
        let row = $(this);
        let hfAccID = row.find("#hfAccID").val();
        let vouType = row.find("#glVouType").val();
        let vou_sr_no = row.find("#td_vou_sr_no").text();
        if (hfAccID == supp_acc_id) {
            change_vou_sr_no = vou_sr_no;
            row.find("#gl_bill_no").val(bill_no);
            row.find("#gl_bill_date").val(bill_dt);
            let narr = Get_Gl_Narration(vouType, bill_no, bill_dt);
            row.find("#gl_narr span").text(narr);
        } else {
            if (vou_sr_no == change_vou_sr_no) {
                let narr = Get_Gl_Narration(vouType, bill_no, bill_dt);
                row.find("#gl_narr span").text(narr);
            }
        }
    });
}
