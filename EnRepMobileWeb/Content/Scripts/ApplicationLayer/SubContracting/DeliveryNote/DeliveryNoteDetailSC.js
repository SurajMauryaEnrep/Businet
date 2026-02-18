/************************************************
Javascript Name:Delivery Note SC Detail
Created By:Hina Sharma
Created Date: 21-02-2023
Description: This Javascript use for the Delivery Note SC many function

Modified By:
Modified Date:
Description:

*************************************************/

$(document).ready(function () {


    debugger;
    BindDDlContractorList();
    var Supp_id = $("#SupplierName").val();
    var DNNum = $("#DeliveryNoteNumber").val();
    var Message = $("#hdn_massage").val();
    if (Message == "DocModify") {

    }
    else {
        if (DNNum == null || DNNum == "") {
            debugger;
            BindJobOrderNumber(Supp_id);
        }
    }
    var JOdocno = $("#JobOrdNo").val();
    var docno = JOdocno.split(',');
    var JONum = docno[0];
    if (JONum != null || JONum != "") {
        debugger;
        //BindReturnItemDDLOnBehalfOFJobOrdNO("");
        var lencount = $("#ReturnItemDetailsTbl >tbody >tr").length;
        if (lencount > 0) {
            $("#ReturnItemDetailsTbl >tbody >tr").each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                //var JobOrdTyp = $("#DNSChdn_JobOrdTyp").val();
                //if (JobOrdTyp == "D") {
                //    BindByProdctScrapItm_AgainstDircetJO(Sno);
                //}
                //else {
                    BindDLLReturnItemList(Sno);
                //}
                /*BindDLLReturnItemList(Sno);*/
            });
        }
        BindMDNoOnBehalfOFJobOrdNO();
        
    }
    debugger;
   
    
    
    //$("#reqID").css("display", "none");
    $('#dnSCItemDetails').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        
        $(this).closest('tr').remove();
        ItemCode = $(this).closest('tr').find("#hdItemId").val();
        
        var status = $("#hfStatus").val();
        var command = $("#hdn_command").val();
        if (status == "D" && command == "Edit") {
            $("#SaveItemDispatchQtyDetails TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#PD_DispQtyItemID").val();
                if (rowitem == ItemCode) {
                    var Docno = row.find("#PD_DispDocNo").val();
                    var Docdate = row.find("#PD_DispDocDate").val().split("-").reverse().join("-");
                    debugger;
                    $("#MtrlDispNo option[value='" + Docno + "']").select2().hide();
                    var docnoValue = $('#MtrlDispNo option:selected').text();
                    debugger;
                    DeleteSubItemDispQtyDetail(ItemCode, Docno);
                    debugger;
                    if (docnoValue != Docno) {
                        $('#MtrlDispNo').append(`<option data-date="${Docdate}" value="${Docno}">${Docno}</option>`);
                       
                    }
                    
                    
                    
                }
            });
        }
        else {
            $("#SaveItemDispatchQtyDetails TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#PD_DispQtyItemID").val();
                if (rowitem == ItemCode) {
                    var Docno = row.find("#PD_DispDocNo").val();
                    
                    $("#MtrlDispNo option[value='" + Docno + "']").removeClass("select2-hidden-accessible");
                   
                    debugger;
                    $(this).remove();
                }
            });
        }
        
        $("#SaveItemDispatchQtyDetails TBODY TR").remove();
        var Dn_no = $("#DeliveryNoteNumber").val();
        if (Dn_no == null || Dn_no == "") {
            if ($('#DnSCItmDetailsTbl tr').length <= 1) {
                debugger;
                EnableHeaderDetail();
                $("#hdSelectedSourceDocument").val(null);
                //$("#BtnAttribute").css('display', 'block');
                $(".plus_icon1").css('display', 'block');
            }
        }
        debugger;
        
        //$("#hdSelectedSourceDocument").val(null);
        updateItemSerialNumber();
        var DocStatus = $("#hfStatus").val();
        if (DocStatus == "D") {
            if (($("#DnSCItmDetailsTbl >tbody >tr").length == "0") && ($("#ReturnItemDetailsTbl >tbody >tr").length == "0")) {
                //$("#JobOrdNo").prop("disabled", false);
                //$("#SupplierName").prop("disabled", false);
                EnableHeaderDetail();
                $("#SpanMDNoErrorMsg").css("display", "none");
                $("#MtrlDispNo").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "#ced4da");


            }
        }
        
    });
    $('#ReturnItemDetailsTbl').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });

        $(this).closest('tr').remove();
        debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#ItemListName" + SNo).val();
        
        updateReturnItemSerialNumber();
        var DocStatus = $("#hfStatus").val();
        if (DocStatus == "D") {
            if (($("#DnSCItmDetailsTbl >tbody >tr").length == "0") && ($("#ReturnItemDetailsTbl >tbody >tr").length == "0")) {
                EnableHeaderDetail();
                $("#SpanMDNoErrorMsg").css("display", "none");
                $("#MtrlDispNo").css("border-color", "#ced4da");
                $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "#ced4da");

            }
        }
        debugger;
        DeletePrdctScrapSubItemDetail(ItemCode);
    });
    var SuppName = $("#SupplierName").val();
    $("#Hdn_SupplierName").val(SuppName);
    
    dn_no = $("#DeliveryNoteNumber").val();
    $("#hdDoc_No").val(dn_no);
});

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
        $("#itmretrnID").css("display", "none");
        $('#ReturnItemDetailsTbl >tbody >tr').remove();
        
    }
    else {
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_DNSupplierName").val(SuppName)


        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
        $("#itmretrnID").css("display", "none");
        $('#ReturnItemDetailsTbl >tbody >tr').remove();
        
    }
    BindJobOrderNumber(Supp_id);
    //if (doc_no == "---Select---") {
    //    $("#itmretrnID").css("display", "none");
    //    $('#ReturnItemDetailsTbl >tbody >tr').remove();
    //}
}
function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    return true;
}
function OnTextChangeBillNumber() {

    debugger;
    var BillNUmber = $('#BillNumber').val().trim();


    if (BillNUmber != "0") {
        document.getElementById("vmbill_no").innerHTML = "";
        $("#BillNumber").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmbill_no").innerHTML = $("#valueReq").text();
        $("#BillNumber").css("border-color", "red");
    }

}
function OnTextChangeBillDate() {

    debugger;
    var BillDate = $('#BillDate').val().trim();


    if (BillDate != "0") {
        document.getElementById("vmbill_date").innerHTML = "";
        $("#BillDate").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("vmbill_date").innerHTML = $("#valueReq").text();
        $("#BillDate").css("border-color", "red");
    }

}
function onchangVehicleLoadInTonnage(el, e) {
    debugger;
    var QtyDecDigit = $("#RateDigit").text();
    VehicleLoadInTonnage = $("#VehicleLoadInTonnage").val();
    $("#VehicleLoadInTonnage").val(parseFloat(CheckNullNumber(VehicleLoadInTonnage)).toFixed(QtyDecDigit));

}
function BindJobOrderNumber(Supp_id) {
    debugger;
    

    $.ajax({
        type: 'POST',
        url: '/ApplicationLayer/DeliveryNoteSC/GetJobORDDocNOList',
        data: { Supp_id: Supp_id },
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            }
            if (data !== null && data !== "" && data !== "{}") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.Table.length > 0 ) {
                    debugger;
                    $("#JobOrdNo option").remove();
                    $("#JobOrdNo optgroup").remove();
                    //$("#Hdn_FinishProduct").val(arr.Table[0].fitem_name);
                    //$("#Hdn_FinishUom").val(arr.Table[0].fuom);
                    $('#JobOrdNo').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}' data-fitem='${$("#span_FinishedProduct").text()}' data-fuom='${$("#ItemUOM").text()}'></optgroup>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#Textddl').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no + ',' + arr.Table[i].fitem_id + ',' + arr.Table[i].uom_id}" data-fitem = "${arr.Table[i].fitem_name}" data-fuom = "${arr.Table[i].fuom}">${arr.Table[i].doc_no}</option>`);
                       
                    }
                    
                    var firstEmptySelect = true;

                    $('#JobOrdNo').select2({
                        templateResult: function (data) {
                            var DocDate = $(data.element).data('date');
                            var fitem = $(data.element).data('fitem');
                            var fuom = $(data.element).data('fuom');
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var $result = $(
                                '<div class="row jo_dll">' +
                                '<div class="col-md-3 col-xs-4' + classAttr + '">' + data.text + '</div>' +
                                '<div class="col-md-3 col-xs-4' + classAttr + '">' + DocDate + '</div>' +
                                '<div class="col-md-3 col-xs-4' + classAttr + '">' + fitem + '</div>' +
                                '<div class="col-md-3 col-xs-4' + classAttr + '">' + fuom + '</div>' +
                                '</div>'
                            );
                            return $result;
                            firstEmptySelect = false;
                        }
                    });
                    
                    $("#JobOrdDate").val("");


                }
                
            }
        }

    })

}

function OnchangeJobORDDocNumber() {
    debugger;
    var suppname = $("#SupplierName").val();
    /*var doc_no = $("#JobOrdNo").val();*/
    var JOdocno = $("#JobOrdNo").val();
    var docno = JOdocno.split(',');
    var doc_no = docno[0];
    var FitemId = docno[1];
    var FUomId = docno[2];
   
    if (doc_no != 0) {

        var doc_Date = $("#JobOrdNo option:selected")[0].dataset.date
        var FItmName = $("#JobOrdNo option:selected")[0].dataset.fitem
        var FUom = $("#JobOrdNo option:selected")[0].dataset.fuom
        var newdate = doc_Date.split("-").reverse().join("-");
        $("#JobOrdDate").val(newdate);
        $("#Hdn_JobNum").val(doc_no);
        $("#SpanJobOrdNoErrorMsg").css("display", "none");
        $("#JobOrdNo").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-JobOrdNo-container']").css("border-color", "#ced4da");
        debugger;

        $("#FinishProduct").val(FItmName);
        $("#FinishUom").val(FUom);
        $("#Hdn_FinishProductid").val(FitemId);
        $("#Hdn_FinishUomId").val(FUomId);

        BindDLLReturnItemList();
        $("#delBtnIcon").prop("disabled", false);
        $("#itmretrnID").css("display", "block");
        BindMDNoOnBehalfOFJobOrdNO();
        
    }
    if (doc_no == "---Select---,0,0") {
        $("#FinishProduct").val("");
        $("#FinishUom").val("");
        $("#Hdn_FinishProductid").val("");
        $("#Hdn_FinishUomId").val("");
        $("#itmretrnID").css("display", "none");
        $('#ReturnItemDetailsTbl >tbody >tr').remove();
    }
}
function BindMDNoOnBehalfOFJobOrdNO() {
    debugger;
    var docno = $("#MtrlDispNo").val();
    var JONO=$("#Hdn_JobNum").val();
    //$("#JobOrdNo").attr("onchange", "");
    $.ajax({
        type: 'POST',
        url: '/ApplicationLayer/DeliveryNoteSC/GetMDDocNOList',
        data: {
            JONO: JONO

        },
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            }
            if (data !== null && data !== "" && data !== "{}") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.Table.length > 0) {
                    debugger;
                   
                    $("#MtrlDispNo option").remove();
                    $("#MtrlDispNo optgroup").remove();
                    $("#MtrlDispNo").attr("onchange", "OnchangMDDocNumber()");

                    $('#MtrlDispNo').append(`<optgroup class='def-cursor' id="TextddlDNSC" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#TextddlDNSC').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}">${arr.Table[i].doc_no}</option>`);
                        
                    }

                    var firstEmptySelect = true;

                    $('#MtrlDispNo').select2({
                        templateResult: function (data) {
                            var DocDate = $(data.element).data('date');
                            var classAttr = $(data.element).attr('class');
                            var hasClass = typeof classAttr != 'undefined';
                            classAttr = hasClass ? ' ' + classAttr : '';
                            var flg = "N";
                            $("#SaveItemDispatchQtyDetails >tbody >tr").each(function (i, row) {
                                debugger;
                     var currentRow = $(this);
                                var Mdno = currentRow.find("#PD_DispDocNo").val();
                                if (data.text == Mdno) {
                                    debugger;
                                   
                                    flg = "Y";
                                }
                            });
                            if (flg == "N") {
                                var $result = $(
                                    '<div class="row md_ddl">' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                                    '</div>'
                                );
                                
                                return $result;
                            }
                            
                            firstEmptySelect = false;
                        }
                    });

                    $("#MatrilDispDate").val("");


                }
                debugger;
                if (arr.Table1.length > 0) {
                    var JOBOrdTyp = arr.Table1[0].src_type
                    $("#DNSChdn_JobOrdTyp").val(JOBOrdTyp);
                    var JobTyp = $("#DNSChdn_JobOrdTyp").val();
                    //if (JobTyp == "D") {
                    //    var JobOrdtyp ="Direct"
                    //}
                    if (JobTyp == "D") {
                       $("#TH_DNhdnMtrltyp").css("display", "none");
                    }
                    
                }
                if (JobTyp == "D") {
                    if (arr.Table2.length > 0) {
                        for (var y = 0; y < arr.Table2.length; y++) {
                            var ItmId = arr.Table2[y].item_id;
                            $("#hdn_JobOrd_InputItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="JOInputItemId" value='${ItmId}'></td>
                         </tr>`);
                        }
                    }
                }
            }
        }

    })

}

function OnchangMDDocNumber() {
    debugger;
    var suppname = $("#SupplierName").val();
    var doc_no = $("#MtrlDispNo").val();
    
    if (doc_no != "0" || doc_no != "---Select---") {

        var doc_Date = $("#MtrlDispNo option:selected")[0].dataset.date
        var newdate = doc_Date.split("-").reverse().join("-");
        $("#MatrilDispDate").val(newdate);
        $("#Hdn_MDNum").val(doc_no);
        $("#SpanMDNoErrorMsg").css("display", "none");
        $("#MtrlDispNo").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "#ced4da");

    }
    else {
        $("#SpanMDNoErrorMsg").text($("#valueReq").text());
        $("#MtrlDispNo").css("border-color", "red")
        $("#SpanMDNoErrorMsg").css("display", "block");
    }
}
function OnKeyPressBLQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#BilledQuantity_Error").css("display", "none");
    clickedrow.find("#BilledQuantity").css("border-color", "#ced4da");
    
    return true;
}

/*-----------------------------Item DEtail start---------------*/
//function OnChangeBLQty(el, evt) {
//    debugger;
//    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    
//    var clickedrow = $(evt.target).closest("tr");

//    var BilledQuantity = clickedrow.find("#BilledQuantity").val();

//    if ((BilledQuantity == "") || (BilledQuantity == null)) {
//        var test = parseFloat(parseFloat("0")).toFixed(parseFloat(QtyDecDigit));
//        clickedrow.find("#BilledQuantity").val(test);
//    }
//    else {
//        var test = parseFloat(parseFloat(BilledQuantity)).toFixed(parseFloat(QtyDecDigit));
//        clickedrow.find("#BilledQuantity").val(test);
//        clickedrow.find("#ReceivedQuantity").val(test);
//        clickedrow.find("#ReceivedQuantity_Error").css("display", "none");
//        clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
//    }
//}
//function OnKeyPressRecQty(el, evt) {
   
//    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
//        return false;
//    }
//    var clickedrow = $(evt.target).closest("tr");
//    clickedrow.find("#ReceivedQuantity_Error").css("display", "none");
//    clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
//    return true;
//}
//function OnChangeRecQty(el, evt) {
//    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    
//    var clickedrow = $(evt.target).closest("tr");

//    var ReceivedQuantity = clickedrow.find("#ReceivedQuantity").val();

//    if ((ReceivedQuantity == "") || (ReceivedQuantity == null)) {
//        var test = parseFloat(parseFloat("0")).toFixed(parseFloat(QtyDecDigit));
//        clickedrow.find("#ReceivedQuantity").val(test);
//    }
//    else {
//        var test = parseFloat(parseFloat(ReceivedQuantity)).toFixed(parseFloat(QtyDecDigit));
//        clickedrow.find("#ReceivedQuantity").val(test);
//    }
//}
//function ReceiveQtyChange(el, evt) {
//    debugger;
//    var clickedrow = $(evt.target).closest("tr");
//    var str = clickedrow.find("#ReceivedQuantity").val();
//    var clickedrow = $(evt.target).closest("tr");
//    if (clickedrow.find("#ReceivedQuantity").val() == "" || clickedrow.find("#ReceivedQuantity").val() == "0") {
//        clickedrow.find("#ReceivedQuantity_Error").css("display", "block");
//        clickedrow.find("#ReceivedQuantity").css("border-color", "red");
//    }
//    else {
//        clickedrow.find("#ReceivedQuantity_Error").css("display", "none");
//        clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
//    }

//}

function AddItemDetail() {
    debugger;
    var rowCount = $('#DnSCItmDetailsTbl >tbody >tr').length;
    if (rowCount == "0" || rowCount>0) {
        if (DNHeaderValidations() == false) {
            return false;
        }
        if ($("#MtrlDispNo").val() == "0" || $("#MtrlDispNo").val() == "---Select---") {
            $("#SpanMDNoErrorMsg").text($("#valueReq").text());
            $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "red")
            $("#MtrlDispNo").css("border-color", "red")
            $("#SpanMDNoErrorMsg").css("display", "block");
           
            ErrorFlag = "Y";
        }
        else {
            $("#SpanMDNoErrorMsg").css("display", "none");
            $("#MtrlDispNo").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "#ced4da");

        }
       
    }
    


    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var DNNo = $('#DeliveryNoteNumber').val();
    var docno = $('#MtrlDispNo option:selected').text();
    $("#Hdn_MDNum").val(docno);
    var docnoValue = $('#MtrlDispNo option:selected').val();
    if ($('#MtrlDispNo').val() != "0" && $('#MtrlDispNo').val() != "") {
        var text = $('#MtrlDispNo').val();
       
        debugger;
        
        var hdSelectedSourceDocument = null;
        var SourDocumentNo = $('#MtrlDispNo option:selected').text();
        var SourDocumentDate = $('#MatrilDispDate').val();

        //$("#Hdn_MDNum").val(SourDocumentNo);
        //$("#Hdn_MDDate").val(SourDocumentDate);

        hdSelectedSourceDocument = SourDocumentNo;
        $("#hdSelectedSourceDocument").val(hdSelectedSourceDocument);
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/DeliveryNoteSC/getDetailBySourceDocumentMDNo",
                data: {
                    SourDocumentNo: hdSelectedSourceDocument, SourDocumentDate: SourDocumentDate, DNNo: DNNo
                },
                success: function (data) {
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {

                            var rowIdx = 0;
                            //var rowCount = $('#DnSCItmDetailsTbl >tbody >tr').length + 1;
                            //var len = parseInt($('#DnSCItmDetailsTbl tbody tr').length);
                            var rowCount = $('#DnSCItmDetailsTbl >tbody >tr').length;
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }

                                var FDispatchQty = parseFloat(0).toFixed(QtyDecDigit);
                                var FPendingQty = parseFloat(0).toFixed(QtyDecDigit);
                                var FBilledQty = parseFloat(0).toFixed(QtyDecDigit);
                                var FReceiveQty = parseFloat(0).toFixed(QtyDecDigit);
                                

                                var DocNo = arr.Table[i].dispatch_no;
                                var DocDate = arr.Table[i].dispatch_date;
                                var ItemID = arr.Table[i].item_id;
                                var sub_item = arr.Table[i].sub_item;
                                var UOMID = arr.Table[i].base_uom_id;
                                var DispatchQty = arr.Table[i].disp_qty;
                                var PendingQty = arr.Table[i].PendQty;
                                var BillQty = arr.Table[i].BilledQty;
                                var ReceivedQty = arr.Table[i].RecievedQty;
                                if (BillQty == "") {
                                    BillQty = 0;
                                }
                                if (ReceivedQty == "") {
                                    ReceivedQty = 0;
                                }
                                var CheckDocNo = "";
                                $("#SaveItemDispatchQtyDetails TBODY TR").each(function () {
                                    var row = $(this);
                                    var rowitem = row.find("#PD_DispQtyItemID").val();
                                    var Docno = row.find("#PD_DispDocNo").val();
                                    if (rowitem == ItemID && DocNo == Docno) {
                                        CheckDocNo = Docno;
                                        $(this).remove();
                                    }
                                });
                                //if (CheckDocNo == "") {
                                $('#SaveItemDispatchQtyDetails tbody').append(`<tr id="${++rowIdx}">
                        <td class="sr_padding"><span id="SNoRowId">${rowCount}</span></td>
                        <td style="display:none;"><input type="hidden" id="hdSNoRowID" value="${i + 1}" style="display: none;" /></td>
                    <td>
                    <input type="text" id="PD_DispQtyItemID" value="${ItemID}" />
                    <input hidden type="text" id="sub_item" value="${sub_item}" />
                    </td>
                    <td><input type="text" id="PD_DispQtyUomID" value="${UOMID}" /></td>
                    <td><input type="text" id="PD_DispDocNo" value="${DocNo}" /></td>
                    <td><input type="text" id="PD_DispDocDate" value="${DocDate}" /></td>
                    <td>
                        <div class="col-sm-10 lpo_form no-padding">
                        <input type="text" id="PD_DispQty" value="${parseFloat(DispatchQty).toFixed(QtyDecDigit)}" />
                        </div>
                        <div class="col-sm-2 i_Icon no-padding" id="div_PD_SubItemDispQty" >
                        <button type="button" id="PD_SubItemDispQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCDispQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                        </div>
                    </td>
                    <td>
                        <div class="col-sm-10 lpo_form no-padding">
                        <input type="text" id="PD_PendQty" value="${parseFloat(PendingQty).toFixed(QtyDecDigit)}" />
                        </div>
                        <div class="col-sm-2 i_Icon no-padding" id="div_PD_SubItemPendQty" >
                        <button type="button" id="PD_SubItemPendQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCPendQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                        </div>
                    </td>
                    <td>
                        <div class="col-sm-10 lpo_form no-padding">
                            <input type="text" id="PD_BillQty" value="${parseFloat(BillQty).toFixed(QtyDecDigit)}" />
                            <span id="Disp_BilledQuantity_Error" class="error-message is-visible"></span>
                        </div>
                        <div class="col-sm-2 i_Icon no-padding" id="div_PD_SubItemBillQty" >
                        <button type="button" id="PD_SubItemBillQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCBillQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                        </div>
                        
                    </td>
                    <td>
                            <div class="col-sm-10 lpo_form no-padding">
                                <input type="text" id="PD_ReceivQty" value="${parseFloat(ReceivedQty).toFixed(QtyDecDigit)}" />
                                <span id="Disp_ReceivedQuantity_Error" class="error-message is-visible"></span>
                            </div>
                        <div class="col-sm-2 i_Icon no-padding" id="div_PD_SubItemRecvQty" >
                        <button type="button" id="PD_SubItemRecvQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCRecQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                        </div>
                    </td>

                </tr>`)

                                if ($('#DnSCItmDetailsTbl tbody tr').length > 0) {
                                    var hfitemid = "";
                                    $('#DnSCItmDetailsTbl tbody tr').each(function () {
                                        
                                        var CurrRow = $(this);
                                        var tblItemID = CurrRow.find("#hdItemId").val();
                                        var tblOrderQty = CurrRow.find("#DispatchQuantity").val();
                                        var tblPendingrQty = CurrRow.find("#PendingQuantity").val();
                                        var tblBillQty = CurrRow.find("#BilledQuantity").val();
                                        var tblRecievQty = CurrRow.find("#ReceivedQuantity").val();
                                        if (tblBillQty == "") {
                                            tblBillQty = 0;
                                        }
                                        if (tblRecievQty == "") {
                                            tblRecievQty = 0;
                                        }

                                        if (tblItemID === ItemID) {
                                            hfitemid = tblItemID;
                                            if (DocNo != CheckDocNo) {

                                                FDispatchQty = parseFloat(DispatchQty) + parseFloat(tblOrderQty);
                                                FPendingQty = parseFloat(PendingQty) + parseFloat(tblPendingrQty);
                                                FBilledQty = parseFloat(BillQty) + parseFloat(tblBillQty);
                                                FReceiveQty = parseFloat(ReceivedQty) + parseFloat(tblRecievQty);
                                                CurrRow.find("#DispatchQuantity").val(parseFloat(FDispatchQty).toFixed(QtyDecDigit));
                                                CurrRow.find("#PendingQuantity").val(parseFloat(FPendingQty).toFixed(QtyDecDigit));
                                                CurrRow.find("#BilledQuantity").val(parseFloat(FBilledQty).toFixed(QtyDecDigit));
                                                CurrRow.find("#ReceivedQuantity").val(parseFloat(FReceiveQty).toFixed(QtyDecDigit));
                                            }

                                        }
                                        else {
                                            FDispatchQty = parseFloat(DispatchQty);
                                            FPendingQty = parseFloat(PendingQty);
                                            FBilledQty = parseFloat(BillQty);
                                            FReceiveQty = parseFloat(ReceivedQty);
                                        }
                                    });

                                    if (hfitemid != ItemID) {
                                        rowCount = rowCount + 1;



                                        var qc_val = "";
                                        if (arr.Table[i].i_qc == 'Y') {
                                            qc_val = `<div class="custom-control custom-switch sample_issue" >
                                                                    <input type="checkbox" class="custom-control-input  margin-switch" id="QC_${i + 1}" checked>
                                                                    <label class="custom-control-label  for="QC_${i + 1}" ></label>
                                                             </div>`
                                        }
                                        else {
                                            qc_val = `<div class="custom-control custom-switch sample_issue" >
                                        <input type="checkbox" class="custom-control-input margin-switch" id="QC_${i + 1}" disabled>
                                            <label class="custom-control-label for="QC_${i + 1}" ></label>
                                                             </div>`
                                        }

                                        $('#DnSCItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">
                                                       <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                        <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                                                         <td style="display:none;"><input type="hidden" id="hdSpanRowID" value="${i + 1}" style="display: none;" /></td>
                                                        
                                                        <td class="ItmNameBreak itmStick tditemfrz">
                                                            <div class="col-sm-11 lpo_form no-padding" id="">
                                                                <input id="ProductName" class="form-control" autocomplete="off" type="text" value="${arr.Table[i].item_name}" name="ProductName" placeholder="${$("#span_ProductName").text()}" disabled>

                                                                <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                <input type="hidden" id="sub_item" value="${arr.Table[i].sub_item}" style="display: none;" />
                                                            </div>
                                                           <div class="col-sm-1 i_Icon">
                                                                <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="UOM" class="form-control" autocomplete="off" type="text" value="${arr.Table[i].uom_name}" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
                                                            <input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" />
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="DispatchQuantity" value="${parseFloat(FDispatchQty).toFixed(QtyDecDigit)}"  class="form-control num_right" autocomplete="Off" type="text" name="DispatchQuantity" placeholder="0000.00" disabled>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon">
                                                               <button type="button" id="DispatchDetailBtnIcon" class="calculator" onclick="OnClickItemDispatchQtyIconBtn(event);" data-toggle="modal" data-target="#DispatchDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#span_DispatchDetail").text()}"> </button>
                                                             </div>
                                                        </td>
                                                        <td>
                                                        <input id="PendingQuantity" value="${parseFloat(FPendingQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="Off" type="text" name="PendingQuantity" placeholder="0000.00" disabled>
                                                    </td>
                                                        <td>
                                                                   <div class="lpo_form">                     
                                                            <input id="BilledQuantity" value="${parseFloat(FBilledQty).toFixed(QtyDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="BilledQuantity" placeholder="0000.00"  >
                                                                 <span id="BilledQuantity_Error" class="error-message is-visible"></span>
                                                        </div>
                                                        </td>
                                                        <td>
                                                                <div class="lpo_form">
                                                                    <input id="ReceivedQuantity" value="${parseFloat(FReceiveQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="ReceivedQuantity" placeholder="0000.00"  >
                                                                  <span id="ReceivedQuantity_Error" class="error-message is-visible"></span>
                                                        </div>
                                                        </td>
                                                        <td>
                                                       `+ qc_val + `
                                                        </td>
                                                        <td>
                                                          <div class="col-sm-10 lpo_form no-padding">
                                                             <input id="AcceptedQuantity" value="${parseFloat(arr.Table[i].accept_qty).toFixed(QtyDecDigit)}" disabled class="form-control num_right" autocomplete="" type="text" name="AcceptedQuantity" placeholder="0000.00"  >
                                                          </div>
                                                          <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAcceptQty" >
                                                            <button type="button" id="SubItemAcceptQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCAcceptQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                          </div>
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="RejectedQuantity" value="${parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit)}" disabled class="form-control num_right" autocomplete="" type="text" name="RejectedQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemRejectQty" >
                                                            <button type="button" id="SubItemRejectQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCRejctQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="ReworkableQuantity" value="${parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit)}"     disabled class="form-control num_right" autocomplete="" type="text" name="ReworkableQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReworkQty" >
                                                            <button type="button" id="SubItemReworkQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCRewrkQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
</td>
                                                        <td>
                                                                <textarea id="txtItemRemarks"  value="${arr.Table[i].it_remarks}" class="form-control remarksmessage" maxlength="100" onmouseover="OnMouseOver(this)" name="remarks"  placeholder="${$("#span_remarks").text()}"></textarea>
                                                           
                                                        </td>
                                                    </tr>`);
                                    }
                                }
                                else {
                                    rowCount = rowCount + 1;
                                    FDispatchQty = parseFloat(DispatchQty);
                                    FPendingQty = parseFloat(PendingQty);
                                    //FBilledQty = parseFloat(BillQty);
                                    //FReceiveQty = parseFloat(ReceivedQty);
                                    FBilledQty = BillQty;
                                    FReceiveQty = ReceivedQty;
                                    var qc_val = "";
                                    if (arr.Table[i].i_qc == 'Y') {
                                        qc_val = `<div class="custom-control custom-switch sample_issue" >
                                                                    <input type="checkbox" class="custom-control-input  margin-switch" id="QC_${i + 1}" checked>
                                                                    <label class="custom-control-label  for="QC_${i + 1}" ></label>
                                                             </div>`
                                    }
                                    else {
                                        qc_val = `<div class="custom-control custom-switch sample_issue" >
                                        <input type="checkbox" class="custom-control-input margin-switch" id="QC_${i + 1}" disabled>
                                            <label class="custom-control-label for="QC_${i + 1}" ></label>
                                                             </div>`
                                    }

                                    $('#DnSCItmDetailsTbl tbody').append(`<tr id="${++rowIdx}">
                                                       <td class="red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                        <td class="sr_padding"><span id="SpanRowId">${rowCount}</span></td>
                                                         <td style="display:none;"><input type="hidden" id="hdSpanRowID" value="${i + 1}" style="display: none;" /></td>
                                                        
                                                        <td class="ItmNameBreak itmStick tditemfrz">
                                                            <div class="col-sm-11 lpo_form no-padding" id="">
                                                                <input id="ProductName" class="form-control" autocomplete="off" type="text" value="${arr.Table[i].item_name}" name="ProductName" placeholder="${$("#span_ProductName").text()}" disabled>

                                                                <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                <input type="hidden" id="sub_item" value="${arr.Table[i].sub_item}" style="display: none;" />
                                                                </div>
                                                           <div class="col-sm-1 i_Icon">
                                                                <button type="button" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <input id="UOM" class="form-control" autocomplete="off" type="text" value="${arr.Table[i].uom_name}" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
                                                            <input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" />
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="DispatchQuantity" value="${parseFloat(arr.Table[i].disp_qty).toFixed(QtyDecDigit)}"  class="form-control num_right" autocomplete="Off" type="text" name="DispatchQuantity" placeholder="0000.00" disabled>
                                                            </div>
                                                            <div class="col-sm-2 i_Icon">
                                                               <button type="button" id="DispatchDetailBtnIcon" class="calculator" onclick="OnClickItemDispatchQtyIconBtn(event);" data-toggle="modal" data-target="#DispatchDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                             </div>
                                                        </td>
                                                        <td>
                                                        <div class="col-sm-10 lpo_form no-padding">
                                                        <input id="PendingQuantity" value="${parseFloat(arr.Table[i].PendQty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="Off" type="text" name="PendingQuantity" placeholder="0000.00" disabled>
                                                        </div>
                                                            <div class="col-sm-2 i_Icon">
                                                               <button type="button" id="DispatchDetailBtnIcon" class="calculator" onclick="OnClickItemDispatchQtyIconBtn(event);" data-toggle="modal" data-target="#DispatchDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                                   <div class="lpo_form">
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                          <input id="BilledQuantity" value="${arr.Table[i].BilledQty}" class="form-control num_right" autocomplete="off" type="text" name="BilledQuantity" placeholder="0000.00" disabled >
                                                                 <span id="BilledQuantity_Error" class="error-message is-visible"></span>
                                                            </div> <div class="col-sm-2 i_Icon">
                                                                                   
                                                                <button type="button" id="DispatchDetailBtnIcon" class="calculator" onclick="OnClickItemDispatchQtyIconBtn(event);" data-toggle="modal" data-target="#DispatchDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                                                </div>
                                                        </div>
                                                        </td>
                                                        <td>
                                                                <div class="lpo_form">
                                                                <div class="col-sm-10 lpo_form no-padding">
                                                                    <input id="ReceivedQuantity" value="${arr.Table[i].RecievedQty}" class="form-control num_right" autocomplete="off" type="text" name="ReceivedQuantity" placeholder="0000.00" disabled >
                                                                  <span id="ReceivedQuantity_Error" class="error-message is-visible"></span>
 
                                                                        </div>
                                                                        <div class="col-sm-2 i_Icon">
                                                                                    <button type="button" id="DispatchDetailBtnIcon" class="calculator" onclick="OnClickItemDispatchQtyIconBtn(event);" data-toggle="modal" data-target="#DispatchDetail" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_QuantityDetail_Title").text()}"> </button>
                                                                                </div>
                                                        </div>
                                                        </td>
                                                        <td>
                                                       `+ qc_val + `
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="AcceptedQuantity" value="${parseFloat(arr.Table[i].accept_qty).toFixed(QtyDecDigit)}" disabled class="form-control num_right" autocomplete="" type="text" name="AcceptedQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAcceptQty" >
                                                            <button type="button" id="SubItemAcceptQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCAcceptQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">

                                                                <input id="RejectedQuantity" value="${parseFloat(arr.Table[i].reject_qty).toFixed(QtyDecDigit)}" disabled class="form-control num_right" autocomplete="" type="text" name="RejectedQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemRejectQty" >
                                                            <button type="button" id="SubItemRejectQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCRejctQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="col-sm-10 lpo_form no-padding">
                                                                <input id="ReworkableQuantity" value="${parseFloat(arr.Table[i].rework_qty).toFixed(QtyDecDigit)}"     disabled class="form-control num_right" autocomplete="" type="text" name="ReworkableQuantity" placeholder="0000.00"  >
                                                            </div>
                                                            <div class="col-sm-2 i_Icon no-padding" id="div_SubItemReworkQty" >
                                                            <button type="button" id="SubItemReworkQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('DNSCRewrkQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                                                            </div>
                                                        </td>
                                                        <td>
                                                                <textarea id="txtItemRemarks"  value="${arr.Table[i].it_remarks}" class="form-control remarksmessage" maxlength="100" onmouseover="OnMouseOver(this)" name="remarks"  placeholder="${$("#span_remarks").text()}"></textarea>
                                                           
                                                        </td>
                                                    </tr>`);

                                }
                            }
                            //$('#MtrlDispNo').val("---Select---").prop('selected', true);
                            //$('#MtrlDispNo').trigger('change');
                            DisableHeaderDetail();
                            $("#MtrlDispNo option[value='" + docnoValue + "']").select2().hide();
                            $("#MtrlDispNo").val("---Select---").trigger("change");
                            
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    } else {

        var Str = $("#valueReq").text();
        
        $("[aria-labelledby='select2-ddlDocumentNumber-container']").css("border-color", "red");
    }
}
//function ResetMtrlDisp_DDL_DetailAfterAddItem() {
//    debugger;
//    $("#MatrilDispDate").val("");
//    var DocNo = $('#MtrlDispNo').val();
//    $("#MtrlDispNo>optgroup>option[value='" + DocNo + "']").select2().hide();

//    $('#MtrlDispNo').val("---Select---").prop('selected', true);
//    $('#MtrlDispNo').trigger('change'); // Notify any JS components that the value changed
//    $('#MtrlDispNo').select2('close');

//    $("#SpanMDNoErrorMsg").css("display", "none");
//    $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "#ced4da");
//    $("#MtrlDispNo").css("border-color", "red");
//}

function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#DnSCItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};

function DisableHeaderDetail() {
    $("#SupplierName").prop("disabled", true);
    $("#JobOrdNo").prop("disabled", true);
    //$("#BillNumber").prop("readonly", true);
    //$("#BillDate").prop("readonly", true);
    //$("#VehicleNo").prop("readonly", true);
    //$("#VehicleLoadInTonnage").prop("readonly", true);
    //$("#Remarks").prop("readonly", true);
    //$("#MtrlDispNo").prop("disabled", true);
}
function EnableHeaderDetail() {
    $("#SupplierName").prop("disabled", false);
    $("#JobOrdNo").prop("disabled", false);
    $("#BillNumber").prop("readonly", false);
    $("#BillDate").prop("readonly", false);
    $("#VehicleNo").prop("readonly", false);
    $("#VehicleLoadInTonnage").prop("readonly", false);
    $("#Remarks").prop("readonly", false);
    $("#MtrlDispNo").prop("disabled", false);
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    // var Sno = clickedrow.find("#SpanRowId").val();
    var ItmCode = clickedrow.find("#hdItemId").val();
   
    ItemInfoBtnClick(ItmCode);
   
}
/*---------------Insert Save Data Section start---------------*/
function DNHeaderValidations() {
    debugger;
    var ErrorFlag = "N";
    var suppname = $("#SupplierName").val();
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
    }

    if ($("#BillNumber").val() == "" || $("#BillNumber").val() == null) {
        $('#vmbill_no').text($("#valueReq").text());
        $("#vmbill_no").css("display", "block");
        $("#BillNumber").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#vmbill_no").css("display", "none");
        $("#BillNumber").css("border-color", "#ced4da");
    }
    if ($("#BillDate").val() == "" || $("#BillDate").val() == null) {
        $('#vmbill_date').text($("#valueReq").text());
        $("#vmbill_date").css("display", "block");
        $("#BillDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#vmbill_date").css("display", "none");
        $("#BillDate").css("border-color", "#ced4da");
    }
    
    debugger;
    if (suppname != "0") {
        if ($("#JobOrdNo").val() == "0" || $("#JobOrdNo").val() == "---Select---,0,0") {
            $("#SpanJobOrdNoErrorMsg").text($("#valueReq").text());
            $("[aria-labelledby='select2-JobOrdNo-container']").css("border-color", "red")
            $("#SpanJobOrdNoErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanJobOrdNoErrorMsg").css("display", "none");
            $("#JobOrdNo").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-JobOrdNo-container']").css("border-color", "#ced4da");
        }
        //if ($("#MtrlDispNo").val() == "0" || $("#MtrlDispNo").val() == "---Select---") {
        //    $("#SpanMDNoErrorMsg").text($("#valueReq").text());
        //    $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "red")
        //    $("#MtrlDispNo").css("border-color", "red")
        //    $("#SpanMDNoErrorMsg").css("display", "block");
        //    ErrorFlag = "Y";
        //}
        //else {
        //    $("#SpanMDNoErrorMsg").css("display", "none");
        //    $("#MtrlDispNo").css("border-color", "#ced4da");
        //    $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "#ced4da");
            
        //}
        /*if ($("#BillDate").val() != "" || $("#BillDate").val() != null) {*/
            var Billdate = $("#BillDate").val();
            var MDdate = $("#MatrilDispDate").val();
          
            debugger;
            if (Billdate != "" && MDdate != "") {
                if (Billdate < MDdate) {
                    swal("", $("#BillDateExceedsSourceDate").text(), "warning");
                    $("#BillDate").val("");
                    ErrorFlag = "Y";
                }
            }
        /*}*/
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function DNItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#DnSCItmDetailsTbl >tbody >tr").length > 0) {
        $("#DnSCItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            ///   var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#BilledQuantity").val() == "" || parseFloat(currentRow.find("#BilledQuantity").val()) == parseFloat("0")) {
                currentRow.find("#BilledQuantity_Error").text($("#valueReq").text());
                currentRow.find("#BilledQuantity_Error").css("display", "block");
                currentRow.find("#BilledQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#BilledQuantity_Error").css("display", "none");
                currentRow.find("#BilledQuantity").css("border-color", "#ced4da");
            }
            if (currentRow.find("#ReceivedQuantity").val() == "" || parseFloat(currentRow.find("#ReceivedQuantity").val()) == parseFloat("0")) {
                currentRow.find("#ReceivedQuantity_Error").text($("#valueReq").text());
                currentRow.find("#ReceivedQuantity_Error").css("display", "block");
                currentRow.find("#ReceivedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ReceivedQuantity_Error").css("display", "none");
                currentRow.find("#ReceivedQuantity").css("border-color", "#ced4da");
            }
        })
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}

//function DnItemRowANDItemReturnRowCheck() {
//    debugger;
//    var ErrorFlag ="N";
//    if (($("#DnSCItmDetailsTbl >tbody >tr").length > 0) || ($("#ReturnItemDetailsTbl >tbody >tr").length > 0)) {
//        ErrorFlag = "N";
//    }
//    else {

//        swal("", $("#noitemselectedmsg").text(), "warning");
//        ErrorFlag = "Y";
//    }
//    if (ErrorFlag == "Y") {
//        return false;
//    }
//    else {
//        return true;
//    }
//}

function SaveBtnClick() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 15-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    var rowCountItm = $('#DnSCItmDetailsTbl >tbody >tr').length;
    var ItmCode = $("#RQhdItemId").val();
    var rowCountRtrnItm = $('#ReturnItemDetailsTbl >tbody >tr').length;
    if (rowCountItm == "0" || rowCountRtrnItm=="0") {
        if (DNHeaderValidations() == false) {
            return false;
        }
    }
    $("#SpanMDNoErrorMsg").css("display", "none");
    $("#MtrlDispNo").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-MtrlDispNo-container']").css("border-color", "#ced4da");

    //if (DnItemRowANDItemReturnRowCheck() == false) {
    //    return false;
    //}
    //else {
       /* if (DnItemRowANDItemReturnRowCheck() == true) {*/
            debugger;
            if (DNItemValidations() == false) {
                return false;
    }
    if (ItemtbltoDispQtyTbltoSubItmValidation("Y", "") == false) {
        swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        return false
    }
    else {
        $("#SaveAndExitBtn").attr("data-dismiss", "modal");

    }
            if (ReturnItemValidations() == false) {
                return false;
    }
    if (BYProductScrapCheckValidations_forSubItems() == false) {
        return false;
    }
       /* }*/
   /* }*/
    
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {

        var DNNum = $("#DeliveryNoteNumber").val();
        if (DNNum == null || DNNum == "") {
            $("#hdtranstype").val("Save");
            $("#hdn_TransType").val($("#hdtranstype").val());
        }
        else {
            $("#hdtranstype").val("Update");
            $("#hdn_TransType").val($("#hdtranstype").val());
        }
        var FinalDNItemDetail = [];
        FinalDNItemDetail = InsertDNItemDetails();
        $("#hdDNSCItemDetailList").val(JSON.stringify(FinalDNItemDetail));

        /*----------- Dispatch Quantity Sub-item-------------*/

        var SubItemsListArr = DN_SubItemList();
        var str2 = JSON.stringify(SubItemsListArr);
        $('#SubItemDetailsDt').val(str2);

        /*-----------Dispatch Quantity Sub-item end-------------*/

        var FinalDNItemReturnDetail = [];
        FinalDNItemReturnDetail = InsertDNItemReturnDetails();
        $("#hdItemReturnDetailList").val(JSON.stringify(FinalDNItemReturnDetail));
        BindDispatchQtyDetails();
        debugger;
        ///*----------- By Product SCrap Sub-item-------------*/

        var BYPrdctScrapSubItemsListArr = SaveByPrdctScrap_SubItemList();
        var str2 = JSON.stringify(BYPrdctScrapSubItemsListArr);
        $('#SubItemByProductScrapDetailsDt').val(str2);

        ///*-----------By Product SCrap Sub-item end-------------*/
        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $("#hdn_Attatchment_details").val(ItemAttchmentDt);
        /*----- Attatchment End--------*/

        $("#Hdn_SupplierName").val($("#SupplierName").val());
        var JOdocno = $("#JobOrdNo").val();
        var docno = JOdocno.split(',');
        var doc_no = docno[0];
        $("#Hdn_JobNum").val(doc_no);
        $("#Hdn_MDNum").val($("#MtrlDispNo").val());
        //var JoNo = $("#Hdn_JobNum").val();
        //$("#Hdn_ProducNum").val(JoNo);
        debugger; 
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_DNSupplierName").val(SuppName)

        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;

    }
}
function InsertDNItemDetails() {
    debugger;
    var DNItemList = [];
    $("#DnSCItmDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        
        var currentRow = $(this);
        var SNo = currentRow.find("#hdSpanRowID").val();
        var SRCDocNo = "";
        var SRCDocDate = "";
        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = null;
        var UOMName = "";
        var DispQty = "";
        var PendQty = "";
        var BillQty = "";
        var RecievQty = "";
        var QCRequired = "";
        var AccptQty = "";
        var RejQty = "";
        var RewrkQty = "";
        var Remarks = "";

        //SRCDocNo = currentRow.find("#MaterialDispatchNumber").val();
        //SRCDocDate = currentRow.find("#hfsourceDate").val();
        ItemID = currentRow.find("#hdItemId").val();
        ItemName = currentRow.find("#ProductName").val(); subitem
        subitem = currentRow.find("#sub_item").val();
        /*UOMID = currentRow.find("#UOMID").val();*/
        UOMID = currentRow.find("#hdUOMId").val();
        UOMName = currentRow.find("#UOM").val();
        DispQty = currentRow.find("#DispatchQuantity").val();
        PendQty = currentRow.find("#PendingQuantity").val();

        BillQty = currentRow.find("#BilledQuantity").val();
        RecievQty = currentRow.find("#ReceivedQuantity").val();
       
        if (currentRow.find("#QC_" + SNo).is(":checked"))
            QCRequired = 'Y';
        else
            QCRequired = 'N';
       
        AccptQty = currentRow.find("#AcceptedQuantity").val();
        RejQty = currentRow.find("#RejectedQuantity").val();
        RewrkQty = currentRow.find("#ReworkableQuantity").val();
        Remarks = currentRow.find("#txtItemRemarks").val();
        
        DNItemList.push({ /*SRCDocNo: SRCDocNo, SRCDocDate: SRCDocDate,*/ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UOMName: UOMName, DispQty: DispQty, PendQty: PendQty, BillQty: BillQty, RecievQty: RecievQty, QCRequired: QCRequired, AccptQty: AccptQty, RejQty: RejQty, RewrkQty: RewrkQty, Remarks: Remarks });
    });

    return DNItemList;
};
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
/*-------------Insert Section End-------------------------*/
/*-----------Start Workflow--------------*/
function ForwardBtnClick() {
    debugger;
    //var DNStatus = "";
    //DNStatus = $('#hfStatus').val().trim();
    //if (DNStatus === "D" || DNStatus === "F") {

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
        var dnDt = $("#DeliveryNoteDate").val();
        $.ajax({
            type: "POST",
            /*url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: dnDt
            },
            success: function (data) {
                /*  if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var DNStatus = "";
                    DNStatus = $('#hfStatus').val().trim();
                    if (DNStatus === "D" || DNStatus === "F") {

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
                    /*  swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
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
    var DNNo = "";
    var DNDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var WF_Status = "";

    docid = $("#DocumentMenuId").val();
    DNNo = $("#DeliveryNoteNumber").val();
    DNDate = $("#DeliveryNoteDate").val();
    $("#hdDoc_No").val(DNNo);
    WF_Status = $("#WF_Status1").val();
    Remarks = $("#fw_remarks").val();
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
        if (fwchkval != "" && DNNo != "" && DNDate != "" && level != "") {
            debugger;
             Cmn_InsertDocument_ForwardedDetail(DNNo, DNDate, docid, level, forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/DeliveryNoteSC/ToRefreshByJS";
            var list = [{ DNNo: DNNo, DNDate: DNDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/DeliveryNoteSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
    if (fwchkval === "Approve") {
        var list = [{ DNNo: DNNo, DNDate: DNDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/DeliveryNoteSC/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
    }
    //if (fwchkval === "Approve") {
    //    window.location.href = "/ApplicationLayer/DeliveryNoteSC/SIListApprove?SI_No=" + DNNo + "&SI_Date=" + DNDate + "&InvType=" + InvType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&SaleVouMsg=" + SaleVouMsg;

    //}
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DNNo != "" && DNDate != "") {
             Cmn_InsertDocument_ForwardedDetail(DNNo, DNDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            var list = [{ DNNo: DNNo, DNDate: DNDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/DeliveryNoteSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DNNo != "" && DNDate != "") {
             Cmn_InsertDocument_ForwardedDetail(DNNo, DNDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            //window.location.href = "/ApplicationLayer/DeliveryNoteSC/ToRefreshByJS";
            var list = [{ DNNo: DNNo, DNDate: DNDate }];
            var FrwdDtList = JSON.stringify(list);
            window.location.href = "/ApplicationLayer/DeliveryNoteSC/ToRefreshByJS?FrwdDtList=" + FrwdDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_Status=" + WF_Status;

        }
    }
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#DeliveryNoteNumber").val();
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


/*---------- Material Retrun Item work Start----------*/

function BindDLLReturnItemList(RowNo) {
    debugger;
    
    var ItmDDLName = "#ItemListName";
    
    var TableID = "#ReturnItemDetailsTbl";
    var SnoHiddenField = "#SNohiddenfiled";
    BindRtrnItemList(ItmDDLName, RowNo, TableID, SnoHiddenField, "", "DNSC");

}
//function BindDLLReturnItemList() {
//    debugger;

//    var ItmDDLName = "#ItemListName";

//    var TableID = "#ReturnItemDetailsTbl";
//    var SnoHiddenField = "#SNohiddenfiled";
//    BindRtrnItemList(ItmDDLName, "1", TableID, SnoHiddenField, "", "DNSC");

//}


function BindRtrnItemList(ItmDDLName, RowID, TableID, SnoHiddenField, OtherFx, PageName) {
    debugger;
    var JobOrdTyp = $("#DNSChdn_JobOrdTyp").val();
    var ddl_HedrItemId = $("#Hdn_FinishProductid").val();
    if (JobOrdTyp == "D") {
        if ($(ItmDDLName + RowID).val() == "0" || $(ItmDDLName + RowID).val() == "" || $(ItmDDLName + RowID).val() == null) {
            $(ItmDDLName + RowID + " option[value=" + ddl_HedrItemId + "]").select2().hide();
            $(ItmDDLName + RowID).append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
            $('#Textddl' + RowID).append(`<option data-uom="0" value="0">---Select---</option>`);
        }
    }
    else {
        if ($(ItmDDLName + RowID).val() == "0" || $(ItmDDLName + RowID).val() == "" || $(ItmDDLName + RowID).val() == null) {
            $(ItmDDLName + RowID).append(`<optgroup class='def-cursor' id="TextddlItm${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}' data-itmtyp='${$("#ItemType").text()}'></optgroup>`);
            $('#TextddlItm' + RowID).append(`<option data-uom="0" data-itmtyp="0" value="0"> ---Select---</option>`);
        }
    }
    if ($(TableID + " tbody tr").length > 0) {
        $(TableID + " tbody tr").each(function () {
            var currentRow = $(this);
            //debugger;
            var rowno = "";
            if (RowID != null && RowID != "") {
                rowno = currentRow.find(SnoHiddenField).val();
            }
            DynamicSerchableRtrnItemDDL(TableID, ItmDDLName, rowno, SnoHiddenField, OtherFx, PageName)

        });

    }
    else {
        DynamicSerchableRtrnItemDDL(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName)
    }
   

}
function DynamicSerchableRtrnItemDDL(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName) {
    debugger;
    var JobOrdTyp = $("#DNSChdn_JobOrdTyp").val();
    var ddl_HedrItemId = $("#Hdn_FinishProductid").val();
    if (JobOrdTyp == "D") {
        var JONO = $("#Hdn_JobNum").val();
        
        
            
       var firstEmptySelect = true;

        $(ItmDDLName + RowID).select2({

            ajax: {
                url: '/ApplicationLayer/DeliveryNoteSC/BindByProdctScrapItm_AgainstDircetJO',
                data: function (params) {
                    var queryParameters = {
                        SearchName: params.term,
                        PageName: PageName,
                        page: params.page || 1
                    };
                    return queryParameters;
                },
                multiple: true,
                cache: true,
                processResults: function (data, params) {
                    //debugger
                    var pageSize,
                        pageSize = 20;//50000; // or whatever pagesize
                    //results = [];
                    debugger;
                    ConvertIntoRtnArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                    var ItemListArrey = [];
                    if (sessionStorage.getItem("selecteditemlist").length != null) {
                        ItemListArrey = sessionStorage.getItem("selecteditemlist");
                    }
                    var JOInputItmDtl = ConvertIntoRtnArreyListJOInputItmDtl();
                    //sessionStorage.removeItem("selecteditemlist");
                    debugger;
                    let selected = [];
                    selected.push({ id: $(ItmDDLName + RowID).val() });
                    selected = JSON.stringify(selected);

                    var NewArrey = JSON.parse(ItemListArrey).filter(i => !selected.includes(i.id));
                    NewArrey.push({ id: ddl_HedrItemId }) /*For Filter Header Item in this Item List */
                    NewArrey.push({ id: JOInputItmDtl })/*For Filter JobOrderInputItmTbl's Item in this Item List */
                    //NewArrey.push(NewArr.findIndex(v => v.id : JOInpuItemid) > -1)/* For Filter JonOrdInputItemDetail*/
                    data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));
                    //$(ItmDDLName + RowID + " option[value=" + ddl_HedrItemId + "]").select2().hide();
                    if ($(".select2-search__field").parent().find("ul").length == 0) {
                        $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
</strong></li></ul>`)
                    }
                    var page = params.page || 1;
                    // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                    debugger;
                    Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                    if (page == 1) {
                        debugger;
                        if (Fdata[0] != null) {
                            if (Fdata[0].Name.trim() != "---Select---") {
                                var select = { ID: "0_0", Name: "---Select---" };
                                Fdata.unshift(select);
                                //Fdata.unshift("");
                            }
                        }
                    }

                    return {
                        results: $.map(Fdata, function (val, Item) {
                            return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                        }),
                        //results: data.slice((page - 1) * pageSize, page * pageSize),
                        //more: data.length >= page * pageSize,
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
                    '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                    '</div>'
                );
                return $result;
                firstEmptySelect = false;
            },
          
        });
    }
    else {
        var JONO = $("#Hdn_JobNum").val();
        var firstEmptySelect = true;

        $(ItmDDLName + RowID).select2({

            ajax: {
                url: '/ApplicationLayer/DeliveryNoteSC/GetReturnItemDDLList',
                data: function (params) {
                    var queryParameters = {
                        JONO: JONO,
                        SearchName: params.term,
                        PageName: PageName,
                        page: params.page || 1
                    };
                    return queryParameters;
                },
                multiple: true,
                cache: true,
                processResults: function (data, params) {
                    //debugger
                    var pageSize,
                        pageSize = 20;//50000; // or whatever pagesize
                    //results = [];
                    debugger;
                    ConvertIntoRtnArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                    var ItemListArrey = [];
                    if (sessionStorage.getItem("selecteditemlist").length != null) {
                        ItemListArrey = sessionStorage.getItem("selecteditemlist");
                    }

                    //sessionStorage.removeItem("selecteditemlist");
                    debugger;
                    let selected = [];
                    selected.push({ id: $(ItmDDLName + RowID).val() });
                    selected = JSON.stringify(selected);

                    var NewArrey = JSON.parse(ItemListArrey).filter(i => !selected.includes(i.id));


                    data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split(",")[0]));

                    if ($(".select2-search__field").parent().find("ul").length == 0) {
                        $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row">
<div class="col-md-4 col-xs-4 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-4 col-xs-4 def-cursor">${$("#ItemUOM").text()}</div>
<div class="col-md-4 col-xs-4 def-cursor">${$("#ItemType").text()}</div>
</div>

</strong></li></ul>`)
                    }
                    var page = params.page || 1;
                    // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                    debugger;
                    Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                    if (page == 1) {
                        debugger;
                        if (Fdata[0] != null) {
                            if (Fdata[0].Name.trim() != "---Select---") {
                                var select = { ID: "0,,,,,", Name: "---Select---" };
                                Fdata.unshift(select);
                                //Fdata.unshift("");
                            }
                        }
                    }

                    return {
                        results: $.map(Fdata, function (val, Item) {
                            return { id: val.ID.split(",")[0] + "," + val.ID.split(",")[1] + "," + val.ID.split(",")[2] + "," + val.ID.split(",")[3] + "," + val.ID.split(",")[4] + "," + val.ID.split(",")[5], text: val.Name, UOM: val.ID.split(",")[1], UomId: val.ID.split(",")[2], ItmTypId: val.ID.split(",")[3], ItmTyp: val.ID.split(",")[4], IssueQty: val.ID.split(",")[5] };
                        }),
                        //results: data.slice((page - 1) * pageSize, page * pageSize),
                        //more: data.length >= page * pageSize,
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
                    '<div class="col-md-4 col-xs-4' + classAttr + '">' + data.text + '</div>' +
                    '<div class="col-md-4 col-xs-4' + classAttr + '">' + data.UOM + '</div>' +
                    '<div class="col-md-4 col-xs-4' + classAttr + '">' + data.ItmTyp + '</div>' +

                    '</div>'
                );
                return $result;
                firstEmptySelect = false;
            },

        });

    }


}
function ConvertIntoRtnArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField,) {
    debugger;
    let array = [];
    $(TableID + " tbody tr").each(function () {
        var currentRow = $(this);
        //debugger;
        var rowno = currentRow.find(SnoHiddenField).val();
        var itemId = "";
        if (OtherFx == "listToHide") {
            itemId = currentRow.find(SnoHiddenField).val();
        }
        else {
            itemId = currentRow.find(ItmDDLName + rowno).val();
        }
        if (itemId != "0" /*&& itemId != $(ItmDDLName + RowID).val()*/) {
            array.push({ id: itemId });
        }
    });

    sessionStorage.removeItem("selecteditemlist");
    sessionStorage.setItem("selecteditemlist", JSON.stringify(array));
    //return array;
}
function ConvertIntoRtnArreyListJOInputItmDtl() {
    debugger;
    let JoInputarray = [];
    var InputItmLen = $("#hdn_JobOrd_InputItemDetailTbl tbody tr").length;
    if (InputItmLen > 0) {
        $("#hdn_JobOrd_InputItemDetailTbl tbody tr").each(function () {
           var row = $(this);
           var JOInpuItemid = row.find("#JOInputItemId").val();
            if (JOInpuItemid != "0") {
                JoInputarray.push({ id: JOInpuItemid });
            }
        });
    }
    return JoInputarray;
}

function OnchangeRetrnItem(RowID, e) {
    debugger;
    BindReturnItemList(e);
}
function BindReturnItemList(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    Item_ID = clickedrow.find("#ItemListName" + SNo).val();
    var ItmIdBr = Item_ID.split(',');

    var Itm_ID = ItmIdBr[0];
    var ItmTypeId = ItmIdBr[3];
    var ItmType = ItmIdBr[4];
    //var IssueQty = ItmIdBr[5];
    
    clickedrow.find("#hfItemID").val(Itm_ID);
    debugger;
    Cmn_BindUOM(clickedrow, Itm_ID, "", "", "dnsc");
    if (Itm_ID == "0" || Item_ID == "0,,,,,") {
        
        clickedrow.find("#ItemListNameError").text($("#valueReq").text());
        clickedrow.find("#ItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + SNo + "-container']").css("border-color", "red");
        clickedrow.find("#MaterialTypeID").val("");

        clickedrow.find("#MaterialType").val("");
        clickedrow.find("#IssuedQty").val("");
    }
    else {
        clickedrow.find("#ItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-ItemListName" + SNo + "-container']").css("border-color", "#ced4da");
        clickedrow.find("#MaterialTypeID").val(ItmTypeId);

        clickedrow.find("#MaterialType").val(ItmType);
        //var IssuedQuantity = parseFloat(parseFloat(IssueQty)).toFixed(parseFloat(QtyDecDigit));

        //clickedrow.find("#IssuedQty").val(IssuedQuantity);
    }
    debugger;
   

    ClearRowDetails(e, Itm_ID);
}
function ClearRowDetails(e, Itm_ID) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#ItmInfoBtnIcon").val("");
    clickedrow.find("#UOM").val("");
    clickedrow.find("#ReceiveQty").val("");
}
function AddNewRow() {
    debugger;
   var rowIdx = 0;

    var ItemInfo = $("#ItmInfo").text();
    var Span_Delete_Title = $("#Span_Delete_Title").text();
    debugger;

    var rowCount = $('#ReturnItemDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    rowIdx = rowCount-1
    var levels = [];
    $("#ReturnItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        RowNo = parseInt(currentRow.find("#SNohiddenfiled").val());// + 1;
        levels.push(RowNo);
    });
    if (levels.length > 0) {
        RowNo = Math.max(...levels);
    }
    RowNo = RowNo + 1;
    if (RowNo == "0") {
        RowNo = 1;
    }
    var JobOrdTyp = $("#DNSChdn_JobOrdTyp").val();
    if (JobOrdTyp == "D") {
        debugger;
        $('#ReturnItemDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${Span_Delete_Title}"></i></td>
<td id="SRNO" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"><input  type="hidden" id="hfItemID" /></td>
<td>
<div class="col-sm-11 lpo_form no-padding">
<select class="form-control" id="ItemListName${RowNo}" name="ItemListName" onchange ="OnchangeRetrnItem(${RowNo},event)"></select>
<span id="ItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator ItmInfoBtnIcon" onclick="OnClickReturnItemIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button>
</div>
</td>
<td><input id="UOM" class="form-control date" autocomplete="off" type="text" name="UOM"  placeholder="${$("#ItemUOM").text()}" disabled>
<input id="UOMID" type="hidden" />
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="ReceiveQty" class="form-control num_right" onpaste="return CopyPasteData(event)" onkeypress="return OnKeyPressRetrnQty(this,event);" onchange="OnChangeReturnQuantity(this,event)" autocomplete="off" type="text" name="ReceiveQty" placeholder="0000.00">
<span id="ReceiveQtyError" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_BYPSSubItemRecvQty" >
<input hidden type="text" id="BYPSsub_item" value="" />
<button type="button" id="BYPSSubItemRecvQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('BYPSRecvQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>

</tr>`);
    }
    else {
        $('#ReturnItemDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${RowNo}" title="${Span_Delete_Title}"></i></td>
<td id="SRNO" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input  type="hidden" id="SNohiddenfiled" value="${RowNo}" /></td>
<td style="display: none;"><input  type="hidden" id="hfItemID" /></td>
<td>
<div class="col-sm-11 lpo_form no-padding">
<select class="form-control" id="ItemListName${RowNo}" name="ItemListName" onchange ="OnchangeRetrnItem(${RowNo},event)"></select>
<span id="ItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-1 i_Icon">
<button type="button" id="ItmInfoBtnIcon" class="calculator ItmInfoBtnIcon" onclick="OnClickReturnItemIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button>
</div>
</td>
<td><input id="UOM" class="form-control date" autocomplete="off" type="text" name="UOM"  placeholder="${$("#ItemUOM").text()}" disabled>
<input id="UOMID" type="hidden" />
</td>
<td>
<input id="MaterialType" class="form-control" autocomplete="" type="text" name="MaterialType" placeholder="${$("#span_MaterialType").text()}" disabled>
<input id="MaterialTypeID" type="hidden" />
</td>
<td>
<div class="col-sm-10 lpo_form no-padding">
<input id="ReceiveQty" class="form-control num_right" onpaste="return CopyPasteData(event)" onkeypress="return OnKeyPressRetrnQty(this,event);" onchange="OnChangeReturnQuantity(this,event)" autocomplete="off" type="text" name="ReceiveQty" placeholder="0000.00">
<span id="ReceiveQtyError" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_BYPSSubItemRecvQty" >
<input hidden type="text" id="BYPSsub_item" value="" />
<button type="button" id="BYPSSubItemRecvQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('BYPSRecvQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>

</tr>`);
    }
    BindDLLReturnItemList(RowNo)
    
    
};
function updateReturnItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#ReturnItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SRNO").text(SerialNo);

    });
};
function InsertDNItemReturnDetails() {
    debugger;
    var JobOrdTyp = $("#DNSChdn_JobOrdTyp").val();
   
   
    var DNItemreturnList = [];
    $("#ReturnItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;

        var currentRow = $(this);
        var RowSNo = currentRow.find("#SNohiddenfiled").val();
       
        var ItemID = "";
        var ItemName1 = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var UOMName = "";
        var ItmTypId = "";
        var ItmTypName = "";
        var ReceiveQty = "";
        //var IssuedQty = "";
        

        
        ItemID = currentRow.find("#hfItemID").val(); 
        var status = $("#hfStatus").val()
        var command = $("#hdn_command").val()
        if (status == "D" && command == "Edit") {
            ItemName = currentRow.find("#ItemListName" + RowSNo).text().trim();
        }
        else {
            ItemName1 = currentRow.find("#ItemListName" + RowSNo).text().trim();
            var itm = ItemName1.split('-');
            ItemName = itm[6].trim();
        }
        subitem = currentRow.find("#BYPSsub_item").val();
        UOMID = currentRow.find("#UOMID").val();
        UOMName = currentRow.find("#UOM").val();
        if (JobOrdTyp == "D") {
            ItmTypId = 0;
            ItmTypName = "";
        }
        else {
            ItmTypId = currentRow.find("#MaterialTypeID").val().trim();
            ItmTypName = currentRow.find("#MaterialType").val();
        }
        
        //IssuedQty = currentRow.find("#IssuedQty").val();

        ReceiveQty = currentRow.find("#ReceiveQty").val();

       DNItemreturnList.push({ ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID, UOMName: UOMName, ItmTypId: ItmTypId, ItmTypName: ItmTypName, ReceiveQty: ReceiveQty });
    });

    return DNItemreturnList;
};
function OnClickReturnItemIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    var ItmName = "";

    ItmCode = clickedrow.find("#hfItemID").val();
    ItemInfoBtnClick(ItmCode)

}
function ReturnItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#ReturnItemDetailsTbl >tbody >tr").length > 0) {
        $("#ReturnItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
              var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#ItemListName" + Sno).val() == "0" || currentRow.find("#ItemListName" + Sno).val() =="0,,,,," ) {
                currentRow.find("#ItemListNameError").text($("#valueReq").text());
                currentRow.find("#ItemListNameError").css("display", "block");
                debugger;

                currentRow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border", "1px solid red");

                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ItemListNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
           // var IssueQty = currentRow.find("#IssuedQty").val();
            debugger;
            var ReceivQty = currentRow.find("#ReceiveQty").val();
            var ReceivedQuantity = parseFloat(parseFloat(ReceivQty)).toFixed(parseFloat(QtyDecDigit));
            
            if (ReceivQty == "" || ReceivQty == "0.000") {
                currentRow.find("#ReceiveQtyError").text($("#valueReq").text());
                currentRow.find("#ReceiveQtyError").css("display", "block");
                currentRow.find("#ReceiveQty").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ReceiveQtyError").css("display", "none");
                currentRow.find("#ReceiveQty").css("border-color", "#ced4da");
            }
            //var ReturnQuantity = parseFloat(parseFloat(RetrnQty)).toFixed(parseFloat(QtyDecDigit));
            //var IssueQuantity = parseFloat(parseFloat(IssueQty)).toFixed(parseFloat(QtyDecDigit));
            //if (RetrnQty == "" || RetrnQty=="0.000") {
            //    currentRow.find("#ReturnQtyError").text($("#valueReq").text());
            //    currentRow.find("#ReturnQtyError").css("display", "block");
            //    currentRow.find("#ReturnQty").css("border-color", "red");
            //    ErrorFlag = "Y";
            //}
            //else {
            //    var ItemType = currentRow.find("#MaterialTypeID").val();
            //    if (ItemType == "IR") {
            //        if (parseFloat(ReturnQuantity) > parseFloat(IssueQuantity)) {
            //            currentRow.find("#ReturnQtyError").text($("#ExceedingQty").text());
            //            currentRow.find("#ReturnQtyError").css("display", "block");
            //            currentRow.find("#ReturnQty").css("border-color", "red");
            //            ErrorFlag = "Y";
            //        }
            //        else {
            //            currentRow.find("#ReturnQtyError").css("display", "none");
            //            currentRow.find("#ReturnQty").css("border-color", "#ced4da");
            //        }
            //    }
                
            //}
            
           
            
            
            
        })
    }
    
    debugger;
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
       
        return true;
    }
}
function OnChangeReturnQuantity(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity

    var clickedrow = $(evt.target).closest("tr");
    //var ItemType = clickedrow.find("#MaterialTypeID").val();
    var ReturnQuantity = clickedrow.find("#ReceiveQty").val();
    if ((ReturnQuantity == "") || ReturnQuantity == parseFloat(0) || (ReturnQuantity == null)) {
        clickedrow.find("#ReceiveQtyError").text($("#valueReq").text());
        clickedrow.find("#ReceiveQtyError").css("display", "block");
        clickedrow.find("#ReceiveQty").css("border-color", "red");
        clickedrow.find("#ReceiveQty").val("");
        return false;
    }
    else {
        clickedrow.find("#ReceiveQtyError").css("display", "none");
        clickedrow.find("#ReceiveQty").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(ReturnQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#ReceiveQty").val(mi_issueqty);
    }
 }

function OnKeyPressRetrnQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#ReceiveQtyError").css("display", "none");
    clickedrow.find("#ReceiveQty").css("border-color", "#ced4da");
    return true;
}

//------------------------Dispatch Quantity Details-----------------------//
function OnClickItemDispatchQtyIconBtn(e) {
    debugger;
    BindDispatchQtyDetails();
    // var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var DnNo = $("#DeliveryNoteNumber").val()
   
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    var ItmName = clickedrow.find("#ProductName").val();
    var UOM = clickedrow.find("#UOM").val();
    var UOMid = clickedrow.find("#hdUOMId").val(); 
    var HdnMessage = $("#Hdn_Message").val();
    //var OrderQty = clickedrow.find("#DispatchQuantity").val();
    //var PendingQty = clickedrow.find("#PendingQuantity").val();
    //var BillQty = clickedrow.find("#PendingQuantity").val();
    //var ReceiveQty = clickedrow.find("#PendingQuantity").val();
    var SelectedItemdetail = $("#hdDispatchQtyDetails").val();
    debugger;

    var DN_Status = $("#hfStatus").val().trim(); 
    var DN_Command = $("#hdn_command").val().trim()
    if (ItmCode != "" || ItmCode != null) {
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/DeliveryNoteSC/getItemDispatchQuantityDetail",
            data: { ItemID: ItmCode, Status: DN_Status, SelectedItemdetail: SelectedItemdetail, DnNo: DnNo, DN_Command: DN_Command, HdnMessage: HdnMessage },
            success: function (data) {
                debugger;
                $('#ItemDispatchQuantityDetail').html(data);

                $("#DispQtyItemName").val(ItmName);
                $("#hd_DispQtyItemId").val(ItmCode);
                $("#DispQtyUOM").val(UOM);
                $("#hd_DispQtyUOMId").val(UOMid);
                

                TotalDispatchQtyInDispDetail();
            },
        });
    }

    
}
function BindDispatchQtyDetails() {
    debugger;
    var FDocNoWiseItemQtyDetails = [];
    var DocNoWiseItemQtyDetails = JSON.parse(sessionStorage.getItem("ItemQtyDocNoWiseDetails"));
    $("#SaveItemDispatchQtyDetails TBODY TR").each(function () {
        var row = $(this);
        debugger;
        var SrNo = row.find("#hdSNoRowID").val();
        var DocNo = row.find("#PD_DispDocNo").val();
        var DocDate = row.find("#PD_DispDocDate").val();
        var ItemID = row.find("#PD_DispQtyItemID").val();
        var sub_item = row.find("#sub_item").val();
        var UomID = row.find("#PD_DispQtyUomID").val();
        var DispatchQty = row.find("#PD_DispQty").val();
        var PendingQty = row.find("#PD_PendQty").val();
        var BilledQty = row.find("#PD_BillQty").val();
        if (BilledQty == "NaN") {
            BilledQty = "";
        }
        var ReceiveQty = row.find("#PD_ReceivQty").val();
        if (ReceiveQty == "NaN") {
            ReceiveQty = "";
        }
        if (DocNoWiseItemQtyDetails != null) {
            if (DocNoWiseItemQtyDetails.length > 0) {
                for (j = 0; j < DocNoWiseItemQtyDetails.length; j++) {
                    var MDDocNo = DocNoWiseItemQtyDetails[j].DocNo;
                    var MDItemID = DocNoWiseItemQtyDetails[j].ItemID;
                    debugger;
                    if (MDDocNo != DocNo && MDItemID != ItemID) {
                        FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, ItemID: ItemID, sub_item: sub_item, UomID: UomID, DispatchQty: DispatchQty, PendingQty: PendingQty, BilledQty: BilledQty, ReceiveQty: ReceiveQty })
                    }
                }
            }
            else {
                FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, ItemID: ItemID, sub_item: sub_item, UomID: UomID, DispatchQty: DispatchQty, PendingQty: PendingQty, BilledQty: BilledQty, ReceiveQty: ReceiveQty })
            }
        }
        else {
            FDocNoWiseItemQtyDetails.push({ DocNo: DocNo, DocDate: DocDate, ItemID: ItemID, sub_item: sub_item, UomID: UomID, DispatchQty: DispatchQty, PendingQty: PendingQty, BilledQty: BilledQty, ReceiveQty: ReceiveQty})
        }
    });
    debugger;
    if (FDocNoWiseItemQtyDetails != null) {
        if (FDocNoWiseItemQtyDetails.length > 0) {
            debugger;
            //sessionStorage.setItem("ItemQtyDocNoWiseDetails", JSON.stringify(FDocNoWiseItemQtyDetails));
            var str1 = JSON.stringify(FDocNoWiseItemQtyDetails);
            $("#hdDispatchQtyDetails").val(str1);
        }
    }
}
function TotalDispatchQtyInDispDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalDispQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalPenQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalBillQty = parseFloat(0).toFixed(QtyDecDigit);
    var TotalReceivQty = parseFloat(0).toFixed(QtyDecDigit);
    $("#DispatchQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        debugger;
        var DispatchQty = CurrentRow.find("#Disp_DispatchedQuantity").val();
        var PendQty = CurrentRow.find("#Disp_PendingQuantity").val();
        var BillQty = CurrentRow.find("#Disp_BilledQuantity").val();
        var ReceiveQty = CurrentRow.find("#Disp_ReceivedQuantity").val();
        if (DispatchQty != null && DispatchQty != "") {
            TotalDispQty = parseFloat(TotalDispQty) + parseFloat(DispatchQty);
        }
        if (PendQty != null && PendQty != "") {
            TotalPenQty = parseFloat(TotalPenQty) + parseFloat(PendQty);
        }
        if (BillQty != null && BillQty != "") {
            TotalBillQty = parseFloat(TotalBillQty) + parseFloat(BillQty);
        }
        if (ReceiveQty != null && ReceiveQty != "") {
            TotalReceivQty = parseFloat(TotalReceivQty) + parseFloat(ReceiveQty);
        }
    });
    $("#LblTotalDispatchQty").text(parseFloat(TotalDispQty).toFixed(QtyDecDigit));
    $("#LblTotalPendingQty").text(parseFloat(TotalPenQty).toFixed(QtyDecDigit));
    $("#LblTotalBilledQty").text(parseFloat(TotalBillQty).toFixed(QtyDecDigit));
    $("#LblTotalReceivedQty").text(parseFloat(TotalReceivQty).toFixed(QtyDecDigit));
    
}
function OnChangeBLQty(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity

    var clickedrow = $(evt.target).closest("tr");
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(evt.target).closest("tr");
    var BillQty = clickedrow.find("#Disp_BilledQuantity").val();
    if (BillQty == "0" || BillQty == parseFloat(0)) {
        BillQty = "";
    }
    var PendingQty = parseFloat(0).toFixed(QtyDecDigit);
    var BilledQty = parseFloat(0).toFixed(QtyDecDigit);

    PendingQty = parseFloat(clickedrow.find("#Disp_PendingQuantity").val()).toFixed(QtyDecDigit);
    BilledQty = parseFloat(clickedrow.find("#Disp_BilledQuantity").val()).toFixed(QtyDecDigit);
    if (AvoidDot(BilledQty) == false) {
        clickedrow.find("#Disp_BilledQuantity").val("");
        //BilledQty = 0;
    }

    if (BillQty != "") {
       
        clickedrow.find("#Disp_ReceivedQuantity").val(BilledQty);
        clickedrow.find("#Disp_ReceivedQuantity_Error").css("display", "none");
        clickedrow.find("#Disp_ReceivedQuantity").css("border-color", "#ced4da");
        if (parseFloat(CheckNullNumber(BilledQty)) < parseFloat(CheckNullNumber(PendingQty)) || parseFloat(CheckNullNumber(BilledQty)) == parseFloat(CheckNullNumber(PendingQty))) {
            parseFloat(clickedrow.find("#Disp_BilledQuantity").val(BilledQty)).toFixed(QtyDecDigit);
            clickedrow.find("#Disp_BilledQuantity_Error").css("display", "none");
            clickedrow.find("#Disp_BilledQuantity").css("border-color", "#ced4da");
        }
        else {
            clickedrow.find("#Disp_BilledQuantity_Error").text($("#ExceedingPendingQuantity").text());
            clickedrow.find("#Disp_BilledQuantity_Error").css("display", "block");
            clickedrow.find("#Disp_BilledQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }

    }
    else {
        clickedrow.find("#Disp_BilledQuantity_Error").text($("#valueReq").text());
        clickedrow.find("#Disp_BilledQuantity_Error").css("display", "block");
        clickedrow.find("#Disp_BilledQuantity").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
    TotalDispatchQtyInDispDetail();
}
function OnKeyPressRecQty(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    clickedrow.find("#Disp_ReceivedQuantity_Error").css("display", "none");
    clickedrow.find("#Disp_ReceivedQuantity").css("border-color", "#ced4da");
    return true;
}
function OnChangeRecQty(el, evt) {
    debugger;
    var ErrorFlag = "N";
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var clickedrow = $(evt.target).closest("tr");
    var RecevQty = clickedrow.find("#Disp_ReceivedQuantity").val();
    if (RecevQty == "0" || RecevQty == parseFloat(0)) {
        RecevQty = "";
    }
    var PendingQty = parseFloat(0).toFixed(QtyDecDigit);
    var ReceivedQty = parseFloat(0).toFixed(QtyDecDigit);

    PendingQty = parseFloat(clickedrow.find("#Disp_PendingQuantity").val()).toFixed(QtyDecDigit);
    ReceivedQty = parseFloat(clickedrow.find("#Disp_ReceivedQuantity").val()).toFixed(QtyDecDigit);
    if (AvoidDot(ReceivedQty) == false) {
        clickedrow.find("#Disp_ReceivedQuantity").val("");
        //ReceivedQty = 0;
    }
   
    if (RecevQty != "") {

        if (parseFloat(CheckNullNumber(ReceivedQty)) < parseFloat(CheckNullNumber(PendingQty)) || parseFloat(CheckNullNumber(ReceivedQty)) == parseFloat(CheckNullNumber(PendingQty))) {
            parseFloat(clickedrow.find("#Disp_ReceivedQuantity").val(ReceivedQty)).toFixed(QtyDecDigit);
            clickedrow.find("#Disp_ReceivedQuantity_Error").css("display", "none");
            clickedrow.find("#Disp_ReceivedQuantity").css("border-color", "#ced4da");
        }
        else {
            clickedrow.find("#Disp_ReceivedQuantity_Error").text($("#ExceedingPendingQuantity").text());
            clickedrow.find("#Disp_ReceivedQuantity_Error").css("display", "block");
            clickedrow.find("#Disp_ReceivedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }

    }
    else {
        clickedrow.find("#Disp_ReceivedQuantity_Error").text($("#valueReq").text());
        clickedrow.find("#Disp_ReceivedQuantity_Error").css("display", "block");
        clickedrow.find("#Disp_ReceivedQuantity").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
TotalDispatchQtyInDispDetail();
}
function CheckQuantityValidation() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var status = "N";
    $("#DispatchQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        var PendingQuantity = CurrentRow.find("#Disp_PendingQuantity").val();
        
        var BilledQuantity = CurrentRow.find("#Disp_BilledQuantity").val();
        if ((BilledQuantity == "") || (BilledQuantity == null) || (BilledQuantity == "0.000")) {
            CurrentRow.find("#Disp_BilledQuantity_Error").text($("#valueReq").text());
            CurrentRow.find("#Disp_BilledQuantity_Error").css("display", "block");
            CurrentRow.find("#Disp_BilledQuantity").css("border-color", "red");
            status="Y"
        }
        else {
            if (parseFloat(BilledQuantity) > parseFloat(PendingQuantity)) {
                CurrentRow.find("#Disp_BilledQuantity_Error").text($("#ExceedingPendingQuantity").text());
                CurrentRow.find("#Disp_BilledQuantity_Error").css("display", "block");
                CurrentRow.find("#Disp_BilledQuantity").css("border-color", "red");
                status = "Y"
            }
            else {
                CurrentRow.find("#Disp_BilledQuantity_Error").css("display", "none");
                CurrentRow.find("#Disp_BilledQuantity").css("border-color", "#ced4da");
                var test = parseFloat(parseFloat(BilledQuantity)).toFixed(parseFloat(QtyDecDigit));
                CurrentRow.find("#Disp_BilledQuantity").val(test);
            }
        }
        var ReceivedQuantity = CurrentRow.find("#Disp_ReceivedQuantity").val();
      
        if ((ReceivedQuantity == "") || (ReceivedQuantity == null) || (ReceivedQuantity == "0.000")) {

            CurrentRow.find("#Disp_ReceivedQuantity_Error").text($("#valueReq").text());
            CurrentRow.find("#Disp_ReceivedQuantity_Error").css("display", "block");
            CurrentRow.find("#Disp_ReceivedQuantity").css("border-color", "red");
            status = "Y"
        }
        else {
            if (parseFloat(ReceivedQuantity) > parseFloat(PendingQuantity)) {
                CurrentRow.find("#Disp_ReceivedQuantity_Error").text($("#ExceedingPendingQuantity").text());
                CurrentRow.find("#Disp_ReceivedQuantity_Error").css("display", "block");
                CurrentRow.find("#Disp_ReceivedQuantity").css("border-color", "red");
                status = "Y"
            }
            else {

                CurrentRow.find("#Disp_ReceivedQuantity_Error").css("display", "none");
                CurrentRow.find("#Disp_ReceivedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(parseFloat(ReceivedQuantity)).toFixed(parseFloat(QtyDecDigit));
                CurrentRow.find("#Disp_ReceivedQuantity").val(test);
            }
        }
        
    });
    if (status === "Y") {
        return false;
    }
    else {
        return true;
    }
}
function onclickbtnDispQtySaveAndExit() {
    debugger
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    

    if (CheckQuantityValidation() === true) {
        if (CheckValidations_forSubItems() == true) {

            TotalDispatchQtyInDispDetail();

            var ItemPendingQty = $("#ItemPendingQuantity").val();
            var ItemOrder_PackedQty = $("#Order_PackedQty").val();
            var ItemTotalPackedQuantity = $("#LblTotalOrderQty").text();

            //if (parseFloat(ItemTotalPackedQuantity) > parseFloat(ItemPendingQty)) {
            //    swal("", $("#PackedQtycannotexceedtoPendingQty").text(), "warning");
            //}
            //else {
            debugger;
            var SelectedItem = $("#hd_DispQtyItemId").val();
            $("#hd_DispQtyItemId").val(SelectedItem);
            var ItemUomId = $("#hd_DispQtyUOMId").val();
            $("#SaveItemDispatchQtyDetails TBODY TR").each(function () {
                var row = $(this);
                var rowitem = row.find("#PD_DispQtyItemID").val();
                if (rowitem == SelectedItem) {
                    debugger;
                    $(this).remove();
                }
            });
            $("#DispatchQtyInfoTbl tbody tr").each(function () {
                var CurrentRow = $(this);
                var sub_item = CurrentRow.find("#sub_item").val();
                var BillQty = CurrentRow.find("#Disp_BilledQuantity").val();
                var RecQty = CurrentRow.find("#Disp_ReceivedQuantity").val();
                if ((BillQty != null && BillQty != "" && BillQty != "0.000") && (RecQty != null && RecQty != "" && RecQty != "0.000")) {
                    var QtyDocNo = CurrentRow.find("#DispatchQtyDocNo").val();
                    var QtyDocDate = CurrentRow.find("#DispatchQtyDocDate").val();
                    var QtyDispQty = CurrentRow.find("#Disp_DispatchedQuantity").val();
                    var PendingQty = CurrentRow.find("#Disp_PendingQuantity").val();

                    $('#SaveItemDispatchQtyDetails tbody').append(`<tr>
                    <td><input type="text" id="PD_DispQtyItemID" value="${SelectedItem}" />
                    <input hidden type="text" id="sub_item" value="${sub_item}" />
                    </td>
                    <td><input type="text" id="PD_DispQtyUomID" value="${ItemUomId}" /></td>
                    <td><input type="text" id="PD_DispDocNo" value="${QtyDocNo}" /></td>
                    <td><input type="text" id="PD_DispDocDate" value="${QtyDocDate}" /></td>
                    <td><input type="text" id="PD_DispQty" value="${QtyDispQty}" /></td>
                    <td><input type="text" id="PD_PendQty" value="${PendingQty}" /></td>
                    <td>
                            <div class="lpo_form">
                                <input type="text" id="PD_BillQty" value="${BillQty}" />
                                <span id="Disp_BilledQuantity_Error" class="error-message is-visible"></span>
                            </div>
                        </td>
                        <td>
                            <div class="lpo_form">
                                <input type="text" id="PD_ReceivQty" value="${RecQty}" />
                                <span id="Disp_ReceivedQuantity_Error" class="error-message is-visible"></span>
                            </div>
                        </td>
                </tr>`
                    );
                }
            });
            $("#DispatchDetail").modal('hide');



            $("#DnSCItmDetailsTbl >tbody >tr").each(function () {
                debugger;
                var clickedrow = $(this);
                var ItemId = clickedrow.find("#hdItemId").val();

                if (ItemId == SelectedItem) {
                    clickedrow.find("#BilledQuantity").val(parseFloat($("#LblTotalBilledQty").text()).toFixed(QtyDecDigit));
                    clickedrow.find("#BilledQuantity_Error").css("display", "none");
                    clickedrow.find("#BilledQuantity").css("border-color", "#ced4da");
                    clickedrow.find("#ReceivedQuantity").val(parseFloat($("#LblTotalReceivedQty").text()).toFixed(QtyDecDigit));
                    clickedrow.find("#ReceivedQuantity_Error").css("display", "none");
                    clickedrow.find("#ReceivedQuantity").css("border-color", "#ced4da");
                }
            });

        }
    }
    else {
        return;
    }
}
function onclickbtnDispQtyReset() {
    $("#DispatchQtyInfoTbl tbody tr").each(function () {
        var CurrentRow = $(this);
        CurrentRow.find("#Disp_BilledQuantity").val("");
        CurrentRow.find("#Disp_ReceivedQuantity").val("");
    });
    $("#LblTotalBilledQty").text("");
    $("#LblTotalReceivedQty").text("");
}

/***-------------------------------- Dispatch Quantity Sub Item Section-----------------------------------------***/
 
function HideShowPageWise(sub_item, clickedrow) {/*only for Hide show sub item at time of Add New Row in item table*/
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "BYPSsub_item", "BYPSSubItemRecvQty");
    debugger;
    clickedrow.find("#BYPSsub_item").val(sub_item);
    var BYPSsub_item = clickedrow.find("#BYPSsub_item").val();
    clickedrow.find("#BYPSsub_item").val(BYPSsub_item);
}
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    
    $("#hdn_subitmFlag").val(flag);
    if (flag == 'DNSCAcceptQty' || flag == 'DNSCRejctQty' || flag == 'DNSCRewrkQty') {
        var ProductNm = clickdRow.find("#ProductName").val();
        var ProductId = clickdRow.find("#hdItemId").val();
        var UOM = clickdRow.find("#UOM").val();
    }
    else if (flag == 'BYPSRecvQty')
    {
        
        //var ProductNm = clickdRow.find("#ItemListName" + hfsno).text();
        var ProductNm = clickdRow.find("#select2-ItemListName" + hfsno+"-container").text();
        var ProductId = clickdRow.find("#hfItemID").val();
        var UOM = clickdRow.find("#UOM").val();
    }
    else  {
        var ProductNm = $("#DispQtyItemName").val();
        var ProductId = $("#hd_DispQtyItemId").val();
        var UOM = $("#DispQtyUOM").val();
    }
    var JobOrdNo = $("#JobOrdNo option:selected").text();
    var MDNo = clickdRow.find("#DispatchQtyDocNo").val(); 
    var MDDate = clickdRow.find("#DispatchQtyDocDate").val();
    var doc_no = $("#DeliveryNoteNumber").val();
    var doc_dt = $("#DeliveryNoteDate").val();

    //var UOMID = clickdRow.find("#UOMID").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "BYPSRecvQty") {
        $("#hdn_Sub_ItemByProductScrapDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.Item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.Qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#ReceiveQty").val();
    }
    else {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr")
            .find("#subItemSrcDocNo[value='" + MDNo + "']").closest('tr').each(function () {
                var row = $(this);
                var List = {};
                List.Item_id = row.find("#ItemId").val();
                List.sub_item_id = row.find('#subItemId').val();
                List.DispatchQty = row.find('#subItemDispQty').val();
                List.PendingQuantity = row.find('#subItemPendQty').val();
                var Bill_Qty = row.find('#subItemBillQty').val();
                if (Bill_Qty == "0") {
                    Bill_Qty = "";
                    List.BilledQty = Bill_Qty;
                }
                else {
                    List.BilledQty = row.find('#subItemBillQty').val();
                }
                var Recv_Qty = row.find('#subItemReceiveQty').val();
                if (Recv_Qty == "0") {
                    Recv_Qty = "";
                    List.ReceivedQuantity = Recv_Qty;
                }
                else {
                    List.ReceivedQuantity = row.find('#subItemReceiveQty').val();
                }
                
                if (flag == 'DNSCAcceptQty' || flag == 'DNSCRejctQty' || flag == 'DNSCRewrkQty') {
                    List.AcceptedQty = row.find('#subItemAccptQty').val();
                    List.RejectedQty = row.find('#subItemRejctQty').val();
                    List.ReworkableQty = row.find('#subItemRewrkQty').val();
                }
                else {
                    List.AcceptedQty = "";
                    List.RejectedQty = "";
                    List.ReworkableQty = "";
                }
                List.DispSrcDocNo = row.find('#subItemSrcDocNo').val();
                List.DispSrcDocDt = row.find('#subItemSrcDocDate').val();

                NewArr.push(List);
            });
    }
    
    if (flag == "DNSCBillQty") {
        Sub_Quantity = clickdRow.find("#Disp_BilledQuantity").val();
    }
    else if (flag == "DNSCRecQty") {
        Sub_Quantity = clickdRow.find("#Disp_ReceivedQuantity").val();
    }
    else if (flag == "DNSCPendQty") {
        Sub_Quantity = clickdRow.find("#Disp_PendingQuantity").val();
    } else if (flag == "DNSCDispQty") {
        Sub_Quantity = clickdRow.find("#Disp_DispatchedQuantity").val().trim();
    }
    else if (flag == "BYPSRecvQty") {
        Sub_Quantity = clickdRow.find("#ReceiveQty").val();
    }
    else if (flag == "DNSCAcceptQty") {
        Sub_Quantity = clickdRow.find("#AcceptedQuantity").val().trim();
    }
    else if (flag == "DNSCRejctQty") {
        Sub_Quantity = clickdRow.find("#RejectedQuantity").val().trim();
    }
    else  {
        Sub_Quantity = clickdRow.find("#ReworkableQuantity").val().trim();
    }


    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $('#hfStatus').val();
    hd_Status = IsNull(hd_Status, "").trim();
    var JobOrdTyp = $("#DNSChdn_JobOrdTyp").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DeliveryNoteSC/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt,
            JobOrdNo: JobOrdNo,
            MDNo: MDNo,
            MDDate: MDDate,
            JobOrdTyp: JobOrdTyp

        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
            if (flag == 'DNSCAcceptQty' || flag == 'DNSCRejctQty' || flag == 'DNSCRewrkQty' || flag == 'BYPSRecvQty' ) {
                $("#subJONmbr").css("display", "none");
                $("#subSrcDocNo").css("display", "none");
                $("#subSrcDocdate").css("display", "none");
            }
            else {
                $("#subJONmbr").css("display", "block");
                $("#subSrcDocNo").css("display", "block");
                $("#subSrcDocdate").css("display", "block");
            }
            /*$("#subJONmbr").css("display", "block");*/
            
            $("#sub_JONumbr").val(JobOrdNo);
            $("#Sub_SrcDocNo").val(MDNo);
            $("#Sub_SrcDocDate").val(MDDate);
        }
    })

}
function fn_DNSCCustomReSetSubItemData(itemId, flag) {
    debugger;
    var DispSrcDocNo = $("#Sub_SrcDocNo").val();
    var DispSrcDocDt = $("#Sub_SrcDocDate").val();
    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        debugger
        var Crow = $(this).closest("tr");
        var subItemId = Crow.find("#subItemId").val();
        /* var subItemQty = Crow.find("#subItemQty").val();*/
        var subItemDispQty = Crow.find("#subItemDispQty").val();
        var subItemPendQty = Crow.find("#subItemPendQty").val();
        var subItemBillQty = Crow.find("#subItemBillQty").val();
        var subItemRecQty = Crow.find('#subItemReceiveQty').val();
             var AcceptedQty = "";
            var RejectedQty = "";
           var ReworkableQty = "";
        
        
        //var AcceptedQty = Crow.find('#subItemAccptQty').val();
        //var RejectedQty = Crow.find('#subItemRejctQty').val();
        //var ReworkableQty = Crow.find('#subItemRewrkQty').val();
        var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr')
            .find("#subItemSrcDocNo[value='" + DispSrcDocNo + "']").closest('tr').length;
        if (len > 0) {
            $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + DispSrcDocNo + "']").closest('tr').each(function () {
                debugger
                var InnerCrow = $(this).closest("tr");
                //var ItemId = InnerCrow.find("#ItemId").val();
                InnerCrow.find("#subItemDispQty").val(subItemDispQty);
                InnerCrow.find("#subItemPendQty").val(subItemPendQty);
                InnerCrow.find("#subItemBillQty").val(subItemBillQty);
                InnerCrow.find("#subItemReceiveQty").val(subItemRecQty);
                InnerCrow.find('#subItemAccptQty').val(AcceptedQty);
                InnerCrow.find('#subItemRejctQty').val(RejectedQty);
                    InnerCrow.find('#subItemRewrkQty').val(ReworkableQty);
                    InnerCrow.find('#subItemSrcDocNo').val(DispSrcDocNo);
                    InnerCrow.find('#subItemSrcDocDate').val(DispSrcDocDt);

            });
        } else {

            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemDispQty" value='${subItemDispQty}'></td>
                            <td><input type="text" id="subItemPendQty" value='${subItemPendQty}'></td>
                            <td><input type="text" id="subItemBillQty" value='${subItemBillQty}'></td>
                            <td><input type="text" id="subItemReceiveQty" value='${subItemRecQty}'></td>
                            <td><input type="text" id="subItemAccptQty" value='${AcceptedQty}'></td>
                            <td><input type="text" id="subItemRejctQty" value='${RejectedQty}'></td>
                            <td><input type="text" id="subItemRewrkQty" value='${ReworkableQty}'></td>
                            <td><input type="text" id="subItemSrcDocNo" value='${DispSrcDocNo}'></td>
                            <td><input type="text" id="subItemSrcDocDate" value='${DispSrcDocDt}'></td>
                        </tr>`);

        }

    });

}
function DN_CheckValidations_forSubItems(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage) {
    debugger;
    
    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        item_id = $("#" + Item_field_id).val();
        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
        var item_PrdBillQty = PPItemRow.find("#" + "Disp_BilledQuantity").val();
        var PrdRecevQty = PPItemRow.find("#" + "Disp_ReceivedQuantity").val();
        var SrcDocNo = PPItemRow.find("#" + "DispatchQtyDocNo").val();
        var sub_item = PPItemRow.find("#sub_item").val();
        var Sub_Quantity = 0;
        var Sub_BillQuantity = 0;
        var Sub_RecevQuantity = 0;
        
        $("#hdn_Sub_ItemDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr')
        .find("#subItemSrcDocNo[value='" + SrcDocNo + "']").closest('tr').each(function () {
            var Crow = $(this).closest("tr");
            var subItemSrcDocNo = Crow.find("#subItemSrcDocNo").val();
            if (subItemSrcDocNo == SrcDocNo) {
                var subItemBillQty = Crow.find("#subItemBillQty").val();
                Sub_BillQuantity = parseFloat(Sub_BillQuantity) + parseFloat(CheckNullNumber(subItemBillQty));
                var subItemRecvQty = Crow.find("#subItemReceiveQty").val();
                Sub_RecevQuantity = parseFloat(Sub_RecevQuantity) + parseFloat(CheckNullNumber(subItemRecvQty));

            }
            else {
                var subItemBillQty = Crow.find("#subItemBillQty").val();
                Sub_BillQuantity = parseFloat(Sub_BillQuantity) + parseFloat(CheckNullNumber(subItemBillQty));
                var subItemRecvQty = Crow.find("#subItemReceiveQty").val();
                Sub_RecevQuantity = parseFloat(Sub_RecevQuantity) + parseFloat(CheckNullNumber(subItemRecvQty));
            }

            


        });
        if (sub_item == "Y") {

            if (item_PrdBillQty != Sub_BillQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "PD_SubItemBillQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "PD_SubItemBillQty").css("border", "");
            }
            if (PrdRecevQty != Sub_RecevQuantity) {
                flag = "Y";
                PPItemRow.find("#" + "PD_SubItemRecvQty").css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + "PD_SubItemRecvQty").css("border", "");
            }

        }
    });

    if (flag == "Y") {
        if (ShowMessage == "Y") {

            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}

function CheckValidations_forSubItems() {
    debugger;
    var ErrFlg = "";
    if (DN_CheckValidations_forSubItems("DispatchQtyInfoTbl", "", "hd_DispQtyItemId", "Disp_BilledQuantity", "PD_SubItemBillQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (DN_CheckValidations_forSubItems("DispatchQtyInfoTbl", "", "hd_DispQtyItemId", "Disp_ReceivedQuantity", "PD_SubItemRecvQty", "Y") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }
}
function ResetWorningBorderColor() {
    var ErrFlg = "";
    if (DN_CheckValidations_forSubItems("DispatchQtyInfoTbl", "", "hd_DispQtyItemId", "Disp_BilledQuantity", "PD_SubItemBillQty", "N") == false) {
        ErrFlg = "Y"
    }
    if (DN_CheckValidations_forSubItems("DispatchQtyInfoTbl", "", "hd_DispQtyItemId", "Disp_ReceivedQuantity", "PD_SubItemRecvQty", "N") == false) {
        ErrFlg = "Y"
    }
    if (ErrFlg == "Y") {
        return false
    }
    else {
        return true
    }

}
function DN_SubItemList() {
    var NewArr = new Array();
    var SubitmFlag = $("#hdn_subitmFlag").val();
    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        debugger;
        var BillQty ="";
        var RecQty = "";
        BillQty = row.find('#subItemBillQty').val();
        RecQty = row.find('#subItemReceiveQty').val();
        if ((parseFloat(CheckNullNumber(BillQty)) + parseFloat(CheckNullNumber(RecQty))) > 0) {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.DispatchQty = row.find('#subItemDispQty').val();
            List.PendingQuantity = row.find('#subItemPendQty').val();
            List.BilledQty = row.find('#subItemBillQty').val();
            List.ReceivedQuantity = row.find('#subItemReceiveQty').val();
            List.AcceptedQty = row.find('#subItemAccptQty').val();
            List.RejectedQty = row.find('#subItemRejctQty').val();
            List.ReworkableQty = row.find('#subItemRewrkQty').val();
            List.DispSrcDocNo = row.find('#subItemSrcDocNo').val();
            List.DispSrcDocDt = row.find('#subItemSrcDocDate').val();
            NewArr.push(List);
        }



    });
    return NewArr;
}
function DeleteSubItemDispQtyDetail(item_id, MDispNo) {
    debugger;

    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemDetailTbl >tbody >tr").length > 0) {
            if ($("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr')
                .find("#subItemSrcDocNo[value='" + MDispNo + "']").closest('tr').length > 0) {
                $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + item_id + "]").closest('tr')
                    .find("#subItemSrcDocNo[value='" + MDispNo + "']").closest('tr').remove();
            }
        }
    }

}

function ItemtbltoDispQtyTbltoSubItmValidation(ShowMessage) {
    debugger;
    var flag = "N";
    var EffectedRows;
    DNItmLen = $("#DnSCItmDetailsTbl tbody tr").length
   
    if (DNItmLen > 0) {
        $("#DnSCItmDetailsTbl tbody tr").each(function () {
            var Row = $(this);
            var Item_Id = Row.find("#hdItemId").val();

            if (Item_Id != null && Item_Id != "") {
                var Displen = $("#SaveItemDispatchQtyDetails >tbody >tr").length
                if (Displen > 0) {
                    $("#SaveItemDispatchQtyDetails >tbody >tr").each(function () {
                        var currentRow = $(this);
                        debugger;
                        var Sno = currentRow.find("#hdSNoRowID").val();
                        /*var DispItemid = $("#hd_DispQtyItemId").val();*/
                        var DispItemid = currentRow.find("#PD_DispQtyItemID").val();
                        var DispatchNo = currentRow.find("#PD_DispDocNo").val();
                        var Disp_BillQty = currentRow.find("#PD_BillQty").val();
                        var Disp_RecvQty = currentRow.find("#PD_ReceivQty").val();
                        var sub_item = currentRow.find("#sub_item").val();
                        var Sub_BillQty = 0;
                        var Sub_RecvQty = 0;
                        var rows = $("#hdn_Sub_ItemDetailTbl >tbody >tr #ItemId[value=" + DispItemid + "]").closest('tr')
                            .find("#subItemSrcDocNo[value='" + DispatchNo + "']").closest('tr')
                        rows.each(function () {
                            var Crow = $(this).closest("tr");
                            //var subItemId = Crow.find("#subItemId").val();
                            var subItemBillQty = Crow.find("#subItemBillQty").val();
                            Sub_BillQty = parseFloat(Sub_BillQty) + parseFloat(CheckNullNumber(subItemBillQty));
                            var subItemReceiveQty = Crow.find("#subItemReceiveQty").val();
                            Sub_RecvQty = parseFloat(Sub_RecvQty) + parseFloat(CheckNullNumber(subItemReceiveQty));
                        });
                    });
                }
                var SubItemButton = "DispatchDetailBtnIcon";
                if (Item_Id != null && Item_Id != "") {
                    if (sub_item == "Y") {
                        if (Disp_BillQty != Sub_BillQty) {
                            flag = "Y";
                            $("#" + SubItemButton).css("border", "1px solid red");

                        } else {
                            $("#" + SubItemButton).css("border", "");
                        }
                        if (Disp_RecvQty != Sub_RecvQty) {
                            flag = "Y";
                            $("#" + SubItemButton).css("border", "1px solid red");

                        } else {
                            $("#" + SubItemButton).css("border", "");
                        }

                    }
                }

            }
            else {
                flag == "Y"
            }
        });
        if (flag == "Y") {
            if (ShowMessage == "Y") {
                swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
            }

            return false;
        }
        else {
            return true;
        }
    }

    
}

/*--------BY Product Scrap Sub Item ----------------------------*/
function BYProductScrapCheckValidations_forSubItems() {
    debugger
    var RtrnItmLen = $('#ReturnItemDetailsTbl >tbody >tr').length
    if (RtrnItmLen > 0) {
        return BYProductScrapSubItemValidation("ReturnItemDetailsTbl", "SNohiddenfiled", "ItemListName", "ReceiveQty", "BYPSSubItemRecvQty", "Y");
        
    }
}
function DNSCByPrdctScrpResetWorningBorderColor() {
    debugger;
    var RtrnItmLen = $('#ReturnItemDetailsTbl >tbody >tr').length
    if (RtrnItmLen > 0) {
        return BYProductScrapSubItemValidation("ReturnItemDetailsTbl", "SNohiddenfiled", "ItemListName", "ReceiveQty", "BYPSSubItemRecvQty", "N");
    }
}

function BYProductScrapSubItemValidation(Main_table, Row_Id, Item_field_id, Item_Qty_field_id, SubItemButton, ShowMessage) {
    debugger;
   

    var flag = "N";
    $("#" + Main_table + " tbody tr").each(function () {
        var PPItemRow = $(this);
        var item_id;
        var Fullitem;
        if (Row_Id != "" && Row_Id != null) {
            var Sno = PPItemRow.find("#" + Row_Id).val();
            Fullitem = PPItemRow.find("#" + Item_field_id + Sno).val();
            Fullitembrk = Fullitem.split(',')
            item_id = Fullitembrk[0];
        }
        else
        {
           item_id = PPItemRow.find("#" + Item_field_id).val();
        }

        var item_PrdQty = PPItemRow.find("#" + Item_Qty_field_id).val();
       
        var sub_item = PPItemRow.find("#BYPSsub_item").val();
       
        var Sub_Quantity = 0;
        //if (sub_item == "Y") {
        //    var flag = "N";
        //    $("#hdn_Sub_ItemByProductScrapDetailTbl tbody tr #ItemId[value=" + item_id + "]").closest('tr').each(function () {
        //        var Crow = $(this).closest("tr");
        //        var subItemQty = Crow.find("#subItemQty").val();
        //        Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
        //    });
        //    if (parseFloat(item_PrdQty) != parseFloat(Sub_Quantity)) {
        //        flag = "Y";
        //        $("#" + SubItemButton).css("border", "1px solid red");
        //        swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        //        return false;
        //    } else {
        //        $("#" + SubItemButton).css("border", "");
        //        return true;
        //    }
        //}
        var hdnlen = $("#hdn_Sub_ItemByProductScrapDetailTbl tbody tr").length;
        if (hdnlen > 0) {
            $("#hdn_Sub_ItemByProductScrapDetailTbl tbody tr #ItemId[value='" + item_id + "']").closest('tr').each(function () {
               var Crow = $(this).closest("tr");
                //var subItemId = Crow.find("#subItemId").val();
                var subItemQty = Crow.find("#subItemQty").val();
                Sub_Quantity = parseFloat(Sub_Quantity) + parseFloat(CheckNullNumber(subItemQty));
            });
        }
        if (sub_item == "Y") {
            if (item_PrdQty != Sub_Quantity) {
                flag = "Y";
                PPItemRow.find("#" + SubItemButton).css("border", "1px solid red");
            } else {
                PPItemRow.find("#" + SubItemButton).css("border", "");
            }
        }
        
    });

    if (flag == "Y") {
        if (ShowMessage == "Y") {

            swal("", $("#SubItemQuantityDoesNotMatchWithItemQuantity").text(), "warning");
        }

        return false;
    }
    else {
        return true;
    }
}
function SaveByPrdctScrap_SubItemList() {
    debugger;
    var NewArr = new Array();
     $("#hdn_Sub_ItemByProductScrapDetailTbl tbody tr").each(function () {
        var row = $(this);
        debugger;
        if (parseFloat(CheckNullNumber(row.find('#subItemQty').val())) > 0) {
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.Qty = row.find('#subItemQty').val();
            
            NewArr.push(List);
        }
     });
    return NewArr;
}
function DeletePrdctScrapSubItemDetail(item_id) {
    debugger;
    if (item_id != null && item_id != "") {
        if ($("#hdn_Sub_ItemByProductScrapDetailTbl >tbody >tr").length > 0) {
            if ($("#hdn_Sub_ItemByProductScrapDetailTbl >tbody >tr td #ItemId[value='" + item_id + "']").length > 0) {
                $("#hdn_Sub_ItemByProductScrapDetailTbl >tbody >tr td #ItemId[value='" + item_id + "']").closest('tr').remove();
            }
        }
    }

}
/***--------------------------------Sub Item Section End-----------------------------------------***/


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


