$(document).ready(function () {
    debugger;
   
    ReplicateWith();
    $("#divUpdate").hide();
    BindProductNameDDL();
    //BindItemNameDDL();
    $("#divUpdate").hide();
    $("#ddl_opName").select2({ 

    });
    $("#ddl_ShopfloorName").select2();
    debugger;
    $("#bom_list_tbody #datatable-buttons tbody").bind("dblclick", function (e) {
        debugger;
        try {
            debugger;
            var ListFilterData = $('#ListFilterData').val();
            var WF_status = $("#WF_status").val();
            var clickedrow = $(e.target).closest("tr");
            var product_id = clickedrow.children("#hdn_product_id").text();
            var product_name = clickedrow.children("#uom_name").text();
            var rev_no = clickedrow.children("#hdn_rev_no").text();
            window.location = "/ApplicationLayer/BillofMaterial/dbClickEdit/?product_id=" + product_id + "&rev_no=" + rev_no + "&WF_status=" + WF_status+ "&ListFilterData=" + ListFilterData;
        }
        catch (err) {
            debugger;
        }
    });
    $("#tbladd tbody").bind("click", function (e) {
        debugger;
        var clickedrow = $(e.target).closest("tr");
        
        var CheckRow = clickedrow.parent().parent()[0] == null ? "" : clickedrow.parent().parent()[0].id;
        $("#tbladd tbody tr").css("background-color", "#ffffff");
        if (CheckRow != "tbladd") {
            var parentrow = clickedrow.parent().parent().parent().parent();
            var nearestRow = clickedrow.parent().parent().find(" tr");
            $("#tbladd tbody tr").css("background-color", "#ffffff");
            $(parentrow).css("background-color", "rgba(38, 185, 154, .16)");
            $(nearestRow).css("background-color", "rgba(38, 185, 154, .03)");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
        }
        else {
            var ChildRow = clickedrow.children().children().children().children();
            $(ChildRow).css("background-color", "rgba(38, 185, 154, .03)");
            $(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");
        }
         
         
       
    });
    //ListRowHighLight();
  
    $("#datatable-buttons >tbody").bind("click", function (e) {
      //  debugger;
        var clickedrow = $(e.target).closest("tr");
        var product_id = clickedrow.children("#hdn_product_id").text();
        var rev_no = clickedrow.children("#hdn_rev_no").text();
        var docno = product_id + '_' + rev_no;
        var bomdate = clickedrow.children("#create_dt").text();
        Doc_Date = bomdate.split(' ')[0];
        if (Doc_Date.split('-')[2].length == 4) {
            Doc_Date = Doc_Date.split('-').reverse().join('-');
        }
        //var date = bomdate.split("-");
        //var FDate = date[2] + '-' + date[1] + '-' + date[0];
        var Doc_id = $("#DocumentMenuId").val();
        
       // debugger;
        //$("#datatable-buttons >tbody >tr").css("background-color", "#ffffff");
        //$(clickedrow).css("background-color", "rgba(38, 185, 154, .16)");

        var Doc_Status = clickedrow.children("#Doc_Status").text();
        Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
        GetWorkFlowDetails(docno, Doc_Date, Doc_id, Doc_Status);
        $("#hdDoc_No").val(docno);
    });
    

    var ProductID = $("#hdn_product_id").val();
    var ProductName = $("#hdn_product_name").val();
    if (ProductID != "" && ProductName != "") {
        $('#ddl_ProductName').val(ProductID).trigger('change.select2');
        $('#ddl_ProductName').empty().append('<option value=' + ProductID + ' selected="selected">' + ProductName + '</option>');
    }
    var ValDigit = $("#ValDigit").text();
    var totalVal = $("#TotalVal").text();
    $("#TotalVal").text((parseFloat(totalVal).toFixed(ValDigit)));

    HideOperationNameAfterFG();
   // ddl_OPNameChange();
    debugger;
    TableShorting();
    var ddl_op_ID = $("#ddl_opName").val();
    ItemTypeDdlBind(ddl_op_ID);
    GetViewDetails();
    $("#ddlReplicateWith").attr("disabled", true);
});

//------------------Work Flow --------------------------

function GetViewDetails() {
    if ($("#ddl_ProductName").val() != null) {
        var DocNo = $("#ddl_ProductName").val().trim() + "_" + $("#txt_RevisionNumber").val().trim();
        $("#hdDoc_No").val(DocNo);
    }
    SetItemNameSelectOnly();

}
function SetItemNameSelectOnly() {
    $("#ddl_ItemName").html(`<optgroup class='def-cursor' id="Textddl_itemName" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'>
            <option data-uom="0" value="0">---Select---</option>
                </optgroup>`);
    $("#ddl_ItemName").select2();
    $("#ddl_opName").attr("onchange","");
    $("#ddl_opName").val(0).change();
    $("#ddl_opName").attr("onchange", "ddl_OPNameChange()");
}
function CmnGetWorkFlowDetails(e) {
    //debugger;  //changing td to id by SM
    var clickedrow = $(e.target).closest("tr");
    var Doc_No = clickedrow.children("#hdn_product_id").text() + '_' + clickedrow.children("#rev_no").text();
    var Doc_Date = clickedrow.children("#create_dt").text();
    var Doc_id = $("#DocumentMenuId").val();
    $("#hdDoc_No").val(Doc_No);
 //   debugger;
    Doc_Date = Doc_Date.split(' ')[0];
    if (Doc_Date.split('-')[2].length == 4) {
        Doc_Date = Doc_Date.split('-').reverse().join('-');
    }

    var Doc_Status = clickedrow.children("#Doc_Status").text();
    Doc_Status = Doc_Status == "Drafted" ? "D" : (Doc_Status == "Forwarded" ? "F" : "N");
    GetWorkFlowDetails(Doc_No, Doc_Date, Doc_id, Doc_Status);
}
function ForwardBtnClick() {
    debugger;
    //var OrderStatus = "";
    //OrderStatus = $('#hdn_bom_status').val().trim();
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
    //    var DocNo = $("#ddl_ProductName").val().trim() + "_" + $("#txt_RevisionNumber").val().trim();
    //    Cmn_GetForwarderList(Doc_ID, DocNo);

    //}
    //else {
    //    $("#Btn_Forward").attr("data-target", "");
    //    $("#Btn_Forward").attr('onclick', '');
    //    $("#Btn_Forward").css({ "-webkit-filter": "grayscale(100%)", "filter": "grayscale(100%);" });
    //}
    //return false;

    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
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
                OrderStatus = $('#hdn_bom_status').val().trim();
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
                    var DocNo = $("#ddl_ProductName").val().trim() + "_" + $("#txt_RevisionNumber").val().trim();
                    Cmn_GetForwarderList(Doc_ID, DocNo);

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
    DocNo = $("#ddl_ProductName").val().trim() + "_" + $("#txt_RevisionNumber").val().trim();
    DocDate = $("#hdn_bom_DocDate").val();
    docid = $("#DocumentMenuId").val();
    var WF_status1 = $("#WF_status1").val();
    var TrancType = (DocNo + ',' + DocDate + ',' + WF_status1)
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
        var pdfAlertEmailFilePath = "BillOfMaterial_" + GetDatetimeYYYYMMDDHHMMSS() + ".pdf";
        var pdfAlertEmailFilePath1 = await Cmn_GetPdfFilePathToSendonEmailAlert(DocNo, DocDate, pdfAlertEmailFilePath, docid, fwchkval, "/ApplicationLayer/BillofMaterial/SavePdfDocToSendOnEmailAlert");
        if (pdfAlertEmailFilePath1 == "" || pdfAlertEmailFilePath1 == null) {
            pdfAlertEmailFilePath = "";
        }
    }
    if (fwchkval === "Forward") {
        if (fwchkval != "" && DocNo != "" && DocDate != "" && level != "") {
            debugger;
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, level, forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/BillofMaterial/ToRefreshByJS?TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
        }
    
    if (fwchkval === "Approve") {
        var list = [{ DocNo: DocNo, DocDate: DocDate, A_Status: "Approve", A_Level: $("#hd_currlevel").val(), A_Remarks: Remarks }];
        var AppDtList = JSON.stringify(list);
        window.location.href = "/ApplicationLayer/BillofMaterial/ApproveDocByWorkFlow?AppDtList=" + AppDtList + "" + "&WF_status1=" + WF_status1;
    }
    if (fwchkval === "Reject") {
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/BillofMaterial/ToRefreshByJS?TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
        }
    
    if (fwchkval === "Revert") {
        debugger
        if (fwchkval != "" && DocNo != "" && DocDate != "") {
            await Cmn_InsertDocument_ForwardedDetail1(DocNo, DocDate, docid, $("#hd_currlevel").val(), forwardedto, fwchkval, Remarks, pdfAlertEmailFilePath);
            mailerror = $("#MailError").val();
            window.location.href = "/ApplicationLayer/BillofMaterial/ToRefreshByJS?TrancType=" + TrancType + "&Mailerror=" + mailerror;
        }
        }
    
}
//function GetPdfFilePathToSendonEmailAlert(poNo, poDate, fileName) {
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: "/ApplicationLayer/BillofMaterial/SavePdfDocToSendOnEmailAlert",
//        data: { poNo: poNo, poDate: poDate, fileName: fileName },
//        /*dataType: "json",*/
//        success: function (data) {

//        }
//    });
//}
function OtherFunctions(StatusC, StatusName) {

}
function ForwardHistoryBtnClick() {
    var Doc_ID = $("#DocumentMenuId").val();
    var Doc_No = $("#ddl_ProductName").val().trim() + "_" + $("#txt_RevisionNumber").val().trim();
    debugger;
    if (Doc_No != "" && Doc_No != null && Doc_ID != "" && Doc_ID != null)
        Cmn_GetForwarderHistoryList(Doc_No, Doc_ID);
    return false;
}


//--------------------------Work Flow End--------------------------------

function BindProductNameDDL(type) {
    debugger;
    if (type == "new") {
        $("#ddl_ProductName").append("<option value='0' selected>---Select---</option>");
    }
    else {
        $("#ddl_ProductName").append("<option value='0'>---Select---</option>");
    }
    $("#ddl_ProductName").select2({

        ajax: {
            url: "/ApplicationLayer/BillofMaterial/GetProductNameInDDL",
            data: function (params) {
                var queryParameters = {
                    SO_ItemName: params.term,
                    //PageName: PageName,
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
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">UOM</div></div>
</strong></li></ul>`)
                }

                var page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
                    }),
                    //results: data.slice((page - 1) * pageSize, page * pageSize),
                    //more: data.length >= page * pageSize,
                    //pagination: {
                    //    more: (page * 30) < data.total_count
                    //}
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
                    '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                    '</div>'
                );
            
            return $result;

            firstEmptySelect = false;
        },

    });

    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/BillofMaterial/GetProductNameInDDL",
    //        data: function (params) {
    //            var queryParameters = {
    //                SO_ItemName: params.term
    //            };
    //            return queryParameters;
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    sessionStorage.removeItem("PLitemList");
    //                    sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table));
    //                    $('#ddl_ProductName').append(`<optgroup class='def-cursor' id="Textddl" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        $('#Textddl').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                    }
    //                    var firstEmptySelect = true;
    //                    $('#ddl_ProductName').select2({
    //                        templateResult: function (data) {
    //                            var UOM = $(data.element).data('uom');
    //                            var classAttr = $(data.element).attr('class');
    //                            var hasClass = typeof classAttr != 'undefined';
    //                            classAttr = hasClass ? ' ' + classAttr : '';
    //                            var $result = $(
    //                                '<div class="row">' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                                '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                                '</div>'
    //                            );
    //                            return $result;
    //                            firstEmptySelect = false;
    //                        }
    //                    });



    //                }
    //            }
    //        },
    //    });
}

function ddl_ProductName_onchange(e) {
    debugger;
    var Itm_ID = $("#ddl_ProductName").val();
    if (Itm_ID != "0") {
        $("#ddlReplicateWith").attr("disabled", false);
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }
    else {
        $("#ddlReplicateWith").attr("disabled", true);
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
    }
    if (Itm_ID != null) {
        $("#hdn_product_id").val(Itm_ID);
        //$("#ddlReplicateWith").attr("disabled", false);
    }
    else if (Itm_ID == null) {
        Itm_ID = $("#hdn_product_id").val();
        $("#ddlReplicateWith").attr("disabled", true);
    }
    try {
        Cmn_BindUOM(e, Itm_ID, "", "","");
    } catch (err) {
    }
    debugger;
    //$("#ddlReplicateWith").attr("disabled",false);
}

function BindItemNameDDL(ItmType,SFG,Pack,type) {
    debugger;
    if (type == "new") {
        $("#ddl_ItemName").append("<option value='0' selected>---Select---</option>");
    }
    else {
        $("#ddl_ItemName").append("<option value='0'>---Select---</option>");
    }
    $("#ddl_ItemName").select2({

        ajax: {
            url: "/ApplicationLayer/BillofMaterial/BindItemNameDDL",
            data: function (params) {
                var queryParameters = {
                    SO_ItemName: params.term,
                    product_id: $("#ddl_ProductName").val(),
                    ItmType: ItmType,
                    wip: SFG,
                    Pack: Pack,
                    page: params.page || 1
                };
                return queryParameters;
            },
            multiple: true,
            cache: true,
            processResults: function (data, params) {
                debugger
               
                data = JSON.parse(data).Table;
                var pageSize,
                    pageSize = 2000; // or whatever pagesize
                let array = [];
                var ddl_opName = $("#ddl_opName").val();
                var mainItem = $("#ddl_ProductName").val();
                array.push({ id: mainItem });

                $("#tbl_" + ddl_opName + " > tbody > tr").each(function () {
                    var currentRow = $(this);
                    debugger;
                    var itemId = currentRow.find("#tbl_hdn_ddl_ItemName_ID").val();
                    var itemId = currentRow.find("#tbl_hdn_ddl_ItemName_ID").val();
                    if (itemId != "0") {
                        array.push({ id: itemId });
                    }
                });
                $("#tbladd_body > tr").each(function () {
                    var row = $(this);
                    let op_id = row.find("#tbl_hdn_ddl_op_id").val();
                    $("#tbl_" + op_id + " > tbody > tr #tbl_hdn_ddl_ItemType_ID[value='OW']").closest('tr').each(function () {
                        var currentRow = $(this);
                        debugger;
                        var itemId = currentRow.find("#tbl_hdn_ddl_ItemName_ID").val();
                        if (itemId != "0") {
                            array.push({ id: itemId });
                        }
                    });
                });

                array = FilterAlternateItems(array, ddl_opName);//Added by Suraj on 16-10-2023 for filter Alternate item

                var ItemListArrey = JSON.stringify(array);

                let selected = [];
                selected.push({ id: $("#ddl_ItemName").val() });
                selected = JSON.stringify(selected);

                var NewArrey = JSON.parse(ItemListArrey).filter(i => !selected.includes(i.id))

                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.Item_id));
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">${$("#ItemUOM").text()}</div></div>
</strong></li></ul>`)
                }
                page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                if (data[0] != null) {
                    if (data[0].Item_name.trim() != "---Select---") {
                        var select = { Item_id: "0", Item_name: " ---Select---", uom_name: "" };
                        data.unshift(select);
                    }
                }
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.Item_id, text: val.Item_name, UOM: val.uom_name };
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
                    '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
                    '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
                    '</div>'
                );
          
            return $result;

            firstEmptySelect = false;
        },

    });

    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "/ApplicationLayer/BillofMaterial/BindItemNameDDL",
    //        data: /*function (params)*/ {
    //            //var queryParameters = {
    //            //    SO_ItemName: params.term
    //            //};
    //            //return queryParameters;
    //            ItmType: ItmType,
    //            wip: SFG,
    //            Pack: Pack
    //        },
    //        dataType: "json",
    //        success: function (data) {
    //            debugger;
    //            if (data == 'ErrorPage') {
    //                LSO_ErrorPage();
    //                return false;
    //            }
    //            if (data !== null && data !== "") {
    //                var arr = [];
    //                arr = JSON.parse(data);
    //                if (arr.Table.length > 0) {
    //                    $('#ddl_ItemName').empty();
    //                    sessionStorage.removeItem("PLitemList");
    //                    sessionStorage.setItem("PLitemList", JSON.stringify(arr.Table));
    //                    $('#ddl_ItemName').append(`<optgroup class='def-cursor' id="Textddl_itemName" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);

    //                    for (var i = 0; i < arr.Table.length; i++) {
    //                        if ($("#ddl_ProductName").val() != arr.Table[i].Item_id || $("#ddl_ProductName").val() == 0) {
    //                            $('#Textddl_itemName').append(`<option data-uom="${arr.Table[i].uom_name}" value="${arr.Table[i].Item_id}">${arr.Table[i].Item_name}</option>`);
    //                        }
    //                    }
    //                    var firstEmptySelect = true;
                        
    //                    var ddl_opName = $("#ddl_opName").val();
    //                    $('#ddl_ItemName').select2({
    //                        templateResult: function (data) {
    //                            var selected = $("#ddl_ItemName").val();
    //                            if (checkBomItem(data, selected, "#tbl_" + ddl_opName) == true) {
    //                                var UOM = $(data.element).data('uom');
    //                                var classAttr = $(data.element).attr('class');
    //                                var hasClass = typeof classAttr != 'undefined';
    //                                classAttr = hasClass ? ' ' + classAttr : '';
    //                                var $result = $(
    //                                    '<div class="row">' +
    //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + data.text + '</div>' +
    //                                    '<div class="col-md-6 col-xs-6' + classAttr + '">' + UOM + '</div>' +
    //                                    '</div>'
    //                                );
    //                                return $result;
    //                            }
    //                            firstEmptySelect = false;
    //                        }
    //                    });
    //                    debugger;
    //                }
    //                else {
    //                    SetItemNameSelectOnly();
                        
    //                }
                    
    //            }
    //        },
    //    });
}
function ddl_ItemName_onchange(e) {
    debugger;
    $("#spanddl_ProductName").css("display", "none");
    if (e == 'undefined') {

    }
    var Itm_ID = $("#ddl_ItemName").val();//
    $("#hdn_ddl_ItemNameId").val(Itm_ID);
    var hdncompid = $("#hdncompid").val();
    var hdnBranchId = $("#hdnBranchId").val();
    var ddl_ItemName = $('#ddl_ItemName').val();
    if (ddl_ItemName != "0") {
        $("#vmddl_ItemName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da")
    }
    GetItemCost(hdncompid, hdnBranchId, Itm_ID,"hdnitemcost",null)
    if (Itm_ID != "0") {
        //GetBomUomNameItemWise(Itm_ID, "Txt_UOM_itemName", "hdn_Txt_UOM_itemName")
       
    } else {
        let s = '<option value="0">---Select---</option>';
        //$("#Txt_UOM_itemName").val("");
        $("#Txt_UOM_itemName").html(s);
        $("#hdn_Txt_UOM_itemName").val('');
    }
}
function GetBomUomNameItemWise(Itm_ID, UOM, UOMID, CurrRow, arr) {
    try {
        if (arr != null) {
            BomUomListItemWise(UOM, UOMID, CurrRow, arr.Table1, arr.Table2)
        } else {
            $.ajax(
                {
                    type: "POST",
                    url: "/ApplicationLayer/BillofMaterial/GetSOItemUOM",
                    data: {
                        Itm_ID: Itm_ID
                    },
                    success: function (data) {
                        debugger;
                        if (data !== null && data !== "" && data != "ErrorPage") {
                            var array = [];
                            array = JSON.parse(data);
                            BomUomListItemWise(UOM, UOMID, CurrRow, array.Table, array.Table1)
                        }
                        else {
                            $("#" + UOM).val("");
                            $("#" + UOMID).val('');
                        }
                        
                        //if (CurrRow != null) {
                        //    if (data !== null && data !== "" && data != "ErrorPage") {
                        //        var arr = [];
                        //        arr = JSON.parse(data);
                        //        if (UOM == "td_AltUom") {
                        //            if (arr.Table1.length > 0) {

                        //                let s = '';
                        //                for (let i = 0; i < arr.Table1.length; i++) {
                        //                    s += '<option value="' + arr.Table1[i].uom_id + '">' + arr.Table1[i].uom_alias + '</option>';
                        //                }
                        //                let defUomId = CurrRow.find("#tdAltUomId").val();
                        //                defUomId = defUomId == "" ? arr.Table[0].uom_id : defUomId;
                        //                CurrRow.find("#td_AltUom").html(s);
                        //                CurrRow.find("#td_AltUom").val(defUomId).trigger('change');
                        //            }
                        //            else {
                        //                s = '<option value="' + arr.Table[0].uom_id + '">' + arr.Table[0].uom_alias + '</option>';
                        //                CurrRow.find("#td_AltUom").html(s);
                        //                CurrRow.find("#td_AltUom").val(arr.Table[0].uom_id).trigger('change');
                        //            }
                        //        } else {
                        //            if (arr.Table.length > 0) {
                        //                CurrRow.find("#" + UOM).val(arr.Table[0].uom_alias);
                        //                CurrRow.find("#" + UOMID).val(arr.Table[0].uom_id);
                        //            }
                        //            else {
                        //                CurrRow.find("#" + UOM).val("");
                        //                CurrRow.find("#" + UOMID).val("");
                        //            }
                        //        }

                        //    }
                        //    else {
                        //        CurrRow.find("#" + UOM).val("");
                        //        CurrRow.find("#" + UOMID).val('');
                        //    }
                        //}
                        //else {
                        //    if (data !== null && data !== "" && data != "ErrorPage") {
                        //        var arr = [];
                        //        arr = JSON.parse(data);
                        //        if (UOM == "Txt_UOM_itemName") {
                        //            if (arr.Table1.length > 0) {

                        //                let s = '';
                        //                for (let i = 0; i < arr.Table1.length; i++) {
                        //                    s += '<option value="' + arr.Table1[i].uom_id + '">' + arr.Table1[i].uom_alias + '</option>';
                        //                }
                        //                $("#Txt_UOM_itemName").html(s);
                        //                let defUomId = $("#hdn_Txt_UOM_itemName").val();
                        //                defUomId = (defUomId == null || defUomId == "") ? arr.Table[0].uom_id : defUomId;
                        //                $("#Txt_UOM_itemName").val(defUomId).trigger('change');;
                        //            } else {
                        //                s = '<option value="' + arr.Table[0].uom_id + '">' + arr.Table[0].uom_alias + '</option>';
                        //                $("#Txt_UOM_itemName").html(s);
                        //                $("#Txt_UOM_itemName").val(arr.Table[0].uom_id).trigger('change');
                        //            }
                        //        }
                        //        else {
                        //            if (arr.Table.length > 0) {
                        //                $("#" + UOM).val(arr.Table[0].uom_alias);
                        //                $("#" + UOMID).val(arr.Table[0].uom_id);
                        //            }
                        //            else {
                        //                $("#" + UOM).val("");
                        //                $("#" + UOMID).val("");
                        //            }
                        //        }

                        //    }
                        //    else {
                        //        $("#" + UOM).val("");
                        //        $("#" + UOMID).val('');
                        //    }
                        //}

                        //$("#ddl_ItemName").focus();
                    },
                });
        }
       

    
    } catch (err) {
    }
}
function BomUomListItemWise(UOM, UOMID, CurrRow,Table1,Table2) {
    if (CurrRow != null) {
        if (UOM == "td_AltUom") {
            if (Table2.length > 0) {

                let s = '';
                for (let i = 0; i < Table2.length; i++) {
                    s += '<option value="' + Table2[i].uom_id + '">' + Table2[i].uom_alias + '</option>';
                }
                let defUomId = CurrRow.find("#tdAltUomId").val();
                defUomId = defUomId == "" ? Table1[0].uom_id : defUomId;
                CurrRow.find("#td_AltUom").html(s);
                CurrRow.find("#td_AltUom").val(defUomId).trigger('change');
            } else {
                s = '<option value="' + Table1[0].uom_id + '">' + Table1[0].uom_alias + '</option>';
                CurrRow.find("#td_AltUom").html(s);
                CurrRow.find("#td_AltUom").val(Table1[0].uom_id).trigger('change');
            }
        }
        else {
            if (Table1.length > 0) {
                CurrRow.find("#" + UOM).val(Table1[0].uom_alias);
                CurrRow.find("#" + UOMID).val(Table1[0].uom_id);
            }
            else {
                CurrRow.find("#" + UOM).val("");
                CurrRow.find("#" + UOMID).val("");
            }
        }

    }
    else {
        if (UOM == "Txt_UOM_itemName") {
            if (Table2.length > 0) {

                let s = '';
                for (let i = 0; i < Table2.length; i++) {
                    s += '<option value="' + Table2[i].uom_id + '">' + Table2[i].uom_alias + '</option>';
                }
                $("#Txt_UOM_itemName").html(s);
                let defUomId = $("#hdn_Txt_UOM_itemName").val();
                defUomId = (defUomId == null || defUomId == "") ? Table1[0].uom_id : defUomId;
                $("#Txt_UOM_itemName").val(defUomId).trigger('change');;
            } else {
                s = '<option value="' + Table1[0].uom_id + '">' + Table1[0].uom_alias + '</option>';
                $("#Txt_UOM_itemName").html(s);
                $("#Txt_UOM_itemName").val(Table1[0].uom_id).trigger('change');
            }
        }
        else {
            if (Table1.length > 0) {
                $("#" + UOM).val(Table1[0].uom_alias);
                $("#" + UOMID).val(Table1[0].uom_id);
            }
            else {
                $("#" + UOM).val("");
                $("#" + UOMID).val("");
            }
        }
       
    }
}
var rowIdx = 0;
function SetItemTypeSequence(ItemType) {
    debugger;
    var SeqNo = "";
    if (ItemType == "IR") {
        SeqNo = 2;
    }
    if (ItemType == "IW") {
        SeqNo = 1;
    }
    if (ItemType == "OW") {
        SeqNo = 5;
    }
    if (ItemType == "OF") {
        SeqNo = 7;
    }
    if (ItemType == "A") {
        SeqNo = 3;
    }
    if (ItemType == "B") {
        SeqNo = 6;
    }
    if (ItemType == "P") {
        SeqNo = 4;
    }
    if (ItemType == "OS") {
        SeqNo = 8;
    }
    return SeqNo;
}
function AddNewRow(CompId, BranchId) {
    //var rowIdx = 0;
    debugger;
    var ddl_ProductId = "";
        ddl_ProductId= $("#ddl_ProductName").val();
    var QtyDecDigit = $("#QtyDigit").text();
    var txt_Quantity = $("#txt_Quantity").val();
    var itemcost1 = $("#hdnitemcost").val(); 
    var ddl_opName = $("#ddl_opName option:selected").text();
    var ddl_op_ID = $("#ddl_opName").val();

    var ddlItemType = $("#ddlItemType option:selected").text();
    var ddlItemType_ID = $("#ddlItemType").val();
    

    var ddl_ItemName = $("#ddl_ItemName option:selected").text();
    var ddl_ItemName_ID = $("#ddl_ItemName").val();

    var UOM_itemName = $("#Txt_UOM_itemName option:selected").text();
    var hdn_UOM_itemName = $("#Txt_UOM_itemName option:selected").val();//$("#hdn_Txt_UOM_itemName").val();

    var txtQuantityItemName = $("#txtQuantityItemName").val();

    var flag = 'N';
    if (ddl_ProductId != "0") {
        $("#vmddl_ProductName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_ProductName').text($("#valueReq").text());
        $("#vmddl_ProductName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
        flag = 'Y';
    }
    if (txt_Quantity != "0" && txt_Quantity != "") {
        $("#vm_Quantity").css("display", "none");
    }
    else {
        $('#vm_Quantity').text($("#valueReq").text());
        $("#vm_Quantity").css("display", "block");
        flag = 'Y';
    }
    if (ddl_op_ID != "0") {
        $("#vm_ddl_opName").css("display", "none");
        $("[aria-labelledby='select2-ddl_opName-container']").css("border-color", "#ced4da")
    }
    else {
        $('#vm_ddl_opName').text($("#valueReq").text());
        $("#vm_ddl_opName").css("display", "block");
        $("[aria-labelledby='select2-ddl_opName-container']").css("border-color","red")
        flag = 'Y';
    }
    if (ddlItemType_ID != "0") {
        $("#spanItemType").css("display", "none");
    }
    else {
        $('#spanItemType').text($("#valueReq").text());
        $("#spanItemType").css("display", "block");
        $("#ddlItemType").css("border-color", "red");
        flag = 'Y';
    }
    if (ddl_ItemName_ID != "0") {
        $("#vmddl_ItemName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
        $("#ddl_ItemName").css("border-color", "#ced4da");
    }
    else {
        $('#vmddl_ItemName').text($("#valueReq").text());
        $("#vmddl_ItemName").css("display", "block");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red");
        $("#ddl_ItemName").css("border-color", "red");
        flag = 'Y';
    }

    if (hdn_UOM_itemName != "0") {
        $("#vmTxt_UOM_itemName").css("display", "none");
        $("#Txt_UOM_itemName").css("border-color", "#ced4da");
    }
    else {
        $('#vmTxt_UOM_itemName').text($("#valueReq").text());
        $("#vmTxt_UOM_itemName").css("display", "block");
        $("#Txt_UOM_itemName").css("border-color", "red");
        flag = 'Y';
    }

    if (txtQuantityItemName != "" && txtQuantityItemName != "0") {
        $("#spanQuantityItemName").css("display", "none");
        $("#txtQuantityItemName").css("border-color", "#ced4da")
    }
    else {
        $('#spanQuantityItemName').text($("#valueReq").text());
        $("#spanQuantityItemName").css("display", "block");
        $("#txtQuantityItemName").css("border-color", "red")
        flag = 'Y';
    }
    if (flag == 'Y') {
        return false;
    }
    debugger;
    var rowCount = $('#tbladd >tbody >tr').length;

    ////---------------Checking Alternate Item------ Cancelled Commented by Suraj on 16-10-2023
    //var array = new Array();
    //array = FilterAlternateItems(array, ddl_op_ID);
    //if (array.filter(a => a.id == ddl_ItemName_ID).length>0) {
    //    swal("","This item is already exist as alternate item for item .","warning")
    //    return false;
    //}
    ////----------------------------------------------

    var flag_check_op_id = 'N';
    var rowspn = parseInt(0);
    $("#tbladd >tbody >tr").each(function (i, row) {
        debugger;

        var currentRow = $(this);
        if (rowCount == 0) {
            rowspn = 1;
        }

        if (rowCount > 0) {
            var op_id = currentRow.find("#tbl_hdn_ddl_op_id").val();
            rowspn = parseInt(currentRow.find("#hdnspan").val());
            if (ddl_op_ID == op_id) {
                flag_check_op_id = 'Y';
            }
        }

    });
    debugger;
    var itemcost = (parseFloat(itemcost1).toFixed(RateDigit));
    if (ddlItemType_ID == "A" || ddlItemType_ID == "OS") {
        itemcost1 = 0;
    }
    
    var trop = "";
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var txtQuantityItemName1 = (parseFloat(txtQuantityItemName).toFixed(QtyDecDigit));
    var totalitemvalue1 = txtQuantityItemName * itemcost1;
    
    if (ddlItemType_ID == "OW" || ddlItemType_ID == "OF") {
        var itemVal = 0;
        $('#tbl_' + ddl_op_ID + ' tbody tr').each(function () {
            var CurrentRow1 = $(this);
            var tbl_hdn_totalitemvalue = CurrentRow1.find("#tbl_hdn_totalitemvalue").val();
            itemVal = parseFloat(itemVal) + parseFloat(tbl_hdn_totalitemvalue);

        });
        itemcost1 = parseFloat(itemVal) / parseFloat(txtQuantityItemName);
        totalitemvalue1 = itemVal;
    }
    if (ddlItemType_ID == "IW") {
        totalitemvalue1 = $("#hdn_Item_Value").val();
    }
    itemcost1 = parseFloat(itemcost1).toFixed(RateDigit);
    var totalitemvalue = (parseFloat(totalitemvalue1).toFixed(ValDigit));

    var AlternateIcon = "";
    if (ddlItemType_ID.trim() == "IR" || ddlItemType_ID.trim() == "P") {
        AlternateIcon =`<div class="col-sm-2 i_Icon">
     <button type="button" id="btnAltItemInfo" class="calculator alter" onclick="OnClickAlternateItem(event)" data-toggle="modal" data-target="#AlternateItemDetail" data-backdrop="static" data-keyboard="false"  title="${$("#span_AlternateItemDetail").text()}"><i class="fa fa-th-large" aria-hidden="true"></i> </button>
 </div>`
    }

    debugger;
    if (flag_check_op_id == 'N') {//first time
        rowIdx = rowIdx + 1
        trop += "<tr id='tr_" + ddl_op_ID + "'>";
        debugger;  //changing td to id by SM
        trop += '<td align="left" valign="top" class="red center bom_width_td" > <i class="fa fa-trash" aria-hidden="true" title="'+$("#Span_Delete_Title").text()+'" onclick="deleteMainRow(this, event, ' + ddl_op_ID + ', \'' + ddl_ItemName_ID + '\', \'' + ddlItemType_ID + '\')"></i></td>';
        trop += "<td id='srno'></td>";
        trop += "<td id='opname'>" + ddl_opName + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_op_id' value=" + ddl_op_ID + " /></td>";
        trop += "<td colspan='6' class='no-padding'>";

        trop += "<table id='tbl_" + ddl_op_ID + "' border='0' width='100%'>";
        trop += "<tbody id='tbody_" + ddl_op_ID + "'>";
        trop += " <tr>";
        trop += '<td align="left" valign="top" class="red center bom_width_td"> <i class="fa fa-trash" aria-hidden="true" title="' + $("#Span_Delete_Title").text() +'" onclick="deleteRow(this, event, ' + ddl_op_ID + ', \'' + ddl_ItemName_ID + '\', \'' + ddlItemType_ID + '\')"></i></td>';
        trop += "<td class='bom_width_td' align='left' valign='top'>";
        if (ddlItemType_ID != "OF" && ddlItemType_ID != "IW") {
            trop += "<i class='fa fa-edit' aria-hidden='true' id='' title=" + $("#Edit").text() + " onclick='editRow(this, event, " + ddl_op_ID + ")'></i>";
        }
        debugger;  //changing td to id by SM
        trop += " </td>";
        trop += `<td id='op_Item_type_name' width='17%'>
<div class="col-sm-10 no-padding">${ddlItemType}</div>
${AlternateIcon}
</td>`;
        trop += "<td  hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemType_ID' value=" + ddlItemType_ID + " /></td>";
        trop += '<td id="op_item_name" width="32%"> <div class="col-sm-11 no-padding">' + ddl_ItemName + '</div><div class="col-sm-1 i_Icon"> <button type ="button" id ="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,\'' + ddl_ItemName_ID + '\', \'' + ddl_ItemName + '\',\'edit\');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" > <img src="/Content/Images/iIcon1.png" alt="" title="' + $("#Span_ItemInformation_Title").text() +'"> </button> </div></td>';
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemName_ID' value=" + ddl_ItemName_ID + " /></td>";
        trop += "<td id='op_uom_name' width='12%'>" + UOM_itemName + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_UOM_itemName' value=" + hdn_UOM_itemName + " /></td>";
        trop += "<td id='op_qty' width='13%' class='num_right'>" + txtQuantityItemName1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_txtQuantityItemName' value=" + txtQuantityItemName1 + " /></td>";
        trop += "<td id='op_item_cost' width='13%' class='num_right'>" + itemcost1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_itemcost' value=" + itemcost1 + " /></td>";
        trop += "<td id='op_item_value' width='13%' class='num_right'>" + totalitemvalue + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_totalitemvalue' value=" + totalitemvalue + " /></td>";
        trop += "<td hidden='hidden'>";
        trop += "<input type='hidden' name='ChildRowNo' id='ChildRowNo' />";
        trop += "<input type = 'hidden' name = 'ChildTableId' id = 'ChildTableId' /> ";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_id' id='tbl_hdn_id' value=" + ddl_op_ID + " /></td>";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_seq_no' id='tbl_hdn_seq_no' value=" + SetItemTypeSequence(ddlItemType_ID)+" /></td>";
        trop += "</td>";
        trop += "</tr>";
        trop += "</tbody>";

        trop += "</table>";
        trop += "</td>";
        trop += "</tr >";

      
        $('#tbladd_body').append(trop);
        $("#ddl_ItemName option").removeClass("select2-hidden-accessible");
        $('#ddl_ItemName').val("0").change();
        disablePrName_Qty();
        HideItemType_AfterAdd(ddl_ItemName_ID, ddlItemType_ID)
        //AddRemoveItemName(ddl_op_ID)
        SerialNoAfterDelete();
    }
    
    if (flag_check_op_id == 'Y')
    {

        debugger;
        trop += " <tr>";
        trop += '<td align = "left" valign = "top" class="red center bom_width_td"> <i class="fa fa-trash" aria-hidden="true" title="' + $("#Span_Delete_Title").text() +'" onclick="deleteRow(this, event, ' + ddl_op_ID + ', \'' + ddl_ItemName_ID + '\', \'' + ddlItemType_ID + '\')"></i></td >';
        trop += "<td width='20' align='left' valign='top'> ";
        if (ddlItemType_ID != "OF" && ddlItemType_ID != "IW") {
            trop += "<i class='fa fa-edit' aria-hidden='true' id='' title=" + $("#Edit").text() + " onclick='editRow(this, event, " + ddl_op_ID + ")'></i>";
        }
        debugger;  //changing td to id by SM
        trop += "</td>";
        trop += `<td id='op_Item_type_name' width='17%'>
<div class="col-sm-10 no-padding">${ddlItemType}</div>
${AlternateIcon}
</td>`;
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemType_ID' value=" + ddlItemType_ID + " /></td>";
        trop += '<td id="op_item_name" width="32%"> <div class="col-sm-11 no-padding">' + ddl_ItemName + '</div><div class="col-sm-1 i_Icon"> <button type ="button" id ="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,\'' + ddl_ItemName_ID + '\', \'' + ddl_ItemName + '\',\'edit\');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" > <img src="/Content/Images/iIcon1.png" alt="" title="' + $("#Span_ItemInformation_Title").text() +'"> </button> </div></td>';
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemName_ID' value=" + ddl_ItemName_ID + " /></td>";
        trop += "<td id='op_uom_name' width='12%'>" + UOM_itemName + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_UOM_itemName' value=" + hdn_UOM_itemName + " /></td>";
        trop += "<td id='op_qty' width='13%' id='' class='num_right'>" + txtQuantityItemName1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_txtQuantityItemName' value=" + txtQuantityItemName1 + " /></td>";
        trop += "<td id='op_item_cost' width='13%' class='num_right'>" + itemcost1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_itemcost' value=" + itemcost1 + " /></td>";
        trop += "<td id='op_item_value' width='13%' class='num_right'>" + totalitemvalue + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_totalitemvalue' value=" + totalitemvalue + " /></td>";
        trop += "<td hidden='hidden'>";
        trop += "<input type='hidden' name='ChildRowNo' id='ChildRowNo' />";
        trop += "<input type = 'hidden' name = 'ChildTableId' id = 'ChildTableId' /> ";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_id' id='tbl_hdn_id' value=" + ddl_op_ID + " /></td>";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_seq_no' id='tbl_hdn_seq_no' value=" + SetItemTypeSequence(ddlItemType_ID) + " /></td>";
        trop += "</td>";
        trop += "</tr>";

        
        $('#tbl_' + ddl_op_ID + ' tbody').append(trop);
        HideOperationNameAfterFG();
        $("#ddl_ItemName option").removeClass("select2-hidden-accessible");
        $('#ddl_ItemName').val("0").change();
        disablePrName_Qty();
        HideItemType_AfterAdd(ddl_ItemName_ID, ddlItemType_ID);
        //AddRemoveItemName(ddl_op_ID);
        SerialNoAfterDelete();
    }
    CalCulateTotalAndSet(ddl_op_ID);
    //$("#ddl_ItemName").html(`<optgroup class='def-cursor' id="Textddl_itemName" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'>
    //        <option data-uom="0" value="0">---Select---</option>
    //            </optgroup>`);
    SetItemNameSelectOnly();
    ResetOperationsFields();
    $("#ddl_opName").val(ddl_op_ID).change();
    //$("#UOM_itemName").val("");
    //$("#hdn_UOM_itemName").val("");
    //$("#ddl_ItemName ,#txtQuantityItemName").attr("disabled", false);
    debugger;
    sortTable('tbl_' + ddl_op_ID + '');
    $("#tbladd tbody tr").css("background-color", "#ffffff");
    //sortTableuser('tbl_' + ddl_op_ID + '');
};
function HideOperationNameAfterFG() {
    var ddlItemType_ID = "";//$("#ddlItemType").val();
    
    ddlItemType_ID=$("#tbladd > tbody > tr > td > table >tbody >tr td #tbl_hdn_ddl_ItemType_ID[value='OF']").val();
    if (ddlItemType_ID == "OF") {
        debugger;
       
        var ddl_OP = '<option value="0">---Select---</option>';
        var lentbl = $("#tbladd >tbody > tr").length;
        $("#tbladd >tbody > tr").each(function () {
            debugger;  //changing td to id by SM
            var OP_Id = $(this).find("#tbl_hdn_ddl_op_id").val();
            var OP_Name = $(this).find("#opname").text();
            var Srno = $(this).find("#srno").text();
            if (Srno == lentbl) {
                ddl_OP += '<option value="' + OP_Id + '" selected>' + OP_Name + '</option>'
            }
            else {
                ddl_OP += '<option value="' + OP_Id + '">' + OP_Name + '</option>'
            }         
        });
        $("#ddl_opName").attr("onchange", "");
        $("#ddl_opName").html(ddl_OP);
        $("#ddl_opName").attr("onchange", "ddl_OPNameChange()");
    }

}
function editRow(el, e, option) {
    debugger;
    var rowJavascript = el.parentNode.parentNode;
    var d = el.parentNode.parentNode.rowIndex;
    try {
        $("#divAddNew").hide(); 
        $("#divUpdate").show();
        var clickedrow = $(e.target).closest("tr");
        $('#ChildRowNo').val(d);
        $('#ChildTableId').val('#tbl_' + option);
        debugger;  //changing td to id by SM
        $('#ddl_opName').val(option).change();
        var ddlItemType_ID = clickedrow.find("#tbl_hdn_ddl_ItemType_ID").val().trim();
        var ddlItemType_Name = clickedrow.find("#op_Item_type_name").text().trim();
        $('#ddlItemType option[value=' + ddlItemType_ID + ']').show();
        //$('#ddlItemType').append(`<option value='${ddlItemType_ID}' selected>${ddlItemType_Name}</option>`); //.val(ddlItemType_ID);//.change();
        $('#ddlItemType').val(ddlItemType_ID);//.change();
        var ddl_ItemName_ID = clickedrow.find("#tbl_hdn_ddl_ItemName_ID").val().trim();
        debugger;  //changing td to id by SM
        var ddl_ItemName_Name = clickedrow.find("#op_item_name").text().trim();
        var ddl_ItemUOM_Name = clickedrow.find("#op_uom_name").text().trim();
        var ddl_ItemUOM_ID = clickedrow.find("#tbl_hdn_UOM_itemName").val().trim();
        $("#ddl_ItemName").html(`<optgroup class='def-cursor' id="Textddl_itemName" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'>
            <option data-uom="0" value="0">---Select---</option>
            <option data-uom='${ddl_ItemUOM_Name}' value='${ddl_ItemName_ID}' selected>${ddl_ItemName_Name}</option>
                </optgroup>`);
        $('#ddl_ItemName').val(ddl_ItemName_ID);//.change();
      
        //$("#Txt_UOM_itemName").val(ddl_ItemUOM_Name);
        //let s = '<option value="' + ddl_ItemUOM_ID + '">' + ddl_ItemUOM_Name + '</option>';
        //$("#Txt_UOM_itemName").val("");
        //$("#Txt_UOM_itemName").html(s);
        $("#UOM_Txt_itemName_id").val(ddl_ItemUOM_ID);
        $("#hdn_Txt_UOM_itemName").val(ddl_ItemUOM_ID);
        
        GetBomUomNameItemWise(ddl_ItemName_ID, "Txt_UOM_itemName", "hdn_Txt_UOM_itemName")
        var opt_qty = clickedrow.find("#tbl_hdn_txtQuantityItemName").val();
        $('#txtQuantityItemName').val(opt_qty);

        var tbl_hdn_itemcost = clickedrow.find("#tbl_hdn_itemcost").val();//item price
        $('#hdnitemcost').val(tbl_hdn_itemcost);

        $('#hdnUpdateInTable').val(rowJavascript.rowIndex);
        
        $("#ddl_opName").prop("disabled", true);
        if (opt_qty != "" && opt_qty != "0") {
            $("#spanOptmQty").css("display", "none");
        }
        else {
            $('#spanOptmQty').text($("#valueReq").text());
            $("#spanOptmQty").css("display", "block");
          
        }
        $("#ddlItemType").prop("disabled", true);
        if (ddlItemType_ID != "" && ddlItemType_ID != "0") {
            $("#spanItemType").css("display", "none");
            $("#ddlItemType").css("border-color", "#ced4da");
        }
        else {
            $('#spanItemType').text($("#valueReq").text());
            $("#spanItemType").css("display", "block");
            $("#ddlItemType").css("border-color", "red");
            
        }
        $("#ddl_ItemName").prop("disabled", true);
        if (ddl_ItemName_ID != "" && ddl_ItemName_ID != "0") {
            $("#vmddl_ItemName").css("display", "none");
            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
            $("#ddl_ItemName").css("border-color", "#ced4da");
        }
        else {
            $('#vmddl_ItemName').text($("#valueReq").text());
            $("#vmddl_ItemName").css("display", "block");
            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red")
            $("#ddl_ItemName").css("border-color", "red");
         
        }
        var ItemQty = $('#txtQuantityItemName').val();
        if (ItemQty != "" && ItemQty != "0") {
            $("#spanQuantityItemName").css("display", "none");
            $("#txtQuantityItemName").css("border-color", "#ced4da")
        }
        else {
            $('#vmddl_ItemName').text($("#valueReq").text());
            $("#spanQuantityItemName").css("display", "block");
            $("#txtQuantityItemName").css("border-color", "red")

        }
        $("#disabledAltItem").val("Y");
    }
    catch (err) {
        debugger
       
    }
   
}
function OnClickParaItemUpdateBtn(e) {
    debugger; 
    var RowNo = $('#hdnUpdateInTable').val();
    if (RowNo != '') {
        var ddl_op_ID = $("#ddl_opName").val();
        var ddlItemType_ID = $("#ddlItemType").val();
        var ddl_ItemName_ID = $("#ddl_ItemName").val();
        var txtQuantityItemName = $("#txtQuantityItemName").val();
        var uom_name = $("#Txt_UOM_itemName option:selected").text();
        var uom_id = $("#Txt_UOM_itemName option:selected").val();
        var flag = 'N';
        if (ddl_op_ID != "0") {
            $("#vm_ddl_opName").css("display", "none");
            $("[aria-labelledby='select2-ddl_opName-container']").css("border-color", "#ced4da");
        }
        else {
            $('#vm_ddl_opName').text($("#valueReq").text());
            $("#vm_ddl_opName").css("display", "block");
            $("[aria-labelledby='select2-ddl_opName-container']").css("border-color", "red");
            flag = 'Y';
        }
        if (ddlItemType_ID != "0") {
            $("#spanItemType").css("display", "none");
            $("#ddlItemType").css("border-color", "#ced4da");
        }
        else {
            $('#spanItemType').text($("#valueReq").text());
            $("#spanItemType").css("display", "block");
            flag = 'Y';
        }
        if (ddl_ItemName_ID != "0") {
            $("#vmddl_ItemName").css("display", "none");
            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da")
        }
        else {
            $('#vmddl_ItemName').text($("#valueReq").text());
            $("#vmddl_ItemName").css("display", "block");
            $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "red")
            flag = 'Y';
        }
        if (txtQuantityItemName != "" && txtQuantityItemName != "0") {
            $("#spanQuantityItemName").css("display", "none");
            $("#txtQuantityItemName").css("border-color","#ced4da")
        }
        else {
            $('#spanQuantityItemName').text($("#valueReq").text());
            $("#spanQuantityItemName").css("display", "block");
            $("#txtQuantityItemName").css("border-color", "red")
            flag = 'Y';
        }

        if (uom_id != "0" && uom_id != "") {
            $("#vmTxt_UOM_itemName").css("display", "none");
            $("#Txt_UOM_itemName").css("border-color", "#ced4da");
        }
        else {
            $('#vmTxt_UOM_itemName').text($("#valueReq").text());
            $("#vmTxt_UOM_itemName").css("display", "block");
            $("#Txt_UOM_itemName").css("border-color", "red");
            flag = 'Y';
        }
        if (flag == 'Y') {
            return false;
        }

        debugger;
        var ValDigit = $("#ValDigit").text();
        var tableRow = $('#ChildRowNo').val();
        var tableID = $('#ChildTableId').val();
        var qty = $("#txtQuantityItemName").val();
        
        //var tbl_hdn_itemcost = $(tableID).find("tr:eq(" + tableRow + ")").find("#tbl_hdn_itemcost").val();
        var tbl_hdn_itemcost = $("#hdnitemcost").val();
        $(tableID).find("tr:eq(" + tableRow + ")").find("#op_item_cost").text(((parseFloat(tbl_hdn_itemcost).toFixed(ValDigit))));
        $(tableID).find("tr:eq(" + tableRow + ")").find("#tbl_hdn_itemcost").val(((parseFloat(tbl_hdn_itemcost).toFixed(ValDigit))));
        if (ddlItemType_ID == "OW" || ddlItemType_ID == "OF") {
            var itemVal = 0;

            $('#tbl_' + ddl_op_ID + ' tbody tr').each(function () {
                var CurrentRow1 = $(this);
                var tbl_hdn_totalitemvalue = CurrentRow1.find("#tbl_hdn_totalitemvalue").val();
                if (CurrentRow1.find("#tbl_hdn_ddl_ItemType_ID").val() != "OW") {
                    itemVal = parseFloat(itemVal) + parseFloat(tbl_hdn_totalitemvalue);
                }
            });
            debugger;  //changing td to id by SM
            tbl_hdn_itemcost = parseFloat(itemVal) / parseFloat(qty);
            //totalitemvalue1 = itemVal;
            $(tableID).find("tr:eq(" + tableRow + ")").find("#op_item_cost").text(((parseFloat(tbl_hdn_itemcost).toFixed(ValDigit))));
            $(tableID).find("tr:eq(" + tableRow + ")").find("#tbl_hdn_itemcost").val(((parseFloat(tbl_hdn_itemcost).toFixed(ValDigit))));
        }
        if (ddlItemType_ID == "A" || ddlItemType_ID == "OS") {
            tbl_hdn_itemcost = 0;
        }
        debugger;  //changing td to id by SM
        var itemvalue = qty * tbl_hdn_itemcost;
        $(tableID).find("tr:eq(" + tableRow + ")").find("#op_qty").text(qty);
        $(tableID).find("tr:eq(" + tableRow + ")").find("#tbl_hdn_txtQuantityItemName").val(qty);
        //$(tableID).find("tr:eq(" + tableRow + ")").find("td:eq(12)").text(itemvalue);
        $(tableID).find("tr:eq(" + tableRow + ")").find("#op_item_value").text(((parseFloat(itemvalue).toFixed(ValDigit))));
        $(tableID).find("tr:eq(" + tableRow + ")").find("#tbl_hdn_totalitemvalue").val(((parseFloat(itemvalue).toFixed(ValDigit))));
        $(tableID).find("tr:eq(" + tableRow + ")").find("#op_uom_name").text(uom_name);
        $(tableID).find("tr:eq(" + tableRow + ")").find("#tbl_hdn_UOM_itemName").val(uom_id);

        if (uom_id == "0") {
            $(tableID).find("tr:eq(" + tableRow + ")").find("#op_uom_name").css("border", "2px solid red");
            Error = "Y";
        } else {
            $(tableID).find("tr:eq(" + tableRow + ")").find("#op_uom_name").css("border", "");
        }

        if (ddlItemType_ID == "OW") {
            debugger;  //changing td to id by SM
            var FindNextOperationRow = $(tableID).parent().parent().next().find(" >td table >tbody tr td #tbl_hdn_ddl_ItemType_ID[value='IW']").closest('tr');
            FindNextOperationRow.find("#op_qty").text(qty);
            FindNextOperationRow.find(" #tbl_hdn_txtQuantityItemName").val(qty);
            FindNextOperationRow.find("#op_item_value").text(((parseFloat(itemvalue).toFixed(ValDigit))));
            FindNextOperationRow.find("#tbl_hdn_totalitemvalue").val(((parseFloat(itemvalue).toFixed(ValDigit))));
            FindNextOperationRow.find("#op_item_cost").text(((parseFloat(tbl_hdn_itemcost).toFixed(ValDigit))));
            FindNextOperationRow.find("#tbl_hdn_itemcost").val(((parseFloat(tbl_hdn_itemcost).toFixed(ValDigit))));
        }
        CalCulateTotalAndSet(ddl_op_ID);


        $("#ddl_opName").prop("disabled", false);
        $("#ddlItemType").prop("disabled", false);
        $("#ddl_ItemName").prop("disabled", false);
        $("#divAddNew").show();
        $("#divUpdate").hide();
        $('#ddlItemType').val("0").change();
        $('#ddl_ItemName').val("0").change();
        $("#txtQuantityItemName").val('');
        //$("#Txt_UOM_itemName").val("");
        let s = '<option value="0">---Select---</option>';
        //$("#Txt_UOM_itemName").val("");
        $("#Txt_UOM_itemName").html(s);
        $("#hdn_Txt_UOM_itemName").val("");
        SerialNoAfterDelete();
        var ddl_op_ID = $("#ddl_opName").val();
        ItemTypeDdlBind(ddl_op_ID);
        //$("#ddl_opName").val(ddl_op_ID).change();
        SetItemNameSelectOnly();
        $("#disabledAltItem").val("N");
    }
}
function deleteMainRow(i, e, ddl_op_ID, ddl_ItemName_ID, ddlItemType_ID) {
    debugger;

    $(i).closest('tr').find('table > tbody >tr').each(function () {
        let row = $(this);
        let item_id = row.find("#tbl_hdn_ddl_ItemName_ID").val();
        $("#HdnTable_AltItemDetail > tbody > tr").each(function () {
            let innerRow = $(this);
            let op_id = innerRow.find("#td_altOpId").text();
            let parent_id = innerRow.find("#td_altParentItemId").text();
            if (op_id == ddl_op_ID && parent_id == item_id) {
                innerRow.remove();
            }
        });
    });

    $(e.target).closest('tr').next().find('table > tbody >tr #tbl_hdn_ddl_ItemType_ID[value="IW"]').closest('tr').remove();
    $(i).closest('tr').remove();
    DeleteAlternateWhenItemDelete(ddl_op_ID, ddl_ItemName_ID);//Added By Suraj on 16-10-2023 for delete Aternate item
    $("#tbladd tbody tr").css("background-color", "#ffffff");
   
    var checkOF = $("#tbladd >tbody >tr td tr #tbl_hdn_ddl_ItemType_ID[value='OF']").val();
    if (checkOF != 'OF') {
        GetOpNameList();
       
    }
    else {
        HideOperationNameAfterFG();
    }
    
    $("#ddl_opName").val(0).change();
    $("#vm_ddl_opName").css("display", "none");
    $("[aria-labelledby='select2-ddl_opName-container']").css("border-color", "#ced4da");
    SerialNoAfterDelete();
    ResetOperationsFields();
};
function deleteRow(i, e, option, ddl_ItemName_ID, ddlItemType_ID) {
    debugger;

    var lenght = $('#tbl_' + option + ' tr').length;
    DeleteAlternateWhenItemDelete(option, ddl_ItemName_ID);//Added By Suraj on 16-10-2023 for delete Aternate item
    if (lenght == 1) {
        $("#tbladd >tbody >tr").each(function (i, row) {
            debugger;
            var currentRow = $(this);
            var op_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
            var ddl_ItemName_ID = currentRow.find("#tbl_hdn_ddl_ItemName_ID").val();
            var ddlItemType_ID = currentRow.find("#tbl_hdn_ddl_ItemType_ID").val();

            if (option == op_id_tbl) {
                ShowItemType_AfterDelete(i,ddl_ItemName_ID, ddlItemType_ID);
                debugger;
                currentRow.closest('tr').remove();
                $("#tbladd tbody tr").css("background-color", "#ffffff");
                var ddl_op_ID = $("#ddl_opName").val();
                ItemTypeDdlBind(ddl_op_ID);

                CalCulateTotalAndSet(option);
              
            }
           
        });
    }
    else {
        debugger;
        var ddlItemType_ID1 = ddlItemType_ID;
        var ddl_ItemName_ID1 = ddl_ItemName_ID;
        if (ddlItemType_ID1 == "OF") {
            var op_id = $("#ddl_opName").val();
            GetOpNameList();
            
            $("#ddl_opName").val(op_id).change();
        }
        if (ddlItemType_ID1 == "OW") {
            var a = $(i).closest('tr').parent().parent().parent().parent().next().find(" table >tbody tr input[value='IW']").closest('tr')[0];
            var optionISFG = $(i).closest('tr').parent().parent().parent().parent().next().find(" #tbl_hdn_ddl_op_id").val();
            var ddlItemType_ID_ISFG = $(i).closest('tr').parent().parent().parent().parent().next().find(" table >tbody tr input[value='IW']").val();
            if (optionISFG != null) {
                deleteRow(a, e, optionISFG, ddl_ItemName_ID, ddlItemType_ID_ISFG);
            }
        }
        //ShowItemType_AfterDelete(i,ddl_ItemName_ID1, ddlItemType_ID1);
        debugger;
        $(i).closest('tr').remove();
        
        CalCulateTotalAndSet(option);
    
        $("#tbladd tbody tr").css("background-color", "#ffffff");
        var ddl_op_ID = $("#ddl_opName").val();
        ItemTypeDdlBind(ddl_op_ID);

       
    }

    $("#ddl_opName").val(option).change();
    SerialNoAfterDelete();
    ResetOperationsFields();
};
function CalCulateTotalAndSet(option) {
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    debugger
    //--------------------------------------------------
    var itemVal = 0;
    $('#tbl_' + option + ' tbody tr').each(function () {
        var CurrentRow1 = $(this);
        var tbl_hdn_totalitemvalue = CurrentRow1.find("#tbl_hdn_totalitemvalue").val();
        if (CurrentRow1.find("#tbl_hdn_ddl_ItemType_ID").val() != "OW" && CurrentRow1.find("#tbl_hdn_ddl_ItemType_ID").val() != "OF")
        {
            itemVal = parseFloat(itemVal) + parseFloat(tbl_hdn_totalitemvalue);
        }

    });
    var getRow = $('#tbl_' + option + ' tbody tr').find("#tbl_hdn_ddl_ItemType_ID[value=OF]").closest("tr");
    if (getRow.length ==0) {
        getRow = $('#tbl_' + option + ' tbody tr').find("#tbl_hdn_ddl_ItemType_ID[value=OW]").closest("tr");
    }
    
    var txtQuantityItemName = getRow.find("#tbl_hdn_txtQuantityItemName").val();
    var itemcost1 = parseFloat(parseFloat(itemVal) / parseFloat(txtQuantityItemName)).toFixed(RateDigit);
    itemVal = parseFloat(itemVal).toFixed(ValDigit);

    getRow.find("#tbl_hdn_totalitemvalue").val(itemVal);
    getRow.find("#op_item_value").text(itemVal);
    getRow.find("#tbl_hdn_itemcost").val(itemcost1);
    getRow.find("#op_item_cost").text(itemcost1);

    var NextRow = $('#tbl_' + option + ' tbody').parent().parent().parent().next();
    var nextOption = NextRow.find("#tbl_hdn_ddl_op_id").val();
    var getNextRow = $('#tbl_' + nextOption + ' tbody tr').find("#tbl_hdn_ddl_ItemType_ID[value=IW]").closest("tr");
    if (getNextRow.length != 0) {
        getNextRow.find("#tbl_hdn_totalitemvalue").val(itemVal);
        getNextRow.find("#op_item_value").text(itemVal);
        getNextRow.find("#tbl_hdn_itemcost").val(itemcost1);
        getNextRow.find("#op_item_cost").text(itemcost1);

        if (nextOption != null && nextOption != "") {
            CalCulateTotalAndSet(nextOption);
        }
    }
    

    
        //------------------------------
}
function GetOpNameList() {

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BillofMaterial/GetBomOpNameList",
        data: {},
        success: function (data) {
            var arr = JSON.parse(data);
            debugger;
            var options = '<option value="0">---Select---</option>';
            for (let i = 0; i < arr.length; i++) {
                options += '<option value=' + arr[i].op_id + '>' + arr[i].op_name + '</option>';
            }
            $("#ddl_opName").html(options);
            
        }

    })


}
function SerialNoAfterDelete() {
    debugger;
    var SerialNo = 0;
    var SerialNoParent = 0;
    var totalVal = 0;
    var ValDigit = $("#ValDigit").text();    
    
    $("#TotalVal").text('');
    $("#tbladd >tbody >tr").each(function (i, row) {
        debugger;  //changing td to id by SM
        var currentRow = $(this);
        var op_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
        SerialNoParent = SerialNoParent + 1;
        currentRow.find("#srno").text(SerialNoParent);
        $("#tbl_" + op_id_tbl + " >tbody >tr").each(function (j, rows) {
            var currentRowChild = $(this);             
            SerialNo = SerialNo + 1;
            
           // currentRowChild.find("#tbl_hdn_seq_no").val(SerialNo);
            var tbl_hdn_totalitemvalue =Number( currentRowChild.find("#tbl_hdn_totalitemvalue").val());
            var totalitemvalue = (parseFloat(tbl_hdn_totalitemvalue).toFixed(ValDigit));
            totalVal +=Number(totalitemvalue);
        });        
    });
    $("#TotalVal").text((parseFloat(totalVal).toFixed(ValDigit)));
    
};
function HideItemType_AfterAdd(item_id, ddlItemType_ID) {
    debugger;
    if (item_id != '') {
        ddl_OPNameChange();
        //$("#ddlItemType option[value='OW']").show();
        //$("#ddlItemType option[value='OF']").show();
        //if ($("#tbladd tbody").closest('tr').length > 1)
        //$("#ddlItemType option[value='IW']").show();

        //if (ddlItemType_ID == "OW" || ddlItemType_ID == "OF") {
        //    $("#ddlItemType option[value='OF']").hide();
        //    $("#ddlItemType option[value='OW']").hide();
        //    $('#ddlItemType').val("0").change();
        //}
        //else {
        //    $('#ddlItemType').val("0");
        //    $('#ddlItemType').val("0").change();
        //}
        //if (ddlItemType_ID == "IW") {
        //    $("#ddlItemType option[value=" + ddlItemType_ID + "]").hide();
        //    $('#ddlItemType').val("0").change();
        //}
        //else {
        //    $('#ddlItemType').val("0").change();
        //}
        $('#txtQuantityItemName').val('');
    }
}
function ShowItemType_AfterDelete(i,ddl_ItemName_ID, ddlItemType_ID) {
    debugger;
    //$("#ddl_ItemName option[value=" + ddl_ItemName_ID + "]").removeClass("select2-hidden-accessible");
    //$('#ddl_ItemName').val("0").change();

    $("#ddlItemType option[value=" + ddlItemType_ID + "]").show();
    $('#ddlItemType').val("0");//.change();
    if (ddlItemType_ID == "OW" || ddlItemType_ID == "OF") {
        var CheckNextRow = $(i).closest('tr').parent().parent().parent().parent().next().find(" #tbl_hdn_ddl_op_id").val();
        if (CheckNextRow == null || CheckNextRow == "") {
            $("#ddlItemType option[value='OW']").show();
            $("#ddlItemType option[value='OF']").show();
            $('#ddlItemType').val("0").change();
        }
        else {
            $("#ddlItemType option[value='OW']").show();
        }

    }
}
function ddl_OPNameChange() {
    debugger;
    var ddl_op_ID = $("#ddl_opName").val();
    if (ddl_op_ID != "0") {
        $("#vm_ddl_opName").css("display", "none");
        $("[aria-labelledby='select2-ddl_opName-container']").css("border-color", "#ced4da")
    }
    else {
        $('#vm_ddl_opName').text($("#valueReq").text());
        $("#vm_ddl_opName").css("display", "block");   
        $("[aria-labelledby='select2-ddl_opName-container']").css("border-color", "red")
    }
    ItemTypeDdlBind(ddl_op_ID);
    //AddRemoveItemName(ddl_op_ID);
    //var flag = 'N'
    //$("#ddlItemType option[value='OW']").hide();
    //$("#ddlItemType option[value='OF']").hide();
    ////$("#ddlItemType option[value='IW']").show();
    //$('#ddlItemType').val("0");
    //if ($("#tbladd tbody").closest('tr').length > 1 || ($("#tbladd tbody").closest('tr').find("#tbl_hdn_ddl_op_id").val() != ddl_op_ID)) {
    //    $("#ddlItemType option[value='IW']").show();

    //}
       
    //if ($("#tbladd tbody tr").length == 1 || ($("#tbladd tbody").closest('tr').find("#tbl_hdn_ddl_op_id").val() == ddl_op_ID)) 
    //    $("#ddlItemType option[value='IW']").hide();
    

    //if ($("#tbladd tbody tr").length == 0) {
    //    $("#ddlItemType option[value='IW']").hide();
    //    $("#ddlItemType option[value='OW']").hide();
    //    $("#ddlItemType option[value='OF']").hide();
    //    $("#ddlItemType option[value='B']").hide();
    //}
    //else {
    //    $("#ddlItemType option[value='B']").show();
    //}
    //var IsPaked = "N";
    //$("#tbladd >tbody >tr").each(function (i, row) {
    //    debugger;
    //    var currentRow = $(this);
    //    var op_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
    //    IsPaked = $("#tbl_" + ddl_op_ID + " >tbody >tr td").find("#tbl_hdn_ddl_ItemType_ID[value='P']").val();
    //    if (ddl_op_ID == op_id_tbl) {
    //        $("#tbl_" + ddl_op_ID + " >tbody >tr").each(function (j, rows) {
    //            var currentRowChild = $(this);
    //            //var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
    //            //if (itemtypeid == "O") {
    //            //    flag = 'Y';
    //            //}
    //            debugger;
    //            var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
    //            if (itemtypeid == "OW" || itemtypeid == "OF") {
    //                flag = 'Y';
                    
    //            }
                
    //            if (itemtypeid == "IW") {
    //                $("#ddlItemType option[value='IW']").hide();
    //                $('#ddlItemType').val("0");//.change();
    //            }

    //        });
    //        var CheckNextRow = currentRow.next().find("#tbl_hdn_ddl_op_id").val();
    //        if (flag == 'Y') {
    //            $("#ddlItemType option[value='OW']").hide();
    //            $("#ddlItemType option[value='OF']").hide();
    //            $('#ddlItemType').val("0");
    //        }
    //        else {
    //            if (CheckNextRow == null || CheckNextRow == "") {
    //                $("#ddlItemType option[value='OW']").show();
    //                $("#ddlItemType option[value='OF']").show();
    //                $('#ddlItemType').val("0");
    //            }
    //            else {
    //                $("#ddlItemType option[value='OW']").show();
    //                $('#ddlItemType').val("0");
    //            }
            
    //        }
    //    }
    //});
    //if (IsPaked == "P") {
    //    $("#ddlItemType option[value='P']").hide();
    //}
    //else {
    //    $("#ddlItemType option[value='P']").show();
    //}


    //if (flag == 'Y') {
    //    $("#ddlItemType option[value='O']").hide();
    //    $('#ddlItemType').val("0").change();
    //}
    //else if (flag == 'N') {
    //    $("#ddlItemType option[value='O']").show();
    //    $('#ddlItemType').val("0").change();
        
    //}

}
function ItemTypeDdlBind(ddl_op_ID) {
    var flag = 'N'
    $("#ddlItemType option[value='OW']").hide();
    $("#ddlItemType option[value='OF']").hide();
    //$("#ddlItemType option[value='IW']").show();
    $('#ddlItemType').val("0");
    if ($("#tbladd tbody").closest('tr').length > 1 || ($("#tbladd tbody").closest('tr').find("#tbl_hdn_ddl_op_id").val() != ddl_op_ID)) {
        $("#ddlItemType option[value='IW']").show();

    }

    if ($("#tbladd tbody tr").length == 1 || ($("#tbladd tbody").closest('tr').find("#tbl_hdn_ddl_op_id").val() == ddl_op_ID))
        $("#ddlItemType option[value='IW']").hide();


    if ($("#tbladd tbody tr").length == 0) {
        $("#ddlItemType option[value='IW']").hide();
        $("#ddlItemType option[value='OW']").hide();
        $("#ddlItemType option[value='OF']").hide();
        $("#ddlItemType option[value='B']").hide();
        $("#ddlItemType option[value='OS']").hide();
    }
    else {
        $("#ddlItemType option[value='B']").show();
        $("#ddlItemType option[value='OS']").show();
    }
    var IsPaked = "N";
    $("#tbladd >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var op_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
        IsPaked = $("#tbl_" + ddl_op_ID + " >tbody >tr td").find("#tbl_hdn_ddl_ItemType_ID[value='P']").val();
        if (ddl_op_ID == op_id_tbl) {
            $("#tbl_" + ddl_op_ID + " >tbody >tr").each(function (j, rows) {
                var currentRowChild = $(this);
                //var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
                //if (itemtypeid == "O") {
                //    flag = 'Y';
                //}
                debugger;
                var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
                if (itemtypeid == "OW" || itemtypeid == "OF") {
                    flag = 'Y';

                }

                if (itemtypeid == "IW") {
                    $("#ddlItemType option[value='IW']").hide();
                    $('#ddlItemType').val("0");//.change();
                }

            });
            var CheckNextRow = currentRow.next().find("#tbl_hdn_ddl_op_id").val();
            if (flag == 'Y') {
                $("#ddlItemType option[value='OW']").hide();
                $("#ddlItemType option[value='OF']").hide();
                $('#ddlItemType').val("0");
            }
            else {
                if (CheckNextRow == null || CheckNextRow == "") {
                    $("#ddlItemType option[value='OW']").show();
                    $("#ddlItemType option[value='OF']").show();
                    $('#ddlItemType').val("0");
                }
                else {
                    $("#ddlItemType option[value='OW']").show();
                    $('#ddlItemType').val("0");
                }

            }
        }
    });
    if (IsPaked == "P") {
        //$("#ddlItemType option[value='P']").hide();
    }
    else {
        //$("#ddlItemType option[value='P']").show();
    }
}

function OnClickIconBtn(e, item_id, item_mame, edit) {
    debugger;

    var ItmCode = "";
   // var ItmName = "";

    ItmCode = $("#ddl_ProductName").val();
   
    //ItmName = $("#ddl_ProductName option:selected").text();
    if (edit == 'edit') {
        ItmCode = item_id;
      //  ItmName = item_mame;
    }
    ItemInfoBtnClick(ItmCode);
    //if (ItmCode != "" && ItmCode != null) {
    //    try {
    //        $.ajax(
    //            {
    //                type: "POST",
    //                url: "/ApplicationLayer/DPO/GetPOItemDetail",
    //                data: { ItemID: ItmCode },
    //                success: function (data) {
    //                    debugger;
    //                    if (data == 'ErrorPage') {
    //                        PO_ErrorPage();
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
    //                            $("#SpanItemDescription").text(ItmName);
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
    //                        else {
    //                            $("#Txt_ItmOEMNo").val("");
    //                            $("#Txt_ItmSampCode").val("");
    //                            $("#Txt_ItmRefNo").val("");
    //                            $("#Txt_ItmHSNCode").val("");
    //                            $("#SpanItemDescription").text("");
    //                            $("#myCarousel").html("");
    //                        }
    //                    }
    //                    else {
    //                        $("#Txt_ItmOEMNo").val("");
    //                        $("#Txt_ItmSampCode").val("");
    //                        $("#Txt_ItmRefNo").val("");
    //                        $("#Txt_ItmHSNCode").val("");
    //                        $("#SpanItemDescription").text("");
    //                        $("#myCarousel").html("");
    //                    }
    //                },
    //            });
    //    } catch (err) {
    //        console.log("GetMenuData Error : " + err.message);
    //    }
    //} 
}
function GetItemCost(CompId, BranchId, ddl_ProductId, fieldName, CurrRow) {
    var cost = 0;
    debugger;
    try {
        $.ajax(
            {
                type: "POST",
                url: "/ApplicationLayer/BillofMaterial/GetItemCost",
                data: {
                    CompId: CompId, BranchId: BranchId, ddl_ProductId: ddl_ProductId
                },
                success: function (data) {
                    debugger;
                    if (data !== null && data !== "" && data != "ErrorPage") {
                        var arr = [];
                        var RateDigit = $("#RateDigit").text();
                        arr = JSON.parse(data);
                        if (arr.Table.length > 0) {
                            cost = arr.Table[0].cost_price;
                            if (CurrRow != null) {
                                CurrRow.find("#" + fieldName).val(parseFloat(CheckNullNumber(cost)).toFixed(RateDigit));
                            } else {
                                $("#" + fieldName).val(parseFloat(CheckNullNumber(cost)).toFixed(RateDigit));
                            }
                            
                        }
                        else {
                            cost = 0;
                        }
                        if (fieldName == "td_AltItemCost") {
                            GetBomUomNameItemWise(ddl_ProductId, "td_AltUom", "tdAltUomId", CurrRow, arr);
                        } else {
                            GetBomUomNameItemWise(ddl_ProductId, "Txt_UOM_itemName", "hdn_Txt_UOM_itemName", CurrRow, arr);
                        }
                        
                    }
                    else {
                        cost = 0;
                    }
                },
            });
    } catch (err) {
    }
    return cost;
} 
function OnChangetxt_Quantity() {
    debugger;
    var op_name = $('#txt_Quantity').val().trim();
    /*Add by Hina on 02-03-2024 to show blank instaed of 0 in inserted field*/
    if (op_name == "0") {
        $('#txt_Quantity').val("");
    }
    if (op_name > 0) {/*Code End*/
        if (op_name != "" && op_name != "0") {
            document.getElementById("vm_Quantity").innerHTML = "";
            $("#txt_Quantity").css("border-color", "#ced4da");
        }
        else {
            document.getElementById("vm_Quantity").innerHTML = $("#valueReq").text();
            $("#txt_Quantity").css("border-color", "red");
        }
    }
    else {
        document.getElementById("vm_Quantity").innerHTML = $("#valueReq").text();
        $("#txt_Quantity").css("border-color", "red");
        $('#txt_Quantity').val("")
    }
    
}
 
function validateBOMDetailInsert() {
    debugger;

    var btn = $("#hdnsavebtn").val();
    if (btn == "AllreadyclickSaveBtn") { /**Added this Condition by Nitesh 11-01-2024 for Disable Save btn after one Click**/
        $("#btn_save").attr("disabled", true);
        $("#btn_save").css("filter", "grayscale(100%)");
        return false;
    }


    var ddl_ProductName = $("#ddl_ProductName").val();
    var ValidationFlag = true;
    if (ddl_ProductName == "0") {
        document.getElementById("vmddl_ProductName").innerHTML = $("#valueReq").text();
        $("[aria-labelledby='select2-ddl_ProductName-container']").css("border-color", "red");
        ValidationFlag = false;
        
    }
    var txt_Quantity = $("#txt_Quantity").val();
    if (txt_Quantity == "0" || txt_Quantity == "") {
        document.getElementById("vm_Quantity").innerHTML = $("#valueReq").text();
        $("#txt_Quantity").css("border-color", "red");
        ValidationFlag = false;
       
    }

    //Added by Suraj on 29-03-2024 Start
    let outputFGQty = $("#tbladd > tbody > tr table tbody tr #tbl_hdn_ddl_ItemType_ID[value='OF']").closest('tr').find('#tbl_hdn_txtQuantityItemName').val()
    if (parseFloat(CheckNullNumber(txt_Quantity)) !== parseFloat(CheckNullNumber(outputFGQty))) {
        swal("", $("#TheOutputFGQuantityDoesNotMatchTheBillOfMaterialsQuantity").text(), "warning");
        ValidationFlag = false;
    }
    //Added by Suraj on 29-03-2024 End
    if (ValidationFlag == false) {
        return false;
    }
    if ($("#tbladd >tbody >tr").length == 0) {
        swal("", $("#noitemselectedmsg").text(), "warning");

        ValidationFlag = false;
        return false;
    }
    //if (ValidationFlag == true) {
    //    var Inflag = 'N';
    //    var Outflag = 'N';
    //    $("#tbladd >tbody >tr").each(function (i, row) {
    //        debugger;
    //        var Input = 'N';
    //        var Output = 'N';
    //        Inflag = 'N';
    //        Outflag = 'N';
    //        var currentRow = $(this);
    //        var op_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
    //        $("#tbl_" + op_id_tbl + " >tbody >tr").each(function (j, rows) {
    //            var currentRowChild = $(this);
    //            var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
    //            if (itemtypeid == "I") {
    //                Input = 'Y';
    //                Inflag = 'Y';
    //            }
    //            if (itemtypeid == "O") {
    //                Output = 'Y';
    //                Outflag = 'Y';
    //            }

    //        });
    //        if (Input == "Y" && Output == "Y") {
    //            ValidationFlag = true;
    //        }
    //        else {
    //            ValidationFlag = false;
    //            return false;
    //        }
    //    });

    //    if (Inflag == "Y" && Outflag == "Y") {

    //    }
    //    else {
    //        swal("", $("#Eachoptreqatleastonin_outputitem").text(), "warning");
    //        return false;
    //    }

    //    if (ValidationFlag == true) {
    //        InsertItemDetail();
    //        return true;
    //    }
    //}
    if (ValidationFlag == true) {
        var Inflag = 'N';
        var Outflag = 'N';
        var Msg = "";
     
        var lastOP = $("#tbladd >tbody >tr:last-child td #tbl_hdn_ddl_op_id").val();
        $("#tbladd >tbody >tr").each(function (i, row) {
            debugger;
            var Inflag = 'N';
            var Outflag = 'N';

            var InputRM = 'N';
            var InputSFG = 'N';
            var OutputSFG = 'N';
            var OutputFG = 'N';
            debugger;  //changing td to id by SM
            var currentRow = $(this);
            var op_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
            var op_Name = currentRow.find("#opname").text();
            
            var SrNo = currentRow.find("#srno").text();
            if (SrNo == 1) {
                $("#tbl_" + op_id_tbl + " >tbody >tr").each(function (j, rows) {
                    var currentRowChild = $(this);
                    var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
                    if (itemtypeid == "IR") {
                        InputRM = 'Y';
                        Inflag = 'Y';
                    }
                    
                    if (lastOP == op_id_tbl) {
                        if (itemtypeid == "OF") {
                            OutputFG = 'Y';
                            Outflag = 'Y';
                        }
                    }
                    else {
                        if (itemtypeid == "OW") {
                            OutputSFG = 'Y';
                            Outflag = 'Y';
                        }
                    }
                    
                });
            }
            else {
                $("#tbl_" + op_id_tbl + " >tbody >tr").each(function (j, rows) {
                    var currentRowChild = $(this);
                    var itemtypeid = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
                    if (itemtypeid == "IW") {
                        Inflag = 'Y';
                        InputSFG = 'Y';
                    }
                    
                    if (lastOP == op_id_tbl) {
                        if (itemtypeid == "OF") {
                            OutputFG = 'Y';
                            Outflag = 'Y';
                        }
                    }
                    else {
                        if (itemtypeid == "OW") {
                            OutputSFG = 'Y';
                            Outflag = 'Y';
                        }
                    }
                    
                });
            }
            if (lastOP == op_id_tbl) {

                if (SrNo == 1) {
                    if (InputRM == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#InputRMNotFoundInOperation").text()+" " + op_Name;
                    }
                    if (OutputFG == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#OutputFGNotFoundInOperation").text() +" " + op_Name;
                    }
                }
                else {
                    if (InputSFG == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#InputSFGNotFoundInOperation").text() +" " + op_Name;
                    }
                    if (OutputFG == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#OutputFGNotFoundInOperation").text() +" " + op_Name;
                    }
                }
                
            }

            if (lastOP != op_id_tbl) {

                if (SrNo == 1) {
                    if (InputRM == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#InputRMNotFoundInOperation").text() +" " + op_Name;
                    }
                    if (OutputSFG == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#OutputSFGNotFoundInOperation").text() +" " + op_Name;
                    }
                }
                else {
                    if (InputSFG == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#InputSFGNotFoundInOperation").text() +" " + op_Name;
                    }
                    if (OutputSFG == "N") {
                        if (Msg != "") {
                            Msg += "\n";
                        }
                        Msg += " " + $("#OutputSFGNotFoundInOperation").text() +" " + op_Name;
                    }
                }

                
            }
            
            if (Inflag == "Y" && Outflag == "Y") {
                //ValidationFlag = true;
            }
            else {
                ValidationFlag = false;
               // return false;
            }
        });
        //var IsOF = $("#tbladd >tbody >tr table >tbody >tr td #tbl_hdn_ddl_ItemType_ID[value='OF']").val();
        //    if (IsOF == "OF") {
        //        OutputFG = "Y";
        //    }

        //if (OutputFG == "N") {
        //    if (Msg != "") {
        //        Msg += "\n";
        //    }
        //    Msg += " Output-FG not found in last operation.";
        //}
        //if (Inflag == "Y" && Outflag == "Y" && OutputFG == "Y") {

        //}
        //else {
        //    swal("", Msg, "warning");
        //    //swal("", $("#Eachoptreqatleastonin_outputitem").text(), "warning");
        //    return false;
        //}

        if (ValidationFlag == true) {
            //InsertItemDetail();
            if (InsertItemDetail() == true) {
                debugger;
                $("#chk_act_status").prop('disabled', false);
                $("#chk_def_status").prop('disabled', false);
                $("#hdnsavebtn").val("AllreadyclickSaveBtn");
            }
            else {
                return false;
            }
            return true;
        }
        else {
            swal("", Msg, "warning");
            return false;
        }
    }
    else {
        return false;
    }
}
function InsertItemDetail() {
    debugger;
    if (navigator.onLine === true)/*Checing For Internet is open or not*/ {
        debugger;
        FinalItemDetail = InsertItemAttributeDetails();

        debugger;
        if (FinalItemDetail == false) {
            return false;
        } else {
            var ItemAttrDt = JSON.stringify(FinalItemDetail);
            $('#hdnbomitemattr').val(ItemAttrDt);

            var AltItemDt = JSON.stringify(InsertAlternateItemDetails());
            $('#hdnBomAltItemDetail').val(AltItemDt);

        }
        
        return true;
    }
    else {
       
        return false;
    }
};
function InsertItemAttributeDetails() {
    debugger;
    var Error = "N";
    var AttributeList = [];
    var itemDTransType = sessionStorage.getItem("ItmDTransType");
    var itmcode = sessionStorage.getItem("EditItemCode");
    var TransType = '';
    if (itemDTransType === 'Update') {
        TransType = 'Update';
    }
    else {
        TransType = 'Save';
    }
    var AttrList = [];
    var pr_id = $("#ddl_ProductName").val();
    var rev_no = $("#txt_RevisionNumber").val();
    var seq_number = 1;
    $("#tbladd >tbody >tr").each(function (i, row) {
        debugger;
        var currentRow = $(this);
        var pr_id_tbl = currentRow.find("#tbl_hdn_ddl_op_id").val();
        var opname = currentRow.find("#opname").text();
        
        $("#tbl_" + pr_id_tbl + " >tbody >tr").each(function (j, rows) {
            var currentRowChild = $(this);
            debugger;
            var Item_type = currentRowChild.find("#tbl_hdn_ddl_ItemType_ID").val();
            var ddl_ItemName_ID = currentRowChild.find("#tbl_hdn_ddl_ItemName_ID").val();
            //var OpItemName = currentRowChild.find("#op_item_name").text();
            var uom_id = currentRowChild.find("#tbl_hdn_UOM_itemName").val();
            if (uom_id == "0" || uom_id == "") {
                currentRowChild.find("#op_uom_name").css("border", "2px solid red");
                Error = "Y";
            } else {
                currentRowChild.find("#op_uom_name").css("border", "");
            }
            var qty = currentRowChild.find("#tbl_hdn_txtQuantityItemName").val();
            var item_cost = currentRowChild.find("#tbl_hdn_itemcost").val();
            var item_value = currentRowChild.find("#tbl_hdn_totalitemvalue").val();
            var seq_no = seq_number + '_' + currentRowChild.find("#tbl_hdn_seq_no").val();//currentRowChild.find("#tbl_hdn_seq_no").val();
            //var ItemTypeSeqNo = currentRowChild.find("#tbl_hdn_seq_no").val();
            if (ddl_ItemName_ID != '') {
                AttrList.push({ op_product_id: pr_id, op_rev_no: rev_no, op_op_id: pr_id_tbl, op_Item_type: Item_type, op_item_id: ddl_ItemName_ID, op_uom_id: uom_id, op_qty: qty, op_item_cost: item_cost, op_item_value: item_value, seq_no,})
            }
        });
        seq_number++;
    });
    if (Error == "Y") {
        swal("", $("#TheUnitOfMeasureUOMIsMissingForCertainItems").text(),"warning");
        return false;
    } else {
        return AttrList;
    }
    
};

function ddlItemType_onchange() {
    debugger;
    var ddlItemType = $('#ddlItemType').val();
    if (ddlItemType != "0") {
        $("#spanItemType").css("display", "none");
        $("#ddlItemType").css("border-color", "#ced4da");
        $("#vmddl_ItemName").css("display", "none");
        $("[aria-labelledby='select2-ddl_ItemName-container']").css("border-color", "#ced4da");
        $("#spanQuantityItemName").css("display", "none");
        $("#txtQuantityItemName").css("border-color", "#ced4da");
    }
    debugger;
    var ddl_opName = $("#ddl_opName").val();
    var ddlItemTypeTxt = $('#ddlItemType').val().trim();
    if (ddlItemTypeTxt == "IW") {
        var itmID = "";
        var itmName = "";
        var uom = "";
        var uomid = "";
        var Qty = "";
        var ItemCost = "";
        var lenTbl = $("#tbladd >tbody >tr").length;
        debugger;  //changing td to id by SM
        var RowNo = $("#tbladd >tbody > #tr_" + ddl_opName).find("#srno").text().trim();
        if (RowNo == null || RowNo == "") {
            RowNo = lenTbl + 1;
        }
        $("tbl_" + ddl_opName).closest('tr').prev('tr').find("td #tbl_hdn_ddl_ItemType_ID[value='OW']").val()
        var IsOW = $("#tbladd >tbody >tr:nth-child(" + (RowNo - 1) + ") table >tbody >tr td #tbl_hdn_ddl_ItemType_ID[value='OW']").val();
        if (IsOW == "OW") {
            var IsOWCheck
            $("#tbladd >tbody >tr:nth-child(" + (RowNo - 1) + ") table >tbody >tr").find("td #tbl_hdn_ddl_ItemType_ID[value='OW']").closest('tr').each(function () {
                var CurrentRow = $(this);
                IsOWCheck = CurrentRow.find("td #tbl_hdn_ddl_ItemType_ID").val();
                if (IsOWCheck == "OW") {
                    debugger;  //changing td to id by SM
                    itmName = CurrentRow.find("#op_item_name div").text().trim();
                    itmID = CurrentRow.find("td #tbl_hdn_ddl_ItemName_ID").val().trim();
                    uom = CurrentRow.find("#op_uom_name").text().trim();
                    uomid = CurrentRow.find("td #tbl_hdn_UOM_itemName").val().trim();
                    Qty = CurrentRow.find("#op_qty").text().trim();
                    ItemCost = CurrentRow.find("#tbl_hdn_itemcost").val().trim();
                    ItemValue = CurrentRow.find("#tbl_hdn_totalitemvalue").val().trim();
                    $("#hdn_Item_Value").val(ItemValue);
                }


            })
            if (IsOWCheck != "OW") {
                swal("", $("#NoOutputSFGDefinedInPreviousOperation").text(), "warning");
                return false;
            }
        }
        else {
            swal("", $("#NoOutputSFGDefinedInPreviousOperation").text(), "warning");
            //$("#ddl_ItemName").html(`<optgroup class='def-cursor' id="Textddl_itemName" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'>
            //<option data-uom="0" value="0">---Select---</option>
            //    </optgroup>`);
            SetItemNameSelectOnly();
            $("#ddl_ItemName").val(0);
            ResetOperationsFields();
            $("#ddl_ItemName ,#txtQuantityItemName").attr("disabled", false);
            return false;
        }
        
        BindSFGItemsInBomItemType(itmID, itmName, uom, uomid, Qty, ItemCost);
    }
    if (ddlItemTypeTxt == "IR") {
        BindItemNameDDL(ddlItemTypeTxt,"Y", "N","new");
        ResetOperationsFields();
    }
    if (ddlItemTypeTxt == "OW") {
        BindItemNameDDL(ddlItemTypeTxt, "Y", "N", "new");
        ResetOperationsFields();
    }
    if (ddlItemTypeTxt == "B") {
        BindItemNameDDL(ddlItemTypeTxt, "N", "N", "new");
        ResetOperationsFields();
    }
    //if (ddlItemTypeTxt == "A") {
    //    BindItemNameDDL(ddlItemTypeTxt, "N", "N", "new");
    //    ResetOperationsFields();
    //}
    if (ddlItemTypeTxt == "OS") {
        BindItemNameDDL(ddlItemTypeTxt, "N", "N", "new");
        ResetOperationsFields();
    }
    if (ddlItemTypeTxt == "P") {
        BindItemNameDDL(ddlItemTypeTxt, "N", "Y", "new");
        ResetOperationsFields();
    }
    if (ddlItemTypeTxt == "OF") {
        var itmID = $("#ddl_ProductName").val();
        var itmName = $("#ddl_ProductName option:selected").text();
        var uom = $("#UOM").val();
        var uomid = $("#UOMID").val();
        var Qty = $("#txt_Quantity").val();
        BindSFGItemsInBomItemType(itmID, itmName, uom, uomid, Qty, "0");
    

    }
    if (ddlItemTypeTxt != "OF" && ddlItemTypeTxt != "IW") {
        $("#ddl_ItemName ,#txtQuantityItemName").attr("disabled", false);
    }

}
function ResetOperationsFields() {

    /*$("#Txt_UOM_itemName").val("");*/
    let s = '<option value="0">---Select---</option>';
    $("#Txt_UOM_itemName").html(s);
    $("#hdn_Txt_UOM_itemName").val("");
    $("#txtQuantityItemName").val("");
    $("#ddl_ItemName").val(0);
    $("#divUpdate").css("display", "none");
    $("#divAddNew").css("display", "block");

    $('[aria-labelledby="select2-ddl_ItemName-container"]').css("border-color", "#ced4da");
    document.getElementById('vmddl_ItemName').innerHTML = "";
    document.getElementById('spanQuantityItemName').innerHTML = "";
    $("#txtQuantityItemName").css("border-color", "#ced4da");
    $("#ddl_ItemName ,#txtQuantityItemName ,#ddl_opName ,#ddlItemType").attr("disabled", false);
}
function BindSFGItemsInBomItemType(itmID, itmName,Uom,UomId,Qty,ItemCost) {
    //var hdncompid = $("#hdncompid").val();
    //var hdnBranchId = $("#hdnBranchId").val(); 
    //GetItemCost(hdncompid, hdnBranchId, itmID);
    // $("#ddl_ItemName").html("");
    debugger;
    $("#ddl_ItemName").html(`<optgroup class='def-cursor' id="Textddl_itemName" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'>
            <option data-uom="0" value="0">---Select---</option>
            <option data-uom="${Uom}" value="${itmID}">${itmName}</option>
                </optgroup>`);
    $("#ddl_ItemName").val(itmID);
    //$("#Txt_UOM_itemName").val(Uom);
    let s = '<option value="' + UomId + '">' + Uom+'</option>';
    $("#Txt_UOM_itemName").html(s);
    $("#hdn_Txt_UOM_itemName").val(UomId);
    $("#txtQuantityItemName").val(parseFloat(Qty).toFixed($("#QtyDigit").text()));
    $("#hdnitemcost").val(ItemCost);
    
  
    $("#ddl_ItemName ,#txtQuantityItemName").attr("disabled", true);
}
function checkBomItem(data, selected, TableNameID) {

    var Flag = "N";
    $(TableNameID + " tbody tr").each(function () {
        var currentRow = $(this);
        debugger;
       // var rowno = currentRow.find(HiddenSrNoID).val();
        var itemId = currentRow.find("#tbl_hdn_ddl_ItemName_ID").val();
        if (itemId != null) {
            if (itemId == data.id) {
                if (itemId == "0") {
                }
                else if (selected == itemId) {
                }
                else {
                    Flag = "Y";
                }
                return false;
            }
            
        }
    });
    debugger;
    if (Flag == "Y") {
        return false;
    }
    else {
        return true;
    }
}
function OnChangeTxtQty() {
    //debugger;
    var txtQuantityItemName = $('#txtQuantityItemName').val().trim();
    if (txtQuantityItemName >0) {
        document.getElementById("spanQuantityItemName").innerHTML = "";
        $("#txtQuantityItemName").css("border-color", "#ced4da");
    }
    else {
        document.getElementById("spanQuantityItemName").innerHTML = $("#valueReq").text();
        $("#txtQuantityItemName").css("border-color", "red");
    }
}
function disablePrName_Qty() {
    
    $("#ddl_ProductName").prop("disabled", true);
    $("#txt_Quantity").prop("readonly", true);

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
function OnclickReqQty(e) {
    debugger;
    var QtyDecDigit = $("#QtyDigit").text();
    var txtQuantityItemName = $("#txtQuantityItemName").val();
    //if (txtQuantityItemName == "" || txtQuantityItemName == "0") {
    if (txtQuantityItemName ==0) {
        $("#spanQuantityItemName").text($("#valueReq").text());
        $("#spanQuantityItemName").css("display", "block");
        $("#txtQuantityItemName").css("border-color", "red")
        $("#txtQuantityItemName").val('');
        return false;
    }
    else {
        $("#spanQuantityItemName").css("display", "none");
        $("#txtQuantityItemName").css("border-color", "#ced4da");
    }
    if (isNaN(parseFloat(txtQuantityItemName).toFixed(QtyDecDigit))) {
        //txtQuantityItemName = 0;
        $("#txtQuantityItemName").val('');
    }
    else {
        $("#txtQuantityItemName").val(parseFloat(txtQuantityItemName).toFixed(QtyDecDigit));
    }
}
function InterValueonly(event) {
    debugger;
    if (event.which == 46) {
        if ($(this).val().indexOf('.') != -1) {
            return false;
        }
    }
    if (event.which != 8 && event.which != 0 && event.which != 46 && (event.which < 48 || event.which > 57)) {
        return false;
    }
}
function numValueOnly(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 58)) {
        return false;
    }
    return true;
}
function AmtFloatQtyonly(el, evt) {
    if (Cmn_FloatValueonly(el, evt, "#QtyDigit") == false) {
        return false;
    }
    //var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}

    //if (charCode == 46 && el.value.indexOf(".") !== -1) {
    //    return false;
    //}
    //debugger;
    //var QtyDecDigit = $("#QtyDigit").text();
    //var number = el.value.split('.');
    //if (number.length == 2 && number[1].length > (parseInt(QtyDecDigit) - 1)) {
    //    return false;
    //}

    return true;
}

function onclick_chk_act_status() {

    if ($("#chk_act_status").is(":checked"))
    {
        $("#chk_def_status").prop('disabled', false);        
    }
    else
    {
        $("#chk_def_status").prop('disabled', true);
        $("#chk_def_status").removeAttr('checked');
    }
};



function sortTableuser(tableid) {
    var table, rows, rows1, switching, i, x, y, x1, y1, shouldSwitch;
    table = document.getElementById(tableid);
    switching = true;

    while (switching) {

        switching = false;
        rows = table.rows;
        rows1 = table.rows;
        for (i = 0; i < (rows.length - 1); i++) {
            debugger;
            shouldSwitch = false;
            x1 = rows1[i].getElementsByTagName("TD")[2];
            y1 = rows1[i + 1].getElementsByTagName("TD")[2];
            if (x1.innerHTML.toLowerCase() == y1.innerHTML.toLowerCase()) {
                x = rows[i].getElementsByTagName("TD")[4];
                y = rows[i + 1].getElementsByTagName("TD")[4];

                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {

                    shouldSwitch = true;
                    break;
                }
            }

        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}
function sortTable(tableid) {
    var table, rows, switching, i, x, y, x1, y1, shouldSwitch;
    table = document.getElementById(tableid);
    switching = true;
    /*Make a loop that will continue until
    no switching has been done:*/
    while (switching) {
        //start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /*Loop through all table rows (except the
        first, which contains table headers):*/
        for (i = 0; i < (rows.length - 1); i++) {
            //start by saying there should be no switching:
            shouldSwitch = false;
            /*Get the two elements you want to compare,
            one from current row and one from the next:*/
            x = rows[i].getElementsByTagName("TD")[16];
            y = rows[i + 1].getElementsByTagName("TD")[16];

            //check if the two rows should switch place:
            if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {

                //if so, mark as a switch and break the loop:
                shouldSwitch = true;
                break;
            }
        }
        if (shouldSwitch) {
            /*If a switch has been marked, make the switch
            and mark that a switch has been done:*/
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
    sortTableuser(tableid);
}
function TableShorting() {
    $("#tbladd > #tbladd_body > tr").each(function () {
        var currentRow = $(this);
        var tbl_number = currentRow.find("td #tbl_hdn_ddl_op_id").val();

        sortTable('tbl_' + tbl_number + '');
       // sortTableuser('tbl_' + tbl_number + '');
    })
    
} 

/* -----------------------------Alternate Item section------------------------------ */
function OnClickAlternateItem(e) {
    debugger;
    var CurrRow = $(e.target).closest('tr');
    var ParentRow = $(e.target).closest('tr').parent().closest('tr'); //Targeting parent Row
    let ItemType = CurrRow.find("#op_Item_type_name").text().trim();
    let ItemTypeId = CurrRow.find("#tbl_hdn_ddl_ItemType_ID").val().trim();
    let ItemName = CurrRow.find("#op_item_name").text().trim();
    let ItemId = CurrRow.find("#tbl_hdn_ddl_ItemName_ID").val().trim();
    let ItemUom = CurrRow.find("#op_uom_name").text().trim();
    let ItemUomId = CurrRow.find("#tbl_hdn_UOM_itemName").val().trim();
    let ItemQty = CurrRow.find("#op_qty").text().trim();
    let OpName = ParentRow.find("#opname").text().trim();
    let OpId = ParentRow.find("#tbl_hdn_ddl_op_id").val().trim();

    var AltArr = new Array();

    $("#HdnTable_AltItemDetail > tbody > tr #td_altOpId:contains('" + OpId + "')").closest('tr').find("#td_altParentItemId:contains('" + ItemId + "')").closest('tr').each(function () {
        var CurrRow = $(this);
        //let RowNo = CurrRow.find("#HdnAltRowNo").text();
        let td_ItemName = CurrRow.find("#td_altItemName").text();
        let td_ItemNameId = CurrRow.find("#td_altItemId").text();
        let td_ItemUom = CurrRow.find("#td_altUom").text();
        let td_ItemUomId = CurrRow.find("#td_altUomId").text();
        let td_AltQty = CurrRow.find("#td_altItemQty").text();
        let td_AltItemCost = CurrRow.find("#td_altItemCost").text();
        AltArr.push({
            ItemName: td_ItemName, ItemNameId: td_ItemNameId, ItemUom: td_ItemUom
            , ItemUomId: td_ItemUomId, AltQty: td_AltQty, AltItemCost:td_AltItemCost
        })
        

    });

    $("#tbl_AlternateItem > tbody > tr").remove();

    let disableAlt = $("#disabledAltItem").val();
    let disabled = "";
    if (disableAlt == "Y") {
        disabled = "disabled";
        
    } else {
       
    }

    $.ajax({
        type: "POST",
        url: "/ApplicationLayer/BillofMaterial/GetAlternateItemDetail",
        data: { disabled: disableAlt, ArrListAltItemDetail: JSON.stringify(AltArr) },
        success: function (data) {

            $("#PopupAlternateItemDetail").html(data);
            $("#Alt_OpName").val(OpName);
            $("#hdn_Alt_OpId").val(OpId);
            $("#Alt_ItemType").val(ItemType);
            $("#hdn_Alt_ItemTypeId").val(ItemTypeId);
            $("#Alt_ItemName").val(ItemName);
            $("#hdn_Alt_ItemId").val(ItemId);
            $("#Alt_ItemUom").val(ItemUom);
            $("#hdn_Alt_ItemUomId").val(ItemUomId);
            $("#Alt_ItemQty").val(ItemQty);

            $("#tbl_AlternateItem > tbody > tr").each(function () {
                var CurrRow = $(this);
                let RowNo = CurrRow.find("#HdnAltRowNo").text();
                let Itm_ID = CurrRow.find("#td_Alt_ddlItem" + RowNo + " option:selected").val();
                BindAlternateItemName(RowNo, ItemTypeId, "N", "N", "new");

                GetBomUomNameItemWise(Itm_ID, "td_AltUom", "tdAltUomId", CurrRow)
            });
            Fun_Alt_deleteIcon();

            //$("#HdnTable_AltItemDetail > tbody > tr #td_altOpId:contains('" + OpId + "')").closest('tr').find("#td_altParentItemId:contains('" + ItemId + "')").closest('tr').each(function () {
            //    var CurrRow = $(this);
            //    //let RowNo = CurrRow.find("#HdnAltRowNo").text();
            //    let td_ItemName = CurrRow.find("#td_altItemName").text();
            //    let td_ItemNameId = CurrRow.find("#td_altItemId").text();
            //    let td_ItemUom = CurrRow.find("#td_altUom").text();
            //    let td_ItemUomId = CurrRow.find("#td_altUomId").text();
            //    let td_AltQty = CurrRow.find("#td_altItemQty").text();
            //    let td_AltItemCost = CurrRow.find("#td_altItemCost").text();
            //    AddNewAlternateItemRow(td_ItemName, td_ItemNameId, td_ItemUom, td_ItemUomId, td_AltQty, td_AltItemCost, disabled);

            //});
        }
    })
}
function AddNewAlternateItemRow(itemName='---Select---', itemId='0', uom='', uomId='', Qty='', ItmCost='',disbled='') {

    let SrNo = $("#tbl_AlternateItem tbody tr").length+1;
    let HdnRowNo = $("#tbl_AlternateItem tbody tr:last-child #HdnAltRowNo").text();
    HdnRowNo = parseInt(CheckNullNumber(HdnRowNo)) + 1;
    var GrayDelete = "";
    if (disbled == "disabled") {
        GrayDelete = 'style="filter: grayscale(100%)"';
    }
    $("#tbl_AlternateItem tbody").append(`<tr>
           <td width="20" class="red center"> <i class="fa fa-trash ${disbled == "disabled"?"":"Alt_deleteIcon"}" ${GrayDelete} aria-hidden="true" title="${$("#Span_Delete_Title").text()}"></i></td>
           <td class="pl-2" id="AltSrNo">${SrNo}</td>
           <td class="pl-2" hidden id="HdnAltRowNo">${HdnRowNo}</td>
           <td>
               <div class="lpo_form col-sm-11 no-padding">
                   <select class=" form-control" id="td_Alt_ddlItem${HdnRowNo}" ${disbled} onchange="AltItemChange(event)">
                       <option value="${itemId}">${itemName}</option>
                   </select>
                    <span id="AltItemNameError" class="error-message is-visible"></span>
               </div>
               <div class="col-sm-1 i_Icon">
                   <button type="button" id="" class="calculator" onclick="AltItemInfoBtnClick(event);" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false"> <img src="/Content/Images/iIcon1.png" alt="" title="${$("#ItmInfo").text()}"> </button>
               </div>
           </td>
           <td>
            <div class="lpo_form no-padding">
                <select id="td_AltUom" class="form-control date" onchange="OnChangeAltTxtUom('td_AltUom','td_Alt_ddlItem${HdnRowNo}',event)">
                    <option value="${uomId == "" ? "0" : uomId}">${uom == "" ? "---Select---" : uom}</option>
                </select>
                <input hidden id="tdAltUomId" value='${uomId}'/>
                <span id="vmtd_AltUom"  class="error-message is-visible"></span>
           </div>
            </td>
           <td>
            <div class="lpo_form col-sm-12 no-padding">
            <input id="td_AltQty" ${disbled} value='${Qty}' class="form-control date num_right" onkeypress="return AmtFloatQtyonly(this,event)" onpaste="return CopyPasteData(event)" onchange="OnChangeAltItemQty(event)" autocomplete="off" type="text" name="Quantity" placeholder="0000:00">
            <span id="AltQtyError" class="error-message is-visible"></span>
            </div>
           </td>
           <td><input id="td_AltItemCost" value='${ItmCost}' class="form-control date num_right" autocomplete="off" type="text" name="ItemCost" placeholder="0000:00" disabled=""></td>
       </tr>`);

    let altItemType = $("#hdn_Alt_ItemTypeId").val();
    BindAlternateItemName(HdnRowNo, altItemType, "N", "N", "new");
    Fun_Alt_deleteIcon();

}
function AltItemChange(e) {

    let CurrRow = $(e.target).closest('tr');
    let HdnRowNo = CurrRow.find("#HdnAltRowNo").text();
    let Itm_ID = CurrRow.find("#td_Alt_ddlItem" + HdnRowNo).val();
    var hdncompid = $("#hdncompid").val();
    var hdnBranchId = $("#hdnBranchId").val();

    let td_ItemNameId = CurrRow.find("#td_Alt_ddlItem" + HdnRowNo).val();
    if (td_ItemNameId == "0" || td_ItemNameId == "") {
        CurrRow.find('[aria-labelledby="select2-td_Alt_ddlItem' + HdnRowNo + '-container"]').css("border-color", "red");
        CurrRow.find("#AltItemNameError").text($("#valueReq").text());
        CurrRow.find("#AltItemNameError").css("display", "block");
        FlagErr = "Y";
    } else {
        CurrRow.find('[aria-labelledby="select2-td_Alt_ddlItem' + HdnRowNo + '-container"]').css("border-color", "#ced4da");
        CurrRow.find("#AltItemNameError").text("");
        CurrRow.find("#AltItemNameError").css("display", "none");
    }

    GetItemCost(hdncompid, hdnBranchId, Itm_ID, "td_AltItemCost", CurrRow)
    //GetBomUomNameItemWise(Itm_ID, "td_AltUom", "tdAltUomId", CurrRow)
    //OnChangeAltItemQty(e);
}
function AltItemInfoBtnClick(e) {
    var CurrRow = $(e.target).closest('tr');
    let HdnRowNo = CurrRow.find("#HdnAltRowNo").text();
    let Itm_ID = CurrRow.find("#td_Alt_ddlItem" + HdnRowNo).val();
    ItemInfoBtnClick(Itm_ID);
}
function OnChangeAltItemQty(e) {
    debugger;
    let QtyDecDigit = $("#QtyDigit").text();
    let CurrRow = $(e.target).closest('tr');

    CurrRow.find("#td_AltQty").val(parseFloat(CheckNullNumber(CurrRow.find("#td_AltQty").val())).toFixed(QtyDecDigit));

    if (parseFloat(CheckNullNumber(CurrRow.find("#td_AltQty").val())) <= 0) {
        CurrRow.find('#td_AltQty').css("border-color", "red");
        CurrRow.find("#AltQtyError").text($("#valueReq").text());
        CurrRow.find("#AltQtyError").css("display", "block");
        FlagErr = "Y";
    } else {
        CurrRow.find('#td_AltQty').css("border-color", "#ced4da");
        CurrRow.find("#AltQtyError").text("");
        CurrRow.find("#AltQtyError").css("display", "none");
    }

    //let td_AltTotalQty = 0;
    //let td_AltTotalCost = 0;
    
    //$("#tbl_AlternateItem tbody tr").each(function (index) {
    //    var CurrRow = $(this);
    //    td_AltTotalQty = parseFloat(td_AltTotalQty) + parseFloat(CheckNullNumber(CurrRow.find("#td_AltQty").val()));
    //    td_AltTotalCost = parseFloat(td_AltTotalCost) + parseFloat(CheckNullNumber(CurrRow.find("#td_AltItemCost").val()));
    //});
    //$("#tf_TotalAltItemQty").text(parseFloat(td_AltTotalQty).toFixed(QtyDecDigit));
    //$("#tf_TotalAltItemCost").text(parseFloat(td_AltTotalCost).toFixed(QtyDecDigit));

}
function Fun_Alt_deleteIcon() {
    $('.Alt_deleteIcon').on("click", function () {
        var ClickedRow = $(this).closest('tr');
        ClickedRow.remove();
        ResetAltSerialNo();
    });
}
function DeleteAlternateWhenItemDelete(Op_id,Item_id) {
    $("#HdnTable_AltItemDetail > tbody > tr #td_altOpId:contains('" + Op_id + "')").closest("tr").find("#td_altParentItemId:contains('" + Item_id + "')").closest('tr').remove();
}
function ResetAltSerialNo() {
    $("#tbl_AlternateItem tbody tr").each(function (index) {
        var CurrRow = $(this);
        CurrRow.find("#AltSrNo").text(index+1);
    });
}
function AlternateItemSaveAndExit() {
    debugger
    let OpId = $("#hdn_Alt_OpId").val();
    let ItemTypeId = $("#hdn_Alt_ItemTypeId").val();
    let ItemId = $("#hdn_Alt_ItemId").val();
    //let ItemUomId = $("#hdn_Alt_ItemUomId").val();
    if (ValidateAlternateItemPopupDt() == true) {
        var MatchedItemRows = $("#HdnTable_AltItemDetail tbody tr #td_altOpId:contains('" + OpId + "')").closest("tr").find("#td_altParentItemId:contains('" + ItemId + "')").closest('tr');
        MatchedItemRows.remove();
        $("#tbl_AlternateItem > tbody > tr").each(function () {
            var CurrRow = $(this);
            let RowNo = CurrRow.find("#HdnAltRowNo").text();
            let td_ItemName = CurrRow.find("#td_Alt_ddlItem" + RowNo + " option:selected").text();
            let td_ItemNameId = CurrRow.find("#td_Alt_ddlItem" + RowNo).val();
            let td_ItemUom = CurrRow.find("#td_AltUom option:selected").text();
            let td_ItemUomId = CurrRow.find("#td_AltUom option:selected").val();// CurrRow.find("#tdAltUomId").val();
            let td_AltQty = CurrRow.find("#td_AltQty").val();
            let td_AltItemCost = CurrRow.find("#td_AltItemCost").val();

            $("#HdnTable_AltItemDetail tbody").append(` <tr>
                     <td id="td_altOpId">${OpId}</td>
                     <td id="td_altParentItemId">${ItemId}</td>
                     <td id="td_altItemName">${td_ItemName}</td>
                     <td id="td_altItemId">${td_ItemNameId}</td>
                     <td id="td_altUom">${td_ItemUom}</td>
                     <td id="td_altUomId">${td_ItemUomId}</td>
                     <td id="td_altItemQty">${td_AltQty}</td>
                     <td id="td_altItemCost">${td_AltItemCost}</td>
                 </tr>`)
        });

        $("#BtnAlternateSaveAndExit").attr("data-dismiss", "modal");
        $("#ddl_ItemName").val(0).trigger('change');
        var MatchedItemRowsLength = $("#HdnTable_AltItemDetail tbody tr #td_altOpId:contains('" + OpId + "')").closest("tr").find("#td_altParentItemId:contains('" + ItemId + "')").closest('tr').length;
        if (MatchedItemRowsLength > 0) {
            $("#tr_" + OpId + " #tbl_" + OpId + " > tbody > tr #tbl_hdn_ddl_ItemName_ID[value='" + ItemId + "']").closest('tr').find("#btnAltItemInfo").addClass("green1");
        } else {
            $("#tr_" + OpId + " #tbl_" + OpId + " > tbody > tr #tbl_hdn_ddl_ItemName_ID[value='" + ItemId + "']").closest('tr').find("#btnAltItemInfo").removeClass("green1");
        }
        
    }
    else {
        $("#BtnAlternateSaveAndExit").attr("data-dismiss", "");
        return false;
    }

}
function ValidateAlternateItemPopupDt() {

    var FlagErr = "N";
    $("#tbl_AlternateItem > tbody > tr").each(function () {
        var CurrRow = $(this);
        let RowNo = CurrRow.find("#HdnAltRowNo").text();
        let td_ItemName = CurrRow.find("#td_Alt_ddlItem" + RowNo + " option:selected").text();
        let td_ItemNameId = CurrRow.find("#td_Alt_ddlItem" + RowNo).val();
        let td_AltQty = CurrRow.find("#td_AltQty").val();
        let td_AltUomId = CurrRow.find("#td_AltUom").val();
        if (td_ItemNameId == "0" || td_ItemNameId == "") {
            CurrRow.find('[aria-labelledby="select2-td_Alt_ddlItem' + RowNo + '-container"]').css("border-color", "red");
            CurrRow.find("#AltItemNameError").text($("#valueReq").text());
            CurrRow.find("#AltItemNameError").css("display", "block");
            FlagErr = "Y";
        } else {
            CurrRow.find('[aria-labelledby="select2-td_Alt_ddlItem' + RowNo + '-container"]').css("border-color", "#ced4da");
            CurrRow.find("#AltItemNameError").text("");
            CurrRow.find("#AltItemNameError").css("display", "none");
        }

        if (td_AltUomId == "0" || td_AltUomId == "" || td_AltUomId == null) {
            CurrRow.find('#td_AltUom').css("border-color", "red");
            CurrRow.find("#vmtd_AltUom").text($("#valueReq").text());
            CurrRow.find("#vmtd_AltUom").css("display", "block");
            FlagErr = "Y";
        } else {
            CurrRow.find('#td_AltUom').css("border-color", "#ced4da");
            CurrRow.find("#vmtd_AltUom").text("");
            CurrRow.find("#vmtd_AltUom").css("display", "none");
        }

        if (parseFloat(CheckNullNumber(td_AltQty)) <= 0) {
            CurrRow.find('#td_AltQty').css("border-color", "red");
            CurrRow.find("#AltQtyError").text($("#valueReq").text());
            CurrRow.find("#AltQtyError").css("display", "block");
            FlagErr = "Y";
        } else {
            CurrRow.find('#td_AltQty').css("border-color", "#ced4da");
            CurrRow.find("#AltQtyError").text("");
            CurrRow.find("#AltQtyError").css("display", "none");
        }
    });
    if (FlagErr == "Y") {
        return false;
    } else {
        return true;
    }
}
function BindAlternateItemName(SrNo,ItmType, SFG, Pack, type) {
    debugger;

    $("#td_Alt_ddlItem" + SrNo).select2({

        ajax: {
            url: "/ApplicationLayer/BillofMaterial/BindItemNameDDL",
            data: function (params) {
                var queryParameters = {
                    SO_ItemName: params.term,
                    ItmType: ItmType,
                    wip: SFG,
                    Pack: Pack,
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
                let array = [];
                data = JSON.parse(data).Table;
                var ddl_opName = $("#hdn_Alt_OpId").val();
                var mainItem = $("#ddl_ProductName").val();
                array.push({ id: mainItem });
                $("#tbl_" + ddl_opName + " > tbody > tr").each(function () {
                    var currentRow = $(this);
                    debugger;
                    var itemId = currentRow.find("#tbl_hdn_ddl_ItemName_ID").val();
                    if (itemId != "0") {
                        array.push({ id: itemId });
                    }

                });
                
                $("#tbl_AlternateItem > tbody > tr").each(function (index) {
                    var currentRow = $(this);
                    debugger;
                    var RowNo = currentRow.find("#HdnAltRowNo").text();
                    var itemId = currentRow.find("#td_Alt_ddlItem" + RowNo).val();
                    if (itemId != "0" && $("#td_Alt_ddlItem" + SrNo).val() != itemId) {
                        array.push({ id: itemId });
                    }

                });

                array = FilterAlternateItems(array, ddl_opName);//Added by Suraj on 16-10-2023 for filter Alternate item

                var ItemListArrey = JSON.stringify(array);

                let selected = [];
                selected.push({ id: $("#td_Alt_ddlItem" + SrNo).val() });
                selected = JSON.stringify(selected);

                var NewArrey = JSON.parse(ItemListArrey).filter(i => !selected.includes(i.id))

                //data = data.filter(j => !JSON.stringify(NewArrey).includes(j.ID.split("_")[0]));
                data = data.filter(j => !JSON.stringify(NewArrey).includes(j.Item_id));
                if ($(".select2-search__field").parent().find("ul").length == 0) {
                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
<div class="col-md-3 col-xs-6 def-cursor">UOM</div></div>
</strong></li></ul>`)
                }
                page = 1;
                data = data.slice((page - 1) * pageSize, page * pageSize)
                if (data[0] != null) {
                    if (data[0].Item_name.trim() != "---Select---") {
                        var select = { Item_id: "0", Item_name: " ---Select---",uom_name:"" };
                        data.unshift(select);
                    }
                }
                return {
                    results: $.map(data, function (val, Item) {
                        return { id: val.Item_id, text: val.Item_name, UOM: val.uom_name };
                    }),

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
//Created by Suraj on 16-10-2023 for filter Alternate item
function FilterAlternateItems(array,ddl_opName) {
    $("#HdnTable_AltItemDetail > tbody > tr #td_altOpId:contains('" + ddl_opName + "')").closest('tr').each(function (index) {
        var currentRow = $(this);
        debugger;
        var itemId = currentRow.find("#td_altItemId").text();
        if (itemId != "0") {
            array.push({ id: itemId });
        }

    });
    return array;
}
function InsertAlternateItemDetails() {
    var Arr = new Array();
    $("#HdnTable_AltItemDetail > tbody > tr").closest('tr').each(function () {
        var CurrRow = $(this);
        //let RowNo = CurrRow.find("#HdnAltRowNo").text();

        let OpId = CurrRow.find("#td_altOpId").text();
        let ItemId = CurrRow.find("#td_altParentItemId").text();
        let td_ItemName = CurrRow.find("#td_altItemName").text();
        let AltItemId = CurrRow.find("#td_altItemId").text();
        let td_ItemUom = CurrRow.find("#td_altUom").text();
        let UomId = CurrRow.find("#td_altUomId").text();
        let Qty = CurrRow.find("#td_altItemQty").text();
        let ItemCost = CurrRow.find("#td_altItemCost").text();
        Arr.push({ op_id: OpId, item_id: ItemId, alt_item_id: AltItemId, uom_id: UomId, qty: Qty, item_cost: ItemCost });
    });
    return Arr;
}
function OnClickAltBtnReset() {
    $("#tbl_AlternateItem > tbody > tr").remove();
}

/* -----------------------------Alternate Item section End------------------------------ */

function OnChangeTxtUom(UOM_Field_Id, Item_Field_Id, Cp_FieldId = 'hdnitemcost', CurrRow) {
    let UomId = $("#" + UOM_Field_Id).val();
    let ItemId = $("#" + Item_Field_Id).val();
   
    if (CurrRow != null) {
        UomId = CurrRow.find("#" + UOM_Field_Id).val();
        ItemId = CurrRow.find("#" + Item_Field_Id).val();
        if (UomId != "0") {
            CurrRow.find("#vm" + UOM_Field_Id).css("display", "none");
            CurrRow.find("#" + UOM_Field_Id).css("border-color", "#ced4da");
        }
        else {
            CurrRow.find('#vm' + UOM_Field_Id).text($("#valueReq").text());
            CurrRow.find("#vm" + UOM_Field_Id).css("display", "block");
            CurrRow.find("#" + UOM_Field_Id).css("border-color", "red");
            flag = 'Y';
        }
    } else {
        if (UomId != "0") {
            $("#vm" + UOM_Field_Id).css("display", "none");
            $("#" + UOM_Field_Id).css("border-color", "#ced4da");
        }
        else {
            $('#vm' + UOM_Field_Id).text($("#valueReq").text());
            $("#vm" + UOM_Field_Id).css("display", "block");
            $("#" + UOM_Field_Id).css("border-color", "red");
            flag = 'Y';
        }
    }

    

    var RateDigit = $("#RateDigit").text();
    $.ajax({
        type:"POST",
        url: "/ApplicationLayer/BillofMaterial/getUomConvRate",
        data: {
            ItemId: ItemId,
            UomId: UomId
        },
        success: function (data) {
            debugger;
            if (data !== null && data !== "" && data != "ErrorPage") {
                var arr = [];
                arr = JSON.parse(data);
                if (arr.Table.length > 0) {
                    cost = arr.Table[0].conv_cost_price;
                    if (CurrRow != null) {
                        CurrRow.find("#" + Cp_FieldId).val(parseFloat(CheckNullNumber(cost)).toFixed(RateDigit));
                    } else {
                        $("#" + Cp_FieldId).val(parseFloat(CheckNullNumber(cost)).toFixed(RateDigit));
                    }

                }
            }
            else {
                $("#txt_Quantity").val("");
            }
        }
    })
}
function OnChangeAltTxtUom(UOM_Field_Id, Item_Field_Id, e) {
    let CurrRow = $(e.target).closest("tr");
    OnChangeTxtUom(UOM_Field_Id, Item_Field_Id, 'td_AltItemCost', CurrRow);
}
// Added this function by nitesh 26-10-2023 for onchange replicate item set data in table 
function ddlReplicateWith_onchange() {
    debugger;
    $("#tbl_AlternateItem > tbody > tr").remove();
    $('#tbladd >tbody >tr').remove();
  var replicate_item =   $("#ddlReplicateWith").val();
    
    $.ajax(
        {
        type: "POST",
            url: "/ApplicationLayer/BillofMaterial/getreplicateitemdata",
           data : {
               replicate_item: replicate_item
        },
        success: function (data) {
            debugger;
            if (data !== null && data !== "" && data != "ErrorPage") {
                var arr = [];
                arr = JSON.parse(data);
                //if (arr.Table.length > 0) {                      //Commented by nitesh 26-10-2023  for quantity is accoding to user
                //    $("#txt_Quantity").val(arr.Table[0].qty);                
                //}
                //else {
                //    $("#txt_Quantity").val("");               
                //}
                if (arr.Table.length > 0) {
                    //let productQty = $("#txt_Quantity").val();
                    for (var i = 0; i < arr.Table.length;i++) {

                        var ddl_ProductId = arr.Table[i].item_id;
                        
                        var QtyDecDigit = $("#QtyDigit").text();
                        var txt_Quantity = arr.Table[i].qty;
                        var itemcost1 = arr.Table[i].item_cost;
                        var ddl_opName = arr.Table[i].op_name;
                        var ddl_op_ID = arr.Table[i].op_id;
                        var ddlItemType = arr.Table[i].Item_type_name;
                        var ddlItemType_ID = arr.Table[i].Item_type_id;

                        var ddl_ItemName = arr.Table[i].item_name;
                        var ddl_ItemName_ID = arr.Table[i].item_id;

                        var UOM_itemName = arr.Table[i].uom_alias;
                        var hdn_UOM_itemName = arr.Table[i].uom_id;
                        var txtQuantityItemName = arr.Table[i].quantity;
                        var total_itemvalue = arr.Table[i].item_value;
                        //var total_itemvalue = (parseFloat(itemcost1) * parseFloat(txtQuantityItemName)).toFixed(QtyDecDigit);
                        var seq_no = arr.Table[i].seq_no;
                        var itmseq_no = arr.Table[i].item_type_seq_no;

                        onchangereplicate_item(ddl_ProductId, QtyDecDigit, txt_Quantity, itemcost1, ddl_opName,
                            ddl_op_ID, ddlItemType, ddlItemType_ID, ddl_ItemName, ddl_ItemName_ID, UOM_itemName,
                            hdn_UOM_itemName, txtQuantityItemName, total_itemvalue, seq_no, itmseq_no);
                    
                    }
                }
            }
            else {
                $("#txt_Quantity").val("");
            }
        }
    })

}


function onchangereplicate_item(ddl_ProductId, QtyDecDigit, txt_Quantity, itemcost1, ddl_opName,
    ddl_op_ID, ddlItemType, ddlItemType_ID, ddl_ItemName, ddl_ItemName_ID, UOM_itemName, hdn_UOM_itemName,
    txtQuantityItemName, total_itemvalue, seq_no, itmseq_no)
{
  
    debugger;
    var rowCount = $('#tbladd >tbody >tr').length;
    var flag_check_op_id = 'N';
    var rowspn = parseInt(0);
    $("#tbladd >tbody >tr").each(function (i, row) {
        debugger;

        var currentRow = $(this);
        if (rowCount == 0) {
            rowspn = 1;
        }

        if (rowCount > 0) {
            var op_id = currentRow.find("#tbl_hdn_ddl_op_id").val();
            rowspn = parseInt(currentRow.find("#hdnspan").val());
            if (ddl_op_ID == op_id) {
                flag_check_op_id = 'Y';
            }
        }

    });
    debugger;
    var itemcost = (parseFloat(itemcost1).toFixed(RateDigit));
    if (ddlItemType_ID == "A" || ddlItemType_ID == "OS") {
        itemcost1 = 0;
    }

    var trop = "";
    var QtyDecDigit = $("#QtyDigit").text();
    var RateDigit = $("#RateDigit").text();
    var ValDigit = $("#ValDigit").text();
    var txtQuantityItemName1 = (parseFloat(txtQuantityItemName).toFixed(QtyDecDigit));
    //var totalitemvalue1 = parseFloat(txtQuantityItemName) * parseFloat(itemcost1);

    if (ddlItemType_ID == "OW" || ddlItemType_ID == "OF") {
        var itemVal = 0;
        $('#tbl_' + ddl_op_ID + ' tbody tr').each(function () {
            var CurrentRow1 = $(this);
            var tbl_hdn_totalitemvalue = CurrentRow1.find("#tbl_hdn_totalitemvalue").val();
            itemVal = parseFloat(itemVal) + parseFloat(tbl_hdn_totalitemvalue);

        });
        itemcost1 = parseFloat(itemVal) / parseFloat(txtQuantityItemName);
       //totalitemvalue1 = itemVal;
    }
    //if (ddlItemType_ID == "IW") {
    //   // totalitemvalue1 = $("#hdn_Item_Value").val();
    //}
    itemcost1 = parseFloat(itemcost1).toFixed(RateDigit);
   // var totalitemvalue = (parseFloat(totalitemvalue1).toFixed(ValDigit));

    var AlternateIcon = "";
    if (ddlItemType_ID.trim() == "IR" || ddlItemType_ID.trim() == "P") {
        AlternateIcon = `<div class="col-sm-2 i_Icon">
     <button type="button" id="btnAltItemInfo" class="calculator alter" onclick="OnClickAlternateItem(event)" data-toggle="modal" data-target="#AlternateItemDetail" data-backdrop="static" data-keyboard="false"  title="${$("#span_AlternateItemDetail").text()}"><i class="fa fa-th-large" aria-hidden="true"></i> </button>
 </div>`
    }

    debugger;
    if (flag_check_op_id == 'N') {//first time
        rowIdx = rowIdx + 1
        trop += "<tr id='tr_" + ddl_op_ID + "'>";
        debugger;  //changing td to id by SM
        trop += '<td align="left" valign="top" class="red center bom_width_td" > <i class="fa fa-trash" aria-hidden="true" title="' + $("#Span_Delete_Title").text() + '" onclick="deleteMainRow(this, event, ' + ddl_op_ID + ', \'' + ddl_ItemName_ID + '\', \'' + ddlItemType_ID + '\')"></i></td>';
        trop += "<td id='srno'></td>";
        trop += "<td id='opname'>" + ddl_opName + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_op_id' value=" + ddl_op_ID + " /></td>";
        trop += "<td colspan='6' class='no-padding'>";

        trop += "<table id='tbl_" + ddl_op_ID + "' border='0' width='100%'>";
        trop += "<tbody id='tbody_" + ddl_op_ID + "'>";
        trop += " <tr>";
        trop += '<td align="left" valign="top" class="red center bom_width_td"> <i class="fa fa-trash" aria-hidden="true" title="' + $("#Span_Delete_Title").text() + '" onclick="deleteRow(this, event, ' + ddl_op_ID + ', \'' + ddl_ItemName_ID + '\', \'' + ddlItemType_ID + '\')"></i></td>';
        trop += "<td class='bom_width_td' align='left' valign='top'>";
        if (ddlItemType_ID != "OF" && ddlItemType_ID != "IW") {
            trop += "<i class='fa fa-edit' aria-hidden='true' id='' title=" + $("#Edit").text() + " onclick='editRow(this, event, " + ddl_op_ID + ")'></i>";
        }
        debugger;  //changing td to id by SM
        trop += " </td>";
        trop += `<td id='op_Item_type_name' width='17%'>
<div class="col-sm-10 no-padding">${ddlItemType}</div>
${AlternateIcon}
</td>`;
        if (ddlItemType_ID == "OF") {
            var prod_val = $("#ddl_ProductName option:selected").text();
            var prod_id = $("#ddl_ProductName").val();

            trop += "<td  hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemType_ID' value=" + ddlItemType_ID + " /></td>";
            trop += '<td id="op_item_name" width="32%"> <div class="col-sm-11 no-padding">' + prod_val + '</div><div class="col-sm-1 i_Icon"> <button type ="button" id ="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,\'' + prod_id + '\', \'' + prod_val + '\',\'edit\');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" > <img src="/Content/Images/iIcon1.png" alt="" title="' + $("#Span_ItemInformation_Title").text() + '"> </button> </div></td>';
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemName_ID' value=" + prod_id + " /></td>";
        }
        else {
            trop += "<td  hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemType_ID' value=" + ddlItemType_ID + " /></td>";
            trop += '<td id="op_item_name" width="32%"> <div class="col-sm-11 no-padding">' + ddl_ItemName + '</div><div class="col-sm-1 i_Icon"> <button type ="button" id ="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,\'' + ddl_ItemName_ID + '\', \'' + ddl_ItemName + '\',\'edit\');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" > <img src="/Content/Images/iIcon1.png" alt="" title="' + $("#Span_ItemInformation_Title").text() + '"> </button> </div></td>';
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemName_ID' value=" + ddl_ItemName_ID + " /></td>";
        }
        if (ddlItemType_ID == "OF") {
            var uom_name = $("#UOM").text();
            var uom_ID = $("#UOMID").val();
            trop += "<td id='op_uom_name' width='12%'>" + uom_name + "</td>";
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_UOM_itemName' value=" + uom_ID + " /></td>";
        }
        else {
            trop += "<td id='op_uom_name' width='12%'>" + UOM_itemName + "</td>";
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_UOM_itemName' value=" + hdn_UOM_itemName + " /></td>";
        }
        trop += "<td id='op_qty' width='13%' class='num_right'>" + txtQuantityItemName1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_txtQuantityItemName' value=" + txtQuantityItemName1 + " /></td>";
        trop += "<td id='op_item_cost' width='13%' class='num_right'>" + itemcost1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_itemcost' value=" + itemcost1 + " /></td>";
        trop += "<td id='op_item_value' width='13%' class='num_right'>" + total_itemvalue + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_totalitemvalue' value=" + total_itemvalue + " /></td>";
        trop += "<td hidden='hidden'>";
        trop += "<input type='hidden' name='ChildRowNo' id='ChildRowNo' />";
        trop += "<input type = 'hidden' name = 'ChildTableId' id = 'ChildTableId' /> ";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_id' id='tbl_hdn_id' value=" + ddl_op_ID + " /></td>";
       // trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_seq_no' id='tbl_hdn_seq_no' value=" + SetItemTypeSequence(ddlItemType_ID) + " /></td>";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_seq_no' id='tbl_hdn_seq_no' value=" + itmseq_no +" /></td>";
        trop += "</td>";
        trop += "</tr>";
        trop += "</tbody>";
        trop += "</table>";
        trop += "</td>";
        trop += "</tr >";


        $('#tbladd_body').append(trop);
        $("#ddl_ItemName option").removeClass("select2-hidden-accessible");
        $('#ddl_ItemName').val("0").change();
        disablePrName_Qty();
        HideItemType_AfterAdd(ddl_ItemName_ID, ddlItemType_ID)
        //AddRemoveItemName(ddl_op_ID)
        SerialNoAfterDelete();
    }

    if (flag_check_op_id == 'Y') {

        debugger;
        trop += " <tr>";
        trop += '<td align = "left" valign = "top" class="red center bom_width_td"> <i class="fa fa-trash" aria-hidden="true" title="' + $("#Span_Delete_Title").text() + '" onclick="deleteRow(this, event, ' + ddl_op_ID + ', \'' + ddl_ItemName_ID + '\', \'' + ddlItemType_ID + '\')"></i></td >';
        trop += "<td width='20' align='left' valign='top'> ";
        if (ddlItemType_ID != "OF" && ddlItemType_ID != "IW") {
            trop += "<i class='fa fa-edit' aria-hidden='true' id='' title=" + $("#Edit").text() + " onclick='editRow(this, event, " + ddl_op_ID + ")'></i>";
        }
        debugger;  //changing td to id by SM
        trop += "</td>";
        trop += `<td id='op_Item_type_name' width='17%'>
<div class="col-sm-10 no-padding">${ddlItemType}</div>
${AlternateIcon}
</td>`;
        if (ddlItemType_ID == "OF") {
            var prod_val = $("#ddl_ProductName option:selected").text();
            var prod_id = $("#ddl_ProductName").val();
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemType_ID' value=" + ddlItemType_ID + " /></td>";
            trop += '<td id="op_item_name" width="32%"> <div class="col-sm-11 no-padding">' + prod_val + '</div><div class="col-sm-1 i_Icon"> <button type ="button" id ="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,\'' + prod_id + '\', \'' + prod_val + '\',\'edit\');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" > <img src="/Content/Images/iIcon1.png" alt="" title="' + $("#Span_ItemInformation_Title").text() + '"> </button> </div></td>';
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemName_ID' value=" + prod_id + " /></td>";
        }
        else {
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemType_ID' value=" + ddlItemType_ID + " /></td>";
            trop += '<td id="op_item_name" width="32%"> <div class="col-sm-11 no-padding">' + ddl_ItemName + '</div><div class="col-sm-1 i_Icon"> <button type ="button" id ="ItmInfoBtnIcon" class="calculator" onclick="OnClickIconBtn(event,\'' + ddl_ItemName_ID + '\', \'' + ddl_ItemName + '\',\'edit\');" data-toggle="modal" data-target="#ItemInfo" data-backdrop="static" data-keyboard="false" > <img src="/Content/Images/iIcon1.png" alt="" title="' + $("#Span_ItemInformation_Title").text() + '"> </button> </div></td>';
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_ddl_ItemName_ID' value=" + ddl_ItemName_ID + " /></td>";
        }
        if (ddlItemType_ID == "OF") {
            var uom_name = $("#UOM").val();
            var uom_ID = $("#UOMID").val();
            trop += "<td id='op_uom_name' width='12%'>" + uom_name + "</td>";
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_UOM_itemName' value=" + uom_ID + " /></td>";
        }
        else {
            trop += "<td id='op_uom_name' width='12%'>" + UOM_itemName + "</td>";
            trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_UOM_itemName' value=" + hdn_UOM_itemName + " /></td>";
        }
        
        trop += "<td id='op_qty' width='13%' id='' class='num_right'>" + txtQuantityItemName1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_txtQuantityItemName' value=" + txtQuantityItemName1 + " /></td>";
        trop += "<td id='op_item_cost' width='13%' class='num_right'>" + itemcost1 + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_itemcost' value=" + itemcost1 + " /></td>";
        trop += "<td id='op_item_value' width='13%' class='num_right'>" + total_itemvalue + "</td>";
        trop += "<td hidden='hidden'><input type='hidden' id='tbl_hdn_totalitemvalue' value=" + total_itemvalue + " /></td>";
        trop += "<td hidden='hidden'>";
        trop += "<input type='hidden' name='ChildRowNo' id='ChildRowNo' />";
        trop += "<input type = 'hidden' name = 'ChildTableId' id = 'ChildTableId' /> ";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_id' id='tbl_hdn_id' value=" + ddl_op_ID + " /></td>";
        //trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_seq_no' id='tbl_hdn_seq_no' value=" + SetItemTypeSequence(ddlItemType_ID) + " /></td>";
        trop += "<td hidden='hidden'><input type='hidden' name='tbl_hdn_seq_no' id='tbl_hdn_seq_no' value=" + itmseq_no +  " /></td>";
        trop += "</td>";
        trop += "</tr>";


        $('#tbl_' + ddl_op_ID + ' tbody').append(trop);
        HideOperationNameAfterFG();
        $("#ddl_ItemName option").removeClass("select2-hidden-accessible");
        $('#ddl_ItemName').val("0").change();
        disablePrName_Qty();
        HideItemType_AfterAdd(ddl_ItemName_ID, ddlItemType_ID);
        SerialNoAfterDelete();
    }
    CalCulateTotalAndSet(ddl_op_ID);
    SetItemNameSelectOnly();
    ResetOperationsFields();
    $("#ddl_opName").val(ddl_op_ID).change();
    debugger;
    sortTable('tbl_' + ddl_op_ID + '');
    $("#tbladd tbody tr").css("background-color", "#ffffff");

  //  $("#ddlReplicateWith").val("0");
};
function ReplicateWith() {
    debugger;
    var item = $("#ddlReplicateWith").val();
       $('#ddlReplicateWith').append(`<option data-uom="0" value="0">---Select---</option>`);
    DynamicSerchableItemDDL("", "#ddlReplicateWith", "", "#hdn_ReplicateWith", "", "BOM","");

//    $("#ddlReplicateWith").select2({ /*Commented By NItesh Replcate with Dropdown is only Comman  item bind*/
//        ajax: {
//            url: "/ApplicationLayer/BillofMaterial/BindReplicateWithlist",
//            data: function (params) {
//                var queryParameters = {
//                    SO_ItemName: params.term,
//                    //PageName: PageName,
//                    page: params.page || 1
//                };
//                return queryParameters;
//            },
//            multiple: true,
//            cache: true,
//            processResults: function (data, params) {
//                debugger
//                var pageSize,
//                    pageSize = 2000; // or whatever pagesize

//                if ($(".select2-search__field").parent().find("ul").length == 0) {

//                    $(".select2 - search__field").append(`<optgroup class='def-cursor' id="Textddl${RowID}" label='${$("#ItemName").text()}' data-uom='${$("#ItemUOM").text()}'></optgroup>`);
//                    $(".select2 - search__field").append(`<option data-uom="0" value="0">---Select---</option>`);
////                    $(".select2-search__field").parent().append(`<ul class="select2-results__options"><li class="select2-results__option"><strong class="select2-results__group">
////<div class="row"><div class="col-md-9 col-xs-6 def-cursor">${$("#ItemName").text()}</div>
////<div class="col-md-3 col-xs-6 def-cursor">UOM</div></div>
////</strong></li></ul>`)
//                }

//                var page = 1;
//                data = data.slice((page - 1) * pageSize, page * pageSize)
//                return {
//                    results: $.map(data, function (val, Item) {
//                        return { id: val.ID.split("_")[0], text: val.Name, UOM: val.ID.split("_")[1] };
//                    }),
//                };
//            },
//            cache: true
//        },
//        templateResult: function (data) {
//            debugger

//            var classAttr = $(data.element).attr('class');
//            var hasClass = typeof classAttr != 'undefined';
//            classAttr = hasClass ? ' ' + classAttr : '';
//            var $result;

//            $result = $(
//                '<div class="row">' +
//                '<div class="col-md-9 col-xs-6' + classAttr + '">' + data.text + '</div>' +
//                '<div class="col-md-3 col-xs-6' + classAttr + '">' + data.UOM + '</div>' +
//                '</div>'
//            );

//            return $result;

//            firstEmptySelect = false;
//        },
//    });
}

function approveonclick() { /**Added this Condition by Nitesh 11-01-2024 for Disable Approve btn after one Click**/
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
function BtnSearch() {
    debugger;
    try {
        var Act = $("#ActiveID").val();
        var Status = $("#ddlStatus").val();
        $.ajax({
            type: "POST",
            url: "/ApplicationLayer/BillofMaterial/SearchDataBOM",
            data: {
                Act: Act,
                Status: Status
            },
            success: function (data) {
                debugger;
                $('#bom_list_tbody').html(data);
                $('#ListFilterData').val(Act + ',' + Status);
            },
            error: function OnError(xhr, errorType, exception) {
                debugger;
            }
        });
    } catch (err) {
        debugger;
        console.log("MRP Error : " + err.message);

    }
}