$(document).ready(function () {
    debugger;
    $('#ddlMtrlTrackListSupplier').select2();
    //BindSupplierMaterialTrackList();
    $('#ddl_MTProductNameListPage').empty();
    BindMetrlTrackProductNameInListPage();
    $('#MTJob_Ord_number').select2();
    BindMTJobOrderNumber();
});
//function BindSupplierMaterialTrackList() {
//    debugger;
//    var Branch = sessionStorage.getItem("BranchID");
//    $("#MtrlTrackListSupplier").select2({
//        ajax: {
//            url: $("#SuppNameList").val(),
//            data: function (params) {
//                var queryParameters = {
//                    SuppName: params.term, // search term like "a" then "an"
//                    SuppPage: params.page,
//                    BrchID: Branch
//                };
//                return queryParameters;
//            },
//            dataType: "json",
//            cache: true,
//            delay: 250,
//            contentType: "application/json; charset=utf-8",
//            processResults: function (data, params) {
//                if (data == 'ErrorPage') {
//                    ErrorPage();
//                    return false;
//                }
//                params.page = params.page || 1;
//                return {
//                    //results:data.results,
//                    results: $.map(data, function (val, item) {
//                        return { id: val.ID, text: val.Name };
//                    })
//                };
//            }
//        },
//    });
//}

function BindMetrlTrackProductNameInListPage() {
    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/ApplicationLayer/MaterialTracking/GetProductNameInDDLListPage",
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
                        
                        $('#ddl_MTProductNameListPage').empty();
                        $('#ddl_MTProductNameListPage').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}'></optgroup>`);
                        for (var i = 0; i < arr.Table.length; i++) {
                            $('#Textddl').append(`<option value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
                        }
                        $('#ddl_MTProductNameListPage').select2({
                            
                        });
                        var hdn_product_idListPage = $("#MThdn_product_idListPage").val();
                        if (hdn_product_idListPage != '') {
                            $("#ddl_MTProductNameListPage").val(hdn_product_idListPage).trigger('change.select2');;
                        }
                    }
                }
            },
        });
}

function ddl_MTProductNameListPage_onchange() {
    debugger;
    var ddl_ProductNameListPage = $("#ddl_MTProductNameListPage").val();
    $("#MThdn_product_idListPage").val(ddl_ProductNameListPage);

}
function BindMTJobOrderNumber() {
    debugger;
    
    $.ajax({
        type: 'POST',
        url: "/ApplicationLayer/MaterialTracking/GetJobORDDocList",
        datatype: "json",
        data: {
            
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
                if (arr.length > 0) {

                    $("#MTJob_Ord_number option").remove();
                    $("#MTJob_Ord_number optgroup").remove();
                    $('#MTJob_Ord_number').append(`<optgroup class='def-cursor' id="Textddlmt" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                    for (var i = 0; i < arr.length; i++) {
                        $('#Textddlmt').append(`<option data-date="${arr[i].doc_dt}" value="${arr[i].docno}">${arr[i].doc_no}</option>`);
                    }
                    var firstEmptySelect = true;
                    $('#MTJob_Ord_number').select2({
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

                    //$("#src_doc_date").val("");

                }
            }
        }

    })

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
    FilterMaterialTrackDtList();

}
function FilterMaterialTrackDtList() {
    debugger;
    try {
        var SuppId = $("#ddlMtrlTrackListSupplier option:selected").val();
        var OutOPProdctID = $("#ddl_MTProductNameListPage option:selected").val();
        var JobOrdNO = $("#MTJob_Ord_number option:selected").val();
        var Fromdate = $("#txtFromdate").val();
        var Todate = $("#txtTodate").val();
        //var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/MaterialTracking/SearchMaterialTrackDetail",
            data: {
                SuppId: SuppId,
                OutOPProdctID: OutOPProdctID,
                JobOrdNO: JobOrdNO,
                Fromdate: Fromdate,
                Todate: Todate
               
            },
            success: function (data) {
                debugger;
                $('#tbodyMISMaterialTrackingListDetails').html(data);
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
function OnClickMTMaterialIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hdnMTMtrlId").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickMTOpOutPrdctIconBtn(e) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    var ItmCode = "";
    ItmCode = clickedrow.find("#hdnMTOpoutPrcdtId").val();
    ItemInfoBtnClick(ItmCode);
}
function OnClickMTAllQtyIconBtn(e, Type) {
    debugger;
    var clickedrow = $(e.target).closest("tr");
    
    var MaterialID = "";
    var MaterialName = "";
    var MaterialUomID = "";
    var MaterialUomName = "";
    //var SuppID = "";
    //var SuppName = "";
    //var JobOrdNo = "";
    //var JobOrdDt = "";
    
    //var OpOutItmID = "";
    //var OpOutItmName = "";
    //var OpOutUomID = "";
    //var OpOutUomName = "";
    //var OrderQty = "";
    //var IssueQty = "";
    //var ConsumeQty = "";
    //var ReturnQty = "";
    //var BalanceQty = "";

    MaterialID = clickedrow.find("#hdnMTMtrlId").val();
    MaterialName = clickedrow.find("#tblMTMatrlItmNameSpan").text();
    MaterialUomID = clickedrow.find("#hdnMTMtrlUomId").val();
    MaterialUomName = clickedrow.find("#tblMTMtrlUom").text();
    //SuppID = clickedrow.find("#hfMTSuppId").val();
    //SuppName = clickedrow.find("#tblMTSuppName").text();
    JobOrdNo = clickedrow.find("#tblMTOrdNo").text();
    JobOrdDate = clickedrow.find("#tblMTOrddate").text();
    hdnJobOrdDt = clickedrow.find("#hfMTOrderDT").val();
    
    //OpOutItmID = clickedrow.find("#hdnMTOpoutPrcdtId").val();
    //OpOutItmName = clickedrow.find("#tblMTOpOutItmNameSpan").text();
    //OpOutUomID = clickedrow.find("#hdnMTOpoutUomId").val();
    //OpOutUomName = clickedrow.find("#tblMTOpOutUom").text();
    //OrderQty = clickedrow.find("#tblMTOrdQty").text();
    //IssueQty = clickedrow.find("#tblMTIssueQty").text();
    //ConsumeQty = clickedrow.find("#tblMTConsumeQty").text();
    //ReturnQty = clickedrow.find("#tblMTRetrnQty").text();
    //BalanceQty = clickedrow.find("#tblMTBalancQty").text();

    var QtyDecDigit = $("#QtyDigit").text();

    if (JobOrdNo != "" && JobOrdNo != null && hdnJobOrdDt != "" && hdnJobOrdDt != null) {
        try {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/MaterialTracking/GetMTAllQtyItemDetailList",
                    data: { Type: Type, JobOrdNo: JobOrdNo, hdnJobOrdDt: hdnJobOrdDt, MaterialID: MaterialID },
                    dataType: "json",
                    success: function (data) {
                        debugger;
                        if (data == 'ErrorPage') {
                            return false;
                        }
                        var tbl_id = "";
                        if (Type == "Issue") {
                            tbl_id = "#TblMTIssueQtyDetail";
                        }
                        if (Type == "Consume") {
                            tbl_id = "#TblMTConsumeQtyDetail";
                        }
                        //if (Type == "GRN") {
                        //    tbl_id = "#TblGRNQtyDetail";
                        //}

                        cmn_delete_datatable(tbl_id);

                        if (data !== null && data !== "") {
                            var arr = [];
                            arr = JSON.parse(data);
                            if (arr.Table.length > 0) {
                                if (Type == "Issue") {
                                    debugger;
                                    $("#txtIssueMaterialName").val(MaterialName);
                                    $("#txtIssueMaterialUom").val(MaterialUomName);
                                    //$("#txtIssueSuppName").val(SuppName);
                                    //$("#txtIssueJONum").val(JobOrdNo);
                                    //$("#txtIssueJODt").val(JobOrdDate);
                                    //$("#hdntxtIssueJODt").val(hdnJobOrdDt);
                                    //$("#txtIssueOpOutItmName").val(OpOutItmName);
                                    //$("#txtIssueOpOutUom").val(OpOutUomName);
                                    //$("#txtIssueOrdQty").val(OrderQty);
                                    //$("#txtIssueIssueQty").val(OrderQty);
                                    var rowIdx = 0;
                                
                                    $('#TblMTIssueQtyDetail tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                       
                                        $('#TblMTIssueQtyDetail tbody').append
                                            (
                                                `<tr id="R${++rowIdx}">
                                                <td width="5%">${k + 1}</td>
                                                <td id='tblMTDispNo'>${arr.Table[k].dispatch_no}</td>
                                                <td id='tblMTDispDate'>${arr.Table[k].DispatchDate}</td>
                                                <td hidden id='hdnMTDispDt'>${arr.Table[k].DispatchDT}</td>
                                                <td id='tblMTIssueQty' class="num_right">${arr.Table[k].IssueQty}</td>
                                                
                                            </tr>`
                                            );
                                    }
                                   
                                }
                                
                                if (Type == "Consume") {
                                    debugger;
                                    $("#txtMTConsumeMaterialName").val(MaterialName);
                                    $("#txtMTConsumeMaterialUom").val(MaterialUomName);
                                    //$("#txtGRNJONum").val(JobOrdNo);
                                    //$("#txtGRNJODt").val(JobOrdDate);
                                    //$("#txtGRNSuppName").val(SuppName);
                                    //$("#txtGRNFItmName").val(FinishItmName);
                                    //$("#txtGRNFUom").val(FinishUomName);
                                    //$("#txtGRNOpName").val(OpName);
                                    //$("#txtGRNOpOutItmName").val(OpOutItmName);
                                    //$("#txtGRNOpOutUom").val(OpOutUomName);
                                    //$("#txtGRNOrdQty").val(OrderQty);
                                    var rowIdx = 0;
                                    
                                    $('#TblMTConsumeQtyDetail tbody tr').remove();
                                    for (var k = 0; k < arr.Table.length; k++) {
                                       
                                        $('#TblMTConsumeQtyDetail tbody').append(
                                            `<tr id="R${++rowIdx}">
                                                <td width="5%">${k + 1}</td>
                                                <td>${arr.Table[k].mr_no}</td>
                                                <td>${arr.Table[k].MRDate}</td>
                                                <td hidden id='hdnMTGRNDt'>${arr.Table[k].MRDT}</td>
                                                <td class="num_right">${arr.Table[k].ConsumeQty}</td>
                                           </tr>`
                                        );
                                    }

                                }
                                //if (Type == "Recev") {
                                //    debugger;
                                //    $("#txtRecvJONum").val(JobOrdNo);
                                //    $("#txtRecvJODt").val(JobOrdDate);
                                //    $("#txtRecvSuppName").val(SuppName);
                                //    $("#txtRecvFItmName").val(FinishItmName);
                                //    $("#txtRecvFUom").val(FinishUomName);
                                //    $("#txtRecvOpName").val(OpName);
                                //    $("#txtRecvOpOutItmName").val(OpOutItmName);
                                //    $("#txtRecvOpOutUom").val(OpOutUomName);
                                //    $("#txtRecvOrdQty").val(OrderQty);
                                //    var rowIdx = 0;

                                //    $('#TblDNReceiveQtyDetail tbody tr').remove();
                                //    for (var k = 0; k < arr.Table.length; k++) {

                                //        $('#TblDNReceiveQtyDetail tbody').append(
                                //            `<tr id="R${++rowIdx}">
                                //                <td width="5%">${k + 1}</td>
                                //                <td>${arr.Table[k].dn_no}</td>
                                //                <td>${arr.Table[k].DNDate}</td>
                                //                <td hidden id='hdnDNDt'>${arr.Table[k].DNDT}</td>
                                //                <td class="num_right">${arr.Table[k].BillQty}</td>
                                //                <td class="num_right">${arr.Table[k].RecevQty}</td>
                                //            </tr>`
                                //        );
                                //    }

                                //}
                            }
                            else {
                                if (Type == "Issue") {
                                    $('#TblMTIssueQtyDetail tbody tr').remove();
                                }
                                if (Type == "Consume") {
                                    $('#TblMTConsumeQtyDetail tbody tr').remove();
                                }
                                //if (Type == "GRN") {
                                //    $('#TblGRNQtyDetail tbody tr').remove();
                                //}

                            }
                        }
                        else {
                            if (Type == "Issue") {
                                $('#TblMTIssueQtyDetail tbody tr').remove();
                            }
                            if (Type == "Consume") {
                                $('#TblMTConsumeQtyDetail tbody tr').remove();
                            }
                            //if (Type == "GRN") {
                            //    $('#TblGRNQtyDetail tbody tr').remove();
                            //}

                        }
                        cmn_apply_datatable(tbl_id);

                    },
                });
        } catch (err) {
            //console.log(" Error : " + err.message);
        }
    }
}




