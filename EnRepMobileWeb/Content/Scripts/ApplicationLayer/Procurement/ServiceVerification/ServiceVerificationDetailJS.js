/************************************************
/************************************************
Javascript Name:Service Verification Detail
Created By:Mukesh
Created Date: 01-03-2023
Description: This Javascript use for the Service Verification many function

Modified By:
Modified Date: 
Description:

*************************************************/

$(document).ready(function () {
    BindSuppList();
   /* PQSelectAddress();*/
    // jQuery button click event to remove a row. 
    $('#ServicePOItemTbl tbody').on('click', '.deleteIcon', function () {       
        var child = $(this).closest('tr').nextAll();       
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
        $(this).closest('tr').remove();
        var SNo = $(this).closest('tr').find("#SNohiddenfiled").val();
        var ItemCode = "";
        ItemCode = $(this).closest('tr').find("#ItemListName" + SNo).val();      
      
        SerialNoAfterDelete();
        
    });

    SrVer_No = $("#SrVerNumber").val();
    $("#hdDoc_No").val(SrVer_No);
});

function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    $("#ServicePOItemTbl >tbody >tr").each(function (i, row) {
        //debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#srno").text(SerialNo);
    });
};

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
    InsertSPOApproveDetails("", "", "");
}
function DeleteBtnClick() {
    //RemoveSession();
    Delete_PoDetails();
    ResetWF_Level();

}
function ForwardBtnClick() {
    debugger;
    //var OrderStatus = "";
    //OrderStatus = $('#SrVerStatus').val();
    //if (OrderStatus === "D" || OrderStatus === "F") {

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

    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
    try {
        var compId = $("#CompID").text();
        var brId = $("#BrId").text();
        $.ajax({
            type: "POST",
            url: "/Common/Common/CheckFinancialYear",
            data: {
                compId: compId,
                brId: brId
            },
            success: function (data) {
                if (data == "Exist") { /*End to chk Financial year exist or not*/
                    var OrderStatus = "";
                    OrderStatus = $('#SrVerStatus').val();
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
                else {/* to chk Financial year exist or not*/
                    swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");
                    $("#Btn_Forward").attr("data-target", "");
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
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#SrVerNumber").val();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}
 function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var DocNo = "";
    var DocDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";

    Remarks = $("#fw_remarks").val();
    DocNo = $("#SrVerNumber").val();
    DocDate = $("#SrVer_date").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (DocNo + ',' + DocDate + ',' + WF_status1)
    var ListFilterData1 = $('#ListFilterData1').val();
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
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            //var Action_name = "DPODetail,DPO";
            // location.href = "/Common/Common/InsertForwardDetailsNew/?docno=" + DocNo + "&docdate=" + DocDate + "&doc_id=" + docid + "&level=" + level + "&forwardedto=" + forwardedto + "&fstatus=" + fwchkval + "&remarks=" + Remarks + "&action_name=" + Action_name;
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ServiceVerification/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Approve") {

        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: fwchkval, A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/ServiceVerification/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&ListFilterData1=" + ListFilterData1 + "&WF_status1=" + WF_status1;


    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ServiceVerification/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
    if (fwchkval === "Revert") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            Cmn_InsertDocument_ForwardedDetail(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks);
            window.location.href = "/ApplicationLayer/ServiceVerification/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&TrancType=" + TrancType;
        }
    }
}
function BindSuppList() {
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
function OnChangeSuppName(SuppID) {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var Supp_id = SuppID.value;
    $("#vmSrcDocNo").css("display", "none");
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
        $("#ServicePOItemTbl .plus_icon1").css("display", "none");
        $("#Address").val("");
        $("#ddlCurrency").html("");
        $("#conv_rate").val("");
        $("#conv_rate").prop("readonly", true);
    }
    else {
        var SuppName = $("#SupplierName option:selected").text();
        $("#Hdn_SRPOSuppName").val(SuppName)

        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")

        $("#ServicePOItemTbl >tbody >tr").each(function () {
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            currentRow.find("#ItemListName" + Sno).attr("disabled", false);
            currentRow.find("#ItmInfoBtnIcon").attr("disabled", false);
            currentRow.find("#BtnTxtCalculation").attr("disabled", true);
            currentRow.find("#ord_qty").attr("disabled", false);
            currentRow.find("#item_rate").attr("disabled", false);
            currentRow.find("#remarks").attr("disabled", false);
            currentRow.find("#ItemListNameError").css("display", "none");
            currentRow.find("#ItemListName" + Sno).css("border-color", "#ced4da");//aria-labelledby="select2-ItemListName1-container"
            currentRow.find("select2-selection select2-selection--single").css("border", "1px solid #aaa");
            currentRow.find("[aria-labelledby='select2-ItemListName" + Sno + "-container']").css("border-color", "#ced4da");
            currentRow.find("#ord_qtyError").css("display", "none");
            currentRow.find("#ord_qty").css("border-color", "#ced4da");
            currentRow.find("#item_rateError").css("display", "none");
            currentRow.find("#item_rate").css("border-color", "#ced4da");
        })

        $("#ServicePOItemTbl #delBtnIcon1").css("display", "block");
        $("#ServicePOItemTbl .plus_icon1").css("display", "block");

    } //debugger;
    $("#Hdn_SupplierName").val(Supp_id);
    GetSuppAddress(Supp_id);
    $("#txtsrcdocdate").val("");
    BindDocumentNumberList(Supp_id);
}
function BindDocumentNumberList(Supp_id) {

    try {
        debugger;

        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ServiceVerification/GetServicePOList",
            data: { Supp_id: Supp_id },
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
                        debugger;
                        $("#ddlSPONo option").remove();
                        $("#ddlSPONo optgroup").remove();
                        $('#ddlSPONo').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option data-date="${arr.Table[i].spo_dt}" value="${arr.Table[i].spo_no}">${arr.Table[i].spo_no}</option>`);
                        }
                        var firstEmptySelect = true;
                        $('#ddlSPONo').select2({
                            templateResult: function (data) {
                                var DocDate = $(data.element).data('date');
                                var classAttr = $(data.element).attr('class');
                                var hasClass = typeof classAttr != 'undefined';
                                classAttr = hasClass ? ' ' + classAttr : '';
                                var $result = $(
                                    '<div class="row">' +
                                    '<div class="col-md-7 col-xs-12' + classAttr + '">' + data.text + '</div>' +
                                    '<div class="col-md-5 col-xs-12' + classAttr + '">' + DocDate + '</div>' +
                                    '</div>'
                                );
                                return $result;
                                firstEmptySelect = false;
                            }
                        });

                        $("#SPO_Date").val("");
                    }
                }
            },
        });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function GetSuppAddress(Supp_id) {
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ServicePurchaseOrder/GetSuppAddrDetail",
                data: {
                    Supp_id: Supp_id,
                },
                success: function (data) {
                    debugger;
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
                            $("#ddlCurrency").val(arr.Table[0].curr_name);
                            $("#Hdn_ddlCurrency").val(arr.Table[0].curr_id);

                            $("#conv_rate").val(parseFloat(arr.Table[0].conv_rate).toFixed($("#RateDigit").text()));
                            $("#conv_rate").prop("readonly", true);
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
                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function InsertVerificationDetail() {
    debugger;  
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    if (CheckSrverHeaderValidations() == false) {
        return false;
    }
    if (CheckSrverItemValidations() == false) {
        return false;
    }
    
    var FinalItemDetail = [];
    FinalItemDetail = InsertVerificationItemDetails();
    var ItemDt = JSON.stringify(FinalItemDetail);
    $('#hdItemDetailList').val(ItemDt);
   
    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    /*----- Attatchment End--------*/
    /*----- Attatchment start--------*/
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
    var SuppName = $("#SupplierName option:selected").text();
    $("#Hdn_SRPOSuppName").val(SuppName);

    $("#hdnsavebtn").val("AllreadyclickSaveBtn");

    return true;
};

function CheckSrverHeaderValidations() {
    debugger;
    var ErrorFlag = "N";
    if ($("#SupplierName").val() === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "Red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("#SupplierName").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
    }    
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function CheckSrverItemValidations() {
    debugger;
    var RateDecDigit = $("#RateDigit").text();
    var QtyDecDigit = $("#QtyDigit").text();
    var ErrorFlag = "N";
    if ($("#ServicePOItemTbl >tbody >tr").length > 0) {
        $("#ServicePOItemTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var Sno = currentRow.find("#SNohiddenfiled").val();
            var VerifyQty = currentRow.find("#Verific_qty").val();
            var Verifyby = currentRow.find("#VerifiedBy").val();
            var Verifyon = currentRow.find("#VerifiedOn").val();
            if (currentRow.find("#ServiceName" + Sno).val() == "0") {
                currentRow.find("#ServiceNameError").text($("#valueReq").text());
                currentRow.find("#ServiceNameError").css("display", "block");
                currentRow.find("[aria-labelledby='select2-ServiceName" + Sno + "-container']").css("border", "1px solid red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#ServiceNameError").css("display", "none");
                currentRow.find(".select2-selection select2-selection--single").css("border", "1px solid #aaa");
            }
            if (VerifyQty == "" || VerifyQty == parseFloat(0).toFixed(QtyDecDigit)) {
                currentRow.find("#Verific_qtyError").text($("#valueReq").text());
                currentRow.find("#Verific_qtyError").css("display", "block");
                currentRow.find("#Verific_qty").css("border-color", "red");
                currentRow.find("#Verific_qty").focus();
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#Verific_qtyError").css("display", "none");
                currentRow.find("#Verific_qty").css("border-color", "#ced4da");
            }
            if (Verifyby == "" || Verifyby == parseFloat(0).toFixed(QtyDecDigit)) {
               
                    currentRow.find("#VerifiedByError").text($("#valueReq").text());
                    currentRow.find("#VerifiedByError").css("display", "block");
                    currentRow.find("#VerifiedBy").css("border-color", "red");
                    currentRow.find("#VerifiedBy").focus();
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#VerifiedByError").css("display", "none");
                    currentRow.find("#VerifiedBy").css("border-color", "#ced4da");
            }
            
            if (Verifyon == "") {
               
                    currentRow.find("#VerifiedOnError").text($("#valueReq").text());
                    currentRow.find("#VerifiedOnError").css("display", "block");
                    currentRow.find("#VerifiedOn").css("border-color", "red");
                    currentRow.find("#VerifiedOn").focus();
                    ErrorFlag = "Y";
                }
                else {
                    currentRow.find("#VerifiedOnError").css("display", "none");
                    currentRow.find("#VerifiedOn").css("border-color", "#ced4da");
                }
            
        });
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
function InsertVerificationItemDetails() {
    debugger;
    var ItemDetailList = new Array();
    $("#ServicePOItemTbl TBODY TR").each(function () {
        debugger;
        var currentRow = $(this);
        var rowid = currentRow.find("#SNohiddenfiled").val();
       
        var ItemList = {};
        ItemList.ItemId = currentRow.find("#hfItemID").val();
        ItemList.ItemName = currentRow.find("#ServiceName").val();
        ItemList.HsnNo = currentRow.find("#HsnNo").val();
        ItemList.ord_qty = currentRow.find("#ord_qty").val();
        ItemList.pend_qty = currentRow.find("#pend_qty").val();
        ItemList.Verific_qty = currentRow.find("#Verific_qty").val();
        ItemList.VerifiedBy = currentRow.find("#VerifiedBy").val();
        ItemList.VerifiedOn = currentRow.find("#VerifiedOn").val();
        ItemList.Remarks = currentRow.find("#remarks").val();

        ItemDetailList.push(ItemList);

    });

    return ItemDetailList;
};
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
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#SupplierName').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#SrVerStatus").val().trim();
    var PQDTransType = sessionStorage.getItem("PQTransType");
    //var SuppPros_type = "S";
    //if ($("#Supplier").is(":checked")) {
    //    SuppPros_type = "S";
    //}
    //if ($("#Prospect").is(":checked")) {
    //    SuppPros_type = "P";
    //    bill_add_id = "";
    //}
    Cmn_SuppAddrInfoBtnClick1(Supp_id, bill_add_id, status, PQDTransType);

}
function OtherFunctions(StatusC, StatusName) {
    window.location.reload();
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
}
function OnChangeSPONo(Spo_No) {
    debugger;
    var SpoNo = Spo_No.value;
    var SPODate = $('#ddlSPONo').select2("data")[0].element.attributes[0].value;
    var SPODP = SPODate.split("-");
    $("#HDddlSPONo").val(SpoNo);
    var FSPODate = (SPODP[2] + "-" + SPODP[1] + "-" + SPODP[0]);
    if (SpoNo == "---Select---") {
        $("#SPO_Date").val("");
        $('#vmSrcDocNo').text($("#valueReq").text());
        $("#vmSrcDocNo").css("display", "block");
        $("[aria-labelledby='select2-ddlSPONo-container']").css("border-color", "red");
    }
    else {
        $("#SPO_Date").val(FSPODate);
        $("#vmSrcDocNo").css("display", "none");
        $("[aria-labelledby='select2-ddlSPONo-container']").css("border-color", "#ced4da");
    }
}
function OnClickAddButton() {
    debugger;
    var ConvRate;
    ConvRate = $("#conv_rate").val();
    if (ConvRate == "" || ConvRate == null || ConvRate == "0") {
        ConvRate = 1;
    }
    $("#conv_rate").prop("readonly", true);
    var SPONo = $('#ddlSPONo').val();
    var SPODate = $('#SPO_Date').val();
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount
    if (SPONo == "---Select---" || SPONo == "0") {
        $('#vmSrcDocNo').text($("#valueReq").text());
        $("#vmSrcDocNo").css("display", "block");
        $("[aria-labelledby='select2-ddlSPONo-container']").css("border-color", "red");
        $("#ddlSPONo").css("border-color", "red");
    }
    else {
        $("#vmSrcDocNo").css("display", "none");
        $("[aria-labelledby='select2-ddlSPONo-container']").css("border-color", "#ced4da");
        $("#ddlSPONo").css("border-color", "#ced4da");
        $("#SupplierName").attr("disabled", "disabled");
        $("#ddlSPONo").attr("disabled", "disabled");       
        $(".plus_icon1").css("display", "none");
        try {
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ServiceVerification/GetSPODetails",
                data: { SPONo: SPONo, SPODate: SPODate },
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
                            for (var k = 0; k < arr.Table.length; k++) {
                                var BaseVal;
                                BaseVal = (parseFloat(ConvRate).toFixed(ValDecDigit) * parseFloat(arr.Table[k].item_gross_val).toFixed(ValDecDigit))
                                var S_NO = $('#ServicePOItemTbl tbody tr').length + 1;
                                $('#ServicePOItemTbl tbody').append(`<tr id="R${++rowIdx}">
                                   
                                                        <td class="red center">
                                                                        <i class="fa fa-trash deleteIcon" aria-hidden="true" id="delBtnIcon1" title="${$("#Span_Delete_Title").text()}"></i>
                                                                    </td>
                                                                     <td class="sr_padding" id="srno">${S_NO}</td>
                                                                    <td style="display: none;"><input class="" type="hidden" id="SNohiddenfiled" value="${S_NO}" /></td>
                                                                    <td class="ItmNameBreak itmStick tditemfrz">
                                                                        <div class="lpo_form">
                                                                            <input id="ServiceName" class="form-control" autocomplete="off"  type="text" name="ServiceName" value='${arr.Table[k].item_name}' placeholder='${$("#ItemName").text()}' disabled>
                                                                            <span id="ServiceNameError" class="error-message is-visible"></span>
                                                                            <input  type="hidden" id="hfItemID" value="${arr.Table[k].item_id}" />
                                                                        </div>
                                                                    </td>
                                                                        <td>
                                                                        <div class="lpo_form">
                                                                            <input id="HsnNo" class="form-control" autocomplete="off" type="text" name="HsnNo" value="${arr.Table[k].hsn_no}" placeholder="0000.00" disabled>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="lpo_form">
                                                                            <input id="ord_qty" class="form-control num_right" autocomplete="off" onchange="" onkeypress="" type="text" name="ord_qty" value="${parseFloat(arr.Table[k].ord_qty).toFixed(QtyDecDigit)}" placeholder="0000.00" disabled>
                                                                        </div>
                                                                    </td>
                                                                            <td>
                                                                        <div class="lpo_form">
                                                                            <input id="pend_qty" class="form-control num_right" autocomplete="off" onchange="" onkeypress="" type="text" name="pend_qty" value="${parseFloat(arr.Table[k].pend_qty).toFixed(QtyDecDigit)}" placeholder="0000.00" disabled>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="lpo_form">
                                                                            <input id="Verific_qty" class="form-control num_right" autocomplete="off" onchange ="OnChangeVerificationQty(this,event)" onkeypress="return AmountFloatQty(this,event);"  type="text" name="Verific_qty" placeholder="0000.00">
                                                                        <span id="Verific_qtyError" class="error-message is-visible"></span>
                                                                        </div>
                                                                    </td>                                                                    
                                                                    <td>
                                                                        <div class="lpo_form">
                                                                            <input id="VerifiedBy" class="form-control" autocomplete="off" onchange ="OnChangeVerificationBy(this,event)" onkeypress="" type="text" name="VerifiedBy" placeholder="${$("#span_VerifiedBy").text()}">
                                                                    <span id="VerifiedByError" class="error-message is-visible"></span>
                                                                            </div>
                                                                    </td>
                                                                            <td>
                                                                        <div class="lpo_form">
                                                                            <input id="VerifiedOn" class="form-control" autocomplete="off"  onchange = "OnChangeVerifyDate(this,event)" type = "date" name="VerifiedOn">
                                                                         <span id="VerifiedOnError" class="error-message is-visible"></span>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength="250" placeholder="${$("#span_remarks").text()}" onmouseover="OnMouseOver(this)">${arr.Table[k].it_remarks}</textarea>
                                                                    </td>

                            </tr>`);                               
                            }                          
                        }
                    }
                },
            });
        } catch (err) {
            console.log("GoodReceiptInvoice Error : " + err.message);
        }
    }
}
function OnChangeVerificationQty(RowID, e)  {
    debugger;
    var docid = $("#DocumentMenuId").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var ValDecDigit = $("#ValDigit").text();
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var errorFlag = "N";
    var VerifyQty;
    var ItemName;   
    VerifyQty = clickedrow.find("#Verific_qty").val();
    PendQty = clickedrow.find("#pend_qty").val();
    if (VerifyQty != "" && VerifyQty != ".") {
        VerifyQty = parseFloat(VerifyQty);
    }
    if (VerifyQty == ".") {
        VerifyQty = 0;
    }
    if (VerifyQty == "" || VerifyQty == 0) {
        clickedrow.find("#Verific_qtyError").text($("#valueReq").text());
        clickedrow.find("#Verific_qtyError").css("display", "block");
        clickedrow.find("#Verific_qty").css("border-color", "red");
        clickedrow.find("#Verific_qty").val("");
        clickedrow.find("#Verific_qty").focus();
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#Verific_qtyError").text("");
        clickedrow.find("#Verific_qtyError").css("display", "none");
        clickedrow.find("#Verific_qty").css("border-color", "#ced4da");
    }

    var VerifyQty = clickedrow.find("#Verific_qty").val();
   
    if (AvoidDot(VerifyQty) == false) {
        clickedrow.find("#Verific_qty").val("");
        VerifyQty = 0;
    }

    if (parseFloat(VerifyQty) > parseFloat(PendQty)) {
        clickedrow.find("#Verific_qtyError").text($("#ExceedingQty").text());
        clickedrow.find("#Verific_qtyError").css("display", "block");
        clickedrow.find("#Verific_qty").css("border-color", "red");
        clickedrow.find("#Verific_qty").val("");
        VerifyQty = 0;
    }
    else {
        if (VerifyQty == "" || VerifyQty == 0) {
            clickedrow.find("#Verific_qtyError").text($("#valueReq").text());
            clickedrow.find("#Verific_qtyError").css("display", "block");
            clickedrow.find("#Verific_qty").css("border-color", "red");
            clickedrow.find("#Verific_qty").val("");
            clickedrow.find("#Verific_qty").focus();
            errorFlag = "Y";
        }
        else {
            clickedrow.find("#Verific_qtyError").text("");
            clickedrow.find("#Verific_qtyError").css("display", "none");
            clickedrow.find("#Verific_qty").css("border-color", "#ced4da");
        }       
    }
  
    clickedrow.find("#Verific_qty").val(parseFloat(VerifyQty).toFixed(QtyDecDigit));

  

}
function OnChangeVerificationBy(RowID, e) {
    debugger;
   
    var clickedrow = $(e.target).closest("tr");
    var Sno = clickedrow.find("#SNohiddenfiled").val();
    var errorFlag = "N";
   
    VerifyBy = clickedrow.find("#VerifiedBy").val();
   
    if (VerifyBy == "") {
        clickedrow.find("#VerifiedByError").text($("#valueReq").text());
        clickedrow.find("#VerifiedByError").css("display", "block");
        clickedrow.find("#VerifiedBy").css("border-color", "red");
        clickedrow.find("#VerifiedBy").val("");
        clickedrow.find("#VerifiedBy").focus();
        errorFlag = "Y";
    }
    else {
        clickedrow.find("#VerifiedByError").text("");
        clickedrow.find("#VerifiedByError").css("display", "none");
        clickedrow.find("#VerifiedBy").css("border-color", "#ced4da");
        clickedrow.find("#VerifiedBy").val(VerifyBy);
    }

}
function AmountFloatQty(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }

    var clickedrow = $(evt.target).closest("tr");
    var RowNo = clickedrow.find("#SNohiddenfiled").val();

    clickedrow.find("#Verific_qtyError" + RowNo).css("display", "none");
    clickedrow.find("#Verific_qty" + RowNo).css("border-color", "#ced4da");

   
    return true;
}

function OnChangeVerifyDate(ValidUpToDate,evt) {
    debugger;
    var ValidUpTo = ValidUpToDate.value;
    var SPO_Date = $("#SPO_Date").val();
    var clickedrow = $(evt.target).closest("tr");
    try {
        if (SPO_Date > ValidUpTo) {
            clickedrow.find("#VerifiedOn").val("")
            clickedrow.find('#VerifiedOnError').text($("#JC_InvalidDate").text());
            clickedrow.find("#VerifiedOnError").css("display", "block");
            return false;
        }
        else {
            clickedrow.find("#VerifiedOnError").css("display", "none");
            clickedrow.find("#VerifiedOn").css("border-color", "#ced4da");
        }
        var now = new Date();
        var month = (now.getMonth() + 1);
        var day = now.getDate();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        var today = now.getFullYear() + '-' + month + '-' + day;
        if (ValidUpTo > today) {
            var now = new Date();
            var month = (now.getMonth() + 1);
            var day = now.getDate();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            var today = now.getFullYear() + '-' + month + '-' + day;
            clickedrow.find("#VerifiedOn").val("")
            clickedrow.find('#VerifiedOnError').text($("#JC_InvalidDate").text());
            clickedrow.find("#VerifiedOnError").css("display", "block");
            return false;
        }
           

        var VerifiedOnDt = clickedrow.find("#VerifiedOn").val();

        if (VerifiedOnDt == "") {
            clickedrow.find("#VerifiedOnError").text($("#valueReq").text());
            clickedrow.find("#VerifiedOnError").css("display", "block");
            clickedrow.find("#VerifiedOn").css("border-color", "red");
            clickedrow.find("#VerifiedOn").val("");
            clickedrow.find("#VerifiedOn").focus();
            errorFlag = "Y";
        }
        else {
            clickedrow.find("#VerifiedOnError").text("");
            clickedrow.find("#VerifiedOnError").css("display", "none");
            clickedrow.find("#VerifiedOn").css("border-color", "#ced4da");
            //clickedrow.find("#VerifiedBy").val(VerifyBy);
        }
    } catch (err) {

    }
}
function CheckedCancelled() {
    //debugger;
    if ($("#Cancelled").is(":checked")) {       
        $("#btn_save").attr('onclick', 'return SaveBtnClick()');
        $("#btn_save").attr('disabled', false);
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    }
    else {
        $("#btn_save").attr('onclick', '');
        $("#btn_save").attr('disabled', true);
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });

    }
}

function PQSelectAddress() {

    $("#ItemDetailsTbl tbody").bind("click", function (e) {
        debugger;

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

}

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
function FilterItemDetail(e) {//added by Prakash Kumar on 26-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "ServicePOItemTbl", [{ "FieldId": "ServiceName", "FieldType": "input" }]);
}
