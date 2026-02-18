$(document).ready(function () {
    debugger;

    BindSupplierJOTrackList();
    $('#ddl_JOTProductNameListPage').empty();
    BindJOTrackProductNameInListPage();

});
function BindSupplierJOTrackList() {
    var Branch = sessionStorage.getItem("BranchID");
    $("#JOTrackListSupplier").select2({
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
                    ErrorPage();
                    return false;
                }
                params.page = params.page || 1;
                return {
                    //results:data.results,
                    results: $.map(data, function (val, item) {
                        return { id: val.ID, text: val.Name };
                    })
                };
            }
        },
    });
}

function BindJOTrackProductNameInListPage() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/JobOrderTracking/GetProductNameInDDLListPage",
            data: function (params) {
                var queryParameters = {
                    JO_ItemName: params.term
                };
                return queryParameters;
            },
            dataType: "json",
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
                        $('#ddl_JOTFProductNameListPage').empty();
                        $('#ddl_JOTFProductNameListPage').append(`<optgroup class='def-cursor' id="FTextddl" label='${$("#ItemName").text()}'></optgroup>`);

                        $('#ddl_JOTProductNameListPage').empty();
                        $('#ddl_JOTProductNameListPage').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}'></optgroup>`);

                        
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#FTextddl').append(`<option value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);

                            $('#Textddl').append(`<option value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);

                               }
                        $('#ddl_JOTFProductNameListPage').select2({
                            
                        });
                        $('#ddl_JOTProductNameListPage').select2({
                            //templateResult: function (data) {

                            //    var classAttr = $(data.element).attr('class');
                            //    var hasClass = typeof classAttr != 'undefined';
                            //    classAttr = hasClass ? ' ' + classAttr : '';
                            //    var $result = $(
                            //        '<div class="row">' +
                            //        '<div class="col-md-12 col-xs-12' + classAttr + '">' + data.text + '</div>' +

                            //        '</div>'
                            //    );
                            //    return $result;
                            //    firstEmptySelect = false;
                            //}
                        });

                        var hdn_Fproduct_idListPage = $("#JOThdn_Fproduct_idListPage").val();
                        if (hdn_Fproduct_idListPage != '') {
                            $("#ddl_JOTFProductNameListPage").val(hdn_Fproduct_idListPage).trigger('change.select2');;
                        }

                        var hdn_product_idListPage = $("#JOThdn_product_idListPage").val();
                        if (hdn_product_idListPage != '') {
                            $("#ddl_JOTProductNameListPage").val(hdn_product_idListPage).trigger('change.select2');;
                        }
                    }
                }
            },
        });
}

function ddl_JOTProductNameListPage_onchange() {
    debugger;
    var ddl_ProductNameListPage = $("#ddl_JOTProductNameListPage").val();
    $("#JOThdn_product_idListPage").val(ddl_ProductNameListPage);

}
function ddl_JOTFProductNameListPage_onchange() {
    debugger;
    var ddl_FProductNameListPage = $("#ddl_JOTFProductNameListPage").val();
    $("#JOThdn_Fproduct_idListPage").val(ddl_FProductNameListPage);

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
            //alert('From Date cannnot be greater then To date');
            swal("", $("#fromtodatemsg").text(), "warning");
        }
    }
    else {
        //alert('please select from and to date');
    }
}

function BtnSearch() {
    debugger;
    FilterJobOrdTrackDtList();
    
}
function FilterJobOrdTrackDtList() {
    debugger;
    try {
        var SuppId = $("#JOTrackListSupplier option:selected").val();
        var OutOPProdctID = $("#ddl_JOTProductNameListPage option:selected").val();
        var FinishProdctID = $("#ddl_JOTFProductNameListPage option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/JobOrderTracking/SearchJOTrackDetail",
            data: {
                SuppId: SuppId,
                FinishProdctID: FinishProdctID,
                OutOPProdctID: OutOPProdctID,
                Fromdate: Fromdate,
                Todate: Todate,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#tbodyMISJOTrackingListDetails').html(data);
                //$('#ListFilterData').val(SuppId + ',' + ProdctID + ',' + Fromdate + ',' + Todate + ',' + Status);
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



function OnClickFinishPrdctIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hdnFPrdctId").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickOpOutPrdctIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hdnOpoutPrcdtId").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickAllQtyIconBtn(e, Type) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var JobOrdNo = "";
    var JobOrdDt = "";
    var SuppID = "";
    var SuppName = "";
    var FinishItmID = "";
    var FinishItmName = "";
    var FinishUomID = "";
    var FinishUomName = "";
    var OpId = "";
    var OpName = "";
    var OpOutItmID = "";
    var OpOutItmName = "";
    var OpOutUomID = "";
    var OpOutUomName = "";
    var OrderQty = "";

    JobOrdNo = clickedrow.find("#tblOrdNo").text();
    JobOrdDate = clickedrow.find("#tblOrddate").text();
    hdnJobOrdDt = clickedrow.find("#hfOrderDT").val();
    SuppID = clickedrow.find("#hfSuppId").val();
    SuppName = clickedrow.find("#tblSuppName").text();
    FinishItmID = clickedrow.find("#hdnFPrdctId").val();
    FinishItmName = clickedrow.find("#tblFItmNameSpan").text();
    FinishUomID = clickedrow.find("#hdnFPrdctUomId").val();
    FinishUomName = clickedrow.find("#tblFItmUom").text();
    OpId = clickedrow.find("#hdnOpId").val();
    OpName = clickedrow.find("#tblOPName").text();
    OpOutItmID = clickedrow.find("#hdnOpoutPrcdtId").val();
    OpOutItmName = clickedrow.find("#tblOpOutItmNameSpan").text();
    OpOutUomID = clickedrow.find("#hdnOpoutUomId").val();
    OpOutUomName = clickedrow.find("#tblOpOutUom").text();
    OrderQty = clickedrow.find("#tblOrdQty").text();

    var QtyDecDigit = $("#QtyDigit").text();
    
    if (JobOrdNo != "" && JobOrdNo != null && hdnJobOrdDt != "" && hdnJobOrdDt != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/JobOrderTracking/GetAllQtyItemDetailList",
                    data: { Type: Type, JobOrdNo: JobOrdNo, hdnJobOrdDt: hdnJobOrdDt},
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            return false;
                        }
                        var tbl_id = "";
                        if (Type == "Disp") {
                            tbl_id = "#TblDispatchQtyDetail";
                        }
                        if (Type == "Recev") {
                            tbl_id = "#TblDNReceiveQtyDetail";
                        }
                        if (Type == "GRN") {
                            tbl_id = "#TblGRNQtyDetail";
                        }

                        cmn_delete_datatable(tbl_id);

                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                if (Type == "Disp") {
                                    debugger;
                                    $("#txtDispJONum").val(JobOrdNo);
                                    $("#txtDispJODt").val(JobOrdDate);
                                    $("#hdntxtDispJODt").val(hdnJobOrdDt);
                                    $("#txtDispSuppName").val(SuppName);
                                    $("#txtDispFItmName").val(FinishItmName);
                                    $("#txtDispFUom").val(FinishUomName);
                                    $("#txtDispOPName").val(OpName);
                                    $("#txtDispOpOutItmName").val(OpOutItmName);
                                    $("#txtDispOpOutUom").val(OpOutUomName);
                                    $("#txtDispOrdQty").val(OrderQty);
                                    var rowIdx = 0;
                                    //var QtyDigit = parseFloat(0).toFixed(QtyDecDigit);
                                    $('#TblDispatchQtyDetail tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        //QtyDigit = parseFloat(QtyDigit) + parseFloat(arr.Table[k].qty_bs);
                                        $('#TblDispatchQtyDetail tbody').append
                                        (
                                            `<tr id="R${++rowIdx}">
                                                <td width="5%">${k + 1}</td>
                                                <td id='tblDispNo'>${arr.Table[k].dispatch_no}</td>
                                                <td id='tblDispDate'>${arr.Table[k].DispatchDate}</td>
                                                <td hidden id='hdnDispDt'>${arr.Table[k].DispatchDT}</td>
                                                <td id='tblDispQty' class="num_right">${arr.Table[k].DispQty}</td>
                                                <td>
                                                    <div class="col-sm-12 no-padding center">
                                                    <button type="button" value="Y" class="calculator" onclick="OnClickDispatchBatchLotDetailIcon(event);" id="BtnMaterialDetail" data-toggle="modal" data-target="#MaterialDetail" data-backdrop="static" data-keyboard="false"><i class="fa fa-eye" aria-hidden="true" data-toggle="" title="${$("#span_MaterialDetail").text()}"></i></button>
                                                    </div>
                                                </td>
                                            </tr>`
                                        );
                                    }
                                    //OnClickDispatchBatchLotDetail(event)
                                }
                                if (Type == "Recev") {
                                    debugger;
                                    $("#txtRecvJONum").val(JobOrdNo);
                                    $("#txtRecvJODt").val(JobOrdDate);
                                    $("#txtRecvSuppName").val(SuppName);
                                    $("#txtRecvFItmName").val(FinishItmName);
                                    $("#txtRecvFUom").val(FinishUomName);
                                    $("#txtRecvOpName").val(OpName);
                                    $("#txtRecvOpOutItmName").val(OpOutItmName);
                                    $("#txtRecvOpOutUom").val(OpOutUomName);
                                    $("#txtRecvOrdQty").val(OrderQty);
                                    var rowIdx = 0;
                                    //var QtyDigit = parseFloat(0).toFixed(QtyDecDigit);
                                    $('#TblDNReceiveQtyDetail tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++)
                                    {
                                        //QtyDigit = parseFloat(QtyDigit) + parseFloat(arr.Table[k].qty_bs);
                                        $('#TblDNReceiveQtyDetail tbody').append(
                                            `<tr id="R${++rowIdx}">
                                                <td width="5%">${k + 1}</td>
                                                <td>${arr.Table[k].dn_no}</td>
                                                <td>${arr.Table[k].DNDate}</td>
                                                <td hidden id='hdnDNDt'>${arr.Table[k].DNDT}</td>
                                                <td class="num_right">${arr.Table[k].BillQty}</td>
                                                <td class="num_right">${arr.Table[k].RecevQty}</td>
                                            </tr>`
                                        );
                                    }

                                }
                                if (Type == "GRN") {
                                    debugger;
                                    $("#txtGRNJONum").val(JobOrdNo);
                                    $("#txtGRNJODt").val(JobOrdDate);
                                    $("#txtGRNSuppName").val(SuppName);
                                    $("#txtGRNFItmName").val(FinishItmName);
                                    $("#txtGRNFUom").val(FinishUomName);
                                    $("#txtGRNOpName").val(OpName);
                                    $("#txtGRNOpOutItmName").val(OpOutItmName);
                                    $("#txtGRNOpOutUom").val(OpOutUomName);
                                    $("#txtGRNOrdQty").val(OrderQty);
                                    var rowIdx = 0;
                                    //var QtyDigit = parseFloat(0).toFixed(QtyDecDigit);
                                    $('#TblGRNQtyDetail tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        //QtyDigit = parseFloat(QtyDigit) + parseFloat(arr.Table[k].qty_bs);
                                        $('#TblGRNQtyDetail tbody').append(
                                            `<tr id="R${++rowIdx}">
                                                <td width="5%">${k + 1}</td>
                                                <td class="ItmNameBreak itmStick tditemfrz">${arr.Table[k].mr_no}</td>
                                                <td>${arr.Table[k].MRDate}</td>
                                                <td hidden id='hdnDNDt'>${arr.Table[k].MRDT}</td>
                                                <td>${arr.Table[k].qc_no}</td>
                                                <td>${arr.Table[k].QCDate}</td>
                                                <td hidden id='hdnDNDt'>${arr.Table[k].QCDT}</td>
                                                <td class="num_right">${arr.Table[k].AccptQty}</td>
                                                <td class="num_right">${arr.Table[k].RejctQty}</td>
                                                <td class="num_right">${arr.Table[k].RewrkQty}</td>
                                                
                                            </tr>`
                                        );
                                    }

                                }
                                
                            }
                            else {
                                if (Type == "Disp") {
                                    $('#TblDispatchQtyDetail tbody tr').remove();
                                }
                                if (Type == "Recev") {
                                    $('#TblDNReceiveQtyDetail tbody tr').remove();
                                 }
                                if (Type == "GRN") {
                                    $('#TblGRNQtyDetail tbody tr').remove();
                                }

                            }
                        }
                        else {
                            if (Type == "Disp") {
                                $('#TblDispatchQtyDetail tbody tr').remove();
                            }
                            if (Type == "Recev") {
                                $('#TblDNReceiveQtyDetail tbody tr').remove();
                            }
                            if (Type == "GRN") {
                                $('#TblGRNQtyDetail tbody tr').remove();
                            }

                        }
                        cmn_apply_datatable(tbl_id);
                        
                    },
                });
        } catch (err) {
            //console.log(" Error : " + err.message);
        }
    }
}

function OnClickDispatchBatchLotDetailIcon(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var JobOrdrNo = "";
    var JobOrdrDate = "";
    var DispatchNo = "";
    var DispatchDate = "";
    var hdnDispatchDt = "";
    var DispatchQty = "";

    JobOrdrNo = $("#txtDispJONum").val();
    JobOrdrDate = $("#hdntxtDispJODt").val(); 
    DispatchNo = clickedrow.find("#tblDispNo").text();
    DispatchDate = clickedrow.find("#tblDispDate").text();
    hdnDispatchDt = clickedrow.find("#hdnDispDt").text();
    DispatchQty = clickedrow.find("#tblDispQty").text();

    var QtyDecDigit = $("#QtyDigit").text();
    //$("#DispatchQuantity").modal('hide');
    if (DispatchNo != "" && DispatchNo != null && hdnDispatchDt != "" && hdnDispatchDt != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/JobOrderTracking/GetMtrlDispRawMaterialDetailList",
                    data: {
                        JobOrdNo: JobOrdrNo,
                        JobOrdrDate: JobOrdrDate,
                        DispatchNo: DispatchNo,
                        hdnDispatchDt: hdnDispatchDt
                    },
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            return false;
                        }
                        var tbl_id = "";
                        tbl_id = "#TblDispatchRawMaterialDetail";
                        cmn_delete_datatable(tbl_id);

                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                debugger;
                                    $("#txtMDispatchNo").val(DispatchNo);
                                    $("#txtMDispatchDate").val(DispatchDate);
                                    $("#txtMDispatchQty").val(DispatchQty);
                                    
                                    var rowIdx = 0;
                                    //var QtyDigit = parseFloat(0).toFixed(QtyDecDigit);
                                    $('#TblDispatchRawMaterialDetail tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                        //QtyDigit = parseFloat(QtyDigit) + parseFloat(arr.Table[k].qty_bs);
                                        $('#TblDispatchRawMaterialDetail tbody').append
                                            (
                                                `<tr id="R${++rowIdx}">
                                                <td width="5%">${k + 1}</td>
                                                <td>${arr.Table[k].item_name}</td>
                                                <td hidden id='MItmId'>${arr.Table[k].item_id}</td>
                                                <td>${arr.Table[k].uom_alias}</td>
                                                <td hidden id='MUomId'>${arr.Table[k].uom_id}</td>
                                                <td hidden id='MWhId'>${arr.Table[k].wh_id}</td>
                                                <td>${arr.Table[k].wh_name}</td>
                                                <td class="num_right">${arr.Table[k].issue_qty}</td>
                                                </tr>`
                                            );
                                    }
                            }
                            else {
                                $('#TblDispatchRawMaterialDetail tbody tr').remove();
                            }
                        }
                        else {
                            $('#TblDispatchRawMaterialDetail tbody tr').remove();
                        }

                        cmn_apply_datatable(tbl_id);
                    },
                });
        } catch (err) {
            //console.log(" Error : " + err.message);
        }
    }
}


