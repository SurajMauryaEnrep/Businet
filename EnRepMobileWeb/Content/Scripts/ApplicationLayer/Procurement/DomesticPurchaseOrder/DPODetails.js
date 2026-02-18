
/*-----------------Modified by Suraj Maurya on 22-10-2024-------------------*/
const ExchDecDigit = $("#ExchDigit").text();///Amount
const import_ord_type = $("#OrderTypeImport").is(":checked");///Amount
const QtyDecDigit = import_ord_type ? $("#ExpImpQtyDigit").text() : $("#QtyDigit").text();///Quantity
const RateDecDigit = import_ord_type ? $("#ExpImpRateDigit").text() : $("#RateDigit").text();///Rate And Percentage
const ValDecDigit = import_ord_type ? $("#ExpImpValDigit").text() : $("#ValDigit").text();///Amount
/*-----------------Modified by Suraj Maurya on 22-10-2024 End-------------------*/

$(document).ready(function () {
    ReplicateWith();
    //$("#OrderTypeLocal").prop("checked", true);
    //$("#OrderTypeImport").prop("disabled", true);
    $("#OrderTypeService").prop("disabled", true);
    //$("#imp_file_no").prop("readonly", true);
    //$("#cntry_origin").prop("disabled", true);
    $("#ddlRequiredArea").select2();
    var Pending_Flag = $("#Pending_SourceDocumentLink").val();
    debugger
    if (Pending_Flag == "CreateDocument_Pending") {
        OnChangeSourType();
        var supp_id = $("#Pending_Supp_id").val()
        OnChangeSuppName(supp_id);
    }

    BindDDlPOList();
    if ($("#src_type").val() == "D" && ($("#hfStatus").val() == "D" || $("#hfStatus").val() == "")) {

        BindPOItmList(1);
    }

    // jQuery button click event to remove a row. 
    $('#PoItmDetailsTbl tbody').on('click', '.deleteIcon', function () {
        ////debugger;
        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();
        // Iterating across all the rows  
        // obtained to change the index 
        child.each(function () {
            // Getting <tr> id. 
            var id = $(this).attr('id');
            // Getting the <p> inside the .row-index class. 
            var idx = $(this).children('.row-index').children('p');
            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));
            // Modifying row index. 
            idx.html(`Row ${dig - 1}`);
            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });
        // Removing the current row. 
        $(this).closest('tr').remove();
        //debugger;
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";

        ItemCode = $(this).closest('tr').find("#POItemListName" + SNo).val();

        ////var ItemCode = $(this).closest('tr')[0].cells[3].children[0].children[0].value;
        // ShowItemListItm(ItemCode);
        CalculateAmount();
        var TOCAmount = parseFloat($("#PO_OtherCharges").val());
        if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
            TOCAmoun = 0;
        }
        Calculate_OC_AmountItemWise(TOCAmount);
        Cmn_DeleteSubItemQtyDetail(ItemCode);
        SerialNoAfterDelete();
        AfterDeleteResetPO_ItemTaxDetail();
        DelDeliSchAfterDelItem(ItemCode);
        BindOtherChargeDeatils();
        if ($("#src_type").val() == "PR" || $("#src_type").val() == "Q") {
            if ($("#PoItmDetailsTbl >tbody >tr").length == 0) {
                var Status = $("#hfStatus").val();
                if (Status == "" || Status == null) {
                    $("#src_doc_number").val("---Select---").trigger('change');
                }
                $("#src_doc_number").attr('disabled', true);
                //else {  //commented by shubham maurya on 06-10-2025 
                //    $("#src_doc_number").val("0").trigger('change');
                //}
                //$("#ddlRequiredArea").attr('disabled', false);
                //$("#ddlRequiredArea").val("0").trigger('change');
            }
        }
        // Decreasing total number of rows by 1. 
        //rowIdx--;
    });


    

    Cmn_DeleteDeliverySch();
    DeleteTermCondition();
    BindCountryList();

    po_no = $("#po_no").val();
    $("#hdDoc_No").val(po_no);

    if ($("#ddlstockable").is(":checked")) {
        $("#PoItmDetailsTbl").removeClass('width2500');
        $("#PoItmDetailsTbl").addClass('width3100');

    }
    if ($("#ddlconsumable").is(":checked")) {
        $("#PoItmDetailsTbl").removeClass('width3100');
        $("#PoItmDetailsTbl").addClass('width2500');

    }
    $("#ItemDetailsTbl tbody").bind("click", function (e) {
        ////debugger;

        var clickedrow = $(e.target).closest("tr");
        if (clickedrow.find('#OrderTypeLocal').prop("disabled") == false) {
            $("#ItemDetailsTbl >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");

            clickedrow.find('#OrderTypeLocal').prop("checked", true);
            var Address_id = clickedrow.find("#addhd").text();
            var Address = clickedrow.find("#cust_add").text();
            var pin = clickedrow.find("#cust_pin").text();
            var city = clickedrow.find("#city_name").text();
            var dist = clickedrow.find("#district_name").text();
            var state = clickedrow.find("#state_name").text();
            var country = clickedrow.find("#country_name").text();

            $('#hd_TxtBillingAddr').val(Address + ',' + ' ' + city + '-' + ' ' + pin + ',' + ' ' + dist + ',' + ' ' + state + ',' + ' ' + country);
            $("#hd_bill_add_id").val(Address_id);
        }
    });
    CancelledRemarks("#Cancelled", "Disabled");
});
function BindCountryList() {
    //debugger;   
    $("#cntry_origin").select2({
        ajax: {
            url: $("#cntry_originList").val(),
            data: function (params) {
                var queryParameters = {
                    SO_Country: params.term,
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    //LSO_ErrorPage();
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
function SaveBtnClick() {
    ////debugger;
    try {
        let result = true;
        let OrdStatus = $("#hfStatus").val();
        let _po_no = $("#po_no").val();
        let _po_date = $("#po_date").val();
        if (!["D", "F", "", null].includes(OrdStatus)) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/DPO/CheckDependency",
                data: { OrdStatus: OrdStatus, PO_No: _po_no, PO_Date: _po_date },
                async: false,
                success: function (data) {
                    debugger;
                    if (data == 'Used') {
                        swal("", $("#PurchaseOrderHasBnPrcsdCanNotBeMdfd").text(), "warning");
                        result = false;
                    } else {
                        result = InsertPODetails();
                    }

                }
            })
            if (result == true) {
                return true;
            } else {
                return false;
            }
        } else {
            return InsertPODetails();
        }
       
    }
    catch (EX) {
        console.log(EX);
    }
    
}
function ShowSavedAttatchMentFiles(_LPO_No, _LPO_Date) {

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DPO/GetPOAttatchDetailEdit",
        data: { PO_No: _LPO_No, PO_Date: _LPO_Date },
        success: function (data) {
            debugger;
            if (data == 'ErrorPage') {
                PO_ErrorPage();
                return false;
            } else {
                $("#PartialImageBind").html(data);
            }

        }
    });

}
function ApproveBtnClick() {
    InsertPOApproveDetails("", "", "");
}
function DeleteBtnClick() {
    //RemoveSession();
    Delete_PoDetails();
    ResetWF_Level();

}
function OnClickPrintBtn() {
    debugger;
    $("#hd_order_no").val($("#po_no").val());
    $("#hd_order_dt").val($("#po_date").val());
    return true;
}
function ForwardBtnClick() {
    /*commented by Hina on 08-02-2024 to chk Financial year exist or not*/
    debugger;
    try {
        var OrderStatus = "";
        OrderStatus = $('#hfStatus').val();
        if (OrderStatus === "D" || OrderStatus === "F") {

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
    catch (ex) {
        console.log(ex);
        return false;
    }
    

    return false;

    /*commented by Hina on 30-05-225 as per discuss with vishal sir*/

    //debugger;
    ///*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
    //var compId = $("#CompID").text();
    //var brId = $("#BrId").text();
    //var po_date = $("#po_date").val();
    //$.ajax({
    //    type: "POST",
    //    /* url: "/Common/Common/CheckFinancialYear",*/
    //    url: "/Common/Common/CheckFinancialYearAndPreviousYear",
    //    data: {
    //        compId: compId,
    //        brId: brId,
    //        DocDate: po_date
    //    },
    //    success: function (data) {
    //        /* if (data == "Exist") { *//*End to chk Financial year exist or not*/
    //        if (data == "TransAllow") {
    //            var OrderStatus = "";
    //            OrderStatus = $('#hfStatus').val();
    //            if (OrderStatus === "D" || OrderStatus === "F") {

    //                if ($("#hd_nextlevel").val() === "0") {
    //                    $("#Btn_Forward").attr("data-target", "");
    //                }
    //                else            {
    //                    $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //                 $("#Btn_Approve").attr("data-target", "");
    //                }
    //                var Doc_ID = $("#DocumentMenuId").val();
    //                $("#OKBtn_FW").attr("data-dismiss", "modal");

    //                Cmn_GetForwarderList(Doc_ID);

    //            }
    //            else {
    //                $("#Btn_Forward").attr("data-target", "");
    //                $("#Btn_Forward").attr('onclick', '');
    //                $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //            }
    //        }
    //        else {/* to chk Financial year exist or not*/
    //            swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
    //            $("#Btn_Forward").attr("data-target", "");
    //            $("#Btn_Approve").attr("data-target", "");
    //            $("#Forward_Pop").attr("data-target", "");

    //        }
    //    }
    //});
    ///*End to chk Financial year exist or not*/
    //return false;
}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#po_no").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var PONo = "";
    var PODate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    PONo = $("#po_no").val();
    PODate = $("#po_date").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var TrancType = (PONo + ',' + PODate + ',' + "Update" + ',' + WF_status1 + ',' + docid)
    try {
        //var pdfAlertEmailFilePath = 'PO_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        //await GetPdfFilePathToSendonEmailAlert(PONo, PODate, pdfAlertEmailFilePath);
        if (fwchkval != "Approve") {
            var pdfAlertEmailFilePath = PONo.replace(/\//g, "") + "_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
            var pdfAlertEmailFilePath1 = await GetPdfFilePathToSendonEmailAlert(PONo, PODate, pdfAlertEmailFilePath, docid, fwchkval);
            if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
                pdfAlertEmailFilePath = "";
            }
        }
    }
    catch {
        debugger;
    }
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        //debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });

    if (fwchkval === "Forward") {
        if (fwchkval != "" && PONo != "" && PODate != "" && level != "") {
            debugger;
            //var Action_name = "DPODetail,DPO";
            // location.href = "/Common/Common/InsertForwardDetailsNew/?docno=" + PONo + "&docdate=" + PODate + "&doc_id=" + docid + "&level=" + level + "&forwardedto=" + forwardedto + "&fstatus=" + fwchkval + "&remarks=" + Remarks + "&action_name=" + Action_name;
            /*Cmn_InsertDocument_ForwardedDetail(PONo, PODate, docid, level, forwardedto, fwchkval, Remarks);*/
            await Cmn_InsertDocument_ForwardedDetail1(PONo, PODate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DPO/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var src_doc_number = $("#src_doc_number").val();
        var src_doc_date = $("#src_doc_date").val();
        var list = [{ PONo: PONo, PODate: PODate, src_doc_number: src_doc_number, src_doc_date: src_doc_date, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/DPO/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1 + "&docid=" + docid;
        //InsertPOApproveDetails("Approve", $("#hd_currlevel").val(), Remarks);

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && PONo != "" && PODate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PONo, PODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DPO/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && PONo != "" && PODate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(PONo, PODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/DPO/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
async function GetPdfFilePathToSendonEmailAlert(poNo, poDate, fileName, docid, fwchkval) {
    debugger;
    var filepath = "";
    var PrintFormat = [];
    PrintFormat.push({
        ShowProdDesc: $("#ShowProdDesc").val(),
        ShowProdTechDesc: $("#ShowProdTechDesc").val(),
        PrintFormat: $("#PrintFormat").val(),
        ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
        ShowSubItem: $("#ShowSubItem").val(),
        ItemAliasName: $("#ItemAliasName").val(),
        ShowHsnNumber: $("#ShowHSNNumber").val(),
        ShowDeliverySchedule: $("#ShowDeliverySchedule").val(),
        CustAliasName: "N",
        PrintRemarks: "N",
        ShowMRP: $("#hdn_ShowNRP").val(),
        ShowPackSize: $("#hdn_ShowPackSize").val(),
    })
    try {
        filepath = await $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DPO/SavePdfDocToSendOnEmailAlert",
            data: { poNo: poNo, poDate: poDate, fileName: fileName, PrintFormat: JSON.stringify(PrintFormat), docid: docid, docstatus: fwchkval },
            /*dataType: "json",*/
            success: function (data) {

            }
        });
    }
    catch (error) {
    }
    return filepath;
}
//function onclickAmendmentBtn() {
//    debugger;
//    var PONo = $("#po_no").val();
//    var PODate = $("#po_date").val();
//    var docid = $("#DocumentMenuId").val();
//    var TrancType = (PONo + ',' + PODate + ',' + docid)
//    var fwchkval = "Reject";
//    var Remarks = "";
//    var forwardedto = "";
//    if (fwchkval != "" && PONo != "" && PODate != "") {
//        Cmn_InsertDocument_ForwardedDetail(PONo, PODate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
//        window.location.href = "/ApplicationLayer/DPO/ToAmendmentByJS?TrancType=" + TrancType;
//    }
//}
function BindDDlPOList() {
    var Branch = sessionStorage.getItem("BranchID");
    var Pending_SourceDocument = $("#Pending_SourceDocumentLink").val();
    $("#SupplierName").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                debugger;
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SrcType: $("#SrcType").val(), // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch,
                    Pending_SourceDocument: Pending_SourceDocument
                };
                debugger;
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
function BindPOItmList(ID) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
    var ItemCode = "";
    ItemCode = $(this).closest('tr').find("#POItemListName" + SNo).val();
    Cmn_DeleteSubItemQtyDetail(ItemCode);

    if ($("#ddlstockable").is(":checked")) {
        Branchchk = "S";
        BindPItemList("#POItemListName", ID, "#PoItmDetailsTbl", "#SNohiddenfiled", "", "PO");
    }
    if ($("#ddlconsumable").is(":checked")) {
        Branchchk = "C";
        // $("#src_type").attr('disabled', true);
        //$("#src_type").val('D')
        BindPItemList("#POItemListName", ID, "#PoItmDetailsTbl", "#SNohiddenfiled", "", "ConsumableInvoice");
    }
    if ($("#ddlAll").is(":checked")) {
        Branchchk = "A";
        BindPItemList("#POItemListName", ID, "#PoItmDetailsTbl", "#SNohiddenfiled", "", "AllPOandConsumble");
    }
    if (docid == "105101136") {
        Branchchk = "C";
        $("#src_type").val('D')
        BindPItemList("#POItemListName", ID, "#PoItmDetailsTbl", "#SNohiddenfiled", "", "ConsumableInvoice");
    }
}
function BindPItemList(ItmDDLName, RowID, TableID, SnoHiddenField, OtherFx, PageName) {
    debugger;
    if ($(ItmDDLName + RowID).val() == "0" || $(ItmDDLName + RowID).val() == "" || $(ItmDDLName + RowID).val() == null) {
        $(ItmDDLName + RowID).append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
        $('#Textddl' + RowID).append(`<option data-uom="0" value="0">---Select---</option>`);
    }
    if ($(TableID + " tbody tr").length > 0) {
        $(TableID + " tbody tr").each(function () {
            var currentRow = $(this);
            var rowno = "";
            if (RowID != null && RowID != "") {
                rowno = currentRow.find(SnoHiddenField).val();
            }
            DynamicSerchableItemDDLPO(TableID, ItmDDLName, rowno, SnoHiddenField, OtherFx, PageName)

        });
    }
    else {
        DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName)
    }
    if (OtherFx == "BindData") {
        BindData();
    }
}
function DynamicSerchableItemDDLPO(TableID, ItmDDLName, RowID, SnoHiddenField, OtherFx, PageName) {
    var firstEmptySelect = true;
    $(ItmDDLName + RowID).select2({

        ajax: {
            url: "/Common/Common/GetItemList3",
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

                ConvertIntoArreyList(TableID, ItmDDLName, OtherFx, SnoHiddenField);
                var ItemListArrey = [];
                if (sessionStorage.getItem("selecteditemlist").length != null) {
                    ItemListArrey = JSON.parse(sessionStorage.getItem("selecteditemlist"));
                }
                let selected = [];
                selected.push({ id: $(ItmDDLName + RowID).val() });
                selected = JSON.stringify(selected);

                var NewArrey = ItemListArrey.filter(i => !selected.includes(i.id));


                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));

                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="">
<div class="row"><div class="col-md-6 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemType").text()}</div></div>
</strong></li></ul>`)
                }
                var page = params.page || 1;
                // data = data.slice((page - 1) * pageSize, page * pageSize); /*commented by Suraj For creating scroll*/
                Fdata = data.slice((page - 1) * pageSize, page * pageSize);
                if (page == 1) {
                    if (Fdata[0] != null) {
                        if (Fdata[0].Name.trim() != "---Select---") {
                            var select = { ID: "0_0", Name: " ---Select---_0" };
                            Fdata.unshift(select);
                        }
                    }
                }

                return {
                    results: $.map(Fdata, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name.split("_")[0], UOM: val.ID.split("_")[1], type: val.Name.split("_")[1] };
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
                '<div class="row dpo_item">' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.type + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        },
    });
}
function OnChangeSuppName(SuppID) {
    debugger;
    var Supp_id = "";
    var docid = $("#DocumentMenuId").val();
    var Pending_Flag = $("#Pending_SourceDocumentLink").val();
    if (Pending_Flag == "CreateDocument_Pending") {
        Supp_id = SuppID;
    }
    else {
        Supp_id = SuppID.value;
    }
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        DisableItemDetailsPO();
        $("#PoItmDetailsTbl .plus_icon1").css("display", "none");
        $("#Address").val("");
        $("#ddlCurrency").html("");
        $("#conv_rate").val("");
        $("#conv_rate").prop("readonly", true);
        $("#ddlReplicateWith").attr("disabled", true);
    }
    else {
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_SuppName").val(SuppName)
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")

        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            currentRow.find("#POItemListName" + Sno).attr("disabled", false);
            currentRow.find("#UOM").attr("disabled", false);
            currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
            currentRow.find("#BtnTxtCalculation").attr("disabled", true);
            currentRow.find("#ord_qty_spec").attr("disabled", false);
            currentRow.find("#item_rate").attr("disabled", false);
            currentRow.find("#TaxExempted").attr("disabled", false);
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                currentRow.find("#ManualGST").attr("disabled", false);
            }
            currentRow.find("#item_disc_perc").attr("disabled", false);
            currentRow.find("#item_disc_amt").attr("disabled", false);
            currentRow.find("#item_disc_amt").attr("readonly", false);
            //currentRow.find("#item_ass_val").attr("disabled", false); //Commented by Suraj on 01-04-2024
            currentRow.find("#remarks").attr("disabled", false);
            //currentRow.find("#SimpleIssue").attr("disabled", false);commented by shubham maurya on 01-10-2025
            currentRow.find("#POItemListNameError").css("display", "none");
            currentRow.find("#POItemListName" + Sno).css("border-color", "#ced4da");//aria-labelledby="select2-POItemListName1-container"
            currentRow.find("select2-selection select2-selection--single").css("border", "1px solid #aaa");
            currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border-color", "#ced4da");
            currentRow.find("#ord_qty_specError").css("display", "none");
            currentRow.find("#ord_qty_spec").css("border-color", "#ced4da");
            currentRow.find("#item_rateError").css("display", "none");
            currentRow.find("#item_rate").css("border-color", "#ced4da");
        })

        var SourceType = $("#src_type").val();
        if (SourceType == "PR") {
            // disableItemDetails();
            $("#PoItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                //currentRow.find("#delBtnIcon").css("display", "none");
                currentRow.find("#POItemListName" + Sno).attr("disabled", true);
                currentRow.find("#UOM").attr("disabled", true);
                currentRow.find("#ItmInfoBtnIcon").attr("disabled", true);
                currentRow.find("#ord_qty_spec").attr("disabled", true);
                //currentRow.find("#item_rate").attr("disabled", true);
                //currentRow.find("#item_disc_perc").attr("disabled", true);
                //currentRow.find("#TaxExempted").attr("disabled", true);
                currentRow.find("#item_ass_val").attr("disabled", true);
                //currentRow.find("#remarks").attr("disabled", f);
                //currentRow.find("#SimpleIssue").attr("disabled", true); commented by shubham maurya on 01 - 10 - 2025
                currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
                if (docid != '105101140101') {
                    currentRow.find("#BtnTxtCalculation").attr("disabled", false);
                }
            });
            $("#src_doc_number").attr('disabled', false);
            $("#ddlRequiredArea").attr('disabled', false);
        }
        //else {
        //    $("#PoItmDetailsTbl #delBtnIcon").css("display", "block");
        //    $("#PoItmDetailsTbl .plus_icon1").css("display", "block");
        //}

        else if (SourceType == "Q") {
            // disableItemDetails();
            $("#PoItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                currentRow.find("#delBtnIcon").css("display", "none");
                currentRow.find("#POItemListName" + Sno).attr("disabled", true);
                currentRow.find("#UOM").attr("disabled", true);
                currentRow.find("#ItmInfoBtnIcon").attr("disabled", true);
                currentRow.find("#ord_qty_spec").attr("disabled", true);
                //currentRow.find("#item_rate").attr("disabled", true);
                //currentRow.find("#item_disc_perc").attr("disabled", true);
                //currentRow.find("#item_disc_amt").attr("disabled", true);
                currentRow.find("#item_ass_val").attr("disabled", true);
                //currentRow.find("#remarks").attr("disabled", f);
                //currentRow.find("#SimpleIssue").attr("disabled", true);commented by shubham maurya on 01-10-2025
                currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
                if (docid != '105101140101') {
                    currentRow.find("#BtnTxtCalculation").attr("disabled", false);
                }

                BindSrcDocNumberOnBehalfSrcType(SourceType);
            });
            $("#src_doc_number").attr('disabled', false);
        }
        else {
            $("#PoItmDetailsTbl #delBtnIcon").css("display", "block");
            $("#PoItmDetailsTbl .plus_icon1").css("display", "block");
            $("#ddlReplicateWith").attr("disabled", false);
        }
      
    }
    GetSuppAddress(Supp_id);

}
function onChangeRequiredArea() {
    debugger;
    var SourceType = $("#src_type").val();
    var ddlRequiredArea = $("#ddlRequiredArea").val();
    var HdnReqArea = $("#HdnReqArea").val();
    if (HdnReqArea == "N") {
        BindSrcDocNumberOnBehalfSrcType(SourceType, null, ddlRequiredArea);
    }
}
function GetContectDeatilSupplier() {
    debugger;
    var Cont_per = $("#hdnContectPersone").val();
    var Cont_no = $("#hdnContectNumber").val();
    var Status = $("#hfStatus").val();
    var Disable = $("#Disable").val();
    var Suppid = $("#SupplierName option:selected").val();
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/DPO/GetSuppContectDetail",
                data: {
                    Suppid: Suppid,
                    Status: Status,
                    Cont_per: Cont_per,
                    Cont_no: Cont_no,
                    Disable: Disable

                },
                success: function (data) {
                    //debugger;
                    if (data == 'ErrorPage') {
                        PO_ErrorPage();
                        return false;
                    }
                    $("#ContectdeatilData").html(data);
                }
            })

    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }


    // }


}
function OnClickSaveAndExitContectDeatil() {
    debugger;
    var contectPerson = $("#ContactPerson").val();
    var contectNumber = $("#ContactNumber").val();
    $("#hdnContectPersone").val(contectPerson);
    $("#hdnContectNumber").val(contectNumber);
    $("#AlredySaveContectDetail").val("SaveData");
}
function OnClickResetBtnContectDeatil() {
    $("#ContactPerson").val("");
    $("#ContactNumber").val("");
    //$("#hdnContectPersone").val(null);
    //$("#hdnContectNumber").val(null);
    $("#AlredySaveContectDetail").val("");
}
function GetSuppAddress(Supp_id) {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/DPO/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                async:false,//Added by Suraj Maurya on 03-11-2025 to get conv_rate at instance.
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
                            $("#Ship_StateCode").val(arr.Table[0].state_code);
                            $("#Pymnt_terms").val(arr.Table[0].paym_term);/*Add by Hina Sharma on 22-04-2025*/
                            var s = '<option value="' + arr.Table[0].curr_id + '">' + arr.Table[0].curr_name + '</option>';
                            $("#ddlCurrency").html(s);
                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#ExchDigit").text()));
                            if ($("#OrderTypeImport").is(":checked")) {
                                if (arr.Table1[0].bs_curr_id == arr.Table[0].curr_id) {
                                    $("#conv_rate").prop("readonly", true);
                                } else {
                                    $("#conv_rate").prop("readonly", false);
                                }
                            }
                            else {
                                $("#conv_rate").prop("readonly", true);
                            }
                            if ($("#conv_rate").val() == "") {
                                $('#SpanSuppExRateErrorMsg').text($("#valueReq").text());
                                $("#conv_rate").css("border-color", "Red");
                                $("#SpanSuppExRateErrorMsg").css("display", "block");
                                ErrorFlag = "Y";
                            }
                            else {
                                $("#SpanSuppExRateErrorMsg").css("display", "none");
                                $("#conv_rate").css("border-color", "#ced4da");
                            }
                            //CheckPOHraderValidations();
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
                        else {
                            $("#Address").val(null);
                            $("#bill_add_id").val(null);
                            $("#ship_add_gstNo").val(null);
                            $("#Ship_StateCode").val(null);
                            var s = '<option value="' + "0" + '">' + "---Select---" + '</option>';
                            $("#ddlCurrency").html(s);
                            $("#conv_rate").val(parseFloat(0).toFixed(RateDecDigit));
                            $("#conv_rate").prop("readonly", false);
                        }


                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function OnChangeCurrency(CurrID) {
    //debugger;
    var Curr_id = CurrID.value;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/DPO/GetSuppExRateDetail",
                data: {
                    Curr_id: Curr_id
                },
                async: false,
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
                            $("#conv_rate").val(arr.Table[0].conv_rate);
                        }
                        else {
                            $("#conv_rate").val("0");
                        }
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function OnChangePOItemName(RowID, e) {
    //debugger;
    BindPOItemList(e);
}
function OnChangePOItemQty(RowID, e) {
    //debugger;
    CalculationBaseQty(e);
}
function OnChangePOItemRate(RowID, e) {
    //debugger;
    let trgtrow = $(e.target).closest("tr");
    CalculationBaseRate(trgtrow);
}
function OnChangePOItemDiscountPerc(RowID, e) {
    //debugger;
    let trgtrow = $(e.target).closest("tr");
    CalculateDisPercent(trgtrow);
}
function OnChangePOItemDiscountAmt(RowID, e) {
    //debugger;
    let trgtrow = $(e.target).closest("tr");
    CalculateDisAmt(trgtrow);
}
function OnChangeGrossAmt() {
    debugger;
    /*commented and modify by Hina on 09-07-2025*/
    //var TotalOCAmt = $('#Total_OC_Amount').text();
    //var Total_PO_OCAmt = $('#PO_OtherCharges').val();
    //if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
    //    if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {
    //        Calculate_OC_AmountItemWise(TotalOCAmt);
    //    }
    //}
    var TotalOCAmt = $('#_OtherChargeTotalAmt').text();

    if ($("#DocumentMenuId").val() == "105101130") {
        TotalOCAmt = $('#_OtherChargeTotalAmt').text();
    }
    else {
        TotalOCAmt = $('#_OtherChargeTotal').text();
    }
    var Total_PO_OCAmt = $('#PO_OtherCharges').val();
    if (parseFloat(TotalOCAmt) > 0 && parseFloat(Total_PO_OCAmt) > 0) {
        /* if (parseFloat(TotalOCAmt) === parseFloat(Total_PO_OCAmt)) {*/
        Calculate_OC_AmountItemWise(TotalOCAmt);
        /* }*/
    }
}
function OnChangeAssessAmt(e) {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    var errorFlag = "N";
    //var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var AssessAmt = parseFloat(0).toFixed(ValDecDigit);
    ItmCode = clickedrow.find("#POItemListName" + Sno).val();
    AssessAmt = clickedrow.find("#item_ass_val").val();
    if (AvoidDot(AssessAmt) == false) {
        clickedrow.find("#item_ass_valError").text($("#valueReq").text());
        clickedrow.find("#item_ass_valError").css("display", "block");
        clickedrow.find("#item_ass_val").css("border-color", "red");
        errorFlag = "Y";
    }
    if (CheckItemRowValidation(e) == false) {
        errorFlag = "Y";
    }
    if (errorFlag == "Y") {
        clickedrow.find("#item_ass_val").val("");
        AssessAmt = 0;
        //return false;
    }
    if (parseFloat(AssessAmt) > 0) {
        if (docid != '105101140101') {
            $("#BtnTxtCalculation").prop("disabled", false);
        }
    }
    else {
        $("#BtnTxtCalculation").prop("disabled", true);
    }
    if (parseFloat(AssessAmt).toFixed(ValDecDigit) > 0 && ItmCode != "" && Sno != null && Sno != "") {
        //debugger;        
        clickedrow.find("#item_ass_val").val(parseFloat(AssessAmt).toFixed(ValDecDigit));
        CalculateTaxAmount_ItemWise(Sno, ItmCode, parseFloat(AssessAmt).toFixed(ValDecDigit));
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
function OtherFunctions(StatusC, StatusName) {
    window.location.reload();
}
function OnChangeValidUpToDate(ValidUpToDate) {
    //debugger;
    var ValidUpTo = ValidUpToDate.value;
    var PODate = $("#po_date").val();
    try {
        if (PODate > ValidUpTo) {
            $("#ValidUptoDate").val("")
            $('#SpanValidUpToErrorMsg').text($("#VDateCNBGTPODate").text());
            $("#SpanValidUpToErrorMsg").css("display", "block");
            return false;
        }
        else {
            $("#SpanValidUpToErrorMsg").css("display", "none");
            $("#ValidUptoDate").css("border-color", "#ced4da");
        }

    } catch (err) {

    }
}
function OnChangeSourType() {
    var SourceType = $("#src_type").val();
    var docid = $("#DocumentMenuId").val();
    debugger
    if (SourceType == "D" || SourceType == "0") {
        $("#src_doc_number").attr('disabled', true);
        $("#SupplierName").attr('disabled', false);
        $("#SourceDocNo").prop('hidden', true);
        $("#Rqrdrate,#Rqrdcurr,#Rqrdaddr,#Rqrdsupp").css("display", "");

        $("#SupplierName").val(0).trigger('change');
        $("#src_doc_number").append(`<option value="0" selected>---Select---</option>`);
        $("#src_doc_date").val(0);
        $("#PoItmDetailsTbl thead tr .required").css("display", "");
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
        $("#src_doc_number").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da");
        $("#src_doc_number").parent().css("display", "none");
        $("#src_doc_date").parent().css("display", "none");
        $("#PoItmDetailsTbl >tbody >tr").remove();
        //AddNewRowForEditPOItem();
        $("#div_ReqArea").css("display", "none");
        $("#div_replicateWith").css("display", "");
    }
    else if (SourceType == "Q") {
        $("#src_doc_number").attr('disabled', true);
        //$("#SupplierName").attr('disabled', true);
        $("#SourceDocNo").prop('hidden', false);
        $("#SupplierName").attr("onchange", "");
        var Pending_Flag = $("#Pending_SourceDocumentLink").val();
        if (Pending_Flag == "CreateDocument_Pending") {
            var supp_id = $("#Pending_Supp_id").val()
            $("#SupplierName").val(supp_id).trigger('change');
        }
        else {
            $("#SupplierName").val(0).trigger('change');
        }

        $("#SupplierName").attr("onchange", "OnChangeSuppName(this)");
        $("#Address").val("");
        $("#conv_rate").val("");
        $("#ddlCurrency").val("");
        $("#PoItmDetailsTbl >tbody >tr").remove();
        AddNewRowForEditPOItem('');
        //BindSrcDocNumberOnBehalfSrcType(SourceType);
        DisableItemDetailsPO();
        $("#BtnAddItem").closest('div').css("display", "none");
        $("#PoItmDetailsTbl thead tr .required").css("display", "none");
        $("#src_doc_number").parent().css("display", "block");
        $("#src_doc_date").parent().css("display", "block");
        $("#div_ReqArea").css("display", "none");
        //$("#Rqrdrate,#Rqrdcurr,#Rqrdaddr,#Rqrdsupp").css("display", "none");
        $("#div_replicateWith").css("display", "none");
    }
    else if (SourceType == 'PR') {
        $("#src_doc_number").attr('disabled', true);
        $("#ddlRequiredArea").attr('disabled', true);
        $("#SourceDocNo").prop('hidden', false);
        $("#SupplierName").attr("onchange", "");
        $("#SupplierName").val(0).trigger('change');
        $("#SupplierName").attr("onchange", "OnChangeSuppName(this)");
        $("#Address").val("");
        $("#conv_rate").val("");
        $("#ddlCurrency").val("");
        $("#PoItmDetailsTbl >tbody >tr").remove();
        AddNewRowForEditPOItem('');
        //BindSrcDocNumberOnBehalfSrcType(SourceType);//Commented by Shubham on 25-10-2024
        if (docid == "105101136") {
            BindSrcDocNumberOnBehalfSrcType('PRConsum', "CM");
        }
        else if ($("#ddlAll").is(":checked")) {//Added by Shubham on 25-10-2024
            BindSrcDocNumberOnBehalfSrcType(SourceType, "All");
        }
        else {
            BindSrcDocNumberOnBehalfSrcType(SourceType);
        }
        DisableItemDetailsPO();
        $("#BtnAddItem").closest('div').css("display", "none");
        // $("#PoItmDetailsTbl thead tr .required").css("display", "none");
        $("#Rqrdrate,#Rqrdcurr,#Rqrdaddr,#Rqrdsupp").css("display", "");
        $("#src_doc_number").parent().css("display", "block");
        $("#src_doc_date").parent().css("display", "block");
        $("#div_ReqArea").css("display", "block");
        $("#div_replicateWith").css("display", "none");
    }
    else {
        $("#src_doc_number").attr('disabled', false);
        $("#SupplierName").attr('disabled', false);
        $("#SourceDocNo").prop('hidden', false);
        $("#div_ReqArea").css("display", "none");
        $("#div_replicateWith").css("display", "none");
    }
}
function BindSrcDocNumberOnBehalfSrcType(SourceType, ItemType, ddlRequiredArea) {
    debugger;
    SuppID = $("#SupplierName").val();
    if (SourceType != null && SourceType != "") {
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/DPO/GetSourceDocList',
            data: { Flag: SourceType, SuppID: SuppID, ItemType: ItemType, RequiredArea: ddlRequiredArea },
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
                        if (SourceType == "PR") {
                            $("#src_doc_number option").remove();
                            $("#src_doc_number optgroup").remove();
                            $('#src_doc_number').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}' data-value='${$("#span_RequirementArea").text()}'></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#Textddl').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}" data-value="${arr.Table[i].req_area}" data-id="${arr.Table[i].Req_area_id}">${arr.Table[i].doc_no}</option>`);
                            }
                            var firstEmptySelect = true;
                            $('#src_doc_number').select2({
                                templateResult: function (data) {
                                    var DocDate = $(data.element).data('date');
                                    var ReqArea = $(data.element).data('value');
                                    var classAttr = $(data.element).attr('class');
                                    var hasClass = typeof classAttr != 'undefined';
                                    classAttr = hasClass ? ' ' + classAttr : '';
                                    var $result = $(
                                        '<div class="row">' +
                                        '<div class="col-md-4 col-xs-4' + classAttr + '">' + data.text + '</div>' +
                                        '<div class="col-md-4 col-xs-4' + classAttr + '">' + DocDate + '</div>' +
                                        '<div class="col-md-4 col-xs-4' + classAttr + '">' + ReqArea + '</div>' +
                                        '</div>'
                                    );
                                    return $result;
                                    firstEmptySelect = false;
                                }
                            });
                            var Pending_Flag = $("#Pending_SourceDocumentLink").val();
                            debugger
                            if (Pending_Flag == "CreateDocument_Pending") {
                                var src_doc_no = $("#Pending_Src_DocNo").val()
                                $("#src_doc_number").val(src_doc_no).trigger('change');
                            }
                            else {
                                $("#src_doc_date").val("");
                            }
                        }
                        else {
                            $("#src_doc_number option").remove();
                            $("#src_doc_number optgroup").remove();
                            $('#src_doc_number').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                            for (var i = 0; i < arr.Table.length; i++) {
                                $('#Textddl').append(`<option data-date="${arr.Table[i].doc_dt}" value="${arr.Table[i].doc_no}">${arr.Table[i].doc_no}</option>`);
                            }
                            var firstEmptySelect = true;
                            $('#src_doc_number').select2({
                                templateResult: function (data) {
                                    var DocDate = $(data.element).data('date');
                                    var classAttr = $(data.element).attr('class');
                                    var hasClass = typeof classAttr != 'undefined';
                                    classAttr = hasClass ? ' ' + classAttr : '';
                                    var $result = $(
                                        '<div class="row">' +
                                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                                        '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                                        '</div>'
                                    );
                                    return $result;
                                    firstEmptySelect = false;
                                }
                            });
                            var Pending_Flag = $("#Pending_SourceDocumentLink").val();
                            debugger
                            if (Pending_Flag == "CreateDocument_Pending") {
                                var src_doc_no = $("#Pending_Src_DocNo").val()
                                $("#src_doc_number").val(src_doc_no).trigger('change');
                            }
                            else {
                                $("#src_doc_date").val("");
                            }
                        }
                    }
                }
            }

        })
    }
}
function OnchangeSrcDocNumber() {
    var docid = $("#DocumentMenuId").val();
    var doc_no = $("#src_doc_number").val();
    var ReplicateType="";
    var Src_Type = $("#src_type").val();
   
    //var QtyDecDigit = $("#QtyDigit").text();///Quantity
    debugger
    var newdate;
    var doc_Date;
    if (Src_Type == "D") {
        //  Doc_no = $("#ddlReplicateWith").val();
        //doc_Date = $("#ddlReplicateWith option:selected")[0].dataset.date
        //newdate = doc_Date.split("-").reverse().join("-");

        var replicate = $("#ddlReplicateWith").val();
        doc_no = replicate.split(",")[0];
        var so_dt1 = replicate.split(",")[1]

        var date = so_dt1.split("-");
        newdate = date[2] + '-' + date[1] + '-' + date[0];

        
        $('#PoItmDetailsTbl >tbody >tr').remove();
        $("#ddlReplicateWith").attr("disabled", true);
        $("#SupplierName").attr("disabled", true);

        Src_Type = "Replicate";


    }
    if (doc_no != 0 && doc_no != "---Select---") {
        var Status = $("#hfStatus").val();
        if (Src_Type != "Replicate") {
            if (Status == "" || Status == null) {
                doc_Date = $("#src_doc_number option:selected")[0].dataset.date
                newdate = doc_Date.split("-").reverse().join("-");
            }
            else {
                /*var doc_dt = $("#src_doc_date").val();*/
                //var date = doc_dt.split("-");
                //var newdate = date[2] + '-' + date[1] + '-' + date[0];
                newdate = $("#src_doc_date").val();
            }

            $("#src_doc_date").val(newdate);
            $("#src_doc_number").val(doc_no);
            $("#Hdn_src_doc_number").val(doc_no);

            if (Src_Type == "Q") {
                //$("#SourceDocNo").prop('hidden', true);
                $("#PoItmDetailsTbl thead tr .required").css("display", "none");
                $("#Rqrdrate,#Rqrdcurr,#Rqrdaddr,#Rqrdsupp").css("display", "none");
            }

            if (docid == "105101136") {
                Src_Type = "PRCon";
            }
        }
        
       
        var Ordertype = "";
        var OrderImport = $("#OrderTypeImport").is(":checked");
        if (OrderImport == true) {
            Ordertype = "I";
        }
        else {
            Ordertype = "D";
        }
       
        $.ajax({
            type: 'POST',
            url: '/ApplicationLayer/DPO/GetDetailsAgainstQuotationOrPR',
            data: {
                Doc_no: doc_no,
                Doc_date: newdate,
                Src_Type: Src_Type,
                Ordertype: Ordertype,
               
            },
            success: function (data) {
                debugger;
                var arr = JSON.parse(data);
                if (docid == "105101136" && Src_Type != "Replicate") {
                    Src_Type = "PR";

                }
                if (Src_Type == "Q" || Src_Type == "PR") {
                    if ($("#src_doc_number").val() == "0" || $("#src_doc_number").val() == "---Select---") {
                        $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
                        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red")
                        $("#SpanSourceDocNoErrorMsg").css("display", "block");
                        ErrorFlag = "Y";
                    }
                    else {
                        $("#SpanSourceDocNoErrorMsg").css("display", "none");
                        $("#src_doc_number").css("border-color", "#ced4da");
                        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da")
                    }

                }
                if (Src_Type == "Q" ) {
                    BindDataInCaseOfQuotation(arr);
                    DisableAfterSave();
                    EnableForQtsnAndPREdit();
                    debugger
                    if (arr.Table6.length > 0) {
                        for (var y = 0; y < arr.Table6.length; y++) {

                            var ItmId = arr.Table6[y].item_id;
                            var SubItmId = arr.Table6[y].sub_item_id;
                            var SubItmName = arr.Table6[y].sub_item_name;
                            //var AvlStk = arr.Table2[y].avl_stk;
                            var Qty = arr.Table6[y].Qty;

                            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            
                            <td><input type="text" id="subItemQty" value='${Qty}'></td>
                            
                            </tr>`);
                            /*<td><input type="text" id="subItemName" value='${SubItmName}'></td>*/
                            /*<td><input type="text" id="subItemAvlStk" value='${AvlStk}'></td>*/
                        }

                    }
                }
                if (Src_Type == "PR") {
                    var req_area = $('#ddlRequiredArea').val();
                    if (req_area == "0") {
                        var req_area_id = $("#src_doc_number option:selected")[0].dataset.id;
                        $("#HdnReqArea").val("Y");
                        $("#ddlRequiredArea").val(req_area_id).trigger('change');
                    }
                    $("#src_type").attr("disabled", true);
                    $("#PoItmDetailsTbl >tbody >tr").remove();

                    if (arr.Table.length > 0) {

                        for (var a = 0; a < arr.Table.length; a++) {
                            var subitem = arr.Table[0].sub_item;
                            AddNewRowForEditPOItem(subitem);

                        }
                    }

                    if ($("#PoItmDetailsTbl >tbody >tr").length == arr.Table.length) {
                        var PRCons = 'N';//Shubham 23-10-2024
                        $("#PoItmDetailsTbl >tbody >tr").each(function () {
                            var currentRow = $(this);
                            var SNo = currentRow.find("#SNohiddenfiled").val();
                            var DataSno;
                            DataSno = parseInt(SNo) - 1;
                            debugger;
                            currentRow.find("#POItemListName" + SNo).append(`<option value="${arr.Table[DataSno].item_id}" selected>${arr.Table[DataSno].item_name}</option>`);//.val(arr.Table[DataSno].item_id).trigger('change');
                            currentRow.find("#hfItemID").val(arr.Table[DataSno].item_id);
                            //currentRow.find("#UOM").val(arr.Table[DataSno].uom_name);
                            if (arr.Table[DataSno].ItemType == "Service") {
                                if (arr.Table[DataSno].uom_id == "0") {
                                    currentRow.find("#UOM").html(`<option value="${0}">${'---Select---'}</option>`);
                                }
                                else {
                                    currentRow.find("#UOM").html(`<option value="${arr.Table[DataSno].uom_id}">${arr.Table[DataSno].uom_name}</option>`);
                                }
                            }
                            else {
                                currentRow.find("#UOM").html(`<option value="${arr.Table[DataSno].uom_id}">${arr.Table[DataSno].uom_name}</option>`);
                            }

                            currentRow.find("#UOMID").val(arr.Table[DataSno].uom_id);
                            currentRow.find("#ItemHsnCode").val(arr.Table[DataSno].hsn_code);
                            currentRow.find("#ItemtaxTmplt").val(arr.Table[DataSno].tmplt_id);
                            currentRow.find("#hdnItemtype").val(arr.Table[DataSno].ItemType);
                            if (docid != "105101136") {
                                currentRow.find("#IDItemtype").val(arr.Table[DataSno].ItemType);
                            }
                            currentRow.find("#AvailableStockInBase").val(parseFloat(arr.Table[DataSno].Avlstock).toFixed(QtyDecDigit));
                            currentRow.find("#ord_qty_spec").val(parseFloat(arr.Table[DataSno].pr_qty).toFixed(QtyDecDigit));
                            currentRow.find("#ord_qty_base").val(parseFloat(arr.Table[DataSno].pr_qty).toFixed(QtyDecDigit));
                            if (docid != "105101136") {
                                if (arr.Table[DataSno].ItemType == "Consumable") {//Shubham 23-10-2024
                                    PRCons = "Y";
                                }
                            }
                            HideShowPageWise(arr.Table[DataSno].sub_item, currentRow);
                            if (docid != '105101140101') {
                                currentRow.find("#BtnTxtCalculation").attr("disabled", false);
                            }
                            var k = DataSno;
                            currentRow.find("#remarks").val(arr.Table[DataSno].it_remarks);
                            var Itm_ID = arr.Table[k].item_id;
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            var docid = $("#DocumentMenuId").val();
                            if (docid == '105101130' || docid == '105101136') {
                                if (GstApplicable == "Y") {
                                    $("#HdnTaxOn").val("Item");
                                    Cmn_ApplyGSTToAtable("PoItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val", arr.Table1);
                                }
                                else {
                                    $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                                    if (arr.Table[k].tmplt_id != 0 && arr.Table1.length > 0) {
                                        $("#HdnTaxOn").val("Item");
                                        $("#TaxCalcItemCode").val(Itm_ID);
                                        $("#HiddenRowSNo").val(SNo);
                                        $("#Tax_AssessableValue").val(arr.Table[k].item_gross_val);
                                        $("#TaxCalcGRNNo").val(arr.Table[k].mr_no);
                                        $("#TaxCalcGRNDate").val(arr.Table[k].mr_date);
                                        var TaxArr = arr.Table1;
                                        let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                                        selected = JSON.stringify(selected);
                                        TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                                        selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                                        selected = JSON.stringify(selected);
                                        TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                                        if (TaxArr.length > 0) {
                                            AddTaxByHSNCalculation(TaxArr);
                                            OnClickSaveAndExit();
                                            var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                                            Reset_ReOpen_LevelVal(lastLevel);
                                        }
                                    }

                                }
                            }

                        });
                        if (PRCons == "Y") {//Shubham 23-10-2024
                            $("#ddlAll").prop("checked", true);
                        }
                        CalculateAmount();
                        OnChangeGrossAmt();
                    }
                    debugger
                    //if (arr.Table2.length > 0) {
                    //    for (var y = 0; y < arr.Table2.length; y++) {

                    //        var ItmId = arr.Table2[y].item_id;
                    //        var SubItmId = arr.Table2[y].sub_item_id;
                    //        var SubItmName = arr.Table2[y].sub_item_name;
                    //        var Qty = arr.Table2[y].Qty;

                    //        $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                    //        <td><input type="text" id="ItemId" value='${ItmId}'></td>
                    //        <td><input type="text" id="subItemName" value='${SubItmName}'></td>
                    //        <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                    //        <td><input type="text" id="subItemQty" value='${Qty}'></td>

                    //        </tr>`);
                    //    }

                    //}
                    //$("#SupplierName").prop("disabled", false);
                    //$("#ddlCurrency").prop("disabled", false);
                    //$("#conv_rate").prop("disabled", false);
                    $("#src_doc_number").prop("disabled", true);
                    $("#ddlRequiredArea").prop("disabled", true);
                    DisableHeaderField();
                    $("#PoItmDetailsTbl .plus_icon1").css("display", "none");                 
                    debugger;
                    $("#PoItmDetailsTbl >tbody >tr").each(function () {
                        var currentRow = $(this);
                        var Sno = currentRow.find("#SNohiddenfiled").val();
                        debugger;
                        // Disable the PO Item input
                        currentRow.find("#POItemListName" + Sno).prop("disabled", true);
                       
                        var Count = 0;
                    
                        if (docid === '105101130' && Count === 0)
                        {
                            const isStockableChecked = $("#ddlstockable").is(":checked");
                            const itemType = currentRow.find("#IDItemtype").val();

                            if (isStockableChecked && (itemType === "Service" || itemType === "Consumable"))
                            {
                                $("#ddlAll").prop("checked", true);
                                Count = 1;
                            }
                        }
                      
                    });

                }
                if (Src_Type == "Replicate") {
                    BindDataInCaseOfReplicate(arr);
                  //  DisableAfterSave();
                   // EnableForQtsnAndPREdit();
                    debugger
                    if (arr.Table6.length > 0) {
                        for (var y = 0; y < arr.Table6.length; y++) {

                            var ItmId = arr.Table6[y].item_id;
                            var SubItmId = arr.Table6[y].sub_item_id;
                            var SubItmName = arr.Table6[y].sub_item_name;
                            //var AvlStk = arr.Table2[y].avl_stk;
                            var Qty = arr.Table6[y].Qty;

                            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${ItmId}'></td>
                            
                            <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                            
                            <td><input type="text" id="subItemQty" value='${Qty}'></td>
                            
                            </tr>`);                          
                        }

                    }
                }
                debugger;
               // BindDelivertSchItemList();
            }
        });
    }
}
function OnchangePriceBasis() {
    if ($("#PriceBasis").val() == "") {
        $('#SpanPriceBasisErrorMsg').text($("#valueReq").text());
        $("#SpanPriceBasisErrorMsg").css("display", "block");
        $("#PriceBasis").css("border-color", "Red");
    }
    else {
        $("#SpanPriceBasisErrorMsg").css("display", "none");
        $("#PriceBasis").css("border-color", "#ced4da");
    }
}
function OnchangeFreightType() {
    if ($("#FreightType").val() == "") {
        $('#SpanFreightTypeErrorMsg').text($("#valueReq").text());
        $("#SpanFreightTypeErrorMsg").css("display", "block");
        $("#FreightType").css("border-color", "Red");

    }
    else {
        $("#SpanFreightTypeErrorMsg").css("display", "none");
        $("#FreightType").css("border-color", "#ced4da");
    }
}
function OnchangeModeOfTransport() {
    if ($("#ModeOfTransport").val() == "") {
        $('#SpanModeOfTransportErrorMsg').text($("#valueReq").text());
        $("#SpanModeOfTransportErrorMsg").css("display", "block");
        $("#ModeOfTransport").css("border-color", "Red");

    }
    else {
        $("#SpanModeOfTransportErrorMsg").css("display", "none");
        $("#ModeOfTransport").css("border-color", "#ced4da");
    }
}
function OnchangeDestination() {
    if ($("#Destination").val() == "") {
        $('#SpanDestinationErrorMsg').text($("#valueReq").text());
        $("#SpanDestinationErrorMsg").css("display", "block");
        $("#Destination").css("border-color", "Red");
    }
    else {
        $("#SpanDestinationErrorMsg").css("display", "none");
        $("#Destination").css("border-color", "#ced4da");
    }
}
function BindDataInCaseOfQuotation(arr) {
    //var QtyDecDigit = $("#QtyDigit").text();///Quantity
    //var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    //var ValDecDigit = $("#ValDigit").text();///Amount
    var docid = $("#DocumentMenuId").val();

    $("#PoItmDetailsTbl >tbody >tr").remove();
    if (arr.Table.length > 0) {

        for (var a = 0; a < arr.Table.length; a++) {
            debugger;
            var subitem = arr.Table[0].sub_item;
            AddNewRowForEditPOItem(subitem);

        }
    }
    if ($("#PoItmDetailsTbl >tbody >tr").length == arr.Table.length) {
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var SNo = currentRow.find("#SNohiddenfiled").val();
            var DataSno;
            DataSno = parseInt(SNo) - 1;
            debugger;
            currentRow.find("#POItemListName" + SNo).append(`<option value="${arr.Table[DataSno].item_id}" selected>${arr.Table[DataSno].item_name}</option>`);//.val(arr.Table[DataSno].item_id).trigger('change');
            currentRow.find("#hfItemID").val(arr.Table[DataSno].item_id);
            /* start Code by Hina on 20-01-2023 to add sub itm colmn for chk src document*/
            currentRow.find("#sub_item").val(arr.Table[DataSno].sub_item);
            var subitm = currentRow.find("#sub_item").val();
            if (subitm == "Y") {
                currentRow.find('#SubItemPOQTOrdQty').attr("disabled", false);

            }
            else {
                currentRow.find('#SubItemPOQTOrdQty').attr('disabled', true);

            }
            /* start Code by Hina on 20-01-2023 to add sub itm colmn for chk src document*/
            //currentRow.find("#UOM").val(arr.Table[DataSno].uom_name);
            currentRow.find("#UOM").html(`<option value='${arr.Table[DataSno].uom_id}'>${arr.Table[DataSno].uom_name}</option>`).trigger('change');
            currentRow.find("#UOMID").val(arr.Table[DataSno].uom_id);
            //clickedrow.find("#UOM").trigger('change');
            currentRow.find("#ItemHsnCode").val(arr.Table[DataSno].hsn_code);
            currentRow.find("#ItemtaxTmplt").val(arr.Table[DataSno].tmplt_id);
            currentRow.find("#hdnItemtype").val(arr.Table[DataSno].ItemType);
            currentRow.find("#IDItemtype").val(arr.Table[DataSno].ItemType);
            currentRow.find("#AvailableStockInBase").val(parseFloat(arr.Table[DataSno].Avlstock).toFixed(QtyDecDigit));
            currentRow.find("#ord_qty_spec").val(parseFloat(arr.Table[DataSno].qt_qty).toFixed(QtyDecDigit));
            currentRow.find("#ord_qty_base").val(parseFloat(arr.Table[DataSno].qt_qty1).toFixed(QtyDecDigit));
            currentRow.find("#item_rate").val(parseFloat(arr.Table[DataSno].item_rate).toFixed(RateDecDigit)).trigger('change');
            currentRow.find("#item_disc_perc").val(parseFloat(arr.Table[DataSno].item_disc_perc).toFixed(cmn_PerDecDigit));
            currentRow.find("#item_disc_amt").val(parseFloat(arr.Table[DataSno].item_disc_amt).toFixed(ValDecDigit));
            currentRow.find("#item_disc_val").val(parseFloat(arr.Table[DataSno].item_disc_val).toFixed(ValDecDigit));
            currentRow.find("#item_gr_val").val(parseFloat(arr.Table[DataSno].item_gr_val).toFixed(ValDecDigit));
            currentRow.find("#item_ass_val").val(parseFloat(arr.Table[DataSno].item_ass_val).toFixed(ValDecDigit));//
            //currentRow.find("#item_tax_amt").val(parseFloat(arr.Table[DataSno].item_tax_amt).toFixed(ValDecDigit));
            currentRow.find("#item_oc_amt").val(parseFloat(arr.Table[DataSno].item_oc_amt).toFixed(ValDecDigit));
            currentRow.find("#item_net_val_spec").val(parseFloat(arr.Table[DataSno].item_net_val_spec).toFixed(ValDecDigit));
            currentRow.find("#item_net_val_bs").val(parseFloat(arr.Table[DataSno].item_net_val_bs).toFixed(ValDecDigit));
            HideShowPageWise(arr.Table[DataSno].sub_item, currentRow);
            if (arr.Table[DataSno].tax_expted == "Y") {
                currentRow.find('#TaxExempted').prop("checked", true);
            }
            else {
                currentRow.find('#TaxExempted').prop("checked", false);
            }
            var GstApplicable = $("#Hdn_GstApplicable").text();
            //if (GstApplicable == "Y") {
            //    if (arr.Table[DataSno].manual_gst == "Y") {
            //        currentRow.find('#ManualGST').prop("checked", true);
            //    }
            //    else {
            //        currentRow.find('#ManualGST').prop("checked", false);
            //    }
            //}
            if (docid != '105101140101') {
                currentRow.find("#BtnTxtCalculation").attr("disabled", false);
                var Itm_ID = arr.Table[DataSno].item_id
                if (GstApplicable == "Y") {
                    $("#HdnTaxOn").val("Item");
                    Cmn_ApplyGSTToAtable("PoItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val", arr.Table5);
                }
                else {
                    var k = DataSno;
                    $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                    if (arr.Table[k].tmplt_id != 0 && arr.Table5.length > 0) {
                        $("#HdnTaxOn").val("Item");
                        $("#TaxCalcItemCode").val(Itm_ID);
                        $("#HiddenRowSNo").val(SNo);
                        $("#Tax_AssessableValue").val(arr.Table[k].item_ass_val);
                        //$("#TaxCalcGRNNo").val(arr.Table[k].mr_no);
                        //$("#TaxCalcGRNDate").val(arr.Table[k].mr_date);
                        var TaxArr = arr.Table5;
                        let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                        selected = JSON.stringify(selected);
                        TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                        selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                        selected = JSON.stringify(selected);
                        TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                        if (TaxArr.length > 0) {
                            AddTaxByHSNCalculation(TaxArr);
                            OnClickSaveAndExit("Y");
                            var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                            Reset_ReOpen_LevelVal(lastLevel);
                        }
                    }

                }


            }
            currentRow.find("#remarks").val(arr.Table[DataSno].it_remarks);

        });
        CalculateAmount();
        OnChangeGrossAmt();
    }
    /*code start Add by Hina sharma on 18-06-2025 */
    if (arr.Table2.length > 0) {
        GetOCWithOCTaxDetailByQTNumber(arr)
        OnClickSaveAndExit_OC_Btn();
        CalculateAmount();

    }
    if (arr.Table4.length > 0) {
        $('#TblTerms_Condition tbody tr').remove();
        var rowIdx = 0;
        var deletetext = $("#Span_Delete_Title").text();
        
        for (var y = 0; y < arr.Table4.length; y++) {
            $('#TblTerms_Condition tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
                            <td class="red"><i id="TC_DelIcon" class="deleteIcon fa fa-trash" title="${deletetext}"></i></td>
                            <td id="term_desc">${arr.Table4[y].term_desc}</td>
                            </tr>`);
        }
    }
    if (arr.Table3.length > 0) {
        var rowIdx = 0;
        for (var x = 0; x < arr.Table3.length; x++) {
            Cmn_appendDeleveryschedule(++rowIdx, arr.Table3[x].item_name, arr.Table3[x].item_id, arr.Table3[x].sch_date, arr.Table3[x].delv_qty, 0, arr.Table3[x].delv_qty, 0);
        }
    }
    
    /*code end Add by Hina sharma on 18-06-2025 */
    var UserID = $("#UserID").text();
    //    if (arr.Table2.length > 0) {
    //        var rowIdx = 0;
    //        var TaxCalculationList = [];
    //        for (var j = 0; j < arr.Table2.length; j++) {
    //            var RowSNo;
    //            $("#PoItmDetailsTbl >tbody >tr").each(function () {
    //                ////debugger;
    //                var currentRow = $(this);
    //                var SNo = currentRow.find("#SNohiddenfiled").val();
    //                var ItmCode = "";

    //                ItmCode = currentRow.find('#POItemListName').val();

    //                if (ItmCode == arr.Table2[j].item_id) {
    //                    RowSNo = SNo;
    //                }
    //            });
    //            var TaxItmCode = arr.Table2[j].item_id;
    //            var TaxName = arr.Table2[j].tax_name;
    //            var TaxNameID = arr.Table2[j].tax_id;
    //            var TaxPercentage = arr.Table2[j].tax_rate + "%";
    //            var TaxLevel = arr.Table2[j].tax_level;
    //            var TaxApplyOn = arr.Table2[j].tax_apply_Name;
    //            var TaxAmount = arr.Table2[j].tax_val;
    //            var TotalTaxAmount = arr.Table2[j].item_tax_amt;
    //            var TaxApplyOnID = arr.Table2[j].tax_apply_on;
    //            $("#Hdn_TaxCalculatorTbl > tbody").append(`
    //                                <tr>

    //                                    <td id="TaxItmCode">${TaxItmCode}</td>
    //                                    <td id="TaxName">${TaxName}</td>
    //                                    <td id="TaxNameID">${TaxNameID}</td>
    //                                    <td id="TaxPercentage">${TaxPercentage}</td>
    //                                    <td id="TaxLevel">${TaxLevel}</td>
    //                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
    //                                    <td id="TaxAmount">${TaxAmount}</td>
    //                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
    //                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
    //                                </tr>`);
    //            TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    //        }
    //        ////debugger;
    //        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
    //        BindTaxAmountDeatils(TaxCalculationList);
    //    }
    //    if (arr.Table3.length > 0) {
    //        var rowIdx = 0;
    //        var deletetext = $("#Span_Delete_Title").text();
    //        for (var k = 0; k < arr.Table3.length; k++) {

    //            ODTableDataBind(rowIdx, arr.Table3[k].oc_name, arr.Table3[k].curr_name, arr.Table3[k].conv_rate, arr.Table3[k].oc_id, arr.Table3[k].oc_val, arr.Table3[k].OCValBs)

    //            $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
    //                <td id="OC_name">${arr.Table3[k].oc_name}</td>
    //                <td id="OC_Curr">${arr.Table3[k].curr_name}</td>
    //                <td id="OC_Conv">${parseFloat(arr.Table3[k].conv_rate).toFixed(RateDecDigit)}</td>
    //                <td hidden="hidden" id="OC_ID">${arr.Table3[k].oc_id}</td>
    //                <td align="right" id="OCAmtBs">${parseFloat(arr.Table3[k].oc_val).toFixed(ValDecDigit)}</td>
    //<td align="right" id="OCAmtSp">${parseFloat(arr.Table3[k].OCValBs).toFixed(ValDecDigit)}</td>
    //                </tr>`);
    //        }
    //        $("#Total_OC_Amount").text(parseFloat(arr.Table[0].oc_amt).toFixed(ValDecDigit));
    //        BindOtherChargeDeatils();
    //    }
    //    else {
    //        $('#Tbl_OC_Deatils tbody tr').remove();
    //        $('#ht_Tbl_OC_Deatils tbody tr').remove();
    //        $("#PO_OtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
    //        BindOtherChargeDeatils();
    //    }

    //if (arr.Table4.length > 0) {
    //    var rowIdx = 0;
    //    for (var x = 0; x < arr.Table4.length; x++) {
    //        //appendDeleveryschedule(++rowIdx, arr.Table4[x].item_name, arr.Table4[x].item_id, arr.Table4[x].sch_date, arr.Table4[x].delv_qty)
    //        Cmn_appendDeleveryschedule(++rowIdx, arr.Table4[x].item_name, arr.Table4[x].item_id, arr.Table4[x].sch_date, arr.Table4[x].delv_qty,0,0,0);
    //    }
    //}
    //if (arr.Table5.length > 0) {
    //    var rowIdx = 0;
    //    for (var y = 0; y < arr.Table5.length; y++) {
    //        appendTermsAndCondition(++rowIdx, arr.Table5[y].term_desc);
    //    }
    //}
}
function BindDataInCaseOfReplicate(arr) {
  
    var docid = $("#DocumentMenuId").val();

    $("#PoItmDetailsTbl >tbody >tr").remove();
    if (arr.Table.length > 0) {

        for (var a = 0; a < arr.Table.length; a++) {
            debugger;
            var subitem = arr.Table[0].sub_item;
            AddNewRowForEditPOItem(subitem);

        }
    }
    if ($("#PoItmDetailsTbl >tbody >tr").length == arr.Table.length) {
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var SNo = currentRow.find("#SNohiddenfiled").val();
            var DataSno;
            DataSno = parseInt(SNo) - 1;
            debugger;
            currentRow.find("#POItemListName" + SNo).append(`<option value="${arr.Table[DataSno].item_id}" selected>${arr.Table[DataSno].item_name}</option>`);//.val(arr.Table[DataSno].item_id).trigger('change');
            currentRow.find("#hfItemID").val(arr.Table[DataSno].item_id);
           
            currentRow.find("#sub_item").val(arr.Table[DataSno].sub_item);
            var subitm = currentRow.find("#sub_item").val();
            if (subitm == "Y") {
                currentRow.find('#SubItemPOQTOrdQty').attr("disabled", false);

            }
            else {
                currentRow.find('#SubItemPOQTOrdQty').attr('disabled', true);

            }
            
            currentRow.find("#UOM").html(`<option value='${arr.Table[DataSno].uom_id}'>${arr.Table[DataSno].uom_name}</option>`).trigger('change');
            currentRow.find("#UOMID").val(arr.Table[DataSno].uom_id);
           
            currentRow.find("#ItemHsnCode").val(arr.Table[DataSno].hsn_code);
            currentRow.find("#ItemtaxTmplt").val(arr.Table[DataSno].tmplt_id);
            currentRow.find("#hdnItemtype").val(arr.Table[DataSno].ItemType);
            currentRow.find("#IDItemtype").val(arr.Table[DataSno].ItemType);
            currentRow.find("#AvailableStockInBase").val(parseFloat(arr.Table[DataSno].Avlstock).toFixed(QtyDecDigit));
            currentRow.find("#ord_qty_spec").val(parseFloat(arr.Table[DataSno].ord_qty_spec).toFixed(QtyDecDigit));
            currentRow.find("#ord_qty_base").val(parseFloat(arr.Table[DataSno].ord_qty_base).toFixed(QtyDecDigit));
            currentRow.find("#item_rate").val(parseFloat(arr.Table[DataSno].item_rate).toFixed(RateDecDigit)).trigger('change');
            currentRow.find("#item_disc_perc").val(parseFloat(arr.Table[DataSno].item_disc_perc).toFixed(cmn_PerDecDigit));
            currentRow.find("#item_disc_amt").val(parseFloat(arr.Table[DataSno].item_disc_amt).toFixed(ValDecDigit));
            currentRow.find("#item_disc_val").val(parseFloat(arr.Table[DataSno].item_disc_val).toFixed(ValDecDigit));
            currentRow.find("#item_gr_val").val(parseFloat(arr.Table[DataSno].item_gr_val).toFixed(ValDecDigit));
            currentRow.find("#item_ass_val").val(parseFloat(arr.Table[DataSno].item_ass_val).toFixed(ValDecDigit));//
           
            currentRow.find("#item_oc_amt").val(parseFloat(arr.Table[DataSno].item_oc_amt).toFixed(ValDecDigit));
            currentRow.find("#item_net_val_spec").val(parseFloat(arr.Table[DataSno].item_net_val_spec).toFixed(ValDecDigit));
            currentRow.find("#item_net_val_bs").val(parseFloat(arr.Table[DataSno].item_net_val_bs).toFixed(ValDecDigit));

            currentRow.find("#mrp").val(parseFloat(arr.Table[DataSno].mrp).toFixed(ValDecDigit));
            currentRow.find("#PackSize").val(arr.Table[DataSno].pack_size);
            HideShowPageWise(arr.Table[DataSno].sub_item, currentRow);
            if (arr.Table[DataSno].tax_expted == "Y") {
                currentRow.find('#TaxExempted').prop("checked", true);
            }
            else {
                currentRow.find('#TaxExempted').prop("checked", false);
            }
            var GstApplicable = $("#Hdn_GstApplicable").text();
            //if (GstApplicable == "Y") {
            //    if (arr.Table[DataSno].manual_gst == "Y") {
            //        currentRow.find('#ManualGST').prop("checked", true);
            //    }
            //    else {
            //        currentRow.find('#ManualGST').prop("checked", false);
            //    }
            //}
            if (docid != '105101140101') {
                currentRow.find("#BtnTxtCalculation").attr("disabled", false);
                var Itm_ID = arr.Table[DataSno].item_id
                if (GstApplicable == "Y") {
                    $("#HdnTaxOn").val("Item");
                    Cmn_ApplyGSTToAtable("PoItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val", arr.Table5);
                }
                else {
                    var k = DataSno;
                    $("#hd_tax_id").val(arr.Table[k].tmplt_id);
                    if (arr.Table[k].tmplt_id != 0 && arr.Table5.length > 0) {
                        $("#HdnTaxOn").val("Item");
                        $("#TaxCalcItemCode").val(Itm_ID);
                        $("#HiddenRowSNo").val(SNo);
                        $("#Tax_AssessableValue").val(arr.Table[k].item_ass_val);
                        //$("#TaxCalcGRNNo").val(arr.Table[k].mr_no);
                        //$("#TaxCalcGRNDate").val(arr.Table[k].mr_date);
                        var TaxArr = arr.Table5;
                        let selected = []; selected.push({ item_id: arr.Table[k].item_id });
                        selected = JSON.stringify(selected);
                        TaxArr = TaxArr.filter(i => selected.includes(i.item_id));
                        selected = []; selected.push({ tmplt_id: arr.Table[k].tmplt_id });
                        selected = JSON.stringify(selected);
                        TaxArr = TaxArr.filter(i => selected.includes(i.tmplt_id));
                        if (TaxArr.length > 0) {
                            AddTaxByHSNCalculation(TaxArr);
                            OnClickSaveAndExit("Y");
                            var lastLevel = TaxArr[TaxArr.length - 1].tax_level;
                            Reset_ReOpen_LevelVal(lastLevel);
                        }
                    }

                }


            }
            currentRow.find("#remarks").val(arr.Table[DataSno].it_remarks);

        });
        CalculateAmount();
        OnChangeGrossAmt();
    }
    /*code start Add by Hina sharma on 18-06-2025 */
    if (arr.Table2.length > 0) {
        GetOCWithOCTaxDetailByQTNumber(arr)
        OnClickSaveAndExit_OC_Btn();
        CalculateAmount();

    }
    if (arr.Table4.length > 0) {
        $('#TblTerms_Condition tbody tr').remove();
        var rowIdx = 0;
        var deletetext = $("#Span_Delete_Title").text();

        for (var y = 0; y < arr.Table4.length; y++) {
            $('#TblTerms_Condition tbody').append(`<tr id="R${++rowIdx}" class="even pointer">
                            <td class="red"><i id="TC_DelIcon" class="deleteIcon fa fa-trash" title="${deletetext}"></i></td>
                            <td id="term_desc">${arr.Table4[y].term_desc}</td>
                            </tr>`);
        }
    }
    if (arr.Table3.length > 0) {
        var rowIdx = 0;
        for (var x = 0; x < arr.Table3.length; x++) {
            //Cmn_appendDeleveryschedule(++rowIdx, arr.Table3[x].item_name, arr.Table3[x].item_id, arr.Table3[x].sch_date, arr.Table3[x].delv_qty, 0, arr.Table3[x].delv_qty, 0);
            $("#DeliverySchItemDDL").append(
                `<option value="${arr.Table3[x].item_id}">${arr.Table3[x].item_name}</option>`
            );
        }
    }

    /*code end Add by Hina sharma on 18-06-2025 */
    var UserID = $("#UserID").text();
    //    if (arr.Table2.length > 0) {
    //        var rowIdx = 0;
    //        var TaxCalculationList = [];
    //        for (var j = 0; j < arr.Table2.length; j++) {
    //            var RowSNo;
    //            $("#PoItmDetailsTbl >tbody >tr").each(function () {
    //                ////debugger;
    //                var currentRow = $(this);
    //                var SNo = currentRow.find("#SNohiddenfiled").val();
    //                var ItmCode = "";

    //                ItmCode = currentRow.find('#POItemListName').val();

    //                if (ItmCode == arr.Table2[j].item_id) {
    //                    RowSNo = SNo;
    //                }
    //            });
    //            var TaxItmCode = arr.Table2[j].item_id;
    //            var TaxName = arr.Table2[j].tax_name;
    //            var TaxNameID = arr.Table2[j].tax_id;
    //            var TaxPercentage = arr.Table2[j].tax_rate + "%";
    //            var TaxLevel = arr.Table2[j].tax_level;
    //            var TaxApplyOn = arr.Table2[j].tax_apply_Name;
    //            var TaxAmount = arr.Table2[j].tax_val;
    //            var TotalTaxAmount = arr.Table2[j].item_tax_amt;
    //            var TaxApplyOnID = arr.Table2[j].tax_apply_on;
    //            $("#Hdn_TaxCalculatorTbl > tbody").append(`
    //                                <tr>

    //                                    <td id="TaxItmCode">${TaxItmCode}</td>
    //                                    <td id="TaxName">${TaxName}</td>
    //                                    <td id="TaxNameID">${TaxNameID}</td>
    //                                    <td id="TaxPercentage">${TaxPercentage}</td>
    //                                    <td id="TaxLevel">${TaxLevel}</td>
    //                                    <td id="TaxApplyOn">${TaxApplyOn}</td>
    //                                    <td id="TaxAmount">${TaxAmount}</td>
    //                                    <td id="TotalTaxAmount">${TotalTaxAmount}</td>
    //                                    <td id="TaxApplyOnID">${TaxApplyOnID}</td>
    //                                </tr>`);
    //            TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    //        }
    //        ////debugger;
    //        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationList));
    //        BindTaxAmountDeatils(TaxCalculationList);
    //    }
    //    if (arr.Table3.length > 0) {
    //        var rowIdx = 0;
    //        var deletetext = $("#Span_Delete_Title").text();
    //        for (var k = 0; k < arr.Table3.length; k++) {

    //            ODTableDataBind(rowIdx, arr.Table3[k].oc_name, arr.Table3[k].curr_name, arr.Table3[k].conv_rate, arr.Table3[k].oc_id, arr.Table3[k].oc_val, arr.Table3[k].OCValBs)

    //            $('#ht_Tbl_OC_Deatils tbody').append(`<tr>
    //                <td id="OC_name">${arr.Table3[k].oc_name}</td>
    //                <td id="OC_Curr">${arr.Table3[k].curr_name}</td>
    //                <td id="OC_Conv">${parseFloat(arr.Table3[k].conv_rate).toFixed(RateDecDigit)}</td>
    //                <td hidden="hidden" id="OC_ID">${arr.Table3[k].oc_id}</td>
    //                <td align="right" id="OCAmtBs">${parseFloat(arr.Table3[k].oc_val).toFixed(ValDecDigit)}</td>
    //<td align="right" id="OCAmtSp">${parseFloat(arr.Table3[k].OCValBs).toFixed(ValDecDigit)}</td>
    //                </tr>`);
    //        }
    //        $("#Total_OC_Amount").text(parseFloat(arr.Table[0].oc_amt).toFixed(ValDecDigit));
    //        BindOtherChargeDeatils();
    //    }
    //    else {
    //        $('#Tbl_OC_Deatils tbody tr').remove();
    //        $('#ht_Tbl_OC_Deatils tbody tr').remove();
    //        $("#PO_OtherCharges").val(parseFloat(0).toFixed(ValDecDigit));
    //        BindOtherChargeDeatils();
    //    }

    //if (arr.Table4.length > 0) {
    //    var rowIdx = 0;
    //    for (var x = 0; x < arr.Table4.length; x++) {
    //        //appendDeleveryschedule(++rowIdx, arr.Table4[x].item_name, arr.Table4[x].item_id, arr.Table4[x].sch_date, arr.Table4[x].delv_qty)
    //        Cmn_appendDeleveryschedule(++rowIdx, arr.Table4[x].item_name, arr.Table4[x].item_id, arr.Table4[x].sch_date, arr.Table4[x].delv_qty,0,0,0);
    //    }
    //}
    //if (arr.Table5.length > 0) {
    //    var rowIdx = 0;
    //    for (var y = 0; y < arr.Table5.length; y++) {
    //        appendTermsAndCondition(++rowIdx, arr.Table5[y].term_desc);
    //    }
    //}
}
function GetOCWithOCTaxDetailByQTNumber(arr) {
    debugger;

    let Oclen = $("#ht_Tbl_OC_Deatils > tbody>tr").length;
    var CurrencyInBase = "N";
    if (arr.Table2.length > 0) {
        var hdbs_curr = arr.Table2[0].bs_curr_id;
        var hdcurr = $("#ddlCurrency").val();
        if (hdbs_curr == hdcurr) {
            CurrencyInBase = "Y";
        } else {
            CurrencyInBase = "N";
        }
    }
    $("#CurrencyInBase").val(CurrencyInBase);

    if (arr.Table2.length > 0) {
        var rowIdx = 0;
        var deletetext = $("#Span_Delete_Title").text();
        var tax_info = $("#Span_TaxCalculator_Title").text();
        var tdTax = "";
        var disableRoundOff = "disabled";
        var DMenuId = $("#DocumentMenuId").val();
        var disblForJO = "";
        var OcSupp_Name = "";
        var OcSupp_Id = "";
        var oc_supp_type = "";
        var OCSuppAccId = "";
        var displaynoneinDomestic = "";
        var OCAmountsp = "";
        var matchDoc = ("105101130").split(",").includes(DMenuId);
        displaynoneinDomestic = matchDoc ? "style='display:none;'" : "";
        var OC_For = "";
        var OCAccId = "";


        for (var m = 0; m < arr.Table2.length; m++) {

            var OCName = arr.Table2[m].oc_name;
            var OCID = arr.Table2[m].oc_id;
            var OCValue = arr.Table2[m].oc_val;
            var OCCurrId = arr.Table2[m].curr_id;
            var OCCurr = arr.Table2[m].curr_name;
            var OCConv = arr.Table2[m].conv_rate;
            var OcAmtBs = arr.Table2[m].OCValBs;
            //var OCTaxAmt = arr.Table3[m].tax_amt;
            //var OCTaxTotalAmt = arr.Table3[m].total_amt;
            var OCTaxAmt = parseFloat(0).toFixed(cmn_ValDecDigit)
            var OCTaxTotalAmt = arr.Table2[m].OCValBs;

            debugger;
            if (DMenuId == "105101130" && CurrencyInBase == "Y") {
                tdTax = `<td class="num_right">
                    <div class="col-sm-10 lpo_form" id="OCTaxAmt">${parseFloat(CheckNullNumber(OCTaxAmt)).toFixed(cmn_ValDecDigit)}</div>
                    <div class="col-md-2 col-sm-12" style="padding:0px;"><button type="button" id="OCTaxBtnCal" class="calculator" data-toggle="modal" onclick="OnClickOCTaxCalculationBtn(event)" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" title="${tax_info}"></i></button></div>
                </td>
                <td class="num_right" id="OCTotalTaxAmt">${parseFloat(CheckNullNumber(OCTaxTotalAmt)).toFixed(cmn_ValDecDigit)}</td>
                `;
            }

            $('#Tbl_OC_Deatils tbody').append(`<tr id="R${rowIdx}">
<td id="deletetext" class="red center">${`<i class="deleteIcon fa fa-trash" id="OCDelIcon" aria-hidden="true" title="${deletetext}" disabled></i>`}</td>
<td id="OCName" >${OCName}</td>
<td id="OCCurr" class="center" ${disblForJO}>${OCCurr}</td>
<td id="HdnOCCurrId" hidden>${OCCurrId}</td>
<td id="td_OCSuppName" style="display:none">${OcSupp_Name}</td>
<td id="td_OCSuppID" hidden >${OcSupp_Id}</td>
<td id="td_OCSuppType" hidden >${oc_supp_type}</td>
<td id="OCConv" class="num_right" ${disblForJO}>${parseFloat(OCConv).toFixed(cmn_ExchDecDigit)}</td>
<td hidden="hidden" id="OCValue">${OCID}</td>
<td class="num_right" id="OCAmount" ${displaynoneinDomestic}>${parseFloat(OCValue).toFixed(cmn_ValDecDigit)}</td>
<td class="num_right" id="OcAmtBs" ${disblForJO}>${parseFloat(OcAmtBs).toFixed(cmn_ValDecDigit)}</td>
 `+ tdTax + `
<td id="OCAccId" hidden >${OCAccId}</td>
<td id="OCSuppAccId" hidden >${OCSuppAccId}</td>
<td id="OCFor" hidden>${OC_For}</td>
</tr>`);

        }
       /* `+ tdTax + `*/
    }

}
function OnClickHistoryIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";
    var UOMName = "";
    var SuppID = "";
    SuppID = $("#SupplierName").val();
    ItmCode = clickedrow.find("#POItemListName" + Sno).val();
    ItmName = clickedrow.find("#POItemListName" + Sno + " option:selected").text();
    UOMName = clickedrow.find("#UOM option:selected").text();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
    $("#tbl_trackingDetail").DataTable().destroy();
}
function OnclickHistorySearchBtn() {
    debugger;
    ItmCode = $("#hfHistoryItemID").val();
    ItmName = $("#hfHistoryItemName").val();
    UOMName = $("#hfHistoryUOM").val();
    var SuppID = "";
    SuppID = $("#SupplierName").val();
    PurchaseOrderHistoryBtnClick(ItmCode, SuppID, ItmName, UOMName);
    $("#tbl_trackingDetail").DataTable().destroy();
}
function DisableHeaderField() {
    //debugger;
    $("#src_type").attr('disabled', true);
    $("#SupplierName").attr('disabled', true);
    $("#ddlCurrency").attr('disabled', true);
    $("#ddlstockable").attr('disabled', true);
    $("#ddlAll").attr('disabled', true);
    //$("#conv_rate").prop("readonly", true);
}
function BindPOItemList(e) {
    debugger;
    //var QtyDecDigit = $("#QtyDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var Itm_ID;
    var SNo = clickedrow.find("#SNohiddenfiled").val();
    Cmn_DeleteSubItemQtyDetail(ItemID);
    Itm_ID = clickedrow.find("#POItemListName" + SNo).val();
    clickedrow.find("#hfItemID").val(Itm_ID);

    if (Itm_ID == "0") {
        clickedrow.find("#POItemListNameError").text($("#valueReq").text());
        clickedrow.find("#POItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + SNo + "-container']").css("border-color", "red");
    }
    else {
        clickedrow.find("#POItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + SNo + "-container']").css("border-color", "#ced4da");
    }
    ClearRowDetails(e, ItemID);
    // HidePOItemListItm(SNo);
    //HideSelectedItem("#POItemListName", SNo, "#PoItmDetailsTbl", "#SNohiddenfiled");
    DisableHeaderField();
    try {
        Cmn_BindUOM(clickedrow, Itm_ID, "", "Y", "pur");
        debugger
        //GetUomNameItemWise(Itm_ID, "UOM", "UOMID", clickedrow);
    } catch (err) {
    }
    BindDelivertSchItemList();
    getMrpBoxDetailPrice(e);//add by Shubham Maurya on 18-12-2025
}
function getMrpBoxDetailPrice(e) {
    var clickedrow = $(e.target).closest("tr");
    var ItemID = clickedrow.find("#hfItemID").val();
    var supp_id = $('#SupplierName').val();
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/DPO/GetSPLDetail",
            data: {
                ItemID: ItemID,
                supp_id: supp_id
            },
            success: function (data) {
                debugger;
                    if (data !== null && data !== "" && data != "ErrorPage") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            clickedrow.find("#PackSize").val(arr.Table[0].pack_size);
                            clickedrow.find("#mrp").val(parseFloat(arr.Table[0].mrp).toFixed(RateDecDigit));
                            clickedrow.find("#item_rate").val(parseFloat(arr.Table[0].price).toFixed(RateDecDigit));
                            clickedrow.find("#item_rate" + RowNo).val(parseFloat(arr.Table[0].price).toFixed(RateDecDigit));
                            clickedrow.find("#item_disc_perc").val(parseFloat(arr.Table[0].item_disc_perc).toFixed(cmn_PerDecDigit));
                        }
                    }
            },
        });
}
function ClearRowDetails(e, ItemID) {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    var clickedrow = $(e.target).closest("tr");
    var SNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#ItmInfoBtnIcon").val("");
    clickedrow.find("#UOM").val("");
    clickedrow.find("#BtnTxtCalculation").val("");
    clickedrow.find("#ord_qty_spec").val("");
    clickedrow.find("#ord_qty_base").val("");
    clickedrow.find("#item_rate").val("");
    clickedrow.find("#item_disc_perc").val("");
    clickedrow.find("#item_disc_amt").val("");
    clickedrow.find("#item_disc_val").val("");
    clickedrow.find("#item_gr_val").val("");
    clickedrow.find("#item_ass_val").val("");
    clickedrow.find("#item_tax_amt").val("");
    clickedrow.find("#item_oc_amt").val("");
    clickedrow.find("#item_net_val_spec").val("");
    clickedrow.find("#item_net_val_bs").val("");
    clickedrow.find("#remarks").val("");
    //clickedrow.find("#SimpleIssue").attr("checked", false)commented by shubham maurya on 01-10-2025
    clickedrow.find("#ItmFClosed").attr("checked", false)

    //if (ItemID != "" && ItemID != null) {
    //    ShowItemListItm(ItemID);
    //}
    CalculateAmount();
    var TOCAmount = parseFloat($("#PO_OtherCharges").val());
    if (TOCAmount == "" || TOCAmount == null || TOCAmount == "NaN") {
        TOCAmoun = 0;
    }
    Calculate_OC_AmountItemWise(TOCAmount);
    AfterDeleteResetPO_ItemTaxDetail();
    if (ItemID != "" && ItemID != null) {
        DelDeliSchAfterDelItem(ItemID);
    }
}
function CalculateDisPercent(clickedrow) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    //var RateDecDigit = $("#RateDigit").text();
    //var ValDecDigit = $("#ValDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisPer;

    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (AvoidDot(DisPer) == false) {
        clickedrow.find("#item_disc_perc").val("");
        DisPer = 0;
        //return false;
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        var FAmt = (ItmRate * DisPer) / 100;
        var GAmt = OrderQty * (ItmRate - FAmt);
        var DisVal = OrderQty * FAmt;
        var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
        var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);

        clickedrow.find("#item_gr_val").val(FinGVal);
        clickedrow.find("#item_ass_val").val(FinGVal);
        clickedrow.find("#item_net_val_spec").val(FinGVal);
        FinalGVal = ConvRate * FinGVal
        clickedrow.find("#item_net_val_bs").val(parseFloat(FinalGVal).toFixed(ValDecDigit));
        clickedrow.find("#item_disc_val").val(FinDisVal);
        CalculateAmount();
        clickedrow.find("#item_disc_perc").val(parseFloat(DisPer).toFixed(cmn_PerDecDigit));
        clickedrow.find("#item_disc_amt").prop("readonly", true);
        clickedrow.find("#item_disc_amt").val("");
    }
    else {
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalVal = ConvRate * FinVal
            clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateAmount();
        }
        clickedrow.find("#item_disc_amt").prop("readonly", false);
    }
    OnChangeGrossAmt();
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());

}
function CalculateDisAmt(clickedrow) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    //var ValDecDigit = $("#ValDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItemName;
    var OrderQty;
    var ItmRate;
    var DisAmt;
    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    if (AvoidDot(DisAmt) == false) {
        clickedrow.find("#item_disc_amt").val("");
        DisAmt = 0;
        //return false;
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
        if (Math.fround(ItmRate) > Math.fround(DisAmt)) {
            var FRate = (ItmRate - DisAmt);
            var GAmt = OrderQty * FRate;
            var DisVal = OrderQty * DisAmt;
            var FinGVal = parseFloat(GAmt).toFixed(ValDecDigit);
            var FinDisVal = parseFloat(DisVal).toFixed(ValDecDigit);
            clickedrow.find("#item_disc_amtError").text("");
            clickedrow.find("#item_disc_amtError").css("display", "none");
            clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
            clickedrow.find("#item_disc_val").val(FinDisVal);
            clickedrow.find("#item_gr_val").val(FinGVal);
            clickedrow.find("#item_ass_val").val(FinGVal);
            clickedrow.find("#item_net_val_spec").val(FinGVal);
            FinalGVal = ConvRate * FinGVal
            clickedrow.find("#item_net_val_bs").val(parseFloat(FinalGVal).toFixed(ValDecDigit));
            clickedrow.find("#item_disc_perc").prop("readonly", true);
            clickedrow.find("#item_disc_perc").val("");
            CalculateAmount();
        }
        else {
            clickedrow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
            clickedrow.find("#item_disc_amtError").css("display", "block");
            clickedrow.find("#item_disc_amt").css("border-color", "red");
            clickedrow.find("#item_disc_amt").val('');
            clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
        }
        clickedrow.find("#item_disc_amt").val(parseFloat(DisAmt).toFixed(ValDecDigit));
    }
    else {
        clickedrow.find("#item_disc_amtError").text("");
        clickedrow.find("#item_disc_amtError").css("display", "none");
        clickedrow.find("#item_disc_amt").css("border-color", "#ced4da");
        if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "") {
            var FAmt = OrderQty * ItmRate;
            var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
            clickedrow.find("#item_gr_val").val(FinVal);
            clickedrow.find("#item_ass_val").val(FinVal);
            clickedrow.find("#item_net_val_spec").val(FinVal);
            FinalVal = ConvRate * FinVal
            clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
            CalculateAmount();
        }
        clickedrow.find("#item_disc_val").val(parseFloat(0).toFixed(ValDecDigit));
        clickedrow.find("#item_disc_perc").prop("readonly", false);
    }
    OnChangeGrossAmt();
    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
}
function CalculationBaseQty(e) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    var docid = $("#DocumentMenuId").val();
    //var QtyDecDigit = $("#QtyDigit").text();
    //var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var errorFlag = "N";
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;

    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        clickedrow.find("#POItemListNameError").text($("#valueReq").text());
        clickedrow.find("#POItemListNameError").css("display", "block");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
        clickedrow.find("#ord_qty_spec").val("");

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#POItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (OrderQty != "" && OrderQty != ".") {
        OrderQty = parseFloat(OrderQty);
    }
    if (OrderQty == ".") {
        OrderQty = 0;
    }
    if (OrderQty == "" || OrderQty == 0 || ItemName == "0") {
        clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
        clickedrow.find("#ord_qty_specError").css("display", "block");
        clickedrow.find("#ord_qty_spec").css("border-color", "red");
        clickedrow.find("#ord_qty_spec").val("");
        clickedrow.find("#ord_qty_spec").focus();
        clickedrow.find("#item_oc_amt").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qty_specError").text("");
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
    }

    var OrderQty = clickedrow.find("#ord_qty_spec").val();
    var ItmRate = clickedrow.find("#item_rate").val();

    if (AvoidDot(OrderQty) == false) {
        clickedrow.find("#ord_qty_spec").val("");
        OrderQty = 0;
    }

    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);

        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#item_net_val_spec").val(FinVal);
        FinalFinVal = ConvRate * FinVal
        clickedrow.find("#item_net_val_bs").val(parseFloat(FinalFinVal).toFixed(ValDecDigit));
        CalculateAmount();
    }
    clickedrow.find("#ord_qty_base").val(parseFloat(OrderQty).toFixed(QtyDecDigit));
    clickedrow.find("#ord_qty_spec").val(parseFloat(OrderQty).toFixed(QtyDecDigit));

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "") {
        CalculateDisPercent(clickedrow);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "") {
        CalculateDisAmt(clickedrow);
    }
    OnChangeGrossAmt();

    CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
    var assVal = clickedrow.find("#item_ass_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        if (docid != '105101140101') {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        clickedrow.find("#item_ass_valError").css("display", "none");
        clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
    BindDelivertSchItemList();
}
function CalculationBaseRate(clickedrow, flag) {
    debugger;
    var ConvRate = $("#conv_rate").val();
    var docid = $("#DocumentMenuId").val();
    //var RateDecDigit = $("#RateDigit").text();
    //var ValDecDigit = $("#ValDigit").text();
    //var clickedrow = $(e.target).closest("tr");
    var errorFlag = "N";
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var OrderQty;
    var ItemName;
    var ItmRate;
    var DisAmt;
    var DisPer;
    OrderQty = clickedrow.find("#ord_qty_spec").val();
    ItemName = clickedrow.find("#POItemListName" + Sno).val();
    ItmRate = clickedrow.find("#item_rate").val();
    DisAmt = clickedrow.find("#item_disc_amt").val();
    DisPer = clickedrow.find("#item_disc_perc").val();

    if (ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate 
            clickedrow.find("#POItemListNameError").text($("#valueReq").text());
            clickedrow.find("#POItemListNameError").css("display", "block");
            clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
            clickedrow.find("#item_rate").val("");
        }

        errorFlag = "Y";
    }
    else {
        clickedrow.find("#POItemListNameError").css("display", "none");
        clickedrow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid #aaa");
    }
    if (OrderQty == "" || OrderQty == "0" || ItemName == "0") {
        if (flag != "hideError") {//Added by Suraj on 07-11-2024 to validate without showing error message in change of exchange rate
            clickedrow.find("#ord_qty_specError").text($("#valueReq").text());
            clickedrow.find("#ord_qty_specError").css("display", "block");
            clickedrow.find("#ord_qty_spec").css("border-color", "red");
            clickedrow.find("#ord_qty_spec").val("");
            clickedrow.find("#ord_qty_spec").focus();
            clickedrow.find("#item_rate").val("");
        }
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#ord_qty_specError").css("display", "none");
        clickedrow.find("#ord_qty_spec").css("border-color", "#ced4da");
    }
    if (errorFlag == "Y") {
        return false;
    }
    if (ItmRate != "" && ItmRate != ".") {
        ItmRate = parseFloat(ItmRate);
    }
    if (ItmRate == ".") {
        ItmRate = 0;
    }
    if (ItmRate == "" || ItmRate == 0 || ItemName == "0") {
        clickedrow.find("#item_rateError").text($("#valueReq").text());
        clickedrow.find("#item_rateError").css("display", "block");
        clickedrow.find("#item_rate").css("border-color", "red");
        clickedrow.find("#item_rate").val("");
        if (OrderQty != "" && OrderQty != null) {
            clickedrow.find("#item_rate").focus();
        }
        clickedrow.find("#item_oc_amt").val("");
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#item_rateError").css("display", "none");
        clickedrow.find("#item_rate").css("border-color", "#ced4da");
    }

    if (AvoidDot(ItmRate) == false) {
        clickedrow.find("#item_rate").val("");
        ItmRate = 0;
        // return false;
    }
    if ((OrderQty !== 0 || OrderQty !== null || OrderQty !== "") && (ItmRate !== 0 || ItmRate !== null || ItmRate !== "")) {
        var FAmt = OrderQty * ItmRate;
        var FinVal = parseFloat(FAmt).toFixed(ValDecDigit);
        clickedrow.find("#item_gr_val").val(FinVal);
        clickedrow.find("#item_ass_val").val(FinVal);
        clickedrow.find("#item_net_val_spec").val(FinVal);
        FinalVal = FinVal * ConvRate
        clickedrow.find("#item_net_val_bs").val(parseFloat(FinalVal).toFixed(ValDecDigit));
        CalculateAmount();
    }

    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisPer !== 0 && DisPer !== "" && DisPer !== "0.00") {
        CalculateDisPercent(clickedrow);
    }
    if (OrderQty !== 0 && OrderQty !== "" && ItmRate !== 0 && ItmRate !== "" && DisAmt !== 0 && DisAmt !== "" && DisAmt !== "0.00") {
        CalculateDisAmt(clickedrow);
    }
    clickedrow.find("#item_rate").val(parseFloat(ItmRate).toFixed(RateDecDigit));
    OnChangeGrossAmt();
    if (clickedrow.find("#TaxExempted").is(":checked")) {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
        clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
    }
    else {
        CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
    }
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (GstApplicable == "Y") {
        if (clickedrow.find("#ManualGST").is(":checked")) {
            clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
            clickedrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
        else {
            CalculateTaxAmount_ItemWise(Sno, ItemName, clickedrow.find("#item_ass_val").val());
        }
    }
    var assVal = clickedrow.find("#item_ass_val").val();
    assVal = parseFloat(assVal);
    if (assVal > 0) {
        if (docid != '105101140101') {
            clickedrow.find("#BtnTxtCalculation").prop("disabled", false);
        }
        clickedrow.find("#item_ass_valError").css("display", "none");
        clickedrow.find("#item_ass_val").css("border-color", "#ced4da");
    }
    else {
        clickedrow.find("#BtnTxtCalculation").prop("disabled", true);
    }
}

//------------------Tax Amount Calculation------------------//
function OnClickTaxCalculationBtn(e) {
    debugger;
    var POItemListName = "#POItemListName";
    var SNohiddenfiled = "#SNohiddenfiled";
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
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, POItemListName);
    debugger;
    if (GstApplicable == "Y") {
        if ($("#Disable").val() == "Disable") {
            $("#Tax_Template").attr("disabled", true);
            $("#SaveAndExitBtn").css("display", "none");
        }
        else {
            if (currentRow.find("#ManualGST").is(":checked")) {
                $("#Tax_Template").attr("disabled", false);
                $("#SaveAndExitBtn").css("display", "Block");
            }
            else {
                $("#Tax_Template").attr("disabled", true);
                $("#SaveAndExitBtn").css("display", "none");
            }
        }
    }
}

function OnClickSaveAndExit() {
    debugger;
    var ConvRate = $("#conv_rate").val();
    
    //var ValDecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var UserID = $("#UserID").text();

    /*commented by Hina on 14-07-2025*/
    //var tbllenght = $("#TaxCalculatorTbl tbody tr").length
    //if (tbllenght == 0) {
    //    $("[aria-labelledby='select2-Tax_Template-container']").css("border-color", "red");
    //    $("#SpanTax_Template").text($("#valueReq").text());
    //    $("#SpanTax_Template").css("display", "block");
    //    $("#SaveAndExitBtn").attr("data-dismiss", "");
    //    return false;
    //}
    //else {
    //    $("#SaveAndExitBtn").attr("data-dismiss", "modal");
    //}
    debugger;
    let NewArr = [];
    /*Start Code Add by Hina Sharma on 09-07-2025 */
    var TaxOn = $("#HdnTaxOn").val();
    //var TaxOn = "";
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    
    /*var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;*/
    var FTaxDetails = $("#" + HdnTaxCalculateTable + " > tbody > tr").length;
    /*End Code Add by Hina Sharma on 09-07-2025 */
    if (FTaxDetails > 0) {
        /* $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {*//*commented and modify by Hina on 09-07-2025*/
        $("#" + HdnTaxCalculateTable + " > tbody > tr").each(function () {
            var currentRow = $(this);
            var TaxItemID = currentRow.find("#TaxItmCode").text();
            var TaxName = currentRow.find("#TaxName").text();
            var TaxNameID = currentRow.find("#TaxNameID").text();
            var TaxPercentage = currentRow.find("#TaxPercentage").text();
            var TaxLevel = currentRow.find("#TaxLevel").text();
            var TaxApplyOn = currentRow.find("#TaxApplyOn").text();
            var TaxAmount = currentRow.find("#TaxAmount").text();
            var TaxApplyOnID = currentRow.find("#TaxApplyOnID").text();
            if (TaxItemID == TaxItmCode) {
                currentRow.remove();
            }
            else {

                NewArr.push({ TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID });
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
            /*  $("#Hdn_TaxCalculatorTbl > tbody").append(`*//*commented and modify by Hina on 09-07-2025*/
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                <tr>
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
                                </tr>`)
            NewArr.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        //BindTaxAmountDeatils(NewArr);/*commented and modify by Hina on 09-07-2025*/
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(NewArr);
        }
    }
    else {
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
            /* $("#Hdn_TaxCalculatorTbl > tbody").append(`*//*commented and modify by Hina on 09-07-2025*/
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                <tr>
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
                                </tr>
            `)
            TaxCalculationList.push({ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        });
        /*BindTaxAmountDeatils(TaxCalculationList);*//*commented and modify by Hina on 09-07-2025*/
        if (TaxOn != "OC") {
            BindTaxAmountDeatils(TaxCalculationList);
        }
    }
    /*Code start add by Hina on 09-07-2025*/
    if (TaxOn == "OC") {

        $("#Tbl_OC_Deatils >tbody >tr").each(function (i, row) {
            // debugger;
            var currentRow = $(this);
            if (currentRow.find("#OCValue").text() == TaxItmCode) {
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                currentRow.find("#OCTaxAmt").text(TaxAmt);
                var OCAmt = currentRow.find("#OcAmtBs").text().trim();
                var Total = parseFloat(parseFloat(OCAmt) + parseFloat(TaxAmt)).toFixed(ValDecDigit)
                currentRow.find("#OCTotalTaxAmt").text(Total);
            }
        });
        Calculate_OCAmount();
    }
    else {
        /*Code end add by Hina on 09-07-2025*/
        $("#PoItmDetailsTbl >tbody >tr").each(function (i, row) {
            //debugger;
            var currentRow = $(this);
            if (currentRow.find("#SNohiddenfiled").val() == RowSNo) {

                //var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit);
                TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                if (TaxAmt == "" || TaxAmt == "NaN") {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                }
                if (currentRow.find("#TaxExempted").is(":checked")) {
                    TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                }
                else {
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                }
                //var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                //var TaxAmt1 = "";
                //var TaxAmt2 = parseFloat(0).toFixed(ValDecDigit)
                //if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt1) {
                //    TaxAmt = (parseFloat(0)).toFixed(ValDecDigit);
                //}
                //if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt2) {
                //    TaxAmt = (parseFloat(0)).toFixed(ValDecDigit);
                //}
                OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                var itmocamt = currentRow.find("#item_oc_amt").val();
                if (itmocamt != null && itmocamt != "") {
                    OC_Amt = parseFloat(CheckNullNumber(itmocamt)).toFixed(ValDecDigit);
                }
                AssessableValue = (parseFloat(CheckNullNumber(currentRow.find("#item_gr_val").val()))).toFixed(ValDecDigit);
                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TaxAmt) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                currentRow.find("#item_net_val_spec").val(parseFloat(NetOrderValueSpec).toFixed(ValDecDigit));
                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                currentRow.find("#item_net_val_bs").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));

            }
            var TaxAmt1 = parseFloat(0).toFixed(ValDecDigit)
            var ItemTaxAmt = currentRow.find("#item_tax_amt").val();
            if (ItemTaxAmt != TaxAmt1) {
                currentRow.find('#BtnTxtCalculation').css('border', '#ced4da');
            }
        });
        CalculateAmount();
    }
    $('#BtnTxtCalculation').css('border', '"#ced4da"');
}
function OnClickReplicateOnAllItems() {
    debugger
    var ConvRate = $("#conv_rate").val();
    var errorflag = "N";
    //var ValDecDigit = $("#ValDigit").text();
    var TotalTaxAmount = $('#TotalTaxAmount').text();
    var RowSNo = $("#HiddenRowSNo").val();
    var TaxItmCode = $("#TaxCalcItemCode").val();
    var CitmRowSNo = RowSNo;
    var CitmTaxItmCode = TaxItmCode;
    var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);
    var UserID = $("#UserID").text();
    var TaxOn = $("#HdnTaxOn").val();/*add by Hina on 09-07-2025*/
    debugger;
    var TaxCalculationListFinalList = [];
    var TaxCalculationList = [];
    //$("#Hdn_TaxCalculatorTbl > tbody > tr").remove();/*commented by Hina on 09-07-2025*/
    $("#TaxCalculatorTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var TaxName = currentRow.find("#taxname").text();
        var TaxNameID = currentRow.find("#taxid").text();
        var TaxPercentage = currentRow.find("#taxrate").text();
        var TaxLevel = currentRow.find("#taxlevel").text();
        var TaxApplyOn = currentRow.find("#taxapplyonname").text();
        var TaxAmount = currentRow.find("#taxval").text();
        var TaxApplyOnID = currentRow.find("#taxapplyon").text();

        TaxCalculationList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
        TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID })
    });
    if (TaxCalculationList != null) {
        if (TaxCalculationList.length > 0) {
            /*Code start add by Hina on 09-07-2025*/
            var SnoForOc = 0;
            var TaxApplicableTable = "PoItmDetailsTbl";
            if (TaxOn == "OC") {
                TaxApplicableTable = "Tbl_OC_Deatils";
            }

            $("#" + TaxApplicableTable + " >tbody >tr").each(function () {

                /*$("#PoItmDetailsTbl >tbody >tr").each(function () {*/
                /*Code end add by Hina on 09-07-2025*/
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemCode;
                var AssessVal;
                //debugger;
                /*Code start commented and modify  by Hina on 09-07-2025*/
                //ItemCode = currentRow.find("#POItemListName" + Sno).val();
                //AssessVal = currentRow.find("#item_ass_val").val();
                if (TaxOn == "OC") {
                    ItemCode = currentRow.find("#OCValue").text();
                    AssessVal = currentRow.find("#OcAmtBs").text();
                    SnoForOc++;
                    Sno = SnoForOc;
                } else {
                    ItemCode = currentRow.find("#POItemListName" + Sno).val();
                    AssessVal = currentRow.find("#item_ass_val").val();
                }
                /*Code end commented and modify by Hina on 09-07-2025*/
                var AssessValcheck = parseFloat(AssessVal);
                if (AssessVal != "" && AssessValcheck > 0) {
                    var NewArray = [];
                    var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);

                    for (i = 0; i < TaxCalculationList.length; i++) {
                        var TaxPercentage = "";
                        var TaxName = TaxCalculationList[i].TaxName;
                        var TaxNameID = TaxCalculationList[i].TaxNameID;
                        var TaxItmCode = TaxCalculationList[i].TaxItmCode;
                        var RowSNo = TaxCalculationList[i].RowSNo;
                        TaxPercentage = TaxCalculationList[i].TaxPercentage;
                        var TaxLevel = TaxCalculationList[i].TaxLevel;
                        var TaxApplyOn = TaxCalculationList[i].TaxApplyOn;
                        var TaxApplyOnID = TaxCalculationList[i].TaxApplyOnID;
                        var TaxAmount;
                        var TaxPec;
                        TaxPec = TaxPercentage.replace('%', '');

                        if (TaxApplyOn == "Immediate Level") {
                            if (TaxLevel == "1") {
                                TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                            }
                            else {
                                var TaxAMountColIL = parseFloat(0).toFixed(ValDecDigit);
                                var TaxLevelTbl = parseInt(TaxLevel) - 1;
                                for (j = 0; j < NewArray.length; j++) {
                                    var Level = NewArray[j].TaxLevel;
                                    var TaxAmtLW = NewArray[j].TaxAmount;
                                    if (TaxLevelTbl == Level) {
                                        TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                    }
                                }
                                var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(ValDecDigit);
                                TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                            }
                        }
                        if (TaxApplyOn == "Cummulative") {
                            var Level = TaxLevel;
                            if (TaxLevel == "1") {
                                TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                            }
                            else {
                                var TaxAMountCol = parseFloat(0).toFixed(ValDecDigit);

                                for (j = 0; j < NewArray.length; j++) {
                                    var Level = NewArray[j].TaxLevel;
                                    var TaxAmtLW = NewArray[j].TaxAmount;
                                    if (TaxLevel != Level) {
                                        TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                    }
                                }
                                var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(ValDecDigit);
                                TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                                TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                            }
                        }
                        NewArray.push({ /*UserID: UserID, RowSNo: Sno,*/ TaxItmCode: ItemCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                    }
                    if (NewArray != null) {
                        if (NewArray.length > 0) {
                            for (k = 0; k < NewArray.length; k++) {
                                var TaxName = NewArray[k].TaxName;
                                var TaxNameID = NewArray[k].TaxNameID;
                                var TaxItmCode = NewArray[k].TaxItmCode;
                                var RowSNo = NewArray[k].RowSNo;
                                var TaxPercentage = NewArray[k].TaxPercentage;
                                var TaxLevel = NewArray[k].TaxLevel;
                                var TaxApplyOn = NewArray[k].TaxApplyOn;
                                var TaxAmount = NewArray[k].TaxAmount;
                                var TaxApplyOnID = NewArray[k].TaxApplyOnID;
                                /* if (CitmTaxItmCode != TaxItmCode && CitmRowSNo != RowSNo) {*//*commented and modify by Hina on 09-07-2025*/
                                if (CitmTaxItmCode != TaxItmCode /*&& CitmRowSNo != RowSNo*/) {
                                    TaxCalculationListFinalList.push({ /*UserID: UserID, RowSNo: RowSNo,*/ TaxItmCode: TaxItmCode, TaxName: TaxName, TaxNameID: TaxNameID, TaxPercentage: TaxPercentage, TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn, TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmt, TaxApplyOnID: TaxApplyOnID })
                                }
                            }
                        }
                    }
                }

            });
        }
    }
    /*Code start add by Hina on 09-07-2025*/
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";
    if (TaxOn == "OC") {
        HdnTaxCalculateTable = "Hdn_OCTemp_TaxCalculatorTbl";
    }
    $("#" + HdnTaxCalculateTable + " > tbody >tr").remove();
    /*Code end add by Hina on 09-07-2025*/
    if (TaxCalculationListFinalList.length > 0) {
      
        for (var j = 0; j < TaxCalculationListFinalList.length; j++) {
            /* $("#Hdn_TaxCalculatorTbl > tbody").append(`*//*commented and modify by Hina on 09-07-2025*/
            $("#" + HdnTaxCalculateTable + " > tbody").append(`
                                 <tr>
                                    <td id="DocNo"></td>
                                    <td id="DocDate"></td>
                                    <td id="TaxItmCode">${TaxCalculationListFinalList[j].TaxItmCode}</td>
                                    <td id="TaxName">${TaxCalculationListFinalList[j].TaxName}</td>
                                    <td id="TaxNameID">${TaxCalculationListFinalList[j].TaxNameID}</td>
                                    <td id="TaxPercentage">${TaxCalculationListFinalList[j].TaxPercentage}</td>
                                    <td id="TaxLevel">${TaxCalculationListFinalList[j].TaxLevel}</td>
                                    <td id="TaxApplyOn">${TaxCalculationListFinalList[j].TaxApplyOn}</td>
                                    <td id="TaxAmount">${TaxCalculationListFinalList[j].TaxAmount}</td>
                                    <td id="TotalTaxAmount">${TaxCalculationListFinalList[j].TotalTaxAmount}</td>
                                    <td id="TaxApplyOnID">${TaxCalculationListFinalList[j].TaxApplyOnID}</td>
                                </tr>`)
        }
        if (TaxOn != "OC") {/*add by Hina on 09-07-2025*/
            BindTaxAmountDeatils(TaxCalculationListFinalList);
        }
    } else {
        //sessionStorage.setItem("TaxCalcDetails", JSON.stringify(TaxCalculationListFinalList));
        if (TaxOn != "OC") {/*add by Hina on 09-07-2025*/
            BindTaxAmountDeatils(TaxCalculationListFinalList);
        }
    }
    /*Code start add by Hina on 09-07-2025*/
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
                            var NetAmt = (parseFloat(OCAmt) + parseFloat(TotalTaxAmtF)).toFixed(ValDecDigit)
                            currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                        }
                    }
                }
                else {
                    currentRow.find("#OCTaxAmt").text(parseFloat(0).toFixed(ValDecDigit));
                    var OCAmt = parseFloat(CheckNullNumber(currentRow.find("#OcAmtBs").text()));
                    var NetAmt = (parseFloat(OCAmt) + parseFloat(0)).toFixed(ValDecDigit)
                    currentRow.find("#OCTotalTaxAmt").text(NetAmt);
                }
            }
        });
        Calculate_OCAmount();
    } else {
        /*Code end add by Hina on 09-07-2025*/
        debugger;
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
            //var Sno = currentRow.find("#SNohiddenfiled").val();
            var hfItemID = currentRow.find("#hfItemID").val();
            if (TaxCalculationListFinalList != null) {
                if (TaxCalculationListFinalList.length > 0) {
                    for (i = 0; i < TaxCalculationListFinalList.length; i++) {
                        var TotalTaxAmtF = TaxCalculationListFinalList[i].TotalTaxAmount;
                        //var RowSNoF = TaxCalculationListFinalList[i].RowSNo;
                        var txtTaxItmCode = TaxCalculationListFinalList[i].TaxItmCode;

                        if (hfItemID == txtTaxItmCode) {

                            TaxAmt = (parseFloat(TotalTaxAmount)).toFixed(ValDecDigit);
                            currentRow.find("#item_tax_amt").val(TotalTaxAmtF);
                            var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                            if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                            }
                            AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(ValDecDigit);
                            NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                            currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                            FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                            currentRow.find("#item_net_val_bs").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));
                        }
                    }
                }
                else {
                    var GrossAmtOR = parseFloat(0).toFixed(ValDecDigit);
                    if (currentRow.find("#item_gr_val").val() != "") {
                        GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(ValDecDigit);
                    }
                    currentRow.find("#item_tax_amt").val(TaxAmt);
                    var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                    if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                        OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                    }
                    var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                    currentRow.find("#item_ass_val").val(GrossAmtOR);
                    currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                    FinalGrossAmtOR = ConvRate * FGrossAmtOR
                    currentRow.find("#item_net_val_bs").val(parseFloat(FinalGrossAmtOR).toFixed(ValDecDigit));

                }
            }
        });
        CalculateAmount();
    }
}
function CalculateTaxAmount_ItemWise(RowSNo, ItmCode, AssAmount) {
    debugger;
    //var ValDecDigit = $("#ValDigit").text();
    var UserID = $("#UserID").text();
    var ConvRate = $("#conv_rate").val();
    var TaxOn = $("#HdnTaxOn").val();/*add by Hina on 09-07-2025*/
    var HdnTaxCalculateTable = "Hdn_TaxCalculatorTbl";/*add by Hina on 09-07-2025*/

    if (AssAmount != "" && AssAmount != null) {
        /*commented and modify by Hina on 09-07-2025*/
        /*var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;*///JSON.parse(sessionStorage.getItem("TaxCalcDetails"));
        var FTaxDetailsItemWise = $("#" + HdnTaxCalculateTable + " > tbody > tr").length;

        if (FTaxDetailsItemWise > 0) {
            var TotalTaxAmt = parseFloat(0).toFixed(ValDecDigit);
            var NewArray = [];
            /* $("#Hdn_TaxCalculatorTbl > tbody > tr").each(function () {*/  /*commented and modify by Hina on 09-07-2025*/
            $("#" + HdnTaxCalculateTable + " > tbody > tr").each(function () {
                debugger;
                var currentRow = $(this);
                //var TaxUserID = currentRow.find("#UserID").text();
                //var TaxRowID = currentRow.find("#RowSNo").text();
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                var AssessVal = parseFloat(AssAmount).toFixed(ValDecDigit);
                if ( /*TaxUserID == UserID &&TaxRowID == RowSNo &&*/ TaxItemID == ItmCode) {

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
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountColIL = parseFloat(0).toFixed(ValDecDigit);
                            var TaxLevelTbl = parseInt(TaxLevel) - 1;
                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevelTbl == Level) {
                                    TaxAMountColIL = (parseFloat(TaxAMountColIL) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmtIL = (parseFloat(TaxAMountColIL)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmtIL) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    if (TaxApplyOn == "Cummulative") {
                        var Level = TaxLevel;
                        if (TaxLevel == "1") {
                            TaxAmount = ((parseFloat(AssessVal) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                        else {
                            var TaxAMountCol = parseFloat(0).toFixed(ValDecDigit);

                            for (j = 0; j < NewArray.length; j++) {
                                var Level = NewArray[j].TaxLevel;
                                var TaxAmtLW = NewArray[j].TaxAmount;
                                if (TaxLevel != Level) {
                                    TaxAMountCol = (parseFloat(TaxAMountCol) + parseFloat(TaxAmtLW)).toFixed(ValDecDigit);
                                }
                            }
                            var FinalAssAmt = (parseFloat(AssessVal) + parseFloat(TaxAMountCol)).toFixed(ValDecDigit);
                            TaxAmount = ((parseFloat(FinalAssAmt) * parseFloat(TaxPec)) / 100).toFixed(ValDecDigit);
                            TotalTaxAmt = (parseFloat(TotalTaxAmt) + parseFloat(TaxAmount)).toFixed(ValDecDigit);
                        }
                    }
                    //debugger;
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

                NewArray.push({
                    TaxItmCode: TaxItemID, TaxName: TaxName, TaxNameID: TaxNameID,
                    TaxPercentage: TaxPercentage, TaxLevel: TaxLevel, TaxApplyOn: TaxApplyOn,
                    TaxAmount: TaxAmount, TotalTaxAmount: TotalTaxAmount, TaxApplyOnID: TaxApplyOnID
                });

            });
            /* $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {*//*commented and modify by Hina on 09-07-2025*/
            $("#" + HdnTaxCalculateTable + " > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent().each(function () {
                var currentRow = $(this);
                var TaxItemID = currentRow.find("#TaxItmCode").text();
                if (TaxItemID == ItmCode) {
                    currentRow.find("#TotalTaxAmount").text(TotalTaxAmt);
                }
            });
            $("#PoItmDetailsTbl > tbody > tr #hfItemID[value=" + ItmCode + "]").parent().parent().each(function () {
                debugger;
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                var ItemNo;

                ItemNo = currentRow.find("#POItemListName" + Sno).val();

                if (Sno == RowSNo && ItemNo == ItmCode) {
                    var FTaxDetailsItemWise = $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains(" + ItmCode + ")").parent();
                    if (FTaxDetailsItemWise.length > 0) {
                        FTaxDetailsItemWise.each(function () {
                            debugger;
                            var CRow = $(this);
                            var TotalTaxAmtF = CRow.find("#TotalTaxAmount").text();
                            var ItemTaxAmt = currentRow.find("#item_tax_amt").val()
                            if (currentRow.find("#TaxExempted").is(":checked")) {
                                TotalTaxAmtF = 0;
                            }
                            var TaxAmt = parseFloat(0).toFixed(ValDecDigit)
                            var GstApplicable = $("#Hdn_GstApplicable").text();
                            if (GstApplicable == "Y") {
                                if (currentRow.find("#ManualGST").is(":checked") && ItemTaxAmt == TaxAmt) {
                                    TotalTaxAmtF = parseFloat(0).toFixed(ValDecDigit)
                                }
                            }
                            var TaxItmCode = CRow.find("#TaxItmCode").text();
                            if (TaxItmCode == ItmCode) {
                                currentRow.find("#item_tax_amt").val(parseFloat(TotalTaxAmtF).toFixed(ValDecDigit));
                                var OC_Amt = (parseFloat(0)).toFixed(ValDecDigit);
                                if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                                    OC_Amt = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                                }
                                AssessableValue = (parseFloat(currentRow.find("#item_gr_val").val())).toFixed(ValDecDigit);
                                NetOrderValueSpec = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                                NetOrderValueBase = (parseFloat(AssessableValue) + parseFloat(TotalTaxAmtF) + parseFloat(OC_Amt)).toFixed(ValDecDigit);
                                currentRow.find("#item_net_val_spec").val(NetOrderValueSpec);
                                FinalNetOrderValueBase = ConvRate * NetOrderValueBase
                                currentRow.find("#item_net_val_bs").val(parseFloat(FinalNetOrderValueBase).toFixed(ValDecDigit));

                                //}
                            }
                        });
                    }
                    else {
                        debugger;
                        var TaxAmt = parseFloat(0).toFixed(ValDecDigit);
                        var GrossAmtOR = parseFloat(currentRow.find("#item_gr_val").val()).toFixed(ValDecDigit);
                        currentRow.find("#item_tax_amt").val(TaxAmt);
                        var OC_Amt_OR = (parseFloat(0)).toFixed(ValDecDigit);
                        if (currentRow.find("#item_oc_amt").val() != null && currentRow.find("#item_oc_amt").val() != "") {
                            OC_Amt_OR = parseFloat(currentRow.find("#item_oc_amt").val()).toFixed(ValDecDigit);
                        }
                        var FGrossAmtOR = (parseFloat(OC_Amt_OR) + parseFloat(GrossAmtOR)).toFixed(ValDecDigit);
                        currentRow.find("#item_net_val_spec").val(FGrossAmtOR);
                        FinalFGrossAmtOR = ConvRate * FGrossAmtOR
                        currentRow.find("#item_net_val_bs").val(parseFloat(FinalFGrossAmtOR).toFixed(ValDecDigit));
                    }
                }
            });
            CalculateAmount();
            BindTaxAmountDeatils(NewArray);
        }
    }
}
function BindTaxAmountDeatils(TaxAmtDetail) {
    debugger;

    var PO_ItemTaxAmountList = "#Tbl_ItemTaxAmountList";
    var PO_ItemTaxAmountTotal = "#_ItemTaxAmountTotal";

    CMNBindTaxAmountDeatilsModel(TaxAmtDetail, PO_ItemTaxAmountList, PO_ItemTaxAmountTotal);
}
function AfterDeleteResetPO_ItemTaxDetail() {
    debugger;
    var PoItmDetailsTbl = "#PoItmDetailsTbl";
    var SNohiddenfiled = "#SNohiddenfiled";
    var POItemListName = "#POItemListName";
    CMNAfterDeleteReset_ItemTaxDetailModel(PoItmDetailsTbl, SNohiddenfiled, POItemListName);
}
function OnClickSaveAndExit_OC_Btn() {
    debugger;
    var NetOrderValueSpe = "#NetOrderValueSpe";
    var NetOrderValueInBase = "#NetOrderValueInBase";
    /*var PO_otherChargeId = '#Tbl_OtherChargeList';*//*commented and modify by Hina on 09-07-2025*/
    var PO_otherChargeId = '#PO_OtherCharges';
    CMNOnClickSaveAndExit_OC_Btn(PO_otherChargeId, NetOrderValueSpe, NetOrderValueInBase);
}
function Calculate_OC_AmountItemWise(TotalOCAmt) {
    debugger;
    //var ValDecDigit = $("#ValDigit").text();
    var TotalGAmt = $("#TxtGrossValue").val();
    var ConvRate = $("#conv_rate").val();
    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();

        var GrossValue = currentRow.find("#item_gr_val").val();
        if (GrossValue == "" || GrossValue == null) {
            GrossValue = "0";
        }
        if (parseFloat(GrossValue) > 0 && parseFloat(TotalGAmt) > 0 && parseFloat(TotalOCAmt) > 0) {
            //debugger;
            var OCPercentage = ((parseFloat(GrossValue) / parseFloat(TotalGAmt)) * 100).toFixed(ValDecDigit);
            var OCAmtItemWise = ((parseFloat(OCPercentage) * parseFloat(TotalOCAmt)) / 100).toFixed(ValDecDigit);
            if (parseFloat(OCAmtItemWise) > 0) {
                currentRow.find("#item_oc_amt").val(parseFloat(OCAmtItemWise).toFixed(ValDecDigit));
            }
            else {
                currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(ValDecDigit));
            }
        }
        if (parseFloat(TotalOCAmt) == 0) {
            currentRow.find("#item_oc_amt").val(parseFloat(0).toFixed(ValDecDigit));
        }
    });
    var TotalOCAmount = parseFloat(0).toFixed(ValDecDigit);
    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var OCValue = currentRow.find("#item_oc_amt").val();
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
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) - parseFloat(AmtDifference)).toFixed(ValDecDigit));
            }
        });
    }
    if (parseFloat(TotalOCAmount) < parseFloat(TotalOCAmt)) {
        var AmtDifference = parseFloat(TotalOCAmt) - parseFloat(TotalOCAmount);
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            //debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var OCValue = currentRow.find("#item_oc_amt").val();
            if (OCValue == "" || OCValue == null) {
                OCValue = "0";
            }
            if (Sno === "1") {
                currentRow.find("#item_oc_amt").val((parseFloat(OCValue) + parseFloat(AmtDifference)).toFixed(ValDecDigit));
            }
        });
    }
    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        var POItm_GrossValue = currentRow.find("#item_gr_val").val();
        var POItm_TaxAmt = parseFloat(0).toFixed(ValDecDigit);
        var POItm_OCAmt = currentRow.find("#item_oc_amt").val();
        if (POItm_GrossValue == null || POItm_GrossValue == "") {
            POItm_GrossValue = "0";
        }
        if (POItm_OCAmt == null || POItm_OCAmt == "") {
            POItm_OCAmt = "0";
        }
        if (currentRow.find("#item_tax_amt").val() != null && currentRow.find("#item_tax_amt").val() != "") {
            POItm_TaxAmt = parseFloat(currentRow.find("#item_tax_amt").val()).toFixed(ValDecDigit);
        }
        var POItm_NetOrderValueSpec = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        var POItm_NetOrderValueBase = (parseFloat(POItm_GrossValue) + parseFloat(POItm_TaxAmt) + parseFloat(POItm_OCAmt));
        currentRow.find("#item_net_val_spec").val((parseFloat(CheckNullNumber(POItm_NetOrderValueSpec))).toFixed(ValDecDigit));
        FinalPOItm_NetOrderValueBase = ConvRate * POItm_NetOrderValueBase
        currentRow.find("#item_net_val_bs").val((parseFloat(CheckNullNumber(FinalPOItm_NetOrderValueBase))).toFixed(ValDecDigit));
    });
    CalculateAmount();
};
function BindOtherChargeDeatils() {
    debugger;
    //var ValDecDigit = $("#ValDigit").text();//Tbl_OC_Deatils
    if ($("#PoItmDetailsTbl >tbody >tr").length == 0) {
        $("#Tbl_OC_Deatils >tbody >tr").remove();
        $("#Total_OC_Amount").text(parseFloat(0).toFixed(ValDecDigit));
        //$("#PO_OtherCharges").val(parseFloat(0).toFixed(ValDecDigit));/*commented and modify by Hina on 09-07-2025*/
        $("#_OtherChargeTotal").val(parseFloat(0).toFixed(ValDecDigit));
    }
    /*Code start add by Hina on 09-07-2025*/
    var DocumentMenuId = $("#DocumentMenuId").val();
    var TotalAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalTaxAMount = parseFloat(0).toFixed(ValDecDigit);
    var TotalAMountWT = parseFloat(0).toFixed(ValDecDigit);
    /*Code end add by Hina on 09-07-2025*/

    $("#Tbl_OtherChargeList >tbody >tr").remove();
    $("#PO_OtherChargeTotal").text(parseFloat(0).toFixed(ValDecDigit));
    
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {

        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            /*Code start add by Hina on 09-07-2025*/
            var td = "";
            if (DocumentMenuId == "105101130") {
                td = `<td align="right">${currentRow.find("#OCTaxAmt").text()}</td>
<td align="right">${currentRow.find("#OCTotalTaxAmt").text()}</td>`
            } /*Code end add by Hina on 09-07-2025*/

            $('#Tbl_OtherChargeList tbody').append(`<tr>
<td id=othrChrg_Name>${currentRow.find("#OCName").text()}</td>
<td align="right">${currentRow.find("#OCAmount").text()}</td>
`+ td +`
</tr>`);
            TotalAMount = (parseFloat(TotalAMount) + parseFloat(currentRow.find("#OCAmount").text())).toFixed(ValDecDigit);
            /*Code start add by Hina on 09-07-2025*/
            if (DocumentMenuId == "105101130") {
                TotalTaxAMount = (parseFloat(TotalTaxAMount) + parseFloat(currentRow.find("#OCTaxAmt").text())).toFixed(ValDecDigit);
                TotalAMountWT = (parseFloat(TotalAMountWT) + parseFloat(currentRow.find("#OCTotalTaxAmt").text())).toFixed(ValDecDigit);
            }
            /*Code end add by Hina on 09-07-2025*/
        });

    }
    /*commented and modify by Hina on 09-07-2025*/
    //$("#_OtherChargeTotal").text(TotalAMount);
    //$("#PO_OtherCharges").val(TotalAMount);
    $("#_OtherChargeTotal").text(TotalAMount);
    if (DocumentMenuId == "105101130") {
        $("#_OtherChargeTotalTax").text(TotalTaxAMount);
        $("#_OtherChargeTotalAmt").text(TotalAMountWT);
    }
}
function SetOtherChargeVal() {

}
//------------------Tax For OC Calculation------------------//
function OnClickOCTaxCalculationBtn(e) {
    var OC_ID = "#OCValue";
    var SNohiddenfiled = "OC";
    CMNOnClickTaxCalculationBtnModel(e, SNohiddenfiled, OC_ID)
}
//------------------End------------------//
//------------------End------------------//

//------------------Delivert Section------------------//
function BindDelivertSchItemList() {
    Cmn_BindDelivertSchItemList("PoItmDetailsTbl", "N", "SNohiddenfiled", "POItemListName", "ord_qty_spec")
};
function OnClickAddDeliveryDetail() {
    Cmn_OnClickAddDeliveryDetail("PoItmDetailsTbl", "N", "SNohiddenfiled", "POItemListName", "ord_qty_spec");
}
function OnClickReplicateDeliveryShdul() {
    Cmn_OnClickReplicateDeliveryShdul("PoItmDetailsTbl", "N", "SNohiddenfiled", "POItemListName", "ord_qty_spec");
}
function DeleteDeliverySch() {
    Cmn_DeleteDeliverySch();
}
function OnChangeDelSchItm() {
    Cmn_OnChangeDelSchItm("PoItmDetailsTbl", "N", "SNohiddenfiled", "POItemListName", "ord_qty_spec");
}
function OnChangeDeliveryDate(DeliveryDate) {
    Cmn_OnChangeDeliveryDate(DeliveryDate);
}
function OnChangeDeliveryQty() {
    Cmn_OnChangeDeliveryQty("PoItmDetailsTbl", "N", "SNohiddenfiled", "POItemListName", "ord_qty_spec");
}
function DelDeliSchAfterDelItem(ItemID) {
    Cmn_DelDeliSchAfterDelItem(ItemID, "PoItmDetailsTbl", "N", "SNohiddenfiled", "POItemListName", "ord_qty_spec");
}
//------------------End------------------//

function appendTermsAndCondition(rowIdx, TandC) {
    var deletetext = $("#Span_Delete_Title").text();
    $('#TblTerms_Condition tbody').append(`<tr id="R${rowIdx}" class="even pointer">
<td class="red"><i class="deleteIcon fa fa-trash" id="TC_DelIcon" title="${deletetext}"></i> </td>
<td>${TandC}</td>
</tr>`);
}
function EnableDeleteAction() {
    DeleteTermCondition();
    DeleteDeliverySch();
    Cmn_EnableDeleteAttatchFiles();
    //$("#PartialImageBind .file-preview .file-preview-thumbnails").each(function () {
    //    var Current = $(this);
    //    var filename = Current.find(".file-caption-info")[0].innerText;
    //    Current.find("#RemoveAttatchbtn").attr("onclick", "RemoveAttatchFile(event,'" + filename + "')");
    //});
    //$("#RemoveAttatchbtn").attr("onclick", "RemoveAttatchFile(event,'')")
}
function Ass_valFloatValueonly(el, evt) {
    let ValDigit = import_ord_type ? "#ExpImpValDigit" : "#ValDigit";//Added by Suraj on 22-10-2024
    if (Cmn_FloatValueonly(el, evt, ValDigit) == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //if (Cmn_CheckSelectedTextInTextField(evt) == true) {
    //    return true;
    //};
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_ass_valError" + RowNo).css("display", "none");
    clickedrow.find("#item_ass_val" + RowNo).css("border-color", "#ced4da");

    //var RatelDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(RatelDecDigit) - 1)) {
    //    return false;
    //}
    return true;
}
function ExchRateFloatValueonly(el, evt) {

    if (Cmn_FloatValueonly(el, evt, "#ExchDigit") == false) {
        return false;
    }
    $("#SpanSuppExRateErrorMsg").css("display", "none");
    $("#conv_rate").css("border-color", "#ced4da");
    return true;
}
function RateFloatValueonly(el, evt) {
    let RateDigit = import_ord_type ? "#ExpImpRateDigit" : "#RateDigit";//Added by Suraj on 22-10-2024
    if (Cmn_FloatValueonly(el, evt, RateDigit) == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //if (Cmn_CheckSelectedTextInTextField(evt) == true) {
    //    return true;
    //};
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();
    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#item_rateError" + RowNo).css("display", "none");
    clickedrow.find("#item_rate" + RowNo).css("border-color", "#ced4da");

    //var RatelDecDigit = $("#RateDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(RatelDecDigit) - 1)) {
    //    return false;
    //}
    return true;
}
function AmountFloatQty(el, evt) {
    let QtyDigit = import_ord_type ? "#ExpImpQtyDigit" : "#QtyDigit";//Added by Suraj on 22-10-2024
    if (Cmn_FloatValueonly(el, evt, QtyDigit) == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //if (Cmn_CheckSelectedTextInTextField(evt) == true) {
    //    return true;
    //};
    //debugger;
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");
    clickedrow.find("#ord_qty_specError" + RowNo).css("display", "none");
    clickedrow.find("#ord_qty_spec" + RowNo).css("border-color", "#ced4da");

    ////var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}
    return true;
}
function AmtFloatValueonly(el, evt) {
    let ValDigit = import_ord_type ? "#ExpImpValDigit" : "#ValDigit";//Added by Suraj on 22-10-2024
    if (Cmn_FloatValueonly(el, evt, ValDigit) == false) {
        return false;
    }
    var clickedrow = $(evt.target).closest("tr");
    var key = evt.key;
    var number = el.value.split('.');
    var item_rate = clickedrow.find("#item_rate").val();
    item_rate = CheckNullNumber(item_rate);

    var selectedval = Cmn_SelectedTextInTextField(evt);
    if (number.length == 1) {
        var valPer = number[0] + '' + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;
        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }

    }
    else {
        var valPer = el.value + key;
        var KeyLocation = evt.currentTarget.selectionStart;
        valPer = el.value.splice(KeyLocation, 0, key);
        valPer = Cmn_CheckSelectedTextInTextField(evt) == true ? el.value.replace(selectedval, key) : valPer;

        if (parseFloat(valPer) >= parseFloat(item_rate)) {
            return false;
        }
    }
    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#item_disc_amtError" + RowNo).css("display", "none");
    clickedrow.find("#item_disc_amt" + RowNo).css("border-color", "#ced4da");

    return true;
}
function FloatValuePerOnly(el, evt) {
    // var charCode = (evt.which) ? evt.which : event.keyCode;
    $("#SpanTaxPercent").css("display", "none");
    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    ////debugger;
    //var key = evt.key;
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > 1) {
    //    return false;
    //}
    if (Cmn_PercentageValueonly(el, evt) == false) {
        return false;
    } else {
        return true;
    }

}
function AddNewRow() {
    var rowIdx = 0;
    //debugger;
    /*var origin = window.location.origin + "/Content/Images/Calculator.png";*/
    var rowCount = $('#PoItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#PoItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
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
    AddNewRowForEditPOItem('');
    BindPOItmList(RowNo);

};
function AddNewRowForEditPOItem(sub_item) {
    var rowIdx = 0;
    debugger;
    var docid = $("#DocumentMenuId").val();
    var displaynoneinDomestic = "";
    var toggledisplay = "";
    if (docid != "105101140101") {
        displaynoneinDomestic = 'style="display: none;"';
    }
    //if ($("#ddlconsumable").is(":checked")) {
    //    toggledisplay = 'style="display: none;"';
    //}
    if (docid == "105101136") {
        toggledisplay = 'style="display: none;"';
    }
    /* var origin = window.location.origin + "/Content/Images/Calculator.png";*/
    var rowCount = $('#PoItmDetailsTbl >tbody >tr').length + 1;
    var RowNo = 0;
    var levels = [];
    $("#PoItmDetailsTbl >tbody >tr").each(function (i, row) {
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
    var CountRows = RowNo;
    RowNo = "";
    var deletetext = $("#Span_Delete_Title").text();
    var span_SubItemDetail = $("#span_SubItemDetail").text();
    var FCloseAccess = $("#FCloseAccess").val();
    var FcloseHtml = "";
    if (FCloseAccess == "Y") {
        FcloseHtml = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onchange="CheckedItemWiseFClose()" id="ItmFClosed${RowNo}" disabled><label class="custom-control-label for=" ItmFClosed${RowNo}" > </label></div></td>'
    }
    var ManualGst = "";
    var TaxExempted = "";
    //let SampleIssueAndMRS = "";
    var GstApplicable = $("#Hdn_GstApplicable").text();
    var mrp = "";
    var packSize = "";
    if (docid != "105101140101") {
        if (GstApplicable == "Y") {
            ManualGst = '<td class="qt_to"><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickManualGSTCheckBox(event)" id="ManualGST"><label class="custom-control-label" disabled for="" style="padding:3px 0px;"> </label></div></td>'
        }
        TaxExempted = '<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" onclick="OnClickTaxExemptedCheckBox(event)" id="TaxExempted"><label class="custom-control-label" disabled for="" style="padding: 3px 0px;"> </label></div></td>'
        //SampleIssueAndMRS = `<td><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" id="SimpleIssue${RowNo}"><label class="custom-control-label " for="SimpleIssue${RowNo}"> </label></div></td>
        //                    <td><input id="MRSNumber${RowNo}" class="form-control date" autocomplete="off" type="text" name="MRSNumber"  placeholder="${$("#span_MRSNumber").text()}"  onblur="this.placeholder='${$("#span_MRSNumber").text()}'" disabled></td>`

        mrp = ` <td>
                                                                        <input id="mrp" class="form-control num_right" autocomplete="off" type="text" name="mrp" placeholder="${$("#span_MRP").text()}" value="" disabled>
                                                                    </td>`
        packSize = ` <td>
                                                                        <input id="PackSize" class="form-control" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="" onmouseover="OnMouseOver(this)" disabled>
                                                                    </td>`
    }
    var SaleHistory = "";
    if ($("#SaleHistoryFeatureId").val() == "true") {
        SaleHistory = '<button type="button" id="SaleHistory" class="calculator" onclick="OnClickHistoryIconBtn(event);" data-toggle="modal" data-target="#SaleHistory" data-backdrop="static" data-keyboard="false"> <i class="fa fa-history" aria-hidden="true" data-toggle="" title="${$("#span_PurchaseHistory_Title").text()}"></i> </button>'
    }
    var subitem_val = "";
    var Sourcetyp = $("#src_type").val();
    if (Sourcetyp == "D") {
        subitem_val = `<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrderSpecQty" ${toggledisplay}>
    <button type="button" id="SubItemOrderSpecQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return SubItemDetailsPopUp('QuantitySpec',event)" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>
</div>`
    }
    else if (Sourcetyp == "PR") {
        subitem_val = `<div class="col-sm-2 i_Icon no-padding" id="div_SubItemPOPROrdQty" ${toggledisplay}>
                <input hidden type="text" id="sub_item" value="${sub_item}" />
                <button type="button" id="SubItemPOPROrdQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('POPROrdQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>
                </div >`

    }
    else {
        subitem_val = `<div class="col-sm-2 i_Icon no-padding" id="div_SubItemPOQTOrdQty">
                <input hidden type="text" id="sub_item" value="${sub_item}" />
                <button type="button" id="SubItemPOQTOrdQty" disabled class="calculator subItmImg" onclick="return SubItemDetailsPopUp('POQTOrdQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$(" #span_SubItemDetail").text()}"> </button>
                </div >`
    }
    if (docid == "105101136") {
        $('#PoItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${CountRows}" title="${deletetext}"></i></td>
<td id="srno" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${CountRows}" /></td>
<td style="display: none;"><input class="" type="hidden" id="hfItemID" /><input hidden type="text" id="sub_item" value="" /></td>
<td class="itmStick"><div class="col-sm-10 lpo_form no-padding">
<select class="form-control" id="POItemListName${CountRows}" name="POItemListName${CountRows}" onchange="OnChangePOItemName(${CountRows},event)" ></select>
<span id="POItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 no-padding">
<div class="col-sm-4 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
</div>
<div class="col-sm-4 i_Icon"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button>
</div><div class="col-sm-4 i_Icon">
`+ SaleHistory + `
</div>
</div>
</td>
<td>
<select id="UOM${RowNo}" class="form-control date" onchange="onChangeUom(event)">
    <option value="0">---Select---</option>
</select>
<input id="UOMID${RowNo}" type="hidden" />
<input id="ItemHsnCode${RowNo}" type="hidden" />
<input id="ItemtaxTmplt" type="hidden" />
 <input id="hdnItemtype" type="hidden" />
</td>
<td  ${toggledisplay}>
 <div class="col-sm-12 lpo_form no-padding">
  <input id="IDItemtype" class="form-control num_right" autocomplete="off" type="text" name="Itemtype" placeholder="${$("#ItemType").text()}" readonly>
   </div>
 </td>
`+ mrp + `
`+ packSize +`
<td ${toggledisplay}>
<div class="col-sm-8 lpo_form no-padding ">
<input id="AvailableStockInBase${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="AvailableStockInBase"  placeholder="0000.00"  readonly>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
    <button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
<div class="col-sm-2 i_Icon no-padding">
<button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Warehouse Wise Stock"> </button>
</div>
</td>
<td>
<div class="col-sm-12 lpo_form no-padding" >
<input id="ord_qty_spec${RowNo}" class="form-control date num_right" autocomplete="off" onchange ="OnChangePOItemQty(1,event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="ord_qty_specError${RowNo}" class="error-message is-visible"></span>
</div>
  `+ subitem_val + `
</td>
<td style="display:${(docid == "105101130" || docid == "105101136") ? "none" : ""}">
<div class="col-sm-12 lpo_form no-padding">
<input id="ord_qty_base${RowNo}" class="form-control date num_right " autocomplete="off" type="text" name="ord_qty_base"  placeholder="0000.00"  disabled>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrderBaseQty" ${toggledisplay}>
    <button type="button" id="SubItemOrderBaseQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return SubItemDetailsPopUp('QuantityBase',event)" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td style="display:none;"><input id="QCQuantity${RowNo}" class="form-control" autocomplete="off" type="text" name="QCQuantity"  placeholder="0000.00"  disabled></td>
<td><div class="lpo_form"><input id="item_rate${RowNo}" class="form-control date num_right" maxlength="10" onchange ="OnChangePOItemRate(1,event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"> <span id="item_rateError${RowNo}" class="error-message is-visible"></span></div></td>
<td><input id="item_disc_perc${RowNo}" class="form-control date num_right" onchange ="OnChangePOItemDiscountPerc(1,event)" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"></td>
<td> <div class="lpo_form"><input id="item_disc_amt${RowNo}" class="form-control date num_right" onchange ="OnChangePOItemDiscountAmt(1,event)" autocomplete="off" onkeypress="return AmtFloatValueonly(this,event);" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="item_disc_amtError${RowNo}" class="error-message is-visible"></span></div></td>
<td><input id="item_disc_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_disc_val"  placeholder="0000.00"  disabled></td>
<td><input id="item_gr_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_gr_val"  placeholder="0000.00"  disabled></td>
<td hidden><div class="lpo_form"><input id="item_ass_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text"  name="item_ass_val" disabled placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="item_ass_valError${RowNo}" class="error-message is-visible"></span></div></td>
`+ TaxExempted + `
`+ ManualGst + `
<td><div class="col-sm-10 no-padding"><input id="item_tax_amt${RowNo}" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"  disabled></div><div class="col-sm-2 no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation${RowNo}"
disabled="disabled" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="${$("#Span_TaxCalculator_Title").text()}" data-original-title="${$("#Span_TaxCalculator_Title").text()}"></i></button></div></td>
<td><input id="item_oc_amt${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_oc_amt"  placeholder="0000.00"  disabled></td>

<td ${displaynoneinDomestic}><input id="item_net_val_spec${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_spec"  placeholder="0000.00"  disabled></td>

<td><input id="item_net_val_bs${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_bs"  placeholder="0000.00"  disabled></td>

`+ FcloseHtml + `
<td><textarea id="remarks${RowNo}"  class="form-control remarksmessage" name="remarks" maxlength = "300", onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}"   data-parsley-validation-threshold="10" ></textarea></td>
</tr>`);
        $("#POItemListName" + CountRows).focus()

    }
//    <td ${toggledisplay}><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" id="SimpleIssue${RowNo}"><label class="custom-control-label " for="SimpleIssue${RowNo}"> </label></div></td>
//<td ${toggledisplay}><input id="MRSNumber${RowNo}" class="form-control date" autocomplete="off" type="text" name="MRSNumber"  placeholder="${$("#span_MRSNumber").text()}"  onblur="this.placeholder='${$("#span_MRSNumber").text()}'" disabled></td>

        //    else if ($("#ddlconsumable").is(":checked")) {

    //        $('#PoItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
    //<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${CountRows}" title="${deletetext}"></i></td>
    //<td id="srno" class="sr_padding">${rowCount}</td>
    //<td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${CountRows}" /></td>
    //<td style="display: none;"><input class="" type="hidden" id="hfItemID" /><input hidden type="text" id="sub_item" value="" /></td>
    //<td><div class="col-sm-10 lpo_form no-padding">
    //<select class="form-control" id="POItemListName${CountRows}" name="POItemListName${CountRows}" onchange="OnChangePOItemName(${CountRows},event)" ></select>
    //<span id="POItemListNameError" class="error-message is-visible"></span>
    //</div>
    //<div class="col-sm-2 no-padding">
    //<div class="col-sm-4 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
    //</div>
    //<div class="col-sm-4 i_Icon"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button>
    //</div><div class="col-sm-4 i_Icon">
    //`+ SaleHistory + `
    //</div>
    //</div>
    //</td>
    //<td>
    //<select id="UOM${RowNo}" class="form-control date" onchange="onChangeUom(event)">
    //    <option value="0">---Select---</option>
    //</select>
    //<input id="UOMID${RowNo}" type="hidden" />
    //<input id="ItemHsnCode${RowNo}" type="hidden" />
    //<input id="ItemtaxTmplt" type="hidden" />
    // <input id="hdnItemtype" type="hidden" />
    //</td>
    //<td ${toggledisplay}>
    //<div class="col-sm-10 lpo_form no-padding ">
    //<input id="AvailableStockInBase${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="AvailableStockInBase"  placeholder="0000.00"  readonly>
    //</div>
    //<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
    //    <button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
    //</div>
    //</td>
    //<td>
    //<div class="col-sm-12 lpo_form no-padding" >
    //<input id="ord_qty_spec${RowNo}" class="form-control date num_right" autocomplete="off" onchange ="OnChangePOItemQty(1,event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="ord_qty_specError${RowNo}" class="error-message is-visible"></span>
    //</div>
    //  `+ subitem_val + `
    //</td>
    //<td style="display:${docid == "105101130"?"none":""}">
    //<div class="col-sm-12 lpo_form no-padding">
    //<input id="ord_qty_base${RowNo}" class="form-control date num_right " autocomplete="off" type="text" name="ord_qty_base"  placeholder="0000.00"  disabled>
    //</div>
    //<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrderBaseQty" ${toggledisplay}>
    //    <button type="button" id="SubItemOrderBaseQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return SubItemDetailsPopUp('QuantityBase',event)" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
    //</div>
    //</td>
    //<td style="display:none;"><input id="QCQuantity${RowNo}" class="form-control" autocomplete="off" type="text" name="QCQuantity"  placeholder="0000.00"  disabled></td>
    //<td><div class="lpo_form"><input id="item_rate${RowNo}" class="form-control date num_right" maxlength="10" onchange ="OnChangePOItemRate(1,event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"> <span id="item_rateError${RowNo}" class="error-message is-visible"></span></div></td>
    //<td><input id="item_disc_perc${RowNo}" class="form-control date num_right" onchange ="OnChangePOItemDiscountPerc(1,event)" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"></td>
    //<td> <div class="lpo_form"><input id="item_disc_amt${RowNo}" class="form-control date num_right" onchange ="OnChangePOItemDiscountAmt(1,event)" autocomplete="off" onkeypress="return AmtFloatValueonly(this,event);" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="item_disc_amtError${RowNo}" class="error-message is-visible"></span></div></td>
    //<td><input id="item_disc_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_disc_val"  placeholder="0000.00"  disabled></td>
    //<td><input id="item_gr_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_gr_val"  placeholder="0000.00"  disabled></td>
    //<td hidden><div class="lpo_form"><input id="item_ass_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text"  name="item_ass_val" disabled placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="item_ass_valError${RowNo}" class="error-message is-visible"></span></div></td>
    //`+ TaxExempted + `
    //`+ ManualGst + `
    //<td><div class="col-sm-10 no-padding"><input id="item_tax_amt${RowNo}" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"  disabled></div><div class="col-sm-2 no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation${RowNo}" disabled="disabled" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="${$("#Span_TaxCalculator_Title").text()}" data-original-title="${$("#Span_TaxCalculator_Title").text()}"></i></button></div></td>
    //<td><input id="item_oc_amt${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_oc_amt"  placeholder="0000.00"  disabled></td>

    //<td ${displaynoneinDomestic}><input id="item_net_val_spec${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_spec"  placeholder="0000.00"  disabled></td>

    //<td><input id="item_net_val_bs${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_bs"  placeholder="0000.00"  disabled></td>
    //<td ${toggledisplay}><div class="custom-control custom-switch sample_issue"><input type="checkbox" class="custom-control-input margin-switch" id="SimpleIssue${RowNo}"><label class="custom-control-label " for="SimpleIssue${RowNo}"> </label></div></td>
    //<td ${toggledisplay}><input id="MRSNumber${RowNo}" class="form-control date" autocomplete="off" type="text" name="MRSNumber"  placeholder="${$("#span_MRSNumber").text()}"  onblur="this.placeholder='${$("#span_MRSNumber").text()}'" disabled></td>
    //`+ FcloseHtml + `
    //<td><textarea id="remarks${RowNo}"  class="form-control remarksmessage" name="remarks" maxlength = "300", onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}"   data-parsley-validation-threshold="10" ></textarea></td>
    //</tr>`);
    //        $("#POItemListName" + CountRows).focus()

    //    }
    else {

        $('#PoItmDetailsTbl tbody').append(`<tr id="R${++rowIdx}">
<td class=" red center"> <i class="deleteIcon fa fa-trash" aria-hidden="true" id="delBtnIcon${CountRows}" title="${deletetext}"></i></td>
<td id="srno" class="sr_padding">${rowCount}</td>
<td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${CountRows}" /></td>
<td style="display: none;"><input class="" type="hidden" id="hfItemID" /><input hidden type="text" id="sub_item" value="" /></td>
<td class="itmStick"><div class="col-sm-10 lpo_form no-padding">
<select class="form-control" id="POItemListName${CountRows}" name="POItemListName${CountRows}" onchange="OnChangePOItemName(${CountRows},event)" ></select>
<span id="POItemListNameError" class="error-message is-visible"></span>
</div>
<div class="col-sm-2 no-padding">
<div class="col-sm-4 i_Icon"><button type="button" id="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
</div>
<div class="col-sm-4 i_Icon"><button type="button" id="SupplierInformation" class="calculator" onclick="OnClickSupplierInfoIconBtn(event);" data-toggle="modal" data-target="#SupplierInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_SupplierInformation_Title").text()}"></i> </button>
</div><div class="col-sm-4 i_Icon">
`+ SaleHistory + `
</div>
</div>
</td>
<td>
<select id="UOM${RowNo}" class="form-control date" onchange="onChangeUom(event)">
    <option value="0">---Select---</option>
</select>

<input id="UOMID${RowNo}" type="hidden" />
<input id="ItemHsnCode${RowNo}" type="hidden" />
<input id="ItemtaxTmplt" type="hidden" />
 <input id="hdnItemtype" type="hidden" />
</td>
<td>
<div class="col-sm-12 lpo_form no-padding">
<input id="IDItemtype" class="form-control num_right" autocomplete="off" type="text" name="Itemtype" placeholder="${$("#ItemType").text()}" readonly>
</div>

</td>
`+ mrp + `
`+ packSize +`
<td>
<div class="col-sm-8 lpo_form no-padding">
<input id="AvailableStockInBase${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="AvailableStockInBase"  placeholder="0000.00"  readonly>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlQty">
    <button type="button" id="SubItemAvlQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return GetSubItemAvlStock(event)" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>

<div class="col-sm-2 i_Icon no-padding">
<button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#ItemStockLotBatchSrWise" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="Warehouse Wise Stock"> </button>
</div>
</td>
<td>
<div class="col-sm-10 lpo_form no-padding" >
<input id="ord_qty_spec${RowNo}" class="form-control date num_right" autocomplete="off" onchange ="OnChangePOItemQty(1,event)" onkeypress="return AmountFloatQty(this,event);" type="text" name="ord_qty_spec"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="ord_qty_specError${RowNo}" class="error-message is-visible"></span>
</div>
  `+ subitem_val + `
</td>
<td style="display:${docid == "105101130" ? "none" : ""}">
<div class="col-sm-10 lpo_form no-padding">
<input id="ord_qty_base${RowNo}" class="form-control date num_right " autocomplete="off" type="text" name="ord_qty_base"  placeholder="0000.00"  disabled>
</div>
<div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrderBaseQty" >
    <button type="button" id="SubItemOrderBaseQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return SubItemDetailsPopUp('QuantityBase',event)" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
</div>
</td>
<td style="display:none;"><input id="QCQuantity${RowNo}" class="form-control" autocomplete="off" type="text" name="QCQuantity"  placeholder="0000.00"  disabled></td>
<td><div class="lpo_form"><input id="item_rate${RowNo}" class="form-control date num_right" maxlength="10" onchange ="OnChangePOItemRate(1,event)" autocomplete="off" onkeypress="return RateFloatValueonly(this,event);" type="text" name="item_rate"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"> <span id="item_rateError${RowNo}" class="error-message is-visible"></span></div></td>
<td><input id="item_disc_perc${RowNo}" class="form-control date num_right" onchange ="OnChangePOItemDiscountPerc(1,event)" onkeypress="return FloatValuePerOnly(this,event);" autocomplete="off" type="text" name="item_disc_perc"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"></td>
<td> <div class="lpo_form"><input id="item_disc_amt${RowNo}" class="form-control date num_right" onchange ="OnChangePOItemDiscountAmt(1,event)" autocomplete="off" onkeypress="return AmtFloatValueonly(this,event);" onkeypress="return AmtFloatValueonly(this,event);" type="text" name="item_disc_amt"  placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="item_disc_amtError${RowNo}" class="error-message is-visible"></span></div></td>
<td><input id="item_disc_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_disc_val"  placeholder="0000.00"  disabled></td>
<td><input id="item_gr_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_gr_val"  placeholder="0000.00"  disabled></td>
<td hidden><div class="lpo_form"><input id="item_ass_val${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_ass_val" disabled placeholder="0000.00"  onblur="this.placeholder='0000.00'"><span id="item_ass_valError${RowNo}" class="error-message is-visible"></span></div></td>
`+ TaxExempted + `
`+ ManualGst + `
<td><div class="col-sm-10 no-padding"><input id="item_tax_amt${RowNo}" class="form-control num_right" autocomplete="off" type="text" name="item_tax_amt"  placeholder="0000.00"  disabled></div><div class="col-sm-2 no-padding"> <button type="button" class="calculator" id="BtnTxtCalculation${RowNo}" disabled="disabled" onclick="OnClickTaxCalculationBtn(event)" data-toggle="modal" data-target="#ItemCalculate" data-backdrop="static" data-keyboard="false"><i class="fa fa-calculator" aria-hidden="true" data-toggle="tooltip" title="${$("#Span_TaxCalculator_Title").text()}" data-original-title="${$("#Span_TaxCalculator_Title").text()}"></i></button></div></td>
<td><input id="item_oc_amt${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_oc_amt"  placeholder="0000.00"  disabled></td>

<td ${displaynoneinDomestic}><input id="item_net_val_spec${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_spec"  placeholder="0000.00"  disabled></td>

<td><input id="item_net_val_bs${RowNo}" class="form-control date num_right" autocomplete="off" type="text" name="item_net_val_bs"  placeholder="0000.00"  disabled></td>

`+ FcloseHtml + `
<td><textarea id="remarks${RowNo}"  class="form-control remarksmessage" name="remarks" maxlength = "300", onmouseover="OnMouseOver(this)" placeholder="${$("#span_remarks").text()}"   data-parsley-validation-threshold="10" ></textarea></td>
</tr>`);
        $("#POItemListName" + CountRows).focus()
    }
    // //onchange = "OnChangePOItemName(${RowNo},event)"

    //        < div class="col-sm-2 i_Icon no-padding" id = "div_SubItemOrderSpecQty" >
    //            <button type="button" id="SubItemOrderSpecQty" disabled class="calculator subItmImg" data-toggle="modal" onclick="return SubItemDetailsPopUp('QuantitySpec',event)" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
    //</div>
};
function CheckPOValidations() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var itemtype_toggle = "";
    if ($("#ddlstockable").is(":checked")) {
        itemtype_toggle = "S";
        $("#ddlstockable").val(itemtype_toggle);

    }
    if ($("#ddlconsumable").is(":checked")) {
        itemtype_toggle = "C";
        $("#ddlconsumable").val(itemtype_toggle);
    }
    var itemtyp_toggle = $("#ddlstockable").val();
    if (docid == "105101136") {
        itemtyp_toggle = "C";
        $("#ddlconsumable").val("C");
    }
    $("#hdn_item_type").val(itemtyp_toggle);
    var ErrorFlag = "N";
    var src_type = $("#src_type").val().trim();
    var src_type = $("#src_type").val().trim();
    if (src_type == "Q" || src_type == "PR") {
        if ($("#src_doc_number").val() == "0" || $("#src_doc_number").val() == "---Select---") {
            $('#SpanSourceDocNoErrorMsg').text($("#valueReq").text());
            $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "red")
            $("#SpanSourceDocNoErrorMsg").css("display", "block");
            ErrorFlag = "Y";
        }
        else {
            $("#SpanSourceDocNoErrorMsg").css("display", "none");
            $("#src_doc_number").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da")
        }

    }
    else {
        $("#SpanSourceDocNoErrorMsg").css("display", "none");
        $("#src_doc_number").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-src_doc_number-container']").css("border-color", "#ced4da")
    }
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

        if ($("#Address").val() === "" || $("#Address").val() === null) {
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
    if ($("#ddlCurrency").val() == null || $("#ddlCurrency").val() == "") {
        $("#ddlCurrency").css("border-color", "Red");
        $('#SpanSuppCurrErrorMsg').text($("#valueReq").text());
        $("#SpanSuppCurrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppCurrErrorMsg").css("display", "none");
        $("#ddlCurrency").css("border-color", "#ced4da");
    }
    if (parseFloat(CheckNullNumber($("#conv_rate").val())) === 0 || $("#conv_rate").val() == "") {
        $('#SpanSuppExRateErrorMsg').text($("#ValueShouldBeGreaterThan0").text());
        $("#SpanSuppExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }
    if ($("#ValidUptoDate").val() == "") {
        $('#SpanValidUpToErrorMsg').text($("#valueReq").text());
        $("#SpanValidUpToErrorMsg").css("display", "block");
        $("#ValidUptoDate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanValidUpToErrorMsg").css("display", "none");
        $("#ValidUptoDate").css("border-color", "#ced4da");
    }
    if ($("#Cancelled").is(":checked"))
    {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            ErrorFlag = "Y";
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
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
function OnClickTaxExemptedCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#POItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_ass_val").val();

    $("#HdnTaxOn").val("Item");
    $("#TaxCalcItemCode").val(ItmCode);
    $("#Tax_AssessableValue").val(AssAmount);
    $("#HiddenRowSNo").val(RowSNo)
    currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
    var GstApplicable = $("#Hdn_GstApplicable").text();
    if (currentrow.find("#TaxExempted").is(":checked")) {
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find("#item_rate").trigger('change');
        currentrow.find("#BtnTxtCalculation").prop("disabled", true);
        if (GstApplicable == "Y") {
            currentrow.find('#ManualGST').prop("checked", false);
        }
    }
    else {
        currentrow.find("#BtnTxtCalculation").prop("disabled", false);
        //if ($("#taxTemplate").text() == "GST Slab") {
        if (GstApplicable == "Y") {
            var gst_number = $("#Ship_Gst_number").val()
            Cmn_OnSaveAddressApplyGST(gst_number, "PoItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val")
            //}
            //CalculationBaseRate(e)
            //CalculateAmount();
        }
        else {
            $("#Tax_ItemID").val(ItmCode);
            var ItmTmplt_id = currentrow.find("#ItemtaxTmplt").val();
            OnChangeTaxTmpltDdl(ItmTmplt_id);
        }
    }
}
function OnClickManualGSTCheckBox(e) {
    debugger;
    var currentrow = $(e.target).closest('tr');
    var RowSNo = currentrow.find("#SNohiddenfiled").val();
    var ItmCode = currentrow.find("#POItemListName" + RowSNo).val();
    var AssAmount = currentrow.find("#item_ass_val").val();
    if (currentrow.find("#ManualGST").is(":checked")) {
        $("#taxTemplate").text("Template");
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find("#item_rate").trigger('change');
        currentrow.find("#item_tax_amt").val(parseFloat(0).toFixed(ValDecDigit));
        currentrow.find('#TaxExempted').prop("checked", false);
    }
    else {
        currentrow.find('#BtnTxtCalculation').css('border', '#ced4da');
        //var gst_number = $("#ship_add_gstNo").val()
        var gst_number = $("#Ship_Gst_number").val()
        Cmn_OnSaveAddressApplyGST(gst_number, "PoItmDetailsTbl", "hfItemID", "SNohiddenfiled", "item_ass_val");
        var trgtrow = $(e.target).closest("tr");
        CalculationBaseRate(trgtrow)
        CalculateAmount();
        $("#taxTemplate").text("Template");
    }
}
function CheckPOHraderValidations() {
    //debugger;
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SupplierName").css("border-color", "Red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
    }
    if ($("#ddlCurrency").val() == null || $("#ddlCurrency").val() == "") {
        $("#ddlCurrency").css("border-color", "Red");
        $('#SpanSuppCurrErrorMsg').text($("#valueReq").text());
        $("#SpanSuppCurrErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppCurrErrorMsg").css("display", "none");
        $("#ddlCurrency").css("border-color", "#ced4da");
    }
    if ($("#conv_rate").val() === "0" || $("#conv_rate").val() == "") {
        $('#SpanSuppExRateErrorMsg').text($("#valueReq").text());
        $("#SpanSuppExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }
}
function CheckPOItemValidations() {
    debugger;
    //var RateDecDigit = $("#RateDigit").text();
    //var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    var itemtypeFlag = "N";
    if ($("#PoItmDetailsTbl >tbody >tr").length > 0) {
        let isFocused = false;
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();

            if (currentRow.find("#POItemListName" + Sno).val() == "0") {
                currentRow.find("#POItemListNameError").text($("#valueReq").text());
                currentRow.find("#POItemListNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#POItemListNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (currentRow.find("#ord_qty_spec").val() == "") {
                currentRow.find("#ord_qty_specError").text($("#valueReq").text());
                currentRow.find("#ord_qty_specError").css("display", "block");
                currentRow.find("#ord_qty_spec").css("border-color", "red");
                currentRow.find("#ord_qty_spec").focus();
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ord_qty_specError").css("display", "none");
                currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
            }
            if (currentRow.find("#ord_qty_spec").val() != "") {
                if (parseFloat(currentRow.find("#ord_qty_spec").val()).toFixed(QtyDecDigit) == 0) {
                    currentRow.find("#ord_qty_specError").text($("#valueReq").text());
                    currentRow.find("#ord_qty_specError").css("display", "block");
                    currentRow.find("#ord_qty_spec").css("border-color", "red");
                    currentRow.find("#ord_qty_spec").focus();
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#ord_qty_specError").css("display", "none");
                    currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
                }
            }
            if (currentRow.find("#item_rate").val() == "") {
                currentRow.find("#item_rateError").text($("#valueReq").text());
                currentRow.find("#item_rateError").css("display", "block");
                currentRow.find("#item_rate").css("border-color", "red");
                if (currentRow.find("#ord_qty_spec").val() != "" && currentRow.find("#ord_qty_spec").val() != null) {
                    currentRow.find("#item_rate").focus();
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_rateError").css("display", "none");
                currentRow.find("#item_rate").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_rate").val() != "") {
                if (parseFloat(currentRow.find("#item_rate").val()) == 0) {
                    currentRow.find("#item_rateError").text($("#valueReq").text());
                    currentRow.find("#item_rateError").css("display", "block");
                    currentRow.find("#item_rate").css("border-color", "red");
                    if (!isFocused) {
                        currentRow.find("#item_rate").focus();
                        isFocused = true;
                    }
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_rateError").css("display", "none");
                    currentRow.find("#item_rate").css("border-color", "#ced4da");
                }
            }
            if (parseFloat(currentRow.find("#item_disc_amt").val()) > 0) {
                if (parseFloat(currentRow.find("#item_disc_amt").val()) >= parseFloat(currentRow.find("#item_rate").val())) {
                    currentRow.find("#item_disc_amtError").text($("#GDAmtTORate").text());
                    currentRow.find("#item_disc_amtError").css("display", "block");
                    currentRow.find("#item_disc_amt").css("border-color", "red");
                    if (!isFocused) {
                        currentRow.find("#item_disc_amt").focus();
                        isFocused = true;
                    }
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_disc_amtError").css("display", "none");
                    currentRow.find("#item_disc_amt").css("border-color", "#ced4da");
                }
            }
            if (currentRow.find("#item_ass_val").val() == "") {
                currentRow.find("#item_ass_valError").text($("#valueReq").text());
                currentRow.find("#item_ass_valError").css("display", "block");
                currentRow.find("#item_ass_val").css("border-color", "red");
                if (!isFocused) {
                    currentRow.find("#item_ass_val").focus();
                    isFocused = true;
                }
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#item_ass_valError").css("display", "none");
                currentRow.find("#item_ass_val").css("border-color", "#ced4da");
            }
            if (currentRow.find("#item_ass_val").val() != "") {
                if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(RateDecDigit) == 0) {
                    currentRow.find("#item_ass_valError").text($("#valueReq").text());
                    currentRow.find("#item_ass_valError").css("display", "block");
                    currentRow.find("#item_ass_val").css("border-color", "red");
                    if (!isFocused) {
                        currentRow.find("#item_ass_val").focus();
                        isFocused = true;
                    }
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#item_ass_valError").css("display", "none");
                    currentRow.find("#item_ass_val").css("border-color", "#ced4da");
                }
            }
            var docid = $("#DocumentMenuId").val();
            if (docid == "105101130") {
                var Itemtype = currentRow.find("#hdnItemtype").val();
                if (Itemtype == "Stockable" && itemtypeFlag != "Y") {

                    itemtypeFlag = "Y";
                }
            }
            else {
                itemtypeFlag = "Y";
            }

        });
    }
    else {
        swal("", $("#noitemselectedmsg").text(), "warning");
        ErrorFlag = "Y";
    }
    if (itemtypeFlag == "N") {
        swal("", $("#AtleastOneStockableItemIsRequiredInOrder").text(), "warning");
        ErrorFlag = "Y";
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {

        return true;
    }
}
function CheckItemRowValidation(e) {
    //debugger;
    //var RateDecDigit = $("#RateDigit").text();
    //var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    var currentRow = $(e.target).closest("tr");
    var Sno = currentRow.find("#SNohiddenfiled").val();

    if (currentRow.find("#POItemListName" + Sno).val() == "0") {
        currentRow.find("#POItemListNameError").text($("#valueReq").text());
        currentRow.find("#POItemListNameError").css("display", "block");
        currentRow.find("[aria-labelledby='select2-POItemListName" + Sno + "-container']").css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#POItemListNameError").css("display", "none");
        currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
    }
    if (currentRow.find("#ord_qty_spec").val() == "") {
        currentRow.find("#ord_qty_specError").text($("#valueReq").text());
        currentRow.find("#ord_qty_specError").css("display", "block");
        currentRow.find("#ord_qty_spec").css("border-color", "red");
        currentRow.find("#ord_qty_spec").focus();
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#ord_qty_specError").css("display", "none");
        currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
    }
    if (currentRow.find("#ord_qty_spec").val() != "") {
        if (parseFloat(currentRow.find("#ord_qty_spec").val()).toFixed(QtyDecDigit) == 0) {
            currentRow.find("#ord_qty_specError").text($("#valueReq").text());
            currentRow.find("#ord_qty_specError").css("display", "block");
            currentRow.find("#ord_qty_spec").css("border-color", "red");
            currentRow.find("#ord_qty_spec").focus();
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#ord_qty_specError").css("display", "none");
            currentRow.find("#ord_qty_specError").css("border-color", "#ced4da");
        }
    }
    if (currentRow.find("#item_rate").val() == "") {
        currentRow.find("#item_rateError").text($("#valueReq").text());
        currentRow.find("#item_rateError").css("display", "block");
        currentRow.find("#item_rate").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#item_rateError").css("display", "none");
        currentRow.find("#item_rate").css("border-color", "#ced4da");
    }
    if (currentRow.find("#item_rate").val() != "") {
        if (parseFloat(currentRow.find("#item_rate").val()).toFixed(RateDecDigit) == 0) {
            currentRow.find("#item_rateError").text($("#valueReq").text());
            currentRow.find("#item_rateError").css("display", "block");
            currentRow.find("#item_rate").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_rateError").css("display", "none");
            currentRow.find("#item_rate").css("border-color", "#ced4da");
        }
    }
    if (currentRow.find("#item_ass_val").val() == "") {
        currentRow.find("#item_ass_valError").text($("#valueReq").text());
        currentRow.find("#item_ass_valError").css("display", "block");
        currentRow.find("#item_ass_val").css("border-color", "red");
        ErrorFlag = "Y";
    }
    else {
        currentRow.find("#item_ass_valError").css("display", "none");
        currentRow.find("#item_ass_val").css("border-color", "#ced4da");
    }
    if (currentRow.find("#item_ass_val").val() != "") {
        if (parseFloat(currentRow.find("#item_ass_val").val()).toFixed(RateDecDigit) == 0) {
            currentRow.find("#item_ass_valError").text($("#valueReq").text());
            currentRow.find("#item_ass_valError").css("display", "block");
            currentRow.find("#item_ass_val").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#item_ass_valError").css("display", "none");
            currentRow.find("#item_ass_val").css("border-color", "#ced4da");
        }
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnClickIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    ItmCode = clickedrow.find("#POItemListName" + Sno).val();
    ItemInfoBtnClick(ItmCode);
}
function InsertPODetails() {
    try {

        debugger;
        var btn = $("#hdnsavebtn").val();
        var uptodate = $("#ValidUptoDate").val();
        var date = moment().format('YYYY-MM-DD');
        if (uptodate < date) {
            $("#btn_save").attr("disabled", false);
            $("#hdnsavebtn").val("");
        }
        else {
            if (btn == "AllreadyclickSaveBtn") {
                $("#btn_save").attr("disabled", true);
                $("#btn_save").css("filter", "grayscale(100%)");
                return false;
            }
        }
        //var ValDecDigit = $("#ValDigit").text();
        var PODTransType = sessionStorage.getItem("POTransType");
        var POStatus = "";
        var docid = $("#DocumentMenuId").val();
        if (CheckPOValidations() == false) {
            return false;
        }



        if (CheckPOItemValidations() == false) {
            return false;
        }
        else if (docid == "105101136") {

        }
        else {
            if (CheckValidations_forSubItems() == false) {
                return false;
            }
        }
        if (docid == "105101130" || docid == "105101136") {
            var GstApplicable = $("#Hdn_GstApplicable").text();
            if (GstApplicable == "Y") {
                if (Cmn_taxVallidation("PoItmDetailsTbl", "item_tax_amt", "POItemListName", "Hdn_TaxCalculatorTbl", "SNohiddenfiled") == false) {
                    return false;
                }
            }
        }
        if (Cmn_CheckDeliverySchdlValidations("PoItmDetailsTbl", "hfItemID", "POItemListName", "ord_qty_spec", "SNohiddenfiled") == false) {
            return false;
        }

        if (navigator.onLine === true)/*Checing For Internet is open or not*/ {

            Src_Type = $("#src_type").val();

            var FinalPOItemDetail = [];
            var FinalPODeliveryDetail = [];
            var FinalPOTaxDetail = [];
            var FinalPOOCDetail = [];
            var FinalPOOCTaxDetail = [];
            var FinalPOTermDetail = [];

            FinalPOItemDetail = InsertPOItemDetails();
            FinalPOTaxDetail = InsertPOTaxDetails();
            FinalPOOCDetail = InsertPOOtherChargeDetails();
            FinalPOOCTaxDetail = InsertPO_OCTaxDetails();
            FinalPODeliveryDetail = InsertPOItem_DeliverySchDetails();
            FinalPOTermDetail = InsertPOTermConditionDetails();

            $("#hdItemDetailList").val(JSON.stringify(FinalPOItemDetail));
            $("#hdTaxDetailList").val(JSON.stringify(FinalPOTaxDetail));
            $("#hdOCDetailList").val(JSON.stringify(FinalPOOCDetail));
            $("#hdOCTaxDetailList").val(JSON.stringify(FinalPOOCTaxDetail));
            $("#hdDelSchDetailList").val(JSON.stringify(FinalPODeliveryDetail));
            $("#hdTermsDetailList").val(JSON.stringify(FinalPOTermDetail));

            /*----- Attatchment start--------*/
            $(".fileinput-upload").click();/*To Upload Img in folder*/
            var FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

            var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);

            $("#hdn_Attatchment_details").val(ItemAttchmentDt);
            /*----- Attatchment End--------*/

            ///*-----------Sub-item-------------*/

            var SubItemsListArr = Cmn_SubItemList();
            var str2 = JSON.stringify(SubItemsListArr);
            $('#SubItemDetailsDt').val(str2);

            ///*-----------Sub-item end-------------*/

            $("#Hdn_SupplierName").val($("#SupplierName").val());
            $("#Hdn_src_type").val($("#src_type").val());
            $("#Hdn_ddlCurrency").val($("#ddlCurrency").val());
            $("#Hdn_conv_rate").val($("#conv_rate").val());
            if (Src_Type != "D") {
                $("#Hdn_src_doc_number").val($("#src_doc_number").val());
            }
            var SuppName = $("#SupplierName option:selected").text();
            $("#Hdn_SuppName").val(SuppName)

            var CountryName = $("#cntry_origin option:selected").text();
            $("#Hdn_POCntryName").val(CountryName)
            var CountryId = $("#cntry_origin option:selected").val();
            $("#Hdn_POCntryId").val(CountryId)
            if (CheckNetValue() == false) {
                return false;
            }
            if (uptodate < date) {
                $("#btn_save").attr("disabled", false);
                $("#hdnsavebtn").val("");
            }
            else {
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            }
            $("#ddlRequiredArea").attr('disabled', false);
            $("#ddlstockable").attr('disabled', false);
            $("#ddlAll").attr('disabled', false);

            return true;
        }
    }
    catch (ex) {
        console.log(ex);
        return false;
    }
};
function CheckNetValue() {
    try {
        let Errflag = "N";

        $("#PoItmDetailsTbl > tbody > tr").each(function () {
            let row = $(this);
            
            if (parseFloat(CheckNullNumber(row.find("#item_net_val_bs").val())) == 0) {
                Errflag = "Y";
                row.removeClass("highlight_tr");
                row.addClass("highlight_tr_red");
            }
            else {
                row.removeClass("highlight_tr_red");
            }

        });
        if (parseFloat(CheckNullNumber($("#NetOrderValueInBase").val())) == 0) {
            $("#NetOrderValueInBase").css("border-color", "red");
            Errflag = "Y";
        }
        if (Errflag == "Y") {
            swal("", $("#NetValueCanNotBe0").text(), "warning");
            return false;
        }
        return true;
    }
    catch (ex) {
        return false;
        console.log(ex);
    }
    
}
function InsertPOItemDetails() {
    debugger;
    var POItemList = [];
    $("#PoItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var ItemID = "";
        var ItemName = "";
        var subitem = "";
        var UOMID = "";
        var UOMName = "";
        var AvalStk = "";
        var OrderQty = "";
        var OrderBQty = "";
        var DnQty = "";
        var GRNQty = "";
        var InvQty = "";
        var QCQty = "";
        var ItmRate = "";
        var ItmDisPer = "";
        var ItmDisAmt = "";
        var DisVal = "";
        var GrossVal = "";
        var AssVal = "";
        var TaxAmt = "";
        var OCAmt = "";
        var NetValSpec = "";
        var NetValBase = "";
        var SimpleIssue = "";
        var MRSNo = "";
        var FClosed = "";
        var Remarks = "";
        var TaxExempted = "";
        var ManualGST = "";
        var hsn_code = "";
        var currentRow = $(this);
        var SNo = currentRow.find("#SNohiddenfiled").val();
        var sr_no = currentRow.find("#srno").text();

        ItemID = currentRow.find("#POItemListName" + SNo).val();
        ItemName = currentRow.find("#POItemListName" + SNo + " option:selected").text();
        subitem = currentRow.find("#sub_item").val();
        //UOMID = currentRow.find("#UOMID").val();
        //UOMName = currentRow.find("#UOM").val();
        var UOM_ID = currentRow.find("#UOM option:selected").val();
        var Itemtype = currentRow.find("#hdnItemtype").val();
        if (Itemtype == "Service") {
            if (UOM_ID != null && UOM_ID != "" && UOM_ID != "0" && UOM_ID != "NaN") {
                UOMID = UOM_ID;
            }
            else {
                UOMID = "0";
            }

        }
        else {
            UOMID = UOM_ID;
        }

        UOMName = currentRow.find("#UOM option:selected").text();
        AvalStk = currentRow.find("#AvailableStockInBase").val();
        OrderQty = currentRow.find("#ord_qty_spec").val();
        OrderBQty = currentRow.find("#ord_qty_base").val();
        hsn_code = currentRow.find("#ItemHsnCode").val();
        if (currentRow.find("#dn_qty").val() == null || currentRow.find("#dn_qty").val() == "") {
            DnQty = "0";
        }
        else {
            DnQty = currentRow.find("#dn_qty").val();
        }
        if (currentRow.find("#grn_qty").val() == null || currentRow.find("#grn_qty").val() == "") {
            GRNQty = "0";
        }
        else {
            GRNQty = currentRow.find("#grn_qty").val();
        }
        if (currentRow.find("#inv_qty").val() == null || currentRow.find("#inv_qty").val() == "") {
            InvQty = "0";
        }
        else {
            InvQty = currentRow.find("#inv_qty").val();
        }
        if (currentRow.find("#qc_qty").val() == null || currentRow.find("#qc_qty").val() == "") {
            QCQty = "0";
        }
        else {
            QCQty = currentRow.find("#qc_qty").val();
        }
        ItmRate = currentRow.find("#item_rate").val();
        if (currentRow.find("#item_disc_perc").val() === "") {
            ItmDisPer = "0";
        }
        else {
            ItmDisPer = currentRow.find("#item_disc_perc").val();
        }
        if (currentRow.find("#item_disc_amt").val() === "") {
            ItmDisAmt = "0";
        }
        else {
            ItmDisAmt = currentRow.find("#item_disc_amt").val();
        }
        if (currentRow.find("#item_disc_val").val() === "" || currentRow.find("#item_disc_val").val() === null) {
            DisVal = "0";
        }
        else {
            DisVal = currentRow.find("#item_disc_val").val();
        }
        GrossVal = currentRow.find("#item_gr_val").val();
        AssVal = currentRow.find("#item_ass_val").val();
        if (currentRow.find("#item_tax_amt").val() === "" || currentRow.find("#item_tax_amt").val() === null) {
            TaxAmt = "0";
        }
        else {
            TaxAmt = currentRow.find("#item_tax_amt").val();
        }
        if (currentRow.find("#item_oc_amt").val() === "" || currentRow.find("#item_oc_amt").val() === null) {
            OCAmt = "0";
        }
        else {
            OCAmt = currentRow.find("#item_oc_amt").val();
        }

        NetValSpec = currentRow.find("#item_net_val_spec").val();
        NetValBase = currentRow.find("#item_net_val_bs").val();
        //if (currentRow.find("#SimpleIssue").is(":checked")) {commented by shubham maurya on 01-10-2025
        //    SimpleIssue = "Y";
        //}
        //else {
            SimpleIssue = "N";
        //}

        MRSNo = "0";//currentRow.find("#MRSNumber").val();
        if (currentRow.find("#ItmFClosed").is(":checked")) {
            FClosed = "Y";
        }
        else {
            FClosed = "N";
        }
        Remarks = currentRow.find("#remarks").val();
        if (currentRow.find("#TaxExempted").is(":checked")) {
            TaxExempted = "Y"
        }
        else {
            TaxExempted = "N"
        }
        var GstApplicable = $("#Hdn_GstApplicable").text();
        if (GstApplicable == "Y") {
            if (currentRow.find("#ManualGST").is(":checked")) {
                ManualGST = "Y"
            }
            else {
                ManualGST = "N"
            }
        }
        var PackSize = currentRow.find("#PackSize").val();
        var mrp = currentRow.find("#mrp").val();
        POItemList.push({
            ItemID: ItemID, ItemName: ItemName, subitem: subitem, UOMID: UOMID,
            UOMName: UOMName, AvalStk: AvalStk, OrderQty: OrderQty, OrderBQty: OrderBQty,
            DnQty: DnQty, GRNQty: GRNQty, InvQty: InvQty, QCQty: QCQty, ItmRate: ItmRate,
            ItmDisPer: ItmDisPer, ItmDisAmt: ItmDisAmt, DisVal: DisVal, GrossVal: GrossVal, AssVal: AssVal,
            TaxAmt: TaxAmt, OCAmt: OCAmt, NetValSpec: NetValSpec, NetValBase: NetValBase,
            SimpleIssue: SimpleIssue, MRSNo: MRSNo == null ? "" : MRSNo, FClosed: FClosed,
            Remarks: Remarks, TaxExempted: TaxExempted, hsn_code: hsn_code, ManualGST: ManualGST, sr_no: sr_no, Itemtype: Itemtype, PackSize: PackSize, mrp: mrp
        });
    });
    return POItemList;
};
function InsertPOTaxDetails() {
    debugger;
    var FTaxDetails = $("#Hdn_TaxCalculatorTbl > tbody > tr").length;
    var POTaxList = [];
    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        if (currentRow.find("#TaxExempted").is(":checked")) {

        }
        else {
            var RowSNo = currentRow.find("#SNohiddenfiled").val();
            var ItmCode = currentRow.find("#POItemListName" + RowSNo).val();
            if (FTaxDetails != null) {
                if (FTaxDetails > 0) {
                    $("#Hdn_TaxCalculatorTbl > tbody > tr #TaxItmCode:contains('" + ItmCode + "')").closest("tr").each(function () {
                        var Crow = $(this);
                        var ItemID = Crow.find("#TaxItmCode").text().trim();
                        var TaxID = Crow.find("#TaxNameID").text().trim();
                        var TaxName = Crow.find("#TaxName").text().trim();
                        var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                        var TaxLevel = Crow.find("#TaxLevel").text().trim();
                        var TaxValue = Crow.find("#TaxAmount").text().trim();
                        var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                        var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                        var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();
                        POTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });
                    });
                }
            }
        }
    });
    return POTaxList;
};
function InsertPOOtherChargeDetails() {
    debugger;
    var PO_OCList = [];
    if ($("#Tbl_OC_Deatils >tbody >tr").length > 0) {
        $("#Tbl_OC_Deatils >tbody >tr").each(function () {
            var currentRow = $(this);
            var OC_ID = "";
            var OCValue = "";
            var OCName = "";
            var OC_Curr = "";
            var OC_Conv = "";
            var OC_AmtBs = "";
            var OC_TaxAmt = "";
            var OC_TotlAmt = "";
            OC_ID = currentRow.find("#OCValue").text();
            OCValue = currentRow.find("#OCAmount").text();
            OCName = currentRow.find("#OCName").text();
            OC_Curr = currentRow.find("#OCCurr").text();
            OC_Conv = currentRow.find("#OCConv").text();
            OC_AmtBs = currentRow.find("#OcAmtBs").text();
            OC_TaxAmt = IsNull(currentRow.find("#OCTaxAmt").text(),0);
            OC_TotlAmt = IsNull(currentRow.find("#OCTotalTaxAmt").text(),0);
            PO_OCList.push({ OC_ID: OC_ID, OCValue: OCValue, OCName: OCName, OC_Curr: OC_Curr, OC_Conv: OC_Conv, OC_AmtBs: OC_AmtBs, OC_TaxAmt: OC_TaxAmt, OC_TotlAmt: OC_TotlAmt });
        });
    }
    return PO_OCList;
};
function InsertPO_OCTaxDetails() {/*Add by Hina Sharma on 09-07-2025*/
    debugger;
    

    var FTaxDetails = $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").length;
    var PO_OCTaxList = [];

    if (FTaxDetails != null) {
        if (FTaxDetails > 0) {

            $("#Hdn_OC_TaxCalculatorTbl > tbody > tr").each(function () {
                var Crow = $(this);
                
                debugger;
                var ItemID = Crow.find("#TaxItmCode").text().trim();
                var TaxID = Crow.find("#TaxNameID").text().trim();
                var TaxName = Crow.find("#TaxName").text().trim();
                var TaxRate = Crow.find("#TaxPercentage").text().trim().replace('%', '');;
                var TaxLevel = Crow.find("#TaxLevel").text().trim();
                var TaxValue = Crow.find("#TaxAmount").text().trim();
                var TaxApplyOn = Crow.find("#TaxApplyOnID").text().trim();
                var taxapplyname = Crow.find("#TaxApplyOn").text().trim();
                var TotalTaxAmount = Crow.find("#TotalTaxAmount").text().trim();

                PO_OCTaxList.push({ ItemID: ItemID, TaxID: TaxID, TaxName: TaxName, TaxRate: TaxRate, TaxLevel: TaxLevel, TaxValue: TaxValue, TaxApplyOn: TaxApplyOn, taxapplyname: taxapplyname, TotalTaxAmount: TotalTaxAmount });

                
            });
        }
    }
    return PO_OCTaxList;
};
function InsertPOItem_DeliverySchDetails() {
    var PODelieryList = [];
    if ($("#DeliverySchTble >tbody >tr").length > 0) {
        $("#DeliverySchTble >tbody >tr").each(function () {
            var currentRow = $(this);
            var ItemID = "";
            var ItemName = "";
            var SchDate = "";
            var DeliveryQty = "";
            var RecQty = "";
            var PendingQty = "";
            ItemID = currentRow.find("#Hd_ItemIdFrDS").text();
            ItemName = currentRow.find("#Hd_ItemNameFrDS").text();
            DeliveryQty = currentRow.find("#delv_qty").text();
            SchDate = currentRow.find("#sch_date").text();
            RecQty = currentRow.find("#SRQty").text();
            PendingQty = currentRow.find("#PenQty").text();

            PODelieryList.push({ ItemID: ItemID, ItemName: ItemName, SchDate: SchDate, DeliveryQty: DeliveryQty });
        });
    }
    return PODelieryList;
};
function InsertPOTermConditionDetails() {
    var POTermsList = [];
    if ($("#TblTerms_Condition >tbody >tr").length > 0) {
        $("#TblTerms_Condition >tbody >tr").each(function () {
            var currentRow = $(this);
            var TermsDesc = "";
            TermsDesc = currentRow.find("#term_desc").text();
            POTermsList.push({ TermsDesc: TermsDesc });
        });
    }
    return POTermsList;
};
function InsertPOAttachmentDetails() {
    var formData = new FormData();
    var fp = $('#file-1');
    var lg = fp[0].files.length;
    if (lg > 0) {
        for (var i = 0; i < lg; i++) {
            formData.append('file', $('#file-1')[0].files[i]);
        }
    }
    return formData;
};
function CalculateAmount() {
    //debugger;
    //var ValDecDigit = $("#ValDigit").text();
    var GrossValue = parseFloat(0).toFixed(ValDecDigit);
    var AssessableValue = parseFloat(0).toFixed(ValDecDigit);
    var TaxValue = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueSpec = parseFloat(0).toFixed(ValDecDigit);
    var NetOrderValueBase = parseFloat(0).toFixed(ValDecDigit);

    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        //debugger;
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        if (currentRow.find("#item_gr_val").val() == "" || currentRow.find("#item_gr_val").val() == "NaN") {
            GrossValue = (parseFloat(GrossValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            GrossValue = (parseFloat(GrossValue) + parseFloat(currentRow.find("#item_gr_val").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#item_ass_val").val() == "" || currentRow.find("#item_ass_val").val() == "NaN") {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            AssessableValue = (parseFloat(AssessableValue) + parseFloat(currentRow.find("#item_ass_val").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#item_tax_amt").val() == "" || currentRow.find("#item_tax_amt").val() == "0" || currentRow.find("#item_tax_amt").val() == "NaN") {
            TaxValue = (parseFloat(TaxValue) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            TaxValue = (parseFloat(TaxValue) + parseFloat(currentRow.find("#item_tax_amt").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#item_net_val_spec").val() == "" || currentRow.find("#item_net_val_spec").val() == "NaN") {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            NetOrderValueSpec = (parseFloat(NetOrderValueSpec) + parseFloat(currentRow.find("#item_net_val_spec").val())).toFixed(ValDecDigit);
        }
        if (currentRow.find("#item_net_val_bs").val() == "" || currentRow.find("#item_net_val_bs").val() == "NaN") {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(0)).toFixed(ValDecDigit);
        }
        else {
            NetOrderValueBase = (parseFloat(NetOrderValueBase) + parseFloat(currentRow.find("#item_net_val_bs").val())).toFixed(ValDecDigit);
        }
    });
    $("#TxtGrossValue").val(GrossValue);
    $("#TxtAssessableValue").val(AssessableValue);
    $("#TxtTaxAmount").val(TaxValue);
    $("#NetOrderValueSpe").val(NetOrderValueSpec);
    $("#NetOrderValueInBase").val(NetOrderValueBase);
};
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#PoItmDetailsTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
    if ($("#PoItmDetailsTbl >tbody >tr").length == 0) {
        $("#src_doc_number").prop("disabled", false);
    }
};
function DisableAfterSave() {
    DisableHeaderField();
    $("#ValidUptoDate").prop("readonly", true);
    $("#remarks").prop("readonly", true);
    $("#src_doc_number").prop("disabled", true);
    $("#PoItmDetailsTbl .plus_icon1").css("display", "none");
    DisableItemDetailsPO();
    $("#ForceClosed").attr('disabled', true);
    $("#Cancelled").attr('disabled', true);
    $("#Tax_Template").attr('disabled', true);
    $("#Tax_Type").attr('disabled', true);
    $("#Tax_Percentage").prop("readonly", true);
    $("#Tax_Level").attr('disabled', true);
    $("#Tax_ApplyOn").attr('disabled', true);
    $("#TaxResetBtn").css("display", "none");
    $("#TaxSaveTemplate").css("display", "none");
    $("#ReplicateOnAllItemsBtn").css("display", "none");
    $("#SaveAndExitBtn").css("display", "none");
    $("#TaxExitAndDiscard").css("display", "none");
    $("#OtherCharge_DDL").attr('disabled', true);
    $("#TxtOCAmt").prop("readonly", true);
    $("#SaveAndExitBtn_OC").css("display", "none");
    $("#DiscardAndExit_OC").css("display", "none");

    $("#ForDisableOCDlt").val("Disable");
    //$("#Tbl_OC_Deatils >tbody >tr").each(function () {
    //    var currentRow = $(this);
    //    currentRow.find("#OCDelIcon").css("display", "none");
    //});
    $("#DeliverySchItemDDL").attr('disabled', true);
    $("#DeliveryDate").prop("readonly", true);
    $("#DeliverySchQty").prop("readonly", true);
    $(".plus_icon").css("display", "none");
    $("#DeliverySchTble >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#DeliveryDelIconBtn").css("display", "none");
    });
    $("#TxtTerms_Condition").prop("readonly", true);
    $(".plus_icon1").css("display", "none");
    $("#TblTerms_Condition >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TC_DelIcon").css("display", "none");
    });
    $("#TemplateTermsddl").attr('disabled', true);
    //$("#file-1").attr('disabled', true);

}
function DisableItemDetailsPO() {
    var docid = $("#DocumentMenuId").val();
    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#delBtnIcon").css("display", "none");
        currentRow.find("#POItemListName" + Sno).attr("disabled", true);
        currentRow.find("#ItmInfoBtnIcon").attr("disabled", true);
        currentRow.find("#ord_qty_spec").attr("disabled", true);
        currentRow.find("#item_rate").attr("disabled", true);
        currentRow.find("#item_disc_perc").attr("disabled", true);
        currentRow.find("#item_disc_amt").attr("disabled", true);
        currentRow.find("#item_ass_val").attr("disabled", true);
        currentRow.find("#remarks").attr("disabled", true);
        //currentRow.find("#SimpleIssue").attr("disabled", true);commented by shubham maurya on 01-10-2025
        currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
        if (docid != '105101140101') {
            currentRow.find("#BtnTxtCalculation").attr("disabled", false);
        }
        currentRow.find("#ItmFClosed").attr("disabled", true);
    });
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
        if (isConfirm) {
            $("#hdnAction").val("Delete");
            //location.href = "/ApplicationLayer/PurchaseQuotation/PQDetailsActionCommands";
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function EnableForEdit() {
    //debugger;
    var docid = $("#DocumentMenuId").val();
    $("#ValidUptoDate").prop("readonly", false);
    $("#remarks").prop("readonly", false);
    if ($("#hdn_attatchment_list tbody tr").length < 5) {
        $("#file-1").attr('disabled', false);
        $("#FilesUpload").css("display", "block");
    }
    var src_type = $("#src_type").val();
    if (src_type == "D") {


        if ($("#SupplierName").val() != "" && $("#SupplierName").val() != null && $("#SupplierName").val() != "0") {
            $("#PoItmDetailsTbl >tbody >tr").each(function () {
                var currentRow = $(this);
                var Sno = currentRow.find("#SNohiddenfiled").val();
                debugger;
                currentRow.find("#POItemListName" + Sno).attr("disabled", false);
                currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
                if (docid != '105101140101') {
                    currentRow.find("#BtnTxtCalculation").attr("disabled", false);
                }
                currentRow.find("#ord_qty_spec").attr("disabled", false);
                currentRow.find("#item_rate").attr("disabled", false);
                var DisPerc = currentRow.find("#item_disc_perc").val();
                var DisAmt = currentRow.find("#item_disc_amt").val();
                if (DisPerc != null && DisPerc != "" && DisPerc != "0" && DisPerc != 0 && DisPerc != "NaN") {
                    currentRow.find("#item_disc_perc").attr("disabled", false);
                }
                else {
                    currentRow.find("#item_disc_amt").attr("disabled", false);
                }
                if (DisAmt != null && DisAmt != "" && DisAmt != "0" && DisAmt != 0 && DisAmt != "NaN") {
                    currentRow.find("#item_disc_amt").attr("disabled", false);
                }
                else {
                    currentRow.find("#item_disc_perc").attr("disabled", false);
                }
                //currentRow.find("#item_ass_val").attr("disabled", false);//Commented by Suraj on 01-04-2024
                currentRow.find("#remarks").attr("disabled", false);
                //currentRow.find("#SimpleIssue").attr("disabled", false);commented by shubham maurya on 01-10-2025
            });
        }

        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            currentRow.find("#delBtnIcon").css("display", "block");
        });

        $("#Tax_Template").attr('disabled', false);
        $("#Tax_Type").attr('disabled', false);
        $("#Tax_Percentage").prop("readonly", false);
        $("#Tax_Level").attr('disabled', false);
        $("#Tax_ApplyOn").attr('disabled', false);
        $("#TaxResetBtn").css("display", "block");
        $("#TaxSaveTemplate").css("display", "block");
        $("#ReplicateOnAllItemsBtn").css("display", "block");
        $("#SaveAndExitBtn").css("display", "block");
        $("#TaxExitAndDiscard").css("display", "block");

        $("#OtherCharge_DDL").attr('disabled', false);
        $("#TxtOCAmt").prop("readonly", false);
        $("#SaveAndExitBtn_OC").css("display", "block");
        $("#DiscardAndExit_OC").css("display", "block");

        $("#ForDisableOCDlt").val("Enable");
        //$("#Tbl_OC_Deatils >tbody >tr").each(function () {
        //    var currentRow = $(this);
        //    currentRow.find("#OCDelIcon").css("display", "block");
        //});

        $("#DeliverySchItemDDL").attr('disabled', false);
        $("#DeliveryDate").prop("readonly", false);
        $("#DeliverySchQty").prop("readonly", false);
        $(".plus_icon").css("display", "block");
        $("#DeliverySchTble >tbody >tr").each(function () {
            var currentRow = $(this);
            currentRow.find("#DeliveryDelIconBtn").css("display", "block");
        });

        $("#TxtTerms_Condition").prop("readonly", false);
        $(".plus_icon1").css("display", "block");
        //$(".plus_icon").css("display", "block");
        if ($("#SupplierName").val() == "" || $("#SupplierName").val() == null || $("#SupplierName").val() == "0") {
            $("#PoItmDetailsTbl .plus_icon1").css("display", "none");
        }

        $("#TblTerms_Condition >tbody >tr").each(function () {
            var currentRow = $(this);
            currentRow.find("#TC_DelIcon").css("display", "block");
        });
        $("#TemplateTermsddl").attr('disabled', false);



    }
    else {
        EnableForQtsnAndPREdit();
    }
}
function EnableForQtsnAndPREdit() {
    var docid = $("#DocumentMenuId").val();
    $("#ValidUptoDate").prop("readonly", false);
    $("#remarks").prop("readonly", false);
    var src_type = $("#src_type").val();
    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        //currentRow.find("#SimpleIssue").attr("disabled", false); commented by shubham maurya on 01-10-2025
        currentRow.find("#remarks").attr("disabled", false);
        if (src_type == "PR") {
            currentRow.find("#delBtnIcon").css("display", "block");
            currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
            if (docid != '105101140101') {
                currentRow.find("#BtnTxtCalculation").attr("disabled", false);
            }
            currentRow.find("#ord_qty_spec").attr("disabled", false);
            currentRow.find("#item_rate").attr("disabled", false);
            var DisPerc = currentRow.find("#item_disc_perc").val();
            var DisAmt = currentRow.find("#item_disc_amt").val();
            if (DisPerc != null && DisPerc != "" && DisPerc != "0" && DisPerc != 0 && DisPerc != "NaN") {
                currentRow.find("#item_disc_perc").attr("disabled", false);
            }
            else {
                currentRow.find("#item_disc_amt").attr("disabled", false);
            }
            if (DisAmt != null && DisAmt != "" && DisAmt != "0" && DisAmt != 0 && DisAmt != "NaN") {
                currentRow.find("#item_disc_amt").attr("disabled", false);
            }
            else {
                currentRow.find("#item_disc_perc").attr("disabled", false);
            }
            //currentRow.find("#item_ass_val").attr("disabled", false);//Commented by Suraj on 01-04-2024
        }
    });

    $("#Tax_Template").attr('disabled', false);
    $("#Tax_Type").attr('disabled', false);
    $("#Tax_Percentage").prop("readonly", false);
    $("#Tax_Level").attr('disabled', false);
    $("#Tax_ApplyOn").attr('disabled', false);
    $("#TaxResetBtn").css("display", "block");
    $("#TaxSaveTemplate").css("display", "block");
    $("#ReplicateOnAllItemsBtn").css("display", "block");
    $("#SaveAndExitBtn").css("display", "block");
    $("#TaxExitAndDiscard").css("display", "block");
    $("#OtherCharge_DDL").attr('disabled', false);
    $("#TxtOCAmt").prop("readonly", false);
    $("#SaveAndExitBtn_OC").css("display", "block");
    $("#DiscardAndExit_OC").css("display", "block");

    $("#ForDisableOCDlt").val("Enable");
    $(".plus_icon1").css("display", "block");

    $("#DeliverySchItemDDL").attr('disabled', false);
    $("#DeliveryDate").prop("readonly", false);
    $("#DeliverySchQty").prop("readonly", false);

    $("#DeliverySchTble >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#DeliveryDelIconBtn").css("display", "block");
    });

    $("#TemplateTermsddl").attr('disabled', false);
    $("#TxtTerms_Condition").prop("readonly", false);
    $("#TblTerms_Condition >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#TC_DelIcon").css("display", "block");
    });
    $("#BtnAddItem").closest('div').css("display", "none");
    DeleteDeliverySch();
    DeleteTermCondition();
}
function EnableForEditAfterApprove(Status) {
    if (Status == "A") {
        $("#Cancelled").attr('disabled', false);
    }
    if (Status === "PDL" || Status === "PR" || Status === "PN") {
        $("#ForceClosed").attr('disabled', false);
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItmFClosed").prop("checked") == true) {
                currentRow.find("#ItmFClosed").attr("disabled", true);
            } else {
                currentRow.find("#ItmFClosed").attr("disabled", false);
            }
        });
    }
}
function DisableForSaveAfterApprove() {
    $("#ForceClosed").attr('disabled', true);
    $("#Cancelled").attr('disabled', true);

    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        currentRow.find("#ItmFClosed").attr("disabled", true);
    });
}
function CheckedFClose() {
    //debugger;
    if ($("#ForceClosed").is(":checked")) {
        $("#Cancelled").attr("disabled", true);
        $("#Cancelled").prop("checked", false);
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItmFClosed").prop("disabled") == true) {

            } else {
                currentRow.find("#ItmFClosed").prop("checked", true);
                //currentRow.find("#ItmFClosed").attr("disabled", true);
            }

        });
        $("#btn_save").attr('onclick', 'SaveBtnClick()');
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#PoItmDetailsTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            //debugger;
            var Sno = currentRow.find("#SNohiddenfiled").val();
            if (currentRow.find("#ItmFClosed").prop("disabled") == true) {

            } else {
                currentRow.find("#ItmFClosed").prop("checked", false);
                //currentRow.find("#ItmFClosed").attr("disabled", false);
            }

        });
        CheckedItemWiseFClose();
    }
}
function CheckedCancelled() {
    //debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#ForceClosed").attr("disabled", true);
        $("#ForceClosed").prop("checked", false);
        $("#btn_save").attr('onclick', 'return SaveBtnClick()');
        $("#btn_save").attr('disabled', false);
     
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
      
    }
    else {
        $("#btn_save").attr('onclick', '');
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });       
    }
    CancelledRemarks("#Cancelled", "Enable");
}
function CheckedItemWiseFClose() {
    //debugger;
    var FClose = "N";
    var FinalFClose = "Y";
    $("#PoItmDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        var Sno = currentRow.find("#SNohiddenfiled").val();
        //debugger;
        if (currentRow.find("#ItmFClosed").is(":checked")) {
            FClose = "Y";
        }
        else {
            FinalFClose = "N";

        }
    });
    if (FClose == "Y") {
        $("#btn_save").attr('onclick', 'SaveBtnClick()');
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr('onclick', '');
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
    if (FClose == "Y" && FinalFClose == "Y") {
        $("#ForceClosed").prop("checked", true);
    }
    else {
        $("#ForceClosed").prop("checked", false);
    }
}
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hfStatus").val().trim();
    var PODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        PODTransType = "Update";
    }

    Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, PODTransType);
}
function OnClickSupplierInfoIconBtn(e) {
    //debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";

    ItmCode = clickedrow.find("#POItemListName" + Sno).val();
    var Supp_id = $('#SupplierName').val();

    Cmn_SupplierInfoIconBtnClick(ItmCode, Supp_id);
}
function GetTrackingDetails() {
    //debugger;
    $("#tbl_trackingDetail").DataTable().destroy();
    $("#tbl_trackingDetail").DataTable();
}
//function CheckPOOtherDetails_Validations() {
//    var ErrorFlag = "N";
//    if ($("#PriceBasis").val() == "") {
//        $('#SpanPriceBasisErrorMsg').text($("#valueReq").text());
//        $("#SpanPriceBasisErrorMsg").css("display", "block");
//        $("#PriceBasis").css("border-color", "Red");

//        if (ErrorFlag == "N") {
//            $("#collapseFour").addClass("show");
//            $("#PriceBasis").focus();
//        }
//        ErrorFlag = "Y";

//    }
//    else {
//        $("#SpanPriceBasisErrorMsg").css("display", "none");
//        $("#PriceBasis").css("border-color", "#ced4da");
//    }
//    if ($("#FreightType").val() == "") {
//        $('#SpanFreightTypeErrorMsg').text($("#valueReq").text());
//        $("#SpanFreightTypeErrorMsg").css("display", "block");
//        $("#FreightType").css("border-color", "Red");
//        if (ErrorFlag == "N") {
//            $("#collapseFour").addClass("show");
//            $("#FreightType").focus();
//        }
//        ErrorFlag = "Y";
//    }
//    else {
//        $("#SpanFreightTypeErrorMsg").css("display", "none");
//        $("#FreightType").css("border-color", "#ced4da");
//    }
//    if ($("#ModeOfTransport").val() == "") {
//        $('#SpanModeOfTransportErrorMsg').text($("#valueReq").text());
//        $("#SpanModeOfTransportErrorMsg").css("display", "block");
//        $("#ModeOfTransport").css("border-color", "Red");
//        if (ErrorFlag == "N") {
//            $("#collapseFour").addClass("show");
//            $("#ModeOfTransport").focus();
//        }
//        ErrorFlag = "Y";
//    }
//    else {
//        $("#SpanModeOfTransportErrorMsg").css("display", "none");
//        $("#ModeOfTransport").css("border-color", "#ced4da");
//    }
//    if ($("#Destination").val() == "") {
//        $('#SpanDestinationErrorMsg').text($("#valueReq").text());
//        $("#SpanDestinationErrorMsg").css("display", "block");
//        $("#Destination").css("border-color", "Red");
//        if (ErrorFlag == "N") {
//            $("#collapseFour").addClass("show");
//            $("#Destination").focus();
//        }
//        ErrorFlag = "Y";
//    }
//    else {
//        $("#SpanDestinationErrorMsg").css("display", "none");
//        $("#Destination").css("border-color", "#ced4da");
//    }

//    if (ErrorFlag == "Y") {
//        return false;
//    } else {
//        return true;
//    }

//}


/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemAvlQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemOrderSpecQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPOPROrdQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemPOQTOrdQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemOrderBaseQty");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var src_type = $("#src_type").val();
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#POItemListName" + hfsno + " option:selected").text();
    var ProductId = clickdRow.find("#POItemListName" + hfsno).val();
    var UOMId = clickdRow.find("#UOM option:selected").val();
    var UOM = clickdRow.find("#UOM option:selected").text();
    var Doc_no = $("#po_no").val();
    var Doc_dt = $("#po_date").val();
    var IsDisabled = $("#DisableSubItem").val();

    var Sub_Quantity = 0;
    var NewArr = new Array();

    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {

        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        //if (src_type == "Q") {
        //    List.sub_item_name = row.find('#subItemName').val();
        //}
        List.qty = row.find('#subItemQty').val();
        NewArr.push(List);
    });
    if (flag == "QuantitySpec") {
        Sub_Quantity = clickdRow.find("#ord_qty_spec").val();

    }
    else if (src_type == "D" && flag == "QuantityBase") {
        Sub_Quantity = clickdRow.find("#ord_qty_base").val();
        IsDisabled = "Y";
    }
    else if ((src_type == "Q" && flag == "POQTOrdQty") || (src_type == "Q" && flag == "QuantityBase")) {
        Sub_Quantity = clickdRow.find("#ord_qty_spec").val();
        var SRCDoc_no = $("#src_doc_number").val();
        var SRCDoc_no = $("#Hdn_src_doc_number").val();
        var SRCDoc_date = $("#src_doc_date").val();
        IsDisabled = "Y";
    }
    else if (flag == "POPROrdQty" || flag == "QuantityBase") {
        Sub_Quantity = clickdRow.find("#ord_qty_spec").val();
        var SRCDoc_no = $("#src_doc_number").val();
        var SRCDoc_no = $("#Hdn_src_doc_number").val();
        var SRCDoc_date = $("#src_doc_date").val();
        if (flag == "QuantityBase") {
            IsDisabled = "Y";
        }

    }

    var hd_Status = $("#hfStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/DPO/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            SRCDoc_no: SRCDoc_no,
            SRCDoc_date: SRCDoc_date,
            src_type: src_type,
            UOMId: UOMId
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);

            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    });

}
function CheckValidations_forSubItems() {
    debugger
    var Src_Type = $("#src_type").val();
    if (Src_Type == "D") {

        return Cmn_CheckValidations_forSubItems("PoItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemOrderSpecQty", "Y");
    }
    if (Src_Type == "PR") {

        return Cmn_CheckValidations_forSubItems("PoItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemPOPROrdQty", "Y");
    }

    //var ErrFlg = "";

    //if (Cmn_CheckValidations_forSubItems("PoItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemOrderSpecQty", "Y") == false) {
    //    ErrFlg = "Y"
    //}
    //if (Cmn_CheckValidations_forSubItems("PoItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemPOQTOrdQty", "Y") == false) {
    //    ErrFlg = "Y"
    //}
    //if (ErrFlg == "Y") {
    //    return false
    //}
    //else {
    //    return true
    //}

}
function ResetWorningBorderColor() {
    debugger;
    var Src_Type = $("#src_type").val();
    if (Src_Type == "D") {
        return Cmn_CheckValidations_forSubItems("PoItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemOrderSpecQty", "N");
    }
    if (Src_Type == "PR") {

        return Cmn_CheckValidations_forSubItems("PoItmDetailsTbl", "SNohiddenfiled", "POItemListName", "ord_qty_spec", "SubItemPOPROrdQty", "N");
    }
}
function GetSubItemAvlStock(e) {
    var Crow = $(e.target).closest('tr');
    var rowNo = Crow.find("#SNohiddenfiled").val();
    var ProductNm = Crow.find("#POItemListName" + rowNo + " option:selected").text();
    var ProductId = Crow.find("#POItemListName" + rowNo + " option:selected").val();
    var UOMId = Crow.find("#UOM").val();
    var UOM = Crow.find("#UOM option:selected").text();
    var AvlStk = Crow.find("#AvailableStockInBase").val();
    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, "0", AvlStk, "br", UOMId);
}
/***--------------------------------Sub Item Section End-----------------------------------------***/

function item_type() {
    debugger;
    var Branchchk = "";
    if ($("#ddlstockable").is(":checked")) {
        Branchchk = "S";
        $("#src_type").attr("Disabled", false)
        //$('#src_type option[value="Q"]').hide();
        //$('#src_type option[value="PR"]').hide();
        $("#PoItmDetailsTbl").removeClass('width2500');
        $("#PoItmDetailsTbl").addClass('width3100');
        $("#avlstock").show();
        $("#th_sampleissue").show();
        $("#th_mrs_no").show();
        $("#iconddl").show();
        //$("#PoItmDetailsTbl >tbody >tr").remove();
        RemoveAllTables();
        if ($("#src_type").val() == "PR") { //shubham maurya 23-10-2024
            $("#SupplierName").attr("disabled", false);
        }
        BindPOItmList(1);
    }
    if ($("#ddlconsumable").is(":checked")) {
        Branchchk = "C";
        $("#src_type").attr('disabled', true);
        $("#src_type").val('D');
        $("#avlstock").hide();
        $("#th_sampleissue").hide();
        $("#th_mrs_no").hide();
        $("#iconddl").hide();
        $("#PoItmDetailsTbl").removeClass('width3100');
        $("#PoItmDetailsTbl").addClass('width2500');
        BindPOItmList(1);
        $("#PoItmDetailsTbl >tbody >tr").remove();
    }
    if ($("#ddlAll").is(":checked")) {
        //$("#src_type").attr("Disabled", false)
        $("#src_type").attr('disabled', false);
      //  $('#src_type option[value="Q"]').hide();
        $("#src_type").val('D');
        $("#PoItmDetailsTbl").removeClass('width2500');
        $("#PoItmDetailsTbl").addClass('width3100');
        $("#avlstock").show();
        $("#th_sampleissue").show();
        $("#th_mrs_no").show();
        $("#iconddl").show();
        //$("#PoItmDetailsTbl >tbody >tr").remove();
        RemoveAllTables();
        BindPOItmList(1);
    }
    OnChangeSourType();
}
function RemoveAllTables() {
    /*code start by Hina sharma on 18-06-2025*/

    $("#PoItmDetailsTbl >tbody >tr").remove();
    $("#hdn_Sub_ItemDetailTbl >tbody >tr").remove();
    $("#ht_Tbl_OC_Deatils > tbody>tr").remove();
    $("#Tbl_OC_Deatils > tbody >tr").remove();
    $("#Tbl_OtherChargeList > tbody >tr").remove();
    $("#TblTerms_Condition > tbody >tr").remove();
    $("#DeliverySchTble > tbody >tr").remove();
    $("#Tbl_ItemTaxAmountList > tbody >tr").remove();
    $("#TxtGrossValue").val("");
    $("#TxtTaxAmount").val("");
    $("#NetOrderValueInBase").val("");
    $("#PO_OtherCharges").val("");
    $("#_ItemTaxAmountTotal").text("");
    $("#_OtherChargeTotal").text("");
    //$("#Hdn_OC_TaxCalculatorTbl > tbody>tr").remove();
    //$("#Hdn_OCTemp_TaxCalculatorTbl > tbody >tr").remove();
}
//function appendthbody(Branchchk) {
//    debugger;
//    var sou_type = $("#ddlsource_type" + " option:selected").val();
//    if (Branchchk == "S") {
//        $('#PoItmDetailsTbl thead th').remove();
//        $('#PoItmDetailsTbl thead').append(` <tr class="headings">
//                                                                    <th width="1%"> &nbsp;</th>
//                                                                    <th width="2%">${$("#span_srno").text()}</th>
//                                                                    <th width="13%">
//                                                                        <div class="col-md-3 col-sm-12"></div>
//                                                                        <div class="col-md-6 col-sm-12">
//                                                                            <span class="item_add">
//                                                                                 ${$("#ItemName").text()}<span class="required">*</span>
//                                                                            </span>

//                                                                            <a href="#">
//                                                                                <div class="plus_icon1" style="display: none;"> <i class="fa fa-plus" id="BtnAddItem" onclick="AddNewRow()" title="@Resource.AddNew"></i> </div>
//                                                                            </a>
//                                                                        </div>
//                                                                        <div class="col-md-3 col-sm-12"></div>
//                                                                    </th>
//                                                                    <th width="2%" title="@Resource.UnitOfMeasure"> ${$("#ItemUOM").text()}</th>

//                                                                    <th width="5%" id="avlstock">${$("#span_AvailableStock").text()} <br /> ${$("#span_InBase").text()}</th>
//                                                                    <th width="5%">
//                                                                        <div class="col-md-10 col-sm-12">
//                                                                            <span class="item_add">
//                                                                                ${$("#span_OrderQuantity").text()}<span class="required">*</span>
//                                                                            </span>
//                                                                        </div>
//                                                                        <div class="col-md-2 col-sm-12 i_Icon" id="iconddl"><button type="button" id="TrackingDetail" class="calculator available_credit_limit" onclick="GetTrackingDetails();" data-toggle="modal" data-target="#TrackingDetail" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt=""  ${$("#span_TrackingDetail").text()}> </button></div>
//                                                                    </th>
//                                                                    <th width="5%">@Resource.OrderQuantity <br /> @Resource.InBase</th>
//                                                                    <th width="3%" style="display:none;">@Resource.QualityCheck @Resource.Quantity</th>
//                                                                    <th width="5%">@Resource.Rate <span class="required">*</span></th>
//                                                                    <th width="5%">@Resource.Discount <br /> @Resource.InPercentage</th>
//                                                                    <th width="5%">@Resource.Discount <br /> @Resource.InAmount</th>
//                                                                    <th width="5%">@Resource.Discount <br /> @Resource.Value</th>
//                                                                    <th width="5%">@Resource.GrossValue</th>
//                                                                    <th width="5%" style="display:@displaynoneinDomestic">@Resource.Assessable <br /> @Resource.Value <span class="required">*</span></th>
//                                                                    @if (Model.DocumentMenuId != "105101140101")
//                                                                    {
//                                                                        <th width="3%">@Resource.TaxExempted</th>
//                                                                        if (ViewBag.GstApplicable == "Y")
//                                                                        {
//                                                                            <th width="3%">@Resource.Manual @Resource.GST</th>
//                                                                        }
//                                                                    }
//                                                                    <th width="6%">@Resource.TaxAmount</th>
//                                                                    <th width="5%">@Resource.OtherCharges</th>
//                                                                    <th width="5%" style="display:@displaynoneinDomestic">@Resource.NetValue <br /> @Resource.InSpecific</th>
//                                                                    <th width="5%">@Resource.NetValue <br /> @Resource.InBase</th>
//                                                                    <th width="3%">@Resource.Sample<br /> @Resource.Issue</th>
//                                                                    <th width="5%">@Resource.MRS<br /> @Resource.Number</th>
//                                                                    @if (FCloseAccess == "Y")
//                                                                    {
//                                                                        <th width="3%">@Resource.ForceClosed</th>
//                                                                    }
//                                                                    <th width="11%">@Resource.remarks</th>

//                         </tr>`);
//    }

//}

function approveonclick() {
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

function onChangeConversionRate(e) {
    debugger;
    //let ExchDecDigit = $("#ExchDigit").text();
    let ConvRate;
    ConvRate = $("#conv_rate").val();
    if (AvoidDot(ConvRate) == false) {
        $("#conv_rate").val(parseFloat(0).toFixed(ExchDecDigit));
    } else {
        $("#conv_rate").val(parseFloat(ConvRate).toFixed(ExchDecDigit));
    }
    if (parseFloat(CheckNullNumber($("#conv_rate").val())) <= 0) {
        $('#SpanSuppExRateErrorMsg').text($("#ValueShouldBeGreaterThan0").text());
        $("#SpanSuppExRateErrorMsg").css("display", "block");
        $("#conv_rate").css("border-color", "Red");
        //ErrorFlag = "Y";
    }
    else {
        $('#SpanSuppExRateErrorMsg').text("");
        $("#SpanSuppExRateErrorMsg").css("display", "none");
        $("#conv_rate").css("border-color", "#ced4da");
    }

    $("#PoItmDetailsTbl > tbody > tr").each(function () {
        var clickedrow = $(this);
        CalculationBaseRate(clickedrow, "hideError");//passed "hideError" parameter by Suraj Maurya on 07-11-2024
    });
    $("#ht_Tbl_OC_Deatils > tbody > tr").each(function () {
        var clickedrow = $(this);
        clickedrow.find("#OC_Conv").text(ConvRate);
        var spcAmt = clickedrow.find("#OCAmtSp").text();
        var bsAmt = ConvRate * spcAmt;
        clickedrow.find("#OCAmtBs").text(bsAmt);
    });
}

/*------------------------------------------UOM Conversion-------------------------------------------*/

function GetUomNameItemWise(Itm_ID, UOM, UOMID, CurrRow) {
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/BillofMaterial/GetSOItemUOM",
                data: {
                    Itm_ID: Itm_ID
                },
                success: function (data) {
                    debugger;
                    if (CurrRow != null) {
                        if (data !== null && data !== "" && data != "ErrorPage") {
                            var arr = [];
                            arr = JSON.parse(data);

                            if (arr.Table1.length > 0) {

                                let s = '';
                                for (let i = 0; i < arr.Table1.length; i++) {
                                    s += '<option value="' + arr.Table1[i].uom_id + '">' + arr.Table1[i].uom_alias + '</option>';
                                }

                                let defUomId = CurrRow.find("#" + UOMID).val();
                                defUomId = (defUomId == "" || defUomId == null) ? arr.Table[0].uom_id : defUomId;

                                CurrRow.find("#" + UOM).html(s);
                                CurrRow.find("#" + UOM).val(defUomId).trigger('change');
                            } else {
                                s = '<option value="' + arr.Table[0].uom_id + '">' + arr.Table[0].uom_alias + '</option>';
                                CurrRow.find("#" + UOM).html(s);
                                CurrRow.find("#" + UOM).val(arr.Table[0].uom_id).trigger('change');
                            }

                        }
                        else {
                            CurrRow.find("#" + UOM).html(`<option value="0">---Select---</option>`);

                        }
                    }

                },
            });
    } catch (err) {
    }
}
/*------------------------------------------UOM Conversion End-------------------------------------------*/

async function onChangeUom(e) {
    debugger;
    let Crow = $(e.target).closest('tr');
    //var QtyDecDigit = $("#QtyDigit").text();

    let Sno = Crow.find("#SNohiddenfiled").val();
    let ItemId = Crow.find("#POItemListName" + Sno).val();
    let UomId = Crow.find("#UOM").val();
    let ItemType = Crow.find("#hdnItemtype").val();

    await Cmn_StockUomWise(ItemId, UomId).then((res) => {
        Crow.find("#AvailableStockInBase").val(res);
        Crow.find("#UOMID").val(UomId);

        if (ItemType == "Consumable") {
            Crow.find("#AvailableStockInBase").val(parseFloat(0).toFixed(QtyDecDigit));
            //Crow.find("#SimpleIssue").attr("disabled", true);commented by shubham maurya on 01-10-2025
            //Crow.find("#MRSNumber").attr("disabled", true);commented by shubham maurya on 01-10-2025
        }

    }).catch(err => console.log(err.message));

    //if (ItemType == "Consumable") {
    //Crow.find("#UOM").val(0);

    //}
}
/***---------------------Print Start-------------------------------***/
function OnCheckedChangeProdDesc() {
    debugger;
    if ($('#chkproddesc').prop('checked')) {
        /* $('#chkshowcustspecproddesc').prop('checked', false);*/
        /* $('#ShowCustSpecProdDesc').val('N');*/
        $("#ShowProdDesc").val("Y");
    }
    else {
        $("#ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        //$('#chkproddesc').prop('checked', false);
        //$("#ShowProdDesc").val("N");
        $('#ShowCustSpecProdDesc').val('Y');
    }
    else {
        $('#ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeProdTechDesc() {
    if ($('#chkprodtechdesc').prop('checked')) {
        $('#ShowProdTechDesc').val('Y');
    }
    else {
        $('#ShowProdTechDesc').val('N');
    }
}
function OnCheckedChangeShowRemarksBlwItm() {
    debugger;
    if ($('#chkshowremarksblwitm').prop('checked')) {
        $('#hdn_ShowRemarksBlwItm').val('Y');
    }
    else {
        $('#hdn_ShowRemarksBlwItm').val('N');
    }
}
function OnCheckedChangeShowSubItem() {
    if ($('#chkshowsubitm').prop('checked')) {
        $('#ShowSubItem').val('Y');
    }
    else {
        $('#ShowSubItem').val('N');
    }
}
function onCheckedChangeFormatBtn() {
    if ($("#OrderTypeI").is(":checked")) {
        $('#PrintFormat').val('F2');
    }
    else {
        $('#PrintFormat').val('F1');
    }
}
function OnCheckedChangeItemAliasName() {/*add by Hina on 14-08-2024*/
    debugger;
    if ($('#chkitemaliasname').prop('checked')) {
        $('#ItemAliasName').val('Y');
    }
    else {
        $('#ItemAliasName').val('N');
    }
}
function OnCheckedChangeSuppAliasName() {/*add by Hina on 14-08-2024*/
    debugger;
    if ($('#chksuppaliasname').prop('checked')) {
        $('#SuppAliasName').val('Y');
    }
    else {
        $('#SuppAliasName').val('N');
    }
}
function OnCheckedChangeDeliverySchedule() {/*add by Hina Sharma on 26-12-2024*/
    if ($('#chkshowdeliveryschedule').prop('checked')) {
        $("#ShowDeliverySchedule").val("Y");
    }
    else {
        $("#ShowDeliverySchedule").val("N");
    }
}
function OnCheckedChangeHsnNumber() {/*add by Hina Sharma on 26-12-2024*/
    if ($('#chkhsnnumber').prop('checked')) {
        $("#ShowHSNNumber").val("Y");
    }
    else {
        $("#ShowHSNNumber").val("N");
    }
}
function OnCheckedChangePayTerms() {/*add by Hina Sharma on 26-12-2024*/
    if ($('#chkshowpayterms').prop('checked')) {
        $("#hdnShowPayTerms").val("Y");
    }
    else {
        $("#hdnShowPayTerms").val("N");
    }
}
function OnCheckedChangePrintQuantityTotal() {
    debugger;
    if ($('#chkshowtotalqty').prop('checked')) {
        $('#hdn_ShowTotalQty').val('Y');
    }
    else {
        $('#hdn_ShowTotalQty').val('N');
    }
}
function OnCheckedChangePrintMRP() {
    debugger;
    if ($('#chkshowmrp').prop('checked')) {
        $('#hdn_ShowNRP').val('Y');
    }
    else {
        $('#hdn_ShowMRP').val('N');
    }
}
function OnCheckedChangePrintPackSize() {
    debugger;
    if ($('#chkshowPackSize').prop('checked')) {
        $('#hdn_ShowPackSize').val('Y');
    }
    else {
        $('#hdn_ShowPackSize').val('N');
    }
}
/***---------------------Print End-------------------------------***/

function FilterItemDetail(e) {//added by Suraj Maurya on 13-02-2025 to filter item details
    Cmn_FilterTableData(e, "PoItmDetailsTbl", [{ "FieldId": "POItemListName", "FieldType": "select", "SrNo": "SNohiddenfiled" }]);
}
// Added by Nidhi on 28-05-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var supp_id = $("#SupplierName").val();
    Cmn_SendEmail(docid, supp_id,'Supp');
}
function ReorderSerialNumbers() {
    $("#emailTableBody tr").each(function (index) {
        $(this).find("td:eq(2)").text((index + 1).toString().padStart(2));
    });
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#po_no").val();
    var Doc_dt = $("#po_date").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/DPO/SendEmailAlert", filepath)
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#po_no").val();
    var Doc_dt = $("#po_date").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray(docid);
        if (docid === "105101140101") {
            var pdfAlertEmailFilePath = 'OTP_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        }
        if (docid === "105101130") {
            var pdfAlertEmailFilePath = 'PO_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        }
        if (docid === "105101136") {
            var pdfAlertEmailFilePath = 'CPI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        }
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/DPO/SavePdfDocToSendOnEmailAlert_Ext",
                data: { poNo: Doc_no, poDate: Doc_dt, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
                /*dataType: "json",*/
                success: function (data) {
                     filepath = data;
                    $('#hdfilepathpdf').val(filepath)
                    Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
                }
            });
        }
        else {
            Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, filepath);
        }
    }
    else {
        Cmn_ViewEmailAlert(mail_id, status, docid, Doc_no, Doc_dt);
    }
}
//Added by Nidhi on 05-08-2025
function EmailAlertLogDetails() {
    var Doc_dt = $("#po_date").val();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#po_no").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
//Added by Nidhi on 06-08-2025
function PrintFormate() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hfStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#po_no").val();
    var Doc_dt = $("#po_date").val();
    if (status == 'A') {
        var PrintFormatArray = GetPrintFormatArray(docid);
        if (docid === "105101140101") {
            var pdfAlertEmailFilePath = 'OTP_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        }
        if (docid === "105101130") {
            var pdfAlertEmailFilePath = 'PO_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        }
        if (docid === "105101136") {
            var pdfAlertEmailFilePath = 'CPI_' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        }
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/DPO/SavePdfDocToSendOnEmailAlert_Ext",
            data: { poNo: Doc_no, poDate: Doc_dt, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
            /*dataType: "json",*/
            success: function (data) {
                var filepath = data;
                $('#hdfilepathpdf').val(filepath);
                $("#btn_mail_print").css("display", "none");
                $("#btn_print").css("display", "");
            }
        });
    }
}
function GetPrintFormatArray(docid) {
    debugger;
    var formatObj = {
        PrintFormat: $('#PrintFormat').val(),
        ShowProdDesc: $("#ShowProdDesc").val(),
        ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
        ShowProdTechDesc: $("#ShowProdTechDesc").val(),
        ShowSubItem: $("#ShowSubItem").val(),
        SupplierAliasName: $('#SuppAliasName').val(),
        ShowDeliverySchedule: $("#ShowDeliverySchedule").val(),
        ShowHsnNumber: $("#ShowHSNNumber").val(),
        ShowRemarksBlwItm: $('#hdn_ShowRemarksBlwItm').val()
    };

    if (docid === "105101140101") {
        formatObj.ShowTotalQty = $("#hdn_ShowTotalQty").val();
    }

    if (docid === "105101130") {
        formatObj.ItemAliasName = $("#ItemAliasName").val();
        formatObj.ShowPayTerms = $("#hdnShowPayTerms").val();
    }

    return [formatObj];
}

//END

function ReplicateWith() {
    debugger;
    var item = $("#ddlReplicateWith").val();
    var Doc_type;
    var Item_type;
    var src_type = $("#src_type").val();
    var SupplierName = $("#SupplierName").val();
  
        if ($("#OrderTypeLocal").is(":checked")) {
            Doc_type = "D";
        }
        if ($("#OrderTypeImport").is(":checked")) {
            Doc_type = "I";
        }
        if ($("#ddlstockable").is(":checked")) {
            Item_type = "S";
          //  $("#ddlAll").attr("disabled", true);
        }
        if ($("#ddlAll").is(":checked")) {
            Item_type = "A";
          //  $("#ddlstockable").attr("disabled", true);
        }
  
        $("#ddlReplicateWith").append("<option value='0'>---Select---</option>");
        $("#ddlReplicateWith").select2({
            ajax: {
                url: "/ApplicationLayer/DPO/BindReplicateWithlist",
                data: function (params) {
                    var queryParameters = {
                        item: params.term,
                        POOrderType: Doc_type,
                        Item_type: Item_type,
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
                    <div class="col-md-4 col-xs-12 def-cursor">${$("#DocNo").text()}</div>
                    <div class="col-md-3 col-xs-12 def-cursor" id="soDocDate">${$("#DocDate").text()}</div>
                    <div class="col-md-5 col-xs-12 def-cursor">${$("#span_CustomerName").text()}</div>
                    </div>
                    </strong></li></ul>`)
                    }
                    var page = 1;
                    data = data.slice((page - 1) * pageSize, page * pageSize)
                    debugger;
                    return {
                        results: $.map(data, function (val, Item) {
                            return { id: val.ID, text: val.ID.split(",")[0], document: val.ID.split(",")[1], UOM: val.Name, };
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
                    '<div class="row">' +
                    '<div class="col-md-4 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                    '<div class="col-md-3 col-xs-12' + classAttr + '">' + data.document + '</div>' +
                    '<div class="col-md-5 col-xs-12' + classAttr + '">' + data.UOM + '</div>' +
                    '</div>'
                );
                return $result;
                firstEmptySelect = false;
            },
        });
  
    
}
function OnChanngReplicateWith() {
    debugger;
    $("#src_type").attr("disabled", true);
    OnchangeSrcDocNumber();
    if ($("#ddlstockable").is(":checked")) {
      
          $("#ddlAll").attr("disabled", true);
    }
    if ($("#ddlAll").is(":checked")) {
      
          $("#ddlstockable").attr("disabled", true);
    }
}


function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        
        var clickedrow = $(evt.target).closest("tr");

        var SNohf = clickedrow.find("#SNohiddenfiled").val();;
        var ItemName = clickedrow.find("#POItemListName" + SNohf + " option:selected").text();
        var ItemId = clickedrow.find("#POItemListName" + SNohf + " option:selected").val();
        var UOMName = clickedrow.find("#UOM option:selected").text();
        Cmn_GetItemStockWhLotBatchSerialWise(ItemId, ItemName, UOMName);

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}