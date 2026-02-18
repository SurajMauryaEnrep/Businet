$(document).ready(function () {
    debugger;
    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    var CustomInvDate = $("#CustomInvDate").val();
    if (CustomInvDate == "" || CustomInvDate == "0" || CustomInvDate == null) {
        $("#CustomInvDate").val(today);
    }
    $("#txttrpt_name").select2();
    //$("#PortOfLoading").select2();
    BindDnCustomerList();
    GetViewDetails();
    $("#ItemUpdateBtn").css("display", "none");
    $('#datatable-buttons2 tbody').on('click', '.deleteIcon', function () {
        debugger;
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            debugger;
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);

        });
        $(this).closest('tr').remove();
        SerialNoAfterDelete();
        AfterDeleteResetPacks();
        ResetTranspoterDetail();
    });
    BindPortOfLoading();
    //BindPlOfReceiptByPreCarrier();
    onpasteValueDiscard();
    CancelledRemarks("#Cancelled", "Disabled");
});
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
function OnChangeCustomInvNo() {
    debugger;
    $("#Span_custom_inv_no").css("display", "none");
    $("#custom_inv_no").css("border-color", "#ced4da");
}
function AfterDeleteResetPacks() {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145120") {
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var QtyDecDigit = $("#QtyDigit").text();
    }
    
    var TotalPack1 = TotalTablePacksQty();
    var packs = parseFloat($("#hdnNumberOfPacks").val())
    totalPacks = packs - parseFloat(TotalPack1);
    $("#NoOfPacks").val(parseFloat(totalPacks).toFixed(QtyDecDigit));
}
function ResetTranspoterDetail() {
    $("#gr_no").val("");
    $("#GRDate").val("");
    $("#txttrpt_name").val(0).trigger("change");
    $("#txtveh_number").val("");
    $("#txtdriver_name").val("");
    $("#MobileNumber").val("");
    $("#txttot_tonnage").val("");

    $("#ItemUpdateBtn").css("display", "none");
    $("#BtnTranspoterAdd").css("display", "block");
}
function SerialNoAfterDelete() {
    var rowIdx = 0;
    debugger;
    $("#datatable-buttons2 >tbody >tr").each(function () {
        var currentRow = $(this);
        currentRow.find("#Srno").text(++rowIdx);
    });
}
var QtyDecDigit = $("#QtyDigit").text();///Quantity
var ValDecDigit = $("#ValDigit").text();
function GetViewDetails() {
    var Doc_No = $("#ShipmentNumber").val();
    $("#hdDoc_No").val(Doc_No);
}
function QtyFloatValueonly(el, evt) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145120") {
        if (Cmn_IntValueonly(el, evt, "#ExpImpQtyDigit") == false) {
            return false;
        }
    }
    else {
        if (Cmn_IntValueonly(el, evt, "#QtyDigit") == false) {
            return false;
        }
    }
    return true;
}
function onchangeFreightAmount(el, e) {
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145120") {
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var QtyDecDigit = $("#QtyDigit").text();
    }
    
    FreightAmount = $("#NoOfPacks").val();
    $("#NoOfPacks").val(parseFloat(CheckNullNumber(FreightAmount)).toFixed(QtyDecDigit));
    $("#Span_No_Of_Packages").css("display", "none");
    $("#NoOfPacks").css("border-color", "#ced4da");
}
//--------------------------------WorkFlow---------------------------------------//
function ForwardBtnClick() {
    debugger;
    //var Status = "";
    //Status = $('#hdShipmentStatus').val().trim();
    //if (Status === "D" || Status === "F") {

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

    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        var ShipDate = $("#txtship_dt").val();
        $.ajax({
            type: "POST",
            /*  url: "/Common/Common/CheckFinancialYear",*/
            url: "/Common/Common/CheckFinancialYearAndPreviousYear",
            data: {
                compId: compId,
                brId: brId,
                DocDate: ShipDate
            },
            success: function (data) {
                /*   if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
                /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                if (data == "TransAllow") {
                    var Status = "";
                    Status = $('#hdShipmentStatus').val().trim();
                    if (Status === "D" || Status === "F") {

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
                    /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
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
function AddTranspoterDetails() {
    debugger;

    var TransportDetail = CheckTransportDetailValidation();
    if (TransportDetail == false) {
        return false;
    }
    var rowIdx = "";
    var rowId = $("#datatable-buttons2 tbody tr").length;
    var datatable = $(".dataTables_empty").length;
    if (datatable == "1") {
        $("#datatable-buttons2 tbody tr").remove();
    }
    if (datatable == "1") {
        //$("#datatable-buttons2 tbody tr").remove();
        rowIdx = 1;
    }
    else {
        rowIdx = parseInt(rowId) + 1;
    }
    var duplicate = "N";
    var gr_no = $("#gr_no").val();
    $("#datatable-buttons2 >tbody >tr").each(function () {
        var currentRow = $(this);
        var tblGrNo = currentRow.find("#GrNumber").text();
        if (tblGrNo == gr_no) {
            duplicate = "Y";
        }
    });
    if (duplicate == "Y") {
        $("#Span_GRNumber").text($("#valueduplicate").text());
        $("#Span_GRNumber").css("display", "block");
        $("#gr_no").css("border-color", "red");
        return false;
    }

    //var totalPackegs = TotalnNumberOfPackags();

    var GRDate = $("#GRDate").val();
    var GRDate1 = moment(GRDate).format('DD-MM-YYYY');
    var txttrpt_name = $("#txttrpt_name option:selected").text();
    var trpt_id = $("#txttrpt_name").val();
    var txtveh_number = $("#txtveh_number").val();
    var txtdriver_name = $("#txtdriver_name").val();
    var MobileNumber = $("#MobileNumber").val();
    var txttot_tonnage = $("#txttot_tonnage").val();
    var packes = $("#NoOfPacks").val();

    var TotalPack1 = TotalTablePacksQty();
    var packs1 = parseFloat(TotalPack1) + parseFloat(packes);

    if (parseFloat(packs1) > parseFloat($("#hdnNumberOfPacks").val())) {
        $("#Span_No_Of_Packages").text($("#ExceedingPackages").text());
        $("#Span_No_Of_Packages").css("display", "block");
        $("#NoOfPacks").css("border-color", "red");
    }
    else {
        $('#datatable-buttons2 tbody').append(`<tr id="R ${rowIdx}">
 <td class="center">
  <i type="submit" class="fa fa-edit" id="editBtnIcon" onclick="EditTranspoterDetails()" aria-hidden="true" title="${$("#Edit").text()}"></i>
  </td>
  <td class="center">
     <i class="deleteIcon fa fa-trash red" aria-hidden="true" title="${$("#Span_Delete_Title").text()}" onclick=""></i>
  </td>
  <td class="sorting_1" id="Srno">${rowIdx}</td>
  <td id="GrNumber">${gr_no}</td>
  <td id="GRDt">${GRDate1}</td>
 <td id="tdNumOfPackegs">${packes}</td>
  <td id="hdnGRDt" hidden="hidden">${GRDate}</td>
  <td id="trpt_name">${txttrpt_name}</td>
  <td id="trpt_ID" hidden="hidden">${trpt_id}</td>
  <td id="veh_number">${txtveh_number}</td>
  <td id="driver_name">${txtdriver_name}</td>
  <td id="Mobile_no">${MobileNumber}</td>
  <td class="num_right" id="tot_tonnage">${txttot_tonnage}</td>
 </tr>`);

        $("#gr_no").val("");
        $("#GRDate").val("");
        $("#txttrpt_name").val(0).trigger("change");
        $("#txtveh_number").val("");
        $("#txtdriver_name").val("");
        $("#MobileNumber").val("");
        $("#txttot_tonnage").val("");
        debugger;
        var TotalPack = TotalTablePacksQty();

        var TatalPacks = parseFloat($("#hdnNumberOfPacks").val()) - parseFloat(TotalPack);
        
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145120") {
            var QtyDecDigit = $("#ExpImpQtyDigit").text();
        }
        else {
            var QtyDecDigit = $("#QtyDigit").text();
        }
        $("#NoOfPacks").val(parseFloat(TatalPacks).toFixed(QtyDecDigit));
    }
}
function TotalTablePacksQty() {
    var NumberOfPacks = 0;
    $("#datatable-buttons2 TBODY TR").each(function () {
        var row = $(this);
        var NoOfPacks = row.find("#tdNumOfPackegs").text();
        if (NoOfPacks == "") {
            NoOfPacks = 0;
        }
        NumberOfPacks = NumberOfPacks + parseFloat(NoOfPacks);
    });
    return NumberOfPacks;
}
function TotalTablePacksQty1() {
    var NumberOfPacks = 0;
    var GrNo = $("#hdnGrNumber").val();
    $("#datatable-buttons2 TBODY TR").each(function () {
        var row = $(this);
        var tblGrNo = row.find("#GrNumber").text();
        if (tblGrNo == GrNo) {

        }
        else {
            var NoOfPacks = row.find("#tdNumOfPackegs").text();
            if (NoOfPacks == "") {
                NoOfPacks = 0;
            }
            NumberOfPacks = NumberOfPacks + parseFloat(NoOfPacks);
        }
    });
    return NumberOfPacks;
}
function EditTranspoterDetails() {
    debugger;
    $("#BtnTranspoterAdd").css("display", "none");
    $("#ItemUpdateBtn").css("display", "block");

    OnChangeGRNumber();
    OnChangeGRDate();
    OnChangeTransporterName();
    OnChangeVehicleNumber();

    $("#datatable-buttons2 >tbody >tr").on('click', "#editBtnIcon", function (e) {

        debugger;
        var currentRow = $(this).closest('tr')
        var row_index = currentRow.index();

        var GrNumber = currentRow.find("#GrNumber").text()
        var GRDt = currentRow.find("#hdnGRDt").text()
        var trpt_ID = currentRow.find("#trpt_ID").text()
        var veh_number = currentRow.find("#veh_number").text()
        var driver_name = currentRow.find("#driver_name").text()
        var Mobile_no = currentRow.find("#Mobile_no").text()
        var tot_tonnage = currentRow.find("#tot_tonnage").text()
        var tdNumOfPackegs = currentRow.find("#tdNumOfPackegs").text()

        //var GRDt1 = moment(GRDt).format('yyyy-MM-DD');
        $("#gr_no").val(GrNumber);
        $("#GRDate").val(GRDt);
        $("#txttrpt_name").val(trpt_ID).trigger("change");
        $("#txtveh_number").val(veh_number);
        $("#txtdriver_name").val(driver_name);
        $("#MobileNumber").val(Mobile_no);
        $("#txttot_tonnage").val(tot_tonnage);
        $("#hdnGrNumber").val(GrNumber);
        $("#NoOfPacks").val(tdNumOfPackegs);
    });
}
function OnClickTranspoterUpdateBtn() {
    debugger;
    var TransportDetail = CheckTransportDetailValidation();
    if (TransportDetail == false) {
        return false;
    }
    var GrNo = $("#hdnGrNumber").val();
    var gr_no = $("#gr_no").val();
    var GRDate = $("#GRDate").val();
    var txttrpt_name = $("#txttrpt_name option:selected").text();
    var trpt_id = $("#txttrpt_name").val();
    var txtveh_number = $("#txtveh_number").val();
    var txtdriver_name = $("#txtdriver_name").val();
    var MobileNumber = $("#MobileNumber").val();
    var txttot_tonnage = $("#txttot_tonnage").val();
    var NoOfPacks = $("#NoOfPacks").val();

    var duplicate = "N";
    //var gr_no = $("#gr_no").val();
    $("#datatable-buttons2 >tbody >tr").each(function () {
        var currentRow = $(this);
        var tblGrNo = currentRow.find("#GrNumber").text();
        if (GrNo != tblGrNo && tblGrNo == gr_no) {
            duplicate = "Y";
        }
    });
    if (duplicate == "Y") {
        $("#Span_GRNumber").text($("#valueduplicate").text());
        $("#Span_GRNumber").css("display", "block");
        $("#gr_no").css("border-color", "red");
        return false;
    }
    var TotalPack1 = TotalTablePacksQty1();
    var packs = parseFloat(TotalPack1) + parseFloat(NoOfPacks);
    if (parseFloat(packs) > parseFloat($("#hdnNumberOfPacks").val())) {
        $("#Span_No_Of_Packages").text($("#ExceedingQty").text());
        $("#Span_No_Of_Packages").css("display", "block");
        $("#NoOfPacks").css("border-color", "red");
    }
    else {
        $("#datatable-buttons2 >tbody >tr").each(function () {
            var currentRow = $(this);
            var tblGrNo = currentRow.find("#GrNumber").text();
            if (tblGrNo == GrNo) {
                currentRow.find("#GrNumber").text(gr_no);
                currentRow.find("#GRDt").text(moment(GRDate).format('DD-MM-YYYY'));
                currentRow.find("#hdnGRDt").text(GRDate);
                currentRow.find("#trpt_ID").text(trpt_id);
                currentRow.find("#trpt_name").text(txttrpt_name);
                currentRow.find("#veh_number").text(txtveh_number);
                currentRow.find("#driver_name").text(txtdriver_name);
                currentRow.find("#Mobile_no").text(MobileNumber);
                currentRow.find("#tot_tonnage").text(txttot_tonnage);
                currentRow.find("#tdNumOfPackegs").text(NoOfPacks);
            }
        });
        $("#gr_no").val("");
        $("#GRDate").val("");
        $("#txttrpt_name").val(0).trigger("change");
        $("#txtveh_number").val("");
        $("#txtdriver_name").val("");
        $("#MobileNumber").val("");
        $("#txttot_tonnage").val("");

        var TotalPack = TotalTablePacksQty();
        var TatalPacks = parseFloat($("#hdnNumberOfPacks").val()) - parseFloat(TotalPack);
        
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145120") {
            var QtyDecDigit = $("#ExpImpQtyDigit").text();
        }
        else {
            var QtyDecDigit = $("#QtyDigit").text();
        }
        $("#NoOfPacks").val(parseFloat(TatalPacks).toFixed(QtyDecDigit));

        $("#ItemUpdateBtn").css("display", "none");
        $("#BtnTranspoterAdd").css("display", "block");
    }
}
function OnchangePortOfLoading() {
    debugger;
    //var PortOfLoading = $("#PortOfLoading option:selected").text();
    $("#Span_loading_port").css("display", "none");
    $("[aria-labelledby='select2-PortOfLoading-container']").css("border-color", "#ced4da");
}
function InsertTransPoterDetails() {
    debugger;
    var TransPoterList = new Array();
    $("#datatable-buttons2 TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var ItemList = {};
        ItemList.GrNumber = currentRow.find("#GrNumber").text();
        ItemList.GRDt = currentRow.find("#hdnGRDt").text();
        ItemList.trpt_ID = currentRow.find("#trpt_ID").text();
        ItemList.trpt_name = currentRow.find("#trpt_name").text();
        ItemList.veh_number = currentRow.find("#veh_number").text();
        ItemList.driver_name = currentRow.find("#driver_name").text();
        ItemList.Mobile_no = currentRow.find("#Mobile_no").text();
        ItemList.tot_tonnage = currentRow.find("#tot_tonnage").text();
        ItemList.no_of_pkgs = currentRow.find("#tdNumOfPackegs").text();

        TransPoterList.push(ItemList);
    });
    return TransPoterList;
}
function OnClickbillingAddressIconBtn(e) {
    debugger;

    $('#AddressInfo_LblName').text($('#AddressInfo_BillAdd').text());
    var Cust_id = $('#ddlCustomer_Name').val();
    var Cust_id = $("#ddlCustomer_Name option:selected").val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var CustPros_type = "C";

    var status = $("#hdShipmentStatus").val().trim();
    var SODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SODTransType = "Update";
    }
    var SO_no = $("#ShipmentNumber").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, bill_add_id, status, SODTransType);
}
function OnClickShippingAddressIconBtn(e) {
    debugger;
    $('#AddressInfo_LblName').text($('#AddressInfo_ShipAdd').text());
    var Cust_id = $('#ddlCustomer_Name').val();
    var ship_add_id = $('#ship_add_id').val().trim();
    $('#hd_add_type').val("S");
    var CustPros_type = "C";
    var status = $("#hdShipmentStatus").val().trim();
    var SODTransType = "";
    if ($("#ForDisableOCDlt").val() == "Enable") {
        SODTransType = "Update";
    }

    var ShipmentNumber = $("#ShipmentNumber").val();
    Cmn_AddrInfoBtnClick(Cust_id, CustPros_type, ship_add_id, status, SODTransType, ShipmentNumber);
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var mailerror = "";

    Remarks = $("#fw_remarks").val();
    DocNo = $("#ShipmentNumber").val();
    DocDate = $("#txtship_dt").val();
    docid = $("#DocumentMenuId").val();
    var WF_Status1 = $("#WF_Status1").val();
    var shp_typ = $("#ShipMent_type").val();
    var Approve = "Approve";
    var TrancType = (DocNo + ',' + DocDate + ',' + docid + ',' + WF_Status1 + ',' + shp_typ)
    var ListFilterData1 = $("#ListFilterData1").val();
    //var Narr = $('#JVRaisedAgainstShipment').text(); 
    $("#forwardDetailsTbl >tbody >tr").each(function () {
        var currentRow = $(this);
        debugger;
        var Userid = currentRow.find("#user").text();
        if ($("#r_" + Userid).is(":checked")) {
            //docid = currentRow.find("td:eq(2)").text();
            forwardedto = currentRow.find("#user").text();
            level = currentRow.find("#level").text();
        }
    });
    if (fwchkval != "Approve") {
        var pdfAlertEmailFilePath = "Shipment_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/Shipment/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }

    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/Shipment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        var ShipType = "";
        if ($("#rbDomestic").is(":checked")) {
            ShipType = "D"
        }
        if ($("#rbExport").is(":checked")) {
            ShipType = "E"
        }
        window.location.href = "/ApplicationLayer/Shipment/ShipmentApprove?DocNo=" + DocNo + "&DocDate=" + DocDate + "&ShipType=" + ShipType + "&A_Status=Approve" + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&command=" + Approve + "&ListFilterData1=" + ListFilterData1 + "&docid=" + docid + "&WF_Status1=" + WF_Status1;
        // InsertPOApproveDetails("Approve", $("#hd_currlevel").val(), Remarks);
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/Shipment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/Shipment/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
    }
}
//function GetPdfFilePathToSendonEmailAlert(shpNo, shpDt, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/Shipment/SavePdfDocToSendOnEmailAlert",
//        data: { shpNo: shpNo, shpDt: shpDt, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#ShipmentNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}

function BindDnCustomerList() {
    // debugger;
    var Branch = sessionStorage.getItem("BranchID");
    var DocId = $("#DocumentMenuId").val();
    $("#ddlCustomer_Name").select2({
        ajax: {
            url: $("#CustNameList").val(),
            data: function (params) {
                debugger;
                var queryParameters = {
                    SO_CustName: params.term,
                    BrchID: Branch,
                    DocumentMenuId: DocId
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    LSO_ErrorPage();
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
function OnChangeCustomer() {
    try {
        debugger;
        var CustID = $('#ddlCustomer_Name').val();
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/Shipment/GetCustomerAddress",
                data: { CustID: CustID },
                success: function (data) {
                    debugger;
                    $('#CustomerAddress').html(data);
                    var custtrnsp_id = $("#cust_trnsportid").val();
                    if (custtrnsp_id != "" && custtrnsp_id != "0" && custtrnsp_id != null) {
                        $("#txttrpt_name").val(custtrnsp_id).trigger('change');
                    }
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
    debugger;
  
    
    BindddlPackList();
    debugger;
    var CustomerName = $('#ddlCustomer_Name').val();
    var ShippingAddress = $('#TxtShippingAddr').val();
    var BillingAddress = $('#TxtBillingAddr').val();

    if (CustomerName == "" || CustomerName == "0") {
        document.getElementById("vmcust_id").innerHTML = $("#valueReq").text();
        $(".select2-container--default .select2-selection--single").css("border-color", "red");

    }
    else {
        var CustID = $("#ddlCustomer_Name option:selected").val();
        $("#Hdn_ShipCustID").val(CustID);
        var CustName = $("#ddlCustomer_Name option:selected").text();
        $("#Hdn_ShipCustName").val(CustName);
        $("#ddlCustomer_Name").val(CustID);

        document.getElementById("vmcust_id").innerHTML = "";
        $(".select2-container--default .select2-selection--single").css("border-color", "#ced4da");
    }

    if (BillingAddress == "" || BillingAddress == "0") {
        document.getElementById("vm_bill_address").innerHTML = $("#valueReq").text();
        $("#TxtBillingAddr").css("border-color", "red");

    }
    else {


        document.getElementById("vm_bill_address").innerHTML = "";
        $("#TxtBillingAddr").css("border-color", "#ced4da");
    }
    if (ShippingAddress == "" || ShippingAddress == "0") {
        document.getElementById("vm_ship_address").innerHTML = $("#valueReq").text();
        $("#TxtShippingAddr").css("border-color", "red");

    }
    else {
        document.getElementById("vm_ship_address").innerHTML = "";
        $("#TxtShippingAddr").css("border-color", "#ced4da");
    }

    $('#ddlPackListNumber').val("0").trigger('change.select2');
    $("#ddlPackListNumber").val('0');
    $("#hdcust_Id").val($("#ddlCustomer_Name option:selected").val());
    $("#txpack_dt").val("");

}
function OnchangePlOfReceiptByPreCarrier() {
    $("#Span_pi_rcpt_carr").css("display", "none");
    $("[aria-labelledby='select2-PlOfReceiptByPreCarrier-container']").css("border-color", "#ced4da");
}
function BindPlOfReceiptByPreCarrier() {
    debugger;
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/Shipment/GetPlOfReceiptByPreCarrierList",
        success: function (data) {
            debugger
            arr = JSON.parse(data);
            if (arr.length > 0) {
                $("#PlOfReceiptByPreCarrier option").remove();
                $("#PlOfReceiptByPreCarrier optgroup").remove();
                $("#PlOfReceiptByPreCarrier").attr("onchange", "OnchangePlOfReceiptByPreCarrier()");
                $('#PlOfReceiptByPreCarrier').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#span_PortOfLoading").text()}' data-date='${$("#span_State").text()}'></optgroup>`);
                for (var i = 0; i < arr.length; i++) {
                    $('#Textddl').append(`<option data-date="${arr[i].state_name}" value="${arr[i].port_id}">${arr[i].port_desc}</option>`);
                }
            }
            var firstEmptySelect = true;
            $('#PlOfReceiptByPreCarrier').select2({
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
            var Hdnloading_port = $("#Hdnpi_rcpt_carr").val();
            if (Hdnloading_port != "") {
                $('#PlOfReceiptByPreCarrier').val(Hdnloading_port).trigger('change')
            }
        },
    })
}
function BindPortOfLoading() {
    debugger;
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/Shipment/GetPortOfLoadingList",
        success: function (data) {
            debugger
            arr = JSON.parse(data);
            if (arr.length > 0) {
                $("#PortOfLoading option").remove();
                $("#PortOfLoading optgroup").remove();
                $("#PortOfLoading").attr("onchange", "OnchangePortOfLoading()");
                $('#PortOfLoading').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#span_PortOfLoading").text()}' data-date='${$("#span_State").text()}'></optgroup>`);
                for (var i = 0; i < arr.length; i++) {
                    $('#Textddl').append(`<option data-date="${arr[i].state_name}" value="${arr[i].port_id}">${arr[i].port_desc}</option>`);
                }
            }
            var firstEmptySelect = true;
            $('#PortOfLoading').select2({
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
            var Hdnloading_port = $("#Hdnloading_port").val();
            if (Hdnloading_port != "") {
                $('#PortOfLoading').val(Hdnloading_port).trigger('change')
            }
            BindPlOfReceiptByPreCarrier();
        },
    })
}
function BindddlPackList() {
    debugger;
    var CustID = $('#ddlCustomer_Name').val();
    $("#ddlPackListNumber").attr("onchange", "");
    $.ajax({
        type: "Post",
        url: "/ApplicationLayer/Shipment/ShipmentPackingList",
        data: {
            CustID: CustID
        },
        success: function (data) {
            debugger
            arr = JSON.parse(data);
            if (arr.Table.length > 0) {

                $("#ddlPackListNumber option").remove();
                $("#ddlPackListNumber optgroup").remove();
                $("#ddlPackListNumber").attr("onchange", "ddlPackingListNumberSelect()");
                $('#ddlPackListNumber').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                for (var i = 0; i < arr.Table.length; i++) {

                    $('#Textddl').append(`<option data-date="${arr.Table[i].pack_dt}" value="${arr.Table[i].pack_no}">${arr.Table[i].pack_no}</option>`);
                }
                var firstEmptySelect = true;
                $('#ddlPackListNumber').select2({
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
            }

        },
    })

}
function ddlPackingListNumberSelect() {
    debugger;
    //var SourceDocumentDate = $('#ddlPackListNumber').val();
    var doc_Date = $("#ddlPackListNumber option:selected")[0].dataset.date
    var date = doc_Date.split("-");
    var FDate = date[2] + '-' + date[1] + '-' + date[0];
    $("#txpack_dt").val(FDate);
    var ListNumberNumber = $('#ddlPackListNumber').val();
    if (ListNumberNumber != "0" && ListNumberNumber != "---Select---") {
        document.getElementById("vmso_no").innerHTML = null;
        $("#ddlPackListNumber").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddlPackListNumber-container']").css("border-color", "#ced4da");

        var docid = $("#DocumentMenuId").val();
        if (docid == "105103145120") {
            try {
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/Shipment/getcurr_Detail",
                        data: {
                            Pack_NO: ListNumberNumber,
                            Pack_date: FDate
                        },
                        success: function (data) {
                            debugger;
                            if (data !== null && data !== "") {
                                var arr = [];
                                arr = JSON.parse(data);
                                if (arr.length > 0) {
                                    $("#txtCurr_des").val(arr[0].curr_name);
                                    $("#HdnCurr_Id").val(arr[0].curr_id);
                                }
                                else {
                                    $("#txtCurr_des").val("");
                                    $("#HdnCurr_Id").val("");
                                }
                            }
                            else {
                                $("#txtCurr_des").val("");
                                $("#HdnCurr_Id").val("");
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            debugger;
                        }
                    });

            } catch (err) {
                console.log("UserValidate Error : " + err.message);
            }
        }
    }
    else {
        document.getElementById("vmso_no").innerHTML = $("#valueReq").text();
        $("#ddlPackListNumber").css("border-color", "red");
        $("[aria-labelledby='select2-ddlPackListNumber-container']").css("border-color", "red");
        $("#txpack_dt").val("");
    }
}

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();

    if (ItmCode != "" && ItmCode != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/LSODetail/GetSOItemDetail",
                    data: { ItemID: ItmCode },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
                                $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
                                $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
                                $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
                                var ImgFlag = "N";
                                for (var i = 0; i < arr.Table.length; i++) {
                                    if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
                                        ImgFlag = "Y";
                                    }
                                }
                                if (ImgFlag == "Y") {
                                    var OL = '<ol class="carousel-indicators">';
                                    var Div = '<div class="carousel-inner">';
                                    for (var i = 0; i < arr.Table.length; i++) {
                                        var ImgName = arr.Table[i].item_img_name;
                                        var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
                                        if (i === 0) {
                                            OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
                                            Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
                                        }
                                        else {
                                            OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
                                            Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
                                        }
                                    }
                                    OL += '</ol>'
                                    Div += '</div>'

                                    var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
                                    var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

                                    $("#myCarousel").html(OL + Div + Ach + Ach1);
                                }
                                else {
                                    $("#myCarousel").html("");
                                }
                            }
                        }
                    },
                });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }
}
function AddPackingListDetails() {
    debugger;
    
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145120") {
        var QtyDecDigit = $("#ExpImpQtyDigit").text();///Quantity
    }
    else {
        var QtyDecDigit = $("#QtyDigit").text();///Quantity
    }
    var WeightDigit = $("#WeightDigit").text();
    var packno = $('#ddlPackListNumber option:selected').text();

    if ($('#ddlPackListNumber').val() != "0" && $('#ddlPackListNumber').val() != "" && $('#ddlPackListNumber').val() != "---Select---") {
        //$(".plus_icon1").css('display', 'none');
        $("#PopulateItem").css('display', 'none');
        debugger;
        $("#ddlCustomer_Name").prop("disabled", true);
        $("#ddlPackListNumber").prop("disabled", true);

        var packdate = $("#txpack_dt").val();
        $("#hd_pack_dt").val(packdate);
        $("#hd_pack_no").val(packno);
        var Shipaddresss = $("#TxtShippingAddr").val();
        var Billaddresss = $("#TxtBillingAddr").val();
        $("#TxtBillingAddr").val(Billaddresss);
        $("#TxtShippingAddr").val(Shipaddresss);

        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/Shipment/getDetailPckingListByPackNo",
                data: {
                    Pack_NO: packno,
                    Pack_date: packdate,
                    DocumentMenuId: DocumentMenuId
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var Iserial = "";
                            var Ibatch = "";
                            var ItemType = "";
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                //if (arr.Table[i].i_serial == 'Y') {
                                //    Iserial = `<td class="center"><button type="button" id="btnserialdeatil" onclick="ItemStockSerialWise(this,event)" class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                //                <input type="hidden" id="hdi_serial" value="Y" style="display: none;" /></td>`;
                                //}
                                //else {
                                //    Iserial = `<td class="center"><button type="button" id="btnserialdeatil" onclick="ItemStockSerialWise(this,event)" disabled class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                //                <input type="hidden" id="hdi_serial" value="N" style="display: none;" /></td>`;
                                //}
                                Ibatch = `<td class="center"><button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)" onchange="OnChangeIssueQty" class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_batch" value="${arr.Table[i].i_batch}" style="display: none;" />
                                                <input type="hidden" id="hdi_serial" value="${arr.Table[i].i_serial}" style="display: none;" /></td>`;

                                //if (arr.Table[i].i_batch == 'Y') {
                                //    Ibatch = `<td class="center"><button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)" onchange="OnChangeIssueQty" class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                //                <input type="hidden" id="hdi_batch" value="Y" style="display: none;" />
                                //                <input type="hidden" id="hdi_serial" value="${arr.Table[i].i_serial}" style="display: none;" /></td>`;
                                //}
                                //else {
                                //    Ibatch = `<td class="center"><button type="button" id="btnbatchdeatil" onclick="ItemStockBatchWise(this,event)"  onchange="OnChangeIssueQty" disabled class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                //            <input type="hidden" id="hdi_batch" value="N" style="display: none;" /></td>`;
                                //}

                                if (arr.Table[i].i_batch == 'N' && arr.Table[i].i_serial == 'N') {
                                    ItemType = `<td style="display:none"><input id="ItemType" type="text" value="YES" /></td>`;
                                }
                                else {
                                    ItemType = `<td style="display:none"><input id="ItemType" type="text" value="NO" /></td>`;
                                }
                                var PackSize = "";
                                if (arr.Table[i].pack_size != null) {
                                    PackSize = arr.Table[i].pack_size;
                                }
                                $('#ShipmentItmDetailsTbl tbody').append(`    <tr id="${++rowIdx}">
                <td><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled" value="${rowIdx}"</td>                                                               
                <td class="ItmNameBreak itmStick tditemfrz"><div class="col-sm-10" style="padding:0px;">
                <input id="ItemName" class="form-control" type="text" name="OrderNumber" disabled><input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                </div><div class="col-sm-1 i_Icon"><button type="button" onclick="OnClickIconBtn(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" ><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"></button>
                </div><div class="col-sm-1" style="padding:0px; text-align:right;"><button type="button" id="BuyerInfoIcon" class="calculator" onclick="OnClickBuyerInfoIconBtn(event);" data-toggle="modal" data-target="#CustomerInformation" data-backdrop="static" data-keyboard="false"> <i class="fa fa-user" aria-hidden="true" title="${$("#Span_CustomerInformation_Title").text()}"></i> </button></div></td>
                <td><input id="UOM"  value="${arr.Table[i].uom_name}" class="form-control" autocomplete="off" type="text" name="UOM"  placeholder="${$("#ItemUOM").text()}" disabled><input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" /></td>
<td>
                                                                                <input id="PackSize" maxlength="50" class="form-control remarksmessage" autocomplete="off" type="text" name="PackSize" placeholder="${$("#span_PackSize").text()}" value="${PackSize}" onmouseover="OnMouseOver(this)" disabled>
                                                                            </td>
<td>
                <div class="col-sm-10 lpo_form no-padding">
                <input id="PackedQuantity" value="${parseFloat(arr.Table[i].pack_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="PackedQuantity" disabled>
                </div>
                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemOrderQty">
                <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                <button type="button" id="SubItemOrderQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('ShipPacked',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                </div>
                </td>
                <td><input id="NumberOfPacks" value="${parseFloat(arr.Table[i].pack_nos).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="NumberOfPacks" disabled></td>
                <td><input id="NetWeight" value="${parseFloat(arr.Table[i].net_wght).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="NetWeight" disabled></td>
                <td><input id="GrossWeight" value="${parseFloat(arr.Table[i].gr_wght).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="GrossWeight" disabled></td>
                <td><input id="CBM" value="${parseFloat(arr.Table[i].tot_cbm).toFixed(QtyDecDigit)}" class="form-control num_right" type="text" name="CBM" disabled></td>
                <td>
                <div class="col-sm-10 lpo_form" style="padding:0px;">
                <input id="ShippedQuantity" value="${parseFloat(arr.Table[i].ship_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="" type="text" name="ShippedQuantity" disabled>
                </div>
                <div class="col-sm-2 i_Icon no-padding" id="div_SubItemTrfQty" >
                <button type="button" id="SubItemTrfQty" ${subitmDisable} class="calculator subItmImg" onclick="return SubItemDetailsPopUp('Shipped',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                </div>
                </td>
                <td width="7%"><input id="txtWh_name" value="${arr.Table[i].wh_name}" class="form-control" type="text" name="txtWh_name" disabled><input type="hidden" id="hdwh_Id" value="${arr.Table[i].wh_id}" style="display: none;" /></td>
                <td>
                    <div class="col-sm-10 lpo_form no-padding">
                    <input id="AvailableStock" value="${parseFloat(arr.Table[i].availale_qty).toFixed(QtyDecDigit)}"  class="form-control num_right" type="text" name="AvailableStock" disabled>
                     </div>
                    <div class="col-sm-2 i_Icon no-padding" id="div_SubItemAvlStk">
                        <button type="button" id="SubItemAvlStk" ${subitmDisable} class="calculator subItmImg" onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${$("#span_SubItemDetail").text()}"> </button>
                    </div>
                 </td>
                <td><input id="InvoicedQuantity" value="${parseFloat(arr.Table[i].invoice_qty).toFixed(QtyDecDigit)}"  class="form-control num_right" type="text" name="InvoicedQuantity" disabled></td>`
                                    + Ibatch + ``
                                    + Iserial + ``
                                    + ItemType + `
                <td>
                <textarea id="remarks" maxlength="100" value="${arr.Table[i].it_remarks}" class="form-control remarksmessage" name="remarks" placeholder="${$("#span_remarks").text()}" ></textarea >
                </td>
                </tr>`);
                                var clickedrow = $("#ShipmentItmDetailsTbl >tbody >tr #SNohiddenfiled[value=" + rowIdx + "]").closest("tr");
                                clickedrow.find("#ItemName").val(arr.Table[i].item_name);
                                //BindWarehouseList(rowIdx);
                            }
                            var s = $("#NetWeight").val();
                            $("#TotalGrossWeight").val(parseFloat(arr.Table[0].tot_gr_wght).toFixed(WeightDigit));
                            $("#TotalNetWeight").val(parseFloat(arr.Table[0].tot_net_wght).toFixed(WeightDigit));
                            $("#TotalCBM").val(parseFloat(arr.Table[0].tot_hcbm).toFixed(QtyDecDigit));


                            if (arr.Table1.length > 0) {
                                for (var j = 0; j < arr.Table1.length; j++) {
                                    $('#SaveItemBatchTbl tbody').append(`<tr>
                                  <td><input type="text" id="shipmentlineBatchLotNo" value="${arr.Table1[j].lot_no}" /></td>
                                  <td><input type="text" id="shipmentlineBatchItemId" value="${arr.Table1[j].item_id}" /></td>
                                  <td><input type="text" id="shipmentlineBatchUOMId" value="${arr.Table1[j].uom_id}" /></td>
                                  <td><input type="text" id="shipmentlineBatchBatchNo" value="${arr.Table1[j].batch_no}" /></td>
                                  <td><input type="text" id="shipmentlineBatchBatchAvlStk" value="${parseFloat(arr.Table1[j].avl_batch_qty).toFixed(QtyDecDigit)}" /></td>
                                  <td><input type="text" id="shipmentlineBatchIssueQty" value="${parseFloat(arr.Table1[j].issue_qty).toFixed(QtyDecDigit)} " /></td>
                                  <td><input type="text" id="shipmentlineBatchExpiryDate" value="${arr.Table1[j].expiry_date}" /></td>
                                  <td><input type="text" id="shipmentlineBatchMfgName" value="${IsNull(arr.Table1[j].mfg_name,'')}" /></td>
                                  <td><input type="text" id="shipmentlineBatchMfgMrp" value="${IsNull(arr.Table1[j].mfg_mrp,'')}" /></td>
                                  <td><input type="text" id="shipmentlineBatchMfgDate" value="${IsNull(arr.Table1[j].mfg_date,'')}" /></td>
                                  </tr>`);
                                }
                            }
                            //if (arr.Table2.length > 0) {
                            //    for (var k = 0; k < arr.Table2.length; k++) {
                            //        $('#SaveItemSerialTbl tbody').append(`<tr>
                            //          <td><input type="text" id="shipmentlineSerialItemId" value="${arr.Table2[k].item_id}" /></td>
                            //          <td><input type="text" id="shipmentlineSerialUOMId" value="${arr.Table2[k].uom_id}" /></td>
                            //          <td><input type="text" id="shipmentlineSerialLOTNo" value="${arr.Table2[k].lot_no}" /></td>
                            //          <td><input type="text" id="shipmentlineSerialIssueQty" value="${arr.Table2[k].issue_qty}" /></td>
                            //          <td><input type="text" id="shipmentlineBatchSerialNO" value="${arr.Table2[k].serial_no}" /></td>
                            //          </tr>`);
                            //    }
                            //}
                            debugger;
                            if (arr.Table2.length > 0) {
                                var rowIdx = 0;
                                for (var y = 0; y < arr.Table2.length; y++) {
                                    var ItmId = arr.Table2[y].item_id;
                                    var SubItmId = arr.Table2[y].sub_item_id;
                                    var ShippedQty = arr.Table2[y].shipped_qty;

                                    $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                                    <td><input type="text" id="ItemId" value='${ItmId}'></td>
                                    <td><input type="text" id="subItemId" value='${SubItmId}'></td>
                                    <td><input type="text" id="subItemQty" value='${parseFloat(ShippedQty).toFixed(QtyDecDigit)}'></td>
                                    </tr>`);
                                }

                            }
                        }
                        CalculateNumberOfPacks();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                }
            });
    } else {

        document.getElementById("vmso_no").innerHTML = $("#valueReq").text();
        $("#ddlPackListNumber").css("border-color", "red");
        $("[aria-labelledby='select2-ddlPackListNumber-container']").css("border-color", "red");
    }
}
function CalculateNumberOfPacks() {
    debugger;
    var NumberOfPacks = 0;    
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145120") {
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var QtyDecDigit = $("#QtyDigit").text();
    }
    $("#ShipmentItmDetailsTbl TBODY TR").each(function () {
        var row = $(this);
        var NoOfPacks = row.find("#NumberOfPacks").val();
        NumberOfPacks = NumberOfPacks + parseFloat(NoOfPacks);
    });
    $("#NoOfPacks").val(parseFloat(NumberOfPacks).toFixed(QtyDecDigit));
    $("#hdnNumberOfPacks").val(parseFloat(NumberOfPacks).toFixed(QtyDecDigit));
}
function CheckFormValidation() {

    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 10-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var rowcount = $('#ShipmentItmDetailsTbl tr').length;
    var ValidationFlag = true;
    var CustomerName = $('#ddlCustomer_Name').val();
    var PackingListNumber = $('#ddlPackListNumber').val();
    var ShippingAddress = $('#TxtShippingAddr').val();
    var BillingAddress = $('#TxtBillingAddr').val();

    /**Added BY Nitesh 02-03-2024 For Check Domestic And Export**/
    if ($("#rbDomestic").is(":checked")) {
        ShipType = "D"
    }
    if ($("#rbExport").is(":checked")) {
        ShipType = "E"
        var custom_inv_no = $('#custom_inv_no').val();
        var CustomInvDate = $('#CustomInvDate').val();

        if (custom_inv_no == "" || custom_inv_no == "0" || custom_inv_no == null) {
            //document.getElementById("Span_custom_inv_no").innerHTML = $("#valueReq").text();
            $("#Span_custom_inv_no").text($("#valueReq").text());
            $("#Span_custom_inv_no").css("display", "block");
            $("#custom_inv_no").css("border-color", "red");
            ValidationFlag = false;
        }
        if (CustomInvDate == "" || CustomInvDate == "0" || CustomInvDate == null) {
            //document.getElementById("Span_CustomInvDate").innerHTML = $("#valueReq").text();
            $("#Span_CustomInvDate").text($("#valueReq").text());
            $("#Span_CustomInvDate").css("display", "block");
            $("#CustomInvDate").css("border-color", "red");
            ValidationFlag = false;
        }
    }
    /***End***/


    if (CustomerName == "" || CustomerName == "0") {
        //$(".select2-container--default .select2-selection--single").css("border-color", "red");
        $("[aria-labelledby='select2-ddlCustomer_Name-container']").css("border-color", "red");
        ValidationFlag = false;
    }
    if (PackingListNumber == "" || PackingListNumber == "0") {
        document.getElementById("vmso_no").innerHTML = $("#valueReq").text();
        $("#ddlPackListNumber").css("border-color", "red");
        $("[aria-labelledby='select2-ddlPackListNumber-container']").css("border-color", "red");
        ValidationFlag = false;
    }
    if (BillingAddress == "" || BillingAddress == "0") {
        document.getElementById("vm_bill_address").innerHTML = $("#valueReq").text();
        $("#TxtBillingAddr").css("border-color", "red");
        ValidationFlag = false;
    }
    if (ShippingAddress == "" || ShippingAddress == "0") {
        document.getElementById("vm_ship_address").innerHTML = $("#valueReq").text();
        $("#TxtShippingAddr").css("border-color", "red");
        ValidationFlag = false;
    }
    var Sh_Status = $('#hdShipmentStatus').val();
    if (Sh_Status == "SH") {
        if (CheckCancelledStatus() == false) {
            return false;
        }
    }
    if (ValidationFlag == false) {
        return false;
    }
    var ShipType = "";
    if ($("#rbExport").is(":checked")) {
        ShipType = "E"
    }
    if ($("#Cancelled").is(":checked")) {
        if ($("#Cancelledremarks").text() == "" || $("#Cancelledremarks").text() == null) {
            $('#SpanCancelledRemarks').text($("#valueReq").text());
            $("#SpanCancelledRemarks").css("display", "block");
            $("#Cancelledremarks").css("border-color", "Red");
            ValidationFlag = false;
            return false;
        }
        else {
            $('#SpanCancelledRemarks').text("");
            $("#SpanCancelledRemarks").css("display", "none");
            $("#Cancelledremarks").css("border-color", "#ced4da");
        }

    }
    if (ValidationFlag == true) {
        if (rowcount > 1) {
            var Batchflag = CheckItemBatchValidation();
            if (Batchflag == false) {
                return false;
            }
            //var SerialFlag = CheckItemSerialValidation();
            //if (SerialFlag == false) {
            //    return false;
            //}
            //var TransportDetail = CheckTransportDetailValidation();
            //if (TransportDetail == false) {
            //return false;
            //}
            debugger;
            var datatable = $(".dataTables_empty").length;
            if (datatable == "1") {
                $("#datatable-buttons2 tbody tr").remove();
            }
            var length = $("#datatable-buttons2 tbody tr").length;
            if (length == 0) {
                swal("", $("#TransporterDetailNotFound").text(), "warning");
                return false;
            }
            var TotalPack = TotalTablePacksQty();
            if (parseFloat(TotalPack) != parseFloat($("#hdnNumberOfPacks").val())) {
                swal("", $("#MismatchWithNumberOfPackages").text(), "warning");
                return false;
            }
            debugger;
            if (ShipType == "E") {
                var Flag = "N";
                var POL = $("#PortOfLoading option:selected").val();
                var PI = $("#PlOfReceiptByPreCarrier option:selected").val();
                if (POL == "0" || POL == "") {
                    $("#Span_loading_port").text($("#valueReq").text());
                    $("#Span_loading_port").css("display", "block");
                    $("[aria-labelledby='select2-PortOfLoading-container']").css("border-color", "red");
                    Flag = "Y";
                }
                if (PI == "0" || PI == "") {
                    $("#Span_pi_rcpt_carr").text($("#valueReq").text());
                    $("#Span_pi_rcpt_carr").css("display", "block");
                    $("[aria-labelledby='select2-PlOfReceiptByPreCarrier-container']").css("border-color", "red");
                    Flag = "Y";
                }
                if (Flag == "Y") {
                    $("#collapseSix").addClass("show");
                    return false;
                }
            }
            //return false;
            if (Batchflag == true /*&& SerialFlag == true*/) {

                var DeliveryNoteItemDetailList = new Array();
                $("#ShipmentItmDetailsTbl TBODY TR").each(function () {
                    var row = $(this);

                    //var Index = row.find("#SNohiddenfiled").val();
                    //var whERRID = "#wh_id" + Index + "  option:selected";
                    var ItemList = {};
                    debugger;
                    ItemList.ItemName = row.find("#ItemName").val();
                    ItemList.ItemId = row.find("#hdItemId").val();
                    ItemList.UOM = row.find("#UOM").val();
                    ItemList.UOMId = row.find('#hdUOMId').val();
                    ItemList.sub_item = row.find('#sub_item').val();
                    ItemList.WareHouseId = row.find("#hdwh_Id").val();
                    ItemList.WareHouseName = row.find("#txtWh_name").val();
                    ItemList.i_batch = row.find("#hdi_batch").val();
                    ItemList.i_serial = row.find("#hdi_serial").val();
                    ItemList.PackedQuantity = row.find('#PackedQuantity').val();
                    ItemList.NumberOfPacks = row.find("#NumberOfPacks").val();
                    ItemList.NetWeight = row.find("#NetWeight").val();
                    ItemList.GrossWeight = row.find("#GrossWeight").val();
                    ItemList.CBM = row.find("#CBM").val();
                    ItemList.ShippedQuantity = row.find("#ShippedQuantity").val();
                    ItemList.AvailableStock = row.find('#AvailableStock').val();
                    ItemList.InvoicedQuantity = row.find('#InvoicedQuantity').val();
                    ItemList.remarks = row.find('#remarks').val();
                    ItemList.PackSize = row.find('#PackSize').val();
                    DeliveryNoteItemDetailList.push(ItemList);

                });
                var str = JSON.stringify(DeliveryNoteItemDetailList);
                $('#hdShipmentItemDetail').val(str);
                BindShipmentItemBatchDetail();
                BindShipmentItemSerialDetail();
                //BindGLVoucherDetail();
                /*-----------Sub-item-------------*/

                var SubItemsListArr = Cmn_SubItemList();
                var str2 = JSON.stringify(SubItemsListArr);
                $('#SubItemDetailsDt').val(str2);

                /*-----------Sub-item end-------------*/
                /*----- Attatchment start--------*/
                $(".fileinput-upload").click();/*To Upload Img in folder*/
                FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
                /*----- Attatchment End--------*/
                /*----- Attatchment start--------*/
                var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
                $('#hdn_Attatchment_details').val(ItemAttchmentDt);
                /*----- Attatchment End--------*/
                var CustID = $("#ddlCustomer_Name option:selected").val();
                $("#Hdn_ShipCustID").val(CustID);
                $("#ddlCustomer_Name").val(CustID);
                var CustName = $("#ddlCustomer_Name option:selected").text();
                $("#Hdn_ShipCustName").val(CustName);

                var FinalTransPoterDetail = [];
                FinalTransPoterDetail = InsertTransPoterDetails();
                var TransDtl = JSON.stringify(FinalTransPoterDetail);
                $('#hdTranspoterDetail').val(TransDtl);
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
                return true;
            }
            else {
                return false;
            }
        }
        else {
            swal("", $("#noitemselectedmsg").text(), "warning");
            return false;
        }
    }
    else {
        return false;
    }
}
function OnChangeCustomInvDate() {
    $("#Span_CustomInvDate").css("display", "none");
    $("#CustomInvDate").css("border-color", "#ced4da");
}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
   
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145120") {
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var QtyDecDigit = $("#QtyDigit").text();
    }
    $("#ShipmentItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#ShippedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Batchable = "Y";//clickedrow.find("#hdi_batch").val();//Commented by Suraj on 13-09-2024

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                clickedrow.find("#btnbatchdeatil").css("border-color", "red");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#shipmentlineBatchIssueQty').val();
                    var bchitemid = currentRow.find('#shipmentlineBatchItemId').val();
                    var bchuomid = currentRow.find('#shipmentlineBatchUOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });
                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    clickedrow.find("#btnbatchdeatil").css("border-color", "#007bff");
                }
                else {
                    clickedrow.find("#btnbatchdeatil").css("border-color", "red");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterBatchDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#BatchQtydoesnotmatchwithShippedQty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckItemSerialValidation() {
    debugger;
    var ErrorFlag = "N";
    var SerialableFlag = "N";
    var EmptyFlag = "";
    
    var DocumentMenuId = $("#DocumentMenuId").val();
    if (DocumentMenuId == "105103145120") {
        var QtyDecDigit = $("#ExpImpQtyDigit").text();
    }
    else {
        var QtyDecDigit = $("#QtyDigit").text();
    }
    $("#ShipmentItmDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#ShippedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                clickedrow.find("#btnserialdeatil").css("border-color", "red");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#shipmentlineSerialIssueQty').val();
                    var srialitemid = currentRow.find('#shipmentlineSerialItemId').val();
                    var srialuomid = currentRow.find('#shipmentlineSerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    clickedrow.find("#btnserialdeatil").css("border-color", "#007bff");
                }
                else {
                    clickedrow.find("#btnserialdeatil").css("border-color", "red");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#PleaseenterSerialDetails").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#SerializedQtydoesnotmatchwithShippedQty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }

    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckTransportDetailValidation() {
    debugger;
    var ErrorFlag = "N";
    var qtydigit = $("#QtyDigit").text();
    var gr_no = $("#gr_no").val();
    var GRDate = $("#GRDate").val();
    var txttrpt_name = $("#txttrpt_name").val();
    var txtveh_number = $("#txtveh_number").val();
    var NoOfPacks = $("#NoOfPacks").val();
    var DocMenuId = $("#DocumentMenuId").val();

    if (gr_no == "" || gr_no == "0") {
        $("#Span_GRNumber").text($("#valueReq").text());
        $("#Span_GRNumber").css("display", "block");
        $("#gr_no").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (GRDate == "" || GRDate == "0") {
        $("#Span_GRDate").text($("#valueReq").text());
        $("#Span_GRDate").css("display", "block");
        $("#GRDate").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (NoOfPacks == "" || NoOfPacks == "0" || NoOfPacks == "0.000" ||
        parseFloat(NoOfPacks).toFixed(qtydigit) == parseFloat(0).toFixed(qtydigit)) {
        $("#Span_No_Of_Packages").text($("#valueReq").text());
        $("#Span_No_Of_Packages").css("display", "block");
        $("#NoOfPacks").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (txttrpt_name == "" || txttrpt_name == "0") {
        $("#Span_TransporterName").text($("#valueReq").text());
        $("#Span_TransporterName").css("display", "block");
        //$("#txttrpt_name").css("border-color", "red");
        $("[aria-labelledby='select2-txttrpt_name-container']").css("border-color", "red");
        ErrorFlag = "Y";
    }
    if (DocMenuId != "105103135") {//Added by Suraj Maurya on 15-04-2025 in case for Eway-bill part b optional
        if (txtveh_number == "" || txtveh_number == "0") {
            $("#Span_VehicleNumber").text($("#valueReq").text());
            $("#Span_VehicleNumber").css("display", "block");
            $("#txtveh_number").css("border-color", "red");
            ErrorFlag = "Y";
        }
    }
    
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnChangeGRNumber() {
    $("#Span_GRNumber").css("display", "none");
    $("#gr_no").css("border-color", "#ced4da");
}
function OnChangeGRDate() {
    $("#Span_GRDate").css("display", "none");
    $("#GRDate").css("border-color", "#ced4da");
}
function OnChangeTransporterName() {
    $("#Span_TransporterName").css("display", "none");
    //$("#txttrpt_name").css("border-color", "#ced4da");
    $("[aria-labelledby='select2-txttrpt_name-container']").css("border-color", "#ced4da");
}
function OnChangeVehicleNumber() {
    $("#Span_VehicleNumber").css("display", "none");
    $("#txtveh_number").css("border-color", "#ced4da");
}
function UpdateTransportConfirm(event) {
    var status = $("#hdShipmentStatus").val();
    if (status == "SH" || status == "IN") {
        swal({
            title: $("#UpdateTransportDetail").text() + "?",
            //text: $("#deltext").text() + "!",
            type: "warning",
            showCancelButton: true,
            //confirmButtonText: "No",
            confirmButtonClass: "btn-success",
            cancelButtonClass: "btn-danger",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                $("#HdEditCommand").val("UpdateTransPortDetail");
                $('form').submit();

                return true;
            } else {
                $("#HdDeleteCommand").val("Edit");
                $('form').submit();
                return true;
                //return false;
            }
        });
        return false;
    }
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();

            return true;
        } else {
            return false;
        }
    });
    return false;
}
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        BindShipmentItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ShippedQty = clickedrow.find("#ShippedQuantity").val();
        var SelectedItemdetail = $("#HDSelectedBatchwise").val();
        var docid = $("#DocumentMenuId").val();
        var CMD = $("#CMN_Command").val();
        var typ = $("#hdn_TransType").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var DocumentMenuId = $("#DocumentMenuId").val();
        if (DocumentMenuId == "105103145120") {
            QtyDecDigit = $("#ExpImpQtyDigit").text();
        }
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/Shipment/getItemStockBatchWise",
            data: {
                ItemId: ItemId,
                SelectedItemdetail: SelectedItemdetail,
                docid: docid,
                CMD: CMD,
                typ: typ

            },
            success: function (data) {
                debugger;
                $('#ItemStockBatchWise').html(data);

                $("#ItemNameBatchWise").val(ItemName);
                $("#UOMBatchWise").val(UOMName);
                $("#QuantityBatchWise").val(ShippedQty);
                $("#HDItemNameBatchWise").val(ItemId);
                $("#HDUOMBatchWise").val(UOMId);

                var TotalIssuedBatch = parseFloat(0).toFixed(QtyDecDigit);

                if ($("#SaveItemBatchTbl TBODY TR").length > 0) {
                    $("#SaveItemBatchTbl TBODY TR").each(function () {
                        var row = $(this)
                        var BtItemId = row.find("#shipmentlineBatchItemId").val();
                        if (BtItemId === ItemId) {
                            TotalIssuedBatch = parseFloat(TotalIssuedBatch) + parseFloat(row.find("#shipmentlineBatchIssueQty").val());
                        }
                    });
                }

                $("#BatchWiseItemStockTbl TBODY TR").each(function () {
                    var row = $(this)
                    var issueQty = row.find("#IssuedQuantity").val();
                    var AvlblQty = row.find("#AvailableQuantity").val();
                    if (issueQty != null && issueQty != "") {
                        row.find("#IssuedQuantity").val(parseFloat(issueQty).toFixed(QtyDecDigit))
                        row.find("#AvailableQuantity").val(parseFloat(AvlblQty).toFixed(QtyDecDigit))
                    }
                });

                $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssuedBatch).toFixed(QtyDecDigit));

            },
        });
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;
        BindShipmentItemSerialDetail();
        var clickedrow = $(evt.target).closest("tr");
        //var Index = clickedrow.find("#SNohiddenfiled").val();
        //var ddlId = "#wh_id" + Index;

        // var WarehouseId = clickedrow.find(ddlId).val();
        //var CompId = $("#HdCompId").val();
        //var BranchId = $("#HdBranchId").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#hdItemId").val();;
        var UOMID = clickedrow.find("#hdUOMId").val();
        var ShippedQty = clickedrow.find("#ShippedQuantity").val();
        var docid = $("#DocumentMenuId").val();
        var SelectedItemSerial = $("#HDSelectedSerialwise").val();
        var CMD = $("#CMN_Command").val();
        var typ = $("#hdn_TransType").val();
        //$("#ItemNameSerialWise").val(ItemName);
        //$("#UOMSerialWise").val(UOMName);
        //$("#ShippedQuantitySerialWise").val(ShippedQty);

        //$("#HDItemIDSerialWise").val(ItemId);
        //$("#HDUOMIDSerialWise").val(UOMID);
        //$("#TotalIssuedSerial").val(""); 
        //var Sh_Status = $("#hdShipmentStatus").val();
        //if (Sh_Status == "" || Sh_Status == null) {
        $.ajax({
            type: "Post",
            url: "/ApplicationLayer/Shipment/getItemstockSerialWise",
            data: {
                ItemId: ItemId,
                SelectedItemSerial: SelectedItemSerial,
                docid: docid,
                CMD: CMD,
                typ: typ

            },
            success: function (data) {
                debugger;
                $('#ItemStockSerialWise').html(data);

                $("#ItemNameSerialWise").val(ItemName);
                $("#UOMSerialWise").val(UOMName);
                $("#QuantitySerialWise").val(ShippedQty);

                $("#HDItemIDSerialWise").val(ItemId);
                $("#HDUOMIDSerialWise").val(UOMID);

                var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                    $("#SaveItemSerialTbl TBODY TR").each(function () {
                        var row = $(this)
                        var ItemId = row.find("#shipmentlineSerialItemId").val();
                        if (ItemId === ItemId) {
                            TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#shipmentlineSerialIssueQty").val());
                        }
                    });
                }
                $("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
            },
        });
        //}
        //if (Sh_Status == "C" || Sh_Status == "SH") {
        //    var Sh_Type = "D";
        //    var Sh_No = $("#ShipmentNumber").val();
        //    var Sh_Date = $("#txtship_dt").val();

        //    $.ajax({
        //        type: "Post",
        //        url: "/ApplicationLayer/Shipment/getItemstockSerialWiseAfterInsert",
        //        data: {
        //            Sh_Type: Sh_Type,
        //            Sh_No: Sh_No,
        //            Sh_Date: Sh_Date,
        //            ItemId: ItemId
        //        },
        //        success: function (data) {
        //            debugger;
        //            $('#ItemStockSerialWise').html(data);
        //        },
        //    });
        //}

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function BindShipmentItemBatchDetail() {
    //var QtyDigit = "";
    var DocumentMenuId = $("#DocumentMenuId").val();
    //if (DocumentMenuId == "105103145120") {
    //    QtyDigit = $("#ExpImpQtyDigit").text();
    //}
    //else {
    //    QtyDigit = $("#QtyDigit").text();
    //}
    var batchrowcount = $('#SaveItemBatchTbl tbody tr').length;
    if (batchrowcount > 0) {
        var ItemBatchList = new Array();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this)
            var batchList = {};
            debugger;
            batchList.LotNo = row.find('#shipmentlineBatchLotNo').val();
            batchList.ItemId = row.find('#shipmentlineBatchItemId').val();
            batchList.UOMId = row.find('#shipmentlineBatchUOMId').val();
            batchList.BatchNo = row.find('#shipmentlineBatchBatchNo').val();
            batchList.BatchAvlStock = row.find('#shipmentlineBatchBatchAvlStk').val();
            //var Qty = row.find('#shipmentlineBatchIssueQty').val();
            batchList.IssueQty = row.find('#shipmentlineBatchIssueQty').val();

            var ExDate = row.find('#shipmentlineBatchExpiryDate').val().trim();
            var FDate = "";
            if (ExDate == "" || ExDate == null) {
                FDate = "";
            }
            else {
                //var date = ExDate.split("-");
                //FDate = date[2] + '-' + date[1] + '-' + date[0];
                FDate = ExDate;
            }
            batchList.ExpiryDate = FDate;
            batchList.mfg_name = IsNull(row.find('#shipmentlineBatchMfgName').val(),"");
            batchList.mfg_mrp = IsNull(row.find('#shipmentlineBatchMfgMrp').val(),"");
            batchList.mfg_date = IsNull(row.find('#shipmentlineBatchMfgDate').val(),"");
            ItemBatchList.push(batchList);
            debugger;
        });
        var str1 = JSON.stringify(ItemBatchList);
        $("#HDSelectedBatchwise").val(str1);
    }

}
function BindShipmentItemSerialDetail() {
    var serialrowcount = $('#SaveItemSerialTbl tbody tr').length;
    if (serialrowcount > 0) {
        var ItemSerialList = new Array();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this)
            var SerialList = {};
            SerialList.ItemId = row.find("#shipmentlineSerialItemId").val();
            SerialList.UOMId = row.find("#shipmentlineSerialUOMId").val();
            SerialList.LOTId = row.find("#shipmentlineSerialLOTNo").val();
            SerialList.IssuedQuantity = row.find("#shipmentlineSerialIssueQty").val();
            SerialList.SerialNO = row.find("#shipmentlineBatchSerialNO").val();
            SerialList.mfg_name = IsNull(row.find("#shipmentlineBatchMfgName").val(),"");
            SerialList.mfg_mrp = IsNull(row.find("#shipmentlineBatchMfgMrp").val(),"");
            SerialList.mfg_date = IsNull(row.find("#shipmentlineBatchMfgDate").val(),"");
            ItemSerialList.push(SerialList);
            debugger;
        });
        var str2 = JSON.stringify(ItemSerialList);
        $("#HDSelectedSerialwise").val(str2);
    }
}

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();

    if (ItmCode != "" && ItmCode != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/LSODetail/GetSOItemDetail",
                    data: { ItemID: ItmCode },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
                                $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
                                $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
                                $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
                                var ImgFlag = "N";
                                for (var i = 0; i < arr.Table.length; i++) {
                                    if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
                                        ImgFlag = "Y";
                                    }
                                }
                                if (ImgFlag == "Y") {
                                    var OL = '<ol class="carousel-indicators">';
                                    var Div = '<div class="carousel-inner">';
                                    for (var i = 0; i < arr.Table.length; i++) {
                                        var ImgName = arr.Table[i].item_img_name;
                                        var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
                                        if (i === 0) {
                                            OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
                                            Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
                                        }
                                        else {
                                            OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
                                            Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
                                        }
                                    }
                                    OL += '</ol>'
                                    Div += '</div>'

                                    var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
                                    var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

                                    $("#myCarousel").html(OL + Div + Ach + Ach1);
                                }
                                else {
                                    $("#myCarousel").html("");
                                }
                            }
                        }
                    },
                });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }
}
function CheckedCancelled() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return CheckFormValidation();");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
        CancelledRemarks("#Cancelled", "Enable");
        return true;
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
        CancelledRemarks("#Cancelled", "Enable");
        return false;
    }
   
}
function CheckCancelledStatus() {
    debugger;
    if ($("#Cancelled").is(":checked")) {
        return true;
    }
    else {
        //***Modifyed By Shubham Maurya on 15-12-2023 For Edit Time Update Transport Details***//
        if ($("#HdEditCommand").val() != "UpdateTransPortDetail") {
            return false;
        }
        else {
            return true;
        }
    }
}
function OnClickBuyerInfoIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var ItmCode = "";
    var ItmName = "";

    $("#ItemCode").val("");
    $("#ItemDescription").val("");
    $("#PackingDetail").val("");
    $("#detailremarks").val("");


    ItmCode = clickedrow.find("#hdItemId").val();
    //ItmName = clickedrow.find("#SQItemListName" + Sno + " option:selected").text()
    var Cust_id = $('#ddlCustomer_Name').val();

    if (ItmCode != "" && ItmCode != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/DomesticSalesQuotation/GetItemCustomerInfo",
                    data: { ItemID: ItmCode, CustID: Cust_id },
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            LSO_ErrorPage();
                            return false;
                        }
                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                $("#ItemCode").val(arr.Table[0].Item_code);
                                $("#ItemDescription").val(arr.Table[0].item_des);
                                $("#PackingDetail").val(arr.Table[0].pack_dt);
                                $("#detailremarks").val(arr.Table[0].remark);

                            }
                        }
                    },
                });
        } catch (err) {
            console.log("GetMenuData Error : " + err.message);
        }
    }
}
/***--------------------------------Sub Item Section-----------------------------------------***/
function HideShowPageWise(sub_item, clickedrow) {
    debugger;
    Cmn_SubItemHideShow(sub_item, clickedrow, "sub_item", "SubItemOrderQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlQty");
    Cmn_SubItemHideShow(sub_item, clickedrow, "", "SubItemAvlStk");
}
function SubItemDetailsPopUp(flag, e) {
    debugger;
    var DocumentMenuId = $("#DocumentMenuId").val();
    var ship_type = '';
    var shiptyp = $("#rbDomestic").val();
    var shiptype = $("#rbExport").val();
    if (shiptyp || shiptype) {
        if (shiptyp == 'D') {
            ship_type = 'D'
        }
        else {
            ship_type = 'E'
        }
    }
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var UOM = clickdRow.find("#UOM").val();
    var SrcDoc_no = $("#ddlPackListNumber").val();
    var SrcDoc_dt = $("#txpack_dt").val();
    var Doc_no = $("#ShipmentNumber").val();
    var Doc_dt = $("#txtship_dt").val();

    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "Quantity") {
        $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.item_id = row.find("#ItemId").val();
            List.sub_item_id = row.find('#subItemId').val();
            List.qty = row.find('#subItemQty').val();
            NewArr.push(List);
        });
        Sub_Quantity = clickdRow.find("#QuotQty").val();
    }
    else if (flag == "ShipPacked") {
        Sub_Quantity = clickdRow.find("#PackedQuantity").val();
    }
    else if (flag == "Shipped") {
        Sub_Quantity = clickdRow.find("#ShippedQuantity").val();
    }
    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdShipmentStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/Shipment/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            SrcDoc_no: SrcDoc_no,
            SrcDoc_dt: SrcDoc_dt,
            Doc_no: Doc_no,
            Doc_dt: Doc_dt,
            DocumentMenuId: DocumentMenuId
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
    return Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "ord_qty_spec", "SubItemOrderQty", "Y");
}
function ResetWorningBorderColor() {
    debugger;
    return Cmn_CheckValidations_forSubItems("SOItmDetailsTbl", "SNohiddenfiled", "SOItemListName", "ord_qty_spec", "SubItemOrderQty", "N");
}
function GetSubItemAvlStock(e) {
    var Crow = $(e.target).closest('tr');
    // var rowNo = Crow.find("#SNohiddenfiled").val();
    var ProductNm = Crow.find("#ItemName").val();
    var ProductId = Crow.find("#hdItemId").val();
    var UOM = Crow.find("#UOM").val();
    var AvlStk = Crow.find("#AvailableStock").val();
    var hdwh_Id = Crow.find("#hdwh_Id").val();
    Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, hdwh_Id, AvlStk, "wh");
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
//function GetAllGLID() {
//    debugger;
//    var Compid = $("#CompID").text();
//    var DocType = "D";
//    //var TransType = '';
//    var GLDetail = [];
//    $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {

//        var currentRow = $(this);
//        var id = currentRow.find("#id").text();
//        var Amt = currentRow.find("#amt").text();
//        var type = currentRow.find("#type").text();

//        GLDetail.push({
//            comp_id: Compid, id: id, type: type, doctype: DocType, Value: Amt, DrAmt: 0, CrAmt: 0, TransType: 'ShipCogs' });
//    });
//    $("#hd_GL_DeatilTbl >tbody >tr").each(function (i, row) {

//        var currentRow = $(this);
//        var id = currentRow.find("#id").text();
//        var Amt = currentRow.find("#amt").text();
//        var type = currentRow.find("#type").text();

//        GLDetail.push({ comp_id: Compid, id: id, type: type, doctype: DocType, Value: Amt, DrAmt: 0, CrAmt: 0, TransType: 'ShipStk'  });
//    });
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/Shipment/GetGLDetails",
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
//                if (arr.Table.length > 0) {
//                    $('#VoucherDetail tbody tr').remove();
//                    var rowIdx = $('#VoucherDetail tbody tr').length;
//                    for (var j = 0; j < arr.Table.length; j++) {

//                        if (arr.Table[j].acc_id != "" && arr.Table[j].acc_id != "0" && arr.Table[j].acc_id != null) {
//                            if (arr.Table[j].type == "ShipCogs") {
//                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>  
//                                                    <td>&nbsp;</td>

//                            </tr>`);
//                            }
//                            else {
//                                $('#VoucherDetail tbody').append(`<tr id="R${++rowIdx}">
//                                    <td class="sr_padding">${rowIdx}</td>
//                                    <td>${arr.Table[j].acc_name} <input  type="hidden" id="hfAccID" value="${arr.Table[j].acc_id}" /></td>                                    
//                                    <td id="dramt" class="num_right">${parseFloat(0).toFixed(ValDecDigit)}</td>
//                                    <td id="cramt" class="num_right">${parseFloat(arr.Table[j].Value).toFixed(ValDecDigit)}</td>  
//                                                    <td>&nbsp;</td>

//                            </tr>`);

//                            }

//                        //$("#VoucherDetail >tbody >tr:first").find("#dramt").text(parseFloat(TotInvVal).toFixed(ValDecDigit));

//                        }
//                    }

//                    CalculateVoucherTotalAmount();

//                    var errors = [];
//                    var step = [];

//                    for (var i = 0; i < arr.Table1.length; i++) {
//                        debugger;
//                        if (arr.Table1[i].acc_id == "" || arr.Table1[i].acc_id == "0" || arr.Table1[i].acc_id == null || arr.Table1[i].acc_id == 0) {
//                            errors.push($("#GLAccountisnotLinked").text() + ' ' + arr.Table1[i].EntityName)
//                            step.push(parseInt(i));
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

//            }

//        }
//    });
//}

//function CalculateVoucherTotalAmount() {
//    debugger;
//    var ValDecDigit = $("#ValDigit").text();
//    var DrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    var CrTotAmt = parseFloat(0).toFixed(ValDecDigit);
//    $("#VoucherDetail >tbody >tr").each(function () {
//        debugger;
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
//    $("#DrTotal").text(DrTotAmt);
//    $("#CrTotal").text(CrTotAmt);
//    if (parseFloat(DrTotAmt) != parseFloat(CrTotAmt)) {
//        AddRoundOffGL(DrTotAmt, CrTotAmt);
//    }
//}

/*--------------------------Print Type Start-------------------------*/
function OnClickPrintBtn() {
    let PrintOptions = $("#span_Hdn_ExportCommercialPrintCustomOptions").text();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/Shipment/GetSelectPrintTypePopup",
        data: { PrintOptions: PrintOptions },
        success: function (data) {
            debugger;
            $("#PopupSelectPrintType").html(data);
            SelectPrintType();
        }
    })
    return false;
}
function OnClickPrint() {
    let Selected = "N";
    let PrintType = "";
    $("#Table_SelectPrintType tbody tr").each(function () {
        var Row = $(this);
        if (Row.find("#Rdo_PackType").prop("checked")) {
            Selected = "Y";
            PrintType = Row.find("#Rdo_PackType").val();
        }
    })
    if (Selected == "Y") {
        let ErFlg = "N";
        if ($("#ChkPrtOpt_catlog_number").is(":checked")) {
            ErFlg = "Y";
            $("#PrtOpt_catlog_number").val("Y");
        } else {
            $("#PrtOpt_catlog_number").val("N");
        }

        if ($("#ChkPrtOpt_item_code").is(":checked")) {
            ErFlg = "Y";
            $("#PrtOpt_item_code").val("Y");
        } else {
            $("#PrtOpt_item_code").val("N");
        }

        if ($("#ChkPrtOpt_item_desc").is(":checked")) {
            ErFlg = "Y";
            $("#PrtOpt_item_desc").val("Y");
        } else {
            $("#PrtOpt_item_desc").val("N");
        }
        if (PrintType == "Commercial") {
            if (ErFlg == "N") {
                swal("", $("#span_PleaseselectAtleastOnePrintOption").text(), "warning");
                return false;
            }
        }
        $("#PrintButtonDiv").html('<button hidden type="submit" id="HdnButtonToPrint" name="Command" hidden value="Print" >Print</button>')
        $("#OKBtn_PrintSelect").attr("onclick", "");
        $("#OKBtn_PrintSelect").attr("data-dismiss", "modal");
        //$("form").submit();
        $("#HdnButtonToPrint").click();
    } else {
        swal("", $("#span_PleaseSelectPrintType").text(), "warning");
    }


}

function SelectPrintType() {
    $("#Table_SelectPrintType tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");

        if (clickedrow.find('#Rdo_PackType').prop("disabled") == false) {
            $("#Table_SelectPrintType >tbody >tr").css("background-color", "#ffffff");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
            clickedrow.find('#Rdo_PackType').prop("checked", true);
            if (clickedrow.find('#Rdo_PackType').val() == "Commercial") {
                $("#Div_CustomPrintOptions").css("display", "block");
            }
            else {
                $("#Div_CustomPrintOptions").css("display", "none");
            }
        }
    });
}
/*--------------------------Print Type End-------------------------*/
/*----------------- export items list to excel ------------------*/
function ExportItemsToExcel() {
    debugger;
    var shipNo = $('#hdship_no').val();
    var shipDate = $('#hdship_dt').val();
    if (shipNo != null && shipNo != "" && shipNo != undefined) {
        window.location.href = "/ApplicationLayer/Shipment/ExportItemsToExcel?shipmentNo=" + shipNo + "&shipmentDate=" + shipDate;
    }
}
/*----------------- export items list to excel END------------------*/

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

function OnCheckedChangeProdDesc() {
    if ($('#chkproddesc').prop('checked')) {
        $('#chkshowcustspecproddesc').prop('checked', false);
        $('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $("#ShowProdDesc").val("Y");
        $('#ShowCustSpecProdDesc').val('N');
        $('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $("#ShowProdDesc").val("N");
    }
}
function onCheckedChangeCustSpecProdDesc() {
    if ($('#chkshowcustspecproddesc').prop('checked')) {
        $('#chkproddesc').prop('checked', false);
        $('#chkitmaliasname').prop('checked', false);/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
        $('#ShowCustSpecProdDesc').val('Y');
        $('#ShowProdDesc').val('N');
        $('#ItemAliasName').val('N');/*Add by hina sharma on 13-11-2024 after discuss with vishal sir*/
    }
    else {
        $('#ShowCustSpecProdDesc').val('N');
    }
}
function OnCheckedChangeItemAliasName() {/*Add by Hina sharma on 13-11-2024 */
    debugger;
    if ($('#chkitmaliasname').prop('checked')) {
        $('#chkproddesc').prop('checked', false);
        $('#chkshowcustspecproddesc').prop('checked', false);
        $('#ItemAliasName').val('Y');
        $("#ShowProdDesc").val('N');
        $('#ShowCustSpecProdDesc').val('N');
    }
    else {
        $('#ItemAliasName').val('N');
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
function OnCheckedChangeShowSubItem() {
    if ($('#chkshowsubitm').prop('checked')) {
        $('#ShowSubItem').val('Y');
    }
    else {
        $('#ShowSubItem').val('N');
    }
}
function onCheckedChangeFormatBtn() {
    if ($("#OrderTypeImport").is(":checked")) {
        $('#PrintFormat').val('F2');
    }
    else {
        $('#PrintFormat').val('F1');
    }
}
function OnCheckedChangeCustAliasName() {
    debugger;
    if ($('#chkcustaliasname').prop('checked')) {
        $('#CustomerAliasName').val('Y');
    }
    else {
        $('#CustomerAliasName').val('N');
    }
}
function OnCheckedChangePrintRemarks() {
    if ($('#chkprintremarks').prop('checked')) {
        $('#ShowItemRemarks').val('Y');
    }
    else {
        $('#ShowItemRemarks').val('N');
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
function OnChangePlaceOfSupply() {
    var pos = $('#txtplaceofsupply').val();
    $('#txtremarks').val(pos);
}
function FilterItemDetail(e) {//added by Prakash Kumar on 17-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "ShipmentItmDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}
function CustInvoiceNoKeyDown(event) {
    debugger;
    const key = event.key;

    // Allow letters, digits, hyphen (-), slash (/), backspace, tab, left/right arrows
    const allowedKeys = /^[a-zA-Z0-9\-\/]$/;

    // Allow control keys like backspace, tab, delete, arrows
    const controlKeys = ['Backspace', 'Tab', 'ArrowLeft', 'ArrowRight', 'Delete','Enter'];

    if (!allowedKeys.test(key) && !controlKeys.includes(key)) {
        // Show validation error
        $("#custom_inv_no").css("border-color", "red");
        $("#Span_custom_inv_no").text($("#span_InvalidNumber").text() + ' (' + "A-Z,/,-,0-9" + ')');
        $("#Span_custom_inv_no").css("font-weight", "bold");
        $("#Span_custom_inv_no").css("display", "block");
        event.preventDefault();
        return false;
    }

    // Hide validation if key is allowed
    $("#custom_inv_no").css("border-color", "");
    $("#Span_custom_inv_no").css("display", "none");
}
function onpasteValueDiscard() {
    $('#custom_inv_no').on('paste', function (e) {
        debugger;
        let pastedData = e.originalEvent.clipboardData.getData('text');

        // Allow only letters, digits, hyphen, and slash
        const allowedPattern = /^[a-zA-Z0-9\/\-]*$/;

        if (!allowedPattern.test(pastedData)) {
            e.preventDefault(); // Prevent paste
            $("#custom_inv_no").css("border-color", "red");
            $("#Span_custom_inv_no").text($("#span_InvalidNumber").text()+' ('+ "A-Z,/,-,0-9" +')');
            $("#Span_custom_inv_no").css("font-weight", "bold");
            $("#Span_custom_inv_no").css("display", "block");
        } else {
            // Optional: clear warning if pasted content is valid
            $("#custom_inv_no").css("border-color", "");
            $("#Span_custom_inv_no").css("display", "none");
        }
    });
}
// Added by Nidhi on 20-08-2025
function SendEmail() {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var Cust_id = $('#ddlCustomer_Name').val();
    Cmn_SendEmail(docid, Cust_id, 'Cust');
}
function SendEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hdShipmentStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#ShipmentNumber").val();
    var Doc_dt = $("#txtship_dt").val();
    var statusAM = $("#Amendment").val();
    var filepath = $('#hdfilepathpdf').val();
    Cmn_SendEmailAlert(mail_id, status, docid, Doc_no, Doc_dt, statusAM, "/ApplicationLayer/Shipment/SendEmailAlert", filepath)
}
function ViewEmailAlert() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hdShipmentStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#ShipmentNumber").val();
    var Doc_dt = $("#txtship_dt").val();
    var filepath = $('#hdfilepathpdf').val();
    if (status == 'SH' && docid == '105103135') {
        var PrintFormatArray = [];
        var printFormat = {
            PrintFormat: $('#PrintFormat').val(),
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            ItemAliasName: $("#ItemAliasName").val(),
            CustomerAliasName: $("#CustomerAliasName").val(),
            ShowItemRemarks: $("#ShowItemRemarks").val(),
            ShowPackSize: $("#hdn_ShowPackSize").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SHP__' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        if (filepath == "" || filepath == null) {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/Shipment/SavePdfDocToSendOnEmailAlert_Ext",
                data: { poNo: Doc_no, poDate: Doc_dt, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
                /*dataType: "json",*/
                success: function (data) {
                    filepath = data;
                    $('#hdfilepathpdf').val(filepath)
                    Cmn_ViewEmailAlert(mail_id, 'A', docid, Doc_no, Doc_dt, filepath);
                }
            });
        }
        else {
            Cmn_ViewEmailAlert(mail_id, 'A', docid, Doc_no, Doc_dt, filepath);
        }
    }
    else {
        Cmn_ViewEmailAlert(mail_id, 'A', docid, Doc_no, Doc_dt);
    }
}
//Added by Nidhi on 05-08-2025
function EmailAlertLogDetails() {
    var Doc_dt = $("#txtship_dt").val();
    var status = $('#hdShipmentStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#ShipmentNumber").val();
    Cmn_EmailAlertLogDetails(status, docid, Doc_no, Doc_dt)
}
//Added by Nidhi on 06-08-2025
function PrintFormate() {
    debugger;
    var mail_id = $("#Email").val().trim();
    var status = $('#hdShipmentStatus').val();
    var docid = $("#DocumentMenuId").val();
    var Doc_no = $("#ShipmentNumber").val();
    var Doc_dt = $("#txtship_dt").val();
    if (status == 'SH') {
        var PrintFormatArray = [];
        var printFormat = {
            PrintFormat: $('#PrintFormat').val(),
            ShowProdDesc: $("#ShowProdDesc").val(),
            ShowCustSpecProdDesc: $("#ShowCustSpecProdDesc").val(),
            ShowProdTechDesc: $("#ShowProdTechDesc").val(),
            ShowSubItem: $("#ShowSubItem").val(),
            ItemAliasName: $("#ItemAliasName").val(),
            CustomerAliasName: $("#CustomerAliasName").val(),
            ShowItemRemarks: $("#ShowItemRemarks").val(),
            ShowPackSize: $("#hdn_ShowPackSize").val(),
        };
        PrintFormatArray.push(printFormat);
        var pdfAlertEmailFilePath = 'SHP__' + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/Shipment/SavePdfDocToSendOnEmailAlert_Ext",
            data: { shpNo: Doc_no, shpDt: Doc_dt, fileName: pdfAlertEmailFilePath, PrintFormat: JSON.stringify(PrintFormatArray) },
            /*dataType: "json",*/
            success: function (data) {
                var filepath = data;
                $('#hdfilepathpdf').val(filepath)
            }
        });
    }
}
//END 20-08-2025