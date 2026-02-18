/*----------------THIS PAGE IS CREATED BY HINA SHARMA ON 13-02-2025----------------------------------*/
$(document).ready(function () {
    debugger;
    BindDDlSupplierList();

    //$('#SupplierName').val("0").trigger('change');

   
    //var tableLEN = $('#datatable-buttons > tbody >tr').length;
    //debugger
    //if (tableLEN > 0) {
    //    $("#btn_edit").attr("disabled", false);
    //    $("#btn_edit").css("filter", "");
    //    $("#btn_refresh").attr("disabled", false);
    //    $("#btn_refresh").css("filter", "");
    //}
    //else {
        $("#btn_edit").attr("disabled", true);
        $("#btn_edit").css("filter", "grayscale(100%)");
        $("#btn_refresh").attr("disabled", true);
        $("#btn_refresh").css("filter", "grayscale(100%)");
    /*}*/
    //$('#dummydamdam').DataTable({
    //    order: [[0, 'asc']]
    //});
});
/*----------------SEARCH SECTION START----------------- */
function BindDDlSupplierList() {
    var Branch = sessionStorage.getItem("BranchID");
   $("#SupplierName").select2({
        ajax: {
            url: $("#SuppNameList").val(),
            data: function (params) {
                debugger;
                var queryParameters = {
                    SuppName: params.term, // search term like "a" then "an"
                    SuppPage: params.page,
                    BrchID: Branch,
                    
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
function OnChangeSuppName(SuppID) {
    debugger;
    var Supp_id = "";
    var docid = $("#DocumentMenuId").val();
     Supp_id = SuppID.value;
   
    if (Supp_id == "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("#SpanSuppNameErrorMsg").css("display", "block");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "red")
     }
    else {
        var SuppName = $("#SupplierName option:selected").text();
        var SupplierId = $("#SupplierName option:selected").val();
        $("#Hdn_SupplierID").val(SupplierId)
        $("#Hdn_SuppName").val(SuppName)
        
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da")
        
        BindSrcDocPONumberList(Supp_id);
        
    }
}
function BindSrcDocPONumberList(Supp_id) {
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ImportTracking/GetSrcDocPONumberList",
        data: { Supp_id: Supp_id },
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
                    $("#ddl_SrcDocNo option").remove();
                    $("#ddl_SrcDocNo optgroup").remove();
                    $('#ddl_SrcDocNo').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#DocNo").text()}' data-date='${$("#DocDate").text()}'></optgroup>`);
                    for (var i = 0; i < arr.Table.length; i++) {
                        $('#Textddl').append(`<option data-date="${arr.Table[i].po_dt}" value="${arr.Table[i].app_po_no}">${arr.Table[i].app_po_no}</option>`);
                    }
                    $("#SpanSrcDocNoErrorMsg").css("display", "none");
                    var firstEmptySelect = true;
                    $('#ddl_SrcDocNo').select2({
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

                    //$("#qt_date").val("");
                }
            }
        },
    });
}
function OnChangeSrcPONo(SrcPoNo) {
    debugger;
    var RateDecDigit = $("#RateDigit").text();///Rate And Percentage
    var ExchDecDigit = $("#ExchDigit").text();///Rate And Percentage
    var PONo = SrcPoNo.value;
    if (PONo == "---Select---" || PONo=="0") {
        //$("#qt_dt").val("");
        $("#txt_SrcDocDate").val("");
    }
    else {
        $("#hdn_SrcDocNo").val(PONo);
        $("#SpanSrcDocNoErrorMsg").css("display", "none");
        $("#ddl_SrcDocNo").css("border-color", "#ced4da");
        $('[aria-labelledby="select2-ddl_SrcDocNo-container"]').css("border-color", "#ced4da");
    }
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/ImportTracking/GetSrcPONumDetail",
                data: {
                    PONo: PONo
                },
                success: function (data) {
                    // debugger;
                    if (data == 'ErrorPage') {
                        //PO_ErrorPage();
                        return false;
                    }
                    if (data !== null && data !== "") {
                        var arr = [];
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            $("#txt_SrcDocDate").val(arr.Table[0].po_dt);
                            $("#hdn_SrcDocDate").val(arr.Table[0].po_dt);
                            
                            $("#txt_imp_file_no").val(arr.Table[0].imp_file_no);
                            $("#txt_Currency").val(arr.Table[0].curr_name);
                            $("#hdn_Curr_id").val(arr.Table[0].curr_id);
                            if (arr.Table[0].trade_terms == 0) {
                                $("#txt_TradeTerms").val("");
                            }
                            else {
                                $("#txt_TradeTerms").val(arr.Table[0].trade_terms);
                            }
                            
                            $("#txt_CountryOfOrigin").val(arr.Table[0].country_name);
                            $("#txt_PortOfOrigin").val(arr.Table[0].port_origin);
                            $("#txt_Remarks").val(arr.Table[0].po_rem);
                            //$("#divAddNew_IT").css("display", "");
                            //$("#div_AttachId").css("display", "");
                            
                        }
                    }
                    else {
                        //$("#divAddNew_IT").css("display", "none");
                        //$("#div_AttachId").css("display", "none");
                    }
                },
            });
    } catch (err) {
        console.log("OnChangeSrcPONo Error : " + err.message);
    }

}
function CheckSrchValidations() {
    // debugger;
    var ErrorFlag = "N";
    var SuppId = $("#SupplierName").val();
    if (SuppId === "0") {
        $('#SpanSuppNameErrorMsg').text($("#valueReq").text());
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "Red");
        $("#SpanSuppNameErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSuppNameErrorMsg").css("display", "none");
        $("[aria-labelledby='select2-SupplierName-container']").css("border-color", "#ced4da");
    }
    var SrcPoNo = $("#ddl_SrcDocNo").val();
    if (SrcPoNo == null || SrcPoNo == "" || SrcPoNo == "0" || SrcPoNo == "---Select---") {
        $('#SpanSrcDocNoErrorMsg').text($("#valueReq").text());
        $("#ddl_SrcDocNo").css("border-color", "Red");
        $("[aria-labelledby='select2-ddl_SrcDocNo-container']").css("border-color", "Red");
        $("#SpanSrcDocNoErrorMsg").css("display", "block");
       
        ErrorFlag = "Y";
    }
    else {
        $("#SpanSrcDocNoErrorMsg").css("display", "none");
        $("#ddl_SrcDocNo").css("border-color", "#ced4da");
        $("[aria-labelledby='select2-ddl_SrcDocNo-container']").css("border-color", "#ced4da");
    }
    
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function SearchImportTrackListData() {
    debugger;
    try {
        if (CheckSrchValidations() == false) {
            return false
        }
        else {
            
            var SuppId = $("#SupplierName option:selected").val();
            var SrcDocNo = $("#ddl_SrcDocNo option:selected").val();
            
            var DocumentMenuId = $("#DocumentMenuId").val();
            var UserId = $("#User_ID").val();
            var CreateId = $("#Create_ID").val();
            $.ajax({
                type: "POST",
                url: "/ApplicationLayer/ImportTracking/SearchImpTrckAutoAndNewDetail",
                data: {
                    SuppId: SuppId,
                    SrcDocNo: SrcDocNo
                    
                },
                success: function (data) {
                    debugger;
                    $('#tbody_ImpTrackList').html(data);
                    
                    /* var tableLEN = $('#datatable-buttons').rows().data().toArray().length;*/
                    var tableLEN = $('#datatable-buttons > tbody >tr').length;
                    debugger
                    if (tableLEN > 0) {
                        /*if (CreateId == UserId) {*/
                            $("#btn_edit").attr("disabled", false);
                            $("#btn_edit").css("filter", "");
                            $("#btn_refresh").attr("disabled", false);
                            $("#btn_refresh").css("filter", "");
                        //}
                        //else {
                        //    $("#btn_edit").attr("disabled", true);
                        //    $("#btn_edit").css("filter", "");
                        //    $("#btn_refresh").attr("disabled", true);
                        //    $("#btn_refresh").css("filter", "");
                        //}
                    }
                    else {
                        $("#btn_edit").attr("disabled", true);
                        $("#btn_edit").css("filter", "grayscale(100%)");
                        $("#btn_refresh").attr("disabled", true);
                        $("#btn_refresh").css("filter", "grayscale(100%)");
                    }
                    $('#ListFilterData').val(SuppId + ',' + SrcDocNo );
                },
                error: function OnError(xhr, errorType, exception) {
                    debugger;
                }
            });
        }
        
    } catch (err) {
        debugger;
        console.log("ItemSetup Error : " + err.message);

    }
}

/*----------------SEARCH SECTION END----------------- */

/*----------------ADD NEW DOCUMENT DETAIL SECTION START----------------- */
function AddNewDocDetail() {
    debugger
    if (AddNewDocValidation() == false) {
        return false
    }
    else {
        var SrcDocPONo = $("#hdn_SrcDocNo").val();
        var Date = $("#txt_Date").val().split("-").reverse().join("-");
        var DocNo = $("#txt_DocNo").val();
        var DocDate = $("#txt_DocDate").val().split("-").reverse().join("-");
        var Status = $("#txt_Status").val();
        var FlagIdentify = "N";
        var TblLen = $('#datatable-buttons').DataTable().rows().data().toArray().length;

        //var max = 0;
        //var max1 = 0;
        //$('#datatable-buttons tbody tr').each(function () {
        //    debugger
        //    //max = max + 1;
        //    var clickedrow = $(this).closest("tr");
        //    max = parseInt(clickedrow.find("#spanrowid").text());
        //    if (max > max1)
        //        max1 = max;
        //});
        //alert(max1);
        //rowIdx = max1 + 1;


        if (TblLen == 1) {
            /*var sno = $("#spanrowid").text();*/
            var sno = $("#spanrowid").val();
            if (sno != "" && sno != null) {
                rowIdx = TblLen + 1;
            }
            else {
                rowIdx = TblLen;
            }
        }
        else {
            rowIdx = TblLen + 1;
        }

        
        
        var t = $('#datatable-buttons').DataTable();
        debugger;
        t.row.add([
            `<td><div class="red center"><i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" onclick="deleteRow(this, event)"></i></div></td>`,
            `<td><div class="center edit_icon"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" onclick="editItem(this, event)" title="${$("#Edit").text()}"></i></div></td>`,
            `<td id=""><span id="tbl_Date">${Date}</span></td>`,
            `<td id=""><span id="tbl_DocNo">${DocNo}</span>
            <input type="hidden" id="spanrowid" value="${rowIdx}" style="display:none;" >
            <input type="hidden" id="tbl_SrcDocPONo" value="${SrcDocPONo}" style = "display:none;" >
            <input type="hidden" id="tbl_Flag" value="${FlagIdentify}" style = "display:none;" >
            </td>`,
            `<td id=""><span id="tbl_DocDate">${DocDate}</span></td>`,
            `<td id=""><span id="tbl_Status">${Status}</span></td>`,
            `<td id="tbl_Attach" class="center sorting_1 pt-1">
            <button type="button" id="btn_tblattach" class="calculator" data-toggle="modal" onclick="OnClkViewAttachDtlRowWise(this,event,'View')" data-target="#PreviewITAttachment"
            data-backdrop="static" data-keyboard="false"><i type="" class="fa fa-paperclip tracking-attach" onclick="" aria-hidden="true" title="${$("#span_Attachment").text()}"></i></button>
            </td>`,
           
        ]).draw();
      /*  `<td><div class=""></div></td>`,*/
        //hdn_attatchment_list
        $("#Temphdn_Doc_AttachDetailTbl> tbody > tr").each(function () {
            var row = $(this);
            var add_attachid = row.find("#tmp_srnoattachid").val();
            var add_fileName = row.find("#tmp_fileName").val();
            var add_filePath = row.find("#tmp_filePath").val();
            $("#hdn_attatchment_list >tbody").append(`<tr>
                            <td><input type="text" id="attch_Id" value='${add_attachid}'></td>
                            <td><input type="text" id="attch_fileName" value='${add_fileName}'></td>
                            <td><input type="text" id="attch_filePath" value='${add_filePath}'></td>
                            
                        </tr>`);
            
        });
        ClearAddNewDocDetails();
        $("#SupplierName").attr("disabled", true);
        $("#ddl_SrcDocNo").attr("disabled", true);
        $("#BtnSearch").hide();
        $(".fileinput-remove").click();
        /*`<td><span id ="spanrowid">${rowIdx}</span></td>`,*/
        /*<input type="hidden" id="spanrowid" value="${rowIdx}" style="display:none;" >*/
        //<td id="tbl_Attach" class="center sorting_1 pt-1"><div><i type="submit" id="BtnPaperClip" class="fa fa-paperclip tracking-attach"
        //    onclick="OnClkViewAttachDtlRowWise(this,event)" data-target="#ITAttachment" aria-hidden="true" title="${$(" #Span_Attachment").text()}">
        //    </i></div></td >,

    }
}
function AddNewDocValidation() {
    debugger
    var ErrorFlag = "N";
    var Date = $("#txt_Date").val();
    if (Date == null || Date == "" || Date == "0") {
        $("#txt_Date").css("border-color", "Red");
        $('#SpanDateErrorMsg').text($("#valueReq").text());
        $("#SpanDateErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanDateErrorMsg").css("display", "none");
        $("#txt_Date").css("border-color", "#ced4da");
    }
    var DocNo = $("#txt_DocNo").val();
    if (DocNo == null || DocNo == "" || DocNo == "0") {
        $("#txt_DocNo").css("border-color", "Red");
        $('#SpanDocNoErrorMsg').text($("#valueReq").text());
        $("#SpanDocNoErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanDocNoErrorMsg").css("display", "none");
        $("#txt_DocNo").css("border-color", "#ced4da");
    }
    var DocDate = $("#txt_DocDate").val();
    if (DocDate == null || DocDate == "" || DocDate == "0") {
        $("#txt_DocDate").css("border-color", "Red");
        $('#SpanDocDateErrorMsg').text($("#valueReq").text());
        $("#SpanDocDateErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanDocDateErrorMsg").css("display", "none");
        $("#txt_DocDate").css("border-color", "#ced4da");
    }
    var Status = $("#txt_Status").val();
    if (Status == null || Status == "" || Status == "0") {
        $("#txt_Status").css("border-color", "Red");
        $('#SpanStatusErrorMsg').text($("#valueReq").text());
        $("#SpanStatusErrorMsg").css("display", "block");
        ErrorFlag = "Y";
    }
    else {
        $("#SpanStatusErrorMsg").css("display", "none");
        $("#txt_Status").css("border-color", "#ced4da");
    }
    if (ErrorFlag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function ClearAddNewDocDetails() {
    $("#txt_Date").val("");
    $("#txt_DocNo").val("");
    $("#txt_DocDate").val("");
    $("#txt_Status").val("");
}
function OnChangeDate(Date) {
    debugger;
    var DT = Date.value;
    var Txtdate = $('#txt_Date').val();
    //var date = $("#txt_Date").val().replace("-", "");
    //var date1 = date.replace("-", "");
    if (Txtdate == "") {
        $('#txt_Date').css("border-color", "red");
        $('#SpanDateErrorMsg').text($("#valueReq").text());
        $("#SpanDateErrorMsg").css("display", "block");
    }
    else {
        $('#txt_Date').css("border-color", "#ced4da");
        $("#SpanDateErrorMsg").css("display", "none");
    }
}
function OnChangeDocNo() {
    debugger;
    var DocNo = $('#txt_DocNo').val();
    if (DocNo == "") {
        $('#txt_DocNo').css("border-color", "red");
        $('#SpanDocNoErrorMsg').text($("#valueReq").text());
        $("#SpanDocNoErrorMsg").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_DocNo').css("border-color", "#ced4da");
        $("#SpanDocNoErrorMsg").css("display", "none");
    }
}
function OnChangeDocDate() {
    debugger;
    var DocDate = $('#txt_DocDate').val();
    if (DocDate == "") {
        $('#txt_DocDate').css("border-color", "red");
        $('#SpanDocDateErrorMsg').text($("#valueReq").text());
        $("#SpanDocDateErrorMsg").css("display", "block");
        Errorflag = "Y";
    }
    else {
        $('#txt_DocDate').css("border-color", "#ced4da");
        $("#SpanDocDateErrorMsg").css("display", "none");
    }
}
function OnChangeStatus() {
    debugger;
    var Discusremarks = $('#txt_Status').val();
    if (Discusremarks == "") {
        $('#txt_Status').css("border-color", "red");
        $('#SpanStatusErrorMsg').text($("#valueReq").text());
        $("#SpanStatusErrorMsg").css("display", "block");
        Errorflag = "Y";
    }
    else {
        
        $('#txt_Status').css("border-color", "#ced4da");
        $("#SpanStatusErrorMsg").css("display", "none");
    }
}
function RemoveValidateOnEdit() {
    $('#txt_Date').css("border-color", "#ced4da");
    $("#SpanDateErrorMsg").css("display", "none");
    $('#txt_DocNo').css("border-color", "#ced4da");
    $("#SpanDocNoErrorMsg").css("display", "none");
    $('#txt_DocDate').css("border-color", "#ced4da");
    $("#SpanDocDateErrorMsg").css("display", "none");
    $('#txt_Status').css("border-color", "#ced4da");
    $("#SpanStatusErrorMsg").css("display", "none");
}

function editItem(el, e) {
    debugger;
    var EditFlag = "Yes";
   
    RemoveValidateOnEdit();
    var rowJavascript = el.parentNode.parentNode;
    var clickedrow = $(e.target).closest("tr");
    $('#hdnUpdateInTable').val(rowJavascript._DT_CellIndex.row); 
    var TblSrNo = clickedrow.find("#spanrowid").val();
    var TblDate = clickedrow.find("#tbl_Date").text().split("-").reverse().join("-");
    var TblDocNo = clickedrow.find("#tbl_DocNo").text();
    var TblDocDate = clickedrow.find("#tbl_DocDate").text().split("-").reverse().join("-");
    var TblStatus = clickedrow.find("#tbl_Status").text();
    
    $("#txt_Date").val(TblDate);
    $("#txt_DocNo").val(TblDocNo);
    $("#txt_DocDate").val(TblDocDate);
    $("#txt_Status").val(TblStatus);
    $("#hdn_TblEditFlag").val(EditFlag);
    $("#hdn_EditSrNoforAttch").val(TblSrNo);

    $("#divAddNew_IT").hide();
    $("#divUpdate_IT").show();
    $("#datatable-buttons >tbody >tr").each(function (i, rows) {
        var clickedrow = $(this);
        clickedrow.find("#delBtnIcon").css("filter", "grayscale(100%)");
        clickedrow.find("#delBtnIcon").removeClass("deleteIcon");
        clickedrow.find("#delBtnIcon").attr('onclick', '');

    });
    $("#datatable-buttons >tbody >tr").each(function (i, rows) {
        var clickedrow = $(this);
        clickedrow.find("#btn_tblattach").css("filter", "grayscale(100%)");
        //clickedrow.find("#btn_tblattach").removeClass("deleteIcon");
        clickedrow.find("#btn_tblattach").attr('onclick', '');
        clickedrow.find("#btn_tblattach").attr("data-target", "");

    });
   
    //Editattachment(clickedrow)
}
function OnClickItemUpdateBtn(e) {
    debugger;
    //var clickedrow = $(e.target).closest("tr");
    //var SNo = clickedrow.find("#spanrowid").val();
    if (AddNewDocValidation() == false) {
        return false
    }
    else {

        var SrcDocPONo = $("#hdn_SrcDocNo").val();
        var Date = $("#txt_Date").val().split("-").reverse().join("-");
        var DocNo = $("#txt_DocNo").val();
        var DocDate = $("#txt_DocDate").val().split("-").reverse().join("-");
        var Status = $("#txt_Status").val();
        var FlagIdentify = "N";

        debugger;

        var tableRow = $('#hdnUpdateInTable').val();
        tableRow = parseInt(tableRow) + 1;

        //--------------------------------
        
        var pageParamTable = $('#datatable-buttons').DataTable();
        var tableRowdt = pageParamTable.row($('#hdnUpdateInTable').val());
        var rData = [
            `<td><div class="red center"><i class="fa fa-trash" aria-hidden="true" id="delBtnIcon" onclick="deleteRow(this, event)"></i></div></td>`,
            `<td><div class="center edit_icon"><i class="fa fa-edit" aria-hidden="true" id="editBtnIcon" onclick="editItem(this, event)" title="${$("#Edit").text()}"></i></div></td>`,
            `<td id=""><span id="tbl_Date">${Date}</span></td>`,
            `<td id=""><span id="tbl_DocNo">${DocNo}</span>
            <input type="hidden" id="spanrowid" value="${tableRow}" style="display:none;" >
            <input type="hidden" id="tbl_SrcDocPONo" value="${SrcDocPONo}" style = "display:none;" >
            <input type="hidden" id="tbl_Flag" value="${FlagIdentify}" style = "display:none;" >
            </td>`,
            `<td id=""><span id="tbl_DocDate">${DocDate}</span></td>`,
            `<td id=""><span id="tbl_Status">${Status}</span></td>`,
            `<td id="tbl_Attach" class="center sorting_1 pt-1">
            <button type="button" id="btn_tblattach" class="calculator" data-toggle="modal" onclick="OnClkViewAttachDtlRowWise(this,event,'View')" data-target="#PreviewITAttachment"
            data-backdrop="static" data-keyboard="false"><i type="" class="fa fa-paperclip tracking-attach" onclick="" aria-hidden="true" title="${$("#span_Attachment").text()}"></i></button>
            </td>`,
            
        ];
        pageParamTable
            .row(tableRowdt)
            .data(rData)
            .draw();
       /* `<td><div class=""></div></td>`,*/
        /*--------Update attchement work start-----------------*/
        var Updt_srno = $("#hdn_EditSrNoforAttch").val();
        var Updt_date = $("#txt_Date").val().split("-").reverse().join("-");
        var Updt_date1 = Updt_date.replace("-", "");
        var Updt_date1 = Updt_date1.replace("-", "");
        //var Updt_SrNoattchId = srno + '' + Updt_srno;
        //len = $("#Temphdn_Doc_AttachDetailTbl >tbody >tr td #tmp_srnoattachid[value=" + Updt_SrNoattchId + "]").closest('tr').length;
        //$("#Temphdn_Doc_AttachDetailTbl >tbody >tr td #tmp_srnoattachid[value=" + Updt_SrNoattchId + "]").closest('tr').remove();
        //for (var y = 0; y < arr1.length; y++) {

        //    var TmpFileName = arr1[y].file_name;
        //    var TmpFilePath = arr1[y].file_path;

        //    $("#Temphdn_Doc_AttachDetailTbl >tbody").append(`<tr>
        //                    <td><input type="text" id="tmp_srnoattachid" value='${srnodateAttchId}'></td>
        //                    <td><input type="text" id="tmp_fileName" value='${TmpFileName}'></td>
        //                    <td><input type="text" id="tmp_filePath" value='${TmpFilePath}'></td>
                            
        //                </tr>`);
        //}
        /*--------Update attchement work End-----------------*/
        /* `<td><span id ="spanrowid">${tableRow}</span></td>`,*/
      /*  `<td hidden='hidden'><input type='hidden' id='spanrowid' value="${tableRow}" /></td>`,*/
        /*<input type="hidden" id="spanrowid" value="${tableRow}" style="display:none;" >*/
        ClearAddNewDocDetails();
        $("#divAddNew_IT").show();
        $("#divUpdate_IT").hide();
        $("#datatable-buttons >tbody >tr").each(function (i, rows) {
            debugger;
            var clickedrow = $(this);
            clickedrow.find("#delBtnIcon").css("filter", "");
            clickedrow.find("#delBtnIcon").addClass("deleteIcon");
            clickedrow.find("#delBtnIcon").attr('onclick', 'deleteRow(this, event)');

        });
        
    }
    
    
}
function deleteRow(el, e) {
    debugger;
    //var i = el.parentNode.parentNode.rowIndex - 1;
    var i = el.parentNode.parentNode.parentNode.rowIndex - 1;

    var clickedrow = $(e.target).closest("tr");
    
    var table = $('#datatable-buttons').DataTable();
    table.row(i).remove().draw(false);
   
    
    $("#divAddNew_IT").show();
    $("#divUpdate_IT").hide();
    
    SerialNoAfterDelete();
   //var tableLEN = $('#datatable-buttons').DataTable().rows().data().toArray().length;
   //if (tableLEN == 0) {
   //    $("#BtnSearch").show();
   //    $("#SupplierName").attr("disabled", false);
   //    $("#ddl_SrcDocNo").attr("disabled", false);
   // }
}
function SerialNoAfterDelete() {
    var SerialNo = 0;
    debugger

    $("#datatable-buttons >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        SerialNo = SerialNo + 1;
        /*currentRow.find("#spanrowid").text(SerialNo);*/
        currentRow.find("#spanrowid").val(SerialNo);

    });
}
/*----------------ADD NEW DOCUMENT DETAIL SECTION END----------------- */
/*-----------------INSERT DATA (SAVE)  SECTION START*/
function InsertImportNewDocDetail() {
    debugger;
    //var btn = $("#hdnsavebtn").val();
    //$("#hdn_TransType").val("Update");
   
    var FinalDocDetail = [];
    FinalDocDetail = InsertDocumentDetails();
    $('#hdnNewDocDetail').val(JSON.stringify(FinalDocDetail));

    /*----- Attatchment start--------*/
    $(".fileinput-upload").click();/*To Upload Img in folder*/
    FinalAttatchmentDetail = Cmn_InsertAttatchmentDetails();
    /*----- Attatchment End--------*/
    /*----- Attatchment start--------*/
    var ItemAttchmentDt = JSON.stringify(FinalAttatchmentDetail);
    $('#hdn_Attatchment_details').val(ItemAttchmentDt);
    /*----- Attatchment End--------*/
    var SupplierId = $("#SupplierName option:selected").val();
    $("#Hdn_SupplierID").val(SupplierId)
    var srcdocno = $("#ddl_SrcDocNo option:selected").val()
    $("#Hdn_SupplierID").val(SuppID)
    $("#hdn_SrcDocNo").val(srcdocno);
    
    return true;
};
function InsertDocumentDetails() {
    debugger;
    
    var docAddList = [];
    $("#datatable-buttons >tbody >tr").each(function (i, row) {
        debugger;
        var SRno = "";
        var Date = "";
        var DocNo = "";
        var DocDate = "";
        var Status = "";
        var Entryflag = "";
        var TSRno = "";
        var TDate = "";
        var TDocNo = "";
        var TDocDate = "";
        var TStatus = "";
        var TEntryflag = "";

        var currentRow = $(this);
        TSRno = currentRow.find("#spanrowid").val();
        TDate = currentRow.find("#tbl_Date").text().split("-").reverse().join("-");
        TDocNo = currentRow.find("#tbl_DocNo").text().trim();
        TDocDate = currentRow.find("#tbl_DocDate").text().split("-").reverse().join("-");
        TStatus = currentRow.find("#tbl_Status").text();
        TEntryflag = currentRow.find("#tbl_Flag").val();

        SRno = TSRno.trim();
        Date = TDate.trim();
        DocNo = TDocNo;
        DocDate = TDocDate.trim();
        Status = TStatus.trim();
        Entryflag = TEntryflag.trim();


        docAddList.push({ SRno: SRno, Date: Date, DocNo: DocNo, DocDate: DocDate, Status: Status, Entryflag: Entryflag});
    });
    return docAddList;
};
function ImpTrckDtlEnable_EditBtn() {
    debugger;
    $("#SupplierName").attr("disabled", true);
    $("#ddl_SrcDocNo").attr("disabled", true);
    $("#txt_Date").attr("disabled", false);
    $("#txt_DocNo").attr("disabled", false);
    $("#txt_DocDate").attr("disabled", false);
    $("#txt_Status").attr("disabled", false);
    $("#divAddNew_IT").css("display", "");
    $("#div_AttachId").css("display", "");
    $("#BtnSearch").hide();
}

/*-----------------Attachment Section----------------*/
//function OnClickOpenAttach() {
//    debugger
//    $(".collapseFive").show();
//}
function GetDataPreviewImages(responseText) {
    debugger;
    var detail = responseText;
    var date = $("#txt_Date").val().replace("-", "");
    var date1 = date.replace("-", "");
     var tableLEN = $('#datatable-buttons > tbody >tr').length
    var NextLen = tableLEN + 1;
    var srnodateAttchId = NextLen + '' + date1
    var AttachTbl = new Array();
    AttachTbl = responseText
    var arr = [];
    var arr1 = [];
    //DataTable = JSON.parse(AttachTbl);
    arr = JSON.parse(AttachTbl);
    arr1 = JSON.parse(arr);
   /* arr1 = JSON.stringify(responseText)*/
    if (arr1.length > 0) {
        len = $("#Temphdn_Doc_AttachDetailTbl >tbody >tr td #tmp_srnoattachid[value=" + srnodateAttchId + "]").closest('tr').length;
        $("#Temphdn_Doc_AttachDetailTbl >tbody >tr td #tmp_srnoattachid[value=" + srnodateAttchId + "]").closest('tr').remove();
        for (var y = 0; y < arr1.length; y++) {

             var TmpFileName = arr1[y].file_name;
        var TmpFilePath = arr1[y].file_path;

            $("#Temphdn_Doc_AttachDetailTbl >tbody").append(`<tr>
                            <td><input type="text" id="tmp_srnoattachid" value='${srnodateAttchId}'></td>
                            <td><input type="text" id="tmp_fileName" value='${TmpFileName}'></td>
                            <td><input type="text" id="tmp_filePath" value='${TmpFilePath}'></td>
                            
                        </tr>`);
            
            }
    }
    $("#AttachmentSaveAndExitBtn").attr("data-dismiss", "modal");
    
}
function TempSaveAndExitAttch(Iflag) {
    debugger;
    $("#hdn_TransType").val("Save")
    var Txtdate = $('#txt_Date').val();
    //var date = $("#txt_Date").val().replace("-", "");
    //var date1 = date.replace("-", "");
    if (Txtdate == "") {
        $('#txt_Date').css("border-color", "red");
        $('#SpanDateErrorMsg').text($("#valueReq").text());
        $("#SpanDateErrorMsg").css("display", "block");
        return false;
    }
    else {
        $('#txt_Date').css("border-color", "#ced4da");
        $("#SpanDateErrorMsg").css("display", "none");
        $(".fileinput-upload").click();
    }
 }
function OnClkViewAttachDtlRowWise(el, e, Attachflag) {
    debugger;
    
       var NewArr = new Array();
        var Editflag = $("#hdn_TblEditFlag").val();

    if (Attachflag == "New" && Editflag == "")
      {
            if (AddNewDocValidation() == false) {
                $("#btn_headerattach").attr("data-target", "");
                return false
            }
            else {
                $("#btn_headerattach").attr("data-target", "#ITAttachment");
                var IsDisabled = false;
                var AttachIdSRNoTBlDt = "";
                $(".fileinput-upload").click();
            }
        }
        else {
            if (Attachflag == "View" && Editflag == "") {
                var IsDisabled = true;
                var Attachflag = 'View';
            }
            if (Attachflag == "New" && Editflag == "Yes") {
                var IsDisabled = false;
                var Attachflag = 'New';
                $("#btn_headerattach").attr("data-target", "#ITAttachment");
            }
            //var IsDisabled = true;
            //var Attachflag = 'View';
        if (Editflag == "Yes") {
            var SrNo = $("#hdn_EditSrNoforAttch").val();
            var TblDate = $("#txt_Date").val();
        }
        else {
            var clickedrow = $(e.target).closest("tr");
            var SrNo = clickedrow.find("#spanrowid").val()
            var TblDate = clickedrow.find("#tbl_Date").text().split("-").reverse().join("-");
        }
            //var clickedrow = $(e.target).closest("tr");
            //var SrNo = clickedrow.find("#spanrowid").val()
            //var TblDate = clickedrow.find("#tbl_Date").text().split("-").reverse().join("-");
            var date = TblDate.replace("-", "");
            var date1 = date.replace("-", "");
            var AttachIdSRNoTBlDt = SrNo + '' + date1;
            var templen = $("#Temphdn_Doc_AttachDetailTbl> tbody > tr").length;
            if (templen > 0) {
                $("#Temphdn_Doc_AttachDetailTbl tbody tr td #tmp_srnoattachid[value=" + AttachIdSRNoTBlDt + "]").closest("tr").each(function () {
                    var row = $(this);
                    var List = {};
                    List.id = row.find("#tmp_srnoattachid").val();
                    List.file_name = row.find('#tmp_fileName').val();
                    List.file_path = row.find('#tmp_filePath').val();
                    //list.file_def = 'Y';
                    NewArr.push(List);
                });
            }
        }


        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/ImportTracking/GetAttachmentDetail",
            data: {
                AttachIdSRNoTBlDt: AttachIdSRNoTBlDt,
                AttachmentListwithPageData: JSON.stringify(NewArr),
                IsDisabled: IsDisabled,
                Attachflag: Attachflag,
                Editflag: Editflag

                //DocumentMenuId: DocumentMenuId
            },
            success: function (data) {
                debugger;
                if (Attachflag == "View" && Editflag == "") {
                    $("#PreviewShowItAttachment").html(data);

                    $("#RemoveAttatchbtn").attr("disabled", true);
                }
                if (Attachflag == "New" && Editflag == "Yes") {
                    $("#ShowItAttachment #PartialImageBind").html(data);
                }

            }
        });
    //}
        
}
function Editattachment(clickedrow) {
    //var clickedrow = $(e.target).closest("tr");
    var NewArr = new Array();
    var Attachflag = "New"
        var IsDisabled = false;
        var AttachIdSRNoTBlDt = "";
        //$(".fileinput-upload").click();
   
    var SrNo = clickedrow.find("#spanrowid").val()
    var TblDate = clickedrow.find("#tbl_Date").text().split("-").reverse().join("-");
    var date = TblDate.replace("-", "");
    var date1 = date.replace("-", "");
    var AttachIdSRNoTBlDt = SrNo + '' + date1;
    var templen = $("#Temphdn_Doc_AttachDetailTbl> tbody > tr").length;
    if (templen > 0) {
        $("#Temphdn_Doc_AttachDetailTbl tbody tr td #tmp_srnoattachid[value=" + AttachIdSRNoTBlDt + "]").closest("tr").each(function () {
            var row = $(this);
            var List = {};
            List.id = row.find("#tmp_srnoattachid").val();
            List.file_name = row.find('#tmp_fileName').val();
            List.file_path = row.find('#tmp_filePath').val();
            //list.file_def = 'Y';
            NewArr.push(List);
        });
    }
    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/ImportTracking/GetAttachmentDetail",
        data: {
            AttachIdSRNoTBlDt: AttachIdSRNoTBlDt,
            AttachmentListwithPageData: JSON.stringify(NewArr),
            IsDisabled: IsDisabled,
            Attachflag: Attachflag

            //DocumentMenuId: DocumentMenuId
        },
        success: function (data) {
            debugger;
            $("#PreviewShowItAttachment").html(data);

            //$("#RemoveAttatchbtn").attr("disabled", true);

        }
    });
}

/*****----------------------------PDF Work on 03-12-2025-----------------------------******/

function IT_GetAllDocumentDetailPDF(e) {
    debugger;
    
   
    
    //var clickedrow = $(e.target).closest("tr");
    //var TblDocNo = clickedrow.find("#tbl_DocNo").text();
    //var TblDocDate = clickedrow.find("#tbl_DocDate").text().split("-").reverse().join("-");
    //var Invno = TblDocNo.split("/");
    //var INo = Invno[3];
    //var code = INo.split("0");
    //var Doccode = code[0];
    ////$("#hdnDocCode_InvDtlPopupPDF").val(Doccode);
    ////clickedrow.find("#hdnDocCode").text(Doccode);
    //if (Doccode == "IPO" || Doccode == "GE" || Doccode == "QC" || Doccode == "GRN" || Doccode == "IPI")
    //    window.location.href = "/ApplicationLayer/ImportTracking/GetAllDocPdfData?Dt =""invNo=" + TblDocNo + "&invDate=" + TblDocDate + "&dataType=" + Doccode;

    //$('form').submit();
}
//$("#btnShowPdf").click(function () {
//    $("#pdfFrame").attr("src", "/UploadedFiles/Docs/sample.pdf");
//    $("#pdfModal").show();
//});