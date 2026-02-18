$(document).ready(function () {
    debugger;
    $("#fromWh").select2();
    $("#toWh").select2();
    $('#MaterialReceiptItemDetailsTbl').on('click', '.deleteIcon', function () {
        var child = $(this).closest('tr').nextAll();
        child.each(function () {
            var id = $(this).attr('id');
            var idx = $(this).children('.row-index').children('p');
            var dig = parseInt(id.substring(1));
            idx.html(`Row ${dig - 1}`);
            $(this).attr('id', `R${dig - 1}`);
        });
        $(this).closest('tr').remove();
        var MaterialIssueNo = $("#txtMaterialIssueNo").val();
        if (MaterialIssueNo == null || MaterialIssueNo == "") {
            if ($('#MaterialReceiptItemDetailsTbl tr').length <= 1) {
                debugger;
                $("#ddlMTR_No").prop("disabled", false);
                $("#ddlRequisitionTypeList").prop("disabled", false);
                $("#ddlRequiredArea").prop("disabled", false);
                $(".plus_icon1").css('display', 'block');
            }
        }
        updateItemSerialNumber()
    });

    //var now = new Date();
    //var month = (now.getMonth() + 1);
    //var day = now.getDate();
    //if (month < 10)
    //    month = "0" + month;
    //if (day < 10)
    //    day = "0" + day;
    //var today = now.getFullYear() + '-' + month + '-' + day;
    //$("#txtTodate").val(today);
    sessionStorage.removeItem("BatchDetailSession");
    sessionStorage.removeItem("SerialDetailSession");
    $("#datatable-buttons >tbody").bind("click", function (e) {
        var clickedrow = $(e.target).closest("tr");
        $("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
    });

    //$("#datatable-buttons >tbody").bind("dblclick", function (e) {
    //    debugger;
    //    try {
    //        var clickedrow = $(e.target).closest("tr");
    //        var IssueType = clickedrow.children("#Trf_Type").text();
    //        var IssueNumber = clickedrow.children("#MRNo").text();
    //        var IssueDate = clickedrow.children("#MR_Dt").text();
    //        if (IssueNumber != null && IssueNumber != "") {
    //            window.location.href = "/ApplicationLayer/MaterialIssue/EditMaterialIssue/?IssueType=" + IssueType + "&IssueNumber=" + IssueNumber + "&IssueDate=" + IssueDate;
    //        }

    //    }
    //    catch (err) {
    //        debugger
    //    }
    //});

    var Doc_no = $("#txtMaterialReceiptNo").val();
    $("#hdDoc_No").val(Doc_no);
});
function OnChangeCancelFlag() {
    debugger;
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return CheckFormValidation()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}
function updateItemSerialNumber() {
    debugger;
    var SerialNo = 0;
    $("#MaterialReceiptItemDetailsTbl >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        currentRow.find("#SpanRowId").text(SerialNo);

    });
};
function onChangeddlMTR_No() {
    debugger;
    var MTRDate = $("#ddlMTR_No").val();
    var MIDate = $("#ddlMI_No").val();
    var date1 = MIDate.split("-");
    var DFinal1 = date1[2] + "-" + date1[1] + "-" + date1[0];

    var date = MTRDate.split("-");
    var DFinal = date[2] + "-" + date[1] + "-" + date[0];
    var MTRNo = $("#ddlMTR_No option:selected").text();
    mrs_type = $("#ddlRequisitionTypeList").val();
    $("#txtMTR_Date").val(DFinal);
    $("#txtMI_Date").val(DFinal1);
    if (MTRDate != "0") {
        document.getElementById("vmMTR_No").innerHTML = null;
        $("#ddlMTR_No").css("border-color", "#ced4da");
        //$(".plus_icon1").css('display', 'block');
        BindddlMaterialIssueList();
    }
    else {
        document.getElementById("vmMTR_No").innerHTML = $("#valueReq").text();
        $("#ddlMTR_No").css("border-color", "red");
        //$(".plus_icon1").css('display', 'none');
        $("#txtMTR_Date").val("");
        $("#txtEntityType").val("");
    }

}
function onChangeddlMI_No() {
    debugger;
    var MIDate = $("#ddlMI_No").val();
    //var MTRDate = $('#txtMTR_Date').val();
    var date = MIDate.split("-");
    var DFinal = date[2] + "-" + date[1] + "-" + date[0];
    var MINo = $("#ddlMI_No option:selected").text();
    //$("#ddlMI_No").val(MINo);
    mrs_type = $("#ddlRequisitionTypeList").val();
    $("#txtMI_Date").val(DFinal);
    if (MIDate != "0") {
        document.getElementById("vmMI_No").innerHTML = null;
        $("#ddlMI_No").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-ddlMI_No-container"]').css("border-color", "#ced4da");
        $(".plus_icon1").css('display', 'block');

    }
    else {
        document.getElementById("vmMI_No").innerHTML = $("#valueReq").text();
        $("#ddlMI_No").css("border-color", "red");
        $('[aria-labelledby="select2-ddlMI_No-container"]').css("border-color", "red");
        $(".plus_icon1").css('display', 'none');
        $("#txtMI_Date").val("");
        $("#txtEntityType").val("");
    }

}
function BindddlMaterialList() {
    debugger;
    var FromWH = $('#fromWh').val();
    var ToWH = $('#toWh').val();
    var ToBR = $('#hfbr_ID').val();
    var FromBR = $("#Fr_branch").val();
    var TransferType = $('#TransferType').val();
    //$("#ddlMTR_No").select2({
    //    ajax: {
    //        url: $("#hdMaterialReceipt_Path").val(),
    //        data: function (params) {
    //            var queryParameters = {
    //                FilterMTRNo: params.term, // search term like "a" then "an"
    //                FilterSourceWH: FromWH,
    //                FilterToWH: ToWH,
    //                FilterFromBR: FromBR,
    //                FilterTransferType: TransferType
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
    //                $("#ddlMTR_No option").remove();
    //                $("#ddlMTR_No optgroup").remove();
    //                $('#ddlMTR_No').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
    //                for (var i = 0; i < arr.length; i++) {
    //                    $('#Textddl').append(`<option data-date="${arr[i].ID}" value="${arr[i].ID}">${arr[i].Name}</option>`);
    //                }
    //                var firstEmptySelect = true;
    //                $('#ddlMTR_No').select2({
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
    //            }

    //        }
    //    },

    //})

    $("#ddlMTR_No").select2({
        ajax: {
            url: $("#hdMaterialReceipt_Path").val(),
            data: function (params) {
                var queryParameters = {
                    FilterMTRNo: params.term, // search term like "a" then "an"
                    FilterSourceWH: FromWH,
                    FilterToWH: ToWH,
                    FilterFromBR: FromBR,
                    FilterTransferType: TransferType
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                debugger;
                arr = data;//JSON.parse(data);
                var pageSize,
                    pageSize = 20;
                if (arr.length > 0) {
                }
                var page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                    //results: data.slice((page - 1) * pageSize, page * pageSize),
                    //more: data.length >= page * pageSize,
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };

            }
        },
        templateResult: function (data) {
            var DocDate = $(data.element).data('date');
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result = $(
                '<div class="row">' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.id + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        }
    })
    document.getElementById("vmMTR_No").innerHTML = null;
    $("#ddlMTR_No").css("border-color", "#ced4da");
}
function BindddlMaterialIssueList() {
    debugger;
    var FromWH = $('#fromWh').val();
    var MTRNo = $("#ddlMTR_No option:selected").text();
    var MTRdate = $('#txtMTR_Date').val();
    var FromBR = $("#Fr_branch").val();

    $("#ddlMI_No").select2({
        ajax: {
            url: $("#hdMaterialIssue_Path").val(),
            data: function (params) {
                var queryParameters = {
                    FilterMTRNo: params.term, // search term like "a" then "an"
                    FilterSourceWH: FromWH,
                    FilterFromBR: FromBR,
                    MTR_No: MTRNo,
                    MTR_date: MTRdate,
                };
                return queryParameters;
            },
            dataType: "json",
            cache: true,
            delay: 250,
            //type: 'POST',
            contentType: "application/json; charset=utf-8",
            processResults: function (data, params) {
                arr = data;//JSON.parse(data);
                var pageSize,
                    pageSize = 20;
                if (arr.length > 0) {
                }
                var page = params.page || 1;
                if (arr.length > 0) {
                    //$("#ddlMI_No option").remove();
                    //$("#ddlMI_No optgroup").remove();
                    //$('#ddlMI_No').append(`<optgroup class='def-cursor' id="Textddl_MI" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                    //for (var i = 0; i < arr.length; i++) {
                    //    $('#Textddl_MI').append(`<option data-date="${arr[i].ID}" value="${arr[i].ID}">${arr[i].Name}</option>`);
                    //}
                    //var firstEmptySelect = true;
                    //$('#ddlMI_No').select2({
                    //    templateResult: function (data) {
                    //        var DocDate = $(data.element).data('date');
                    //        var classAttr = $(data.element).attr('class');
                    //        var hasClass = typeof classAttr != 'undefined';
                    //        classAttr = hasClass ? ' ' + classAttr : '';
                    //        var $result = $(
                    //            '<div class="row">' +
                    //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    //            '<div class="col-md-6 col-xs-6' + classAttr + '">' + DocDate + '</div>' +
                    //            '</div>'
                    //        );
                    //        return $result;
                    //        firstEmptySelect = false;
                    //    }
                    //});
                }
                var page = params.page || 1;
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID, text: val.Name };
                    }),
                    //results: data.slice((page - 1) * pageSize, page * pageSize),
                    //more: data.length >= page * pageSize,
                    pagination: {
                        more: (page * pageSize) < data.length
                    }
                };

            }
        },
        templateResult: function (data) {
            var DocDate = $(data.element).data('date');
            var classAttr = $(data.element).attr('class');
            var hasClass = typeof classAttr != 'undefined';
            classAttr = hasClass ? ' ' + classAttr : '';
            var $result = $(
                '<div class="row">' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.id + '</div>' +
                '</div>'
            );
            return $result;
            firstEmptySelect = false;
        }
        //$("#vmMI_No").innerHTML(null);




    });
    document.getElementById("vmMI_No").innerHTML = null;
    $("#ddlMI_No").css("border-color", "#ced4da");
}
function AddItemDetail() {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();///Quantity
    var ddlMI_No = $("#ddlMI_No").val();
    var ddlMTR_No = $("#ddlMTR_No").val();
    var span_SubItemDetail = $("#span_SubItemDetail").text();
    if (ddlMTR_No != 0 && ddlMI_No != 0) {

        $("#HdMTRNo").val($('#ddlMTR_No option:selected').text());
        $("#HdMINo").val($('#ddlMI_No option:selected').text());
        //$("#HdMTRDate").val($('#ddlMTR_No').val());
        $(".plus_icon1").css('display', 'none');
        debugger;
        $("#ddlMTR_No").prop("readonly", true);
        $("#txtMTR_Date").prop("readonly", true);
        $("#TransferType").prop("disabled", true);
        $("#fromWh").prop("disabled", true);
        $("#toWh").prop("disabled", true);
        $("#tobranch").prop("disabled", true);
        $("#ddlMTR_No").prop("disabled", true);
        $("#ddlMI_No").prop("disabled", true);
        $("#Fr_branch").prop("disabled", true);



        var TRFType = $('#TransferType').val();
        var MTRNo = $('#ddlMTR_No option:selected').text();
        var MTRDate = $('#txtMTR_Date').val();
        var MINo = $('#ddlMI_No option:selected').text();
        var MIDate = $('#txtMI_Date').val();
        var frombranch = $("#Fr_branch").val();
        debugger;
        $.ajax(
            {
                type: "Post",
                url: "/ApplicationLayer/MaterialTransferReceipt/GetMTRDetailByNumber",
                data: {
                    TRFType: TRFType,
                    TRFNo: MTRNo,
                    TRFDate: MTRDate,
                    MINo: MINo,
                    MIDate: MIDate,
                    frombranch: frombranch

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
                               

                                if (arr.Table[i].i_batch == 'Y') {
                                    Ibatch = `<td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" onchange="OnChangeIssueQty" class="calculator" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                              <input type="hidden" id="hdi_batch" value="Y" style="display: none;" /></td>`;
                                }
                                else {
                                    Ibatch = `<td class="center"><button type="button" onclick="ItemStockBatchWise(this,event)" disabled class="calculator subItmImg" data-toggle="modal" data-target="#BatchNumber" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_BatchDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_batch" value="N" style="display: none;" /></td>`;
                                }
                                if (arr.Table[i].i_serial == 'Y') {
                                    Iserial = `<td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" class="calculator " data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="Y" style="display: none;" /></td>`;
                                }
                                else {
                                    Iserial = `<td class="center"><button type="button" onclick="ItemStockSerialWise(this,event)" disabled class="calculator subItmImg" data-toggle="modal" data-target="#SerialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_SerialDetail").text()}"></i></button>
                                                <input type="hidden" id="hdi_serial" value="N" style="display: none;" /></td>`;
                                }
                                if (arr.Table[i].i_batch == 'N' && arr.Table[i].i_serial == 'N') {
                                    ItemType = `<td style="display:none"><input id="ItemType" type="text" value="YES" /></td>`;
                                }
                                else {
                                    ItemType = `<td style="display:none"><input id="ItemType" type="text" value="NO" /></td>`;
                                }

                                $('#MaterialReceiptItemDetailsTbl tbody').append(`
                                   <tr id="${++rowIdx}">
                                                                                                                           
                                                            <td class="sr_padding"><span id="SpanRowId"  value="${rowIdx}" >${rowIdx}</span> <input  type="hidden" id="SNohiddenfiled" value="${rowIdx}"</td>                                                        
                                                            <td>
                                                                <div class=" col-sm-11 no-padding">
                                                                    <input id="ItemName" value="${arr.Table[i].item_name}" class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                                                    <input type="hidden" id="hdItemId" value="${arr.Table[i].item_id}" style="display: none;" />
                                                                </div>
                                                                <div class="col-sm-1 i_Icon">
                                                                    <button type="button" onclick="OnClickIconBtn(event);" class="calculator" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/iIcon1.png" alt="" title="${$("#Span_ItemInformation_Title").text()}"> </button>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <input value="${arr.Table[i].uom_name}" id="UOM" class="form-control" autocomplete="off" type="text" name="UOM" placeholder="${$("#ItemUOM").text()}" disabled>
                                                                <input type="hidden" id="hdUOMId" value="${arr.Table[i].base_uom_id}" style="display: none;" />
                                                            </td>
                                                            <td>
                                                               <div class="col-sm-9 lpo_form no-padding">
                                                                <input id="RequisitionQuantity"  value="${parseFloat(arr.Table[i].req_qty).toFixed(QtyDecDigit)}" class="form-control num_right" autocomplete="off" type="text" name="RequisitionQuantity" placeholder="0000.00"  disabled>
                                                               </div>
                                                               <div class="col-sm-3 i_Icon " id="div_SubItemReqQty">
                                                                   <input hidden type="text" id="sub_item" value="${arr.Table[i].sub_item}" />
                                                                   <button type="button" id="SubItemReqQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('MTReciept',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                               </div>
                                                            </td>
                                                            <td>
                                                             <div class="col-sm-9 lpo_form no-padding">
                                                                <input id="PendingQuantity" value="${parseFloat(arr.Table[i].PendingQuantity).toFixed(QtyDecDigit)}"  class="form-control date num_right" autocomplete="off" type="text" name="PendingQuantity" placeholder="0000.00"  disabled>
                                                              </div>
                                                              <div class="col-sm-3 i_Icon no-padding" id="div_SubItemReqQty">
                                                                  <button type="button" id="SubItemReqQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('MTReciept',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                              </div>
                                                            </td>
                                                                <td>
                                                             <div class="col-sm-9 lpo_form no-padding">
                                                                    <input id="ReceiptQuantity" value="${parseFloat(arr.Table[i].rec_qty).toFixed(QtyDecDigit)}" class="form-control num_right" readonly="readonly" name="ReceiptQuantity" placeholder="30:00"  onblur="this.placeholder='30:00'" disabled="disabled">
                                                                    <span id="ReceiptQuantity_Error" class="error-message is-visible"></span>
                                                              </div>
                                                              <div class="col-sm-3 i_Icon no-padding" id="div_SubItemReqQty">
                                                                  <button type="button" id="SubItemReqQty" class="calculator subItmImg" ${subitmDisable} onclick="return SubItemDetailsPopUp('MTReciept',event)" data-toggle="modal" data-target="#SubItem" data-backdrop="static" data-keyboard="false"><img src="/Content/Images/subItem.png" alt="" title="${span_SubItemDetail}"> </button>
                                                              </div>
                                                            </td>
                                                           <td>
                                                               
                                                                    <div class="lpo_form">
                                                                     <input id="WHName" value="${arr.Table[i].wh_name}" class="form-control" autocomplete="off" type="text" name="OrderNumber" placeholder="${$("#ItemName").text()}"  onblur="this.placeholder='${$("#ItemName").text()}'" disabled>
                                                                      <input type="hidden" id="hdwhId" value="${arr.Table[i].wh_id}"  style="display: none;" />
                                                                     </div>
                                                                                                                                
                                                                </td>     
                                                           `
                                    + Ibatch + ``
                                    + Iserial + ``
                                    + ItemType + `
                                                           
                                                        </tr>
                                `);

                                BindWarehouseList(rowIdx);
                            }
                        }
                        if (arr.Table1.length > 0) {
                            for (var j = 0; j < arr.Table1.length; j++) {
                                $('#BatchDetailTbl tbody').append(`<tr>
<td align="right"><input type="hidden" id="hdItemId" value="${arr.Table1[j].item_id}" style="display: none;" /></td>
<td id="lot_id">${arr.Table1[j].lot_id}</td>
<td id="batch_no">${arr.Table1[j].batch_no}</td>
<td id="bt_mfg_name" >${arr.Table1[j].mfg_name}</td>
<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(arr.Table1[j].mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
<td id="bt_mfg_date" >${Cmn_FormatDate_ddmmyyyy(arr.Table1[j].mfg_date)}</td>
<td id="bt_mfg_hdn_date" hidden="hidden">${arr.Table1[j].mfg_date}</td>
<td id="exp_dt">${arr.Table1[j].exp_dt}</td>
<input type="hidden" id="hfExDate" value="${arr.Table1[j].exp_date}"/>
<td id="IssueQty" align="right">${arr.Table1[j].IssueQty}</td>
</tr>`);
                                
                            }
                            var BatchDetailList = [];
                            $("#BatchDetailTbl >tbody >tr").each(function () {
                                debugger;
                                var currentRow = $(this);
                                var ItemID = currentRow.find("#hdItemId").val();
                                var BatchQty = currentRow.find("#IssueQty").text();
                                var BatchNo = currentRow.find("#batch_no").text();
                                var lot_id = currentRow.find("#lot_id").text();
                                //var BatchExDate = currentRow.find("#exp_dt").text();
                                var BatchExpDate = currentRow.find("#hfExDate").val();
                                var mfg_name = currentRow.find("#bt_mfg_name").text();
                                var mfg_mrp = currentRow.find("#bt_mfg_mrp").text();
                                var mfg_date = currentRow.find("#bt_mfg_hdn_date").text();
                                if (BatchExpDate == "null") {
                                    BatchExpDate = "01-Jan-1900"
                                }
                                BatchDetailList.push({
                                    ItemID: ItemID, BatchQty: BatchQty, BatchNo: BatchNo, lot_id: lot_id, BatchExpDate: BatchExpDate
                                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                                })
                            });
                            sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
                        }
                        if (arr.Table2.length > 0) {
                            var SerialDetailList = [];

                            for (var k = 0; k < arr.Table2.length; k++) {
                                var RowSNo;


                                var SItmCode = arr.Table2[k].item_id;
                                var SSerialNo = arr.Table2[k].serial_no;
                                var serial_lot_id = arr.Table2[k].lot_id;
                                var mfg_name = arr.Table2[k].mfg_name;
                                var mfg_mrp = arr.Table2[k].mfg_mrp;
                                var mfg_date = arr.Table2[k].mfg_date;

                                SerialDetailList.push({
                                    ItemID: SItmCode, SerialNo: SSerialNo, serial_lot_id: serial_lot_id
                                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                                })
                            }
                            debugger;
                            sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
                        }
                        //if (arr.Table3.length > 0) {

                        //    for (var k = 0; k < arr.Table3.length; k++) {

                        //        $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
                        //        <td><input type="text" id="ItemId" value='${arr.Table3[k].item_id}'></td>
                        //        <td><input type="text" id="subItemId" value='${arr.Table3[k].sub_item_id}'></td>
                        //        <td><input type="text" id="subItemQty" value='${arr.Table3[k].Qty}'></td>
                        //        <td><input type="text" id="subItemReqQty" value='${arr.Table3[k].req_qty}'></td>
                        //        <td><input type="text" id="subItemPendQty" value='${arr.Table3[k].pend_qty}'></td>
                        //    </tr>`);

                        //    }

                        //}
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                    //   alert("some error");
                }
            });
    }
    else {
        if (ddlMTR_No == 0) {
            document.getElementById("vmMTR_No").innerHTML = $("#valueReq").text();
            $("#ddlMTR_No").css("border-color", "red");
            $('[aria-labelledby="select2-ddlMTR_No-container"]').css("border-color", "red");
        }
        if (ddlMI_No == 0) {
            document.getElementById("vmMI_No").innerHTML = $("#valueReq").text();
            $("#ddlMI_No").css("border-color", "red");
            $('[aria-labelledby="select2-ddlMI_No-container"]').css("border-color", "red");
        }
    }

}
function BindWarehouseList(id) {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/GRNDetail/GetWarehouseList",
            data: {},
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
                        var s = '<option value="0">---Select---</option>';
                        for (var i = 0; i < arr.Table.length; i++) {
                            s += '<option value="' + arr.Table[i].wh_id + '">' + arr.Table[i].wh_name + '</option>';
                        }
                        $("#wh_id" + id).html(s);
                    }
                }
            },
        });
}
function ItemStockWareHouseWise(el, evt) {
    try {
        debugger;
        BindItemBatchDetailMTR();
        var clickedrow = $(evt.target).closest("tr");
        var ItemId = clickedrow.find("#hdItemId").val();;
        var CompId = $("#HdCompId").val();
        var BranchId = $("#HdBranchId").val();
        var ItemId = clickedrow.find("#hdItemId").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();

        $("#WareHouseWiseItemName").val(ItemName);
        $("#WareHouseWiseUOM").val(UOMName);
        $.ajax(
            {

                type: "Post",
                url: "/ApplicationLayer/Shipment/getItemstockWareHouselWise",
                data: {
                    ItemId: ItemId,
                    CompId: CompId,
                    BranchId: BranchId
                },
                success: function (data) {
                    debugger;
                    $('#ItemStockWareHouseWise').html(data);
                },
            });

    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function OnClickIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = clickedrow.find("#hdItemId").val();
    ItemInfoBtnClick(ItmCode);

}
function BindItemBatchDetailMTR() {
    debugger;
    //var ItemID = $('#hfItemID').val();
    var GRNItemsBatchDetail = [];

    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    if (FBatchDetails == null) {
        var BatchDetailList = [];
        $("#BatchDetailTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var ItemID = currentRow.find("#hdItemId").val();
            var BatchQty = currentRow.find("#IssueQty").text();
            var BatchNo = currentRow.find("#batch_no").text();
            var lot_id = currentRow.find("#lot_id").text();
            var BatchExpDate = currentRow.find("#hfExDate").val();
            var mfg_name = currentRow.find("#bt_mfg_name").text();
            var mfg_mrp = currentRow.find("#bt_mfg_mrp").text();
            var mfg_date = currentRow.find("#bt_mfg_hdn_date").text();
            if (BatchExpDate == null) {
                BatchExpDate = "01-Jan-1900"
            }

            BatchDetailList.push({
                ItemID: ItemID, BatchQty: BatchQty, BatchNo: BatchNo, lot_id: lot_id, BatchExpDate: BatchExpDate
                , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
            })
        });
        sessionStorage.removeItem("BatchDetailSession");
        sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));
    }
    var FBatchDetails1 = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
    if (FBatchDetails1 != null) {
        if (FBatchDetails1.length > 0) {
            for (i = 0; i < FBatchDetails1.length; i++) {
                var ItemID = FBatchDetails1[i].ItemID;
                var BatchNo = FBatchDetails1[i].BatchNo;
                var lot_id = FBatchDetails1[i].lot_id;
                var BatchQty = FBatchDetails1[i].BatchQty;
                var BatchExDate = FBatchDetails1[i].BatchExDate;
                var BatchExpDate = FBatchDetails1[i].BatchExpDate;
                var mfg_name = FBatchDetails1[i].mfg_name;
                var mfg_mrp = FBatchDetails1[i].mfg_mrp;
                var mfg_date = FBatchDetails1[i].mfg_date;

                GRNItemsBatchDetail.push({ item_id: ItemID, batch_no: BatchNo, lot_id: lot_id, batch_qty: BatchQty, exp_dt: BatchExDate, exp_date: BatchExpDate, mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date});
            }
        }
    }
    var str1 = JSON.stringify(GRNItemsBatchDetail);
    $("#HDSelectedBatchwise").val(str1);
}

function QtyFloatValueonly(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    if (charCode == 46 && el.value.indexOf(".") !== -1) {
        return false;
    }
    var QtyDecDigit = $("#QtyDigit").text();
    var number = el.value.split('.');
    if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
        return false;
    }

    return true;
}
function BindItemSerialDetailMTR() {
    debugger;
    var GRNItemsSerialDetail = [];

    var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    if (FSerialDetails == null) {
        var SerialDetailList = [];
        $("#SerialDetailTbl >tbody >tr").each(function () {
            debugger;
            var currentRow = $(this);
            var ItemID = currentRow.find("#hdsrItemId").val();
            var SerialNo = currentRow.find("#serial_no_").text();
            var serial_lot_id = currentRow.find("#serial_lot_id").text();
            var mfg_name = currentRow.find("#sr_mfg_name").text();
            var mfg_mrp = currentRow.find("#sr_mfg_mrp").text();
            var mfg_date = currentRow.find("#sr_mfg_hdn_date").text();

            SerialDetailList.push({
                ItemID: ItemID, SerialNo: SerialNo, serial_lot_id: serial_lot_id
                , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
            })
        });
        sessionStorage.removeItem("SerialDetailSession");
        sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));
    }
    var FSerialDetails1 = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
    if (FSerialDetails1 != null) {
        if (FSerialDetails1.length > 0) {
            for (i = 0; i < FSerialDetails1.length; i++) {
                var ItemID = FSerialDetails1[i].ItemID;
                var SerialNo = FSerialDetails1[i].SerialNo;
                var serial_lot_id = FSerialDetails1[i].serial_lot_id;
                var mfg_name = FSerialDetails1[i].mfg_name;
                var mfg_mrp = FSerialDetails1[i].mfg_mrp;
                var mfg_date = FSerialDetails1[i].mfg_date;
                GRNItemsSerialDetail.push({ ItemID: ItemID, SerialNo: SerialNo, serial_lot_id: serial_lot_id, mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date });
            }
        }
    }
    var str1 = JSON.stringify(GRNItemsSerialDetail);
    $("#HDSelectedSerialwise").val(str1);
}
function ItemStockBatchWise(el, evt) {
    //  Cmn_ItemStockBatchWise(evt, "#ItemName", "#UOM", "#ReceiptQuantity", "#hdItemId");
    try {
        debugger;
        var QtyDecDigit = $("#QtyDigit").text();///Quantity
        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ReceiptQuantity = clickedrow.find("#ReceiptQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        //var UOMId = clickedrow.find("#hdUOMId").val();
        //var TRFType = $('#TransferType').val();
        //var MINo = $('#ddlMI_No option:selected').text();
        //var MIDate = $('#txtMI_Date').val();
        //var frombranch = $("#Fr_branch").val();
        $("#ItemNameBatchWise").val(ItemName);
        $("#UOMBatchWise").val(UOMName);
        $("#BatchReceivedQuantity").val(ReceiptQuantity);
        if (parseFloat(IssueQuantity) == "0" || parseFloat(IssueQuantity) == "") {
            $("#BatchNumber").css("display", "block");
            clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            clickedrow.find("#IssuedQuantity_Error").css("display", "block");
            clickedrow.find("#IssuedQuantity").css("border-color", "red");
        }
        else {
            var Mrs_IssueType = $("#TransferType").val();
            var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
            if (FBatchDetails != null) {
                if (FBatchDetails.length > 0) {
                    $("#BatchDetailTbl >tbody >tr").remove();
                    for (i = 0; i < FBatchDetails.length; i++) {
                        var SItemID = FBatchDetails[i].ItemID;
                        if (SItemID == ItemId) {
                            var date = moment(FBatchDetails[i].BatchExpDate).format('DD-MM-YYYY');
                            if (date == "01-01-1900") {
                                date = "";
                            }
                            $('#BatchDetailTbl tbody').append(`<tr> 
<input type="hidden" id="hdItemId" value="${FBatchDetails[i].ItemID}" style="display: none;"/>
<td id="lot_id">${FBatchDetails[i].lot_id}</td>
<td id="batch_no">${FBatchDetails[i].BatchNo}</td>
<td id="bt_mfg_name" >${FBatchDetails[i].mfg_name}</td>
<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(FBatchDetails[i].mfg_mrp)).toFixed(QtyDecDigit)}</td>
<td id="bt_mfg_date" >${Cmn_FormatDate_ddmmyyyy(FBatchDetails[i].mfg_date)}</td>
<td id="bt_mfg_hdn_date" hidden="hidden">${FBatchDetails[i].mfg_date}</td>
<td id="exp_dt">${date}</td>
<input type="hidden" id="hfExDate" value="${FBatchDetails[i].BatchExpDate}"/>
<td id="IssueQty" align="right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
</tr>`);
                        }
                    }
                }
                else {
                    var BatchDetailList = [];
                    $("#BatchDetailTbl >tbody >tr").each(function () {
                        debugger;
                        var currentRow = $(this);
                        var ItemID = currentRow.find("#hdItemId").val();
                        var BatchQty = currentRow.find("#IssueQty").text();
                        var BatchNo = currentRow.find("#batch_no").text();
                        var lot_id = currentRow.find("#lot_id").text();
                        var BatchExpDate = currentRow.find("#hfExDate").val();
                        var mfg_name = currentRow.find("#bt_mfg_name").text();
                        var mfg_mrp = currentRow.find("#bt_mfg_mrp").text();
                        var mfg_date = currentRow.find("#bt_mfg_hdn_date").text();
                        if (BatchExpDate == null) {
                            BatchExpDate = "01-Jan-1900"
                        }

                        BatchDetailList.push({
                            ItemID: ItemID, BatchQty: BatchQty, BatchNo: BatchNo, lot_id: lot_id, BatchExpDate: BatchExpDate
                            , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                        })
                    });
                    sessionStorage.removeItem("BatchDetailSession");
                    sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));

                    var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
                    if (FBatchDetails != null) {
                        if (FBatchDetails.length > 0) {
                            $("#BatchDetailTbl >tbody >tr").remove();
                            for (i = 0; i < FBatchDetails.length; i++) {
                                var SItemID = FBatchDetails[i].ItemID;
                                if (SItemID == ItemId) {
                                    var date = moment(FBatchDetails[i].BatchExpDate).format('DD-MM-YYYY');
                                    if (date == "01-01-1900") {
                                        date = "";
                                    }
                                    $('#BatchDetailTbl tbody').append(`<tr> 
<input type="hidden" id="hdItemId" value="${FBatchDetails[i].ItemID}" style="display: none;"/>
<td id="lot_id">${FBatchDetails[i].lot_id}</td>
<td id="batch_no">${FBatchDetails[i].BatchNo}</td>
<td id="bt_mfg_name" >${FBatchDetails[i].mfg_name}</td>
<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(FBatchDetails[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
<td id="bt_mfg_date" >${Cmn_FormatDate_ddmmyyyy(FBatchDetails[i].mfg_date)}</td>
<td id="bt_mfg_hdn_date" hidden="hidden">${FBatchDetails[i].mfg_date}</td>
<td id="exp_dt">${date}</td>
<input type="hidden" id="hfExDate" value="${FBatchDetails[i].BatchExpDate}"/>
<td id="IssueQty" align="right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
</tr>`);
                                }
                            }
                        }
                    }
                }
            }
            else {
                var BatchDetailList = [];
                $("#BatchDetailTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var ItemID = currentRow.find("#hdItemId").val();
                    var BatchQty = currentRow.find("#IssueQty").text();
                    var BatchNo = currentRow.find("#batch_no").text();
                    var lot_id = currentRow.find("#lot_id").text();
                    var BatchExpDate = currentRow.find("#hfExDate").val();
                    var mfg_name = currentRow.find("#bt_mfg_name").text();
                    var mfg_mrp = currentRow.find("#bt_mfg_mrp").text();
                    var mfg_date = currentRow.find("#bt_mfg_hdn_date").text();
                    if (BatchExpDate == "null") {
                        BatchExpDate = "01-Jan-1900"
                    }

                    BatchDetailList.push({
                        ItemID: ItemID, BatchQty: BatchQty, BatchNo: BatchNo, lot_id: lot_id, BatchExpDate: BatchExpDate
                        , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                    })
                });
                sessionStorage.removeItem("BatchDetailSession");
                sessionStorage.setItem("BatchDetailSession", JSON.stringify(BatchDetailList));

                var FBatchDetails = JSON.parse(sessionStorage.getItem("BatchDetailSession"));
                if (FBatchDetails != null) {
                    if (FBatchDetails.length > 0) {
                        $("#BatchDetailTbl >tbody >tr").remove();
                        for (i = 0; i < FBatchDetails.length; i++) {
                            var SItemID = FBatchDetails[i].ItemID;
                            if (SItemID == ItemId) {
                                var date = moment(FBatchDetails[i].BatchExpDate).format('DD-MM-YYYY');
                                if (date == "01-01-1900") {
                                    date = "";
                                }
                                $('#BatchDetailTbl tbody').append(`<tr> 
<input type="hidden" id="hdItemId" value="${FBatchDetails[i].ItemID}" style="display: none;"/>
<td id="lot_id">${FBatchDetails[i].lot_id}</td>
<td id="batch_no">${FBatchDetails[i].BatchNo}</td>
<td id="bt_mfg_name" >${FBatchDetails[i].mfg_name}</td>
<td id="bt_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(FBatchDetails[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
<td id="bt_mfg_date" >${Cmn_FormatDate_ddmmyyyy(FBatchDetails[i].mfg_date)}</td>
<td id="bt_mfg_hdn_date" hidden="hidden">${FBatchDetails[i].mfg_date}</td>
<td id="exp_dt">${date}</td>
<input type="hidden" id="hfExDate" value="${FBatchDetails[i].BatchExpDate}"/>
<td id="IssueQty" align="right">${parseFloat(FBatchDetails[i].BatchQty).toFixed(QtyDecDigit)}</td>
</tr>`);
                            }
                        }
                    }
                }
            }

        }
    } catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
function ItemStockSerialWise(el, evt) {
    try {
        debugger;

        var clickedrow = $(evt.target).closest("tr");
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();

        var ItemName = clickedrow.find("#ItemName").val();
        var UOMName = clickedrow.find("#UOM").val();
        var ItemId = clickedrow.find("#hdItemId").val();;
        var UOMID = clickedrow.find("#hdUOMId").val();
        var ReceiptQuantity = clickedrow.find("#ReceiptQuantity").val();
        $("#SerialReceivedQuantity").val(ReceiptQuantity);
        $("#serialUOM").val(UOMName);
        $("#SerialItemName").val(ItemName);


        var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
        if (FSerialDetails != null) {
            if (FSerialDetails.length > 0) {
                $("#SerialDetailTbl >tbody >tr").remove();
                for (i = 0; i < FSerialDetails.length; i++) {
                    var SItemID = FSerialDetails[i].ItemID;
                    if (SItemID == ItemId) {
                        $('#SerialDetailTbl tbody').append(`<tr> 
<td>${i + 1}</td>
<td id="serial_lot_id">${FSerialDetails[i].serial_lot_id}</td>
<td id="serial_no_">${FSerialDetails[i].SerialNo}</td>
<td style="display:none"><input type="hidden" id="hdsrItemId" value="${FSerialDetails[i].ItemID}" style="display: none;"/></td>
<td id="sr_mfg_name" >${FSerialDetails[i].mfg_name}</td>
<td id="sr_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(FSerialDetails[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
<td id="sr_mfg_date" >${Cmn_FormatDate_ddmmyyyy(FSerialDetails[i].mfg_date)}</td>
<td id="sr_mfg_hdn_date" hidden="hidden">${FSerialDetails[i].mfg_date}</td>
</tr>`);
                    }
                }
            }
            else {
                var SerialDetailList = [];
                $("#SerialDetailTbl >tbody >tr").each(function () {
                    debugger;
                    var currentRow = $(this);
                    var ItemID = currentRow.find("#hdsrItemId").val();
                    var SerialNo = currentRow.find("#serial_no_").text();
                    var serial_lot_id = currentRow.find("#serial_lot_id").text();
                    var mfg_name = currentRow.find("#sr_mfg_name").text();
                    var mfg_mrp = currentRow.find("#sr_mfg_mrp").text();
                    var mfg_date = currentRow.find("#sr_mfg_hdn_date").text();

                    SerialDetailList.push({
                        ItemID: ItemID, SerialNo: SerialNo, serial_lot_id: serial_lot_id
                        , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                    })
                });
                sessionStorage.removeItem("SerialDetailSession");
                sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));

                var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
                if (FSerialDetails != null) {
                    if (FSerialDetails.length > 0) {
                        $("#SerialDetailTbl >tbody >tr").remove();
                        for (i = 0; i < FSerialDetails.length; i++) {
                            var SItemID = FSerialDetails[i].ItemID;
                            if (SItemID == ItemId) {
                                $('#SerialDetailTbl tbody').append(`<tr> 
<td>${i + 1}</td>
<td id="serial_lot_id">${FSerialDetails[i].serial_lot_id}</td>
<td id="serial_no_">${FSerialDetails[i].SerialNo}</td>
<td style="display:none"><input type="hidden" id="hdsrItemId" value="${FSerialDetails[i].ItemID}" style="display: none;"/></td>
<td id="sr_mfg_name" >${FSerialDetails[i].mfg_name}</td>
<td id="sr_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(FSerialDetails[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
<td id="sr_mfg_date" >${Cmn_FormatDate_ddmmyyyy(FSerialDetails[i].mfg_date)}</td>
<td id="sr_mfg_hdn_date" hidden="hidden">${FSerialDetails[i].mfg_date}</td>
</tr>`);
                            }
                        }
                    }
                }
            }
        }
        else {
            var SerialDetailList = [];
            $("#SerialDetailTbl >tbody >tr").each(function () {
                debugger;
                var currentRow = $(this);
                var ItemID = currentRow.find("#hdsrItemId").val();
                var SerialNo = currentRow.find("#serial_no_").text();
                var serial_lot_id = currentRow.find("#serial_lot_id").text();
                var mfg_name = currentRow.find("#sr_mfg_name").text();
                var mfg_mrp = currentRow.find("#sr_mfg_mrp").text();
                var mfg_date = currentRow.find("#sr_mfg_hdn_date").text();
                SerialDetailList.push({
                    ItemID: ItemID, SerialNo: SerialNo, serial_lot_id: serial_lot_id
                    , mfg_name: mfg_name, mfg_mrp: mfg_mrp, mfg_date: mfg_date
                })
            });
            sessionStorage.removeItem("SerialDetailSession");
            sessionStorage.setItem("SerialDetailSession", JSON.stringify(SerialDetailList));

            var FSerialDetails = JSON.parse(sessionStorage.getItem("SerialDetailSession"));
            if (FSerialDetails != null) {
                if (FSerialDetails.length > 0) {
                    $("#SerialDetailTbl >tbody >tr").remove();
                    for (i = 0; i < FSerialDetails.length; i++) {
                        var SItemID = FSerialDetails[i].ItemID;
                        if (SItemID == ItemId) {
                            $('#SerialDetailTbl tbody').append(`<tr> 
<td>${i + 1}</td>
<td id="serial_lot_id">${FSerialDetails[i].serial_lot_id}</td>
<td>${FSerialDetails[i].SerialNo}</td>
<td style="display:none"><input type="hidden" id="hdsrItemId" value="${FSerialDetails[i].ItemID}" style="display: none;"/></td>
<td id="sr_mfg_name" >${FSerialDetails[i].mfg_name}</td>
<td id="sr_mfg_mrp" class="num_right">${parseFloat(CheckNullNumber(FSerialDetails[i].mfg_mrp)).toFixed(cmn_RateDecDigit)}</td>
<td id="sr_mfg_date" >${Cmn_FormatDate_ddmmyyyy(FSerialDetails[i].mfg_date)}</td>
<td id="sr_mfg_hdn_date" hidden="hidden">${FSerialDetails[i].mfg_date}</td>
</tr>`);
                        }
                    }
                }
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
                clickedrow.find("#IssuedQuantity").val(test);
            }
        }

        $("#MI_ItemBatchTbl TBODY TR").each(function () {
            var row = $(this);
            var Issueqty = row.find("#IssuedQuantity").val();
            if (Issueqty != "" && Issueqty != null) {
                TotalIssueQty = parseFloat(TotalIssueQty) + parseFloat(Issueqty);
            }
            debugger;
        });

        $("#MI_TotalIssuedQuantity").val(parseFloat(TotalIssueQty).toFixed(QtyDecDigit));
    }
    catch (err) {
        console.log("UserValidate Error : " + err.message);
    }
}
//function onclickbtnItemBatchReset() {
//    $("#MI_ItemBatchTbl TBODY TR").each(function () {
//        var row = $(this);
//        row.find("#IssuedQuantity").val("");
//    });
//    $("#MI_TotalIssuedQuantity").val("");
//}
function onclickbtnItemBatchSaveAndExit() {
    debugger
    var IsuueFlag = true;
    var ItemMI_Qty = $("#ShippedQuantityBatchWise").val();
    var ItemTotalIssuedQuantity = $("#MI_TotalIssuedQuantity").val();
    $("#MI_ItemBatchTbl TBODY TR").each(function () {
        var row = $(this);
        var AvailableQuantity = row.find("#AvailableQuantity").val();
        var IssuedQuantity = row.find("#IssuedQuantity").val();
        if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
            row.find("#IssuedQuantity_Error").text($("#BatchIssuedQtyGreaterthanAvaiQty").text());
            row.find("#IssuedQuantity_Error").css("display", "block");
            row.find("#IssuedQuantity").css("border-color", "red");
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

            $("#MI_ItemBatchTbl TBODY TR").each(function () {
                debugger;
                var row = $(this);
                var ItemIssueQuantity = row.find("#IssuedQuantity").val();
                if (ItemIssueQuantity != "" && ItemIssueQuantity != null) {

                    var ItemUOMID = $("#HDUOMBatchWise").val();
                    var ItemId = $("#HDItemNameBatchWise").val();
                    var ItemBatchNo = row.find("#BatchNumber").val();
                    //var ItemExpiryDate = row.find("#ExpiryDate").val();
                    var ItemExpiryDate = row.find("#hfExDate").val();
                    var AvailableQty = row.find("#AvailableQuantity").val();
                    var LotNo = row.find("#Lot").val();
                    $('#SaveItemBatchTbl tbody').append(
                        `<tr>
                    <td><input type="text" id="mi_lineBatchLotNo" value="${LotNo}" /></td>
                    <td><input type="text" id="mi_lineBatchItemId" value="${ItemId}" /></td>
                    <td><input type="text" id="mi_lineBatchUOMId" value="${ItemUOMID}" /></td>
                    <td><input type="text" id="mi_lineBatchBatchNo" value="${ItemBatchNo}" /></td>
                    <td><input type="text" id="mi_lineBatchIssueQty" value="${ItemIssueQuantity}" /></td>
                    <td><input type="text" id="mi_lineBatchExpiryDate" value="${ItemExpiryDate}" /></td>
                    <td><input type="text" id="mi_lineBatchavl_batch_qty" value="${AvailableQty}" /></td>
                </tr>`
                    );
                }
            });
            $("#BatchNumber").modal('hide');
        }
        $("#MaterialReceiptItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
            }
        });
    }
}
function onchangeChkItemSerialWise() {
    var TotalIssueLot = 0;
    $("#mi_ItemSerialTbl TBODY TR").each(function () {
        var row = $(this);
        if (row.find("#ChkItemSerialWise").is(":checked")) {
            TotalIssueLot = parseFloat(TotalIssueLot) + 1;
        }
    });
    $("#TotalIssuedSerial").val(parseFloat(TotalIssueLot));
    // localStorage.setItem('BatchResetFlag', 'True');
}
function onclickbtnItemSerialReset() {
    $("#mi_ItemSerialTbl TBODY TR").each(function () {
        var row = $(this);
        row.find("#ChkItemSerialWise").attr("checked", false);
    });
    $("#TotalIssuedSerial").val("");
}
function onclickbtnItemSerialSaveAndExit() {
    debugger;
    var ItemMI_Qty = $("#ShippedQuantitySerialWise").val();
    var ItemTotalIssuedQuantity = $("#TotalIssuedSerial").val();
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
        $("#mi_ItemSerialTbl TBODY TR").each(function () {
            var row = $(this);

            var ItemUOMID = $("#HDUOMIDSerialWise").val();
            var ItemId = $("#HDItemIDSerialWise").val();
            var ItemLOTNO = row.find("#Lot").val();
            var ItemIssueQuantity = "1";
            var ItemSerialNO = row.find("#SerialNumberDetail").val();
            if (row.find("#ChkItemSerialWise").is(":checked")) {
                $('#SaveItemSerialTbl tbody').append(
                    `
            <tr>
            <td><input type="text" id="mi_lineSerialItemId" value="${ItemId}" /></td>
            <td><input type="text" id="mi_lineSerialUOMId" value="${ItemUOMID}" /></td>
            <td><input type="text" id="mi_lineSerialLOTNo" value="${ItemLOTNO}" /></td>
            <td><input type="text" id="mi_lineSerialIssueQty" value="${ItemIssueQuantity}" /></td>
            <td><input type="text" id="mi_lineBatchSerialNO" value="${ItemSerialNO}" /></td>
            </tr>
            `
                );
            }
        });
        $("#SerialDetail").modal('hide');

        $("#MaterialReceiptItemDetailsTbl >tbody >tr").each(function () {
            debugger;
            var clickedrow = $(this);
            var ItemId = clickedrow.find("#hdItemId").val();
            if (ItemId == SelectedItem) {
                clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
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
    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }

    debugger;
    var rowcount = $('#MaterialReceiptItemDetailsTbl tr').length;
    var ValidationFlag = true;
    var TransferType = $('#TransferType').val();
    var fromwh = $('#fromWh').val();
    var towh = $('#toWh').val();
    var Frbranch = $("#Fr_branch").val();
    var MTR_No = $('#ddlMTR_No').val();
    var MI_No = $('#ddlMI_No').val();


    $("#hdtrfType").val(TransferType);
    $("#hdWh_ID").val(fromwh);
    $("#hdWhID").val(towh);
    $("#hdBRID").val(Frbranch);

    var PR_number = $('#ddlMTR_No option:selected').text();
    var PR_date = $('#ddlMTR_No option:selected').val();
    var Mi_Number = $('#ddlMI_No option:selected').text();
    var Mi_Date = $('#ddlMI_No option:selected').val();
    var tobranch = $('#tobranch').val().trim();

    $("#hdReq_No").val(PR_number);
    $("#hdReq_Dt").val(PR_date);
    $("#hdIssue_No").val(Mi_Number);
    $("#hdIssue_dt").val(Mi_Date);
    $("#hdto_bridName").val(tobranch);


    //$('#hdMrsType').val(MRS_Type);   

    if (TransferType == "" || TransferType == "0") {
        document.getElementById("vmTransferType").innerHTML = $("#valueReq").text();
        $("#TransferType").css("border-color", "red");
        ValidationFlag = false;
    }
    if (fromwh == "" || fromwh == "0") {
        $('#spanFromWh').text($("#valueReq").text());
        $("#spanFromWh").css("display", "block");
        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "red");
        //document.getElementById("vmwarehouse").innerHTML = $("#valueReq").text();
        //$("#fromWh").css("border-color", "red");
        ValidationFlag = false;
    }
    if (towh == "" || towh == "0") {
        $('#spantoWh').text($("#valueReq").text());
        $("#spantoWh").css("display", "block");
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "red");
        //document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        //$("#toWh").css("border-color", "red");
        ValidationFlag = false;
    }
    if (Frbranch == "" || Frbranch == "0") {
        document.getElementById("vmbranch").innerHTML = $("#valueReq").text();
        $("#Fr_branch").css("border-color", "red");
        ValidationFlag = false;
    }
    if (MTR_No == "" || MTR_No == "0") {
        document.getElementById("vmMTR_No").innerHTML = $("#valueReq").text();
        $("#ddlMTR_No").css("border-color", "red");
        $(".select2-container--default .select2-selection--single").css("border-color", "red");
        ValidationFlag = false;
    }
    if (MI_No == "" || MI_No == "0") {
        document.getElementById("vmMI_No").innerHTML = $("#valueReq").text();
        $("#ddlMI_No").css("border-color", "red");
        $(".select2-container--default .select2-selection--single").css("border-color", "red");
        ValidationFlag = false;
    }
    //var MRS_Status = $('#hdMTRStatus').val().trim();

    //if (MRS_Status == "I") {
    //    return true;
    //}
    if (ValidationFlag == true) {

        /*----- Attatchment start--------*/
        $(".fileinput-upload").click();/*To Upload Img in folder*/
        FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();

        var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
        $('#hdn_Attatchment_details').val(ItemAttchmentDt);
        /*----- Attatchment End--------*/
        if (rowcount > 1) {

            //var flag = CheckMI_mentItemValidations();
            //if (flag == false) {
            //    return false;
            //}
            //var Batchflag = CheckItemBatchValidation();
            //if (Batchflag == false) {
            //    return false;
            //}
            //var SerialFlag = CheckItemSerialValidation();
            //if (SerialFlag == false) {
            //    return false;
            //}

            //if (flag == true && Batchflag == true && SerialFlag == true) {
            debugger;
            var MTRNo = $("#ddlMTR_No option:selected").text();
            $("#ddlMTR_No").val(MTRNo);

            var MI_NO = $("#ddlMI_No option:selected").text();
            $("#ddlMI_No").val(MI_NO);

            var Mr_dt = $("hdmr_dt").val();
            $("#txtship_dt").val(Mr_dt);

            debugger;

            var ItemDetailList = new Array();
            $("#MaterialReceiptItemDetailsTbl TBODY TR").each(function () {
                var row = $(this);

                var ItemList = {};

                ItemList.ItemName = row.find("#ItemName").val();
                ItemList.ItemId = row.find("#hdItemId").val();
                ItemList.UOM = row.find("#UOM").val();
                ItemList.UOMId = row.find('#hdUOMId').val();
                ItemList.WareHouseId = towh;
                ItemList.mrs_qty = row.find('#RequisitionQuantity').val();
                ItemList.pend_qty = row.find("#PendingQuantity").val();
                ItemList.rec_qty = row.find('#ReceiptQuantity').val();
                ItemList.WHName = row.find("#WHName").val()
                ItemList.sub_item = row.find("#sub_item").val()
                ItemList.i_batch = row.find("#hdi_batch").val()
                ItemList.i_serial = row.find("#hdi_serial").val()

                ItemDetailList.push(ItemList);
                debugger;
            });
            var str = JSON.stringify(ItemDetailList);
            $('#hdItemDetailList').val(str);
            BindItemBatchDetailMTR();
            BindItemSerialDetailMTR();
            $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            return true;
            //}
            //else {
            //    return false;
            //}
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
function CheckItemBatchValidation() {
    debugger;
    var ErrorFlag = "N";
    var BatchableFlag = "N";
    var EmptyFlag = "";
    var QtyDecDigit = $("#QtyDigit").text();
    $("#MaterialReceiptItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Batchable = clickedrow.find("#hdi_batch").val();

        if (Batchable == "Y") {
            var TotalItemBatchQty = parseFloat("0");
            if (Batchable == "Y" && $("#SaveItemBatchTbl >tbody >tr").length == 0) {
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
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
                    clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                }
                else {
                    clickedrow.find("#IssuedQuantity").css("border-color", "red");
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
    $("#MaterialReceiptItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var clickedrow = $(this);
        var IssueQuantity = clickedrow.find("#IssuedQuantity").val();
        var ItemId = clickedrow.find("#hdItemId").val();
        var UOMId = clickedrow.find("#hdUOMId").val();
        var Serialable = clickedrow.find("#hdi_serial").val();

        if (Serialable == "Y") {
            var TotalItemSerialQty = parseFloat("0");
            if (Serialable == "Y" && $("#SaveItemSerialTbl >tbody >tr").length == 0) {
                clickedrow.find("#IssuedQuantity").css("border-color", "red");
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
                    clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
                }
                else {
                    clickedrow.find("#IssuedQuantity").css("border-color", "red");
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
        clickedrow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
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
        clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
    }
    if (parseFloat(IssuedQuantity) > parseFloat(AvailableQuantity)) {
        debugger;
        clickedrow.find("#IssuedQuantity_Error").text($("#IssuedQtyGreaterthanAvaiQty").text());
        clickedrow.find("#IssuedQuantity_Error").css("display", "block");
        clickedrow.find("#IssuedQuantity").css("border-color", "red");
        clickedrow.find("#IssuedQuantity").val(parseFloat(0).toFixed(QtyDecDigit));
        return false;
    }
    else {
        clickedrow.find("#IssuedQuantity_Error").css("display", "none");
        clickedrow.find("#IssuedQuantity").css("border-color", "#ced4da");
        var mi_issueqty = parseFloat(parseFloat(IssuedQuantity)).toFixed(parseFloat(QtyDecDigit));
        clickedrow.find("#IssuedQuantity").val(mi_issueqty);
    }

}
function CheckMI_mentItemValidations() {
    debugger;
    var ErrorFlag = "N";
    var RowIndex = 0;
    var MI_Type = $('#hdMrsType').val();

    $("#MaterialReceiptItemDetailsTbl >tbody >tr").each(function () {
        debugger;
        var currentRow = $(this);
        RowIndex++;
        var ddlId = "#wh_id" + RowIndex;
        var whERRID = "#wh_Error" + RowIndex;

        var IssueQuantity = currentRow.find("#IssuedQuantity").val();
        var PendingQuantity = currentRow.find("#PendingQuantity").val();
        var AvaialbleQuantity = currentRow.find("#AvailableQuantity").val();

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
        if (parseFloat(IssueQuantity) > parseFloat(AvaialbleQuantity)) {
            currentRow.find("#IssuedQuantity_Error").text($("#IssuedQtyGreaterthanAvaiQty").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        if (parseFloat(IssueQuantity) > parseFloat(PendingQuantity)) {
            currentRow.find("#IssuedQuantity_Error").text($("#IssuedQtycannotbegreaterthanPendingQty").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        if (parseFloat(IssueQuantity) == parseFloat("0") || parseFloat(IssueQuantity) == "") {
            currentRow.find("#IssuedQuantity_Error").text($("#FillQuantity").text());
            currentRow.find("#IssuedQuantity_Error").css("display", "block");
            currentRow.find("#IssuedQuantity").css("border-color", "red");
            ErrorFlag = "Y";
        }
        else {
            currentRow.find("#IssuedQuantity_Error").css("display", "none");
            currentRow.find("#IssuedQuantity").css("border-color", "#ced4da");
        }
        if (MI_Type === 'E') {
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
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialIssue/MaterialIssuetListSearch",
            data: {
                RequisitionTyp: RequisitionTyp,
                RequiredArea: RequiredArea,
                MaterialIssueTo: MaterialIssueTo,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#MaterialIssuetbBody').html(data);
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
function OnChangeFromBranch() {
    debugger;
    var Tobranch = $("#Fr_branch").val();
    $("#vmbranch").css("display", "none");
    $("#Fr_branch").css("border-color", "#ced4da");
    $("#vmbranch").text("");

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferReceipt/GetToWHList",
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: true,
        data: { Tobranch: Tobranch, },/*Registration pass value like model*/
        success: function (data) {
            if (data == 'ErrorPage') {
                ErrorPage();
                return false;
            }
            /*dynamically dropdown list of all Assessment */
            debugger;
            if (data !== null && data !== "") /*For Chekin Json Data is return or not */ {
                var arr = [];
                arr = JSON.parse(data);/*this is use for json data braking in array type and put in a Array*/
                var s = '<option value="0">---Select---</option>';
                for (var i = 0; i < arr.length; i++) {
                    s += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
                }
                $("#fromWh").html(s);

            }
        },
        error: function (Data) {
        }
    });
}
function OnChangeTranferType() {
    debugger;
    document.getElementById("vmTransferType").innerHTML = null;
    $("#TransferType").css("border-color", "#ced4da");
    var TraType = $("#TransferType").val();
    if (TraType == "W") {

        //$('#Fr_branch').val($("#HdBranchId").val()).prop('selected', true);
        //OnChangeFromBranch();
        //$("#Fr_branch").attr('disabled', true);
        //$("#vmbranch").css("display", "none");
        //$("#Fr_branch").css("border-color", "#ced4da");
        //$("#vmbranch").text("");
        $("#hdtrfType").val(TraType);
        var DestBrnch = $("#hfbr_ID").val();
        $("#Fr_branch option[value=" + DestBrnch + "]").select2().show();
        $("#Fr_branch").attr('disabled', true);
        $('#Fr_branch').val($("#hfbr_ID").val()).prop('selected', true);
        BindSrcWHList(DestBrnch)
    }
    else {
        //$("#Fr_branch").attr('disabled', false);
        var DestBrnch = $("#hfbr_ID").val();
        var SrcBrnch = $("#Fr_branch").val();
        //$("#Fr_branch option[value=" + DestBrnch + "]").attr('selected', true);
        $("#Fr_branch option[value=" + DestBrnch + "]").select2().hide();
        $("#Fr_branch").attr('disabled', false);
        $('#Fr_branch').val(0).prop('selected', true);
    }

}
function BindSrcWHList(branch) {
    debugger;
    //var SrcBrId = $("#hfbr_ID").val();

    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialTransferReceipt/GetToWHList1",
            data: {
                SrcBrId: branch
                /*FromWhID: fromwh*/
            },
            dataType: "json",
            success: function (data) {
                debugger;
                if (data == 'ErrorPage') {
                    GRN_ErrorPage();
                    return false;
                }
                if (data !== null && data !== "") {
                    var arr = [];
                    arr = JSON.parse(data);
                    if (arr.length > 0) {

                        var s = '<option value="0">---Select---</option>';

                        for (var i = 0; i < arr.length; i++) {
                            //var wh_id=  arr[i].wh_id
                            //  if (fromwh == wh_id) {

                            //  }
                            //  else {
                            s += '<option value="' + arr[i].wh_id + '">' + arr[i].wh_name + '</option>';
                            /* }*/

                        }
                        $("#fromWh").html(s);
                        $("#fromWh").select2()

                    }
                }
            },
        });
}
function OnChangefromWh() {
    debugger;
    var fromwh = $('#fromWh').val();
    if (fromwh == "0" || fromwh == "") {
        //document.getElementById("vmwarehouse").innerHTML = $("#valueReq").text();
        //$("#fromWh").css("border-color", "red");
        $('#spanFromWh').text($("#valueReq").text());
        $("#spanFromWh").css("display", "block");
        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "red");
    }
    else {
        $('#hdWh_ID').val(fromwh);
        BindDestinationDDL()
        //document.getElementById("vmwarehouse").innerHTML = null;
        //$("#fromWh").css("border-color", "#ced4da");
        $("#spanFromWh").css("display", "none");
        $("[aria-labelledby='select2-fromWh-container']").css("border-color", "#ced4da");
        var transtyp = $("#hdtrfType").val();
        if (transtyp == "W") {
            $("#toWh").select2({
                templateResult: function (data) {
                    debugger
                    var selected = $("#fromWh").val();
                    if (data.id != selected) {

                        var classAttr = $(data.element).attr('class');
                        var hasClass = typeof classAttr != 'undefined';
                        classAttr = hasClass ? ' ' + classAttr : '';
                        var $result = $(
                            '<div ' + classAttr + '">' + data.text + '</div>'
                        );
                        return $result;
                    }
                    firstEmptySelect = false;
                }
            });
        }
        else {
            $("#toWh").select2();
        }
    }
    BindddlMaterialList();
}
function BindDestinationDDL() {
    debugger;
    var wh_id = $("#fromWh").val();
    var doc_id = $("#DocumentMenuId").val();
    if (TransferType == "0") {
        TransferType = "B"
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferOrder/GetSourceAndDestinationList",
        data: { wh_id: wh_id, doc_id: doc_id },
        success: function (data) {
            debugger;
            var arr = JSON.parse(data);
            // if (TransferType == "SW") {
            var b = '<option value="0">---Select---</option>';
            for (var i = 0; i < arr.Table.length; i++) {
                b += '<option value=' + arr.Table[i].wh_id + '>' + arr.Table[i].wh_name + '</option>'
            }
            $("#toWh").html(b);
            //} else {
            //$("#DestinationShopfloor").html(a);
            //}
        },
    });
}
function OnChangeToWh() {
    debugger;
    var towh = $('#toWh').val();
    if (towh == "0" || towh == "") {
        //document.getElementById("vmtowarehouse").innerHTML = $("#valueReq").text();
        //$("#toWh").css("border-color", "red");
        $('#spantoWh').text($("#valueReq").text());
        $("#spantoWh").css("display", "block");
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "red");
    }
    else {
        //document.getElementById("vmtowarehouse").innerHTML = null;
        //$("#toWh").css("border-color", "#ced4da");
        $("#spantoWh").css("display", "none");
        $("[aria-labelledby='select2-toWh-container']").css("border-color", "#ced4da");
    }
    BindddlMaterialList();
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
            $("#HdDeleteCommand").val("Delete");
            $('form').submit();
            debugger;
            return true;
        } else {
            return false;
        }
    });
    return false;
}

/*------------- For Workflow,Forward,Approve------------------*/
function ForwardBtnClick() {
    debugger;
    //var MTRStatus = "";
    //MTRStatus = $('#hdMTRStatus').val().trim();
    //if (MTRStatus === "D" || MTRStatus === "F") {

    //    if ($("#hd_nextlevel").val() === "0") {
    //        $("#Btn_Forward").attr("data-target", "");
    //    }
    //    else {
    //        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
    //        $("#Btn_Approve").attr("data-target", "");

    //    }
    //    //$("#radio_reject").prop("disabled", true);
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

    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
    var compId = $("#CompID").text();
    var brId = $("#BrId").text();
    var RcpDt = $("#txtMaterialReceiptDate").val();
    $.ajax({
        type: "POST",
        /* url: "/Common/Common/CheckFinancialYear",*/
        url: "/Common/Common/CheckFinancialYearAndPreviousYear",
        data: {
            compId: compId,
            brId: brId,
            DocDate: RcpDt
        },
        success: function (data) {
            /* if (data == "Exist") {*/ /*End to chk Financial year exist or not*/
            /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            if (data == "TransAllow") {
                var MTRStatus = "";
                MTRStatus = $('#hdMTRStatus').val().trim();
                if (MTRStatus === "D" || MTRStatus === "F") {

                    if ($("#hd_nextlevel").val() === "0") {
                        $("#Btn_Forward").attr("data-target", "");
                    }
                    else {
                        $("#Btn_Forward").attr("data-target", "#Forward_Pop");
                        $("#Btn_Approve").attr("data-target", "");

                    }
                    //$("#radio_reject").prop("disabled", true);
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
                /*   swal("", $("#FinancialYearDoesNotExistTransactionCanNotBeDone").text(), "warning");*/
                /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                swal("", $("#NomodificationisallowedinpreviousFYdocument").text(), "warning");
                $("#Btn_Forward").attr("data-target", "");
                $("#Btn_Approve").attr("data-target", "");
                $("#Forward_Pop").attr("data-target", "");

            }
        }
    });
    /*End to chk Financial year exist or not*/
    return false;
}
async function OnClickForwardOK_Btn() {
    debugger;
    var fwchkval = $("input[name='forward_action']:checked").val();
    var MTRNo = "";
    var MTRDate = "";
    var docid = "";
    var level = "";
    var Remarks = "";
    var forwardedto = "";
    var transtype = "";
    var mailerror = "";
    var WF_Status1 = $("#WF_status1").val();
    docid = $("#DocumentMenuId").val();
    MTRNo = $("#txtMaterialReceiptNo").val();
    MTRDate = $("#txtMaterialReceiptDate").val();
    transtype = (docid + ',' + MTRNo + ',' + MTRDate + ',' + WF_Status1);
    $("#hdDoc_No").val(MTRNo);
    Remarks = $("#fw_remarks").val();
    var ListFilterData1 = $("#ListFilterData1").val();
    var SrcBranch = $("#Fr_branch").val();
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
        var pdfAlertEmailFilePath = "MaterialTransferReceipt_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(MTRNo, MTRDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/MaterialTransferReceipt/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && MTRNo != "" && MTRDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(MTRNo, MTRDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialTransferReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&transtype=" + transtype + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Approve") {
        window.location.href = "/ApplicationLayer/MaterialTransferReceipt/Approve_TMR?MTR_no=" + MTRNo + "&MTR_date=" + MTRDate + "&A_Status=" + fwchkval + "&A_Level=" + $("#hd_currlevel").val() + "&A_Remarks=" + Remarks + "&ListFilterData1=" + ListFilterData1 + "&WF_Status1=" + WF_Status1 + "&docid=" + docid + "&SrcBranch=" + SrcBranch;

    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && MTRNo != "" && MTRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(MTRNo, MTRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialTransferReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&transtype=" + transtype + "&Mailerror=" + mailerror;
        }
    }
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && MTRNo != "" && MTRDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(MTRNo, MTRDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/MaterialTransferReceipt/ToRefreshByJS?ListFilterData1=" + ListFilterData1 + "&transtype=" + transtype + "&Mailerror=" + mailerror;
        }
    }

}
// added by Nidhi on 23-06-2025
//async function GetPdfFilePathToSendonEmailAlert(MTRNo, MTRDate, fileName) {
//    debugger;
//    await $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/MaterialTransferReceipt/SavePdfDocToSendOnEmailAlert",
//        data: { MTRNo: MTRNo, MTRDate: MTRDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#txtMaterialReceiptNo").val();
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
function OnChangeCancelFlag() {
    if ($("#CancelFlag").is(":checked")) {
        $("#btn_save").attr("disabled", false);
        $("#btn_save").attr("onclick", "return CheckFormValidation()");
        $("#btn_save").css({ "-webkit-filter": "grayscale(0%)", "filter": "grayscale(0%);" });
    } else {
        $("#btn_save").attr("disabled", true);
        $("#btn_save").attr("onclick", "");
        $("#btn_save").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    }
}

/***--------------------------------Sub Item Section-----------------------------------------***/

function SubItemDetailsPopUp(flag, e) {
    debugger
    var clickdRow = $(e.target).closest('tr');
    var ProductNm = clickdRow.find("#ItemName").val();
    var ProductId = clickdRow.find("#hdItemId").val();
    var UOM = $("#UOM").val();
    var doc_no = $("#HdMINo").val();
    var doc_dt = $("#txtMI_Date").val();
    var Sub_Quantity = 0;
    var NewArr = new Array();
    if (flag == "MTReciept") {
        Sub_Quantity = clickdRow.find("#ReceiptQuantity").val();
    }
    else if (flag == "MTPending") {
        Sub_Quantity = clickdRow.find("#PendingQuantity").val();
    }
    else if (flag == "MTRequisition") {
        Sub_Quantity = clickdRow.find("#RequisitionQuantity").val();
    }
    flag = "MTReciept"
    var IsDisabled = "Y";//$("#DisableSubItem").val();
    var hd_Status = $("#hdMTRStatus").val();
    hd_Status = IsNull(hd_Status, "").trim();
    if (hd_Status != "") {
        doc_no = $("#txtMaterialReceiptNo").val();
        doc_dt = $("#txtMaterialReceiptDate").val();
    }
    var frombranch = $("#Fr_branch").val();
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/MaterialTransferReceipt/GetSubItemDetails",
        data: {
            Item_id: ProductId,
            SubItemListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Flag: flag,
            Status: hd_Status,
            Doc_no: doc_no,
            Doc_dt: doc_dt,
            frombranch: frombranch
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
//function fn_CustomReSetSubItemData(itemId) {

//    $("#Sub_ItemDetailTbl > tbody > tr").each(function () {
//        var Crow = $(this).closest("tr");
//        var subItemId = Crow.find("#subItemId").val();
//        var subItemQty = Crow.find("#subItemQty").val();
//        var subItemReqQty = Crow.find("#subItemReqQty").val();
//        var subItemPendQty = Crow.find("#subItemPendQty").val();
//        var subItemAvlQty = Crow.find("#subItemAvlQty").val();
//        var len = $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').length;
//        if (len > 0) {
//            $("#hdn_Sub_ItemDetailTbl >tbody >tr td #subItemId[value=" + subItemId + "]").closest('tr').each(function () {
//                var InnerCrow = $(this).closest("tr");
//                //var ItemId = InnerCrow.find("#ItemId").val();
//                InnerCrow.find("#subItemQty").val(subItemQty);
//                InnerCrow.find("#subItemReqQty").val(subItemReqQty);
//                InnerCrow.find("#subItemPendQty").val(subItemPendQty);
//                InnerCrow.find("#subItemAvlQty").val(subItemAvlQty);

//            });
//        } else {

//            $("#hdn_Sub_ItemDetailTbl >tbody").append(`<tr>
//                            <td><input type="text" id="ItemId" value='${itemId}'></td>
//                            <td><input type="text" id="subItemId" value='${subItemId}'></td>
//                            <td><input type="text" id="subItemQty" value='${subItemQty}'></td>
//                            <td><input type="text" id="subItemReqQty" value='${subItemReqQty}'></td>
//                            <td><input type="text" id="subItemPendQty" value='${subItemPendQty}'></td>
//                            <td><input type="text" id="subItemAvlQty" value='${subItemAvlQty}'></td>
//                        </tr>`);

//        }

//    });

//}
//function CheckValidations_forSubItems() {
//    //return Cmn_CheckValidations_forSubItems("ddl_ProductName", "txt_JobQuantity", "SubItemOrdQty", "Y");
//    return Cmn_CheckValidations_forSubItems("MaterialIssueItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "SubItemIssueQty", "Y");
//}
//function ResetWorningBorderColor() {
//    return Cmn_CheckValidations_forSubItems("MaterialIssueItemDetailsTbl", "", "hdItemId", "IssuedQuantity", "SubItemIssueQty", "N");
//}
//function MTR_SubItemList() {
//    var NewArr = new Array();

//    $("#hdn_Sub_ItemDetailTbl tbody tr").each(function () {
//        var row = $(this);
//        //if (row.find('#subItemQty').val() != "" && row.find('#subItemQty').val() != "0") {
//        var List = {};
//        List.item_id = row.find("#ItemId").val();
//        List.sub_item_id = row.find('#subItemId').val();
//        List.qty = row.find('#subItemQty').val();
//        List.req_qty = row.find('#subItemReqQty').val();
//        List.pend_qty = row.find('#subItemPendQty').val();
//        List.avl_qty = row.find('#subItemAvlQty').val();
//        NewArr.push(List);
//        //}
//    });
//    return NewArr;
//}
/***--------------------------------Sub Item Section End-----------------------------------------***/

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
function FilterItemDetail(e) {//added by Prakash Kumar on 18-02-2025 to filter item details
    debugger;
    Cmn_FilterTableData(e, "MaterialReceiptItemDetailsTbl", [{ "FieldId": "ItemName", "FieldType": "input" }]);
}