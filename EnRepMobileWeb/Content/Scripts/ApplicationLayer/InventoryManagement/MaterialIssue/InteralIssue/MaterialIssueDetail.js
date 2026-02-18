$(document).ready(function () {
    debugger;
    BindInternalIssueItmList();
    var Doc_id = $("#DocumentMenuId").val();
    if (Doc_id == "105102130108") {
        $("#ddlissuedby").select2();
    }
   
    $("#ddlRequiredArea").select2();
    var MRSType = $('#ddlRequisitionTypeList').val();
    if (MRSType === "I") {
        $('#MaterialIssueItemDetailsTbl td:nth-child(' + 10 + ')').hide();
        $('#MaterialIssueItemDetailsTbl th:nth-child(' + 10 + ')').hide();
        $("#div_IssueTo").css('display', 'none');
    }
    if (MRSType === "E" || MRSType === "S") {
        $('#MaterialIssueItemDetailsTbl td:nth-child(' + 10 + ')').show();
        $('#MaterialIssueItemDetailsTbl th:nth-child(' + 10 + ')').show();
        $("#div_IssueTo").css('display', 'block');
    }
    $('#MaterialIssueItemDetailsTbl').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        var Itemid = $(this).closest('tr').find("#hdItemId").val();
        Cmn_DeleteSubItemQtyDetail(Itemid);
        DeleteItemBatchSerialOrderQtyDetails(Itemid);
        $(this).closest('tr').remove();
        var MaterialIssueNo = $("#txtMaterialIssueNo").val();
        if (MaterialIssueNo == null || MaterialIssueNo == "") {
            if ($('#MaterialIssueItemDetailsTbl tr').length <= 1) {
                debugger;
                $("#ddlMRS_No").prop("disabled", false);
                //$("#ddlRequisitionTypeList").prop("disabled", false);
                $("#ddlRequiredArea").prop("disabled", false);
                $(".plus_icon1").css('display', 'block');
            }
        }
        updateItemSerialNumber()
    });
    //onChangeddlRequisitionTypeList();
    //var now = new Date();
    //var month = (now.getMonth() + 1);
    //var day = now.getDate();
    //if (month < 10)
    //    month = "0" + month;
    //if (day < 10)
    //    day = "0" + day;
    //var today = now.getFullYear() + '-' + month + '-' + day;
   // $("#txtTodate").val();
 
    
    
    $("#datatable-buttons >tbody").bind("dblclick", function (e) {
        debugger;
        try {
            var ListFilterData = $('#ListFilterData').val();
            var clickedrow = $(e.target).closest("tr");
            var IssueType = clickedrow.children("#issuetype").text();
            var IssueNumber = clickedrow.children("#issue_no").text();
            var IssueDate = clickedrow.children("#issue_date").text();
            var DocumentMenuId = $("#DocumentMenuId").val();
            if (IssueNumber != null && IssueNumber != "") {
                window.location.href = "/ApplicationLayer/MaterialIssue/EditMaterialIssue/?IssueType=" + IssueType + "&IssueNumber=" + IssueNumber + "&IssueDate=" + IssueDate + "&DMenuId=" + DocumentMenuId + "&ListFilterData=" + ListFilterData;
            }
            
        }
        catch (err) {
            debugger
        }
    });
    BindIssutToData();
    var status = $("#hdMrsStatus").val();
    var MrsType = $("#hdMrsType").val();
    if (MrsType == "E") {
        if (status == "I") {
            var entitytype = $("#hdentitytype").val();

            if (entitytype == "E") {
                $("#Div_Address").css("display", "none")
            }
        }
    }
   // if (MRSType === "E") {
        CancelledRemarks("#CancelFlag", "Disabled");
   // }
    var itemId = $("#hdnStockItemWiseMessage").val();
    if (itemId) {
        var msg = itemId.split(',');
        $('#MaterialIssueItemDetailsTbl > tbody > tr').each(function () {
            var cellText = $(this).find('#hdItemId').val().trim();
            if (msg.includes(cellText)) {
                $(this).css('border', '3px solid red');
            }
        });
        $("#hdnStockItemWiseMessage").val("");
    }
});
function onchangeIssuedby() {
    var issuedby = $("#ddlissuedby").val();
    if (issuedby != "" && issuedby != "0" && issuedby != null) {
        $("#hdnddlissuedby").val(issuedby);
    }
}
function OnClickbillingAddressIconBtn(e) {
    debugger;
    var Supp_id = $('#hdissueto').val();
    var entitytype = $('#hdentitytype').val();
    var bill_add_id = $('#bill_add_id').val().trim();
    $('#hd_add_type').val("B");
    var status = $("#hdMrsStatus").val();
    var PODTransType = "";
  var Disabled=  $("#DisableSubItem").val()
    if (Disabled == "N") {
        PODTransType = "Update";
    }
    if (entitytype == "S") {
        Cmn_SuppAddrInfoBtnClick(Supp_id, bill_add_id, status, PODTransType);

    }
    else if (entitytype == "C") {
        $('#AddressInfo_LblName').text($('#AddressInfo_BillAdd').text());
        var CustPros_type = "C";
        Cmn_AddrInfoBtnClick(Supp_id, CustPros_type, bill_add_id, status, PODTransType,'');
    }
    else {
      

            var Cust_id = "0";
        var bill_add_id1 = $('#bill_add_id').val();
            $('#hd_add_type').val("B");
        var CustPros_type1 = "B";
        var status1 = $("#hdMrsStatus").val().trim();
            var SSIDTransType1 = "";
        var Disabled1 = $("#DisableSubItem").val();
        if (Disabled1 == "Y") {
            SSIDTransType1 = "Update";
            }
            var SPI_no = $("#InvoiceNumber").val();
        Cmn_AddrInfoBtnClick(Cust_id, CustPros_type1, bill_add_id1, status1, SSIDTransType1, '');
            //Cmn_SuppAddrInfoBtnClick(Cust_id, bill_add_id, status, PODTransType);
        
    }
   
}

//function OnClickbillingAddressIconBtn(e) {

    
//    var Cust_id = $('#hdissueto').val();
//    var bill_add_id = $('#bill_add_id').val().trim();
//    $('#hd_add_type').val("B");
  

//    var status = $("#hfStatus").val().trim();
//    var SODTransType = "";
//    if ($("#ForDisableOCDlt").val() == "Enable") {
//        SODTransType = "Update";
//    } 
    
//}

function DeleteItemBatchSerialOrderQtyDetails(Itemid) {
    $("#SaveItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var rowitem = row.find("#mi_lineBatchItemId").val();
        //var rowitem = $("#HDItemNameBatchWise").val();
        if (rowitem == Itemid) {
            debugger;
            $(this).remove();
        }
    });
    $("#SaveItemSerialTbl TBODY TR").each(function () {
        var row = $(this)
        var HdnItemId = row.find("#mi_lineSerialItemId").val();
        if (HdnItemId == Itemid) {
            debugger;
            $(this).remove();
        }
    })
}

function BindIssutToData() {
    var sr_type=  $("#ddlRequisitionTypeList").val();
    $("#ddlMaterialIssueTo").select2({
        ajax: {
            url: $("#SearchIssueToList").val(),
            data: function (params) {
                var queryParameters = {
                    SearchIssueToName: params.term, // search term like "a" then "an"
                    sr_type: sr_type
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                if (data == 'ErrorPage') {
                    ErrorPage();
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
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function onChangeddlMRS_No() {
    debugger;
    var MRSDate = $("#ddlMRS_No").val();
    var entitytype = $("#hdentitytype").val();
    //var MRSDate = $('#txtMRS_Date').val();
    var date = MRSDate.split("-");
    var DFinal = date[2] + "-" + date[1] + "-" + date[0];
    var MRSNo = $("#ddlMRS_No option:selected").text();
    mrs_type  = $("#ddlRequisitionTypeList").val();
    $("#txtMRS_Date").val(DFinal);
        if (MRSDate != "0") {
            document.getElementById("vmMRS_No").innerHTML = null;
            $("#ddlMRS_No").css("border-color", "#ced4da");
            $("[aria-labelledby='select2-ddlMRS_No-container']").css("border-color", "#ced4da");
            $(".plus_icon1").css('display', 'block');
        }
        else {
            document.getElementById("vmMRS_No").innerHTML = $("#valueReq").text();
            $("#ddlMRS_No").css("border-color", "red");
            $("[aria-labelledby='select2-ddlMRS_No-container']").css("border-color", "red");
            $(".plus_icon1").css('display', 'none');
            $("#txtMRS_Date").val("");
            $("#txtEntityType").val("");
        }
    if (mrs_type == "E" || mrs_type == "S") {
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/MaterialIssue/GetMaterialRequisitionIssueTo",
                data: {
                    MRSDate: DFinal,
                    MRSNo: MRSNo,
                    mrs_type: mrs_type
                },
                success: function (data) {
                    debugger;
                    $('#IssueTo').html(data);
                    //$('#ddlissuedby').val($("#hdnissuebyID").val());
                 
                },
            });
       
    }
  
    if (mrs_type == "E") {
        GetSuppAddress(DFinal, MRSNo, mrs_type);
    }
}
function GetSuppAddress(DFinal, MRSNo, mrs_type) {
    debugger;
    try {
     
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/MaterialIssue/GetSuppAddrDetail",
                data: {
                    MRSDate: DFinal,
                    MRSNo: MRSNo,
                    mrs_type: mrs_type
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
                        var entitytype = $("#hdentitytype").val();
                        if (entitytype == "E") {
                            $("#Div_Address").css("display", "none");
                        }
                        if (entitytype != "E") {
                            if (arr.Table.length > 0) {
                                $("#Rqrdaddr").css("display", "");
                                $("#Address").val(arr.Table[0].BillingAddress);
                                $("#bill_add_id").val(arr.Table[0].bill_add_id);
                                $("#ship_add_gstNo").val(arr.Table[0].supp_gst_no);
                                $("#Ship_StateCode").val(arr.Table[0].state_code);
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
                                $("#conv_rate").val(parseFloat(0).toFixed($("#ExchDigit").text()));
                                $("#conv_rate").prop("readonly", false);

                            }
                        }
                      
                        else {
                           
                            $("#SpanSuppAddrErrorMsg").css("display", "none");
                            $("#SpanSuppAddrErrorMsg").css("display", "none");
                            $("#Address").css("border-color", "#ced4da");
                            $('#SpanSuppAddrErrorMsg').text("");
                            $("#Address").val(null);
                            $("#bill_add_id").val(null);
                            $("#ship_add_gstNo").val(null);
                            $("#Ship_StateCode").val(null);
                            var s = '<option value="' + "0" + '">' + "---Select---" + '</option>';
                            $("#ddlCurrency").html(s);
                            $("#conv_rate").val(parseFloat(0).toFixed($("#ExchDigit").text()));
                            $("#conv_rate").prop("readonly", false);
                        }


                    }
                },
            });
    } catch (err) {
        console.log("GetMenuData Error : " + err.message);
    }
}
function BindddlMaterialList() {
    debugger;
    var Area = $('#ddlRequiredArea').val();
    //var MRSNo = $('#ddlMRS_No').val();
    var RequisitionType = $('#ddlRequisitionTypeList').val();

    $.ajax({
        url: "/ApplicationLayer/MaterialIssue/GetMaterialIssueList",
        data: { FilterArea: Area, FilterRequisitionType: RequisitionType},
        success: function (data) {
            debugger;
            arr = data;//JSON.parse(data);
            
            $(".plus_icon1").css('display', 'none');
            if (arr.length > 0) {
                $("#ddlMRS_No option").remove();
                $("#ddlMRS_No optgroup").remove();
                $('#ddlMRS_No').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                for (var i = 0; i < arr.length; i++) {
                    $('#Textddl').append(`<option data-date="${arr[i].ID}" value="${arr[i].ID}">${arr[i].Name}</option>`);
                }
                
                Select2MaterialList();
                document.getElementById("vmMRS_No").innerHTML = null;
            }
            else {
                $("#ddlMRS_No option").remove();
                $("#ddlMRS_No optgroup").remove();
                $('#ddlMRS_No').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                $('#Textddl').append(`<option data-date="0" value="0">---Select---</option>`);
                Select2MaterialList();
                document.getElementById("vmMRS_No").innerHTML = null;
                
                //$("#ddlMRS_No").html('<option value="0">---Select---</option>');
            }
        },
    });

    //$("#ddlMRS_No").select2({
    //    ajax: {
    //        url: $("#hdMaterialIssueNo_Path").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                FilterMRSNo: params.term, // search term like "a" then "an"
    //                FilterArea: Area,
    //                FilterRequisitionType: RequisitionType
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        cache: true,
    //        delay: 250,
    //        //type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        processResults: function (data, params) {
    //            debugger;
    //            arr = data;//JSON.parse(data);
    //            if (arr.length > 0) {
    //                $("#ddlMRS_No option").remove();
    //                $("#ddlMRS_No optgroup").remove();
    //                $('#ddlMRS_No').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
    //                for (var i = 0; i < arr.length; i++) {
    //                    $('#Textddl').append(`<option data-date="${arr[i].ID}" value="${arr[i].ID}">${arr[i].Name}</option>`);
    //                }
    //                var firstEmptySelect = true;
    //                $('#ddlMRS_No').select2({
    //                    templateResult: function (data) {
    //                        var DocDate = $(data.element).data('date');
    //                        var classAttr = $(data.element).attr('class');
    //                        var hasClass = typeof classAttr != 'undefined';
    //                        classAttr = hasClass ? ' ' + classAttr : '';
    //                        var $result = $(
    //                            '<div class="row">' +
    //                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                            '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
    //                            '</div>'
    //                        );
    //                        return $result;
    //                        firstEmptySelect = false;
    //                    }
    //                });
    //                document.getElementById("vmMRS_No").innerHTML = null;
    //            }

    //        }
    //    },


    //})
}
function Select2MaterialList() {
    var firstEmptySelect = true;
    $('#ddlMRS_No').select2({
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
function AddItemDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ValDecDigit = $("#ValDigit").text();///Amount

    $("#HdMrsNo").val($('#ddlMRS_No option:selected').text());
    //$("#HdMrsDate").val($('#ddlMRS_No').val());
    $(".plus_icon1").css('display', 'none');
    debugger;
    $("#ddlMRS_No").prop("readonly", true);
    $("#txtMRS_Date").prop("readonly", true);
    $("#ddlRequisitionTypeList").prop("disabled", true);
    $("#ddlRequiredArea").prop("disabled", true);
    $("#ddlMRS_No").prop("disabled", true);

    var span_SubItemDetail = $("#span_SubItemDetail").text();
    var MRSType = $('#ddlRequisitionTypeList').val();
    var MRSNo = $('#ddlMRS_No option:selected').text();
    var MRSDate = $('#txtMRS_Date').val();

        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/MaterialIssue/getMaterialRequisitionDetailByNumber",
                data: {
                    MRSType: MRSType,
                    MRSNo: MRSNo,
                    MRSDate: MRSDate
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            var rowIdx = 0;
                            var WHAndAvlStk = "";
                            var Iserial = "";
                            var Ibatch = "";
                            var ItemType = "";
                            
                            for (var i = 0; i < arr.Table.length; i++) {
                                debugger;
                                var FlagRwk = arr.Table[i].FlagRwkJO;
                                var SrcTyp = arr.Table[i].src_doc_id;
                                $('#hdSrcTyp').val(SrcTyp);
                                var subitmDisable = "";
                                if (arr.Table[i].sub_item != "Y") {
                                    subitmDisable = "disabled";
                                }
                                // comment by Hina on 19-10-2023
                                // for reworkable job order when req material subitem in working with header subitem
                                //if (SrcTyp == "105105127") {
                                //    if (FlagRwk == "RWK") {
                                //        if (arr.Table[i].sub_item != "Y") {
                                //            subitmDisable = "disabled";
                                //        }
                                //        else {
                                //            subitmDisable = "Enabled";
                                //        }
                                //    }
                                //    else {
                                //        if (arr.Table[i].sub_item == "Y") {
                                //            subitmDisable = "disabled";
                                //        }
                                //        else {
                                //            subitmDisable = "disabled";
                                //        }
                                //    }
                                //}
                                //else {
                                //    if (arr.Table[i].sub_item != "Y") {
                                //        subitmDisable = "disabled";
                                //    }
                                //}
                                
                                var costprice = "";
                                if (parseFloat(arr.Table[i].IssuedQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
                                    costprice = "";
                                }
                                else {
                                    costprice = parseFloat(arr.Table[i].IssuedQuantity).toFixed(QtyDecDigit)
                                }
                                if (MRSType != "S") {
                                    
                                    WHAndAvlStk = `  <td width="7%">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <div class="lpo_form">
                                                                    <select class="form-control" id="wh_id${rowIdx+1}" onchange="OnChangeWarehouse(this,event)"><option value="0">---Select---</option></select>
                                                                    <input type="hidden" id="hdnWHId" value="${IsNull(arr.Table[i].wh_id,0)}" style="display: none;" />
                                                                      <span id="wh_Error${rowIdx + 1}" class="error-message is-visible"></span></div>
                                                                    </div> 
                                                                     <div class="col-sm-1 i_Icon">
                                                                    <button type="button" class="calculator" onclick="ItemStockWareHouseWise(this,event)" data-toggle="modal" data-target="#WareHouse" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_WarehouseWiseStock_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-9 lpo_form no-padding">
                                                                <input id="AvailableQuantity" value="${parseFloat(arr.Table[i].AvailableQuantity).toFixed(QtyDecDigit)}" readonly class="form-control num_right" autocomplete="off" type="text" name="AvailableQuantity" placeholder="0000.00"  >
                                                                </div>
                                                                <div class="col-sm-3 i_Icon no-padding" id="div_SubItemAvlStock">
                                                                    <button type="button" id="SubItemAvlStock" class="calculator subItmImg" ${subitmDisable} onclick="return GetSubItemAvlStock(event)" data-toggle="modal" data-target="#SubItemStock" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                </div>
                                                            </td>`;
                                    if (arr.Table[i].i_serial == 'Y') {
                                        Iserial = ` <td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" id="BtnBatchDetails" class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="Y" style="display: none;" /></td>`;
                                    }
                                    else {
                                        Iserial = ` <td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" id="BtnBatchDetails" disabled class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="N" style="display: none;" /></td>`;
                                    }

                                    if (arr.Table[i].i_batch == 'Y') {
                                        Ibatch = ` <td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" id="BtnBatchDetails" onchange="OnChangeIssueQty" class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                              <input type="hidden" id="hdi_batch" value="Y" style="display: none;" /></td>`;
                                    }
                                    else {
                                        Ibatch = ` <td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" id="BtnBatchDetails" onchange="OnChangeIssueQty" disabled class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_batch" value="N" style="display: none;" /></td>`;
                                    }

                                    if (arr.Table[i].i_batch == 'N' && arr.Table[i].i_serial == 'N') {
                                        ItemType = `<td style="display:none"><input id="ItemType" type="text" value="YES" /></td>`;
                                    }
                                    else {
                                        ItemType = `<td style="display:none"><input id="ItemType" type="text" value="NO" /></td>`;
                                    }

                                }
                                var today_date = moment().format('YYYY-MM-DD');
                                if (MRSType == "S") {
                                    $('#MaterialIssueItemDetailsTbl tbody').append(`
                                   <tr id="${++rowIdx}">
                                                            <td class=" red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                            <td class="sr_padding"><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled" value="${rowIdx}"</td>                                                        
                                                            <td class="ItmNameBreak itmStick tditemfrz">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="ItemName" value='${arr.Table[i].item_name}' class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder='${$("#ItemName").text()}' disabled>
                                                                    <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" onclick="OnClickIconBtn(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input value='${arr.Table[i].uom_name}' id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder='${$("#ItemUOM").text()}' disabled>
                                                                <input type="hidden" id="hdUOMId" value="${arr.Table[i].uom_id}" style="display: none;" />
                                                            </td>
                                                            <td><input id="SampleType"  class="form-control" autocomplete="off" type="text"  maxlength = "50", onkeyup="OnKeyupsam_typ(event)" onchange="OnChangesm_type(event);" onmouseover="OnMouseOver(this.value)" name="SampleType" placeholder='${$("#span_SampleType").text()}'></td>
                                                            <td><input id="OtherDetail"  class="form-control" autocomplete="off" type="text"  maxlength = "250", name="OtherDetail" onkeyup="OnKeyupoth_dtl(event)" onchange="OnChangeoth_dtl(event);" onmouseover="OnMouseOver(this.value)" placeholder='${$("#span_OtherDetail").text()}'></td>
                                                            <td>
                                                                 <div class="lpo_form"><input id="IssueDate"  class="form-control" autocomplete="off" max="${today_date}" onchange="onchangeIssuedate(this,event)" type="date" name="IssueDate" >
                                                            <span id="spanIssuedate" class="error-message is-visible"></span></td>
                                                                </div>
                                                            <td>
                                                                <div cclass="lpo_form">
                                                                   <input id="RequisitionQuantity"  value="${parseFloat(arr.Table[i].Requisition_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                              
                                                            </td>
                                                            <td>
                                                                <div class="lpo_form">
                                                                <input id="PendingQuantity" value="${parseFloat(arr.Table[i].PendingQuantity).toFixed(QtyDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="PendingQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                              
                                                            </td>
                                                            `+ WHAndAvlStk + `
                                                          
                                                            <td>
                                                               <div class="lpo_form">
                                                                <input id="IssuedQuantity" value="${parseFloat(arr.Table[i].PendingQuantity).toFixed(QtyDecDigit)}" onchange="OnChangeIssueQuantity(this,event)" onkeypress="return QtyFloatValueonly(this, event)" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00"  >
                                                                    <span id="IssuedQuantity_Error" class="error-message is-visible"></span>
                                                                </div>
                                                             
                                                            </td>
                                                            <td>
                                                                <div class="lpo_form">
                                                                <input id="CostPrice" value="${parseFloat(arr.Table[i].IssuedQuantity).toFixed(QtyDecDigit)}" maxlength = "20" onkeypress="return QtyFloatValueonly(this, event)" onpaste="return CopyPasteData(event);" onchange="OnChangeCostPrice(this,event)" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00"  >
                                                                <span id="CostPrice_Error" class="error-message is-visible"></span>
                                                                </div>
                                                            </td>
                                                           `
                                        + Ibatch + ``
                                        + Iserial + ``
                                        + ItemType + `

                                                                  <td>
                                                                <textarea id="BinLocation" class="form-control " name="BinLocation" maxlength = "25",  placeholder="${$("#span_BinLocation").text()}"></textarea>
                                                            </td>
                                                            <td>
                                                                <textarea id="remarks" class="form-control remarksmessage"  name="remarks" maxlength = "250",  placeholder="${$("#span_remarks").text()}">${arr.Table[i].remarks}</textarea>
                                                            </td>
                                                        </tr>
                                  `);
                                }
                                else {
                                    $('#MaterialIssueItemDetailsTbl tbody').append(`
                                   <tr id="${++rowIdx}">
                                                            <td class=" red center"><i class="deleteIcon fa fa-trash" aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
                                                            <td class="sr_padding"><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled" value="${rowIdx}"</td>                                                        
                                                            <td class="ItmNameBreak itmStick tditemfrz">
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="ItemName" value='${arr.Table[i].item_name}' class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder='${$("#ItemName").text()}' disabled>
                                                                    <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                    <input type="hidden" id="hdn_RwkJobOrderFlag" value="${IsNull(arr.Table[i].FlagRwkJO, '')}" style="display: none;" />

</div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" onclick="OnClickIconBtn(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input value='${arr.Table[i].uom_name}' id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder='${$("#ItemUOM").text()}' disabled>
                                                                <input type="hidden" id="hdUOMId" value="${arr.Table[i].uom_id}" style="display: none;" />
                                                            </td>
                                                          
                                                            <td>
                                                                <div class="col-sm-9 lpo_form no-padding">
                                                                   <input id="RequisitionQuantity"  value="${parseFloat(arr.Table[i].Requisition_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                                <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                                                <div class="col-sm-3 i_Icon no-padding" id="div_SubItemReqQty">
                                                                    <button type="button" id="SubItemReqQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('ReqQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-9 lpo_form no-padding">
                                                                <input id="PendingQuantity" value="${parseFloat(arr.Table[i].PendingQuantity).toFixed(QtyDecDigit)}"  class="form-control num_right" autocomplete="off" type="text" name="PendingQuantity" placeholder="0000.00"  disabled>
                                                                </div>
                                                                <div class="col-sm-3 i_Icon no-padding" id="div_SubItemPendQty">
                                                                    <button type="button" id="SubItemPendQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('PendQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                </div>
                                                            </td>
                                                            `+ WHAndAvlStk + `
                                                          
                                                            <td>
                                                               <div class="col-sm-9 lpo_form no-padding">
                                                                <input id="IssuedQuantity" value="${parseFloat(arr.Table[i].PendingQuantity).toFixed(QtyDecDigit)}" onchange="OnChangeIssueQuantity(this,event)" onkeypress="return QtyFloatValueonly(this, event)" class="form-control num_right" autocomplete="off" type="text" name="IssuedQuantity" placeholder="0000.00"  >
                                                                    <span id="IssuedQuantity_Error" class="error-message is-visible"></span>
                                                                </div>
                                                                <div class="col-sm-3 i_Icon no-padding" id="div_SubItemIssueQty">
                                                                    <button type="button" id="SubItemIssueQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('IssueQty',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="lpo_form">
                                                                <input id="CostPrice" value="${costprice}"  maxlength = "20" onchange="OnChangeCostPrice(this,event)" onkeypress="return QtyFloatValueonly(this, event)" class="form-control num_right" autocomplete="off" type="text"  name="IssuedQuantity" placeholder="0000.00"  >
                                                                <span id="CostPrice_Error" class="error-message is-visible"></span>
                                                                </div>
                                                            </td>
                                                           `
                                        + Ibatch + ``
                                        + Iserial + ``
                                        + ItemType + `
                                                            <td>
                                                                <textarea id="remarks" class="form-control remarksmessage" name="remarks" maxlength = "250",  placeholder="${$("#span_remarks").text()}"></textarea>
                                                            </td>
                                                        </tr>
                                  `);
                                }

                              
                                debugger;
                                $("#wh_id" + rowIdx).val(IsNull(arr.Table[i].wh_id, 0));
                                if (MRSType === "I") {
                                    $('#MaterialIssueItemDetailsTbl td:nth-child(' + 10 + ')').hide();
                                    $('#MaterialIssueItemDetailsTbl th:nth-child(' + 10 + ')').hide();
                                }
                                else {
                                    $('#MaterialIssueItemDetailsTbl td:nth-child(' + 10 + ')').show();
                                    $('#MaterialIssueItemDetailsTbl th:nth-child(' + 10 + ')').show();
                                }
                            }
                            BindWarehouseList(null);
                            debugger;
                            for (var i = 0; i < arr.Table1.length; i++) {
                                $("#hdn_Sub_ItemDetailTbl tbody").append(
                                    `<tr>
                                                <td><input type="text" id="ItemId" value='${arr.Table1[i].item_id}'></td>
                                                <td><input type="text" id="subItemId" value='${arr.Table1[i].sub_item_id}'></td>
                                                <td><input type="text" id="subItemQty" value=''></td>
                                                <td><input type="text" id="subItemReqQty" value='${arr.Table1[i].mrs_qty}'></td>
                                                <td><input type="text" id="subItemPendQty" value='${arr.Table1[i].pend_qty}'></td>
                                                <td><input type="text" id="subItemAvlQty" value=''></td>
                                            </tr>`);
                            }
                            debugger;
                            //if (arr.Table2.length > 0) {
                            //    for (var j = 0; j < arr.Table2.length; j++) {
                            //        debugger;
                            //        var RJO_No = arr.Table2[j].job_no;
                            //        var RJOitemID = arr.Table2[j].item_id;
                            //        var RJOWhId = arr.Table2[j].wh_id;
                            //        var RJOWhName = arr.Table2[j].wh_name;
                            //        if (RJO_No != "") {
                            //            debugger;
                            //            $("#MaterialIssueItemDetailsTbl TBODY TR").each(function () {
                            //                var Crow = $(this);
                            //                var Index = Crow.find("#SNohiddenfiled").val();
                            //                debugger;
                                           
                            //                var tblItemID = Crow.find("#hdItemId").val();
                            //                if (tblItemID == RJOitemID) {
                            //                    var hdnwhid = Crow.find("#hdnWHId").val();
                            //                    Crow.find("#wh_id" + Index).val(hdnwhid);
                            //                   $("#wh_id" + Index).prop("disabled", true)
                            //                }
                            //            });
                            //        }
                                
                            //    }
                               
                            //}
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
}
function onclickReplicateWithAllItems() {
    $("#Replicat_wh_id_Error").css("display", "none");
    $("#Replicat_wh_id").css("border-color", "#ced4da");
    $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    BindWarehouseList("Replicate");
}
function BindWarehouseList(id) {
    debugger;
    var DMenuId = $("#DocumentMenuId").val();
    $.ajax(
        {
            type: "POST",
            //url: "/ApplicationLayer/GRNDetail/GetWarehouseList1",
            url: "/ApplicationLayer/MaterialIssue/GetWarehouseList1",
            data: { doc_id: DMenuId},
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    //PO_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);

                    if (arr.Table.length > 0) {
                        if (id == "Replicate") {
                            var PreWhVal = $("#Replicat_wh_id").val();
                            var s = '<option value="0">---Select---</option>';
                            for (var i = 0; i < arr.Table.length; i++) {
                                s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                            }
                            $("#Replicat_wh_id").html(s);
                            $("#Replicat_wh_id").val(IsNull(PreWhVal, '0'));
                            $("#Replicat_wh_id").select2();
                        }
                        else {
                            var s = '<option value="0">---Select---</option>';
                            for (var i = 0; i < arr.Table.length; i++) {
                                s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                            }

                            var HdnWhId = arr.Table[0].wh_id;
                            if (id == null) {
                                $('#MaterialIssueItemDetailsTbl tbody tr').each(function () {
                                    var row = $(this);
                                    let srNo = row.find("#SNohiddenfiled").val();
                                    $("#wh_id" + srNo).html(s);
                                });
                            } else {
                                $("#wh_id" + id).html(s);
                            }
                            //$("#wh_id" + id).html(s);

                            $("#MaterialIssueItemDetailsTbl TBODY TR").each(function () {
                                var Crow = $(this);
                                var Index = Crow.find("#SNohiddenfiled").val();
                                var hdnwhid = Crow.find("#hdnWHId").val();
                                var hdItemId = Crow.find("#hdItemId").val();
                                debugger;
                                //if (HdnWhId == hdnwhid) {
                                if (hdnwhid != "0") {
                                    Crow.find("#wh_id" + Index).val(hdnwhid);
                                    $("#wh_id" + Index).prop("disabled", true);
                                }
                                else {

                                }
                            });
                        }
                       
                            
                        
                        debugger;
                        //$("#wh_id" + id).html(s);
                        
                    }
                }
            },
        });
}
function OnClickSaveAndExitReplicateBtn() {
    debugger;
    var ErrorFlag = "N";
    var wh_id = $("#Replicat_wh_id").val();
    if (wh_id == "0" || wh_id == "") {
        $("#Replicat_wh_id_Error").text($("#valueReq").text());
        $("#Replicat_wh_id_Error").css("display", "block");
        $("#Replicat_wh_id").css("border-color", "red");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        $("#Replicat_wh_id_Error").css("display", "none");
        $("#Replicat_wh_id").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    }
    if (ErrorFlag == "Y") {
        $("#SaveExitReplicateBtn").attr("data-dismiss", "");
        return false;
    }
    $("#MaterialIssueItemDetailsTbl > tbody > tr").each(function ()
    {
        var row = $(this);
        //var rowNo = row.find("#hdSpanRowId").val();
        var rowNo = row.find("#SNohiddenfiled").val();
        var hdnwhid = row.find("#hdnWHId").val();
        if (hdnwhid != "0" && hdnwhid != "" &&  hdnwhid != null) {
            row.find("#wh_id" + row).val(hdnwhid);
            row.find("#wh_id" + row).prop("disabled", true);
        }
        else {
            row.find("#wh_id" + rowNo).val(wh_id).trigger("change");
        }
      
    });
    $("#SaveExitReplicateBtn").attr("data-dismiss", "modal");
}
function OnChangeReplicatWarehouse() {
    var ErrorFlag = "N";
    var wh_id = $("#Replicat_wh_id").val();
    if (wh_id == "0" || wh_id == "") {
        $("#Replicat_wh_id_Error").text($("#valueReq").text());
        $("#Replicat_wh_id_Error").css("display", "block");
        $("#Replicat_wh_id").css("border-color", "red");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid red");
        ErrorFlag = "Y";
    }
    else {
        $("#Replicat_wh_id_Error").css("display", "none");
        $("#Replicat_wh_id").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-Replicat_wh_id-container"]').css("border", "1px solid #ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        BindItemBatchDetail();
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#hdItemId").val();;
        //var CompId = $("#HdCompId").val();
        //var BranchId = $("#HdBranchId").val();
        //var ItemId = clickedrow.find("#hdItemId").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var UOMID = clickedrow.find("#hdUOMId").val();

        $("#WareHouseWiseItemName").val(ItemName);
        $("#WareHouseWiseUOM").val(UOMName);
        $.ajax(
            {

                type: "Post",
                url: "/Common/Common/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId, UomId: UOMID
                    //CompId: CompId,
                    //BranchId: BranchId
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockWareHouseWise').html(data);
                    $("#WareHouseWiseItemName").val(ItemName);
                    $("#WareHouseWiseUOM").val(UOMName);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnChangeWarehouse(el, evt) {
    debugger;
    var clickedrow = $(evt.target).closest("tr");
    var Index = clickedrow.find("#SNohiddenfiled").val();
    var ddlId = "#wh_id" + Index;
    var whERRID = "#wh_Error" + Index;
    if (clickedrow.find(ddlId).val() == "0") {
        debugger;
        clickedrow.find(whERRID).text($("#valueReq").text());
        clickedrow.find(whERRID).css("display", "block");
        clickedrow.find(ddlId).css("border-color", "red");
    }
    else {
        clickedrow.find(whERRID).css("display", "none");
        clickedrow.find(ddlId).css("border-color", "#ced4da");
    }

    var ItemId = clickedrow.find("#hdItemId").val();;
    var UOMID = clickedrow.find("#hdUOMId").val();;
    var WarehouseId = clickedrow.find(ddlId).val();
    //var CompId = $("#HdCompId").val();
    //var BranchId = $("#HdBranchId").val();
    debugger;
    if (WarehouseId != "0") {
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#mi_lineBatchItemId").val();
            if (rowitem == ItemId) {
                debugger;
                $(this).remove();
            }
        });

        $.ajax({
            type: "Post",
            url: "/Common/Common/getWarehouseWiseItemStock",
            data: {
                ItemId: ItemId,
                WarehouseId: WarehouseId,
                UomId: UOMID
                //CompId: CompId,
                //BranchId: BranchId
            },
            success: function (data) {
                var QtyDecDigit = $("#QtyDigit").text();///Quantity

                var avaiableStock = JSON.parse(data);
                var parseavaiableStock = parseFloat(avaiableStock).toFixed(QtyDecDigit)
                clickedrow.find("#AvailableQuantity").val(parseavaiableStock);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                debugger;
            }
        });
    }
}

function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();

    ItemInfoBtnClick(ItmCode);

    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/LSODetail/GetSOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        ErrorPage();
    //                        return false;
    //                    }
    //                    if (data !== null && data !== "") {
    //                        var arr = [];
    //                        arr = JSON.parse(data);
    //                        if (arr.Table.length > 0) {
    //                            $("#Txt_ItmOEMNo").val(arr.Table[0].item_oem_no);
    //                            $("#Txt_ItmSampCode").val(arr.Table[0].item_sam_cd);
    //                            $("#Txt_ItmRefNo").val(arr.Table[0].RefNo);
    //                            $("#Txt_ItmHSNCode").val(arr.Table[0].HSN_code);
    //                            var ImgFlag = "N";
    //                            for (var i = 0; i < arr.Table.length; i++) {
    //                                if (arr.Table[i].item_img_name != "" && arr.Table[i].item_img_name != null) {
    //                                    ImgFlag = "Y";
    //                                }
    //                            }
    //                            if (ImgFlag == "Y") {
    //                                var OL = '<ol class="carousel-indicators">';
    //                                var Div = '<div class="carousel-inner">';
    //                                for (var i = 0; i < arr.Table.length; i++) {
    //                                    var ImgName = arr.Table[i].item_img_name;
    //                                    var origin = window.location.origin + "/Attachment/ItemSetup/" + ImgName;
    //                                    if (i === 0) {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '" class="active"> <img src="' + origin + '" />'
    //                                        Div += '<div class="carousel-item active"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                    else {
    //                                        OL += '<li data-target="#myCarousel" data-slide-to="' + i + '"><img src="' + origin + '" /></li>'
    //                                        Div += '<div class="carousel-item"><img src = "' + origin + '" style = "width:100%; max-height:200px;" /></div>'
    //                                    }
    //                                }
    //                                OL += '</ol>'
    //                                Div += '</div>'

    //                                var Ach = '<a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev"><span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="sr-only">Previous</span></a>';
    //                                var Ach1 = '<a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next"><span class="carousel-control-next-icon" aria-hidden="true"></span><span class="sr-only">Next</span></a>';

    //                                $("#myCarousel").html(OL + Div + Ach + Ach1);
    //                            }
    //                            else {
    //                                $("#myCarousel").html("");
    //                            }
    //                        }
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //}
}

//function BindItemBatchDetail() {
//    var batchrowcount = $('#SaveItemBatchTbl tr').length;
//    if (batchrowcount > 1) {
//        var ItemBatchList = new Array();
//        $("#SaveItemBatchTbl TBODY TR").each(function () {
//            var row = $(this)
//            var batchList = {};
//            batchList.LotNo = row.find('#mi_lineBatchLotNo').val();
//            batchList.ItemId = row.find('#mi_lineBatchItemId').val();
//            batchList.UOMId = row.find('#mi_lineBatchUOMId').val();
//            batchList.BatchNo = row.find('#mi_lineBatchBatchNo').val();
//            batchList.IssueQty = row.find('#mi_lineBatchIssueQty').val();
//            batchList.ExpiryDate = row.find('#mi_lineBatchExpiryDate').val();
//            batchList.avl_batch_qty = row.find('#mi_lineBatchavl_batch_qty').val();
//            ItemBatchList.push(batchList);
//            debugger;
//        });
//        var str1 = JSON.stringify(ItemBatchList);
//        $("#HDSelectedBatchwise").val(str1);
//    }

//}

function QtyFloatValueonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    else {
        return true;
    }
    
}
//function BindItemSerialDetail() {
//    var serialrowcount = $('#SaveItemSerialTbl tr').length;
//    if (serialrowcount > 1) {
//        var ItemSerialList = new Array();
//        $("#SaveItemSerialTbl TBODY TR").each(function () {
//            var row = $(this)
//            var SerialList = {};
//            SerialList.ItemId = row.find("#mi_lineSerialItemId").val();
//            SerialList.UOMId = row.find("#mi_lineSerialUOMId").val();
//            SerialList.LOTId = row.find("#mi_lineSerialLOTNo").val();
//            SerialList.IssuedQuantity = row.find("#mi_lineSerialIssueQty").val();
//            SerialList.SerialNO = row.find("#mi_lineBatchSerialNO").val();
//            ItemSerialList.push(SerialList);
//            debugger;
//        });
//        var str2 = JSON.stringify(ItemSerialList);
//        $("#HDSelectedSerialwise").val(str2);

//    }

//}
function ItemStockBatchWise(el, evt) {
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var MI_pedQty = clickedrow.find("#IssuedQuantity").val();

        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var HdnitmRJOFlag = clickedrow.find("#hdn_RwkJobOrderFlag").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "";
        }
        var MRSNo = $('#ddlMRS_No option:selected').text();
        
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#_mdlCommand").val();
        var TransType = $("#TransType").val();
        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/MaterialIssue/getItemStockBatchWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemdetail: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType,
                        
                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockBatchWise').html(data);
                       
                    },
                });
        }
        else {
            var Mrs_Status = $("#hdMrsStatus").val();
            if (Mrs_Status == "" || Mrs_Status == null) {
                BindItemBatchDetail();
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                var ddlId = "#wh_id" + Index;
                //var ItemId = clickedrow.find("#hdItemId").val();;
                var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();

                //$("#ItemNameBatchWise").val(ItemName);
                //$("#UOMBatchWise").val(UOMName);
                //$("#QuantityBatchWise").val(MI_pedQty);
                //$("#HDItemNameBatchWise").val(ItemId);
                //$("#HDUOMBatchWise").val(UOMId);
               // $("#MI_TotalIssuedQuantity").val("");
               // $("#IssuedQuantity").val("");
                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialIssue/getItemStockBatchWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemdetail: SelectedItemdetail,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            MRSNo: MRSNo,
                            HdnitmRJOFlag: HdnitmRJOFlag,
                            UomId:UOMId
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            var Index = clickedrow.find("#SNohiddenfiled").val();
                            var SelectedItemdetail = $("#HDSelectedBatchwise").val();
                            var ddlId = "#wh_id" + Index;
                            //var ItemId = clickedrow.find("#hdItemId").val();;
                            var WarehouseId = clickedrow.find(ddlId).val();
                            var CompId = $("#HdCompId").val();
                            var BranchId = $("#HdBranchId").val();

                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(MI_pedQty);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            // $("#MI_TotalIssuedQuantity").val("");
                            $("#BatchwiseTotalIssuedQuantity").val("");

                            //Added by Suraj on 20-02-2024
                            try {
                                //For Auto fill Quantity on FIFO basis in the Batch Table.
                                //this will work only first time after save old value will come in the table
                                Cmn_AutoFillBatchQty("BatchWiseItemStockTbl", MI_pedQty, "AvailableQuantity", "IssuedQuantity", "BatchwiseTotalIssuedQuantity");
                            } catch (err) {
                                console.log('Error : ' + err.message)
                            }
                        },
                    });
            }
            //if (Mrs_Status == "C" || Mrs_Status == "I") {
            else {
           

                var Mrs_IssueType = $("#hdMrsType").val();
                var Mrs_No = $("#txtMaterialIssueNo").val();
                var Mrs_Date = $("#txtMaterialIssueDate").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialIssue/getItemStockBatchWiseAfterStockUpadte",
                        data: {
                            IssueType: Mrs_IssueType,
                            IssueNo: Mrs_No,
                            IssueDate: Mrs_Date,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            UomId: UOMId
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockBatchWise').html(data);
                            $("#ItemNameBatchWise").val(ItemName);
                            $("#UOMBatchWise").val(UOMName);
                            $("#QuantityBatchWise").val(MI_pedQty);
                            $("#HDItemNameBatchWise").val(ItemId);
                            $("#HDUOMBatchWise").val(UOMId);
                            $("#BatchwiseTotalIssuedQuantity").val("");
                        },
                    });
            }
        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickItemBatchResetbtn() {
    $("#BatchWiseItemStockTbl tbody tr").each(function () {
        $(this).find("#IssuedQuantity").val("");
    });    
    $("#BatchwiseTotalIssuedQuantity").text("");
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;
       
        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();

        var QtyDecDigit = $("#QtyDigit").text();
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#hdItemId").val();;
        var UOMID = clickedrow.find("#hdUOMId").val();
        var HdnitmRJOFlag = clickedrow.find("#hdn_RwkJobOrderFlag").val();
        
        var MRSNo = $('#ddlMRS_No option:selected').text();
        var MI_pedQty = clickedrow.find("#IssuedQuantity").val();
        var DMenuId = $("#DocumentMenuId").val();
        var _mdlCommand = $("#_mdlCommand").val();
        var TransType = $("#TransType").val();

        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
            $.ajax(
                {
                    type: "Post",
                    url: "/ApplicationLayer/MaterialIssue/getItemstockSerialWise",
                    data: {
                        ItemId: "",
                        WarehouseId: "",
                        CompId: "",
                        BranchId: "",
                        SelectedItemSerial: "",
                        DMenuId: DMenuId,
                        Command: _mdlCommand,
                        TransType: TransType
                    },
                    success: function (data) {
                        debugger;
                        $('#ItemStockSerialWise').html(data);
                    },
                });

        }
        else {

            var Mrs_Status = $("#hdMrsStatus").val();
            if (Mrs_Status == "" || Mrs_Status == null) {
                BindItemSerialDetail();
                var Index = clickedrow.find("#SNohiddenfiled").val();
                var ddlId = "#wh_id" + Index;

                var WarehouseId = clickedrow.find(ddlId).val();
                var CompId = $("#HdCompId").val();
                var BranchId = $("#HdBranchId").val();
                var SelectedItemSerial = $("#HDSelectedSerialwise").val();

              

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialIssue/getItemstockSerialWise",
                        data: {
                            ItemId: ItemId,
                            WarehouseId: WarehouseId,
                            CompId: CompId,
                            BranchId: BranchId,
                            SelectedItemSerial: SelectedItemSerial,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType,
                            MRSNo: MRSNo,
                            HdnitmRJOFlag: HdnitmRJOFlag
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(MI_pedQty);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            //$("#TotalIssuedSerial").text("");
                        },
                    });
            }
            if (Mrs_Status == "C" || Mrs_Status == "I") {
           

                var Mrs_IssueType = $("#hdMrsType").val();
                var Mrs_No = $("#txtMaterialIssueNo").val();
                var Mrs_Date = $("#txtMaterialIssueDate").val();

                $.ajax(
                    {
                        type: "Post",
                        url: "/ApplicationLayer/MaterialIssue/getItemstockSerialWiseAfterStockUpadte",
                        data: {
                            IssueType: Mrs_IssueType,
                            IssueNo: Mrs_No,
                            IssueDate: Mrs_Date,
                            ItemID: ItemId,
                            DMenuId: DMenuId,
                            Command: _mdlCommand,
                            TransType: TransType
                        },
                        success: function (data) {
                            debugger;
                            $('#ItemStockSerialWise').html(data);
                            $("#ItemNameSerialWise").val(ItemName);
                            $("#UOMSerialWise").val(UOMName);
                            $("#QuantitySerialWise").val(MI_pedQty);
                            $("#HDItemIDSerialWise").val(ItemId);
                            $("#HDUOMIDSerialWise").val(UOMID);
                            //var TotalIssuedSerial = parseFloat(0).toFixed(QtyDecDigit);

                            //if ($("#SaveItemSerialTbl TBODY TR").length > 0) {
                            //    $("#SaveItemSerialTbl TBODY TR").each(function () {
                            //        var row = $(this)
                            //        var HdnItemId = row.find("#mi_lineSerialItemId").val();
                            //        if (ItemId === HdnItemId) {
                            //            TotalIssuedSerial = parseFloat(TotalIssuedSerial) + parseFloat(row.find("#mi_lineSerialIssueQty").val());
                            //        }
                            //    });
                            //}
                            //$("#TotalIssuedSerial").text(parseFloat(TotalIssuedSerial).toFixed(QtyDecDigit));
                        },
                    });
            }
           
        }

    } catch (err) {
        console.log("Material Issue Error : " + err.message);
    }
}
function OnChangeIssueQty(el, evt) {
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var TotalIssueQty = 0;
    try {
        debugger;
        var clickedrow = $(evt.target).closest("tr");
        var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
        var AvailableQuantity = clickedrow.find("#AvailableQuantity").val();
        if (AvoidDot(IssuedQuantity) == false) {
            IssuedQuantity = "0";
        }
        if (IssuedQuantity != "" && IssuedQuantity != null && AvailableQuantity != "" && AvailableQuantity != null) {
            if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {

                clickedrow.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
                clickedrow.find("#IssuedQuantity_Error").css("display", "block");
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
                var test = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
                clickedrow.find("#IssuedQuantity").val(test);
            }
            else {
                clickedrow.find("#IssuedQuantity_Error").css("display", "none");
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                var test = parseFloat(IssuedQuantity).toFixed(QtyDecDigit);
                if (parseFloat(IssuedQuantity).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
                    clickedrow.find("#IssuedQuantity").val("");
                }
                else {
                    clickedrow.find("#IssuedQuantity").val(test);
                }
              
            }
        }

        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#IssuedQuantity").val(); 
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty)+parseFloat(Issueqty);
            }
            debugger;
        });

        $("#BatchwiseTotalIssuedQuantity").text(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
//function onclickbtnItemBatchReset() {
//    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
//        var row = $(this);
//        row.find("#IssuedQuantity").val("");
//    });
//    $("#MI_TotalIssuedQuantity").val("");
//}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var IsuueFlag = true;
    var ItemMI_Qty = $("#QuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#BatchwiseTotalIssuedQuantity").text();
    $("#BatchWiseItemStockTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(CheckNullNumber(IssuedQuantity)) > parseFloat(AvailableQuantity)) {
            row.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
            ValidateEyeColor(row, "BtnBatchDetails", "Y");
            IsuueFlag = false;
        }
    });

    if (IsuueFlag) { 
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemNameBatchWise").val();
        $("#SaveItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#mi_lineBatchItemId").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });

        $("#BatchWiseItemStockTbl TBODY TR").each(function () {
            debugger;
            var row = $(this);
            var ItemIssueQuantity = row.find("#IssuedQuantity").val();
            //if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {
            if (CheckNullNumber(ItemIssueQuantity)>0) {

                var ItemUOMID = $("#HDUOMBatchWise").val();
                var ItemId = $("#HDItemNameBatchWise").val();
                var ItemBatchNo = row.find("#BatchNumber").val();
                //var ItemExpiryDate = row.find("#ExpiryDate").val();
                var ItemExpiryDate = row.find("#hfExDate").val();
                var AvailableQty = row.find("#AvailableQuantity").val();
                var LotNo = row.find("#Lot").val();
                var MfgName = row.find("#BtMfgName").val();
                var MfgMrp = row.find("#BtMfgMrp").val();
                var MfgDate = row.find("#BtMfgDate").val();
                $('#SaveItemBatchTbl tbody').append(
                    `<tr>
                    <td><input type="text" id="mi_lineBatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="mi_lineBatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="mi_lineBatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="mi_lineBatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="mi_lineBatchIssueQty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="mi_lineBatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="mi_lineBatchavl_batch_qty" value="${AvailableQty}" /></td>
                    <td id="mi_lineBatchMfgName">${MfgName}</td>
                    <td><input type="text" id="mi_lineBatchMfgMrp" value="${MfgMrp}" /></td>
                    <td><input type="text" id="mi_lineBatchMfgDate" value="${MfgDate}" /></td>
                </tr>`
                );
            }
        });
        $("#BatchNumber").modal('hide');
        }
        $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
            }
        });
    }
}
function onchangeChkItemSerialWise() {
    var TotalIssueLot = 0;
    var QtyDigit = $("#QtyDigit").text();
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalIssueLot = parseFloat(TotalIssueLot) + 1;
        }
    });
    $("#TotalIssuedSerial").text(parseFloat(TotalIssueLot).toFixed(QtyDigit));
    // localStorage.setItem('BatchResetFlag', 'True');
}
function onclickbtnItemSerialReset() {
    $("#ItemSerialwiseTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").text("");
}
function onclickbtnItemSerialSaveAndExit() {
    debugger;
    var ItemMI_Qty = $("#QuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").text();
    if (parseFloat(ItemMI_Qty) != parseFloat(ItemTotalIssuedQuantity)) {
        swal("", $("#SerializedIssuedQtydoesnotmatchwithIssueQty").text(), "warning");
    }
    else {
        debugger;
        var SelectedItem = $("#HDItemIDSerialWise").val();
        $("#SaveItemSerialTbl TBODY TR").each(function () {
            var row = $(this);
            var rowitem = row.find("#mi_lineSerialItemId").val();
            if (rowitem == SelectedItem) {
                debugger;
                $(this).remove();
            }
        });
        $("#ItemSerialwiseTbl TBODY TR").each(function () {
            var row = $(this);
            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            var mfg_name = row.find("#SrMfgName").val();
            var mfg_mrp = row.find("#SrMfgMrp").val();
            var mfg_date = row.find("#SrMfgDate").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(`<tr>
            <td><input type="text" id="mi_lineSerialItemId" value="${ItemId}" /></td>
            <td><input type="text" id="mi_lineSerialUOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="mi_lineSerialLOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="mi_lineSerialIssueQty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="mi_lineSerialSerialNO" value="${ItemSerialNO}" /></td>
            <td><input type="text" id="mi_lineSerialMfgName" value="${mfg_name}" /></td>
            <td><input type="text" id="mi_lineSerialMfgMrp" value="${mfg_mrp}" /></td>
            <td><input type="text" id="mi_lineSerialMfgDate" value="${mfg_date}" /></td>
            </tr>
            `
                );
            }
        });
        $("#SerialDetail").modal('hide');

        $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
            }
        });
    }
}
function onclickbtnItemBatchDiscardAndExit() {
    $("#BatchNumber").modal('hide');
}
function onclickbtnItemSerialDiscardAndExit() {
    $("#SerialDetail").modal('hide');
}
function CheckFormValidation() {
    debugger;
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }
    var MRSType = $('#ddlRequisitionTypeList').val();
    
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
    var rowcount = $('#MaterialIssueItemDetailsTbl tr').length;
    var ValidationFlag = true;
    var RequisitionType = $('#ddlRequisitionTypeList').val();
    var RequiredArea = $('#ddlRequiredArea').val();
    var MRS_No = $('#ddlMRS_No').val();
    var MRS_Type = $('#ddlRequisitionTypeList').val();
    var HD_SrcTyp =  $('#hdSrcTyp').val();
    $('#hdMrsType').val(MRS_Type);
    $('#hdRequiredArea').val(RequiredArea);

    var ReqArea = $('#ddlRequiredArea option:selected').text();
    var Req_dt = $('#ddlMRS_No option:selected').val();
    var Req_Number = $('#ddlMRS_No option:selected').text();

    $('#hidenRequiredArea').val(ReqArea);
    $('#hiddenMRS_No').val(Req_Number);
    $('#hiddenMRS_Date').val(Req_dt);

    if (RequisitionType == "" || RequisitionType == "0") {
        document.getElementById("vmRequisitionTypeList").innerHTML = $("#valueReq").text();
        $("#ddlRequisitionTypeList").css("border-color", "red");
        ValidationFlag = false;
    }
    if (RequiredArea == "" || RequiredArea == "0") {
        //document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        $('#vmRequiredArea').text($("#valueReq").text());
        $("#ddlRequiredArea").css("border-color", "red");
       /* $("[aria-labelledby='select2-ddlRequiredArea-container']").css("border-color", "red");*/
        ValidationFlag = false;
    }
    if (MRS_No == "" || MRS_No == "0") {
        document.getElementById("vmMRS_No").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddlMRS_No-container']").css("border-color", "red");
        $("#ddlMRS_No").css("border-color", "red");
        ValidationFlag = false;
    }


    var MRS_Status = $('#hdMrsStatus').val().trim(); 

    if (MRS_Status == "I") {
        $("#hdnsavebtn").val("AllreadyclickSaveBtn");
        return true;
    }

    HideErrorMsg();

    if (ValidationFlag == true) {
        debugger
        if (rowcount > 1) {

            var flag = CheckMI_mentItemValidations();
            if (flag == false) {
                return false;
            }
            var Batchflag = true, SerialFlag = true;
            if (MRS_Type == "I" || MRS_Type == "E") {
                 Batchflag = CheckItemBatchValidation();
                if (Batchflag == false) {
                    return false;
                }
                 SerialFlag = CheckItemSerialValidation();
                if (SerialFlag == false) {
                    return false;
                }
                var subitmFlag = CheckValidations_forSubItems();
                if (subitmFlag == false) {
                    return false;
                }
            }
           

            if (flag == true && Batchflag == true && SerialFlag==true) {

                var MRSNo = $("#ddlMRS_No option:selected").text();
                //$("#HdMrsNo").val(MRSNo);
                $("#ddlMRS_No").val(MRSNo);

                var MI_NONo = $("hdmi_no").val();
                $("#mi_Number").val(MI_NONo);

                var MI_dt = $("hdmi_dt").val();
                $("#txtship_dt").val(MI_dt);

                var DeliveryNoteItemDetailList = new Array();
                $("#MaterialIssueItemDetailsTbl TBODY TR").each(function () {
                    var row = $(this);

                    var Index = row.find("#SNohiddenfiled").val();
                    var whERRID = "#wh_id" + Index + "  option:selected";
                    var ItemList = {};

                    ItemList.ItemName = row.find("#ItemName").val();
                    ItemList.ItemId = row.find("#hdItemId").val();
                    ItemList.FlagRwkJO = IsNull(row.find("#hdn_RwkJobOrderFlag").val(),'');
                    ItemList.UOM = row.find("#UOM").val();
                    ItemList.UOMId = row.find('#hdUOMId').val();
                    ItemList.WareHouseId = CheckNullNumber(row.find(whERRID).val());
                    ItemList.mrs_qty = row.find('#RequisitionQuantity').val();
                    ItemList.pend_qty = row.find("#PendingQuantity").val();
                    ItemList.TotalWeight = row.find("#TotalWeight").val();
                    ItemList.MI_pedQuantity = row.find("#ShippedQuantity").val();
                    ItemList.avl_stock = CheckNullNumber(row.find('#AvailableQuantity').val());
                    ItemList.issue_qty = row.find('#IssuedQuantity').val();
                    ItemList.remarks = row.find('#remarks').val();
                    ItemList.CostPrice = row.find('#CostPrice').val();                    
                    ItemList.sr_type = row.find('#SampleType').val();
                    ItemList.other_dtl = row.find('#OtherDetail').val();
                    ItemList.issue_date = row.find('#IssueDate').val();
                    ItemList.sub_item = row.find('#sub_item').val();
                    ItemList.i_batch = row.find('#hdi_batch').val();
                    ItemList.i_serial = row.find('#hdi_serial').val();
                    ItemList.bin_loc = row.find('#BinLocation').val();
                    DeliveryNoteItemDetailList.push(ItemList);
                    debugger;
                });
                var str = JSON.stringify(DeliveryNoteItemDetailList);
                $('#hdMaterialIssueItemDetails').val(str);
                if (MRS_Type == "I" || MRS_Type == "E") {
                    BindItemBatchDetail();
                    BindItemSerialDetail();
                }

                /*-----------Sub-item-------------*/
                if (MRS_Type != "S") {
                    var SubItemsListArr = MI_SubItemList();
                    var str2 = JSON.stringify(SubItemsListArr);
                    $('#SubItemDetailsDt').val(str2);
                }

                /*-----------Sub-item end-------------*/
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
        //if (TotalBatchIssueQty != TotalMI_Qty) {
        //    swal("", $("#IssuedExceedsRequireQty").text(), "warning");
        //}
        return false;
    }

}
function OnClickCancelFlag() {
    debugger;
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").css("filter", "grayscale(0%)");
        $("#btn_save").attr("onclick","return  CheckFormValidation();")
    }
    else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        $("#btn_save").attr("onclick", "")
    }
    var MRSType = $('#ddlRequisitionTypeList').val();
    //if (MRSType === "E") {
        CancelledRemarks("#CancelFlag", "Enable");
    //}
}
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Batchable = clickedrow.find("#hdi_batch").val();
        
        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                BatchableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemBatchTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var bchIssueQty = currentRow.find('#mi_lineBatchIssueQty').val();
                    var bchitemid = currentRow.find('#mi_lineBatchItemId').val();
                    var bchuomid = currentRow.find('#mi_lineBatchUOMId').val();
                    if (ItemId == bchitemid && UOMId == bchuomid) {
                        TotalItemBatchQty = parseFloat(TotalItemBatchQty) + parseFloat(bchIssueQty);
                    }
                });

                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemBatchQty).toFixed(QtyDecDigit)) {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
                }
                else {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                    BatchableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (BatchableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (BatchableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Batchqtydoesnotmatchwithissuedqty").text(), "warning");
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
    var QtyDecDigit = $("#QtyDigit").text();
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                SerialableFlag = "Y";
                EmptyFlag = "Y";
            }
            else {
                $("#SaveItemSerialTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var srialIssueQty = currentRow.find('#mi_lineSerialIssueQty').val();
                    var srialitemid = currentRow.find('#mi_lineSerialItemId').val();
                    var srialuomid = currentRow.find('#mi_lineSerialUOMId').val();
                    if (ItemId == srialitemid && UOMId == srialuomid) {
                        TotalItemSerialQty = parseFloat(TotalItemSerialQty) + parseFloat(srialIssueQty);
                    }
                });

                if (parseFloat(IssueQuantity).toFixed(QtyDecDigit) == parseFloat(TotalItemSerialQty).toFixed(QtyDecDigit)) {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "N");
                }
                else {
                    //clickedrow.find("#IssuedQuantity").css("border-color", "red");
                    ValidateEyeColor(clickedrow, "BtnBatchDetails", "Y");
                    SerialableFlag = "Y";
                    EmptyFlag = "N";
                }
            }
        }
    });
    if (SerialableFlag == "Y" && EmptyFlag == "Y") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
        ErrorFlag = "Y";
        return false;
    }
    if (SerialableFlag == "Y" && EmptyFlag == "N") {
        swal("", $("#Serialqtydoesnotmatchwithissuedqty").text(), "warning");
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

function OnChangeIssueQuantity(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    
    var clickedrow = $(evt.target).closest("tr");
    var IssuedQuantity = clickedrow.find("#IssuedQuantity").val();
    if ((IssuedQuantity == "") || (parseFloat(IssuedQuantity) == parseFloat("0")) || (IssuedQuantity == null)) {
        clickedrow.find("#IssuedQuantity").val("");
        clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        clickedrow.find("#IssuedQuantity").val("");
       // clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));

        HideErrorMsg();

        return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
    }
    var PendingQuantity = clickedrow.find("#PendingQuantity").val();
    var AvailableQuantity = clickedrow.find("#AvailableQuantity").val(); 

    
    if (parseFloat(IssuedQuantity) > parseFloat(PendingQuantity)) {
        debugger;
        clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtycannotbegreaterthanPendingQty").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        //clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val("");

        HideErrorMsg();

        return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
    }
    var Issue_Type = $('#ddlRequisitionTypeList').val();
    if (Issue_Type == "I" || Issue_Type == "E") {
        if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
            debugger;
            clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtyGreaterthanAvaiQty").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
           // clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
            clickedrow.find("#IssuedQuantity").val("");

            HideErrorMsg();

            return false;
        }
        else {
            clickedrow.find("#IssuedQuantity_Error").css("display", "none");
            clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
            var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
            clickedrow.find("#IssuedQuantity").val(mi_issueqty);
        }
    }
}
function OnChangeCostPrice(el, evt) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity

    var clickedrow = $(evt.target).closest("tr");
    var CostPrice = clickedrow.find("#CostPrice").val();

    if (CostPrice == "0" || CostPrice == "" || CostPrice == null || parseFloat(CostPrice).toFixed(QtyDecDigit) == parseFloat(0).toFixed(QtyDecDigit)) {
        clickedrow.find("#CostPrice_Error").text($("#valueReq").text());
        clickedrow.find("#CostPrice_Error").css("display", "block");
        clickedrow.find("#CostPrice").css("border-color", "red");
        clickedrow.find("#CostPrice").val("");
        return false;
    }
    else {
        clickedrow.find("#CostPrice_Error").css("display", "none");
        clickedrow.find("#CostPrice").css("border-color", "#ced4da");
        var mi_costprice = parseFloat(CostPrice).toFixed(QtyDecDigit);
        clickedrow.find("#CostPrice").val(mi_costprice);
    }
}
function CheckMI_mentItemValidations() {
    debugger;
    var ErrorFlag = "N";
    var RowIndex = 0;
    var MI_Type = $('#hdMrsType').val();
    var TxtQtyFocus = false;
    $("#MaterialIssueItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        RowIndex++;
        var ddlId = "#wh_id" + RowIndex;
        var whERRID = "#wh_Error" + RowIndex;

        var IssueQuantity = currentRow.find("#IssuedQuantity").val();
        var PendingQuantity = currentRow.find("#PendingQuantity").val();
        var AvaialbleQuantity = currentRow.find("#AvailableQuantity").val();
        var Issue_Type = $('#ddlRequisitionTypeList').val();      
        if (Issue_Type == "I" || Issue_Type == "E") {
            if (currentRow.find(ddlId).val() == "" || parseFloat(currentRow.find(ddlId).val()) == parseFloat("0")) {
                currentRow.find(whERRID).text($("#valueReq").text());
                currentRow.find(whERRID).css("display", "block");
                currentRow.find(ddlId).css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find(whERRID).css("display", "none");
                currentRow.find(ddlId).css("border-color", "#ced4da");
            }
            debugger;
            if (parseFloat(IssueQuantity) > parseFloat(AvaialbleQuantity)) {
                currentRow.find("#IssuedQuantity_Error").text($("#IssuedQtyGreaterthanAvaiQty").text());
                currentRow.find("#IssuedQuantity_Error").css("display", "block");
                currentRow.find("#IssuedQuantity").css("border-color", "red");
                ErrorFlag = "Y";
                if (!TxtQtyFocus) {
                    currentRow.find("#IssuedQuantity").focus();
                    TxtQtyFocus = true;
                }
            }
            else {
                currentRow.find("#IssuedQuantity_Error").css("display", "none");
                currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
            }
        }
        else {
            var issue_date = currentRow.find('#IssueDate').val();
            if (issue_date == "" || issue_date == "1990-01-01") {
                currentRow.find("#spanIssuedate").text($("#valueReq").text());
                currentRow.find("#spanIssuedate").css("display", "block");
                currentRow.find("#IssueDate").css("border-color", "Red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#spanIssuedate").css("display", "none");
                currentRow.find("#IssueDate").css("border-color", "#ced4da");
            }
        }
        if (ErrorFlag == "Y") {
            return false;
        }
        if (parseFloat(IssueQuantity) > parseFloat(PendingQuantity)) {
            currentRow.find("#IssuedQuantity_Error").text($("#IssuedQtycannotbegreaterthanPendingQty").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
            if (!TxtQtyFocus) {
                currentRow.find("#IssuedQuantity").focus();
                TxtQtyFocus = true;
            }
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        if (parseFloat(CheckNullNumber(IssueQuantity))==0) {
            currentRow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
            if (!TxtQtyFocus) {
                currentRow.find("#IssuedQuantity").focus();
                TxtQtyFocus = true;
            }
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        if (MI_Type === 'E' || MI_Type === 'S') {
            var CostPrice = currentRow.find("#CostPrice").val();

            if (parseFloat(CostPrice) == parseFloat("0") || parseFloat(CostPrice) == "" || parseFloat(CostPrice) == null) {
                currentRow.find("#CostPrice_Error").text($("#valueReq").text());
                currentRow.find("#CostPrice_Error").css("display", "block");
                currentRow.find("#CostPrice").css("border-color", "red");
                ErrorFlag = "Y";
            }
            else {
                currentRow.find("#CostPrice_Error").css("display", "none");
                currentRow.find("#CostPrice").css("border-color", "#ced4da");
            }
        }
    })
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
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
            //  var today = fromDate.getFullYear() + '-' + month + '-' + day;

            var today = fromDate.getFullYear() + '-' + day + '-' + month;
            $("#txtFromdate").val(today);
            // alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");



        }
    }
    else {
        //  alert('please select from and to date');
    }
}
function FilterMaterialIssueListData() {
    debugger;
    try {
        var RequisitionTyp = $("#ddlRequisitionTypeList option:selected").val();
        var RequiredArea = $("#ddlRequiredArea option:selected").val();
        var MaterialIssueTo = $("#ddlMaterialIssueTo option:selected").val();   
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus option:selected").val();
        var Docid = $("#DocumentMenuId").val();
        var flag = "ListPage";
        $("#datatable-buttons2")[0].id = "dttbl";
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialIssue/MaterialIssuetListSearch",
            data: {
                RequisitionTyp: RequisitionTyp,
                RequiredArea: RequiredArea,
                MaterialIssueTo: MaterialIssueTo,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status,
                flag: flag,
                Docid: Docid
            },
            success: function (data) {
                debugger;
                $('#MaterialIssuetbBody').html(data);
                $('#ListFilterData').val(RequisitionTyp+','+RequiredArea + ',' + MaterialIssueTo + ',' + Fromdate + ',' + Todate + ',' +  Status);
                $("#dttbl")[0].id = "datatable-buttons2";
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }

        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}
function onChangeddlRequisitionTypeList() {
    debugger;
    $("#ddlMRS_No").val(0);
    $("#txtMRS_Date").val("");
    $("#txtEntityType").val("");
    
    var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
    if (RequisitionTypeList != "0") {
       
        if (RequisitionTypeList == "E" || RequisitionTypeList == "S") {
            $("#div_IssueTo").css('display', 'block');
            //$("#txtEntityType").css('display', 'block');
            //$("#lblIssueTo").css('display', 'block');
        }
        if (RequisitionTypeList == "I") {
            $("#div_IssueTo").css('display', 'none');
            //$("#txtEntityType").css('display', 'none');
            //$("#lblIssueTo").css('display', 'none');
        }
        document.getElementById("vmRequisitionTypeList").innerHTML = null;
        $("#ddlRequisitionTypeList").css("border-color", "#ced4da");
        var RequiredArea = $('#ddlRequiredArea').val();
        if (RequiredArea != "0") {
            BindddlMaterialList();
        }
    }
    else {

        document.getElementById("vmRequisitionTypeList").innerHTML = $("#valueReq").text();
        $("#ddlRequisitionTypeList").css("border-color", "red");   
        $("#txtEntityType").css('display', 'none');
        $("#lblIssueTo").css('display', 'none');
    }
}
function onChangeRequiredArea() {
    debugger;
    //$("#ddlMRS_No").val(0);
    $("#txtMRS_Date").val("");
    $("#txtEntityType").val("");
    var RequiredArea = $('#ddlRequiredArea').val();
    if (RequiredArea != "0") {
        document.getElementById("vmRequiredArea").innerHTML = null;
        $("#ddlRequiredArea").css("border-color", "#ced4da");
        var RequisitionTypeList = $('#ddlRequisitionTypeList').val();
        if (RequisitionTypeList != "0") {
            BindddlMaterialList();
        }
    }
    else {
        document.getElementById("vmRequiredArea").innerHTML = $("#valueReq").text();
        $("#ddlRequiredArea").css("border-color", "red");
        
    }
}

/***--------------------------------Sub Item Section-----------------------------------------***/
function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    var hfsno = clickdRow.find("#SNohiddenfiled").val();
    var RwkJobOrdFlag = clickdRow.find("#hdn_RwkJobOrderFlag").val();
    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var wh_id = clickdRow.find("#wh_id" + hfsno).val();
    var UOM = clickdRow.find("#UOM").val();
    var UOMID = clickdRow.find("#hdUOMId").val();

    var doc_no = $("#txtMaterialIssueNo").val();
    var doc_dt = $("#txtMaterialIssueDate").val();

    //var UOMID = clickdRow.find("#UOMID").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    $("#hdn_Sub_ItemDetailTbl tbody tr td #ItemId[value=" + ProductId + "]").closest("tr").each(function () {
        var row = $(this);
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        List.req_qty = row.find('#subItemReqQty').val();
        List.pend_qty = row.find('#subItemPendQty').val();
        List.avl_stock = row.find('#subItemAvlQty').val();
        NewArr.push(List);
    });
    if (flag == "IssueQty") {
        Sub_Quantity = clickdRow.find("#IssuedQuantity").val();
    } else if (flag == "PendingQty") {
        Sub_Quantity = clickdRow.find("#PendingQuantity").val();
    } else if (flag == "ReqQty") {
        Sub_Quantity = clickdRow.find("#RequisitionQuantity").val().trim();
    }

    var IsDisabled = $("#DisableSubItem").val();
    var hd_Status = $("#hdMrsStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    var MI_Type = $("#ddlRequisitionTypeList").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialIssue/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt,
            MI_Type: MI_Type,
            wh_id: wh_id,
            RwkJobOrdFlag: RwkJobOrdFlag, UomId: UOMID
        },
        success: function (data) {
            debugger;
            $("#SubItemPopUp").html(data);
            $("#Sub_ProductlName").val(ProductNm);
            $("#Sub_ProductlId").val(ProductId);
            $("#Sub_serialUOM").val(UOM);
            $("#Sub_Quantity").val(Sub_Quantity);
        }
    })

}
function fn_CustomReSetSubItemData(itemId) {

    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
        var Crow = $(this).closest("tr");
        var subItemId = Crow.find("#subItemId").val();
        var subItemQty = Crow.find("#subItemQty").val();
        var subItemReqQty = Crow.find("#subItemReqQty").val();
        var subItemPendQty = Crow.find("#subItemPendQty").val();
        var subItemAvlQty = Crow.find("#subItemAvlQty").val();
        var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').length;
        if (len > 0) {
            $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').each(function () {
                var InnerCrow = $(this).closest("tr");
                //var ItemId = InnerCrow.find("#ItemId").val();
                InnerCrow.find("#subItemQty").val(subItemQty);
                InnerCrow.find("#subItemReqQty").val(subItemReqQty);
                InnerCrow.find("#subItemPendQty").val(subItemPendQty);
                InnerCrow.find("#subItemAvlQty").val(subItemAvlQty);

            });
        } else {

            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="ItemId" value='${itemId}'></td>
                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
                            <td><input type="text" id="subItemReqQty" value='${subItemReqQty}'></td>
                            <td><input type="text" id="subItemPendQty" value='${subItemPendQty}'></td>
                            <td><input type="text" id="subItemAvlQty" value='${subItemAvlQty}'></td>
                        </tr>`);

        }

    });

}
function CheckValidations_forSubItems() {
    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
    return Cmn_CheckValidations_forSubItems("MaterialIssueItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "SubItemIssueQty", "Y");
}

function GetSubItemAvlStock(e) {
    debugger;
    var Crow = $(e.target).closest('tr');
    var ProductNm = Crow.find("#ItemName").val();
    var ProductId = Crow.find("#hdItemId").val();
    var UOM = Crow.find("#UOM").val();
    var UOMID = Crow.find("#hdUOMId").val();
    var hfsno = Crow.find("#SNohiddenfiled").val();
    var hdwhId = Crow.find("#wh_id"+hfsno).val();
    var AvlStk = Crow.find("#AvailableQuantity").val();
    var RwkJobOrdFlag = Crow.find("#hdn_RwkJobOrderFlag").val();
    if (RwkJobOrdFlag == "RWK") {
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, hdwhId, AvlStk, "Rwkwh", UOMID);

    }
    else {
        Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, hdwhId, AvlStk, "wh", UOMID);

    }
    //Cmn_SubItemWareHouseAvlStock(ProductNm, ProductId, UOM, hdwhId, AvlStk, "wh");

}

function ResetWorningBorderColor() {
    return Cmn_CheckValidations_forSubItems("MaterialIssueItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "SubItemIssueQty", "N");
}
function MI_SubItemList() {
    var NewArr = new Array();

    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
        var row = $(this);
        debugger;
        var Qty = row.find('#subItemQty').val();
        if (parseFloat(CheckNullNumber(Qty)) >0) {
        var List = {};
        List.item_id = row.find("#ItemId").val();
        List.sub_item_id = row.find('#subItemId').val();
        List.qty = row.find('#subItemQty').val();
        List.req_qty = row.find('#subItemReqQty').val();
        List.pend_qty = row.find('#subItemPendQty').val();
        List.avl_qty = row.find('#subItemAvlQty').val();
        NewArr.push(List);
        }
    });
    return NewArr;
}
/***--------------------------------Sub Item Section End-----------------------------------------***/
function onchangeIssuedate(el, evt) {
    debugger;
    var currentRows = $(evt.target).closest("tr");      
        var issuedt = currentRows.find("#IssueDate").val();
        if (issuedt == "" || issuedt == "1990-01-01") {
            currentRows.find("#spanIssuedate").text($("#valueReq").text());
            currentRows.find("#spanIssuedate").css("display", "block");
            currentRows.find("#IssueDate").css("border-color", "Red");
            return false;
        }
        else {
            currentRows.find("#spanIssuedate").css("display", "none");
            currentRows.find("#IssueDate").css("border-color", "#ced4da");
    }
    HideErrorMsg();
}

function onclickiconButton(e) {
    debugger;
  var issueto = $("#txtEntityType").val();
    var IssueTo_id = $("#hdissueto").val();
    var issue_type = $("#hdentitytype").val();
    var CurrentDate = moment().format('YYYY-MM-DD');
    var date = CurrentDate.split('-');
    var year = date[0];
    var dt = "01";
    var month = "01";
    $("#txtsFromdate").val(year + '-' + month + '-' + dt);
    $("#txtsFromdate").val()
    if (issueto != null && issueto != "") {
        $("#textIssueTo").val(issueto);
        $("#txtsTodate").val('');         
    }
    $("#txtsTodate").val(CurrentDate);
    //if ($("#datatable-buttons TBODY TR").length > 0) {
    //    $("#datatable-buttons TBODY TR").each(function () {
    //        var row = $(this);

    //        $(this).remove();

    //    });
    //}
    FilterIssuetoList();
}

function FilterIssuetoList() {
        debugger;
    try {
        if ($("#datatable-buttons TBODY TR").length > 0) {
            $("#datatable-buttons TBODY TR").each(function () {
                var row = $(this);

                $(this).remove();

            });
        }
        var issueto = $("#txtEntityType").val();
            var RequisitionTyp = "";
            var RequiredArea = "";
            var flag = "DetailPage";
            //var MaterialIssueTo = $("#ddlMaterialIssueTo option:selected").val();
             var Fromdate = $("#txtsFromdate").val();
            var Todate = $("#txtsTodate").val();
            var MaterialIssueTo = $("#hdissueto").val();          
            var Status = $("#ddlStatus option:selected").val();          
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/MaterialIssue/MaterialIssuetListSearch",
                data: {
                    RequisitionTyp: RequisitionTyp,
                    RequiredArea: RequiredArea,
                    MaterialIssueTo: MaterialIssueTo,
                    Fromdate: Fromdate,
                    Todate: Todate,
                    Status: Status,
                    flag: flag
                },
                success: function (data) {
                    debugger;
                    $('#issuetodatatable').html(data);
                    $('#txtsFromdate').val(Fromdate);
                    $('#txtsTodate').val(Todate);
                    $('#textIssueTo').val(issueto);
                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                }

            });
    }
    catch (err) {
            debugger;
            console.log("ItemSetup Error : " + err.message);

    }
}
function GetPendingDocument(flag) {
    debugger;
    var ItemID = $("#ddlItemName").val();
    var Item_name = $("#ddlItemName option:selected").text();
    var Docid = $("#DocumentMenuId").val();
    $("#datatable-buttons")[0].id = "dttbl1";

    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialIssue/GetPendingDocument",
            data: {
                Docid: Docid,
                ItemID: ItemID,
                flag: flag
            },
            success: function (data) {
                debugger;

                $('#MIPendingSourceDocument').html(data);
                $("#dttbl1")[0].id = "datatable-buttons";

               
                $('#ddlItemName').append('<option value="' + ItemID + '">' + Item_name + '</option>');
                $('#ddlItemName').val(ItemID).trigger('change');
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
                console.error("Error:", exception);
            }
        });
    } catch (err) {
        debugger;
        console.log("ItemSetup Error: " + err.message);
    }
}



function OnClickMrsItemDetail(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var doc_no = clickedrow.find("#Docno").text().trim();
    var doc_dt = clickedrow.find("#hdnDocdt").text().trim();    
    try {
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialIssue/GetPendingDocumentitemDetail",
            data: {
                doc_no: doc_no,
                doc_dt: doc_dt,
            },
            success: function (data) {
                $('#ItemInfoMIList1').html(data);

            },

        });
    }
        catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}

function onchangeCancelledRemarks() {
    debugger;
    
    var remrks1 = $("#Cancelledremarks").val().trim();
    $("#Cancelledremarks").text(remrks1);
    $("#Cancelledremarks").val(remrks1);
    $('#SpanCancelledRemarks').text("");
    $("#SpanCancelledRemarks").css("display", "none");
    $("#Cancelledremarks").css("border-color", "#ced4da");
}

function OnChangeItemName() {

    var ItemID = $("#ddlItemName").val();
    $("#hf_ItemID").val(ItemID);
}
function BindInternalIssueItmList() {
    debugger;
    DynamicSerchableItemDDL("", "#ddlItemName", "", "", "", "MRS");
}
function PendingDocumentBtnSearch() {
    GetPendingDocument("Search");
}
function onclickReturnflag() {

    if ($("#Returnable").is(":checked")) {
        $("#Returnable").val(true);
    }
    else {
        $("#Returnable").val(false);
    }
};